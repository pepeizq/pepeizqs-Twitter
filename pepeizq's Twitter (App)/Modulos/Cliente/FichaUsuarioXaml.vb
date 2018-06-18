Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Newtonsoft.Json
Imports pepeizq.Twitter
Imports pepeizq.Twitter.OAuth
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core
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
            Dim enlaceString As String = "https://api.twitter.com/1.1/users/show.json?screen_name=" + cosas.ScreenNombre

            Dim enlace As New Uri(enlaceString)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, cosas.MegaUsuario.Servicio.twitterDataProvider._tokens)

            cosas.Usuario = JsonConvert.DeserializeObject(Of TwitterUsuario)(resultado)
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
            gridUsuario.Background = New SolidColorBrush(color)
        Else
            Dim acrilico As New AcrylicBrush With {
                .BackgroundSource = AcrylicBackgroundSource.Backdrop,
                .TintOpacity = 0.7,
                .TintColor = color
            }

            gridUsuario.Background = acrilico
        End If

        If Not objetoAnimar Is Nothing Then
            Try
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionUsuario", objetoAnimar)

                Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionUsuario")

                If Not animacion Is Nothing Then
                    animacion.TryStart(gridUsuario)
                End If
            Catch ex As Exception

            End Try
        End If

        gridUsuario.Visibility = Visibility.Visible

        Dim lvTweets As ListView = pagina.FindName("lvTweetsUsuario")
        lvTweets.IsItemClickEnabled = True

        If lvTweets.Items.Count > 0 Then
            lvTweets.Items.Clear()
        End If

        AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

        Dim prTweets As ProgressRing = pagina.FindName("prTweetsUsuario")
        prTweets.Foreground = New SolidColorBrush(color)

        Dim svTweets As ScrollViewer = pagina.FindName("svTweetsUsuario")
        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(cosas.MegaUsuario, Nothing, prTweets, 2, usuario.ScreenNombre, color)
        svTweets.Foreground = New SolidColorBrush(("#" + usuario.ColorTexto).ToColor)
        AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

        '------------------------------------

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

        Dim botonEnlace As Button = pagina.FindName("botonEnlaceUsuario")

        If Not usuario.Entidades.Enlace Is Nothing Then
            Try
                botonEnlace.Tag = New Uri(usuario.Entidades.Enlace.Enlaces(0).Expandida)

                Dim tbEnlace As New TextBlock With {
                    .Text = usuario.Entidades.Enlace.Enlaces(0).Mostrar,
                    .Foreground = New SolidColorBrush(Colors.White),
                    .FontWeight = FontWeights.SemiBold
                }

                botonEnlace.Content = tbEnlace
                botonEnlace.Visibility = Visibility.Visible
            Catch ex As Exception
                botonEnlace.Content = Nothing
                botonEnlace.Visibility = Visibility.Collapsed
            End Try
        Else
            botonEnlace.Content = Nothing
            botonEnlace.Visibility = Visibility.Collapsed
        End If

        Dim tbNumTweets As TextBlock = pagina.FindName("tbNumTweetsUsuario")
        tbNumTweets.Text = String.Format("{0:n0}", Integer.Parse(usuario.NumTweets))

        Dim botonNumTweets As Button = pagina.FindName("botonNumTweetsUsuario")
        botonNumTweets.Tag = New Uri("https://twitter.com/" + usuario.ScreenNombre)

        Dim tbNumSeguidores As TextBlock = pagina.FindName("tbNumSeguidoresUsuario")
        tbNumSeguidores.Text = String.Format("{0:n0}", Integer.Parse(usuario.Followers))

        Dim botonSeguidores As Button = pagina.FindName("botonSeguidoresUsuario")
        botonSeguidores.Tag = New Uri("https://twitter.com/" + usuario.ScreenNombre + "/followers")

        Dim tbNumFavoritos As TextBlock = pagina.FindName("tbNumFavoritosUsuario")
        tbNumFavoritos.Text = String.Format("{0:n0}", Integer.Parse(usuario.Favoritos))

        Dim botonFavoritos As Button = pagina.FindName("botonFavoritosUsuario")
        botonFavoritos.Tag = New Uri("https://twitter.com/" + usuario.ScreenNombre + "/likes")

        '------------------------------------

        Dim botonSeguir As Button = pagina.FindName("botonSeguirUsuario")
        Dim tbSeguir As TextBlock = pagina.FindName("tbSeguirUsuario")

        If usuario.Siguiendo = True Then
            tbSeguir.Text = recursos.GetString("Following")
        Else
            tbSeguir.Text = recursos.GetString("Follow")
        End If

        botonSeguir.Background = New SolidColorBrush(color)
        botonSeguir.Tag = New pepeizq.Twitter.Objetos.SeguirUsuarioBoton(cosas.MegaUsuario, cosas.Usuario)
        AddHandler botonSeguir.Click, AddressOf BotonSeguirClick

        Dim botonMasOpciones As Button = pagina.FindName("botonMasOpcionesUsuario")
        botonMasOpciones.Background = New SolidColorBrush(color)
        botonMasOpciones.Tag = cosas

        AddHandler botonMasOpciones.Click, AddressOf BotonMasOpcionesClick

        Dim botonSubir As Button = pagina.FindName("botonSubirArribaUsuario")
        botonSubir.Background = New SolidColorBrush(color)

        '------------------------------------

        Dim listaTweets As New List(Of Tweet)

        listaTweets = Await TwitterPeticiones.UserTimeline(listaTweets, cosas.MegaUsuario, usuario.ScreenNombre, Nothing)

        If listaTweets.Count > 0 Then
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
                    lvTweets.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, cosas.MegaUsuario, color))
                End If
            Next
        End If

    End Sub

    Private Async Sub BotonSeguirClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim tbSeguir As TextBlock = boton.Content
        Dim cosas As pepeizq.Twitter.Objetos.SeguirUsuarioBoton = boton.Tag

        If tbSeguir.Text = recursos.GetString("Following") Then
            Await cosas.MegaUsuario.Servicio.DeshacerSeguirUsuario(cosas.MegaUsuario.Servicio.twitterDataProvider._tokens, cosas.Usuario.ID)
            tbSeguir.Text = recursos.GetString("Follow")
        Else
            Await cosas.MegaUsuario.Servicio.SeguirUsuario(cosas.MegaUsuario.Servicio.twitterDataProvider._tokens, cosas.Usuario.ID)
            tbSeguir.Text = recursos.GetString("Following")
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
        AddHandler botonBloquearUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonBloquearUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonBloquearUsuario)

        Dim botonMutearUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("Mute") + " @" + cosas.Usuario.ScreenNombre,
            .Tag = cosas
        }

        AddHandler botonMutearUsuario.Click, AddressOf BotonMutearUsuarioClick
        AddHandler botonMutearUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonMutearUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonMutearUsuario)

        Dim botonReportarUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("Report") + " @" + cosas.Usuario.ScreenNombre,
            .Tag = cosas
        }

        AddHandler botonReportarUsuario.Click, AddressOf BotonReportarUsuarioClick
        AddHandler botonReportarUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonReportarUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonReportarUsuario)

        Dim separador As New MenuFlyoutSeparator
        menu.Items.Add(separador)

        Dim botonCompartirUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("ShareUser"),
            .Tag = cosas
        }

        AddHandler botonCompartirUsuario.Click, AddressOf BotonCompartirUsuarioClick
        AddHandler botonCompartirUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonCompartirUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonCompartirUsuario)

        Dim botonAbrirNavegadorUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("OpenWebBrowserUser"),
            .Tag = cosas
        }

        AddHandler botonAbrirNavegadorUsuario.Click, AddressOf BotonAbrirNavegadorUsuarioClick
        AddHandler botonAbrirNavegadorUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonAbrirNavegadorUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonAbrirNavegadorUsuario)

        FlyoutBase.SetAttachedFlyout(boton, menu)
        menu.ShowAt(boton)

    End Sub

    Private Async Sub BotonBloquearUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await cosas.MegaUsuario.Servicio.BloquearUsuario(cosas.MegaUsuario.Servicio.twitterDataProvider._tokens, cosas.Usuario.ScreenNombre)
        Catch ex As Exception

        End Try

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Async Sub BotonMutearUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await cosas.MegaUsuario.Servicio.MutearUsuario(cosas.MegaUsuario.Servicio.twitterDataProvider._tokens, cosas.Usuario.ScreenNombre)
        Catch ex As Exception

        End Try

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Async Sub BotonReportarUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await cosas.MegaUsuario.Servicio.ReportarUsuario(cosas.MegaUsuario.Servicio.twitterDataProvider._tokens, cosas.Usuario.ScreenNombre)
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

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
