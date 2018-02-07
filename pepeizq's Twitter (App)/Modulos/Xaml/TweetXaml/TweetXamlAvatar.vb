Imports pepeizq.Twitter.Tweet
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Shapes

Namespace pepeTwitterXaml
    Module TweetXamlAvatar

        Public Function Generar(tweet As Tweet, megaUsuario As pepeizq.Twitter.MegaUsuario)

            Dim botonAvatar As New Button With {
                .Background = New SolidColorBrush(Colors.Transparent),
                .BorderBrush = New SolidColorBrush(Colors.Transparent),
                .BorderThickness = New Thickness(0, 0, 0, 0),
                .VerticalAlignment = VerticalAlignment.Top,
                .Margin = New Thickness(15, 0, 10, 0),
                .Padding = New Thickness(0, 0, 0, 0),
                .Height = 48,
                .Width = 48
            }

            Dim imagenAvatar As New ImageBrush With {
                .Stretch = Stretch.Uniform
            }

            If tweet.Retweet Is Nothing Then
                Try
                    imagenAvatar.ImageSource = New BitmapImage(New Uri(tweet.Usuario.ImagenAvatar))
                Catch ex As Exception

                End Try
            Else
                Try
                    imagenAvatar.ImageSource = New BitmapImage(New Uri(tweet.Retweet.Usuario.ImagenAvatar))
                Catch ex As Exception

                End Try
            End If

            Dim circulo As New Ellipse With {
                .Fill = imagenAvatar,
                .Height = 48,
                .Width = 48
            }

            botonAvatar.Tag = New pepeizq.Twitter.Objetos.UsuarioAmpliado(megaUsuario, tweet.Usuario)
            botonAvatar.Content = circulo

            AddHandler botonAvatar.Click, AddressOf UsuarioPulsaBoton
            AddHandler botonAvatar.PointerEntered, AddressOf UsuarioEntraBoton
            AddHandler botonAvatar.PointerExited, AddressOf UsuarioSaleBoton

            Return botonAvatar

        End Function

        Private Sub UsuarioPulsaBoton(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            Dim cosas As pepeizq.Twitter.Objetos.UsuarioAmpliado = boton.Tag

            FichaUsuarioXaml.Generar(cosas)

        End Sub

        Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

        End Sub

        Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

            Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

        End Sub

    End Module
End Namespace

