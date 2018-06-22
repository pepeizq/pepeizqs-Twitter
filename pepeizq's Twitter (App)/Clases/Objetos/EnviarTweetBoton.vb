Imports pepeizq.Twitter.Tweet

Namespace pepeizq.Twitter.Objetos
    Public Class EnviarTweetBoton

        Public CajaTexto As TextBox
        Public MegaUsuario As MegaUsuario
        Public Tweet As Tweet
        Public ListaMenciones As TweetMencion()
        Public BotonImagenes As Button
        Public SpImagenes As StackPanel

        Public Sub New(cajaTexto As TextBox, megaUsuario As MegaUsuario, tweet As Tweet, listaMenciones As TweetMencion(), botonImagenes As Button, spImagenes As StackPanel)
            Me.CajaTexto = cajaTexto
            Me.MegaUsuario = megaUsuario
            Me.Tweet = tweet
            Me.ListaMenciones = listaMenciones
            Me.BotonImagenes = botonImagenes
            Me.SpImagenes = spImagenes
        End Sub

    End Class
End Namespace

