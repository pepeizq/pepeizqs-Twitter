Imports pepeizq.Twitter

Namespace pepeizq.Twitter.Objetos
    Public Class UsuarioAmpliado

        Public MegaUsuario As MegaUsuario
        Public Usuario As TwitterUsuario

        Public Sub New(megaUsuario As MegaUsuario, usuario As TwitterUsuario)
            Me.MegaUsuario = megaUsuario
            Me.Usuario = usuario
        End Sub

    End Class
End Namespace

