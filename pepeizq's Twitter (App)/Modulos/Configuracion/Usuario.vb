Imports Windows.Security.Credentials
Imports Windows.Storage
Imports Windows.UI

Namespace Configuracion
    Module Usuario

        Public Function AñadirListaGuardados(nombre As String)

            Dim recursos As New Resources.ResourceLoader

            Dim grid As New Grid With {
                .Margin = New Thickness(0, 0, 0, 20),
                .Tag = nombre
            }

            Dim col1 As New ColumnDefinition
            Dim col2 As New ColumnDefinition
            Dim col3 As New ColumnDefinition
            Dim col4 As New ColumnDefinition

            col1.Width = New GridLength(1, GridUnitType.Auto)
            col2.Width = New GridLength(1, GridUnitType.Star)
            col3.Width = New GridLength(1, GridUnitType.Auto)
            col4.Width = New GridLength(1, GridUnitType.Auto)

            grid.ColumnDefinitions.Add(col1)
            grid.ColumnDefinitions.Add(col2)
            grid.ColumnDefinitions.Add(col3)
            grid.ColumnDefinitions.Add(col4)

            '-------------------------------------------------------------

            Dim tb As New TextBlock With {
                .Text = nombre,
                .Foreground = New SolidColorBrush(Colors.White),
                .VerticalAlignment = VerticalAlignment.Center
            }

            tb.SetValue(Grid.ColumnProperty, 0)
            grid.Children.Add(tb)

            '-------------------------------------------------------------

            Dim tbConectar As New TextBlock With {
                .Text = recursos.GetString("ConnectTwitter"),
                .Foreground = New SolidColorBrush(Colors.White),
                .Margin = New Thickness(5, 0, 0, 0)
            }

            Dim cb As New CheckBox With {
                .Content = tbConectar,
                .BorderBrush = New SolidColorBrush(Colors.White),
                .RequestedTheme = ElementTheme.Dark,
                .BorderThickness = New Thickness(1, 1, 1, 1),
                .Tag = nombre,
                .VerticalAlignment = VerticalAlignment.Center
            }

            AddHandler cb.Checked, AddressOf Usuario_Conectar
            AddHandler cb.Unchecked, AddressOf Usuario_Desconectar
            AddHandler cb.PointerEntered, AddressOf Interfaz.Entra_Basico
            AddHandler cb.PointerExited, AddressOf Interfaz.Sale_Basico

            If Not ApplicationData.Current.LocalSettings.Values("idUsuario") = Nothing Then
                If nombre = ApplicationData.Current.LocalSettings.Values("idUsuario") Then
                    cb.IsChecked = True
                    Conexion.Conectar(ApplicationData.Current.LocalSettings.Values("idUsuario"), True)
                End If
            End If

            cb.SetValue(Grid.ColumnProperty, 2)
            grid.Children.Add(cb)

            '-------------------------------------------------------------

            Dim iconoBorrar As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Times,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim botonBorrar As New Button With {
                .Content = iconoBorrar,
                .Background = New SolidColorBrush(Colors.Transparent),
                .Padding = New Thickness(12, 10, 12, 10),
                .Style = App.Current.Resources("ButtonRevealStyle"),
                .Margin = New Thickness(30, 0, 20, 0),
                .Tag = nombre,
                .VerticalAlignment = VerticalAlignment.Center
            }

            AddHandler botonBorrar.Click, AddressOf Usuario_Borrar
            AddHandler botonBorrar.PointerEntered, AddressOf Interfaz.Entra_Boton_Icono
            AddHandler botonBorrar.PointerExited, AddressOf Interfaz.Sale_Boton_Icono

            botonBorrar.SetValue(Grid.ColumnProperty, 3)
            grid.Children.Add(botonBorrar)

            Return grid

        End Function

        Private Sub Usuario_Conectar(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim cb As CheckBox = sender
            Dim nombre As String = cb.Tag

            Dim spUsuarios As StackPanel = pagina.FindName("spUsuariosGuardadosLista")

            For Each hijo In spUsuarios.Children
                Dim grid As Grid = hijo
                Dim cbUsuario As CheckBox = grid.Children(1)

                Dim tbNombre As TextBlock = grid.Children(0)

                If Not tbNombre.Text = nombre Then
                    cbUsuario.IsChecked = False
                Else
                    ApplicationData.Current.LocalSettings.Values("idUsuario") = nombre
                    Conexion.Conectar(ApplicationData.Current.LocalSettings.Values("idUsuario"), False)
                End If
            Next

        End Sub

        Private Sub Usuario_Desconectar(ByVal sender As Object, ByVal e As RoutedEventArgs)

        End Sub

        Private Sub Usuario_Borrar(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonBorrar As Button = sender
            Dim nombre As String = botonBorrar.Tag

            Dim spUsuarios As StackPanel = pagina.FindName("spUsuariosGuardadosLista")

            For Each hijo In spUsuarios.Children
                Dim grid As Grid = hijo
                Dim tbNombre As TextBlock = grid.Children(0)

                If tbNombre.Text = nombre Then
                    spUsuarios.Children.Remove(grid)

                    Dim caja As New PasswordVault
                    Dim listaUsuarios As IReadOnlyList(Of PasswordCredential) = caja.FindAllByResource(Package.Current.DisplayName)

                    Dim i As Integer = 0
                    For Each usuario In listaUsuarios
                        If usuario.UserName = nombre Then
                            Exit For
                        End If
                        i += 1
                    Next

                    Dim credencial As PasswordCredential = listaUsuarios(i)
                    credencial.RetrievePassword()
                    caja.Remove(New PasswordCredential(Package.Current.DisplayName, nombre, credencial.Password))
                End If
            Next

        End Sub

        Public Sub Usuario_Texto_Cambia(sender As Object, e As TextChangedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tb As TextBox = sender

            Dim botonAñadirUsuario As Button = pagina.FindName("botonAñadirUsuario")

            If tb.Text.Trim.Length > 0 Then
                Dim pbUsuarioContraseña As PasswordBox = pagina.FindName("pbUsuarioContraseña")

                If pbUsuarioContraseña.Password.Trim.Length > 0 Then
                    botonAñadirUsuario.IsEnabled = True
                Else
                    botonAñadirUsuario.IsEnabled = False
                End If
            Else
                botonAñadirUsuario.IsEnabled = False
            End If

        End Sub

        Public Sub Contraseña_Texto_Cambia(sender As Object, e As PasswordBoxPasswordChangingEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pb As PasswordBox = sender

            Dim botonAñadirUsuario As Button = pagina.FindName("botonAñadirUsuario")

            If pb.Password.Trim.Length > 0 Then
                Dim tbUsuario As TextBox = pagina.FindName("tbUsuario")

                If tbUsuario.Text.Trim.Length > 0 Then
                    botonAñadirUsuario.IsEnabled = True
                Else
                    botonAñadirUsuario.IsEnabled = False
                End If
            Else
                botonAñadirUsuario.IsEnabled = False
            End If

        End Sub

    End Module
End Namespace

