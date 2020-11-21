Imports System.Net
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Tweetinvi
Imports Tweetinvi.Models
Imports Windows.ApplicationModel.Core
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Media.Core
Imports Windows.Networking.BackgroundTransfer
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.System
Imports Windows.System.Threading
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Documents
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module Tweets

        Public Function GenerarTweet(cliente As TwitterClient, tweet As ITweet, ampliar As Boolean)

            If Not tweet Is Nothing Then
                Dim recursos As New Resources.ResourceLoader

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
                    .Orientation = Orientation.Vertical,
                    .HorizontalAlignment = HorizontalAlignment.Center
                }

                spIzquierda.SetValue(Grid.ColumnProperty, 0)

                spIzquierda.Children.Add(Avatar(cliente, tweet))

                If ampliar = True Then
                    Dim iconoResponder As New FontAwesome5.FontAwesome With {
                        .Foreground = New SolidColorBrush(Colors.White),
                        .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Comment
                    }

                    Dim botonResponder As New Button With {
                        .Padding = New Thickness(0, 0, 0, 0),
                        .Background = New SolidColorBrush(Colors.Transparent),
                        .BorderThickness = New Thickness(0, 0, 0, 0),
                        .Style = App.Current.Resources("ButtonRevealStyle"),
                        .Content = iconoResponder,
                        .Visibility = Visibility.Collapsed,
                        .Margin = New Thickness(0, 20, 5, 0),
                        .Tag = tweet,
                        .HorizontalAlignment = HorizontalAlignment.Center
                    }

                    ToolTipService.SetToolTip(botonResponder, recursos.GetString("Reply"))
                    ToolTipService.SetPlacement(botonResponder, PlacementMode.Bottom)

                    AddHandler botonResponder.Click, AddressOf Enviar.Responder
                    AddHandler botonResponder.PointerEntered, AddressOf Entra_Boton_Icono
                    AddHandler botonResponder.PointerExited, AddressOf Sale_Boton_Icono

                    spIzquierda.Children.Add(botonResponder)

                    Dim iconoAmpliar As New FontAwesome5.FontAwesome With {
                        .Foreground = New SolidColorBrush(Colors.White),
                        .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Eye
                    }

                    Dim botonAmpliar As New Button With {
                        .Padding = New Thickness(0, 0, 0, 0),
                        .Background = New SolidColorBrush(Colors.Transparent),
                        .BorderThickness = New Thickness(0, 0, 0, 0),
                        .Style = App.Current.Resources("ButtonRevealStyle"),
                        .Content = iconoAmpliar,
                        .Visibility = Visibility.Collapsed,
                        .Margin = New Thickness(0, 15, 5, 0),
                        .Tag = tweet,
                        .HorizontalAlignment = HorizontalAlignment.Center
                    }

                    ToolTipService.SetToolTip(botonAmpliar, recursos.GetString("Open"))
                    ToolTipService.SetPlacement(botonAmpliar, PlacementMode.Bottom)

                    ' AddHandler botonResponder.Click, AddressOf Enviar.Responder
                    AddHandler botonAmpliar.PointerEntered, AddressOf Entra_Boton_Icono
                    AddHandler botonAmpliar.PointerExited, AddressOf Sale_Boton_Icono

                    spIzquierda.Children.Add(botonAmpliar)
                End If

                gridInferior.Children.Add(spIzquierda)

                '-----------------------------

                Dim spInferiorCentro As New StackPanel With {
                    .Orientation = Orientation.Vertical
                }

                spInferiorCentro.SetValue(Grid.ColumnProperty, 1)

                spInferiorCentro.Children.Add(Usuario(cliente, tweet))

                Dim tbTweet As TextBlock = Texto(tweet)

                If Not tbTweet Is Nothing Then
                    If tbTweet.Text.Length > 0 Then
                        spInferiorCentro.Children.Add(tbTweet)
                    End If
                End If

                If Not tweet.QuotedTweet Is Nothing Then
                    If tweet.IsRetweet = False Then
                        spInferiorCentro.Children.Add(Cita(cliente, tweet))
                    Else
                        spInferiorCentro.Children.Add(Cita(cliente, tweet.RetweetedTweet))
                    End If
                End If

                spInferiorCentro.Children.Add(Media(tweet))

                gridInferior.Children.Add(spInferiorCentro)

                '-----------------------------

                Dim gridInferiorDerecha As New Grid With {
                    .HorizontalAlignment = HorizontalAlignment.Right,
                    .Margin = New Thickness(10, 5, 25, 0),
                    .Width = 50
                }

                gridInferiorDerecha.SetValue(Grid.ColumnProperty, 2)

                Dim spDerecha As New StackPanel With {
                    .Orientation = Orientation.Vertical,
                    .HorizontalAlignment = HorizontalAlignment.Right
                }

                Dim tbTiempo As New TextBlock With {
                    .FontSize = 13,
                    .Foreground = New SolidColorBrush(Colors.White),
                    .HorizontalAlignment = HorizontalAlignment.Right,
                    .Margin = New Thickness(0, 0, 5, 0)
                }

                SumarTiempo(tbTiempo, tweet.CreatedAt.LocalDateTime)

                spDerecha.Children.Add(tbTiempo)

                spDerecha.Children.Add(Botones(cliente, tweet))

                gridInferiorDerecha.Children.Add(spDerecha)

                '-----------------------------

                gridInferior.Children.Add(gridInferiorDerecha)

                '-----------------------------

                grid.Children.Add(gridInferior)

                AddHandler grid.PointerEntered, AddressOf Entra_Tweet
                AddHandler grid.PointerExited, AddressOf Sale_Tweet

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

        Private Function Avatar(cliente As TwitterClient, tweet As ITweet)

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

            botonAvatar.Tag = New ClienteyTweet(cliente, tweet)
            botonAvatar.Content = circulo

            AddHandler botonAvatar.Click, AddressOf OtroUsuario.CargarClick
            AddHandler botonAvatar.PointerEntered, AddressOf Entra_Boton_Ellipse
            AddHandler botonAvatar.PointerExited, AddressOf Sale_Boton_Ellipse

            Return botonAvatar

        End Function

        Private Function Usuario(cliente As TwitterClient, tweet As ITweet)

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim botonUsuario As New Button With {
                .Padding = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Style = App.Current.Resources("ButtonRevealStyle")
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
                botonUsuario.Tag = New ClienteyTweet(cliente, tweet)
            Else
                tb1.Text = tweet.RetweetedTweet.CreatedBy.Name
                tb2.Text = "@" + tweet.RetweetedTweet.CreatedBy.ScreenName
                botonUsuario.Tag = New ClienteyTweet(cliente, tweet.RetweetedTweet)
            End If

            spUsuario.Children.Add(tb1)
            spUsuario.Children.Add(tb2)

            botonUsuario.Content = spUsuario

            AddHandler botonUsuario.Click, AddressOf CargarClick
            AddHandler botonUsuario.PointerEntered, AddressOf Entra_Basico
            AddHandler botonUsuario.PointerExited, AddressOf Sale_Basico

            sp.Children.Add(botonUsuario)

            '-------------------------------------

            Dim respuestaUsuarioScreenNombre As String = Nothing

            If tweet.IsRetweet = False Then
                If Not tweet.InReplyToScreenName = Nothing Then
                    respuestaUsuarioScreenNombre = tweet.InReplyToScreenName
                End If
            Else
                If Not tweet.RetweetedTweet.InReplyToScreenName = Nothing Then
                    respuestaUsuarioScreenNombre = tweet.RetweetedTweet.InReplyToScreenName
                End If
            End If

            If Not respuestaUsuarioScreenNombre = Nothing Then
                Dim recursos As New Resources.ResourceLoader

                Dim textoSpanRespuesta As New Span

                Dim fragmento As New Run With {
                    .Text = recursos.GetString("ReplyingTo") + " ",
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                textoSpanRespuesta.Inlines.Add(fragmento)

                Dim contenidoEnlace As New Run With {
                    .Text = "@" + respuestaUsuarioScreenNombre
                }

                Dim enlace As New Hyperlink With {
                    .TextDecorations = Nothing,
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                AddHandler enlace.Click, AddressOf CargarClick2

                enlace.Inlines.Add(contenidoEnlace)
                textoSpanRespuesta.Inlines.Add(enlace)

                Dim tbRespuesta As New TextBlock With {
                    .TextWrapping = TextWrapping.Wrap,
                    .Margin = New Thickness(10, 5, 5, 5),
                    .FontSize = 13,
                    .VerticalAlignment = VerticalAlignment.Center
                }

                tbRespuesta.Inlines.Add(textoSpanRespuesta)
                sp.Children.Add(tbRespuesta)
            End If

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
                                Dim enlaceUsuario As New Hyperlink With {
                                    .TextDecorations = Nothing,
                                    .Foreground = New SolidColorBrush(App.Current.Resources("ColorTerciario"))
                                }

                                AddHandler enlaceUsuario.Click, AddressOf OtroUsuario.CargarClick2

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

        Private Function Cita(cliente As TwitterClient, tweet As ITweet)

            Dim sp As New StackPanel With {
                .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
                .Margin = New Thickness(5, 15, 5, 5),
                .Padding = New Thickness(15, 15, 15, 0),
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                .BorderThickness = New Thickness(1, 1, 1, 1)
            }

            sp.Children.Add(Usuario(cliente, tweet.QuotedTweet))

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
                    Dim gvMedia As New GridView With {
                        .Margin = New Thickness(5, 15, 0, 0)
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
                                .Margin = New Thickness(0, 0, 5, 5)
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
                                imagenMedia.MaxWidth = tamañoPantalla.Width / 6.5
                                imagenMedia.MaxHeight = tamañoPantalla.Height / 3
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
                                imagenMedia.Tag = imagenUrl
                            Catch ex As Exception

                            End Try

                            If objetoString = "photo" Then
                                ToolTipService.SetToolTip(gridMedia, recursos.GetString("ClickExpandImage"))
                                ToolTipService.SetPlacement(gridMedia, PlacementMode.Bottom)

                                AddHandler gridMedia.PointerPressed, AddressOf AbrirImagen
                                AddHandler gridMedia.PointerEntered, AddressOf Entra_Boton_Imagen
                                AddHandler gridMedia.PointerExited, AddressOf Sale_Boton_Imagen
                            ElseIf objetoString = "video" Then
                                Dim listaVideos As Entities.ExtendedEntities.IVideoEntityVariant() = itemMedia.VideoDetails.Variants
                                Dim listaOrdenada As New List(Of Entities.ExtendedEntities.IVideoEntityVariant)

                                For Each item In listaVideos
                                    listaOrdenada.Add(item)
                                Next

                                listaOrdenada.Sort(Function(x, y) y.Bitrate.CompareTo(x.Bitrate))

                                gridMedia.Tag = listaOrdenada(0).URL

                                ToolTipService.SetToolTip(gridMedia, recursos.GetString("ClickExpandVideo"))
                                ToolTipService.SetPlacement(gridMedia, PlacementMode.Bottom)

                                AddHandler gridMedia.PointerPressed, AddressOf AbrirVideo
                                AddHandler gridMedia.PointerEntered, AddressOf Entra_Boton_Imagen
                                AddHandler gridMedia.PointerExited, AddressOf Sale_Boton_Imagen
                            ElseIf objetoString = "animated_gif" Then
                                Dim listaVideos As Entities.ExtendedEntities.IVideoEntityVariant() = itemMedia.VideoDetails.Variants
                                Dim listaOrdenada As New List(Of Entities.ExtendedEntities.IVideoEntityVariant)

                                For Each item In listaVideos
                                    listaOrdenada.Add(item)
                                Next

                                listaOrdenada.Sort(Function(x, y) y.Bitrate.CompareTo(x.Bitrate))

                                gridMedia.Tag = listaOrdenada(0).URL

                                ToolTipService.SetToolTip(gridMedia, recursos.GetString("ClickExpandGif"))
                                ToolTipService.SetPlacement(gridMedia, PlacementMode.Bottom)

                                AddHandler gridMedia.PointerPressed, AddressOf AbrirVideo
                                AddHandler gridMedia.PointerEntered, AddressOf Entra_Video
                                AddHandler gridMedia.PointerExited, AddressOf Sale_Video
                            End If

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

                            gvMedia.Items.Add(gridMedia)
                        End If
                    Next

                    If gvMedia.Items.Count > 0 Then
                        Return gvMedia
                    End If
                End If
            End If

            Return New StackPanel
        End Function

        Private Sub AbrirImagen(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")
            gridUsuarioBotones.Visibility = Visibility.Visible

            Dim botonImagen As Button = pagina.FindName("botonUsuarioImagen")
            botonImagen.Visibility = Visibility.Visible

            Dim gridImagen As Grid = pagina.FindName("gridUsuarioImagen")
            Pestañas.Visibilidad_Pestañas_Usuario(botonImagen, gridImagen)

            Dim gridMedia As Grid = sender
            Dim imagenOrigen As ImageEx = gridMedia.Children(0)

            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("imagenAmpliada", gridMedia)

            Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imagenAmpliada")

            If Not animacion Is Nothing Then
                animacion.TryStart(gridImagen)
            End If

            Dim imagenMostrar As ImageEx = pagina.FindName("imagenUsuario")
            imagenMostrar.Source = imagenOrigen.Source
            imagenMostrar.Tag = imagenOrigen.Tag

            Dim botonCopiar As Button = pagina.FindName("botonUsuarioImagenCopiar")
            RemoveHandler botonCopiar.Click, AddressOf CopiarImagen
            AddHandler botonCopiar.Click, AddressOf CopiarImagen

            RemoveHandler botonCopiar.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonCopiar.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonCopiar.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonCopiar.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonDescargar As Button = pagina.FindName("botonUsuarioImagenDescargar")
            RemoveHandler botonDescargar.Click, AddressOf DescargarImagen
            AddHandler botonDescargar.Click, AddressOf DescargarImagen

            RemoveHandler botonDescargar.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonDescargar.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonDescargar.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonDescargar.PointerExited, AddressOf Sale_Boton_Icono

        End Sub

        Private Sub CopiarImagen(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim imagen As ImageEx = pagina.FindName("imagenUsuario")

            Dim paquete As New DataPackage
            paquete.SetText(imagen.Tag.ToString)
            Clipboard.SetContent(paquete)

        End Sub

        Private Async Sub DescargarImagen(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim imagen As ImageEx = pagina.FindName("imagenUsuario")

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim enlace As New Uri(imagen.Tag.ToString)

            Dim picker As New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            Try
                Dim carpeta As StorageFolder = Await picker.PickSingleFolderAsync()
                Dim fichero As StorageFile = Await carpeta.CreateFileAsync("twitter.jpg", CreationCollisionOption.ReplaceExisting)
                Dim descargador As New BackgroundDownloader
                Dim descarga As DownloadOperation = descargador.CreateDownload(enlace, fichero)
                Await descarga.StartAsync
            Catch ex As Exception

            End Try

            boton.IsEnabled = True

        End Sub

        Private Sub AbrirVideo(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim gridUsuarioBotones As Grid = pagina.FindName("gridUsuarioBotones")
            gridUsuarioBotones.Visibility = Visibility.Visible

            Dim botonVideo As Button = pagina.FindName("botonUsuarioVideo")
            botonVideo.Visibility = Visibility.Visible

            Dim gridVideo As Grid = pagina.FindName("gridUsuarioVideo")
            Pestañas.Visibilidad_Pestañas_Usuario(botonVideo, gridVideo)

            Dim gridMedia As Grid = sender
            Dim enlaceVideo As String = gridMedia.Tag

            Dim videoReproductor As MediaPlayerElement = pagina.FindName("videoUsuario")

            Try
                videoReproductor.Source = MediaSource.CreateFromUri(New Uri(enlaceVideo))
                videoReproductor.Tag = enlaceVideo
                videoReproductor.MediaPlayer.Play()
            Catch ex As Exception

            End Try

            Dim botonCopiar As Button = pagina.FindName("botonUsuarioVideoCopiar")
            RemoveHandler botonCopiar.Click, AddressOf CopiarVideo
            AddHandler botonCopiar.Click, AddressOf CopiarVideo

            RemoveHandler botonCopiar.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonCopiar.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonCopiar.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonCopiar.PointerExited, AddressOf Sale_Boton_Icono

            Dim botonDescargar As Button = pagina.FindName("botonUsuarioVideoDescargar")
            RemoveHandler botonDescargar.Click, AddressOf DescargarVideo
            AddHandler botonDescargar.Click, AddressOf DescargarVideo

            RemoveHandler botonDescargar.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonDescargar.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonDescargar.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonDescargar.PointerExited, AddressOf Sale_Boton_Icono

        End Sub

        Private Sub CopiarVideo(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim video As MediaPlayerElement = pagina.FindName("videoUsuario")
            Dim fuente As MediaSource = video.Source

            Dim paquete As New DataPackage
            paquete.SetText(fuente.Uri.ToString)
            Clipboard.SetContent(paquete)

        End Sub

        Private Async Sub DescargarVideo(sender As Object, e As RoutedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim video As MediaPlayerElement = pagina.FindName("videoUsuario")
            Dim fuente As MediaSource = video.Source
            Dim enlace As New Uri(fuente.Uri.ToString)

            Dim picker As New FolderPicker()

            picker.FileTypeFilter.Add("*")
            picker.ViewMode = PickerViewMode.List

            Try
                Dim carpeta As StorageFolder = Await picker.PickSingleFolderAsync()
                Dim fichero As StorageFile = Await carpeta.CreateFileAsync("twitter.mp4", CreationCollisionOption.ReplaceExisting)
                Dim descargador As New BackgroundDownloader
                Dim descarga As DownloadOperation = descargador.CreateDownload(enlace, fichero)
                Await descarga.StartAsync
            Catch ex As Exception

            End Try

            boton.IsEnabled = True

        End Sub

        Private Sub Entra_Tweet(sender As Object, e As PointerRoutedEventArgs)

            Dim grid As Grid = sender
            Dim subgrid As Grid = grid.Children(1)
            Dim sp2 As StackPanel = subgrid.Children(0)
            Dim boton As Button = sp2.Children(1)

            If Not boton Is Nothing Then
                boton.Visibility = Visibility.Visible
            End If

            Dim boton2 As Button = sp2.Children(2)

            If Not boton2 Is Nothing Then
                boton2.Visibility = Visibility.Visible
            End If

            Dim subgrid2 As Grid = subgrid.Children(subgrid.Children.Count - 1)
            Dim sp As StackPanel = subgrid2.Children(0)
            Dim spBotones As StackPanel = sp.Children(1)
            spBotones.Visibility = Visibility.Visible

        End Sub

        Private Sub Sale_Tweet(sender As Object, e As PointerRoutedEventArgs)

            Dim grid As Grid = sender
            Dim subgrid As Grid = grid.Children(1)
            Dim sp2 As StackPanel = subgrid.Children(0)
            Dim boton As Button = sp2.Children(1)

            If Not boton Is Nothing Then
                boton.Visibility = Visibility.Collapsed
            End If

            Dim boton2 As Button = sp2.Children(2)

            If Not boton2 Is Nothing Then
                boton2.Visibility = Visibility.Collapsed
            End If

            Dim subgrid2 As Grid = subgrid.Children(subgrid.Children.Count - 1)
            Dim sp As StackPanel = subgrid2.Children(0)
            Dim spBotones As StackPanel = sp.Children(1)
            spBotones.Visibility = Visibility.Collapsed

        End Sub

        Private Function Botones(cliente As TwitterClient, tweet As ITweet)

            Dim spBotones As New StackPanel With {
                .Orientation = Orientation.Vertical,
                .Margin = New Thickness(10, 25, 0, 0),
                .Visibility = Visibility.Collapsed
            }

            Dim iconoRetweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Retweet,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            If tweet.Retweeted = True Then
                iconoRetweet.Foreground = New SolidColorBrush(Colors.LightGreen)
            End If

            Dim botonRetweet As New Button With {
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Style = App.Current.Resources("ButtonRevealStyle"),
                .Content = iconoRetweet,
                .Tag = New ClienteyTweet(cliente, tweet),
                .HorizontalAlignment = HorizontalAlignment.Center
            }

            AddHandler botonRetweet.Click, AddressOf RetweetClick
            AddHandler botonRetweet.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonRetweet.PointerExited, AddressOf Sale_Boton_Icono

            spBotones.Children.Add(botonRetweet)

            '------------------------------------------

            Dim iconoFavorito As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Heart,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            If tweet.Favorited = True Then
                iconoFavorito.Foreground = New SolidColorBrush(Colors.IndianRed)
            End If

            Dim botonFavorito As New Button With {
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(0, 10, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Style = App.Current.Resources("ButtonRevealStyle"),
                .Content = iconoFavorito,
                .Tag = New ClienteyTweet(cliente, tweet),
                .HorizontalAlignment = HorizontalAlignment.Center
            }

            AddHandler botonFavorito.Click, AddressOf FavoritoClick
            AddHandler botonFavorito.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonFavorito.PointerExited, AddressOf Sale_Boton_Icono

            spBotones.Children.Add(botonFavorito)

            '------------------------------------------

            Dim iconoMasOpciones As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_EllipsisH,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim botonMasOpciones As New Button With {
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(0, 10, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Style = App.Current.Resources("ButtonRevealStyle"),
                .Content = iconoMasOpciones,
                .Tag = New ClienteyTweet(cliente, tweet),
                .HorizontalAlignment = HorizontalAlignment.Center
            }

            AddHandler botonMasOpciones.Click, AddressOf MasOpcionesClick
            AddHandler botonMasOpciones.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonMasOpciones.PointerExited, AddressOf Sale_Boton_Icono

            spBotones.Children.Add(botonMasOpciones)

            Return spBotones

        End Function

        Private Async Sub RetweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As FontAwesome5.FontAwesome = boton.Content
            Dim clienteyTweet As ClienteyTweet = boton.Tag

            Dim cliente As TwitterClient = clienteyTweet.cliente
            Dim tweet As ITweet = clienteyTweet.tweet

            If tweet.Retweeted = False Then
                icono.Foreground = New SolidColorBrush(Colors.LightGreen)
                Await cliente.Tweets.PublishRetweetAsync(tweet.Id)
            Else
                icono.Foreground = New SolidColorBrush(Colors.White)
                Await cliente.Tweets.DestroyRetweetAsync(tweet.Id)
            End If

        End Sub

        Private Async Sub FavoritoClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim icono As FontAwesome5.FontAwesome = boton.Content
            Dim clienteyTweet As ClienteyTweet = boton.Tag

            Dim cliente As TwitterClient = clienteyTweet.cliente
            Dim tweet As ITweet = clienteyTweet.tweet

            If tweet.Favorited = False Then
                icono.Foreground = New SolidColorBrush(Colors.IndianRed)
                Await cliente.Tweets.FavoriteTweetAsync(tweet.Id)
            Else
                icono.Foreground = New SolidColorBrush(Colors.White)
                Await cliente.Tweets.UnfavoriteTweetAsync(tweet.Id)
            End If

        End Sub

        Private Sub MasOpcionesClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim boton As Button = sender
            Dim clienteyTweet As ClienteyTweet = boton.Tag

            Dim menu As New MenuFlyout With {
                .Placement = FlyoutPlacementMode.Bottom,
                .ShowMode = FlyoutShowMode.Transient
            }

            Dim iconoCompartirTweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_ShareAlt,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonCompartirTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("ShareTweet"),
                .Icon = iconoCompartirTweet,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = clienteyTweet
            }

            AddHandler botonCompartirTweet.Click, AddressOf CompartirTweetClick
            AddHandler botonCompartirTweet.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonCompartirTweet.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonCompartirTweet)

            Dim iconoCopiarEnlaceTweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_Copy,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonCopiarEnlaceTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("CopyUrl2"),
                .Icon = iconoCopiarEnlaceTweet,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = clienteyTweet
            }

            AddHandler botonCopiarEnlaceTweet.Click, AddressOf CopiarEnlaceTweetClick
            AddHandler botonCopiarEnlaceTweet.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonCopiarEnlaceTweet.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonCopiarEnlaceTweet)

            Dim separador As New MenuFlyoutSeparator
            menu.Items.Add(separador)

            Dim iconoAbrirNavegadorTweet As New FontAwesome5.FontAwesome With {
                .Icon = FontAwesome5.EFontAwesomeIcon.Brands_Edge,
                .Foreground = New SolidColorBrush(Colors.Black)
            }

            Dim botonAbrirNavegadorTweet As New MenuFlyoutItem With {
                .Text = recursos.GetString("OpenWebBrowser"),
                .Icon = iconoAbrirNavegadorTweet,
                .Foreground = New SolidColorBrush(Colors.Black),
                .Tag = clienteyTweet
            }

            AddHandler botonAbrirNavegadorTweet.Click, AddressOf AbrirNavegadorTweetClick
            AddHandler botonAbrirNavegadorTweet.PointerEntered, AddressOf Entra_MFItem_Icono
            AddHandler botonAbrirNavegadorTweet.PointerExited, AddressOf Sale_MFItem_Icono
            menu.Items.Add(botonAbrirNavegadorTweet)

            FlyoutBase.SetAttachedFlyout(boton, menu)
            menu.ShowAt(boton)

        End Sub

        Private Sub CompartirTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim clienteyTweet As ClienteyTweet = boton.Tag

            Dim cliente As TwitterClient = clienteyTweet.cliente
            Dim tweet As ITweet = clienteyTweet.tweet

            If tweet.IsRetweet = False Then
                ApplicationData.Current.LocalSettings.Values("TweetCompartirTitulo") = "@" + tweet.CreatedBy.ScreenName
                ApplicationData.Current.LocalSettings.Values("TweetCompartirDescripcion") = WebUtility.HtmlDecode(tweet.FullText)
                ApplicationData.Current.LocalSettings.Values("TweetCompartirEnlace") = "https://twitter.com/" + tweet.CreatedBy.ScreenName + "/status/" + tweet.Id.ToString
            Else
                ApplicationData.Current.LocalSettings.Values("TweetCompartirTitulo") = "@" + tweet.RetweetedTweet.CreatedBy.ScreenName
                ApplicationData.Current.LocalSettings.Values("TweetCompartirDescripcion") = WebUtility.HtmlDecode(tweet.RetweetedTweet.FullText)
                ApplicationData.Current.LocalSettings.Values("TweetCompartirEnlace") = "https://twitter.com/" + tweet.RetweetedTweet.CreatedBy.ScreenName + "/status/" + tweet.RetweetedTweet.Id.ToString
            End If

            Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
            AddHandler datos.DataRequested, AddressOf DatosCompartirClick

            DataTransferManager.ShowShareUI()

        End Sub

        Private Sub DatosCompartirClick(sender As Object, e As DataRequestedEventArgs)

            Dim request As DataRequest = e.Request

            request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("TweetCompartirTitulo")
            request.Data.Properties.Description = ApplicationData.Current.LocalSettings.Values("TweetCompartirDescripcion")
            request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("TweetCompartirEnlace")))

        End Sub

        Private Sub CopiarEnlaceTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As ClienteyTweet = boton.Tag

            Dim cliente As TwitterClient = cosas.cliente
            Dim tweet As ITweet = cosas.tweet

            Dim texto As New DataPackage

            If tweet.IsRetweet = False Then
                texto.SetText("https://twitter.com/" + tweet.CreatedBy.ScreenName + "/status/" + tweet.Id.ToString)
            Else
                texto.SetText("https://twitter.com/" + tweet.RetweetedTweet.CreatedBy.ScreenName + "/status/" + tweet.RetweetedTweet.Id.ToString)
            End If

            Clipboard.SetContent(texto)

        End Sub

        Private Async Sub AbrirNavegadorTweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As MenuFlyoutItem = sender
            Dim cosas As ClienteyTweet = boton.Tag

            Dim cliente As TwitterClient = cosas.cliente
            Dim tweet As ITweet = cosas.tweet

            If tweet.IsRetweet = False Then
                Try
                    Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/" + tweet.CreatedBy.ScreenName + "/status/" + tweet.Id.ToString))
                Catch ex As Exception

                End Try
            Else
                Try
                    Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/" + tweet.RetweetedTweet.CreatedBy.ScreenName + "/status/" + tweet.RetweetedTweet.Id.ToString))
                Catch ex As Exception

                End Try
            End If

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

    Public Class ClienteyTweet

        Public cliente As TwitterClient
        Public tweet As ITweet

        Public Sub New(ByVal cliente As TwitterClient, ByVal tweet As ITweet)
            Me.cliente = cliente
            Me.tweet = tweet
        End Sub

    End Class
End Namespace