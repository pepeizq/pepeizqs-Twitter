Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Media.Core
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module EfectosHover

        Public Sub Entra_Basico(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Basico(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Boton_IconoTexto(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim icono As FontAwesome5.FontAwesome = sp.Children(0)
            icono.Saturation(1).Scale(1.1, 1.1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Dim texto As TextBlock = sp.Children(1)
            texto.Saturation(1).Scale(1.1, 1.1, texto.ActualWidth / 2, texto.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Boton_IconoTexto(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim icono As FontAwesome5.FontAwesome = sp.Children(0)
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Dim texto As TextBlock = sp.Children(1)
            texto.Saturation(1).Scale(1, 1, texto.ActualWidth / 2, texto.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Boton_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As FontAwesome5.FontAwesome = boton.Content
            icono.Saturation(1).Scale(1.1, 1.1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Boton_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As FontAwesome5.FontAwesome = boton.Content
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_NVItem_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim item As NavigationViewItem = sender
            Dim icono As FontAwesome5.FontAwesome = item.Icon
            icono.Saturation(1).Scale(1.2, 1.2, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_NVItem_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim item As NavigationViewItem = sender
            Dim icono As FontAwesome5.FontAwesome = item.Icon
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_NVItem_Ellipse(sender As Object, e As PointerRoutedEventArgs)

            Dim item As NavigationViewItem = sender
            Dim sp As StackPanel = item.Content

            Dim icono As Ellipse = sp.Children(0)
            icono.Saturation(1).Scale(1.1, 1.1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_NVItem_Ellipse(sender As Object, e As PointerRoutedEventArgs)

            Dim item As NavigationViewItem = sender
            Dim sp As StackPanel = item.Content

            Dim icono As Ellipse = sp.Children(0)
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Boton_Ellipse(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As Ellipse = boton.Content
            icono.Saturation(1).Scale(1.1, 1.1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Boton_Ellipse(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As Ellipse = boton.Content
            icono.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Boton_Imagen(sender As Object, e As PointerRoutedEventArgs)

            Dim grid As Grid = sender
            Dim imagen As ImageEx = grid.Children(0)
            imagen.Saturation(1).Scale(1.02, 1.02, imagen.ActualWidth / 2, imagen.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Boton_Imagen(sender As Object, e As PointerRoutedEventArgs)

            Dim grid As Grid = sender
            Dim imagen As ImageEx = grid.Children(0)
            imagen.Saturation(1).Scale(1, 1, imagen.ActualWidth / 2, imagen.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Video(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

            Dim grid As Grid = sender
            Dim enlace As String = grid.Tag

            Dim imagen As ImageEx = grid.Children(0)

            Dim añadirReproductor As Boolean = True

            For Each hijo In grid.Children
                If TypeOf hijo Is MediaPlayerElement Then
                    añadirReproductor = False

                    Dim reproductor As MediaPlayerElement = hijo
                    reproductor.MediaPlayer.Play()
                End If
            Next

            If añadirReproductor = True Then
                Dim pr As New ProgressRing With {
                    .IsActive = True,
                    .Width = 20,
                    .Height = 20,
                    .Foreground = New SolidColorBrush(Colors.White),
                    .HorizontalAlignment = HorizontalAlignment.Left,
                    .VerticalAlignment = VerticalAlignment.Top,
                    .Margin = New Thickness(5, 5, 5, 5)
                }

                grid.Children.Add(pr)

                Try
                    Dim reproductor As New MediaPlayerElement With {
                        .Source = MediaSource.CreateFromUri(New Uri(enlace)),
                        .Width = imagen.ActualWidth,
                        .Height = imagen.ActualHeight,
                        .MinWidth = 0
                    }
                    reproductor.MediaPlayer.IsLoopingEnabled = True
                    reproductor.MediaPlayer.Play()

                    grid.Children.Add(reproductor)
                Catch ex As Exception

                End Try
            End If

        End Sub

        Public Sub Sale_Video(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

            Dim grid As Grid = sender

            For Each hijo In grid.Children
                If TypeOf hijo Is MediaPlayerElement Then
                    Dim reproductor As MediaPlayerElement = hijo
                    reproductor.MediaPlayer.Pause()
                End If
            Next

        End Sub

        Public Sub Entra_MFItem_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim item As MenuFlyoutItem = sender
            Dim icono As FontAwesome5.FontAwesome = item.Icon
            icono.Saturation(1).Scale(1.2, 1.2, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_MFItem_Icono(sender As Object, e As PointerRoutedEventArgs)

            Dim item As MenuFlyoutItem = sender
            Dim icono As FontAwesome5.FontAwesome = item.Icon
            icono.Saturation(1).Scale(1, 1, icono.ActualWidth / 2, icono.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Public Sub Entra_Boton_Texto(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim tb As TextBlock = boton.Content
            tb.Saturation(1).Scale(1.1, 1.1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Public Sub Sale_Boton_Texto(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim tb As TextBlock = boton.Content
            tb.Saturation(1).Scale(1, 1, boton.ActualWidth / 2, boton.ActualHeight / 2).Start()

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace