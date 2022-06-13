Public Class RegistroSesiones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
            CargarCatalogo()
        End If
    End Sub

    Private Sub CargarFiltros()
        ucFiltro1.resetSession()
        Dim objUsuario As New Entities.Usuario
        ucFiltro1.AddFilter("Usuario", ucFiltro.AcceptedControls.DropDownList, objUsuario.ObtenerTodos.ToTable, "NOMBRE_COMPLETO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Activo", ucFiltro.AcceptedControls.CheckBox, , , "N_FLAG_ACTIVO", ucFiltro.DataValueType.BoolType)
        ucFiltro1.AddFilter("Fecha", ucFiltro.AcceptedControls.Calendar, , , "F_FECH_INI", ucFiltro.DataValueType.StringType, False, , True, True, True, DateTime.Now.ToString("dd/MM/yyyy"))
        ucFiltro1.LoadDDL("RegistroSesiones.aspx")
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Private Sub CargarCatalogo()
        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim entSesion As New Entities.Sesion
        Dim dv As DataView = entSesion.ObtenerRango

        dv.RowFilter = consulta

        gvConsulta.DataSource = dv.ToTable()
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            pnlNoExiste.Visible = True
            gvConsulta.Visible = False
        Else
            pnlNoExiste.Visible = False
            gvConsulta.Visible = True
        End If
    End Sub

    Private Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcel.Click

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        utl.ExportaGrid(dt, gvConsulta, "Registro de Sesiones", referencias)

    End Sub

End Class