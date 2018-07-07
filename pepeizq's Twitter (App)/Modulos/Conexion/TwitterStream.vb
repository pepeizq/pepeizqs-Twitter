Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.Core
Imports Windows.ApplicationModel.Store
Imports Windows.Storage
Imports Windows.System.Threading
Imports Windows.UI.Core

Module TwitterStream

    Public Sub Iniciar(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        '------------------

        Dim gridInicio As Grid = pagina.FindName("gridTweets" + usuario.ID)
        Dim lvInicio As New ListView

        If Not gridInicio Is Nothing Then
            Dim sv As ScrollViewer = gridInicio.Children(1)
            lvInicio = sv.Content
        Else
            lvInicio = Nothing
        End If

        '------------------

        Dim gridMenciones As Grid = pagina.FindName("gridMenciones" + usuario.ID)
        Dim lvMenciones As New ListView

        If Not gridMenciones Is Nothing Then
            Dim sv As ScrollViewer = gridMenciones.Children(0)
            lvMenciones = sv.Content
        Else
            lvMenciones = Nothing
        End If

        '------------------

        Dim periodoHome As TimeSpan = TimeSpan.FromSeconds(70)
        Dim contadorHome As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                      Try
                                                                                          Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Async Sub()
                                                                                                                                                                                            Dim listaTweets As New List(Of Tweet)

                                                                                                                                                                                            listaTweets = Await TwitterPeticiones.HomeTimeline(listaTweets, megaUsuario, Nothing)

                                                                                                                                                                                            If listaTweets.Count > 0 Then
                                                                                                                                                                                                Dim listaTweetsAñadir As New List(Of Tweet)

                                                                                                                                                                                                For Each tweetNuevo As Tweet In listaTweets
                                                                                                                                                                                                    Dim mostrar As Boolean = True

                                                                                                                                                                                                    For Each item In lvInicio.Items
                                                                                                                                                                                                        If TypeOf item Is ListViewItem Then
                                                                                                                                                                                                            Dim lvItem As ListViewItem = item
                                                                                                                                                                                                            Dim grid As Grid = lvItem.Content
                                                                                                                                                                                                            Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = grid.Tag
                                                                                                                                                                                                            Dim itemTweet As Tweet = tweetAmpliado.Tweet

                                                                                                                                                                                                            If itemTweet.ID = tweetNuevo.ID Then
                                                                                                                                                                                                                mostrar = False
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            If Not tweetNuevo.RespuestaUsuarioScreenNombre = Nothing Then
                                                                                                                                                                                                                mostrar = False
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            If Not tweetNuevo.Retweet Is Nothing Then
                                                                                                                                                                                                                If tweetNuevo.Retweet.Usuario.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
                                                                                                                                                                                                                    mostrar = False
                                                                                                                                                                                                                End If
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    Next

                                                                                                                                                                                                    If mostrar = True Then
                                                                                                                                                                                                        listaTweetsAñadir.Add(tweetNuevo)
                                                                                                                                                                                                    End If
                                                                                                                                                                                                Next

                                                                                                                                                                                                If listaTweetsAñadir.Count > 0 Then
                                                                                                                                                                                                    If listaTweetsAñadir.Count > 1 Then
                                                                                                                                                                                                        listaTweetsAñadir.Reverse()
                                                                                                                                                                                                    End If

                                                                                                                                                                                                    Dim segundos As Integer = 0

                                                                                                                                                                                                    If ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo") = True Then
                                                                                                                                                                                                        segundos = ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos")
                                                                                                                                                                                                    End If

                                                                                                                                                                                                    If megaUsuario.NotificacionInicio = True Then
                                                                                                                                                                                                        If ApplicationData.Current.LocalSettings.Values("notificacionAgrupar") = True Then
                                                                                                                                                                                                            If listaTweetsAñadir.Count > 1 Then
                                                                                                                                                                                                                Notificaciones.ToastTweets(listaTweetsAñadir.Count, megaUsuario, segundos)
                                                                                                                                                                                                            Else
                                                                                                                                                                                                                Notificaciones.ToastTweet(listaTweetsAñadir(0), megaUsuario, segundos)
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    End If

                                                                                                                                                                                                    For Each tweetAñadir As Tweet In listaTweetsAñadir
                                                                                                                                                                                                        lvInicio.Items.Insert(0, pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweetAñadir, megaUsuario, Nothing))

                                                                                                                                                                                                        If megaUsuario.NotificacionInicio = True Then
                                                                                                                                                                                                            If ApplicationData.Current.LocalSettings.Values("notificacionAgrupar") = False Then
                                                                                                                                                                                                                Notificaciones.ToastTweet(tweetAñadir, megaUsuario, segundos)
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        End If

                                                                                                                                                                                                        For Each item In lvInicio.Items
                                                                                                                                                                                                            If TypeOf item Is Grid Then
                                                                                                                                                                                                                Dim grid As Grid = item

                                                                                                                                                                                                                If grid.Name.Contains("gridAnuncio") Then
                                                                                                                                                                                                                    lvInicio.Items.Remove(item)
                                                                                                                                                                                                                End If
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        Next

                                                                                                                                                                                                        Dim licencia As LicenseInformation = Nothing

                                                                                                                                                                                                        Try
                                                                                                                                                                                                            licencia = CurrentApp.LicenseInformation
                                                                                                                                                                                                        Catch ex As Exception

                                                                                                                                                                                                        End Try

                                                                                                                                                                                                        If Not licencia Is Nothing Then
                                                                                                                                                                                                            If Not licencia.ProductLicenses("NoAds").IsActive Then
                                                                                                                                                                                                                lvInicio.Items.Insert(2, AñadirAnuncio("1100022916"))
                                                                                                                                                                                                                lvInicio.Items.Insert(6, AñadirAnuncio("1100022920"))
                                                                                                                                                                                                                lvInicio.Items.Insert(11, AñadirAnuncio("1100022962"))
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        Else
                                                                                                                                                                                                            lvInicio.Items.Insert(2, AñadirAnuncio("1100022916"))
                                                                                                                                                                                                            lvInicio.Items.Insert(6, AñadirAnuncio("1100022920"))
                                                                                                                                                                                                            lvInicio.Items.Insert(11, AñadirAnuncio("1100022962"))
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    Next
                                                                                                                                                                                                End If
                                                                                                                                                                                            End If
                                                                                                                                                                                        End Sub))
                                                                                      Catch ex As Exception

                                                                                      End Try
                                                                                  End Sub, periodoHome)
        megaUsuario.StreamHome = contadorHome

        Dim periodoMentions As TimeSpan = TimeSpan.FromSeconds(20)
        Dim contadorMentions As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                          Try
                                                                                              Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Async Sub()
                                                                                                                                                                                                Dim listaTweets As New List(Of Tweet)

                                                                                                                                                                                                listaTweets = Await TwitterPeticiones.MentionsTimeline(listaTweets, megaUsuario, Nothing)

                                                                                                                                                                                                If listaTweets.Count > 0 Then
                                                                                                                                                                                                    For Each tweetNuevo As Tweet In listaTweets
                                                                                                                                                                                                        Dim mostrar As Boolean = True

                                                                                                                                                                                                        For Each item In lvMenciones.Items
                                                                                                                                                                                                            If TypeOf item Is ListViewItem Then
                                                                                                                                                                                                                Dim lvItem As ListViewItem = item
                                                                                                                                                                                                                Dim grid As Grid = lvItem.Content
                                                                                                                                                                                                                Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = grid.Tag
                                                                                                                                                                                                                Dim itemTweet As Tweet = tweetAmpliado.Tweet

                                                                                                                                                                                                                If itemTweet.ID = tweetNuevo.ID Then
                                                                                                                                                                                                                    mostrar = False
                                                                                                                                                                                                                End If
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        Next

                                                                                                                                                                                                        If mostrar = True Then
                                                                                                                                                                                                            lvMenciones.Items.Insert(0, pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweetNuevo, megaUsuario, Nothing))

                                                                                                                                                                                                            If megaUsuario.NotificacionMenciones = True Then
                                                                                                                                                                                                                Dim segundos As Integer = 0

                                                                                                                                                                                                                If ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo") = True Then
                                                                                                                                                                                                                    segundos = ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos")
                                                                                                                                                                                                                End If

                                                                                                                                                                                                                Notificaciones.ToastTweet(tweetNuevo, megaUsuario, segundos)
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    Next
                                                                                                                                                                                                End If
                                                                                                                                                                                            End Sub))
                                                                                          Catch ex As Exception

                                                                                          End Try
                                                                                      End Sub, periodoMentions)
        megaUsuario.StreamMentions = contadorMentions

        Dim periodoBloqueos As TimeSpan = TimeSpan.FromMinutes(2)
        Dim contadorBloqueos As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                          Try
                                                                                              Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Async Sub()
                                                                                                                                                                                                Dim listaBloqueos As New List(Of String)

                                                                                                                                                                                                listaBloqueos = Await TwitterPeticiones.CogerListaBloqueos(listaBloqueos, megaUsuario)

                                                                                                                                                                                                If listaBloqueos.Count > 0 Then
                                                                                                                                                                                                    megaUsuario.UsuariosBloqueados = listaBloqueos
                                                                                                                                                                                                End If
                                                                                                                                                                                            End Sub))
                                                                                          Catch ex As Exception

                                                                                          End Try
                                                                                      End Sub, periodoBloqueos)

        Dim periodoMuteos As TimeSpan = TimeSpan.FromMinutes(2)
        Dim contadorMuteos As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                        Try
                                                                                            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Async Sub()
                                                                                                                                                                                              Dim listaMuteados As New List(Of String)

                                                                                                                                                                                              listaMuteados = Await TwitterPeticiones.CogerListaMuteados(listaMuteados, megaUsuario)

                                                                                                                                                                                              If listaMuteados.Count > 0 Then
                                                                                                                                                                                                  megaUsuario.UsuariosMuteados = listaMuteados
                                                                                                                                                                                              End If
                                                                                                                                                                                          End Sub))
                                                                                        Catch ex As Exception

                                                                                        End Try
                                                                                    End Sub, periodoMuteos)

    End Sub

End Module
