Imports FontAwesome.UWP
Imports pepeizq.Twitter
Imports Windows.UI
Imports Windows.UI.Core

Module MencionesXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario2.Usuario

        Dim gridMenciones As New Grid With {
            .Name = "gridMenciones" + usuario.ScreenNombre,
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
        AddHandler lvTweets.ItemClick, AddressOf LvTweets_ItemClick

        svTweets.Content = lvTweets
        gridMenciones.Children.Add(svTweets)

        '---------------------------------

        Dim iconoSubir As New FontAwesome.UWP.FontAwesome With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Icon = FontAwesomeIcon.ArrowCircleUp
        }

        Dim botonSubir As New Button
        botonSubir.SetValue(Grid.RowProperty, 0)
        botonSubir.Name = "botonSubirArribaMenciones" + usuario.ScreenNombre
        botonSubir.Margin = New Thickness(20, 20, 20, 20)
        botonSubir.HorizontalAlignment = HorizontalAlignment.Right
        botonSubir.VerticalAlignment = VerticalAlignment.Bottom
        botonSubir.Padding = New Thickness(10, 10, 10, 10)
        botonSubir.BorderBrush = New SolidColorBrush(Colors.White)
        botonSubir.BorderThickness = New Thickness(1, 1, 1, 1)
        botonSubir.Content = iconoSubir
        botonSubir.Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
        botonSubir.Visibility = Visibility.Collapsed
        botonSubir.Tag = svTweets

        AddHandler botonSubir.Click, AddressOf BotonSubirClick
        AddHandler botonSubir.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonSubir.PointerExited, AddressOf UsuarioSaleBoton

        gridMenciones.Children.Add(botonSubir)

        '---------------------------------

        Dim pbTweets As New ProgressBar
        pbTweets.SetValue(Grid.RowProperty, 1)
        pbTweets.IsIndeterminate = True
        pbTweets.Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
        pbTweets.Visibility = Visibility.Collapsed
        pbTweets.Margin = New Thickness(10, 10, 10, 10)
        pbTweets.Padding = New Thickness(10, 10, 10, 10)
        pbTweets.Name = "pbTweets" + usuario.ScreenNombre

        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(megaUsuario, Nothing, pbTweets, 1, Nothing, Nothing)

        gridMenciones.Children.Add(pbTweets)

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
