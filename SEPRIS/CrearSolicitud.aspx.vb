Imports Entities
Public Class CrearSolicitud
    Inherits System.Web.UI.Page
    Public Property Mensaje As String
    Public Shared idSol As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarSolicitudes()
            If Not Session("FolioEnviado") Is Nothing Then
                Dim errores As New Entities.EtiquetaError(2080)
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                errores.Descripcion = errores.Descripcion.Replace("[FOLIO]", Session("FolioEnviado").ToString)
                Mensaje = errores.Descripcion
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmación de envío a autorización", "AquiMuestroMensaje();", True)
                Session("FolioEnviado") = Nothing
            End If
        End If
        CargarBanner()
    End Sub
    Public Sub CargarBanner()
        Dim objPaquete As New Paquete
        Dim dtPaquetes As DataTable = objPaquete.ObtenerTodosImagen()
        pnlCarrusel.Controls.Add(New LiteralControl("<div id='carouselh'>"))
        pnlCarrusel.Controls.Add(New LiteralControl("<div><img alt='' src='Imagenes/Home.png?id=-1' /><br />"))
        pnlCarrusel.Controls.Add(New LiteralControl("<span class='thumbnail-text'>" & Conexion.SQLServer.Parametro.ObtenerValor("Nombre Solicitud Normal") & "</span></div>"))
        For Each row As DataRow In dtPaquetes.Rows()
            'AQUI VA EL BUSCAR Y CONCATENAR LOS SERVICIOS DE CADA PAQWUETE
            Dim tooltip As String = String.Empty
            objPaquete.Identificador = row("N_ID_GRUPO_SERVICIO")
            Dim dtTool As DataTable = objPaquete.ObtenerServiciosTooltip()
            For Each trow As DataRow In dtTool.Rows()
                If CInt(trow("N_ID_APLICATIVO")) = 0 Then
                    tooltip = tooltip & CStr(trow("N_ID_NIVELES_SERVICIO")) & "-" & CStr(trow("T_DSC_SERVICIO"))
                Else
                    tooltip = tooltip & CStr(trow("N_ID_NIVELES_SERVICIO")) & "-" & CStr(trow("T_DSC_APLICATIVO")) & "-" & CStr(trow("T_DSC_SERVICIO"))
                End If
                tooltip = tooltip & "&#013;"
            Next
            pnlCarrusel.Controls.Add(New LiteralControl("<div><img alt='' src='Imagenes/Errores/" & row("IMAGEN") & "' title='" & tooltip & "' /><br />"))
            pnlCarrusel.Controls.Add(New LiteralControl("<span class='thumbnail-text'>" & row("T_DSC_GRUPO_SERVICIO") & "</span></div>"))
        Next
        pnlCarrusel.Controls.Add(New LiteralControl("</div>"))
    End Sub
    Private Sub CargarSolicitudes()
        Dim objSolicitud As New Solicitud
        Dim usuario As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
        Dim dt As DataTable = objSolicitud.CargarDatos(usuario.IdentificadorUsuario)
        gvConsultaSolicitudes.DataSource = dt
        gvConsultaSolicitudes.DataBind()
        MuestraImagenes()
    End Sub
    Protected Sub gvConsultaSolicitudes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvConsultaSolicitudes.RowCommand
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        idSol = gvConsultaSolicitudes.DataKeys(index)("N_ID_SOLICITUD_SERVICIO").ToString()
        Dim errores

        If e.CommandName.Equals("Modificar") Then
            Dim objSolicitud As New Solicitud(CInt(idSol))
            If objSolicitud.ObtenerTipoFlujo() = 3 Then
                Session("idPaquete") = "0"
            End If
            Session("idSolicitud") = idSol
            Response.Redirect("AltaSolicitud.aspx")
        ElseIf e.CommandName.Equals("Borrar") Then
            btnAceptarM2B1A.CommandArgument = "imbBorrar"
            errores = New Entities.EtiquetaError(1192)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
        ElseIf e.CommandName.Equals("Historial") Then
            CargarHistorialComentarios()
            pnlHistorial.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SEDI", "MensajeHistorial();", True)
        End If
    End Sub

    Public Sub CargarHistorialComentarios()
        Dim objHist As New HistorialSolicitud()
        objHist.IdSolicitud = idSol
        gvHistCom.DataSource = objHist.ObtenerHistorialComentarios()
        gvHistCom.DataBind()
        If gvHistCom.Rows.Count > 0 Then
            imgHistComentarios.Visible = False
        Else
            imgHistComentarios.Visible = True
        End If
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "imbBorrar"
                Dim objSolicitud As New Solicitud()
                objSolicitud.Identificador = idSol
                objSolicitud.Eliminar()
                CargarSolicitudes()
        End Select
    End Sub

    'Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrear.Click
    '    Session("idSolicitud") = Nothing
    '    Response.Redirect("AltaSolicitud.aspx")
    'End Sub

    Protected Sub lnkImagen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkImagen.Click
        Dim idPaq As String = hdfImagen.Value.Substring(hdfImagen.Value.LastIndexOf("=") + 1)
        If idPaq.Equals("-1") Then
            Session("idSolicitud") = Nothing
            Session("idPaquete") = Nothing
        Else
            Session("idSolicitud") = Nothing
            Session("idPaquete") = idPaq
        End If
        Response.Redirect("AltaSolicitud.aspx")
    End Sub

    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvConsultaSolicitudes.Rows.Count > 0 Then
            imgNoDisp.Visible = False
        Else
            imgNoDisp.Visible = True
        End If
    End Sub
End Class