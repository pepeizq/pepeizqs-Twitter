Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI

Namespace pepeTwitterXaml
    Module TweetXamlBotones

        Public Function Generar(tweet As Tweet, gridTweet As Grid, megaUsuario As pepeTwitter.MegaUsuario)

            Dim recursos As New Resources.ResourceLoader

            Dim spBotones As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(5, 5, 0, 0),
                .Height = 35
            }

            '------------------------------------------

            Dim botonResponder As New Button With {
                .Content = ConstructorBotones(57862, recursos.GetString("ReplyTweet")),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = gridTweet,
                .Visibility = Visibility.Collapsed,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler botonResponder.Click, AddressOf BotonResponderClick
            AddHandler botonResponder.PointerEntered, AddressOf BotonResponderUsuarioEntra
            AddHandler botonResponder.PointerExited, AddressOf BotonResponderUsuarioSale

            spBotones.Children.Add(botonResponder)

            '------------------------------------------

            Dim botonRetweet As New Button With {
                .Content = ConstructorBotones(59627, recursos.GetString("Retweet")),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeTwitter.Objetos.RetweetBoton(tweet, megaUsuario, gridTweet),
                .Visibility = Visibility.Collapsed,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            If tweet.Retwitteado = True Then
                botonRetweet.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
                botonRetweet.Foreground = New SolidColorBrush(Colors.White)
            End If

            AddHandler botonRetweet.Click, AddressOf BotonRetweetClick
            AddHandler botonRetweet.PointerEntered, AddressOf BotonRetweetUsuarioEntra
            AddHandler botonRetweet.PointerExited, AddressOf BotonRetweetUsuarioSale

            spBotones.Children.Add(botonRetweet)

            '------------------------------------------

            Dim botonFavorito As New Button With {
                .Content = ConstructorBotones(57350, recursos.GetString("Favorite")),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeTwitter.Objetos.RetweetBoton(tweet, megaUsuario, gridTweet),
                .Visibility = Visibility.Collapsed,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            If tweet.Favoriteado = True Then
                botonFavorito.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
                botonFavorito.Foreground = New SolidColorBrush(Colors.White)
            End If

            AddHandler botonFavorito.Click, AddressOf BotonFavoritoClick
            AddHandler botonFavorito.PointerEntered, AddressOf BotonFavoritoUsuarioEntra
            AddHandler botonFavorito.PointerExited, AddressOf BotonFavoritoUsuarioSale

            spBotones.Children.Add(botonFavorito)

            '------------------------------------------

            Dim botonMasOpciones As New Button With {
                .Content = ConstructorBotones(59154, recursos.GetString("MoreOptions")),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeTwitter.Objetos.RetweetBoton(tweet, megaUsuario, gridTweet),
                .Visibility = Visibility.Collapsed,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler botonMasOpciones.Click, AddressOf BotonMasOpcionesClick

            spBotones.Children.Add(botonMasOpciones)

            '------------------------------------------

            Return spBotones

        End Function

        Private Sub BotonResponderClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Tag
            Dim grid2 As Grid = grid.Children(1)
            Dim sp As StackPanel = grid2.Children(1)

            Dim gridResponder As Grid = sp.Children(sp.Children.Count - 1)

            If gridResponder.Visibility = Visibility.Visible Then
                gridResponder.Visibility = Visibility.Collapsed
                boton.Background = New SolidColorBrush(Colors.Transparent)
                boton.Foreground = New SolidColorBrush(Colors.Black)
            Else
                gridResponder.Visibility = Visibility.Visible
                boton.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
                boton.Foreground = New SolidColorBrush(Colors.White)
            End If

        End Sub

        Private Async Sub BotonRetweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeTwitter.Objetos.RetweetBoton = boton.Tag

            Dim status As New TwitterStatus With {
                .TweetID = cosas.Tweet.ID
            }

            Dim recursos As New Resources.ResourceLoader

            If cosas.Tweet.Retwitteado = False Then
                Await cosas.MegaUsuario.Servicio.Retwitear(status)

                Notificaciones.Toast.Enseñar(recursos.GetString("RetweetSent"))

                boton.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

                Dim spBoton As StackPanel = boton.Content
                Dim tb As TextBlock = spBoton.Children(0)
                tb.Foreground = New SolidColorBrush(Colors.White)

                cosas.Tweet.Retwitteado = True
            Else
                Await cosas.MegaUsuario.Servicio.DeshacerRetweet(status)

                boton.Background = New SolidColorBrush(Colors.Transparent)

                Dim spBoton As StackPanel = boton.Content
                Dim tb As TextBlock = spBoton.Children(0)
                tb.Foreground = New SolidColorBrush(Colors.Black)

                cosas.Tweet.Retwitteado = False
            End If

        End Sub

        Private Async Sub BotonFavoritoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeTwitter.Objetos.RetweetBoton = boton.Tag

            Dim status As New TwitterStatus With {
                .TweetID = cosas.Tweet.ID
            }

            Dim recursos As New Resources.ResourceLoader

            If cosas.Tweet.Favoriteado = False Then
                Await cosas.MegaUsuario.Servicio.Favoritear(status)

                Notificaciones.Toast.Enseñar(recursos.GetString("FavoriteSent"))

                boton.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

                Dim spBoton As StackPanel = boton.Content
                Dim tb As TextBlock = spBoton.Children(0)
                tb.Foreground = New SolidColorBrush(Colors.White)

                cosas.Tweet.Favoriteado = True
            Else
                Await cosas.MegaUsuario.Servicio.DeshacerFavorito(status)

                boton.Background = New SolidColorBrush(Colors.Transparent)

                Dim spBoton As StackPanel = boton.Content
                Dim tb As TextBlock = spBoton.Children(0)
                tb.Foreground = New SolidColorBrush(Colors.Black)

                cosas.Tweet.Favoriteado = False
            End If

        End Sub

        Private Sub BotonMasOpcionesClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim boton As Button = sender

            Dim menu As New Flyout With {
                .Placement = FlyoutPlacementMode.Bottom
            }

            Dim listaBotones As New ListView

            Dim botonCopiarEnlaceTweet As New ListViewItem With {
                .Content = recursos.GetString("CopyUrlTweet"),
                .Style = App.Current.Resources("ListViewEstilo1")
            }

            listaBotones.Items.Add(botonCopiarEnlaceTweet)

            menu.Content = listaBotones

            FlyoutBase.SetAttachedFlyout(boton, menu)
            menu.ShowAt(boton)

        End Sub

        '------------------------------------------

        Private Function ConstructorBotones(icono As Integer, texto As String)

            Dim tbSimbolo As New TextBlock With {
                .Text = Char.ConvertFromUtf32(icono),
                .FontFamily = New FontFamily("Segoe MDL2 Assets"),
                .FontSize = 20,
                .VerticalAlignment = VerticalAlignment.Center
            }

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(2, 2, 2, 2)
            }

            Dim tbToolTip As TextBlock = New TextBlock With {
                .Text = texto,
                .FontSize = 15
            }

            ToolTipService.SetToolTip(sp, tbToolTip)
            ToolTipService.SetPlacement(sp, PlacementMode.Mouse)

            sp.Children.Add(tbSimbolo)

            Return sp

        End Function

        Private Sub BotonResponderUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Tag
            Dim grid2 As Grid = grid.Children(1)
            Dim sp As StackPanel = grid2.Children(1)

            Dim gridResponder As Grid = sp.Children(sp.Children.Count - 1)

            Dim spBoton As StackPanel = boton.Content

            Dim tb As TextBlock = spBoton.Children(0)

            If gridResponder.Visibility = Visibility.Collapsed Then
                tb.Foreground = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
                boton.Background = New SolidColorBrush(Colors.Transparent)
            Else
                tb.Foreground = New SolidColorBrush(Colors.White)
                boton.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
            End If

        End Sub

        Private Sub BotonResponderUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Tag
            Dim grid2 As Grid = grid.Children(1)
            Dim sp As StackPanel = grid2.Children(1)

            Dim gridResponder As Grid = sp.Children(sp.Children.Count - 1)

            Dim spBoton As StackPanel = boton.Content

            Dim tb As TextBlock = spBoton.Children(0)

            If gridResponder.Visibility = Visibility.Collapsed Then
                tb.Foreground = New SolidColorBrush(Colors.Black)
                boton.Background = New SolidColorBrush(Colors.Transparent)
            Else
                tb.Foreground = New SolidColorBrush(Colors.White)
                boton.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
            End If

        End Sub

        Private Sub BotonRetweetUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim spBoton As StackPanel = boton.Content

            Dim tb As TextBlock = spBoton.Children(0)
            tb.Foreground = New SolidColorBrush(Colors.Green)

        End Sub

        Private Sub BotonRetweetUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeTwitter.Objetos.RetweetBoton = boton.Tag
            Dim spBoton As StackPanel = boton.Content

            Dim tb As TextBlock = spBoton.Children(0)

            If cosas.Tweet.Retwitteado = True Then
                tb.Foreground = New SolidColorBrush(Colors.White)
            Else
                tb.Foreground = New SolidColorBrush(Colors.Black)
            End If

        End Sub

        Private Sub BotonFavoritoUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim spBoton As StackPanel = boton.Content

            Dim tb As TextBlock = spBoton.Children(0)
            tb.Foreground = New SolidColorBrush(Colors.Red)

        End Sub

        Private Sub BotonFavoritoUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeTwitter.Objetos.RetweetBoton = boton.Tag
            Dim spBoton As StackPanel = boton.Content

            Dim tb As TextBlock = spBoton.Children(0)

            If cosas.Tweet.Favoriteado = True Then
                tb.Foreground = New SolidColorBrush(Colors.White)
            Else
                tb.Foreground = New SolidColorBrush(Colors.Black)
            End If

        End Sub

    End Module
End Namespace

