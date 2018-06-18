Imports Newtonsoft.Json
Imports pepeizq.Twitter.Tweet

Public Class TweetsBusqueda

    <JsonProperty("statuses")>
    Public Resultados As List(Of Tweet)

End Class
