Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Module FichaOEmbedXaml

    Public Async Sub Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, tweet As Tweet)

        Dim oembed As New pepeizq.Twitter.OEmbed

        oembed = Await TwitterPeticiones.CogerOEmbedTweet(oembed, megaUsuario, "https://twitter.com/" + tweet.Usuario.ScreenNombre + "/status/" + tweet.ID)

        If Not oembed Is Nothing Then
            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridOEmbed As Grid = pagina.FindName("gridOEmbedAmpliado")
            gridOEmbed.Visibility = Visibility.Visible

            Dim tbOEmbed As TextBox = pagina.FindName("tbOEmbed")
            tbOEmbed.Text = oembed.Html
        End If

    End Sub

End Module
