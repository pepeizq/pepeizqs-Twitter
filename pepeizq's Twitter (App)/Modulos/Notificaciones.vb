Imports Microsoft.Toolkit.Uwp.Notifications
Imports Windows.Storage
Imports Windows.UI.Notifications
Imports Windows.UI.Popups
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports System.Text

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

    Public Sub ToastTweet(tweet As Tweet, megaUsuario As pepeizq.Twitter.MegaUsuario, segundos As Integer)

        Dim tb As New TextBlock
        tb = pepeizq.Twitter.Xaml.TweetTexto.Generar(tweet, Nothing, Nothing, megaUsuario, True)

        If Not tb.Text = String.Empty Then
            Dim textoTweet As New AdaptiveText With {
                .Text = tb.Text,
                .HintMaxLines = 4
            }

            Dim cuenta As String = Nothing

            If tweet.Retweet Is Nothing Then
                cuenta = tweet.Usuario.Nombre
            Else
                cuenta = tweet.Retweet.Usuario.Nombre
            End If

            Dim textoCuenta As New AdaptiveText With {
                .Text = cuenta,
                .HintMaxLines = 1
            }

            Dim avatarUrl As String = Nothing

            If tweet.Retweet Is Nothing Then
                avatarUrl = tweet.Usuario.ImagenAvatar
            Else
                avatarUrl = tweet.Retweet.Usuario.ImagenAvatar
            End If

            Dim logo As New ToastGenericAppLogo With {
                .Source = avatarUrl,
                .HintCrop = ToastGenericAppLogoCrop.Circle
            }

            Dim hero As ToastGenericHeroImage = Nothing

            If Not ApplicationData.Current.LocalSettings.Values("notificacionImagen") Is Nothing Then
                If ApplicationData.Current.LocalSettings.Values("notificacionImagen") = True Then
                    If Not tweet.Entidades.Media Is Nothing Then
                        Dim tweetMedia As TweetMedia() = tweet.Entidades.Media

                        If Not tweetMedia(0).Enlace = Nothing Then
                            hero = New ToastGenericHeroImage With {
                                .Source = tweetMedia(0).Enlace
                            }
                        End If
                    End If
                End If
            End If

            Dim atribucion As ToastGenericAttributionText = Nothing

            If ApplicationData.Current.LocalSettings.Values("notificacionUsuario") = True Then
                atribucion = New ToastGenericAttributionText With {
                    .Text = "@" + megaUsuario.Usuario.ScreenNombre
                }
            End If

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

            If Not tweet.Entidades.Enlaces Is Nothing Then
                Dim urlFinal As String = Nothing

                For Each url In tweet.Entidades.Enlaces
                    If Not url Is Nothing Then
                        urlFinal = url.Enlace.ToString
                    End If
                Next

                If Not urlFinal = Nothing Then
                    botonAbrir = New ToastButton(recursos.GetString("Open"), urlFinal) With {
                        .ActivationType = ToastActivationType.Protocol
                    }
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

            Dim bytesTexto() As Byte = Encoding.Default.GetBytes(tweet.TextoCompleto)
            Dim textoFinal As String = Encoding.UTF8.GetString(bytesTexto)

            Dim tostada As New ToastContent With {
                .Launch = textoFinal,
                .Visual = tostadaVisual,
                .Actions = tostadaAcciones,
                .Audio = tostadaAudio
            }

            Try
                Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml)

                If Not segundos = Nothing Then
                    notificacion.ExpirationTime = DateTime.Now.AddSeconds(segundos)
                End If

                Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
                notificador.Show(notificacion)
            Catch ex As Exception

            End Try
        End If

    End Sub

    Public Sub ToastTweets(cantidad As Integer, megaUsuario As pepeizq.Twitter.MegaUsuario, segundos As Integer)

        Dim recursos As New Resources.ResourceLoader

        Dim textoFinal As String = "@" + megaUsuario.Usuario.ScreenNombre + " " + recursos.GetString("UserNewTweets") + " (" + cantidad.ToString + ")"

        Dim texto As New AdaptiveText With {
           .Text = textoFinal,
           .HintMaxLines = 4
        }

        Dim logo As New ToastGenericAppLogo With {
           .Source = megaUsuario.Usuario.ImagenAvatar,
           .HintCrop = ToastGenericAppLogoCrop.Circle
        }

        Dim contenido As New ToastBindingGeneric With {
            .AppLogoOverride = logo
        }

        contenido.Children.Add(texto)

        Dim tostadaVisual As New ToastVisual With {
            .BindingGeneric = contenido
        }

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

        Dim tostada As New ToastContent With {
            .Launch = textoFinal,
            .Visual = tostadaVisual,
            .Audio = tostadaAudio
        }

        Try
            Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml)

            If Not segundos = Nothing Then
                notificacion.ExpirationTime = DateTime.Now.AddSeconds(segundos)
            End If

            Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
            notificador.Show(notificacion)
        Catch ex As Exception

        End Try

    End Sub

    Public Sub ToastMencion(usuario As TwitterUsuario)

        Dim recursos As New Resources.ResourceLoader

        Dim textoFinal As String = "@" + usuario.ScreenNombre + " " + recursos.GetString("UserNewMentions")

        Dim texto As New AdaptiveText With {
           .Text = textoFinal,
           .HintMaxLines = 4
        }

        Dim logo As New ToastGenericAppLogo With {
           .Source = usuario.ImagenAvatar,
           .HintCrop = ToastGenericAppLogoCrop.Circle
        }

        Dim contenido As New ToastBindingGeneric With {
            .AppLogoOverride = logo
        }

        contenido.Children.Add(texto)

        Dim tostadaVisual As New ToastVisual With {
            .BindingGeneric = contenido
        }

        Dim tostada As New ToastContent With {
            .Launch = textoFinal,
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

End Module

