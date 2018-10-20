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

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

End Module
