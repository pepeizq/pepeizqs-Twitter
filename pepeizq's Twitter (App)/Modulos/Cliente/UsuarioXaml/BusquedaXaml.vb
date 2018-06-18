Imports pepeizq.Twitter
Imports pepeizq.Twitter.Busqueda
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Module BusquedaXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim recursos As New Resources.ResourceLoader

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridFondo As New Grid
        gridFondo.SetValue(Grid.RowProperty, 1)
        gridFondo.Name = "gridBusqueda" + usuario.ScreenNombre
        gridFondo.Visibility = visibilidad
        gridFondo.Padding = New Thickness(10, 10, 10, 10)

        Dim transpariencia As New UISettings

        If transpariencia.AdvancedEffectsEnabled = True Then
            gridFondo.Background = App.Current.Resources("GridAcrilico")
        Else
            gridFondo.Background = New SolidColorBrush(Colors.LightGray)
        End If

        Dim spBusqueda As New StackPanel With {
            .Orientation = Orientation.Horizontal,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim tbFondo As New Border With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
            .VerticalAlignment = VerticalAlignment.Top
        }

        Dim tbBusqueda As New TextBlock With {
            .Text = recursos.GetString("Users"),
            .Padding = New Thickness(15, 10, 15, 10),
            .Foreground = New SolidColorBrush(Colors.White)
        }

        tbFondo.Child = tbBusqueda
        spBusqueda.Children.Add(tbFondo)

        Dim gridBusqueda As New Grid With {
            .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
            .BorderThickness = New Thickness(1, 1, 1, 1)
        }

        '---------------------------------

        Dim color1 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#e0e0e0"),
            .Offset = 0.5
        }

        Dim color2 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#d6d6d6"),
            .Offset = 1.0
        }

        Dim coleccion As New GradientStopCollection From {
            color1,
            color2
        }

        Dim brush As New LinearGradientBrush With {
            .StartPoint = New Point(0.5, 0),
            .EndPoint = New Point(0.5, 1),
            .GradientStops = coleccion
        }

        gridBusqueda.Background = brush

        Dim rowBusqueda1 As New RowDefinition
        Dim rowBusqueda2 As New RowDefinition

        rowBusqueda1.Height = New GridLength(1, GridUnitType.Auto)
        rowBusqueda2.Height = New GridLength(1, GridUnitType.Star)

        gridBusqueda.RowDefinitions.Add(rowBusqueda1)
        gridBusqueda.RowDefinitions.Add(rowBusqueda2)

        '---------------------------------

        Dim spUsuarios As New StackPanel With {
            .Orientation = Orientation.Horizontal,
            .Margin = New Thickness(20, 20, 20, 20)
        }

        spUsuarios.SetValue(Grid.RowProperty, 0)

        Dim spUsuariosBusqueda As New StackPanel With {
            .Orientation = Orientation.Vertical
        }

        Dim tbUsuariosBusqueda As New TextBox With {
            .MinWidth = 350
        }

        AddHandler tbUsuariosBusqueda.TextChanged, AddressOf TbUsuariosBusquedaTextoCambia

        spUsuariosBusqueda.Children.Add(tbUsuariosBusqueda)

        Dim botonUsuariosBusqueda As New Button With {
            .Content = recursos.GetString("Search2"),
            .Foreground = New SolidColorBrush(Colors.White),
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Margin = New Thickness(0, 10, 0, 0),
            .Padding = New Thickness(15, 10, 15, 10),
            .Style = App.Current.Resources("ButtonRevealStyle")
        }

        AddHandler botonUsuariosBusqueda.Click, AddressOf BotonUsuariosBusquedaClick
        AddHandler botonUsuariosBusqueda.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonUsuariosBusqueda.PointerExited, AddressOf UsuarioSaleBoton

        spUsuariosBusqueda.Children.Add(botonUsuariosBusqueda)

        spUsuarios.Children.Add(spUsuariosBusqueda)

        gridBusqueda.Children.Add(spUsuarios)

        '--------------------------

        Dim lvResultados As New ListView With {
            .IsItemClickEnabled = True,
            .ItemContainerStyle = App.Current.Resources("ListViewEstilo1"),
            .Margin = New Thickness(10, 10, 10, 10),
            .Visibility = Visibility.Collapsed
        }

        AddHandler lvResultados.ItemClick, AddressOf LvResultadosItemClick

        lvResultados.SetValue(Grid.RowProperty, 1)

        botonUsuariosBusqueda.Tag = New pepeizq.Twitter.Objetos.BusquedaUsuario(megaUsuario, Nothing, lvResultados)

        gridBusqueda.Children.Add(lvResultados)

        spBusqueda.Children.Add(gridBusqueda)
        gridFondo.Children.Add(spBusqueda)

        Return gridFondo

    End Function

    Private Sub TbUsuariosBusquedaTextoCambia(sender As Object, e As TextChangedEventArgs)

        Dim tb As TextBox = sender

        If tb.Text.Trim.Length > 0 Then
            ApplicationData.Current.LocalSettings.Values("UsuarioBuscar") = tb.Text
        End If

    End Sub

    Private Async Sub BotonUsuariosBusquedaClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.BusquedaUsuario = boton.Tag

        Dim provider As TwitterDataProvider = cosas.MegaUsuario.Servicio.Provider

        Dim usuarioBuscar As String = ApplicationData.Current.LocalSettings.Values("UsuarioBuscar")

        Dim usuarios As List(Of TwitterUsuario) = Await provider.BuscarUsuarios(cosas.MegaUsuario.Servicio.twitterDataProvider._tokens, usuarioBuscar, New TwitterBusquedaUsuariosParser)
        Dim visibilidad As Visibility = Visibility.Collapsed

        If usuarios.Count > 0 Then
            visibilidad = Visibility.Visible
        End If

        Dim lv As ListView = cosas.ListView
        lv.Items.Clear()
        lv.Visibility = visibilidad

        For Each usuario In usuarios
            Dim imagenAvatar As New ImageBrush With {
                .Stretch = Stretch.Uniform,
                .ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))
            }

            Dim circulo As New Ellipse With {
                .Fill = imagenAvatar,
                .Height = 48,
                .Width = 48
            }

            Dim sp1 As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .VerticalAlignment = VerticalAlignment.Center,
                .Tag = New pepeizq.Twitter.Objetos.UsuarioAmpliado(cosas.MegaUsuario, usuario, Nothing)
            }

            sp1.Children.Add(circulo)

            Dim sp2 As New StackPanel With {
                .Orientation = Orientation.Vertical,
                .Margin = New Thickness(10, 0, 0, 0)
            }

            Dim tbNombre As New TextBlock With {
                .Text = usuario.Nombre,
                .TextWrapping = TextWrapping.Wrap
            }

            sp2.Children.Add(tbNombre)

            Dim tbScreenNombre As New TextBlock With {
                .Text = "@" + usuario.ScreenNombre,
                .FontSize = 13
            }

            sp2.Children.Add(tbScreenNombre)

            sp1.Children.Add(sp2)

            Dim botonUsuario As New ListViewItem With {
                .Content = sp1,
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(10, 10, 10, 10),
                .HorizontalAlignment = HorizontalAlignment.Stretch,
                .HorizontalContentAlignment = HorizontalAlignment.Left
            }

            AddHandler botonUsuario.PointerPressed, AddressOf UsuarioPulsaUsuario
            AddHandler botonUsuario.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler botonUsuario.PointerExited, AddressOf UsuarioSaleBoton

            lv.Items.Add(botonUsuario)
        Next

    End Sub

    Private Sub UsuarioPulsaUsuario(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        FichaUsuarioXaml.Generar(cosas, boton)

    End Sub

    Private Sub LvResultadosItemClick(sender As Object, e As ItemClickEventArgs)

        Dim sp As StackPanel = e.ClickedItem
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = sp.Tag

        FichaUsuarioXaml.Generar(cosas, sp)

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
