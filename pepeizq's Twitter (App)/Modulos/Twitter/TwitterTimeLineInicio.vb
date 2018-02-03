Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Module TwitterTimeLineInicio

    Dim intentosCarga As Integer = 0

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

        Dim gridTweets As Grid = gridUsuario.Children(1)

        If Not gridTweets Is Nothing Then
            Dim pr As ProgressRing = gridTweets.Children(0)
            Dim pb As ProgressBar = gridTweets.Children(2)

            If Not ultimoTweet = Nothing Then
                pb.Visibility = Visibility.Visible
            End If

            '-------------------------------

            Dim sv As ScrollViewer = gridTweets.Children(1)
            Dim lv As ListView = sv.Content

            Dim provider As TwitterDataProvider = megaUsuario.Servicio.Provider
            Dim listaTweets As New List(Of Tweet)

            Try
                listaTweets = Await provider.CogerTweetsTimelineInicio(Of Tweet)(ultimoTweet, New TweetParser)
            Catch ex As Exception

            End Try

            If listaTweets.Count = 0 Then

                pr.IsActive = True

                Await Task.Delay(20000)

                intentosCarga = intentosCarga + 1
                pr.IsActive = False

                If intentosCarga < 500 Then
                    TwitterTimeLineInicio.CargarTweets(megaUsuario, ultimoTweet)
                Else
                    TwitterConexion.Desconectar(megaUsuario.Servicio)

                    Dim megaUsuarioNuevo As pepeizq.Twitter.MegaUsuario = Await TwitterConexion.Iniciar(megaUsuario.Usuario)

                    If Not megaUsuarioNuevo Is Nothing Then
                        TwitterTimeLineInicio.CargarTweets(megaUsuarioNuevo, Nothing)

                        intentosCarga = 0
                    End If
                End If
            Else
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

                pr.IsActive = False

                If Not ultimoTweet = Nothing Then
                    pb.Visibility = Visibility.Collapsed
                End If
            End If
        End If

    End Sub

End Module
