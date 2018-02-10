Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports NeoSmart.Unicode
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Xaml.Documents

Namespace pepeTwitterXaml
    Module TweetXamlEnviarTweet

        Public Function Generar(tweet As Tweet, megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility, color As Color)

            If color = Nothing Then
                color = App.Current.Resources("ColorCuarto")
            End If

            Dim colorBoton As Color = Nothing

            If color = App.Current.Resources("ColorCuarto") Then
                colorBoton = App.Current.Resources("ColorSecundario")
            Else
                colorBoton = color
            End If

            Dim recursos As New Resources.ResourceLoader

            Dim nombreGrid As String = Nothing

            If tweet Is Nothing Then
                nombreGrid = "gridTweetEscribir"
            Else
                nombreGrid = "gridTweetEscribir" + tweet.ID
            End If

            Dim gridTweetEscribir As New Grid With {
                .Name = nombreGrid,
                .Visibility = visibilidad,
                .Margin = New Thickness(5, 10, 0, 5)
            }

            Dim rowTweetEscribir1 As New RowDefinition
            Dim rowTweetEscribir2 As New RowDefinition
            Dim rowTweetEscribir3 As New RowDefinition

            rowTweetEscribir1.Height = New GridLength(1, GridUnitType.Auto)
            rowTweetEscribir2.Height = New GridLength(1, GridUnitType.Auto)
            rowTweetEscribir3.Height = New GridLength(1, GridUnitType.Auto)

            gridTweetEscribir.RowDefinitions.Add(rowTweetEscribir1)
            gridTweetEscribir.RowDefinitions.Add(rowTweetEscribir2)
            gridTweetEscribir.RowDefinitions.Add(rowTweetEscribir3)

            '---------------------------------

            Dim listaMenciones As TweetMencion() = Nothing

            If Not tweet Is Nothing Then
                Dim tbRespondiendo As New TextBlock
                tbRespondiendo.SetValue(Grid.RowProperty, 0)

                Dim tbRespondiendoSpan As New Span

                Dim fragmento1 As New Run With {
                    .Text = recursos.GetString("ReplyingTo") + ": "
                }

                tbRespondiendoSpan.Inlines.Add(fragmento1)

                If Not tweet.Usuario.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
                    Dim fragmento2 As New Run With {
                        .Text = "@" + tweet.Usuario.ScreenNombre
                    }

                    Dim enlace As New Hyperlink With {
                        .NavigateUri = New Uri("https://twitter.com/" + tweet.Usuario.ScreenNombre),
                        .TextDecorations = Nothing,
                        .Foreground = New SolidColorBrush(color)
                    }

                    enlace.Inlines.Add(fragmento2)
                    tbRespondiendoSpan.Inlines.Add(enlace)
                End If

                If tweet.Retweet Is Nothing Then
                    listaMenciones = tweet.Entidades.Menciones
                Else
                    listaMenciones = tweet.Retweet.Entidades.Menciones
                End If

                For Each mencion In listaMenciones
                    If Not mencion.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
                        If tbRespondiendoSpan.Inlines.Count > 1 Then
                            Dim fragmentoAnterior As New Run With {
                               .Text = ", "
                            }

                            tbRespondiendoSpan.Inlines.Add(fragmentoAnterior)
                        End If

                        Dim fragmentoMencion As New Run With {
                            .Text = "@" + mencion.ScreenNombre
                        }

                        Dim enlaceMencion As New Hyperlink With {
                            .NavigateUri = New Uri("https://twitter.com/" + mencion.ScreenNombre),
                            .TextDecorations = Nothing,
                            .Foreground = New SolidColorBrush(color)
                        }

                        enlaceMencion.Inlines.Add(fragmentoMencion)
                        tbRespondiendoSpan.Inlines.Add(enlaceMencion)
                    End If
                Next

                tbRespondiendo.Inlines.Add(tbRespondiendoSpan)

                gridTweetEscribir.Children.Add(tbRespondiendo)
            End If

            '---------------------------------

            Dim bordeTbMensaje As New Border With {
                .BorderThickness = New Thickness(1, 1, 1, 1),
                .BorderBrush = New SolidColorBrush(color),
                .Height = 110,
                .Width = 600,
                .HorizontalAlignment = HorizontalAlignment.Left,
                .Margin = New Thickness(0, 10, 0, 10)
            }

            Dim tbMensaje As New TextBox With {
                .MaxLength = 280,
                .AcceptsReturn = True,
                .TextWrapping = TextWrapping.Wrap,
                .BorderThickness = New Thickness(0, 0, 0, 0)
            }

            AddHandler tbMensaje.TextChanged, AddressOf TbTweetEscribirTextChanged

            bordeTbMensaje.SetValue(Grid.RowProperty, 1)
            bordeTbMensaje.Child = tbMensaje

            gridTweetEscribir.Children.Add(bordeTbMensaje)

            '---------------------------------

            Dim gridInferior As New Grid
            gridInferior.SetValue(Grid.RowProperty, 2)
            gridInferior.HorizontalAlignment = HorizontalAlignment.Left
            gridInferior.Width = 600

            Dim colGridInferior1 As New ColumnDefinition
            Dim colGridInferior2 As New ColumnDefinition
            Dim colGridInferior3 As New ColumnDefinition

            colGridInferior1.Width = New GridLength(1, GridUnitType.Star)
            colGridInferior2.Width = New GridLength(1, GridUnitType.Auto)
            colGridInferior3.Width = New GridLength(1, GridUnitType.Auto)

            gridInferior.ColumnDefinitions.Add(colGridInferior1)
            gridInferior.ColumnDefinitions.Add(colGridInferior2)
            gridInferior.ColumnDefinitions.Add(colGridInferior3)

            '---------------------------------

            Dim botonEnviarTweet As New Button
            botonEnviarTweet.SetValue(Grid.ColumnProperty, 0)
            botonEnviarTweet.Padding = New Thickness(10, 10, 10, 10)
            botonEnviarTweet.Background = New SolidColorBrush(colorBoton)
            botonEnviarTweet.IsEnabled = False
            botonEnviarTweet.HorizontalAlignment = HorizontalAlignment.Left

            Dim spBoton As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim simboloBoton As New SymbolIcon With {
                .Symbol = Symbol.Send,
                .Margin = New Thickness(0, 0, 10, 0),
                .Foreground = New SolidColorBrush(Colors.White),
                .VerticalAlignment = VerticalAlignment.Center
            }

            spBoton.Children.Add(simboloBoton)

            Dim tbBoton As New TextBlock With {
                .Foreground = New SolidColorBrush(Colors.White),
                .Text = recursos.GetString("SendTweet")
            }

            spBoton.Children.Add(tbBoton)

            botonEnviarTweet.Content = spBoton
            botonEnviarTweet.Tag = New pepeizq.Twitter.Objetos.EnviarTweetBoton(tbMensaje, megaUsuario, tweet, listaMenciones)
            AddHandler botonEnviarTweet.Click, AddressOf BotonEnviarTweetClick

            gridInferior.Children.Add(botonEnviarTweet)

            '---------------------------------

            Dim prContadorCaracteres As New RadialProgressBar
            prContadorCaracteres.SetValue(Grid.ColumnProperty, 1)
            prContadorCaracteres.Maximum = 280
            prContadorCaracteres.VerticalAlignment = VerticalAlignment.Center
            prContadorCaracteres.Height = 44
            prContadorCaracteres.Width = 44

            gridInferior.Children.Add(prContadorCaracteres)

            Dim tbContadorCaracteres As New TextBlock
            tbContadorCaracteres.SetValue(Grid.ColumnProperty, 1)
            tbContadorCaracteres.VerticalAlignment = VerticalAlignment.Center
            tbContadorCaracteres.HorizontalAlignment = HorizontalAlignment.Center

            gridInferior.Children.Add(tbContadorCaracteres)

            tbMensaje.Tag = New pepeizq.Twitter.Objetos.EnviarTweetTextoCambia(botonEnviarTweet, tbContadorCaracteres, prContadorCaracteres)

            '---------------------------------

            Dim botonEmojis As New Button
            botonEmojis.SetValue(Grid.ColumnProperty, 2)
            botonEmojis.Padding = New Thickness(10, 10, 10, 10)
            botonEmojis.VerticalAlignment = VerticalAlignment.Center
            botonEmojis.Margin = New Thickness(15, 0, 0, 0)
            botonEmojis.Background = New SolidColorBrush(colorBoton)
            AddHandler botonEmojis.Click, AddressOf BotonEmojisClick

            Dim tbEmojis As New TextBlock With {
                .Text = "😃"
            }

            botonEmojis.Content = tbEmojis

            gridInferior.Children.Add(botonEmojis)

            '---------------------------------

            Dim popupEmojis As New Popup
            popupEmojis.SetValue(Grid.ColumnProperty, 0)
            popupEmojis.IsOpen = False
            popupEmojis.VerticalOffset = 50

            Dim gridEmojis As New Grid With {
                .Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#FFE4E4E4")),
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
                .BorderThickness = New Thickness(1, 1, 1, 1)
            }

            Dim gvEmojis As New GridView With {
                .MaxWidth = 600,
                .HorizontalContentAlignment = HorizontalAlignment.Center,
                .IsItemClickEnabled = True,
                .ItemContainerStyle = App.Current.Resources("GridViewEstilo1"),
                .Tag = tbMensaje
            }

            AddHandler gvEmojis.ItemClick, AddressOf GvEmojisItemClick

            gridEmojis.Children.Add(gvEmojis)
            popupEmojis.Child = gridEmojis
            botonEmojis.Tag = popupEmojis

            gridInferior.Children.Add(popupEmojis)

            '---------------------------------

            gridTweetEscribir.Children.Add(gridInferior)

            Return gridTweetEscribir

        End Function

        Private Sub TbTweetEscribirTextChanged(sender As Object, e As TextChangedEventArgs)

            Dim tb As TextBox = sender
            Dim objeto As pepeizq.Twitter.Objetos.EnviarTweetTextoCambia = tb.Tag

            Dim boton As Button = objeto.Boton

            If tb.Text.Length > 0 Then
                boton.IsEnabled = True
            Else
                boton.IsEnabled = False
            End If

            Dim contador As TextBlock = objeto.Contador
            Dim anillo As RadialProgressBar = objeto.Anillo

            anillo.Value = tb.Text.Length

            If tb.Text.Length < 141 Then
                anillo.Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                contador.Text = String.Empty
            ElseIf tb.Text.Length > 140 And tb.Text.Length < 201 Then
                anillo.Foreground = New SolidColorBrush(Windows.UI.Colors.Goldenrod)
                contador.Text = tb.Text.Length.ToString
            ElseIf tb.Text.Length > 200 Then
                anillo.Foreground = New SolidColorBrush(Windows.UI.Colors.Red)
                contador.Text = tb.Text.Length.ToString
            End If

        End Sub

        Private Async Sub BotonEnviarTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.EnviarTweetBoton = boton.Tag

            Dim tb As TextBox = cosas.CajaTexto
            Dim mensaje As String = Nothing

            mensaje = mensaje + "@" + cosas.Tweet.Usuario.ScreenNombre + " "

            For Each mencion In cosas.ListaMenciones
                If Not mencion.ScreenNombre = cosas.MegaUsuario.Usuario.ScreenNombre Then
                    mensaje = mensaje + "@" + mencion.ScreenNombre + " "
                End If
            Next

            mensaje = mensaje + tb.Text

            Dim status As New TwitterStatus With {
                .InReplyToStatusId = cosas.Tweet.ID,
                .Mensaje = mensaje
            }

            Await cosas.MegaUsuario.Servicio.EnviarTweet(cosas.MegaUsuario.Usuario.Tokens, status)

            tb.Text = String.Empty

            Dim recursos As New Resources.ResourceLoader

            Notificaciones.Toast.Enseñar(recursos.GetString("TweetSent"))

        End Sub

        Private Sub BotonEmojisClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim popup As Popup = boton.Tag
            Dim grid As Grid = popup.Child
            Dim gv As GridView = grid.Children(0)

            If popup.IsOpen = False Then
                popup.IsOpen = True

                If gv.Items.Count = 0 Then
                    Dim listaEmojis As New List(Of SingleEmoji)

                    For Each emoj In Emoji.All
                        listaEmojis.Add(emoj)
                    Next

                    Dim i As Integer = 0
                    While i < 100
                        Dim item As New GridViewItem With {
                            .Background = New SolidColorBrush(Windows.UI.Colors.Transparent),
                            .Content = listaEmojis(i).Sequence.AsString,
                            .Padding = New Thickness(0, 0, 0, 0)
                        }

                        gv.Items.Add(item)

                        i += 1
                    End While
                End If
            Else
                popup.IsOpen = False
            End If

        End Sub

        Private Sub GvEmojisItemClick(sender As Object, e As ItemClickEventArgs)

            Dim gv As GridView = sender
            Dim tb As TextBox = gv.Tag

            tb.Text = tb.Text + e.ClickedItem

        End Sub

    End Module
End Namespace

