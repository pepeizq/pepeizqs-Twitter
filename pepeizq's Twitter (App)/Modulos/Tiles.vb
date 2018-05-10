Imports Microsoft.Toolkit.Uwp.Notifications
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.Networking.BackgroundTransfer
Imports Windows.Storage
Imports Windows.UI.Notifications
Imports Windows.UI.StartScreen

Module Tiles

    Public Async Sub Generar(megaUsuario As pepeizq.Twitter.MegaUsuario)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim boton As Button = pagina.FindName("botonAñadirTile" + megaUsuario.Usuario2.Usuario.Id)
        boton.IsEnabled = False

        Dim nuevaTile As New SecondaryTile(megaUsuario.Usuario2.Usuario.Id, megaUsuario.Usuario2.Usuario.Nombre, megaUsuario.Usuario2.Usuario.ScreenNombre, New Uri("ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "ancha.png", UriKind.RelativeOrAbsolute), TileSize.Square150x150)

        'nuevaTile.VisualElements.Wide310x150Logo = New Uri("ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "ancha.png", UriKind.RelativeOrAbsolute)
        'nuevaTile.VisualElements.Square310x310Logo = New Uri("ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "grande.png", UriKind.RelativeOrAbsolute)

        Await nuevaTile.RequestCreateAsync()

        '-----------------------

        'Dim imagenDRM As AdaptiveImage = Nothing

        'If ApplicationData.Current.LocalSettings.Values("drm_tile") = True Then
        '    imagenDRM = New AdaptiveImage With {
        '        .HintRemoveMargin = True,
        '        .HintAlign = AdaptiveImageAlign.Right
        '    }

        '    Dim cbIconosLista As ComboBox = pagina.FindName("cbTilesIconosLista")
        '    Dim imagenIcono As ImageEx = cbIconosLista.SelectedItem
        '    imagenDRM.Source = imagenIcono.Source
        'End If

        '-----------------------

        Dim imagenPequeña As New ImageEx With {
            .Source = New Uri(megaUsuario.Usuario2.Usuario.ImagenAvatar)
        }
        Dim boolImagenPequeña As Boolean = Await DescargaImagen(imagenPequeña, megaUsuario.Usuario2.Usuario.Id + "pequena")

        Dim imagenMediana As New ImageEx With {
            .Source = New Uri(megaUsuario.Usuario2.Usuario.ImagenAvatar)
        }
        Dim boolImagenMediana As Boolean = Await DescargaImagen(imagenMediana, megaUsuario.Usuario2.Usuario.Id + "mediana")

        Dim imagenAncha As New ImageEx
        Dim boolImagenAncha As Boolean = Await DescargaImagen(imagenAncha, megaUsuario.Usuario2.Usuario.Id + "ancha")

        Dim imagenGrande As New ImageEx With {
            .Source = New Uri(megaUsuario.Usuario2.Usuario.ImagenAvatar)
        }
        Dim boolImagenGrande As Boolean = Await DescargaImagen(imagenGrande, megaUsuario.Usuario2.Usuario.Id + "grande")

        '-----------------------

        Dim contenidoPequeño As New TileBindingContentAdaptive

        If boolImagenPequeña = True Then
            Dim fondoImagenPequeña As New TileBackgroundImage With {
                .Source = "ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "pequena.png",
                .HintCrop = AdaptiveImageCrop.Default
            }

            contenidoPequeño = New TileBindingContentAdaptive With {
                .BackgroundImage = fondoImagenPequeña
            }
        End If

        'If Not imagenDRM Is Nothing Then
        '    contenidoSmall.Children.Add(imagenDRM)
        'End If

        Dim tilePequeño As New TileBinding With {
            .Content = contenidoPequeño
        }

        '-----------------------

        Dim contenidoMediano As New TileBindingContentAdaptive

        If boolImagenMediana = True Then
            Dim fondoImagenMediano As New TileBackgroundImage With {
                .Source = "ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "mediana.png",
                .HintCrop = AdaptiveImageCrop.Default
            }

            contenidoMediano = New TileBindingContentAdaptive With {
                .BackgroundImage = fondoImagenMediano
            }
        End If

        'If Not imagenDRM Is Nothing Then
        '    contenidoMediano.Children.Add(imagenDRM)
        'End If

        Dim tileMediano As New TileBinding With {
            .Content = contenidoMediano
        }

        '-----------------------

        Dim contenidoAncho As New TileBindingContentAdaptive

        If boolImagenAncha = True Then
            Dim fondoImagenAncha As New TileBackgroundImage With {
                .Source = "ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "ancha.png",
                .HintCrop = AdaptiveImageCrop.Default
            }

            contenidoAncho = New TileBindingContentAdaptive With {
                .BackgroundImage = fondoImagenAncha
            }
        End If

        'If Not imagenDRM Is Nothing Then
        '    contenidoAncho.Children.Add(imagenDRM)
        'End If

        Dim tileAncha As New TileBinding With {
            .Content = contenidoAncho
        }

        '-----------------------

        Dim contenidoGrande As New TileBindingContentAdaptive

        If boolImagenGrande = True Then
            Dim fondoImagenGrande As New TileBackgroundImage With {
                .Source = "ms-appdata:///local/" + megaUsuario.Usuario2.Usuario.Id + "grande.png",
                .HintCrop = AdaptiveImageCrop.Default
            }

            contenidoGrande = New TileBindingContentAdaptive With {
                .BackgroundImage = fondoImagenGrande
            }
        End If

        'If Not imagenDRM Is Nothing Then
        '    contenidoGrande.Children.Add(imagenDRM)
        'End If

        Dim tileGrande As New TileBinding With {
            .Content = contenidoGrande
        }

        '-----------------------

        tileAncha.Branding = TileBranding.Name
        tilePequeño.Branding = TileBranding.Name
        tileMediano.Branding = TileBranding.Name
        tileGrande.Branding = TileBranding.Name

        Dim visual As New TileVisual With {
            .TileWide = tileAncha,
            .TileSmall = tilePequeño,
            .TileMedium = tileMediano,
            .TileLarge = tileGrande
        }

        Dim contenido As New TileContent With {
            .Visual = visual
        }

        Dim notificacion As New TileNotification(contenido.GetXml)

        Try
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(megaUsuario.Usuario2.Usuario.Id).Update(notificacion)
        Catch ex As Exception

        End Try

        boton.IsEnabled = True

    End Sub

    Public Async Function DescargaImagen(imagen As ImageEx, clave As String) As Task(Of Boolean)

        Dim descargaFinalizada As Boolean = False

        Dim fuente As Object = imagen.Source

        If TypeOf fuente Is Uri Then
            Dim ficheroImagen As StorageFile = Await ApplicationData.Current.LocalFolder.CreateFileAsync(clave + ".png", CreationCollisionOption.ReplaceExisting)
            Dim descargador As New BackgroundDownloader

            Try
                Dim descarga As DownloadOperation = descargador.CreateDownload(fuente, ficheroImagen)
                Await descarga.StartAsync
                descargaFinalizada = True
            Catch ex As Exception

            End Try
        End If

        If TypeOf fuente Is BitmapImage Then
            Dim ficheroOrigen As StorageFile = imagen.Tag
            Dim ficheroNuevo As StorageFile = Await ApplicationData.Current.LocalFolder.CreateFileAsync(clave + ".png", CreationCollisionOption.ReplaceExisting)

            Await ficheroOrigen.CopyAndReplaceAsync(ficheroNuevo)
            descargaFinalizada = True
        End If

        Return descargaFinalizada
    End Function

End Module
