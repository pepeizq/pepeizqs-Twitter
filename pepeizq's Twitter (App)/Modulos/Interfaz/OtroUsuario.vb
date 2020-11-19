Imports Tweetinvi
Imports Tweetinvi.Models
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

            Dim cliente As TwitterClient = Interfaz.cliente_

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
                            spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet))
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
                                spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet))
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

    End Module
End Namespace