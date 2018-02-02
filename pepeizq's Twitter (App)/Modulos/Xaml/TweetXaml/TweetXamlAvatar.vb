Imports pepeizq.Twitter.Tweet
Imports Windows.UI.Xaml.Shapes

Namespace pepeTwitterXaml
    Module TweetXamlAvatar

        Public Function Generar(tweet As Tweet)

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
                .VerticalAlignment = VerticalAlignment.Top,
                .Margin = New Thickness(15, 0, 10, 0),
                .Height = 50,
                .Width = 50
            }

            Return circulo

        End Function

    End Module
End Namespace

