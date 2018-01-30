Imports pepeizq.Twitter.Tweet

Namespace pepeTwitter.Objetos
    Public Class RetweetBoton

        Public Tweet As Tweet
        Public MegaUsuario As MegaUsuario

        Public Sub New(tweet As Tweet, megaUsuario As MegaUsuario)
            Me.Tweet = tweet
            Me.MegaUsuario = megaUsuario
        End Sub

    End Class
End Namespace

