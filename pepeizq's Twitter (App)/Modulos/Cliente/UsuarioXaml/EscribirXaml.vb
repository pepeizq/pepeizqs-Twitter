Imports pepeizq.Twitter

Module EscribirXaml

    Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

        Dim usuario As TwitterUsuario = megaUsuario.Usuario

        Dim gridEscribir As New Grid
        gridEscribir.SetValue(Grid.RowProperty, 1)
        gridEscribir.Name = "gridEscribir" + usuario.ScreenNombre
        gridEscribir.Padding = New Thickness(15, 0, 0, 0)
        gridEscribir.Visibility = visibilidad

        Dim color1 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#e0e0e0"),
            .Offset = 0.5
        }

        Dim color2 As New GradientStop With {
            .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#d6d6d6"),
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

        gridEscribir.Background = brush

        gridEscribir.Children.Add(pepeizq.Twitter.Xaml.TweetEnviarTweet.Generar(Nothing, megaUsuario, Visibility.Visible, Nothing))

        Return gridEscribir

    End Function

End Module
