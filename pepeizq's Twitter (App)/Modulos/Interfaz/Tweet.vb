Imports System.Globalization
Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Tweetinvi.Models
Imports Windows.ApplicationModel.Core
Imports Windows.System.Threading
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Documents
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module Tweet

        Public Function Generar(tweet As ITweet)

            If Not tweet Is Nothing Then
                Dim colorFondo As New SolidColorBrush With {
                    .Color = App.Current.Resources("ColorCuarto"),
                    .Opacity = 0.8
                }

                Dim grid As New Grid With {
                    .Name = "gridTweet" + tweet.Id.ToString,
                    .Margin = New Thickness(0, 0, 30, 20),
                    .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                    .BorderThickness = New Thickness(1, 1, 1, 1),
                    .Background = colorFondo,
                    .Padding = New Thickness(15, 15, 15, 15),
                    .Tag = tweet.Id
                }

                Dim row1 As New RowDefinition
                Dim row2 As New RowDefinition

                row1.Height = New GridLength(1, GridUnitType.Auto)
                row2.Height = New GridLength(1, GridUnitType.Star)

                grid.RowDefinitions.Add(row1)
                grid.RowDefinitions.Add(row2)

                '-----------------------------

                Dim gridSuperior As New Grid
                gridSuperior.SetValue(Grid.RowProperty, 0)

                If tweet.IsRetweet = True Then
                    gridSuperior.Children.Add(Retweet(tweet))
                End If

                grid.Children.Add(gridSuperior)

                '-----------------------------

                Dim gridInferior As New Grid
                gridInferior.SetValue(Grid.RowProperty, 1)

                Dim col1 As New ColumnDefinition
                Dim col2 As New ColumnDefinition
                Dim col3 As New ColumnDefinition

                col1.Width = New GridLength(1, GridUnitType.Auto)
                col2.Width = New GridLength(1, GridUnitType.Star)
                col3.Width = New GridLength(1, GridUnitType.Auto)

                gridInferior.ColumnDefinitions.Add(col1)
                gridInferior.ColumnDefinitions.Add(col2)
                gridInferior.ColumnDefinitions.Add(col3)

                '-----------------------------

                Dim spIzquierda As New StackPanel With {
                    .Orientation = Orientation.Vertical
                }

                spIzquierda.SetValue(Grid.ColumnProperty, 0)

                spIzquierda.Children.Add(Avatar(tweet))

                gridInferior.Children.Add(spIzquierda)

                '-----------------------------

                Dim spInferiorCentro As New StackPanel With {
                    .Orientation = Orientation.Vertical
                }

                spInferiorCentro.SetValue(Grid.ColumnProperty, 1)

                spInferiorCentro.Children.Add(Usuario(tweet))

                Dim tbTweet As TextBlock = Texto(tweet)

                If Not tbTweet Is Nothing Then
                    If tbTweet.Text.Length > 0 Then
                        spInferiorCentro.Children.Add(tbTweet)
                    End If
                End If

                If Not tweet.QuotedTweet Is Nothing Then
                    If tweet.IsRetweet = False Then
                        spInferiorCentro.Children.Add(Cita(tweet))
                    Else
                        spInferiorCentro.Children.Add(Cita(tweet.RetweetedTweet))
                    End If
                End If

                spInferiorCentro.Children.Add(Media(tweet))

                'spInferiorCentro.Children.Add(TweetBotones.Generar(tweet, grid, megaUsuario, 0, color))
                'spInferiorCentro.Children.Add(TweetEnviarTweet.Generar(tweet, megaUsuario, Visibility.Collapsed, color))

                gridInferior.Children.Add(spInferiorCentro)

                '-----------------------------

                Dim gridInferiorDerecha As New Grid With {
                    .HorizontalAlignment = HorizontalAlignment.Right,
                    .Margin = New Thickness(0, 5, 25, 0)
                }

                gridInferiorDerecha.SetValue(Grid.ColumnProperty, 2)

                Dim tbTiempo As New TextBlock With {
                    .FontSize = 12,
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                SumarTiempo(tbTiempo, tweet.CreatedAt.LocalDateTime)

                gridInferiorDerecha.Children.Add(tbTiempo)

                '-----------------------------

                gridInferior.Children.Add(gridInferiorDerecha)

                '-----------------------------

                grid.Children.Add(gridInferior)

                Return grid
            End If

            Return Nothing

        End Function

        'CONTADOR_TIEMPO-------------------------------------------------

        Private Sub SumarTiempo(tb As TextBlock, fecha As DateTime)

            Dim periodo As TimeSpan = TimeSpan.FromSeconds(1)

            Dim contador As ThreadPoolTimer = Nothing
            contador = ThreadPoolTimer.CreatePeriodicTimer(Async Sub(tiempo)
                                                               Try
                                                                   Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Sub()
                                                                                                                                                                     If Not tb.Text Is Nothing Then
                                                                                                                                                                         Dim fechaFinal As TimeSpan = DateTime.Now - fecha
                                                                                                                                                                         Dim totalSegundos As Integer = fechaFinal.TotalSeconds

                                                                                                                                                                         fechaFinal = fechaFinal.Add(periodo)
                                                                                                                                                                         MostrarTiempo(tb, fechaFinal)
                                                                                                                                                                     End If
                                                                                                                                                                 End Sub))
                                                               Catch ex As Exception

                                                               End Try
                                                           End Sub, periodo)

        End Sub

        Private Sub MostrarTiempo(tb As TextBlock, tiempo As TimeSpan)

            If tiempo.TotalSeconds > -1 And tiempo.TotalSeconds < 60 Then
                Dim segundos As Integer = Convert.ToInt32(tiempo.TotalSeconds)
                tb.Text = segundos.ToString + "s"
            ElseIf tiempo.TotalMinutes >= 1 And tiempo.TotalMinutes < 60 Then
                Dim minutos As Integer = Convert.ToInt32(tiempo.TotalMinutes)
                tb.Text = minutos.ToString + "m"
            ElseIf tiempo.TotalHours >= 1 And tiempo.TotalHours < 24 Then
                Dim horas As Integer = Convert.ToInt32(tiempo.TotalHours)
                tb.Text = horas.ToString + "h"
            ElseIf tiempo.TotalDays >= 1 Then
                Dim dias As Integer = Convert.ToInt32(tiempo.TotalDays)
                tb.Text = dias.ToString + "d"
            End If

        End Sub

        'PARTES-------------------------------------------------

        Private Function Retweet(tweet As ITweet)

            Dim recursos As New Resources.ResourceLoader

            Dim spRetweet As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(65, 0, 0, 10)
            }

            Dim iconoRetweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Retweet,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            spRetweet.Children.Add(iconoRetweet)

            Dim usuarioRetweet As New TextBlock With {
                .Text = recursos.GetString("Retweeted") + " " + tweet.CreatedBy.Name,
                .Margin = New Thickness(10, 0, 0, 0),
                .FontSize = 13,
                .VerticalAlignment = VerticalAlignment.Center,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            spRetweet.Children.Add(usuarioRetweet)

            Return spRetweet

        End Function

        Private Function Avatar(tweet As ITweet)

            Dim botonAvatar As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Style = App.Current.Resources("ButtonRevealStyle"),
                .VerticalAlignment = VerticalAlignment.Top,
                .Margin = New Thickness(0, 0, 10, 0),
                .Padding = New Thickness(5, 5, 5, 5),
                .Height = 50,
                .Width = 50
            }

            Dim imagenAvatar As New ImageBrush With {
                .Stretch = Stretch.Uniform
            }

            If tweet.IsRetweet = False Then
                imagenAvatar.ImageSource = New BitmapImage(New Uri(tweet.CreatedBy.ProfileImageUrl))
            Else
                imagenAvatar.ImageSource = New BitmapImage(New Uri(tweet.RetweetedTweet.CreatedBy.ProfileImageUrl))
            End If

            Dim circulo As New Ellipse With {
                .Fill = imagenAvatar,
                .Height = 40,
                .Width = 40
            }

            'botonAvatar.Tag = New Objetos.UsuarioAmpliado(megaUsuario, tweet.Usuario, Nothing)
            botonAvatar.Content = circulo

            'AddHandler botonAvatar.Click, AddressOf UsuarioPulsaBoton
            AddHandler botonAvatar.PointerEntered, AddressOf Entra_Boton_Ellipse
            AddHandler botonAvatar.PointerExited, AddressOf Sale_Boton_Ellipse

            Return botonAvatar

        End Function

        Private Function Usuario(tweet As ITweet)

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim botonUsuario As New Button With {
                .Padding = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0)
            }

            Dim spUsuario As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Padding = New Thickness(5, 5, 5, 5),
                .Background = New SolidColorBrush(Colors.Transparent)
            }

            Dim tb1 As New TextBlock With {
                .FontSize = 14,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim tb2 As New TextBlock With {
                .FontSize = 12,
                .Margin = New Thickness(5, 0, 0, 0),
                .VerticalAlignment = VerticalAlignment.Center,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            If tweet.IsRetweet = False Then
                tb1.Text = tweet.CreatedBy.Name
                tb2.Text = "@" + tweet.CreatedBy.ScreenName
                'botonUsuario.Tag = New Objetos.UsuarioAmpliado(megaUsuario, tweet.Usuario, Nothing)
            Else
                tb1.Text = tweet.RetweetedTweet.CreatedBy.Name
                tb2.Text = "@" + tweet.RetweetedTweet.CreatedBy.ScreenName
                'botonUsuario.Tag = New Objetos.UsuarioAmpliado(megaUsuario, tweet.Retweet.Usuario, Nothing)
            End If

            spUsuario.Children.Add(tb1)
            spUsuario.Children.Add(tb2)

            botonUsuario.Content = spUsuario

            'AddHandler botonUsuario.Click, AddressOf UsuarioPulsaBoton
            'AddHandler botonUsuario.PointerEntered, AddressOf UsuarioEntraBoton
            'AddHandler botonUsuario.PointerExited, AddressOf UsuarioSaleBoton

            sp.Children.Add(botonUsuario)

            '-------------------------------------

            'Dim respuestaUsuarioScreenNombre As String = Nothing

            'If tweet.Retweet Is Nothing Then
            '    If Not tweet.RespuestaUsuarioScreenNombre = Nothing Then
            '        respuestaUsuarioScreenNombre = tweet.RespuestaUsuarioScreenNombre
            '    End If
            'Else
            '    If Not tweet.Retweet.RespuestaUsuarioScreenNombre = Nothing Then
            '        respuestaUsuarioScreenNombre = tweet.Retweet.RespuestaUsuarioScreenNombre
            '    End If
            'End If

            'If Not respuestaUsuarioScreenNombre = Nothing Then
            '    cosas = New Objetos.UsuarioAmpliado(megaUsuario, Nothing, respuestaUsuarioScreenNombre)

            '    Dim recursos As New Resources.ResourceLoader

            '    Dim textoSpanRespuesta As New Span

            '    Dim fragmento As New Run With {
            '        .Text = recursos.GetString("ReplyingTo") + " ",
            '        .Foreground = New SolidColorBrush(Colors.Black)
            '    }

            '    textoSpanRespuesta.Inlines.Add(fragmento)

            '    Dim contenidoEnlace As New Run With {
            '        .Text = "@" + respuestaUsuarioScreenNombre
            '    }

            '    Dim colorRespuesta As New Color

            '    If color = App.Current.Resources("ColorSecundario") Then
            '        colorRespuesta = App.Current.Resources("ColorCuarto")
            '    Else
            '        colorRespuesta = color
            '    End If

            '    Dim enlace As New Hyperlink With {
            '        .TextDecorations = Nothing,
            '        .Foreground = New SolidColorBrush(colorRespuesta)
            '    }

            '    AddHandler enlace.Click, AddressOf EnlaceClick

            '    enlace.Inlines.Add(contenidoEnlace)
            '    textoSpanRespuesta.Inlines.Add(enlace)

            '    Dim tbRespuesta As New TextBlock With {
            '        .TextWrapping = TextWrapping.Wrap,
            '        .Margin = New Thickness(10, 5, 5, 5),
            '        .FontSize = 13,
            '        .VerticalAlignment = VerticalAlignment.Center
            '    }

            '    tbRespuesta.Inlines.Add(textoSpanRespuesta)
            '    sp.Children.Add(tbRespuesta)
            'End If

            Return sp

        End Function

        Public Function Texto(tweet As ITweet)

            Dim textoSpan As New Span
            Dim textoTweet As String = Nothing

            Dim coordenadas1 As Integer = 0
            Dim coordenadas2 As Integer = 0

            If tweet.IsRetweet = False Then
                If Not tweet.FullText = String.Empty Then
                    textoTweet = tweet.FullText
                End If

                If textoTweet = String.Empty Then
                    textoTweet = tweet.Text
                End If

                If Not tweet.DisplayTextRange Is Nothing Then
                    If Not tweet.DisplayTextRange(0) = Nothing Then
                        coordenadas1 = tweet.DisplayTextRange(0)
                    End If

                    If Not tweet.DisplayTextRange(1) = Nothing Then
                        coordenadas2 = tweet.DisplayTextRange(1)
                    End If
                End If
            Else
                If Not tweet.RetweetedTweet.FullText = String.Empty Then
                    textoTweet = tweet.RetweetedTweet.FullText
                End If

                If textoTweet = String.Empty Then
                    textoTweet = tweet.RetweetedTweet.Text
                End If

                If Not tweet.RetweetedTweet.DisplayTextRange Is Nothing Then
                    If Not tweet.RetweetedTweet.DisplayTextRange(0) = Nothing Then
                        coordenadas1 = tweet.RetweetedTweet.DisplayTextRange(0)
                    End If

                    If Not tweet.RetweetedTweet.DisplayTextRange(1) = Nothing Then
                        coordenadas2 = tweet.RetweetedTweet.DisplayTextRange(1)
                    End If
                End If
            End If

            If Not textoTweet = String.Empty Then
                If Not coordenadas1 = coordenadas2 Then
                    textoTweet = textoTweet.Remove(0, coordenadas1)
                End If

                textoTweet = WebUtility.HtmlDecode(textoTweet)

                Dim entidades As Entities.ITweetEntities = Nothing

                If tweet.IsRetweet = False Then
                    entidades = tweet.Entities
                Else
                    entidades = tweet.RetweetedTweet.Entities
                End If

                Dim listaEntidades As New List(Of SustituirTextoTweet)

                For Each entidadUrl In entidades.Urls
                    listaEntidades.Add(New SustituirTextoTweet(entidadUrl.Indices(0), entidadUrl.Indices(1), entidadUrl.DisplayedURL, entidadUrl.ExpandedURL))
                Next

                For Each entidadMencion In entidades.UserMentions
                    listaEntidades.Add(New SustituirTextoTweet(entidadMencion.Indices(0), entidadMencion.Indices(1), entidadMencion.Name, "@" + entidadMencion.ScreenName))
                Next

                For Each entidadHastags In entidades.Hashtags
                    listaEntidades.Add(New SustituirTextoTweet(entidadHastags.Indices(0), entidadHastags.Indices(1), entidadHastags.Text, "#" + entidadHastags.Text))
                Next

                listaEntidades.Sort(Function(x, y) x.posicion1.CompareTo(y.posicion1))

                For Each entidad In listaEntidades
                    If entidad.textoEnlace.Contains("https://") Then
                        If textoTweet.Contains("https://") Then
                            Dim int As Integer = textoTweet.IndexOf("https://")
                            Dim temp As String = textoTweet.Remove(0, int)

                            Dim int2 As Integer = temp.IndexOf(" ")

                            If int2 = -1 Then
                                int2 = temp.Length
                            End If

                            Dim tbTextoAnterior As String = WebUtility.HtmlDecode(textoTweet)

                            If tbTextoAnterior.Trim.Length >= 0 Then
                                Dim textoFragmentoAnterior As String = Nothing

                                If (tbTextoAnterior.Length - int) >= 0 Then
                                    textoFragmentoAnterior = tbTextoAnterior.Remove(int, tbTextoAnterior.Length - int)
                                Else
                                    textoFragmentoAnterior = tbTextoAnterior
                                End If

                                Dim fragmentoAnterior As New Run With {
                                    .Text = textoFragmentoAnterior,
                                    .Foreground = New SolidColorBrush(Colors.White)
                                }

                                textoSpan.Inlines.Add(fragmentoAnterior)
                            End If

                            textoTweet = textoTweet.Remove(0, int + int2)

                            Dim contenidoEnlace As New Run With {
                                .Text = entidad.textoMostrar
                            }

                            Dim enlace As New Hyperlink With {
                                .NavigateUri = New Uri(entidad.textoEnlace),
                                .TextDecorations = Nothing,
                                .Foreground = New SolidColorBrush(App.Current.Resources("ColorTerciario"))
                            }

                            enlace.Inlines.Add(contenidoEnlace)
                            textoSpan.Inlines.Add(enlace)
                        End If
                    Else
                        If textoTweet.ToLower.Contains(entidad.textoEnlace.ToLower) Then
                            textoTweet = WebUtility.HtmlDecode(textoTweet)

                            Dim int As Integer = textoTweet.ToLower.IndexOf(entidad.textoEnlace.ToLower)

                            Dim textoFragmentoAnterior As String = Nothing

                            If (textoTweet.Length - int) >= 0 Then
                                textoFragmentoAnterior = textoTweet.Remove(int, textoTweet.Length - int)
                            Else
                                textoFragmentoAnterior = textoTweet
                            End If

                            Dim fragmentoAnterior As New Run With {
                                .Text = textoFragmentoAnterior,
                                .Foreground = New SolidColorBrush(Colors.White)
                            }

                            textoSpan.Inlines.Add(fragmentoAnterior)

                            textoTweet = textoTweet.Remove(0, int + entidad.textoEnlace.Length)

                            Dim contenidoEnlace As New Run With {
                                .Text = entidad.textoEnlace
                            }

                            If entidad.textoEnlace.Contains("@") Then
                                'cosas = New Objetos.UsuarioAmpliado(megaUsuario, Nothing, Nothing)

                                Dim enlaceUsuario As New Hyperlink With {
                                    .TextDecorations = Nothing,
                                    .Foreground = New SolidColorBrush(App.Current.Resources("ColorTerciario"))
                                }

                                'Dim spUsuario As New StackPanel With {
                                '    .Tag = New Objetos.UsuarioAmpliado(megaUsuario, Nothing, entidad.Mostrar)
                                '}
                                'AddHandler spUsuario.Loaded, AddressOf SpUsuarioLoaded

                                'ToolTipService.SetToolTip(enlaceUsuario, spUsuario)
                                'ToolTipService.SetPlacement(enlaceUsuario, PlacementMode.Bottom)

                                'AddHandler enlaceUsuario.Click, AddressOf EnlaceUsuarioClick

                                enlaceUsuario.Inlines.Add(contenidoEnlace)
                                textoSpan.Inlines.Add(enlaceUsuario)
                            ElseIf entidad.textoEnlace.Contains("#") Then
                                Dim enlaceHashtag As New Hyperlink With {
                                    .TextDecorations = Nothing,
                                    .Foreground = New SolidColorBrush(App.Current.Resources("ColorTerciario"))
                                }

                                'AddHandler enlaceHashtag.Click, AddressOf EnlaceHashtagClick

                                enlaceHashtag.Inlines.Add(contenidoEnlace)
                                textoSpan.Inlines.Add(enlaceHashtag)
                            End If
                        End If
                    End If
                Next

                If textoTweet.Trim.Length >= 0 Then
                    textoTweet = TextoLimpiarEnlaces(textoTweet)

                    If textoTweet.Trim.Length >= 0 Then
                        Dim fragmento As New Run With {
                            .Text = WebUtility.HtmlDecode(textoTweet),
                            .Foreground = New SolidColorBrush(Colors.White)
                        }

                        textoSpan.Inlines.Add(fragmento)
                    End If
                End If

                If textoSpan.Inlines.Count > 0 Then
                    Dim tbTweet As New TextBlock With {
                        .TextWrapping = TextWrapping.Wrap,
                        .Margin = New Thickness(5, 10, 5, 5),
                        .HorizontalAlignment = HorizontalAlignment.Left
                    }

                    tbTweet.Inlines.Add(textoSpan)

                    Return tbTweet
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        End Function

        Private Function TextoLimpiarEnlaces(texto As String)

            Dim i As Integer = 0
            While i < 5
                If texto.Contains("https://") Then
                    Dim temp As String
                    Dim int, int2 As Integer

                    int = texto.IndexOf("https://")
                    temp = texto.Remove(0, int)

                    int2 = temp.IndexOf(" ")

                    If int2 = -1 Then
                        int2 = temp.Length
                    End If

                    texto = texto.Remove(int, int2)
                End If
                i += 1
            End While

            Return texto

        End Function

        Private Function Cita(tweet As ITweet)

            Dim sp As New StackPanel With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .Margin = New Thickness(5, 15, 5, 5),
                .Padding = New Thickness(15, 15, 15, 15),
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                .BorderThickness = New Thickness(1, 1, 1, 1)
            }

            sp.Children.Add(Usuario(tweet.QuotedTweet))

            Dim tbTweet As TextBlock = Texto(tweet.QuotedTweet)

            If Not tbTweet Is Nothing Then
                If tbTweet.Text.Length > 0 Then
                    sp.Children.Add(tbTweet)
                End If
            End If

            sp.Children.Add(Media(tweet.QuotedTweet))

            Return sp

        End Function

        Private Function Media(tweet As ITweet)

            Dim recursos As New Resources.ResourceLoader

            Dim listaMedia As New List(Of Entities.IMediaEntity)

            If tweet.IsRetweet = False Then
                listaMedia = tweet.Media
            Else
                listaMedia = tweet.RetweetedTweet.Media
            End If

            If Not listaMedia Is Nothing Then
                If listaMedia.Count > 0 Then
                    Dim spMedia As New StackPanel With {
                        .Orientation = Orientation.Horizontal,
                        .Margin = New Thickness(5, 15, 0, 5)
                    }

                    For Each itemMedia In listaMedia
                        Dim objetoString As String = Nothing

                        If itemMedia.MediaType = "photo" Then
                            objetoString = itemMedia.MediaType
                        ElseIf itemMedia.MediaType = "video" Then
                            objetoString = itemMedia.MediaType
                        ElseIf itemMedia.MediaType = "animated_gif" Then
                            objetoString = itemMedia.MediaType
                        End If

                        If Not objetoString = Nothing Then
                            Dim gridMedia As New Grid With {
                                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                                .BorderThickness = New Thickness(1, 1, 1, 1),
                                .HorizontalAlignment = HorizontalAlignment.Left,
                                .Margin = New Thickness(0, 0, 10, 0)
                            }

                            Dim pantalla As DisplayInformation = DisplayInformation.GetForCurrentView()
                            Dim tamañoPantalla As Size = New Size(pantalla.ScreenWidthInRawPixels, pantalla.ScreenHeightInRawPixels)

                            Dim imagenMedia As New ImageEx With {
                                .Stretch = Stretch.Uniform,
                                .IsCacheEnabled = True
                            }

                            If listaMedia.Count = 1 Then
                                imagenMedia.MaxWidth = tamañoPantalla.Width / 4
                                imagenMedia.MaxHeight = tamañoPantalla.Height / 1.8
                            ElseIf listaMedia.Count > 1 Then
                                imagenMedia.MaxWidth = tamañoPantalla.Width / 6
                                imagenMedia.MaxHeight = tamañoPantalla.Height / 2
                            End If

                            Dim imagenUrl As String = String.Empty

                            If Not itemMedia.MediaURLHttps = String.Empty Then
                                imagenUrl = itemMedia.MediaURLHttps
                            End If

                            If imagenUrl = String.Empty Then
                                imagenUrl = itemMedia.URL
                            End If

                            Try
                                imagenMedia.Source = New BitmapImage(New Uri(imagenUrl))
                            Catch ex As Exception

                            End Try

                            If objetoString = "photo" Then
                                AddHandler gridMedia.PointerPressed, AddressOf AbrirImagen
                                AddHandler gridMedia.PointerEntered, AddressOf Entra_Boton_Imagen
                                AddHandler gridMedia.PointerExited, AddressOf Sale_Boton_Imagen
                            ElseIf objetoString = "video" Then
                                'Dim listaVideos As TweetVideoVariante() = itemMedia.Video.Variantes
                                'Dim listaOrdenada As New List(Of TweetVideoVariante)

                                'For Each item In listaVideos
                                '    listaOrdenada.Add(item)
                                'Next

                                'listaOrdenada.Sort(Function(x, y) y.Bitrate.CompareTo(x.Bitrate))

                                'datos.Enlace = listaOrdenada(0).Enlace

                                'AddHandler gridMedia.PointerPressed, AddressOf UsuarioClickeaVideo
                                'AddHandler gridMedia.PointerEntered, AddressOf UsuarioEntraVideo
                                'AddHandler gridMedia.PointerExited, AddressOf UsuarioSaleVideo
                            ElseIf objetoString = "animated_gif" Then
                                'datos.Enlace = itemMedia.Video.Variantes(0).Enlace
                                'AddHandler gridMedia.PointerPressed, AddressOf UsuarioClickeaVideo
                                'AddHandler gridMedia.PointerEntered, AddressOf UsuarioEntraVideo
                                'AddHandler gridMedia.PointerExited, AddressOf UsuarioSaleVideo
                            End If

                            'gridMedia.Tag = datos
                            gridMedia.Children.Add(imagenMedia)

                            Dim gridTipo As New Grid With {
                                .HorizontalAlignment = HorizontalAlignment.Left,
                                .VerticalAlignment = VerticalAlignment.Bottom,
                                .Padding = New Thickness(5, 5, 5, 5),
                                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
                            }

                            If Not objetoString = "animated_gif" Then
                                Dim iconoTipo As New FontAwesome5.FontAwesome With {
                                    .Foreground = New SolidColorBrush(Colors.White),
                                    .FontSize = 12
                                }

                                If objetoString = "video" Then
                                    iconoTipo.Icon = FontAwesome5.EFontAwesomeIcon.Solid_Video
                                ElseIf objetoString = "photo" Then
                                    iconoTipo.Icon = FontAwesome5.EFontAwesomeIcon.Solid_Image
                                End If

                                gridTipo.Children.Add(iconoTipo)
                            Else
                                Dim tbTipo As New TextBlock With {
                                    .Foreground = New SolidColorBrush(Colors.White),
                                    .FontSize = 12,
                                    .Text = "gif"
                                }

                                gridTipo.Children.Add(tbTipo)
                            End If

                            gridMedia.Children.Add(gridTipo)

                            'If ApplicationData.Current.LocalSettings.Values("tooltipsayuda") = True Then
                            '    ToolTipService.SetToolTip(gridMedia, recursos.GetString("ClickExpand"))
                            '    ToolTipService.SetPlacement(gridMedia, PlacementMode.Bottom)
                            'End If

                            spMedia.Children.Add(gridMedia)
                        End If
                    Next

                    If spMedia.Children.Count > 0 Then
                        Return spMedia
                    End If
                End If
            End If

            Return New StackPanel
        End Function

        Private Sub AbrirImagen(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spUsuarioBotones As StackPanel = pagina.FindName("spUsuarioBotones")
            spUsuarioBotones.Visibility = Visibility.Visible

            Dim botonImagen As Button = pagina.FindName("botonUsuarioImagen")
            botonImagen.Visibility = Visibility.Visible

            Dim gridTweets As Grid = pagina.FindName("gridUsuarioImagen")
            Pestañas.Visibilidad_Pestañas_Usuario(botonImagen, gridTweets)

            Dim gridMedia As Grid = sender
            Dim imagenOrigen As ImageEx = gridMedia.Children(0)

            Dim imagenMostrar As ImageEx = pagina.FindName("imagenUsuario")
            imagenMostrar.Source = imagenOrigen.Source

        End Sub

    End Module

    Public Class SustituirTextoTweet

        Public posicion1 As Integer
        Public posicion2 As Integer
        Public textoMostrar As String
        Public textoEnlace As String

        Public Sub New(ByVal posicion1 As Integer, ByVal posicion2 As Integer, ByVal textomostrar As String, ByVal textoenlace As String)
            Me.posicion1 = posicion1
            Me.posicion2 = posicion2
            Me.textoMostrar = textomostrar
            Me.textoEnlace = textoenlace
        End Sub

    End Class
End Namespace