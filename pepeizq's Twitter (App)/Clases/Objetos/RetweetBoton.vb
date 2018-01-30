Imports pepeizq.Twitter.Tweet

Namespace pepeTwitter.Objetos
    Public Class RetweetBoton

        Public Tweet As Tweet
        Public MegaUsuario As MegaUsuario
        Public Grid As Grid

        Public Sub New(tweet As Tweet, megaUsuario As MegaUsuario, grid As Grid)
            Me.Tweet = tweet
            Me.MegaUsuario = megaUsuario
            Me.Grid = grid
        End Sub

    End Class
End Namespace

