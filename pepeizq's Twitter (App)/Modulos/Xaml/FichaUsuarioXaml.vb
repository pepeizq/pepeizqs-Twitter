Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Banner
Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Xaml.Shapes

Module FichaUsuarioXaml

    Public Async Sub Generar(cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado)

        Dim recursos As New Resources.ResourceLoader

        Dim usuario As TwitterUsuario = cosas.Usuario
        Dim provider As TwitterDataProvider = cosas.MegaUsuario.Servicio.Provider

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridTitulo As Grid = pagina.FindName("gridTitulo")

        Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")
        gridUsuario.Visibility = Visibility.Visible

        Dim color As Color = Nothing

        Try
            color = ("#" + usuario.ColorEnlace).ToColor
        Catch ex As Exception
            color = App.Current.Resources("ColorSecundario")
        End Try

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

        Dim lvTweets As ListView = pagina.FindName("lvTweetsUsuario")

        If lvTweets.Items.Count > 0 Then
            lvTweets.Items.Clear()
        End If

        Dim pbTweets As ProgressBar = pagina.FindName("pbTweetsUsuario")
        Dim svTweets As ScrollViewer = pagina.FindName("svTweetsUsuario")
        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(cosas.MegaUsuario, Nothing, pbTweets, 2, usuario.ScreenNombre)
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
            imagenFondo.Source = New Uri(banner.Tamaños.I600x200.Enlace)
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

        Dim spEnlace As StackPanel = pagina.FindName("spEnlaceUsuario")

        If Not usuario.Entidades.Enlaces Is Nothing Then
            If Not usuario.Entidades.Enlaces(0) Is Nothing Then
                Try
                    Dim hlEnlace As HyperlinkButton = pagina.FindName("hlEnlaceUsuario")
                    hlEnlace.NavigateUri = New Uri(usuario.Entidades.Enlaces(0).Expandida)
                    hlEnlace.Content = usuario.Entidades.Enlaces(0).Mostrar
                    spEnlace.Visibility = Visibility.Visible
                Catch ex As Exception
                    spEnlace.Visibility = Visibility.Collapsed
                End Try
            Else
                spEnlace.Visibility = Visibility.Collapsed
            End If
        Else
            spEnlace.Visibility = Visibility.Collapsed
        End If

        Dim tbNumTweets As TextBlock = pagina.FindName("tbNumTweetsUsuario")
        tbNumTweets.Text = String.Format("{0:n0}", Integer.Parse(usuario.NumTweets))

        Dim tbNumSeguidores As TextBlock = pagina.FindName("tbNumSeguidoresUsuario")
        tbNumSeguidores.Text = String.Format("{0:n0}", Integer.Parse(usuario.Followers))

        Dim botonSeguir As Button = pagina.FindName("botonSeguirUsuario")

        If usuario.Siguiendo = True Then
            botonSeguir.Content = recursos.GetString("Following")
        Else
            botonSeguir.Content = recursos.GetString("Follow")
        End If

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
                Dim lvTweet As Tweet = gridTweet.Tag

                If lvTweet.ID = tweet.ID Then
                    boolAñadir = False
                End If
            Next

            If boolAñadir = True Then
                lvTweets.Items.Add(TweetXaml.Añadir(tweet, cosas.MegaUsuario, color))
            End If
        Next

    End Sub

End Module
