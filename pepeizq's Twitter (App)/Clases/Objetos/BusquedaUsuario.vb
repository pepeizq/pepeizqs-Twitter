Namespace pepeizq.Twitter.Objetos
    Public Class BusquedaUsuario

        Public MegaUsuario As MegaUsuario
        Public Busqueda As String
        Public ListView As ListView

        Public Sub New(megaUsuario As MegaUsuario, busqueda As String, listview As ListView)
            Me.MegaUsuario = megaUsuario
            Me.Busqueda = busqueda
            Me.ListView = listview
        End Sub

    End Class
End Namespace
