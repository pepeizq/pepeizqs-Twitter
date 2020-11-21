Imports System.Net
Imports Tweetinvi
Imports Tweetinvi.Models
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Xaml.Documents
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module OtroUsuario

        Public Sub CargarClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As ClienteyTweet = boton.Tag

            Cargar(cosas.cliente, cosas.tweet, Nothing)

        End Sub

        Public Sub CargarClick2(sender As Object, e As RoutedEventArgs)

            Dim enlace As Hyperlink = sender
            Dim contenido As Run = enlace.Inlines(0)
            Dim usuario As String = contenido.Text

            Dim cliente As TwitterClient = Interfaz.Usuario.cliente_

            Cargar(cliente, Nothing, usuario)

        End Sub

        Public Sub CargarClick3(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim usuario As String = boton.Tag

            Dim cliente As TwitterClient = Interfaz.Usuario.cliente_

            Cargar(cliente, Nothing, usuario)

        End Sub

        Public Async Sub Cargar(cliente As TwitterClient, tweet As ITweet, usuarioString As String)

            Dim usuario As IUser = Nothing

            If Not tweet Is Nothing Then
                usuario = tweet.CreatedBy
            Else
                usuario = Await cliente.Users.GetUserAsync(usuarioString)
            End If

            If Not usuario Is Nothing Then
                Dim recursos As New Resources.ResourceLoader

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim gridUsuario As Grid = pagina.FindName("gridUsuario")
                Pestañas.Visibilidad_Pestañas(gridUsuario)

                Dim botonTweets As Button = pagina.FindName("botonOtroUsuarioTweets")
                Dim gridTweets As Grid = pagina.FindName("gridOtroUsuarioTweets")
                Pestañas.Visibilidad_Pestañas_Usuario(botonTweets, gridTweets)

                Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")
                gridUsuarioBotones.Visibility = Visibility.Visible

                Dim botonOtroUsuarioTweets As Button = pagina.FindName("botonOtroUsuarioTweets")
                botonOtroUsuarioTweets.Visibility = Visibility.Visible

                Dim avatarOtroUsuario As Ellipse = pagina.FindName("ellipseOtroUsuarioTweets")
                Dim avatarOtroUsuarioPerfil As Ellipse = pagina.FindName("ellipseOtroUsuarioPerfil")

                Dim imagenAvatar As New ImageBrush With {
                    .Stretch = Stretch.Uniform,
                    .ImageSource = New BitmapImage(New Uri(usuario.ProfileImageUrl))
                }

                avatarOtroUsuario.Fill = imagenAvatar
                avatarOtroUsuarioPerfil.Fill = imagenAvatar

                '--------------------------------------------------------

                Dim relacion As IRelationshipDetails = Await cliente.Users.GetRelationshipBetweenAsync(Interfaz.Usuario.usuario_.Id, usuario.Id)

                Dim botonSeguir As Button = pagina.FindName("botonOtroUsuarioSeguir")
                botonSeguir.Tag = New ClienteyUsuario(cliente, usuario)
                RemoveHandler botonSeguir.Click, AddressOf BotonSeguirClick
                AddHandler botonSeguir.Click, AddressOf BotonSeguirClick

                RemoveHandler botonSeguir.PointerEntered, AddressOf Entra_Boton_Texto
                AddHandler botonSeguir.PointerEntered, AddressOf Entra_Boton_Texto

                RemoveHandler botonSeguir.PointerExited, AddressOf Sale_Boton_Texto
                AddHandler botonSeguir.PointerExited, AddressOf Sale_Boton_Texto

                Dim tbSeguir As TextBlock = pagina.FindName("tbOtroUsuarioSeguir")

                If relacion.Following = True Then
                    tbSeguir.Text = recursos.GetString("Following")
                Else
                    tbSeguir.Text = recursos.GetString("Follow")
                End If

                Dim botonMasOpciones As Button = pagina.FindName("botonOtroUsuarioMasOpciones")
                botonMasOpciones.Tag = New ClienteyUsuario(cliente, usuario)
                RemoveHandler botonMasOpciones.Click, AddressOf MasOpcionesClick
                AddHandler botonMasOpciones.Click, AddressOf MasOpcionesClick

                RemoveHandler botonMasOpciones.PointerEntered, AddressOf Entra_Boton_Icono
                AddHandler botonMasOpciones.PointerEntered, AddressOf Entra_Boton_Icono

                RemoveHandler botonMasOpciones.PointerExited, AddressOf Sale_Boton_Icono
                AddHandler botonMasOpciones.PointerExited, AddressOf Sale_Boton_Icono

                '--------------------------------------------------------

                Dim iconoVerificado As FontAwesome5.FontAwesome = pagina.FindName("iconoOtroUsuarioVerificado")

                If usuario.Verified = True Then
                    iconoVerificado.Visibility = Visibility.Visible
                Else
                    iconoVerificado.Visibility = Visibility.Collapsed
                End If

                Dim tbUsuario As TextBlock = pagina.FindName("tbOtroUsuarioTweets")
                tbUsuario.Text = usuario.Name

                Dim tbUsuarioPerfil As TextBlock = pagina.FindName("tbOtroUsuarioPerfilNombre")
                tbUsuarioPerfil.Text = usuario.Name

                Dim tbUsuario2Perfil As TextBlock = pagina.FindName("tbOtroUsuarioPerfilScreenNombre")
                tbUsuario2Perfil.Text = "@" + usuario.ScreenName
                tbUsuario2Perfil.Tag = usuario.Id

                Dim tbDescripcionPerfil As TextBlock = pagina.FindName("tbOtroUsuarioPerfilDescripcion")
                tbDescripcionPerfil.Text = usuario.Description

                Dim tbSeguidoresPerfil As TextBlock = pagina.FindName("tbOtroUsuarioPerfilSeguidores")
                tbSeguidoresPerfil.Text = usuario.FollowersCount.ToString("###,###,####")

                Dim spTweets As StackPanel = pagina.FindName("spOtroUsuarioTweets")
                spTweets.Children.Clear()

                Dim gridCarga As Grid = pagina.FindName("gridCargaOtroUsuarioTweets")
                gridCarga.Visibility = Visibility.Visible

                Dim parametros As New Parameters.GetUserTimelineParameters(usuario.Id) With {
                    .PageSize = 20
                }

                Dim tweets As ITweet() = Nothing

                Try
                    tweets = Await cliente.Timelines.GetUserTimelineAsync(parametros)
                Catch ex As Exception

                End Try

                gridCarga.Visibility = Visibility.Collapsed

                If Not tweets Is Nothing Then
                    For Each tweet In tweets
                        Dim añadir As Boolean = True

                        For Each hijo In spTweets.Children
                            Dim grid As Grid = hijo
                            Dim id As Long = grid.Tag

                            If id = tweet.Id Then
                                añadir = False
                            End If
                        Next

                        If añadir = True Then
                            spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                        End If
                    Next
                End If

                Dim svTweets As ScrollViewer = pagina.FindName("svOtroUsuarioTweets")
                svTweets.Tag = cliente
                RemoveHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging
                AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

                Dim botonSubir As Button = pagina.FindName("botonSubirArribaOtroUsuarioTweets")
                botonSubir.Tag = svTweets

                RemoveHandler botonSubir.Click, AddressOf BotonSubirClick
                AddHandler botonSubir.Click, AddressOf BotonSubirClick

                RemoveHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono
                AddHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono

                RemoveHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono
                AddHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono
            End If

        End Sub

        Private Async Sub SvTweets_ViewChanging(sender As Object, e As ScrollViewerViewChangingEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonSubir As Button = pagina.FindName("botonSubirArribaOtroUsuarioTweets")
            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")

            Dim sv As ScrollViewer = sender

            If sv.VerticalOffset > 50 Then
                gridUsuarioBotones.Visibility = Visibility.Collapsed
                botonSubir.Visibility = Visibility.Visible
            Else
                gridUsuarioBotones.Visibility = Visibility.Visible
                botonSubir.Visibility = Visibility.Collapsed
            End If

            Dim prTweets As ProgressRing = pagina.FindName("prOtroUsuarioTweets")

            If prTweets.Visibility = Visibility.Collapsed Then
                If sv.ScrollableHeight < sv.VerticalOffset + 200 Then
                    prTweets.Visibility = Visibility.Visible

                    Dim spTweets As StackPanel = pagina.FindName("spOtroUsuarioTweets")
                    Dim gridUltimoTweet As Grid = spTweets.Children(spTweets.Children.Count - 1)
                    Dim ultimoTweet As Long = gridUltimoTweet.Tag

                    Dim cliente As TwitterClient = sv.Tag

                    Dim tbUsuario2Perfil As TextBlock = pagina.FindName("tbOtroUsuarioPerfilScreenNombre")
                    Dim usuarioId As Long = tbUsuario2Perfil.Tag

                    Dim parametros As New Parameters.GetUserTimelineParameters(usuarioId) With {
                        .PageSize = 40,
                        .MaxId = ultimoTweet
                    }

                    Dim tweets As ITweet() = Nothing

                    Try
                        tweets = Await cliente.Timelines.GetUserTimelineAsync(parametros)
                    Catch ex As Exception

                    End Try

                    If Not tweets Is Nothing Then
                        For Each tweet In tweets
                            Dim añadir As Boolean = True

                            For Each hijo In spTweets.Children
                                Dim grid As Grid = hijo
                                Dim id As Long = grid.Tag

                                If id = tweet.Id Then
                                    añadir = False
                                End If
                            Next

                            If añadir = True Then
                                spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                            End If
                        Next
                    End If

                    prTweets.Visibility = Visibility.Collapsed
                End If
            End If

        End Sub

        Private Sub BotonSubirClick(sender As Object, e As RoutedEventArgs)

            Dim botonSubir As Button = sender
            Dim svTweets As ScrollViewer = botonSubir.Tag

            svTweets.ChangeView(Nothing, 0, Nothing)
            botonSubir.Visibility = Visibility.Collapsed

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")
            gridUsuarioBotones.Visibility = Visibility.Visible

        End Sub

        Private Async Sub BotonSeguirClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim recursos As New Resources.ResourceLoader
            Dim cosas As ClienteyUsuario = boton.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbSeguir As TextBlock = pagina.FindName("tbOtroUsuarioSeguir")

            If tbSeguir.Text = recursos.GetString("Following") Then
                Await cosas.cliente.Users.UnfollowUserAsync(cosas.usuario.Id)
                tbSeguir.Text = recursos.GetString("Follow")
            Else
                Await cosas.cliente.Users.FollowUserAsync(cosas.usuario.Id)
                tbSeguir.Text = recursos.GetString("Following")
            End If

            Dim spTweets As StackPanel = pagina.FindName("spUsuarioTweets")
            spTweets.Children.Clear()

            Dim parametros As New Parameters.GetHomeTimelineParameters With {
                .PageSize = 20
            }

            Dim tweets As ITweet() = Nothing

            Try
                tweets = Await cosas.cliente.Timelines.GetHomeTimelineAsync(parametros)
            Catch ex As Exception

            End Try

            If Not tweets Is Nothing Then
                For Each tweet In tweets
                    spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cosas.cliente, tweet, True))
                Next
            End If

            boton.IsEnabled = True

        End Sub

        Private Async Sub MasOpcionesClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim boton As Button = sender
            Dim cosas As ClienteyUsuario = boton.Tag

            Dim relacion As IRelationshipDetails = Await cosas.cliente.Users.GetRelationshipBetweenAsync(Interfaz.Usuario.usuario_.Id, cosas.usuario.Id)

            Dim menu As New MenuFlyout With {
                .Placement = FlyoutPlacementMode.Bottom,
                .ShowMode = FlyoutShowMode.Transient
            }

            Dim iconoBloquearPerfil As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Lock,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonBloquearPerfil As New MenuFlyoutItem With {
                .Text = recursos.GetString("BlockUser"),
                .Icon = iconoBloquearPerfil,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = cosas
            }

            AddHandler botonBloquearPerfil.Click, AddressOf BloquearPerfilClick
            AddHandler botonBloquearPerfil.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonBloquearPerfil.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonBloquearPerfil)

            Dim iconoDesbloquearPerfil As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Unlock,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonDesbloquearPerfil As New MenuFlyoutItem With {
                .Text = recursos.GetString("UnblockUser"),
                .Icon = iconoDesbloquearPerfil,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = cosas
            }

            AddHandler botonDesbloquearPerfil.Click, AddressOf DesbloquearPerfilClick
            AddHandler botonDesbloquearPerfil.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonDesbloquearPerfil.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonDesbloquearPerfil)

            If relacion.Blocking = True Then
                botonBloquearPerfil.Visibility = Visibility.Collapsed
                botonDesbloquearPerfil.Visibility = Visibility.Visible
            Else
                botonBloquearPerfil.Visibility = Visibility.Visible
                botonDesbloquearPerfil.Visibility = Visibility.Collapsed
            End If

            '-----------------------------------------------------

            Dim separador As New MenuFlyoutSeparator
            menu.Items.Add(separador)

            Dim iconoCompartirPerfil As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_ShareAlt,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonCompartirPerfil As New MenuFlyoutItem With {
                .Text = recursos.GetString("ShareUser"),
                .Icon = iconoCompartirPerfil,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = cosas
            }

            AddHandler botonCompartirPerfil.Click, AddressOf CompartirPerfilClick
            AddHandler botonCompartirPerfil.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonCompartirPerfil.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonCompartirPerfil)

            Dim iconoCopiarEnlaceTweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Copy,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonCopiarEnlaceTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("CopyUrl2"),
                .Icon = iconoCopiarEnlaceTweet,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = cosas
            }

            AddHandler botonCopiarEnlaceTweet.Click, AddressOf CopiarEnlacePerfilClick
            AddHandler botonCopiarEnlaceTweet.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonCopiarEnlaceTweet.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonCopiarEnlaceTweet)

            '-----------------------------------------------------

            Dim separador2 As New MenuFlyoutSeparator
            menu.Items.Add(separador2)

            Dim iconoAbrirNavegadorTweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Brands_Edge,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonAbrirNavegadorTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("OpenWebBrowser"),
                .Icon = iconoAbrirNavegadorTweet,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = cosas
            }

            AddHandler botonAbrirNavegadorTweet.Click, AddressOf AbrirNavegadorUsuarioClick
            AddHandler botonAbrirNavegadorTweet.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonAbrirNavegadorTweet.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonAbrirNavegadorTweet)

            FlyoutBase.SetAttachedFlyout(boton, menu)
            menu.ShowAt(boton)

        End Sub

        Private Async Sub BloquearPerfilClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            boton.IsEnabled = False

            Dim recursos As New Resources.ResourceLoader
            Dim cosas As ClienteyUsuario = boton.Tag

            Dim cliente As TwitterClient = cosas.cliente
            Dim usuario As IUser = cosas.usuario

            Dim resultado As IUser = Await cliente.Users.BlockUserAsync(usuario.Id)

            If Not resultado Is Nothing Then
                boton.Visibility = Visibility.Collapsed

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim botonSeguir As Button = pagina.FindName("botonOtroUsuarioSeguir")
                botonSeguir.Visibility = Visibility.Collapsed

                Dim spTweets As StackPanel = pagina.FindName("spUsuarioTweets")
                spTweets.Children.Clear()

                Dim parametros As New Parameters.GetHomeTimelineParameters With {
                    .PageSize = 20
                }

                Dim tweets As ITweet() = Nothing

                Try
                    tweets = Await cliente.Timelines.GetHomeTimelineAsync(parametros)
                Catch ex As Exception

                End Try

                If Not tweets Is Nothing Then
                    For Each tweet In tweets
                        spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                    Next
                End If

                Dim spOtroUsuarioTweets As StackPanel = pagina.FindName("spOtroUsuarioTweets")
                spOtroUsuarioTweets.Children.Clear()
            End If

            boton.IsEnabled = True

        End Sub

        Private Async Sub DesbloquearPerfilClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            boton.IsEnabled = False

            Dim recursos As New Resources.ResourceLoader
            Dim cosas As ClienteyUsuario = boton.Tag

            Dim cliente As TwitterClient = cosas.cliente
            Dim usuario As IUser = cosas.usuario

            Dim resultado As IUser = Await cliente.Users.UnblockUserAsync(usuario.Id)

            If Not resultado Is Nothing Then
                boton.Visibility = Visibility.Collapsed

                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim botonSeguir As Button = pagina.FindName("botonOtroUsuarioSeguir")
                botonSeguir.Visibility = Visibility.Visible

                Dim tbSeguir As TextBlock = pagina.FindName("tbOtroUsuarioSeguir")
                tbSeguir.Text = recursos.GetString("Follow")

                Dim spTweets As StackPanel = pagina.FindName("spUsuarioTweets")
                spTweets.Children.Clear()

                Dim parametros As New Parameters.GetHomeTimelineParameters With {
                    .PageSize = 20
                }

                Dim tweets As ITweet() = Nothing

                Try
                    tweets = Await cliente.Timelines.GetHomeTimelineAsync(parametros)
                Catch ex As Exception

                End Try

                If Not tweets Is Nothing Then
                    For Each tweet In tweets
                        spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                    Next
                End If

                Dim spOtroUsuarioTweets As StackPanel = pagina.FindName("spOtroUsuarioTweets")
                spOtroUsuarioTweets.Children.Clear()

                Dim gridCarga As Grid = pagina.FindName("gridCargaOtroUsuarioTweets")
                gridCarga.Visibility = Visibility.Visible

                Dim parametros2 As New Parameters.GetUserTimelineParameters(usuario.Id) With {
                    .PageSize = 20
                }

                Dim tweets2 As ITweet() = Nothing

                Try
                    tweets2 = Await cliente.Timelines.GetUserTimelineAsync(parametros2)
                Catch ex As Exception

                End Try

                gridCarga.Visibility = Visibility.Collapsed

                If Not tweets2 Is Nothing Then
                    For Each tweet In tweets2
                        Dim añadir As Boolean = True

                        For Each hijo In spOtroUsuarioTweets.Children
                            Dim grid As Grid = hijo
                            Dim id As Long = grid.Tag

                            If id = tweet.Id Then
                                añadir = False
                            End If
                        Next

                        If añadir = True Then
                            spOtroUsuarioTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                        End If
                    Next
                End If
            End If

            boton.IsEnabled = True

        End Sub

        Private Sub CompartirPerfilClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As ClienteyUsuario = boton.Tag

            ApplicationData.Current.LocalSettings.Values("PerfilCompartirTitulo") = "@" + cosas.usuario.ScreenName
            ApplicationData.Current.LocalSettings.Values("PerfilCompartirDescripcion") = WebUtility.HtmlDecode(cosas.usuario.Description)
            ApplicationData.Current.LocalSettings.Values("PerfilCompartirEnlace") = "https://twitter.com/" + cosas.usuario.ScreenName

            Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
            AddHandler datos.DataRequested, AddressOf DatosCompartirPerfilClick

            DataTransferManager.ShowShareUI()

        End Sub

        Private Sub DatosCompartirPerfilClick(sender As Object, e As DataRequestedEventArgs)

            Dim request As DataRequest = e.Request

            request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("PerfilCompartirTitulo")
            request.Data.Properties.Description = ApplicationData.Current.LocalSettings.Values("PerfilCompartirDescripcion")
            request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("PerfilCompartirEnlace")))

        End Sub

        Private Sub CopiarEnlacePerfilClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As ClienteyUsuario = boton.Tag

            Dim texto As New DataPackage
            texto.SetText("https://twitter.com/" + cosas.usuario.ScreenName)
            Clipboard.SetContent(texto)

        End Sub

        Private Async Sub AbrirNavegadorUsuarioClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As ClienteyUsuario = boton.Tag

            Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/" + cosas.usuario.ScreenName))

        End Sub

    End Module

    Public Class ClienteyUsuario

        Public cliente As TwitterClient
        Public usuario As IUser

        Public Sub New(ByVal cliente As TwitterClient, ByVal usuario As IUser)
            Me.cliente = cliente
            Me.usuario = usuario
        End Sub

    End Class

End Namespace