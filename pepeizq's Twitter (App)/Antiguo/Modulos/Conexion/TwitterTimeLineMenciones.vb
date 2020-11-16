Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.Storage
Imports Windows.UI

Module TwitterTimeLineMenciones

    'Public Async Sub CargarTweets(megaUsuario As pepeizq.Twitter.MegaUsuario, ultimoTweet As String, limpiar As Boolean, inicio As Boolean)

    '    Dim recursos As New Resources.ResourceLoader

    '    Dim frame As Frame = Window.Current.Content
    '    Dim pagina As Page = frame.Content

    '    Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
    '    Dim gridUsuario As New Grid

    '    Dim gridTweets As Grid = pagina.FindName("gridMenciones" + megaUsuario.Usuario.ID)

    '    If Not gridTweets Is Nothing Then
    '        gridTweets.Background = New SolidColorBrush(Colors.LightGray)

    '        Dim sv As ScrollViewer = gridTweets.Children(0)
    '        Dim lv As ListView = sv.Content

    '        If limpiar = True Then
    '            lv.Items.Clear()
    '        End If

    '        Dim listaTweets As New List(Of Tweet)

    '        listaTweets = Await TwitterPeticiones.MentionsTimeline(listaTweets, megaUsuario, ultimoTweet)

    '        If listaTweets.Count > 0 Then
    '            For Each tweet In listaTweets
    '                Dim boolAñadir As Boolean = True

    '                For Each item In lv.Items
    '                    Dim lvItem As ListViewItem = item
    '                    Dim gridTweet As Grid = lvItem.Content
    '                    Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
    '                    Dim lvTweet As Tweet = tweetAmpliado.Tweet

    '                    If lvTweet.ID = tweet.ID Then
    '                        boolAñadir = False
    '                    End If
    '                Next

    '                If boolAñadir = True Then
    '                    lv.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, megaUsuario, Nothing))
    '                End If
    '            Next

    '            If inicio = True Then
    '                If ApplicationData.Current.LocalSettings.Values("ultimaMencion" + megaUsuario.Usuario.ScreenNombre) Is Nothing Then
    '                    ApplicationData.Current.LocalSettings.Values("ultimaMencion" + megaUsuario.Usuario.ScreenNombre) = listaTweets(0).ID
    '                Else
    '                    If Not listaTweets(0).ID = ApplicationData.Current.LocalSettings.Values("ultimaMencion" + megaUsuario.Usuario.ScreenNombre) Then
    '                        ApplicationData.Current.LocalSettings.Values("ultimaMencion" + megaUsuario.Usuario.ScreenNombre) = listaTweets(0).ID
    '                        Notificaciones.ToastMencion(megaUsuario.Usuario)
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End If

    'End Sub

End Module
