Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Newtonsoft.Json
Imports pepeizq.Twitter
Imports pepeizq.Twitter.OAuth
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Module TwitterConexion

    'Public Async Function Iniciar(usuarioRecibido As TwitterUsuario) As Task(Of pepeizq.Twitter.MegaUsuario)

    '    'Dim consumerKey As String = ApplicationData.Current.LocalSettings.Values("consumerkey")
    '    'Dim consumerSecret As String = ApplicationData.Current.LocalSettings.Values("consumersecret")

    '    'Dim estado As Boolean = False
    '    'Dim servicio As New TwitterServicio

    '    'If Not usuarioRecibido Is Nothing Then
    '    '    ApplicationData.Current.LocalSettings.Values("TwitterScreenNombre") = usuarioRecibido.ScreenNombre
    '    'Else
    '    '    ApplicationData.Current.LocalSettings.Values("TwitterScreenNombre") = Nothing
    '    'End If

    '    'Try
    '    '    servicio.Iniciar(consumerKey, consumerSecret, "https://pepeizqapps.com/")
    '    '    estado = Await servicio.Provider.Logear
    '    'Catch ex As Exception
    '    '    estado = False
    '    'End Try

    '    'If estado = True Then
    '    '    Dim enlaceString As String = Nothing

    '    '    If Not usuarioRecibido Is Nothing Then
    '    '        enlaceString = "https://api.twitter.com/1.1/users/show.json?screen_name=" + usuarioRecibido.ScreenNombre
    '    '    Else
    '    '        enlaceString = "https://api.twitter.com/1.1/users/show.json?screen_name=" + servicio.twitterDataProvider.UsuarioScreenNombre
    '    '    End If

    '    '    Dim enlace As New Uri(enlaceString)
    '    '    Dim request As New TwitterOAuthRequest
    '    '    Dim resultado As String = Await request.EjecutarGetAsync(enlace, servicio.twitterDataProvider._tokens)
    '    '    Dim usuario As TwitterUsuario = JsonConvert.DeserializeObject(Of TwitterUsuario)(resultado)

    '    '    If Not usuario Is Nothing Then
    '    '        Dim megaUsuario As New pepeizq.Twitter.MegaUsuario(usuario, servicio, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

    '    '        If ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionInicio") Is Nothing Then
    '    '            ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionInicio") = True
    '    '        End If

    '    '        megaUsuario.NotificacionInicio = ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionInicio")

    '    '        If ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionMenciones") Is Nothing Then
    '    '            ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionMenciones") = True
    '    '        End If

    '    '        megaUsuario.NotificacionMenciones = ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionMenciones")

    '    '        Dim listaBloqueos As New List(Of String)

    '    '        listaBloqueos = Await TwitterPeticiones.CogerListaBloqueos(listaBloqueos, megaUsuario)

    '    '        If listaBloqueos.Count > 0 Then
    '    '            megaUsuario.UsuariosBloqueados = listaBloqueos
    '    '        End If

    '    '        Dim listaMuteados As New List(Of String)

    '    '        listaMuteados = Await TwitterPeticiones.CogerListaMuteados(listaMuteados, megaUsuario)

    '    '        If listaMuteados.Count > 0 Then
    '    '            megaUsuario.UsuariosMuteados = listaMuteados
    '    '        End If

    '    '        Dim helper As New LocalObjectStorageHelper

    '    '        Dim listaUsuarios As New List(Of TwitterUsuario)

    '    '        If helper.KeyExists("listaUsuarios5") Then
    '    '            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios5")
    '    '        End If

    '    '        Dim añadirLista As Boolean = True

    '    '        For Each item In listaUsuarios
    '    '            If item.ID = usuario.ID Then
    '    '                añadirLista = False
    '    '            End If
    '    '        Next

    '    '        If añadirLista = True Then
    '    '            listaUsuarios.Add(usuario)

    '    '            helper.Save("listaUsuarios5", listaUsuarios)
    '    '        End If

    '    '        Dim frame As Frame = Window.Current.Content
    '    '        Dim pagina As Page = frame.Content

    '    '        Dim lvUsuarios As ListView = pagina.FindName("lvConfigUsuarios")

    '    '        lvUsuarios.Items.Add(ConfigAñadirUsuarioXaml(megaUsuario))

    '    '        Return megaUsuario
    '    '    Else
    '    '        Return Nothing
    '    '    End If
    '    'Else
    '    '    Return Nothing
    '    'End If

    'End Function

    'Public Function Desconectar(servicio As TwitterServicio)

    '    Try
    '        servicio.Deslogear()
    '    Catch ex As Exception

    '    End Try

    '    Return servicio.Provider.Logeado

    'End Function

    Private Function ConfigAñadirUsuarioXaml(megaUsuario As pepeizq.Twitter.MegaUsuario)

        'Dim recursos As New Resources.ResourceLoader

        'Dim usuario As TwitterUsuario = megaUsuario.Usuario

        'Dim gridUsuario As New Grid With {
        '    .Padding = New Thickness(5, 5, 5, 5),
        '    .Tag = megaUsuario
        '}

        'Dim col1 As New ColumnDefinition
        'Dim col2 As New ColumnDefinition
        'Dim col3 As New ColumnDefinition
        'Dim col4 As New ColumnDefinition
        'Dim col5 As New ColumnDefinition
        'Dim col6 As New ColumnDefinition

        'col1.Width = New GridLength(1, GridUnitType.Auto)
        'col2.Width = New GridLength(1, GridUnitType.Star)
        'col3.Width = New GridLength(1, GridUnitType.Auto)
        'col4.Width = New GridLength(1, GridUnitType.Auto)
        'col5.Width = New GridLength(1, GridUnitType.Auto)
        'col6.Width = New GridLength(1, GridUnitType.Auto)

        'gridUsuario.ColumnDefinitions.Add(col1)
        'gridUsuario.ColumnDefinitions.Add(col2)
        'gridUsuario.ColumnDefinitions.Add(col3)
        'gridUsuario.ColumnDefinitions.Add(col4)
        'gridUsuario.ColumnDefinitions.Add(col5)
        'gridUsuario.ColumnDefinitions.Add(col6)

        'Dim imagenAvatar As New ImageBrush With {
        '    .Stretch = Stretch.Uniform,
        '    .ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))
        '}

        'Dim circulo As New Ellipse With {
        '    .Fill = imagenAvatar,
        '    .VerticalAlignment = VerticalAlignment.Center,
        '    .Margin = New Thickness(0, 0, 10, 0),
        '    .Height = 40,
        '    .Width = 40
        '}

        'circulo.SetValue(Grid.ColumnProperty, 0)
        'gridUsuario.Children.Add(circulo)

        'Dim tbUsuario As New TextBlock With {
        '    .Text = usuario.Nombre + " (@" + usuario.ScreenNombre + ")",
        '    .VerticalAlignment = VerticalAlignment.Center
        '}

        'tbUsuario.SetValue(Grid.ColumnProperty, 1)
        'gridUsuario.Children.Add(tbUsuario)

        ''--------------------------------------

        'Dim spBotonAñadirTile As New StackPanel With {
        '    .Orientation = Orientation.Horizontal
        '}

        'Dim simboloBotonAñadirTile As New FontAwesome.UWP.FontAwesome With {
        '    .Foreground = New SolidColorBrush(Colors.White),
        '    .Icon = FontAwesomeIcon.ThumbTack
        '}

        'spBotonAñadirTile.Children.Add(simboloBotonAñadirTile)

        'Dim textoBotonAñadirTile As New TextBlock With {
        '    .Foreground = New SolidColorBrush(Colors.White),
        '    .Text = recursos.GetString("ButtonAddTile"),
        '    .Margin = New Thickness(10, 0, 0, 0)
        '}

        'spBotonAñadirTile.Children.Add(textoBotonAñadirTile)

        'Dim botonAñadirTile As New Button With {
        '    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
        '    .Content = spBotonAñadirTile,
        '    .Tag = megaUsuario,
        '    .Padding = New Thickness(10, 10, 10, 10),
        '    .Name = "botonAñadirTile" + megaUsuario.Usuario.ID,
        '    .Margin = New Thickness(0, 0, 15, 0)
        '}

        'AddHandler botonAñadirTile.Click, AddressOf BotonAñadirTileCuenta
        'AddHandler botonAñadirTile.PointerEntered, AddressOf UsuarioEntraBoton
        'AddHandler botonAñadirTile.PointerExited, AddressOf UsuarioSaleBoton

        'botonAñadirTile.SetValue(Grid.ColumnProperty, 2)
        'gridUsuario.Children.Add(botonAñadirTile)

        ''--------------------------------------

        'Dim spBotonNotificaciones As New StackPanel With {
        '    .Orientation = Orientation.Horizontal
        '}

        'Dim simboloBotonNotificaciones As New FontAwesome.UWP.FontAwesome With {
        '    .Foreground = New SolidColorBrush(Colors.White),
        '    .Icon = FontAwesomeIcon.Comment
        '}

        'spBotonNotificaciones.Children.Add(simboloBotonNotificaciones)

        'Dim textoBotonNotificaciones As New TextBlock With {
        '    .Foreground = New SolidColorBrush(Colors.White),
        '    .Text = recursos.GetString("Notifications"),
        '    .Margin = New Thickness(10, 0, 0, 0)
        '}

        'spBotonNotificaciones.Children.Add(textoBotonNotificaciones)

        'Dim botonNotificaciones As New Button With {
        '    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
        '    .Content = spBotonNotificaciones,
        '    .Tag = megaUsuario,
        '    .Padding = New Thickness(10, 10, 10, 10)
        '}

        'AddHandler botonNotificaciones.Click, AddressOf BotonNotificacionesClick
        'AddHandler botonNotificaciones.PointerEntered, AddressOf UsuarioEntraBoton
        'AddHandler botonNotificaciones.PointerExited, AddressOf UsuarioSaleBoton

        'botonNotificaciones.SetValue(Grid.ColumnProperty, 3)
        'gridUsuario.Children.Add(botonNotificaciones)

        ''--------------------------------------

        'Dim separador As New AppBarSeparator With {
        '    .Foreground = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        '}

        'separador.SetValue(Grid.ColumnProperty, 4)
        'gridUsuario.Children.Add(separador)

        ''--------------------------------------

        'Dim simboloQuitar As New FontAwesome.UWP.FontAwesome With {
        '    .Foreground = New SolidColorBrush(Colors.White),
        '    .Icon = FontAwesomeIcon.Times,
        '    .VerticalAlignment = VerticalAlignment.Center
        '}

        'Dim botonQuitar As New Button With {
        '    .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
        '    .Content = simboloQuitar,
        '    .Tag = megaUsuario,
        '    .Padding = New Thickness(10, 10, 10, 10)
        '}

        'ToolTipService.SetToolTip(botonQuitar, recursos.GetString("ButtonDeleteAccount"))
        'ToolTipService.SetPlacement(botonQuitar, PlacementMode.Bottom)

        'AddHandler botonQuitar.Click, AddressOf BotonQuitarCuenta
        'AddHandler botonQuitar.PointerEntered, AddressOf UsuarioEntraBoton
        'AddHandler botonQuitar.PointerExited, AddressOf UsuarioSaleBoton

        'botonQuitar.SetValue(Grid.ColumnProperty, 5)
        'gridUsuario.Children.Add(botonQuitar)

        'Return gridUsuario

    End Function

    'Private Sub BotonAñadirTileCuenta(sender As Object, e As RoutedEventArgs)

    '    Dim boton As Button = sender
    '    Dim megaUsuario As pepeizq.Twitter.MegaUsuario = boton.Tag

    '    Tiles.Generar(megaUsuario)

    'End Sub

    'Private Sub BotonNotificacionesClick(sender As Object, e As RoutedEventArgs)

    '    Dim recursos As New Resources.ResourceLoader

    '    Dim boton As Button = sender
    '    Dim megaUsuario As pepeizq.Twitter.MegaUsuario = boton.Tag

    '    Dim menu As New MenuFlyout With {
    '        .Placement = FlyoutPlacementMode.Bottom
    '    }

    '    Dim toggleNotificacionInicio As New ToggleMenuFlyoutItem With {
    '        .Text = recursos.GetString("Home"),
    '        .IsChecked = megaUsuario.NotificacionInicio,
    '        .Tag = megaUsuario
    '    }

    '    AddHandler toggleNotificacionInicio.Click, AddressOf ToggleNotificacionInicioClick
    '    AddHandler toggleNotificacionInicio.PointerEntered, AddressOf UsuarioEntraBoton
    '    AddHandler toggleNotificacionInicio.PointerExited, AddressOf UsuarioSaleBoton

    '    menu.Items.Add(toggleNotificacionInicio)

    '    Dim toggleNotificacionMenciones As New ToggleMenuFlyoutItem With {
    '        .Text = recursos.GetString("Mentions"),
    '        .IsChecked = megaUsuario.NotificacionMenciones,
    '        .Tag = megaUsuario
    '    }

    '    AddHandler toggleNotificacionMenciones.Click, AddressOf ToggleNotificacionMencionesClick
    '    AddHandler toggleNotificacionMenciones.PointerEntered, AddressOf UsuarioEntraBoton
    '    AddHandler toggleNotificacionMenciones.PointerExited, AddressOf UsuarioSaleBoton

    '    menu.Items.Add(toggleNotificacionMenciones)

    '    FlyoutBase.SetAttachedFlyout(boton, menu)
    '    menu.ShowAt(boton)

    'End Sub

    Private Sub ToggleNotificacionInicioClick(sender As Object, e As RoutedEventArgs)

        'Dim item As ToggleMenuFlyoutItem = sender
        'Dim megaUsuario As pepeizq.Twitter.MegaUsuario = item.Tag
        'Dim usuario As TwitterUsuario = megaUsuario.Usuario

        'megaUsuario.NotificacionInicio = item.IsChecked
        'ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionInicio") = item.IsChecked

    End Sub

    Private Sub ToggleNotificacionMencionesClick(sender As Object, e As RoutedEventArgs)

        'Dim item As ToggleMenuFlyoutItem = sender
        'Dim megaUsuario As pepeizq.Twitter.MegaUsuario = item.Tag
        'Dim usuario As TwitterUsuario = megaUsuario.Usuario

        'megaUsuario.NotificacionMenciones = item.IsChecked
        'ApplicationData.Current.LocalSettings.Values(usuario.ID + "notificacionMenciones") = item.IsChecked

    End Sub

    Private Sub BotonQuitarCuenta(sender As Object, e As RoutedEventArgs)

        'Dim recursos As New Resources.ResourceLoader

        'Dim boton As Button = sender
        'Dim megaUsuario As pepeizq.Twitter.MegaUsuario = boton.Tag
        'Dim servicio As TwitterServicio = megaUsuario.Servicio

        'Dim conexion As Boolean = TwitterConexion.Desconectar(servicio)

        'If conexion = False Then
        '    If Not megaUsuario.StreamHome Is Nothing Then
        '        megaUsuario.StreamHome.Cancel()
        '    End If

        '    If Not megaUsuario.StreamMentions Is Nothing Then
        '        megaUsuario.StreamMentions.Cancel()
        '    End If

        '    Dim frame As Frame = Window.Current.Content
        '    Dim pagina As Page = frame.Content

        '    Dim lvConfigUsuarios As ListView = pagina.FindName("lvConfigUsuarios")

        '    Dim i As Integer = 0
        '    For Each item In lvConfigUsuarios.Items
        '        Dim grid As Grid = item
        '        Dim megaUsuarioGrid As pepeizq.Twitter.MegaUsuario = grid.Tag

        '        If megaUsuarioGrid.Usuario.ID = megaUsuario.Usuario.ID Then
        '            lvConfigUsuarios.Items.RemoveAt(i)
        '            Exit For
        '        End If

        '        i += 1
        '    Next

        '    Dim menu As MenuFlyout = pagina.FindName("botonUsuariosMenu")

        '    i = 0
        '    For Each item As MenuFlyoutItem In menu.Items
        '        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = item.Tag

        '        If cosas.Usuario.ID = megaUsuario.Usuario.ID Then
        '            menu.Items.RemoveAt(i)

        '            Exit For
        '        End If

        '        i += 1
        '    Next

        '    If menu.Items.Count > 0 Then
        '        If i = 0 Then
        '            Dim nuevoUsuario As TwitterUsuario = menu.Items(0).Tag
        '            UsuarioXaml.CambiarCuenta(megaUsuario, nuevoUsuario.ScreenNombre, False)
        '        End If
        '    End If

        '    Dim gridUsuario As Grid = pagina.FindName("gridUsuario" + megaUsuario.Usuario.ID)
        '    gridUsuario.Children.Clear()

        '    Dim helper As New LocalObjectStorageHelper

        '    Dim listaUsuarios As New List(Of TwitterUsuario)

        '    If helper.KeyExists("listaUsuarios5") Then
        '        listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios5")
        '    End If

        '    i = 0
        '    For Each item In listaUsuarios
        '        If item.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
        '            listaUsuarios.RemoveAt(i)
        '            Exit For
        '        End If

        '        i += 1
        '    Next

        '    helper.Save("listaUsuarios5", listaUsuarios)

        '    Dim tbNumeroCuentas As TextBlock = pagina.FindName("tbNumeroCuentas")
        '    tbNumeroCuentas.Text = listaUsuarios.Count.ToString + "/25"

        '    If listaUsuarios.Count < 26 Then
        '        Dim botonAñadirCuenta As Button = pagina.FindName("botonAñadirCuenta")
        '        botonAñadirCuenta.IsEnabled = True
        '    End If

        '    Dim nvPrincipal As NavigationView = pagina.FindName("nvPrincipal")
        '    Dim nvItemUsuarios As NavigationViewItem = pagina.FindName("nvItemUsuarios")

        '    If lvConfigUsuarios.Items.Count = 0 Then
        '        For Each item In nvPrincipal.MenuItems
        '            If TypeOf item Is NavigationViewItem Then
        '                Dim nvItem As NavigationViewItem = item
        '                nvItem.Visibility = Visibility.Collapsed
        '            End If
        '        Next

        '        nvItemUsuarios.Visibility = Visibility.Collapsed
        '    Else
        '        For Each item In nvPrincipal.MenuItems
        '            If TypeOf item Is NavigationViewItem Then
        '                Dim nvItem As NavigationViewItem = item
        '                nvItem.Visibility = Visibility.Visible
        '            End If
        '        Next

        '        If lvConfigUsuarios.Items.Count = 1 Then
        '            nvItemUsuarios.Visibility = Visibility.Collapsed
        '        Else
        '            nvItemUsuarios.Visibility = Visibility.Visible
        '        End If
        '    End If

        'End If

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
