Public Class BitacoraVisita
    Inherits System.Web.UI.UserControl

    Private ReadOnly Property Visita
        Get
            Return Session("ID_VISITA")
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
        End If

        btnFiltrar_Click(Nothing, Nothing)
    End Sub
    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Fecha movimiento      ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_REGISTRO", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Usuario    ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraVisita.ObtenerUsuarios(Visita), "T_ID_USUARIO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Paso       ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraVisita.ObtenerPasos(Visita), "I_ID_PASO", "I_ID_PASO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Comentario     ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraVisita.ObtenerAccion(Visita), "T_DSC_COMENTARIO", "T_DSC_COMENTARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Descripción de movimiento     ", ucFiltro.AcceptedControls.DropDownList, Entities.BitacoraVisita.ObtenerAccion(Visita), "T_DSC_MOVIMIENTO", "T_DSC_MOVIMIENTO", ucFiltro.DataValueType.StringType)


        ucFiltro1.LoadDDL("BitacoraVisita.ascx")


    End Sub

    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim app As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)

        app.ExportaGrid(dt, gvConsulta, "BitacoraVisita", referencias)
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        Dim consulta As String = "WHERE BVE.I_ID_VISITA = " + Visita.ToString() + " AND BVE.T_ID_TIPO_COMENTARIO = 'USUARIO' "

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next
        gvConsulta.DataSource = Entities.BitacoraVisita.ObtenerEntradas(consulta)
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