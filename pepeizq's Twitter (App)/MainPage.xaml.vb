Imports System.Net.NetworkInformation
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports Windows.Media.Core
Imports Windows.Media.Playback
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Media.Animation

Public NotInheritable Class MainPage
    Inherits Page

    Private Async Sub Page_Loaded(sender As FrameworkElement, args As Object)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Configuracion.Iniciar()
        MasCosas.Generar()

        Dim recursos As New Resources.ResourceLoader

        GridVisibilidad(gridPrincipal, Nothing)

        If NetworkInterface.GetIsNetworkAvailable = True Then
            Dim helper As New LocalObjectStorageHelper

            Dim listaUsuarios As New List(Of TwitterUsuario)

            If helper.KeyExists("listaUsuarios2") Then
                listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
            End If

            Dim i As Integer = 0

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

                    If Not megaUsuario Is Nothing Then
                        UsuarioXaml.Generar(megaUsuario, visibilidad)
                    End If

                    i += 1
                End If
            Next

            If i = 0 Then
                botonConfigVolver.Visibility = Visibility.Collapsed
                GridVisibilidad(gridConfig, recursos.GetString("Config"))
            Else
                botonConfigVolver.Visibility = Visibility.Visible
            End If
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
                                                                       gridPrincipal.Background = App.Current.Resources("GridTituloBackground")
                                                                       gridConfig.Background = App.Current.Resources("GridAcrilico")
                                                                       gridConfigCuentas.Background = App.Current.Resources("GridTituloBackground")
                                                                       gridConfigNotificaciones.Background = App.Current.Resources("GridTituloBackground")
                                                                       gridImagenAmpliada.Background = App.Current.Resources("GridAcrilico")
                                                                       gridVideoAmpliado.Background = App.Current.Resources("GridAcrilico")
                                                                       gridOEmbedAmpliado.Background = App.Current.Resources("GridAcrilico")
                                                                       gridMasCosas.Background = App.Current.Resources("GridAcrilico")
                                                                   Else
                                                                       gridPrincipal.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                       gridConfig.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridConfigCuentas.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                       gridConfigNotificaciones.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                       gridImagenAmpliada.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridVideoAmpliado.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridOEmbedAmpliado.Background = New SolidColorBrush(Colors.LightGray)
                                                                       gridMasCosas.Background = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
                                                                   End If
                                                               End Sub)

    End Sub

    Public Sub GridVisibilidad(grid As Grid, tag As String)

        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ")"

        If Not tag = String.Empty Then
            tbTitulo.Text = tbTitulo.Text + " - " + tag
        End If

        gridImagenAmpliada.Visibility = Visibility.Collapsed
        gridVideoAmpliado.Visibility = Visibility.Collapsed
        gridUsuarioAmpliado.Visibility = Visibility.Collapsed
        gridTweetAmpliado.Visibility = Visibility.Collapsed
        gridConfig.Visibility = Visibility.Collapsed
        gridMasCosas.Visibility = Visibility.Collapsed

        grid.Visibility = Visibility.Visible

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

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
                                    AddHandler subCuenta.PointerEntered, AddressOf UsuarioEntraBoton
                                    AddHandler subCuenta.PointerExited, AddressOf UsuarioSaleBoton
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

    Private Sub BotonConfigAppAutoArranque_Checked(sender As Object, e As RoutedEventArgs) Handles botonConfigAppAutoArranque.Click

        Configuracion.AutoArranque()

    End Sub

    Private Sub CbConfigAppCargarMedia_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppCargarMedia.Checked

        Configuracion.CargarMedia(True)

    End Sub

    Private Sub CbConfigAppCargarMedia_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppCargarMedia.Unchecked

        Configuracion.CargarMedia(False)

    End Sub

    Private Sub CbConfigAppTweetCard_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppTweetCard.Checked

        Configuracion.CargarTweetCard(True)

    End Sub

    Private Sub CbConfigAppTweetCard_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigAppTweetCard.Unchecked

        Configuracion.CargarTweetCard(False)

    End Sub

    Private Sub CbConfigNotificaciones_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificaciones.Checked

        Configuracion.NotificacionesEnseñar(True)

    End Sub

    Private Sub CbConfigNotificaciones_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificaciones.Unchecked

        Configuracion.NotificacionesEnseñar(False)

    End Sub

    Private Sub CbConfigNotificacionesTiempo_Checked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesTiempo.Checked

        Configuracion.NotificacionesTiempo(True)

    End Sub

    Private Sub CbConfigNotificacionesTiempo_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbConfigNotificacionesTiempo.Unchecked

        Configuracion.NotificacionesTiempo(False)

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

    Private Sub BotonCerrarUsuario_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarUsuario.Click

        App.Current.Resources("ButtonBackgroundPointerOver") = App.Current.Resources("ColorPrimario")

        gridTitulo.Background = App.Current.Resources("GridTituloBackground")
        gridUsuarioAmpliado.Visibility = Visibility.Collapsed

    End Sub

    Private Sub BotonCerrarTweet_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarTweet.Click

        App.Current.Resources("ButtonBackgroundPointerOver") = App.Current.Resources("ColorPrimario")

        gridTitulo.Background = App.Current.Resources("GridTituloBackground")
        gridTweetAmpliado.Visibility = Visibility.Collapsed

    End Sub

    Private Sub BotonCerrarOEmbed_Click(sender As Object, e As RoutedEventArgs) Handles botonCerrarOEmbed.Click

        gridOEmbedAmpliado.Visibility = Visibility.Collapsed

    End Sub

End Class

