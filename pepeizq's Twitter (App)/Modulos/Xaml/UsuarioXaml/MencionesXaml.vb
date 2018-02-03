Imports pepeizq.Twitter

Module MencionesXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridMenciones As New Grid With {
            .Name = "gridMenciones" + usuario.ScreenNombre
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

        Dim pbTweets As New ProgressBar
        pbTweets.SetValue(Grid.RowProperty, 1)
        pbTweets.IsIndeterminate = True
        pbTweets.Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
        pbTweets.Visibility = Visibility.Collapsed
        pbTweets.Margin = New Thickness(10, 10, 10, 10)
        pbTweets.Padding = New Thickness(10, 10, 10, 10)
        pbTweets.Name = "pbTweets" + usuario.ScreenNombre

        svTweets.Tag = New pepeizq.Twitter.Objetos.ScrollViewerTweets(megaUsuario, Nothing, pbTweets, 1)

        gridMenciones.Children.Add(pbTweets)

        '---------------------------------

        Return gridMenciones

    End Function

End Module
