Imports Windows.UI

Namespace pepeizq.Twitter.Objetos
    Public Class ScrollViewerTweets

        Public MegaUsuario As MegaUsuario
        Public Anillo As ProgressRing
        Public Barra As ProgressBar
        Public Query As Integer
        Public UsuarioScreenNombre As String
        Public Color As Color

        Public Sub New(megaUsuario As MegaUsuario, anillo As ProgressRing, barra As ProgressBar, query As Integer,
                       usuarioScreenNombre As String, color As Color)
            Me.MegaUsuario = megaUsuario
            Me.Anillo = anillo
            Me.Barra = barra
            Me.Query = query
            Me.UsuarioScreenNombre = usuarioScreenNombre
            Me.Color = color
        End Sub

    End Class
End Namespace

