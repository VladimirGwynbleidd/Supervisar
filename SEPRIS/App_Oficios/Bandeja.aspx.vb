Imports System.Data
Imports System.Web.Configuration
Imports Clases
Imports AccesoDatos
Imports System.IO
Imports LogicaNegocioSICOD
Imports SICOD.Generales
Imports System.Net
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Drawing.Imaging
Imports System.Globalization

Imports iTextSharp.text.pdf


#Region "Enums"

<Obsolete("Don't use Old enum, use New enum Oficio.TipoOficio", True)>
Public Enum TIPO_DOCUMENTO
    OFICIO_EXTERNO = 1
    DICTAMEN = 2
    ATENTA_NOTA = 3
    OFICIO_INTERNO = 4
End Enum

<Obsolete("Don't use Old enum, use New enum Oficio.EstatusOficio", True)>
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

#End Region

Public Class BandejaOficios
    Inherits System.Web.UI.Page

    Private Const IndexSelectedSession As String = "IndexSelectedOFICIOS"
    Private Const ScrollPositionSession As String = "ScrollPositionSessionOFICIOS"

#Region "Constantes"

    Public Const SessionFiltros As String = "ssnBandejaOficios"
    Public Const SessionFiltrosInner As String = "ssnBandejaOficiosInner"

#End Region

