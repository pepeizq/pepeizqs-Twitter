Imports pepeizq.Twitter

Namespace pepeizq.Twitter
    Public Class TwitterUsuario2

        Public Usuario As TwitterUsuario
        Public Notificacion As Boolean

        Public Sub New(usuario As TwitterUsuario, notificacion As Boolean)
            Me.Usuario = usuario
            Me.Notificacion = notificacion
        End Sub

    End Class
End Namespace