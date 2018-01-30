Imports Microsoft.Toolkit.Uwp.UI.Controls

Namespace pepeTwitter.Objetos
    Public Class EnviarTweetTextoCambia

        Public Boton As Button
        Public Contador As TextBlock
        Public Anillo As RadialProgressBar

        Public Sub New(boton As Button, contador As TextBlock, anillo As RadialProgressBar)
            Me.Boton = boton
            Me.Contador = contador
            Me.Anillo = anillo
        End Sub

    End Class
End Namespace

