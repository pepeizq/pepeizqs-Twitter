NotInheritable Class App
    Inherits Application

    Protected Overrides Async Sub OnLaunched(e As LaunchActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        If rootFrame Is Nothing Then
            rootFrame = New Frame()

            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: Cargar el estado de la aplicación suspendida previamente
            End If

            Window.Current.Content = rootFrame
        End If

        If e.Kind = ActivationKind.Launch Then
            If Not e.Arguments = String.Empty Then
                UsuarioXaml.CambiarCuenta(Nothing, e.Arguments, True)
            End If
        End If

        If e.PrelaunchActivated = False Then
            If rootFrame.Content Is Nothing Then
                rootFrame.Navigate(GetType(MainPage), e.Arguments)
            End If

            Window.Current.Activate()
        End If
    End Sub

    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)

        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)

    End Sub

    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending

        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        deferral.Complete()

    End Sub

    Protected Overrides Sub OnActivated(args As IActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        If rootFrame Is Nothing Then
            rootFrame = New Frame()
            Window.Current.Content = rootFrame
        End If

        Dim payload As String = String.Empty

        If args.Kind = ActivationKind.StartupTask Then
            Dim startupArgs As StartupTaskActivatedEventArgs = args
            payload = ActivationKind.StartupTask.ToString()
        End If

        rootFrame.Navigate(GetType(MainPage), payload)
        Window.Current.Activate()
    End Sub

End Class
