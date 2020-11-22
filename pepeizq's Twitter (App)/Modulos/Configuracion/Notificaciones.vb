Imports Windows.Media.Core
Imports Windows.Media.Playback
Imports Windows.Storage

Namespace Configuracion
    Module Notificaciones

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spTiempo As StackPanel = pagina.FindName("spConfigNotificacionesTiempo")
            Dim toggleNotificaciones As ToggleSwitch = pagina.FindName("tsNotificacionesTiempo")
            toggleNotificaciones.OnContent = recursos.GetString("Yes")
            toggleNotificaciones.OffContent = recursos.GetString("No")

            If ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo") = 1
                toggleNotificaciones.IsOn = True
                spTiempo.Visibility = Visibility.Visible
            Else
                If ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo") = 1 Then
                    toggleNotificaciones.IsOn = True
                    spTiempo.Visibility = Visibility.Visible
                Else
                    toggleNotificaciones.IsOn = False
                    spTiempo.Visibility = Visibility.Collapsed
                End If
            End If

            RemoveHandler toggleNotificaciones.Toggled, AddressOf Notificaciones_Tiempo
            AddHandler toggleNotificaciones.Toggled, AddressOf Notificaciones_Tiempo

            RemoveHandler toggleNotificaciones.PointerEntered, AddressOf Interfaz.Entra_Basico
            AddHandler toggleNotificaciones.PointerEntered, AddressOf Interfaz.Entra_Basico

            RemoveHandler toggleNotificaciones.PointerExited, AddressOf Interfaz.Sale_Basico
            AddHandler toggleNotificaciones.PointerExited, AddressOf Interfaz.Sale_Basico

            Dim sliderNotificaciones As Slider = pagina.FindName("sliderNotificacionesTiempo")
            Dim tbTiempo As TextBlock = pagina.FindName("tbNotificacionesTiempo")

            If ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo_segundos") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo_segundos") = 30
                sliderNotificaciones.Value = 30
                tbTiempo.Text = 30.ToString + " " + recursos.GetString("Seconds")
            Else
                sliderNotificaciones.Value = ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo_segundos")
                tbTiempo.Text = ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo_segundos").ToString + " " + recursos.GetString("Seconds")
            End If

            RemoveHandler sliderNotificaciones.ValueChanged, AddressOf Notificaciones_Tiempo_Segundos
            AddHandler sliderNotificaciones.ValueChanged, AddressOf Notificaciones_Tiempo_Segundos

            RemoveHandler sliderNotificaciones.PointerEntered, AddressOf Interfaz.Entra_Basico
            AddHandler sliderNotificaciones.PointerEntered, AddressOf Interfaz.Entra_Basico

            RemoveHandler sliderNotificaciones.PointerExited, AddressOf Interfaz.Sale_Basico
            AddHandler sliderNotificaciones.PointerExited, AddressOf Interfaz.Sale_Basico

            Dim spSonido As StackPanel = pagina.FindName("spConfigNotificacionesSonido")
            Dim toggleSonido As ToggleSwitch = pagina.FindName("tsNotificacionesSonido")
            toggleSonido.OnContent = recursos.GetString("Yes")
            toggleSonido.OffContent = recursos.GetString("No")

            If ApplicationData.Current.LocalSettings.Values("notificaciones_sonido") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificaciones_sonido") = 0
                toggleSonido.IsOn = False
                spSonido.Visibility = Visibility.Collapsed
            Else
                If ApplicationData.Current.LocalSettings.Values("notificaciones_sonido") = 1 Then
                    toggleSonido.IsOn = True
                    spSonido.Visibility = Visibility.Visible
                Else
                    toggleSonido.IsOn = False
                    spSonido.Visibility = Visibility.Collapsed
                End If
            End If

            RemoveHandler toggleSonido.Toggled, AddressOf Notificaciones_Sonido
            AddHandler toggleSonido.Toggled, AddressOf Notificaciones_Sonido

            RemoveHandler toggleSonido.PointerEntered, AddressOf Interfaz.Entra_Basico
            AddHandler toggleSonido.PointerEntered, AddressOf Interfaz.Entra_Basico

            RemoveHandler toggleSonido.PointerExited, AddressOf Interfaz.Sale_Basico
            AddHandler toggleSonido.PointerExited, AddressOf Interfaz.Sale_Basico

            Dim cbSonidoElegir As ComboBox = pagina.FindName("cbConfigNotificacionesSonidoElegir")

            If ApplicationData.Current.LocalSettings.Values("notificaciones_sonido_elegir") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificaciones_sonido_elegir") = "ms-appx:///Assets/Sonidos/Twitter.Default.mp3"
                cbSonidoElegir.SelectedIndex = 0
            Else
                For Each item As ComboBoxItem In cbSonidoElegir.Items
                    If item.Tag = ApplicationData.Current.LocalSettings.Values("notificaciones_sonido_elegir") Then
                        cbSonidoElegir.SelectedItem = item
                    End If
                Next
            End If

            RemoveHandler cbSonidoElegir.SelectionChanged, AddressOf Notificaciones_Sonido_Elegir
            AddHandler cbSonidoElegir.SelectionChanged, AddressOf Notificaciones_Sonido_Elegir

            RemoveHandler cbSonidoElegir.PointerEntered, AddressOf Interfaz.Entra_Basico
            AddHandler cbSonidoElegir.PointerEntered, AddressOf Interfaz.Entra_Basico

            RemoveHandler cbSonidoElegir.PointerExited, AddressOf Interfaz.Sale_Basico
            AddHandler cbSonidoElegir.PointerExited, AddressOf Interfaz.Sale_Basico

            Dim botonSonido As Button = pagina.FindName("botonConfigNotificacionesSonido")

            RemoveHandler botonSonido.Click, AddressOf Notificaciones_Sonido_Reproducir
            AddHandler botonSonido.Click, AddressOf Notificaciones_Sonido_Reproducir

            RemoveHandler botonSonido.PointerEntered, AddressOf Interfaz.Entra_Boton_IconoTexto
            AddHandler botonSonido.PointerEntered, AddressOf Interfaz.Entra_Boton_IconoTexto

            RemoveHandler botonSonido.PointerExited, AddressOf Interfaz.Sale_Boton_IconoTexto
            AddHandler botonSonido.PointerExited, AddressOf Interfaz.Sale_Boton_IconoTexto

        End Sub

        Private Sub Notificaciones_Tiempo(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spTiempo As StackPanel = pagina.FindName("spConfigNotificacionesTiempo")

            Dim toggle As ToggleSwitch = sender

            If toggle.IsOn = True Then
                ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo") = 1
                spTiempo.Visibility = Visibility.Visible
            Else
                ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo") = 0
                spTiempo.Visibility = Visibility.Collapsed
            End If

        End Sub

        Private Sub Notificaciones_Tiempo_Segundos(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim slider As Slider = sender

            ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo_segundos") = slider.Value

            Dim tbTiempo As TextBlock = pagina.FindName("tbNotificacionesTiempo")
            tbTiempo.Text = ApplicationData.Current.LocalSettings.Values("notificaciones_tiempo_segundos").ToString + " " + recursos.GetString("Seconds")

        End Sub

        Private Sub Notificaciones_Sonido(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spSonido As StackPanel = pagina.FindName("spConfigNotificacionesSonido")

            Dim toggle As ToggleSwitch = sender

            If toggle.IsOn = True Then
                ApplicationData.Current.LocalSettings.Values("notificaciones_sonido") = 1
                spSonido.Visibility = Visibility.Visible
            Else
                ApplicationData.Current.LocalSettings.Values("notificaciones_sonido") = 0
                spSonido.Visibility = Visibility.Collapsed
            End If

        End Sub

        Private Sub Notificaciones_Sonido_Elegir(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim cbSonido As ComboBox = sender
            Dim sonidoElegido As ComboBoxItem = cbSonido.SelectedItem

            ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = sonidoElegido.Tag

        End Sub

        Private Sub Notificaciones_Sonido_Reproducir(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim cbSonido As ComboBox = pagina.FindName("cbConfigNotificacionesSonidoElegir")
            Dim sonidoElegido As ComboBoxItem = cbSonido.SelectedItem

            Dim sonido As New MediaPlayer With {
                .Source = MediaSource.CreateFromUri(New Uri(sonidoElegido.Tag))
            }

            sonido.Play()

        End Sub

    End Module
End Namespace













'Public Sub Iniciar2()

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content


'    Dim botonArranque As Button = pagina.FindName("botonConfigAppAutoArranque")

'    If Not ApplicationData.Current.LocalSettings.Values("autoarranque") Is Nothing Then
'        If ApplicationData.Current.LocalSettings.Values("autoarranque") = False Then
'            botonArranque.IsEnabled = False
'        End If
'    End If

'    If ApplicationData.Current.LocalSettings.Values("tooltipsayuda") Is Nothing Then
'        CargarTooltipsAyuda(True)
'    Else
'        CargarTooltipsAyuda(ApplicationData.Current.LocalSettings.Values("tooltipsayuda"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("media") Is Nothing Then
'        CargarMedia(True)
'    Else
'        CargarMedia(ApplicationData.Current.LocalSettings.Values("media"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("tweetretweets") Is Nothing Then
'        CargarTweetRetweets(True)
'    Else
'        CargarTweetRetweets(ApplicationData.Current.LocalSettings.Values("tweetretweets"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("tweetcard") Is Nothing Then
'        CargarTweetCard(False)
'    Else
'        CargarTweetCard(ApplicationData.Current.LocalSettings.Values("tweetcard"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo") Is Nothing Then
'        NotificacionesInicioTiempo(False)
'    Else
'        NotificacionesInicioTiempo(ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo") Is Nothing Then
'        NotificacionesMencionesTiempo(False)
'    Else
'        NotificacionesMencionesTiempo(ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar") Is Nothing Then
'        NotificacionesInicioAgrupar(True)
'    Else
'        NotificacionesInicioAgrupar(ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar") Is Nothing Then
'        NotificacionesMencionesAgrupar(False)
'    Else
'        NotificacionesMencionesAgrupar(ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionSonido") Is Nothing Then
'        NotificacionesSonido(True)
'    Else
'        NotificacionesSonido(ApplicationData.Current.LocalSettings.Values("notificacionSonido"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionImagen") Is Nothing Then
'        NotificacionesImagen(True)
'    Else
'        NotificacionesImagen(ApplicationData.Current.LocalSettings.Values("notificacionImagen"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("notificacionUsuario") Is Nothing Then
'        NotificacionesUsuario(True)
'    Else
'        NotificacionesUsuario(ApplicationData.Current.LocalSettings.Values("notificacionUsuario"))
'    End If

'    If ApplicationData.Current.LocalSettings.Values("seguirDeals") Is Nothing Then
'        SeguirDeals(True, True)
'    Else
'        SeguirDeals(ApplicationData.Current.LocalSettings.Values("seguirDeals"), True)
'    End If

'End Sub

'Public Sub CargarConsumerKey(clave As String)

'    ApplicationData.Current.LocalSettings.Values("consumerkey") = clave

'End Sub

'Public Sub CargarConsumerSecret(clave As String)

'    ApplicationData.Current.LocalSettings.Values("consumersecret") = clave

'End Sub

'Public Async Sub AutoArranque()

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    Dim botonArranque As Button = pagina.FindName("botonConfigAppAutoArranque")

'    Dim startupTarea As StartupTask = Await StartupTask.GetAsync("arranque")
'    Dim resultado As StartupTaskState = Await startupTarea.RequestEnableAsync()

'    If resultado = StartupTaskState.Enabled Then
'        botonArranque.IsEnabled = False
'        ApplicationData.Current.LocalSettings.Values("autoarranque") = False
'    End If

'End Sub

'Public Sub CargarTooltipsAyuda(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("tooltipsayuda") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigAppTooltipsAyuda")
'    cb.IsChecked = estado

'End Sub

'Public Sub CargarMedia(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("media") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigAppCargarMedia")
'    cb.IsChecked = estado

'    Dim sp As StackPanel = pagina.FindName("spConfigAppCargarMediaVistaPrevia")

'    If estado = True Then
'        sp.Visibility = Visibility.Visible

'        Dim sliderAlto As Slider = pagina.FindName("sliderMediaVistaPreviaAlto")

'        If ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto") Is Nothing Then
'            ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto") = 200
'        End If

'        sliderAlto.Value = Double.Parse(ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto"))

'        Dim sliderAncho As Slider = pagina.FindName("sliderMediaVistaPreviaAncho")

'        If ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho") Is Nothing Then
'            ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho") = 500
'        End If

'        sliderAncho.Value = ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho")
'    Else
'        sp.Visibility = Visibility.Collapsed
'    End If

'End Sub

'Public Sub CargarTweetRetweets(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("tweetretweets") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigAppTweetRetweets")
'    cb.IsChecked = estado

'End Sub

'Public Sub CargarTweetCard(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("tweetcard") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigAppTweetCard")
'    cb.IsChecked = estado

'End Sub

'Public Sub NotificacionesInicioTiempo(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesInicioTiempo")
'    cb.IsChecked = estado

'    Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesInicioTiempo")

'    If estado = True Then
'        sp.Visibility = Visibility.Visible

'        Dim slider As Slider = pagina.FindName("sliderNotificacionesInicioTiempo")

'        If ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos") = Nothing Then
'            ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos") = 30
'        End If

'        slider.Value = ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos")
'    Else
'        sp.Visibility = Visibility.Collapsed
'    End If

'End Sub

'Public Sub NotificacionesMencionesTiempo(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesMencionesTiempo")
'    cb.IsChecked = estado

'    Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesMencionesTiempo")

'    If estado = True Then
'        sp.Visibility = Visibility.Visible

'        Dim slider As Slider = pagina.FindName("sliderNotificacionesMencionesTiempo")

'        If ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos") = Nothing Then
'            ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos") = 30
'        End If

'        slider.Value = ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos")
'    Else
'        sp.Visibility = Visibility.Collapsed
'    End If

'End Sub

'Public Sub NotificacionesInicioAgrupar(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesInicioAgrupar")
'    cb.IsChecked = estado

'End Sub

'Public Sub NotificacionesMencionesAgrupar(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesMencionesAgrupar")
'    cb.IsChecked = estado

'End Sub

'Public Sub NotificacionesSonido(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionSonido") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesSonido")
'    cb.IsChecked = estado

'    Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesSonido")

'    If estado = True Then
'        sp.Visibility = Visibility.Visible

'        Dim cbSonido As ComboBox = pagina.FindName("cbConfigNotificacionesSonidoElegido")

'        If ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = Nothing Then
'            cbSonido.SelectedIndex = 0
'            ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = "ms-appx:///Assets/Sonidos/Twitter.Default.mp3"
'        Else
'            For Each item As ComboBoxItem In cbSonido.Items
'                If item.Tag = ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") Then
'                    cbSonido.SelectedItem = item
'                End If
'            Next
'        End If
'    Else
'        sp.Visibility = Visibility.Collapsed
'    End If

'End Sub

'Public Sub NotificacionesImagen(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionImagen") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesImagen")
'    cb.IsChecked = estado

'End Sub

'Public Async Sub NotificacionesUsuario(estado As Boolean)

'    Dim frame As Frame = Window.Current.Content
'    Dim pagina As Page = frame.Content

'    ApplicationData.Current.LocalSettings.Values("notificacionUsuario") = estado

'    Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesUsuario")
'    cb.IsChecked = estado

'    Dim helper As New LocalObjectStorageHelper

'    Dim listaNotificaciones As New List(Of String)

'    If helper.KeyExists("listaNotificaciones") Then
'        listaNotificaciones = Await helper.ReadFileAsync(Of List(Of String))("listaNotificaciones")
'    End If

'    listaNotificaciones.Clear()
'    Await helper.SaveFileAsync(Of List(Of String))("listaNotificaciones", listaNotificaciones)

'End Sub

'Public Sub SeguirDeals(estado As Boolean, inicio As Boolean)

'    ApplicationData.Current.LocalSettings.Values("seguirDeals") = estado

'    If inicio = True Then
'        Dim frame As Frame = Window.Current.Content
'        Dim pagina As Page = frame.Content

'        Dim cb As CheckBox = pagina.FindName("cbConfigSeguirDeals")
'        cb.IsChecked = estado
'    End If

'End Sub
