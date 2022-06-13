Public Class Logout
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Propiedad que regresa el nombre de la página
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SE EMPLEA EN LA MASTER PAGE</remarks>
    Public Shared ReadOnly Property Nombre As String
        Get
            Return "Logout.aspx"
        End Get
    End Property

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session.Clear()
        'Session.Abandon()
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

        System.Web.HttpContext.Current.Session.Clear()
        System.Web.HttpContext.Current.Session.Abandon()


        Dim manager As New SessionState.SessionIDManager()
        Dim newID As String = manager.CreateSessionID(System.Web.HttpContext.Current)

        manager.SaveSessionID(System.Web.HttpContext.Current, newID, False, False)

    End Sub

    Protected Sub btnCerrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCerrar.Click

        Response.Redirect("Login.aspx", False)


    End Sub
End Class