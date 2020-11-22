Imports Microsoft.Toolkit.Uwp.Helpers
Imports Tweetinvi
Imports Tweetinvi.Models
Imports Windows.ApplicationModel.Core
Imports Windows.System.Threading
Imports Windows.UI.Core

Namespace Interfaz
    Module Menciones

        Public Async Sub Cargar()

            Dim recursos As New Resources.ResourceLoader

            Dim listaMenciones As New List(Of Mencion)
            Dim helper As New LocalObjectStorageHelper

            If Await helper.FileExistsAsync("menciones") Then
                listaMenciones = Await helper.ReadFileAsync(Of List(Of Mencion))("menciones")
            End If

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spMenciones As StackPanel = pagina.FindName("spMenciones")
            spMenciones.Children.Clear()

            Dim parametros As New Parameters.GetMentionsTimelineParameters With {
                .PageSize = 20
            }

            Dim tweets As ITweet() = Nothing

            Try
                tweets = Await cliente_.Timelines.GetMentionsTimelineAsync(parametros)
            Catch ex As Exception

            End Try

            If Not tweets Is Nothing Then
                For Each tweet In tweets
                    spMenciones.Children.Add(Interfaz.Tweets.GenerarTweet(cliente_, tweet, True))

                    Dim añadir As Boolean = True

                    For Each mencion In listaMenciones
                        If mencion.idTweet = tweet.Id And mencion.idUsuario = usuario_.Id Then
                            añadir = False
                        End If
                    Next

                    If añadir = True Then
                        listaMenciones.Add(New Mencion(usuario_.Id, tweet.Id))
                    End If
                Next
            End If

            Try
                Await helper.SaveFileAsync(Of List(Of Mencion))("menciones", listaMenciones)
            Catch ex As Exception

            End Try

            Dim svMenciones As ScrollViewer = pagina.FindName("svMenciones")
            RemoveHandler svMenciones.ViewChanging, AddressOf SvMenciones_ViewChanging
            AddHandler svMenciones.ViewChanging, AddressOf SvMenciones_ViewChanging

            Dim botonSubir As Button = pagina.FindName("botonSubirArribaMenciones")
            botonSubir.Tag = svMenciones

            RemoveHandler botonSubir.Click, AddressOf BotonSubirClick
            AddHandler botonSubir.Click, AddressOf BotonSubirClick

            RemoveHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonSubir.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonSubir.PointerExited, AddressOf Sale_Boton_Icono

            Dim periodoMenciones As TimeSpan = TimeSpan.FromSeconds(130)
            Dim contadorMenciones As ThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Async Sub()
                                                                                               Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Sub()
                                                                                                                                                                                                ActualizarMenciones(cliente_)
                                                                                                                                                                                            End Sub)
                                                                                           End Sub, periodoMenciones)

        End Sub

        Private Async Sub ActualizarMenciones(cliente As TwitterClient)

            Dim recursos As New Resources.ResourceLoader

            Dim listaMenciones As New List(Of Mencion)
            Dim helper As New LocalObjectStorageHelper

            If Await helper.FileExistsAsync("menciones") Then
                listaMenciones = Await helper.ReadFileAsync(Of List(Of Mencion))("menciones")
            End If

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spMenciones As StackPanel = pagina.FindName("spMenciones")

            Dim parametros As New Parameters.GetMentionsTimelineParameters With {
                .PageSize = 20
            }

            Dim tweets As ITweet() = Nothing

            Try
                tweets = Await cliente_.Timelines.GetMentionsTimelineAsync(parametros)
            Catch ex As Exception

            End Try

            Dim i As Integer = 0
            If Not tweets Is Nothing Then
                For Each tweet In tweets
                    Dim añadir As Boolean = True

                    For Each hijo In spMenciones.Children
                        Dim grid As Grid = hijo
                        Dim id As Long = grid.Tag

                        If id = tweet.Id Then
                            añadir = False
                        End If
                    Next

                    If añadir = True Then
                        spMenciones.Children.Insert(i, Interfaz.Tweets.GenerarTweet(cliente, tweet, True))
                        listaMenciones.Add(New Mencion(usuario_.Id, tweet.Id))
                        i += 1
                    End If
                Next
            End If

            Try
                Await helper.SaveFileAsync(Of List(Of Mencion))("menciones", listaMenciones)
            Catch ex As Exception

            End Try

            Dim spAviso As StackPanel = pagina.FindName("spMencionesAviso")

            If i > 0 Then
                spAviso.Visibility = Visibility.Visible

                Dim tbAviso As TextBlock = pagina.FindName("tbMencionesAviso")

                If i = 1 Then
                    tbAviso.Text = i.ToString + " " + recursos.GetString("NewMention")
                Else
                    tbAviso.Text = i.ToString + " " + recursos.GetString("NewMentions")
                End If
            Else
                spAviso.Visibility = Visibility.Collapsed
            End If

        End Sub

        Private Async Sub SvMenciones_ViewChanging(sender As Object, e As ScrollViewerViewChangingEventArgs)

            Dim listaMenciones As New List(Of Mencion)
            Dim helper As New LocalObjectStorageHelper

            If Await helper.FileExistsAsync("menciones") Then
                listaMenciones = Await helper.ReadFileAsync(Of List(Of Mencion))("menciones")
            End If

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonSubir As Button = pagina.FindName("botonSubirArribaMenciones")
            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")

            Dim sv As ScrollViewer = sender

            If sv.VerticalOffset > 50 Then
                gridUsuarioBotones.Visibility = Visibility.Collapsed
                botonSubir.Visibility = Visibility.Visible
            Else
                gridUsuarioBotones.Visibility = Visibility.Visible
                botonSubir.Visibility = Visibility.Collapsed
            End If

            Dim prMenciones As ProgressRing = pagina.FindName("prMenciones")

            If prMenciones.Visibility = Visibility.Collapsed Then
                If sv.ScrollableHeight < sv.VerticalOffset + 200 Then
                    prMenciones.Visibility = Visibility.Visible

                    Dim spMenciones As StackPanel = pagina.FindName("spMenciones")
                    Dim gridUltimoTweet As Grid = spMenciones.Children(spMenciones.Children.Count - 1)
                    Dim ultimoTweet As Long = gridUltimoTweet.Tag

                    Dim parametros As New Parameters.GetMentionsTimelineParameters With {
                        .PageSize = 40,
                        .MaxId = ultimoTweet
                    }

                    Dim tweets As ITweet() = Nothing

                    Try
                        tweets = Await cliente_.Timelines.GetMentionsTimelineAsync(parametros)
                    Catch ex As Exception

                    End Try

                    If Not tweets Is Nothing Then
                        For Each tweet In tweets
                            Dim añadir As Boolean = True

                            For Each hijo In spMenciones.Children
                                Dim grid As Grid = hijo
                                Dim id As Long = grid.Tag

                                If id = tweet.Id Then
                                    añadir = False
                                End If
                            Next

                            If añadir = True Then
                                spMenciones.Children.Add(Interfaz.Tweets.GenerarTweet(cliente_, tweet, True))
                            End If

                            Dim añadir2 As Boolean = True

                            For Each mencion In listaMenciones
                                If mencion.idTweet = tweet.Id And mencion.idUsuario = usuario_.Id Then
                                    añadir2 = False
                                End If
                            Next

                            If añadir2 = True Then
                                listaMenciones.Add(New Mencion(usuario_.Id, tweet.Id))
                            End If
                        Next
                    End If

                    Try
                        Await helper.SaveFileAsync(Of List(Of Mencion))("menciones", listaMenciones)
                    Catch ex As Exception

                    End Try

                    prMenciones.Visibility = Visibility.Collapsed
                End If
            End If

        End Sub

        Private Sub BotonSubirClick(sender As Object, e As RoutedEventArgs)

            Dim botonSubir As Button = sender
            Dim svTweets As ScrollViewer = botonSubir.Tag

            svTweets.ChangeView(Nothing, 0, Nothing)
            botonSubir.Visibility = Visibility.Collapsed

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")
            gridUsuarioBotones.Visibility = Visibility.Visible

        End Sub

    End Module

    Public Class Mencion

        Public idUsuario As Long
        Public idTweet As Long

        Public Sub New(ByVal idusuario As Long, ByVal idtweet As Long)
            Me.idUsuario = idusuario
            Me.idTweet = idtweet
        End Sub

    End Class
End Namespace

