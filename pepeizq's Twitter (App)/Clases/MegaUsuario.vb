Imports pepeizq.Twitter

Namespace pepeizq.Twitter
    Public Class MegaUsuario

        Public Usuario As TwitterUsuario
        Public Servicio As TwitterServicio

        Public Sub New(usuario As TwitterUsuario, servicio As TwitterServicio)
            Me.Usuario = usuario
            Me.Servicio = servicio
        End Sub

    End Class
End Namespace

