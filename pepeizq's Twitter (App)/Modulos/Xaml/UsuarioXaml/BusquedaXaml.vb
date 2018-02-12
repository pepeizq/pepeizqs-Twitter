Imports pepeizq.Twitter
Imports pepeizq.Twitter.Busqueda
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Xaml.Shapes

Module BusquedaXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim recursos As New Resources.ResourceLoader

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridBusqueda As New Grid With {
            .Name = "gridBusqueda" + usuario.ScreenNombre
        }
        gridBusqueda.SetValue(Grid.RowProperty, 1)

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
            .Margin = New Thickness(25, 25, 25, 25)
        }

        spUsuarios.SetValue(Grid.RowProperty, 0)

        Dim tbUsuarios As New TextBlock With {
            .Text = recursos.GetString("Users"),
            .Margin = New Thickness(0, 5, 15, 0),
            .VerticalAlignment = VerticalAlignment.Top
        }

        spUsuarios.Children.Add(tbUsuarios)

        Dim spUsuariosBusqueda As New StackPanel With {
            .Orientation = Orientation.Vertical
        }

        Dim tbUsuariosBusqueda As New TextBox With {
            .MinWidth = 200
        }

        AddHandler tbUsuariosBusqueda.TextChanged, AddressOf TbUsuariosBusquedaTextoCambia

        spUsuariosBusqueda.Children.Add(tbUsuariosBusqueda)

        Dim botonUsuariosBusqueda As New Button With {
            .Content = recursos.GetString("Search2"),
            .Foreground = New SolidColorBrush(Colors.White),
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Margin = New Thickness(0, 10, 0, 10),
            .Padding = New Thickness(5, 5, 5, 5),
            .Style = App.Current.Resources("ButtonRevealStyle")
        }

        AddHandler botonUsuariosBusqueda.Click, AddressOf BotonUsuariosBusquedaClick

        spUsuariosBusqueda.Children.Add(botonUsuariosBusqueda)

        spUsuarios.Children.Add(spUsuariosBusqueda)

        gridBusqueda.Children.Add(spUsuarios)

        '--------------------------

        Dim gvResultados As New GridView With {
            .IsItemClickEnabled = True,
            .ItemContainerStyle = App.Current.Resources("GridViewEstilo1"),
            .Margin = New Thickness(10, 10, 10, 10)
        }

        AddHandler gvResultados.ItemClick, AddressOf GvResultadosItemClick

        gvResultados.SetValue(Grid.RowProperty, 1)

        botonUsuariosBusqueda.Tag = New pepeizq.Twitter.Objetos.BusquedaUsuario(megaUsuario, Nothing, gvResultados)

        gridBusqueda.Children.Add(gvResultados)

        Return gridBusqueda

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

        Dim usuarios As List(Of TwitterUsuario) = Await provider.BuscarUsuarios(cosas.MegaUsuario.Usuario.Tokens, usuarioBuscar, New TwitterBusquedaUsuariosParser)

        Dim gv As GridView = cosas.GridView

        gv.Items.Clear()

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
                .Text = usuario.Nombre
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
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(10, 10, 10, 10),
                .HorizontalAlignment = HorizontalAlignment.Stretch,
                .HorizontalContentAlignment = HorizontalAlignment.Left
            }

            AddHandler botonUsuario.PointerPressed, AddressOf UsuarioPulsaUsuario

            gv.Items.Add(botonUsuario)
        Next

    End Sub

    Private Sub UsuarioPulsaUsuario(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

        FichaUsuarioXaml.Generar(cosas, boton)

    End Sub

    Private Sub GvResultadosItemClick(sender As Object, e As ItemClickEventArgs)

        Dim sp As StackPanel = e.ClickedItem
        Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = sp.Tag

        FichaUsuarioXaml.Generar(cosas, sp)

    End Sub

End Module
