Imports Windows.UI

Namespace pepeizq.Twitter.Objetos
    Public Class ScrollViewerTweets

        Public MegaUsuario As MegaUsuario
        Public Anillo1 As ProgressRing
        Public Anillo2 As ProgressRing
        Public Query As Integer
        Public UsuarioScreenNombre As String
        Public Color As Color

        Public Sub New(megaUsuario As MegaUsuario, anillo1 As ProgressRing, anillo2 As ProgressRing, query As Integer,
                       usuarioScreenNombre As String, color As Color)
            Me.MegaUsuario = megaUsuario
            Me.Anillo1 = anillo1
            Me.Anillo2 = anillo2
            Me.Query = query
            Me.UsuarioScreenNombre = usuarioScreenNombre
            Me.Color = color
        End Sub

    End Class
End Namespace

