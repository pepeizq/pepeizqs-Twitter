Imports Windows.Services.Store
Imports Windows.Storage
Imports Windows.System

Module Trial

    Public Async Sub Cargar()

        Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

        Dim usuarios As IReadOnlyList(Of User) = Await User.FindAllAsync

        If Not usuarios Is Nothing Then
            If usuarios.Count > 0 Then
                Dim usuario As User = usuarios(0)

                Dim contexto As StoreContext = StoreContext.GetForUser(usuario)
                Dim licencia As StoreAppLicense = Await contexto.GetAppLicenseAsync

                If licencia.IsActive = True And licencia.IsTrial = False Then
                    config.Values("Estado_App") = 1
                Else
                    config.Values("Estado_App") = 1 '0
                End If
            End If
        End If

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridMensajeNotificaciones As Grid = pagina.FindName("gridMensajeTrialNotificaciones")

        Dim toggleNotificaciones As ToggleSwitch = pagina.FindName("tsNotificacionesTiempo")
        Dim sliderNotificaciones As Slider = pagina.FindName("sliderNotificacionesTiempo")
        Dim toggleNotificaciones2 As ToggleSwitch = pagina.FindName("tsNotificacionesSonido")
        Dim cbNotificaciones As ComboBox = pagina.FindName("cbConfigNotificacionesSonidoElegir")
        Dim botonNotificaciones As Button = pagina.FindName("botonConfigNotificacionesSonido")

        If config.Values("Estado_App") = 1 Then
            gridMensajeNotificaciones.Visibility = Visibility.Collapsed

            toggleNotificaciones.IsEnabled = True
            sliderNotificaciones.IsEnabled = True
            toggleNotificaciones2.IsEnabled = True
            cbNotificaciones.IsEnabled = True
            botonNotificaciones.IsEnabled = True
        Else
            gridMensajeNotificaciones.Visibility = Visibility.Visible

            toggleNotificaciones.IsEnabled = False
            sliderNotificaciones.IsEnabled = False
            toggleNotificaciones2.IsEnabled = False
            cbNotificaciones.IsEnabled = False
            botonNotificaciones.IsEnabled = False

            Dim botonComprarApp As Button = pagina.FindName("botonComprarApp")

            RemoveHandler botonComprarApp.Click, AddressOf ComprarAppClick
            AddHandler botonComprarApp.Click, AddressOf ComprarAppClick

            RemoveHandler botonComprarApp.PointerEntered, AddressOf Interfaz.Entra_Boton_Texto
            AddHandler botonComprarApp.PointerEntered, AddressOf Interfaz.Entra_Boton_Texto

            RemoveHandler botonComprarApp.PointerExited, AddressOf Interfaz.Sale_Boton_Texto
            AddHandler botonComprarApp.PointerExited, AddressOf Interfaz.Sale_Boton_Texto
        End If

    End Sub

    Private Async Sub ComprarAppClick(sender As Object, e As RoutedEventArgs)

        Dim usuarios As IReadOnlyList(Of User) = Await User.FindAllAsync

        If Not usuarios Is Nothing Then
            If usuarios.Count > 0 Then
                Dim usuario As User = usuarios(0)

                Dim contexto As StoreContext = StoreContext.GetForUser(usuario)
                Await contexto.RequestPurchaseAsync("9NXXXKFMR46S")
            End If
        End If

    End Sub

End Module