#Region "Propiedades de la página"

    'NHM INI
    Public Property ID_EXPEDIENTE_EXT() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE_EXT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE_EXT") = value
        End Set
    End Property
    'NHM FIN
    Public Property ID_UNIDAD_ADM() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM") = value
        End Set
    End Property

    Public Property T_AREA_OFICIO() As String
        Get
            Return ViewState("T_AREA_OFICIO").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("T_AREA_OFICIO") = value
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

    Public Property FILE_UPLOAD_STATUS() As String
        Get
            Return Session("FILE_UPLOAD_STATUS").ToString
        End Get
        Set(ByVal value As String)
            Session("FILE_UPLOAD_STATUS") = value
        End Set
    End Property

    Public Property TMP_ARCHIVO_ADJUNTAR() As String
        Get
            Return Session("TMP_PATH_ARCHIVO_ADJUNTAR").ToString
        End Get
        Set(ByVal value As String)
            Session("TMP_PATH_ARCHIVO_ADJUNTAR") = value
        End Set
    End Property

    Public Property FILE_UPLOAD_ERROR() As String
        Get
            Return Session("FILE_UPLOAD_ERROR").ToString
        End Get
        Set(ByVal value As String)
            Session("FILE_UPLOAD_ERROR") = value
        End Set
    End Property

    Public Property CODIGO_AREA As Integer
        Get
            Return CInt(ViewState("CODIGO_AREA"))
        End Get
        Set(ByVal value As Integer)
            ViewState("CODIGO_AREA") = value
        End Set
    End Property

    Public Property ARCHIVO_ABRIR As String
        Get
            Return Session("ARCHIVO_ABRIR").ToString
        End Get
        Set(ByVal value As String)
            Session("ARCHIVO_ABRIR") = value
        End Set
    End Property

    Public Property FECHA_ACUSE_TEMP As DateTime
        Get
            Return Convert.ToDateTime(Session("FECHA_ACUSE_TEMP"))
        End Get
        Set(ByVal value As DateTime)
            Session("FECHA_ACUSE_TEMP") = value
        End Set
    End Property

    Public Property SHOW_DICTAMINADO_COLUMN As Boolean
        Get
            Return CBool(Session("SHOW_DICTAMINADO_COLUMN"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_DICTAMINADO_COLUMN") = value
        End Set
    End Property

    Public Property SHOW_FIRMA_DIGITAL_COLUMN As Boolean
        Get
            Return CBool(Session("SHOW_FIRMA_DIGITAL_COLUMN"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_FIRMA_DIGITAL_COLUMN") = value
        End Set
    End Property

    Public Property SHOW_CEDULA_DIGITAL_COLUMN As Boolean
        Get
            Return CBool(Session("SHOW_CEDULA_DIGITAL_COLUMN"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_CEDULA_DIGITAL_COLUMN") = value
        End Set
    End Property

    Public Property SHOW_CEDULA_PDF_COLUMN As Boolean
        Get
            Return CBool(Session("SHOW_CEDULA_PDF_COLUMN"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_CEDULA_PDF_COLUMN") = value
        End Set
    End Property

    Public Property SHOW_GENERACION_CEDULA_PDF As Boolean
        Get
            Return CBool(Session("SHOW_GENERACION_CEDULA_PDF"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_GENERACION_CEDULA_PDF") = value
        End Set
    End Property

    Public Property PUEDE_NOTIFICAR As Boolean
        Get
            Return CBool(Session("PUEDE_NOTIFICAR"))
        End Get
        Set(ByVal value As Boolean)
            Session("PUEDE_NOTIFICAR") = value
        End Set
    End Property

    Public Property SHOW_NOTIFICAR As Boolean
        Get
            Return CBool(Session("SHOW_NOTIFICAR"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_NOTIFICAR") = value
        End Set
    End Property

    Public Property SHOW_NOTIFICACION As Boolean
        Get
            Return CBool(Session("SHOW_NOTIFICACION"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_NOTIFICACION") = value
        End Set
    End Property

    Public Property SHOW_EXPEDIENTE As Boolean
        Get
            Return CBool(Session("SHOW_EXPEDIENTE"))
        End Get
        Set(ByVal value As Boolean)
            Session("SHOW_EXPEDIENTE") = value
        End Set
    End Property

    Public Property TIPO_ARCHIVO_ADJUNTAR As String
        Get
            Return Session("TIPO_ARCHIVO_ADJUNTAR").ToString
        End Get
        Set(ByVal value As String)
            Session("TIPO_ARCHIVO_ADJUNTAR") = value
        End Set
    End Property

    Public Property ID_UNIDAD_ADM_USUARIO As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM_USUARIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM_USUARIO") = value
        End Set

    End Property

    Public Property TOP_ID_UNIDAD_ADM_USUARIO As Integer
        Get
            Return CInt(ViewState("TOP_ID_UNIDAD_ADM_USUARIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("TOP_ID_UNIDAD_ADM_USUARIO") = value
        End Set
    End Property

    Public Property ISMODAL() As Boolean
        Get
            Return CBool(ViewState("ISMODAL"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("ISMODAL") = value
        End Set
    End Property

#End Region

#Region "Eventos"

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("Usuario") Is Nothing Then logOut()

        USUARIO = Session("Usuario").ToString

        '-----------------------------------------------
        ' Verificar Sesión y Perfil de usuario
        '-----------------------------------------------

        If IsNothing(Session("IdOficioSISAN")) And IsNothing(Session("IdOficioSEPRIS")) Then

            verificaSesion()
            verificaPerfil()

        End If



        If Not IsNothing(Session(ScrollPositionSession)) Then

            hdnScrollPosition.Value = Session(ScrollPositionSession)

        End If


        If Not IsPostBack Then

            Dim Con As Conexion = Nothing

            Try

                '--------------------------------------------------------------------------------'
                ' Entidades desde Osiris
                '--------------------------------------------------------------------------------'
                VerificaCargaEntidadesOsiris()


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
                        "SELECT DISTINCT " &
                        "T_CLASIFICACION, ID_CLASIFICACION " &
                        "FROM " & Conexion.Owner & " BDA_CLASIFICACION_OFICIO  " &
                        "WHERE VIG_FLAG = 1 " &
                        "ORDER BY T_CLASIFICACION"

                Con.ConsultaAdapter(sql).Fill(ds, "CLASIFICACION")

                '-----------------------------------
                ' TIPO DE DOCUMENTO
                '-----------------------------------
                sql =
                        "SELECT '-- TODOS --' AS T_TIPO_DOCUMENTO, -1 AS ID_TIPO_DOCUMENTO UNION ALL SELECT " &
                        "T_TIPO_DOCUMENTO, ID_TIPO_DOCUMENTO " &
                        "FROM " & Conexion.Owner & " BDA_TIPO_DOCUMENTO " &
                        "WHERE VIG_FLAG = 1 " &
                        "ORDER BY T_TIPO_DOCUMENTO"
                Con.ConsultaAdapter(sql).Fill(ds, "TIPO_DOCUMENTO")

                '-----------------------------------
                ' ESTATUS
                '-----------------------------------
                sql =
                        "SELECT " &
                        "ID_ESTATUS,T_ESTATUS " &
                        "FROM " & Conexion.Owner & "BDA_ESTATUS_OFICIO " &
                        "WHERE VIG_FLAG = 1 AND ID_ESTATUS NOT IN (" & OficioEstatus.Cancelado & "," & OficioEstatus.Concluido & ") " &
                        "ORDER BY T_ESTATUS"
                Con.ConsultaAdapter(sql).Fill(ds, "ESTATUS")

                '-----------------------------------
                ' UNIDAD ADMINISTRATIVA (ÁREA)
                '-----------------------------------

                sql = "   SELECT DISTINCT ID_UNIDAD_ADM    " +
                        "   FROM [BDA_R_USUARIO_UNIDAD_ADM]  " +
                        "   WHERE VIG_FLAG = 1               " +
                        "   AND USUARIO = '" + USUARIO + "' AND ID_T_UNIDAD_ADM=2"
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
                        "   ORDER BY I_CODIGO_AREA                                                 "

                'NHM INI - Agrega order by
                Dim Perfil As New Perfil
                If Perfil.FuncionPerfil(41) Then
                    sql = "   SELECT DISTINCT ID_UNIDAD_ADM, DSC_UNIDAD_ADM, CAST(I_CODIGO_AREA as varchar) + '   -   ' + DSC_UNIDAD_ADM AS DSC_COMPOSITE, I_CODIGO_AREA " +
                          "   FROM BDA_C_UNIDAD_ADM  " +
                          "   WHERE VIG_FLAG = 1  AND ID_T_UNIDAD_ADM=2  order by I_CODIGO_AREA asc "
                End If
                'NHM FIN

                Dim dtJerarquia As DataSet = Con.Datos(sql, False)
                dtJerarquia.Tables(0).TableName = "UNIDAD_ADM"
                ds.Tables.Add(dtJerarquia.Tables(0).Copy)

                '-----------------------------------
                ' ENTIDAD
                '-----------------------------------
                'sql =
                '        "SELECT ISNULL(T_ENTIDAD_CORTO, '') AS T_ENTIDAD_CORTO, ID_ENTIDAD " +
                '        "FROM " & Conexion.Owner & " BDA_ENTIDAD " +
                '        "WHERE VIG_FLAG= 1 " & _
                '        "ORDER BY T_ENTIDAD_CORTO"
                'Con.ConsultaAdapter(sql).Fill(ds, "ENTIDAD")
                Try
                    ds.Tables.Add(New DataTable("ENTIDAD"))
                    ds.Merge(CType(Session(LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.IdSessionEntidad), DataSet).Tables("ENTIDAD"))
                Catch ex As Exception

                End Try



                '-----------------------------------
                ' DESTINATARIO
                '-----------------------------------
                sql =
                        "SELECT DISTINCT ISNULL(T_NOMBRE, '') + ' ' + ISNULL(T_APELLIDO_P, '') + ' ' + ISNULL(T_APELLIDO_M, '') AS DESTINATARIO, ID_PERSONA " &
                        "FROM " & Conexion.Owner & " BDA_PERSONAL P JOIN BDA_OFICIO O ON P.ID_PERSONA=O.ID_DESTINATARIO " &
                        "WHERE P.VIG_FLAG = 1 " &
                        "ORDER BY DESTINATARIO"
                Con.ConsultaAdapter(sql).Fill(ds, "DESTINATARIO")

                '-----------------------------------
                ' USUARIO ALTA
                '-----------------------------------
                sql =
                        "SELECT DISTINCT U.NOMBRE + ' ' + U.APELLIDOS AS REGISTRO, U.USUARIO AS USUARIO_ALTA " +
                        "FROM BDS_USUARIO U " +
                        "LEFT JOIN BDA_OFICIO O ON U.USUARIO=O.USUARIO_ALTA " +
                        "WHERE U.VIG_FLAG = 1 " &
                        "ORDER BY REGISTRO"
                Con.ConsultaAdapter(sql).Fill(ds, "REGISTRO")

                '-----------------------------------
                ' USUARIO ELABORÓ
                '-----------------------------------
                sql =
                        "SELECT DISTINCT U.NOMBRE + ' ' +U.APELLIDOS AS ELABORO, U.USUARIO AS USUARIO_ELABORO " +
                        "FROM " & Conexion.Owner & " BDS_USUARIO U " +
                        "LEFT JOIN BDA_OFICIO O ON U.USUARIO = O.USUARIO_ELABORO " +
                        "WHERE U.VIG_FLAG = 1 " &
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
                ' Día Festivo
                '-----------------------------------
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
                ucFiltro1.AddFilter("Área", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("UNIDAD_ADM"), "DSC_COMPOSITE", "ID_UNIDAD_ADM", ucFiltroOficios.DataValueType.IntegerType, False, False, , True, True, unidadUsuario)
                ucFiltro1.AddFilter("Año", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("ANIO"), "ID_ANIO", "ID_ANIO", ucFiltroOficios.DataValueType.IntegerType, False, False, , True, True, Today.Year.ToString)
                ucFiltro1.AddFilter("Tipo de documento", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("TIPO_DOCUMENTO"), "T_TIPO_DOCUMENTO", "ID_TIPO_DOCUMENTO", ucFiltroOficios.DataValueType.IntegerType, False, False, , True, True, -1)
                ucFiltro1.AddFilter("Fecha de documento", ucFiltroOficios.AcceptedControls.Calendar, Nothing, "", "F_FECHA_OFICIO", ucFiltroOficios.DataValueType.StringType, False, False, False, True, False, initValueDate)
                ucFiltro1.AddFilter("Destinatario", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("DESTINATARIO"), "DESTINATARIO", "ID_PERSONA", ucFiltroOficios.DataValueType.IntegerType, False, False)
                ucFiltro1.AddFilter("Clasificación", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("CLASIFICACION"), "T_CLASIFICACION", "ID_CLASIFICACION", ucFiltroOficios.DataValueType.IntegerType, False, False)
                ucFiltro1.AddFilter("Estatus", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("ESTATUS"), "T_ESTATUS", "ID_ESTATUS", ucFiltroOficios.DataValueType.IntegerType, False, False)
                ucFiltro1.AddFilter("Entidad", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("ENTIDAD"), "T_ENTIDAD_CORTO", "ID_ENTIDAD", ucFiltroOficios.DataValueType.IntegerType, False, False)
                ucFiltro1.AddFilter("Fecha de acuse", ucFiltroOficios.AcceptedControls.Calendar, Nothing, "", "F_FECHA_ACUSE", ucFiltroOficios.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Fecha de recepción", ucFiltroOficios.AcceptedControls.Calendar, Nothing, "", "F_FECHA_RECEPCION", ucFiltroOficios.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Número de Documento", ucFiltroOficios.AcceptedControls.TextBox, Nothing, "", "I_OFICIO_CONSECUTIVO", ucFiltroOficios.DataValueType.IntegerType, False, False, True)
                ucFiltro1.AddFilter("Asunto", ucFiltroOficios.AcceptedControls.TextBox, Nothing, "", "T_ASUNTO", ucFiltroOficios.DataValueType.StringType, False, True, False)
                ucFiltro1.AddFilter("Registró", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("REGISTRO"), "REGISTRO", "USUARIO_ALTA", ucFiltroOficios.DataValueType.StringType, False, False)
                ucFiltro1.AddFilter("Elaboró", ucFiltroOficios.AcceptedControls.DropDownList, ds.Tables("ELABORO"), "ELABORO", "USUARIO_ELABORO", ucFiltroOficios.DataValueType.StringType, False, False)

                ucFiltro1.SelectionButton = Me.btnFiltrar.ClientID

                ds.Tables.Clear()
                ds = Nothing


            Catch ex As Exception
                Throw New Exception(ex.Message & " Not again!")
            Finally
                If Con IsNot Nothing Then Con.Cerrar()
                Con = Nothing
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            chkSoloMios.Checked = True

            '------------------------------------------
            ' Método de ucFiltro para cargar las opciones del DropDown (campos por los cuales filtrar)
            '------------------------------------------
            ucFiltro1.LoadDDL(SessionFiltros)
            CargaValoresFiltroInner()

            '------------------------------------------
            ' Cargar la lista personalizada de columnas visibles al usuario.
            '------------------------------------------
            ModalPersonalizarColumas.LoadListasPersonalizadas(Session("Usuario").ToString, 2)
            ManejarColumnasGridView(ModalPersonalizarColumas.PerzonalizaColumasOcultasDataGrid(), ModalPersonalizarColumas.PerzonalizaColumasVisiblesDataGrid())

            '------------------------------------------
            ' Manejar las columnas que no deban mostrarse cuando no es Oficio Externo.
            '------------------------------------------
            SHOW_DICTAMINADO_COLUMN = gvBandejaOficios.Columns(26).Visible
            SHOW_CEDULA_DIGITAL_COLUMN = gvBandejaOficios.Columns(19).Visible
            SHOW_FIRMA_DIGITAL_COLUMN = gvBandejaOficios.Columns(20).Visible
            SHOW_NOTIFICACION = gvBandejaOficios.Columns(28).Visible
            SHOW_NOTIFICAR = gvBandejaOficios.Columns(17).Visible
            SHOW_EXPEDIENTE = gvBandejaOficios.Columns(25).Visible
            SHOW_CEDULA_PDF_COLUMN = gvBandejaOficios.Columns(18).Visible
            SHOW_GENERACION_CEDULA_PDF = gvBandejaOficios.Columns(14).Visible
#If DEBUG Then
            SHOW_GENERACION_CEDULA_PDF = True
#End If
            '------------------------------------------
            ' Establecer si el usuario tiene facultades de notificación.
            '------------------------------------------
            PUEDE_NOTIFICAR = BusinessRules.BDS_USUARIO.ConsultarUsuarioPuedeNotificar(USUARIO)

            '------------------------------------------
            ' Revisar cuántos resultados de la bandeja a desplegar.
            '------------------------------------------
            ddlVerUltimos.Items(0).Text = "Últimos " & WebConfigurationManager.AppSettings("MAX_RESULTADOS_BANDEJA").ToString


            '-----------------------------------------
            ' Obtener la top unidad del usuario
            '-----------------------------------------
            Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, 2)
            ID_UNIDAD_ADM_USUARIO = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            TOP_ID_UNIDAD_ADM_USUARIO = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(ID_UNIDAD_ADM_USUARIO, 2)

            ' Definimos si se levanta en ventana modal
            ISMODAL = Request.QueryString("vm") IsNot Nothing

            btnCancelar.Visible = ISMODAL

            If Request.QueryString("cl") IsNot Nothing Then

                rblEstatusOficio.SelectedValue = "2"
                rblEstatusOficio.Enabled = False

            End If


            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Archivo", "setTimeout('CallClickINICIO()', 650);", True)


            '' Detectar si proviene de SISAN
            If Not IsNothing(Session("IdOficioSISAN")) Then

                Me.BtnPersonalizar.Visible = False
                Me.filtroOficiosWrapper.Visible = False
                Filtraje()

            End If

            If Not IsNothing(Session("IdOficioSEPRIS")) Then

                Me.BtnPersonalizar.Visible = False
                Me.filtroOficiosWrapper.Visible = False
                Filtraje()

            End If



        Else
            If Request.Params(Page.postEventArgumentID).Contains("rowDoubleClick") Then
                Dim strRequest() As String = Request.Params(Page.postEventArgumentID).Split("?", 6, StringSplitOptions.None)
                handleRowDoubleClick(strRequest)
            End If
        End If
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click,
                                                                                        btnExportarExcelTop.Click
        Try

            If gvBandejaOficios.Rows.Count > 0 Then

                Dim Reference As List(Of String) = New List(Of String)
                Reference.Add("OFICIOS")
                Reference.Add(USUARIO)
                Reference.Add(DateTime.Today.ToString("dd/MM/yyyy"))

                Dim HeaderColumns As List(Of String()) = New List(Of String())
                HeaderColumns.Add({"Area", "DSC_UNIDAD_ADM"})
                HeaderColumns.Add({"Oficio", "T_OFICIO_NUMERO"})
                HeaderColumns.Add({"Asunto", "T_ASUNTO"})
                HeaderColumns.Add({"Elaboró", "ELABORO"})
                HeaderColumns.Add({"Registró", "REGISTRO"})
                HeaderColumns.Add({"Entidad", "T_ENTIDAD_CORTO"})
                HeaderColumns.Add({"Año", "ID_ANIO"})
                HeaderColumns.Add({"Tipo de Documento", "T_TIPO_DOCUMENTO"})
                HeaderColumns.Add({"Destinatario", "DESTINATARIO"})
                HeaderColumns.Add({"Estatus", "T_ESTATUS"})
                HeaderColumns.Add({"Fecha de Recepción", "F_FECHA_RECEPCION"})
                HeaderColumns.Add({"Fecha de Documento", "F_FECHA_OFICIO"})
                HeaderColumns.Add({"Fecha de Acuse", "F_FECHA_ACUSE"})
                HeaderColumns.Add({"Dictaminado", "DICTAMINADO_FLAG"})
                HeaderColumns.Add({"Fecha Vencimiento", "F_FECHA_VENCIMIENTO"})
                HeaderColumns.Add({"Notificación", "NOTIF_ELECTRONICA_FLAG"})
                HeaderColumns.Add({"Clasificación", "T_CLASIFICACION"})


                Dim export As New OpenXML.ExportExcel()
                export.SheetName = "Oficios"
                export.TableName = "Lista de Oficios"
                export.HeaderColor = "236b7c"
                export.HeaderForeColor = "FFFFFF"
                export.CellForeColor = "000000"
                export.ShowGridLines = True
                export.Reference = Reference
                export.HeaderColumns = HeaderColumns
                export.DataSource = CType(Session("ConsultaBO"), DataTable)
                export.CreatePackage("OFICIOS")
            Else
                Throw New ApplicationException("Debe haber registros para exportar")
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion BtnExportarInferior_Click(): " & ex.ToString(), EventLogEntryType.Error)
        End Try
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFiltrar.Click
        Filtraje()
    End Sub

    Protected Sub AsyncFileUpload2_UploadedComplete(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs)

        If e.state = AjaxControlToolkit.AsyncFileUploadState.Success Then

            Dim ExtensionOriginal As String = ""
            Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Select Case TIPO_ARCHIVO_ADJUNTAR

                Case "T_HYP_FIRMADIGITAL"
                    ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_ARCHIVOSCAN").ToString)

                Case "T_CEDULADIGITAL"
                    ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_CEDULAPDF").ToString)

                Case "T_ANEXO_DOS"
                    ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_ANEXO_UNO").ToString)

            End Select

            If Not IO.Path.GetExtension(AsyncFileUpload2.FileName).ToUpper = ".SBMX" Then
                ExtensionOriginal = ""
            End If

            Dim NombreSharepoint As String = crearNombreArchivo(AsyncFileUpload2.FileName, TIPO_ARCHIVO_ADJUNTAR, ExtensionOriginal)

            AsyncFileUpload2.SaveAs(System.IO.Path.GetTempPath() + NombreSharepoint)


            TMP_ARCHIVO_ADJUNTAR = NombreSharepoint
            FILE_UPLOAD_STATUS = False
            FILE_UPLOAD_ERROR = ""
        Else
            TMP_ARCHIVO_ADJUNTAR = ""
            FILE_UPLOAD_STATUS = True
            FILE_UPLOAD_ERROR = "Error subiendo archivo"
        End If

    End Sub

    Protected Sub btnModalFileUploadOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModalFileUploadOK.Click

        Try

            If TMP_ARCHIVO_ADJUNTAR <> String.Empty Then
                Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO
                objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO
                objOficio.IdAnio = ID_ANIO
                objOficio.IdArea = ID_UNIDAD_ADM
                objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
                objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO

                Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                Dim fechaVencimientoValidacion As String = "NULL"
                If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                    fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                End If

                objOficio.UsuarioElaboro = USUARIO
                objOficio.Comentario = fechaVencimientoValidacion

                Select Case TIPO_ARCHIVO_ADJUNTAR
                    Case "T_HYP_ARCHIVOWORD"

                        '---------------------------------------
                        'Archivo de Trabajo (word)
                        'valida que el tipo de documento sea  word
                        '----------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".doc") Or (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".docx") Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoWord = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoWord = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo en SharePoint")
                            Else
                                If Not BusinessRules.BDA_OFICIO.ActualizarArchivoWord(objOficio) Then Throw New ApplicationException("Error guardando el archivo ")
                            End If
                        Else
                            Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                        End If
                    Case "T_HYP_ARCHIVOSCAN"

                        '---------------------------------------
                        'Archivo PDF
                        'valida que el tipo de documento sea  PDF
                        '---------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".pdf") Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoPDF = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoPDF = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo en SharePoint")
                            Else

                                If Not BusinessRules.BDA_OFICIO.ActualizarArchivoPDF(objOficio) Then Throw New ApplicationException("Error guardando el archivo ")

                            End If
                        Else
                            Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                        End If
                    Case "T_CEDULADIGITAL"

                        '---------------------------------------
                        'Cédula de notificación electrónica
                        'valida que el tipo de documento sea .sbm
                        '---------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR).Contains(".sbm")) Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoCedulaDigital = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoCedulaDigital = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo en SharePoint")
                            Else
                                If Not BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio) Then Throw New ApplicationException("Error guardando el archivo")
                            End If
                        Else
                            Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                        End If

                    Case "T_HYP_CEDULAPDF"
                        '---------------------------------------
                        ' Archivo PDF de la cédula
                        '---------------------------------------

                        '---------------------------------------
                        'valida que el tipo de documento sea .pdf
                        '---------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".pdf") Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoCedulaPDF = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoCedulaPDF = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo en SharePoint")
                            Else

                                If Not BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaPDF(objOficio) Then Throw New ApplicationException("Error guardando el archivo ")

                            End If
                        Else
                            Throw New ApplicationException("El archivo  no es un tipo de archivo valido")
                        End If
                    Case "T_HYP_FIRMADIGITAL"
                        '---------------------------------------
                        ' Archivo de la Firma Digital
                        '---------------------------------------

                        '---------------------------------------
                        'valida que el tipo de documento sea .sbm
                        '---------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR).Contains(".sbm")) Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoFirmaDigital = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoFirmaDigital = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo en SharePoint")
                            Else
                                If Not BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio) Then Throw New ApplicationException("Error guardando el archivo")
                            End If
                        Else
                            Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                        End If
                    Case "T_HYP_RESPUESTAOFICIO"
                        '---------------------------------------
                        ' Archivo de la Respuesta electrónica
                        '---------------------------------------

                        '---------------------------------------
                        'valida que el tipo de documento sea .sbm
                        '---------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR).Contains(".pdf")) Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoRespuesta = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoRespuesta = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo  en SharePoint")
                            Else
                                If Not BusinessRules.BDA_OFICIO.ActualizarArchivoRespuesta(objOficio) Then Throw New ApplicationException("Error guardando el archivo")

                            End If
                        Else
                            Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                        End If
                    Case "T_HYP_ACUSERESPUESTA"
                        '---------------------------------------
                        ' Archivo del Acuse
                        '---------------------------------------

                        '---------------------------------------
                        'valida que el tipo de documento sea .pdf
                        '---------------------------------------
                        If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".pdf") Then
                            '---------------------------------------
                            ' Guarda el archivo en Sharepoint
                            '---------------------------------------
                            objOficio.ArchivoAcuse = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                            If objOficio.ArchivoAcuse = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo en SharePoint")
                            Else

                                If Not FECHA_ACUSE_TEMP = Nothing Then

                                    If Not BusinessRules.BDA_OFICIO.ActualizarArchivoAcuse(objOficio) Then
                                        Throw New ApplicationException("Error guardando el archivo")
                                    End If

                                    objOficio.FechaAcuse = FECHA_ACUSE_TEMP
                                    If Not BusinessRules.BDA_OFICIO.ActualizarFechaAcuse(objOficio) Then
                                        Throw New ApplicationException("Error actualizando fecha del acuse, intente de nuevo")
                                    End If

                                Else
                                    Throw New ApplicationException("Debe seleccionar la fecha del acuse, intente de nuevo")
                                End If

                            End If

                        Else
                            Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                        End If

                    Case "T_ANEXO_UNO"
                        '---------------------------------------
                        ' Archivo Anexo 1
                        '---------------------------------------

                        '---------------------------------------
                        ' Guarda el archivo en Sharepoint
                        '---------------------------------------
                        objOficio.ArchivoAnexo1 = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                        If objOficio.ArchivoAnexo1 = String.Empty Then
                            Throw New ApplicationException("Error guardando el archivo en SharePoint")
                        Else

                            If Not BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo1(objOficio) Then Throw New ApplicationException("Error guardando el archivo")

                        End If

                    Case "T_ANEXO_DOS"
                        '---------------------------------------
                        ' Archivo Anexo 2
                        '---------------------------------------

                        '---------------------------------------
                        ' Guarda el archivo en Sharepoint
                        '---------------------------------------
                        objOficio.ArchivoAnexo2 = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                        If objOficio.ArchivoAnexo2 = String.Empty Then
                            Throw New ApplicationException("Error guardando el archivo en SharePoint")
                        Else

                            If Not BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio, True) Then Throw New ApplicationException("Error guardando el archivo")

                        End If

                    Case "T_HYP_EXPEDIENTE"
                        '---------------------------------------
                        ' Expediente
                        '---------------------------------------

                        '---------------------------------------
                        'valida que el tipo de documento sea .doc o .docx
                        '---------------------------------------
                        'NHM INI
                        If ID_TIPO_DOCUMENTO = OficioTipo.Dictamen Then

                            If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".pdf") Or (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".zip") Then
                                '---------------------------------------
                                ' Guarda el archivo en Sharepoint
                                '---------------------------------------
                                objOficio.ArchivoExpediente = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                                If objOficio.ArchivoExpediente = String.Empty Then
                                    Throw New ApplicationException("Error guardando el archivo en SharePoint")
                                Else
                                    'NHM - metodos nuevos
                                    If Not LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoExpediente(objOficio, OficioTipo.Dictamen) Then
                                        Throw New ApplicationException("Error guardando el archivo")
                                    Else
                                        GenerarSeguimientoExpediente(TMP_ARCHIVO_ADJUNTAR)
                                    End If
                                    'NHM FIN

                                End If
                            Else
                                Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                            End If

                        Else

                            If (System.IO.Path.GetExtension(TMP_ARCHIVO_ADJUNTAR) = ".pdf") Then
                                '---------------------------------------
                                ' Guarda el archivo en Sharepoint
                                '---------------------------------------
                                objOficio.ArchivoExpediente = subirArchivo(TMP_ARCHIVO_ADJUNTAR, objOficio.IdAnio.ToString, objOficio.IOficioConsecutivo.ToString)
                                If objOficio.ArchivoExpediente = String.Empty Then
                                    Throw New ApplicationException("Error guardando el archivo en SharePoint")
                                Else
                                    If Not LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoExpediente(objOficio) Then Throw New ApplicationException("Error guardando el archivo")
                                End If
                            Else
                                Throw New ApplicationException("El archivo no es un tipo de archivo valido")
                            End If

                        End If

                        'NHM FIN
                End Select

            End If

            modalFileUpload.Hide()

            Filtraje()
            upGridView.Update()

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        End Try
    End Sub

    'NHM INI
    Public Sub GenerarSeguimientoExpediente(ByVal NombreArchivoNuevo As String)

        Dim objSeguimiento As New LogicaNegocioSICOD.Entities.BDA_SEGUIMIENTO_OFICIOEnt
        Dim FilePath As String = String.Empty
        Dim objBitacora As Bitacora.nsBitacora.Bitacora = Nothing
        Dim objBd As ConexionBaseDatos.ConexionBD.ConexionSql
        Dim arrValoresSeguimiento(9) As Object
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim objSP As New nsSharePoint.FuncionesSharePoint


        Dim resultado As Boolean = False
        Dim fechaVencimientoValidacion As String = "NULL"

        Try


            objSeguimiento.ID_ANIO = ID_ANIO
            objSeguimiento.ID_AREA_OFICIO = ID_UNIDAD_ADM
            objSeguimiento.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
            objSeguimiento.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO

            objSeguimiento.ID_USUARIO = USUARIO
            objSeguimiento.F_SEGUIMIENTO = DateTime.Now
            objSeguimiento.T_SEGUIMIENTO = HttpUtility.HtmlEncode("Se vincula por default el expediente")

            objSeguimiento.OFICIOPROCEDENTE_FLAG = 1
            objSeguimiento.T_OFICIO_PROCEDENTE = NombreArchivoNuevo

            'ID_EXPEDIENTE_EXT
            If ID_EXPEDIENTE_EXT = 0 Then

                'hay que crear expediente
                Dim Campos As String
                Dim Valores As String
                Dim Con As Conexion = Nothing
                Dim Dr As Odbc.OdbcDataReader = Nothing


                Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO.ToString,
                                              ID_UNIDAD_ADM.ToString,
                                              ID_TIPO_DOCUMENTO.ToString,
                                              I_OFICIO_CONSECUTIVO.ToString)

                fechaVencimientoValidacion = "NULL"
                If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                    fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                End If


                Con = New Conexion()

                '' creamos nuevo expediente (nadie tiene expediente)
                Dr = Con.Consulta("SELECT ISNULL(MAX(ID_EXPEDIENTE),0)+1 AS ID_EXPEDIENTE FROM " & Conexion.Owner & "BDA_C_EXPEDIENTE")
                Dr.Read()
                ID_EXPEDIENTE_EXT = Convert.ToInt32(Dr.Item("ID_EXPEDIENTE"))
                Dr.Close()

                Campos = "ID_EXPEDIENTE,DSC_EXPEDIENTE,USUARIO_CREACION,VIG_FLAG,FECH_INI_VIG"
                Valores = ID_EXPEDIENTE_EXT.ToString & ", '', '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_C_EXPEDIENTE", Campos, Valores) Then
                    Throw New ApplicationException("No pudo crearse el expediente ")
                End If


                Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                Valores = ID_EXPEDIENTE_EXT.ToString & "," & ID_UNIDAD_ADM.ToString & "," & ID_TIPO_DOCUMENTO.ToString & "," & ID_ANIO.ToString & "," &
                    I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If

                'GUARDAMOS EN BITACORA PARA OFICIO
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" &
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = ID_UNIDAD_ADM.ToString & "," &
                    ID_TIPO_DOCUMENTO.ToString & "," &
                    ID_ANIO.ToString & "," &
                    I_OFICIO_CONSECUTIVO.ToString & ",'" &
                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & ID_EXPEDIENTE_EXT.ToString() & "'," & fechaVencimientoValidacion

                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If


            End If


            objSeguimiento.ID_EXPEDIENTE = ID_EXPEDIENTE_EXT


            'Asigna valores para la bitacora

            arrValoresSeguimiento(1) = objSeguimiento.ID_USUARIO
            arrValoresSeguimiento(2) = objSeguimiento.I_OFICIO_CONSECUTIVO
            arrValoresSeguimiento(3) = objSeguimiento.ID_ANIO
            arrValoresSeguimiento(4) = objSeguimiento.ID_TIPO_DOCUMENTO
            arrValoresSeguimiento(5) = objSeguimiento.ID_AREA_OFICIO
            arrValoresSeguimiento(6) = objSeguimiento.F_SEGUIMIENTO
            arrValoresSeguimiento(7) = objSeguimiento.T_SEGUIMIENTO
            arrValoresSeguimiento(8) = objSeguimiento.OFICIOPROCEDENTE_FLAG
            arrValoresSeguimiento(9) = objSeguimiento.T_OFICIO_PROCEDENTE


            objBitacora = New Bitacora.nsBitacora.Bitacora()
            objBd = New ConexionBaseDatos.ConexionBD.ConexionSql()

            'Inserta los datos en la tabla seguimiento
            resultado = LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.InsertarComentario(objSeguimiento)

            'Valida si se ha guardado o no y manda un mensaje
            If (resultado) Then

            Else
                Throw New ApplicationException("Ocurrio un error al guardar el archivo")
            End If

            'Obtiene el id del seguimiento insertado
            arrValoresSeguimiento(0) = LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.ObtenerMaximoIdSeguimiento()

            objBitacora.IniciarBitacora("Insertar comentario/archivo", objBd, Session("IDsesion").ToString(), USUARIO, CType(Session("Perfil"), Integer), ID_UNIDAD_ADM)
            Dim arrCamposSeguimiento() As String = {"ID_SEGUIMIENTO_OFICIO", "ID_USUARIO",
                                        "I_OFICIO_CONSECUTIVO", "ID_ANIO", "ID_TIPO_DOCUMENTO", "ID_AREA_OFICIO",
                                        "F_SEGUIMIENTO", "T_SEGUIMIENTO", "OFICIOPROCEDENTE_FLAG", "T_OFICIO_PROCEDENTE"}

            objBitacora.BitacoraIniciarTransaccion()
            objBitacora.BitacoraInsertar("BDA_SEGUIMIENTO_OFICIO", arrCamposSeguimiento, arrValoresSeguimiento, True, resultado, "")
            objBitacora.BitacoraFinalizarTransaccion(True)

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
            If Not objBitacora Is Nothing Then objBitacora.BitacoraFinalizarTransaccion(False)
        Catch ex As Exception
            EscribirError(ex, "Error al insertar el comentario")
            If Not objBitacora Is Nothing Then objBitacora.BitacoraFinalizarTransaccion(False)
        Finally
            If System.IO.File.Exists(FilePath) Then System.IO.File.Delete(FilePath)
        End Try

    End Sub
    'NHM FIN

#Region "Gridview"

    Protected Sub gvBandejaOficios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

        If ISMODAL Then Return

        If Not e.CommandName = "Sort" Then



            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim rowOsiris(0) As DataRow

            With gvBandejaOficios
                ID_ANIO = .DataKeys(index)("ID_ANIO").ToString()
                ID_TIPO_DOCUMENTO = .DataKeys(index)("ID_TIPO_DOCUMENTO").ToString()
                I_OFICIO_CONSECUTIVO = .DataKeys(index)("I_OFICIO_CONSECUTIVO").ToString()
                ID_UNIDAD_ADM = .DataKeys(index)("ID_UNIDAD_ADM").ToString()
                T_AREA_OFICIO = .DataKeys(index)("DSC_UNIDAD_ADM").ToString()
            End With


            'gvBandejaOficios.SelectedRowStyle.BackColor = System.Drawing.Color.FromArgb(127, 166, 161)
            'gvBandejaOficios.SelectedRowStyle.Font.Bold = True
            'gvBandejaOficios.SelectedRowStyle.ForeColor = System.Drawing.Color.White

            gvBandejaOficios.SelectedIndex = index
            AsignaValoresScrollRowIndex(index)

            '----------------------------------------------------
            ' Obtener código de área de unidad adm
            '----------------------------------------------------
            Dim dtCodigoArea As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.GetCodigoAreaPorUnidadAdm(ID_UNIDAD_ADM)
            CODIGO_AREA = CInt(dtCodigoArea.Rows(0)("I_CODIGO_AREA"))

            Select Case e.CommandName
                Case "Edit"
                    Session("ID_ANIO") = ID_ANIO
                    Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
                    Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
                    Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
                    Session("CODIGO_AREA") = CODIGO_AREA
                    Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1", False)
                Case "Notificar"
                    checkEnviarNotificacion()

                Case "Cedula"
                    ''agc crea cedula notificacion
                    showGenerarCedulaPDF()

                Case Else

                    '----------------------------------------
                    ' Se quiere adjuntar o ver archivo asociado al folio
                    '----------------------------------------
                    Dim CommandName As String() = e.CommandName.Split("?", 2, StringSplitOptions.None)
                    'NHM INI
                    'processFileRequest(CommandName(0), CommandName(1))
                    processFileRequest(CommandName(0), CommandName(1), ID_TIPO_DOCUMENTO)
                    'NHM FIN

            End Select
        End If

    End Sub

    Private Sub gvBandejaOficios_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBandejaOficios.RowDataBound


        If e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(6).ToolTip = "Entidad Destinataria"
            e.Row.Cells(10).ToolTip = "Destinatario del oficio"
            e.Row.Cells(11).ToolTip = "Fecha de Recepción del oficio"
            e.Row.Cells(12).ToolTip = "Fecha de Registro del oficio"
            e.Row.Cells(13).ToolTip = "Fecha del Acuse del oficio"
            e.Row.Cells(14).ToolTip = "Oprima el botón para generar y/o ver la cédula de notificación en PDF"
            e.Row.Cells(15).ToolTip = "Oprima el botón para generar el correo de notificación del oficio"
            e.Row.Cells(16).ToolTip = "Oprima el botón para guardar y/o ver el oficio en formato Word"
            e.Row.Cells(17).ToolTip = "Oprima el botón para guardar y/o ver el oficio en formato PDF"
            e.Row.Cells(18).ToolTip = "Oprima el botón para guardar y/o ver la cédula de notificación en formato Word"
            e.Row.Cells(19).ToolTip = "Oprima el botón para guardar la cédula de notificación (Formato SMB)"
            e.Row.Cells(20).ToolTip = "Oprima el botón para guardar el oficio (Formato SMB)"
            e.Row.Cells(22).ToolTip = "Oprima el botón para guardar y/o ver el acuse del oficio"
            e.Row.Cells(23).ToolTip = "Oprima el botón para guardar y/o ver los anexos del oficio. En caso de tener más de 1 anexo se deberán almacenar en un .zip"
            e.Row.Cells(24).ToolTip = "Oprima el botón para guardar el(los) anexo(s) del oficio (Formato SMB)"
            e.Row.Cells(25).ToolTip = "Oprima el botón para guardar y/o ver el archivo expediente dictamen"
            e.Row.Cells(26).ToolTip = "Indicador de oficio dictaminado"
            e.Row.Cells(28).ToolTip = "Indicador de la notificación"
            e.Row.Cells(29).ToolTip = "Tipo de oficio generado"
            e.Row.Cells(31).ToolTip = "Estatus del oficio"

        End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            Dim ib As ImageButton
            Dim lb As LinkButton
            Dim lbl As Label

            Dim esOficioExterno As Boolean = rowView("ID_TIPO_DOCUMENTO").ToString() = "1"
            Dim esDictamen As Boolean = rowView("ID_TIPO_DOCUMENTO").ToString() = "2"
            Dim esNoSIE As Boolean = rowView("FIRMA_SIE_FLAG").ToString() = "0"



            lb = TryCast(e.Row.Cells(0).Controls(0), LinkButton)
            lb.OnClientClick = "return GetScrollGrid();"

            Dim rowOsiris(0) As DataRow

            '' Obtenemos el registro de entidad de osiris

            If Not IsDBNull(rowView("ID_ENTIDAD")) Then
                If Not CType(Session(LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.IdSessionEntidad), DataSet) Is Nothing Then
                    rowOsiris = CType(Session(LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.IdSessionEntidad), DataSet).Tables("BDV_C_ENTIDAD").Select("CVE_ID_ENT= " & rowView("ID_ENTIDAD").ToString & " and ID_T_ENT=" & rowView("ID_ENTIDAD_TIPO").ToString)
                End If

            End If

            Try

                If rowOsiris.Length > 0 Then

                    If Not rowOsiris(0) Is Nothing Then e.Row.Cells(7).Text = rowOsiris(0)("SIGLAS_ENT").ToString

                    'Session("consultaBO") 
                    'T_ENTIDAD_CORTO
                    Dim registros As DataRow() = CType(Session("consultaBO"), DataTable).Select("ID_ENTIDAD=" & rowView("ID_ENTIDAD").ToString & " AND ID_ENTIDAD_TIPO=" & rowView("ID_ENTIDAD_TIPO").ToString & " AND T_ENTIDAD_CORTO=''")

                    If registros.Length > 0 Then

                        For Each renglon As DataRow In CType(Session("consultaBO"), DataTable).Select("ID_ENTIDAD=" & rowView("ID_ENTIDAD").ToString & " AND ID_ENTIDAD_TIPO=" & rowView("ID_ENTIDAD_TIPO").ToString & " AND T_ENTIDAD_CORTO=''")

                            If Not rowOsiris(0) Is Nothing Then renglon.Item("T_ENTIDAD_CORTO") = rowOsiris(0)("SIGLAS_ENT").ToString

                        Next

                        CType(Session("consultaBO"), DataTable).AcceptChanges()

                    End If

                End If

            Catch ex As Exception

            End Try







            '--------------------------------------
            '
            '--------------------------------------
            lbl = TryCast(e.Row.Cells(10).FindControl("destinatario"), Label)

            If Not IsDBNull(rowView("DESTINATARIO")) AndAlso Not Trim(rowView("DESTINATARIO").ToString) = String.Empty Then
                lbl.Text = rowView("DESTINATARIO").ToString
            ElseIf Not IsDBNull(rowView("T_DESTINATARIO")) Then
                lbl.Text = rowView("T_DESTINATARIO").ToString
            Else
                lbl.Text = rowView("T_FUNCION").ToString
            End If


            '--------------------------------------
            ' Generar Cédula
            '--------------------------------------

            TryCast(e.Row.Cells(15).Controls(0), ImageButton).Visible = (esOficioExterno OrElse esDictamen) And Not esNoSIE And Not rblEstatusOficio.SelectedValue = "2"

            TryCast(e.Row.Cells(14).Controls(0), ImageButton).Visible = (esOficioExterno OrElse esDictamen) And Not esNoSIE And Not rblEstatusOficio.SelectedValue = "2"

            '--------------------------------------
            ' Notificación Electrónica
            '--------------------------------------
            ib = TryCast(e.Row.Cells(15).Controls(0), ImageButton)
            ib.OnClientClick = "javascript:ShowProcesa();"
            ib.ToolTip = "Notificación Pendiente"
            If Not IsDBNull(rowView("NOTIF_ELECTRONICA_FLAG")) AndAlso Not rowView("NOTIF_ELECTRONICA_FLAG").ToString.Equals(String.Empty) Then
                If rowView("NOTIF_ELECTRONICA_FLAG") Then
                    ib.ImageUrl = "~/imagenes/notificar_generado.png"
                    ib.ToolTip = "Notificación Generada"
                End If
            ElseIf rblEstatusOficio.SelectedValue = "2" Then
                ib.Visible = False
            End If

            '--------------------------------------
            ' Archivo Word
            '--------------------------------------
            ib = TryCast(e.Row.Cells(16).Controls(0), ImageButton)
            ib.ToolTip = "Subir Archivo Word"
            If Not IsDBNull(rowView("T_HYP_ARCHIVOWORD")) AndAlso Not rowView("T_HYP_ARCHIVOWORD").ToString.Equals(String.Empty) Then
                ib.ImageUrl = "~/imagenes/archivo-word.gif"
                ib.ToolTip = "Ver Archivo Word"
            ElseIf rblEstatusOficio.SelectedValue = "2" Then
                ib.Visible = False
            End If
            ib.CommandName += "?" + rowView("T_HYP_ARCHIVOWORD").ToString

            '--------------------------------------
            ' Archivo PDF
            '--------------------------------------
            ib = TryCast(e.Row.Cells(17).Controls(0), ImageButton)
            ib.ToolTip = "Subir Archivo PDF"
            'NHM INI - se oculta icono si es dictamen
            If esDictamen Then
                ib.Visible = False
            Else
                If Not IsDBNull(rowView("T_HYP_ARCHIVOSCAN")) AndAlso Not rowView("T_HYP_ARCHIVOSCAN").ToString.Equals(String.Empty) Then
                    ib.ImageUrl = "~/imagenes/archivo-pdf.gif"
                    ib.ToolTip = "Ver Archivo PDF"
                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False
                End If
            End If
            'NHM FIN
            ib.CommandName += "?" + rowView("T_HYP_ARCHIVOSCAN").ToString

            '--------------------------------------
            ' Cédula
            '--------------------------------------
            ib = TryCast(e.Row.Cells(18).Controls(0), ImageButton)
            If (esOficioExterno OrElse esDictamen) And Not esNoSIE Then
                ib.ToolTip = "Subir Archivo Cédula PDF"
                If Not IsDBNull(rowView("T_HYP_CEDULAPDF")) AndAlso Not rowView("T_HYP_CEDULAPDF").ToString.Equals(String.Empty) Then
                    ib.ImageUrl = "~/imagenes/archivo-cedula.gif"
                    ib.ToolTip = "Ver Archivo Cédula PDF"
                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False
                End If
                ib.CommandName += "?" + rowView("T_HYP_CEDULAPDF").ToString
            Else
                ib.Visible = False
            End If


            '--------------------------------------
            ' Cedula SBM
            '--------------------------------------
            ib = TryCast(e.Row.Cells(19).Controls(0), ImageButton)
            If (esOficioExterno OrElse esDictamen) And Not esNoSIE Then


                ib.ToolTip = "Subir Archivo Cédula Digital"

                If Not IsDBNull(rowView("T_CEDULADIGITAL")) AndAlso
                        Not rowView("T_CEDULADIGITAL").ToString.Equals(String.Empty) Then

                    If rowView("T_CEDULADIGITAL").ToString.Equals("0") Then
                        ib.ImageUrl = "~/imagenes/question.png"
                        ib.ToolTip = "SBM pendiente de confirmar - Haga click para comprobar."
                    Else
                        ib.ImageUrl = "~/imagenes/archivo-cne.gif"
                        ib.ToolTip = "Ver Archivo Cédula Digital"
                    End If

                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False
                End If

                ib.CommandName += "?" + rowView("T_CEDULADIGITAL").ToString
            Else
                ib.Visible = False
            End If



            '--------------------------------------
            ' Firma SBM
            '--------------------------------------
            ib = TryCast(e.Row.Cells(20).Controls(0), ImageButton)
            If (esOficioExterno OrElse esDictamen) And Not esNoSIE Then

                ib.ToolTip = "Subir Archivo Firma Digital"
                If Not IsDBNull(rowView("T_HYP_FIRMADIGITAL")) AndAlso Not rowView("T_HYP_FIRMADIGITAL").ToString.Equals(String.Empty) Then

                    If rowView("T_HYP_FIRMADIGITAL").ToString.Equals("0") Then
                        ib.ImageUrl = "~/imagenes/question.png"
                        ib.ToolTip = "SBM pendiente de confirmar - Haga click para comprobar."
                    Else
                        ib.ImageUrl = "~/imagenes/archivo-firma.gif"
                        ib.ToolTip = "Ver Archivo Firma Digital"
                    End If

                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False

                End If
                ib.CommandName += "?" + rowView("T_HYP_FIRMADIGITAL").ToString

            Else
                ib.Visible = False
            End If
            '--------------------------------------
            ' Respuesta
            '--------------------------------------
            ib = TryCast(e.Row.Cells(21).Controls(0), ImageButton)
            ib.ToolTip = "Subir Archivo Respuesta"
            If Not IsDBNull(rowView("T_HYP_RESPUESTAOFICIO")) AndAlso Not rowView("T_HYP_RESPUESTAOFICIO").ToString.Equals(String.Empty) Then
                ib.ImageUrl = "~/imagenes/archivo-respuesta.gif"
                ib.ToolTip = "Ver Archivo Respuesta"
            ElseIf rblEstatusOficio.SelectedValue = "2" Then
                ib.Visible = False
            End If

            ib.CommandName += "?" + rowView("T_HYP_RESPUESTAOFICIO").ToString

            ''--------------------------------------
            ' Acuse
            ''--------------------------------------
            ib = TryCast(e.Row.Cells(22).Controls(0), ImageButton)
            ib.ToolTip = "Archivo Acuse"
            ib.ToolTip = "Subir Archivo Acuse"
            'NHM INI - se oculta icono si es dictamen
            If esDictamen Then
                ib.Visible = False
            Else
                If Not IsDBNull(rowView("T_HYP_ACUSERESPUESTA")) AndAlso Not rowView("T_HYP_ACUSERESPUESTA").ToString.Equals(String.Empty) Then
                    ib.ImageUrl = "~/imagenes/archivo-acuse.gif"
                    ib.ToolTip = "Ver Archivo Acuse"
                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False
                End If
            End If
            'NHM FIN            
            ib.CommandName += "?" + rowView("T_HYP_ACUSERESPUESTA").ToString

            ' ''--------------------------------------
            ' '' Anexo 1
            ' ''--------------------------------------
            ''ib = TryCast(e.Row.Cells(24).Controls(0), ImageButton)
            ''ib.ToolTip = "Subir Archivo Anexo 1"


            ''If Not IsDBNull(rowView("T_ANEXO_UNO")) AndAlso Not rowView("T_ANEXO_UNO").ToString.Equals(String.Empty) Then

            ''    Dim tieneSBM As Boolean = False

            ''    If Not rowView("T_ANEXO_UNO").ToString.Contains("@") Then

            ''        Dim nombreArchivo As String = String.Empty
            ''        If rowView("T_ANEXO_UNO").ToString.Contains("#") AndAlso
            ''                        rowView("T_ANEXO_UNO").ToString.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
            ''            nombreArchivo = rowView("T_ANEXO_UNO").ToString.Substring(0, rowView("T_ANEXO_UNO").ToString.IndexOf("#"))
            ''        Else
            ''            nombreArchivo = rowView("T_ANEXO_UNO").ToString
            ''        End If

            ''        If Path.GetExtension(nombreArchivo).ToLower = ".sbm" OrElse
            ''                Path.GetExtension(nombreArchivo).ToLower = ".sbx" Then
            ''            tieneSBM = True
            ''        End If
            ''    Else
            ''        tieneSBM = True
            ''    End If

            ''    If tieneSBM Then
            ''        ib.ImageUrl = "~/imagenes/archivo-anexo1.gif"
            ''        ib.ToolTip = "Ver Archivo Anexo 1"
            ''    Else
            ''        ib.ImageUrl = "~/imagenes/question_anexos.png"
            ''        ib.ToolTip = "Anexo por encriptar"
            ''    End If

            ''End If


            ''ib.CommandName &= "?" & rowView("T_ANEXO_UNO").ToString

            ' ''--------------------------------------
            ' '' Anexo 2
            ' ''--------------------------------------
            ''ib = TryCast(e.Row.Cells(25).Controls(0), ImageButton)
            ''ib.ToolTip = "Subir Archivo Anexo 2"
            ''If Not IsDBNull(rowView("T_ANEXO_DOS")) AndAlso Not rowView("T_ANEXO_DOS").ToString.Equals(String.Empty) Then

            ''    Dim tieneSBM As Boolean = False

            ''    If Not rowView("T_ANEXO_DOS").ToString.Contains("@") Then

            ''        Dim nombreArchivo As String = String.Empty
            ''        If rowView("T_ANEXO_DOS").ToString.Contains("#") AndAlso
            ''                        rowView("T_ANEXO_DOS").ToString.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
            ''            nombreArchivo = rowView("T_ANEXO_DOS").ToString.Substring(0, rowView("T_ANEXO_DOS").ToString.IndexOf("#"))
            ''        Else
            ''            nombreArchivo = rowView("T_ANEXO_DOS").ToString
            ''        End If

            ''        If Path.GetExtension(nombreArchivo).ToLower = ".sbm" OrElse
            ''                Path.GetExtension(nombreArchivo).ToLower = ".sbx" Then
            ''            tieneSBM = True
            ''        End If
            ''    Else
            ''        tieneSBM = True
            ''    End If

            ''    If tieneSBM Then
            ''        ib.ImageUrl = "~/imagenes/archivo-anexo2.gif"
            ''        ib.ToolTip = "Ver Archivo Anexo 2"
            ''    Else
            ''        ib.ImageUrl = "~/imagenes/question_anexos.png"
            ''        ib.ToolTip = "Anexo por encriptar"
            ''    End If

            ''End If

            ''ib.CommandName += "?" + rowView("T_ANEXO_DOS").ToString




            '--------------------------------------
            ' Anexo
            '--------------------------------------
            ib = TryCast(e.Row.Cells(23).Controls(0), ImageButton)
            ib.ToolTip = "Subir Archivo Anexo"
            'Dim tieneSBM As Boolean = False

            'NHM INI - se oculta icono si es dictamen
            If esDictamen Then
                ib.Visible = False
            Else
                If Not IsDBNull(rowView("T_ANEXO_UNO")) AndAlso Not rowView("T_ANEXO_UNO").ToString.Equals(String.Empty) Then

                    'If Not rowView("T_ANEXO_UNO").ToString.Contains("@") Then

                    '    Dim nombreArchivo As String = String.Empty
                    '    If rowView("T_ANEXO_UNO").ToString.Contains("#") AndAlso
                    '                    rowView("T_ANEXO_UNO").ToString.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                    '        nombreArchivo = rowView("T_ANEXO_UNO").ToString.Substring(0, rowView("T_ANEXO_UNO").ToString.IndexOf("#"))
                    '    Else
                    '        nombreArchivo = rowView("T_ANEXO_UNO").ToString
                    '    End If

                    '    If Path.GetExtension(nombreArchivo).ToLower = ".sbm" OrElse
                    '            Path.GetExtension(nombreArchivo).ToLower = ".sbx" Then
                    '        tieneSBM = True
                    '    End If
                    'Else
                    '    tieneSBM = True
                    'End If

                    ib.ImageUrl = "~/imagenes/archivo-anexo1.gif"
                    ib.ToolTip = "Ver Archivo Anexo"

                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False
                End If
            End If
            'NHM FIN
            ib.CommandName &= "?" & rowView("T_ANEXO_UNO").ToString



            '--------------------------------------
            ' Anexo SBM
            '--------------------------------------
            ib = TryCast(e.Row.Cells(24).Controls(0), ImageButton)

            'NHM INI - se oculta icono si es dictamen
            If esDictamen Then
                ib.Visible = False
            Else
                If Not esNoSIE Then

                    ib.ToolTip = "Subir Archivo Anexo Digital"


                    If Not IsDBNull(rowView("T_ANEXO_DOS")) AndAlso Not rowView("T_ANEXO_DOS").ToString.Equals(String.Empty) Then


                        If rowView("T_ANEXO_DOS").ToString.Equals("0") Then
                            ib.ImageUrl = "~/imagenes/question.png"
                            ib.ToolTip = "SBM pendiente de confirmar - Haga click para comprobar."
                        Else
                            ib.ImageUrl = "~/imagenes/archivo-anexo2.gif"
                            ib.ToolTip = "Ver Archivo Anexo Digital"
                        End If

                    ElseIf rblEstatusOficio.SelectedValue = "2" Then
                        ib.Visible = False

                    End If

                    ib.CommandName &= "?" & rowView("T_ANEXO_DOS").ToString


                Else
                    ib.Visible = False
                End If
            End If
            'NHM FIN


            '--------------------------------------
            'Expediente()
            '--------------------------------------
            ib = TryCast(e.Row.Cells(25).Controls(0), ImageButton)
            If esDictamen Then

                ib.ToolTip = "Subir Archivo Expediente"
                If Not IsDBNull(rowView("T_HYP_EXPEDIENTE")) AndAlso Not rowView("T_HYP_EXPEDIENTE").ToString.Equals(String.Empty) Then
                    ib.ImageUrl = "~/imagenes/archivo-expediente.gif"
                    ib.ToolTip = "Ver Archivo Expediente"
                ElseIf rblEstatusOficio.SelectedValue = "2" Then
                    ib.Visible = False
                End If
                ib.CommandName += "?" + rowView("T_HYP_EXPEDIENTE").ToString
            Else
                ib.Visible = False
            End If

            '--------------------------------------
            'Dictaminado_flag
            '--------------------------------------
            lb = TryCast(e.Row.Cells(26).Controls(0), LinkButton)

            If esOficioExterno Then
                If Not IsDBNull(rowView("DICTAMINADO_FLAG")) AndAlso CBool(rowView("DICTAMINADO_FLAG")) AndAlso Not IsDBNull(rowView("T_HYP_ACUSERESPUESTA")) Then
                    lb.Text = "Acuse"
                    lb.CommandName += "?" + rowView("T_HYP_ACUSERESPUESTA").ToString
                End If
            Else
                lb.Visible = False
            End If


            '--------------------------------------
            ' Logo
            '--------------------------------------
            Dim img As Image = Nothing
            img = TryCast(e.Row.Cells(6).FindControl("logoImg"), Image)

            If rowOsiris.Length > 0 Then
                'If Not IsDBNull(rowOsiris(0)("LOGO_ENT")) Then
                If IO.File.Exists(System.IO.Path.GetTempPath() & "ide_" & Session.SessionID & "_" & rowView("ID_ENTIDAD").ToString() & ".gif") Then

                    Try

                        'Dim a As System.Drawing.Image
                        'a = ByteArrayToImage(DirectCast(rowOsiris(0)("LOGO_ENT"), Byte()))
                        'a.Save(System.IO.Path.GetTempPath() & "ide_" & rowView("ID_ENTIDAD").ToString() & ".gif", System.Drawing.Imaging.ImageFormat.Gif)
                        img.ImageUrl = "UserControls/ImageHandlerOficios.ashx?ide=" & Session.SessionID & "_" & rowView("ID_ENTIDAD").ToString() & "&ext=gif"

                    Catch ex As Exception

                        img.Visible = False

                    End Try


                Else

                    img.Visible = False

                End If

            Else

                img.Visible = False

            End If


            '--------------------------------------
            ' Vencimiento
            '--------------------------------------
            img = TryCast(e.Row.Cells(27).Controls(1), Image)
            If Not IsDBNull(rowView("ID_ESTATUS")) Then
                If CInt(rowView("ID_ESTATUS")) = OficioEstatus.Cancelado Then
                    img.ImageUrl = "~/imagenes/ATENDIDO.png"
                    img.AlternateText = "Cancelado"
                    img.ToolTip = "Cancelado"

                ElseIf CInt(rowView("ID_ESTATUS")) = OficioEstatus.Concluido Then
                    img.ImageUrl = "~/imagenes/ATENDIDO.png"
                    img.AlternateText = "Concluido"
                    img.ToolTip = "Concluido"
                Else
                    If Not IsDBNull(rowView("F_FECHA_VENCIMIENTO")) Then
                        '---------------------------------------------------
                        'Llama a la función para calcular estatus del vencimiento
                        '---------------------------------------------------
                        Dim fVencimiento As Date

                        If IsDBNull(rowView("F_FECHA_VENCIMIENTO")) Then
                            fVencimiento = Now.AddYears(1)
                        Else
                            fVencimiento = CType(rowView("F_FECHA_VENCIMIENTO"), Date)
                        End If

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


            TryCast(e.Row.Cells(28).Controls(1), Image).Visible = (esOficioExterno OrElse esDictamen)


            '--------------------------------------
            ' Color texto columna ESTATUS
            '--------------------------------------

            Dim celda As System.Web.UI.WebControls.TableCell = e.Row.Cells(31)
            If Not IsDBNull(rowView("ID_ESTATUS")) Then

                If CInt(rowView("ID_ESTATUS")) = OficioEstatus.Concluido Then

                    celda.ForeColor = Drawing.Color.Blue

                ElseIf CInt(rowView("ID_ESTATUS")) = OficioEstatus.Cancelado Then

                    celda.ForeColor = Drawing.Color.Green
                Else

                    Dim fVencimiento2 As Date

                    If IsDBNull(rowView("F_FECHA_VENCIMIENTO")) Then
                        fVencimiento2 = Now.AddYears(1)
                    Else
                        fVencimiento2 = CType(rowView("F_FECHA_VENCIMIENTO"), Date)
                    End If


                    Select Case LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultaEstatusVencimiento(fVencimiento2)
                        Case "Vencido"
                            celda.ForeColor = Drawing.Color.Red
                        Case Else
                            celda.ForeColor = Drawing.Color.Green

                    End Select

                End If

            Else

                celda.ForeColor = Drawing.Color.Green

            End If


            If ISMODAL Then

                CType(e.Row.Cells(0).Controls(0), LinkButton).Enabled = False

            End If



            '--------------------------------------
            ' Establece la clase para la fila impar
            '--------------------------------------
            'If e.Row.RowIndex Mod 2 = 1 Then e.Row.CssClass = "tr_odd"
            '--------------------------------------
            ' Establece el evento para el double click.
            '--------------------------------------
            If Not IsDBNull(rowView("ID_UNIDAD_ADM")) AndAlso
                Not IsDBNull(rowView("ID_ANIO")) AndAlso
                Not IsDBNull(rowView("I_OFICIO_CONSECUTIVO")) AndAlso
                Not IsDBNull(rowView("ID_TIPO_DOCUMENTO")) Then

                Dim strDblClick As String = String.Format("Id_Area={0}&Id_Oficio={1}&Id_TipDoc={2}&Id_Anio={3}'",
                                                            rowView("ID_UNIDAD_ADM").ToString,
                                                            rowView("I_OFICIO_CONSECUTIVO").ToString,
                                                            rowView("ID_TIPO_DOCUMENTO").ToString,
                                                            rowView("ID_ANIO").ToString)

                Dim dblClickLinkButton As LinkButton = TryCast(e.Row.Cells(30).Controls(0), LinkButton)

                Dim df As String = Page.ClientScript.GetPostBackEventReference(dblClickLinkButton,
                                                                               "rowDoubleClick?" & rowView("ID_ANIO") & "?" &
                                                                               rowView("ID_UNIDAD_ADM") & "?" &
                                                                               rowView("ID_TIPO_DOCUMENTO").ToString & "?" &
                                                                               rowView("I_OFICIO_CONSECUTIVO").ToString & "?" &
                                                                                e.Row.RowIndex)

                e.Row.Attributes("ondblclick") = "document.getElementById('hdnScrollPosition').innerText = $get('GRID').scrollTop; " & df


            End If
        End If

    End Sub

    Private Sub gvBandejaOficios_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvBandejaOficios.RowEditing

        If ISMODAL Then Return

        gvBandejaOficios.EditIndex = e.NewEditIndex
    End Sub

    Private Sub gvBandejaOficios_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBandejaOficios.Sorting

        If ISMODAL Then Return

        Dim dv As DataView = TryCast(Session("consultaBO"), DataTable).DefaultView
        If dv IsNot Nothing Then
            dv.Sort = e.SortExpression & " " & GetSortDirection(e.SortExpression)
            gvBandejaOficios.DataSource = dv
            gvBandejaOficios.DataBind()
        End If
    End Sub

#End Region

    Protected Sub btnAcualizaGridPersonalizado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAcualizaGridPersonalizado.Click
        ManejarColumnasGridView(ModalPersonalizarColumas.PerzonalizaColumasOcultasDataGrid(), ModalPersonalizarColumas.PerzonalizaColumasVisiblesDataGrid())
        SHOW_DICTAMINADO_COLUMN = gvBandejaOficios.Columns(26).Visible
        SHOW_CEDULA_DIGITAL_COLUMN = gvBandejaOficios.Columns(19).Visible
        SHOW_FIRMA_DIGITAL_COLUMN = gvBandejaOficios.Columns(20).Visible
        SHOW_NOTIFICAR = gvBandejaOficios.Columns(15).Visible
        SHOW_NOTIFICACION = gvBandejaOficios.Columns(28).Visible
        SHOW_EXPEDIENTE = gvBandejaOficios.Columns(25).Visible
        SHOW_CEDULA_PDF_COLUMN = gvBandejaOficios.Columns(18).Visible
        SHOW_GENERACION_CEDULA_PDF = gvBandejaOficios.Columns(14).Visible
        Filtraje()
        upGridView.Update()
    End Sub

    Protected Sub rblEstatusOficio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblEstatusOficio.SelectedIndexChanged
        Filtraje()
    End Sub

    Private Sub BtnModalFechaAcuseOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnModalFechaAcuseOk.Click
        If Not FECHA_ACUSE_TEMP = Nothing Then
            showModalFileUpload("T_HYP_ACUSERESPUESTA")
        Else
            modalPopupFechaAcuse.Hide()
            modalMensaje("Debe seleccionar una fecha")
        End If
    End Sub

    Private Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        FECHA_ACUSE_TEMP = Calendar1.SelectedDate
        modalPopupFechaAcuse.Show()
    End Sub

    Private Sub Calendar1_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged
        modalPopupFechaAcuse.Show()
    End Sub

    Private Sub BtnModalOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnModalOk.Click
        If lblModalPostBack.Text = "CorreoNotificacion" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Outlook", ViewState("TempSCRIPTOUTLOOK").ToString, True)
            '---------------------------------------------
            'actualizar estatus de la notificación
            '---------------------------------------------
            If BusinessRules.BDA_OFICIO.ActualizarNotificacionPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, True) > 0 Then
                modalMensaje("Notificación Generada", "NotificacionGenerada", "INFORMACIÓN")
            End If
        ElseIf lblModalPostBack.Text = "NotificacionGenerada" Then
            Filtraje()
            upGridView.Update()
        End If
    End Sub

    ''' <summary>
    ''' Genera cedula notificacion AGC
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAceptarCedula_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarCedula.Click
        Try

            If RegularExpressionValidatorFechaCedula.IsValid Then
                GenerarCedulaElectronica()
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "cierraModalCedula", "closePopupCedula();", True)
            Else
                ModalCedula.Show()
            End If

        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
            '---------------------------------------------

        Finally

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "arrowCursor", "removeHourglass();", True)

        End Try
    End Sub
#End Region

#Region "Métodos"

    Private Sub showGenerarCedulaPDF()
        Try
            If Not verificaUsuario() Then Throw New ApplicationException("Documento de sólo lectura")

            'CargarCombo(ddlNotificador, BusinessRules.BDA_FIRMA_ELECTRONICA.ConsultarNotificadoresJerarquia(TOP_ID_UNIDAD_ADM_USUARIO), "NOMBRE", "USUARIO")
            CargarCombo(ddlNotificador, LogicaNegocioSICOD.FirmaElectronica.GetNotificadoresJerarquia(TOP_ID_UNIDAD_ADM_USUARIO), "NOMBRE", "USUARIO")

            txtFechaCedula.Text = Date.Now.ToShortDateString()
            txtHora.Text = Date.Now.Hour.ToString()
            txtMin.Text = Date.Now.Minute.ToString()
            txtHora.Text = Microsoft.VisualBasic.Format(CType(txtHora.Text, Integer), "00")
            txtMin.Text = Microsoft.VisualBasic.Format(CType(txtMin.Text, Integer), "00")
            lblTituloModal.Style.Add("display", "block")
            lblTituloModal.Text = "Datos para la Cédula Electrónica"
            upCedula.Update()
            ModalCedula.Show()
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Manejar el doble click en cada fila del gridview
    ''' </summary>
    ''' <param name="strRequest"></param>
    ''' <remarks></remarks>
    Private Sub handleRowDoubleClick(ByVal strRequest As String())
        Session("ID_ANIO") = strRequest(1)
        Session("ID_UNIDAD_ADM") = strRequest(2)
        Session("ID_TIPO_DOCUMENTO") = strRequest(3)
        Session("I_OFICIO_CONSECUTIVO") = strRequest(4)

        If Not ISMODAL Then

            AsignaValoresScrollRowIndex(CInt(strRequest(5)))

            Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1", True)

        Else

            Session(BusinessRules.BDA_OFICIO.SessionAtencionResult) = String.Format("{0}|{1}|{2}|{3}*", strRequest(2), strRequest(3), strRequest(1), strRequest(4))

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

        End If


    End Sub


    ''' <summary>
    ''' Evento Click del botón lnkdl que abre el archivo (al estar fuera del update panel del gridview)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DownloadFile(ByVal sender As Object, ByVal e As EventArgs)
        abreArchivoLink(ARCHIVO_ABRIR)
    End Sub

    ''' <summary>
    ''' Mandar llamar a una petición de filtraje y mostrar los resultados en el gridview.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Filtraje()


        Dim con As New Conexion
        Dim dtb As New DataTable


        If Not hdn_width.Value = "" Then
            GRID.Style.Item("width") = (CInt(hdn_width.Value) - 10) & "px"
            'GRID.Style.Item("width") = CInt(hdn_width.Value) & "px"
            upGridView.Update()
        End If


        Try

            Dim strWhere As String = " 1=1"
            Dim hasUnidadAdm As Boolean = False
            Dim esOficioExterno As Boolean = False
            Dim esDictamen As Boolean = False
            Dim hasRangoConsecutivo As Boolean = False
            Dim list As List(Of String) = ucFiltro1.getFilterSelection
            Dim FiltroParaSesionInner As New Dictionary(Of String, String)
            Dim esTodos As Boolean = False

            For Each listItem As String In list

                If listItem.Contains("ID_CLASIFICACION=") Then
                    strWhere += " AND  CL." + listItem

                ElseIf listItem.Contains("ID_TIPO_DOCUMENTO=") Then

                    If Not listItem = "ID_TIPO_DOCUMENTO=-1" Then

                        strWhere += " AND T." + listItem
                        '------------------------------------
                        ' If tipo documento es oficio externo (1) mostrar columna de dictaminado, ocultar de lo contrario
                        '------------------------------------
                        If listItem = "ID_TIPO_DOCUMENTO=1" Then esOficioExterno = True
                        If listItem = "ID_TIPO_DOCUMENTO=2" Then esDictamen = True
                    Else

                        esTodos = True

                    End If


                ElseIf listItem.Contains("ID_ESTATUS=") Then
                    strWhere += " AND E." + listItem

                ElseIf listItem.Contains("ID_UNIDAD_ADM=") Then
                    strWhere += " AND A." + listItem
                    hasUnidadAdm = True
                ElseIf listItem.Contains("ID_ENTIDAD=") Then
                    strWhere += " AND O." + listItem

                ElseIf listItem.Contains("ID_PERSONA=") Then
                    strWhere += " AND P." + listItem

                Else
                    If Not String.IsNullOrEmpty(listItem) Then
                        strWhere += " AND O." + listItem
                        If listItem.Contains("I_OFICIO_CONSECUTIVO") Then
                            hasRangoConsecutivo = True
                        End If
                    End If

                End If
            Next

            If hasRangoConsecutivo And Not hasUnidadAdm Then Throw New SystemException("Debe filtrar por Área Administrativa para aplicar el rango")

            '-------------------------------------
            ' si checkbox de sólo míos está seleccionada.
            '-------------------------------------
            Dim injectQuery As String = String.Empty
            If chkSoloMios.Checked Then injectQuery = getSoloMiosQuery()
            FiltroParaSesionInner.Add("chkSoloMios", IIf(chkSoloMios.Checked, "1", "0"))

            Dim MAX_RESULTS As String = String.Empty
            If ddlVerUltimos.SelectedIndex = 0 Then
                MAX_RESULTS = " TOP " & WebConfigurationManager.AppSettings("MAX_RESULTADOS_BANDEJA")
            End If
            FiltroParaSesionInner.Add("ddlVerUltimos", ddlVerUltimos.SelectedValue)

            Dim sql As String = String.Empty


            '' En caso de invocarse desde SISAN, se filtra solo por el numero de oficio
            If Not IsNothing(Session("IdOficioSISAN")) Then

                strWhere = "O.T_OFICIO_NUMERO IN (" & Session("IdOficioSISAN").ToString() & ")"
                injectQuery = ""

            End If

            If Not IsNothing(Session("IdOficioSEPRIS")) Then

                strWhere = "O.T_OFICIO_NUMERO IN ('" & Session("IdOficioSEPRIS").ToString() & "')"
                injectQuery = ""

            End If



            sql = "SELECT " + MAX_RESULTS + " A.DSC_UNIDAD_ADM " +
                " ,A.ID_UNIDAD_ADM " +
                " ,O.T_OFICIO_NUMERO " +
                " ,O.T_ASUNTO " +
                " ,UE.USUARIO AS ELABORO " +
                " ,UA.USUARIO AS REGISTRO " +
                " ,T_TIPO_DOCUMENTO " +
                " ,(CASE WHEN ISNULL(P.DESTINATARIO, '') <> '' THEN P.DESTINATARIO ELSE (CASE WHEN O.T_DESTINATARIO IS NOT NULL THEN O.T_DESTINATARIO " +
                " ELSE ISNULL(F.T_FUNCION, '') END) END) DESTINATARIO " +
                " ,E.T_ESTATUS " +
                " ,O.ID_ESTATUS " +
                " ,CONVERT(VARCHAR(10), O.F_FECHA_RECEPCION,103) AS F_FECHA_RECEPCION " +
                " ,CONVERT(VARCHAR(10), O.F_FECHA_OFICIO,103) AS F_FECHA_OFICIO " +
                " ,CONVERT(VARCHAR(10), O.F_FECHA_ACUSE,103) AS F_FECHA_ACUSE " +
                " ,CONVERT(VARCHAR(10), O.F_FECHA_VENCIMIENTO,103) AS F_FECHA_VENCIMIENTO " +
                " ,O.DICTAMINADO_FLAG " +
                " ,O.NOTIF_ELECTRONICA_FLAG " +
                " ,'' T_ENTIDAD_CORTO " +
                " ,'' T_LOGO " +
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
                " ,O.IS_FILE_FLAG " +
                " ,O.T_DESTINATARIO " +
                " ,ISNULL(F.T_FUNCION, '') T_FUNCION " +
                " ,O.ID_ENTIDAD , O.ID_ENTIDAD_TIPO, O.ID_SUBENTIDAD, O.FIRMA_SIE_FLAG " +
                " FROM BDA_OFICIO O " +
                injectQuery +
                " INNER JOIN BDA_C_UNIDAD_ADM A ON A.ID_UNIDAD_ADM = O.ID_AREA_OFICIO " +
                " LEFT OUTER JOIN BDS_USUARIO UE ON UE.USUARIO = O.USUARIO_ELABORO " +
                " LEFT OUTER JOIN BDS_USUARIO UA ON UA.USUARIO = O.USUARIO_ALTA " +
                " LEFT OUTER JOIN BDA_TIPO_DOCUMENTO T ON T.ID_TIPO_DOCUMENTO = O.ID_TIPO_DOCUMENTO " +
                " LEFT OUTER JOIN (SELECT ISNULL(XP.T_NOMBRE,'')  + ' ' + ISNULL(XP.T_APELLIDO_P,'') + ' ' + ISNULL(XP.T_APELLIDO_M,'') AS DESTINATARIO, XP.ID_PERSONA " +
                " FROM BDA_PERSONAL XP) P ON P.ID_PERSONA=O.ID_DESTINATARIO " +
                " LEFT OUTER JOIN BDA_ESTATUS_OFICIO E ON E.ID_ESTATUS = O.ID_ESTATUS " +
                " LEFT OUTER JOIN BDA_CLASIFICACION_OFICIO CL ON CL.ID_CLASIFICACION = O.ID_CLASIFICACION " +
                " LEFT OUTER JOIN BDA_FUNCION F ON O.ID_PUESTO_DESTINATARIO = f.ID_FUNCION " +
                " WHERE " + strWhere + " AND A.ID_T_UNIDAD_ADM = 2 "


            'ID_T_UNIDAD_ADM
            '-----------------------------------------
            'Asegurarse de quitar "cancelados/terminados" de los resultados en base a la selección
            ' del radiobuttonlist "pendientes/terminados"
            '-----------------------------------------
            If rblEstatusOficio.SelectedValue = "1" Then
                sql += " AND O.ID_ESTATUS NOT IN (" & OficioEstatus.Concluido & "," & OficioEstatus.Cancelado & ") "
            Else
                sql += " AND O.ID_ESTATUS IN (" & OficioEstatus.Concluido & "," & OficioEstatus.Cancelado & ") "
            End If
            FiltroParaSesionInner.Add("rblEstatusOficio", rblEstatusOficio.SelectedValue)



            Session(SessionFiltrosInner) = Nothing
            Session(SessionFiltrosInner) = FiltroParaSesionInner



            '' en caso de solicitarse solo libres de expediente
            If Request.QueryString("le") IsNot Nothing Then

                sql += " AND O.T_OFICIO_NUMERO NOT IN (SELECT a1.T_OFICIO_NUMERO FROM BDA_OFICIO a1 "
                sql += " INNER JOIN BDA_R_EXPEDIENTE_OFICIO b1 ON a1.ID_AREA_OFICIO = b1.ID_AREA_OFICIO "
                sql += " AND a1.ID_TIPO_DOCUMENTO = b1.ID_TIPO_DOCUMENTO AND a1.ID_ANIO = b1.ID_ANIO "
                sql += " AND a1.I_OFICIO_CONSECUTIVO = b1.I_OFICIO_CONSECUTIVO AND b1.VIG_FLAG = 1) "

            End If


            If Request.QueryString("cl") IsNot Nothing Then

                sql += " AND O.ID_ESTATUS = " & OficioEstatus.Concluido & " "

            End If


            sql += " ORDER BY O.F_FECHA_OFICIO DESC, O.I_OFICIO_CONSECUTIVO DESC"




            EventLogWriter.EscribeEntrada(sql, EventLogEntryType.Information)
            con.ConsultaAdapter(sql).Fill(dtb)

            EventLogWriter.EscribeEntrada("Consulta realizada: " + dtb.Rows.Count.ToString() + "Registros.", EventLogEntryType.Information)

            '' Verificamos variable de sesion de entidades desde osiris
            VerificaCargaEntidadesOsiris()


            '-----------------------------------------
            ' Guardar en sesión los resultados del filtraje.
            '-----------------------------------------
            Session("consultaBO") = dtb


            ' Poblamos GridView
            If dtb.Rows.Count > 0 Then
                gvBandejaOficios.DataSource = dtb
                gvBandejaOficios.DataBind()


                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "dictaminado"
                '-----------------------------------------
                If SHOW_DICTAMINADO_COLUMN Then
                    If esOficioExterno OrElse esTodos Then
                        gvBandejaOficios.Columns(26).Visible = True
                    Else
                        gvBandejaOficios.Columns(26).Visible = False
                    End If

                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Generar Cédula PDF"
                '-----------------------------------------
                If SHOW_GENERACION_CEDULA_PDF Then

                    If rblEstatusOficio.SelectedValue = "2" Then

                        gvBandejaOficios.Columns(14).Visible = False

                    Else

                        If esOficioExterno OrElse esDictamen OrElse esTodos Then
                            gvBandejaOficios.Columns(14).Visible = True
                        Else
                            gvBandejaOficios.Columns(14).Visible = False
                        End If

                    End If
                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Cédula PDF"
                '-----------------------------------------
                If SHOW_CEDULA_PDF_COLUMN Then
                    If esOficioExterno OrElse esDictamen OrElse esTodos Then
                        gvBandejaOficios.Columns(18).Visible = True
                    Else
                        gvBandejaOficios.Columns(18).Visible = False
                    End If

                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Cédula SBM"
                '-----------------------------------------
                If SHOW_CEDULA_DIGITAL_COLUMN Then
                    If esOficioExterno OrElse esDictamen OrElse esTodos Then
                        gvBandejaOficios.Columns(19).Visible = True
                    Else
                        gvBandejaOficios.Columns(19).Visible = False
                    End If

                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Firma SBM"
                '-----------------------------------------
                If SHOW_FIRMA_DIGITAL_COLUMN Then
                    If esOficioExterno OrElse esDictamen OrElse esTodos Then
                        gvBandejaOficios.Columns(20).Visible = True
                    Else
                        gvBandejaOficios.Columns(20).Visible = False
                    End If
                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Expediente"
                '-----------------------------------------
                If SHOW_EXPEDIENTE Then
                    If esDictamen OrElse esTodos Then
                        gvBandejaOficios.Columns(25).Visible = True
                    Else
                        'NHM INI - TMP
                        'gvBandejaOficios.Columns(25).Visible = False
                        gvBandejaOficios.Columns(25).Visible = True
                        'NHM FIN
                    End If
                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Notificación"
                '-----------------------------------------
                If SHOW_NOTIFICACION Then
                    If esOficioExterno OrElse esDictamen OrElse esTodos Then
                        gvBandejaOficios.Columns(28).Visible = True
                    Else
                        gvBandejaOficios.Columns(28).Visible = False
                    End If
                End If

                '-----------------------------------------
                ' Determinar si se debe mostrar la columna de "Notificar"
                '-----------------------------------------
                If SHOW_NOTIFICAR AndAlso PUEDE_NOTIFICAR Then
                    If esOficioExterno OrElse esDictamen OrElse esTodos Then
                        gvBandejaOficios.Columns(15).Visible = True
                    Else
                        gvBandejaOficios.Columns(15).Visible = False
                    End If
                Else
                    gvBandejaOficios.Columns(15).Visible = False
                End If

            End If

            '-----------------------------------------
            ' Ocultar o mostrar botón superior de Exportación a Excel
            ' (debido a que está fuera del UpdatePanel pero ligado como Trigger no se puede ocultar con la propiedad Visible).
            '-----------------------------------------
            Dim hideBtnExportarExcel As New StringBuilder
            hideBtnExportarExcel.Append("var _btnExportarExcel = document.getElementById('" & btnExportarExcelTop.ClientID & "');")
            hideBtnExportarExcel.Append("if (_btnExportarExcel != null)")
            hideBtnExportarExcel.Append("{")
            If dtb.Rows.Count > 0 Then

                If Not ISMODAL Then
                    hideBtnExportarExcel.Append("_btnExportarExcel.style.visibility='visible';")
                Else
                    hideBtnExportarExcel.Append("_btnExportarExcel.style.visibility='hidden';")
                End If
                If Not ISMODAL Then btnExcel.Style.Remove("display")

                ''pnlVencimiento.Style.Remove("display")
                Panel2.Style.Remove("display")
                Panel1.Style.Remove("display")
                pnlImagenNoExisten.Style.Add("display", "none")
                GRID.Style.Remove("display")
                tbl_footer.Style.Remove("display")
            Else

                hideBtnExportarExcel.Append("_btnExportarExcel.style.visibility='hidden';")
                btnExcel.Style.Add("display", "none")
                ''pnlVencimiento.Style.Add("display", "none")
                Panel2.Style.Add("display", "none")
                Panel1.Style.Add("display", "none")
                pnlImagenNoExisten.Style.Remove("display")
                GRID.Style.Add("display", "none")
                tbl_footer.Style.Add("display", "none")
            End If
            hideBtnExportarExcel.Append("}")
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ocultarBotonExportar", hideBtnExportarExcel.ToString, True)


            '---------------------------------------------------
            ' Mostrar Grid y footer (íconos de descripción)
            ' Ocultar imagen gif "procesando"
            '---------------------------------------------------
            Imagen_procesando.Style.Add("display", "none")
            'GRID.Style.Remove("display")
            'tbl_footer.Style.Remove("display")


        Catch ex As SystemException
            modalMensaje(ex.Message)
        End Try

    End Sub

#Region "Mensaje Modal"

    ''' <summary>
    ''' Modalmensaje
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <param name="PostBackCall"></param>
    ''' <param name="Titulo"></param>
    ''' <param name="showCancelButton"></param>
    ''' <param name="AcceptButtonText"></param>
    ''' <param name="CancelButtonText"></param>
    ''' <remarks></remarks>
    Private Sub modalMensaje(
                               ByVal mensaje As String, Optional ByVal PostBackCall As String = "",
                                   Optional ByVal Titulo As String = "ALERTA",
                                       Optional ByVal showCancelButton As Boolean = False,
                                           Optional ByVal AcceptButtonText As String = "Aceptar",
                                               Optional ByVal CancelButtonText As String = "Cancelar"
                                                                                               )

        lblErroresTitulo.Style.Add("display", "block")
        lblErroresTitulo.Text = Titulo
        lblErroresPopup.Text = "<ul><li>" & mensaje & "</li></ul>"
        lblErroresPopup.Style.Add("display", "block")
        lblModalPostBack.Text = PostBackCall
        BtnModalOk.Text = AcceptButtonText

        'BtnCancelarModal.Visible = showCancelButton
        If showCancelButton Then
            BtnCancelarModal.CssClass = "botones"
        Else
            BtnCancelarModal.CssClass = "hide"
        End If

        BtnCancelarModal.Text = CancelButtonText
        up2.Update()
        ModalPopupExtenderErrores.Show()

    End Sub

#End Region

    ''' <summary>
    ''' Procesar petición de archivo al dar click en botón del gridview (adjuntar o descargar).
    ''' </summary>
    ''' <param name="tipoArchivo"></param>
    ''' <param name="nombreArchivo"></param>
    ''' <remarks></remarks>
    ''' NHM INI
    ''' Private Sub processFileRequest(ByVal tipoArchivo As String, ByVal nombreArchivo As String)
    ''' NHM FIN
    Private Sub processFileRequest(ByVal tipoArchivo As String, ByVal nombreArchivo As String, ByVal idTipoDocumento As Int32)
        Try

            If nombreArchivo Is String.Empty Then
                '-----------------------------------------------
                ' Verificar que el usuario pueda adjuntar
                ' en base a que haya registrado (dado de alta), firmado, rubricado o elaborado el Oficio.
                ' o en caso que sea asistente y que su jefe haya hecho o sea parte de las funciones anteriores.
                '-----------------------------------------------
                If verificaUsuario() Then
                    'NHM - Agrega parametro: idTipoDocumento al metodo
                    'If onReglasDeNegocioOK(tipoArchivo) Then
                    If onReglasDeNegocioOK(tipoArchivo, idTipoDocumento) Then
                        TIPO_ARCHIVO_ADJUNTAR = tipoArchivo
                        If TIPO_ARCHIVO_ADJUNTAR = "T_HYP_ACUSERESPUESTA" Then
                            modalPopupFechaAcuse.Show()
                        Else
                            showModalFileUpload(TIPO_ARCHIVO_ADJUNTAR)
                        End If
                    Else
                        Throw New ApplicationException("Otros archivos deben ser generados primero")
                    End If
                Else
                    Throw New ApplicationException("Documento de sólo lectura.")
                End If
            Else

                '---------------------------------
                ' Poner en session el nombre del archivo a abrir.
                ' Manda llamar a través de javascript el evento click del linkutton lnkdl (DownloadFile)
                ' el cuál está fuera del UpdatePanel y permite descargar el archivo.
                '---------------------------------
                Dim enc As New YourCompany.Utils.Encryption.Encryption64
                Dim ruta As String = String.Empty
                Dim UsuarioSp As String = AppSettings("UsuarioSp")
                Dim PassEncSp As String = enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR")
                Dim Domain As String = enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR")
                Dim ServidorSp As String = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR")
                Dim bibliotecaSp As String = enc.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")
                Dim isFile As Boolean = False
                Dim NombreArchivoPaso As String

                Dim credentials As NetworkCredential = Nothing
                credentials = New NetworkCredential(UsuarioSp, PassEncSp, Domain)

                Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                Dim fechaVencimientoValidacion As String = "NULL"
                If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                    fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                End If

                Dim objOficio As New Entities.BDA_OFICIO
                objOficio.IdAnio = ID_ANIO
                objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
                objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO
                objOficio.IdArea = ID_UNIDAD_ADM

                objOficio.UsuarioElaboro = USUARIO
                objOficio.Comentario = fechaVencimientoValidacion

                '---------------------------------------------
                ' Revisa si el archivo tiene la bandera es file o es ruta Sharepoint
                '---------------------------------------------
                If nombreArchivo.Contains("#") AndAlso nombreArchivo.ToLower.Contains(WebConfigurationManager.AppSettings("FILES_PATH").ToLower.ToString) Then

                    '------------------------------------------------------
                    ' Sólo en caso que debamos enviar la ruta al portapapeles
                    '------------------------------------------------------
                    nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.IndexOf("#"))
                    ruta = nombreArchivo

                    '------------------------------------------------------
                    ' Verificar que el archivo exista.
                    '------------------------------------------------------
                    If Not File.Exists(ruta) Then Throw New ApplicationException("Archivo no existe o permisos insuficientes")
                    isFile = True
                Else
                    '------------------------------------------------------
                    ' Sólo en caso que debamos enviar la ruta al portapapeles
                    '------------------------------------------------------
                    ruta = enc.DecryptFromBase64String(AppSettings("PathDocumentosBrigde"), "webCONSAR")
                    ruta &= "\" & nombreArchivo


                    '------------------------------------------------------
                    ' Si el archivo no es SBM, verificar que exista.
                    '------------------------------------------------------
                    If Not tipoArchivo = "T_CEDULADIGITAL" AndAlso
                            Not tipoArchivo = "T_HYP_FIRMADIGITAL" AndAlso
                            Not tipoArchivo = "T_ANEXO_DOS" Then
                        'Not tipoArchivo = "T_ANEXO_UNO" AndAlso

                        NombreArchivoPaso = nombreArchivo
                        If tipoArchivo = "T_ANEXO_UNO" AndAlso nombreArchivo.Contains("@") Then
                            NombreArchivoPaso = nombreArchivo.Substring(0, nombreArchivo.IndexOf("@"))
                        End If

                        If Not UrlExiste(ServidorSp & "/" & bibliotecaSp & "/" & NombreArchivoPaso, credentials) Then
                            Throw New ApplicationException("Archivo no existe o permisos insuficientes")
                        End If

                    End If
                End If

                If ID_TIPO_DOCUMENTO = OficioTipo.Oficio_Externo Then
                    Dim ExtensionOriginal As String = ""
                    If tipoArchivo = "T_HYP_ARCHIVOSCAN" Or tipoArchivo = "T_HYP_CEDULAPDF" Then

                        ruta = ruta.Replace("\", "\\")

                        '---------------------------------------------
                        ' Copia los archivos al portapapels (sólo funciona en IE)
                        '---------------------------------------------
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "copyToClipboard", "copyToClipboard('" + ruta + "');", True)

                        '---------------------------------------------
                        ' Establece el ícono de interrogante sobre el archivo SBM
                        '---------------------------------------------
                        If tipoArchivo = "T_HYP_ARCHIVOSCAN" AndAlso
                                Not BusinessRules.BDA_OFICIO.ConsultarTieneFirmaSBM(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then

                            objOficio.ArchivoFirmaDigital = "0"
                            BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)

                            ' Copiamos archivo a ruta puente para que pueda extraerlo el websec
                            If Not CopiaArchivoBridge(nombreArchivo) Then Throw New ApplicationException("Error al depositar el archivo para el websec")

                        ElseIf tipoArchivo = "T_HYP_CEDULAPDF" AndAlso
                                Not BusinessRules.BDA_OFICIO.ConsultarTieneCedulaSBM(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then

                            objOficio.ArchivoCedulaDigital = "0"
                            BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)

                            ' Copiamos archivo a ruta puente para que pueda extraerlo el websec
                            If Not CopiaArchivoBridge(nombreArchivo) Then Throw New ApplicationException("Error al depositar el archivo para el websec")

                        End If

                        Filtraje()
                        upGridView.Update()

                    ElseIf tipoArchivo = "T_CEDULADIGITAL" OrElse tipoArchivo = "T_HYP_FIRMADIGITAL" Then

                        If nombreArchivo = "0" Then

                            If tipoArchivo = "T_HYP_FIRMADIGITAL" Then


                                ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_ARCHIVOSCAN").ToString)
                                VerificaArchivoSBM(tipoArchivo, objOficio, ExtensionOriginal)

                                Dim proposedFileName As String = crearNombreArchivo("dummy.sbm", "T_HYP_FIRMADIGITAL")
                                Dim proposedUrl As String = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName

                                If UrlExiste(proposedUrl, credentials) Then
                                    objOficio.ArchivoFirmaDigital = proposedFileName
                                    BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)
                                    nombreArchivo = proposedFileName
                                Else


                                    proposedFileName = crearNombreArchivo("dummy.sbmx", "T_HYP_FIRMADIGITAL", ExtensionOriginal)
                                    proposedUrl = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName

                                    If UrlExiste(proposedUrl, credentials) Then

                                        objOficio.ArchivoFirmaDigital = proposedFileName
                                        BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)
                                        nombreArchivo = proposedFileName

                                    Else

                                        Throw New ApplicationException("No se ha creado el archivo SBM de la firma")

                                    End If


                                End If
                            ElseIf tipoArchivo = "T_CEDULADIGITAL" Then

                                ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_CEDULAPDF").ToString)
                                VerificaArchivoSBM(tipoArchivo, objOficio, ExtensionOriginal)

                                Dim proposedFileName As String = crearNombreArchivo("dummy.sbm", "T_CEDULADIGITAL")
                                Dim proposedUrl As String = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName

                                If UrlExiste(proposedUrl, credentials) Then
                                    objOficio.ArchivoCedulaDigital = proposedFileName
                                    BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)
                                    nombreArchivo = proposedFileName
                                Else

                                    proposedFileName = crearNombreArchivo("dummy.sbmx", "T_CEDULADIGITAL", ExtensionOriginal)
                                    proposedUrl = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName

                                    If UrlExiste(proposedUrl, credentials) Then

                                        objOficio.ArchivoCedulaDigital = proposedFileName
                                        BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)
                                        nombreArchivo = proposedFileName

                                    Else

                                        Throw New ApplicationException("No se ha creado el archivo SBM de la cédula")

                                    End If



                                End If
                            End If
                            Filtraje()
                            upGridView.Update()
                        End If

                    ElseIf tipoArchivo = "T_ANEXO_UNO" Then

                        If nombreArchivo.Contains("@") Then
                            '--------------------------------------------------
                            ' Ya tiene SBM
                            '--------------------------------------------------
                            nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.IndexOf("@"))
                            ruta = enc.DecryptFromBase64String(AppSettings("PathDocumentosBrigde"), "webCONSAR") & nombreArchivo

                        End If

                        '---------------------------------------------
                        ' Copia los archivos al portapapels (sólo funciona en IE)
                        '---------------------------------------------
                        ruta = ruta.Replace("\", "\\")
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "copyToClipboard", "copyToClipboard('" + ruta + "');", True)

                        tipoArchivo = "T_ANEXO_UNO"
                        ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_ANEXO_UNO").ToString)
                        VerificaArchivoSBM(tipoArchivo, objOficio, ExtensionOriginal)

                        Dim sbmExists As Boolean = False

                        sbmExists = BusinessRules.BDA_OFICIO.ConsultarTieneAnexoSBM(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)


                        If Not sbmExists Then

                            objOficio.ArchivoAnexo2 = "0"
                            BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio, False)

                            ' Copiamos archivo a ruta puente para que pueda extraerlo el websec
                            If Not CopiaArchivoBridge(nombreArchivo) Then Throw New ApplicationException("Error al depositar el archivo para el websec")

                        End If

                        Filtraje()
                        upGridView.Update()

                    ElseIf tipoArchivo = "T_ANEXO_DOS" Then

                        'tipoArchivo = "T_ANEXO_UNO"
                        If 1 = 0 Then 'nombreArchivo.Contains("@") Then

                            '--------------------------------------------------
                            ' Ya tiene SBM
                            '--------------------------------------------------
                            nombreArchivo = nombreArchivo.Substring(nombreArchivo.IndexOf("@") + 1, nombreArchivo.Length - (nombreArchivo.IndexOf("@") + 1))
                            ruta = enc.DecryptFromBase64String(AppSettings("PathDocumentosBrigde"), "webCONSAR") & nombreArchivo

                        Else
                            ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_ANEXO_UNO").ToString)

                            tipoArchivo = "T_ANEXO_UNO"
                            VerificaArchivoSBM(tipoArchivo, objOficio, ExtensionOriginal)

                            Dim sbmExists As Boolean = False

                            Dim proposedFileName As String = crearNombreArchivo("dummy.sbm", tipoArchivo)
                            If isFile Then

                                '--------------------------------------------------
                                ' Verificar si ya está el SBM en Consarfile
                                '--------------------------------------------------
                                sbmExists = File.Exists(proposedFileName)

                                If Not sbmExists Then

                                    proposedFileName = crearNombreArchivo("dummy.sbmx", tipoArchivo, ExtensionOriginal)
                                    sbmExists = File.Exists(proposedFileName)

                                End If

                            Else
                                '--------------------------------------------------
                                ' Verificar si ya está el SBM en Sharepoint
                                '--------------------------------------------------
                                Dim proposedUrl As String = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName
                                sbmExists = UrlExiste(proposedUrl, credentials)

                                If Not sbmExists Then

                                    proposedFileName = crearNombreArchivo("dummy.sbmx", tipoArchivo, ExtensionOriginal)
                                    proposedUrl = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName
                                    sbmExists = UrlExiste(proposedUrl, credentials)

                                End If

                            End If

                            If sbmExists Then

                                If isFile Then
                                    objOficio.ArchivoAnexo2 = nombreArchivo & "#" & nombreArchivo & "@" & proposedFileName
                                Else
                                    objOficio.ArchivoAnexo2 = proposedFileName 'nombreArchivo & "@" & proposedFileName
                                End If
                                BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio, False)
                                nombreArchivo = proposedFileName
                            Else
                                Throw New ApplicationException("No se ha creado el archivo SBM del Anexo")
                            End If

                            Filtraje()
                            upGridView.Update()

                        End If


                        'ElseIf tipoArchivo = "T_ANEXO_UNO" OrElse tipoArchivo = "T_ANEXO_DOS" Then

                        '    If nombreArchivo.Contains("@") Then

                        '        '--------------------------------------------------
                        '        ' Ya tiene SBM
                        '        '--------------------------------------------------
                        '        nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.IndexOf("@"))
                        '        ruta = enc.DecryptFromBase64String(AppSettings("PathDocumentosBrigde"), "webCONSAR") & nombreArchivo

                        '    Else

                        '        VerificaArchivoSBM(tipoArchivo, objOficio)


                        '        '--------------------------------------------------
                        '        ' No tiene SBM
                        '        ' Verifica si existe SBM para el anexo
                        '        '------------------------------------------------
                        '        Dim sbmExists As Boolean = False
                        '        Dim proposedFileName As String = Path.GetFileNameWithoutExtension(ruta) & ".sbm"
                        '        If isFile Then

                        '            '--------------------------------------------------
                        '            ' Verificar si ya está el SBM en Consarfile
                        '            '--------------------------------------------------
                        '            sbmExists = File.Exists(proposedFileName)
                        '        Else
                        '            '--------------------------------------------------
                        '            ' Verificar si ya está el SBM en Sharepoint
                        '            '--------------------------------------------------
                        '            Dim proposedUrl As String = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName
                        '            sbmExists = UrlExiste(proposedUrl, credentials)
                        '        End If

                        '        If sbmExists Then
                        '            If tipoArchivo = "T_ANEXO_UNO" Then

                        '                If isFile Then
                        '                    objOficio.ArchivoAnexo1 = nombreArchivo & "#" & nombreArchivo & "@" & proposedFileName
                        '                Else
                        '                    objOficio.ArchivoAnexo1 = nombreArchivo & "@" & proposedFileName
                        '                End If
                        '                BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo1(objOficio)
                        '            ElseIf tipoArchivo = "T_ANEXO_DOS" Then
                        '                If isFile Then
                        '                    objOficio.ArchivoAnexo2 = nombreArchivo & "#" & nombreArchivo & "@" & proposedFileName
                        '                Else
                        '                    objOficio.ArchivoAnexo2 = nombreArchivo & "@" & proposedFileName
                        '                End If
                        '                BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio)
                        '            End If

                        '            Filtraje()
                        '            upGridView.Update()
                        '        Else
                        '            '---------------------------------------------
                        '            ' Copia los archivos al portapapels (sólo funciona en IE)
                        '            '---------------------------------------------
                        '            ruta = ruta.Replace("\", "\\")
                        '            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "copyToClipboard", "copyToClipboard('" + ruta + "');", True)

                        '            ' Copiamos archivo a ruta puente para que pueda extraerlo el websec
                        '            If Not CopiaArchivoBridge(nombreArchivo) Then Throw New ApplicationException("Error al depositar el archivo para el websec")


                        '        End If
                        '    End If
                    End If
                End If

                ARCHIVO_ABRIR = nombreArchivo
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "downloadFile", "clickDownloadButton()", True)
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message, "", "Información")
        End Try
    End Sub

    ''' <summary>
    ''' Revisa las reglas de negocio
    ''' </summary>
    ''' <param name="tipoArchivo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' NHM INI
    ''' Private Function onReglasDeNegocioOK(ByVal tipoArchivo As String) As Boolean
    ''' NHM FIN
    Private Function onReglasDeNegocioOK(ByVal tipoArchivo As String, ByVal idTipoDocumento As Int32) As Boolean

        Select Case tipoArchivo

            Case "T_CEDULADIGITAL"
                '----------------------------------------
                ' No se puede generar cédula digital sin cédula
                '----------------------------------------
                Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoCedula(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Case "T_HYP_CEDULAPDF"
                '----------------------------------------
                ' No se puede generar cédula sin PDF
                '----------------------------------------
                Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Case "T_HYP_FIRMADIGITAL"
                '----------------------------------------
                ' No se puede generar la firma digital sin PDF
                '----------------------------------------
                Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Case "T_ANEXO_DOS"
                '----------------------------------------
                ' No se puede generar el anexo digital sin el anexo
                '----------------------------------------
                Return Not String.IsNullOrEmpty(BusinessRules.BDA_OFICIO.ConsultaDocAnexo1(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO))

            Case "T_HYP_RESPUESTAOFICIO"
                Return True
            Case "T_HYP_ACUSERESPUESTA"
                '----------------------------------------
                ' No se puede generar el acuse sin PDF
                '----------------------------------------
                Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
            Case "T_HYP_EXPEDIENTE"
                ' NHM INI - agrega nuevo case
                If idTipoDocumento = OficioTipo.Dictamen Then
                    '----------------------------------------
                    ' No se puede subir Expediente sin Oficio Word
                    '----------------------------------------
                    Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoWord(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                Else
                    Return True
                End If
                'NHM FIN
            Case Else
                Return True
        End Select

    End Function

    ''' <summary>
    ''' Mostrar la ventan modal de carga de archivo, determinar qué extensiones de archivo son las permitidas.
    ''' </summary>
    ''' <param name="tipoArchivo"></param>
    ''' <remarks></remarks>
    Private Sub showModalFileUpload(ByVal tipoArchivo As String)
        Select Case tipoArchivo
            Case "T_HYP_ARCHIVOWORD"
                lblTipoArchivo.Text = "Adjuntar archivo Word"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .doc, .docx"
            Case "T_HYP_ARCHIVOSCAN"
                lblTipoArchivo.Text = "Adjuntar archivo PDF"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .pdf"
            Case "T_CEDULADIGITAL"
                lblTipoArchivo.Text = "Adjuntar Cédula de Notificación Electrónica"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .sbm, .sbmx"
            Case "T_HYP_CEDULAPDF"
                lblTipoArchivo.Text = "Adjuntar Cédula"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .pdf"
            Case "T_HYP_FIRMADIGITAL"
                lblTipoArchivo.Text = "Adjuntar Firma Digital"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .sbm, .sbmx"
            Case "T_HYP_RESPUESTAOFICIO"
                lblTipoArchivo.Text = "Adjuntar Respuesta al Oficio"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .pdf"
            Case "T_HYP_ACUSERESPUESTA"
                lblTipoArchivo.Text = "Adjuntar Acuse del documento"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .pdf"
            Case "T_ANEXO_UNO"
                lblTipoArchivo.Text = "Adjuntar Anexo"
                lblExtensionesPermitidas.Text = "Archivos permitidos: todos"
            Case "T_ANEXO_DOS"
                lblTipoArchivo.Text = "Adjuntar Anexo sbm"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .sbm, .sbmx"
            Case "T_HYP_EXPEDIENTE"
                lblTipoArchivo.Text = "Adjuntar Expediente"
                'NHM INI
                'lblExtensionesPermitidas.Text = "Archivos permitidos: .pdf"
                lblExtensionesPermitidas.Text = "Archivos permitidos: .pdf, .zip"
                'NHM FIN
        End Select

        modalFileUpload.Show()
        btnModalFileUploadOK.Enabled = False
        upFileUpload.Update()
    End Sub

    ''' <summary>
    ''' Crear nombre del adjunto.
    ''' </summary>
    ''' <param name="nombreArchivo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function crearNombreArchivo(ByVal nombreArchivo As String, ByVal tipoArchivo As String, Optional ByVal ExtensionOriginalParaSBMX As String = "") As String
        Dim tipoDocumentoShort As String = String.Empty

        '----------------------------------------------------
        ' Seleccionar el prefijo del tipo de documento.
        '----------------------------------------------------
        Select Case ID_TIPO_DOCUMENTO
            Case OficioTipo.Oficio_Externo
                tipoDocumentoShort = "EX"
            Case OficioTipo.Oficio_Interno
                tipoDocumentoShort = "IN"
            Case OficioTipo.Atenta_Nota
                tipoDocumentoShort = "AN"
            Case OficioTipo.Dictamen
                tipoDocumentoShort = "DI"
        End Select

        Dim fileName As String =
                                tipoDocumentoShort & "_" &
                                Format(CODIGO_AREA, "000").ToString + "_" &
                                Format(I_OFICIO_CONSECUTIVO, "0000").ToString() & "_" &
                                ID_ANIO.ToString

        Select Case tipoArchivo
            Case "T_HYP_ARCHIVOWORD"
                fileName = "WRD_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_HYP_ARCHIVOSCAN"
                fileName = "PDF_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_CEDULADIGITAL"
                fileName = "CNE_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_HYP_CEDULAPDF"
                fileName = "CNE_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_HYP_FIRMADIGITAL"
                fileName = "PDF_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_HYP_RESPUESTAOFICIO"
                fileName = "RES_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_HYP_ACUSERESPUESTA"
                fileName = "ACU_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_ANEXO_UNO"
                fileName = "AX1_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_ANEXO_DOS"
                fileName = "AX1_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_HYP_EXPEDIENTE"
                fileName = "EXP_" & fileName & Path.GetExtension(nombreArchivo)
        End Select

        Return fileName

    End Function

    ''' <summary>
    '''  Abrir archivo
    ''' </summary>
    ''' <param name="NombreArchivo"></param>
    ''' <remarks></remarks>
    Protected Sub abreArchivoLink(ByVal nombreArchivo As String)

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

                If nombreArchivo.Contains("#") AndAlso nombreArchivo.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                    nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.IndexOf("#"))
                    Archivo = cliente.DownloadData(nombreArchivo)
                Else
                    usuario = AppSettings("UsuarioSp")
                    passwd = enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR")
                    ServSharepoint = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR")
                    Dominio = enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR")
                    Biblioteca = enc.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")

                    cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)
                    url = ServSharepoint & "/" & Biblioteca & "/" & nombreArchivo

                    urlEncode = Server.UrlPathEncode(url)
                    Archivo = cliente.DownloadData(ResolveUrl(urlEncode))

                End If
                filename = "attachment; filename=" & Server.UrlPathEncode(nombreArchivo)

            Catch ex As Exception
                Throw New ApplicationException("Hubo un error abriendo el documento. Posiblemente no existe o no tiene permisos para verlo.")

            End Try

            If Not Archivo Is Nothing Then

                Dim tipo_arch As String = nombreArchivo.Substring(nombreArchivo.LastIndexOf(".") + 1)

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
            modalMensaje(ex.Message)
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoLink")
        End Try
    End Sub

    Private Function archivoSharepoint(ByVal nombreArchivo As String) As Boolean
        Dim archivoNuevo As Boolean = True

        If (InStr(nombreArchivo, "\") > 0) Then
            archivoNuevo = False

        End If

        Return archivoNuevo
    End Function

    ''' <summary>
    ''' Cargar archivo a Sharepoint
    ''' </summary>
    ''' <param name="NombreSharepoint"></param>
    ''' <param name="IdAnio"></param>
    ''' <param name="consecutivo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function subirArchivo(ByVal NombreSharepoint As String, ByVal IdAnio As Integer, ByVal consecutivo As Integer) As String

        Dim FilePath As String = String.Empty

        Try

            '--------------------------------------------------------------------------
            ' Conectar con Sharepoint
            '--------------------------------------------------------------------------
            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim objSP As New nsSharePoint.FuncionesSharePoint

            objSP.ServidorSharePoint = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR")
            objSP.Biblioteca = enc.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")
            objSP.Usuario = AppSettings("UsuarioSp").ToString()
            objSP.Password = enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR")
            objSP.Dominio = enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR")
            objSP.RutaArchivo = System.IO.Path.GetTempPath()
            objSP.NombreArchivo = NombreSharepoint
            '--------------------------------------------------------------------------
            ' Carga el archivo (directorio temporal - nombre de archivo
            '--------------------------------------------------------------------------
            'If Not objSP.CargarArchivo() Then Throw New Exception
            If Not objSP.UploadFileToSharePoint() Then Throw New ApplicationException("Error subiendo archivo a Sharepoint")

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            NombreSharepoint = String.Empty
        Finally

            If System.IO.File.Exists(FilePath) Then System.IO.File.Delete(FilePath)

        End Try
        Return NombreSharepoint
    End Function

    Private Function GetSortDirection(ByVal column As String) As String
        Dim sortDirection = "ASC"
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)

        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
                  AndAlso lastDirection = "ASC" Then

                    sortDirection = "DESC"

                End If
            End If
        End If

        ' Save new values in ViewState.
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column

        Return sortDirection

    End Function

    Protected Function EstatusNotificacion(ByVal Estatus As Object) As String
        Try
            If IsDBNull(Estatus) Then
                Return "~/imagenes/ERROR.gif"
            Else
                If CBool(Estatus) Then
                    Return "~/imagenes/OK.gif"
                Else
                    Return "~/imagenes/ERROR.gif"
                End If
            End If

        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion EstatusNotificacion: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function

    Protected Function EstatusNotificacionTooltip(ByVal Estatus As Object) As String
        Try
            If IsDBNull(Estatus) Then

                Return "Notificación Pendiente"
            Else
                If CBool(Estatus) Then
                    Return "Notificación Enviada"
                Else
                    Return "Notificación Pendiente"
                End If

            End If
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("EstatusNotificacionTooltip: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' Ocultas la filas del GridView
    ''' </summary>
    ''' <param name="listaOcultar">Recibe un ListBox de la lista oculta de columnas</param>
    ''' <remarks></remarks>
    Private Sub ManejarColumnasGridView(ByVal listaOcultar As ListBox, ByVal listaVisible As ListBox)
        Try
            If listaOcultar.Items.Count > 0 Then
                For contSinAsignados = 0 To listaOcultar.Items.Count - 1
                    For contPerzonalizada = 0 To gvBandejaOficios.Columns.Count - 1
                        If DirectCast(listaOcultar.Items(contSinAsignados).Text, String) = gvBandejaOficios.Columns(contPerzonalizada).HeaderText Then
                            gvBandejaOficios.Columns(contPerzonalizada).Visible = False
                            Exit For
                        End If
                    Next
                Next
            End If

            If listaVisible.Items.Count > 0 Then
                For contAsignados = 0 To listaVisible.Items.Count - 1
                    For contPerzonalizada = 1 To gvBandejaOficios.Columns.Count - 1
                        If DirectCast(listaVisible.Items(contAsignados).Text, String) = gvBandejaOficios.Columns(contPerzonalizada).HeaderText Then
                            gvBandejaOficios.Columns(contPerzonalizada).Visible = True
                            Exit For
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            modalMensaje("Se genero el siguiente Error: " + ex.Message)
        End Try
    End Sub

#Region "Verificar Sesión y perfil de Usuario"
    Private Sub verificaSesion()
        Dim logout As Boolean = False
        Dim sesion As New Seguridad
        Try
            'Verifica la sesion de usuario
            Select Case sesion.ContinuarSesionAD()
                Case -1
                    logout = True
                Case 0, 3
                    logout = True
            End Select
        Catch ex As Exception
            EscribirError(ex, "verificaSesion")
        Finally
            If Not sesion Is Nothing Then
                sesion.CerrarCon()
                sesion = Nothing
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

        If Session("PERFIL_ASISTENTE") Is Nothing Then Session("PERFIL_ASISTENTE") = BusinessRules.BDS_C_PERFIL.ConsultarPerfilPorNombre("ASISTENTE")

        If CInt(Session("perfil")) = CInt(Session("PERFIL_ASISTENTE")) Then
            Dim dtUsuarios As DataTable = BusinessRules.BDA_R_USUARIO_ASISTENTE.getUsuarios(USUARIO)
            Dim list As New List(Of String)
            If dtUsuarios.Rows.Count > 0 Then
                For Each row As DataRow In dtUsuarios.Rows
                    list.Add(row("USUARIO").ToString())
                Next
                If BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, list) Then
                    Return True
                End If
            End If
        End If

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

    Private Function getSoloMiosQuery() As String
        Dim sql As String = Nothing
        '--------------------------------------
        ' Obtener el ID del tipo de perfil "asistente"
        '--------------------------------------
        If Session("PERFIL_ASISTENTE") Is Nothing Then Session("PERFIL_ASISTENTE") = BusinessRules.BDS_C_PERFIL.ConsultarPerfilPorNombre("ASISTENTE")

        If CInt(Session("perfil")) = CInt(Session("PERFIL_ASISTENTE")) Then

            '--------------------------------------
            ' Si usuario tiene perfil de asistente, traerse a su(s) jefe(s).
            ' y traerse también como propios, aquellos en que el jefe haya participado (registrado, firmado, elaborado o rubricado).
            '--------------------------------------
            Dim dtUsuarios As DataTable = BusinessRules.BDA_R_USUARIO_ASISTENTE.getUsuarios(USUARIO)
            Dim inClauseArguments As String = String.Empty

            inClauseArguments &= "'" & USUARIO & "',"

            If dtUsuarios.Rows.Count > 0 Then
                For Each row As DataRow In dtUsuarios.Rows
                    inClauseArguments &= "'" & row("USUARIO").ToString() & "',"
                Next
            End If
            inClauseArguments = inClauseArguments.Substring(0, inClauseArguments.LastIndexOf(","))

            sql =
               " INNER JOIN ( " +
               " SELECT DISTINCT O_INNER.ID_AREA_OFICIO, O_INNER.ID_ANIO, O_INNER.I_OFICIO_CONSECUTIVO, O_INNER.ID_TIPO_DOCUMENTO " +
               " FROM BDA_OFICIO O_INNER " +
               " LEFT JOIN BDA_FIRMA F ON F.ID_TIPO_DOCUMENTO = O_INNER.ID_TIPO_DOCUMENTO AND F.ID_ANIO = O_INNER.ID_ANIO AND F.ID_AREA_OFICIO = O_INNER.ID_AREA_OFICIO AND F.I_OFICIO_CONSECUTIVO = O_INNER.I_OFICIO_CONSECUTIVO " +
               " WHERE F.USUARIO IN (" + inClauseArguments + ") " +
               "                UNION " +
               " SELECT DISTINCT O_UNION.ID_AREA_OFICIO, O_UNION.ID_ANIO, O_UNION.I_OFICIO_CONSECUTIVO, O_UNION.ID_TIPO_DOCUMENTO " +
               " FROM BDA_OFICIO O_UNION " +
               " WHERE USUARIO_ALTA IN (" + inClauseArguments + ") OR USUARIO_ELABORO IN (" + inClauseArguments + ")) AS O_UNION2 " +
               " ON  O.ID_AREA_OFICIO = O_UNION2.ID_AREA_OFICIO AND O.ID_ANIO = O_UNION2.ID_ANIO AND O.I_OFICIO_CONSECUTIVO = O_UNION2.I_OFICIO_CONSECUTIVO AND O.ID_TIPO_DOCUMENTO = O_UNION2.ID_TIPO_DOCUMENTO "
        Else
            '--------------------------------------
            ' Traerse nada mas aquellos en los que el usuario haya registrado, firmado, elaborado o rubricado
            '--------------------------------------
            sql =
               " INNER JOIN ( " +
               " SELECT DISTINCT O_INNER.ID_AREA_OFICIO, O_INNER.ID_ANIO, O_INNER.I_OFICIO_CONSECUTIVO, O_INNER.ID_TIPO_DOCUMENTO " +
               " FROM BDA_OFICIO O_INNER " +
               " LEFT JOIN BDA_FIRMA F ON F.ID_TIPO_DOCUMENTO = O_INNER.ID_TIPO_DOCUMENTO AND F.ID_ANIO = O_INNER.ID_ANIO AND F.ID_AREA_OFICIO = O_INNER.ID_AREA_OFICIO AND F.I_OFICIO_CONSECUTIVO = O_INNER.I_OFICIO_CONSECUTIVO " +
               " WHERE F.USUARIO = '" + USUARIO + "' " +
               "                UNION " +
               " SELECT DISTINCT O_UNION.ID_AREA_OFICIO, O_UNION.ID_ANIO, O_UNION.I_OFICIO_CONSECUTIVO, O_UNION.ID_TIPO_DOCUMENTO " +
               " FROM BDA_OFICIO O_UNION " +
               " WHERE USUARIO_ALTA = '" + USUARIO + "' OR USUARIO_ELABORO = '" + USUARIO + "') AS O_UNION2 " +
               " ON  O.ID_AREA_OFICIO = O_UNION2.ID_AREA_OFICIO AND O.ID_ANIO = O_UNION2.ID_ANIO AND O.I_OFICIO_CONSECUTIVO = O_UNION2.I_OFICIO_CONSECUTIVO AND O.ID_TIPO_DOCUMENTO = O_UNION2.ID_TIPO_DOCUMENTO "
        End If
        Return sql

    End Function

#Region "Notificación"
    Private Sub checkEnviarNotificacion()
        Try
            If Not verificaUsuario() Then Throw New ApplicationException("Documento de sólo lectura")

            Dim dtEstatus As DataTable = BusinessRules.BDA_OFICIO.ConsultarEstatusOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            If dtEstatus.Rows(0)("T_ESTATUS").ToString().Trim.ToUpper = "CANCELADO" Or
                        dtEstatus.Rows(0)("T_ESTATUS").ToString().Trim.ToUpper = "CONCLUIDO" Then
                Throw New ApplicationException("No se puede envíar la notificación porque el Estatus del Oficio no lo permite")
            Else

                '------------------------------------------------
                ' Debe tener primero el archivo PDF
                '------------------------------------------------
                If Not BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then
                    Throw New ApplicationException("Debe estar registrado el PDF del Oficio. Verificar por favor.")
                End If

                '------------------------------------------------
                ' Debe tener la cédula
                '------------------------------------------------
                If Not BusinessRules.BDA_OFICIO.ConsultarTieneArchivoCedula(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then
                    Throw New ApplicationException("Debe estar registrada el archivo Cédula de notificación. Verificar por favor.")
                End If

                '------------------------------------------------
                ' Revisar SBMs
                '------------------------------------------------
                Dim dtOficio As DataTable = BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                existenSBMs(dtOficio)

                '------------------------------------------------
                ' Continuar
                '------------------------------------------------
                Dim dtUsuario As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetAllPorUsuario(USUARIO)
                EnviarNotificacion(dtUsuario.Rows(0)("NOMBRECOMPLETO").ToString())
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "Enviar Notificacion")
        End Try
    End Sub

    Private Sub existenSBMs(ByVal dtOficio As DataTable)
        Dim urlToCheck As String = String.Empty
        Try

            '---------------------------------------------------
            ' Obtener datos Sharepoint
            '---------------------------------------------------
            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim sServidorSharepoint As String = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios").ToString, "webCONSAR")
            Dim sBibliotecaSharepoint As String = enc.DecryptFromBase64String(AppSettings("DocLibraryOficios").ToString, "webCONSAR")
            Dim sUsuario As String = AppSettings("UsuarioSp")
            Dim passwd As String = enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR")
            Dim Dominio As String = enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR")
            '---------------------------------------------------
            ' Prepara las credenciales para hacer el request de verificación de archivos SBM
            '---------------------------------------------------
            Dim credentials As NetworkCredential = New NetworkCredential(sUsuario, passwd, Dominio)

            '---------------------------------------------------
            ' Objeto Entity de Oficio
            '---------------------------------------------------
            Dim objOficio As New Entities.BDA_OFICIO
            objOficio.IdAnio = ID_ANIO
            objOficio.IdArea = ID_UNIDAD_ADM
            objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
            objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO

            '---------------------------------------------------
            ' Revisar archivo Firma Digital
            '---------------------------------------------------
            If IsDBNull(dtOficio.Rows(0)("T_HYP_FIRMADIGITAL")) Or String.IsNullOrEmpty(dtOficio.Rows(0)("T_HYP_FIRMADIGITAL").ToString()) Then

                Dim sbmFilename As String = dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN").ToString

                If sbmFilename.Contains("#") AndAlso sbmFilename.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                    sbmFilename = sbmFilename.ToString.Substring(0, sbmFilename.IndexOf("#"))
                End If

                sbmFilename = Path.ChangeExtension(sbmFilename, "sbm")

                urlToCheck = sServidorSharepoint & "/" & sBibliotecaSharepoint & "/" & sbmFilename

                If UrlExiste(urlToCheck, credentials) Then
                    objOficio.ArchivoFirmaDigital = sbmFilename
                    BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)
                Else
                    Throw New ApplicationException("El archivo de Firma Digital (SBM) no existe. Verificar por favor.")
                End If
            End If

            '---------------------------------------------------
            ' Revisar archivo Cédula Digital
            '---------------------------------------------------
            If IsDBNull(dtOficio.Rows(0)("T_HYP_FIRMADIGITAL")) Or String.IsNullOrEmpty(dtOficio.Rows(0)("T_HYP_FIRMADIGITAL").ToString()) Then
                Dim sbmFilename As String = dtOficio.Rows(0)("T_HYP_CEDULAPDF").ToString

                If sbmFilename.Contains("#") AndAlso sbmFilename.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                    sbmFilename = sbmFilename.ToString.Substring(0, sbmFilename.IndexOf("#"))
                End If

                sbmFilename = Path.ChangeExtension(sbmFilename, "sbm")

                urlToCheck = sServidorSharepoint & "/" & sBibliotecaSharepoint & "/" & sbmFilename

                If UrlExiste(urlToCheck, credentials) Then
                    objOficio.ArchivoCedulaDigital = sbmFilename
                    BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)
                Else
                    Throw New ApplicationException("El archivo de Cédula Digital (SBM) no existe. Verificar por favor.")
                End If
            End If

        Catch ex As ApplicationException
            Throw ex
        Catch ex As Exception
            EscribirError(ex, "ExisteEnSharepointYActualizar")
        End Try
    End Sub

    ''' <summary>
    ''' Revisa si el archivo remoto existe en el Servidor
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="credentials"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UrlExiste(ByVal url As String, ByVal credentials As NetworkCredential) As Boolean
        Dim oRequest As System.Net.WebRequest
        oRequest = System.Net.WebRequest.Create(url)
        oRequest.Credentials = credentials
        Try
            Dim myResponse As System.Net.HttpWebResponse = CType(oRequest.GetResponse(), HttpWebResponse)
            myResponse.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub EnviarNotificacion(ByVal nUsuario As String)

        Try
            '------------------------------------------
            '   CONSULTAR OFICIO
            '------------------------------------------
            Dim dtOficio As DataTable = BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            '------------------------------------------
            '   DESTINATARIOS
            '------------------------------------------

            Dim dtFirmaElectronica As New DataTable

            Dim ID_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_CONSAR")
            Dim ID_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR")
            Dim ID_TIPO_ENTIDAD_PROCESAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_PROCESAR")
            Dim ID_ENTIDAD_PROCESAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_PROCESAR")

            Dim dt As New DataTable
            If CInt(dtOficio.Rows(0)("ID_ENTIDAD")) = ID_ENTIDAD_CONSAR AndAlso CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO")) = ID_TIPO_ENTIDAD_CONSAR Then

                dtFirmaElectronica = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadSOLOCONSAR

            ElseIf CInt(dtOficio.Rows(0)("ID_ENTIDAD")) = ID_ENTIDAD_PROCESAR AndAlso CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO")) = ID_TIPO_ENTIDAD_PROCESAR Then

                dtFirmaElectronica = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadSOLOPROCESAR

            Else

                dtFirmaElectronica = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadNOCONSAR(CInt(dtOficio.Rows(0)("ID_ENTIDAD")), CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO")))

            End If



            If dtFirmaElectronica.Rows.Count = 0 Then Throw New ApplicationException("No existen Destinatarios con firma electrónica para la entidad del oficio")

            '------------------------------------------
            'Correo
            '------------------------------------------
            Dim dsTexto As DataSet = BusinessRules.BDA_CORREO_NOTIFICACION.ConsultarCorreoNotificacion(ID_UNIDAD_ADM)
            If dsTexto.Tables(0).Rows.Count = 0 Then Throw New ApplicationException("No existe la configuración de email para el área asignada al oficio.")

            '------------------------------------------
            ' ASUNTO
            '------------------------------------------
            Dim asuntoCorreo As String = "Se notifica Oficio No. " & dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString()
            asuntoCorreo = asuntoCorreo.Replace("/", "\/")

            '------------------------------------------
            ' Notificador
            '------------------------------------------
            Dim dtUsuario As DataTable = BusinessRules.BDS_USUARIO.GetAllPorUsuario(USUARIO)
            Dim nombreNotificador As String = dtUsuario(0)("NOMBRE").ToString & " " & dtUsuario(0)("APELLIDOS").ToString

            '------------------------------------------
            ' TO (Destinatarios y copias)
            '------------------------------------------
            Dim lstDestinatarios As New List(Of String)

            For Each dr As DataRow In dtFirmaElectronica.Rows

                If Not IsDBNull(dr("EMAIL")) Then
                    lstDestinatarios.Add(dr("EMAIL").ToString.Trim)
                End If
            Next

            '------------------------------------------
            ' Cuerpo del correo
            '------------------------------------------
            Dim cuerpoCorreo As New StringBuilder

            For Each drFirma As DataRow In dtFirmaElectronica.Rows
                cuerpoCorreo.Append(drFirma("NOMBRE_USR").ToString() & "  " & drFirma("APE_PATERNO").ToString() & "  " & drFirma("APE_MATERNO").ToString() + "\r")
            Next

            'cuerpoCorreo.Append("\r" & dtTexto(0)("TEXTO").ToString())
            'cuerpoCorreo.Append(" así como en el artículo 134, fracción I del Código Fiscal de la Federación; artículos 114, 115, 116, 118, 119, 120, 121 y 122 del Reglamento de la Ley de los Sistemas de Ahorro para el Retiro, publicado en el Diario Oficial de la Federación el 24 de agosto de 2009; 336, 337 y 339 de las DISPOSICIONES de carácter general en materia de operaciones de los sistemas de ahorro para el retiro se les notifica el oficio " & dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString() & " anexo.")
            cuerpoCorreo.Append("\r" & dsTexto.Tables(0)(0)("TEXTO").ToString().Replace("#NUM_OFICIO#", dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString()))
            cuerpoCorreo.Append("\r\rAtentamente,\r\r\r\r")
            cuerpoCorreo.Append(nombreNotificador + "\r\r\r\r")



            Dim lstCC As New List(Of String)
            'For Each dr As DataRow In dtTexto.Rows
            '    If Not IsDBNull(dr("CORREO")) Then
            '        lstCC.Add(dr("CORREO").ToString.Trim)
            '    End If
            '    If Not IsDBNull(dr("CC")) Then
            '        cuerpoCorreo.Append(dr("CC").ToString & "\r\r")
            '    End If
            'Next
            For Each dr As DataRow In dsTexto.Tables(1).Rows
                If Not IsDBNull(dr("CC_CORREO")) Then
                    lstCC.Add(dr("CC_CORREO").ToString.Trim)
                End If
                If Not String.IsNullOrEmpty(dr("CC_TEXTO").ToString()) Then
                    cuerpoCorreo.Append(dr("CC_TEXTO").ToString & "\r\r")
                End If
            Next

            Dim strCuerpoCorreo As String = cuerpoCorreo.ToString()
            strCuerpoCorreo = strCuerpoCorreo.Replace("{", "")
            strCuerpoCorreo = strCuerpoCorreo.Replace("}", "")
            strCuerpoCorreo = strCuerpoCorreo.Replace("/", "\/")

            '------------------------------------------
            ' Documentos Adjuntos
            '------------------------------------------
            Dim lstAdjuntos As New List(Of String)

            Dim ruta As String = String.Empty

            Dim nombreArchivoPDF As String = dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN").ToString()
            Dim nombreArchivoCedula As String = dtOficio.Rows(0)("T_HYP_CEDULAPDF").ToString()
            Dim nombreArchivoCedulaDigital As String = dtOficio.Rows(0)("T_CEDULADIGITAL").ToString()
            Dim nombreArchivoFirmaDigital As String = dtOficio.Rows(0)("T_HYP_FIRMADIGITAL").ToString()
            Dim nombreAnexoUno As String = String.Empty
            Dim nombreAnexoDos As String = String.Empty

            '------------------------------------------
            ' Obtener nombre de anexo uno si existe
            '------------------------------------------
            If Not IsDBNull(dtOficio.Rows(0)("T_ANEXO_UNO")) AndAlso Not String.IsNullOrEmpty(dtOficio.Rows(0)("T_ANEXO_UNO").ToString) Then

                If IsDBNull(dtOficio.Rows(0)("T_ANEXO_DOS")) OrElse String.IsNullOrEmpty(dtOficio.Rows(0)("T_ANEXO_DOS").ToString) Then
                    Throw New ApplicationException("Archivo Anexo 1 existe pero no está encriptado, por favor encriptar")
                End If

                nombreAnexoUno = dtOficio.Rows(0)("T_ANEXO_DOS").ToString()

                'If nombreAnexoUno.Contains("@") Then
                '    nombreAnexoUno = nombreAnexoUno.Substring(nombreAnexoUno.IndexOf("@") + 1)
                'Else
                '    Throw New ApplicationException("Archivo Anexo 1 existe pero no está encriptado, por favor encriptar")
                'End If

            End If

            ''------------------------------------------
            '' Obtener nombre de anexo dos si existe
            ''------------------------------------------
            'If Not IsDBNull(dtOficio.Rows(0)("T_ANEXO_DOS")) AndAlso Not String.IsNullOrEmpty(dtOficio.Rows(0)("T_ANEXO_DOS").ToString) Then

            '    nombreAnexoDos = dtOficio.Rows(0)("T_ANEXO_DOS").ToString()

            '    If nombreAnexoDos.Contains("@") Then
            '        nombreAnexoDos = nombreAnexoDos.Substring(nombreAnexoDos.IndexOf("@") + 1)
            '    Else
            '        Throw New ApplicationException("Archivo Anexo 2 existe pero no está encriptado, por favor encriptar")
            '    End If

            'End If

            Dim encrip As New YourCompany.Utils.Encryption.Encryption64
            If Not CBool(dtOficio.Rows(0)("IS_FILE_FLAG")) Then

                Dim biblioteca As String = encrip.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")

                '-------------------------------------------------
                ' Descargar Cedula Digital a directorio temporal
                ' agregar a lista de adjuntos.
                '-------------------------------------------------
                If Not BajarArchivo(nombreArchivoCedulaDigital) Then Throw New ApplicationException("Error adjuntando archivo Cédula Digital. Posiblemente no exista.")
                'NHM INI - Se corrgie bug que agrega puerto a la url en producción
                'ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreArchivoCedulaDigital
                ruta = get_URL_UriPartial_Authority() & AppSettings("TEMP_PATH").ToString & nombreArchivoCedulaDigital
                'NHM FIN

                lstAdjuntos.Add(ruta)
                '-------------------------------------------------
                ' Descargar Archivo Firma Digital a directorio temporal
                ' agregar a alista de adjuntos
                '-------------------------------------------------
                ruta = Server.MapPath(Me.Request.ApplicationPath) + AppSettings("TEMP_PATH").ToString.Replace("/", "")
                If Not BajarArchivo(nombreArchivoFirmaDigital) Then Throw New ApplicationException("Error adjuntando archivo Firma Digital. Posiblemente no exista.")

                'NHM INI - Se corrgie bug que agrega puerto a la url en producción
                'ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreArchivoFirmaDigital
                ruta = get_URL_UriPartial_Authority() & AppSettings("TEMP_PATH").ToString & nombreArchivoFirmaDigital
                'NHM FIN
                lstAdjuntos.Add(ruta)

                '-------------------------------------------------
                ' Descargar Archivo Anexo 1 a dir temporal
                ' agregar a lista de adjuntos
                '-------------------------------------------------
                If Not nombreAnexoUno = String.Empty Then
                    ruta = Server.MapPath(Me.Request.ApplicationPath) & AppSettings("TEMP_PATH").ToString.Replace("/", "")
                    If Not BajarArchivo(nombreAnexoUno) Then Throw New ApplicationException("Error adjuntando archivo Anexo 1. Posiblemente no exista.")
                    'NHM INI - Se corrgie bug que agrega puerto a la url en producción
                    'ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreAnexoUno
                    ruta = get_URL_UriPartial_Authority() & AppSettings("TEMP_PATH").ToString & nombreAnexoUno
                    'NHM FIN
                    lstAdjuntos.Add(ruta)
                End If

                '-------------------------------------------------
                ' Descargar Archivo Anexo 2 a dir temporal
                ' agregar a lista de adjuntos
                '-------------------------------------------------
                If Not nombreAnexoDos = String.Empty Then
                    ruta = Server.MapPath(Me.Request.ApplicationPath) & AppSettings("TEMP_PATH").ToString.Replace("/", "")
                    If Not BajarArchivo(nombreAnexoDos) Then Throw New ApplicationException("Error adjuntando archivo Anexo 2. Posiblemente no exista.")
                    'NHM INI - Se corrgie bug que agrega puerto a la url en producción
                    'ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreAnexoDos
                    ruta = get_URL_UriPartial_Authority() & AppSettings("TEMP_PATH").ToString & nombreAnexoDos
                    'NHM FIN
                    lstAdjuntos.Add(ruta)
                End If

            Else
                If nombreArchivoCedulaDigital.Contains("#") AndAlso nombreArchivoCedulaDigital.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                    nombreArchivoCedulaDigital = nombreArchivoCedulaDigital.Substring(0, nombreArchivoCedulaDigital.IndexOf("#"))
                    lstAdjuntos.Add(nombreArchivoCedulaDigital)
                End If

                If nombreArchivoFirmaDigital.Contains("#") AndAlso nombreArchivoFirmaDigital.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                    nombreArchivoFirmaDigital = nombreArchivoFirmaDigital.Substring(0, nombreArchivoFirmaDigital.IndexOf("#"))
                    lstAdjuntos.Add(nombreArchivoFirmaDigital)
                End If

                If Not nombreAnexoUno = String.Empty Then
                    If nombreAnexoUno.Contains("#") AndAlso nombreAnexoUno.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                        nombreAnexoUno = nombreAnexoUno.Substring(0, nombreAnexoUno.IndexOf("#"))
                        lstAdjuntos.Add(nombreAnexoUno)
                    End If
                End If

                If Not nombreAnexoDos = String.Empty Then
                    If nombreAnexoDos.Contains("#") AndAlso nombreAnexoDos.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
                        nombreAnexoDos = nombreAnexoDos.Substring(0, nombreAnexoDos.IndexOf("#"))
                        lstAdjuntos.Add(nombreAnexoDos)
                    End If
                End If

            End If

            '---------------------------------------
            ' Compone la cadena de destinatarios
            '---------------------------------------
            Dim destinatariosString As String = String.Empty
            If lstDestinatarios.Count > 0 Then
                For Each item As String In lstDestinatarios
                    destinatariosString += item + ";"
                Next
                destinatariosString = destinatariosString.Substring(0, destinatariosString.LastIndexOf(";"))
            End If


            '---------------------------------------
            ' Compone la cadena de destinatarios de copia
            '---------------------------------------
            Dim destinatariosCopiaString As String = String.Empty
            If lstCC.Count > 0 Then
                For Each item As String In lstCC
                    destinatariosCopiaString += item + ";"
                Next
                destinatariosCopiaString = destinatariosCopiaString.Substring(0, destinatariosCopiaString.LastIndexOf(";"))
            End If

            Dim openOutlookScript As New StringBuilder
            openOutlookScript.Append("var theApp;")
            openOutlookScript.Append("var theMailItem;")
            openOutlookScript.Append("var subject = '" + asuntoCorreo + "';")
            openOutlookScript.Append("var msg = '" + strCuerpoCorreo + "';")
            openOutlookScript.Append("var to = '" + destinatariosString + "';")
            openOutlookScript.Append("var cc = '" + destinatariosCopiaString + "';")
            openOutlookScript.Append("try {")
            openOutlookScript.Append("var theApp = new ActiveXObject(""Outlook.Application"");")
            openOutlookScript.Append("var theMailItem = theApp.CreateItem(0);")
            openOutlookScript.Append("theMailItem.to = to;")
            openOutlookScript.Append("theMailItem.Subject = (subject);")
            openOutlookScript.Append("theMailItem.Body = (msg);")
            openOutlookScript.Append("theMailItem.CC = (cc);")

            '---------------------------------------
            ' Compone la cadena de adjuntos
            '---------------------------------------
            Dim adjuntosString As String = String.Empty
            For Each item As String In lstAdjuntos
                '---------------------------------------
                ' Escapa la cadena si usas archivos locales al servidor, (C:\dummy.txt -> C:\\dummy.txt, \\CONSARFILE -> \\\\CONSARFILE)
                ' si son archivos de url, no modificar.
                '---------------------------------------
                If CBool(dtOficio.Rows(0)("IS_FILE_FLAG")) Then item = item.Replace("\", "\\")

                openOutlookScript.Append("theMailItem.Attachments.Add(""" + item + """);")
            Next

            openOutlookScript.Append("theMailItem.display();")
            openOutlookScript.Append("} catch (err) { alert(err + ' Intente de nuevo.'); }")

            ViewState("TempSCRIPTOUTLOOK") = openOutlookScript.ToString

            modalMensaje("Archivos adjuntados, abrir Outlook?", "CorreoNotificacion", "Notificación", True)

        Catch ex As ApplicationException
            modalMensaje(ex.Message, , "Error", False, "Aceptar")
        Catch ex As Exception
            EscribirError(ex, "Enviar Notificación")
        Finally


            mpeProcesa.Hide()

        End Try

    End Sub

    'NHM INI
    Public Function get_URL_UriPartial_Authority() As String

        Dim ruta As String = Request.Url.GetLeftPart(UriPartial.Authority).ToLower().Trim()

        Dim textBuscar As String = AppSettings("URL_DOMINIO_PROD").ToString().ToLower().Trim()

        If ruta.Contains(textBuscar) = True Then

            Dim arraySplit As String() = ruta.Split(New Char() {":"c})

            If Not IsNothing(arraySplit) AndAlso arraySplit.Count >= 2 Then
                ruta = arraySplit(0) & ":" & arraySplit(1)
            End If

        End If

        Return ruta
    End Function
    'NHM FIN

    Private Function BajarArchivo(ByVal pNombreArchivo As String) As Boolean
        Dim cliente As WebClient
        Dim binArchivo() As Byte
        Dim resultado As Boolean = False
        Dim pUsuario As String
        Dim pPwd As String
        Dim ServSharepoint As String
        Dim Dominio As String
        Dim Biblioteca As String
        Dim urlEncode As String = String.Empty
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Try

            cliente = New WebClient

            pUsuario = WebConfigurationManager.AppSettings("UsuarioSp")
            pPwd = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
            ServSharepoint = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
            Dominio = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
            Biblioteca = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")

            cliente.Credentials = New NetworkCredential(pUsuario, pPwd, Dominio)


            Dim Url As String = ServSharepoint & "/" & Biblioteca & "/" & pNombreArchivo
            urlEncode = Server.UrlPathEncode(Url)


            Dim pRutaDestino As String = Server.MapPath(Me.Request.ApplicationPath) + WebConfigurationManager.AppSettings("TEMP_PATH").ToString.Replace("/", "")
            Dim r As New System.Web.UI.Control

            Try
                binArchivo = cliente.DownloadData(ResolveUrl(urlEncode))
            Catch ex As Exception
                Throw New ApplicationException
            End Try

            File.WriteAllBytes(String.Format("{0}\{1}", pRutaDestino, pNombreArchivo), binArchivo)
            resultado = True

        Catch ex As ApplicationException

        Catch ex As Exception
            ControlErrores.nsControlErrores.ControlErrores.EscribirEvento(ex.Message, EventLogEntryType.Error, "FuncionesSharePoint", "")
            Throw ex
        Finally

        End Try
        Return resultado
    End Function


    ''' <summary>
    ''' Limpia el codigo HTML de codigos innesarios, ademas agrega un salto de linea en parrafos en vacios
    ''' </summary>
    ''' <param name="Plantilla">Codigo HTML a limpiar</param>
    ''' <returns>Codigo HTML procesado</returns>
    ''' <remarks>Funcona en conjunto con la funcion REReemplaza</remarks>
    Private Function LimpiarHTML(ByVal Plantilla As String) As String
        Dim Patron As String, CambiarPor As String

        ' Elimina etiquetas de redundantes
        Patron = "<([\w]+)><\1>([ ;\&\#\d\s\w\r\n\t]*)<\/\1><\/\1>"
        CambiarPor = "<$1>$2</$1>"
        Plantilla = REReemplaza(Plantilla, Patron, CambiarPor)

        ' Elimina etiquetas sin contenido
        Patron = "<([\w]+)>([\s ]*)<\1>"
        CambiarPor = ""
        Plantilla = REReemplaza(Plantilla, Patron, CambiarPor)

        ' Agrega <br/> en parrafos vacios
        Patron = "(<p([\s\d\w]*=""[ \s\d\w-:;]*"")*>)([ \s\r\n\t]*)(&nbsp;)*([ \s\r\n\t]*)(<\/p>)"
        CambiarPor = "$1$4$6<br/>"
        Plantilla = Regex.Replace(Plantilla, Patron, CambiarPor)

        Return Plantilla
    End Function
    Private Function REReemplaza(ByVal Cnt As String, ByVal Pat As String, ByVal Cam As String) As String
        Dim Ant As String = String.Empty

        While (Ant <> Cnt)
            If (Ant <> String.Empty) Then
                Cnt = Ant
            End If
            Ant = Regex.Replace(Cnt, Pat, Cam)
        End While
        Return Ant
    End Function

    'NHM INI
    Public Function useFontSpecific(ByVal textoHTML As String) As Boolean

        'NOTA: tipos de fuentes que despliega el componente checkeditor, del módulo de cédulas de notificación:

        'Style = "font-family: arial, helvetica, sans-serif"
        'Style = "font-family: adobe caslon pro, helvetica"
        'Style = "font-family: comic sans ms, cursive"
        'Style = "font-family: courier new, courier, monospace"
        'Style = "font-family: georgia, serif"
        'Style = "font-family: lucida sans unicode, lucida grande, sans-serif"
        'Style = "font-family: tahoma, geneva, sans-serif"
        'Style = "font-family: times new roman, times, serif"
        'Style = "font-family: trebuchet ms, helvetica, sans-serif"
        'Style = "font-family: verdana, geneva, sans-serif"

        textoHTML = textoHTML.ToLower()

        Dim useFontAdobeCaslonPro As Boolean = False

        If textoHTML.Contains("adobe caslon pro") Then
            useFontAdobeCaslonPro = True
        ElseIf textoHTML.Contains("arial") OrElse
               textoHTML.Contains("comic sans ms") OrElse
               textoHTML.Contains("courier new") OrElse
               textoHTML.Contains("georgia") OrElse
               textoHTML.Contains("lucida sans unicode") OrElse
               textoHTML.Contains("tahoma") OrElse
               textoHTML.Contains("times new roman") OrElse
               textoHTML.Contains("trebuchet ms") OrElse
               textoHTML.Contains("verdana") Then

            useFontAdobeCaslonPro = False

        Else
            'Esto signfica que no se especifico ningún tipo de letra, entonces por default utilizar la fuente: Adobe Caslon Pro
            useFontAdobeCaslonPro = True

        End If

        Return useFontAdobeCaslonPro

    End Function
    'NHM FIN

    ''' <summary>
    ''' Generar cedula notificacion AGC
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerarCedulaElectronica()
        Try


            Dim Watermark As iTextSharp.text.Image

            If System.IO.File.Exists(Server.MapPath("~/imagenes/Watermark.png")) Then
                Dim WatermarkLocation As String = Server.MapPath("~/imagenes/Watermark.png")
                Watermark = iTextSharp.text.Image.GetInstance(WatermarkLocation)
                Watermark.ScalePercent(70.0F)
                Watermark.SetAbsolutePosition(120, 150)
            End If



            Dim dtOficio As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioEntidad(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Dim row As DataRow() = CType(Session(LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.IdSessionEntidad), DataSet).Tables("BDV_C_ENTIDAD").Select("CVE_ID_ENT= " & dtOficio.Rows(0)("ID_ENTIDAD").ToString & " and ID_T_ENT=" & dtOficio.Rows(0)("ID_ENTIDAD_TIPO").ToString)
            Dim Entidad As String = ""
            If row.Length > 0 Then
                Entidad = row(0)("DESC_ENT").ToString()
            End If

            Dim dtFirma As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.ConsultarDatosFirmaCargoPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Dim dtDestinatarios As DataTable = Nothing
            If Not IsDBNull(dtOficio.Rows(0)("ID_ENTIDAD")) Then


                Dim ID_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_CONSAR")
                Dim ID_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR")
                Dim ID_TIPO_ENTIDAD_PROCESAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_PROCESAR")
                Dim ID_ENTIDAD_PROCESAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_PROCESAR")

                Dim dt As New DataTable
                If CInt(dtOficio.Rows(0)("ID_ENTIDAD")) = ID_ENTIDAD_CONSAR AndAlso CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO")) = ID_TIPO_ENTIDAD_CONSAR Then

                    dtDestinatarios = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadSOLOCONSAR

                ElseIf CInt(dtOficio.Rows(0)("ID_ENTIDAD")) = ID_ENTIDAD_PROCESAR AndAlso CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO")) = ID_TIPO_ENTIDAD_PROCESAR Then

                    dtDestinatarios = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadSOLOPROCESAR

                Else

                    dtDestinatarios = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadNOCONSAR(CInt(dtOficio.Rows(0)("ID_ENTIDAD")), CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO")))

                End If


                If dtDestinatarios.Rows.Count = 0 Then Throw New ApplicationException("No existen Destinatarios vigentes para la entidad")

            Else
                Throw New ApplicationException("Error en la entidad relacionada el oficio")
            End If

            Dim ruta As String = String.Empty
            Dim hora As String = String.Empty
            Dim fecha As String = String.Empty

            Dim fileName As String = "CNE_EX_" &
                                    Format(CODIGO_AREA, "000").ToString + "_" &
                                    Format(I_OFICIO_CONSECUTIVO, "0000").ToString() & "_" &
                                    ID_ANIO.ToString &
                                    ".pdf"

            '------------------------------------------
            ' Obtén la ruta temporal a la cual vamos a copiar el archivo adjuntado.
            '------------------------------------------
            Dim randomClass As Random = New Random()
            ruta = Path.GetTempPath.ToString() & fileName

            hora = String.Format(" {0} horas con {1} minutos ", txtHora.Text.Trim, txtMin.Text.Trim)
            fecha = CType(txtFechaCedula.Text, DateTime).ToLongDateString()
            Dim strDestinatarios As StringBuilder
            strDestinatarios = New StringBuilder

            For Each dr As DataRow In dtDestinatarios.Rows
                strDestinatarios.Append(String.Format("{0} {1}, ", dr("NombreCompleto").ToString(), dr("EMAIL").ToString()))
            Next

            Dim dtUsuarioNotificador As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetOne(ddlNotificador.SelectedValue)
            Dim strUsuarioNotificador = String.Empty
            If dtUsuarioNotificador.Rows.Count > 0 Then
                strUsuarioNotificador = dtUsuarioNotificador(0)("NOMBRECOMPLETO").ToString().Trim()
            End If

            ' Guardar variables de sesion para los marcadores
            Session("notificacion_Fec") = CType(txtFechaCedula.Text, DateTime).ToString("dd \de MMMM \de yyyy")
            Session("notificacion_Hor") = txtHora.Text & ":" & txtMin.Text
            Session("notificacion_Usr") = strUsuarioNotificador

            Dim etiquetas As New Dictionary(Of String, String)
            etiquetas.Add("<hora/>", hora)
            etiquetas.Add("<fecha/>", fecha)
            etiquetas.Add("<entidad_largo/>", Entidad.ToUpper.Trim)
            etiquetas.Add("<destinatarios/>", strDestinatarios.ToString())
            etiquetas.Add("<fecha_oficio/>", CType(dtOficio.Rows(0)("F_FECHA_OFICIO"), DateTime).ToLongDateString)

            etiquetas.Add("<prefijo/>", dtFirma.Rows(0)("T_PREFIJO").ToString())
            etiquetas.Add("<cargo/>", dtFirma.Rows(0)("T_CARGO").ToString())
            etiquetas.Add("<nombre/>", dtFirma.Rows(0)("NOMBRE").ToString())
            etiquetas.Add("<num_oficio/>", dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString())
            etiquetas.Add("<representante_legal/>", Entidad.ToUpper.Trim())
            etiquetas.Add("<notificador/>", strUsuarioNotificador)

            etiquetas.Add("<frase_plantlillas/>", HttpUtility.HtmlEncode(BusinessRules.BDS_C_PARAMETROS.getValor(16)))

            Dim PdfCedula As String

            Dim PDFManager As New Clases.PDF

            ''RECUPERAR LA CEDULA DE NOTIFICACION E LA BASE DE DATOS
            Dim objCedulaNotif As New Entities.BDS_CEDULA_NOTIF
            objCedulaNotif.N_ID_CEDULA_NOTIF = Entities.BDS_CEDULA_NOTIF.TODOS
            objCedulaNotif.ID_UNIDAD_ADM = CInt(dtOficio.Rows(0)("ID_AREA_OFICIO").ToString())
            objCedulaNotif.ID_TIPO_ENTIDAD = CInt(dtOficio.Rows(0)("ID_ENTIDAD_TIPO").ToString())
            objCedulaNotif.ID_ENTIDAD = CInt(dtOficio.Rows(0)("ID_ENTIDAD").ToString())
            objCedulaNotif.VIG_FLAG = Entities.BDS_CEDULA_NOTIF.Vigencia.Vigente
            objCedulaNotif.CargaCedulaNotif()

            If objCedulaNotif.FILAS > 0 Then
                Dim ContenidoPlantilla As String
                Dim Ant As String = String.Empty
                AgregarMarcadoresParaReemplazar(etiquetas, dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString())

                ContenidoPlantilla = objCedulaNotif.T_DSC_CONTENIDO.Replace("</span></p>", "</span></p><br/>")

                ContenidoPlantilla = LimpiarHTML(ContenidoPlantilla)

                'NHM INI - Corrige para que respete el tipo de letra: Adobe Caslon Pro
                'ANTES:
                'PdfCedula = PDFManager.CreateDocumentoDesdeBD(ContenidoPlantilla, etiquetas)
                'AHORA:
                Dim useFontAdobeCaslonPro As Boolean = useFontSpecific(ContenidoPlantilla)
                If useFontAdobeCaslonPro = True Then
                    PdfCedula = PDFManager.CreateDocumentoDesdeBD_FontSpecific(ContenidoPlantilla, etiquetas)
                Else
                    PdfCedula = PDFManager.CreateDocumentoDesdeBD_Normal(ContenidoPlantilla, etiquetas)
                End If
                'NHM FIN
            Else
                ''VALIDACION, MUESTRA MENSAJE INFORMATIVO SI NO HAY UNA CEDULA CONFIGURADA VIGENTE PARA LA AFORE SELECCIONADA
                ModalCedula.Hide()
                Dim lsMsgError As String = "No existe una Cédula de Notificación registrada para la Entidad: [DSC_ENTIDAD] del área : [DSC_AREA], por favor registre una."
                modalMensaje(lsMsgError.Replace("[DSC_ENTIDAD]", Entidad).Replace("[DSC_AREA]", T_AREA_OFICIO), , "Advertencia", False, "Aceptar")
                Exit Sub
                'Select Case CODIGO_AREA
                '    Case 210
                '        PdfCedula = PDFManager.CreateDocument(Server.MapPath("../Plantillas/210_cedula_notifacion.htm"), etiquetas)
                '    Case 220
                '        PdfCedula = PDFManager.CreateDocument(Server.MapPath("../Plantillas/220_cedula_notifacion.htm"), etiquetas)
                '    Case Else                    
                '        Try
                '            'Si es de un area diferente se trata de buscar la plantilla
                '            Dim strPlantilla As String = String.Format("../Plantillas/{0}_cedula_notifacion.htm", CODIGO_AREA.ToString())
                '            PdfCedula = PDFManager.CreateDocument(Server.MapPath(strPlantilla), etiquetas)
                '        Catch ex As Exception
                '            'Si no existe se va por la 210
                '            PdfCedula = PDFManager.CreateDocument(Server.MapPath("../Plantillas/210_cedula_notifacion.htm"), etiquetas)
                '        End Try
                'End Select
            End If

            'Áreas
            'Dim areasJerarquia As String = String.Empty
            'Dim dtAreasJerarquia As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaBottomUp(CInt(dtOficio.Rows(0)("ID_AREA_OFICIO")))
            'For Each TableRow As DataRow In dtAreasJerarquia.Rows
            '    areasJerarquia &= TableRow("DSC_UNIDAD_ADM").ToString & Environment.NewLine
            'Next
            'ct = New iTextSharp.text.pdf.ColumnText(cb)
            'ct.SetSimpleColumn(New iTextSharp.text.Phrase(New iTextSharp.text.Chunk(areasJerarquia, arialNormal)), 70, 650, 530, 36, 13, iTextSharp.text.Element.ALIGN_RIGHT)
            'ct.Go()



            '--------------------------------------------------
            ' Sube a Sharepoint
            '--------------------------------------------------
            Dim encrip As New YourCompany.Utils.Encryption.Encryption64
            Dim objSP As Clases.nsSharePoint.FuncionesSharePoint
            objSP = New Clases.nsSharePoint.FuncionesSharePoint(encrip.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR"), WebConfigurationManager.AppSettings("UsuarioSp"), encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR"), encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR"))
            objSP.Biblioteca = encrip.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")
            objSP.RutaArchivo = Path.GetTempPath()
            objSP.NombreArchivo = fileName
            objSP.BinFile = System.IO.File.ReadAllBytes(PdfCedula)
            objSP.CargarArchivo()

            Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO
            objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

            Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
            Dim fechaVencimientoValidacion As String = "NULL"
            If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
            End If

            objOficio.ArchivoCedulaPDF = fileName
            objOficio.IdAnio = CType(dtOficio.Rows(0)("ID_ANIO"), Integer)
            objOficio.IdArea = CType(dtOficio.Rows(0)("ID_AREA_OFICIO"), Integer)
            objOficio.IdTipoDocumento = CType(dtOficio.Rows(0)("ID_TIPO_DOCUMENTO"), Integer)
            objOficio.IOficioConsecutivo = CType(dtOficio.Rows(0)("I_OFICIO_CONSECUTIVO"), Integer)
            objOficio.UsuarioElaboro = USUARIO
            objOficio.Comentario = fechaVencimientoValidacion


            Dim resultado As Boolean = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaPDF(objOficio)
            If resultado Then modalMensaje("Se generó la Cédula Electrónica exitosamente", "INFORMACION")
            Filtraje()
            upGridView.Update()
            ''--------------------------------------------------
            '' Borra archivos temporales.
            ''--------------------------------------------------
            Try
                'If File.Exists(rutaPDF) Then File.Delete(rutaPDF)

                If File.Exists(ruta) Then File.Delete(ruta)

            Catch ex As Exception
                '--------------------------------------------------
                ' Exception vacía
                '--------------------------------------------------
            End Try

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            'EscribirError(ex, "Generar cedula electronica - Bandeja")
            EventLogWriter.EscribeEntrada(ex.ToString & "Generar cedula electronica - Bandeja", EventLogEntryType.Error)
        End Try
    End Sub


#End Region

#End Region

#Region "botones popup cédula"
    'Protected Sub btnAumentarMinuto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAumentarMinuto.Click
    '    If (CType(txtMin.Text, Integer) < 59) Then
    '        txtMin.Text = (CType(txtMin.Text, Integer) + 1).ToString()
    '    Else
    '        txtMin.Text = "00"
    '    End If
    '    txtMin.Text = Microsoft.VisualBasic.Format(CType(txtMin.Text, Integer), "00")
    '    ModalCedula.Show()
    'End Sub

    'Protected Sub btnAumentarHora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAumentarHora.Click
    '    txtHora.Text = (CType(txtHora.Text, Integer) + 1).ToString()
    '    If CType(txtHora.Text, Integer) > 23 Or CType(txtHora.Text, Integer) < 0 Then
    '        txtHora.Text = "00"
    '    End If
    '    txtHora.Text = Microsoft.VisualBasic.Format(CType(txtHora.Text, Integer), "00")
    '    ModalCedula.Show()
    'End Sub

    'Protected Sub btnDisminuirHora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisminuirHora.Click
    '    If CType(txtHora.Text, Integer) > 2 Then
    '        txtHora.Text = (CType(txtHora.Text, Integer) - 1).ToString()
    '    ElseIf CType(txtHora.Text, Integer) > 23 Then
    '        txtHora.Text = "00"
    '    Else
    '        txtHora.Text = "00"
    '    End If
    '    txtHora.Text = Microsoft.VisualBasic.Format(CType(txtHora.Text, Integer), "00")
    '    ModalCedula.Show()
    'End Sub

    'Protected Sub btnDisminuirMinuto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisminuirMinuto.Click
    '    If CType(txtMin.Text, Integer) > 2 Then
    '        txtMin.Text = (CType(txtMin.Text, Integer) - 1).ToString()
    '    Else
    '        txtMin.Text = "00"
    '    End If
    '    txtMin.Text = Microsoft.VisualBasic.Format(CType(txtMin.Text, Integer), "00")
    '    ModalCedula.Show()
    'End Sub
#End Region

#Region "Copiado de Archivo en Ruta Compartida"


    Public Shared Function CopiaArchivoBridge(ByVal Archivo As String) As Boolean

        Try


            Dim cliente As WebClient
            Dim binArchivo() As Byte
            Dim urlEncode As String = String.Empty
            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim ctrl As New System.Web.UI.Control

            cliente = New WebClient

            cliente.Credentials = New NetworkCredential(AppSettings("UsuarioSp").ToString(),
                                                        enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR"),
                                                        enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR"))


            Dim Url As String = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR") &
                "/" & enc.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR") & "/" & Archivo
            urlEncode = HttpUtility.UrlPathEncode(Url)

            Dim pRutaDestino As String = enc.DecryptFromBase64String(AppSettings("PathDocumentosBrigde"), "webCONSAR")
            'Dim r As New System.Web.UI.Control


            binArchivo = cliente.DownloadData(ctrl.ResolveUrl(urlEncode))

            File.WriteAllBytes(String.Format("{0}\{1}", pRutaDestino, Archivo), binArchivo)

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


#End Region

    Private Sub VerificaArchivoSBM(ByVal TipoDocto As String, ByRef Oficio As Entities.BDA_OFICIO, ByVal ExtensionOriginalParaSBMX As String)

        Try

            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim Archivo As String = crearNombreArchivo("dummy.sbm", TipoDocto)
            Dim ArchivoOriginal As String = ""
            Dim pRutaPaso As String = enc.DecryptFromBase64String(AppSettings("PathDocumentosBrigde"), "webCONSAR")
            Dim dtOficio As DataTable

            dtOficio = BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(Oficio.IdAnio, Oficio.IdArea, Oficio.IdTipoDocumento, Oficio.IOficioConsecutivo)


            Dim EXISTE As Boolean = False

            EXISTE = File.Exists(pRutaPaso & "\" & Archivo)

            If Not EXISTE Then

                Archivo = crearNombreArchivo("dummy.sbmx", TipoDocto, ExtensionOriginalParaSBMX)
                EXISTE = File.Exists(pRutaPaso & "\" & Archivo)

            End If


            If EXISTE Then

                Dim objSP As New nsSharePoint.FuncionesSharePoint

                objSP.ServidorSharePoint = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR")
                objSP.Biblioteca = enc.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")
                objSP.Usuario = AppSettings("UsuarioSp").ToString()
                objSP.Password = enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR")
                objSP.Dominio = enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR")
                objSP.RutaArchivo = pRutaPaso & "\"
                objSP.NombreArchivo = Archivo
                '--------------------------------------------------------------------------
                ' Carga el archivo (directorio temporal - nombre de archivo
                '--------------------------------------------------------------------------

                If objSP.UploadFileToSharePoint() Then

                    Select Case TipoDocto

                        Case "T_HYP_FIRMADIGITAL"
                            ArchivoOriginal = dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN").ToString()
                        Case "T_CEDULADIGITAL"
                            ArchivoOriginal = dtOficio.Rows(0)("T_HYP_CEDULAPDF").ToString()
                        Case "T_ANEXO_UNO"
                            ArchivoOriginal = dtOficio.Rows(0)("T_ANEXO_UNO").ToString()
                        Case "T_ANEXO_DOS"
                            ArchivoOriginal = dtOficio.Rows(0)("T_ANEXO_DOS").ToString()

                        Case Else


                    End Select

                    If File.Exists(pRutaPaso & "\" & ArchivoOriginal) Then
                        IO.File.Delete(pRutaPaso & "\" & ArchivoOriginal)
                    End If

                    IO.File.Delete(pRutaPaso & "\" & Archivo)

                End If


            End If


        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Session(BusinessRules.BDA_OFICIO.SessionAtencionResult) = Nothing

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

    End Sub

    Private Sub CargaValoresFiltroInner()

        If IsNothing(Session(SessionFiltrosInner)) Then Exit Sub
        Dim _filtros As Dictionary(Of String, String) = CType(Session(SessionFiltrosInner), Dictionary(Of String, String))

        chkSoloMios.Checked = _filtros("chkSoloMios") = "1"
        ddlVerUltimos.SelectedValue = _filtros("ddlVerUltimos")
        rblEstatusOficio.SelectedValue = _filtros("rblEstatusOficio")

    End Sub

#Region "Entidades desde Osiris"


    Public Sub VerificaCargaEntidadesOsiris()

        If IsNothing(Session(LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.IdSessionEntidad)) Then

#If DEBUG Then
            Dim con1 As Conexion
            con1 = Nothing
            con1 = New Conexion()
#Else
            Dim con1 As OracleConexion
            con1 = Nothing
            con1 = New OracleConexion()
#End If


            Try

                Dim ds As New DataSet

#If DEBUG Then
                ds = con1.Datos(" SELECT ID_T_ENT, CVE_ID_ENT, DESC_ENT, SIGLAS_ENT, DESC_ENT, null LOGO_ENT FROM dbo.BDV_C_ENTIDAD WHERE CVE_ID_ENT > 0", True)
#Else
                ds = con1.Datos(" SELECT ID_T_ENT, CVE_ID_ENT, DESC_ENT, SIGLAS_ENT, DESC_ENT, LOGO_ENT FROM " & WebConfigurationManager.AppSettings("EsquemaOracle") & ".BDV_C_ENTIDAD WHERE CVE_ID_ENT > 0")
#End If
                ds.Tables(0).TableName = "BDV_C_ENTIDAD"
                ds.AcceptChanges()

                'ds.Merge(con1.Datos(" SELECT CVE_ID_ENT AS ID_ENTIDAD, DESC_ENT AS T_ENTIDAD_CORTO FROM " & WebConfigurationManager.AppSettings("EsquemaOracle") & ".BDV_C_ENTIDAD WHERE VIG_FLAG = 1 AND CVE_ID_ENT > 0 ORDER BY DESC_ENT ").Tables(0))

#If Not DEBUG Then
                ds.Merge(LogicaNegocioSICOD.OSIRIS.Entidades.GetListaFiltroOficios())
                ds.Tables(1).TableName = "ENTIDAD"
                ds.AcceptChanges()
#End If

                ' Guardamos Imagenes en carpeta temporal
                For Each row In ds.Tables("BDV_C_ENTIDAD").Rows
                    If Not IsDBNull(row("LOGO_ENT")) Then

                        Try

                            Dim a As System.Drawing.Image
                            a = ByteArrayToImage(DirectCast(row("LOGO_ENT"), Byte()))
                            a.Save(System.IO.Path.GetTempPath() & "ide_" & Session.SessionID & "_" & row("CVE_ID_ENT").ToString() & ".gif", System.Drawing.Imaging.ImageFormat.Gif)

                        Catch ex As Exception

                            Console.WriteLine(ex.Message)

                        End Try
                    End If
                Next


                Session(LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.IdSessionEntidad) = ds

            Catch ex As Exception

            End Try


        End If

    End Sub


#End Region

    Private Sub AsignaValoresScrollRowIndex(ByVal Index As Integer)

        Session(IndexSelectedSession) = Index
        Session(ScrollPositionSession) = hdnScrollPosition.Value

    End Sub

    Private Sub btnCargaGRID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargaGRID.Click

        Filtraje()

        If Not Session(IndexSelectedSession) Is Nothing Then

            Try
                gvBandejaOficios.SelectedIndex = CInt(Session(IndexSelectedSession))
            Catch ex As Exception

            End Try

        End If

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "SetScrollPosition", "SetScrollGrid(" & hdnScrollPosition.Value & ");", True)

    End Sub

    Private Sub AgregarMarcadoresParaReemplazar(ByRef pdEtiquetas As Dictionary(Of String, String), oficios As String)
        Dim dtOficios As DataTable = BusinessRules.BDA_OFICIO.ConsultarDatosFormatoOficioMultiplePorNumeros("'" & oficios & "'")
        Dim ID_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_CONSAR")
        Dim ID_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR")
        Dim ID_TIPO_ENTIDAD_PROCESAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_PROCESAR")
        Dim ID_ENTIDAD_PROCESAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_PROCESAR")

        If dtOficios.Rows.Count > 0 Then
            For Each row As DataRow In dtOficios.Rows
                Dim firma As String = ""
                Dim NumOficio As String = row("T_OFICIO_NUMERO")
                Dim dtTagFirma As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.ConsultarDatosFirmaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                If dtTagFirma.Rows.Count > 0 Then
                    firma = dtTagFirma.Rows(0)("NOMBRE").ToString().Trim
                End If

                '------------------------------------------
                ' Reemplaza los campos variables del documento con aquellos del oficio presente en el loop
                '------------------------------------------

                'NHM INI 
                'pdEtiquetas("#frase#") = """" & BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS"))).ToString & """"
                Dim frase2Lineas As String
                frase2Lineas = "<br/>" + BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS"))).ToString
                frase2Lineas = frase2Lineas + "<br/>" + BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS2"))).ToString
                pdEtiquetas("#frase#") = frase2Lineas
                'NHM FIN
                pdEtiquetas("#dtm_fecha_oficio_1#") = CType(row("F_FECHA_OFICIO"), DateTime).ToString("dd \de MMMM \de yyyy")
                pdEtiquetas("#dtm_fecha_oficio_2#") = CType(row("F_FECHA_OFICIO"), DateTime).ToString("dd \de MMMM \de yyyy")
                pdEtiquetas("#id_Oficio_Numero#") = row("T_OFICIO_NUMERO").ToString()
                pdEtiquetas("#txt_Asunto#") = row("T_ASUNTO").ToString()

                pdEtiquetas("#firma#") = firma


                '---------------------------------------------------------------------------------
                ' Datos del documentos de la cédula de notificación
                '-----------------------------------------------------------------------------------
                ' Fecha, hora y usuario notificador
                pdEtiquetas("#hora_notificacion#") = "" & Session("notificacion_Hor") + " hrs."
                pdEtiquetas("#fecha_notificacion#") = "" & Session("notificacion_Fec")
                pdEtiquetas("#notificador#") = "" & Session("notificacion_Usr")


                '------------------------------------------
                ' Creates a TextInfo based on the "en-US" culture.
                '------------------------------------------
                Dim myTI As TextInfo = New CultureInfo("es-MX", False).TextInfo
                'NHM INI
                'pdEtiquetas("#nombre_Area#") = myTI.ToTitleCase(row("DSC_UNIDAD_ADM").ToString().ToLower)
                pdEtiquetas("#nombre_Area#") = row("ALIAS_UNIDAD_ADM").ToString()
                'NHM FIN

                Dim areasJerarquia As String = String.Empty
                Dim dtAreasJerarquia As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaBottomUp(CInt(row("ID_UNIDAD_ADM")))
                pdEtiquetas("#vicepresidencia#") = String.Empty
                For Each TableRow As DataRow In dtAreasJerarquia.Rows
                    If pdEtiquetas("#vicepresidencia#") = String.Empty Then
                        pdEtiquetas("#vicepresidencia#") = TableRow("ALIAS_Unidad_ADM").ToString()
                    End If
                    areasJerarquia += TableRow("ALIAS_Unidad_ADM").ToString() & vbCrLf

                Next


                pdEtiquetas("#coord_vicepre#") = areasJerarquia.Replace(vbCrLf, "<br/>")

                pdEtiquetas("#destinatario#") = row("DESTINATARIO").ToString()

                pdEtiquetas("#cargo_destinatario#") = row("T_FUNCION").ToString()


                '' *****************************************************************
                '' Obtenemos datos DE ENTIDAD de Osiris
                'values("#txt_Entidad_Largo#") = row("T_ENTIDAD_LARGO").ToString()
                pdEtiquetas("#entidad_destinatario#") = " "
                Dim con1 As New OracleConexion
                Try
                    Dim dt As DataTable = con1.Datos("SELECT DESC_ENT FROM osiris.BDV_C_ENTIDAD WHERE ID_T_ENT = " &
                                                     row("ID_ENTIDAD_TIPO").ToString() & " AND CVE_ID_ENT = " &
                                                     row("ID_ENTIDAD").ToString()).Tables(0)

                    If dt.Rows.Count > 0 Then
                        pdEtiquetas("#entidad_destinatario#") = dt.Rows(0)("DESC_ENT").ToString().Trim
                    End If

                Catch ex As Exception

                Finally
                    If Not con1 Is Nothing Then
                        con1.Cerrar()
                    End If

                End Try


                pdEtiquetas("#txt_Entidad_Direccion_1#") = " "
                pdEtiquetas("#txt_Entidad_Direccion_2#") = " "
                pdEtiquetas("#txt_Entidad_CP#") = " "
                pdEtiquetas("#txt_Entidad_Ciudad#") = " "
                pdEtiquetas("#txt_Entidad_Estado#") = " "

                Dim con As New Conexion(Conexion.BD.SICOD)
                Try

                    Dim sql As String = " SELECT A.T_DIRECCION,A.T_COLONIA,A.T_CP,C.T_POBLACION,ED.T_ESTADO " &
                    " FROM BDA_DIRECCION_ENTIDAD A " &
                    " LEFT OUTER JOIN BDA_POBLACION C ON A.ID_POBLACION = C.ID_POBLACION " &
                    " LEFT OUTER JOIN BDA_ESTADO ED ON A.ID_ESTADO = ED.ID_ESTADO " &
                    " WHERE A.DOMICILIO_NOTIFICACIONES_FLAG = 1 AND A.VIG_FLAG = 1 " &
                    " AND A.ID_T_ENT = " & row("ID_ENTIDAD_TIPO").ToString() & " AND A.CVE_ID_ENT = " & row("ID_ENTIDAD").ToString()
                    Dim dt As DataTable = con.Datos(sql, False).Tables(0)

                    If dt.Rows.Count > 0 Then

                        If Not String.IsNullOrEmpty(Trim(dt.Rows(0)("T_DIRECCION").ToString())) Then
                            pdEtiquetas("#txt_Entidad_Direccion_1#") = dt.Rows(0)("T_DIRECCION").ToString()
                        End If

                        pdEtiquetas("#txt_Entidad_Direccion_2#") = dt.Rows(0)("T_COLONIA").ToString()
                        If String.IsNullOrEmpty(pdEtiquetas("#txt_Entidad_Direccion_2#")) Then
                            pdEtiquetas("#txt_Entidad_Direccion_2#") = " "
                        End If

                        pdEtiquetas("#txt_Entidad_CP#") = dt.Rows(0)("T_CP").ToString()

                        pdEtiquetas("#txt_Entidad_Ciudad#") = dt.Rows(0)("T_POBLACION").ToString()
                        pdEtiquetas("#txt_Entidad_Estado#") = dt.Rows(0)("T_ESTADO").ToString()

                    End If
                Catch ex As Exception

                Finally
                    If Not con Is Nothing Then
                        con.Cerrar()
                    End If
                End Try

                ' ******************************************************************
                ' MCS
                'Se integran nuevos marcadores para  hora y fecha de Cédula, Nombre del notificador, 
                'Puesto de la persona que firma el Oficio, Usuarios y correos SIE Autorizados

                ' #hora_Cedula#
                ' #fecha_Cedula#
                ' #nombre_Notificador#
                ' #puesto_Persona_Firma_Oficio#
                ' #usuarios_SIE_Autorizados#
                ' #correo_SIE_Usuario_Autorizado#
                ' ******************************************************************

                pdEtiquetas("#hora_Cedula#") = DateTime.Now.ToString("HH:mm:ss")
                pdEtiquetas("#fecha_Cedula#") = DateTime.Now.ToString("dd \de MMMM \de yyyy")
                pdEtiquetas("#nombre_Notificador#") = " "
                pdEtiquetas("#puesto_Persona_Firma_Oficio#") = " "
                pdEtiquetas("#usuarios_SIE_Autorizados#") = " "
                pdEtiquetas("#correo_SIE_Usuario_Autorizado#") = " "

                ' ******************************************************************
                ' ******************************************************************


                '' ******************************************************************
                pdEtiquetas("#dtm_Ref_3#") = " "
                'If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("FechaReferencia1").ToString()) Then
                '    values("#dtm_Ref_3#") = pDtOficios.Rows(count)("FechaReferencia1").ToString()
                'End If

                pdEtiquetas("#txt_RefOficio_1#") = " "

                'If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("OficioReferencia1").ToString()) Then
                '    values("#txt_RefOficio_1#") = pDtOficios.Rows(count)("OficioReferencia1").ToString()
                'End If

                pdEtiquetas("#dtm_Ref_4#") = " "
                'If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("FechaReferencia2").ToString()) Then
                '    values("#dtm_Ref_4#") = pDtOficios.Rows(count)("FechaReferencia2").ToString()
                'End If

                pdEtiquetas("#txt_RefOficio_2#") = " "
                'If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("OficioReferencia2").ToString()) Then
                '    values("#txt_RefOficio_2#") = pDtOficios.Rows(count)("OficioReferencia2").ToString()
                'End If

                pdEtiquetas("#txt_Copia_1#") = " "
                pdEtiquetas("#txt_Directorio_Puesto_1#") = " "
                pdEtiquetas("#txt_Copia_2#") = " "
                pdEtiquetas("#txt_Directorio_Puesto_2#") = " "

                Dim dtCopia As DataTable = BusinessRules.BDA_COPIA.ConsultarDatosCopiaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, CInt(row("I_OFICIO_CONSECUTIVO")))
                If dtCopia.Rows.Count > 0 Then
                    pdEtiquetas("#txt_Copia_1#") = dtCopia.Rows(0)("NOMBRE").ToString() & " .-"
                    pdEtiquetas("#txt_Directorio_Puesto_1#") = dtCopia.Rows(0)("PUESTO").ToString()
                    pdEtiquetas("#txt_Entidad_Largo_F1#") = row("T_ENTIDAD_LARGO").ToString()
                    pdEtiquetas("#para_conocimiento_1#") = " para su conocimiento."
                Else
                    pdEtiquetas("#txt_Copia_1#") = " "
                    pdEtiquetas("#txt_Directorio_Puesto_1#") = " "
                    pdEtiquetas("#txt_Entidad_Largo_F1#") = " "
                    pdEtiquetas("#para_conocimiento_1#") = " "
                End If

                If dtCopia.Rows.Count > 1 Then
                    pdEtiquetas("#txt_Copia_2#") = dtCopia.Rows(1)("NOMBRE").ToString() & " .-"
                    pdEtiquetas("#txt_Directorio_Puesto_2#") = dtCopia.Rows(1)("PUESTO").ToString()
                    pdEtiquetas("#txt_Entidad_Largo_F2#") = row("T_ENTIDAD_LARGO").ToString()
                    pdEtiquetas("#para_conocimiento_2#") = " para su conocimiento."
                Else
                    pdEtiquetas("#txt_Copia_2#") = " "
                    pdEtiquetas("#txt_Directorio_Puesto_2#") = " "
                    pdEtiquetas("#txt_Entidad_Largo_F2#") = " "
                    pdEtiquetas("#para_conocimiento_2#") = " "
                End If

                '------------------------------------------
                ' Maneja al Director General del área asociada al documento.
                '------------------------------------------
                Dim directorGeneral As String = " "
                Dim dtDirectorGeneral As DataTable = BusinessRules.BDA_OFICIO.ConsultarCargoFirmante(NumOficio)

                pdEtiquetas("#cargo_director_general#") = ""

                If dtDirectorGeneral.Rows.Count > 0 Then
                    directorGeneral = dtDirectorGeneral.Rows(0)("T_PREFIJO").ToString & " " & dtDirectorGeneral.Rows(0)("NOMBRE").ToString()
                    pdEtiquetas("#cargo_director_general#") = dtDirectorGeneral.Rows(0)("Cargo").ToString()
                End If
                If Not String.IsNullOrEmpty(directorGeneral) Then
                    pdEtiquetas("#director_general#") = directorGeneral
                End If

                ' ------------------------------------------
                ' Marcador vicepresidencia
                ' ------------------------------------------
                dtDirectorGeneral.Dispose()



                '------------------------------------------
                ' Obten iniciales del usuario que elaboró.
                '------------------------------------------
                Dim dtUsuarioElaboro As DataTable = BusinessRules.BDS_USUARIO.GetOne(row("USUARIO_ELABORO").ToString)
                pdEtiquetas("#firmados#") = dtUsuarioElaboro(0)("T_INICIALES").ToString

                '------------------------------------------
                ' Maneja las rubricas asociadas al documento.
                '------------------------------------------
                Dim rubricas As String = String.Empty
                Dim dtRubricas As DataTable = BusinessRules.BDA_FIRMA.ConsultarRubricasPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, CInt(row("I_OFICIO_CONSECUTIVO")))
                For Each rubrica As DataRow In dtRubricas.Rows
                    'NHM INI - cambia orden de acuerdo al nivel del usuario
                    'rubricas = rubrica("T_INICIALES").ToString + "/" + rubricas
                    rubricas = rubricas + rubrica("T_INICIALES").ToString + "/"
                    'NHM FIN
                Next
                If Not String.IsNullOrEmpty(rubricas) Then rubricas = rubricas.Substring(0, rubricas.LastIndexOf("/"))
                'NHM INI - cambia orden de acuerdo al nivel, en teoria el que lo elabora tiene el nivel mas bajo
                'pdEtiquetas("#firmados#") &= "/" & rubricas.ToUpper
                pdEtiquetas("#firmados#") = rubricas.ToUpper & "/" & pdEtiquetas("#firmados#")
                'NHM FIN

                Dim dtFirmaElectronica As New DataTable
                If CInt(row("ID_ENTIDAD")) = ID_ENTIDAD_CONSAR AndAlso CInt(row("ID_ENTIDAD_TIPO")) = ID_TIPO_ENTIDAD_CONSAR Then
                    dtFirmaElectronica = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadSOLOCONSAR
                ElseIf CInt(row("ID_ENTIDAD")) = ID_ENTIDAD_PROCESAR AndAlso CInt(row("ID_ENTIDAD_TIPO")) = ID_TIPO_ENTIDAD_PROCESAR Then
                    dtFirmaElectronica = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadSOLOPROCESAR
                Else
                    dtFirmaElectronica = LogicaNegocioSICOD.FirmaElectronica.GetFirmaElectronicaPorEntidadNOCONSAR(CInt(row("ID_ENTIDAD")), CInt(row("ID_ENTIDAD_TIPO")))
                End If
                rubricas = ""
                For Each fila As DataRow In dtFirmaElectronica.Rows
                    rubricas &= ", " & fila("NombreCompleto").ToString().Trim() & " " & fila("email").ToString().Trim()
                Next
                pdEtiquetas("#destinatarios#") = rubricas.Substring(1)

            Next
        End If
    End Sub

End Class