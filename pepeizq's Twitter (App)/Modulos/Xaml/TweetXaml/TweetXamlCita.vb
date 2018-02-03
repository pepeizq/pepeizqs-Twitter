Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter.Tweet

Namespace pepeTwitterXaml
    Module TweetXamlCita

        Public Function Generar(tweet As Tweet, megaUsuario As pepeizq.Twitter.MegaUsuario)

            Dim color1 As New GradientStop With {
                .Color = ColorHelper.ToColor("#e0e0e0"),
                .Offset = 0.5
            }

            Dim color2 As New GradientStop With {
                .Color = ColorHelper.ToColor("#d6d6d6"),
                .Offset = 1.0
            }

            Dim coleccion As New GradientStopCollection From {
                color1,
                color2
            }

            Dim brush As New LinearGradientBrush With {
                .StartPoint = New Point(0.5, 0),
                .EndPoint = New Point(0.5, 1),
                .GradientStops = coleccion
            }

            Dim sp As New StackPanel With {
                .Background = brush,
                .Margin = New Thickness(5, 5, 5, 5),
                .Padding = New Thickness(10, 10, 10, 10),
                .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                .BorderThickness = New Thickness(1, 1, 1, 1),
                .CornerRadius = New CornerRadius(5)
            }

            sp.Children.Add(TweetXamlUsuario.Generar(tweet.Cita, megaUsuario))
            sp.Children.Add(TweetXamlTexto.Generar(tweet.Cita, tweet))
            sp.Children.Add(TweetXamlMedia.Generar(tweet.Cita))

            Return sp

        End Function

    End Module
End Namespace
