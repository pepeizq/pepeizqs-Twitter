Imports Microsoft.Toolkit.Uwp.Notifications
Imports Windows.Storage
Imports Windows.UI.Notifications
Imports Windows.UI.Popups
Imports System.Text
Imports Tweetinvi.Models

Module Notificaciones

    Public Async Sub MessageBox(contenido As String)

        Try
            Dim messageDialog As New MessageDialog(contenido)
            Await messageDialog.ShowAsync()
        Catch ex As Exception

        End Try

    End Sub

    Public Sub Toast(titulo As String)

        Dim texto As New AdaptiveText With {
           .Text = titulo,
           .HintMaxLines = 4
        }

        Dim logo As New ToastGenericAppLogo With {
           .Source = "ms-appx:///Assets/logo2.png"
        }

        Dim contenido As New ToastBindingGeneric With {
            .AppLogoOverride = logo
        }

        contenido.Children.Add(texto)

        Dim tostadaVisual As New ToastVisual With {
            .BindingGeneric = contenido
        }

        Dim tostada As New ToastContent With {
            .Launch = titulo,
            .Visual = tostadaVisual
        }

        Try
            Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml) With {
                .ExpirationTime = DateTime.Now.AddSeconds(20)
            }

            Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
            notificador.Show(notificacion)
        Catch ex As Exception

        End Try

    End Sub

    Public Sub ToastTweet(tweet As ITweet, segundos As Integer)

        Dim mostrar As Boolean = True

        If Not ApplicationData.Current.LocalSettings.Values("ultimoTweet") = Nothing Then
            If ApplicationData.Current.LocalSettings.Values("ultimoTweet") = tweet.Id Then
                mostrar = False
            End If
        End If

        If mostrar = True Then
            ApplicationData.Current.LocalSettings.Values("ultimoTweet") = tweet.Id

            Dim tb As TextBlock = Interfaz.Tweet.Texto(tweet)

            If Not tb.Text = String.Empty Then
                Dim textoTweet As New AdaptiveText With {
                    .Text = tb.Text,
                    .HintMaxLines = 4
                }

                Dim cuenta As String = Nothing

                If tweet.IsRetweet = False Then
                    cuenta = tweet.CreatedBy.Name
                Else
                    cuenta = tweet.RetweetedTweet.CreatedBy.Name
                End If

                Dim textoCuenta As New AdaptiveText With {
                    .Text = cuenta,
                    .HintMaxLines = 1
                }

                Dim avatarUrl As String = Nothing

                If tweet.IsRetweet = False Then
                    avatarUrl = tweet.CreatedBy.ProfileImageUrl
                Else
                    avatarUrl = tweet.RetweetedTweet.CreatedBy.ProfileImageUrl
                End If

                Dim logo As New ToastGenericAppLogo With {
                    .Source = avatarUrl,
                    .HintCrop = ToastGenericAppLogoCrop.Circle
                }

                Dim hero As ToastGenericHeroImage = Nothing

                If Not tweet.Entities Is Nothing Then
                    Dim tweetMedia As List(Of Entities.IMediaEntity) = tweet.Media

                    If tweetMedia.Count > 0 Then
                        If Not tweetMedia(0).ExpandedURL = Nothing Then
                            hero = New ToastGenericHeroImage With {
                                .Source = tweetMedia(0).ExpandedURL
                            }
                        End If
                    End If
                End If

                Dim atribucion As ToastGenericAttributionText = Nothing

                'If ApplicationData.Current.LocalSettings.Values("notificacionUsuario") = True Then
                '    atribucion = New ToastGenericAttributionText With {
                '        .Text = "@" + megaUsuario.Usuario.ScreenNombre
                '    }
                'End If

                Dim contenido As New ToastBindingGeneric With {
                    .AppLogoOverride = logo
                }

                If Not atribucion Is Nothing Then
                    contenido.Attribution = atribucion
                End If

                contenido.Children.Add(textoTweet)
                contenido.Children.Add(textoCuenta)

                If Not hero Is Nothing Then
                    contenido.HeroImage = hero
                End If

                Dim tostadaVisual As New ToastVisual With {
                    .BindingGeneric = contenido
                }

                Dim recursos As New Resources.ResourceLoader

                Dim botonAbrir As ToastButton = Nothing

                If Not tweet.Entities.Urls Is Nothing Then
                    Dim urlFinal As String = Nothing

                    For Each url In tweet.Entities.Urls
                        If Not url Is Nothing Then
                            urlFinal = url.ExpandedURL
                        End If
                    Next

                    If Not urlFinal = Nothing Then
                        botonAbrir = New ToastButton(recursos.GetString("Open"), urlFinal)
                    End If
                End If

                Dim tostadaAcciones As New ToastActionsCustom

                If Not botonAbrir Is Nothing Then
                    tostadaAcciones.Buttons.Add(botonAbrir)
                End If

                Dim tostadaAudio As New ToastAudio

                If ApplicationData.Current.LocalSettings.Values("notificacionSonido") = False Then
                    tostadaAudio.Silent = True
                Else
                    tostadaAudio.Silent = False

                    If ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = Nothing Then
                        tostadaAudio.Src = New Uri("ms-winsoundevent:Notification.Default")
                    Else
                        tostadaAudio.Src = New Uri(ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido"))
                    End If
                End If

                Dim bytesTexto() As Byte = Encoding.Default.GetBytes(tweet.FullText)
                Dim textoFinal As String = Encoding.UTF8.GetString(bytesTexto)

                Dim tostada As New ToastContent With {
                    .Launch = textoFinal,
                    .Visual = tostadaVisual,
                    .Actions = tostadaAcciones,
                    .Audio = tostadaAudio,
                    .ActivationType = ToastActivationType.Background
                }

                Try
                    Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml)

                    If Not segundos = 0 Then
                        notificacion.ExpirationTime = DateTime.Now.AddSeconds(segundos)
                    End If

                    Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
                    notificador.Show(notificacion)
                Catch ex As Exception

                End Try
            End If
        End If

    End Sub

    'Public Sub ToastTweets(cantidad As Integer, megaUsuario As pepeizq.Twitter.MegaUsuario, segundos As Integer, tipo As Integer)

    '    Dim recursos As New Resources.ResourceLoader

    '    Dim textoFinal As String = Nothing

    '    If tipo = 0 Then
    '        textoFinal = "@" + megaUsuario.Usuario.ScreenNombre + " " + recursos.GetString("UserNewTweets") + " (" + cantidad.ToString + ")"
    '    ElseIf tipo = 1 Then
    '        textoFinal = "@" + megaUsuario.Usuario.ScreenNombre + " " + recursos.GetString("UserNewMentions") + " (" + cantidad.ToString + ")"
    '    End If

    '    Dim texto As New AdaptiveText With {
    '       .Text = textoFinal,
    '       .HintMaxLines = 4
    '    }

    '    Dim logo As New ToastGenericAppLogo With {
    '       .Source = megaUsuario.Usuario.ImagenAvatar,
    '       .HintCrop = ToastGenericAppLogoCrop.Circle
    '    }

    '    Dim contenido As New ToastBindingGeneric With {
    '        .AppLogoOverride = logo
    '    }

    '    contenido.Children.Add(texto)

    '    Dim tostadaVisual As New ToastVisual With {
    '        .BindingGeneric = contenido
    '    }

    '    Dim tostadaAudio As New ToastAudio

    '    If ApplicationData.Current.LocalSettings.Values("notificacionSonido") = False Then
    '        tostadaAudio.Silent = True
    '    Else
    '        tostadaAudio.Silent = False

    '        If ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido") = Nothing Then
    '            tostadaAudio.Src = New Uri("ms-winsoundevent:Notification.Default")
    '        Else
    '            tostadaAudio.Src = New Uri(ApplicationData.Current.LocalSettings.Values("notificacionSonidoElegido"))
    '        End If
    '    End If

    '    Dim tostada As New ToastContent With {
    '        .Launch = textoFinal,
    '        .Visual = tostadaVisual,
    '        .Audio = tostadaAudio
    '    }

    '    Try
    '        Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml)

    '        If Not segundos = 0 Then
    '            notificacion.ExpirationTime = DateTime.Now.AddSeconds(segundos)
    '        End If

    '        Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
    '        notificador.Show(notificacion)
    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Public Sub ToastMencion(usuario As TwitterUsuario)

    '    Dim recursos As New Resources.ResourceLoader

    '    Dim textoFinal As String = "@" + usuario.ScreenNombre + " " + recursos.GetString("UserNewMentions")

    '    Dim texto As New AdaptiveText With {
    '       .Text = textoFinal,
    '       .HintMaxLines = 4
    '    }

    '    Dim logo As New ToastGenericAppLogo With {
    '       .Source = usuario.ImagenAvatar,
    '       .HintCrop = ToastGenericAppLogoCrop.Circle
    '    }

    '    Dim contenido As New ToastBindingGeneric With {
    '        .AppLogoOverride = logo
    '    }

    '    contenido.Children.Add(texto)

    '    Dim tostadaVisual As New ToastVisual With {
    '        .BindingGeneric = contenido
    '    }

    '    Dim tostada As New ToastContent With {
    '        .Launch = textoFinal,
    '        .Visual = tostadaVisual
    '    }

    '    Try
    '        Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml) With {
    '            .ExpirationTime = DateTime.Now.AddSeconds(20)
    '        }

    '        Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
    '        notificador.Show(notificacion)
    '    Catch ex As Exception

    '    End Try

    'End Sub

End Module

