Imports System.Net.NetworkInformation
Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Media.Core
Imports Windows.Media.Playback
Imports Windows.Networking.BackgroundTransfer
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.StartScreen
Imports Windows.UI.Xaml.Media.Animation

Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Home"), FontAwesomeIcon.Home, 0))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Mentions"), FontAwesomeIcon.Bell, 1))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("WriteTweet"), FontAwesomeIcon.Pencil, 2))
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Search"), FontAwesomeIcon.Search, 3))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)
        nvPrincipal.MenuItems.Add(NavigationViewItems.Generar(recursos.GetString("Config"), FontAwesomeIcon.Cog, 4))

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim usuario As TwitterUsuario = Nothing

        If TypeOf itemUsuarios.Tag Is TwitterUsuario Then
            usuario = itemUsuarios.Tag
        ElseIf TypeOf itemUsuarios.Tag Is pepeizq.Twitter.MegaUsuario Then
            Dim megaUsuario As pepeizq.Twitter.MegaUsuario = itemUsuarios.Tag
            usuario = megaUsuario.Usuario
        End If

        gridConfig.Visibility = Visibility.Collapsed
        gridImagenAmpliada.Visibility = Visibility.Collapsed
        gridVideoAmpliado.Visibility = Visibility.Collapsed
        gridUsuarioAmpliado.Visibility = Visibility.Collapsed
        gridTweetAmpliado.Visibility = Visibility.Collapsed
        gridOEmbedAmpliado.Visibility = Visibility.Collapsed

        App.Current.Resources("ButtonBackgroundPointerOver") = App.Current.Resources("ColorPrimario")

        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim item As TextBlock = args.InvokedItem

        If Not item Is Nothing Then
            If item.Text = recursos.GetString("Home") Then

                If Not usuario Is Nothing Then
                    Dim grid As Grid = pagina.FindName("gridTweets" + usuario.ScreenNombre)

                    If Not grid Is Nothing Then
                        UsuarioXaml.GridVisibilidad(grid, usuario)
                    End If
                End If

            ElseIf item.Text = recursos.GetString("Mentions") Then

                If Not usuario Is Nothing Then
                    Dim grid As Grid = pagina.FindName("gridMenciones" + usuario.ScreenNombre)

                    If Not grid Is Nothing Then
                        UsuarioXaml.GridVisibilidad(grid, usuario)
                    End If
                End If

            ElseIf item.Text = recursos.GetString("WriteTweet") Then

                If Not usuario Is Nothing Then
                    Dim grid As Grid = pagina.FindName("gridEscribir" + usuario.ScreenNombre)

                    If Not grid Is Nothing Then
                        UsuarioXaml.GridVisibilidad(grid, usuario)
                    End If
                End If

            ElseIf item.Text = recursos.GetString("Search") Then

                If Not usuario Is Nothing Then
                    Dim grid As Grid = pagina.FindName("gridBusqueda" + usuario.ScreenNombre)

                    If Not grid Is Nothing Then
                        UsuarioXaml.GridVisibilidad(grid, usuario)
                    End If
                End If

            ElseIf item.Text = recursos.GetString("Config") Then

                GridVisibilidad(gridConfig, Nothing)
                SpConfigVisibilidad(botonConfigCuentas, spConfigCuentas)

            End If
        End If

    End Sub

    Private Sub Nv_ItemFlyout(sender As NavigationViewItem, args As TappedRoutedEventArgs)

        FlyoutBase.ShowAttachedFlyout(sender)

    End Sub

    Private Async Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Configuracion.Iniciar()
        MasCosas.Generar()

        Dim recursos As New Resources.ResourceLoader

        GridVisibilidad(gridPrincipal, Nothing)
        nvPrincipal.IsPaneOpen = False

        If NetworkInterface.GetIsNetworkAvailable = True Then
            Dim helper As New LocalObjectStorageHelper

            Dim listaUsuarios As New List(Of TwitterUsuario)

            If helper.KeyExists("listaUsuarios4") Then
                listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios4")
            End If

            Dim i As Integer = 0

            If listaUsuarios.Count > 0 Then
                UsuarioXaml.GenerarListaUsuarios(listaUsuarios)

                Dim listaSalto As JumpList = Await JumpList.LoadCurrentAsync
                listaSalto.Items.Clear()

                For Each usuario In listaUsuarios
                    Dim megaUsuario As pepeizq.Twitter.MegaUsuario = Nothing

                    Try
                        megaUsuario = Await TwitterConexion.Iniciar(usuario)
                    Catch ex As Exception

                    End Try

                    If Not megaUsuario Is Nothing Then
                        Dim visibilidad As New Visibility

                        If i = 0 Then
                            visibilidad = Visibility.Visible
                        Else
                            visibilidad = Visibility.Collapsed
                        End If

                        UsuarioXaml.GenerarCadaUsuario(megaUsuario, visibilidad)

                        Dim itemSalto As JumpListItem = JumpListItem.CreateWithArguments(megaUsuario.Usuario.ScreenNombre, megaUsuario.Usuario.Nombre)
                        itemSalto.Logo = New Uri("ms-appx:///Assets/logo2.png")
                        listaSalto.Items.Add(itemSalto)

                        i += 1
                    End If
                Next

                Await listaSalto.SaveAsync

            ElseIf listaUsuarios.Count = 0 Then
                For Each item In nvPrincipal.MenuItems
                    If TypeOf item Is NavigationViewItem Then
                        Dim nvItem As NavigationViewItem = item
                        nvItem.Visibility = Visibility.Collapsed
                    End If
                Next

                GridVisibilidad(gridConfig, recursos.GetString("Config"))
                SpConfigVisibilidad(botonConfigCuentas, spConfigCuentas)
            End If

            tbNumeroCuentas.Text = listaUsuarios.Count.ToString + "/35"
        Else
            For Each item In nvPrincipal.MenuItems
                If TypeOf item Is NavigationViewItem Then
                    Dim nvItem As NavigationViewItem = item
                    nvItem.Visibility = Visibility.Collapsed
                End If
            Next

            GridVisibilidad(gridConfig, recursos.GetString("Config"))
            SpConfigVisibilidad(botonConfigCuentas, spConfigCuentas)
        End If

        '--------------------------------------------------------

        Dim transpariencia As New UISettings
        TransparienciaEfectosFinal(transpariencia.AdvancedEffectsEnabled)
        AddHandler transpariencia.AdvancedEffectsEnabledChanged, AddressOf TransparienciaEfectosCambia

    End Sub

    Private Sub TransparienciaEfectosCambia(sender As UISettings, e As Object)

        TransparienciaEfectosFinal(sender.AdvancedEffectsEnabled)

    End Sub

    Private Async Sub TransparienciaEfectosFinal(estado As Boolean)

        Await Dispatcher.RunAsync(CoreDispatcherPriority.High, Sub()
                                                                   If estado = True Then
                                                                       gridConfig.Background = App.Current.Resources("GridAcrilico")
                                                                       gridImagenAmpliada.Background = App.Current.Resources("GridAcrilico")
                                                                       gridVideoAmpliado.Background = App.Current.Resources("GridAcrilico")
                                                                       gridOEmbedAmpliado.Background = App.Current.Resources("GridAcrilico")
                                                                   Else
                                                                       gridConfig.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridImagenAmpliada.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridVideoAmpliado.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridOEmbedAmpliado.Background = New SolidColorBrush(Colors.LightGray)
                                                                   End If
                                                               End Sub)

    End Sub

    Public Sub GridVisibilidad(grid As Grid, tag As String)

        If Not tag = String.Empty Then
            tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"
            tbTitulo.Text = tbTitulo.Text + " - " + tag
        End If

        gridConfig.Visibility = Visibility.Collapsed
        gridImagenAmpliada.Visibility = Visibility.Collapsed
        gridVideoAmpliado.Visibility = Visibility.Collapsed
        gridUsuarioAmpliado.Visibility = Visibility.Collapsed
        gridTweetAmpliado.Visibility = Visibility.Collapsed
        gridOEmbedAmpliado.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    'CONFIG-----------------------------------------------------------------------------

    Public Sub SpConfigVisibilidad(boton As Button, sp As StackPanel)

        botonConfigCuentas.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonConfigApp.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonConfigNotificaciones.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))

        spConfigCuentas.Visibility = Visibility.Collapsed
        spConfigApp.Visibility = Visibility.Collapsed
        spConfigNotificaciones.Visibility = Visibility.Collapsed

        boton.Background = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
        sp.Visibility = Visibility.Visible

    End Sub

    Private Sub BotonConfigCuentas_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigCuentas.Click

        SpConfigVisibilidad(botonConfigCuentas, spConfigCuentas)

    End Sub

    Private Sub BotonConfigApp_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigApp.Click

        SpConfigVisibilidad(botonConfigApp, spConfigApp)

    End Sub

    Private Sub BotonConfigNotificaciones_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigNotificaciones.Click

        SpConfigVisibilidad(botonConfigNotificaciones, spConfigNotificaciones)

    End Sub

    Private Async Sub BotonAñadirCuenta_Click(sender As Object, e As RoutedEventArgs) Handles botonAñadirCuenta.Click

        Dim recursos As New Resources.ResourceLoader
        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of TwitterUsuario)

        If helper.KeyExists("listaUsuarios4") Then
            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios4")
        End If

        Dim visibilidad As New Visibility

        If listaUsuarios.Count = 0 Then
            visibilidad = Visibility.Visible
        Else
            visibilidad = Visibility.Collapsed
        End If

        Dim megaUsuario As pepeizq.Twitter.MegaUsuario = Await TwitterConexion.Iniciar(Nothing)

        If Not megaUsuario Is Nothing Then
            UsuarioXaml.GenerarCadaUsuario(megaUsuario, visibilidad)
        End If

        tbNumeroCuentas.Text = lvConfigUsuarios.Items.Count.ToString + "/35"

        If lvConfigUsuarios.Items.Count > 0 Then
            If lvConfigUsuarios.Items.Count > 1 Then
                itemUsuarios.Visibility = Visibility.Visible
            Else
                itemUsuarios.Visibility = Visibility.Collapsed
            End If

            If lvConfigUsuarios.Items.Count > 35 Then
                botonAñadirCuenta.IsEnabled = False
            End If

            spCuentaSeleccionada.Visibility = Visibility.Visible

            For Each item In nvPrincipal.MenuItems
                If TypeOf item Is NavigationViewItem Then
                    Dim nvItem As NavigationViewItem = item
                    nvItem.Visibility = Visibility.Visible
                End If
            Next
        Else
            itemUsuarios.Visibility = Visibility.Collapsed
            spCuentaSeleccionada.Visibility = Visibility.Collapsed

            For Each item In nvPrincipal.MenuItems
                If TypeOf item Is NavigationViewItem Then
                    Dim nvItem As NavigationViewItem = item
                    nvItem.Visibility = Visibility.Collapsed
                End If
            Next
        End If

    End Sub

    Private Sub BotonConfigAppAutoArranque_Checked(sender As Object, e As RoutedEventArgs) Handles botonConfigAppAutoArranque.Click

        Configuracion.AutoArranque()

    End Sub

    Private Sub CbConfigAppCargarMedia_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppCargarMedia.Checked

        Configuracion.CargarMedia(True)

    End Sub

    Private Sub CbConfigAppCargarMedia_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppCargarMedia.Unchecked

        Configuracion.CargarMedia(False)

    End Sub

    Private Sub SliderMediaVistaPreviaAlto_ValueChanged(sender As Object, e As RangeBaseValueChangedEventArgs) Handles sliderMediaVistaPreviaAlto.ValueChanged

        Dim recursos As New Resources.ResourceLoader

        If Not sliderMediaVistaPreviaAlto.Value = 1 Then
            ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto") = sliderMediaVistaPreviaAlto.Value

            tbMediaVistaPreviaAlto.Text = sliderMediaVistaPreviaAlto.Value.ToString + "px (" + recursos.GetString("Height") + ")"
        End If

    End Sub

    Private Sub SliderMediaVistaPreviaAncho_ValueChanged(sender As Object, e As RangeBaseValueChangedEventArgs) Handles sliderMediaVistaPreviaAncho.ValueChanged

        Dim recursos As New Resources.ResourceLoader

        If Not sliderMediaVistaPreviaAncho.Value = 1 Then
            ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho") = sliderMediaVistaPreviaAncho.Value

            tbMediaVistaPreviaAncho.Text = sliderMediaVistaPreviaAncho.Value.ToString + "px (" + recursos.GetString("Width") + ")"
        End If

    End Sub

    Private Sub CbConfigAppTweetRetweets_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppTweetRetweets.Checked

        Configuracion.CargarTweetRetweets(True)

    End Sub

    Private Sub CbConfigAppTweetRetweets_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppTweetRetweets.Unchecked

        Configuracion.CargarTweetRetweets(False)

    End Sub

    Private Sub CbConfigAppTweetCard_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppTweetCard.Checked

        Configuracion.CargarTweetCard(True)

    End Sub

    Private Sub CbConfigAppTweetCard_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppTweetCard.Unchecked

        Configuracion.CargarTweetCard(False)

    End Sub

    Private Sub BotonConfigAPIMostrar_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigAPIMostrar.Click

        If spConfigAPI.Visibility = Visibility.Collapsed Then
            spConfigAPI.Visibility = Visibility.Visible
        Else
            spConfigAPI.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Async Sub BotonConfigAbrirNuevaApp_Click(sender As Object, e As RoutedEventArgs) Handles botonConfigAbrirNuevaApp.Click

        Await Launcher.LaunchUriAsync(New Uri("https://apps.twitter.com/"))

    End Sub

    Private Sub TbConfigConsumerKey_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbConfigConsumerKey.TextChanged

        If tbConfigConsumerKey.Text.Trim.Length > 0 Then
            ApplicationData.Current.LocalSettings.Values("consumerkey") = tbConfigConsumerKey.Text.Trim
        End If

    End Sub

    Private Sub TbConfigConsumerSecret_TextChanged(sender As Object, e As TextChangedEventArgs) Handles tbConfigConsumerSecret.TextChanged

        If tbConfigConsumerSecret.Text.Trim.Length > 0 Then
            ApplicationData.Current.LocalSettings.Values("consumersecret") = tbConfigConsumerSecret.Text.Trim
        End If

    End Sub

    Private Sub CbConfigNotificacionesAgrupar_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesAgrupar.Checked

        Configuracion.NotificacionesAgrupar(True)

    End Sub

    Private Sub CbConfigNotificacionesAgrupar_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesAgrupar.Unchecked

        Configuracion.NotificacionesAgrupar(False)

    End Sub

    Private Sub CbConfigNotificacionesTiempo_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesTiempo.Checked

        Configuracion.NotificacionesTiempo(True)

    End Sub

    Private Sub CbConfigNotificacionesTiempo_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesTiempo.Unchecked

        Configuracion.NotificacionesTiempo(False)

    End Sub

    Private Sub SliderNotificacionesTiempo_ValueChanged(sender As Object, e As RangeBaseValueChangedEventArgs) Handles sliderNotificacionesTiempo.ValueChanged

        Dim recursos As New Resources.ResourceLoader

        If Not sliderNotificacionesTiempo.Value = 5 Then
            ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos") = sliderNotificacionesTiempo.Value

            tbConfigNotificacionesTiempo.Text = sliderNotificacionesTiempo.Value.ToString + " " + recursos.GetString("Seconds")
        End If

    End Sub

    Private Sub CbConfigNotificacionesSonido_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesSonido.Checked

        Configuracion.NotificacionesSonido(True)

    End Sub

    Private Sub CbConfigNotificacionesSonido_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesSonido.Unchecked

        Configuracion.NotificacionesSonido(False)

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

        Configuracion.NotificacionesImagen(True)

    End Sub

    Private Sub CbConfigNotificacionesImagen_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesImagen.Unchecked

        Configuracion.NotificacionesImagen(False)

    End Sub

    'MEDIA-----------------------------------------------------------------------------

    Private Sub BotonCerrarImagen_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarImagen.Click

        botonCerrarImagen.Tag = Nothing
        gridImagenAmpliada.Visibility = Visibility.Collapsed

        Dim imagenOrigen As ImageEx = imagenAmpliada.Tag

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imagenReducida", imagenAmpliada)

        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imagenReducida")

        If Not animacion Is Nothing Then
            animacion.TryStart(imagenOrigen)
        End If

    End Sub

    Private Async Sub BotonDescargarImagen_Click(sender As Object, e As RoutedEventArgs) Handles botonDescargarImagen.Click

        botonDescargarImagen.IsEnabled = False
        prDescargaImagen.Visibility = Visibility.Visible

        Dim enlace As New Uri(tbImagenAmpliada.Text)

        Dim picker As New FolderPicker()

        picker.FileTypeFilter.Add("*")
        picker.ViewMode = PickerViewMode.List

        Try
            Dim carpeta As StorageFolder = Await picker.PickSingleFolderAsync()
            Dim fichero As StorageFile = Await carpeta.CreateFileAsync("twitter.jpg", CreationCollisionOption.ReplaceExisting)
            Dim descargador As New BackgroundDownloader
            Dim descarga As DownloadOperation = descargador.CreateDownload(enlace, fichero)
            Await descarga.StartAsync
        Catch ex As Exception

        End Try

        prDescargaImagen.Visibility = Visibility.Collapsed
        botonDescargarImagen.IsEnabled = True

    End Sub

    Private Sub BotonCopiarImagen_Click(sender As Object, e As RoutedEventArgs) Handles botonCopiarImagen.Click

        Dim paquete As New DataPackage
        paquete.SetText(tbImagenAmpliada.Text)

        Clipboard.SetContent(paquete)

    End Sub

    Private Sub BotonCerrarVideo_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarVideo.Click

        botonCerrarVideo.Tag = Nothing
        videoAmpliado.MediaPlayer.Pause()

        gridVideoAmpliado.Visibility = Visibility.Collapsed

        Dim imagenOrigen As ImageEx = videoAmpliado.Tag

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("videoReducido", videoAmpliado)

        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("videoReducido")

        If Not animacion Is Nothing Then
            animacion.TryStart(imagenOrigen)
        End If

    End Sub

    Private Async Sub BotonDescargarVideo_Click(sender As Object, e As RoutedEventArgs) Handles botonDescargarVideo.Click

        botonDescargarVideo.IsEnabled = False
        prDescargaVideo.Visibility = Visibility.Visible

        Dim enlace As New Uri(tbVideoAmpliado.Text)

        Dim picker As New FolderPicker()

        picker.FileTypeFilter.Add("*")
        picker.ViewMode = PickerViewMode.List

        Try
            Dim carpeta As StorageFolder = Await picker.PickSingleFolderAsync()
            Dim fichero As StorageFile = Await carpeta.CreateFileAsync("twitter.mp4", CreationCollisionOption.ReplaceExisting)
            Dim descargador As New BackgroundDownloader
            Dim descarga As DownloadOperation = descargador.CreateDownload(enlace, fichero)
            Await descarga.StartAsync
        Catch ex As Exception

        End Try

        prDescargaVideo.Visibility = Visibility.Collapsed
        botonDescargarVideo.IsEnabled = True

    End Sub

    Private Sub BotonCopiarVideo_Click(sender As Object, e As RoutedEventArgs) Handles botonCopiarVideo.Click

        Dim paquete As New DataPackage
        paquete.SetText(tbVideoAmpliado.Text)

        Clipboard.SetContent(paquete)

    End Sub

    Private Sub BotonCerrarTweet_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarTweet.Click

        App.Current.Resources("ButtonBackgroundPointerOver") = App.Current.Resources("ColorPrimario")

        gridTweetAmpliado.Visibility = Visibility.Collapsed

    End Sub

    Private Sub BotonCerrarOEmbed_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarOEmbed.Click

        gridOEmbedAmpliado.Visibility = Visibility.Collapsed

    End Sub

    'USUARIO-----------------------------------------------------------------------------

    Private Async Sub BotonEnlaceUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonEnlaceUsuario.Click

        Await Launcher.LaunchUriAsync(botonEnlaceUsuario.Tag)

    End Sub

    Private Async Sub BotonNumTweetsUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonNumTweetsUsuario.Click

        Await Launcher.LaunchUriAsync(botonNumTweetsUsuario.Tag)

    End Sub

    Private Async Sub BotonSeguidoresUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonSeguidoresUsuario.Click

        Await Launcher.LaunchUriAsync(botonSeguidoresUsuario.Tag)

    End Sub

    Private Async Sub BotonFavoritosUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonFavoritosUsuario.Click

        Await Launcher.LaunchUriAsync(botonFavoritosUsuario.Tag)

    End Sub

    Private Sub BotonSubirArribaUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonSubirArribaUsuario.Click

        svTweetsUsuario.ChangeView(Nothing, 0, Nothing)
        botonSubirArribaUsuario.Visibility = Visibility.Collapsed

    End Sub

End Class

