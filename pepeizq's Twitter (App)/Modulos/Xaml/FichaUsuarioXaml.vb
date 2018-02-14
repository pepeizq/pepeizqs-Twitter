Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Banner
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Text
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Shapes

Module FichaUsuarioXaml

    Public Async Sub Generar(cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado, objetoAnimar As Object)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridTweetAmpliado As Grid = pagina.FindName("gridTweetAmpliado")
        gridTweetAmpliado.Visibility = Visibility.Collapsed

        Dim provider As TwitterDataProvider = cosas.MegaUsuario.Servicio.Provider
        Dim usuario As TwitterUsuario = Nothing

        If Not cosas.Usuario Is Nothing Then
            usuario = cosas.Usuario
        Else
            usuario = Await provider.GenerarUsuario(cosas.ScreenNombre)
            cosas.Usuario = usuario
        End If

        Dim gridTitulo As Grid = pagina.FindName("gridTitulo")
        Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")

        Dim color As Color = Nothing

        Try
            color = ("#" + usuario.ColorEnlace).ToColor
        Catch ex As Exception
            color = App.Current.Resources("ColorSecundario")
        End Try

        App.Current.Resources("ButtonBackgroundPointerOver") = color

        Dim transpariencia As New UISettings
        Dim boolTranspariencia As Boolean = transpariencia.AdvancedEffectsEnabled

        If boolTranspariencia = False Then
            gridTitulo.Background = New SolidColorBrush(color)
            gridUsuario.Background = New SolidColorBrush(color)
        Else
            Dim acrilico As New AcrylicBrush With {
                .BackgroundSource = AcrylicBackgroundSource.Backdrop,
                .TintOpacity = 0.7,
                .TintColor = color
            }

            gridTitulo.Background = acrilico
            gridUsuario.Background = acrilico
        End If

        Try
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionUsuario", objetoAnimar)

            Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionUsuario")

            If Not animacion Is Nothing Then
                animacion.TryStart(gridUsuario)
            End If
        Catch ex As Exception

        End Try

        gridUsuario.Visibility = Visibility.Visible

        Dim lvTweets As ListView = pagina.FindName("lvTweetsUsuario")
        lvTweets.IsItemClickEnabled = True

        If lvTweets.Items.Count > 0 Then
            lvTweets.Items.Clear()
        End If

        AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

        Dim pbTweets As ProgressBar = pagina.FindName("pbTweetsUsuario")
        Dim svTweets As ScrollViewer = pagina.FindName("svTweetsUsuario")
        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(cosas.MegaUsuario, Nothing, pbTweets, 2, usuario.ScreenNombre, color)
        svTweets.Foreground = New SolidColorBrush(("#" + usuario.ColorTexto).ToColor)
        AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

        '------------------------------------

        Dim botonCerrar As Button = pagina.FindName("botonCerrarUsuario")
        botonCerrar.Background = New SolidColorBrush(color)

        Dim banner As Banner = Nothing

        Try
            banner = Await provider.CogerBannerUsuario(usuario.ScreenNombre, New BannerParser)
        Catch ex As Exception

        End Try

        Dim spFondo As StackPanel = pagina.FindName("gridImagenFondoUsuario")
        Dim imagenFondo As ImageEx = pagina.FindName("imagenFondoUsuario")

        If Not banner Is Nothing Then
            imagenFondo.Source = New Uri(banner.Tamaños.I1500x500.Enlace)
            spFondo.Background = New SolidColorBrush(Colors.Transparent)
        Else
            imagenFondo.Source = Nothing
            spFondo.Background = New SolidColorBrush(Colors.Black)
        End If

        Dim circuloAvatar As Ellipse = pagina.FindName("ellipseAvatar")

        Dim imagenAvatar As New ImageBrush With {
            .Stretch = Stretch.Uniform,
            .ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar.Replace("_normal.png", "_bigger.png")))
        }

        circuloAvatar.Fill = imagenAvatar

        Dim tbNombre As TextBlock = pagina.FindName("tbNombreUsuario")
        tbNombre.Text = usuario.Nombre

        Dim imagenVerificado As ImageEx = pagina.FindName("imagenUsuarioVerificado")

        If usuario.Verificado = True Then
            imagenVerificado.Visibility = Visibility.Visible
        Else
            imagenVerificado.Visibility = Visibility.Collapsed
        End If

        Dim tbScreenNombre As TextBlock = pagina.FindName("tbScreenNombreUsuario")
        tbScreenNombre.Text = "@" + usuario.ScreenNombre

        Dim hlEnlace As HyperlinkButton = pagina.FindName("hlEnlaceUsuario")

        If Not usuario.Entidades.Enlace Is Nothing Then
            Try
                hlEnlace.NavigateUri = New Uri(usuario.Entidades.Enlace.Enlaces(0).Expandida)

                Dim tbEnlace As New TextBlock With {
                    .Text = usuario.Entidades.Enlace.Enlaces(0).Mostrar,
                    .Foreground = New SolidColorBrush(Colors.White),
                    .FontWeight = FontWeights.SemiBold
                }

                hlEnlace.Content = tbEnlace
            Catch ex As Exception
                hlEnlace.Content = Nothing
            End Try
        Else
            hlEnlace.Content = Nothing
        End If

        Dim tbNumTweets As TextBlock = pagina.FindName("tbNumTweetsUsuario")
        tbNumTweets.Text = String.Format("{0:n0}", Integer.Parse(usuario.NumTweets))

        Dim hlNumTweets As HyperlinkButton = pagina.FindName("hlNumTweetsUsuario")
        hlNumTweets.NavigateUri = New Uri("https://twitter.com/" + usuario.ScreenNombre)

        Dim tbNumSeguidores As TextBlock = pagina.FindName("tbNumSeguidoresUsuario")
        tbNumSeguidores.Text = String.Format("{0:n0}", Integer.Parse(usuario.Followers))

        Dim hlSeguidores As HyperlinkButton = pagina.FindName("hlSeguidoresUsuario")
        hlSeguidores.NavigateUri = New Uri("https://twitter.com/" + usuario.ScreenNombre + "/followers")

        Dim tbNumFavoritos As TextBlock = pagina.FindName("tbNumFavoritosUsuario")
        tbNumFavoritos.Text = String.Format("{0:n0}", Integer.Parse(usuario.Favoritos))

        Dim hlFavoritos As HyperlinkButton = pagina.FindName("hlFavoritosUsuario")
        hlFavoritos.NavigateUri = New Uri("https://twitter.com/" + usuario.ScreenNombre + "/likes")

        '------------------------------------

        Dim botonSeguir As Button = pagina.FindName("botonSeguirUsuario")

        If usuario.Siguiendo = True Then
            botonSeguir.Content = recursos.GetString("Following")
        Else
            botonSeguir.Content = recursos.GetString("Follow")
        End If

        botonSeguir.Background = New SolidColorBrush(color)
        botonSeguir.Tag = New pepeizq.Twitter.Objetos.SeguirUsuarioBoton(cosas.MegaUsuario, cosas.Usuario)
        AddHandler botonSeguir.Click, AddressOf BotonSeguirClick

        Dim botonMasOpciones As Button = pagina.FindName("botonMasOpcionesUsuario")
        botonMasOpciones.Content = Char.ConvertFromUtf32(57361)
        botonMasOpciones.Background = New SolidColorBrush(color)
        botonMasOpciones.Tag = cosas

        AddHandler botonMasOpciones.Click, AddressOf BotonMasOpcionesClick

        '------------------------------------

        Dim listaTweets As New List(Of Tweet)

        Try
            listaTweets = Await provider.CogerTweetsTimelineUsuario(Of Tweet)(usuario.ScreenNombre, Nothing, New TweetParser)
        Catch ex As Exception

        End Try

        For Each tweet In listaTweets
            Dim boolAñadir As Boolean = True

            For Each item In lvTweets.Items
                Dim lvItem As ListViewItem = item
                Dim gridTweet As Grid = lvItem.Content
                Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
                Dim lvTweet As Tweet = tweetAmpliado.Tweet

                If lvTweet.ID = tweet.ID Then
                    boolAñadir = False
                End If
            Next

            If boolAñadir = True Then
                lvTweets.Items.Add(TweetXaml.Añadir(tweet, cosas.MegaUsuario, color))
            End If
        Next

    End Sub

    Private Async Sub BotonSeguirClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.SeguirUsuarioBoton = boton.Tag

        If boton.Content = recursos.GetString("Following") Then
            Await cosas.MegaUsuario.Servicio.DeshacerSeguirUsuario(cosas.MegaUsuario.Usuario.Tokens, cosas.Usuario.Id)
            boton.Content = recursos.GetString("Follow")
        Else
            Await cosas.MegaUsuario.Servicio.SeguirUsuario(cosas.MegaUsuario.Usuario.Tokens, cosas.Usuario.Id)
            boton.Content = recursos.GetString("Following")
        End If

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Sub BotonMasOpcionesClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Dim menu As New MenuFlyout With {
            .Placement = FlyoutPlacementMode.Bottom
        }

        Dim botonBloquearUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("Block") + " @" + cosas.Usuario.ScreenNombre,
            .Tag = cosas
        }

        AddHandler botonBloquearUsuario.Click, AddressOf BotonBloquearUsuarioClick
        menu.Items.Add(botonBloquearUsuario)

        Dim botonMutearUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("Mute") + " @" + cosas.Usuario.ScreenNombre,
            .Tag = cosas
        }

        AddHandler botonMutearUsuario.Click, AddressOf BotonMutearUsuarioClick
        menu.Items.Add(botonMutearUsuario)

        Dim botonReportarUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("Report") + " @" + cosas.Usuario.ScreenNombre,
            .Tag = cosas
        }

        AddHandler botonReportarUsuario.Click, AddressOf BotonReportarUsuarioClick
        menu.Items.Add(botonReportarUsuario)

        Dim separador As New MenuFlyoutSeparator
        menu.Items.Add(separador)

        Dim botonCompartirUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("ShareUser"),
            .Tag = cosas
        }

        AddHandler botonCompartirUsuario.Click, AddressOf BotonCompartirUsuarioClick
        menu.Items.Add(botonCompartirUsuario)

        Dim botonAbrirNavegadorUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("OpenWebBrowserUser"),
            .Tag = cosas
        }

        AddHandler botonAbrirNavegadorUsuario.Click, AddressOf BotonAbrirNavegadorUsuarioClick
        menu.Items.Add(botonAbrirNavegadorUsuario)

        FlyoutBase.SetAttachedFlyout(boton, menu)
        menu.ShowAt(boton)

    End Sub

    Private Async Sub BotonBloquearUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await cosas.MegaUsuario.Servicio.BloquearUsuario(cosas.MegaUsuario.Usuario.Tokens, cosas.Usuario.ScreenNombre)
        Catch ex As Exception

        End Try

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Async Sub BotonMutearUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await cosas.MegaUsuario.Servicio.MutearUsuario(cosas.MegaUsuario.Usuario.Tokens, cosas.Usuario.ScreenNombre)
        Catch ex As Exception

        End Try

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Async Sub BotonReportarUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await cosas.MegaUsuario.Servicio.ReportarUsuario(cosas.MegaUsuario.Usuario.Tokens, cosas.Usuario.ScreenNombre)
        Catch ex As Exception

        End Try

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Async Sub BotonAbrirNavegadorUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/" + cosas.Usuario.ScreenNombre))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub BotonCompartirUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        ApplicationData.Current.LocalSettings.Values("UsuarioCompartirTitulo") = "@" + cosas.Usuario.ScreenNombre
        ApplicationData.Current.LocalSettings.Values("UsuarioCompartirDescripcion") = cosas.Usuario.Nombre
        ApplicationData.Current.LocalSettings.Values("UsuarioCompartirEnlace") = "https://twitter.com/" + cosas.Usuario.ScreenNombre

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf DatosCompartirClick

        DataTransferManager.ShowShareUI()

    End Sub

    Private Sub DatosCompartirClick(sender As Object, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request

        request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("UsuarioCompartirTitulo")
        request.Data.Properties.Description = ApplicationData.Current.LocalSettings.Values("UsuarioCompartirDescripcion")
        request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("UsuarioCompartirEnlace")))

    End Sub

End Module
