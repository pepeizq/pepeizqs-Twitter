Imports Windows.Storage

Module Configuracion

    Public Sub Iniciar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonArranque As Button = pagina.FindName("botonConfigAppAutoArranque")

        If Not ApplicationData.Current.LocalSettings.Values("autoarranque") Is Nothing Then
            If ApplicationData.Current.LocalSettings.Values("autoarranque") = False Then
                botonArranque.IsEnabled = False
            End If
        End If

        If ApplicationData.Current.LocalSettings.Values("media") Is Nothing Then
            CargarMedia(True)
        Else
            CargarMedia(ApplicationData.Current.LocalSettings.Values("media"))
        End If

        If ApplicationData.Current.LocalSettings.Values("tweetcard") Is Nothing Then
            CargarTweetCard(False)
        Else
            CargarTweetCard(ApplicationData.Current.LocalSettings.Values("tweetcard"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacion") Is Nothing Then
            NotificacionesEnseñar(True)
        Else
            NotificacionesEnseñar(ApplicationData.Current.LocalSettings.Values("notificacion"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionTiempo") Is Nothing Then
            NotificacionesTiempo(False)
        Else
            NotificacionesTiempo(ApplicationData.Current.LocalSettings.Values("notificacionTiempo"))
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

    Public Sub CargarMedia(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("media") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigAppCargarMedia")
        cb.IsChecked = estado

    End Sub

    Public Sub CargarTweetCard(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("tweetcard") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigAppTweetCard")
        cb.IsChecked = estado

    End Sub

    Public Sub NotificacionesEnseñar(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacion") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificaciones")
        cb.IsChecked = estado

        Dim sp As StackPanel = pagina.FindName("spConfigSubNotificaciones")

        If estado = True Then
            sp.Visibility = Visibility.Visible
        Else
            sp.Visibility = Visibility.Collapsed
        End If

    End Sub

    Public Sub NotificacionesTiempo(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionTiempo") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesTiempo")
        cb.IsChecked = estado

        Dim sp As StackPanel = pagina.FindName("spConfigNotificacionesTiempo")

        If estado = True Then
            sp.Visibility = Visibility.Visible

            Dim tb As TextBox = pagina.FindName("tbConfigNotificacionesSegundos")

            If ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos") = Nothing Then
                ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos") = 30
            End If

            tb.Text = ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos")
        Else
            sp.Visibility = Visibility.Collapsed
        End If

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
