Namespace pepeizq.Twitter.Objetos
    Public Class TextoTweetEntidad

        Public Mostrar As String
        Public Enlace As String
        Public Coordenadas As List(Of Integer)
        Public Tipo As Integer

        Public Sub New(mostrar As String, enlace As String, coordenadas As List(Of Integer), tipo As Integer)
            Me.Mostrar = mostrar
            Me.Enlace = enlace
            Me.Coordenadas = coordenadas
            Me.Tipo = tipo
        End Sub

    End Class
End Namespace

