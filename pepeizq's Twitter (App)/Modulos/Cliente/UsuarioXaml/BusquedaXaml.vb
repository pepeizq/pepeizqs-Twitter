Imports FontAwesome.UWP
Imports pepeizq.Twitter
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
            .Orientation = Orientation.Vertical,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim rowBusqueda1 As New RowDefinition
        Dim rowBusqueda2 As New RowDefinition

        rowBusqueda1.Height = New GridLength(1, GridUnitType.Star)
        rowBusqueda2.Height = New GridLength(1, GridUnitType.Auto)

        gridFondo.RowDefinitions.Add(rowBusqueda1)
        gridFondo.RowDefinitions.Add(rowBusqueda2)

        Dim tbFondo As New Border With {
            .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
            .VerticalAlignment = VerticalAlignment.Top
        }

        Dim tbBusqueda As New TextBlock With {
            .Text = recursos.GetString("Users"),
            .Padding = New Thickness(20, 10, 15, 10),
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

        Dim spContenidoUsuariosBusqueda As New StackPanel With {
            .Orientation = Orientation.Horizontal
        }

        Dim iconoContenidoUsuariosBusqueda As New FontAwesome.UWP.FontAwesome With {
            .Icon = FontAwesomeIcon.Search,
            .Foreground = New SolidColorBrush(Colors.White)
        }

        spContenidoUsuariosBusqueda.Children.Add(iconoContenidoUsuariosBusqueda)

        Dim tbContenidoUsuariosBusqueda As New TextBlock With {
            .Text = recursos.GetString("Search2"),
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(10, 0, 0, 0)
        }

        spContenidoUsuariosBusqueda.Children.Add(tbContenidoUsuariosBusqueda)

        Dim botonUsuariosBusqueda As New Button With {
            .Content = spContenidoUsuariosBusqueda,
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Margin = New Thickness(0, 15, 0, 0),
            .Padding = New Thickness(10, 10, 10, 10),
            .IsEnabled = False
        }

        tbUsuariosBusqueda.Tag = botonUsuariosBusqueda

        AddHandler botonUsuariosBusqueda.Click, AddressOf BotonUsuariosBusquedaClick
        AddHandler botonUsuariosBusqueda.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonUsuariosBusqueda.PointerExited, AddressOf UsuarioSaleBoton

        spUsuariosBusqueda.Children.Add(botonUsuariosBusqueda)

        spUsuarios.Children.Add(spUsuariosBusqueda)

        gridBusqueda.Children.Add(spUsuarios)
        spBusqueda.Children.Add(gridBusqueda)
        gridFondo.Children.Add(spBusqueda)

        '--------------------------

        Dim gvResultados As New GridView With {
            .IsItemClickEnabled = True,
            .ItemContainerStyle = App.Current.Resources("GridViewEstilo1"),
            .Margin = New Thickness(10, 10, 10, 10),
            .Visibility = Visibility.Collapsed,
            .VerticalAlignment = VerticalAlignment.Top,
            .HorizontalAlignment = HorizontalAlignment.Center
        }

        AddHandler gvResultados.ItemClick, AddressOf LvResultadosItemClick

        gvResultados.SetValue(Grid.RowProperty, 1)

        botonUsuariosBusqueda.Tag = New pepeizq.Twitter.Objetos.BusquedaUsuario(megaUsuario, Nothing, gvResultados, rowBusqueda2)

        gridFondo.Children.Add(gvResultados)

        Return gridFondo

    End Function

    Private Sub TbUsuariosBusquedaTextoCambia(sender As Object, e As TextChangedEventArgs)

        Dim tb As TextBox = sender
        Dim boton As Button = tb.Tag

        If tb.Text.Trim.Length > 0 Then
            ApplicationData.Current.LocalSettings.Values("UsuarioBuscar") = tb.Text
            boton.IsEnabled = True
        Else
            boton.IsEnabled = False
        End If

    End Sub

    Private Async Sub BotonUsuariosBusquedaClick(sender As Object, e As RoutedEventArgs)

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.BusquedaUsuario = boton.Tag

        Dim usuarioBuscar As String = ApplicationData.Current.LocalSettings.Values("UsuarioBuscar")

        Dim listaUsuarios As New List(Of TwitterUsuario)

        listaUsuarios = Await TwitterPeticiones.BuscarUsuarios(listaUsuarios, cosas.MegaUsuario, usuarioBuscar)

        Dim visibilidad As Visibility = Visibility.Collapsed

        If listaUsuarios.Count > 0 Then
            visibilidad = Visibility.Visible
            cosas.Fila2.Height = New GridLength(1, GridUnitType.Star)

            Dim gv As GridView = cosas.Resultados
            gv.Items.Clear()
            gv.Visibility = visibilidad

            For Each usuario In listaUsuarios
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

                Dim botonUsuario As New GridViewItem With {
                    .Content = sp1,
                    .Background = New SolidColorBrush(Colors.Transparent),
                    .BorderBrush = New SolidColorBrush(Colors.Transparent),
                    .BorderThickness = New Thickness(0, 0, 0, 0),
                    .Padding = New Thickness(10, 10, 10, 10),
                    .Margin = New Thickness(10, 10, 10, 10),
                    .HorizontalAlignment = HorizontalAlignment.Stretch,
                    .HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    .MinWidth = 250
                }

                AddHandler botonUsuario.PointerPressed, AddressOf UsuarioPulsaUsuario
                AddHandler botonUsuario.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler botonUsuario.PointerExited, AddressOf UsuarioSaleBoton

                gv.Items.Add(botonUsuario)
            Next
        Else
            cosas.Fila2.Height = New GridLength(1, GridUnitType.Auto)
        End If

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
