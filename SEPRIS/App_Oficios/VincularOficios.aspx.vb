Imports System.Data
Imports System.Web.Configuration
Imports Clases
Imports AccesoDatos
Imports System.IO
Imports LogicaNegocioSICOD
Imports SICOD.Generales
Imports System.Net

Public Class VincularOficios
    Inherits System.Web.UI.Page

    Public Enum TIPO_DOCUMENTO
        OFICIO_EXTERNO = 1
        DICTAMEN = 2
        ATENTA_NOTA = 3
        OFICIO_INTERNO = 4
    End Enum

    Public Enum ESTATUS
        ELABORACION = 1
        REVISION = 2
        FIRMA = 3
        NOTIFICADO = 4
        ESPERA = 5
        CONCLUIDO = 6
        CANCELADO = 7
        POR_DICTAMINAR = 9
        DICTAMINADO = 10
        ENSOBRETAR = 11
        NOTIFICAR = 12
        IMPRIMIR = 13
    End Enum

#Region "Constantes"

    Public Const SessionFiltros As String = "ssnVinculaOficios"
    Public Const SessionFiltrosInner As String = "ssnVinculaOficiosInner"

#End Region

#Region "Propiedades de la página"
    Public Property ID_UNIDAD_ADM() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM") = value
        End Set
    End Property

    Public Property ID_ANIO() As Integer
        Get
            Return CInt(ViewState("ID_ANIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO") = value
        End Set
    End Property

    Public Property ID_TIPO_DOCUMENTO() As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOCUMENTO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOCUMENTO") = value
        End Set
    End Property

    Public Property I_OFICIO_CONSECUTIVO() As Integer
        Get
            Return CInt(ViewState("I_OFICIO_CONSECUTIVO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("I_OFICIO_CONSECUTIVO") = value
        End Set
    End Property

    Public Property USUARIO() As String
        Get
            Return ViewState("Usuario").ToString
        End Get
        Set(ByVal value As String)
            ViewState("Usuario") = value
        End Set
    End Property

    Public Property ID_EXPEDIENTE() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE") = value
        End Set
    End Property

    Public Property NUMERO_OFICIO As String
        Get
            Return ViewState("NUMERO_OFICIO").ToString
        End Get
        Set(ByVal value As String)
            ViewState("NUMERO_OFICIO") = value
        End Set
    End Property

    Public Property ID_UNIDAD_ADM_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM_EDIT") = value
        End Set
    End Property

    Public Property ID_ANIO_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_ANIO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO_EDIT") = value
        End Set
    End Property

    Public Property ID_TIPO_DOCUMENTO_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOCUMENTO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOCUMENTO_EDIT") = value
        End Set
    End Property

    Public Property I_OFICIO_CONSECUTIVO_EDIT() As Integer
        Get
            Return CInt(ViewState("I_OFICIO_CONSECUTIVO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("I_OFICIO_CONSECUTIVO_EDIT") = value
        End Set
    End Property
#End Region

    Private Sub VincularOficios_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("Usuario") Is Nothing Then logOut()

        USUARIO = Session("Usuario").ToString

        '-----------------------------------------------
        ' Verificar Sesión y Perfil de usuario
        '-----------------------------------------------
        verificaSesion()
        verificaPerfil()

        If Not IsPostBack Then

            Dim Con As Conexion = Nothing

            Try

                '--------------------------------------------------------------------------------'
                ' ucFiltro
                '--------------------------------------------------------------------------------'
                Con = New Conexion
                Dim ds As New DataSet
                Dim dr As Odbc.OdbcDataReader = Nothing
                Dim sql As String = String.Empty

                Dim ObjDataAccess As New AccesoDatos.DataAccess
                ObjDataAccess.ConectarAplicaciones()

                '-----------------------------------
                ' *CATÁLOGOS PARA EL ucFILTRO*
                '-----------------------------------

                '-----------------------------------
                ' CLASIFICACIÓN
                '-----------------------------------
                sql =
                        "SELECT DISTINCT " & _
                        "T_CLASIFICACION, ID_CLASIFICACION " & _
                        "FROM " & Conexion.Owner & " BDA_CLASIFICACION_OFICIO  " & _
                        "WHERE VIG_FLAG = 1 " & _
                        "ORDER BY T_CLASIFICACION"

                Con.ConsultaAdapter(sql).Fill(ds, "CLASIFICACION")

                '-----------------------------------
                ' TIPO DE DOCUMENTO
                '-----------------------------------
                sql =
                        "SELECT " & _
                        "T_TIPO_DOCUMENTO, ID_TIPO_DOCUMENTO " & _
                        "FROM " & Conexion.Owner & " BDA_TIPO_DOCUMENTO " & _
                        "WHERE VIG_FLAG = 1 " & _
                        "ORDER BY T_TIPO_DOCUMENTO"
                Con.ConsultaAdapter(sql).Fill(ds, "TIPO_DOCUMENTO")

                '-----------------------------------
                ' ESTATUS
                '-----------------------------------
                sql =
                        "SELECT " & _
                        "ID_ESTATUS,T_ESTATUS " & _
                        "FROM " & Conexion.Owner & "BDA_ESTATUS_OFICIO " & _
                        "WHERE VIG_FLAG = 1 AND ID_ESTATUS NOT IN (6,7) " & _
                        "ORDER BY T_ESTATUS"
                Con.ConsultaAdapter(sql).Fill(ds, "ESTATUS")

                '-----------------------------------
                ' UNIDAD ADMINISTRATIVA (ÁREA)
                '-----------------------------------

                sql = "   SELECT DISTINCT ID_UNIDAD_ADM    " +
                        "   FROM [BDA_R_USUARIO_UNIDAD_ADM]  " +
                        "   WHERE VIG_FLAG = 1               " +
                        "   AND USUARIO = '" + USUARIO + "' "
                Dim unidadUsuario As Integer
                dr = Con.Consulta(sql)

                If dr.HasRows Then
                    While dr.Read
                        unidadUsuario = dr("ID_UNIDAD_ADM")
                        Exit While
                    End While
                    dr.Close()
                End If

                sql =
                          "   WITH Jerarquia AS                                                   " +
                          "   (                                                                   " +
                          "   SELECT UA.ID_UNIDAD_ADM, UA.ID_UNIDAD_ADM_DEP,                      " +
                          "      UA.VIG_FLAG                                                      " +
                          "   FROM BDA_C_UNIDAD_ADM UA                                            " +
                          "   WHERE UA.ID_UNIDAD_ADM =  " + unidadUsuario.ToString +
                          "                                                                       " +
                          "   UNION ALL                                                           " +
                          "                                                                       " +
                          "   SELECT UA2.ID_UNIDAD_ADM, UA2.ID_UNIDAD_ADM_DEP,                    " +
                          "       UA2.VIG_FLAG                                                    " +
                          "   FROM BDA_C_UNIDAD_ADM UA2                                           " +
                          "   JOIN Jerarquia On Jerarquia.ID_UNIDAD_ADM_DEP = UA2.ID_UNIDAD_ADM   " +
                          "   )                                                                   " +
                          "   SELECT DISTINCT ID_UNIDAD_ADM                                       " +
                          "   FROM Jerarquia                                                      " +
                          "   WHERE VIG_FLAG = 1"

                Dim dtTopUnidad As DataSet = Con.Datos(sql, False)
                Dim topUnidad As Integer
                If dtTopUnidad.Tables(0).Rows.Count > 0 Then
                    If dtTopUnidad.Tables(0).Rows.Count > 1 Then
                        topUnidad = dtTopUnidad.Tables(0)(1)("ID_UNIDAD_ADM")
                    Else
                        topUnidad = dtTopUnidad.Tables(0)(0)("ID_UNIDAD_ADM")
                    End If
                End If

                sql =
                        "   WITH Jerarquia AS                                                       " +
                        "   (                                                                       " +
                        "   SELECT UA.ID_UNIDAD_ADM, UA.ID_UNIDAD_ADM_DEP, UA.VIG_FLAG,             " +
                        "           UA.DSC_UNIDAD_ADM , UA.I_CODIGO_AREA,  UA.ID_T_UNIDAD_ADM       " +
                        "   FROM BDA_C_UNIDAD_ADM UA                                                " +
                        "   WHERE UA.ID_UNIDAD_ADM = " + topUnidad.ToString +
                        "                                                                           " +
                        "   UNION ALL                                                               " +
                        "                                                                           " +
                        "   SELECT UA2.ID_UNIDAD_ADM, UA2.ID_UNIDAD_ADM_DEP, UA2.VIG_FLAG,          " +
                        "           UA2.DSC_UNIDAD_ADM, UA2.I_CODIGO_AREA,  UA2.ID_T_UNIDAD_ADM     " +
                        "   FROM BDA_C_UNIDAD_ADM UA2                                               " +
                        "   INNER JOIN Jerarquia ON Jerarquia.ID_UNIDAD_ADM = UA2.ID_UNIDAD_ADM_DEP " +
                        "   )                                                                       " +
                        "   SELECT DISTINCT ID_UNIDAD_ADM, DSC_UNIDAD_ADM,                          " +
                        "       CAST(I_CODIGO_AREA as varchar) + '   -   ' + DSC_UNIDAD_ADM AS DSC_COMPOSITE, I_CODIGO_AREA " +
                        "   From Jerarquia                                                          " +
                        "   WHERE VIG_FLAG = 1 and  ID_T_UNIDAD_ADM = 2" +
                        "   ORDER BY I_CODIGO_AREA     "

                '"   SELECT DISTINCT ID_UNIDAD_ADM, DSC_UNIDAD_ADM                           " +
                '"   From Jerarquia                                                          " +
                '"   WHERE VIG_FLAG = 1                                                      " +
                '"   ORDER BY DSC_UNIDAD_ADM                                                 "

                Dim dtJerarquia As DataSet = Con.Datos(sql, False)
                dtJerarquia.Tables(0).TableName = "UNIDAD_ADM"
                ds.Tables.Add(dtJerarquia.Tables(0).Copy)

                '-----------------------------------
                ' ENTIDAD
                '-----------------------------------
                sql =
                        "SELECT ISNULL(T_ENTIDAD_CORTO, '') AS T_ENTIDAD_CORTO, ID_ENTIDAD " +
                        "FROM " & Conexion.Owner & " BDA_ENTIDAD " +
                        "WHERE VIG_FLAG= 1 " & _
                        "ORDER BY T_ENTIDAD_CORTO"
                Con.ConsultaAdapter(sql).Fill(ds, "ENTIDAD")

                '-----------------------------------
                ' DESTINATARIO
                '-----------------------------------
                sql =
                        "SELECT DISTINCT ISNULL(T_NOMBRE, '') + ' ' + ISNULL(T_APELLIDO_P, '') + ' ' + ISNULL(T_APELLIDO_M, '') AS DESTINATARIO, ID_PERSONA " & _
                        "FROM " & Conexion.Owner & " BDA_PERSONAL P JOIN BDA_OFICIO O ON P.ID_PERSONA=O.ID_DESTINATARIO " & _
                        "WHERE P.VIG_FLAG = 1 " & _
                        "ORDER BY DESTINATARIO"
                Con.ConsultaAdapter(sql).Fill(ds, "DESTINATARIO")

                '-----------------------------------
                ' USUARIO ALTA
                '-----------------------------------
                sql =
                        "SELECT DISTINCT U.NOMBRE + ' ' + U.APELLIDOS AS REGISTRO, U.USUARIO AS USUARIO_ALTA " +
                        "FROM BDS_USUARIO U " +
                        "LEFT JOIN BDA_OFICIO O ON U.USUARIO=O.USUARIO_ALTA " +
                        "WHERE U.VIG_FLAG = 1 " & _
                        "ORDER BY REGISTRO"
                Con.ConsultaAdapter(sql).Fill(ds, "REGISTRO")

                '-----------------------------------
                ' USUARIO ELABORÓ
                '-----------------------------------
                sql =
                        "SELECT DISTINCT U.NOMBRE + ' ' +U.APELLIDOS AS ELABORO, U.USUARIO AS USUARIO_ELABORO " +
                        "FROM " & Conexion.Owner & " BDS_USUARIO U " +
                        "LEFT JOIN BDA_OFICIO O ON U.USUARIO = O.USUARIO_ELABORO " +
                        "WHERE U.VIG_FLAG = 1 " & _
                        "ORDER BY ELABORO"
                Con.ConsultaAdapter(sql).Fill(ds, "ELABORO")

                '-----------------------------------
                ' AÑO
                '-----------------------------------
                sql =
                        "SELECT CICLO AS ID_ANIO " +
                        "FROM " & Conexion.Owner & " BDV_C_CICLO " +
                        "ORDER BY CICLO DESC"
                Con.ConsultaAdapter(sql).Fill(ds, "ANIO")

                '-----------------------------------
                ' AÑO
                '-----------------------------------
                'LogicaNegocioSICOD.BusinessRules.BDV_C_DIA_FESTIVO.ConsultarDiasFestivos(Today.Year)
                sql =
                            "   SELECT CICLO" +
                            "       ,MES" +
                            "       ,DIA" +
                            "       ,DIA_DSC " +
                            "   FROM [BDV_C_DIA_FESTIVO] " +
                            "    WHERE CICLO= " + Today.Year.ToString
                dr = Con.Consulta(sql)
                Dim listaAsuetosAnio As New List(Of Date)
                If dr.HasRows Then
                    While dr.Read
                        listaAsuetosAnio.Add(New Date(CInt(dr("CICLO")), CInt(dr("MES")), CInt(dr("DIA"))))
                    End While
                End If
                Dim initValueDate As Date = CalcularDiasHabiles(Date.Today, 5, listaAsuetosAnio, False)

                '--------------------------------------------------------------------------------'
                ' * ucFiltro *
                '--------------------------------------------------------------------------------'               
                ucFiltro1.resetSession()
                ucFiltro1.AddFilter("Área", ucFiltro.AcceptedControls.DropDownList, ds.Tables("UNIDAD_ADM"), "DSC_COMPOSITE", "ID_UNIDAD_ADM", ucFiltro.DataValueType.IntegerType, False, False, , True, True, unidadUsuario)
                ucFiltro1.AddFilter("Año", ucFiltro.AcceptedControls.DropDownList, ds.Tables("ANIO"), "ID_ANIO", "ID_ANIO", ucFiltro.DataValueType.IntegerType, False, False, , True, True, Today.Year.ToString)
                ucFiltro1.AddFilter("Tipo de documento", ucFiltro.AcceptedControls.DropDownList, ds.Tables("TIPO_DOCUMENTO"), "T_TIPO_DOCUMENTO", "ID_TIPO_DOCUMENTO", ucFiltro.DataValueType.IntegerType, False, False, , True, True, 1)
                ucFiltro1.AddFilter("Fecha de documento", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECHA_OFICIO", ucFiltro.DataValueType.StringType, False, False, False, True, False, initValueDate)
                ucFiltro1.AddFilter("Destinatario", ucFiltro.AcceptedControls.DropDownList, ds.Tables("DESTINATARIO"), "DESTINATARIO", "ID_PERSONA", ucFiltro.DataValueType.IntegerType, False, False)
                ucFiltro1.AddFilter("Clasificación", ucFiltro.AcceptedControls.DropDownList, ds.Tables("CLASIFICACION"), "T_CLASIFICACION", "ID_CLASIFICACION", ucFiltro.DataValueType.IntegerType, False, False)

                ucFiltro1.AddFilter("Estatus", ucFiltro.AcceptedControls.DropDownList, ds.Tables("ESTATUS"), "T_ESTATUS", "ID_ESTATUS", ucFiltro.DataValueType.IntegerType, False, False)

                ucFiltro1.AddFilter("Entidad", ucFiltro.AcceptedControls.DropDownList, ds.Tables("ENTIDAD"), "T_ENTIDAD_CORTO", "ID_ENTIDAD", ucFiltro.DataValueType.IntegerType, False, False)
                ucFiltro1.AddFilter("Fecha de acuse", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECHA_ACUSE", ucFiltro.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Fecha de recepción", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECHA_RECEPCION", ucFiltro.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Fecha de vencimiento", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECHA_VENCIMIENTO", ucFiltro.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Número de Documento", ucFiltro.AcceptedControls.TextBox, Nothing, "", "I_OFICIO_CONSECUTIVO", ucFiltro.DataValueType.IntegerType, False, False, True)
                ucFiltro1.AddFilter("Asunto", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_ASUNTO", ucFiltro.DataValueType.StringType, False, True, False)
                ucFiltro1.AddFilter("Registró", ucFiltro.AcceptedControls.DropDownList, ds.Tables("REGISTRO"), "REGISTRO", "USUARIO_ALTA", ucFiltro.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Elaboró", ucFiltro.AcceptedControls.DropDownList, ds.Tables("ELABORO"), "ELABORO", "USUARIO_ELABORO", ucFiltro.DataValueType.StringType, False, False)

                ucFiltro1.SelectionButton = Me.btnFiltrar.ClientID

                ds.Tables.Clear()
                ds = Nothing

            Catch ex As Exception
            Finally
                If Con IsNot Nothing Then Con.Cerrar()
                Con = Nothing
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            chkSoloMios.Checked = True

            ucFiltro1.LoadDDL(SessionFiltros)
            CargaValoresFiltroInner()


            '-----------------------------------------------
            ' Trae variables de sesión de bandeja y establece propiedades de la página en el Viewstate
            '-----------------------------------------------
            ID_UNIDAD_ADM = CInt(Session("ID_UNIDAD_ADM"))
            ID_ANIO = CInt(Session("ID_ANIO"))
            ID_TIPO_DOCUMENTO = CInt(Session("ID_TIPO_DOCUMENTO"))
            I_OFICIO_CONSECUTIVO = CInt(Session("I_OFICIO_CONSECUTIVO"))
            ID_EXPEDIENTE = CInt(Session("ID_EXPEDIENTE"))

            NUMERO_OFICIO = Session("NUMERO_OFICIO").ToString

            lblTitulo.Text = "Asociar Oficios a <strong>" & NUMERO_OFICIO & "</strong>"

            Filtraje()
        Else
            If Not Session("_DataSource") IsNot Nothing Then
                Dim dt As DataTable = Session("_DataSource")

            End If

        End If

    End Sub

    Private Sub Filtraje()
        Dim con As New Conexion
        Dim dtb As New DataTable

        Try

            Dim strWhere As String = " 1=1"
            Dim hasUnidadAdm As Boolean = False
            Dim hasRangoConsecutivo As Boolean = False
            Dim list As List(Of String) = ucFiltro1.getFilterSelection
            Dim FiltroParaSesionInner As New Dictionary(Of String, String)

            For Each listItem As String In list

                If listItem.Contains("ID_CLASIFICACION=") Then
                    strWhere += " AND  CL." + listItem

                ElseIf listItem.Contains("ID_TIPO_DOCUMENTO=") Then
                    strWhere += " AND T." + listItem

                ElseIf listItem.Contains("ID_ESTATUS=") Then
                    strWhere += " AND E." + listItem

                ElseIf listItem.Contains("ID_UNIDAD_ADM=") Then
                    strWhere += " AND A." + listItem
                    hasUnidadAdm = True
                ElseIf listItem.Contains("ID_ENTIDAD=") Then
                    strWhere += " AND ET." + listItem

                ElseIf listItem.Contains("ID_PERSONA=") Then
                    strWhere += " AND P." + listItem

                Else
                    strWhere += " AND O." + listItem
                    If listItem.Contains("I_OFICIO_CONSECUTIVO") Then
                        hasRangoConsecutivo = True
                    End If

                End If
            Next

            If hasRangoConsecutivo And Not hasUnidadAdm Then
                Throw New SystemException("Debe filtrar por Área Administrativa para aplicar el rango")
            End If

            Dim injectQuery As String = String.Empty
            If chkSoloMios.Checked Then
                injectQuery =
                                " INNER JOIN ( " +
                                " SELECT DISTINCT O_INNER.ID_AREA_OFICIO, O_INNER.ID_ANIO, O_INNER.I_OFICIO_CONSECUTIVO, O_INNER.ID_TIPO_DOCUMENTO " +
                                " FROM BDA_OFICIO O_INNER " +
                                " LEFT JOIN BDA_FIRMA F ON F.ID_TIPO_DOCUMENTO = O_INNER.ID_TIPO_DOCUMENTO AND F.ID_ANIO = O_INNER.ID_ANIO AND F.ID_AREA_OFICIO = O_INNER.ID_AREA_OFICIO AND F.I_OFICIO_CONSECUTIVO = O_INNER.I_OFICIO_CONSECUTIVO " +
                                " WHERE F.USUARIO = '" + Session("Usuario").ToString + "' " +
                                "                UNION " +
                                " SELECT DISTINCT O_UNION.ID_AREA_OFICIO, O_UNION.ID_ANIO, O_UNION.I_OFICIO_CONSECUTIVO, O_UNION.ID_TIPO_DOCUMENTO " +
                                " FROM BDA_OFICIO O_UNION " +
                                " WHERE USUARIO_ALTA = '" + Session("Usuario").ToString + "' OR USUARIO_ELABORO = '" + Session("Usuario").ToString + "') AS O_UNION2 " +
                                " ON  O.ID_AREA_OFICIO = O_UNION2.ID_AREA_OFICIO AND O.ID_ANIO = O_UNION2.ID_ANIO AND O.I_OFICIO_CONSECUTIVO = O_UNION2.I_OFICIO_CONSECUTIVO AND O.ID_TIPO_DOCUMENTO = O_UNION2.ID_TIPO_DOCUMENTO "
            End If

            Dim MAX_RESULTS As String = WebConfigurationManager.AppSettings("MAX_RESULTADOS_BANDEJA")

            Dim sql As String = String.Empty

            sql = "SELECT TOP " + MAX_RESULTS + " A.DSC_UNIDAD_ADM " +
                    " ,A.ID_UNIDAD_ADM " +
                    " ,O.T_OFICIO_NUMERO " +
                    " ,O.T_ASUNTO " +
                    " ,UE.USUARIO AS ELABORO " +
                    " ,UA.USUARIO AS REGISTRO " +
                    " ,T_TIPO_DOCUMENTO " +
                    " ,(CASE WHEN (P.ID_PERSONA IS NULL) THEN ISNULL(F.T_FUNCION, '') " +
                    " ELSE (ISNULL(P.T_NOMBRE,'')  + ' ' + ISNULL(P.T_APELLIDO_P,'') + ' ' + ISNULL(P.T_APELLIDO_M,'')) END) AS DESTINATARIO " +
                    " ,E.T_ESTATUS " +
                    " ,O.ID_ESTATUS " +
                    " ,CONVERT(VARCHAR(10), O.F_FECHA_RECEPCION,103) AS F_FECHA_RECEPCION " +
                    " ,CONVERT(VARCHAR(10), O.F_FECHA_OFICIO,103) AS F_FECHA_OFICIO " +
                    " ,CONVERT(VARCHAR(10), O.F_FECHA_ACUSE,103) AS F_FECHA_ACUSE " +
                    " ,CONVERT(VARCHAR(10), O.F_FECHA_VENCIMIENTO,103) AS F_FECHA_VENCIMIENTO " +
                    " ,O.DICTAMINADO_FLAG " +
                    " ,O.NOTIF_ELECTRONICA_FLAG " +
                    " ,ET.T_ENTIDAD_CORTO " +
                    " ,O.I_OFICIO_CONSECUTIVO " +
                    " ,O.ID_ANIO " +
                    " ,CL.T_CLASIFICACION " +
                    " ,O.ID_TIPO_DOCUMENTO " +
                    " ,O.[T_HYP_ARCHIVOSCAN]" +
                    " ,O.[T_HYP_ARCHIVOWORD]" +
                    " ,O.[T_HYP_CEDULAPDF]" +
                    " ,O.[T_HYP_FIRMADIGITAL]" +
                    " ,O.[T_CEDULADIGITAL]" +
                    " ,O.[T_HYP_RESPUESTAOFICIO]" +
                    " ,O.[T_HYP_ACUSERESPUESTA]" +
                    " ,O.[T_HYP_EXPEDIENTE]" +
                    " ,O.[T_ANEXO_UNO]" +
                    " ,O.[T_ANEXO_DOS]" +
                    "FROM BDA_OFICIO O " +
                    injectQuery +
                    " INNER JOIN (SELECT DISTINCT ID_UNIDAD_ADM, DSC_UNIDAD_ADM FROM BDA_C_UNIDAD_ADM) A ON A.ID_UNIDAD_ADM = O.ID_AREA_OFICIO " +
                    " LEFT OUTER JOIN BDS_USUARIO UE ON UE.USUARIO = O.USUARIO_ELABORO " +
                    " LEFT OUTER JOIN BDS_USUARIO UA ON UA.USUARIO = O.USUARIO_ALTA " +
                    " LEFT OUTER JOIN BDA_TIPO_DOCUMENTO T ON T.ID_TIPO_DOCUMENTO = O.ID_TIPO_DOCUMENTO " +
                    " LEFT OUTER JOIN BDA_PERSONAL P ON P.ID_PERSONA=O.ID_DESTINATARIO LEFT OUTER JOIN BDA_ESTATUS_OFICIO E ON E.ID_ESTATUS = O.ID_ESTATUS " +
                    " LEFT OUTER JOIN BDA_ENTIDAD ET ON ET.ID_TIPO_ENTIDAD= O.ID_ENTIDAD_TIPO AND P.ID_ENTIDAD = ET.ID_ENTIDAD " +
                    " LEFT OUTER JOIN BDA_CLASIFICACION_OFICIO CL ON CL.ID_CLASIFICACION = O.ID_CLASIFICACION " +
                    " LEFT OUTER JOIN BDA_FUNCION F ON O.ID_PUESTO_DESTINATARIO = f.ID_FUNCION " +
                    " WHERE " + strWhere

            '-----------------------------------------
            'Asegurarse de quitar "cancelados/terminados" de los resultados en base a la selección del radiobuttonlist
            '-----------------------------------------

            If rblEstatusOficio.SelectedValue = "1" Then
                sql += " AND O.ID_ESTATUS NOT IN (" & ESTATUS.CONCLUIDO & "," & ESTATUS.CANCELADO & ") "
            Else
                sql += " AND O.ID_ESTATUS IN (" & ESTATUS.CONCLUIDO & "," & ESTATUS.CANCELADO & ") "
            End If

            sql += " ORDER BY O.F_FECHA_OFICIO DESC, O.I_OFICIO_CONSECUTIVO DESC"

            FiltroParaSesionInner.Add("chkSoloMios", IIf(chkSoloMios.Checked, "1", "0"))
            FiltroParaSesionInner.Add("rblEstatusOficio", rblEstatusOficio.SelectedValue)

            Session(SessionFiltrosInner) = Nothing
            Session(SessionFiltrosInner) = FiltroParaSesionInner

            con.ConsultaAdapter(sql).Fill(dtb)
            gvVincularOficios.DataSource = dtb
            gvVincularOficios.DataBind()

            If dtb.Rows.Count = 0 Then
                btnSeleccionar.Visible = False
                btnSeleccionar.Enabled = False
                pnlImagenNoExisten.Style.Remove("display")
                pnlFooter.Style.Add("display", "none")

            Else
                btnSeleccionar.Visible = True
                btnSeleccionar.Enabled = True
                pnlImagenNoExisten.Style.Add("display", "none")
                pnlFooter.Style.Remove("display")
            End If

            '---------------------------------------------------
            '
            '---------------------------------------------------
            Imagen_procesando.Style.Add("display", "none")
            GRID.Style.Remove("display")

        Catch ex As SystemException
            ModalMensaje(ex.Message)
        End Try
    End Sub

#Region "Mensaje Modal"

    Private Sub ModalMensaje(ByVal Mensaje As String)
        lblErroresTitulo.Text = "INFORMACIÓN"
        lblErroresPopup.Text = "<ul><li>" & Mensaje & "</li></ul>"
        lblErroresPopup.Style.Add("display", "block")
        ModalPopupExtenderErrores.Show()
        up2.Update()
    End Sub

#End Region

#Region "Verificar Sesión y perfil de Usuario"
    Private Sub verificaSesion()
        Dim logout As Boolean = False
        Dim Sesion As Seguridad = Nothing
        Try
            Sesion = New Seguridad
            'Verifica la sesion de usuario
            Select Case Sesion.ContinuarSesionAD()
                Case -1
                    logout = True
                Case 0, 3
                    logout = True
            End Select
        Catch ex As Exception
            EscribirError(ex, "verificaSesion")
        Finally
            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub

    Private Sub verificaPerfil()
        Dim logout As Boolean = False
        Dim Perfil As Perfil = Nothing
        Try
            Perfil = New Perfil
            'Verifica que el usuario este autorizado para ver esta página
            If Not Perfil.Autorizado("App_Oficios/Bandeja.aspx") Then
                logout = True
            End If

        Catch ex As Exception
            EscribirError(ex, "verificaPerfil")
        Finally
            If Not Perfil Is Nothing Then
                Perfil.CerrarCon()
                Perfil = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub

    Private Sub logOut()
        If Request.Browser.EcmaScriptVersion.Major >= 1 Then
            Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
            Response.End()
        Else
            Response.Redirect("~/logout.aspx")
        End If
    End Sub

    Private Function verificaUsuario() As Boolean

        'para probar otro usuario, deshabilitar siguiente línea
        'USUARIO = "fcarmona"
        Return BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, USUARIO)

    End Function

#End Region

    Private Function CalcularDiasHabiles(ByVal fechaInicio As Date, ByVal numeroDias As Integer, ByVal listaAsuetos As List(Of Date), ByVal agregarDias As Boolean) As Date

        Dim countAllDays As Integer
        If agregarDias Then
            countAllDays = -1
        Else
            countAllDays = +1
        End If

        Dim countBusinessDays As Integer = 0

        Do
            If agregarDias Then
                countAllDays += 1
            Else
                countAllDays -= 1
            End If


            If fechaInicio.AddDays(countAllDays).DayOfWeek <> DayOfWeek.Saturday _
                               AndAlso fechaInicio.AddDays(countAllDays).DayOfWeek <> DayOfWeek.Sunday _
                                       AndAlso Not listaAsuetos.Contains(fechaInicio.AddDays(countAllDays)) Then

                countBusinessDays += 1

            End If

        Loop While countBusinessDays <= numeroDias

        Return fechaInicio.AddDays(countAllDays)

    End Function

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFiltrar.Click
        Filtraje()
    End Sub

    Protected Sub btnSeleccionar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSeleccionar.Click

        Dim pIdAnio As Integer = 0
        Dim pIdArea As Integer = 0
        Dim pIdTipoDocumento As Integer = 0
        Dim pConsecutivo As Integer = 0
        Dim hasChecked As Boolean = False

        Dim Campos As String
        Dim Valores As String
        Dim Sesion As Seguridad = Nothing
        Dim Con As Conexion = Nothing
        Dim tran As Odbc.OdbcTransaction = Nothing
        Dim Dr As Odbc.OdbcDataReader = Nothing

        Dim fechaVencimientoValidacion As String = "NULL"

        Try

            Sesion = New Seguridad

            Con = New Conexion()

            Sesion.BitacoraInicia("Vincula oficios ", Con)
            tran = Con.BeginTran()


            If gvVincularOficios.Rows.Count = 0 Then Throw New ApplicationException("Seleccione un registro")


            ''*********************************************************************************************
            '' VERIFICAMOS SI EL OFICIO TIENE EXPEDIENTE
            Dim Id_Expediente_Ext As Integer = 0
            Dim Id_Expediente_Ext_current As Integer = 0
            Dim Id_Expediente_Ext_old As Integer = 0

            Dim _dt1 As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByOFICIO(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ID_ANIO, I_OFICIO_CONSECUTIVO)
            If _dt1.Rows.Count > 0 Then Id_Expediente_Ext = Convert.ToInt32(_dt1.Rows(0)("ID_EXPEDIENTE").ToString())
            ''*********************************************************************************************


            For Each item As GridViewRow In gvVincularOficios.Rows
                Dim chb As CheckBox = TryCast(item.FindControl("chSeleccionado"), CheckBox)
                If chb.Checked Then
                    hasChecked = True
                    pIdAnio = CInt(TryCast(item.Cells(12).FindControl("hf_ID_ANIO"), HiddenField).Value)
                    pIdArea = CInt(TryCast(item.Cells(13).FindControl("hf_ID_UNIDAD_ADM"), HiddenField).Value)
                    pIdTipoDocumento = CInt(TryCast(item.Cells(14).FindControl("hf_ID_TIPO_DOCUMENTO"), HiddenField).Value)
                    pConsecutivo = CInt(TryCast(item.Cells(15).FindControl("hf_I_OFICIO_CONSECUTIVO"), HiddenField).Value)

                    '------------------------------------------
                    ' Verificar que no tenga otros relacionados este mismo.
                    '------------------------------------------
                    Dim pIdExpediente = BusinessRules.BDA_R_OFICIOS.ConsultarExpedienteOficio(pIdAnio, pIdArea, pIdTipoDocumento, pConsecutivo)
                    Dim dt As DataTable = BusinessRules.BDA_R_OFICIOS.ConsultarOficiosRelacionados(pIdExpediente)
                    If dt.Rows.Count > 1 Then
                        Throw New ApplicationException("El oficio " & item.Cells(2).Text + " ya está relacionado a otro expediente, no se pudo completar la operación")
                    End If


                    Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByOFICIO(pIdArea, pIdTipoDocumento, pIdAnio, pConsecutivo)
                    If _dt.Rows.Count > 0 Then
                        Id_Expediente_Ext_current = Convert.ToInt32(_dt.Rows(0)("ID_EXPEDIENTE").ToString())
                        If Id_Expediente_Ext_old = 0 Then Id_Expediente_Ext_old = Id_Expediente_Ext_current
                    End If

                    If Id_Expediente_Ext_old <> Id_Expediente_Ext_current Then
                        Throw New ApplicationException("Los Folios seleccionados pertenecen a expedientes diferentes ")
                    End If


                End If
            Next


            If Id_Expediente_Ext > 0 And Id_Expediente_Ext_current > 0 And Id_Expediente_Ext <> Id_Expediente_Ext_current Then
                Throw New ApplicationException("El oficio y los folios pertenecen a expedientes diferentes ")
            ElseIf Id_Expediente_Ext = 0 And Id_Expediente_Ext_current > 0 Then

                Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO.ToString, _
                                                                  ID_UNIDAD_ADM.ToString, _
                                                                  ID_TIPO_DOCUMENTO.ToString, _
                                                                  I_OFICIO_CONSECUTIVO.ToString)

                fechaVencimientoValidacion = "NULL"
                If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                    fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                End If

                Id_Expediente_Ext = Id_Expediente_Ext_current
                '' guardamos oficio en el expediente
                Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                Valores = Id_Expediente_Ext.ToString & "," & ID_UNIDAD_ADM.ToString & "," & ID_TIPO_DOCUMENTO.ToString & "," & ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If


                'GUARDAMOS EN BITACORA PARA OFICIO
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = ID_UNIDAD_ADM.ToString & "," & _
                    ID_TIPO_DOCUMENTO.ToString & "," & _
                    ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ",'" & _
                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & Id_Expediente_Ext.ToString() & "'," & fechaVencimientoValidacion

                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If




            ElseIf Id_Expediente_Ext = 0 Then
                '' creamos nuevo expediente (nadie tiene expediente)
                Dr = Con.Consulta("SELECT ISNULL(MAX(ID_EXPEDIENTE),0)+1 AS ID_EXPEDIENTE FROM " & Conexion.Owner & "BDA_C_EXPEDIENTE", tran)
                Dr.Read()
                Id_Expediente_Ext = Convert.ToInt32(Dr.Item("ID_EXPEDIENTE"))
                Dr.Close()

                Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO.ToString, _
                                                                  ID_UNIDAD_ADM.ToString, _
                                                                  ID_TIPO_DOCUMENTO.ToString, _
                                                                  I_OFICIO_CONSECUTIVO.ToString)

                fechaVencimientoValidacion = "NULL"
                If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                    fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                End If

                Campos = "ID_EXPEDIENTE,DSC_EXPEDIENTE,USUARIO_CREACION,VIG_FLAG,FECH_INI_VIG"
                Valores = Id_Expediente_Ext.ToString & ", '', '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_C_EXPEDIENTE", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo crearse el expediente ")
                End If

                Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                Valores = Id_Expediente_Ext.ToString & "," & ID_UNIDAD_ADM.ToString & "," & ID_TIPO_DOCUMENTO.ToString & "," & ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If

                'GUARDAMOS EN BITACORA PARA OFICIO
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = ID_UNIDAD_ADM.ToString & "," & _
                    ID_TIPO_DOCUMENTO.ToString & "," & _
                    ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ",'" & _
                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & Id_Expediente_Ext.ToString() & "'," & fechaVencimientoValidacion

                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If

            End If


            For Each item As GridViewRow In gvVincularOficios.Rows

                Dim chb As CheckBox = TryCast(item.FindControl("chSeleccionado"), CheckBox)
                If chb.Checked Then

                    pIdAnio = CInt(TryCast(item.Cells(12).FindControl("hf_ID_ANIO"), HiddenField).Value)
                    pIdArea = CInt(TryCast(item.Cells(13).FindControl("hf_ID_UNIDAD_ADM"), HiddenField).Value)
                    pIdTipoDocumento = CInt(TryCast(item.Cells(14).FindControl("hf_ID_TIPO_DOCUMENTO"), HiddenField).Value)
                    pConsecutivo = CInt(TryCast(item.Cells(15).FindControl("hf_I_OFICIO_CONSECUTIVO"), HiddenField).Value)

                    Dim objROficio As New LogicaNegocioSICOD.Entities.BDA_R_OFICIOS
                    objROficio.ID_EXPEDIENTE = ID_EXPEDIENTE
                    objROficio.ID_ANIO = pIdAnio
                    objROficio.ID_AREA_OFICIO = pIdArea
                    objROficio.ID_TIPO_DOCUMENTO = pIdTipoDocumento
                    objROficio.I_OFICIO_CONSECUTIVO = pConsecutivo
                    objROficio.USUARIO_ASOCIO = USUARIO
                    objROficio.INICIAL_FLAG = 0
                    If Not BusinessRules.BDA_R_OFICIOS.Actualizar(objROficio) Then Throw New ApplicationException("Error vinculando oficios")


                    'If LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByKey(Id_Expediente_Ext, objROficio.ID_AREA_OFICIO, objROficio.ID_TIPO_DOCUMENTO, _
                    '                                                                objROficio.ID_ANIO, objROficio.I_OFICIO_CONSECUTIVO).Rows.Count = 0 Then
                    If Not Con.BusquedaReg("BDA_R_EXPEDIENTE_OFICIO", "VIG_FLAG = 1 and ID_EXPEDIENTE = " & Id_Expediente_Ext.ToString & _
                                    " AND ID_AREA_OFICIO = " & objROficio.ID_AREA_OFICIO.ToString() & _
                                    " AND ID_TIPO_DOCUMENTO = " & objROficio.ID_TIPO_DOCUMENTO.ToString() & _
                                    " AND ID_ANIO =  " & objROficio.ID_ANIO.ToString() & _
                                    " AND I_OFICIO_CONSECUTIVO =  " & objROficio.I_OFICIO_CONSECUTIVO.ToString(), tran) Then


                        Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(objROficio.ID_ANIO.ToString, _
                                                                          objROficio.ID_AREA_OFICIO.ToString, _
                                                                          objROficio.ID_TIPO_DOCUMENTO.ToString, _
                                                                          objROficio.I_OFICIO_CONSECUTIVO.ToString)

                        fechaVencimientoValidacion = "NULL"
                        If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                            fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                        End If


                        Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                        Valores = Id_Expediente_Ext.ToString & "," & objROficio.ID_AREA_OFICIO.ToString & "," & objROficio.ID_TIPO_DOCUMENTO.ToString & "," & _
                            objROficio.ID_ANIO.ToString & "," & objROficio.I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                        If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then
                            Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                        End If


                        'GUARDAMOS EN BITACORA PARA OFICIO
                        Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                            ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                        Valores = objROficio.ID_AREA_OFICIO.ToString & "," & _
                            objROficio.ID_TIPO_DOCUMENTO.ToString & "," & _
                            objROficio.ID_ANIO.ToString & "," & _
                            objROficio.I_OFICIO_CONSECUTIVO.ToString & ",'" & _
                            Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & Id_Expediente_Ext.ToString() & "'," & fechaVencimientoValidacion

                        If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then
                            Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                        End If


                    End If


                    item.CssClass &= " related_row "

                End If
            Next

            If Not hasChecked Then Throw New ApplicationException("Seleccione un registro")
            ModalMensaje("Oficios relacionados con éxito")



            tran.Commit()
            Sesion.BitacoraFinaliza(True)
            Sesion.BitacoraFinaliza(True)

        Catch ex As ApplicationException
            tran.Rollback()
            Sesion.BitacoraFinaliza(False)
            Sesion.BitacoraFinaliza(False)
            ModalMensaje(ex.Message)
        Catch ex As Exception
            tran.Rollback()
            Sesion.BitacoraFinaliza(False)
            Sesion.BitacoraFinaliza(False)
            EscribirError(ex, "btnSeleccionar_Click")
        Finally
            If Not Con Is Nothing Then
                Con.Cerrar()
                Con = Nothing
            End If

            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
    End Sub

    Private Sub gvVincularOficios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVincularOficios.RowCommand
        Dim linkArchivoPDF As String = String.Empty
        Try

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            With gvVincularOficios
                ID_ANIO_EDIT = .DataKeys(index)("ID_ANIO").ToString()
                ID_TIPO_DOCUMENTO_EDIT = .DataKeys(index)("ID_TIPO_DOCUMENTO").ToString()
                I_OFICIO_CONSECUTIVO_EDIT = .DataKeys(index)("I_OFICIO_CONSECUTIVO").ToString()
                ID_UNIDAD_ADM_EDIT = .DataKeys(index)("ID_UNIDAD_ADM").ToString()
                If Not String.IsNullOrEmpty(.DataKeys(index)("T_HYP_ARCHIVOSCAN").ToString) Then linkArchivoPDF = .DataKeys(index)("T_HYP_ARCHIVOSCAN").ToString
            End With

            If e.CommandName = "VerPDF" Then
                If Not String.IsNullOrEmpty(linkArchivoPDF) Then
                    AbreArchivoLink(linkArchivoPDF)
                Else
                    Throw New ApplicationException("No hay archivo PDF relacionado al Oficio")
                End If
            End If

        Catch ex As ApplicationException
            ModalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "gvVincularOficios_RowCommand")
        End Try
    End Sub

    Protected Sub AbreArchivoLink(ByVal NombreArchivo As String)

        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim usuario As String
        Dim passwd As String
        Dim ServSharepoint As String
        Dim Dominio As String
        Dim Biblioteca As String
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim urlEncode As String = String.Empty
        Dim filename As String = String.Empty
        Dim Archivo() As Byte = Nothing
        Dim url As String = String.Empty

        Try

            Try

                If NombreArchivo.Contains("#") AndAlso NombreArchivo.ToLower.Contains(WebConfigurationManager.AppSettings("FILES_PATH").ToLower.ToString) Then
                    NombreArchivo = NombreArchivo.Substring(0, NombreArchivo.IndexOf("#"))
                    Archivo = cliente.DownloadData(NombreArchivo)
                Else
                    usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
                    passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                    ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
                    Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
                    Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")

                    cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)
                    url = ServSharepoint & "/" & Biblioteca & "/" & NombreArchivo

                    urlEncode = Server.UrlPathEncode(url)
                    Archivo = cliente.DownloadData(ResolveUrl(urlEncode))
                End If
                filename = "attachment; filename=" & Server.UrlPathEncode(NombreArchivo)

            Catch ex As Exception
                Throw New ApplicationException("Hubo un error abriendo el documento. Posiblemente no existe o no tiene permisos para verlo.")
            End Try

            If Not Archivo Is Nothing Then

                Dim tipo_arch As String = NombreArchivo.Substring(NombreArchivo.LastIndexOf(".") + 1)

                Select Case tipo_arch
                    Case "zip"
                        Response.ContentType = "application/x-zip-compressed"
                    Case "pdf"
                        Response.ContentType = "application/pdf"
                    Case "csv"
                        Response.ContentType = "text/csv"
                    Case "doc"
                        Response.ContentType = "application/doc"
                    Case "docx"
                        Response.ContentType = "application/docx"
                    Case "xls"
                        Response.ContentType = "application/xls"
                    Case "xlsx"
                        Response.ContentType = "application/xlsx"
                    Case "png"
                        Response.ContentType = "image/png"
                    Case "gif"
                        Response.ContentType = "image/gif"
                    Case "jpg"
                        Response.ContentType = "image/jpeg"
                    Case "jpeg"
                        Response.ContentType = "image/jpeg"
                    Case "txt"
                        Response.ContentType = "application/txt"
                    Case "ppt"
                        Response.ContentType = "application/vnd.ms-project"
                    Case "pptx"
                        Response.ContentType = "application/vnd.ms-project"
                    Case "bmp"
                        Response.ContentType = "image/bmp"
                    Case "tif"
                        Response.ContentType = "image/tiff"
                    Case "sbm"
                        Response.ContentType = "application/octet-stream"
                    Case Else
                        Response.ContentType = "application/octet-stream"
                End Select

                Response.AddHeader("content-disposition", filename)

                Response.BinaryWrite(Archivo)

                Response.End()
                '---------------------------------------------
                ' No usamos HttpContext.Current.ApplicationInstance.CompleteRequest()
                ' porque en archivos de texto (txt, csv, etc...) agregaba al final el código de la página.
                '---------------------------------------------
            End If
        Catch ex As ApplicationException
            ModalMensaje(ex.Message)
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoLink")
        End Try
    End Sub


    Private Sub gvVincularOficios_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVincularOficios.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            '--------------------------------------
            ' Vencimiento
            '--------------------------------------
            Dim img As Image = TryCast(e.Row.Cells(10).FindControl("imgVencimiento"), Image)
            If Not IsDBNull(rowView("ID_ESTATUS")) Then
                If CInt(rowView("ID_ESTATUS")) = ESTATUS.CANCELADO Then
                    img.ImageUrl = "~/imagenes/ATENDIDO.png"
                    img.AlternateText = "Cancelado"
                    img.ToolTip = "Cancelado"

                ElseIf CInt(rowView("ID_ESTATUS")) = ESTATUS.CONCLUIDO Then
                    img.ImageUrl = "~/imagenes/ATENDIDO.png"
                    img.AlternateText = "Concluido"
                    img.ToolTip = "Concluido"
                Else
                    If Not IsDBNull(rowView("F_FECHA_VENCIMIENTO")) Then
                        '---------------------------------------------------
                        'Llama a la función para calcular estatus del vencimiento
                        '---------------------------------------------------
                        Dim fVencimiento As Date = CType(rowView("F_FECHA_VENCIMIENTO"), Date)
                        Dim status As String = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultaEstatusVencimiento(fVencimiento)
                        Select Case status
                            Case "Vencido"
                                img.ImageUrl = "~/imagenes/VENCIDO.png"
                                img.AlternateText = "Vencido"
                                img.ToolTip = "Vencido"
                            Case "Por Vencer"
                                img.ImageUrl = "~/imagenes/PREVENTIVO.png"
                                img.AlternateText = "Por Vencer"
                                img.ToolTip = "Por Vencer"
                            Case "Normal"
                                img.ImageUrl = "~/imagenes/statusNormal.png"
                                img.AlternateText = "Normal"
                                img.ToolTip = "Normal"
                            Case Else
                                img.Visible = False
                        End Select
                    Else
                        img.Visible = False
                    End If
                End If
            End If

            If e.Row.RowIndex Mod 2 = 1 Then e.Row.CssClass = "tr_odd"
        End If
    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Session("ID_ANIO") = ID_ANIO
        Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
        Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
        Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
        Session("ID_EXPEDIENTE") = ID_EXPEDIENTE
        Response.Redirect("~/App_Oficios/Seguimiento.aspx", False)
    End Sub

    Protected Sub rblEstatusOficio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblEstatusOficio.SelectedIndexChanged
        Filtraje()
    End Sub

    Private Sub CargaValoresFiltroInner()

        If IsNothing(Session(SessionFiltrosInner)) Then Exit Sub
        Dim _filtros As Dictionary(Of String, String) = CType(Session(SessionFiltrosInner), Dictionary(Of String, String))

        chkSoloMios.Checked = _filtros("chkSoloMios") = "1"
        rblEstatusOficio.SelectedValue = _filtros("rblEstatusOficio")

    End Sub

End Class