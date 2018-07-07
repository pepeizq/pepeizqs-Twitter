Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Module UsuarioXaml

    Public Sub GenerarListaUsuarios(listaUsuarios As List(Of TwitterUsuario))

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbUsuarios As New TextBlock With {
            .Text = recursos.GetString("AccountChange"),
            .Foreground = New SolidColorBrush(Colors.White)
        }

        Dim nvItemUsuarios As NavigationViewItem = pagina.FindName("nvItemUsuarios")
        nvItemUsuarios.Content = tbUsuarios

        If listaUsuarios.Count = 1 Then
            nvItemUsuarios.Visibility = Visibility.Collapsed
        ElseIf listaUsuarios.Count > 1 Then
            nvItemUsuarios.Visibility = Visibility.Visible
        End If

    End Sub

    Public Sub GenerarCadaUsuario(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim recursos As New Resources.ResourceLoader

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim menu As MenuFlyout = pagina.FindName("botonUsuariosMenu")
        Dim menuItemUsuario As New MenuFlyoutItem With {
            .Text = usuario.Nombre,
            .Tag = New pepeizq.Twitter.Objetos.UsuarioAmpliado(megaUsuario, usuario, Nothing)
        }

        AddHandler menuItemUsuario.Click, AddressOf BotonCambiarCuentaClick
        AddHandler menuItemUsuario.PointerEntered, AddressOf BotonUsuarioEntra
        AddHandler menuItemUsuario.PointerExited, AddressOf BotonUsuarioSale

        menu.Items.Add(menuItemUsuario)

        Dim gridUsuario As New Grid With {
            .Name = "gridUsuario" + usuario.ScreenNombre,
            .Visibility = visibilidad
        }

        gridUsuario.Children.Add(InicioXaml.Generar(megaUsuario, visibilidad))
        gridUsuario.Children.Add(MencionesXaml.Generar(megaUsuario, Visibility.Collapsed))
        gridUsuario.Children.Add(EscribirXaml.Generar(megaUsuario, Visibility.Collapsed))

        Dim gridBusquedaUsuarios As Grid = pagina.FindName("gridBusquedaUsuarios")
        gridBusquedaUsuarios.Tag = megaUsuario

        Dim gridBusquedaTweets As Grid = pagina.FindName("gridBusquedaTweets")
        gridBusquedaTweets.Tag = megaUsuario

        Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
        gridPrincipal.Children.Insert(0, gridUsuario)

        If visibilidad = Visibility.Visible Then
            Dim spCuentaSeleccionada As StackPanel = pagina.FindName("spCuentaSeleccionada")
            spCuentaSeleccionada.Tag = New pepeizq.Twitter.Objetos.UsuarioAmpliado(megaUsuario, usuario, Nothing)

            Dim elipseCuentaSeleccionada As Ellipse = pagina.FindName("elipseCuentaSeleccionada")

            Dim imagenCuenta As New ImageBrush With {
                .ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))
            }

            elipseCuentaSeleccionada.Fill = imagenCuenta

            Dim tbCuenta As TextBlock = pagina.FindName("tbCuentaSeleccionada")
            tbCuenta.Text = usuario.Nombre

            Dim nvItemUsuarios As NavigationViewItem = pagina.FindName("nvItemUsuarios")
            nvItemUsuarios.Tag = megaUsuario

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + usuario.Nombre
        End If

        TwitterTimeLineInicio.CargarTweets(megaUsuario, Nothing, False)
        TwitterTimeLineMenciones.CargarTweets(megaUsuario, Nothing, False, True)

        TwitterStream.Iniciar(megaUsuario)

    End Sub

    Private Sub BotonUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub BotonUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    Public Sub GridVisibilidad(gridElegido As Grid, usuario As TwitterUsuario)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        'Dim gridTweets As Grid = pagina.FindName("gridTweets" + usuario.ID)
        'gridTweets.Visibility = Visibility.Collapsed

        Dim gridMenciones As Grid = pagina.FindName("gridMenciones" + usuario.ID)
        gridMenciones.Visibility = Visibility.Collapsed

        Dim gridEscribir As Grid = pagina.FindName("gridEscribir" + usuario.ID)
        gridEscribir.Visibility = Visibility.Collapsed

        gridElegido.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonCambiarCuentaClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        CambiarCuenta(cosas.MegaUsuario, cosas.Usuario.ScreenNombre, False)

    End Sub

    Public Async Sub CambiarCuenta(megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioRecibido As String, esperar As Boolean)

        If esperar = True Then
            Await Task.Delay(5000)
        End If

        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of TwitterUsuario)

        If helper.KeyExists("listaUsuarios5") Then
            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios5")
        End If

        Dim usuario As TwitterUsuario = Nothing

        For Each usuario2 As TwitterUsuario In listaUsuarios
            If usuarioRecibido = usuario2.ScreenNombre Then
                usuario = usuario2
            End If
        Next

        If Not usuario Is Nothing Then
            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + usuario.Nombre

            Dim nvItemUsuarios As NavigationViewItem = pagina.FindName("nvItemUsuarios")
            nvItemUsuarios.Tag = usuario

            Dim spCuentaSeleccionada As StackPanel = pagina.FindName("spCuentaSeleccionada")
            spCuentaSeleccionada.Tag = New pepeizq.Twitter.Objetos.UsuarioAmpliado(megaUsuario, usuario, Nothing)

            Dim elipseCuentaSeleccionada As Ellipse = pagina.FindName("elipseCuentaSeleccionada")

            Dim imagenCuenta As New ImageBrush With {
                .ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))
            }

            elipseCuentaSeleccionada.Fill = imagenCuenta

            Dim tbCuenta As TextBlock = pagina.FindName("tbCuentaSeleccionada")
            tbCuenta.Text = usuario.Nombre

            Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")

            For Each grid As Grid In gridPrincipal.Children
                If grid.Name.Contains("gridUsuario") Then
                    If Not grid.Name = "gridUsuarioAmpliado" Then
                        For Each subgrid As Grid In grid.Children
                            subgrid.Visibility = Visibility.Collapsed
                        Next

                        grid.Visibility = Visibility.Collapsed
                    End If
                End If

                If grid.Name = "gridUsuario" + usuario.ID Then
                    For Each subgrid As Grid In grid.Children
                        If subgrid.Name.Contains("gridTweets" + usuario.ID) Then
                            subgrid.Visibility = Visibility.Visible
                        Else
                            subgrid.Visibility = Visibility.Collapsed
                        End If
                    Next

                    grid.Visibility = Visibility.Visible
                End If
            Next
        End If
    End Sub

End Module
