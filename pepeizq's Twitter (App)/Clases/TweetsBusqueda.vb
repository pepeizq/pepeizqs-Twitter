Imports Newtonsoft.Json
Imports pepeizq.Twitter.Tweet

Namespace pepeizq.Twitter
    Public Class TweetsBusqueda

        <JsonProperty("statuses")>
        Public Resultados As List(Of Tweet)

    End Class
End Namespace

