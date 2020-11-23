Imports Windows.Storage

Namespace Configuracion

    Module Inicio

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim toggleUsuario As ToggleSwitch = pagina.FindName("tsInicioUsuario")
            toggleUsuario.OnContent = recursos.GetString("Yes")
            toggleUsuario.OffContent = recursos.GetString("No")

            If ApplicationData.Current.LocalSettings.Values("inicio_usuario") Is Nothing Then
                ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 0
                toggleUsuario.IsOn = False
            Else
                If ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 1 Then
                    toggleUsuario.IsOn = True
                Else
                    toggleUsuario.IsOn = False
                End If
            End If

            RemoveHandler toggleUsuario.Toggled, AddressOf Inicio_Usuario
            AddHandler toggleUsuario.Toggled, AddressOf Inicio_Usuario

            RemoveHandler toggleUsuario.PointerEntered, AddressOf Interfaz.Entra_Basico
            AddHandler toggleUsuario.PointerEntered, AddressOf Interfaz.Entra_Basico

            RemoveHandler toggleUsuario.PointerExited, AddressOf Interfaz.Sale_Basico
            AddHandler toggleUsuario.PointerExited, AddressOf Interfaz.Sale_Basico

        End Sub

        Private Sub Inicio_Usuario(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim toggle As ToggleSwitch = sender

            If toggle.IsOn = True Then
                ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 1
            Else
                ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 0
            End If

        End Sub

    End Module

End Namespace