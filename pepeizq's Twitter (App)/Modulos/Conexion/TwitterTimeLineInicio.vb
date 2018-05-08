Imports Microsoft.Advertising.WinRT.UI
Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Module TwitterTimeLineInicio

    Dim intentosCarga As Integer = 0

    Public Async Sub CargarTweets(megaUsuario As pepeizq.Twitter.MegaUsuario, ultimoTweet As String, limpiar As Boolean)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario2.Usuario

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridTweets As Grid = pagina.FindName("gridTweets" + usuario.ScreenNombre)

        If Not gridTweets Is Nothing Then
            Dim pr As ProgressRing = gridTweets.Children(0)
            Dim pb As ProgressBar = gridTweets.Children(3)

            If Not ultimoTweet = Nothing Then
                pb.Visibility = Visibility.Visible
            End If

            '-------------------------------

            Dim sv As ScrollViewer = gridTweets.Children(1)
            Dim lv As ListView = sv.Content

            If limpiar = True Then
                lv.Items.Clear()
            End If

            Dim provider As TwitterDataProvider = megaUsuario.Servicio.Provider
            Dim listaTweets As New List(Of Tweet)

            Try
                listaTweets = Await provider.CogerTweetsTimelineInicio(Of Tweet)(megaUsuario.Usuario2.Usuario.Tokens, ultimoTweet, New TweetParser)
            Catch ex As Exception

            End Try

            If listaTweets.Count = 0 Then

                pr.IsActive = True

                Await Task.Delay(20000)

                intentosCarga = intentosCarga + 1
                pr.IsActive = False

                If intentosCarga < 500 Then
                    TwitterTimeLineInicio.CargarTweets(megaUsuario, ultimoTweet, False)
                Else
                    TwitterConexion.Desconectar(megaUsuario.Servicio)

                    Dim megaUsuarioNuevo As pepeizq.Twitter.MegaUsuario = Await TwitterConexion.Iniciar(megaUsuario.Usuario2)

                    If Not megaUsuarioNuevo Is Nothing Then
                        TwitterTimeLineInicio.CargarTweets(megaUsuarioNuevo, Nothing, False)

                        intentosCarga = 0
                    End If
                End If
            Else
                For Each tweet In listaTweets
                    Dim boolAñadirTweet As Boolean = True
                    Dim boolAñadirAnuncio1 As Boolean = True
                    Dim boolAñadirAnuncio2 As Boolean = True

                    For Each item In lv.Items
                        If TypeOf item Is ListViewItem Then
                            Dim lvItem As ListViewItem = item
                            Dim gridTweet As Grid = lvItem.Content
                            Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
                            Dim lvTweet As Tweet = tweetAmpliado.Tweet

                            If lvTweet.ID = tweet.ID Then
                                boolAñadirTweet = False
                            End If
                        End If

                        If TypeOf item Is Grid Then
                            Dim gridAnuncio As Grid = item

                            If gridAnuncio.Name = "gridAnuncio1100022916" Then
                                boolAñadirAnuncio1 = False
                            End If

                            If gridAnuncio.Name = "gridAnuncio1100022920" Then
                                boolAñadirAnuncio2 = False
                            End If
                        End If
                    Next

                    If boolAñadirTweet = True Then
                        lv.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, megaUsuario, Nothing))
                    End If

                    If boolAñadirAnuncio1 = True Then
                        lv.Items.Add(AñadirAnuncio("1100022916"))
                    End If

                    If boolAñadirAnuncio1 = False And boolAñadirAnuncio2 = True Then
                        lv.Items.Add(AñadirAnuncio("1100022920"))
                    End If
                Next

                pr.IsActive = False

                If Not ultimoTweet = Nothing Then
                    pb.Visibility = Visibility.Collapsed
                End If
            End If
        End If

    End Sub

    Public Function AñadirAnuncio(id As String)

        Dim gridAnuncio As New Grid With {
            .Name = "gridAnuncio" + id,
            .Padding = New Thickness(10, 10, 10, 10)
        }

        Dim anuncio As New AdControl With {
            .AdUnitId = id,
            .Width = 728,
            .Height = 90,
            .HorizontalAlignment = HorizontalAlignment.Center
        }

        Dim color1 As New GradientStop With {
            .Color = ColorHelper.ToColor("#e0e0e0"),
            .Offset = 0.5
        }

        Dim color2 As New GradientStop With {
            .Color = ColorHelper.ToColor("#d6d6d6"),
            .Offset = 1.0
        }

        Dim coleccion As New GradientStopCollection From {
            color1,
            color2
        }

        Dim brush As New LinearGradientBrush With {
            .StartPoint = New Point(0.5, 0),
            .EndPoint = New Point(0.5, 1),
            .GradientStops = coleccion
        }

        gridAnuncio.Background = brush
        gridAnuncio.Children.Add(anuncio)

        Return gridAnuncio

    End Function

End Module
