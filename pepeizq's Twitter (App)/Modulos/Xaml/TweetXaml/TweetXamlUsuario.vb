Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Documents

Namespace pepeTwitterXaml
    Module TweetXamlUsuario

        Public Function Generar(tweet As Tweet)

            Dim sp As New StackPanel With {
                .Orientation = Orientation.Horizontal
            }

            Dim botonUsuario As New Button With {
                .Padding = New Thickness(0, 0, 0, 0),
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0)
            }

            Dim spUsuario As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Padding = New Thickness(5, 5, 5, 5),
                .CornerRadius = New CornerRadius(5),
                .Background = New SolidColorBrush(App.Current.Resources("ColorSecundario"))
            }

            Dim tb1 As New TextBlock With {
                .FontSize = 14,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            Dim tb2 As New TextBlock With {
                .FontSize = 12,
                .Margin = New Thickness(5, 0, 0, 0),
                .VerticalAlignment = VerticalAlignment.Center,
                .Foreground = New SolidColorBrush(Colors.White)
            }

            If tweet.Retweet Is Nothing Then
                tb1.Text = tweet.Usuario.Nombre
                tb2.Text = "@" + tweet.Usuario.ScreenNombre
                botonUsuario.Tag = tweet.Usuario
            Else
                tb1.Text = tweet.Retweet.Usuario.Nombre
                tb2.Text = "@" + tweet.Retweet.Usuario.ScreenNombre
                botonUsuario.Tag = tweet.Retweet.Usuario
            End If

            spUsuario.Children.Add(tb1)
            spUsuario.Children.Add(tb2)

            botonUsuario.Content = spUsuario

            AddHandler botonUsuario.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler botonUsuario.PointerExited, AddressOf UsuarioSaleBoton

            sp.Children.Add(botonUsuario)

            Dim respuestaUsuarioScreenNombre As String = Nothing

            If tweet.Retweet Is Nothing Then
                If Not tweet.RespuestaUsuarioScreenNombre = Nothing Then
                    respuestaUsuarioScreenNombre = tweet.RespuestaUsuarioScreenNombre
                End If
            Else
                If Not tweet.Retweet.RespuestaUsuarioScreenNombre = Nothing Then
                    respuestaUsuarioScreenNombre = tweet.Retweet.RespuestaUsuarioScreenNombre
                End If
            End If

            If Not respuestaUsuarioScreenNombre = Nothing Then
                Dim recursos As New Resources.ResourceLoader

                Dim textoSpanRespuesta As New Span

                Dim fragmento As New Run With {
                    .Text = recursos.GetString("ReplyingTo") + " "
                }

                textoSpanRespuesta.Inlines.Add(fragmento)

                Dim contenidoEnlace As New Run With {
                    .Text = "@" + respuestaUsuarioScreenNombre
                }

                Dim enlace As New Hyperlink With {
                    .NavigateUri = New Uri("https://twitter.com/" + respuestaUsuarioScreenNombre),
                    .TextDecorations = Nothing,
                    .Foreground = New SolidColorBrush(App.Current.Resources("ColorCuarto"))
                }

                enlace.Inlines.Add(contenidoEnlace)
                textoSpanRespuesta.Inlines.Add(enlace)

                Dim tbRespuesta As New TextBlock With {
                    .TextWrapping = TextWrapping.Wrap,
                    .Margin = New Thickness(10, 5, 5, 5),
                    .FontSize = 13,
                    .VerticalAlignment = VerticalAlignment.Center
                }

                tbRespuesta.Inlines.Add(textoSpanRespuesta)
                sp.Children.Add(tbRespuesta)
            End If

            Return sp

        End Function

        Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace
