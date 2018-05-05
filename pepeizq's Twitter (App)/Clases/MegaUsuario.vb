Imports pepeizq.Twitter

Namespace pepeizq.Twitter
    Public Class MegaUsuario

        Public Usuario As TwitterUsuario
        Public Servicio As TwitterServicio
        Public Notificacion As Boolean

        Public Sub New(usuario As TwitterUsuario, servicio As TwitterServicio, notificacion As Boolean)
            Me.Usuario = usuario
            Me.Servicio = servicio
            Me.Notificacion = notificacion
        End Sub

    End Class
End Namespace

