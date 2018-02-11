Imports pepeizq.Twitter
Imports pepeizq.Twitter.Stream
Imports pepeizq.Twitter.Tweet
Imports Windows.Storage
Imports Windows.UI.Core

Module TwitterStream

    Public Async Sub Iniciar(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
        Dim gridUsuario As New Grid

        For Each item In gridPrincipal.Children
            Dim grid As Grid = item

            If grid.Name = "gridUsuario" + usuario.ScreenNombre Then
                gridUsuario = grid
            End If
        Next

        '------------------

        Dim gridInicio As Grid = gridUsuario.Children(1)
        Dim lvInicio As New ListView

        If Not gridInicio Is Nothing Then
            Dim sv As ScrollViewer = gridInicio.Children(1)
            lvInicio = sv.Content
        Else
            lvInicio = Nothing
        End If

        '------------------

        Dim gridMenciones As Grid = gridUsuario.Children(2)
        Dim lvMenciones As New ListView

        If Not gridMenciones Is Nothing Then
            Dim sv As ScrollViewer = gridMenciones.Children(0)
            lvMenciones = sv.Content
        Else
            lvMenciones = Nothing
        End If

        '------------------

        Try
            Await megaUsuario.Servicio.ArrancarStreamUsuario(megaUsuario.Usuario.Tokens, Async Sub(tweet_)
                                                                                             Await Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Low, Async Sub()

                                                                                                                                                                                                'If TypeOf tweet_ Is TwitterStreamEvento Then
                                                                                                                                                                                                '    Dim evento As TwitterStreamEvento = tweet_

                                                                                                                                                                                                '    Notificaciones.Toast.Enseñar(evento.EventoTipo.ToString)
                                                                                                                                                                                                'End If

                                                                                                                                                                                                If TypeOf tweet_ Is TwitterStreamEventoBorrar Then
                                                                                                                                                                                                    Dim borrar As TwitterStreamEventoBorrar = tweet_

                                                                                                                                                                                                    If Not borrar Is Nothing Then
                                                                                                                                                                                                        Dim i As Integer = 0

                                                                                                                                                                                                        For Each item In lvInicio.Items
                                                                                                                                                                                                            Dim lvitem As ListViewItem = item
                                                                                                                                                                                                            Dim grid As Grid = lvitem.Content
                                                                                                                                                                                                            Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = grid.Tag
                                                                                                                                                                                                            Dim tweet As Tweet = tweetAmpliado.Tweet

                                                                                                                                                                                                            If borrar.Id = tweet.ID Then
                                                                                                                                                                                                                lvInicio.Items.RemoveAt(i)
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            i += 1
                                                                                                                                                                                                        Next

                                                                                                                                                                                                        For Each item In lvMenciones.Items
                                                                                                                                                                                                            Dim lvitem As ListViewItem = item
                                                                                                                                                                                                            Dim grid As Grid = lvitem.Content
                                                                                                                                                                                                            Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = grid.Tag
                                                                                                                                                                                                            Dim lvTweet As Tweet = tweetAmpliado.Tweet

                                                                                                                                                                                                            If borrar.Id = lvTweet.ID Then
                                                                                                                                                                                                                lvMenciones.Items.RemoveAt(i)
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            i += 1
                                                                                                                                                                                                        Next
                                                                                                                                                                                                    End If
                                                                                                                                                                                                End If

                                                                                                                                                                                                If TypeOf tweet_ Is Tweet Then
                                                                                                                                                                                                    Dim tweet As Tweet = tweet_

                                                                                                                                                                                                    If Not tweet Is Nothing Then
                                                                                                                                                                                                        Dim mostrar As Boolean = True

                                                                                                                                                                                                        For Each item In lvInicio.Items
                                                                                                                                                                                                            Dim lvitem As ListViewItem = item
                                                                                                                                                                                                            Dim grid As Grid = lvitem.Content
                                                                                                                                                                                                            Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = grid.Tag
                                                                                                                                                                                                            Dim itemTweet As Tweet = tweetAmpliado.Tweet

                                                                                                                                                                                                            If itemTweet.ID = tweet.ID Then
                                                                                                                                                                                                                mostrar = False
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            If Not tweet.RespuestaUsuarioScreenNombre = Nothing Then
                                                                                                                                                                                                                mostrar = False
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            If Not tweet.Retweet Is Nothing Then
                                                                                                                                                                                                                If tweet.Retweet.Usuario.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
                                                                                                                                                                                                                    mostrar = False
                                                                                                                                                                                                                End If
                                                                                                                                                                                                            End If

                                                                                                                                                                                                        Next

                                                                                                                                                                                                        If mostrar = True Then
                                                                                                                                                                                                            Dim tweetNuevo As Tweet = Await megaUsuario.Servicio.Provider.CogerTweet(megaUsuario.Usuario.Tokens, tweet.ID, New TweetParserIndividual)

                                                                                                                                                                                                            If Not ApplicationData.Current.LocalSettings.Values("notificacion") Is Nothing Then
                                                                                                                                                                                                                If Not ApplicationData.Current.LocalSettings.Values("notificacion") = False Then
                                                                                                                                                                                                                    Notificaciones.ToastTweet.Enseñar(tweetNuevo)
                                                                                                                                                                                                                End If
                                                                                                                                                                                                            End If

                                                                                                                                                                                                            lvInicio.Items.Insert(0, TweetXaml.Añadir(tweetNuevo, megaUsuario, Nothing))
                                                                                                                                                                                                        End If
                                                                                                                                                                                                    End If
                                                                                                                                                                                                End If
                                                                                                                                                                                            End Sub)
                                                                                         End Sub)
        Catch ex As Exception
            megaUsuario.Servicio.PararStreamUsuario()
            Iniciar(megaUsuario)
        End Try

    End Sub

End Module
