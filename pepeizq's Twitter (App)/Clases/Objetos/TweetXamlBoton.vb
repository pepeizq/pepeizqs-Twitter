Imports pepeizq.Twitter.Tweet

Namespace pepeTwitter.Objetos
    Public Class TweetXamlBoton

        Public Tweet As Tweet
        Public MegaUsuario As MegaUsuario
        Public Grid As Grid
        Public Mostrar As Boolean
        Public Boton As Button

        Public Sub New(tweet As Tweet, megaUsuario As MegaUsuario, grid As Grid, mostrar As Boolean, boton As Button)
            Me.Tweet = tweet
            Me.MegaUsuario = megaUsuario
            Me.Grid = grid
            Me.Mostrar = mostrar
            Me.Boton = boton
        End Sub

    End Class
End Namespace

