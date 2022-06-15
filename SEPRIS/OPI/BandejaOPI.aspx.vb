Imports System.Web.Configuration
Imports System.Net

Public Class BandejaOPI
    Inherits System.Web.UI.Page

    Public Property Mensaje As String
    Dim enc As New YourCompany.Utils.Encryption.Encryption64

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
            LLenaGridOPIS()
            CargarImagenes()

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 10
                PersonalizaColumnas.GridViewPersonalizar = gvConsultaOPI
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If
        End If
    End Sub

    Private Sub CargarFiltros()
        Dim usuario As New Entities.Usuario()
        Dim filtroArea As Integer = 0

        usuario = Session(Entities.Usuario.SessionID)

        ucFiltro1.resetSession()

        If Not CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea = 34 Then
            filtroArea = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        End If


        Dim objClasificacionOPI() = {New With {.T_DSC_CLASIFICACION = "Requerimiento de información"},
                           New With {.T_DSC_CLASIFICACION = "Oficio de Observaciones"},
                           New With {.T_DSC_CLASIFICACION = "Aviso de conocimiento"}}

        Dim objProcedeOPI() = {New With {.T_DSC_OPI_PROCEDE = "No procede", .B_PROCEDE = "False"},
                           New With {.T_DSC_OPI_PROCEDE = "Sí procede", .B_PROCEDE = "True"}}

        ucFiltro1.AddFilter("Folio            ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_ID_FOLIO", "T_ID_FOLIO", ucFiltro.DataValueType.StringType, , True)
        ucFiltro1.AddFilter("Fecha de incumplimiento", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_POSIBLE_INC", "F_FECH_POSIBLE_INC", ucFiltro.DataValueType.StringType, True)
        ucFiltro1.AddFilter("Clasificación       ", ucFiltro.AcceptedControls.DropDownList, objClasificacionOPI, "T_DSC_CLASIFICACION", "T_DSC_CLASIFICACION", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Entidad ", ucFiltro.AcceptedControls.DropDownList, ConexionSICOD.ObtenerEntidadesAFORE, "SIGLAS_ENT", "CVE_ID_ENT", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Paso actual  ", ucFiltro.AcceptedControls.DropDownList, ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Días transcurridos en el paso actual  ", ucFiltro.AcceptedControls.TextBox, Nothing, "DIAS_TRANSC", "DIAS_TRANSC", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Supervisor(es)              ", ucFiltro.AcceptedControls.DropDownList, ObtenerSupervisores, "NOMBRE", "T_ID_SUPERVISOR_ASIGNADO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Inspector(es)             ", ucFiltro.AcceptedControls.DropDownList, cInspectorOPI.ObtenerInspectores(usuario), "NOMBRE", "T_ID_INSPECTOR_ASIGNADO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Proceso             ", ucFiltro.AcceptedControls.DropDownList, ObtenerProcesosVigentes, "T_DSC_DESCRIPCION", "I_ID_PROCESO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Subproceso          ", ucFiltro.AcceptedControls.DropDownList, ObtenerTodosSubproceso, "T_DSC_DESCRIPCION", "I_ID_SUBPROCESO", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("¿Procede?         ", ucFiltro.AcceptedControls.DropDownList, objProcedeOPI, "T_DSC_OPI_PROCEDE", "B_PROCEDE", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Estatus          ", ucFiltro.AcceptedControls.DropDownList, ObtenerEstatus, "T_DSC_ESTATUS", "I_ID_ESTATUS", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Fecha de registro", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_REGISTRO", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, False, True, True, False, Date.Today.AddMonths(-6), 10, False)
        ucFiltro1.AddFilter("Días hábiles transcurridos desde la detección del posible incumplimiento ", ucFiltro.AcceptedControls.TextBox, Nothing, "DIAS_HABILES", "DIAS_HABILES", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Área         ", ucFiltro.AcceptedControls.DropDownList, ObtenerArea, "T_DSC_AREA", "I_ID_AREA", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Folio SISAN         ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_ID_FOLIO_SISAN", "T_ID_FOLIO_SISAN", ucFiltro.DataValueType.StringType, , True)
        ucFiltro1.AddFilter("Fecha de Envío a Sanciones", ucFiltro.AcceptedControls.Calendar, Nothing, "Fecha de Envío a Sanciones", "F_FECH_ENVIA_SANSIONES", ucFiltro.DataValueType.StringType, False, False, False, False, False, Date.Today.AddMonths(-6), 10, False)

        ucFiltro1.LoadDDL("BandejaOPI.aspx")

    End Sub

    Private Sub btnPersonalizarColumnas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizarColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 10
        PersonalizaColumnas.GridViewPersonalizar = gvConsultaOPI
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 10
        PersonalizaColumnas.GridViewPersonalizar = gvConsultaOPI
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsultaOPI.DataSource = gvConsultaOPI.DataSourceSession
        gvConsultaOPI.DataBind()
    End Sub

    Protected Sub gvConsultaOPI_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsultaOPI.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Session("I_ID_OPI") = CInt(gvConsultaOPI.DataKeys(index)("I_ID_OPI").ToString())
        Response.Redirect("~/OPI/DetalleOPI.aspx", False)

    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim app As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable


        dt = New DataTable
        dt.Reset()
        dt = TryCast(gvConsultaOPI.DataSourceSession, DataTable)


        If (dt.Columns.Contains("DESC_ENTIDAD") Or dt.Columns.Contains("F_FECH_ENVIA_SANCIONES")) Then
            app.ExportaGridBandejaOPI(dt, gvConsultaOPI, "Bandeja Oficios", referencias)

        Else
            dt.Columns.Add("DESC_ENTIDAD")
            dt.Columns.Add("F_FECH_ENVIA_SANCIONES")
            dt.AcceptChanges()
            For index As Integer = 0 To dt.Rows.Count - 1
                dt(index)("DESC_ENTIDAD") = ConexionSICOD.ObtenerNombreEntidad(dt(index)("I_ID_ENTIDAD").ToString())
                dt(index)("F_FECH_ENVIA_SANCIONES") = ConexionSISAN.ObtenerFechaSancion(dt(index)("T_ID_FOLIO_SISAN").ToString())
            Next
            'app.ExportaGrid(dt, gvConsultaOPI, "Bandeja de Oficios", referencias)
            app.ExportaGridBandejaOPI(dt, gvConsultaOPI, "Bandeja Oficios", referencias)
        End If



    End Sub

    Private Sub LLenaGridOPIS()
        Dim usuario As New Entities.Usuario()

        usuario = Session(Entities.Usuario.SessionID)

        Dim consulta As String = ""
        Dim consultaFechaSanciones As String = ""



        consulta = "WHERE 1=1 "
        consultaFechaSanciones = "1=1"

        If CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea <> 34 Then
            consulta += " AND opi.I_ID_AREA = " & usuario.IdArea
            consultaFechaSanciones += " AND opi.I_ID_AREA = " & usuario.IdArea
        End If

        For Each filtro In ucFiltro1.getFilterSelection
            If String.Equals(filtro.Split("=")(0).Trim(), "I_ID_ESTATUS") Then
                filtro = "opi.I_ID_ESTATUS = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "N_ID_PASO") Then
                filtro = "opi.N_ID_PASO = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "DIAS_TRANSC") Then
                filtro = "CONVERT(INT,DATEDIFF(d, ISNULL(opi.F_FECH_PASO_ACTUAL,GETDATE()), GETDATE())) = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim, "F_FECH_POSIBLE_INC") Then
                filtro = "opi.F_FECH_POSIBLE_INC = " + filtro.Split("=")(1).ToString()
            End If

            If String.Equals(filtro.Split("=")(0).Trim, "F_FECH_REGISTRO") Then
                filtro = "CONVERT(VARCHAR(10), opi.F_FECH_REGISTRO, 111) = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "DIAS_HABILES") Then
                filtro = "DATEDIFF(d,opi.F_FECH_POSIBLE_INC, GETDATE()) = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "I_ID_AREA") Then
                filtro = "opi.I_ID_AREA = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "I_ID_SUBPROCESO") Then
                filtro = "opi.I_ID_SUBPROCESO = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "T_ID_SUPERVISOR_ASIGNADO") Then
                filtro = "sa.T_ID_SUPERVISOR_ASIGNADO = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "T_ID_INSPECTOR_ASIGNADO") Then
                filtro = "i.T_ID_INSPECTOR_ASIGNADO = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "CVE_ID_ENT") Then
                filtro = "opi.I_ID_ENTIDAD = " + filtro.Split("=")(1)
            End If

            If String.Equals(filtro.Split("=")(0).Trim(), "I_ID_PROCESO") Then
                filtro = " opi.I_ID_PROCESO_POSIBLE_INC = " + filtro.Split("=")(1)
            End If

            consulta += " AND " + filtro
            consultaFechaSanciones += " AND " + filtro
        Next


        'If () Then

        'End If

        'If consulta.ToString().Contains("WHERE 1=1  AND F_FECH_ENVIA_SANSIONES") Then
        If consultaFechaSanciones.Contains("F_FECH_ENVIA_SANSIONES") Then
            Dim valor As String
            Dim dt As DataTable
            Dim dv As DataView
            Dim newText = consultaFechaSanciones.Replace("1=1 AND F_FECH_ENVIA_SANSIONES", "1=1 AND F_FECH_REGISTRO >= '" + Session("FechaInicio") + "' AND F_FECH_REGISTRO <= '" + Session("FechaFinal") + "'")


            valor = "WHERE 1=1" + " AND F_FECH_REGISTRO >= '" + Convert.ToDateTime(Session("FechaInicio")).ToString("yyyy/MM/dd") & " 12:00:00 am" + "' AND F_FECH_REGISTRO <= '" + Convert.ToDateTime(Session("FechaFinal")).ToString("yyyy/MM/dd") & " 11:59:59 pm" + "'"
            dt = ObtenerFoliosOPI(valor)

            'F_FECH_ENVIA_SANSIONES
            dt.Columns.Add("F_FECH_ENVIA_SANSIONES", System.Type.GetType("System.String"))

            'Recorrer La columna FOLIO SISAN'
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dr As DataRow = dt.Rows(i)
                dr("F_FECH_ENVIA_SANSIONES") = ConexionSISAN.ObtenerFechaSancion(dr("T_ID_FOLIO_SISAN"))
            Next


            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    dv = dt.DefaultView
                    dv.RowFilter = consultaFechaSanciones

                    Dim dtaux As DataTable = dv.ToTable


                    For i As Integer = 0 To dtaux.Rows.Count - 1
                        Dim dr As DataRow = dtaux.Rows(i)
                        If Not dr("F_FECH_ENVIA_SANSIONES") = "" Then
                            dr("F_FECH_ENVIA_SANSIONES") = Convert.ToDateTime(dr("F_FECH_ENVIA_SANSIONES")).ToString("dd/MM/yyyy")
                        End If
                    Next

                    dv = dtaux.DefaultView

                    gvConsultaOPI.DataSource = dv.Table
                    gvConsultaOPI.DataBind()
                Else
                    gvConsultaOPI.DataSource = Nothing
                    gvConsultaOPI.DataBind()
                    Noexisten.Visible = False
                    gvConsultaOPI.Visible = True
                End If

            End If
        Else
            gvConsultaOPI.DataSource = ObtenerFoliosOPI(consulta)
            gvConsultaOPI.DataBind()

        End If





        If gvConsultaOPI.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsultaOPI.Visible = False
        Else
            Noexisten.Visible = False
            gvConsultaOPI.Visible = True
        End If

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        Call LLenaGridOPIS()

    End Sub

    Public Function ObtenerFoliosOPI(consulta As String) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim consulta2 As String = ""
        Dim usuario As New Entities.Usuario()
        Dim strQuery As String

        usuario = Session(Entities.Usuario.SessionID)

        If usuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
            consulta2 = ""
        Else
            consulta2 = "And (sa.T_ID_SUPERVISOR_ASIGNADO = '" & usuario.IdentificadorUsuario &
                "' or i.T_ID_INSPECTOR_ASIGNADO = '" & usuario.IdentificadorUsuario & "') "
        End If

        strQuery = "SELECT DISTINCT opi.I_ID_ESTATUS, opi.I_ID_OPI, opi.T_ID_FOLIO, opi.I_ID_ENTIDAD, opi.F_FECH_POSIBLE_INC, a.T_DSC_AREA " &
                ", opi.F_FECH_REGISTRO, opi.T_DSC_CLASIFICACION, opi.N_ID_PASO " &
                ", CONVERT(INT,DATEDIFF(d, ISNULL(opi.F_FECH_PASO_ACTUAL,GETDATE()), GETDATE())) DIAS_TRANSC " &
                ", dbo.ObtenerSupervisoresOPI(opi.I_ID_OPI) as D_SUPERVISORES " &
                ", dbo.ObtenerInspectoresOPI(opi.I_ID_OPI) as D_INSPECTORES " &
                ", opi.I_ID_PROCESO_POSIBLE_INC , pr.T_DSC_DESCRIPCION as PR_DSC " &
                ", opi.I_ID_SUBPROCESO, s.T_DSC_DESCRIPCION AS T_DSC_SUBPROCESO " &
                ", CASE when opi.B_PROCEDE = 'True' THEN 'Procede' when opi.B_PROCEDE = 'False' THEN 'No Procede' else '' END as B_PROCEDE "
        strQuery = strQuery & ", dbo.DiasLaborales(convert(varchar(10),opi.F_FECH_POSIBLE_INC,101),convert(varchar(10),getdate(),101)) AS DIAS_HABILES "
        'strQuery = strQuery & ", DATEDIFF(d,opi.F_FECH_POSIBLE_INC,getdate()) AS DIAS_HABILES "
        'strQuery = strQuery & ", dbo.DiasLaborales(opi.F_FECH_POSIBLE_INC,getdate()) AS DIAS_HABILES "
        strQuery = strQuery & ", e.T_DSC_ESTATUS, ISNULL (opi.T_ID_FOLIO_SISAN, 'Sin Folio') AS T_ID_FOLIO_SISAN " &
                " FROM [dbo].[BDS_D_OPI_INCUMPLIMIENTO] opi " &
                " INNER JOIN [dbo].[BDS_C_GR_PROCESO] pr ON (opi.I_ID_PROCESO_POSIBLE_INC = pr.I_ID_PROCESO AND opi.I_ID_AREA = pr.I_ID_AREA) " &
                " left JOIN BDS_C_GR_AREAS a ON (opi.I_ID_AREA = a.I_ID_AREA) " &
                " left JOIN BDS_C_OPI_ESTATUS e ON (opi.I_ID_ESTATUS = e.I_ID_ESTATUS) " &
                " left JOIN BDS_C_GR_SUBPROCESO s ON (opi.I_ID_PROCESO_POSIBLE_INC = s.I_ID_PROCESO AND opi.I_ID_SUBPROCESO = s.I_ID_SUBPROCESO) " &
                " left JOIN BDS_D_OPI_SUPERVISORES_ASIGNADOS sa ON (opi.I_ID_OPI = sa.I_ID_OPI) " &
                " left JOIN BDS_D_OPI_INSPECTORES_ASIGNADOS i ON (opi.I_ID_OPI = i.I_ID_OPI) " & consulta & consulta2 & " ORDER BY  F_FECH_REGISTRO DESC"



        data = conexion.ConsultarDT(strQuery)

        conexion.CerrarConexion()

        Return data

    End Function

    Public Function ObtenerProcesosVigentes() As DataTable
        Dim usuario As New Entities.Usuario()

        usuario = Session(Entities.Usuario.SessionID)


        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim strQuery As String

        strQuery = "SELECT I_ID_PROCESO, T_DSC_DESCRIPCION FROM [dbo].[BDS_C_GR_PROCESO] " &
            "WHERE B_FLAG_VIGENTE = 1"

        If usuario.IdentificadorPerfilActual <> Constantes.PERFIL_ADM Then
            If usuario.IdArea = 35 Or usuario.IdArea = 36 Or usuario.IdArea = 1 Then
                strQuery = strQuery & " and I_ID_AREA = " & usuario.IdArea
            End If
        End If

        data = conexion.ConsultarDT(strQuery)

        conexion.CerrarConexion()

        Return data

    End Function

    Public Function ObtenerTodosSubproceso() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT DISTINCT I_ID_SUBPROCESO, T_DSC_DESCRIPCION FROM [dbo].[BDS_C_GR_SUBPROCESO] WHERE B_FLAG_VIGENTE = 1 ORDER BY T_DSC_DESCRIPCION")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Function ObtenerPasos() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT N_ID_PASO, T_DSC_PASO FROM [dbo].[BDS_C_OPI_PASOS] WHERE B_FLAG_VIGENTE = 1 ")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Function ObtenerEstatus() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_ESTATUS, I_ID_ESTATUS FROM [dbo].[BDS_C_OPI_ESTATUS] WHERE B_FALG_VIGENTE = 1")

        conexion.CerrarConexion()

        Return data

    End Function
    Public Function ObtenerTipoDocumento() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT DISTINCT T_DSC_T_DOC FROM [dbo].[BDS_D_OPI_INCUMPLIMIENTO]")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Function ObtenerEntidad() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT DISTINCT T_DSC_REMITENTE FROM [dbo].[BDS_D_OPI_INCUMPLIMIENTO]")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Function ObtenerArea() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim strQuery As String = ""
        Dim usuario As New Entities.Usuario()

        usuario = Session(Entities.Usuario.SessionID)

        strQuery = "SELECT DISTINCT I_ID_AREA, T_DSC_AREA FROM [dbo].[BDS_C_GR_AREAS]"

        If usuario.IdentificadorPerfilActual <> Constantes.PERFIL_ADM Then
            If usuario.IdArea = 35 Or usuario.IdArea = 36 Or usuario.IdArea = 1 Then
                strQuery = strQuery & " Where I_ID_AREA = " & usuario.IdArea
            End If
        Else
            strQuery = strQuery & "WHERE I_ID_AREA=35 OR I_ID_AREA=36 OR I_ID_AREA=1"
        End If


        data = conexion.ConsultarDT(strQuery)

        conexion.CerrarConexion()

        Return data

    End Function



    Private Function ObtenerSupervisores() As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet
        Dim strQuery As String = ""
        Dim usuario As New Entities.Usuario()

        usuario = Session(Entities.Usuario.SessionID)

        strQuery = " SELECT DISTINCT S.T_ID_USUARIO AS 'T_ID_SUPERVISOR_ASIGNADO' , CASE ISNULL(LEN(LTRIM(RTRIM(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX))),0) " &
                                         " WHEN 0 THEN S.T_ID_USUARIO ELSE T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX END NOMBRE " &
                                         " FROM [dbo].[BDS_C_GR_SUPERVISOR] S LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON S.T_ID_USUARIO = U.T_ID_USUARIO "

        'If usuario.IdentificadorPerfilActual <> Constantes.PERFIL_ADM Then
        If usuario.IdArea = 35 Or usuario.IdArea = 36 Or usuario.IdArea = 1 Then
            strQuery = strQuery & " Inner Join [dbo].[BDS_R_GR_USUARIO_PERFIL] USP ON USP.T_ID_USUARIO = S.T_ID_USUARIO "

        End If
        'End If

        strQuery = strQuery & " WHERE B_FLAG_VIGENTE = 1"


        'If usuario.IdentificadorPerfilActual <> Constantes.PERFIL_ADM Then
        If usuario.IdArea = 35 Or usuario.IdArea = 36 Or usuario.IdArea = 1 Then
            strQuery = strQuery & " And USP.I_ID_AREA = " & usuario.IdArea

        End If
        'End If



        data = conexion.ConsultarDS(strQuery)



        conexion.CerrarConexion()

        Return data

    End Function

    Public Function EliminarOPI(Folio As Integer) As Integer
        Dim conexion As New Conexion.SQLServer()
        Return conexion.Ejecutar("UPDATE [dbo].[BDS_D_OPI_INCUMPLIMIENTO] SET I_ID_ESTATUS = -1 WHERE I_ID_OPI = " + Folio.ToString())

    End Function

    Private Sub gvConsultaOPI_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConsultaOPI.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = TryCast(e.Row.DataItem, DataRowView)


            'Recorrer La columna FOLIO SISAN'
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


            Dim tbl As String
            tbl = ConexionSICOD.ObtenerNombreEntidad(rowView("I_ID_ENTIDAD").ToString())

            lblE.Text = tbl

            If IO.File.Exists(System.IO.Path.GetTempPath() & "ide_" & Session.SessionID & "_" & rowView("I_ID_ENTIDAD").ToString() & ".gif") Then
                Try
                    img.ImageUrl = "../PC/UserControls/ImageHandlerEntidad.ashx?ide=" & Session.SessionID & "_" & rowView("I_ID_ENTIDAD").ToString() & "&ext=gif"
                Catch ex As Exception
                    img.Visible = False
                End Try
            Else
                img.Visible = False
            End If

        End If
    End Sub

    Private Sub CargarImagenes()
        Dim dsEntidades As New DataSet

        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim vecTipoEntidades As New List(Of Integer)

        Dim usuarioActual As New Entities.Usuario()
        usuarioActual = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(7)
        vecTipoEntidades.Add(2)
        vecTipoEntidades.Add(3)
        vecTipoEntidades.Add(4)
        vecTipoEntidades.Add(17)

        Try
            If Not IsNothing(usuarioActual) Then
                Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
                Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
                Dim proxySICOD As New WR_SICOD.ws_SICOD
                proxySICOD.Credentials = credentials
                proxySICOD.ConnectionGroupName = "SEPRIS"
                Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

                For Each ent As Integer In vecTipoEntidades
                    dsEntidades = proxySICOD.GetEntidadesComplete(ent)
                    For Each row As DataRow In dsEntidades.Tables(0).Rows

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
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class