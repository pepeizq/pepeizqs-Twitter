Imports pepeizq.Twitter.Tweet

Namespace pepeTwitter.Objetos
    Public Class EnviarTweetBoton

        Public CajaTexto As TextBox
        Public MegaUsuario As MegaUsuario
        Public Tweet As Tweet
        Public ListaMenciones As TweetMencion()

        Public Sub New(cajaTexto As TextBox, megaUsuario As MegaUsuario, tweet As Tweet, listaMenciones As TweetMencion())
            Me.CajaTexto = cajaTexto
            Me.MegaUsuario = megaUsuario
            Me.Tweet = tweet
            Me.ListaMenciones = listaMenciones
        End Sub

    End Class
End Namespace

