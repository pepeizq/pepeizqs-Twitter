Namespace pepeizq.Twitter.Objetos
    Public Class BusquedaUsuario

        Public MegaUsuario As MegaUsuario
        Public Busqueda As String
        Public GridView As GridView

        Public Sub New(megaUsuario As MegaUsuario, busqueda As String, gridview As GridView)
            Me.MegaUsuario = megaUsuario
            Me.Busqueda = busqueda
            Me.GridView = gridview
        End Sub

    End Class
End Namespace
