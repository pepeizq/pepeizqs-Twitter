Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Tweetinvi.Models
Imports Tweetinvi.Parameters
Imports Windows.Storage
Imports Windows.Storage.AccessCache
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams
Imports Windows.UI

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

            Dim gridBotones As Grid = pagina.FindName("gridUsuarioBotones")
            gridBotones.Visibility = Visibility.Visible

            Dim spTweet As StackPanel = pagina.FindName("spEscribirTweetOrigen")
            spTweet.Children.Clear()

            spTweet.Children.Add(GenerarTweet(Interfaz.Usuario.cliente_, tweet, False))
            spTweet.Visibility = Visibility.Visible

            Dim tbEscribirMensaje As TextBox = pagina.FindName("tbEscribirMensaje")
            tbEscribirMensaje.Text = String.Empty

            Dim spImagenes As StackPanel = pagina.FindName("spEnviarMensajeImagenes")
            spImagenes.Children.Clear()

        End Sub

        Public Sub Cargar()

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbCaracteres As TextBlock = pagina.FindName("tbEnviarMensajeCaracteres")
            tbCaracteres.Text = "0"

            Dim tbEscribirMensaje As TextBox = pagina.FindName("tbEscribirMensaje")

            RemoveHandler tbEscribirMensaje.TextChanged, AddressOf HabilitarEnvio
            AddHandler tbEscribirMensaje.TextChanged, AddressOf HabilitarEnvio

            Dim botonEnviarMensaje As Button = pagina.FindName("botonEnviarMensaje")
            botonEnviarMensaje.IsEnabled = False

            RemoveHandler botonEnviarMensaje.Click, AddressOf EnviarMensaje
            AddHandler botonEnviarMensaje.Click, AddressOf EnviarMensaje

            RemoveHandler botonEnviarMensaje.PointerEntered, AddressOf Entra_Boton_Texto
            AddHandler botonEnviarMensaje.PointerEntered, AddressOf Entra_Boton_Texto

            RemoveHandler botonEnviarMensaje.PointerExited, AddressOf Sale_Boton_Texto
            AddHandler botonEnviarMensaje.PointerExited, AddressOf Sale_Boton_Texto

            Dim botonAñadirImagenes As Button = pagina.FindName("botonEnviarMensajeAñadirImagen")

            RemoveHandler botonAñadirImagenes.Click, AddressOf AñadirImagen
            AddHandler botonAñadirImagenes.Click, AddressOf AñadirImagen

            RemoveHandler botonAñadirImagenes.PointerEntered, AddressOf Entra_Boton_Icono
            AddHandler botonAñadirImagenes.PointerEntered, AddressOf Entra_Boton_Icono

            RemoveHandler botonAñadirImagenes.PointerExited, AddressOf Sale_Boton_Icono
            AddHandler botonAñadirImagenes.PointerExited, AddressOf Sale_Boton_Icono

        End Sub

        Private Async Sub EnviarMensaje(sender As Object, e As RoutedEventArgs)

            Dim botonEnviar As Button = sender
            botonEnviar.IsEnabled = False

            Dim parametros As New PublishTweetParameters

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pr As ProgressRing = pagina.FindName("prEnviarMensaje")
            pr.Visibility = Visibility.Visible

            Dim spTweetOrigen As StackPanel = pagina.FindName("spEscribirTweetOrigen")

            If spTweetOrigen.Children.Count > 0 Then
                Dim boton As Button = spTweetOrigen.Children(0)
                Dim tweetID As Long = boton.Tag

                parametros.InReplyToTweetId = tweetID
            End If

            Dim spImagenes As StackPanel = pagina.FindName("spEnviarMensajeImagenes")

            If spImagenes.Children.Count > 0 Then
                Dim listaMedia As New List(Of IMedia)

                For Each imagen In spImagenes.Children
                    Dim boton As Button = imagen
                    boton.IsEnabled = False
                Next

                For Each imagen In spImagenes.Children
                    Dim boton As Button = imagen
                    Dim ficheroImagen As StorageFile = Await StorageFile.GetFileFromPathAsync(boton.Tag)
                    Dim binario As Byte()

                    Using st As Stream = Await ficheroImagen.OpenStreamForReadAsync
                        Using memoria As New MemoryStream
                            st.CopyTo(memoria)
                            binario = memoria.ToArray
                        End Using
                    End Using

                    Dim subirImagen As IMedia = Await cliente_.Upload.UploadTweetImageAsync(binario)
                    listaMedia.Add(subirImagen)
                Next

                parametros.Medias = listaMedia
            End If

            Dim tbEscribirMensaje As TextBox = pagina.FindName("tbEscribirMensaje")

            If tbEscribirMensaje.Text.Trim.Length > 0 Then
                parametros.Text = tbEscribirMensaje.Text.Trim
            End If

            Await cliente_.Tweets.PublishTweetAsync(parametros)

            If spImagenes.Children.Count > 0 Then
                For Each imagen In spImagenes.Children
                    Dim boton As Button = imagen
                    boton.IsEnabled = True
                Next
            End If

            pr.Visibility = Visibility.Collapsed
            botonEnviar.IsEnabled = True

        End Sub

        Private Sub HabilitarEnvio(sender As Object, e As TextChangedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbEscribirMensaje As TextBox = sender

            Dim botonEnviarMensaje As Button = pagina.FindName("botonEnviarMensaje")

            If tbEscribirMensaje.Text.Trim.Length > 0 And tbEscribirMensaje.Text.Trim.Length <= 280 Then
                botonEnviarMensaje.IsEnabled = True
            Else
                botonEnviarMensaje.IsEnabled = False
            End If

            Dim tbCaracteres As TextBlock = pagina.FindName("tbEnviarMensajeCaracteres")
            tbCaracteres.Text = tbEscribirMensaje.Text.Trim.Length

            Dim pbCaracteres As ProgressBar = pagina.FindName("pbEnviarMensajeCaracteres")
            pbCaracteres.Value = tbEscribirMensaje.Text.Trim.Length

            If pbCaracteres.Value < 180 Then
                pbCaracteres.Foreground = New SolidColorBrush(Colors.LightGreen)
            ElseIf pbCaracteres.Value > 179 And pbCaracteres.Value < 230 Then
                pbCaracteres.Foreground = New SolidColorBrush(Colors.Yellow)
            Else
                pbCaracteres.Foreground = New SolidColorBrush(Colors.IndianRed)
            End If

        End Sub

        Private Async Sub AñadirImagen(sender As Object, e As RoutedEventArgs)

            Dim picker As New FileOpenPicker
            picker.FileTypeFilter.Add(".png")
            picker.FileTypeFilter.Add(".jpg")
            picker.FileTypeFilter.Add(".jpeg")
            picker.ViewMode = PickerViewMode.List

            Dim fichero As StorageFile = Await picker.PickSingleFileAsync

            If Not fichero Is Nothing Then
                Dim frame As Frame = Window.Current.Content
                Dim pagina As Page = frame.Content

                Dim botonAñadirImagenes As Button = pagina.FindName("botonEnviarMensajeAñadirImagen")
                Dim spImagenes As StackPanel = pagina.FindName("spEnviarMensajeImagenes")

                Try
                    Dim bitmap As New BitmapImage
                    Dim stream As FileRandomAccessStream = Await fichero.OpenAsync(FileAccessMode.Read)
                    bitmap.SetSource(stream)

                    Dim imagen As New ImageEx With {
                        .IsCacheEnabled = True,
                        .Source = bitmap,
                        .MaxHeight = 150,
                        .MaxWidth = 150
                    }

                    Dim botonImagen As New Button With {
                        .Padding = New Thickness(0, 0, 0, 0),
                        .Margin = New Thickness(0, 0, 15, 0),
                        .BorderBrush = New SolidColorBrush(App.Current.Resources("ColorPrimario")),
                        .BorderThickness = New Thickness(1, 1, 1, 1),
                        .Tag = fichero.Path,
                        .Content = imagen,
                        .VerticalAlignment = VerticalAlignment.Top
                    }

                    AddHandler botonImagen.Click, AddressOf QuitarImagen
                    AddHandler botonImagen.PointerEntered, AddressOf Entra_Basico
                    AddHandler botonImagen.PointerExited, AddressOf Sale_Basico

                    spImagenes.Children.Add(botonImagen)

                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("imagen" + spImagenes.Children.Count.ToString, fichero)
                Catch ex As Exception

                End Try

                If spImagenes.Children.Count = 4 Then
                    botonAñadirImagenes.IsEnabled = False
                Else
                    botonAñadirImagenes.IsEnabled = True
                End If
            End If
        End Sub

        Private Sub QuitarImagen(sender As Object, e As RoutedEventArgs)

            Dim botonOrigen As Button = sender

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim spImagenes As StackPanel = pagina.FindName("spEnviarMensajeImagenes")

            For Each imagen In spImagenes.Children
                Dim boton As Button = imagen

                If boton.Tag = botonOrigen.Tag Then
                    spImagenes.Children.Remove(boton)
                End If
            Next

            Dim botonAñadirImagenes As Button = pagina.FindName("botonEnviarMensajeAñadirImagen")

            If spImagenes.Children.Count = 4 Then
                botonAñadirImagenes.IsEnabled = False
            Else
                botonAñadirImagenes.IsEnabled = True
            End If

        End Sub

    End Module
End Namespace