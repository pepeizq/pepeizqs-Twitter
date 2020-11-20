Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Tweetinvi
Imports Tweetinvi.Models
Imports Windows.UI
Imports Windows.UI.Xaml.Shapes

Namespace Interfaz
    Module Busqueda

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonBuscar As Button = pagina.FindName("botonBusquedaBuscar")
            botonBuscar.IsEnabled = False

            RemoveHandler botonBuscar.Click, AddressOf BusquedaUsuario
            AddHandler botonBuscar.Click, AddressOf BusquedaUsuario

            RemoveHandler botonBuscar.PointerEntered, AddressOf Entra_Sp_IconoNombre
            AddHandler botonBuscar.PointerEntered, AddressOf Entra_Sp_IconoNombre

            RemoveHandler botonBuscar.PointerExited, AddressOf Sale_Sp_IconoNombre
            AddHandler botonBuscar.PointerExited, AddressOf Sale_Sp_IconoNombre

            Dim tbBuscar As TextBox = pagina.FindName("tbBusqueda")

            RemoveHandler tbBuscar.TextChanged, AddressOf PermitirBuscar
            AddHandler tbBuscar.TextChanged, AddressOf PermitirBuscar

        End Sub

        Private Async Sub BusquedaUsuario(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pantalla As DisplayInformation = DisplayInformation.GetForCurrentView()
            Dim tamañoPantalla As Size = New Size(pantalla.ScreenWidthInRawPixels, pantalla.ScreenHeightInRawPixels)

            Dim gvResultados As AdaptiveGridView = pagina.FindName("gvResultadosBusqueda")
            gvResultados.Items.Clear()
            gvResultados.DesiredWidth = tamañoPantalla.Width / 4

            Dim gridCarga As Grid = pagina.FindName("gridCargaBusqueda")
            gridCarga.Visibility = Visibility.Visible

            Dim tbBuscar As TextBox = pagina.FindName("tbBusqueda")

            Dim cliente As TwitterClient = Interfaz.Tweets.cliente_

            Dim parametros As New Parameters.SearchUsersParameters(tbBuscar.Text) With {
                .PageSize = 20
            }

            Dim usuarios As IUser() = Await cliente.Search.SearchUsersAsync(parametros)

            If Not usuarios Is Nothing Then
                If usuarios.Length > 0 Then
                    For Each usuario In usuarios
                        Dim botonUsuario As New Button With {
                            .Background = New SolidColorBrush(Colors.Transparent),
                            .BorderThickness = New Thickness(0, 0, 0, 0),
                            .Style = App.Current.Resources("ButtonRevealStyle"),
                            .Padding = New Thickness(0, 0, 0, 0),
                            .HorizontalAlignment = HorizontalAlignment.Stretch,
                            .HorizontalContentAlignment = HorizontalAlignment.Stretch,
                            .Tag = usuario.ScreenName,
                            .Margin = New Thickness(5, 5, 5, 5)
                        }

                        Dim colorFondo As New SolidColorBrush With {
                            .Color = App.Current.Resources("ColorCuarto"),
                            .Opacity = 0.8
                        }

                        Dim sp As New StackPanel With {
                            .Orientation = Orientation.Horizontal,
                            .Margin = New Thickness(0, 0, 0, 0),
                            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                            .BorderThickness = New Thickness(1, 1, 1, 1),
                            .Background = colorFondo,
                            .Padding = New Thickness(20, 20, 20, 20)
                        }

                        Dim imagenAvatar As New ImageBrush With {
                            .Stretch = Stretch.Uniform,
                            .ImageSource = New BitmapImage(New Uri(usuario.ProfileImageUrl))
                        }

                        Dim circulo As New Ellipse With {
                            .Fill = imagenAvatar,
                            .Height = 40,
                            .Width = 40
                        }

                        sp.Children.Add(circulo)

                        Dim spVertical As New StackPanel With {
                            .Orientation = Orientation.Vertical,
                            .Margin = New Thickness(20, 0, 0, 0)
                        }

                        Dim spUsuario As New StackPanel With {
                            .Orientation = Orientation.Horizontal,
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Background = New SolidColorBrush(Colors.Transparent)
                        }

                        Dim tb1 As New TextBlock With {
                            .FontSize = 14,
                            .Foreground = New SolidColorBrush(Colors.White),
                            .Text = usuario.Name,
                            .TextWrapping = TextWrapping.Wrap
                        }

                        Dim tb2 As New TextBlock With {
                            .FontSize = 12,
                            .Margin = New Thickness(5, 0, 0, 0),
                            .VerticalAlignment = VerticalAlignment.Center,
                            .Foreground = New SolidColorBrush(Colors.White),
                            .Text = "@" + usuario.ScreenName,
                            .TextWrapping = TextWrapping.Wrap
                        }

                        spUsuario.Children.Add(tb1)
                        spUsuario.Children.Add(tb2)

                        spVertical.Children.Add(spUsuario)

                        Dim spDatos As New StackPanel With {
                            .Orientation = Orientation.Horizontal,
                            .Padding = New Thickness(5, 5, 5, 5),
                            .Background = New SolidColorBrush(Colors.Transparent)
                        }

                        If usuario.Verified = True Then
                            Dim iconoVerificado As New FontAwesome5.FontAwesome With {
                                .Foreground = New SolidColorBrush(Colors.White),
                                .Icon = FontAwesome5.EFontAwesomeIcon.Solid_CheckCircle,
                                .Margin = New Thickness(0, 0, 10, 0)
                            }

                            spDatos.Children.Add(iconoVerificado)
                        End If

                        Dim tbSeguidores As New TextBlock With {
                            .Foreground = New SolidColorBrush(Colors.White),
                            .Text = recursos.GetString("Followers2") + " " + usuario.FollowersCount.ToString("###,###,####")
                        }

                        spDatos.Children.Add(tbSeguidores)

                        spVertical.Children.Add(spDatos)

                        sp.Children.Add(spVertical)

                        botonUsuario.Content = sp

                        AddHandler botonUsuario.Click, AddressOf OtroUsuario.CargarClick3
                        AddHandler botonUsuario.PointerEntered, AddressOf Entra_Basico
                        AddHandler botonUsuario.PointerExited, AddressOf Sale_Basico

                        gvResultados.Items.Add(botonUsuario)
                    Next
                End If
            End If

            gridCarga.Visibility = Visibility.Collapsed

        End Sub

        Private Sub PermitirBuscar(sender As Object, e As TextChangedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonBuscar As Button = pagina.FindName("botonBusquedaBuscar")
            Dim tb As TextBox = sender

            If tb.Text.Trim.Length > 2 Then
                botonBuscar.IsEnabled = True
            Else
                botonBuscar.IsEnabled = False
            End If

        End Sub

    End Module
End Namespace