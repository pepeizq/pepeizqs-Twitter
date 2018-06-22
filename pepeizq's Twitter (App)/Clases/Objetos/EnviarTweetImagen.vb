Imports Windows.Storage

Namespace pepeizq.Twitter.Objetos
    Public Class EnviarTweetImagen

        Public SpImagenes As StackPanel
        Public FicheroImagen As StorageFile

        Public Sub New(spImagenes As StackPanel, ficheroImagen As StorageFile)
            Me.SpImagenes = spImagenes
            Me.FicheroImagen = ficheroImagen
        End Sub

    End Class
End Namespace
