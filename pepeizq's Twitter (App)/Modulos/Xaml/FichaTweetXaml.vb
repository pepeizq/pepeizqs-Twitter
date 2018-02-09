Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Xaml.Media.Animation

Module FichaTweetXaml

    Public Async Sub Generar(cosas As pepeizq.Twitter.Objetos.TweetAmpliado, objetoAnimar As Object)

        Dim tweet As Tweet = cosas.Tweet

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")
        gridUsuario.Visibility = Visibility.Collapsed

        Dim gridTitulo As Grid = pagina.FindName("gridTitulo")
        Dim gridTweet As Grid = pagina.FindName("gridTweetAmpliado")

        Dim provider As TwitterDataProvider = cosas.MegaUsuario.Servicio.Provider

        Dim tweetNuevo As Tweet = Await provider.CogerTweet(cosas.MegaUsuario.Usuario.Tokens, tweet.ID, New TweetParserIndividual)

        Dim color As Color = Nothing

        Try
            color = ("#" + tweetNuevo.Usuario.ColorEnlace).ToColor
        Catch ex As Exception
            color = App.Current.Resources("ColorSecundario")
        End Try

        App.Current.Resources("ButtonBackgroundPointerOver") = color

        Dim transpariencia As New UISettings
        Dim boolTranspariencia As Boolean = transpariencia.AdvancedEffectsEnabled

        If boolTranspariencia = False Then
            gridTitulo.Background = New SolidColorBrush(color)
            gridTweet.Background = New SolidColorBrush(color)
        Else
            Dim acrilico As New AcrylicBrush With {
                .BackgroundSource = AcrylicBackgroundSource.Backdrop,
                .TintOpacity = 0.7,
                .TintColor = color
            }

            gridTitulo.Background = acrilico
            gridTweet.Background = acrilico
        End If

        Try
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionTweet", objetoAnimar)

            Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionTweet")

            If Not animacion Is Nothing Then
                animacion.TryStart(gridTweet)
            End If
        Catch ex As Exception

        End Try

        gridTweet.Visibility = Visibility.Visible

        '-----------------------------

        Dim botonCerrar As Button = pagina.FindName("botonCerrarTweet")
        botonCerrar.Background = New SolidColorBrush(color)

        Dim spIzquierda As StackPanel = pagina.FindName("spTweetIzquierda")
        spIzquierda.Children.Clear()
        spIzquierda.Children.Add(pepeTwitterXaml.TweetXamlAvatar.Generar(tweetNuevo, cosas.MegaUsuario))

        Dim spDerecha As StackPanel = pagina.FindName("spTweetDerecha")
        spDerecha.Children.Clear()
        spDerecha.Children.Add(pepeTwitterXaml.TweetXamlUsuario.Generar(tweetNuevo, cosas.MegaUsuario, color))
        spDerecha.Children.Add(pepeTwitterXaml.TweetXamlTexto.Generar(tweetNuevo, Nothing, color, cosas.MegaUsuario))

        If Not tweetNuevo.Cita Is Nothing Then
            spDerecha.Children.Add(pepeTwitterXaml.TweetXamlCita.Generar(tweetNuevo, cosas.MegaUsuario, color))
        End If

        spDerecha.Children.Add(pepeTwitterXaml.TweetXamlMedia.Generar(tweetNuevo, color))
        spDerecha.Children.Add(pepeTwitterXaml.TweetXamlBotones.Generar(tweetNuevo, gridTweet, cosas.MegaUsuario, 1, color))
        spDerecha.Children.Add(pepeTwitterXaml.TweetXamlEnviarTweet.Generar(tweetNuevo, cosas.MegaUsuario, Visibility.Collapsed, color))

    End Sub

End Module
