Imports pepeizq.Twitter.Tweet
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Namespace pepeizq.Twitter.Xaml
    Module BusquedaTweets

        Public Sub Generar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim grid As Grid = pagina.FindName("gridBusquedaTweets")
            Dim megaUsuario As MegaUsuario = grid.Tag

            Dim botonBuscar As Button = pagina.FindName("botonBuscarTweets")
            botonBuscar.Tag = megaUsuario

            AddHandler botonBuscar.Click, AddressOf BotonBuscarTweetsClick
            AddHandler botonBuscar.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler botonBuscar.PointerExited, AddressOf UsuarioSaleBoton

            Dim tbBuscar As TextBox = pagina.FindName("tbBuscarTweets")
            tbBuscar.Tag = botonBuscar

            AddHandler tbBuscar.TextChanged, AddressOf TbBuscarTextoCambia

            Dim pr As ProgressRing = pagina.FindName("prBusquedaTweets2")

            Dim sv As ScrollViewer = pagina.FindName("svBusquedaTweets")
            sv.Tag = New Objetos.ScrollViewerTweets(megaUsuario, pr, Nothing, 3, Nothing, Nothing)
            AddHandler sv.ViewChanging, AddressOf SvTweets_ViewChanging

            Dim botonSubir As Button = pagina.FindName("botonSubirArribaBusquedaTweets")
            botonSubir.Tag = sv
            AddHandler botonSubir.Click, AddressOf BotonSubirClick

        End Sub

        Public Async Sub BotonBuscarTweetsClick(sender As Object, e As RoutedEventArgs)

            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (Async Sub()
                                                                                                              Dim recursos As New Resources.ResourceLoader

                                                                                                              Dim frame As Frame = Window.Current.Content
                                                                                                              Dim pagina As Page = frame.Content

                                                                                                              Dim boton As Button = pagina.FindName("botonBuscarTweets")
                                                                                                              boton.IsEnabled = False

                                                                                                              Dim gridMensaje As Grid = pagina.FindName("gridBusquedaTweetsMensaje")
                                                                                                              gridMensaje.Visibility = Visibility.Collapsed

                                                                                                              Dim lv As ListView = pagina.FindName("lvResultadosBusquedaTweets")
                                                                                                              lv.Items.Clear()

                                                                                                              AddHandler lv.ItemClick, AddressOf LvResultadosItemClick

                                                                                                              Dim pr As ProgressRing = pagina.FindName("prBusquedaTweets")
                                                                                                              pr.Visibility = Visibility.Visible

                                                                                                              Dim megaUsuario As MegaUsuario = boton.Tag

                                                                                                              Dim tb As TextBox = pagina.FindName("tbBuscarTweets")

                                                                                                              Dim listaTweets As New List(Of Tweet)

                                                                                                              listaTweets = Await TwitterPeticiones.BuscarHashtagTweets(listaTweets, megaUsuario, tb.Text)

                                                                                                              pr.Visibility = Visibility.Collapsed

                                                                                                              Dim gridNoResultados As Grid = pagina.FindName("gridBusquedaTweetsNo")

                                                                                                              If listaTweets.Count > 0 Then
                                                                                                                  gridNoResultados.Visibility = Visibility.Collapsed

                                                                                                                  For Each tweet In listaTweets
                                                                                                                      lv.Items.Add(TweetXaml.Añadir(tweet, megaUsuario, Nothing))
                                                                                                                  Next
                                                                                                              Else
                                                                                                                  gridNoResultados.Visibility = Visibility.Visible
                                                                                                              End If

                                                                                                              boton.IsEnabled = True
                                                                                                          End Sub))

        End Sub

        Private Sub TbBuscarTextoCambia(sender As Object, e As TextChangedEventArgs)

            Dim tb As TextBox = sender
            Dim boton As Button = tb.Tag

            If tb.Text.Trim.Length > 0 Then
                boton.IsEnabled = True
            Else
                boton.IsEnabled = False
            End If

        End Sub

        Public Sub LvResultadosItemClick(sender As Object, e As ItemClickEventArgs)

            Dim grid As Grid = e.ClickedItem

            FichaTweet.Generar(grid.Tag, grid)

        End Sub

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
End Namespace

