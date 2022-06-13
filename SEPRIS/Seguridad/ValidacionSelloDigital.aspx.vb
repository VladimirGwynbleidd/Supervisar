Public Class ValidacionSelloDigital
    Inherits System.Web.UI.Page
    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("idSolicitud") = Nothing
            Session("FolioSolicitud") = Nothing
        Else
            lblCharsRestantes.Text = "Caracteres restantes: " + CStr(250 - txtSelloDigital.Text.Length)
        End If
    End Sub

    Protected Sub btnValidar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnValidar.Click
        Page.Validate("valSelloDigital")
        Dim bitacora As New Conexion.Bitacora("Validación de sello digital", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objSolicitud As New Entities.Solicitud
        objSolicitud = objSolicitud.ObtenerSolicitudSelloDigital(txtSelloDigital.Text.Trim)
        If IsNothing(objSolicitud) Then
            bitacora.Finalizar(False)
            Dim errores As New Entities.EtiquetaError(2083)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
        Else
            bitacora.Finalizar(True)
            Session("idSolicitud") = objSolicitud.Identificador
            Session("FolioSolicitud") = objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString
            Session("SessionCurrentMenu") = 9
            Response.Redirect("../DetalleSolicitud.aspx")
        End If
    End Sub

    Protected Sub cvSelloDigital_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvSelloDigital.ServerValidate
        If Not txtSelloDigital.Text.Trim.Length > 0 Then
            Dim errores As New Entities.EtiquetaError(2084)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
        If txtSelloDigital.Text.Length > 250 Then
            Dim errores As New Entities.EtiquetaError(2085)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

End Class