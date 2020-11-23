Imports Tweetinvi
Imports Tweetinvi.Events
Imports Tweetinvi.Models
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.System.Threading
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module Inicio

        Public cliente_ As TwitterClient
        Public usuario_ As IAuthenticatedUser

        Public Async Sub CargarDatos(cliente As TwitterClient, usuario As IAuthenticatedUser)

            cliente_ = cliente
            usuario_ = usuario

            Enviar.Cargar()
            Menciones.Cargar()
            Busqueda.Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuario As Grid = pagina.FindName("gridUsuario")
            Pestañas.Visibilidad_Pestañas(gridUsuario)

            Dim botonTweets As Button = pagina.FindName("botonUsuarioTweets")
            Dim gridTweets As Grid = pagina.FindName("gridUsuarioTweets")
            Pestañas.Visibilidad_Pestañas_Usuario(botonTweets, gridTweets)

            Dim nvItemCuenta As NavigationViewItem = pagina.FindName("nvItemCuenta")
            nvItemCuenta.Visibility = Visibility.Visible

            Dim imagenCuenta As New ImageBrush With {
                .ImageSource = New BitmapImage(New Uri(usuario.ProfileImageUrl))
            }

            Dim avatar As Ellipse = pagina.FindName("elipseCuentaSeleccionada")
            avatar.Fill = imagenCuenta

            Dim tbCuenta As TextBlock = pagina.FindName("tbCuentaSeleccionada")
            tbCuenta.Text = usuario.Name

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
                    Dim añadir As Boolean = True

                    If ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 0 Then
                        If tweet.CreatedBy.Id = usuario_.Id Then
                            añadir = False
                        End If
                    End If

                    If añadir = True Then
                        spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                    End If
                Next
            End If

            Dim spActualizarUsuarioTweets As StackPanel = pagina.FindName("spActualizarUsuarioTweets")

            If spTweets.Children.Count = 0 Then
                spActualizarUsuarioTweets.Visibility = Visibility.Visible
            Else
                spActualizarUsuarioTweets.Visibility = Visibility.Collapsed
            End If

            Dim botonActualizarUsuarioTweets As Button = pagina.FindName("botonActualizarUsuarioTweets")
            botonActualizarUsuarioTweets.Tag = cliente

            RemoveHandler botonActualizarUsuarioTweets.Click, AddressOf ActualizarTweets2
            AddHandler botonActualizarUsuarioTweets.Click, AddressOf ActualizarTweets2

            RemoveHandler botonActualizarUsuarioTweets.PointerEntered, AddressOf Entra_Boton_IconoTexto
            AddHandler botonActualizarUsuarioTweets.PointerEntered, AddressOf Entra_Boton_IconoTexto

            RemoveHandler botonActualizarUsuarioTweets.PointerExited, AddressOf Sale_Boton_IconoTexto
            AddHandler botonActualizarUsuarioTweets.PointerExited, AddressOf Sale_Boton_IconoTexto

            Dim svTweets As ScrollViewer = pagina.FindName("svUsuarioTweets")
            svTweets.Tag = cliente
            RemoveHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging
            AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

            Dim botonSubir As Button = pagina.FindName("botonSubirArribaUsuarioTweets")
            botonSubir.Tag = svTweets

            RemoveHandler botonSubir.Click, AddressOf BotonSubirClick
            AddHandler botonSubir.Click, AddressOf BotonSubirClick

            RemoveHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono

            Dim stream As Core.Streaming.ITweetStream = cliente.Streams.CreateTweetStream
            'Await stream.StartAsync("https://stream.twitter.com/1.1/statuses/sample.json")

            RemoveHandler stream.TweetDeleted, AddressOf BorrarTweet
            AddHandler stream.TweetDeleted, AddressOf BorrarTweet

            Dim periodoHome As TimeSpan = TimeSpan.FromSeconds(70)
            Dim contadorHome As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                          Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                                                                                           ActualizarTweets(cliente)
                                                                                                                                                                                       End Sub)
                                                                                      End Sub, periodoHome)


        End Sub

        Private Async Sub ActualizarTweets(cliente As TwitterClient)

            Dim config As ApplicationDataContainer = ApplicationData.Current.LocalSettings

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spTweets As StackPanel = pagina.FindName("spUsuarioTweets")

            Dim parametros As New Parameters.GetHomeTimelineParameters With {
                .PageSize = 20
            }

            Dim tweets As ITweet() = Nothing

            Try
                tweets = Await cliente.Timelines.GetHomeTimelineAsync(parametros)
            Catch ex As Exception

            End Try

            Dim i As Integer = 0
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

                    If ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 0 Then
                        If tweet.CreatedBy.Id = usuario_.Id Then
                            añadir = False
                        End If
                    End If

                    If añadir = True Then
                        spTweets.Children.Insert(i, Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                        i += 1

                        Dim mostrar As Boolean = True

                        If tweet.CreatedBy.Id = usuario_.Id Then
                            mostrar = False
                        End If

                        If mostrar = True Then
                            If config.Values("Estado_App") = 1 Then
                                Notificaciones.ToastTweet(tweet)
                            End If
                        End If
                    End If
                Next
            End If

        End Sub

        Private Async Sub ActualizarTweets2(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim cliente As TwitterClient = boton.Tag

            Dim spTweets As StackPanel = pagina.FindName("spUsuarioTweets")

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
                    Dim añadir As Boolean = True

                    For Each hijo In spTweets.Children
                        Dim grid As Grid = hijo
                        Dim id As Long = grid.Tag

                        If id = tweet.Id Then
                            añadir = False
                        End If
                    Next

                    If ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 0 Then
                        If tweet.CreatedBy.Id = usuario_.Id Then
                            añadir = False
                        End If
                    End If

                    If añadir = True Then
                        spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                    End If
                Next
            End If

            If spTweets.Children.Count > 0 Then
                Dim spActualizarUsuarioTweets As StackPanel = pagina.FindName("spActualizarUsuarioTweets")
                spActualizarUsuarioTweets.Visibility = Visibility.Collapsed
            End If

            boton.IsEnabled = True

        End Sub

        Private Sub BorrarTweet(sender As Object, e As TweetDeletedEvent)

            Notificaciones.Toast("Test Tweet Deleted: " + e.TweetId.ToString)

        End Sub

        Private Async Sub SvTweets_ViewChanging(sender As Object, e As ScrollViewerViewChangingEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonSubir As Button = pagina.FindName("botonSubirArribaUsuarioTweets")
            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")

            Dim sv As ScrollViewer = sender

            If sv.VerticalOffset > 50 Then
                gridUsuarioBotones.Visibility = Visibility.Collapsed
                botonSubir.Visibility = Visibility.Visible
            Else
                gridUsuarioBotones.Visibility = Visibility.Visible
                botonSubir.Visibility = Visibility.Collapsed
            End If

            Dim prUsuarioTweets As ProgressRing = pagina.FindName("prUsuarioTweets")

            If prUsuarioTweets.Visibility = Visibility.Collapsed Then
                If sv.ScrollableHeight < sv.VerticalOffset + 200 Then
                    prUsuarioTweets.Visibility = Visibility.Visible

                    Dim spTweets As StackPanel = pagina.FindName("spUsuarioTweets")
                    Dim gridUltimoTweet As Grid = spTweets.Children(spTweets.Children.Count - 1)
                    Dim ultimoTweet As Long = gridUltimoTweet.Tag

                    Dim parametros As New Parameters.GetHomeTimelineParameters With {
                        .PageSize = 40,
                        .MaxId = ultimoTweet
                    }

                    Dim cliente As TwitterClient = sv.Tag
                    Dim tweets As ITweet() = Nothing

                    Try
                        tweets = Await cliente.Timelines.GetHomeTimelineAsync(parametros)
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

                            If ApplicationData.Current.LocalSettings.Values("inicio_usuario") = 0 Then
                                If tweet.CreatedBy.Id = usuario_.Id Then
                                    añadir = False
                                End If
                            End If

                            If añadir = True Then
                                spTweets.Children.Add(Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                            End If
                        Next
                    End If

                    prUsuarioTweets.Visibility = Visibility.Collapsed
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

