Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Private ReadOnly Property ppObjVisita As Visita
        Get
         If IsNothing(Session("DETALLE_VISITA")) Then
            'Return Nothing
            Dim objVisita As Visita = CType(Session("DETALLE_VISITA_V17"), Visita)
            If Not IsNothing(objVisita) Then
               Return objVisita
            Else
               Return Nothing
            End If
         Else
            Dim objVisita As Visita = CType(Session("DETALLE_VISITA"), Visita)
            If Not IsNothing(objVisita) Then
               Return objVisita
            Else
               Return Nothing
            End If
         End If
        End Get
    End Property

    Public Property puObjUsuario As Entities.Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Entities.Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim ex As Exception = Server.GetLastError()
            If TypeOf ex Is HttpUnhandledException AndAlso ex.InnerException IsNot Nothing Then
                ex = ex.InnerException
            End If

            If ex IsNot Nothing Then

                Dim exception As HttpException = TryCast(ex, HttpException)

                If exception Is Nothing Then
                    ManejaError(ex)

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ''Algo
    End Sub

    Private Sub ManejaError(ByVal ex As Exception)
        Try
            Utilerias.ControlErrores.EscribirEvento(ex.StackTrace, EventLogEntryType.Error)

            Try

            If IsNothing(Session("DETALLE_VISITA")) Then
               If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
                  If ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario Then
                     AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
                     Session.Remove("DETALLE_VISITA_V17")
                  End If
               End If
            Else
               If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
                  If ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario Then
                     AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
                     Session.Remove("DETALLE_VISITA")
                  End If
               End If
            End If


                'Mail

            Catch ex3 As Exception

                Utilerias.ControlErrores.EscribirEvento("Error al enviar mail de error", EventLogEntryType.Error)

            End Try


        Catch ex2 As Exception

            Utilerias.ControlErrores.EscribirEvento(ex2.StackTrace, EventLogEntryType.Error)

        End Try

        Session("Exception_1") = ex

#If Not Debug Then
        Response.Redirect("~/Error.aspx", True)
#End If


    End Sub

End Class