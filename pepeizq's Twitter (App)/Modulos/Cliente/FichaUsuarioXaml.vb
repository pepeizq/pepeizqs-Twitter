Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.System
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Text
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Shapes

Module FichaUsuarioXaml

    Public Async Sub Generar(cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado, objetoAnimar As Object)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridTweetAmpliado As Grid = pagina.FindName("gridTweetAmpliado")
        gridTweetAmpliado.Visibility = Visibility.Collapsed

        If cosas.Usuario Is Nothing Then
            cosas.Usuario = Await TwitterPeticiones.CogerUsuario(cosas.Usuario, cosas.MegaUsuario, cosas.ScreenNombre)
        End If

        Dim gridTitulo As Grid = pagina.FindName("gridTitulo")
        Dim gridUsuario As Grid = pagina.FindName("gridUsuarioAmpliado")

        Dim color As Color = Nothing

        Try
            color = ("#" + cosas.Usuario.ColorEnlace).ToColor
        Catch ex As Exception
            color = App.Current.Resources("ColorSecundario")
        End Try

        App.Current.Resources("ButtonBackgroundPointerOver") = color

        Dim transpariencia As New UISettings
        Dim boolTranspariencia As Boolean = transpariencia.AdvancedEffectsEnabled

        If boolTranspariencia = False Then
            gridUsuario.Background = New SolidColorBrush(color)
        Else
            Dim acrilico As New AcrylicBrush With {
                .BackgroundSource = AcrylicBackgroundSource.Backdrop,
                .TintOpacity = 0.7,
                .TintColor = color
            }

            gridUsuario.Background = acrilico
        End If

        If Not objetoAnimar Is Nothing Then
            Try
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionUsuario", objetoAnimar)

                Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionUsuario")

                If Not animacion Is Nothing Then
                    animacion.TryStart(gridUsuario)
                End If
            Catch ex As Exception

            End Try
        End If

        gridUsuario.Visibility = Visibility.Visible

        Dim nvPrincipal As NavigationView = pagina.FindName("nvPrincipal")

        For Each item In nvPrincipal.MenuItems
            If TypeOf item Is NavigationViewItem Then
                Dim nv As NavigationViewItem = item

                If TypeOf nv.Content Is TextBlock Then
                    Dim tb As TextBlock = nv.Content

                    If tb.Text = recursos.GetString("Back") Then
                        nv.Visibility = Visibility.Visible

                        Dim separador As NavigationViewItemSeparator = pagina.FindName("nvSeparadorVolver")
                        separador.Visibility = Visibility.Visible
                    End If
                End If
            End If
        Next

        Dim lvTweets As ListView = pagina.FindName("lvTweetsUsuario")
        lvTweets.IsItemClickEnabled = True

        If lvTweets.Items.Count > 0 Then
            lvTweets.Items.Clear()
        End If

        AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

        Dim prTweets As ProgressRing = pagina.FindName("prTweetsUsuario")
        prTweets.Foreground = New SolidColorBrush(color)

        Dim svTweets As ScrollViewer = pagina.FindName("svTweetsUsuario")
        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(cosas.MegaUsuario, Nothing, prTweets, 2, cosas.Usuario.ScreenNombre, color)
        svTweets.Foreground = New SolidColorBrush(("#" + cosas.Usuario.ColorTexto).ToColor)
        AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

        '------------------------------------

        Dim circuloAvatar As Ellipse = pagina.FindName("ellipseAvatar")

        Dim imagenAvatar As New ImageBrush With {
            .Stretch = Stretch.Uniform,
            .ImageSource = New BitmapImage(New Uri(cosas.Usuario.ImagenAvatar.Replace("_normal.png", "_bigger.png")))
        }

        circuloAvatar.Fill = imagenAvatar

        Dim tbNombre As TextBlock = pagina.FindName("tbNombreUsuario")
        tbNombre.Text = cosas.Usuario.Nombre

        Dim imagenVerificado As ImageEx = pagina.FindName("imagenUsuarioVerificado")

        If cosas.Usuario.Verificado = True Then
            imagenVerificado.Visibility = Visibility.Visible
        Else
            imagenVerificado.Visibility = Visibility.Collapsed
        End If

        Dim tbScreenNombre As TextBlock = pagina.FindName("tbScreenNombreUsuario")
        tbScreenNombre.Text = "@" + cosas.Usuario.ScreenNombre

        '------------------------------------

        Dim gridDescripcion As Grid = pagina.FindName("gridDescripcionUsuario")

        If Not cosas.Usuario.Descripcion = Nothing Then
            gridDescripcion.Visibility = Visibility.Visible

            Dim tbDescripcion As TextBlock = pagina.FindName("tbDescripcionUsuario")
            tbDescripcion.Text = cosas.Usuario.Descripcion
        Else
            gridDescripcion.Visibility = Visibility.Collapsed
        End If

        '------------------------------------

        Dim botonEnlace As Button = pagina.FindName("botonEnlaceUsuario")

        If Not cosas.Usuario.Entidades.Enlace Is Nothing Then
            Try
                botonEnlace.Tag = New Uri(cosas.Usuario.Entidades.Enlace.Enlaces(0).Expandida)

                Dim tbEnlace As New TextBlock With {
                    .Text = cosas.Usuario.Entidades.Enlace.Enlaces(0).Mostrar,
                    .Foreground = New SolidColorBrush(Colors.White),
                    .FontWeight = FontWeights.SemiBold
                }

                botonEnlace.Content = tbEnlace
                botonEnlace.Visibility = Visibility.Visible
            Catch ex As Exception
                botonEnlace.Content = Nothing
                botonEnlace.Visibility = Visibility.Collapsed
            End Try
        Else
            botonEnlace.Content = Nothing
            botonEnlace.Visibility = Visibility.Collapsed
        End If

        Dim tbNumTweets As TextBlock = pagina.FindName("tbNumTweetsUsuario")
        tbNumTweets.Text = String.Format("{0:n0}", Integer.Parse(cosas.Usuario.NumTweets))

        Dim botonNumTweets As Button = pagina.FindName("botonNumTweetsUsuario")
        botonNumTweets.Tag = New Uri("https://twitter.com/" + cosas.Usuario.ScreenNombre)

        Dim tbNumSeguidores As TextBlock = pagina.FindName("tbNumSeguidoresUsuario")
        tbNumSeguidores.Text = String.Format("{0:n0}", Integer.Parse(cosas.Usuario.Followers))

        Dim botonSeguidores As Button = pagina.FindName("botonSeguidoresUsuario")
        botonSeguidores.Tag = New Uri("https://twitter.com/" + cosas.Usuario.ScreenNombre + "/followers")

        Dim tbNumFavoritos As TextBlock = pagina.FindName("tbNumFavoritosUsuario")
        tbNumFavoritos.Text = String.Format("{0:n0}", Integer.Parse(cosas.Usuario.Favoritos))

        Dim botonFavoritos As Button = pagina.FindName("botonFavoritosUsuario")
        botonFavoritos.Tag = New Uri("https://twitter.com/" + cosas.Usuario.ScreenNombre + "/likes")

        '------------------------------------

        Dim botonSeguir As Button = pagina.FindName("botonSeguirUsuario")
        Dim tbSeguir As TextBlock = pagina.FindName("tbSeguirUsuario")

        If cosas.Usuario.Siguiendo = True Then
            tbSeguir.Text = recursos.GetString("Following")
        Else
            tbSeguir.Text = recursos.GetString("Follow")
        End If

        botonSeguir.Background = New SolidColorBrush(color)
        botonSeguir.Tag = New pepeizq.Twitter.Objetos.SeguirUsuarioBoton(cosas.MegaUsuario, cosas.Usuario)
        AddHandler botonSeguir.Click, AddressOf BotonSeguirClick

        Dim botonMasOpciones As Button = pagina.FindName("botonMasOpcionesUsuario")
        botonMasOpciones.Background = New SolidColorBrush(color)
        botonMasOpciones.Tag = cosas

        AddHandler botonMasOpciones.Click, AddressOf BotonMasOpcionesClick

        Dim botonBloquearUsuario As Button = pagina.FindName("botonBloquearUsuario")
        botonBloquearUsuario.Tag = cosas

        Dim boolBloqueo As Boolean = False

        For Each bloqueo In cosas.MegaUsuario.UsuariosBloqueados
            If bloqueo = cosas.Usuario.ID Then
                boolBloqueo = True
            End If
        Next

        If boolBloqueo = True Then
            Dim toolTip As New ToolTip With {
                .Content = recursos.GetString("UserUnblock")
            }
            ToolTipService.SetToolTip(botonBloquearUsuario, toolTip)

            Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.Unlock,
                .Foreground = New SolidColorBrush(Colors.White)
            }
            botonBloquearUsuario.Content = iconoFinal
        Else
            Dim toolTip As New ToolTip With {
                .Content = recursos.GetString("UserBlock")
            }
            ToolTipService.SetToolTip(botonBloquearUsuario, toolTip)

            Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.Lock,
                .Foreground = New SolidColorBrush(Colors.White)
            }
            botonBloquearUsuario.Content = iconoFinal
        End If

        AddHandler botonBloquearUsuario.Click, AddressOf BotonBloquearUsuarioClick

        Dim botonMutearUsuario As Button = pagina.FindName("botonMutearUsuario")
        botonMutearUsuario.Tag = cosas

        Dim boolMuteado As Boolean = False

        If Not cosas.MegaUsuario.UsuariosMuteados Is Nothing Then
            For Each muteo In cosas.MegaUsuario.UsuariosMuteados
                If muteo = cosas.Usuario.ID Then
                    boolMuteado = True
                End If
            Next
        End If

        If boolMuteado = True Then
            Dim toolTip As New ToolTip With {
                .Content = recursos.GetString("UserUnmute")
            }
            ToolTipService.SetToolTip(botonMutearUsuario, toolTip)

            Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.VolumeUp,
                .Foreground = New SolidColorBrush(Colors.White)
            }
            botonMutearUsuario.Content = iconoFinal
        Else
            Dim toolTip As New ToolTip With {
                .Content = recursos.GetString("UserMute")
            }
            ToolTipService.SetToolTip(botonMutearUsuario, toolTip)

            Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.VolumeOff,
                .Foreground = New SolidColorBrush(Colors.White)
            }
            botonMutearUsuario.Content = iconoFinal
        End If

        AddHandler botonMutearUsuario.Click, AddressOf BotonMutearUsuarioClick

        Dim botonReportarUsuario As Button = pagina.FindName("botonReportarUsuario")
        botonReportarUsuario.Tag = cosas

        Dim toolTipReportar As New ToolTip With {
            .Content = recursos.GetString("UserReport")
        }
        ToolTipService.SetToolTip(botonReportarUsuario, toolTipReportar)

        AddHandler botonReportarUsuario.Click, AddressOf BotonReportarUsuarioClick

        '------------------------------------

        Dim botonSubir As Button = pagina.FindName("botonSubirArribaUsuario")
        botonSubir.Background = New SolidColorBrush(color)

        If boolBloqueo = False Then
            lvTweets.Visibility = Visibility.Visible

            Dim listaTweets As New List(Of Tweet)

            listaTweets = Await TwitterPeticiones.UserTimeline(listaTweets, cosas.MegaUsuario, cosas.Usuario.ScreenNombre, Nothing)

            If listaTweets.Count > 0 Then
                For Each tweet In listaTweets
                    Dim boolAñadir As Boolean = True

                    For Each item In lvTweets.Items
                        Dim lvItem As ListViewItem = item
                        Dim gridTweet As Grid = lvItem.Content
                        Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
                        Dim lvTweet As Tweet = tweetAmpliado.Tweet

                        If lvTweet.ID = tweet.ID Then
                            boolAñadir = False
                        End If
                    Next

                    If boolAñadir = True Then
                        lvTweets.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, cosas.MegaUsuario, color))
                    End If
                Next
            End If
        End If

    End Sub

    Private Async Sub BotonSeguirClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim tbSeguir As TextBlock = boton.Content
        Dim cosas As pepeizq.Twitter.Objetos.SeguirUsuarioBoton = boton.Tag

        If tbSeguir.Text = recursos.GetString("Following") Then
            Dim estado As Boolean = False

            estado = Await TwitterPeticiones.DeshacerSeguirUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

            If estado = True Then
                tbSeguir.Text = recursos.GetString("Follow")
            End If
        Else
            Dim estado As Boolean = False

            estado = Await TwitterPeticiones.SeguirUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

            If estado = True Then
                tbSeguir.Text = recursos.GetString("Following")
            End If
        End If

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)

    End Sub

    Private Sub BotonMasOpcionesClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Dim menu As New MenuFlyout With {
            .Placement = FlyoutPlacementMode.Bottom
        }

        Dim botonCompartirUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("ShareUser"),
            .Tag = cosas
        }

        AddHandler botonCompartirUsuario.Click, AddressOf BotonCompartirUsuarioClick
        AddHandler botonCompartirUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonCompartirUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonCompartirUsuario)

        Dim botonAbrirNavegadorUsuario As New MenuFlyoutItem With {
            .Text = recursos.GetString("OpenWebBrowserUser"),
            .Tag = cosas
        }

        AddHandler botonAbrirNavegadorUsuario.Click, AddressOf BotonAbrirNavegadorUsuarioClick
        AddHandler botonAbrirNavegadorUsuario.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonAbrirNavegadorUsuario.PointerExited, AddressOf UsuarioSaleBoton
        menu.Items.Add(botonAbrirNavegadorUsuario)

        FlyoutBase.SetAttachedFlyout(boton, menu)
        menu.ShowAt(boton)

    End Sub

    Private Async Sub BotonBloquearUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvTweets As ListView = pagina.FindName("lvTweetsUsuario")

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag
        Dim icono As FontAwesome.UWP.FontAwesome = boton.Content

        If icono.Icon = FontAwesomeIcon.Unlock Then
            Dim estado As Boolean = False

            estado = Await TwitterPeticiones.DeshacerBloquearUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

            If estado = True Then
                Dim toolTip As New ToolTip With {
                    .Content = recursos.GetString("UserBlock")
                }

                ToolTipService.SetToolTip(boton, toolTip)

                Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                    .Icon = FontAwesomeIcon.Lock,
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                boton.Content = iconoFinal
                lvTweets.Visibility = Visibility.Visible

                cosas.MegaUsuario.UsuariosBloqueados.Remove(cosas.Usuario.ID)

                Dim listaTweets As New List(Of Tweet)

                listaTweets = Await TwitterPeticiones.UserTimeline(listaTweets, cosas.MegaUsuario, cosas.Usuario.ScreenNombre, Nothing)

                If listaTweets.Count > 0 Then
                    For Each tweet In listaTweets
                        Dim boolAñadir As Boolean = True

                        For Each item In lvTweets.Items
                            Dim lvItem As ListViewItem = item
                            Dim gridTweet As Grid = lvItem.Content
                            Dim tweetAmpliado As pepeizq.Twitter.Objetos.TweetAmpliado = gridTweet.Tag
                            Dim lvTweet As Tweet = tweetAmpliado.Tweet

                            If lvTweet.ID = tweet.ID Then
                                boolAñadir = False
                            End If
                        Next

                        If boolAñadir = True Then
                            lvTweets.Items.Add(pepeizq.Twitter.Xaml.TweetXaml.Añadir(tweet, cosas.MegaUsuario, ("#" + cosas.Usuario.ColorEnlace).ToColor))
                        End If
                    Next
                End If
            End If
        Else
            Dim estado As Boolean = False

            estado = Await TwitterPeticiones.BloquearUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

            If estado = True Then
                Dim toolTip As New ToolTip With {
                    .Content = recursos.GetString("UserUnblock")
                }

                ToolTipService.SetToolTip(boton, toolTip)

                Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                    .Icon = FontAwesomeIcon.Unlock,
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                boton.Content = iconoFinal
                cosas.MegaUsuario.UsuariosBloqueados.Add(cosas.Usuario.ID)

                lvTweets.Visibility = Visibility.Collapsed
                lvTweets.Items.Clear()
            End If
        End If

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True, False)

    End Sub

    Private Async Sub BotonMutearUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag
        Dim icono As FontAwesome.UWP.FontAwesome = boton.Content

        If icono.Icon = FontAwesomeIcon.VolumeUp Then
            Dim estado As Boolean = False

            estado = Await TwitterPeticiones.DeshacerMutearUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

            If estado = True Then
                Dim toolTip As New ToolTip With {
                    .Content = recursos.GetString("UserMute")
                }

                ToolTipService.SetToolTip(boton, toolTip)

                Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                    .Icon = FontAwesomeIcon.VolumeOff,
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                boton.Content = iconoFinal

                cosas.MegaUsuario.UsuariosMuteados.Remove(cosas.Usuario.ID)
            End If
        Else
            Dim estado As Boolean = False

            estado = Await TwitterPeticiones.MutearUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

            If estado = True Then
                Dim toolTip As New ToolTip With {
                    .Content = recursos.GetString("UserUnmute")
                }

                ToolTipService.SetToolTip(boton, toolTip)

                Dim iconoFinal As New FontAwesome.UWP.FontAwesome With {
                    .Icon = FontAwesomeIcon.VolumeUp,
                    .Foreground = New SolidColorBrush(Colors.White)
                }

                boton.Content = iconoFinal

                cosas.MegaUsuario.UsuariosMuteados.Add(cosas.Usuario.ID)
            End If
        End If

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True, False)

    End Sub

    Private Async Sub BotonReportarUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim lvTweets As ListView = pagina.FindName("lvTweetsUsuario")

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Dim estado As Boolean = False

        estado = Await TwitterPeticiones.ReportarUsuario(estado, cosas.MegaUsuario, cosas.Usuario.ID)

        If estado = True Then
            lvTweets.Visibility = Visibility.Collapsed
            lvTweets.Items.Clear()
        End If

        TwitterTimeLineInicio.CargarTweets(cosas.MegaUsuario, Nothing, True)
        TwitterTimeLineMenciones.CargarTweets(cosas.MegaUsuario, Nothing, True, False)

    End Sub

    Private Async Sub BotonAbrirNavegadorUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        Try
            Await Launcher.LaunchUriAsync(New Uri("https://twitter.com/" + cosas.Usuario.ScreenNombre))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub BotonCompartirUsuarioClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        ApplicationData.Current.LocalSettings.Values("UsuarioCompartirTitulo") = "@" + cosas.Usuario.ScreenNombre
        ApplicationData.Current.LocalSettings.Values("UsuarioCompartirDescripcion") = cosas.Usuario.Nombre
        ApplicationData.Current.LocalSettings.Values("UsuarioCompartirEnlace") = "https://twitter.com/" + cosas.Usuario.ScreenNombre

        Dim datos As DataTransferManager = DataTransferManager.GetForCurrentView()
        AddHandler datos.DataRequested, AddressOf DatosCompartirClick

        DataTransferManager.ShowShareUI()

    End Sub

    Private Sub DatosCompartirClick(sender As Object, e As DataRequestedEventArgs)

        Dim request As DataRequest = e.Request

        request.Data.Properties.Title = ApplicationData.Current.LocalSettings.Values("UsuarioCompartirTitulo")
        request.Data.Properties.Description = ApplicationData.Current.LocalSettings.Values("UsuarioCompartirDescripcion")
        request.Data.SetWebLink(New Uri(ApplicationData.Current.LocalSettings.Values("UsuarioCompartirEnlace")))

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
