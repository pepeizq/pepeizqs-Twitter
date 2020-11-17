Imports Tweetinvi.Models
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module Usuario

        Public Sub CargarDatos(usuario As IAuthenticatedUser)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuario As Grid = pagina.FindName("gridUsuario")
            Pestañas.Visibilidad_Pestañas(gridUsuario)

            Dim nvItemCuenta As NavigationViewItem = pagina.FindName("nvItemCuenta")
            nvItemCuenta.Visibility = Visibility.Visible

            Dim imagenCuenta As New ImageBrush With {
                .ImageSource = New BitmapImage(New Uri(usuario.ProfileImageUrl))
            }

            Dim avatar As Ellipse = pagina.FindName("elipseCuentaSeleccionada")
            avatar.Fill = imagenCuenta

            Dim tbCuenta As TextBlock = pagina.FindName("tbCuentaSeleccionada")
            tbCuenta.Text = usuario.Name


        End Sub

    End Module
End Namespace

