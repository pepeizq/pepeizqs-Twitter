﻿Imports System.Net
Imports Microsoft.Toolkit.Uwp.Notifications
Imports Windows.Storage
Imports Windows.UI.Notifications
Imports Windows.UI.Popups
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet

Namespace Notificaciones

    Module MessageBox

        Public Async Sub Enseñar(contenido As String)

            Try
                Dim messageDialog = New MessageDialog(contenido)
                Await messageDialog.ShowAsync()
            Catch ex As Exception

            End Try

        End Sub

    End Module

    Module Toast

        Public Sub Enseñar(titulo As String)

            Dim texto As New AdaptiveText With {
               .Text = titulo,
               .HintMaxLines = 4
            }

            Dim logo As New ToastGenericAppLogo With {
               .Source = "ms-appx:///Assets/StoreLogo.png"
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

            Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml)
            Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
            notificador.Show(notificacion)

        End Sub

    End Module

    Module ToastTweet

        Public Sub Enseñar(tweet As Tweet)

            Dim texto As String = Nothing

            Dim rango0 As Integer = 0

            If tweet.Retweet Is Nothing Then
                rango0 = tweet.TextoRango(0)

                If Not tweet.Texto = String.Empty Then
                    texto = tweet.Texto

                    texto = texto.Remove(tweet.TextoRango(1), texto.Length - tweet.TextoRango(1))
                    texto = texto.Remove(0, rango0)
                End If
            Else
                rango0 = tweet.Retweet.TextoRango(0)

                If Not tweet.Retweet.Texto = String.Empty Then
                    texto = tweet.Retweet.Texto

                    texto = texto.Remove(tweet.Retweet.TextoRango(1), texto.Length - tweet.Retweet.TextoRango(1))
                    texto = texto.Remove(0, rango0)
                End If
            End If

            texto = WebUtility.HtmlDecode(texto)

            If Not texto = String.Empty Then
                If texto.Contains("http") Then
                    Dim temp As String
                    Dim int, int2 As Integer

                    int = texto.IndexOf("http")
                    temp = texto.Remove(0, int)

                    int2 = temp.IndexOf(" ")

                    If int2 = -1 Then
                        int2 = temp.Length
                    End If

                    texto = texto.Remove(int, int2)
                End If

                Dim textoTweet As New AdaptiveText With {
                    .Text = texto,
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

                Dim contenido As New ToastBindingGeneric With {
                    .AppLogoOverride = logo
                }

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

                Dim tostada As New ToastContent With {
                    .Launch = tweet.Texto,
                    .Visual = tostadaVisual,
                    .Actions = tostadaAcciones,
                    .Audio = tostadaAudio
                }

                Dim notificacion As ToastNotification = New ToastNotification(tostada.GetXml)

                If Not ApplicationData.Current.LocalSettings.Values("notificacionTiempo") Is Nothing Then
                    If ApplicationData.Current.LocalSettings.Values("notificacionTiempo") = True Then
                        notificacion.ExpirationTime = DateTime.Now.AddSeconds(ApplicationData.Current.LocalSettings.Values("notificacionTiempoSegundos"))
                    End If
                End If

                Dim notificador As ToastNotifier = ToastNotificationManager.CreateToastNotifier()
                notificador.Show(notificacion)
            End If

        End Sub

    End Module

End Namespace