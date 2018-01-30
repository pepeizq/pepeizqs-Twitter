Imports Windows.Storage

Module Configuracion

    Public Sub Iniciar()

        If ApplicationData.Current.LocalSettings.Values("notificacion") Is Nothing Then
            ConfigNotificacion(True)
        Else
            ConfigNotificacion(ApplicationData.Current.LocalSettings.Values("notificacion"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionTiempo") Is Nothing Then
            ConfigNotificacionTiempo(False)
        Else
            ConfigNotificacionTiempo(ApplicationData.Current.LocalSettings.Values("notificacionTiempo"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionSonido") Is Nothing Then
            ConfigNotificacionSonido(True)
        Else
            ConfigNotificacionSonido(ApplicationData.Current.LocalSettings.Values("notificacionSonido"))
        End If

        If ApplicationData.Current.LocalSettings.Values("notificacionImagen") Is Nothing Then
            ConfigNotificacionImagen(True)
        Else
            ConfigNotificacionImagen(ApplicationData.Current.LocalSettings.Values("notificacionImagen"))
        End If

    End Sub

    Public Sub ConfigNotificacion(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacion") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificaciones")
        cb.IsChecked = estado

        Dim grid As Grid = pagina.FindName("gridConfigNotificaciones")
        Dim sp As StackPanel = pagina.FindName("spConfigNotificaciones")

        If estado = True Then
            sp.Visibility = Visibility.Visible
        Else
            sp.Visibility = Visibility.Collapsed
        End If

    End Sub

    Public Sub ConfigNotificacionTiempo(estado As Boolean)

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

    Public Sub ConfigNotificacionSonido(estado As Boolean)

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

    Public Sub ConfigNotificacionImagen(estado As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        ApplicationData.Current.LocalSettings.Values("notificacionImagen") = estado

        Dim cb As CheckBox = pagina.FindName("cbConfigNotificacionesImagen")
        cb.IsChecked = estado

    End Sub

End Module
