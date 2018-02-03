Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Module TwitterTimeLineMenciones

    Public Async Sub CargarTweets(megaUsuario As pepeizq.Twitter.MegaUsuario, ultimoTweet As String)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
        Dim gridUsuario As New Grid

        For Each item In gridPrincipal.Children
            Dim grid As Grid = item

            If grid.Name = "gridUsuario" + usuario.ScreenNombre Then
                gridUsuario = grid
            End If
        Next

        Dim gridTweets As Grid = gridUsuario.Children(2)

        If Not gridTweets Is Nothing Then
            Dim sv As ScrollViewer = gridTweets.Children(0)
            Dim lv As ListView = sv.Content

            Dim provider As TwitterDataProvider = megaUsuario.Servicio.Provider
            Dim listaTweets As New List(Of Tweet)

            Try
                listaTweets = Await provider.CogerTweetsTimelineMenciones(Of Tweet)(ultimoTweet, New TweetParser)
            Catch ex As Exception

            End Try

            If listaTweets.Count > 0 Then
                For Each Tweet In listaTweets
                    Dim boolAñadir As Boolean = True

                    For Each item In lv.Items
                        Dim lvItem As ListViewItem = item
                        Dim gridTweet As Grid = lvItem.Content
                        Dim lvTweet As Tweet = gridTweet.Tag

                        If lvTweet.ID = Tweet.ID Then
                            boolAñadir = False
                        End If
                    Next

                    If boolAñadir = True Then
                        lv.Items.Add(TweetXaml.Añadir(Tweet, megaUsuario))
                    End If
                Next
            End If
        End If

    End Sub

End Module
