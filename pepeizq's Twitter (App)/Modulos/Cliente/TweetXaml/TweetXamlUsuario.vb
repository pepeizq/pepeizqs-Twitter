Imports pepeizq.Twitter
Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Documents

Namespace pepeizq.Twitter.Xaml
    Module TweetUsuario

        Dim cosas As Objetos.UsuarioAmpliado = Nothing

        Public Function Generar(tweet As Tweet, megaUsuario As MegaUsuario, color As Color)

            If color = Nothing Then
                color = App.Current.Resources("ColorSecundario")
            End If

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
                .Background = New SolidColorBrush(color),
                .Margin = New Thickness(1, 1, 1, 1)
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
                botonUsuario.Tag = New Objetos.UsuarioAmpliado(megaUsuario, tweet.Usuario, Nothing)
            Else
                tb1.Text = tweet.Retweet.Usuario.Nombre
                tb2.Text = "@" + tweet.Retweet.Usuario.ScreenNombre
                botonUsuario.Tag = New Objetos.UsuarioAmpliado(megaUsuario, tweet.Retweet.Usuario, Nothing)
            End If

            spUsuario.Children.Add(tb1)
            spUsuario.Children.Add(tb2)

            botonUsuario.Content = spUsuario

            AddHandler botonUsuario.Click, AddressOf UsuarioPulsaBoton
            AddHandler botonUsuario.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler botonUsuario.PointerExited, AddressOf UsuarioSaleBoton

            sp.Children.Add(botonUsuario)

            '-------------------------------------

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
                cosas = New Objetos.UsuarioAmpliado(megaUsuario, Nothing, Nothing)

                Dim recursos As New Resources.ResourceLoader

                Dim textoSpanRespuesta As New Span

                Dim fragmento As New Run With {
                    .Text = recursos.GetString("ReplyingTo") + " ",
                    .Foreground = New SolidColorBrush(Colors.Black)
                }

                textoSpanRespuesta.Inlines.Add(fragmento)

                Dim contenidoEnlace As New Run With {
                    .Text = "@" + respuestaUsuarioScreenNombre
                }

                Dim colorRespuesta As New Color

                If color = App.Current.Resources("ColorSecundario") Then
                    colorRespuesta = App.Current.Resources("ColorCuarto")
                Else
                    colorRespuesta = color
                End If

                Dim enlace As New Hyperlink With {
                    .TextDecorations = Nothing,
                    .Foreground = New SolidColorBrush(colorRespuesta)
                }

                AddHandler enlace.Click, AddressOf EnlaceClick

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

        Private Sub UsuarioPulsaBoton(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As Objetos.UsuarioAmpliado = boton.Tag

            FichaUsuarioXaml.Generar(cosas, boton)

        End Sub

        Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

        Private Sub EnlaceClick(sender As Object, e As HyperlinkClickEventArgs)

            Dim enlace As Hyperlink = sender
            Dim contenido As Run = enlace.Inlines(0)
            Dim usuario As String = contenido.Text

            cosas.ScreenNombre = usuario.Replace("@", Nothing)

            FichaUsuarioXaml.Generar(cosas, enlace)

        End Sub

    End Module
End Namespace
