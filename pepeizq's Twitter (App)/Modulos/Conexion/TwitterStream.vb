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

        '------------------

        Dim gridInicio As Grid = pagina.FindName("gridTweets" + usuario.ScreenNombre)
        Dim lvInicio As New ListView

        If Not gridInicio Is Nothing Then
            Dim sv As ScrollViewer = gridInicio.Children(1)
            lvInicio = sv.Content
        Else
            lvInicio = Nothing
        End If

        '------------------

        Dim gridMenciones As Grid = pagina.FindName("gridMenciones" + usuario.ScreenNombre)
        Dim lvMenciones As New ListView

        If Not gridMenciones Is Nothing Then
            Dim sv As ScrollViewer = gridMenciones.Children(0)
            lvMenciones = sv.Content
        Else
            lvMenciones = Nothing
        End If

        '------------------

        Await megaUsuario.Servicio.ArrancarStreamUsuario(megaUsuario.Usuario.Tokens, Async Sub(cosa)
                                                                                         Await Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()

                                                                                                                                                                                               'If TypeOf tweet_ Is TwitterStreamEvento Then
                                                                                                                                                                                               '    Dim evento As TwitterStreamEvento = tweet_

                                                                                                                                                                                               '    Notificaciones.Toast.Enseñar(evento.EventoTipo.ToString)
                                                                                                                                                                                               'End If

                                                                                                                                                                                               If TypeOf cosa Is TwitterStreamEventoBorrar Then
                                                                                                                                                                                                   Dim borrar As TwitterStreamEventoBorrar = cosa

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

                                                                                                                                                                                               If TypeOf cosa Is Tweet Then
                                                                                                                                                                                                   Dim tweet As Tweet = cosa

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
                                                                                                                                                                                                           Dim tweetNuevo As Tweet = Nothing

                                                                                                                                                                                                           Try
                                                                                                                                                                                                               tweetNuevo = Await megaUsuario.Servicio.Provider.CogerTweet(megaUsuario.Usuario.Tokens, tweet.ID, New TweetParserIndividual)

                                                                                                                                                                                                           Catch ex As Exception

                                                                                                                                                                                                           End Try

                                                                                                                                                                                                           If Not tweetNuevo Is Nothing Then
                                                                                                                                                                                                               lvInicio.Items.Insert(0, pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweetNuevo, megaUsuario, Nothing))

                                                                                                                                                                                                               If megaUsuario.Notificacion = True Then
                                                                                                                                                                                                                   Notificaciones.ToastTweet.Enseñar(tweetNuevo, megaUsuario.Usuario)
                                                                                                                                                                                                               End If
                                                                                                                                                                                                           End If
                                                                                                                                                                                                       End If
                                                                                                                                                                                                   End If
                                                                                                                                                                                               End If
                                                                                                                                                                                           End Sub)
                                                                                     End Sub)
        'Try
        'Catch ex As Exception
        '    megaUsuario.Servicio.PararStreamUsuario()
        '    Iniciar(megaUsuario)
        'End Try

    End Sub

End Module
