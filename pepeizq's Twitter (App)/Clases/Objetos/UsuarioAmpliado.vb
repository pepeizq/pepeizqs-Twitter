Imports pepeizq.Twitter
Imports Windows.UI

Namespace pepeizq.Twitter.Objetos
    Public Class UsuarioAmpliado

        Public MegaUsuario As MegaUsuario
        Public Usuario As TwitterUsuario
        Public ScreenNombre As String

        Public Sub New(megaUsuario As MegaUsuario, usuario As TwitterUsuario, screenNombre As String)
            Me.MegaUsuario = megaUsuario
            Me.Usuario = usuario
            Me.ScreenNombre = screenNombre
        End Sub

    End Class
End Namespace

