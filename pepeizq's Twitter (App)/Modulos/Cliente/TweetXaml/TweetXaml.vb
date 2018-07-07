Imports System.Globalization
Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.System.Threading
Imports Windows.UI.Core

Namespace pepeizq.Twitter.Xaml
    Module TweetXaml

        Public Function Añadir(tweet As Tweet, megaUsuario As MegaUsuario, color As Windows.UI.Color)

            Dim recursos As New Resources.ResourceLoader

            Dim grid As New Grid

            If Not tweet Is Nothing Then
                grid.Name = "gridTweet" + tweet.ID
                grid.Tag = New Objetos.TweetAmpliado(megaUsuario, tweet)
                grid.Padding = New Thickness(0, 15, 0, 10)

                Dim color1 As New GradientStop With {
                    .Color = ColorHelper.ToColor("#e0e0e0"),
                    .Offset = 0.5
                }

                Dim color2 As New GradientStop With {
                    .Color = ColorHelper.ToColor("#d6d6d6"),
                    .Offset = 1.0
                }

                Dim coleccion As New GradientStopCollection From {
                    color1,
                    color2
                }

                Dim brush As New LinearGradientBrush With {
                    .StartPoint = New Point(0.5, 0),
                    .EndPoint = New Point(0.5, 1),
                    .GradientStops = coleccion
                }

                grid.Background = brush

                Dim row1 As New RowDefinition
                Dim row2 As New RowDefinition

                row1.Height = New GridLength(1, GridUnitType.Auto)
                row2.Height = New GridLength(1, GridUnitType.Star)

                grid.RowDefinitions.Add(row1)
                grid.RowDefinitions.Add(row2)

                '-----------------------------

                Dim gridSuperior As New Grid
                gridSuperior.SetValue(Grid.RowProperty, 0)

                If Not tweet.Retweet Is Nothing Then
                    gridSuperior.Children.Add(TweetRetweet.Generar(tweet))
                End If

                grid.Children.Add(gridSuperior)

                '-----------------------------

                Dim gridInferior As New Grid

                Dim col1 As New ColumnDefinition
                Dim col2 As New ColumnDefinition
                Dim col3 As New ColumnDefinition

                col1.Width = New GridLength(1, GridUnitType.Auto)
                col2.Width = New GridLength(1, GridUnitType.Star)
                col3.Width = New GridLength(1, GridUnitType.Auto)

                gridInferior.ColumnDefinitions.Add(col1)
                gridInferior.ColumnDefinitions.Add(col2)
                gridInferior.ColumnDefinitions.Add(col3)
                gridInferior.SetValue(Grid.RowProperty, 1)

                '-----------------------------

                Dim spIzquierda As New StackPanel With {
                    .Orientation = Orientation.Vertical
                }

                spIzquierda.SetValue(Grid.ColumnProperty, 0)

                spIzquierda.Children.Add(TweetAvatar.Generar(tweet, megaUsuario))

                gridInferior.Children.Add(spIzquierda)

                '-----------------------------

                Dim spInferiorCentro As New StackPanel With {
                    .Orientation = Orientation.Vertical
                }

                spInferiorCentro.SetValue(Grid.ColumnProperty, 1)

                spInferiorCentro.Children.Add(TweetUsuario.Generar(tweet, megaUsuario, color))

                Dim tbTweet As TextBlock = TweetTexto.Generar(tweet, Nothing, color, megaUsuario, False)

                If Not tbTweet Is Nothing Then
                    If tbTweet.Text.Length > 0 Then
                        spInferiorCentro.Children.Add(tbTweet)
                    End If
                End If

                If Not tweet.Cita Is Nothing Then
                    If tweet.Retweet Is Nothing Then
                        spInferiorCentro.Children.Add(TweetCita.Generar(tweet, megaUsuario, color))
                    Else
                        spInferiorCentro.Children.Add(TweetCita.Generar(tweet.Retweet, megaUsuario, color))
                    End If
                End If

                If ApplicationData.Current.LocalSettings.Values("media") = True Then
                    spInferiorCentro.Children.Add(TweetMediaXaml.Generar(tweet, color))
                End If

                spInferiorCentro.Children.Add(TweetBotones.Generar(tweet, grid, megaUsuario, 0, color))
                spInferiorCentro.Children.Add(TweetEnviarTweet.Generar(tweet, megaUsuario, Visibility.Collapsed, color))

                gridInferior.Children.Add(spInferiorCentro)

                '-----------------------------

                Dim gridInferiorDerecha As New Grid With {
                    .HorizontalAlignment = HorizontalAlignment.Right,
                    .Margin = New Thickness(0, 5, 25, 0)
                }

                gridInferiorDerecha.SetValue(Grid.ColumnProperty, 2)

                Dim tbTiempo As New TextBlock With {
                    .FontSize = 12
                }

                SumarTiempo(tbTiempo, tweet.Creacion)

                gridInferiorDerecha.Children.Add(tbTiempo)

                '-----------------------------

                gridInferior.Children.Add(gridInferiorDerecha)

                '-----------------------------

                grid.Children.Add(gridInferior)
            End If

            Dim item As New ListViewItem With {
                .Content = grid
            }

            If ApplicationData.Current.LocalSettings.Values("tooltipsayuda") = True Then
                ToolTipService.SetToolTip(item, recursos.GetString("ClickExpandTweet"))
                ToolTipService.SetPlacement(item, PlacementMode.Bottom)
            End If

            AddHandler item.PointerEntered, AddressOf UsuarioEntraItem
            AddHandler item.PointerExited, AddressOf UsuarioSaleItem

            Return item

        End Function

        'CONTADOR_TIEMPO-------------------------------------------------

        Public Sub SumarTiempo(tb As TextBlock, fecha As String)

            Dim fechaTweet As DateTime = Nothing

            DateTime.TryParseExact(fecha, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, fechaTweet)

            Dim periodo As TimeSpan = TimeSpan.FromSeconds(1)

            Dim contador As ThreadPoolTimer = Nothing
            contador = ThreadPoolTimer.CreatePeriodicTimer(Async Sub(tiempo)
                                                               Try
                                                                   Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Sub()
                                                                                                                                                                     If Not tb.Text Is Nothing Then
                                                                                                                                                                         Dim fechaFinal As TimeSpan = DateTime.Now - fechaTweet
                                                                                                                                                                         Dim totalSegundos As Integer = fechaFinal.TotalSeconds

                                                                                                                                                                         fechaFinal = fechaFinal.Add(periodo)
                                                                                                                                                                         MostrarTiempo(tb, fechaFinal)
                                                                                                                                                                     End If
                                                                                                                                                                 End Sub))
                                                               Catch ex As Exception

                                                               End Try
                                                           End Sub, periodo)

        End Sub

        Public Sub MostrarTiempo(tb As TextBlock, tiempo As TimeSpan)

            If tiempo.TotalSeconds > -1 And tiempo.TotalSeconds < 60 Then
                Dim segundos As Integer = Convert.ToInt32(tiempo.TotalSeconds)
                tb.Text = segundos.ToString + "s"
            ElseIf tiempo.TotalMinutes > 0 And tiempo.TotalMinutes < 60 Then
                Dim minutos As Integer = Convert.ToInt32(tiempo.TotalMinutes)
                tb.Text = minutos.ToString + "m"
            ElseIf tiempo.TotalHours > 0 And tiempo.TotalHours < 24 Then
                Dim horas As Integer = Convert.ToInt32(tiempo.TotalHours)
                tb.Text = horas.ToString + "h"
            ElseIf tiempo.TotalDays > 0 Then
                Dim dias As Integer = Convert.ToInt32(tiempo.TotalDays)
                tb.Text = dias.ToString + "d"
            End If

        End Sub

        'BOTONES-------------------------------------------------

        Public Sub UsuarioEntraItem(sender As Object, e As PointerRoutedEventArgs)

            Dim item As ListViewItem = sender
            Dim grid As Grid = item.Content
            Dim subgrid As Grid = grid.Children(1)
            Dim sp As StackPanel = subgrid.Children(1)
            Dim spBotones As StackPanel = sp.Children(sp.Children.Count - 2)

            For Each boton In spBotones.Children
                boton.Visibility = Visibility.Visible
            Next

        End Sub

        Public Sub UsuarioSaleItem(sender As Object, e As PointerRoutedEventArgs)

            Dim item As ListViewItem = sender
            Dim grid As Grid = item.Content
            Dim subgrid As Grid = grid.Children(1)
            Dim sp As StackPanel = subgrid.Children(1)
            Dim spBotones As StackPanel = sp.Children(sp.Children.Count - 2)
            Dim gridResponder As Grid = sp.Children(sp.Children.Count - 1)

            If gridResponder.Visibility = Visibility.Collapsed Then
                Dim mostrar As Boolean = False

                For Each boton As Button In spBotones.Children
                    Dim cosas As Objetos.TweetXamlBoton = boton.Tag

                    If cosas.Mostrar = True Then
                        mostrar = True
                    End If
                Next

                If mostrar = False Then
                    For Each boton As Button In spBotones.Children
                        boton.Visibility = Visibility.Collapsed
                    Next
                Else
                    For Each boton As Button In spBotones.Children
                        boton.Visibility = Visibility.Visible
                    Next
                End If
            Else
                For Each boton As Button In spBotones.Children
                    boton.Visibility = Visibility.Visible
                Next
            End If

        End Sub

    End Module
End Namespace

