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

            Dim botonImagen As Button = pagina.FindName("botonUsuarioImagen")

            RemoveHandler botonImagen.Click, AddressOf Visibilidad_Usuario_Imagen
            AddHandler botonImagen.Click, AddressOf Visibilidad_Usuario_Imagen

            RemoveHandler botonImagen.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonImagen.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonImagen.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonImagen.PointerExited, AddressOf Sale_Boton_Icono

            '------------------------------------------------------

            Dim botonUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")

            RemoveHandler botonUsuarios.Click, AddressOf Visibilidad_Config_Usuarios
            AddHandler botonUsuarios.Click, AddressOf Visibilidad_Config_Usuarios

            RemoveHandler botonUsuarios.PointerEntered, AddressOf Entra_Basico
            AddHandler botonUsuarios.PointerEntered, AddressOf Entra_Basico

            RemoveHandler botonUsuarios.PointerExited, AddressOf Sale_Basico
            AddHandler botonUsuarios.PointerExited, AddressOf Sale_Basico

        End Sub

        '------------------------------------------------------

        Private Sub Visibilidad_Usuario_Tweets(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonUsuarioTweets")
            Dim grid As Grid = pagina.FindName("gridUsuarioTweets")

            Visibilidad_Pestañas_Usuario(boton, grid)

        End Sub

        Private Sub Visibilidad_Usuario_Imagen(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonUsuarioImagen")
            Dim grid As Grid = pagina.FindName("gridUsuarioImagen")

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

            Dim botonImagen As Button = pagina.FindName("botonUsuarioImagen")
            botonImagen.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridImagen As Grid = pagina.FindName("gridUsuarioImagen")
            gridImagen.Visibility = Visibility.Collapsed


            botonMostrar.BorderThickness = New Thickness(0, 0, 0, 1)
            gridMostrar.Visibility = Visibility.Visible

        End Sub

        Public Sub Visibilidad_Pestañas_Config(botonMostrar As Button, gridMostrar As Grid)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")
            botonUsuarios.BorderThickness = New Thickness(0, 0, 0, 0)

            Dim gridUsuarios As Grid = pagina.FindName("gridConfiguracionUsuarios")
            gridUsuarios.Visibility = Visibility.Collapsed


            botonMostrar.BorderThickness = New Thickness(0, 0, 0, 1)
            gridMostrar.Visibility = Visibility.Visible

        End Sub

    End Module
End Namespace