Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports pepeizq.Twitter
Imports Windows.UI
Imports Windows.UI.Xaml.Shapes

Module UsuarioXaml

    Public Sub Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim recursos As New Resources.ResourceLoader

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridUsuario As New Grid With {
            .Name = "gridUsuario" + usuario.ScreenNombre,
            .Visibility = visibilidad
        }

        Dim rowUsuario1 As New RowDefinition
        Dim rowUsuario2 As New RowDefinition

        rowUsuario1.Height = New GridLength(1, GridUnitType.Auto)
        rowUsuario2.Height = New GridLength(1, GridUnitType.Star)

        gridUsuario.RowDefinitions.Add(rowUsuario1)
        gridUsuario.RowDefinitions.Add(rowUsuario2)

        '---------------------------------

        Dim gridBarraSuperior As New Grid With {
            .Name = "gridBarraSuperior" + usuario.ScreenNombre,
            .Margin = New Thickness(20, 10, 20, 0)
        }

        '---------------------------------

        Dim gridBarraSuperiorBotones As New Grid
        gridBarraSuperiorBotones.SetValue(Grid.ColumnProperty, 0)
        gridBarraSuperiorBotones.Name = "gridBarraSuperiorBotones" + usuario.ScreenNombre
        gridBarraSuperiorBotones.HorizontalAlignment = HorizontalAlignment.Left

        Dim rowBarraSuperiorBotones1 As New RowDefinition
        Dim rowBarraSuperiorBotones2 As New RowDefinition

        rowBarraSuperiorBotones1.Height = New GridLength(1, GridUnitType.Auto)
        rowBarraSuperiorBotones2.Height = New GridLength(1, GridUnitType.Auto)

        gridBarraSuperiorBotones.RowDefinitions.Add(rowBarraSuperiorBotones1)
        gridBarraSuperiorBotones.RowDefinitions.Add(rowBarraSuperiorBotones2)

        '---------------------------------

        Dim spBotonesSuperior As New StackPanel
        spBotonesSuperior.SetValue(Grid.RowProperty, 0)
        spBotonesSuperior.Orientation = Orientation.Horizontal

        Dim menu As New Menu With {
            .Margin = New Thickness(0, 0, 10, 0)
        }

        Dim tbHeader As New TextBlock With {
            .Text = Char.ConvertFromUtf32(57365),
            .Padding = New Thickness(5, 5, 5, 5),
            .FontSize = 16,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim menuItem As New MenuItem With {
            .Header = tbHeader,
            .FontFamily = New FontFamily("Segoe MDL2 Assets"),
            .BorderThickness = New Thickness(0, 0, 0, 0),
            .Foreground = New SolidColorBrush(Colors.White)
        }

        Dim menuItemCuentas As New MenuFlyoutSubItem With {
            .Text = recursos.GetString("Accounts")
        }

        Dim helper As New LocalObjectStorageHelper

        Dim listaUsuarios As New List(Of TwitterUsuario)

        If helper.KeyExists("listaUsuarios2") Then
            listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios2")
        End If

        For Each item In listaUsuarios
            Dim subCuenta As New MenuFlyoutItem With {
                .Text = item.Nombre + " (@" + item.ScreenNombre + ")",
                .Tag = item
            }

            AddHandler subCuenta.Click, AddressOf BotonCambiarCuentaClick
            menuItemCuentas.Items.Add(subCuenta)
        Next

        menuItem.Items.Add(menuItemCuentas)

        menuItem.Items.Add(New MenuFlyoutSeparator)

        Dim menuItemConfig As New MenuFlyoutItem With {
            .Text = recursos.GetString("Config")
        }

        AddHandler menuItemConfig.Click, AddressOf BotonConfigClick
        menuItem.Items.Add(menuItemConfig)

        Dim menuItemCosas As New MenuFlyoutItem With {
            .Text = recursos.GetString("MoreThings")
        }

        AddHandler menuItemCosas.Click, AddressOf BotonCosasClick
        menuItem.Items.Add(menuItemCosas)

        menu.Items.Add(menuItem)

        spBotonesSuperior.Children.Add(menu)

        Dim imagenAvatar As New ImageBrush With {
            .Stretch = Stretch.Uniform
        }

        Try
            imagenAvatar.ImageSource = New BitmapImage(New Uri(usuario.ImagenAvatar))
        Catch ex As Exception

        End Try

        Dim circulo As New Ellipse With {
            .Fill = imagenAvatar,
            .VerticalAlignment = VerticalAlignment.Center,
            .Margin = New Thickness(0, 0, 20, 0),
            .Height = 30,
            .Width = 30
        }

        spBotonesSuperior.Children.Add(circulo)

        spBotonesSuperior.Children.Add(ConstructorBotones("botonInicio" + usuario.ScreenNombre, 59407, recursos.GetString("Home"), usuario))
        spBotonesSuperior.Children.Add(ConstructorBotones("botonMenciones" + usuario.ScreenNombre, 60047, recursos.GetString("Mentions"), usuario))
        spBotonesSuperior.Children.Add(ConstructorBotones("botonEscribir" + usuario.ScreenNombre, 59151, recursos.GetString("WriteTweet"), usuario))

        gridBarraSuperiorBotones.Children.Add(spBotonesSuperior)

        '---------------------------------

        Dim gridLineaInferior As New Grid
        gridLineaInferior.SetValue(Grid.RowProperty, 1)
        gridLineaInferior.Background = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
        gridLineaInferior.Height = 6
        gridLineaInferior.Margin = New Thickness(0, 4, 0, 0)

        gridBarraSuperiorBotones.Children.Add(gridLineaInferior)

        gridBarraSuperior.Children.Add(gridBarraSuperiorBotones)

        '---------------------------------

        gridUsuario.Children.Add(gridBarraSuperior)
        gridUsuario.Children.Add(InicioXaml.Generar(megaUsuario, visibilidad))
        gridUsuario.Children.Add(MencionesXaml.Generar(megaUsuario))
        gridUsuario.Children.Add(EscribirXaml.Generar(megaUsuario))

        '---------------------------------

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")
        gridPrincipal.Children.Insert(0, gridUsuario)

        If visibilidad = Visibility.Visible Then
            Dim botonInicio As Button = pagina.FindName("botonInicio" + usuario.ScreenNombre)
            BotonClick(botonInicio, New RoutedEventArgs)
        End If

        TwitterTimeLineInicio.CargarTweets(megaUsuario, Nothing)
        TwitterTimeLineMenciones.CargarTweets(megaUsuario, Nothing)

        TwitterStream.Iniciar(megaUsuario)

    End Sub

    Private Function ConstructorBotones(titulo As String, icono As Integer, texto As String, usuario As TwitterUsuario)

        Dim tbSimbolo As New TextBlock With {
            .Text = Char.ConvertFromUtf32(icono),
            .FontFamily = New FontFamily("Segoe MDL2 Assets"),
            .Foreground = New SolidColorBrush(Colors.White),
            .FontSize = 16,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim tbTexto As New TextBlock With {
            .Text = texto,
            .Foreground = New SolidColorBrush(Colors.White),
            .Margin = New Thickness(10, 0, 0, 0),
            .FontSize = 15
        }

        Dim sp As New StackPanel With {
            .Orientation = Orientation.Horizontal
        }

        sp.Children.Add(tbSimbolo)
        sp.Children.Add(tbTexto)

        Dim boton As New Button With {
            .Name = titulo,
            .Background = New SolidColorBrush(Colors.Transparent),
            .Padding = New Thickness(12, 8, 12, 8),
            .Content = sp,
            .BorderThickness = New Thickness(0, 0, 0, 0),
            .Tag = usuario,
            .Style = App.Current.Resources("ButtonRevealStyle")
        }

        AddHandler boton.Click, AddressOf BotonClick
        AddHandler boton.PointerEntered, AddressOf BotonUsuarioEntra
        AddHandler boton.PointerExited, AddressOf BotonUsuarioSale

        Return boton

    End Function

    Private Sub BotonUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim sp As StackPanel = boton.Content

        If sp.Tag = "0" Then
            For Each item In sp.Children
                If TypeOf item Is SymbolIcon Then
                    Dim si As SymbolIcon = item
                    si.Foreground = New SolidColorBrush(Colors.White)
                End If

                If TypeOf item Is TextBlock Then
                    Dim tb As TextBlock = item
                    tb.Foreground = New SolidColorBrush(Colors.White)
                End If
            Next
        End If

    End Sub

    Private Sub BotonUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim sp As StackPanel = boton.Content

        If sp.Tag = "0" Then
            For Each item In sp.Children
                If TypeOf item Is SymbolIcon Then
                    Dim si As SymbolIcon = item
                    si.Foreground = New SolidColorBrush(Colors.White)
                End If

                If TypeOf item Is TextBlock Then
                    Dim tb As TextBlock = item
                    tb.Foreground = New SolidColorBrush(Colors.White)
                End If
            Next
        End If

    End Sub

    Public Sub BotonClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim boton As Button = sender
        Dim usuario As TwitterUsuario = boton.Tag

        Dim sp As StackPanel = boton.Content
        Dim tb As TextBlock = sp.Children(1)

        If tb.Text = recursos.GetString("Home") Then

            Dim grid As Grid = pagina.FindName("gridTweets" + usuario.ScreenNombre)
            GridVisibilidad(grid, boton)

        ElseIf tb.Text = recursos.GetString("Mentions") Then

            Dim grid As Grid = pagina.FindName("gridMenciones" + usuario.ScreenNombre)
            GridVisibilidad(grid, boton)

        ElseIf tb.Text = recursos.GetString("WriteTweet") Then

            Dim grid As Grid = pagina.FindName("gridEscribir" + usuario.ScreenNombre)
            GridVisibilidad(grid, boton)

        End If

    End Sub

    Private Sub GridVisibilidad(gridElegido As Grid, botonPulsado As Button)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim usuario As TwitterUsuario = botonPulsado.Tag

        Dim grid As Grid = pagina.FindName("gridBarraSuperiorBotones" + usuario.ScreenNombre)
        Dim spBotones As StackPanel = grid.Children(0)

        For Each item In spBotones.Children
            If TypeOf item Is Button Then
                Dim boton As Button = item

                boton.Background = New SolidColorBrush(Colors.Transparent)

                Dim spBoton As StackPanel = boton.Content
                spBoton.Tag = "0"

                For Each subitem In spBoton.Children
                    If TypeOf subitem Is SymbolIcon Then
                        Dim si As SymbolIcon = subitem
                        si.Foreground = New SolidColorBrush(Colors.White)
                    End If

                    If TypeOf subitem Is TextBlock Then
                        Dim tb As TextBlock = subitem
                        tb.Foreground = New SolidColorBrush(Colors.White)
                    End If
                Next
            End If
        Next

        botonPulsado.Background = New SolidColorBrush(App.Current.Resources("ColorCuarto"))

        Dim spBotonPulsado As StackPanel = botonPulsado.Content
        spBotonPulsado.Tag = "1"

        For Each item In spBotonPulsado.Children
            If TypeOf item Is SymbolIcon Then
                Dim si As SymbolIcon = item
                si.Foreground = New SolidColorBrush(Colors.White)
            End If

            If TypeOf item Is TextBlock Then
                Dim tb As TextBlock = item
                tb.Foreground = New SolidColorBrush(Colors.White)
            End If
        Next

        '------------------------------

        Dim gridTweets As Grid = pagina.FindName("gridTweets" + usuario.ScreenNombre)
        gridTweets.Visibility = Visibility.Collapsed

        Dim gridMenciones As Grid = pagina.FindName("gridMenciones" + usuario.ScreenNombre)
        gridMenciones.Visibility = Visibility.Collapsed

        Dim gridEscribir As Grid = pagina.FindName("gridEscribir" + usuario.ScreenNombre)
        gridEscribir.Visibility = Visibility.Collapsed

        gridElegido.Visibility = Visibility.Visible

    End Sub

    Public Sub BotonCambiarCuentaClick(sender As Object, e As RoutedEventArgs)

        Dim boton As MenuFlyoutItem = sender
        Dim usuario As TwitterUsuario = boton.Tag

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridPrincipal As Grid = pagina.FindName("gridPrincipal")

        For Each grid As Grid In gridPrincipal.Children
            If grid.Name.Contains("gridUsuario") Then
                If Not grid.Name = "gridUsuarioAmpliado" Then
                    For Each subgrid As Grid In grid.Children
                        subgrid.Visibility = Visibility.Collapsed
                    Next

                    grid.Visibility = Visibility.Collapsed
                End If
            End If

            If grid.Name = "gridUsuario" + usuario.ScreenNombre Then
                For Each subgrid As Grid In grid.Children
                    If subgrid.Name.Contains("gridBarraSuperior" + usuario.ScreenNombre) Then
                        subgrid.Visibility = Visibility.Visible
                    ElseIf subgrid.Name.Contains("gridTweets" + usuario.ScreenNombre) Then
                        subgrid.Visibility = Visibility.Visible
                    Else
                        subgrid.Visibility = Visibility.Collapsed
                    End If
                Next

                grid.Visibility = Visibility.Visible
            End If
        Next

        Dim botonInicio As Button = pagina.FindName("botonInicio" + usuario.ScreenNombre)
        BotonClick(botonInicio, New RoutedEventArgs)

    End Sub

    Private Sub BotonConfigClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridConfig As Grid = pagina.FindName("gridConfig")

        Dim paginaPrincipal As New MainPage
        paginaPrincipal.GridVisibilidad(gridConfig, recursos.GetString("Config"))

    End Sub

    Private Sub BotonCosasClick(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridConfig As Grid = pagina.FindName("gridMasCosas")

        Dim paginaPrincipal As New MainPage
        paginaPrincipal.GridVisibilidad(gridConfig, recursos.GetString("MoreThings"))

    End Sub

End Module
