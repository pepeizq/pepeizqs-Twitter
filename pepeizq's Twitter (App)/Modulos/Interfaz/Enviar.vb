Imports Tweetinvi.Models

Namespace Interfaz
    Module Enviar

        Public Sub Responder(sender As Object, e As RoutedEventArgs)

            Dim botonRespuesta As Button = sender
            Dim tweet As ITweet = botonRespuesta.Tag

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim boton As Button = pagina.FindName("botonEscribir")
            Dim grid As Grid = pagina.FindName("gridEscribir")

            Pestañas.Visibilidad_Pestañas_Usuario(boton, grid)

            Dim spTweet As StackPanel = pagina.FindName("spEscribirTweetOrigen")
            spTweet.Children.Clear()

            spTweet.Children.Add(GenerarTweet(Interfaz.Usuario.cliente_, tweet, False))
            spTweet.Visibility = Visibility.Visible

        End Sub

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbEscribirMensaje As TextBox = pagina.FindName("tbEscribirMensaje")

            Dim botonEnviarMensaje As Button = pagina.FindName("botonEnviarMensaje")
            botonEnviarMensaje.IsEnabled = False



        End Sub

    End Module
End Namespace