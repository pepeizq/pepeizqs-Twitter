Imports System.Net
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Media.Animation

Namespace pepeizq.Twitter.Xaml
    Module FichaTweet

        Public Async Sub Generar(cosas As Objetos.TweetAmpliado, objetoAnimar As Object)

            Dim tweet As Tweet = Nothing
            tweet = Await TwitterPeticiones.CogerTweet(tweet, cosas.MegaUsuario, cosas.Tweet.ID)

            If Not tweet Is Nothing Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")
                gridUsuario.Visibility = Visibility.Collapsed

                Dim gridTweet As Grid = pagina.FindName("gridTweetAmpliado")

                Dim color As Color = Nothing

                Try
                    color = ("#" + tweet.Usuario.ColorEnlace).ToColor
                Catch ex As Exception
                    color = App.Current.Resources("ColorSecundario")
                End Try

                App.Current.Resources("ButtonBackgroundPointerOver") = color

                Dim transpariencia As New UISettings
                Dim boolTranspariencia As Boolean = transpariencia.AdvancedEffectsEnabled

                If boolTranspariencia = False Then
                    gridTweet.Background = New SolidColorBrush(color)
                Else
                    Dim acrilico As New AcrylicBrush With {
                        .BackgroundSource = AcrylicBackgroundSource.Backdrop,
                        .TintOpacity = 0.7,
                        .TintColor = color
                    }

                    gridTweet.Background = acrilico
                End If

                Try
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionTweet", objetoAnimar)

                    Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionTweet")

                    If Not animacion Is Nothing Then
                        animacion.TryStart(gridTweet)
                    End If
                Catch ex As Exception

                End Try

                gridTweet.Visibility = Visibility.Visible

                '-----------------------------

                Dim botonCerrar As Button = pagina.FindName("botonCerrarTweet")
                botonCerrar.Background = New SolidColorBrush(color)

                Dim spIzquierda As StackPanel = pagina.FindName("spTweetIzquierda")
                spIzquierda.Children.Clear()
                spIzquierda.Children.Add(TweetAvatar.Generar(tweet, cosas.MegaUsuario))

                Dim spDerecha As StackPanel = pagina.FindName("spTweetDerecha")
                spDerecha.Children.Clear()
                spDerecha.Children.Add(TweetUsuario.Generar(tweet, cosas.MegaUsuario, color))
                spDerecha.Children.Add(TweetTexto.Generar(tweet, Nothing, color, cosas.MegaUsuario, False))

                If Not tweet.Cita Is Nothing Then
                    spDerecha.Children.Add(TweetCita.Generar(tweet, cosas.MegaUsuario, color))
                End If

                spDerecha.Children.Add(TweetMediaXaml.Generar(tweet, color))
                spDerecha.Children.Add(TweetBotones.Generar(tweet, gridTweet, cosas.MegaUsuario, 1, color))
                spDerecha.Children.Add(TweetEnviarTweet.Generar(tweet, cosas.MegaUsuario, Visibility.Collapsed, color))

                '-----------------------------

                Dim gridCard As Grid = pagina.FindName("gridTweetCard")
                gridCard.Visibility = Visibility.Collapsed

                If Not ApplicationData.Current.LocalSettings.Values("tweetcard") Is Nothing Then
                    If Not ApplicationData.Current.LocalSettings.Values("tweetcard") = False Then
                        Await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Async Sub()
                                                                                                                                                       If tweet.Entidades.Enlaces.Length > 0 Then
                                                                                                                                                           If Not tweet.Entidades.Enlaces(0).Enlace = String.Empty Then
                                                                                                                                                               Dim html As String = Nothing

                                                                                                                                                               Try
                                                                                                                                                                   html = Await Decompiladores.HttpClient(New Uri(tweet.Entidades.Enlaces(0).Expandida))
                                                                                                                                                               Catch ex As Exception

                                                                                                                                                               End Try

                                                                                                                                                               If Not html = Nothing Then
                                                                                                                                                                   Dim boolEnseñar As Boolean = False

                                                                                                                                                                   If html.Contains(ChrW(34) + "twitter:description" + ChrW(34)) Then
                                                                                                                                                                       Dim temp, temp2, temp3 As String
                                                                                                                                                                       Dim int, int2, int3 As Integer

                                                                                                                                                                       int = html.IndexOf(ChrW(34) + "twitter:description" + ChrW(34))
                                                                                                                                                                       temp = html.Remove(0, int + 2)

                                                                                                                                                                       int2 = temp.IndexOf("content=" + ChrW(34))

                                                                                                                                                                       int2 = temp.IndexOf("content=" + ChrW(34))
                                                                                                                                                                       int3 = temp.IndexOf(">")

                                                                                                                                                                       If int2 > int3 Then
                                                                                                                                                                           temp = html.Remove(int, temp.Length - int)
                                                                                                                                                                           int2 = temp.LastIndexOf("content=" + ChrW(34))
                                                                                                                                                                       End If

                                                                                                                                                                       If int2 = -1 And int3 > 0 Then
                                                                                                                                                                           temp = html.Remove(int, temp.Length - int)
                                                                                                                                                                           int2 = temp.LastIndexOf("content=" + ChrW(34))
                                                                                                                                                                       End If

                                                                                                                                                                       temp2 = temp.Remove(0, int2 + 9)

                                                                                                                                                                       int3 = temp2.IndexOf(ChrW(34))
                                                                                                                                                                       temp3 = temp2.Remove(int3, temp2.Length - int3)

                                                                                                                                                                       Dim descripcion As TextBlock = pagina.FindName("tbTweetCardDescripcion")
                                                                                                                                                                       descripcion.Text = WebUtility.HtmlDecode(temp3.Trim)

                                                                                                                                                                       boolEnseñar = True
                                                                                                                                                                   End If

                                                                                                                                                                   If html.Contains(ChrW(34) + "twitter:image" + ChrW(34)) Then
                                                                                                                                                                       Dim temp, temp2, temp3 As String
                                                                                                                                                                       Dim int, int2, int3 As Integer

                                                                                                                                                                       int = html.IndexOf(ChrW(34) + "twitter:image" + ChrW(34))
                                                                                                                                                                       temp = html.Remove(0, int + 2)

                                                                                                                                                                       int2 = temp.IndexOf("content=" + ChrW(34))
                                                                                                                                                                       int3 = temp.IndexOf(">")

                                                                                                                                                                       If int2 > int3 Then
                                                                                                                                                                           temp = html.Remove(int, temp.Length - int)
                                                                                                                                                                           int2 = temp.LastIndexOf("content=" + ChrW(34))
                                                                                                                                                                       End If

                                                                                                                                                                       If int2 = -1 And int3 > 0 Then
                                                                                                                                                                           temp = html.Remove(int, temp.Length - int)
                                                                                                                                                                           int2 = temp.LastIndexOf("content=" + ChrW(34))
                                                                                                                                                                       End If

                                                                                                                                                                       temp2 = temp.Remove(0, int2 + 9)

                                                                                                                                                                       int3 = temp2.IndexOf(ChrW(34))
                                                                                                                                                                       temp3 = temp2.Remove(int3, temp2.Length - int3)

                                                                                                                                                                       Dim imagen As ImageEx = pagina.FindName("imagenTweetCard")
                                                                                                                                                                       imagen.Source = temp3.Trim

                                                                                                                                                                       Dim borde As Border = pagina.FindName("bordeImagenTweetCard")
                                                                                                                                                                       borde.BorderBrush = New SolidColorBrush(color)

                                                                                                                                                                       boolEnseñar = True
                                                                                                                                                                   End If

                                                                                                                                                                   If boolEnseñar = True Then
                                                                                                                                                                       gridCard.Visibility = Visibility.Visible
                                                                                                                                                                   End If
                                                                                                                                                               End If
                                                                                                                                                           End If
                                                                                                                                                       End If
                                                                                                                                                   End Sub)
                    End If
                End If

                '-----------------------------

                Dim listaTweetRespuestas As New List(Of Tweet)

                listaTweetRespuestas = Await TwitterPeticiones.BuscarRespuestasTweet(listaTweetRespuestas, cosas.MegaUsuario, tweet.ID, tweet.Usuario.ScreenNombre)

                If listaTweetRespuestas.Count > 0 Then
                    Dim lvTweets As ListView = pagina.FindName("lvTweetRespuestas")
                    lvTweets.IsItemClickEnabled = True

                    If lvTweets.Items.Count > 0 Then
                        lvTweets.Items.Clear()
                    End If

                    AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

                    For Each tweetRespuesta In listaTweetRespuestas
                        If tweetRespuesta.RespuestaUsuarioID = tweet.ID Then
                            lvTweets.Items.Insert(0, TweetXaml.Añadir(tweetRespuesta, cosas.MegaUsuario, color))
                        End If
                    Next
                End If
            End If
        End Sub

    End Module
End Namespace