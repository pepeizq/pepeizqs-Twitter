Imports pepeizq.Twitter
Imports Windows.UI

Module EscribirXaml

    'Public Function Generar(megaUsuario As pepeizq.Twitter.MegaUsuario, visibilidad As Visibility)

    '    Dim recursos As New Resources.ResourceLoader

    '    Dim usuario As TwitterUsuario = megaUsuario.Usuario

    '    Dim gridFondo As New Grid
    '    gridFondo.SetValue(Grid.RowProperty, 1)
    '    gridFondo.Name = "gridEscribir" + usuario.ID
    '    gridFondo.Visibility = visibilidad
    '    gridFondo.Padding = New Thickness(10, 10, 10, 10)

    '    Dim transpariencia As New UISettings

    '    If transpariencia.AdvancedEffectsEnabled = True Then
    '        gridFondo.Background = App.Current.Resources("GridAcrilico")
    '    Else
    '        gridFondo.Background = New SolidColorBrush(Colors.LightGray)
    '    End If

    '    Dim spEscribir As New StackPanel With {
    '        .Orientation = Orientation.Vertical,
    '        .HorizontalAlignment = HorizontalAlignment.Center,
    '        .VerticalAlignment = VerticalAlignment.Center
    '    }

    '    Dim tbFondo As New Border With {
    '        .Background = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
    '        .VerticalAlignment = VerticalAlignment.Top
    '    }

    '    Dim tbEscribir As New TextBlock With {
    '        .Text = recursos.GetString("WriteTweet"),
    '        .Padding = New Thickness(20, 10, 15, 10),
    '        .Foreground = New SolidColorBrush(Colors.White)
    '    }

    '    tbFondo.Child = tbEscribir
    '    spEscribir.Children.Add(tbFondo)

    '    Dim gridEscribir As New Grid With {
    '        .Padding = New Thickness(15, 0, 20, 10),
    '        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorCuarto")),
    '        .BorderThickness = New Thickness(1, 1, 1, 1)
    '    }

    '    Dim color1 As New GradientStop With {
    '        .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#e0e0e0"),
    '        .Offset = 0.5
    '    }

    '    Dim color2 As New GradientStop With {
    '        .Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#d6d6d6"),
    '        .Offset = 1.0
    '    }

    '    Dim coleccion As New GradientStopCollection From {
    '        color1,
    '        color2
    '    }

    '    Dim brush As New LinearGradientBrush With {
    '        .StartPoint = New Point(0.5, 0),
    '        .EndPoint = New Point(0.5, 1),
    '        .GradientStops = coleccion
    '    }

    '    gridEscribir.Background = brush

    '    gridEscribir.Children.Add(pepeizq.Twitter.Xaml.TweetEnviarTweet.Generar(Nothing, megaUsuario, Visibility.Visible, Nothing))

    '    spEscribir.Children.Add(gridEscribir)

    '    gridFondo.Children.Add(spEscribir)

    '    Return gridFondo

    'End Function

End Module
