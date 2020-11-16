Imports Tweetinvi
Imports Tweetinvi.Models
Imports Windows.Security.Credentials
Imports Windows.Storage

Module Conexion

    Public Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonAñadirUsuario As Button = pagina.FindName("botonAñadirUsuario")

        RemoveHandler botonAñadirUsuario.Click, AddressOf GuardarUsuario
        AddHandler botonAñadirUsuario.Click, AddressOf GuardarUsuario

        RemoveHandler botonAñadirUsuario.PointerEntered, AddressOf Interfaz.Entra_IconoNombre
        AddHandler botonAñadirUsuario.PointerEntered, AddressOf Interfaz.Entra_IconoNombre

        RemoveHandler botonAñadirUsuario.PointerExited, AddressOf Interfaz.Sale_IconoNombre
        AddHandler botonAñadirUsuario.PointerExited, AddressOf Interfaz.Sale_IconoNombre

        Dim spUsuariosGuardados As StackPanel = pagina.FindName("spUsuariosGuardados")

        Dim caja As New PasswordVault
        Dim listaUsuarios As IReadOnlyList(Of PasswordCredential) = caja.FindAllByResource(Package.Current.DisplayName)

        If listaUsuarios.Count > 0 Then
            spUsuariosGuardados.Visibility = Visibility.Visible

            CargarListaUsuarios()
        Else
            spUsuariosGuardados.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Sub CargarListaUsuarios()

        Dim caja As New PasswordVault
        Dim listaUsuarios As IReadOnlyList(Of PasswordCredential) = caja.FindAllByResource(Package.Current.DisplayName)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spUsuariosGuardadosLista As StackPanel = pagina.FindName("spUsuariosGuardadosLista")
        spUsuariosGuardadosLista.Children.Clear()

        For Each usuario In listaUsuarios
            spUsuariosGuardadosLista.Children.Add(Interfaz.AñadirListaGuardados(usuario.UserName))
        Next

    End Sub

    Private Sub GuardarUsuario(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim guardar As Boolean = True

        Dim tbUsuario As TextBox = pagina.FindName("tbUsuario")

        If tbUsuario.Text.Trim.Length = 0 Then
            guardar = False
        End If

        Dim pbContraseña As PasswordBox = pagina.FindName("pbUsuarioContraseña")

        If pbContraseña.Password.Trim.Length = 0 Then
            guardar = False
        End If

        If guardar = True Then
            Dim caja As New PasswordVault
            Dim listaUsuarios As IReadOnlyList(Of PasswordCredential) = caja.FindAllByResource(Package.Current.DisplayName)

            Dim añadir As Boolean = True

            For Each usuario In listaUsuarios
                If usuario.UserName.ToLower = tbUsuario.Text.ToLower.Trim Then
                    añadir = False
                End If
            Next

            If añadir = True Then
                If listaUsuarios.Count = 0 Then
                    ApplicationData.Current.LocalSettings.Values("idUsuario") = tbUsuario.Text.Trim
                End If

                caja.Add(New PasswordCredential(Package.Current.DisplayName, tbUsuario.Text.Trim, pbContraseña.Password.Trim))
                CargarListaUsuarios()
            End If
        End If

    End Sub

    Public Async Sub Conectar(usuario As String, primeraVez As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbCodigo As TextBox = pagina.FindName("tbConexionCodigo")

        Dim wvTwitter As WebView = pagina.FindName("wvConexionCodigo")

        If primeraVez = False Then
            Await WebView.ClearTemporaryWebDataAsync
        End If

        AddHandler wvTwitter.NavigationCompleted, AddressOf Comprobar

        Dim appCliente As New TwitterClient("poGVvY5De5zBqQ4ceqp7jw7cj", "f8PCcuwFZxYi0r5iG6UaysgxD0NoaCT2RgYG8I41mvjghy58rc")
        Dim peticion As IAuthenticationRequest = Await appCliente.Auth.RequestAuthenticationUrlAsync

        wvTwitter.Source = New Uri(peticion.AuthorizationURL)
        tbCodigo.Tag = New AppClienteyPeticion(appCliente, peticion, usuario)

    End Sub

    Private Async Sub Comprobar(sender As Object, e As WebViewNavigationCompletedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbCodigo As TextBox = pagina.FindName("tbConexionCodigo")
        Dim datos As AppClienteyPeticion = tbCodigo.Tag

        Dim caja As New PasswordVault
        Dim listaUsuarios As IReadOnlyList(Of PasswordCredential) = caja.FindAllByResource(Package.Current.DisplayName)

        Dim i As Integer = 0
        For Each usuario In listaUsuarios
            If usuario.UserName = datos.usuario Then
                Exit For
            End If
            i += 1
        Next

        Dim credencial As PasswordCredential = listaUsuarios(i)
        credencial.RetrievePassword()

        Dim appCliente As TwitterClient = datos.appCliente
        Dim peticion As IAuthenticationRequest = datos.peticion

        Dim wvTwitter As WebView = sender

        If wvTwitter.Source.AbsoluteUri.Contains("https://api.twitter.com/oauth/authorize") Then
            Try
                Dim usuario As String = "document.getElementById('username_or_email').value = '" + credencial.UserName + "'"
                Await wvTwitter.InvokeScriptAsync("eval", New List(Of String) From {usuario})

                Dim contraseña As String = "document.getElementById('password').value = '" + credencial.Password + "'"
                Await wvTwitter.InvokeScriptAsync("eval", New List(Of String) From {contraseña})

                Await wvTwitter.InvokeScriptAsync("eval", New String() {"document.getElementById('allow').click();"})
            Catch ex As Exception

            End Try

            Dim html As String = Await wvTwitter.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})

            If html.Contains("<code>") Then
                Dim int As Integer = html.IndexOf("<code>")
                Dim temp As String = html.Remove(0, int + 6)

                Dim int2 As Integer = temp.IndexOf("</code>")
                Dim temp2 As String = temp.Remove(int2, temp.Length - int2)

                tbCodigo.Text = temp2

                Dim usuarioCredenciales As ITwitterCredentials = Await appCliente.Auth.RequestCredentialsFromVerifierCodeAsync(tbCodigo.Text, peticion)

                Dim usuarioCliente As New TwitterClient(usuarioCredenciales)

                Dim usuario As IAuthenticatedUser = Await usuarioCliente.Users.GetAuthenticatedUserAsync

                Notificaciones.Toast(usuario.Name)
            End If

        End If

    End Sub

End Module

Public Class AppClienteyPeticion

    Public appCliente As TwitterClient
    Public peticion As IAuthenticationRequest
    Public usuario As String

    Public Sub New(appcliente As TwitterClient, peticion As IAuthenticationRequest, usuario As String)
        Me.appCliente = appcliente
        Me.peticion = peticion
        Me.usuario = usuario
    End Sub

End Class

