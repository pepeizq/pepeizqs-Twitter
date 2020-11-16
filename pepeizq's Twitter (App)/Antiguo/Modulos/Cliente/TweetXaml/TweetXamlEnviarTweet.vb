Imports FontAwesome.UWP
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports NeoSmart.Unicode
Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Documents

Namespace pepeizq.Twitter.Xaml
    Module TweetEnviarTweet

        'Dim cosas As Objetos.UsuarioAmpliado = Nothing

        'Public Function Generar(tweet As Tweet, megaUsuario As MegaUsuario, visibilidad As Visibility, color As Color)

        '    If color = Nothing Then
        '        color = App.Current.Resources("ColorCuarto")
        '    End If

        '    Dim colorBoton As Color = Nothing

        '    If color = App.Current.Resources("ColorCuarto") Then
        '        colorBoton = App.Current.Resources("ColorSecundario")
        '    Else
        '        colorBoton = color
        '    End If

        '    Dim recursos As New Resources.ResourceLoader

        '    Dim nombreGrid As String = Nothing

        '    If tweet Is Nothing Then
        '        nombreGrid = "gridTweetEscribir"
        '    Else
        '        nombreGrid = "gridTweetEscribir" + tweet.ID + megaUsuario.Usuario.ID
        '    End If

        '    Dim gridTweetEscribir As New Grid With {
        '        .Name = nombreGrid,
        '        .Visibility = visibilidad,
        '        .Margin = New Thickness(5, 10, 0, 5)
        '    }

        '    Dim rowTweetEscribir1 As New RowDefinition
        '    Dim rowTweetEscribir2 As New RowDefinition

        '    rowTweetEscribir1.Height = New GridLength(1, GridUnitType.Auto)
        '    rowTweetEscribir2.Height = New GridLength(1, GridUnitType.Auto)

        '    gridTweetEscribir.RowDefinitions.Add(rowTweetEscribir1)
        '    gridTweetEscribir.RowDefinitions.Add(rowTweetEscribir2)

        '    '---------------------------------

        '    Dim listaMenciones As TweetMencion() = Nothing

        '    If Not tweet Is Nothing Then
        '        Dim tbRespondiendo As New TextBlock
        '        tbRespondiendo.SetValue(Grid.RowProperty, 0)

        '        Dim tbRespondiendoSpan As New Span

        '        Dim fragmento1 As New Run With {
        '            .Text = recursos.GetString("ReplyingTo") + ": "
        '        }

        '        tbRespondiendoSpan.Inlines.Add(fragmento1)

        '        cosas = New Objetos.UsuarioAmpliado(megaUsuario, Nothing, Nothing)

        '        If tweet.Retweet Is Nothing Then
        '            If Not tweet.Usuario.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
        '                Dim fragmento2 As New Run With {
        '                    .Text = "@" + tweet.Usuario.ScreenNombre
        '                }

        '                Dim enlace As New Hyperlink With {
        '                    .TextDecorations = Nothing,
        '                    .Foreground = New SolidColorBrush(color)
        '                }

        '                AddHandler enlace.Click, AddressOf EnlaceClick

        '                enlace.Inlines.Add(fragmento2)
        '                tbRespondiendoSpan.Inlines.Add(enlace)
        '            End If

        '            listaMenciones = tweet.Entidades.Menciones
        '        Else
        '            If Not tweet.Retweet.Usuario.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
        '                Dim fragmento2 As New Run With {
        '                    .Text = "@" + tweet.Retweet.Usuario.ScreenNombre
        '                }

        '                Dim enlace As New Hyperlink With {
        '                    .TextDecorations = Nothing,
        '                    .Foreground = New SolidColorBrush(color)
        '                }

        '                AddHandler enlace.Click, AddressOf EnlaceClick

        '                enlace.Inlines.Add(fragmento2)
        '                tbRespondiendoSpan.Inlines.Add(enlace)
        '            End If

        '            listaMenciones = tweet.Retweet.Entidades.Menciones
        '        End If

        '        For Each mencion In listaMenciones
        '            If Not mencion.ScreenNombre = megaUsuario.Usuario.ScreenNombre Then
        '                If tbRespondiendoSpan.Inlines.Count > 1 Then
        '                    Dim fragmentoAnterior As New Run With {
        '                       .Text = ", "
        '                    }

        '                    tbRespondiendoSpan.Inlines.Add(fragmentoAnterior)
        '                End If

        '                Dim fragmentoMencion As New Run With {
        '                    .Text = "@" + mencion.ScreenNombre
        '                }

        '                Dim enlaceMencion As New Hyperlink With {
        '                    .TextDecorations = Nothing,
        '                    .Foreground = New SolidColorBrush(color)
        '                }

        '                AddHandler enlaceMencion.Click, AddressOf EnlaceClick

        '                enlaceMencion.Inlines.Add(fragmentoMencion)
        '                tbRespondiendoSpan.Inlines.Add(enlaceMencion)
        '            End If
        '        Next

        '        tbRespondiendo.Inlines.Add(tbRespondiendoSpan)

        '        gridTweetEscribir.Children.Add(tbRespondiendo)
        '    End If

        '    '---------------------------------

        '    Dim gridCentro As New Grid
        '    gridCentro.SetValue(Grid.RowProperty, 1)

        '    Dim colGridCentro1 As New ColumnDefinition
        '    Dim colGridCentro2 As New ColumnDefinition
        '    Dim colGridCentro3 As New ColumnDefinition

        '    colGridCentro1.Width = New GridLength(1, GridUnitType.Auto)
        '    colGridCentro2.Width = New GridLength(1, GridUnitType.Auto)
        '    colGridCentro3.Width = New GridLength(1, GridUnitType.Auto)

        '    gridCentro.ColumnDefinitions.Add(colGridCentro1)
        '    gridCentro.ColumnDefinitions.Add(colGridCentro2)
        '    gridCentro.ColumnDefinitions.Add(colGridCentro3)

        '    Dim gridEscribir As New Grid
        '    gridEscribir.SetValue(Grid.ColumnProperty, 0)

        '    Dim rowGridEscribir1 As New RowDefinition
        '    Dim rowGridEscribir2 As New RowDefinition

        '    rowGridEscribir1.Height = New GridLength(1, GridUnitType.Auto)
        '    rowGridEscribir2.Height = New GridLength(1, GridUnitType.Auto)

        '    gridEscribir.RowDefinitions.Add(rowGridEscribir1)
        '    gridEscribir.RowDefinitions.Add(rowGridEscribir2)

        '    Dim bordeTbMensaje As New Border With {
        '        .BorderThickness = New Thickness(1, 1, 1, 1),
        '        .BorderBrush = New SolidColorBrush(color),
        '        .Height = 110,
        '        .Width = 500,
        '        .HorizontalAlignment = HorizontalAlignment.Left,
        '        .Margin = New Thickness(0, 10, 0, 15)
        '    }

        '    Dim tbMensaje As New TextBox With {
        '        .MaxLength = 280,
        '        .AcceptsReturn = True,
        '        .TextWrapping = TextWrapping.Wrap,
        '        .BorderThickness = New Thickness(0, 0, 0, 0)
        '    }

        '    AddHandler tbMensaje.TextChanged, AddressOf TbTweetEscribirTextChanged

        '    bordeTbMensaje.SetValue(Grid.RowProperty, 0)
        '    bordeTbMensaje.Child = tbMensaje

        '    gridEscribir.Children.Add(bordeTbMensaje)

        '    Dim gridEscribirInferior As New Grid
        '    gridEscribirInferior.SetValue(Grid.RowProperty, 1)
        '    gridEscribirInferior.HorizontalAlignment = HorizontalAlignment.Left
        '    gridEscribirInferior.Margin = New Thickness(0, 0, 0, 5)

        '    Dim colGridInferior1 As New ColumnDefinition
        '    Dim colGridInferior2 As New ColumnDefinition
        '    Dim colGridInferior3 As New ColumnDefinition

        '    colGridInferior1.Width = New GridLength(1, GridUnitType.Auto)
        '    colGridInferior2.Width = New GridLength(1, GridUnitType.Auto)
        '    colGridInferior3.Width = New GridLength(1, GridUnitType.Auto)

        '    gridEscribirInferior.ColumnDefinitions.Add(colGridInferior1)
        '    gridEscribirInferior.ColumnDefinitions.Add(colGridInferior2)
        '    gridEscribirInferior.ColumnDefinitions.Add(colGridInferior3)

        '    '---------------------------------

        '    Dim botonEnviarTweet As New Button
        '    botonEnviarTweet.SetValue(Grid.ColumnProperty, 0)
        '    botonEnviarTweet.Padding = New Thickness(10, 10, 10, 10)
        '    botonEnviarTweet.Background = New SolidColorBrush(colorBoton)
        '    botonEnviarTweet.Margin = New Thickness(0, 0, 15, 0)
        '    botonEnviarTweet.IsEnabled = False

        '    Dim spBotonEnviar As New StackPanel With {
        '        .Orientation = Orientation.Horizontal
        '    }

        '    Dim simboloBotonEnviar As New FontAwesome.UWP.FontAwesome With {
        '        .Icon = FontAwesomeIcon.Send,
        '        .Margin = New Thickness(0, 0, 10, 0),
        '        .Foreground = New SolidColorBrush(Colors.White),
        '        .VerticalAlignment = VerticalAlignment.Center
        '    }

        '    spBotonEnviar.Children.Add(simboloBotonEnviar)

        '    Dim tbBotonEnviar As New TextBlock With {
        '        .Foreground = New SolidColorBrush(Colors.White),
        '        .Text = recursos.GetString("SendTweet")
        '    }

        '    spBotonEnviar.Children.Add(tbBotonEnviar)

        '    botonEnviarTweet.Content = spBotonEnviar
        '    AddHandler botonEnviarTweet.Click, AddressOf BotonEnviarTweetClick
        '    AddHandler botonEnviarTweet.PointerEntered, AddressOf UsuarioEntraBoton
        '    AddHandler botonEnviarTweet.PointerExited, AddressOf UsuarioSaleBoton

        '    gridEscribirInferior.Children.Add(botonEnviarTweet)

        '    '---------------------------------

        '    Dim prContadorCaracteres As New RadialProgressBar
        '    prContadorCaracteres.SetValue(Grid.ColumnProperty, 1)
        '    prContadorCaracteres.Maximum = 280
        '    prContadorCaracteres.VerticalAlignment = VerticalAlignment.Center
        '    prContadorCaracteres.Height = 44
        '    prContadorCaracteres.Width = 44

        '    gridEscribirInferior.Children.Add(prContadorCaracteres)

        '    Dim tbContadorCaracteres As New TextBlock
        '    tbContadorCaracteres.SetValue(Grid.ColumnProperty, 1)
        '    tbContadorCaracteres.VerticalAlignment = VerticalAlignment.Center
        '    tbContadorCaracteres.HorizontalAlignment = HorizontalAlignment.Center

        '    gridEscribirInferior.Children.Add(tbContadorCaracteres)

        '    '---------------------------------

        '    Dim prEnviando As New ProgressRing
        '    prEnviando.SetValue(Grid.ColumnProperty, 2)
        '    prEnviando.Width = 42
        '    prEnviando.Height = 42
        '    prEnviando.Margin = New Thickness(15, 0, 0, 0)
        '    prEnviando.Foreground = New SolidColorBrush(color)
        '    prEnviando.Visibility = Visibility.Collapsed

        '    gridEscribirInferior.Children.Add(prEnviando)

        '    tbMensaje.Tag = New Objetos.EnviarTweet.TextoCambia(botonEnviarTweet, tbContadorCaracteres, prContadorCaracteres)

        '    gridEscribir.Children.Add(gridEscribirInferior)

        '    gridCentro.Children.Add(gridEscribir)

        '    '---------------------------------

        '    Dim spBotones1 As New StackPanel
        '    spBotones1.SetValue(Grid.ColumnProperty, 1)
        '    spBotones1.Orientation = Orientation.Vertical
        '    spBotones1.Margin = New Thickness(15, 10, 15, 10)

        '    Dim botonEmojis As New Button
        '    botonEmojis.SetValue(Grid.ColumnProperty, 3)
        '    botonEmojis.Padding = New Thickness(10, 10, 10, 10)
        '    botonEmojis.VerticalAlignment = VerticalAlignment.Center
        '    botonEmojis.Background = New SolidColorBrush(colorBoton)
        '    AddHandler botonEmojis.Click, AddressOf BotonEmojisClick
        '    AddHandler botonEmojis.PointerEntered, AddressOf UsuarioEntraBoton
        '    AddHandler botonEmojis.PointerExited, AddressOf UsuarioSaleBoton

        '    Dim iconoEmojis As New FontAwesome.UWP.FontAwesome With {
        '        .Foreground = New SolidColorBrush(Colors.White),
        '        .Icon = FontAwesomeIcon.SmileOutline
        '    }

        '    botonEmojis.Content = iconoEmojis

        '    spBotones1.Children.Add(botonEmojis)

        '    Dim popupEmojis As New Popup
        '    popupEmojis.SetValue(Grid.ColumnProperty, 0)
        '    popupEmojis.IsOpen = False
        '    popupEmojis.VerticalOffset = 50

        '    Dim gridEmojis As New Grid With {
        '        .Background = New SolidColorBrush(Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#FFE4E4E4")),
        '        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorSecundario")),
        '        .BorderThickness = New Thickness(1, 1, 1, 1)
        '    }

        '    Dim gvEmojis As New GridView With {
        '        .MaxWidth = 550,
        '        .HorizontalContentAlignment = HorizontalAlignment.Center,
        '        .IsItemClickEnabled = True,
        '        .ItemContainerStyle = App.Current.Resources("GridViewEstilo1"),
        '        .Tag = tbMensaje
        '    }

        '    AddHandler gvEmojis.ItemClick, AddressOf GvEmojisItemClick

        '    gridEmojis.Children.Add(gvEmojis)
        '    popupEmojis.Child = gridEmojis
        '    botonEmojis.Tag = popupEmojis

        '    gridCentro.Children.Add(popupEmojis)

        '    '---------------------------------

        '    Dim spBotonGif As New StackPanel With {
        '        .Orientation = Orientation.Vertical,
        '        .Margin = New Thickness(0, 15, 0, 0)
        '    }

        '    Dim botonGif As New Button With {
        '        .Padding = New Thickness(10, 10, 10, 10),
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .Background = New SolidColorBrush(colorBoton)
        '    }
        '    AddHandler botonGif.Click, AddressOf BotonGifClick
        '    AddHandler botonGif.PointerEntered, AddressOf UsuarioEntraBoton
        '    AddHandler botonGif.PointerExited, AddressOf UsuarioSaleBoton

        '    Dim tbGif As New TextBlock With {
        '        .Foreground = New SolidColorBrush(Colors.White),
        '        .Text = "gif"
        '    }

        '    botonGif.Content = tbGif

        '    spBotonGif.Children.Add(botonGif)

        '    Dim spGif As New StackPanel With {
        '        .Orientation = Orientation.Vertical,
        '        .Visibility = Visibility.Collapsed,
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .MinHeight = 0
        '    }

        '    spBotonGif.Children.Add(spGif)

        '    spBotones1.Children.Add(spBotonGif)

        '    gridCentro.Children.Add(spBotones1)

        '    '---------------------------------

        '    Dim spBotones2 As New StackPanel
        '    spBotones2.SetValue(Grid.ColumnProperty, 2)
        '    spBotones2.Orientation = Orientation.Vertical
        '    spBotones2.Margin = New Thickness(0, 10, 0, 10)

        '    Dim spBotonImagen As New StackPanel With {
        '        .Orientation = Orientation.Horizontal
        '    }

        '    Dim botonImagenes As New Button With {
        '        .Padding = New Thickness(10, 10, 10, 10),
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .Background = New SolidColorBrush(colorBoton)
        '    }
        '    AddHandler botonImagenes.Click, AddressOf BotonImagenesClick
        '    AddHandler botonImagenes.PointerEntered, AddressOf UsuarioEntraBoton
        '    AddHandler botonImagenes.PointerExited, AddressOf UsuarioSaleBoton

        '    Dim iconoImagenes As New FontAwesome.UWP.FontAwesome With {
        '        .Foreground = New SolidColorBrush(Colors.White),
        '        .Icon = FontAwesomeIcon.FileImageOutline
        '    }

        '    botonImagenes.Content = iconoImagenes

        '    spBotonImagen.Children.Add(botonImagenes)

        '    Dim spImagenes As New StackPanel With {
        '        .Orientation = Orientation.Horizontal,
        '        .Visibility = Visibility.Collapsed,
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .MinHeight = 0
        '    }

        '    spBotonImagen.Children.Add(spImagenes)

        '    spBotones2.Children.Add(spBotonImagen)

        '    Dim spBotonVideo As New StackPanel With {
        '        .Orientation = Orientation.Vertical,
        '        .Margin = New Thickness(0, 15, 0, 0),
        '        .Visibility = Visibility.Collapsed
        '    }

        '    Dim botonVideo As New Button With {
        '        .Padding = New Thickness(10, 10, 10, 10),
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .Background = New SolidColorBrush(colorBoton)
        '    }
        '    AddHandler botonVideo.Click, AddressOf BotonVideoClick
        '    AddHandler botonVideo.PointerEntered, AddressOf UsuarioEntraBoton
        '    AddHandler botonVideo.PointerExited, AddressOf UsuarioSaleBoton

        '    Dim iconoVideo As New FontAwesome.UWP.FontAwesome With {
        '        .Foreground = New SolidColorBrush(Colors.White),
        '        .Icon = FontAwesomeIcon.FileMovieOutline
        '    }

        '    botonVideo.Content = iconoVideo

        '    spBotonVideo.Children.Add(botonVideo)

        '    Dim spVideo As New StackPanel With {
        '        .Orientation = Orientation.Vertical,
        '        .Visibility = Visibility.Collapsed,
        '        .VerticalAlignment = VerticalAlignment.Center,
        '        .MinHeight = 0
        '    }

        '    spBotonVideo.Children.Add(spVideo)

        '    spBotones2.Children.Add(spBotonVideo)

        '    gridCentro.Children.Add(spBotones2)

        '    gridTweetEscribir.Children.Add(gridCentro)

        '    '---------------------------------

        '    botonEnviarTweet.Tag = New Objetos.EnviarTweet.Boton(tbMensaje, megaUsuario, tweet, listaMenciones, prEnviando, botonImagenes, botonGif, botonVideo, spImagenes, spGif, spVideo)

        '    botonImagenes.Tag = New Objetos.EnviarTweet.Media(spImagenes, spGif, spVideo, Nothing, colorBoton, botonImagenes, botonGif, botonVideo)
        '    botonGif.Tag = New Objetos.EnviarTweet.Media(spImagenes, spGif, spVideo, Nothing, colorBoton, botonImagenes, botonGif, botonVideo)
        '    botonVideo.Tag = New Objetos.EnviarTweet.Media(spImagenes, spGif, spVideo, Nothing, colorBoton, botonImagenes, botonGif, botonVideo)

        '    Return gridTweetEscribir

        'End Function

        'Private Sub TbTweetEscribirTextChanged(sender As Object, e As TextChangedEventArgs)

        '    Dim tb As TextBox = sender
        '    Dim objeto As Objetos.EnviarTweet.TextoCambia = tb.Tag

        '    Dim boton As Button = objeto.Boton

        '    If tb.Text.Length > 0 Then
        '        boton.IsEnabled = True
        '    Else
        '        boton.IsEnabled = False
        '    End If

        '    Dim contador As TextBlock = objeto.Contador
        '    Dim anillo As RadialProgressBar = objeto.Anillo

        '    anillo.Value = tb.Text.Length

        '    If tb.Text.Length < 141 Then
        '        anillo.Foreground = New SolidColorBrush(App.Current.Resources("ColorPrimario"))
        '        contador.Text = String.Empty
        '    ElseIf tb.Text.Length > 140 And tb.Text.Length < 201 Then
        '        anillo.Foreground = New SolidColorBrush(Colors.Goldenrod)
        '        contador.Text = tb.Text.Length.ToString
        '    ElseIf tb.Text.Length > 200 Then
        '        anillo.Foreground = New SolidColorBrush(Colors.Red)
        '        contador.Text = tb.Text.Length.ToString
        '    End If

        'End Sub

        'Private Async Sub BotonEnviarTweetClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    boton.IsEnabled = False

        '    Dim cosas As Objetos.EnviarTweet.Boton = boton.Tag
        '    cosas.PrEnviando.IsActive = True
        '    cosas.PrEnviando.Visibility = Visibility.Visible

        '    Dim mensaje As String = Nothing

        '    If Not cosas.Tweet Is Nothing Then
        '        mensaje = mensaje + "@" + cosas.Tweet.Usuario.ScreenNombre + " "
        '    End If

        '    If Not cosas.ListaMenciones Is Nothing Then
        '        For Each mencion In cosas.ListaMenciones
        '            If Not mencion.ScreenNombre = cosas.MegaUsuario.Usuario.ScreenNombre Then
        '                mensaje = mensaje + "@" + mencion.ScreenNombre + " "
        '            End If
        '        Next
        '    End If

        '    mensaje = mensaje + cosas.CajaTexto.Text

        '    Dim respuestaID As String = Nothing

        '    If Not cosas.Tweet Is Nothing Then
        '        respuestaID = cosas.Tweet.ID
        '    End If

        '    Dim media As New List(Of IRandomAccessStream)

        '    If cosas.SpImagenes.Children.Count > 0 Then
        '        For Each botonImagen As Button In cosas.SpImagenes.Children
        '            Dim cosas2 As Objetos.EnviarTweet.Media = botonImagen.Tag
        '            Dim stream As FileRandomAccessStream = Await cosas2.Fichero.OpenAsync(FileAccessMode.Read)

        '            media.Add(stream)
        '        Next
        '    End If

        '    If cosas.SpGif.Children.Count > 0 Then
        '        For Each botonGif As Button In cosas.SpGif.Children
        '            Dim cosas2 As Objetos.EnviarTweet.Media = botonGif.Tag
        '            Dim stream As FileRandomAccessStream = Await cosas2.Fichero.OpenAsync(FileAccessMode.Read)

        '            media.Add(stream)
        '        Next
        '    End If

        '    If cosas.SpVideo.Children.Count > 0 Then
        '        For Each botonVideo As Button In cosas.SpVideo.Children
        '            Dim cosas2 As Objetos.EnviarTweet.Media = botonVideo.Tag
        '            Dim stream As FileRandomAccessStream = Await cosas2.Fichero.OpenAsync(FileAccessMode.Read)

        '            media.Add(stream)
        '        Next
        '    End If

        '    Dim estado As Boolean = False

        '    estado = Await TwitterPeticiones.EnviarTweet(estado, cosas.MegaUsuario, mensaje, respuestaID, media)

        '    If estado = True Then
        '        cosas.CajaTexto.Text = String.Empty

        '        cosas.BotonImagenes.IsEnabled = True
        '        cosas.BotonGif.IsEnabled = True
        '        cosas.BotonVideo.IsEnabled = True

        '        cosas.SpImagenes.Children.Clear()
        '        cosas.SpGif.Children.Clear()
        '        cosas.SpVideo.Children.Clear()

        '        Dim recursos As New Resources.ResourceLoader

        '        Notificaciones.Toast(recursos.GetString("TweetSent"))
        '    End If

        '    If cosas.CajaTexto.Text.Length > 0 Then
        '        boton.IsEnabled = True
        '    Else
        '        boton.IsEnabled = False
        '    End If

        '    cosas.PrEnviando.IsActive = False
        '    cosas.PrEnviando.Visibility = Visibility.Collapsed

        'End Sub

        'Private Sub BotonEmojisClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim popup As Popup = boton.Tag
        '    Dim grid As Grid = popup.Child
        '    Dim gv As GridView = grid.Children(0)

        '    If popup.IsOpen = False Then
        '        popup.IsOpen = True

        '        If gv.Items.Count = 0 Then
        '            Dim listaEmojis As New List(Of SingleEmoji)

        '            For Each emoj In Emoji.All
        '                listaEmojis.Add(emoj)
        '            Next

        '            Dim i As Integer = 0
        '            While i < 100
        '                Dim item As New GridViewItem With {
        '                    .Background = New SolidColorBrush(Colors.Transparent),
        '                    .Content = listaEmojis(i).Sequence.AsString,
        '                    .Padding = New Thickness(0, 0, 0, 0)
        '                }

        '                AddHandler item.PointerEntered, AddressOf UsuarioEntraBoton
        '                AddHandler item.PointerExited, AddressOf UsuarioSaleBoton

        '                gv.Items.Add(item)

        '                i += 1
        '            End While
        '        End If
        '    Else
        '        popup.IsOpen = False
        '    End If

        'End Sub

        'Private Sub GvEmojisItemClick(sender As Object, e As ItemClickEventArgs)

        '    Dim gv As GridView = sender
        '    Dim tb As TextBox = gv.Tag

        '    tb.Text = tb.Text + e.ClickedItem

        'End Sub

        'Private Async Sub BotonImagenesClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim cosas As Objetos.EnviarTweet.Media = boton.Tag

        '    Dim picker As New FileOpenPicker With {
        '        .SuggestedStartLocation = PickerLocationId.PicturesLibrary,
        '        .ViewMode = PickerViewMode.Thumbnail
        '    }

        '    picker.FileTypeFilter.Add(".jpg")
        '    picker.FileTypeFilter.Add(".png")

        '    Dim ficheroImagen As StorageFile = Await picker.PickSingleFileAsync

        '    If Not ficheroImagen Is Nothing Then
        '        Dim bitmap As New BitmapImage

        '        Try
        '            Dim stream As FileRandomAccessStream = Await ficheroImagen.OpenAsync(FileAccessMode.Read)
        '            bitmap.SetSource(stream)

        '            Dim botonImagen As New Button With {
        '                .Width = 42,
        '                .Height = 42,
        '                .Padding = New Thickness(0, 0, 0, 0),
        '                .Margin = New Thickness(15, 0, 0, 0),
        '                .BorderThickness = New Thickness(0, 0, 0, 0)
        '            }

        '            cosas.Fichero = ficheroImagen
        '            botonImagen.Tag = cosas
        '            AddHandler botonImagen.Click, AddressOf BotonQuitarImagenClick
        '            AddHandler botonImagen.PointerEntered, AddressOf UsuarioEntraBoton
        '            AddHandler botonImagen.PointerExited, AddressOf UsuarioSaleBoton

        '            Dim imagenBoton As New ImageEx With {
        '                .Source = bitmap
        '            }

        '            botonImagen.Content = imagenBoton

        '            cosas.SpImagenes.Children.Add(botonImagen)

        '            If cosas.SpImagenes.Visibility = Visibility.Collapsed Then
        '                cosas.SpImagenes.Visibility = Visibility.Visible
        '            End If

        '            If cosas.SpImagenes.Children.Count = 4 Then
        '                cosas.BotonImagenes.IsEnabled = False
        '            ElseIf cosas.SpImagenes.Children.Count = 1 Then
        '                cosas.BotonGif.IsEnabled = False
        '                cosas.BotonVideo.IsEnabled = False
        '            End If
        '        Catch ex As Exception

        '        End Try
        '    End If

        'End Sub

        'Private Sub BotonQuitarImagenClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim cosas As Objetos.EnviarTweet.Media = boton.Tag
        '    cosas.SpImagenes.Children.Remove(boton)

        '    If cosas.SpImagenes.Children.Count < 4 Then
        '        cosas.BotonImagenes.IsEnabled = True
        '    Else
        '        cosas.BotonImagenes.IsEnabled = False
        '    End If

        '    If cosas.SpImagenes.Children.Count = 0 Then
        '        cosas.BotonGif.IsEnabled = True
        '        cosas.BotonVideo.IsEnabled = True
        '    End If

        'End Sub

        'Private Async Sub BotonGifClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim cosas As Objetos.EnviarTweet.Media = boton.Tag

        '    Dim picker As New FileOpenPicker With {
        '        .SuggestedStartLocation = PickerLocationId.PicturesLibrary,
        '        .ViewMode = PickerViewMode.Thumbnail
        '    }

        '    picker.FileTypeFilter.Add(".gif")

        '    Dim ficheroGif As StorageFile = Await picker.PickSingleFileAsync

        '    If Not ficheroGif Is Nothing Then
        '        Dim botonGif As New Button With {
        '            .Width = 42,
        '            .Height = 42,
        '            .Padding = New Thickness(0, 0, 0, 0),
        '            .Margin = New Thickness(0, 15, 0, 0),
        '            .BorderThickness = New Thickness(0, 0, 0, 0),
        '            .Background = New SolidColorBrush(cosas.Color)
        '        }

        '        Dim tbGif As New TextBlock With {
        '            .Foreground = New SolidColorBrush(Colors.White),
        '            .Text = "X"
        '        }

        '        botonGif.Content = tbGif

        '        cosas.Fichero = ficheroGif
        '        botonGif.Tag = cosas
        '        AddHandler botonGif.Click, AddressOf BotonQuitarGifClick
        '        AddHandler botonGif.PointerEntered, AddressOf UsuarioEntraBoton
        '        AddHandler botonGif.PointerExited, AddressOf UsuarioSaleBoton

        '        cosas.SpGif.Children.Add(botonGif)

        '        If cosas.SpGif.Visibility = Visibility.Collapsed Then
        '            cosas.SpGif.Visibility = Visibility.Visible
        '        End If

        '        If cosas.SpGif.Children.Count = 1 Then
        '            cosas.BotonGif.IsEnabled = False
        '            cosas.BotonImagenes.IsEnabled = False
        '            cosas.BotonVideo.IsEnabled = False
        '        End If
        '    End If

        'End Sub

        'Private Sub BotonQuitarGifClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim cosas As Objetos.EnviarTweet.Media = boton.Tag
        '    cosas.SpGif.Children.Remove(boton)

        '    If cosas.SpGif.Children.Count = 0 Then
        '        cosas.BotonImagenes.IsEnabled = True
        '        cosas.BotonGif.IsEnabled = True
        '        cosas.BotonVideo.IsEnabled = True
        '    End If

        'End Sub

        'Private Async Sub BotonVideoClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim cosas As Objetos.EnviarTweet.Media = boton.Tag

        '    Dim picker As New FileOpenPicker With {
        '        .SuggestedStartLocation = PickerLocationId.PicturesLibrary,
        '        .ViewMode = PickerViewMode.Thumbnail
        '    }

        '    picker.FileTypeFilter.Add(".mp4")

        '    Dim ficheroVideo As StorageFile = Await picker.PickSingleFileAsync

        '    If Not ficheroVideo Is Nothing Then
        '        Dim botonVideo As New Button With {
        '            .Width = 42,
        '            .Height = 42,
        '            .Padding = New Thickness(0, 0, 0, 0),
        '            .Margin = New Thickness(0, 15, 0, 0),
        '            .BorderThickness = New Thickness(0, 0, 0, 0),
        '            .Background = New SolidColorBrush(cosas.Color)
        '        }

        '        Dim tbVideo As New TextBlock With {
        '            .Foreground = New SolidColorBrush(Colors.White),
        '            .Text = "X"
        '        }

        '        botonVideo.Content = tbVideo

        '        cosas.Fichero = ficheroVideo
        '        botonVideo.Tag = cosas
        '        AddHandler botonVideo.Click, AddressOf BotonQuitarVideoClick
        '        AddHandler botonVideo.PointerEntered, AddressOf UsuarioEntraBoton
        '        AddHandler botonVideo.PointerExited, AddressOf UsuarioSaleBoton

        '        cosas.SpVideo.Children.Add(botonVideo)

        '        If cosas.SpVideo.Visibility = Visibility.Collapsed Then
        '            cosas.SpVideo.Visibility = Visibility.Visible
        '        End If

        '        If cosas.SpVideo.Children.Count = 1 Then
        '            cosas.BotonGif.IsEnabled = False
        '            cosas.BotonImagenes.IsEnabled = False
        '            cosas.BotonVideo.IsEnabled = False
        '        End If
        '    End If

        'End Sub

        'Private Sub BotonQuitarVideoClick(sender As Object, e As RoutedEventArgs)

        '    Dim boton As Button = sender
        '    Dim cosas As Objetos.EnviarTweet.Media = boton.Tag
        '    cosas.SpVideo.Children.Remove(boton)

        '    If cosas.SpVideo.Children.Count = 0 Then
        '        cosas.BotonImagenes.IsEnabled = True
        '        cosas.BotonGif.IsEnabled = True
        '        cosas.BotonVideo.IsEnabled = True
        '    End If

        'End Sub

        'Private Sub EnlaceClick(sender As Object, e As HyperlinkClickEventArgs)

        '    Dim enlace As Hyperlink = sender
        '    Dim contenido As Run = enlace.Inlines(0)
        '    Dim usuario As String = contenido.Text

        '    cosas.ScreenNombre = usuario.Replace("@", Nothing)

        '    FichaUsuarioXaml.Generar(cosas, enlace)

        'End Sub

        'Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        '    Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        'End Sub

        'Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        '    Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        'End Sub

    End Module
End Namespace

