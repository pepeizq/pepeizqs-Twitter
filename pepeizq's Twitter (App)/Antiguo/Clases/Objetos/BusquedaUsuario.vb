Namespace pepeizq.Twitter.Objetos
    Public Class BusquedaUsuario

        Public MegaUsuario As MegaUsuario
        Public Busqueda As String
        Public Resultados As GridView
        Public Fila2 As RowDefinition

        Public Sub New(megaUsuario As MegaUsuario, busqueda As String, resultados As GridView, fila2 As RowDefinition)
            Me.MegaUsuario = megaUsuario
            Me.Busqueda = busqueda
            Me.Resultados = resultados
            Me.Fila2 = fila2
        End Sub

    End Class
End Namespace
