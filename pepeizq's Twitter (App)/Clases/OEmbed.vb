Imports Newtonsoft.Json

Namespace pepeizq.Twitter
    Public Class OEmbed

        <JsonProperty("url")>
        Public Enlace As String

        <JsonProperty("html")>
        Public Html As String

        <JsonProperty("width")>
        Public Ancho As String

        <JsonProperty("height")>
        Public Alto As String

    End Class
End Namespace