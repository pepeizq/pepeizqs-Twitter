Namespace Interfaz
    Module Pestañas

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            '------------------------------------------------------

            Dim botonTweets As Button = pagina.FindName("botonUsuarioTweets")

            RemoveHandler botonTweets.Click, AddressOf Visibilidad_Usuario_Tweets
            AddHandler botonTweets.Click, AddressOf Visibilidad_Usuario_Tweets

            RemoveHandler botonTweets.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonTweets.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonTweets.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonTweets.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonEscribir As Button = pagina.FindName("botonEscribir")

            RemoveHandler botonEscribir.Click, AddressOf Visibilidad_Usuario_Escribir
            AddHandler botonEscribir.Click, AddressOf Visibilidad_Usuario_Escribir

            RemoveHandler botonEscribir.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonEscribir.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonEscribir.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonEscribir.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonBusqueda As Button = pagina.FindName("botonBusqueda")

            RemoveHandler botonBusqueda.Click, AddressOf Visibilidad_Usuario_Busqueda
            AddHandler botonBusqueda.Click, AddressOf Visibilidad_Usuario_Busqueda

            RemoveHandler botonBusqueda.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonBusqueda.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonBusqueda.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonBusqueda.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonOtroUsuarioTweets As Button = pagina.FindName("botonOtroUsuarioTweets")

            RemoveHandler botonOtroUsuarioTweets.Click, AddressOf Visibilidad_OtroUsuario_Tweets
            AddHandler botonOtroUsuarioTweets.Click, AddressOf Visibilidad_OtroUsuario_Tweets

            RemoveHandler botonOtroUsuarioTweets.PointerEntered, AddressOf Entra_Boton_Ellipse
            AddHandler botonOtroUsuarioTweets.PointerEntered, AddressOf Entra_Boton_Ellipse

            RemoveHandler botonOtroUsuarioTweets.PointerExited, AddressOf Sale_Boton_Ellipse
            AddHandler botonOtroUsuarioTweets.PointerExited, AddressOf Sale_Boton_Ellipse

            Dim botonImagen As Button = pagina.FindName("botonUsuarioImagen")

            RemoveHandler botonImagen.Click, AddressOf Visibilidad_Usuario_Imagen
            AddHandler botonImagen.Click, AddressOf Visibilidad_Usuario_Imagen

            RemoveHandler botonImagen.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonImagen.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonImagen.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonImagen.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonVideo As Button = pagina.FindName("botonUsuarioVideo")

            RemoveHandler botonVideo.Click, AddressOf Visibilidad_Usuario_Video
            AddHandler botonVideo.Click, AddressOf Visibilidad_Usuario_Video

            RemoveHandler botonVideo.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonVideo.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonVideo.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonVideo.PointerExited, AddressOf Sale_Boton_Icono

            '------------------------------------------------------

            Dim botonUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")

            RemoveHandler botonUsuarios.Click, AddressOf Visibilidad_Config_Usuarios
            AddHandler botonUsuarios.Click, AddressOf Visibilidad_Config_Usuarios

            RemoveHandler botonUsuarios.PointerEntered, AddressOf Entra_Basico
            AddHandler botonUsuarios.PointerEntered, AddressOf Entra_Basico

            RemoveHandler botonUsuarios.PointerExited, AddressOf Sale_Basico
            AddHandler botonUsuarios.PointerExited, AddressOf Sale_Basico

            Dim botonNotificaciones As Button = pagina.FindName("botonConfiguracionNotificaciones")

            RemoveHandler botonNotificaciones.Click, AddressOf Visibilidad_Config_Notificaciones
            AddHandler botonNotificaciones.Click, AddressOf Visibilidad_Config_Notificaciones

            RemoveHandler botonNotificaciones.PointerEntered, AddressOf Entra_Basico
            AddHandler botonNotificaciones.PointerEntered, AddressOf Entra_Basico

            RemoveHandler botonNotificaciones.PointerExited, AddressOf Sale_Basico
            AddHandler botonNotificaciones.PointerExited, AddressOf Sale_Basico

        End Sub

        '------------------------------------------------------

        Private Sub Visibilidad_Usuario_Tweets(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonUsuarioTweets")
            Dim grid As Grid = pagina.FindName("gridUsuarioTweets")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        Private Sub Visibilidad_Usuario_Escribir(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonEscribir")
            Dim grid As Grid = pagina.FindName("gridEscribir")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        Private Sub Visibilidad_Usuario_Busqueda(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonBusqueda")
            Dim grid As Grid = pagina.FindName("gridBusqueda")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        Private Sub Visibilidad_OtroUsuario_Tweets(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonOtroUsuarioTweets")
            Dim grid As Grid = pagina.FindName("gridOtroUsuarioTweets")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        Private Sub Visibilidad_Usuario_Imagen(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonUsuarioImagen")
            Dim grid As Grid = pagina.FindName("gridUsuarioImagen")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        Private Sub Visibilidad_Usuario_Video(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonUsuarioVideo")
            Dim grid As Grid = pagina.FindName("gridUsuarioVideo")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        '------------------------------------------------------

        Private Sub Visibilidad_Config_Usuarios(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonConfiguracionUsuarios")
            Dim grid As Grid = pagina.FindName("gridConfiguracionUsuarios")

            Visibilidad_Pestañas_Config(boton, grid)

        End Sub

        Private Sub Visibilidad_Config_Notificaciones(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonConfiguracionNotificaciones")
            Dim grid As Grid = pagina.FindName("gridConfiguracionNotificaciones")

            Visibilidad_Pestañas_Config(boton, grid)

        End Sub

        '------------------------------------------------------

        Public Sub Visibilidad_Pestañas(gridMostrar As Grid)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuario As Grid = pagina.FindName("gridUsuario")
            gridUsuario.Visibility = Visibility.Collapsed

            Dim gridCarga As Grid = pagina.FindName("gridCarga")
            gridCarga.Visibility = Visibility.Collapsed

            Dim gridConfig As Grid = pagina.FindName("gridConfig")
            gridConfig.Visibility = Visibility.Collapsed

            gridMostrar.Visibility = Visibility.Visible

        End Sub

        Public Sub Visibilidad_Pestañas_Usuario(botonMostrar As Button, gridMostrar As Grid)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonTweets As Button = pagina.FindName("botonUsuarioTweets")
            botonTweets.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridTweets As Grid = pagina.FindName("gridUsuarioTweets")
            gridTweets.Visibility = Visibility.Collapsed

            Dim botonEscribir As Button = pagina.FindName("botonEscribir")
            botonEscribir.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridEscribir As Grid = pagina.FindName("gridEscribir")
            gridEscribir.Visibility = Visibility.Collapsed

            Dim botonBusqueda As Button = pagina.FindName("botonBusqueda")
            botonBusqueda.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda")
            gridBusqueda.Visibility = Visibility.Collapsed

            Dim botonOtroUsuarioTweets As Button = pagina.FindName("botonOtroUsuarioTweets")
            botonOtroUsuarioTweets.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridOtroUsuarioTweets As Grid = pagina.FindName("gridOtroUsuarioTweets")
            gridOtroUsuarioTweets.Visibility = Visibility.Collapsed

            Dim botonImagen As Button = pagina.FindName("botonUsuarioImagen")
            botonImagen.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridImagen As Grid = pagina.FindName("gridUsuarioImagen")
            gridImagen.Visibility = Visibility.Collapsed

            Dim spImagenBotones As StackPanel = pagina.FindName("spUsuarioImagenBotones")
            spImagenBotones.Visibility = Visibility.Collapsed

            Dim botonVideo As Button = pagina.FindName("botonUsuarioVideo")
            botonVideo.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridVideo As Grid = pagina.FindName("gridUsuarioVideo")
            gridVideo.Visibility = Visibility.Collapsed

            Dim spVideoBotones As StackPanel = pagina.FindName("spUsuarioVideoBotones")
            spVideoBotones.Visibility = Visibility.Collapsed

            '--------------------------------------------------------

            botonMostrar.BorderThickness = New Thickness(0, 0, 0, 1)
            gridMostrar.Visibility = Visibility.Visible

            '--------------------------------------------------------

            If gridMostrar.Name = "gridUsuarioImagen" Then
                spImagenBotones.Visibility = Visibility.Visible
            ElseIf gridMostrar.Name = "gridUsuarioVideo" Then
                spVideoBotones.Visibility = Visibility.Visible
            End If

            Dim videoReproductor As MediaPlayerElement = pagina.FindName("videoUsuario")

            If gridMostrar.Name = "gridUsuarioVideo" Then
                If Not videoReproductor.MediaPlayer Is Nothing Then
                    videoReproductor.MediaPlayer.Play()
                End If
            Else
                If Not videoReproductor.MediaPlayer Is Nothing Then
                    videoReproductor.MediaPlayer.Pause()
                End If
            End If

        End Sub

        Public Sub Visibilidad_Pestañas_Config(botonMostrar As Button, gridMostrar As Grid)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")
            botonUsuarios.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridUsuarios As Grid = pagina.FindName("gridConfiguracionUsuarios")
            gridUsuarios.Visibility = Visibility.Collapsed

            Dim botonNotificaciones As Button = pagina.FindName("botonConfiguracionNotificaciones")
            botonNotificaciones.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridNotificaciones As Grid = pagina.FindName("gridConfiguracionNotificaciones")
            gridNotificaciones.Visibility = Visibility.Collapsed

            '--------------------------------------------------------

            botonMostrar.BorderThickness = New Thickness(0, 0, 0, 1)
            gridMostrar.Visibility = Visibility.Visible

        End Sub

    End Module
End Namespace