Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports Windows.Storage
Imports Windows.UI
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

                Dim botonVolver As Button = pagina.FindName("botonConfigVolver")

                botonVolver.Visibility = Visibility.Visible

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

        Dim simbolo As New SymbolIcon With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Symbol = Symbol.Clear
        }

        Dim botonQuitar As New Button With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Content = simbolo,
            .Tag = megaUsuario
        }

        AddHandler botonQuitar.Click, AddressOf BotonQuitarCuenta

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
                End If

                i += 1
            Next

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

            If listaUsuarios.Count > 0 Then
                Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
                For Each grid As Grid In gridPrincipal.Children
                    If grid.Name.Contains("gridUsuario") Then
                        If Not grid.Name = "gridUsuarioAmpliado" Then
                            If Not grid.Name = "gridUsuario" + megaUsuario.Usuario.ScreenNombre Then
                                Dim subGrid As Grid = grid.Children(0)
                                Dim subGrid_ As Grid = subGrid.Children(0)
                                Dim spBotonesSuperior As StackPanel = subGrid_.Children(0)
                                Dim menu As Menu = spBotonesSuperior.Children(0)
                                Dim menuItem As MenuItem = menu.Items(0)
                                menuItem.Items.RemoveAt(0)

                                Dim menuItemCuentas As New MenuFlyoutSubItem With {
                                    .Text = recursos.GetString("Accounts")
                                }

                                Dim listaUsuarios2 As New List(Of TwitterUsuario)

                                If helper.KeyExists("listaUsuarios2") Then
                                    listaUsuarios2 = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
                                End If

                                For Each item In listaUsuarios2
                                    Dim subCuenta As New MenuFlyoutItem With {
                                        .Text = item.Nombre + " (@" + item.ScreenNombre + ")",
                                        .Tag = item
                                    }

                                    AddHandler subCuenta.Click, AddressOf BotonCambiarCuentaClick
                                    menuItemCuentas.Items.Add(subCuenta)
                                Next

                                menuItem.Items.Insert(0, menuItemCuentas)
                            End If
                        End If
                    End If
                Next
            End If

            'Dim botonAñadirCuenta As Button = pagina.FindName("botonAñadirCuenta")

            If lvConfigUsuarios.Items.Count = 0 Then
                Dim botonVolver As Button = pagina.FindName("botonConfigVolver")
                botonVolver.Visibility = Visibility.Collapsed

                'botonAñadirCuenta.Visibility = Visibility.Visible
            Else
                Dim botonInicio As Button = pagina.FindName("botonInicio" + listaUsuarios(0).ScreenNombre)
                BotonClick(botonInicio, New RoutedEventArgs)
            End If

        End If

    End Sub

End Module
