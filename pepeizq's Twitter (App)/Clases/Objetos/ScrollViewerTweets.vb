Namespace pepeTwitter.Objetos
    Public Class ScrollViewerTweets

        Public MegaUsuario As MegaUsuario
        Public Anillo As ProgressRing
        Public Barra As ProgressBar
        Public Query As Integer

        Public Sub New(megaUsuario As MegaUsuario, anillo As ProgressRing, barra As ProgressBar, query As Integer)
            Me.MegaUsuario = megaUsuario
            Me.Anillo = anillo
            Me.Barra = barra
            Me.Query = query
        End Sub

    End Class
End Namespace

