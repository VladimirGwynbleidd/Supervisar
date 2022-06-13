Public Class BandejaSICOD
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
            btnFiltrar_Click(sender, e)

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 6
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If

        End If
    End Sub


    Private Sub CargarFiltros()

        ucFiltro1.resetSession()


        Dim objEstatus() = {New With {.T_DSC_ESTATUS = "Sin asignar", .I_ID_ESTATUS = 0},
                           New With {.T_DSC_ESTATUS = "Asignado", .I_ID_ESTATUS = 3},
                           New With {.T_DSC_ESTATUS = "En atención", .I_ID_ESTATUS = 4}}

        ucFiltro1.AddFilter("ESTATUS             ", ucFiltro.AcceptedControls.DropDownList, objEstatus, "T_DSC_ESTATUS", "I_ID_ESTATUS", ucFiltro.DataValueType.IntegerType, , , , True, True, 0)
        ucFiltro1.AddFilter("Folio sicod         ", ucFiltro.AcceptedControls.TextBox, Nothing, "N_ID_FOLIO", "N_ID_FOLIO", ucFiltro.DataValueType.IntegerType, , , , , , , 10)
        ucFiltro1.AddFilter("Fecha de registro      ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_REGISTRO", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Número de Oficio    ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_NUM_OFICIO", "T_DSC_NUM_OFICIO", ucFiltro.DataValueType.StringType, , True, , , , , 40)
        ucFiltro1.AddFilter("Referencia          ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_REFERENCIA", "T_DSC_REFERENCIA", ucFiltro.DataValueType.StringType, , True, , , , , 40)
        ucFiltro1.AddFilter("Fecha Documento     ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_DOC", "F_FECH_DOC", ucFiltro.DataValueType.StringType, False, , True, , , "01/01/2018")
        ucFiltro1.AddFilter("Fecha Recepción doc ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_RECEPCION", "F_FECH_RECEPCION", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Tipo Documento   ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerTipoDocumento, "T_DSC_T_DOC", "T_DSC_T_DOC", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Remitente           ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerEntidad, "T_DSC_REMITENTE", "T_DSC_REMITENTE", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Nombre firmante     ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_NOMB_FIRMNT", "T_DSC_NOMB_FIRMNT", ucFiltro.DataValueType.StringType, , True, , , , , 40)
        ucFiltro1.AddFilter("Apellido Paterno    ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_AP_PAT_FIRMNT", "T_DSC_AP_PAT_FIRMNT", ucFiltro.DataValueType.StringType, , True, , , , , 40)
        ucFiltro1.AddFilter("Apellido Materno    ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_AP_MAT_FIRMNT", "T_DSC_AP_MAT_FIRMNT", ucFiltro.DataValueType.StringType, , True, , , , , 40)
        ucFiltro1.AddFilter("ASUNTO              ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_ASUNTO", "T_DSC_ASUNTO", ucFiltro.DataValueType.StringType, , True, , , , , 50)

        ucFiltro1.LoadDDL("BandejaSICOD.aspx")

    End Sub

    Private Sub btnPersonalizarColumnas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizarColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 6
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 6
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Session("ID_FOLIO") = CInt(gvConsulta.DataKeys(index)("N_ID_FOLIO").ToString())
        Session("ID_FOLIO_SICOD") = CInt(gvConsulta.DataKeys(index)("N_ID_SICOD").ToString())


        Response.Redirect("~/PC/Paso1.aspx", False)

    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim app As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)

        app.ExportaGrid(dt, gvConsulta, "Bandeja registros SICOD", referencias)
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        Dim consulta As String = "WHERE (CASE WHEN I_ID_AREA BETWEEN 19 AND 26 OR I_ID_AREA BETWEEN 46 AND 49 THEN 36 WHEN I_ID_AREA BETWEEN 13 AND 18 THEN 35  WHEN I_ID_AREA BETWEEN 6 AND 7 THEN 1 ELSE 34 END) =" + CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea.ToString()

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        gvConsulta.DataSource = BandejaPC.ObtenerFolios(consulta)
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