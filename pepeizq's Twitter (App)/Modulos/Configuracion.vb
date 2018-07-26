Imports Windows.Storage

Module Configuracion

    Public Sub Iniciar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        If ApplicationData.Current.LocalSettings.Values("consumerkey") Is Nothing Then
            CargarConsumerKey("poGVvY5De5zBqQ4ceqp7jw7cj")
        Else
            CargarConsumerKey(ApplicationData.Current.LocalSettings.Values("consumerkey"))
        End If

        If ApplicationData.Current.LocalSettings.Values("consumersecret") Is Nothing Then
            CargarConsumerSecret("f8PCcuwFZxYi0r5iG6UaysgxD0NoaCT2RgYG8I41mvjghy58rc")
        Else
            CargarConsumerSecret(ApplicationData.Current.LocalSettings.Values("consumersecret"))
        End If

        Dim botonArranque As Button = pagina.FindName("botonConfigAppAutoArranque")

        If Not ApplicationData.Current.LocalSettings.Values("autoarranque") Is Nothing Then
            If ApplicationData.Current.LocalSettings.Values("autoarranque") = False Then
                botonArranque.IsEnabled = False
            End If
        End If

        If ApplicationData.Current.LocalSettings.Values("tooltipsayuda") Is Nothing Then
            CargarTooltipsAyuda(True)
        Else
            CargarTooltipsAyuda(ApplicationData.Current.LocalSettings.Values("tooltipsayuda"))
        End If

        If ApplicationData.Current.LocalSettings.Values("media") Is Nothing Then
            CargarMedia(True)
        Else
            CargarMedia(ApplicationData.Current.LocalSettings.Values("media"))
        End If

        If ApplicationData.Current.LocalSettings.Values("tweetretweets") Is Nothing Then
            CargarTweetRetweets(True)
        Else
            CargarTweetRetweets(ApplicationData.Current.LocalSettings.Values("tweetretweets"))
        End If

        If ApplicationData.Current.LocalSettings.Values("tweetcard") Is Nothing Then
            CargarTweetCard(False)
        Else
            CargarTweetCard(ApplicationData.Current.LocalSettings.Values("tweetcard"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo") Is Nothing Then
            NotificacionesInicioTiempo(False)
        Else
            NotificacionesInicioTiempo(ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo") Is Nothing Then
            NotificacionesMencionesTiempo(False)
        Else
            NotificacionesMencionesTiempo(ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar") Is Nothing Then
            NotificacionesInicioAgrupar(True)
        Else
            NotificacionesInicioAgrupar(ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar") Is Nothing Then
            NotificacionesMencionesAgrupar(False)
        Else
            NotificacionesMencionesAgrupar(ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionSonido") Is Nothing Then
            NotificacionesSonido(True)
        Else
            NotificacionesSonido(ApplicationData.Current.LocalSettings.Values("notificacionSonido"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionImagen") Is Nothing Then
            NotificacionesImagen(True)
        Else
            NotificacionesImagen(ApplicationData.Current.LocalSettings.Values("notificacionImagen"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionUsuario") Is Nothing Then
            NotificacionesUsuario(True)
        Else
            NotificacionesUsuario(ApplicationData.Current.LocalSettings.Values("notificacionUsuario"))
        End If

    End Sub

    Public Sub CargarConsumerKey(clave As String)

        ApplicationData.Current.LocalSettings.Values("consumerkey") = clave

    End Sub

    Public Sub CargarConsumerSecret(clave As String)

        ApplicationData.Current.LocalSettings.Values("consumersecret") = clave

    End Sub

    Public Async Sub AutoArranque()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonArranque As Button = pagina.FindName("botonConfigAppAutoArranque")

        Dim startupTarea As StartupTask = Await StartupTask.GetAsync("arranque")
        Dim resultado As StartupTaskState = Await startupTarea.RequestEnableAsync()

        If resultado = StartupTaskState.Enabled Then
            botonArranque.IsEnabled = False
            ApplicationData.Current.LocalSettings.Values("autoarranque") = False
        End If

    End Sub

    Public Sub CargarTooltipsAyuda(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("tooltipsayuda") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigAppTooltipsAyuda")
        cb.IsChecked = estado

    End Sub

    Public Sub CargarMedia(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("media") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigAppCargarMedia")
        cb.IsChecked = estado

        Dim sp As StackPanel = pagina.FindName("spConfigAppCargarMediaVistaPrevia")

        If estado = True Then
            sp.Visibility = Visibility.Visible

            Dim sliderAlto As Slider = pagina.FindName("sliderMediaVistaPreviaAlto")

            If ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto") = 200
            End If

            sliderAlto.Value = Double.Parse(ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto"))

            Dim sliderAncho As Slider = pagina.FindName("sliderMediaVistaPreviaAncho")

            If ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho") = 500
            End If

            sliderAncho.Value = ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho")
        Else
            sp.Visibility = Visibility.Collapsed
        End If

    End Sub

    Public Sub CargarTweetRetweets(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("tweetretweets") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigAppTweetRetweets")
        cb.IsChecked = estado

    End Sub

    Public Sub CargarTweetCard(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("tweetcard") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigAppTweetCard")
        cb.IsChecked = estado

    End Sub

    Public Sub NotificacionesInicioTiempo(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesInicioTiempo")
        cb.IsChecked = estado

        Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesInicioTiempo")

        If estado = True Then
            sp.Visibility = Visibility.Visible

            Dim slider As Slider = pagina.FindName("sliderNotificacionesInicioTiempo")

            If ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos") = Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos") = 30
            End If

            slider.Value = ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos")
        Else
            sp.Visibility = Visibility.Collapsed
        End If

    End Sub

    Public Sub NotificacionesMencionesTiempo(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesMencionesTiempo")
        cb.IsChecked = estado

        Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesMencionesTiempo")

        If estado = True Then
            sp.Visibility = Visibility.Visible

            Dim slider As Slider = pagina.FindName("sliderNotificacionesMencionesTiempo")

            If ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos") = Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos") = 30
            End If

            slider.Value = ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos")
        Else
            sp.Visibility = Visibility.Collapsed
        End If

    End Sub

    Public Sub NotificacionesInicioAgrupar(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesInicioAgrupar")
        cb.IsChecked = estado

    End Sub

    Public Sub NotificacionesMencionesAgrupar(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesMencionesAgrupar")
        cb.IsChecked = estado

    End Sub

    Public Sub NotificacionesSonido(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionSonido") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesSonido")
        cb.IsChecked = estado

        Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesSonido")

        If estado = True Then
            sp.Visibility = Visibility.Visible

            Dim cbSonido As ComboBox = pagina.FindName("cbConfigNotificacionesSonidoElegido")

            If ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = Nothing Then
                cbSonido.SelectedIndex = 0
                ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = "ms-appx:///Assets/Sonidos/Twitter.Default.mp3"
            Else
                For Each item As ComboBoxItem In cbSonido.Items
                    If item.Tag = ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") Then
                        cbSonido.SelectedItem = item
                    End If
                Next
            End If
        Else
            sp.Visibility = Visibility.Collapsed
        End If

    End Sub

    Public Sub NotificacionesImagen(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionImagen") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesImagen")
        cb.IsChecked = estado

    End Sub

    Public Sub NotificacionesUsuario(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionUsuario") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesUsuario")
        cb.IsChecked = estado

    End Sub

End Module
