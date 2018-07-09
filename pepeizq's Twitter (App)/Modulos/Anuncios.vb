Imports Microsoft.Toolkit.Uwp.Helpers
Imports pepeizq.Twitter
Imports Windows.Services.Store

Module Anuncios

    Public Async Sub Quitar()

        Dim contexto As StoreContext = StoreContext.GetDefault
        Dim resultado As StorePurchaseResult = Await contexto.RequestPurchaseAsync("9NVM8JPQ57VT")

        If resultado.Status = StorePurchaseStatus.Succeeded Then

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim helper As New LocalObjectStorageHelper

            Dim listaUsuarios As New List(Of TwitterUsuario)

            If helper.KeyExists("listaUsuarios5") Then
                listaUsuarios = helper.Read(Of List(Of TwitterUsuario))("listaUsuarios5")
            End If

            If listaUsuarios.Count > 0 Then
                For Each usuario In listaUsuarios
                    Dim gridInicio As Grid = pagina.FindName("gridAnunciosInicio" + usuario.ID)

                    If Not gridInicio Is Nothing Then
                        gridInicio.Visibility = Visibility.Collapsed
                    End If

                    Dim gridMenciones As Grid = pagina.FindName("gridAnunciosMenciones" + usuario.ID)

                    If Not gridMenciones Is Nothing Then
                        gridMenciones.Visibility = Visibility.Collapsed
                    End If
                Next
            End If

            Dim gridUsuario As Grid = pagina.FindName("gridAnunciosUsuario")
            gridUsuario.Visibility = Visibility.Collapsed

        End If

    End Sub

End Module
