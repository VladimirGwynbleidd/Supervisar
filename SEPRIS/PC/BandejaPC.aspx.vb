Public Class BandejaPC

    Inherits System.Web.UI.Page

    Public ReadOnly Property IdArea
        Get
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID)
            Return usuario.IdArea
        End Get
    End Property

    Public Property Mensaje As String
    Dim enc As New YourCompany.Utils.Encryption.Encryption64
    Dim validacionF As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
            CargarImagenes()
            btnFiltrar_Click(sender, e)

            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 8
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()

                'For index As Integer = 1 To gvConsulta.Columns.Count - 1
                '    If usuario.IdArea = 36 And gvConsulta.Columns(index).HeaderText = "Sub Entidad" Then
                '        gvConsulta.Columns(index).Visible = True
                '    End If
                'Next

            End If

        End If
    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()

        Dim FiltroArea As Integer = 0


        If Not CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
            FiltroArea = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        End If

        If Not CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea = 34 Then
            FiltroArea = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        End If


        Dim objEstatus() = {New With {.T_DSC_ESTATUS = "Sin asignar", .I_ID_ESTATUS = 0},
                           New With {.T_DSC_ESTATUS = "Asignado", .I_ID_ESTATUS = 1},
                           New With {.T_DSC_ESTATUS = "En atención", .I_ID_ESTATUS = 2}}

        Dim objCumplePC() = {New With {.T_DSC_PC_CUMPLE = "No Cumple", .I_ID_PC_CUMPLE = 0},
                           New With {.T_DSC_PC_CUMPLE = "Sí Cumple", .I_ID_PC_CUMPLE = 1}}

        Dim objResolucionPC() = {New With {.T_DSC_RESOLUCION = "Procede", .I_ID_RESOLUCION = 1},
                           New With {.T_DSC_RESOLUCION = "No Procede", .I_ID_RESOLUCION = 2},
                           New With {.T_DSC_RESOLUCION = "Procede con Plazo", .I_ID_RESOLUCION = 3},
                           New With {.T_DSC_RESOLUCION = "No Presentado", .I_ID_RESOLUCION = 4}}

        ucFiltro1.AddFilter("Folio               ", ucFiltro.AcceptedControls.TextBox, Nothing, "I_ID_FOLIO_SUPERVISAR", "I_ID_FOLIO_SUPERVISAR", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Fecha de registro   ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_REGISTRO", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, False, True, True, False, Date.Today.AddMonths(-6), 10, False)
        ucFiltro1.AddFilter("Entidad             ", ucFiltro.AcceptedControls.DropDownList, ConexionSICOD.ObtenerEntidadesAFOREArea(IdArea), "SIGLAS_ENT", "CVE_ID_ENT", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Núm. PC de Entidad  ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_NUM_OFICIO", "T_DSC_NUM_OFICIO", ucFiltro.DataValueType.StringType, , , , , , , 40)
        ucFiltro1.AddFilter("Fecha de documento  ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_DOC", "F_FECH_DOC", ucFiltro.DataValueType.StringType, False, , True, , , )
        ucFiltro1.AddFilter("Fecha de recepción  ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_RECEPCION", "F_FECH_RECEPCION", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Area                ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerArea(), "T_DSC_AREA", "T_DSC_AREA", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Paso actual         ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Supervisor(es)      ", ucFiltro.AcceptedControls.DropDownList, Supervisor.ObtenerVigentesPorSubproceso(-99, FiltroArea), "NOMBRE", "T_ID_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Inspector(es)       ", ucFiltro.AcceptedControls.DropDownList, Inspector.ObtenerVigentesPorSubproceso(-99, FiltroArea), "NOMBRE", "T_ID_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Proceso             ", ucFiltro.AcceptedControls.DropDownList, Proceso.ObtenerPcVigentes(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea), "T_DSC_DESCRIPCION", "I_ID_PROCESO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Subproceso          ", ucFiltro.AcceptedControls.DropDownList, Subproceso.ObtenerVigentesPorProceso(-99), "T_DSC_DESCRIPCION", "I_ID_SUBPROCESO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("PC Cumple?          ", ucFiltro.AcceptedControls.DropDownList, objCumplePC, "T_DSC_PC_CUMPLE", "I_ID_PC_CUMPLE", ucFiltro.DataValueType.IntegerType, , , , , , , 20)
        ucFiltro1.AddFilter("Resolucion          ", ucFiltro.AcceptedControls.DropDownList, objResolucionPC, "T_DSC_RESOLUCION", "I_ID_RESOLUCION", ucFiltro.DataValueType.StringType, , , , , , , 20)
        ucFiltro1.AddFilter("Estatus             ", ucFiltro.AcceptedControls.DropDownList, Estatus.ObtenerTodos(), "T_DSC_ESTATUS", "N_ID_ESTATUS", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Folio SISAN               ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_ID_FOLIO_SISAN", "T_ID_FOLIO_SISAN", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Fecha de Envío a Sanciones   ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_ENVIA_SANCIONES", "F_FECH_ENVIA_SANCIONES", ucFiltro.DataValueType.StringType, False, False, False, False, False, Date.Today.AddMonths(-6), 10, False)
        ucFiltro1.LoadDDL("BandejaPC.aspx")

    End Sub

    Private Sub btnPersonalizarColumnas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizarColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 8
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 8
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        'Dim usuario As New Entities.Usuario()
        'usuario = Session(Entities.Usuario.SessionID)
        'For index As Integer = 1 To sender.Columns.Count - 1
        '    If usuario.IdArea = 36 And sender.Columns(index).HeaderText = "Sub Entidad" Then
        '        Dim img As Image
        '        For index2 As Integer = 1 To e.Row.Cells.Count - 1
        '            img = e.Row.Cells(index2).FindControl("SubEnt")
        '            If Not IsNothing(img) Then
        '                img.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSubentidad, e.Row.RowIndex.ToString(), False))
        '            End If
        '        Next
        '    End If
        'Next

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Session("ID_FOLIO") = CInt(gvConsulta.DataKeys(index)("N_ID_FOLIO").ToString())
        Session("ID_FOLIO_SICOD") = CInt(gvConsulta.DataKeys(index)("N_ID_SICOD").ToString())
        Response.Redirect("~/PC/DetallePC.aspx", False)

    End Sub

    Protected Sub btnSubentidad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubentidad.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT [I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD] FROM [dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC] WHERE N_ID_FOLIO = " + gvConsulta.DataKeys(index)("N_ID_FOLIO").ToString())

        conexion.CerrarConexion()

        Dim subents As String = ""

        For Each item In data.Rows
            subents += "</br>" + item(0).ToString()
        Next

        If String.Equals(subents, "") Then
            subents = "Sin registro de subentidades."
        End If

        ViewState("subents") = subents

        Dim script As String = "<script type='text/javascript'> FolioSubent();</script>"

        ClientScript.RegisterStartupScript(Me.GetType(), "AlertBox", script)

    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim app As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns.Add("DESC_ENTIDAD")
        dt.Columns.Add("Fecha de Envío a Sanciones")
        dt.AcceptChanges()
        For index As Integer = 0 To dt.Rows.Count - 1
            dt(index)("DESC_ENTIDAD") = ConexionSICOD.ObtenerNombreEntidad(dt(index)("ID_ENTIDAD").ToString())
            dt(index)("Fecha de Envío a Sanciones") = ConexionSISAN.ObtenerFechaSancion(dt(index)("T_ID_FOLIO_SISAN").ToString())
        Next


        app.ExportaGridBandejaPC(dt, gvConsulta, "Bandeja PC", referencias)
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID)
        Dim valorx As Boolean = False
        Dim usua As String = 36



        Dim consulta As String = "WHERE PC.I_ID_ESTATUS > 0"


        If Not CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
            consulta += " AND (dbo.ExisteFolioSupervisor(PC.N_ID_FOLIO,'" & usuario.IdentificadorUsuario & "')='Si' OR dbo.ExisteFolioInspector(PC.N_ID_FOLIO,'" & usuario.IdentificadorUsuario & "')='Si') "
        End If

        If CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea <> 34 Then
            consulta += " AND PC.I_ID_AREA = " & usuario.IdArea
        End If



        For Each filtro In ucFiltro1.getFilterSelection

            If String.Equals(filtro.Split("=")(0), "CVE_ID_ENT") Then
                filtro = "ID_ENTIDAD=" + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0), "I_ID_PROCESO") Then
                filtro = "PC.I_ID_PROCESO=" + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0), "I_ID_SUBPROCESO") Then
                filtro = "PC.I_ID_SUBPROCESO=" + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0), "N_ID_PASO") Then
                filtro = "PC.N_ID_PASO = " + filtro.Split("=")(1)
            End If
            If String.Equals(filtro.Split("=")(0), "N_ID_ESTATUS") Then
                filtro = "PC.I_ID_ESTATUS=" + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0), "I_ID_PC_CUMPLE") Then
                filtro = "PC.I_ID_PC_CUMPLE = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0), "f_fech_doc") Then
                filtro = "f_fech_doc = " + filtro.Split("=")(1)
            End If
            If String.Equals(filtro.Split("=")(0), "T_ID_USUARIO") Then
                filtro = "D_SUPERVISORES = " + filtro.Split("=")(1)
            End If

            consulta += " AND " + filtro
        Next


        gvConsulta.DataSource = BandejaPC.ObtenerFoliosPC(consulta, usuario.IdentificadorUsuario)
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If

    End Sub

    Public Shared Function ObtenerFoliosPC(consulta As String, usuario As String) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT PC.N_ID_FOLIO, N_ID_SICOD, I_ID_FOLIO_SUPERVISAR,T_DSC_NUM_OFICIO,F_FECH_DOC,F_FECH_RECEPCION,AR.T_DSC_AREA,F_FECH_REGISTRO,F_FECH_VENC," &
                        " [I_ID_ESTATUS],STS.T_DSC_ESTATUS,PC.N_ID_PASO,PS.T_DSC_PASO,[N_ID_FOLIO_SISAN],[I_ID_FOLIO_SUPERVISAR],[ID_ENTIDAD],[B_IS_SUBENTIDAD]," &
                        "[I_NUM_PROGRAMA_CORRECION],PC.[I_ID_PROCESO],PR.T_DSC_DESCRIPCION as PR_DSC,PC.[I_ID_SUBPROCESO],SPR.T_DSC_DESCRIPCION as SPR_DSC,PC.[T_DSC_DESCRIPCION]" &
                        ",[T_NUM_INTERNO],[F_FECH_VENC],[N_ID_SUB_PERFIL],ISNULL(I_ID_PC_CUMPLE,0) as I_ID_PC_CUMPLE,I_ID_RESOLUCION, dbo.ObtenerSupervisores(PC.N_ID_FOLIO) as D_SUPERVISORES, ISNULL(PC.T_ID_FOLIO_SISAN,'Sin Folio') as T_ID_FOLIO_SISAN " &
                        ", dbo.ObtenerInspectores(PC.N_ID_FOLIO) as D_INSPECTORES, dbo.ExisteFolioSupervisor(PC.N_ID_FOLIO,'" & usuario & "') as E_SUPERVISORES, dbo.ExisteFolioInspector(PC.N_ID_FOLIO,'" & usuario & "') as E_INSPECTORES " &
                        "FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION] PC " &
                        "inner join [dbo].[BDS_C_GR_PROCESO] PR on PC.I_ID_PROCESO=PR.I_ID_PROCESO " &
                        "inner join [dbo].[BDS_C_GR_SUBPROCESO] SPR on PC.I_ID_PROCESO=SPR.I_ID_PROCESO and PC.I_ID_SUBPROCESO=SPR.I_ID_SUBPROCESO " &
                        "inner join [dbo].[BDS_C_PC_ESTATUS] STS on PC.N_ID_PASO=STS.N_ID_PASO and PC.I_ID_ESTATUS=STS.N_ID_ESTATUS " &
                        "inner join [dbo].[BDS_C_PC_PASOS] PS on PC.N_ID_PASO= PS.N_ID_PASO " &
                        "inner join [dbo].[BDS_C_GR_AREAS] AR on PC.I_ID_AREA=AR.I_ID_AREA " & consulta & " ORDER BY F_FECH_RECEPCION DESC")


        'data = conexion.ConsultarDT("SELECT PC.N_ID_FOLIO, N_ID_SICOD, I_ID_FOLIO_SUPERVISAR,T_DSC_NUM_OFICIO,F_FECH_DOC,F_FECH_RECEPCION,AR.T_DSC_AREA,F_FECH_REGISTRO,F_FECH_VENC," &
        '                " [I_ID_ESTATUS],STS.T_DSC_ESTATUS,PC.N_ID_PASO,PS.T_DSC_PASO,[N_ID_FOLIO_SISAN],[I_ID_FOLIO_SUPERVISAR],[ID_ENTIDAD],[B_IS_SUBENTIDAD]," &
        '                "[I_NUM_PROGRAMA_CORRECION],PC.[I_ID_PROCESO],PR.T_DSC_DESCRIPCION as PR_DSC,PC.[I_ID_SUBPROCESO],SPR.T_DSC_DESCRIPCION as SPR_DSC,PC.[T_DSC_DESCRIPCION]" &
        '                ",[T_NUM_INTERNO],[F_FECH_VENC],[N_ID_SUB_PERFIL],I_ID_PC_CUMPLE,I_ID_RESOLUCION, dbo.ObtenerSupervisores(PC.N_ID_FOLIO) as D_SUPERVISORES, ISNULL(PC.T_ID_FOLIO_SISAN,'Sin Folio') as T_ID_FOLIO_SISAN " &
        '                "dbo.ObtenerInspectores(PC.N_ID_FOLIO) as D_INSPECTORES, dbo.ExisteFolioSupervisor(PC.N_ID_FOLIO,'" & usuario & "') as E_SUPERVISORES, dbo.ExisteFolioInspector(PC.N_ID_FOLIO,'" & usuario & "') as E_INSPECTORES " &
        '                "FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION] PC " &
        '                "inner join [dbo].[BDS_C_GR_PROCESO] PR on PC.I_ID_PROCESO=PR.I_ID_PROCESO " &
        '                "inner join [dbo].[BDS_C_GR_SUBPROCESO] SPR on PC.I_ID_PROCESO=SPR.I_ID_PROCESO and PC.I_ID_SUBPROCESO=SPR.I_ID_SUBPROCESO " &
        '                "inner join [dbo].[BDS_C_PC_ESTATUS] STS on PC.N_ID_PASO=STS.N_ID_PASO and PC.I_ID_ESTATUS=STS.N_ID_ESTATUS " &
        '                "inner join [dbo].[BDS_C_PC_PASOS] PS on PC.N_ID_PASO= PS.N_ID_PASO " &
        '                "inner join [dbo].[BDS_C_GR_AREAS] AR on PC.I_ID_AREA=AR.I_ID_AREA " & consulta & " ORDER BY F_FECH_RECEPCION DESC")

        conexion.CerrarConexion()

        For i As Integer = 0 To data.Rows.Count - 1
            Dim row As DataRow = data.Rows(i)

            If row(data.Columns.IndexOf("I_ID_RESOLUCION")) Is DBNull.Value Then
                row(data.Columns.IndexOf("I_ID_RESOLUCION")) = 0
            End If
            If row(data.Columns.IndexOf("I_ID_RESOLUCION")).ToString.Length = 0 Then
                row(data.Columns.IndexOf("I_ID_RESOLUCION")) = 0
            End If
        Next

        Return data

    End Function

    Public Shared Function ObtenerFolios(consulta As String) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION] C JOIN [dbo].[BDS_C_PC_ESTATUS] E ON C.I_ID_ESTATUS = E.N_ID_ESTATUS " + consulta)
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerFoliosId(Folio As String) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION]  WHERE [N_ID_FOLIO]=" + Folio)
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerTipoDocumento() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT DISTINCT T_DSC_T_DOC FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION]")
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerEntidad() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT DISTINCT T_DSC_REMITENTE FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION]")
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerArea() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT DISTINCT T_DSC_AREA FROM [dbo].[BDS_C_GR_AREAS] WHERE I_ID_AREA IN (35,36,1)")
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerPasos() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT N_ID_PASO, T_DSC_PASO FROM [dbo].[BDS_C_PC_PASOS] WHERE B_FALG_VIGENTE = 1 ")
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerDiasVencimiento() As Integer
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE N_ID_PARAMETRO = '80'")
        conexion.CerrarConexion()

        Return Integer.Parse(data.Rows(0)("T_DSC_VALOR").ToString())

    End Function

    Public Shared Function ObtenerDiasVencimiento(Parametro As Integer) As Integer
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE N_ID_PARAMETRO = '" + Parametro.ToString() + "'")
        conexion.CerrarConexion()


        Return Integer.Parse(data.Rows(0)("T_DSC_VALOR").ToString())
    End Function

    Public Shared Function DiasHabiles(FechaRecepcion As Date, Dias As Integer) As String

        Dim DiasHab As Integer = 0
        Dim NewDate As Date = FechaRecepcion

        While DiasHab < Dias
            NewDate = CDate(NewDate).AddDays(1).ToString("dd/MM/yyyy")
            If Not (NewDate.DayOfWeek = DayOfWeek.Sunday Or NewDate.DayOfWeek = DayOfWeek.Saturday Or ConexionSICOD.IsFestivo(NewDate.ToString("yyyy-MM-dd")) = "Si") Then
                DiasHab = DiasHab + 1
            End If
        End While

        Return NewDate.ToString("dd/MM/yyyy")
    End Function

    Public Shared Function EliminarPC(Folio As Integer) As Integer
        Dim conexion As New Conexion.SQLServer()

        Dim result As Integer
        result = conexion.Ejecutar("UPDATE [dbo].[BDS_D_PC_PROGRAMA_CORRECCION] SET I_ID_ESTATUS = -1 WHERE N_ID_FOLIO = " + Folio.ToString())
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function ActualizarPC(ListaCampos As List(Of String), ListaValores As IList(Of Object), listCamposCondicion As List(Of String), listValoresCondicion As List(Of Object)) As Boolean
        Dim conexion As New Conexion.SQLServer()

        Dim result As Boolean
        result = conexion.Actualizar("[dbo].[BDS_D_PC_PROGRAMA_CORRECCION]", ListaCampos, ListaValores, listCamposCondicion, listValoresCondicion)
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function GuardarRel_Entidad_SubEntidad_PC(ListaCampos As List(Of String), ListaValores As IList(Of Object)) As Boolean

        Dim conexion As New Conexion.SQLServer()
        Dim result As Boolean

        result = conexion.Insertar("[dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC]", ListaCampos, ListaValores)
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function Actualiza_Entidad_SubEntidad_PC(ListaCampos As List(Of String), ListaValores As IList(Of Object), listCamposCondicion As List(Of String), listValoresCondicion As List(Of Object)) As Boolean

        Dim conexion As New Conexion.SQLServer()
        Dim result As Boolean

        result = conexion.Actualizar("[dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC]", ListaCampos, ListaValores, listCamposCondicion, listValoresCondicion)
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function EliminarSupervisores(Folio As Integer) As Integer
        'Elimina supervisor
        Dim conexion As New Conexion.SQLServer()
        Dim result As Integer
        result = conexion.Ejecutar("DELETE FROM [dbo].[BDS_R_PC_SUPERVISOR_PC] WHERE N_ID_FOLIO = " + Folio.ToString())
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function EliminarCheckList(Folio As Integer) As Integer
        'Elimina supervisor
        Dim conexion As New Conexion.SQLServer()
        Dim result As Integer
        result = conexion.Ejecutar("DELETE FROM [dbo].[BDS_R_PC_CHECKLIST] WHERE N_ID_FOLIO = " + Folio.ToString())
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function EliminarInspectores(Folio As Integer) As Integer
        'Elimina checklist
        Dim conexion As New Conexion.SQLServer()
        Dim result As Integer
        result = conexion.Ejecutar("DELETE FROM [dbo].[BDS_R_PC_INSPECTOR_PC] WHERE N_ID_FOLIO = " + Folio.ToString())
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function AsignarSupervidores(ListaCampos As List(Of String), ListaValores As IList(Of Object)) As Boolean

        Dim conexion As New Conexion.SQLServer()
        Dim result As Boolean
        result = conexion.Insertar("[dbo].[BDS_R_PC_SUPERVISOR_PC]", ListaCampos, ListaValores)
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function GuardarRel_PC_INSPECTORES(ListaCampos As List(Of String), ListaValores As IList(Of Object)) As Boolean

        Dim conexion As New Conexion.SQLServer()
        Dim result As Boolean
        result = conexion.Insertar("[dbo].[BDS_R_PC_INSPECTOR_PC]", ListaCampos, ListaValores)
        conexion.CerrarConexion()

        Return result
    End Function

    Public Shared Function GuardarRel_PC_CheckList(ListaCampos As List(Of String), ListaValores As IList(Of Object)) As Boolean

        Dim conexion As New Conexion.SQLServer()
        Dim result As Boolean
        result = conexion.Insertar("[dbo].[BDS_R_PC_CHECKLIST]", ListaCampos, ListaValores)
        conexion.CerrarConexion()

        Return result
    End Function


    Private Sub gvConsulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConsulta.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = TryCast(e.Row.DataItem, DataRowView)


            Dim FechaSancion As Label = Nothing
            FechaSancion = TryCast(e.Row.Cells(17).FindControl("F_FECH_ENVIA_SANCIONES"), Label)

            Dim resultado As String
            resultado = ConexionSISAN.ObtenerFechaSancion(e.Row.DataItem("T_ID_FOLIO_SISAN"))
            FechaSancion.Text = resultado
            '--------------------------------------
            ' Logo
            '--------------------------------------
            Dim img As Image = Nothing
            img = TryCast(e.Row.Cells(2).FindControl("logoImg"), Image)

            Dim lblE As Label = Nothing
            lblE = TryCast(e.Row.Cells(2).FindControl("lblEnt"), Label)


            'Dim tbl As String
            'tbl = ConexionSICOD.ObtenerNombreEntidad(rowView("ID_ENTIDAD").ToString())

            Dim dtEnt As DataTable = ConexionSICOD.ObtenerEntidadesAFOREArea(IdArea)
            Dim dvEnt As DataView = New DataView(dtEnt)

      dvEnt.RowFilter = "(CVE_ID_ENT = " + rowView("ID_ENTIDAD").ToString() + ")"

      If dvEnt.Count > 0 Then

        Dim strEnt As String = dvEnt(0)(3).ToString()

        If Split(strEnt, " ").Count > 1 Then
          If Split(strEnt, " ")(1) = "BANORTE" Then
            strEnt = Split(strEnt, " ")(1)
          Else
            strEnt = Split(strEnt, " ")(0)
          End If
        End If

        If Split(strEnt, "-").Count > 1 Then
          strEnt = Split(strEnt, "-")(1)
        Else
          strEnt = Split(strEnt, "-")(0)
        End If

        If strEnt = "CITIBANAMEX" Then
          strEnt = "BANAMEX"
        End If

        lblE.Text = strEnt

      End If



      If IO.File.Exists(System.IO.Path.GetTempPath() & "ide_" & Session.SessionID & "_" & rowView("ID_ENTIDAD").ToString() & ".gif") Then
                Try
                    img.ImageUrl = "UserControls/ImageHandlerEntidad.ashx?ide=" & Session.SessionID & "_" & rowView("ID_ENTIDAD").ToString() & "&ext=gif"
                Catch ex As Exception
                    img.Visible = False
                End Try
            Else
                img.Visible = False
            End If

        End If
    End Sub

    Private Sub CargarImagenes()

        For Each row As DataRow In ConexionSICOD.ObtenerEntidadesAFOREArea(IdArea).Rows
            If Not IsDBNull(row("LOGO_ENT")) Then
                Try
                    Dim a As System.Drawing.Image
                    a = System.Drawing.Image.FromStream(New System.IO.MemoryStream(DirectCast(row("LOGO_ENT"), Byte())))
                    a.Save(System.IO.Path.GetTempPath() & "ide_" & Session.SessionID & "_" & row("CVE_ID_ENT").ToString() & ".gif", System.Drawing.Imaging.ImageFormat.Gif)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End If
        Next

    End Sub
End Class