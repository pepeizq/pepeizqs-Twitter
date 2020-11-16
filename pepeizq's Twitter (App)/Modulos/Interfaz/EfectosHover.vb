Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Windows.UI.Core

Namespace Interfaz
    Module EfectosHover

        Public Sub Entra_Basico(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Basico(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_IconoNombre(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim icono As FontAwesome5.FontAwesome = sp.Children(0)
            icono.Saturation(1).Scale(1.1, 1.1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Dim texto As TextBlock = sp.Children(1)
            texto.Saturation(1).Scale(1.1, 1.1, texto.ActualWidth / 2, texto.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_IconoNombre(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim icono As FontAwesome5.FontAwesome = sp.Children(0)
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Dim texto As TextBlock = sp.Children(1)
            texto.Saturation(1).Scale(1, 1, texto.ActualWidth / 2, texto.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As FontAwesome5.FontAwesome = boton.Content
            icono.Saturation(1).Scale(1.1, 1.1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As FontAwesome5.FontAwesome = boton.Content
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace