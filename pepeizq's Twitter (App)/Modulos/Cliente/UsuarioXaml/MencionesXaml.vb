Imports FontAwesome.UWP
Imports Microsoft.Advertising.WinRT.UI
Imports pepeizq.Twitter
Imports Windows.ApplicationModel.Store
Imports Windows.UI
Imports Windows.UI.Core

Module MencionesXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim recursos As New Resources.ResourceLoader

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridMenciones As New Grid With {
            .Name = "gridMenciones" + usuario.ID,
            .Visibility = visibilidad
        }
        gridMenciones.SetValue(Grid.RowProperty, 1)

        '---------------------------------

        Dim svTweets As New ScrollViewer
        svTweets.SetValue(Grid.RowProperty, 0)
        AddHandler svTweets.ViewChanging, AddressOf SvTweets_ViewChanging

        Dim lvTweets As New ListView
        lvTweets.SetValue(Grid.RowProperty, 0)
        lvTweets.IsItemClickEnabled = True
        lvTweets.ItemContainerStyle = App.Current.Resources("ListViewEstilo1")
        lvTweets.Tag = usuario
        lvTweets.Name = "lvTweetsMenciones" + usuario.ID
        AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

        svTweets.Content = lvTweets
        gridMenciones.Children.Add(svTweets)

        '---------------------------------

        Dim gridAbajo As New Grid With {
            .Margin = New Thickness(20, 20, 20, 20),
            .HorizontalAlignment = HorizontalAlignment.Stretch,
            .VerticalAlignment = VerticalAlignment.Bottom
        }

        Dim col1 As New ColumnDefinition
        Dim col2 As New ColumnDefinition

        col1.Width = New GridLength(1, GridUnitType.Star)
        col2.Width = New GridLength(100, GridUnitType.Pixel)

        gridAbajo.ColumnDefinitions.Add(col1)
        gridAbajo.ColumnDefinitions.Add(col2)

        'Dim añadirAnuncios As Boolean = False
        'Dim licencia As LicenseInformation = Nothing

        'Try
        '    licencia = CurrentApp.LicenseInformation
        'Catch ex As Exception

        'End Try

        'If Not licencia Is Nothing Then
        '    If Not licencia.ProductLicenses("NoAds").IsActive Then
        '        añadirAnuncios = True
        '    End If
        'Else
        '    añadirAnuncios = True
        'End If

        'If añadirAnuncios = True Then
        '    Dim gridAnuncios As New Grid With {
        '        .Name = "gridAnunciosMenciones" + usuario.ID,
        '        .HorizontalAlignment = HorizontalAlignment.Center,
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .Visibility = Visibility.Collapsed,
        '        .Padding = New Thickness(5, 5, 5, 5),
        '        .BorderThickness = New Thickness(1, 1, 1, 1),
        '        .Background = New SolidColorBrush(Colors.LightGray),
        '        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        '    }
        '    gridAnuncios.SetValue(Grid.ColumnProperty, 0)

        '    Dim colAnuncios1 As New ColumnDefinition
        '    Dim colAnuncios2 As New ColumnDefinition

        '    colAnuncios1.Width = New GridLength(1, GridUnitType.Auto)
        '    colAnuncios2.Width = New GridLength(1, GridUnitType.Auto)

        '    gridAnuncios.ColumnDefinitions.Add(colAnuncios1)
        '    gridAnuncios.ColumnDefinitions.Add(colAnuncios2)

        '    Dim anuncio As New AdControl With {
        '        .AdUnitId = "1100022920",
        '        .Width = 728,
        '        .Height = 90
        '    }
        '    anuncio.SetValue(Grid.ColumnProperty, 0)
        '    gridAnuncios.Children.Add(anuncio)

        '    Dim tbBoton As New TextBlock With {
        '        .Text = recursos.GetString("ButtonRemoveAds"),
        '        .Foreground = New SolidColorBrush(Colors.White)
        '    }

        '    Dim botonQuitarAnuncios As New Button With {
        '        .Padding = New Thickness(15, 10, 15, 10),
        '        .Margin = New Thickness(10, 0, 5, 0),
        '        .Content = tbBoton,
        '        .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        '    }

        '    AddHandler botonQuitarAnuncios.Click, AddressOf BotonQuitarAnunciosClick
        '    AddHandler botonQuitarAnuncios.PointerEntered, AddressOf UsuarioEntraBoton
        '    AddHandler botonQuitarAnuncios.PointerExited, AddressOf UsuarioSaleBoton

        '    botonQuitarAnuncios.SetValue(Grid.ColumnProperty, 1)
        '    gridAnuncios.Children.Add(botonQuitarAnuncios)

        '    gridAbajo.Children.Add(gridAnuncios)
        'End If

        Dim gridAbajoDerecha As New Grid With {
            .HorizontalAlignment = HorizontalAlignment.Right,
            .VerticalAlignment = VerticalAlignment.Bottom
        }
        gridAbajoDerecha.SetValue(Grid.ColumnProperty, 1)

        Dim colAbajoDerecha1 As New ColumnDefinition
        Dim colAbajoDerecha2 As New ColumnDefinition

        colAbajoDerecha1.Width = New GridLength(1, GridUnitType.Auto)
        colAbajoDerecha2.Width = New GridLength(1, GridUnitType.Auto)

        gridAbajoDerecha.ColumnDefinitions.Add(colAbajoDerecha1)
        gridAbajoDerecha.ColumnDefinitions.Add(colAbajoDerecha2)

        Dim prTweetsAbajo As New ProgressRing With {
            .IsActive = True,
            .Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
            .Visibility = Visibility.Collapsed,
            .Margin = New Thickness(0, 0, 15, 0),
            .Padding = New Thickness(10, 10, 10, 10),
            .Name = "prTweetsMenciones" + usuario.ID
        }
        prTweetsAbajo.SetValue(Grid.ColumnProperty, 0)

        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(megaUsuario, Nothing, prTweetsAbajo, 1, Nothing, Nothing)

        gridAbajoDerecha.Children.Add(prTweetsAbajo)

        Dim iconoSubir As New FontAwesome.UWP.FontAwesome With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Icon = FontAwesomeIcon.ArrowCircleUp
        }

        Dim botonSubir As New Button With {
            .Name = "botonSubirArribaMenciones" + usuario.ID,
            .Padding = New Thickness(10, 10, 10, 10),
            .BorderBrush = New SolidColorBrush(Colors.White),
            .BorderThickness = New Thickness(1, 1, 1, 1),
            .Content = iconoSubir,
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Visibility = Visibility.Collapsed,
            .Tag = svTweets
        }
        botonSubir.SetValue(Grid.ColumnProperty, 1)

        AddHandler botonSubir.Click, AddressOf BotonSubirClick
        AddHandler botonSubir.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonSubir.PointerExited, AddressOf UsuarioSaleBoton

        gridAbajoDerecha.Children.Add(botonSubir)

        gridAbajo.Children.Add(gridAbajoDerecha)

        gridMenciones.Children.Add(gridAbajo)

        '---------------------------------

        Return gridMenciones

    End Function

    Private Sub BotonSubirClick(sender As Object, e As RoutedEventArgs)

        Dim botonSubir As Button = sender
        Dim svTweets As ScrollViewer = botonSubir.Tag

        svTweets.ChangeView(Nothing, 0, Nothing)
        botonSubir.Visibility = Visibility.Collapsed

    End Sub

    Private Sub BotonQuitarAnunciosClick(sender As Object, e As RoutedEventArgs)

        Anuncios.Quitar()

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
