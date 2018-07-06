Imports pepeizq.Twitter
Imports Windows.System.Threading

Namespace pepeizq.Twitter
    Public Class MegaUsuario

        Public Usuario As TwitterUsuario
        Public Servicio As TwitterServicio
        Public NotificacionInicio As Boolean
        Public NotificacionMenciones As Boolean
        Public StreamHome As ThreadPoolTimer
        Public StreamMentions As ThreadPoolTimer
        Public UsuariosBloqueados As List(Of String)
        Public UsuariosMuteados As List(Of String)

        Public Sub New(usuario As TwitterUsuario, servicio As TwitterServicio, notificacionInicio As Boolean, notificacionMenciones As Boolean,
                       streamHome As ThreadPoolTimer, streamMentions As ThreadPoolTimer,
                       usuariosBloqueados As List(Of String), usuariosMuteados As List(Of String))
            Me.Usuario = usuario
            Me.Servicio = servicio
            Me.NotificacionInicio = notificacionInicio
            Me.NotificacionMenciones = notificacionMenciones
            Me.StreamHome = streamHome
            Me.StreamMentions = streamMentions
            Me.UsuariosBloqueados = usuariosBloqueados
            Me.UsuariosMuteados = usuariosMuteados
        End Sub

    End Class
End Namespace

