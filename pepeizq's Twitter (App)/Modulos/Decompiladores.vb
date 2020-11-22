Imports Windows.Web.Http

Module Decompiladores

    Public Async Function HttpClient(url As Uri) As Task(Of String)

        Dim cliente As New HttpClient()
        Dim httpFinal As String = Nothing

        cliente.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18362")

        Try
            Dim respuesta As New HttpResponseMessage
            respuesta = Await cliente.GetAsync(url)
            respuesta.EnsureSuccessStatusCode()

            httpFinal = TryCast(Await respuesta.Content.ReadAsStringAsync(), String)
        Catch ex As Exception

        End Try

        cliente.Dispose()
        Return httpFinal
    End Function

End Module
