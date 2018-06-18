Imports Newtonsoft.Json
Imports pepeizq.Twitter.Busqueda
Imports pepeizq.Twitter.OAuth
Imports pepeizq.Twitter.Tweet

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

    Public Async Function CogerTweet(tweet As Tweet, megaUsuario As pepeizq.Twitter.MegaUsuario, idTweet As String) As Task(Of Tweet)

        Try
            Dim enlace As New Uri("https://api.twitter.com/1.1/statuses/show.json?id=" + idTweet + "&tweet_mode=extended")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            Dim parser As New TweetParserIndividual

            tweet = parser.Parse(resultado)
        Catch ex As Exception

        End Try

        Return tweet

    End Function

    Public Async Function BuscarRespuestasTweet(listaTweets As List(Of Tweet), megaUsuario As pepeizq.Twitter.MegaUsuario, idTweet As String, screenNombre As String) As Task(Of List(Of Tweet))

        Try
            Dim busqueda As New TweetsBusqueda
            Dim enlace As New Uri("https://api.twitter.com/1.1/search/tweets.json?q=%3A" + screenNombre + "&result_type=recent&count=100")
            Dim request As New TwitterOAuthRequest
            Dim resultado As String = Await request.EjecutarGetAsync(enlace, megaUsuario.Servicio.twitterDataProvider._tokens)
            busqueda = JsonConvert.DeserializeObject(Of TweetsBusqueda)(resultado)
            listaTweets = busqueda.Resultados
        Catch ex As Exception

        End Try

        Return listaTweets

    End Function

End Module
