Imports pepeizq.Twitter
Imports Windows.System.Threading

Namespace pepeizq.Twitter
    Public Class MegaUsuario

        Public Usuario As TwitterUsuario
        Public Servicio As TwitterServicio
        Public Notificacion As Boolean
        Public StreamHome As ThreadPoolTimer
        Public StreamMentions As ThreadPoolTimer
        Public UsuariosBloqueados As List(Of String)

        Public Sub New(usuario As TwitterUsuario, servicio As TwitterServicio, notificacion As Boolean,
                       streamHome As ThreadPoolTimer, streamMentions As ThreadPoolTimer,
                       usuariosBloqueados As List(Of String))
            Me.Usuario = usuario
            Me.Servicio = servicio
            Me.Notificacion = notificacion
            Me.StreamHome = streamHome
            Me.StreamMentions = streamMentions
            Me.UsuariosBloqueados = usuariosBloqueados
        End Sub

    End Class
End Namespace

