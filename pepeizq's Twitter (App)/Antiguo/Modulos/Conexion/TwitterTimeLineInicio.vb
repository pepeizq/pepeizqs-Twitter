Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Core

Module TwitterTimeLineInicio

    'Public Async Sub CargarTweets(megaUsuario As pepeizq.Twitter.MegaUsuario, ultimoTweet As String, limpiar As Boolean)

    '    Dim usuario As TwitterUsuario = megaUsuario.Usuario

    '    Dim frame As Frame = Window.Current.Content
    '    Dim pagina As Page = frame.Content

    '    Dim gridTweets As Grid = pagina.FindName("gridTweets" + usuario.ID)

    '    If Not gridTweets Is Nothing Then
    '        gridTweets.Background = New SolidColorBrush(Colors.LightGray)

    '        Dim pr As ProgressRing = gridTweets.Children(0)
    '        Dim pb As ProgressBar = gridTweets.Children(3)

    '        If Not ultimoTweet = Nothing Then
    '            pb.Visibility = Visibility.Visible
    '        End If

    '        '-------------------------------

    '        Dim sv As ScrollViewer = gridTweets.Children(1)
    '        Dim lv As ListView = sv.Content

    '        If limpiar = True Then
    '            lv.Items.Clear()
    '        End If

    '        Dim listaTweets As New List(Of Tweet)

    '        listaTweets = Await TwitterPeticiones.HomeTimeline(listaTweets, megaUsuario, ultimoTweet)

    '        If listaTweets.Count > 0 Then
    '            For Each tweet In listaTweets
    '                Dim boolAñadirTweet As Boolean = True

    '                For Each item In lv.Items
    '                    If TypeOf item Is ListViewItem Then
    '                        Dim lvItem As ListViewItem = item
    '                        Dim gridTweet As Grid = lvItem.Content
    '                        Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
    '                        Dim lvTweet As Tweet = tweetAmpliado.Tweet

    '                        If lvTweet.ID = tweet.ID Then
    '                            boolAñadirTweet = False
    '                        End If
    '                    End If
    '                Next

    '                If boolAñadirTweet = True Then
    '                    lv.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, megaUsuario, Nothing))
    '                End If
    '            Next

    '            pr.IsActive = False

    '            If Not ultimoTweet = Nothing Then
    '                pb.Visibility = Visibility.Collapsed
    '            End If
    '        End If
    '    End If

    'End Sub

    'Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

    '    Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    'End Sub

    'Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

    '    Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    'End Sub

End Module
