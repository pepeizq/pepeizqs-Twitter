Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.System.Threading
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        Dim nvItemCuenta As New NavigationViewItem With {
            .Margin = New Thickness(-8, 0, 0, 0),
            .BorderThickness = New Thickness(0, 0, 0, 0),
            .Visibility = Visibility.Collapsed,
            .Name = "nvItemCuenta"
        }

        Dim sp As New StackPanel With {
            .Name = "spCuentaSeleccionada",
            .Orientation = Orientation.Horizontal,
            .Padding = New Thickness(2, 0, 2, 0)
        }

        Dim elipseCuentaSeleccionada As New Ellipse With {
            .Name = "elipseCuentaSeleccionada",
            .Width = 28,
            .Height = 28,
            .Margin = New Thickness(15, 0, 10, 0),
            .VerticalAlignment = VerticalAlignment.Center
        }

        sp.Children.Add(elipseCuentaSeleccionada)

        Dim tbCuentaSeleccionada As New TextBlock With {
            .Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
            .VerticalAlignment = VerticalAlignment.Center,
            .Name = "tbCuentaSeleccionada"
        }

        sp.Children.Add(tbCuentaSeleccionada)

        AddHandler nvItemCuenta.PointerEntered, AddressOf Interfaz.Entra_NVItem_Ellipse
        AddHandler nvItemCuenta.PointerExited, AddressOf Interfaz.Sale_NVItem_Ellipse

        nvItemCuenta.Content = sp

        nvPrincipal.MenuItems.Add(nvItemCuenta)

        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Config"), FontAwesome5.EFontAwesomeIcon.Solid_Cog))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        If TypeOf args.InvokedItem Is StackPanel Then
            Dim sp As StackPanel = args.InvokedItem

            If sp.Name = "spCuentaSeleccionada" Then
                Interfaz.Pestañas.Visibilidad(gridUsuario, sender)
            End If
        End If

        If TypeOf args.InvokedItem Is TextBlock Then
            Dim item As TextBlock = args.InvokedItem

            If item.Text = recursos.GetString("Config") Then
                Interfaz.Pestañas.Visibilidad(gridConfig, sender)
                Interfaz.Pestañas.Visibilidad_Pestañas_Config(botonConfiguracionUsuarios, gridConfiguracionUsuarios)
            End If
        End If

    End Sub

    Private Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

        Trial.Cargar()
        Configuracion.Notificaciones.Cargar()
        Configuracion.Inicio.Cargar()
        Conexion.Cargar()
        Interfaz.Pestañas.Cargar()
        Interfaz.Enviar.Cargar()
        MasCosas.Cargar()

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        If config.Values("Calificar_App") = 0 Then
            Dim periodoCalificar As TimeSpan = TimeSpan.FromSeconds(300)
            Dim contadorCalificar As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                               Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                                                                                                MasCosas.CalificarApp(True)
                                                                                                                                                                                            End Sub)
                                                                                           End Sub, periodoCalificar)
        End If

    End Sub

End Class

