Imports Newtonsoft.Json
Imports pepeizq.Twitter
Imports pepeizq.Twitter.OAuth
Imports pepeizq.Twitter.Tweet
Imports Windows.Storage.Streams

Module TwitterPeticiones

    Public Async Function HomeTimeline(listaTweets As List(Of Tweet), megaUsuario As pepeizq.Twitter.MegaUsuario, ultimoTweet As String) As Task(Of List(Of Tweet))

        If Not ultimoTweet = Nothing Then
            ultimoTweet = "&max_id=" + ultimoTweet
        End If

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/home_timeline.json?tweet_mode=extended" + ultimoTweet)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim parser As New TweetParser

            listaTweets = parser.Parse(resultado)
        Catch ex As Exception

        End Try

        Return listaTweets

    End Function

    Public Async Function MentionsTimeline(listaTweets As List(Of Tweet), megaUsuario As pepeizq.Twitter.MegaUsuario, ultimoTweet As String) As Task(Of List(Of Tweet))

        If Not ultimoTweet = Nothing Then
            ultimoTweet = "&max_id=" + ultimoTweet
        End If

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/mentions_timeline.json?tweet_mode=extended" + ultimoTweet)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim parser As New TweetParser

            listaTweets = parser.Parse(resultado)
        Catch ex As Exception

        End Try

        Return listaTweets

    End Function

    Public Async Function UserTimeline(listaTweets As List(Of Tweet), megaUsuario As pepeizq.Twitter.MegaUsuario, screenNombre As String, ultimoTweet As String) As Task(Of List(Of Tweet))

        If Not ultimoTweet = Nothing Then
            ultimoTweet = "&max_id=" + ultimoTweet
        End If

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=" + screenNombre + "&tweet_mode=extended&include_rts=1" + ultimoTweet)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim parser As New TweetParser

            listaTweets = parser.Parse(resultado)
        Catch ex As Exception

        End Try

        Return listaTweets

    End Function

    Public Async Function CogerUsuario(usuario As TwitterUsuario, megaUsuario As pepeizq.Twitter.MegaUsuario, screenNombre As String) As Task(Of TwitterUsuario)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/users/show.json?screen_name=" + screenNombre)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            usuario = JsonConvert.DeserializeObject(Of TwitterUsuario)(resultado)
        Catch ex As Exception

        End Try

        Return usuario

    End Function

    Public Async Function CogerTweet(tweet As Tweet, megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of Tweet)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/show.json?id=" + tweetID + "&tweet_mode=extended")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim parser As New TweetParserIndividual

            tweet = parser.Parse(resultado)
        Catch ex As Exception

        End Try

        Return tweet

    End Function

    Public Async Function BuscarRespuestasTweet(listaTweets As List(Of Tweet), megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String, screenNombre As String) As Task(Of List(Of Tweet))

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/search/tweets.json?q=%3A" + screenNombre + "&result_type=recent&count=100&since_id=" + tweetID)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim busqueda As pepeizq.Twitter.TweetsBusqueda = JsonConvert.DeserializeObject(Of pepeizq.Twitter.TweetsBusqueda)(resultado)
            listaTweets = busqueda.Resultados
        Catch ex As Exception

        End Try

        Return listaTweets

    End Function

    Public Async Function BuscarUsuarios(listaUsuarios As List(Of TwitterUsuario), megaUsuario As pepeizq.Twitter.MegaUsuario, screenNombre As String) As Task(Of List(Of TwitterUsuario))

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/users/search.json?q=" + screenNombre + "&count=20")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            listaUsuarios = JsonConvert.DeserializeObject(Of List(Of TwitterUsuario))(resultado)
        Catch ex As Exception

        End Try

        Return listaUsuarios

    End Function

    Public Async Function CogerOEmbedTweet(oembed As pepeizq.Twitter.OEmbed, megaUsuario As pepeizq.Twitter.MegaUsuario, enlace As String) As Task(Of pepeizq.Twitter.OEmbed)

        Try
            Dim enlaceUri As New Uri("https://publish.twitter.com/oembed?url=" + enlace)
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlaceUri, megaUsuario.Servicio.twitterDataProvider._tokens)
            oembed = JsonConvert.DeserializeObject(Of pepeizq.Twitter.OEmbed)(resultado)
        Catch ex As Exception

        End Try

        Return oembed

    End Function

    Public Async Function SeguirUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/friendships/create.json?user_id=" + usuarioID + "&follow=true")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function DeshacerSeguirUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/friendships/destroy.json?user_id=" + usuarioID)
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function CogerListaBloqueos(listaIDs As List(Of String), megaUsuario As pepeizq.Twitter.MegaUsuario) As Task(Of List(Of String))

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/blocks/ids.json?stringify_ids=true&cursor=-1")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim bloqueos As pepeizq.Twitter.UsuarioBloqueos = JsonConvert.DeserializeObject(Of pepeizq.Twitter.UsuarioBloqueos)(resultado)
            listaIDs = bloqueos.IDs
        Catch ex As Exception

        End Try

        Return listaIDs

    End Function

    Public Async Function BloquearUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/blocks/create.json?user_id=" + usuarioID + "&skip_status=1")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function DeshacerBloquearUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/blocks/destroy.json?user_id=" + usuarioID + "&skip_status=1")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function CogerListaMuteados(listaIDs As List(Of String), megaUsuario As pepeizq.Twitter.MegaUsuario) As Task(Of List(Of String))

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/mutes/users/ids.json?cursor=-1")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim bloqueos As pepeizq.Twitter.UsuarioMuteados = JsonConvert.DeserializeObject(Of pepeizq.Twitter.UsuarioMuteados)(resultado)
            listaIDs = bloqueos.IDs
        Catch ex As Exception

        End Try

        Return listaIDs

    End Function

    Public Async Function MutearUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/mutes/users/create.json?user_id=" + usuarioID)
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function DeshacerMutearUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/mutes/users/destroy.json?user_id=" + usuarioID)
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function ReportarUsuario(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, usuarioID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/users/report_spam.json?user_id=" + usuarioID + "&perform_block=true")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function FavoritearTweet(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/favorites/create.json?id=" + tweetID)
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function DeshacerFavoritearTweet(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/favorites/destroy.json?id=" + tweetID)
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function RetwittearTweet(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/retweet/" + tweetID + ".json")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function DeshacerRetwittearTweet(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/unretweet/" + tweetID + ".json")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function EnviarTweet(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, mensaje As String, respuestaID As String, medias As List(Of IRandomAccessStream)) As Task(Of Boolean)

        Try
            Dim mediasIDs As String = Nothing

            If Not medias Is Nothing Then
                If medias.Count > 0 Then
                    Dim i As Integer = 0

                    For Each subMedia In medias
                        Dim id As String = Nothing

                        id = Await SubirMedia(id, megaUsuario, subMedia)

                        If Not id = Nothing Then
                            If i = 0 Then
                                mediasIDs = mediasIDs + id
                            Else
                                mediasIDs = mediasIDs + "," + id
                            End If
                            i += 1
                        End If
                    Next
                End If
            End If

            If Not mediasIDs = Nothing Then
                mediasIDs = "&media_ids=" + mediasIDs
            End If

            If Not respuestaID = Nothing Then
                respuestaID = "&in_reply_to_status_id=" + respuestaID
            End If

            Dim encodeMensaje As String = Uri.EscapeDataString(mensaje)
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/update.json?status=" + encodeMensaje + respuestaID + mediasIDs)
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function SubirMedia(id As String, megaUsuario As pepeizq.Twitter.MegaUsuario, stream As IRandomAccessStream) As Task(Of String)

        Try
            Dim ficheroTamaño As Long = stream.Size
            Dim buffer(ficheroTamaño) As Byte
            Await stream.ReadAsync(buffer.AsBuffer, stream.Size, InputStreamOptions.None)
            stream.Seek(0)
            Dim limite As String = DateTime.Now.Ticks.ToString("x")

            Dim enlace As New Uri("https://upload.twitter.com/1.1/media/upload.json")
            Dim request As New TwitterOAuthRequest
            id = Await request.ExecutePostMultipartAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens, limite, buffer)
        Catch ex As Exception

        End Try

        Return id

    End Function

    Public Async Function BorrarTweet(estado As Boolean, megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of Boolean)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/destroy/" + tweetID + ".json")
            Dim request As New TwitterOAuthRequest
            Await request.EjecutarPostAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            estado = True
        Catch ex As Exception

        End Try

        Return estado

    End Function

    Public Async Function BuscarRetweetsTweet(listaTweets As List(Of Tweet), megaUsuario As pepeizq.Twitter.MegaUsuario, tweetID As String) As Task(Of List(Of Tweet))

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/retweets/" + tweetID + ".json?count=100")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            listaTweets = JsonConvert.DeserializeObject(Of List(Of Tweet))(resultado)
        Catch ex As Exception

        End Try

        Return listaTweets

    End Function

End Module
