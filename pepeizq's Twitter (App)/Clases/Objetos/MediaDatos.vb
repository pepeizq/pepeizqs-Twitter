Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI

Namespace pepeizq.Twitter.Objetos
    Public Class MediaDatos

        Public Color As Color
        Public Enlace As String
        Public Imagen As ImageEx

        Public Sub New(color As Color, enlace As String, imagen As ImageEx)
            Me.Color = color
            Me.Enlace = enlace
            Me.Imagen = imagen
        End Sub

    End Class
End Namespace
