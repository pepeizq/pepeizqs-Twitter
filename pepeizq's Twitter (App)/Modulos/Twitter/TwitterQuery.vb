Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Module TwitterQuery

    Public Async Function Ejecutar(servicio As TwitterServicio, opcion As Integer, complemento1 As String, complemento2 As String) As Task(Of List(Of Tweet))

        Dim query As String = Nothing

        If opcion = 0 Then
            If complemento1 = Nothing Then
                query = "/statuses/home_timeline.json?tweet_mode=extended"
            Else
                query = "/statuses/home_timeline.json?tweet_mode=extended&max_id=" + complemento1
            End If
        ElseIf opcion = 1 Then
            If complemento1 = Nothing Then
                query = "/statuses/mentions_timeline.json?tweet_mode=extended"
            Else
                query = "/statuses/mentions_timeline.json?tweet_mode=extended&max_id=" + complemento1
            End If
        ElseIf opcion = 2 Then
            query = "/statuses/show.json?id=" + complemento1 + "&tweet_mode=extended"
        End If

        Dim config As New TwitterDataConfig With {
            .Query = query,
            .QueryTipo = TwitterQueryTipo.Custom
        }

        Dim provider As TwitterDataProvider = servicio.Provider
        Dim listaTweets As New List(Of Tweet)

        Try
            listaTweets = Await provider.LoadDataAsync(config, 20, 0, New TwitterParser(Of Tweet))
        Catch ex As Exception

        End Try

        Return listaTweets
    End Function

End Module
