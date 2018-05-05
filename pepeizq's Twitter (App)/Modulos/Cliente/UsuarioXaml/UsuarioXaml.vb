Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports Windows.UI
Imports Windows.UI.Core

Module UsuarioXaml

    Public Sub GenerarListaUsuarios(listaUsuarios As List(Of pepeizq.Twitter.MegaUsuario))

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbUsuarios As New TextBlock With {
            .Text = recursos.GetString("AccountChange"),
            .Foreground = New SolidColorBrush(Colors.White)
        }

        Dim itemUsuarios As NavigationViewItem = pagina.FindName("itemUsuarios")
        itemUsuarios.Content = tbUsuarios

        If listaUsuarios.Count = 1 Then
            itemUsuarios.Visibility = Visibility.Collapsed
        ElseIf listaUsuarios.Count > 1 Then
            itemUsuarios.Visibility = Visibility.Visible
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
            .Tag = usuario
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
        gridUsuario.Children.Add(BusquedaXaml.Generar(megaUsuario, Visibility.Collapsed))

        Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
        gridPrincipal.Children.Insert(0, gridUsuario)

        If visibilidad = Visibility.Visible Then
            Dim imagenCuenta As ImageBrush = pagina.FindName("imagenCuentaSeleccionada")
            imagenCuenta.ImageSource = New BitmapImage(New Uri(megaUsuario.Usuario.ImagenAvatar))

            Dim tbCuenta As TextBlock = pagina.FindName("tbCuentaSeleccionada")
            tbCuenta.Text = megaUsuario.Usuario.Nombre

            Dim itemUsuarios As NavigationViewItem = pagina.FindName("itemUsuarios")
            itemUsuarios.Tag = megaUsuario.Usuario

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + megaUsuario.Usuario.Nombre
        End If

        TwitterTimeLineInicio.CargarTweets(megaUsuario, Nothing, False)
        TwitterTimeLineMenciones.CargarTweets(megaUsuario, Nothing, False)

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

        Dim gridTweets As Grid = pagina.FindName("gridTweets" + usuario.ScreenNombre)
        gridTweets.Visibility = Visibility.Collapsed

        Dim gridMenciones As Grid = pagina.FindName("gridMenciones" + usuario.ScreenNombre)
        gridMenciones.Visibility = Visibility.Collapsed

        Dim gridEscribir As Grid = pagina.FindName("gridEscribir" + usuario.ScreenNombre)
        gridEscribir.Visibility = Visibility.Collapsed

        Dim gridBusqueda As Grid = pagina.FindName("gridBusqueda" + usuario.ScreenNombre)
        gridBusqueda.Visibility = Visibility.Collapsed

        gridElegido.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonCambiarCuentaClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim usuario As TwitterUsuario = boton.Tag

        CambiarCuenta(usuario.ScreenNombre)

    End Sub

    Public Sub CambiarCuenta(usuarioRecibido As String)

        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of TwitterUsuario)

        If helper.KeyExists("listaUsuarios2") Then
            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
        End If

        Dim usuario As TwitterUsuario = Nothing

        For Each usuarioTwitter As TwitterUsuario In listaUsuarios
            If usuarioRecibido = usuarioTwitter.ScreenNombre Then
                usuario = usuarioTwitter
            End If
        Next

        If Not usuario Is Nothing Then
            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + usuario.Nombre

            Dim itemUsuarios As NavigationViewItem = pagina.FindName("itemUsuarios")
            itemUsuarios.Tag = usuario

            Dim imagenCuenta As ImageBrush = pagina.FindName("imagenCuentaSeleccionada")
            imagenCuenta.ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))

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

                If grid.Name = "gridUsuario" + usuario.ScreenNombre Then
                    For Each subgrid As Grid In grid.Children
                        If subgrid.Name.Contains("gridTweets" + usuario.ScreenNombre) Then
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
