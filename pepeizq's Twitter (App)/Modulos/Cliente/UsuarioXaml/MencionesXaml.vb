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

        Dim spAbajo As New StackPanel With {
            .Orientation = Orientation.Horizontal,
            .Margin = New Thickness(20, 20, 20, 20),
            .HorizontalAlignment = HorizontalAlignment.Right,
            .VerticalAlignment = VerticalAlignment.Bottom
        }
        spAbajo.SetValue(Grid.RowProperty, 0)

        Dim prTweets As New ProgressRing With {
            .IsActive = True,
            .Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
            .Visibility = Visibility.Collapsed,
            .Margin = New Thickness(0, 0, 15, 0),
            .Padding = New Thickness(10, 10, 10, 10),
            .Name = "prTweetsMenciones" + usuario.ScreenNombre
        }

        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(megaUsuario, Nothing, prTweets, 1, Nothing, Nothing)

        spAbajo.Children.Add(prTweets)

        Dim iconoSubir As New FontAwesome.UWP.FontAwesome With {
            .Foreground = New SolidColorBrush(Colors.White),
            .Icon = FontAwesomeIcon.ArrowCircleUp
        }

        Dim botonSubir As New Button With {
            .Name = "botonSubirArribaMenciones" + usuario.ScreenNombre,
            .Padding = New Thickness(10, 10, 10, 10),
            .BorderBrush = New SolidColorBrush(Colors.White),
            .BorderThickness = New Thickness(1, 1, 1, 1),
            .Content = iconoSubir,
            .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
            .Visibility = Visibility.Collapsed,
            .Tag = svTweets
        }

        AddHandler botonSubir.Click, AddressOf BotonSubirClick
        AddHandler botonSubir.PointerEntered, AddressOf UsuarioEntraBoton
        AddHandler botonSubir.PointerExited, AddressOf UsuarioSaleBoton

        spAbajo.Children.Add(botonSubir)

        gridMenciones.Children.Add(spAbajo)

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
