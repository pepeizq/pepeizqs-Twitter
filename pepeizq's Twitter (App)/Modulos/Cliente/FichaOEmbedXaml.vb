Imports pepeizq.Twitter
Imports pepeizq.Twitter.OEmbed
Imports pepeizq.Twitter.Tweet

Module FichaOEmbedXaml

    Public Async Sub Generar(provider As TwitterDataProvider, tweet As Tweet)

        Dim oembed As OEmbed = Await provider.CogerOEmbedTweet("https://twitter.com/" + tweet.Usuario.ScreenNombre + "/status/" + tweet.ID, New OEmbedParser)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridOEmbed As Grid = pagina.FindName("gridOEmbedAmpliado")
        gridOEmbed.Visibility = Visibility.Visible

        Dim tbOEmbed As TextBox = pagina.FindName("tbOEmbed")
        tbOEmbed.Text = oembed.Html

    End Sub

End Module
