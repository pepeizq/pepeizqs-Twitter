Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports Windows.Storage
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Namespace pepeizq.Twitter.Xaml
    Module BusquedaUsuarios

        Public Sub Generar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim grid As Grid = pagina.FindName("gridBusquedaUsuarios")
            Dim megaUsuario As MegaUsuario = grid.Tag

            Dim botonBuscar As Button = pagina.FindName("botonBuscarUsuarios")
            botonBuscar.Tag = megaUsuario

            AddHandler botonBuscar.Click, AddressOf BotonBuscarUsuariosClick
            AddHandler botonBuscar.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler botonBuscar.PointerExited, AddressOf UsuarioSaleBoton

            Dim tbBuscar As TextBox = pagina.FindName("tbBuscarUsuarios")
            tbBuscar.Tag = botonBuscar

            AddHandler tbBuscar.TextChanged, AddressOf TbBuscarTextoCambia

        End Sub

        Private Async Sub BotonBuscarUsuariosClick(sender As Object, e As RoutedEventArgs)

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = sender
            boton.IsEnabled = False

            Dim gridMensaje As Grid = pagina.FindName("gridBusquedaUsuariosMensaje")
            gridMensaje.Visibility = Visibility.Collapsed

            Dim lv As ListView = pagina.FindName("lvResultadosBusquedaUsuarios")
            lv.Items.Clear()

            AddHandler lv.ItemClick, AddressOf LvResultadosItemClick

            Dim pr As ProgressRing = pagina.FindName("prBusquedaUsuarios")
            pr.Visibility = Visibility.Visible

            Dim megaUsuario As MegaUsuario = boton.Tag

            Dim usuarioBuscar As String = ApplicationData.Current.LocalSettings.Values("UsuarioBuscar")

            Dim listaUsuarios As New List(Of TwitterUsuario)

            listaUsuarios = Await TwitterPeticiones.BuscarUsuarios(listaUsuarios, megaUsuario, usuarioBuscar)

            pr.Visibility = Visibility.Collapsed

            Dim gridNoResultados As Grid = pagina.FindName("gridBusquedaUsuariosNo")

            If listaUsuarios.Count > 0 Then
                gridNoResultados.Visibility = Visibility.Collapsed

                For Each usuario In listaUsuarios
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
                        .Tag = New Objetos.UsuarioAmpliado(megaUsuario, usuario, Nothing)
                    }

                    sp1.Children.Add(circulo)

                    Dim sp2 As New StackPanel With {
                        .Orientation = Orientation.Vertical,
                        .VerticalAlignment = VerticalAlignment.Center,
                        .Margin = New Thickness(15, 0, 0, 0)
                    }

                    Dim sp3 As New StackPanel With {
                        .Orientation = Orientation.Horizontal
                    }

                    Dim tbNombre As New TextBlock With {
                        .Text = usuario.Nombre,
                        .TextWrapping = TextWrapping.Wrap
                    }

                    sp3.Children.Add(tbNombre)

                    Dim tbScreenNombre As New TextBlock With {
                        .Text = "@" + usuario.ScreenNombre,
                        .FontSize = 13,
                        .Margin = New Thickness(5, 0, 0, 0)
                    }

                    sp3.Children.Add(tbScreenNombre)

                    sp2.Children.Add(sp3)

                    Dim tbNumFollowers As New TextBlock With {
                        .Text = recursos.GetString("Followers") + " " + String.Format("{0:n0}", Integer.Parse(usuario.Followers)),
                        .FontSize = 13
                    }

                    sp2.Children.Add(tbNumFollowers)

                    sp1.Children.Add(sp2)

                    If usuario.Verificado = True Then
                        Dim imagenVerificado As New ImageEx With {
                            .Source = "Assets\Verificado.png",
                            .IsCacheEnabled = True,
                            .VerticalAlignment = VerticalAlignment.Center,
                            .Margin = New Thickness(15, 0, 0, 0)
                        }

                        sp1.Children.Add(imagenVerificado)
                    End If

                    Dim botonUsuario As New ListViewItem With {
                        .Content = sp1,
                        .Background = brush,
                        .BorderBrush = New SolidColorBrush(Colors.Transparent),
                        .BorderThickness = New Thickness(0, 0, 0, 0),
                        .Padding = New Thickness(10, 10, 10, 10)
                    }

                    AddHandler botonUsuario.PointerEntered, AddressOf UsuarioEntraBoton
                    AddHandler botonUsuario.PointerExited, AddressOf UsuarioSaleBoton

                    lv.Items.Add(botonUsuario)
                Next
            Else
                gridNoResultados.Visibility = Visibility.Visible
            End If

            boton.IsEnabled = True

        End Sub

        Private Sub TbBuscarTextoCambia(sender As Object, e As TextChangedEventArgs)

            Dim tb As TextBox = sender
            Dim boton As Button = tb.Tag

            If tb.Text.Trim.Length > 0 Then
                ApplicationData.Current.LocalSettings.Values("UsuarioBuscar") = tb.Text
                boton.IsEnabled = True
            Else
                boton.IsEnabled = False
            End If

        End Sub

        Private Sub LvResultadosItemClick(sender As Object, e As ItemClickEventArgs)

            Dim sp As StackPanel = e.ClickedItem
            Dim cosas As Objetos.UsuarioAmpliado = sp.Tag

            FichaUsuarioXaml.Generar(cosas, sp)

        End Sub

        Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace

