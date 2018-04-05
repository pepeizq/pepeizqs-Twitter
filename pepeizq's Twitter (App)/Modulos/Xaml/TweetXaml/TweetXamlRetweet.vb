Imports FontAwesome.UWP
Imports pepeizq.Twitter.Tweet
Imports Windows.UI

Namespace pepeTwitterXaml
    Module TweetXamlRetweet

        Public Function Generar(tweet As Tweet)

            Dim recursos As New Resources.ResourceLoader

            Dim spRetweet As New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .Margin = New Thickness(75, 0, 0, 10)
            }

            Dim iconoRetweet As New FontAwesome.UWP.FontAwesome With {
                .Icon = FontAwesomeIcon.Retweet,
                .Foreground = New SolidColorBrush(Colors.Green)
            }

            spRetweet.Children.Add(iconoRetweet)

            Dim usuarioRetweet As New TextBlock With {
                .Text = tweet.Usuario.Nombre + " " + recursos.GetString("Retweeted"),
                .Margin = New Thickness(5, 0, 0, 0),
                .FontSize = 13,
                .VerticalAlignment = VerticalAlignment.Center
            }

            spRetweet.Children.Add(usuarioRetweet)

            Return spRetweet

        End Function

    End Module
End Namespace