Public Class Irregularidad
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    <System.ComponentModel.Browsable(False)>
    Public Property BotonesVisibles As Boolean
        Get
            Return pnlBotones.Visible
        End Get
        Set(value As Boolean)
            pnlBotones.Visible = value
        End Set
    End Property



    Public Sub Inicializar()
        ddlProceso.Attributes.Add("onchange", "ObtenerListaSubprocesos()")
        ddlSubproceso.Attributes.Add("onchange", "ObtenerListaConductaSaludable()")
        ddlConducta.Attributes.Add("onchange", "ObtenerListaIrregularidades()")
        ddlProceso.DataTextField = "DESC_PROCESO"
        ddlProceso.DataValueField = "ID_PROCESO"
        ddlProceso.DataSource = ConexionSISAN.ObtenerProcesos()
        ddlProceso.DataBind()
        ddlProceso.Items.Insert(0, New ListItem("--Selecciona un Proceso--", "-1"))

        If Entities.Irregularidad.ObtenerTodas(Folio).Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Carga de Irregularidades", "BindIrregularidades();", True)
        End If



    End Sub

End Class