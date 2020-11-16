Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Module FichaOEmbedXaml

    'Public Async Sub Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, tweet As Tweet)

    '    Dim recursos As New Resources.ResourceLoader

    '    Dim oembed As New pepeizq.Twitter.OEmbed

    '    oembed = Await TwitterPeticiones.CogerOEmbedTweet(oembed, megaUsuario, "https://twitter.com/" + tweet.Usuario.ScreenNombre + "/status/" + tweet.ID)

    '    If Not oembed Is Nothing Then
    '        Dim frame As Frame = Window.Current.Content
    '        Dim pagina As Page = frame.Content

    '        Dim nvPrincipal As NavigationView = pagina.FindName("nvPrincipal")

    '        For Each item In nvPrincipal.MenuItems
    '            If TypeOf item Is NavigationViewItem Then
    '                Dim nv As NavigationViewItem = item

    '                If TypeOf nv.Content Is TextBlock Then
    '                    Dim tb As TextBlock = nv.Content

    '                    If tb.Text = recursos.GetString("Back") Then
    '                        nv.Visibility = Visibility.Visible

    '                        Dim separador As NavigationViewItemSeparator = pagina.FindName("nvSeparadorVolver")
    '                        separador.Visibility = Visibility.Visible
    '                    End If
    '                End If
    '            End If
    '        Next

    '        Dim gridOEmbed As Grid = pagina.FindName("gridOEmbedAmpliado")
    '        gridOEmbed.Visibility = Visibility.Visible

    '        Dim tbOEmbed As TextBox = pagina.FindName("tbOEmbed")
    '        tbOEmbed.Text = oembed.Html
    '    End If

    'End Sub

End Module
