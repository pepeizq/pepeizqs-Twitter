Namespace Interfaz
    Module Pestañas

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")

            RemoveHandler botonUsuarios.Click, AddressOf Visibilidad_Config_Usuarios
            AddHandler botonUsuarios.Click, AddressOf Visibilidad_Config_Usuarios

            RemoveHandler botonUsuarios.PointerEntered, AddressOf Entra_Basico
            AddHandler botonUsuarios.PointerEntered, AddressOf Entra_Basico

            RemoveHandler botonUsuarios.PointerExited, AddressOf Sale_Basico
            AddHandler botonUsuarios.PointerExited, AddressOf Sale_Basico

        End Sub

        Private Sub Visibilidad_Config_Usuarios(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")
            Dim gridUsuarios As Grid = pagina.FindName("gridConfiguracionUsuarios")

            Visibilidad_Pestañas_Config(botonUsuarios, gridUsuarios)

        End Sub

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