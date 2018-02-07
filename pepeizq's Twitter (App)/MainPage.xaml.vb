Imports Microsoft.Services.Store.Engagement
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports Windows.ApplicationModel.Core
Imports Windows.Media.Core
Imports Windows.Media.Playback
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Xaml.Media.Animation

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub Page_Loaded(sender As FrameworkElement, args As Object)

        Configuracion.Iniciar()

        '--------------------------------------------------------

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Dim coreBarra As CoreApplicationViewTitleBar = CoreApplication.GetCurrentView.TitleBar
        coreBarra.ExtendViewIntoTitleBar = True

        Dim barra As ApplicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar
        barra.ButtonBackgroundColor = Colors.Transparent
        barra.ButtonForegroundColor = Colors.White
        barra.ButtonInactiveBackgroundColor = Colors.Transparent

        '--------------------------------------------------------

        tbTitulo.Text = Package.Current.DisplayName

        Dim recursos As New Resources.ResourceLoader

        GridVisibilidad(gridPrincipal, Nothing)

        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of TwitterUsuario)

        If helper.KeyExists("listaUsuarios2") Then
            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
        End If

        Dim i As Integer = 0

        For Each usuario In listaUsuarios
            Dim megaUsuario As pepeizq.Twitter.MegaUsuario = Await TwitterConexion.Iniciar(usuario)

            Dim visibilidad As New Visibility

            If i = 0 Then
                visibilidad = Visibility.Visible
            Else
                visibilidad = Visibility.Collapsed
            End If

            If Not megaUsuario Is Nothing Then
                UsuarioXaml.Generar(megaUsuario, visibilidad)
            End If

            i += 1
        Next

        If listaUsuarios.Count = 0 Then
            botonConfigVolver.Visibility = Visibility.Collapsed
            'botonAñadirCuenta.Visibility = Visibility.Visible
            GridVisibilidad(gridConfig, recursos.GetString("Config"))
        Else
            botonConfigVolver.Visibility = Visibility.Visible
            'botonAñadirCuenta.Visibility = Visibility.Collapsed
        End If

        '--------------------------------------------------------

        Dim transpariencia As New UISettings
        Dim boolTranspariencia As Boolean = transpariencia.AdvancedEffectsEnabled

        If boolTranspariencia = False Then
            gridConfig.Background = New SolidColorBrush(Colors.LightGray)
            gridConfigCuentas.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridConfigNotificaciones.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
            gridImagenAmpliada.Background = New SolidColorBrush(Colors.LightGray)
            gridVideoAmpliado.Background = New SolidColorBrush(Colors.LightGray)
            gridMasCosas.Background = New SolidColorBrush(Colors.LightGray)
        End If

    End Sub

    Public Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

        If Not tag = String.Empty Then
            tbTitulo.Text = tbTitulo.Text + " - " + tag
        End If

        gridImagenAmpliada.Visibility = Visibility.Collapsed
        gridVideoAmpliado.Visibility = Visibility.Collapsed
        gridUsuarioAmpliado.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    'CONFIG-----------------------------------------------------------------------------

    Private Sub BotonConfigVolver_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigVolver.Click

        Dim recursos As New Resources.ResourceLoader
        GridVisibilidad(gridPrincipal, Nothing)

        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of TwitterUsuario)

        If helper.KeyExists("listaUsuarios2") Then
            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
        End If

        If listaUsuarios.Count > 0 Then
            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim grid As Grid = pagina.FindName("gridUsuario" + listaUsuarios(0).ScreenNombre)

            grid.Visibility = Visibility.Visible
        End If

    End Sub

    Private Async Sub BotonAñadirCuenta_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirCuenta.Click

        Dim recursos As New Resources.ResourceLoader
        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of pepeizq.Twitter.MegaUsuario)

        If helper.KeyExists("listaUsuarios2") Then
            listaUsuarios = helper.Read(Of List(Of pepeizq.Twitter.MegaUsuario))("listaUsuarios2")
        End If

        Dim visibilidad As New Visibility

        'If listaUsuarios.Count = 0 Then
        '    visibilidad = Visibility.Visible
        'Else
        '    visibilidad = Visibility.Collapsed
        'End If

        Dim usuario As pepeizq.Twitter.MegaUsuario = Await TwitterConexion.Iniciar(Nothing)

        If Not usuario Is Nothing Then
            UsuarioXaml.Generar(usuario, visibilidad)

            For Each grid As Grid In gridPrincipal.Children
                If grid.Name.Contains("gridUsuario") Then
                    If Not grid.Name = "gridUsuarioAmpliado" Then
                        If Not grid.Name = "gridUsuario" + usuario.Usuario.ScreenNombre Then
                            Dim subGrid As Grid = grid.Children(0)

                            If Not subGrid Is Nothing Then
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
                End If
            Next
        End If

    End Sub

    Private Sub CbConfigNotificaciones_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificaciones.Checked

        ConfigNotificacion(True)

    End Sub

    Private Sub CbConfigNotificaciones_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificaciones.Unchecked

        ConfigNotificacion(False)

    End Sub

    Private Sub CbConfigNotificacionesTiempo_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesTiempo.Checked

        ConfigNotificacionTiempo(True)

    End Sub

    Private Sub CbConfigNotificacionesTiempo_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesTiempo.Unchecked

        ConfigNotificacionTiempo(False)

    End Sub

    Private Sub TbConfigNotificacionesSegundos_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbConfigNotificacionesSegundos.TextChanged

        If tbConfigNotificacionesSegundos.Text = String.Empty Then
            tbConfigNotificacionesSegundos.Text = 30
        Else
            Dim segundos As String = tbConfigNotificacionesSegundos.Text
            segundos = segundos.Replace("_", Nothing)

            If segundos = "0" Or segundos = "00" Then
                ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos") = 1
                tbConfigNotificacionesSegundos.Text = 1
            Else
                ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos") = segundos
            End If
        End If

    End Sub

    Private Sub CbConfigNotificacionesSonido_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesSonido.Checked

        ConfigNotificacionSonido(True)

    End Sub

    Private Sub CbConfigNotificacionesSonido_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesSonido.Unchecked

        ConfigNotificacionSonido(False)

    End Sub

    Private Sub CbConfigNotificacionesSonidoElegido_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cbConfigNotificacionesSonidoElegido.SelectionChanged

        Dim cbItem As ComboBoxItem = cbConfigNotificacionesSonidoElegido.SelectedItem
        ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = cbItem.Tag

    End Sub

    Private Sub BotonConfigNotificacionesSonido_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigNotificacionesSonido.Click

        Dim cbItem As ComboBoxItem = cbConfigNotificacionesSonidoElegido.SelectedItem

        Dim sonido As New MediaPlayer With {
            .Source = MediaSource.CreateFromUri(New Uri(cbItem.Tag))
        }

        sonido.Play()

    End Sub

    Private Sub CbConfigNotificacionesImagen_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesImagen.Checked

        ConfigNotificacionImagen(True)

    End Sub

    Private Sub CbConfigNotificacionesImagen_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesImagen.Unchecked

        ConfigNotificacionImagen(False)

    End Sub

    'MEDIA-----------------------------------------------------------------------------

    Private Sub BotonCerrarImagen_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarImagen.Click

        gridImagenAmpliada.Visibility = Visibility.Collapsed

        Dim imagenOrigen As ImageEx = imagenAmpliada.Tag

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imagenReducida", imagenAmpliada)

        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imagenReducida")

        If Not animacion Is Nothing Then
            animacion.TryStart(imagenOrigen)
        End If

    End Sub

    Private Sub BotonCerrarVideo_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarVideo.Click

        videoAmpliado.MediaPlayer.Pause()

        gridVideoAmpliado.Visibility = Visibility.Collapsed

        Dim imagenOrigen As ImageEx = videoAmpliado.Tag

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("videoReducido", videoAmpliado)

        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("videoReducido")

        If Not animacion Is Nothing Then
            animacion.TryStart(imagenOrigen)
        End If

    End Sub

    Private Sub BotonCerrarUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarUsuario.Click

        App.Current.Resources("ButtonBackgroundPointerOver") = App.Current.Resources("ColorPrimario")

        gridTitulo.Background = App.Current.Resources("GridTituloBackground")
        gridUsuarioAmpliado.Visibility = Visibility.Collapsed

    End Sub

    'MASCOSAS-----------------------------------------

    Private Async Sub LvMasCosasItemClick(sender As Object, args As ItemClickEventArgs)

        Dim sp As StackPanel = args.ClickedItem

        If sp.Tag.ToString = 0 Then

            Await Launcher.LaunchUriAsync(New Uri("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName))

        ElseIf sp.Tag.ToString = 1 Then

            NavegarMasCosas(lvMasCosasMasApps, "https://pepeizqapps.com/")

        ElseIf sp.Tag.ToString = 3 Then

            NavegarMasCosas(lvMasCosasContacto, "https://pepeizqapps.com/contact/")

        ElseIf sp.Tag.ToString = 4 Then

            If StoreServicesFeedbackLauncher.IsSupported = True Then
                Dim ejecutador As StoreServicesFeedbackLauncher = StoreServicesFeedbackLauncher.GetDefault()
                Await ejecutador.LaunchAsync()
            Else
                NavegarMasCosas(lvMasCosasReportarFallo, "https://pepeizqapps.com/contact/")
            End If

        ElseIf sp.Tag.ToString = 5 Then

            NavegarMasCosas(lvMasCosasTraduccion, "https://poeditor.com/join/project/aKmScyB4QT")

        ElseIf sp.Tag.ToString = 6 Then

            NavegarMasCosas(lvMasCosasCodigoFuente, "https://github.com/pepeizq/Steam-Tiles")

        End If

    End Sub

    Private Sub NavegarMasCosas(lvItem As ListViewItem, url As String)

        lvMasCosasMasApps.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        lvMasCosasContacto.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        lvMasCosasReportarFallo.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        lvMasCosasTraduccion.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        lvMasCosasCodigoFuente.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

        lvItem.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))

        pbMasCosas.Visibility = Visibility.Visible

        wvMasCosas.Navigate(New Uri(url))

    End Sub

    Private Sub WvMasCosas_NavigationCompleted(sender As WebView, args As WebViewNavigationCompletedEventArgs) Handles wvMasCosas.NavigationCompleted

        pbMasCosas.Visibility = Visibility.Collapsed

    End Sub

End Class

