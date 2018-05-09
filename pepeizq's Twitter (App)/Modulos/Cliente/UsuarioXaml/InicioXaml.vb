﻿Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Core

Module InicioXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario2.Usuario

        Dim gridTweets As New Grid
        gridTweets.SetValue(Grid.RowProperty, 1)
        gridTweets.Name = "gridTweets" + usuario.ScreenNombre
        gridTweets.Visibility = visibilidad

        Dim color1 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#e0e0e0"),
            .Offset = 0.5
        }

        Dim color2 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#d6d6d6"),
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

        gridTweets.Background = brush

        Dim rowTweets1 As New RowDefinition
        Dim rowTweets2 As New RowDefinition

        rowTweets1.Height = New GridLength(1, GridUnitType.Star)
        rowTweets2.Height = New GridLength(1, GridUnitType.Auto)

        gridTweets.RowDefinitions.Add(rowTweets1)
        gridTweets.RowDefinitions.Add(rowTweets2)

        '---------------------------------

        Dim prTweets As New ProgressRing
        prTweets.SetValue(Grid.RowProperty, 0)
        prTweets.IsActive = True
        prTweets.Width = 50
        prTweets.Height = 50
        prTweets.Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
        prTweets.VerticalAlignment = VerticalAlignment.Center
        prTweets.HorizontalAlignment = HorizontalAlignment.Center
        prTweets.Name = "prTweets" + usuario.ScreenNombre

        gridTweets.Children.Add(prTweets)

        '---------------------------------

        Dim svTweets As New ScrollViewer
        svTweets.SetValue(Grid.RowProperty, 0)
        AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

        gridTweets.Children.Add(svTweets)

        '---------------------------------

        Dim lvTweets As New ListView
        lvTweets.SetValue(Grid.RowProperty, 0)
        lvTweets.IsItemClickEnabled = True
        lvTweets.ItemContainerStyle = App.Current.Resources("ListViewEstilo1")
        lvTweets.Tag = usuario
        AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

        svTweets.Content = lvTweets

        '---------------------------------

        Dim iconoSubir As New FontAwesome.UWP.FontAwesome With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Icon = FontAwesomeIcon.ArrowCircleUp
        }

        Dim botonSubir As New Button
        botonSubir.SetValue(Grid.RowProperty, 0)
        botonSubir.Name = "botonSubirArribaInicio" + usuario.ScreenNombre
        botonSubir.Margin = New Thickness(20, 20, 20, 20)
        botonSubir.HorizontalAlignment = HorizontalAlignment.Right
        botonSubir.VerticalAlignment = VerticalAlignment.Bottom
        botonSubir.Padding = New Thickness(10, 10, 10, 10)
        botonSubir.BorderBrush = New SolidColorBrush(Colors.White)
        botonSubir.BorderThickness = New Thickness(1, 1, 1, 1)
        botonSubir.Content = iconoSubir
        botonSubir.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonSubir.Visibility = Visibility.Collapsed
        botonSubir.Tag = svTweets

        AddHandler botonSubir.Click, AddressOf BotonSubirClick
        AddHandler botonSubir.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonSubir.PointerExited, AddressOf UsuarioSaleBoton

        gridTweets.Children.Add(botonSubir)

        '---------------------------------

        Dim pbTweets As New ProgressBar
        pbTweets.SetValue(Grid.RowProperty, 1)
        pbTweets.IsIndeterminate = True
        pbTweets.Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
        pbTweets.Visibility = Visibility.Collapsed
        pbTweets.Margin = New Thickness(10, 10, 10, 10)
        pbTweets.Padding = New Thickness(10, 10, 10, 10)
        pbTweets.Name = "pbTweets" + usuario.ScreenNombre

        gridTweets.Children.Add(pbTweets)

        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(megaUsuario, prTweets, pbTweets, 0, Nothing, Nothing)

        '---------------------------------

        Return gridTweets

    End Function

    Public Async Sub SvTweets_ViewChanging(sender As Object, e As ScrollViewerViewChangingEventArgs)

        Dim sv As ScrollViewer = sender
        Dim cosas As pepeizq.Twitter.Objetos.ScrollViewerTweets = sv.Tag

        Dim pr As ProgressRing = cosas.Anillo
        Dim pb As ProgressBar = cosas.Barra

        Dim lv As ListView = Nothing

        If TypeOf sv.Content Is ListView Then
            lv = sv.Content
        End If

        If TypeOf sv.Content Is Grid Then
            Dim grid As Grid = sv.Content
            lv = grid.Children(1)
        End If

        lv.Tag = cosas.MegaUsuario

        If pb.Visibility = Visibility.Collapsed Then
            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            If sv.VerticalOffset > 50 Then
                If cosas.Query = 0 Then
                    Dim botonSubir As Button = pagina.FindName("botonSubirArribaInicio" + cosas.MegaUsuario.Usuario2.Usuario.ScreenNombre)
                    botonSubir.Visibility = Visibility.Visible
                ElseIf cosas.Query = 1 Then
                    Dim botonSubir As Button = pagina.FindName("botonSubirArribaMenciones" + cosas.MegaUsuario.Usuario2.Usuario.ScreenNombre)
                    botonSubir.Visibility = Visibility.Visible
                ElseIf cosas.Query = 2 Then
                    Dim botonSubir As Button = pagina.FindName("botonSubirArribaUsuario")
                    botonSubir.Visibility = Visibility.Visible
                End If
            Else
                If cosas.Query = 0 Then
                    Dim botonSubir As Button = pagina.FindName("botonSubirArribaInicio" + cosas.MegaUsuario.Usuario2.Usuario.ScreenNombre)
                    botonSubir.Visibility = Visibility.Collapsed
                ElseIf cosas.Query = 1 Then
                    Dim botonSubir As Button = pagina.FindName("botonSubirArribaMenciones" + cosas.MegaUsuario.Usuario2.Usuario.ScreenNombre)
                    botonSubir.Visibility = Visibility.Collapsed
                ElseIf cosas.Query = 2 Then
                    Dim botonSubir As Button = pagina.FindName("botonSubirArribaUsuario")
                    botonSubir.Visibility = Visibility.Collapsed
                End If
            End If

            If (sv.ScrollableHeight - 200) < sv.VerticalOffset Then
                Dim mostrar As Boolean = False

                If pr Is Nothing Then
                    mostrar = True
                Else
                    If pr.IsActive = False Then
                        mostrar = True
                    End If
                End If

                If mostrar = True Then
                    pb.Visibility = Visibility.Visible

                    If lv.Items.Count > 0 And lv.Items.Count < 280 Then
                        Dim lvItem As ListViewItem = lv.Items(lv.Items.Count - 1)
                        Dim gridTweet As Grid = lvItem.Content
                        Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
                        Dim ultimoTweet As Tweet = tweetAmpliado.Tweet

                        If Not ultimoTweet.ID = Nothing Then
                            Dim provider As TwitterDataProvider = cosas.MegaUsuario.Servicio.Provider
                            Dim listaTweets As New List(Of Tweet)

                            Try
                                If cosas.Query = 0 Then
                                    listaTweets = Await provider.CogerTweetsTimelineInicio(Of Tweet)(cosas.MegaUsuario.Usuario2.Usuario.Tokens, ultimoTweet.ID, New TweetParser)
                                ElseIf cosas.Query = 1 Then
                                    listaTweets = Await provider.CogerTweetsTimelineMenciones(Of Tweet)(cosas.MegaUsuario.Usuario2.Usuario.Tokens, ultimoTweet.ID, New TweetParser)
                                ElseIf cosas.Query = 2 Then
                                    listaTweets = Await provider.CogerTweetsTimelineUsuario(Of Tweet)(cosas.UsuarioScreenNombre, ultimoTweet.ID, New TweetParser)
                                End If
                            Catch ex As Exception

                            End Try

                            If listaTweets.Count > 0 Then
                                For Each tweet In listaTweets
                                    Dim boolAñadir As Boolean = True

                                    For Each item In lv.Items
                                        If TypeOf item Is ListViewItem Then
                                            Dim lvItem2 As ListViewItem = item
                                            Dim gridTweet2 As Grid = lvItem2.Content
                                            Dim tweetAmpliado2 As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet2.Tag
                                            Dim lvTweet As Tweet = tweetAmpliado2.Tweet

                                            If lvTweet.ID = tweet.ID Then
                                                boolAñadir = False
                                            End If
                                        End If
                                    Next

                                    If boolAñadir = True Then
                                        lv.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, cosas.MegaUsuario, cosas.Color))
                                    End If
                                Next
                            End If
                        End If
                    End If

                    pb.Visibility = Visibility.Collapsed
                End If
            End If
        End If

    End Sub

    Public Sub LvTweets_ItemClick(sender As Object, e As ItemClickEventArgs)

        Dim grid As Grid = e.ClickedItem

        pepeizq.Twitter.Xaml.FichaTweet.Generar(grid.Tag, grid)

    End Sub

    Private Sub BotonSubirClick(sender As Object, e As RoutedEventArgs)

        Dim botonSubir As Button = sender
        Dim svTweets As ScrollViewer = botonSubir.Tag

        svTweets.ChangeView(Nothing, 0, Nothing)
        botonSubir.Visibility = Visibility.Collapsed

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module