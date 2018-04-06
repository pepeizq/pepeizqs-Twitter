Imports System.Globalization
Imports System.Net
Imports FontAwesome.UWP
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core

Namespace pepeTwitterXaml
    Module TweetXamlBotones

        Public Function Generar(tweet As Tweet, gridTweet As Grid, megaUsuario As pepeizq.Twitter.MegaUsuario, estilo As Integer, color As Color)

            Dim colorBoton As Color = Nothing

            If color = Nothing Then
                colorBoton = App.Current.Resources("ColorSecundario")
            Else
                colorBoton = color
            End If

            Dim recursos As New Resources.ResourceLoader

            Dim spBotones As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(5, 5, 0, 0),
                .Height = 35
            }

            '------------------------------------------

            Dim botonResponder As New Button With {
                .Content = ConstructorBotones(FontAwesomeIcon.Reply, recursos.GetString("ReplyTweet"), colorBoton),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeizq.Twitter.Objetos.TweetXamlBoton(tweet, megaUsuario, gridTweet, False, Nothing, color),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            If estilo = 0 Then
                botonResponder.Visibility = Visibility.Collapsed
            ElseIf estilo = 1 Then
                botonResponder.Visibility = Visibility.Visible
            End If

            AddHandler botonResponder.Click, AddressOf BotonResponderClick
            AddHandler botonResponder.PointerEntered, AddressOf BotonResponderUsuarioEntra
            AddHandler botonResponder.PointerExited, AddressOf BotonResponderUsuarioSale

            spBotones.Children.Add(botonResponder)

            '------------------------------------------

            Dim botonRetweet As New Button With {
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeizq.Twitter.Objetos.TweetXamlBoton(tweet, megaUsuario, gridTweet, False, Nothing, color),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            If estilo = 0 Then
                botonRetweet.Visibility = Visibility.Collapsed
                botonRetweet.Content = ConstructorBotones(FontAwesomeIcon.Retweet, recursos.GetString("Retweet"), colorBoton)
            ElseIf estilo = 1 Then
                Dim spBoton As New StackPanel With {
                    .Orientation = Orientation.Horizontal
                }

                spBoton.Children.Add(ConstructorBotones(FontAwesomeIcon.Retweet, recursos.GetString("Retweet"), colorBoton))

                Dim tbBoton As New TextBlock With {
                    .Text = tweet.NumRetweets,
                    .VerticalAlignment = VerticalAlignment.Center,
                    .Margin = New Thickness(5, 0, 0, 0)
                }

                spBoton.Children.Add(tbBoton)

                botonRetweet.Content = spBoton
                botonRetweet.Visibility = Visibility.Visible
            End If

            If tweet.Retwitteado = True Then
                botonRetweet.Background = New SolidColorBrush(colorBoton)
                botonRetweet.Foreground = New SolidColorBrush(Colors.White)
            End If

            AddHandler botonRetweet.Click, AddressOf BotonRetweetClick
            AddHandler botonRetweet.PointerEntered, AddressOf BotonRetweetUsuarioEntra
            AddHandler botonRetweet.PointerExited, AddressOf BotonRetweetUsuarioSale

            spBotones.Children.Add(botonRetweet)

            '------------------------------------------

            Dim botonFavorito As New Button With {
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeizq.Twitter.Objetos.TweetXamlBoton(tweet, megaUsuario, gridTweet, False, Nothing, color),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            If tweet.Favoriteado = True Then
                botonFavorito.Background = New SolidColorBrush(colorBoton)
                botonFavorito.Foreground = New SolidColorBrush(Colors.White)
            End If

            If estilo = 0 Then
                botonFavorito.Visibility = Visibility.Collapsed
                botonFavorito.Content = ConstructorBotones(FontAwesomeIcon.Heart, recursos.GetString("Favorite"), colorBoton)
            ElseIf estilo = 1 Then
                Dim spBoton As New StackPanel With {
                    .Orientation = Orientation.Horizontal
                }

                spBoton.Children.Add(ConstructorBotones(FontAwesomeIcon.Heart, recursos.GetString("Favorite"), colorBoton))

                Dim tbBoton As New TextBlock With {
                    .Text = tweet.NumFavoritos,
                    .VerticalAlignment = VerticalAlignment.Center,
                    .Margin = New Thickness(5, 0, 0, 0)
                }

                spBoton.Children.Add(tbBoton)

                botonFavorito.Content = spBoton
                botonFavorito.Visibility = Visibility.Visible
            End If

            AddHandler botonFavorito.Click, AddressOf BotonFavoritoClick
            AddHandler botonFavorito.PointerEntered, AddressOf BotonFavoritoUsuarioEntra
            AddHandler botonFavorito.PointerExited, AddressOf BotonFavoritoUsuarioSale

            spBotones.Children.Add(botonFavorito)

            '------------------------------------------

            Dim botonMasOpciones As New Button With {
                .Content = ConstructorBotones(FontAwesomeIcon.EllipsisH, recursos.GetString("MoreOptions"), colorBoton),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeizq.Twitter.Objetos.TweetXamlBoton(tweet, megaUsuario, gridTweet, False, Nothing, color),
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            If estilo = 0 Then
                botonMasOpciones.Visibility = Visibility.Collapsed
            ElseIf estilo = 1 Then
                botonMasOpciones.Visibility = Visibility.Visible
            End If

            AddHandler botonMasOpciones.Click, AddressOf BotonMasOpcionesClick
            AddHandler botonMasOpciones.PointerEntered, AddressOf BotonMasOpcionesUsuarioEntra
            AddHandler botonMasOpciones.PointerExited, AddressOf BotonMasOpcionesUsuarioSale

            spBotones.Children.Add(botonMasOpciones)

            '------------------------------------------

            If estilo = 1 Then

                If Not tweet.Creacion = Nothing Then
                    Dim fecha As DateTime = Nothing

                    DateTime.TryParseExact(tweet.Creacion, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, fecha)

                    Dim tbCreacion As New TextBlock With {
                        .Text = fecha,
                        .Margin = New Thickness(40, 0, 0, 0),
                        .VerticalAlignment = VerticalAlignment.Center
                    }

                    spBotones.Children.Add(tbCreacion)
                End If

                '----------------

                If Not tweet.ClienteUsado = Nothing Then
                    Dim cliente As String = tweet.ClienteUsado

                    Dim temp, temp2 As String
                    Dim int, int2 As Integer

                    int = cliente.IndexOf(ChrW(34))
                    temp = cliente.Remove(0, int + 1)

                    int2 = temp.IndexOf(ChrW(34))
                    temp2 = temp.Remove(int2, temp.Length - int2)

                    Dim enlace As String = temp2.Trim

                    Dim temp3, temp4 As String
                    Dim int3, int4 As Integer

                    int3 = cliente.IndexOf(">")
                    temp3 = cliente.Remove(0, int3 + 1)

                    int4 = temp3.IndexOf("<")
                    temp4 = temp3.Remove(int4, temp3.Length - int4)

                    Dim nombreCliente As String = temp4.Trim

                    Dim tbCliente As New TextBlock With {
                        .Text = nombreCliente,
                        .Foreground = New SolidColorBrush(Colors.Black)
                    }

                    Dim botonCliente As New HyperlinkButton With {
                        .Content = tbCliente,
                        .Margin = New Thickness(25, 0, 0, 0),
                        .NavigateUri = New Uri(enlace)
                    }

                    spBotones.Children.Add(botonCliente)
                End If

            End If

            '------------------------------------------

            Return spBotones

        End Function

        Private Sub BotonResponderClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim nombreGrid As String = Nothing

            If cosas.Tweet Is Nothing Then
                nombreGrid = "gridTweetEscribir"
            Else
                nombreGrid = "gridTweetEscribir" + cosas.Tweet.ID
            End If

            Dim gridResponder As Grid = pagina.FindName(nombreGrid)

            If gridResponder.Visibility = Visibility.Visible Then
                gridResponder.Visibility = Visibility.Collapsed
                boton.Background = New SolidColorBrush(Colors.Transparent)
                boton.Foreground = New SolidColorBrush(Colors.Black)
            Else
                Dim color As Color = Nothing

                If cosas.Color = Nothing Then
                    color = App.Current.Resources("ColorSecundario")
                End If

                gridResponder.Visibility = Visibility.Visible
                boton.Background = New SolidColorBrush(cosas.Color)
                boton.Foreground = New SolidColorBrush(Colors.White)
            End If

        End Sub

        Private Async Sub BotonRetweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Dim status As New TwitterStatus With {
                .TweetID = cosas.Tweet.ID
            }

            Dim recursos As New Resources.ResourceLoader

            If cosas.Tweet.Retwitteado = False Then
                Await cosas.MegaUsuario.Servicio.Retwitear(cosas.MegaUsuario.Usuario.Tokens, status)

                Notificaciones.Toast.Enseñar(recursos.GetString("RetweetSent"))

                Dim color As Color = Nothing

                If cosas.Color = Nothing Then
                    color = App.Current.Resources("ColorSecundario")
                End If

                boton.Background = New SolidColorBrush(color)

                Dim spBoton As StackPanel = boton.Content

                If TypeOf spBoton.Children(0) Is TextBlock Then
                    Dim tb As TextBlock = spBoton.Children(0)
                    tb.Foreground = New SolidColorBrush(Colors.White)
                End If

                cosas.Tweet.Retwitteado = True
            Else
                Await cosas.MegaUsuario.Servicio.DeshacerRetweet(cosas.MegaUsuario.Usuario.Tokens, status)

                boton.Background = New SolidColorBrush(Colors.Transparent)

                Dim spBoton As StackPanel = boton.Content

                If TypeOf spBoton.Children(0) Is TextBlock Then
                    Dim tb As TextBlock = spBoton.Children(0)
                    tb.Foreground = New SolidColorBrush(Colors.Black)
                End If

                cosas.Tweet.Retwitteado = False
            End If

        End Sub

        Private Async Sub BotonFavoritoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Dim status As New TwitterStatus With {
                .TweetID = cosas.Tweet.ID
            }

            Dim recursos As New Resources.ResourceLoader

            If cosas.Tweet.Favoriteado = False Then
                Await cosas.MegaUsuario.Servicio.Favoritear(cosas.MegaUsuario.Usuario.Tokens, status)

                Notificaciones.Toast.Enseñar(recursos.GetString("FavoriteSent"))

                boton.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

                Dim spBoton As StackPanel = boton.Content
                Dim tb As TextBlock = spBoton.Children(0)
                tb.Foreground = New SolidColorBrush(Colors.White)

                cosas.Tweet.Favoriteado = True
            Else
                Await cosas.MegaUsuario.Servicio.DeshacerFavorito(cosas.MegaUsuario.Usuario.Tokens, status)

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
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            cosas.Mostrar = True
            cosas.Boton = boton

            Dim color As New Color

            If Not cosas.Color = Nothing Then
                color = cosas.Color
            Else
                color = App.Current.Resources("ColorSecundario")
            End If

            Dim menu As New MenuFlyout With {
                .Placement = FlyoutPlacementMode.Bottom
            }

            AddHandler menu.Closed, AddressOf MenuMasOpcionesCerrar

            Dim iconoCompartirTweet As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.ShareAlt,
                .Foreground = New SolidColorBrush(color)
            }

            Dim botonCompartirTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("ShareTweet"),
                .Tag = cosas,
                .Icon = iconoCompartirTweet
            }

            AddHandler botonCompartirTweet.Click, AddressOf BotonCompartirTweetClick
            AddHandler botonCompartirTweet.PointerEntered, AddressOf BotonMasOpcionesUsuarioEntra
            AddHandler botonCompartirTweet.PointerExited, AddressOf BotonMasOpcionesUsuarioSale
            menu.Items.Add(botonCompartirTweet)

            Dim iconoCopiarEnlaceTweet As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.Copy,
                .Foreground = New SolidColorBrush(color)
            }

            Dim botonCopiarEnlaceTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("CopyUrlTweet"),
                .Tag = cosas,
                .Icon = iconoCopiarEnlaceTweet
            }

            AddHandler botonCopiarEnlaceTweet.Click, AddressOf BotonCopiarEnlaceTweetClick
            AddHandler botonCopiarEnlaceTweet.PointerEntered, AddressOf BotonMasOpcionesUsuarioEntra
            AddHandler botonCopiarEnlaceTweet.PointerExited, AddressOf BotonMasOpcionesUsuarioSale
            menu.Items.Add(botonCopiarEnlaceTweet)

            Dim separador As New MenuFlyoutSeparator
            menu.Items.Add(separador)

            Dim iconoAbrirNavegadorTweet As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.Edge,
                .Foreground = New SolidColorBrush(color)
            }

            Dim botonAbrirNavegadorTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("OpenWebBrowserTweet"),
                .Tag = cosas,
                .Icon = iconoAbrirNavegadorTweet
            }

            AddHandler botonAbrirNavegadorTweet.Click, AddressOf BotonAbrirNavegadorTweetClick
            AddHandler botonAbrirNavegadorTweet.PointerEntered, AddressOf BotonMasOpcionesUsuarioEntra
            AddHandler botonAbrirNavegadorTweet.PointerExited, AddressOf BotonMasOpcionesUsuarioSale
            menu.Items.Add(botonAbrirNavegadorTweet)

            Dim iconoOEmbedTweet As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.Code,
                .Foreground = New SolidColorBrush(color)
            }

            Dim botonOEmbedTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("OEmbedTweet"),
                .Tag = cosas,
                .Icon = iconoOEmbedTweet
            }

            AddHandler botonOEmbedTweet.Click, AddressOf BotonOEmbedTweetClick
            AddHandler botonOEmbedTweet.PointerEntered, AddressOf BotonMasOpcionesUsuarioEntra
            AddHandler botonOEmbedTweet.PointerExited, AddressOf BotonMasOpcionesUsuarioSale
            menu.Items.Add(botonOEmbedTweet)

            FlyoutBase.SetAttachedFlyout(boton, menu)
            menu.ShowAt(boton)

        End Sub

        '------------------------------------------

        Private Function ConstructorBotones(icono As FontAwesomeIcon, texto As String, color As Color)

            Dim simbolo As New FontAwesome.UWP.FontAwesome With {
                .Icon = icono,
                .FontSize = 20,
                .VerticalAlignment = VerticalAlignment.Center,
                .Foreground = New SolidColorBrush(color),
                .Tag = color
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

            sp.Children.Add(simbolo)

            Return sp

        End Function

        Private Sub BotonResponderUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim nombreGrid As String = Nothing

            If cosas.Tweet Is Nothing Then
                nombreGrid = "gridTweetEscribir"
            Else
                nombreGrid = "gridTweetEscribir" + cosas.Tweet.ID
            End If

            Dim gridResponder As Grid = pagina.FindName(nombreGrid)

            If Not gridResponder Is Nothing Then
                Dim spBoton As StackPanel = boton.Content

                Dim icono As FontAwesome.UWP.FontAwesome = spBoton.Children(0)

                Dim color As Color = Nothing

                If cosas.Color = Nothing Then
                    color = App.Current.Resources("ColorSecundario")
                Else
                    color = cosas.Color
                End If

                If gridResponder.Visibility = Visibility.Collapsed Then
                    icono.Foreground = New SolidColorBrush(Colors.Black)
                    boton.Background = New SolidColorBrush(Colors.Transparent)
                Else
                    icono.Foreground = New SolidColorBrush(Colors.White)
                    boton.Background = New SolidColorBrush(color)
                End If
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub BotonResponderUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim nombreGrid As String = Nothing

            If cosas.Tweet Is Nothing Then
                nombreGrid = "gridTweetEscribir"
            Else
                nombreGrid = "gridTweetEscribir" + cosas.Tweet.ID
            End If

            Dim gridResponder As Grid = pagina.FindName(nombreGrid)

            If Not gridResponder Is Nothing Then
                Dim spBoton As StackPanel = boton.Content

                Dim icono As FontAwesome.UWP.FontAwesome = spBoton.Children(0)

                Dim color As Color = Nothing

                If cosas.Color = Nothing Then
                    color = App.Current.Resources("ColorSecundario")
                Else
                    color = cosas.Color
                End If

                If gridResponder.Visibility = Visibility.Collapsed Then
                    icono.Foreground = New SolidColorBrush(icono.Tag)
                    boton.Background = New SolidColorBrush(Colors.Transparent)
                Else
                    icono.Foreground = New SolidColorBrush(Colors.White)
                    boton.Background = New SolidColorBrush(color)
                End If
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Private Sub BotonRetweetUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim spBoton As StackPanel = boton.Content

            If TypeOf spBoton.Children(0) Is FontAwesome.UWP.FontAwesome Then
                Dim icono As FontAwesome.UWP.FontAwesome = spBoton.Children(0)
                icono.Foreground = New SolidColorBrush(Colors.Green)
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub BotonRetweetUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag
            Dim spBoton As StackPanel = boton.Content

            If TypeOf spBoton.Children(0) Is FontAwesome.UWP.FontAwesome Then
                Dim icono As FontAwesome.UWP.FontAwesome = spBoton.Children(0)

                If cosas.Tweet.Retwitteado = True Then
                    icono.Foreground = New SolidColorBrush(Colors.White)
                Else
                    icono.Foreground = New SolidColorBrush(icono.Tag)
                End If
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Private Sub BotonFavoritoUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim spBoton As StackPanel = boton.Content

            If TypeOf spBoton.Children(0) Is FontAwesome.UWP.FontAwesome Then
                Dim icono As FontAwesome.UWP.FontAwesome = spBoton.Children(0)
                icono.Foreground = New SolidColorBrush(Colors.Red)
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub BotonFavoritoUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag
            Dim spBoton As StackPanel = boton.Content

            If TypeOf spBoton.Children(0) Is FontAwesome.UWP.FontAwesome Then
                Dim icono As FontAwesome.UWP.FontAwesome = spBoton.Children(0)

                If cosas.Tweet.Favoriteado = True Then
                    icono.Foreground = New SolidColorBrush(Colors.White)
                Else
                    icono.Foreground = New SolidColorBrush(icono.Tag)
                End If
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Private Sub BotonMasOpcionesUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub BotonMasOpcionesUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        '------------------------------------------

        Private Sub MenuMasOpcionesCerrar(sender As Object, e As Object)

            Dim menu As MenuFlyout = sender
            Dim boton As MenuFlyoutItem = menu.Items(0)
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            cosas.Mostrar = False

        End Sub

        Private Sub BotonCompartirTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            ApplicationData.Current.LocalSettings.Values("TweetCompartirTitulo") = "@" + cosas.Tweet.Usuario.ScreenNombre
            ApplicationData.Current.LocalSettings.Values("TweetCompartirDescripcion") = WebUtility.HtmlDecode(cosas.Tweet.Texto)
            ApplicationData.Current.LocalSettings.Values("TweetCompartirEnlace") = "https://twitter.com/" + cosas.Tweet.Usuario.ScreenNombre + "/status/" + cosas.Tweet.ID

            Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
            AddHandler datos.DataRequested, AddressOf DatosCompartirClick

            DataTransferManager.ShowShareUI()

        End Sub

        Private Sub DatosCompartirClick(sender As Object, e As DataRequestedEventArgs)

            Dim request As DataRequest = e.Request

            request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("TweetCompartirTitulo")
            request.Data.Properties.Description = ApplicationData.Current.LocalSettings.Values("TweetCompartirDescripcion")
            request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("TweetCompartirEnlace")))

        End Sub

        Private Sub BotonCopiarEnlaceTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Dim texto As New DataPackage
            texto.SetText("https://twitter.com/" + cosas.Tweet.Usuario.ScreenNombre + "/status/" + cosas.Tweet.ID)

            Clipboard.SetContent(texto)

        End Sub

        Private Async Sub BotonAbrirNavegadorTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            Try
                Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/" + cosas.Tweet.Usuario.ScreenNombre + "/status/" + cosas.Tweet.ID))
            Catch ex As Exception

            End Try

        End Sub

        Private Sub BotonOEmbedTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As pepeizq.Twitter.Objetos.TweetXamlBoton = boton.Tag

            FichaOEmbedXaml.Generar(cosas.MegaUsuario.Servicio.Provider, cosas.Tweet)

        End Sub

    End Module
End Namespace

