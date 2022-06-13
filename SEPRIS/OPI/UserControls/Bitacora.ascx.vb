Public Class Bitacora2
    Inherits System.Web.UI.UserControl

    Private ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property
    Private _opiDetalle As New OPI_Incumplimiento
    Public Sub Inicializar()
        If Not Page.IsPostBack Then
            CargarFiltros()
        End If
        btnFiltrar_Click(Nothing, Nothing)
    End Sub

    Private Sub CargarFiltros()

        Dim _opiFunc As New Registro_OPI

        _opiDetalle = _opiFunc.GetOPIDetail(Session("I_ID_OPI"))
        Dim folio As String = _opiDetalle.I_ID_OPI

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Fecha      ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_REGISTRO", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Usuario    ", ucFiltro.AcceptedControls.DropDownList, BitacoraOPI.ObtenerUsuarios(folio), "T_DSC_USUARIO", "T_DSC_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Paso       ", ucFiltro.AcceptedControls.DropDownList, BitacoraOPI.ObtenerPasos(folio), "T_DSC_PASO", "T_DSC_PASO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Accion     ", ucFiltro.AcceptedControls.DropDownList, BitacoraOPI.ObtenerAccion(folio), "T_DSC_ACCION", "T_DSC_ACCION", ucFiltro.DataValueType.StringType)
        ucFiltro1.LoadDDL("Bitacora.ascx")


    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim app As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)

        app.ExportaGrid(dt, gvConsulta, "Bitacora", referencias)
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        Dim _opiFunc As New Registro_OPI

        _opiDetalle = _opiFunc.GetOPIDetail(Session("I_ID_OPI"))
        Dim folio As String = _opiDetalle.I_ID_OPI

        Dim consulta As String = "WHERE N_ID_FOLIO = " + folio

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        gvConsulta.DataSource = BitacoraOPI.ObtenerEntradas(consulta)
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If

    End Sub

    Protected Sub gvConsulta_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        Dim _opiFunc As New Registro_OPI
        _opiDetalle = _opiFunc.GetOPIDetail(Session("I_ID_OPI"))
        Dim folio As String = _opiDetalle.I_ID_OPI

        Dim consulta As String = "WHERE N_ID_FOLIO = " + folio

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        gvConsulta.DataSource = BitacoraOPI.ObtenerEntradas(consulta)
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataBind()

    End Sub
End Class