Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Module TwitterConexion

    Public Async Function Iniciar(usuarioRecibido As TwitterUsuario) As Task(Of pepeizq.Twitter.MegaUsuario)

        Dim consumerKey As String = "poGVvY5De5zBqQ4ceqp7jw7cj"
        Dim consumerSecret As String = "f8PCcuwFZxYi0r5iG6UaysgxD0NoaCT2RgYG8I41mvjghy58rc"

        Dim estado As Boolean = False
        Dim servicio As New TwitterServicio

        If Not usuarioRecibido Is Nothing Then
            ApplicationData.Current.LocalSettings.Values("TwitterScreenNombre") = usuarioRecibido.ScreenNombre
        Else
            ApplicationData.Current.LocalSettings.Values("TwitterScreenNombre") = Nothing
        End If

        Try
            servicio.Initialize(consumerKey, consumerSecret, "https://pepeizqapps.com/")
            estado = Await servicio.Provider.Logear
        Catch ex As Exception
            estado = False
        End Try

        If estado = True Then
            Dim usuario As TwitterUsuario = Await servicio.Provider.GenerarUsuario

            If Not usuario Is Nothing Then

                Dim megaUsuario As New pepeizq.Twitter.MegaUsuario(usuario, servicio)

                Dim helper As New LocalObjectStorageHelper

                Dim listaUsuarios As New List(Of TwitterUsuario)

                If helper.KeyExists("listaUsuarios2") Then
                    listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
                End If

                Dim añadirLista As Boolean = True

                For Each item In listaUsuarios
                    If item.Id = usuario.Id Then
                        añadirLista = False
                    End If
                Next

                If añadirLista = True Then
                    listaUsuarios.Add(usuario)

                    helper.Save(Of List(Of TwitterUsuario))("listaUsuarios2", listaUsuarios)
                End If

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim lvUsuarios As ListView = pagina.FindName("lvConfigUsuarios")

                lvUsuarios.Items.Add(ConfigAñadirUsuarioXaml(megaUsuario))

                Return megaUsuario
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Public Function Desconectar(servicio As TwitterServicio)

        Try
            servicio.Deslogear()
        Catch ex As Exception

        End Try

        Return servicio.Provider.Logeado

    End Function

    Private Function ConfigAñadirUsuarioXaml(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridUsuario As New Grid With {
            .Padding = New Thickness(5, 5, 5, 5),
            .Tag = megaUsuario
        }

        Dim col1 As New ColumnDefinition
        Dim col2 As New ColumnDefinition
        Dim col3 As New ColumnDefinition

        col1.Width = New GridLength(1, GridUnitType.Auto)
        col2.Width = New GridLength(1, GridUnitType.Star)
        col3.Width = New GridLength(1, GridUnitType.Auto)

        gridUsuario.ColumnDefinitions.Add(col1)
        gridUsuario.ColumnDefinitions.Add(col2)
        gridUsuario.ColumnDefinitions.Add(col3)

        Dim imagenAvatar As New ImageBrush With {
            .Stretch = Stretch.Uniform,
            .ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))
        }

        Dim circulo As New Ellipse With {
            .Fill = imagenAvatar,
            .VerticalAlignment = VerticalAlignment.Center,
            .Margin = New Thickness(0, 0, 10, 0),
            .Height = 40,
            .Width = 40
        }

        circulo.SetValue(Grid.ColumnProperty, 0)
        gridUsuario.Children.Add(circulo)

        Dim tbUsuario As New TextBlock With {
            .Text = usuario.Nombre + " (@" + usuario.ScreenNombre + ")",
            .VerticalAlignment = VerticalAlignment.Center
        }

        tbUsuario.SetValue(Grid.ColumnProperty, 1)
        gridUsuario.Children.Add(tbUsuario)

        Dim simbolo As New FontAwesome.UWP.FontAwesome With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Icon = FontAwesome.UWP.FontAwesomeIcon.Times
        }

        Dim botonQuitar As New Button With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Content = simbolo,
            .Tag = megaUsuario
        }

        AddHandler botonQuitar.Click, AddressOf BotonQuitarCuenta
        AddHandler botonQuitar.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonQuitar.PointerExited, AddressOf UsuarioSaleBoton

        botonQuitar.SetValue(Grid.ColumnProperty, 2)
        gridUsuario.Children.Add(botonQuitar)

        Return gridUsuario

    End Function

    Private Sub BotonQuitarCuenta(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim megaUsuario As pepeizq.Twitter.MegaUsuario = boton.Tag
        Dim servicio As TwitterServicio = megaUsuario.Servicio

        Dim conexion As Boolean = Desconectar(servicio)

        If conexion = False Then
            servicio.PararStreamUsuario()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim lvConfigUsuarios As ListView = pagina.FindName("lvConfigUsuarios")

            Dim i As Integer = 0
            For Each item In lvConfigUsuarios.Items
                Dim grid As Grid = item
                Dim megaUsuarioGrid As pepeizq.Twitter.MegaUsuario = grid.Tag

                If megaUsuarioGrid.Usuario.Id = megaUsuario.Usuario.Id Then
                    lvConfigUsuarios.Items.RemoveAt(i)
                    Exit For
                End If

                i += 1
            Next

            Dim menu As MenuFlyout = pagina.FindName("botonUsuariosMenu")

            i = 0
            For Each item As MenuFlyoutItem In menu.Items
                Dim usuarioItem As TwitterUsuario = item.Tag

                If usuarioItem.Id = megaUsuario.Usuario.Id Then
                    menu.Items.RemoveAt(i)

                    Exit For
                End If

                i += 1
            Next

            If menu.Items.Count > 0 Then
                If i = 0 Then
                    Dim nuevoUsuario As TwitterUsuario = menu.Items(0).Tag
                    UsuarioXaml.CambiarCuenta(nuevoUsuario.ScreenNombre)
                End If
            End If

            Dim gridUsuario As Grid = pagina.FindName("gridUsuario" + megaUsuario.Usuario.ScreenNombre)
            gridUsuario.Children.Clear()

            Dim helper As New LocalObjectStorageHelper

            Dim listaUsuarios As New List(Of TwitterUsuario)

            If helper.KeyExists("listaUsuarios2") Then
                listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
            End If

            i = 0
            For Each item In listaUsuarios
                If item.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
                    listaUsuarios.RemoveAt(i)
                    Exit For
                End If

                i += 1
            Next

            helper.Save(Of List(Of TwitterUsuario))("listaUsuarios2", listaUsuarios)

            Dim nvPrincipal As NavigationView = pagina.FindName("nvPrincipal")
            Dim itemUsuarios As NavigationViewItem = pagina.FindName("itemUsuarios")
            Dim spCuentaSeleccionada As StackPanel = pagina.FindName("spCuentaSeleccionada")

            If lvConfigUsuarios.Items.Count = 0 Then
                For Each item In nvPrincipal.MenuItems
                    If TypeOf item Is NavigationViewItem Then
                        Dim nvItem As NavigationViewItem = item
                        nvItem.Visibility = Visibility.Collapsed
                    End If
                Next

                itemUsuarios.Visibility = Visibility.Collapsed
                spCuentaSeleccionada.Visibility = Visibility.Collapsed
            Else
                For Each item In nvPrincipal.MenuItems
                    If TypeOf item Is NavigationViewItem Then
                        Dim nvItem As NavigationViewItem = item
                        nvItem.Visibility = Visibility.Visible
                    End If
                Next

                If lvConfigUsuarios.Items.Count = 1 Then
                    itemUsuarios.Visibility = Visibility.Collapsed
                Else
                    itemUsuarios.Visibility = Visibility.Visible
                End If

                spCuentaSeleccionada.Visibility = Visibility.Visible
            End If

        End If

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
