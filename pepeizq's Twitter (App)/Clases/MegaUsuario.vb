Imports pepeizq.Twitter

Namespace pepeizq.Twitter
    Public Class MegaUsuario

        Public Usuario2 As TwitterUsuario2
        Public Servicio As TwitterServicio

        Public Sub New(usuario2 As TwitterUsuario2, servicio As TwitterServicio)
            Me.Usuario2 = usuario2
            Me.Servicio = servicio
        End Sub

    End Class
End Namespace

