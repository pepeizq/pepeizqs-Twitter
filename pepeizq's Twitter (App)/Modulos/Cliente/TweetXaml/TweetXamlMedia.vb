Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter.Tweet
Imports Windows.Media.Core
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Media.Animation

Namespace pepeizq.Twitter.Xaml
    Module TweetMediaXaml

        Public Function Generar(tweet As Tweet, color As Color)

            If color = Nothing Then
                color = App.Current.Resources("ColorSecundario")
            End If

            Dim colorPlay As New Color

            If color = App.Current.Resources("ColorSecundario") Then
                colorPlay = App.Current.Resources("ColorPrimario")
            Else
                colorPlay = color
            End If

            If Not tweet.Entidades.Media Is Nothing Then
                Dim spMedia As New StackPanel With {
                    .Orientation = Orientation.Horizontal,
                    .Margin = New Thickness(5, 0, 0, 5)
                }

                Dim listaMedia() As TweetMedia

                If tweet.Retweet Is Nothing Then
                    listaMedia = tweet.EntidadesExtendida.Media

                    If listaMedia.Count = 0 Then
                        listaMedia = tweet.Entidades.Media
                    End If
                Else
                    listaMedia = tweet.Retweet.EntidadesExtendida.Media

                    If listaMedia.Count = 0 Then
                        listaMedia = tweet.Retweet.Entidades.Media
                    End If
                End If

                For Each itemMedia In listaMedia
                    Dim objetoString As String = Nothing

                    If itemMedia.Tipo = "photo" Then
                        objetoString = itemMedia.Tipo
                    ElseIf itemMedia.Tipo = "video" Then
                        objetoString = itemMedia.Tipo
                    ElseIf itemMedia.Tipo = "animated_gif" Then
                        objetoString = itemMedia.Tipo
                    End If

                    If Not objetoString = Nothing Then
                        Dim gridPlay As New Grid

                        If itemMedia.Tipo = "video" Or itemMedia.Tipo = "animated_gif" Then
                            gridPlay.Background = New SolidColorBrush(colorPlay)
                            gridPlay.MinHeight = 50
                            gridPlay.MinWidth = 50
                            gridPlay.CornerRadius = New CornerRadius(30)

                            Dim simboloPlay As New FontAwesome.UWP.FontAwesome With {
                                .Icon = FontAwesomeIcon.Play,
                                .Foreground = New SolidColorBrush(Colors.White)
                            }

                            gridPlay.Children.Add(simboloPlay)
                        End If

                        Dim gridMedia As New Grid With {
                            .BorderBrush = New SolidColorBrush(color),
                            .BorderThickness = New Thickness(1, 1, 1, 1),
                            .MaxHeight = ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAlto"),
                            .MaxWidth = ApplicationData.Current.LocalSettings.Values("mediaVistaPreviaAncho"),
                            .HorizontalAlignment = HorizontalAlignment.Left
                        }

                        Dim imagenMedia As New ImageEx With {
                            .Stretch = Stretch.Uniform,
                            .IsCacheEnabled = True
                        }

                        Dim imagenUrl As String = String.Empty

                        If Not itemMedia.EnlaceHttps = String.Empty Then
                            imagenUrl = itemMedia.EnlaceHttps
                        End If

                        If imagenUrl = String.Empty Then
                            imagenUrl = itemMedia.Enlace
                        End If

                        Try
                            imagenMedia.Source = New BitmapImage(New Uri(imagenUrl))
                        Catch ex As Exception

                        End Try

                        If itemMedia.Tipo = "video" Then
                            gridMedia.Background = New SolidColorBrush(Colors.Black)
                            imagenMedia.Opacity = 0.6
                        End If

                        gridMedia.BorderThickness = New Thickness(1, 1, 1, 1)
                        gridMedia.Margin = New Thickness(0, 10, 5, 0)

                        Dim datos As New Objetos.MediaDatos(color, imagenUrl, imagenMedia)

                        Dim frame As Frame = Window.Current.Content
                        Dim pagina As Page = frame.Content

                        If itemMedia.Tipo = "photo" Then
                            AddHandler gridMedia.PointerPressed, AddressOf UsuarioClickeaImagen
                            AddHandler gridMedia.PointerEntered, AddressOf UsuarioEntraMedia
                            AddHandler gridMedia.PointerExited, AddressOf UsuarioSaleMedia
                        ElseIf itemMedia.Tipo = "video" Then
                            Dim listaVideos As TweetVideoVariante() = itemMedia.Video.Variantes
                            Dim listaOrdenada As New List(Of TweetVideoVariante)

                            For Each item In listaVideos
                                listaOrdenada.Add(item)
                            Next

                            listaOrdenada.Sort(Function(x, y) y.Bitrate.CompareTo(x.Bitrate))

                            datos.Enlace = listaOrdenada(0).Enlace

                            AddHandler gridMedia.PointerPressed, AddressOf UsuarioClickeaVideo
                            AddHandler gridMedia.PointerEntered, AddressOf UsuarioEntraMedia
                            AddHandler gridMedia.PointerExited, AddressOf UsuarioSaleMedia
                        ElseIf itemMedia.Tipo = "animated_gif" Then
                            datos.Enlace = itemMedia.Video.Variantes(0).Enlace
                            AddHandler gridMedia.PointerPressed, AddressOf UsuarioClickeaVideo
                            AddHandler gridMedia.PointerEntered, AddressOf UsuarioEntraGif
                            AddHandler gridMedia.PointerExited, AddressOf UsuarioSaleGif
                        End If

                        gridMedia.Tag = datos
                        gridMedia.Children.Add(imagenMedia)

                        If itemMedia.Tipo = "video" Then
                            gridPlay.Width = gridMedia.ActualWidth / 2
                            gridPlay.Height = gridMedia.ActualHeight / 2

                            Dim gridTipo As New Grid With {
                                .HorizontalAlignment = HorizontalAlignment.Left,
                                .VerticalAlignment = VerticalAlignment.Bottom,
                                .Padding = New Thickness(3, 3, 3, 3),
                                .Background = New SolidColorBrush(color)
                            }

                            Dim tbTipo As New TextBlock With {
                                .Foreground = New SolidColorBrush(Colors.White),
                                .FontSize = 13
                            }

                            If itemMedia.Tipo = "video" Then
                                tbTipo.Text = "video"
                            End If

                            gridTipo.Children.Add(tbTipo)

                            gridMedia.Children.Add(gridTipo)
                            gridMedia.Children.Add(gridPlay)
                        End If

                        spMedia.Children.Add(gridMedia)
                    End If
                Next

                If spMedia.Children.Count > 0 Then
                    Dim sv As New ScrollViewer With {
                        .HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        .Content = spMedia,
                        .HorizontalAlignment = HorizontalAlignment.Left,
                        .VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
                    }

                    Return sv
                End If
            End If

            Return New StackPanel
        End Function

        Private Sub UsuarioEntraMedia(sender As Object, e As PointerRoutedEventArgs)

            Dim grid As Grid = sender

            If TypeOf grid.Children(0) Is ImageEx Then
                Dim imagen As ImageEx = grid.Children(0)
                imagen.Saturation(0).Start()
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub UsuarioSaleMedia(sender As Object, e As PointerRoutedEventArgs)

            Dim grid As Grid = sender

            If TypeOf grid.Children(0) Is ImageEx Then
                Dim imagen As ImageEx = grid.Children(0)
                imagen.Saturation(1).Start()
            End If

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Private Sub UsuarioEntraGif(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

            Dim grid As Grid = sender
            Dim cosas As Objetos.MediaDatos = grid.Tag
            Dim imagen As ImageEx = grid.Children(0)

            Dim añadirReproductor As Boolean = True

            For Each hijo In grid.Children
                If TypeOf hijo Is MediaPlayerElement Then
                    añadirReproductor = False

                    Dim reproductor As MediaPlayerElement = hijo
                    reproductor.MediaPlayer.Play()
                End If
            Next

            If añadirReproductor = True Then
                Dim pr As New ProgressRing With {
                    .IsActive = True,
                    .Width = 20,
                    .Height = 20,
                    .Foreground = New SolidColorBrush(Colors.White),
                    .HorizontalAlignment = HorizontalAlignment.Left,
                    .VerticalAlignment = VerticalAlignment.Top,
                    .Margin = New Thickness(5, 5, 5, 5)
                }

                grid.Children.Add(pr)

                Dim reproductor As New MediaPlayerElement With {
                    .Source = MediaSource.CreateFromUri(New Uri(cosas.Enlace)),
                    .Width = imagen.ActualWidth,
                    .Height = imagen.ActualHeight,
                    .MinWidth = 0
                }
                reproductor.MediaPlayer.IsLoopingEnabled = True
                reproductor.MediaPlayer.Play()

                grid.Children.Add(reproductor)

            End If

        End Sub

        Private Sub UsuarioSaleGif(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

            Dim grid As Grid = sender

            For Each hijo In grid.Children
                If TypeOf hijo Is MediaPlayerElement Then
                    Dim reproductor As MediaPlayerElement = hijo
                    reproductor.MediaPlayer.Pause()
                End If
            Next

        End Sub

        Public Sub UsuarioClickeaImagen(sender As Object, e As PointerRoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridRecibido As Grid = sender
            Dim datos As Objetos.MediaDatos = gridRecibido.Tag

            Dim color As Color = datos.Color

            If color = Nothing Then
                color = App.Current.Resources("ColorSecundario")
            End If

            Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")

            If gridUsuario.Visibility = Visibility.Collapsed Then
                color = App.Current.Resources("ColorSecundario")
            End If

            Dim botonCerrar As Button = pagina.FindName("botonCerrarImagen")
            botonCerrar.Background = New SolidColorBrush(color)

            Dim tbImagenAmpliada As TextBox = pagina.FindName("tbImagenAmpliada")
            tbImagenAmpliada.Text = datos.Enlace

            Dim botonDescargar As Button = pagina.FindName("botonDescargarImagen")
            botonDescargar.Background = New SolidColorBrush(color)

            Dim botonCopiar As Button = pagina.FindName("botonCopiarImagen")
            botonCopiar.Background = New SolidColorBrush(color)

            Dim bordeImagen As Border = pagina.FindName("bordeImagenAmpliada")
            bordeImagen.BorderBrush = New SolidColorBrush(color)

            Dim gridImagen As Grid = pagina.FindName("gridImagenAmpliada")
            gridImagen.Visibility = Visibility.Visible

            Dim imagenAmpliada As ImageEx = pagina.FindName("imagenAmpliada")

            Try
                imagenAmpliada.Foreground = New SolidColorBrush(color)
                imagenAmpliada.Source = datos.Enlace
                imagenAmpliada.Tag = datos.Imagen

                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imagenAmpliada", datos.Imagen)

                Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imagenAmpliada")

                If Not animacion Is Nothing Then
                    animacion.TryStart(imagenAmpliada)
                End If
            Catch ex As Exception

            End Try

        End Sub

        Public Sub UsuarioClickeaVideo(sender As Object, e As PointerRoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridRecibido As Grid = sender
            Dim datos As Objetos.MediaDatos = gridRecibido.Tag

            Dim color As Color = datos.Color

            If color = Nothing Then
                color = App.Current.Resources("ColorSecundario")
            End If

            Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")

            If gridUsuario.Visibility = Visibility.Collapsed Then
                color = App.Current.Resources("ColorSecundario")
            End If

            Dim botonCerrar As Button = pagina.FindName("botonCerrarVideo")
            botonCerrar.Background = New SolidColorBrush(color)

            Dim tbVideoAmpliado As TextBox = pagina.FindName("tbVideoAmpliado")
            tbVideoAmpliado.Text = datos.Enlace

            Dim botonDescargar As Button = pagina.FindName("botonDescargarVideo")
            botonDescargar.Background = New SolidColorBrush(color)

            Dim botonCopiar As Button = pagina.FindName("botonCopiarVideo")
            botonCopiar.Background = New SolidColorBrush(color)

            Dim bordeVideo As Border = pagina.FindName("bordeVideoAmpliado")
            bordeVideo.BorderBrush = New SolidColorBrush(color)

            Dim gridVideo As Grid = pagina.FindName("gridVideoAmpliado")
            gridVideo.Visibility = Visibility.Visible

            Dim reproductor As MediaPlayerElement = pagina.FindName("videoAmpliado")

            Try
                reproductor.Source = MediaSource.CreateFromUri(New Uri(datos.Enlace))
                reproductor.MediaPlayer.Play()
                reproductor.Tag = datos.Imagen
            Catch ex As Exception

            End Try

        End Sub

    End Module
End Namespace

