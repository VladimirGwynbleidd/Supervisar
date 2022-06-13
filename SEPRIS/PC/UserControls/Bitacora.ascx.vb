Public Class Bitacora1
    Inherits System.Web.UI.UserControl

    Private ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property


    Public Sub Inicializar()
        If Not Page.IsPostBack Then
            CargarFiltros()
        End If

        btnFiltrar_Click(Nothing, Nothing)
    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Fecha      ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_REGISTRO", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Usuario    ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraPC.ObtenerUsuarios(Folio), "T_DSC_NOMBRE", "T_DSC_NOMBRE", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Paso       ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraPC.ObtenerPasos(Folio), "T_DSC_PASO", "T_DSC_PASO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Accion     ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraPC.ObtenerAccion(Folio), "T_DSC_ACCION", "T_DSC_ACCION", ucFiltro.DataValueType.StringType)
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

        Dim consulta As String = "WHERE N_ID_FOLIO = " + Folio.ToString()

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        gvConsulta.DataSource = Entities.BitacoraPC.ObtenerEntradas(consulta)
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If

    End Sub
End Class