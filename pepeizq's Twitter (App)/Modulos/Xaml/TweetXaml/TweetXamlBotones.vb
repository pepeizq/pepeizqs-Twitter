Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI

Namespace pepeTwitterXaml
    Module TweetXamlBotones

        Public Function Generar(tweet As Tweet, gridTweet As Grid, megaUsuario As pepeTwitter.MegaUsuario)

            Dim recursos As New Resources.ResourceLoader

            Dim spBotones As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(5, 5, 0, 0),
                .Height = 35
            }

            Dim botonResponder As New Button With {
                .Content = ConstructorBotones(59217, recursos.GetString("ReplyTweet")),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = gridTweet,
                .Visibility = Visibility.Collapsed,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler botonResponder.Click, AddressOf BotonResponderClick
            AddHandler botonResponder.PointerEntered, AddressOf BotonResponderUsuarioEntra
            AddHandler botonResponder.PointerExited, AddressOf BotonResponderUsuarioSale

            spBotones.Children.Add(botonResponder)

            Dim botonRetweet As New Button With {
                .Content = ConstructorBotones(59627, recursos.GetString("Retweet")),
                .Padding = New Thickness(5, 5, 5, 5),
                .Margin = New Thickness(15, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .Tag = New pepeTwitter.Objetos.RetweetBoton(tweet, megaUsuario),
                .Visibility = Visibility.Collapsed,
                .Style = App.Current.Resources("ButtonRevealStyle")
            }

            AddHandler botonRetweet.Click, AddressOf BotonRetweetClick

            spBotones.Children.Add(botonRetweet)

            Return spBotones
        End Function

        Private Sub BotonResponderClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim grid As Grid = boton.Tag
            Dim grid2 As Grid = grid.Children(1)
            Dim sp As StackPanel = grid2.Children(1)

            Dim gridResponder As Grid = sp.Children(sp.Children.Count - 1)

            If gridResponder.Visibility = Visibility.Visible Then
                gridResponder.Visibility = Visibility.Collapsed
            Else
                gridResponder.Visibility = Visibility.Visible
            End If

        End Sub

        Private Async Sub BotonRetweetClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeTwitter.Objetos.RetweetBoton = boton.Tag

            Dim status As New TwitterStatus With {
               .TweetID = cosas.Tweet.ID
            }

            Await cosas.MegaUsuario.Servicio.reTweetStatusAsync(status)

            Dim recursos As New Resources.ResourceLoader

            Notificaciones.Toast.Enseñar(recursos.GetString("RetweetSent"))

        End Sub

        Private Function ConstructorBotones(icono As Integer, texto As String)

            Dim tbSimbolo As New TextBlock With {
                .Text = Char.ConvertFromUtf32(icono),
                .FontFamily = New FontFamily("Segoe MDL2 Assets"),
                .FontSize = 20,
                .VerticalAlignment = VerticalAlignment.Center
            }

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim tbToolTip As TextBlock = New TextBlock With {
                .Text = texto,
                .FontSize = 15
            }

            ToolTipService.SetToolTip(sp, tbToolTip)
            ToolTipService.SetPlacement(sp, PlacementMode.Mouse)

            sp.Children.Add(tbSimbolo)

            Return sp

        End Function

        Private Sub BotonResponderUsuarioEntra(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim tb As TextBlock = sp.Children(0)
            tb.Foreground = New SolidColorBrush(App.Current.Resources("ColorCuarto"))

        End Sub

        Private Sub BotonResponderUsuarioSale(sender As Object, e As PointerRoutedEventArgs)

            Dim boton As Button = sender
            Dim sp As StackPanel = boton.Content

            Dim tb As TextBlock = sp.Children(0)
            tb.Foreground = New SolidColorBrush(Colors.Black)

        End Sub

    End Module
End Namespace

