Imports Tweetinvi
Imports Tweetinvi.Models
Imports Windows.Security.Credentials
Imports Windows.Storage

Module Conexion

    Public Sub Cargar()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim tbUsuario As TextBox = pagina.FindName("tbUsuario")

        RemoveHandler tbUsuario.TextChanged, AddressOf Configuracion.Usuario_Texto_Cambia
        AddHandler tbUsuario.TextChanged, AddressOf Configuracion.Usuario_Texto_Cambia

        Dim pbUsuarioContraseña As PasswordBox = pagina.FindName("pbUsuarioContraseña")

        RemoveHandler pbUsuarioContraseña.PasswordChanging, AddressOf Configuracion.Contraseña_Texto_Cambia
        AddHandler pbUsuarioContraseña.PasswordChanging, AddressOf Configuracion.Contraseña_Texto_Cambia

        Dim botonAñadirUsuario As Button = pagina.FindName("botonAñadirUsuario")

        RemoveHandler botonAñadirUsuario.Click, AddressOf GuardarUsuario
        AddHandler botonAñadirUsuario.Click, AddressOf GuardarUsuario

        RemoveHandler botonAñadirUsuario.PointerEntered, AddressOf Interfaz.Entra_Boton_IconoTexto
        AddHandler botonAñadirUsuario.PointerEntered, AddressOf Interfaz.Entra_Boton_IconoTexto

        RemoveHandler botonAñadirUsuario.PointerExited, AddressOf Interfaz.Sale_Boton_IconoTexto
        AddHandler botonAñadirUsuario.PointerExited, AddressOf Interfaz.Sale_Boton_IconoTexto

        Dim spUsuariosGuardados As StackPanel = pagina.FindName("spUsuariosGuardados")

        Dim caja As New PasswordVault
        Dim listaUsuarios As IReadOnlyList(Of PasswordCredential)

        Try
            listaUsuarios = caja.FindAllByResource(Package.Current.DisplayName)

            If listaUsuarios.Count > 0 Then
                Dim gridCarga As Grid = pagina.FindName("gridCarga")
                Interfaz.Pestañas.Visibilidad_Pestañas(gridCarga)

                spUsuariosGuardados.Visibility = Visibility.Visible
                CargarListaUsuarios()
            Else
                spUsuariosGuardados.Visibility = Visibility.Collapsed
            End If
        Catch ex As Exception
            spUsuariosGuardados.Visibility = Visibility.Collapsed

            Dim gridConfig As Grid = pagina.FindName("gridConfig")
            Interfaz.Pestañas.Visibilidad_Pestañas(gridConfig)

            Dim botonConfiguracionUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")
            Dim gridConfiguracionUsuarios As Grid = pagina.FindName("gridConfiguracionUsuarios")
            Interfaz.Pestañas.Visibilidad_Pestañas_Config(botonConfiguracionUsuarios, gridConfiguracionUsuarios)
        End Try

    End Sub

    Private Sub CargarListaUsuarios()

        Dim caja As New PasswordVault
        Dim listaUsuarios As IReadOnlyList(Of PasswordCredential) = caja.FindAllByResource(Package.Current.DisplayName)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spUsuariosGuardados As StackPanel = pagina.FindName("spUsuariosGuardados")

        Dim spUsuariosGuardadosLista As StackPanel = pagina.FindName("spUsuariosGuardadosLista")
        spUsuariosGuardadosLista.Children.Clear()

        If listaUsuarios.Count > 0 Then
            spUsuariosGuardados.Visibility = Visibility.Visible

            For Each usuario In listaUsuarios
                spUsuariosGuardadosLista.Children.Add(Configuracion.AñadirListaGuardados(usuario.UserName))
            Next

            Dim enseñarConfig As Boolean = True

            For Each usuario In spUsuariosGuardadosLista.Children
                Dim grid As Grid = usuario
                Dim toggle As ToggleSwitch = grid.Children(1)

                If toggle.IsOn = True Then
                    enseñarConfig = False
                End If
            Next

            If enseñarConfig = True Then
                Dim gridConfig As Grid = pagina.FindName("gridConfig")
                Interfaz.Pestañas.Visibilidad_Pestañas(gridConfig)

                Dim botonConfiguracionUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")
                Dim gridConfiguracionUsuarios As Grid = pagina.FindName("gridConfiguracionUsuarios")
                Interfaz.Pestañas.Visibilidad_Pestañas_Config(botonConfiguracionUsuarios, gridConfiguracionUsuarios)
            End If
        Else
            spUsuariosGuardados.Visibility = Visibility.Collapsed
        End If

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
            Dim listaUsuarios As IReadOnlyList(Of PasswordCredential)

            Try
                listaUsuarios = caja.FindAllByResource(Package.Current.DisplayName)

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
            Catch ex As Exception
                caja.Add(New PasswordCredential(Package.Current.DisplayName, tbUsuario.Text.Trim, pbContraseña.Password.Trim))
                CargarListaUsuarios()
            End Try
        End If

        tbUsuario.Text = String.Empty
        pbContraseña.Password = String.Empty

    End Sub

    Public Async Sub Conectar(usuario As String, primeraVez As Boolean)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spCarga As StackPanel = pagina.FindName("spCarga")
        spCarga.Visibility = Visibility.Visible

        Dim spConexion As StackPanel = pagina.FindName("spConexion")
        spConexion.Visibility = Visibility.Collapsed

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

        Dim mostrarConfig As Boolean = False

        If wvTwitter.Source.AbsoluteUri.Contains("https://twitter.com/login?username_disabled=true&redirect_after_login=") Then
            Dim spCarga As StackPanel = pagina.FindName("spCarga")
            spCarga.Visibility = Visibility.Collapsed

            Dim spConexion As StackPanel = pagina.FindName("spConexion")
            spConexion.Visibility = Visibility.Visible
        End If

        If wvTwitter.Source.AbsoluteUri.Contains("https://api.twitter.com/oauth/authorize") Then
            Try
                Dim usuario As String = "document.getElementById('username_or_email').value = '" + credencial.UserName + "'"
                Await wvTwitter.InvokeScriptAsync("eval", New List(Of String) From {usuario})

                Dim contraseña As String = "document.getElementById('password').value = '" + credencial.Password + "'"
                Await wvTwitter.InvokeScriptAsync("eval", New List(Of String) From {contraseña})
            Catch ex As Exception

            End Try

            Try
                Await wvTwitter.InvokeScriptAsync("eval", New String() {"document.getElementById('remember').checked = true;"})
            Catch ex As Exception

            End Try

            Try
                Await wvTwitter.InvokeScriptAsync("eval", New String() {"document.getElementById('allow').click();"})
            Catch ex As Exception

            End Try

            Dim html As String = Await wvTwitter.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})

            If Not html = Nothing Then
                If html.Contains("<code>") Then
                    Dim int As Integer = html.IndexOf("<code>")
                    Dim temp As String = html.Remove(0, int + 6)

                    Dim int2 As Integer = temp.IndexOf("</code>")
                    Dim temp2 As String = temp.Remove(int2, temp.Length - int2)

                    tbCodigo.Text = temp2

                    Dim usuarioCredenciales As ITwitterCredentials = Await appCliente.Auth.RequestCredentialsFromVerifierCodeAsync(tbCodigo.Text, peticion)

                    If Not usuarioCredenciales Is Nothing Then
                        Dim usuarioCliente As New TwitterClient(usuarioCredenciales)

                        If Not usuarioCliente Is Nothing Then
                            Dim usuario As IAuthenticatedUser = Await usuarioCliente.Users.GetAuthenticatedUserAsync

                            If Not usuario Is Nothing Then
                                Interfaz.Inicio.CargarDatos(usuarioCliente, usuario)
                            Else
                                mostrarConfig = True
                            End If
                        Else
                            mostrarConfig = True
                        End If
                    Else
                        mostrarConfig = True
                    End If
                End If
            End If
        End If

        If mostrarConfig = True Then
            Dim gridConfig As Grid = pagina.FindName("gridConfig")
            Interfaz.Pestañas.Visibilidad_Pestañas(gridConfig)

            Dim botonConfiguracionUsuarios As Button = pagina.FindName("botonConfiguracionUsuarios")
            Dim gridConfiguracionUsuarios As Grid = pagina.FindName("gridConfiguracionUsuarios")
            Interfaz.Pestañas.Visibilidad_Pestañas_Config(botonConfiguracionUsuarios, gridConfiguracionUsuarios)
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

