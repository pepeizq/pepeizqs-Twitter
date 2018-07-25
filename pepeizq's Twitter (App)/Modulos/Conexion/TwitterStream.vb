Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.System.Threading
Imports Windows.UI.Core

Module TwitterStream

    Public Async Sub Iniciar(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim listaBloqueos As New List(Of String)

        listaBloqueos = Await TwitterPeticiones.CogerListaBloqueos(listaBloqueos, megaUsuario)

        If listaBloqueos.Count > 0 Then
            megaUsuario.UsuariosBloqueados = listaBloqueos
        End If

        Dim listaMuteados As New List(Of String)

        listaMuteados = Await TwitterPeticiones.CogerListaMuteados(listaMuteados, megaUsuario)

        If listaMuteados.Count > 0 Then
            megaUsuario.UsuariosMuteados = listaMuteados
        End If

        '----------------------------------

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvInicio As ListView = pagina.FindName("lvTweetsInicio" + usuario.ID)

        If Not lvInicio Is Nothing Then
            Dim periodoHome As TimeSpan = TimeSpan.FromSeconds(70)
            Dim contadorHome As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
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
                                                                                                                                                                                                    Dim segundos As Integer = 0

                                                                                                                                                                                                    If ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempo") = True Then
                                                                                                                                                                                                        segundos = ApplicationData.Current.LocalSettings.Values("notificacionInicioTiempoSegundos")
                                                                                                                                                                                                    End If

                                                                                                                                                                                                    If megaUsuario.NotificacionInicio = True Then
                                                                                                                                                                                                        If listaTweetsAñadir.Count > 1 Then
                                                                                                                                                                                                            listaTweetsAñadir.Reverse()

                                                                                                                                                                                                            If lvInicio.Items.Count > 0 Then
                                                                                                                                                                                                                If ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar") = True Then
                                                                                                                                                                                                                    Notificaciones.ToastTweets(listaTweetsAñadir.Count, megaUsuario, segundos, 0)
                                                                                                                                                                                                                End If
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            For Each tweetAñadir As Tweet In listaTweetsAñadir
                                                                                                                                                                                                                If lvInicio.Items.Count > 0 Then
                                                                                                                                                                                                                    If ApplicationData.Current.LocalSettings.Values("notificacionInicioAgrupar") = False Then
                                                                                                                                                                                                                        Notificaciones.ToastTweet(tweetAñadir, megaUsuario, segundos)
                                                                                                                                                                                                                    End If
                                                                                                                                                                                                                End If

                                                                                                                                                                                                                AñadirTweet(tweetAñadir, lvInicio, megaUsuario)
                                                                                                                                                                                                            Next
                                                                                                                                                                                                        Else
                                                                                                                                                                                                            If lvInicio.Items.Count > 0 Then
                                                                                                                                                                                                                Notificaciones.ToastTweet(listaTweetsAñadir(0), megaUsuario, segundos)
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            AñadirTweet(listaTweetsAñadir(0), lvInicio, megaUsuario)
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    End If
                                                                                                                                                                                                End If
                                                                                                                                                                                            End If
                                                                                                                                                                                        End Sub))
                                                                                      End Sub, periodoHome)
            megaUsuario.StreamHome = contadorHome
        End If

        Dim lvMenciones As ListView = pagina.FindName("lvTweetsMenciones" + usuario.ID)

        If Not lvMenciones Is Nothing Then
            Dim periodoMentions As TimeSpan = TimeSpan.FromSeconds(20)
            Dim contadorMentions As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                              Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Async Sub()
                                                                                                                                                                                                Dim listaTweets As New List(Of Tweet)

                                                                                                                                                                                                listaTweets = Await TwitterPeticiones.MentionsTimeline(listaTweets, megaUsuario, Nothing)

                                                                                                                                                                                                If listaTweets.Count > 0 Then
                                                                                                                                                                                                    Dim listaTweetsAñadir As New List(Of Tweet)

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
                                                                                                                                                                                                            listaTweetsAñadir.Add(tweetNuevo)
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    Next

                                                                                                                                                                                                    If listaTweetsAñadir.Count > 0 Then
                                                                                                                                                                                                        Dim segundos As Integer = 0

                                                                                                                                                                                                        If ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempo") = True Then
                                                                                                                                                                                                            segundos = ApplicationData.Current.LocalSettings.Values("notificacionMencionesTiempoSegundos")
                                                                                                                                                                                                        End If

                                                                                                                                                                                                        If megaUsuario.NotificacionMenciones = True Then
                                                                                                                                                                                                            If listaTweetsAñadir.Count > 1 Then
                                                                                                                                                                                                                listaTweetsAñadir.Reverse()

                                                                                                                                                                                                                If lvMenciones.Items.Count > 0 Then
                                                                                                                                                                                                                    If ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar") = True Then
                                                                                                                                                                                                                        Notificaciones.ToastTweets(listaTweetsAñadir.Count, megaUsuario, segundos, 1)
                                                                                                                                                                                                                    End If
                                                                                                                                                                                                                End If

                                                                                                                                                                                                                For Each tweetAñadir As Tweet In listaTweetsAñadir
                                                                                                                                                                                                                    If lvMenciones.Items.Count > 0 Then
                                                                                                                                                                                                                        If ApplicationData.Current.LocalSettings.Values("notificacionMencionesAgrupar") = False Then
                                                                                                                                                                                                                            Notificaciones.ToastTweet(tweetAñadir, megaUsuario, segundos)
                                                                                                                                                                                                                        End If
                                                                                                                                                                                                                    End If

                                                                                                                                                                                                                    AñadirTweet(tweetAñadir, lvMenciones, megaUsuario)
                                                                                                                                                                                                                Next
                                                                                                                                                                                                            Else
                                                                                                                                                                                                                If lvMenciones.Items.Count > 0 Then
                                                                                                                                                                                                                    Notificaciones.ToastTweet(listaTweetsAñadir(0), megaUsuario, segundos)
                                                                                                                                                                                                                End If

                                                                                                                                                                                                                AñadirTweet(listaTweetsAñadir(0), lvMenciones, megaUsuario)
                                                                                                                                                                                                            End If
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    End If
                                                                                                                                                                                                End If
                                                                                                                                                                                            End Sub))
                                                                                          End Sub, periodoMentions)
            megaUsuario.StreamMentions = contadorMentions
        End If

    End Sub

    Private Sub AñadirTweet(tweet As Tweet, lv As ListView, megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim boolAñadir As Boolean = True

        'If lv.Items.Count > 0 Then
        '    For Each item As ListViewItem In lv.Items
        '        Dim tweetViejo As Tweet = item.Tag

        '        If tweetViejo.ID = tweet.ID Then
        '            boolAñadir = False
        '        End If
        '    Next
        'End If

        If boolAñadir = True Then
            lv.Items.Insert(0, pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, megaUsuario, Nothing))
        End If
    End Sub

End Module
