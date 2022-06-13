Option Strict On
Option Explicit On

Imports LogicaNegocioSICOD
Imports ControlErrores
Imports SICOD.Generales
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Clases
Imports DocumentFormat.OpenXml.Packaging
Imports System.Globalization



Public Class RegistroOficios
    Inherits System.Web.UI.Page


    Dim dsTipoEntidad As New DataSet
    Dim dsEntidad As New DataSet
    Dim dsSubEntidad As New DataSet

#Region "Propiedades de la página"

    Public Property GENERAR_NUM_OFICO() As Boolean
        Get
            Return Convert.ToBoolean(ViewState("GENERAR_NUM_OFICO"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("GENERAR_NUM_OFICO") = value
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

    Public Property isModificar() As Boolean
        Get
            Return CBool(ViewState("IsModificar"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsModificar") = value
        End Set
    End Property

    Public Property I_OFICIO_CONSECUTIVO_DOC_RELACIONADO() As Integer
        Get
            Return CInt(ViewState("I_OFICIO_CONSECUTIVO_DOC_RELACIONADO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("I_OFICIO_CONSECUTIVO_DOC_RELACIONADO") = value
        End Set
    End Property

    Public Property ID_TIPO_DOCUMENTO_DOC_RELACIONADO() As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOCUMENTO_DOC_RELACIONADO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOCUMENTO_DOC_RELACIONADO") = value
        End Set
    End Property

    Public Property ID_ANIO_DOC_RELACIONADO() As Integer
        Get
            Return CInt(ViewState("ID_ANIO_DOC_RELACIONADO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO_DOC_RELACIONADO") = value
        End Set
    End Property

    Public Property ID_UNIDAD_ADM_DOC_RELACIONADO() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM_DOC_RELACIONADO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM_DOC_RELACIONADO") = value
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


    Public Property OLD_ESTATUS() As Integer
        Get
            Return CInt(Session("OLD_ESTATUS"))
        End Get
        Set(ByVal value As Integer)
            Session("OLD_ESTATUS") = value
        End Set
    End Property

    Public Property NEW_ESTATUS() As Integer
        Get
            Return CInt(Session("NEW_ESTATUS"))
        End Get
        Set(ByVal value As Integer)
            Session("NEW_ESTATUS") = value
        End Set
    End Property

    Public Property isEstatusModificado() As Boolean
        Get
            Return CBool(Session("isEstatusModificado"))
        End Get
        Set(ByVal value As Boolean)
            Session("isEstatusModificado") = value
        End Set
    End Property

    Public Property isMultiplesOficios() As Boolean
        Get
            Return CBool(Session("isMultiplesOficios"))
        End Get
        Set(ByVal value As Boolean)
            Session("isMultiplesOficios") = value
        End Set
    End Property

    Public Property isMultiplesOficiosConArchivos() As Boolean
        Get
            Return CBool(Session("isMultiplesOficiosConArchivos"))
        End Get
        Set(ByVal value As Boolean)
            Session("isMultiplesOficiosConArchivos") = value
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

    Public Property TOP_ID_UNIDAD_ADM_USUARIO As Integer
        Get
            Return CInt(ViewState("TOP_ID_UNIDAD_ADM_USUARIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("TOP_ID_UNIDAD_ADM_USUARIO") = value
        End Set
    End Property

    Public Property PREVIOUS_TIPO_DOCUMENTO As Integer
        Get
            Return CInt(ViewState("PREVIOUS_TIPO_DOCUMENTO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("PREVIOUS_TIPO_DOCUMENTO") = value
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

    Public Property ID_UNIDAD_ADM_USUARIO As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM_USUARIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM_USUARIO") = value
        End Set

    End Property

    Public Property ISATENCION() As Boolean
        Get
            Return CBool(ViewState("ISATENCION"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("ISATENCION") = value
        End Set
    End Property

    Public Property FILEMERGEPATH As String
        Get
            Return CStr(Session("FileMergePath"))
        End Get
        Set(ByVal value As String)
            Session("FileMergePath") = value
        End Set
    End Property

    Public Property TOP_AREA_SEL As Integer
        Get
            Return CInt(ViewState("TOP_AREA_SEL"))
        End Get
        Set(ByVal value As Integer)
            ViewState("TOP_AREA_SEL") = value
        End Set

    End Property

    Public Property INDEXMACHOTE As Integer
        Get
            Return CInt(Session("IndexMachote"))
        End Get
        Set(ByVal value As Integer)
            Session("IndexMachote") = value
        End Set
    End Property

    Public Property FileFlag As Boolean
        Get
            Return CBool(Session("FileFlag"))
        End Get
        Set(ByVal value As Boolean)
            Session("FileFlag") = value
        End Set
    End Property

    Private _PnombreFuncion As String = ""

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '-----------------------------------------------
        ' Cache, no permitir que guarde.
        '-----------------------------------------------
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        '-----------------------------------------------
        ' Verificar Sesión y Perfil de usuario
        '-----------------------------------------------
        If IsNothing(Session("IdOficioSISAN")) Then

            verificaSesion()
            verificaPerfil()
            VerificaVisualizaEstructura()

        End If




        If Not IsPostBack Then
            Try

                FileFlag = False

                Session("ddlEntidad.SelectedValue") = 0
                Session("ddlTipoEntidad.SelectedValue") = 0


                If Session("Usuario") Is Nothing Then logOut()
                USUARIO = Session("Usuario").ToString.ToLower

                FILEMERGEPATH = ""

                '-----------------------------------------------
                ' Trae variables de sesión de bandeja y establece propiedades de la página en el Viewstate
                '-----------------------------------------------
                ID_UNIDAD_ADM = CInt(Session("ID_UNIDAD_ADM"))
                ID_ANIO = CInt(Session("ID_ANIO"))
                ID_TIPO_DOCUMENTO = CInt(Session("ID_TIPO_DOCUMENTO"))
                I_OFICIO_CONSECUTIVO = CInt(Session("I_OFICIO_CONSECUTIVO"))
                CODIGO_AREA = CInt(Session("CODIGO_AREA"))

                '------------------------------------------------
                ' Cargar DropDowns y habilítarlos
                '------------------------------------------------
                'CargarDatosIniciales()

                iBtnNuevo.Visible = False

                If Request.QueryString("Modificar") IsNot Nothing Then

                    CargarDatosInicialesMod()
                    'pnlNumeroOficio.Visible = True
                    VerNumeroOficio(True)
                    lblTitulo.Text = "Modificación de Documento"

                    CargarOficio()
                    isModificar = True
                    ddlAreaFirmas.SelectedIndex = 0

                    '-----------------------------------------------
                    ' SI EL DOCUMENTO ESTA CONCLUIDO O CANCELADO, NO SE PUEDE MODIFICAR
                    '-----------------------------------------------
                    If OLD_ESTATUS = 6 OrElse OLD_ESTATUS = 7 Then

                        deshabilitaModificacionOficio()

                        ' Si el oficio esta concluído, se le puede dar seguimiento
                        If OLD_ESTATUS = 6 Then

                            iBtnSeguimiento.Enabled = True
                            iBtnSeguimiento.ToolTip = "Seguimiento"

                        End If


                    Else

                        '-----------------------------------------------
                        ' Verificar que el usuario pueda modificar
                        ' en base a que haya registrado (dado de alta), firmado, rubricado o elaborado el Oficio.
                        ' 
                        ' Si puede modificar, deshabilita los campos representativos de la llave (area, año, tipo de documento...)
                        '-----------------------------------------------
                        If verificaUsuario() Then
                            deshabilitaControlesLlaveOficio()
                            '' *********************************************
                            '' COMENTADO POR JORGE RANGEL  16/AGO/2012
                            '' *********************************************
                            'If BusinessRules.BDS_USUARIO.ConsultarUsuarioPuedeNotificar(USUARIO) AndAlso ID_TIPO_DOCUMENTO = TIPO_DOCUMENTO.OFICIO_EXTERNO Then
                            '    iBtnEnviarNotificacion.Visible = True
                            'Else
                            '    iBtnEnviarNotificacion.Visible = False
                            'End If
                        Else
                            deshabilitaModificacionOficio()
                        End If

                    End If




                    '' *********************************************
                    '' COMENTADO POR JORGE RANGEL  16/AGO/2012
                    '' *********************************************
                    'If Not ID_TIPO_DOCUMENTO = TIPO_DOCUMENTO.OFICIO_EXTERNO Then
                    '    iBtnCedula.Visible = False
                    'End If

                    If Not isEstatusModificado = Nothing AndAlso isEstatusModificado Then
                        Select Case NEW_ESTATUS
                            Case 4, 13, 3, 2, 11, 12
                                modalMensaje("El Estatus ha sido modificado, ¿desea enviar un correo de notificación?", "CorreoCambioEstatus", "ALERTA", True, "Si", "No")
                        End Select

                        Session.Remove("isEstatusModificado")
                    End If


                Else
                    '------------------------------------------------
                    ' Cargar DropDowns y habilítarlos
                    '------------------------------------------------
                    CargarDatosIniciales()

                    If Not isMultiplesOficios = Nothing AndAlso isMultiplesOficios Then
                        If Not isMultiplesOficiosConArchivos = Nothing AndAlso Not isMultiplesOficiosConArchivos Then
                            gvNumerosOficios.Columns(2).Visible = False
                        End If
                        LlenarGridNumerosOficios()
                        Session.Remove("isMultiplesOficios")
                        Session.Remove("isMultiplesOficiosConArchivos")
                        lblTitulo.Text = "Resultado de Múltiples Oficios"
                        hideEverything()
                        btnBandeja.Visible = True
                    Else
                        '-----------------------------------------------
                        ' Es documento nuevo
                        '-----------------------------------------------
                        lblTitulo.Text = "Alta de Nuevo Documento"
                        Nuevo()
                        Me.ddlAreaRubrica.SelectedIndex = 0
                        isModificar = False

                        ISATENCION = Request.QueryString("Atencion") IsNot Nothing

                        If ISATENCION Then

                            'DEJAR LISTA LA PAGINA: NO MULTIPLES, ETC...
                            Me.ddlTipoDocumento.SelectedValue = Request.QueryString("Atencion").ToString()
                            Me.ddlTipoDocumento.Enabled = False
                            Me.ddlTipoDocumento_SelectedIndexChanged(Nothing, Nothing)

                            If Convert.ToInt32(Request.QueryString("Atencion").ToString()) <> 1 Then

                                Me.trMultiplesAfores.Visible = False

                            End If

                        End If

                    End If

                End If

            Catch ex As Exception
                EscribirError(ex, "Cargar pagina")
            End Try
        Else

            '---------------------------------------
            ' Doble click de los listboxes
            '---------------------------------------
            If Request.Params(Page.postEventSourceID) = lstUsuariosFirma.UniqueID OrElse
               Request.Params(Page.postEventSourceID) = lstFirmas.UniqueID OrElse
               Request.Params(Page.postEventSourceID) = lstUsuariosRubrica.UniqueID OrElse
               Request.Params(Page.postEventSourceID) = lstRubricas.UniqueID OrElse
               Request.Params(Page.postEventSourceID) = lstPersonal.UniqueID OrElse
               Request.Params(Page.postEventSourceID) = lstConCopia.UniqueID Then

                Dim ctrlName As String = Request.Params(Page.postEventSourceID)
                Dim args As String = Request.Params(Page.postEventArgumentID)

                Select Case ctrlName
                    Case "lstUsuariosFirma"
                        agregarFirma()
                    Case "lstFirmas"
                        quitarFirma()
                    Case "lstUsuariosRubrica"
                        agregarRubrica()
                    Case "lstRubricas"
                        quitarRubrica()
                    Case "lstPersonal"
                        agregarConCopia()
                    Case "lstConCopia"
                        quitarConCopia()
                End Select

            End If

        End If

    End Sub

    Protected Sub rblEstructuraArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblEstructuraArea.SelectedIndexChanged

        Try

            Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraArea.SelectedValue))
            ID_UNIDAD_ADM_USUARIO = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            TOP_ID_UNIDAD_ADM_USUARIO = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
            Dim dtAreas As DataTable

            dtAreas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
            CargarCombo(ddlArea, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

            '' *********************************************
            '' COMENTADO POR JORGE RANGEL  16/AGO/2012
            '' *********************************************
            ''------------------------------------------------------------
            '' Obtener las áreas de acuerdo al árbol de jerarquía del área del usuario.
            ''------------------------------------------------------------
            'If chkMultiplesAfores.Checked Then
            '    dtAreas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaDGA(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
            '    CargarCombo(ddlArea, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")
            'Else
            '    If CInt(ddlTipoDocumento.SelectedValue) = TIPO_DOCUMENTO.OFICIO_EXTERNO Then

            '        dtAreas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaDGA(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
            '        CargarCombo(ddlArea, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")
            '    Else

            '        dtAreas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
            '        CargarCombo(ddlArea, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

            '    End If
            'End If


            If ddlArea.Items.Count = 0 Then
                txtDestinatario.Text = String.Empty
                destinatarioKey.Value = ""
            Else
                Dim li As ListItem
                li = ddlArea.Items.FindByValue(ID_UNIDAD_ADM_USUARIO.ToString)
                If li IsNot Nothing Then ddlArea.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
            End If

        Catch ex As Exception
            EscribirError(ex, "rblEstructuraArea_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlAreaRubrica_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAreaRubrica.SelectedIndexChanged
        cargarUsuariosRubrica()
    End Sub

    Protected Sub rbEstructuraRubricas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblEstructuraRubricas.SelectedIndexChanged
        Dim Con As Conexion = Nothing
        Try
            Con = New Conexion()

            'Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.GetAll(CInt(rblEstructuraRubricas.SelectedValue))
            'CargarCombo(ddlAreaRubrica, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")
            Dim _tipoUnidad As UnidadAdministrativaTipo = CType(IIf(CInt(rblEstructuraRubricas.SelectedValue) = 1, _
                                                              UnidadAdministrativaTipo.Oficial, _
                                                              UnidadAdministrativaTipo.Funcional), UnidadAdministrativaTipo)
            CargarCombo(ddlAreaRubrica, LogicaNegocioSICOD.UnidadAdministrativa.GetList(_tipoUnidad, UnidadAdministrativaEstatus.Activo), _
                        "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")

            lstUsuariosRubrica.Items.Clear()
        Catch ex As Exception
            EscribirError(ex, "rbEstructuraRubricas_SelectedIndexChanged")
        Finally
            If Con IsNot Nothing AndAlso Con.Estado Then Con.Cerrar()
            Con = Nothing
        End Try
    End Sub

    Protected Sub rblEstructuraElaboro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblEstructuraElaboro.SelectedIndexChanged
        Dim Con As Conexion = Nothing
        Try
            ''Con = New Conexion()

            'Dim topUnidad As Integer = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            'Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.GetAll(CInt(rblEstructuraElaboro.SelectedValue))

            'CargarCombo(ddlAreaElaboro, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

            Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            Dim _ID_UNIDAD_ADM_USUARIO As Integer = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            Dim _TOP_ID_UNIDAD_ADM_USUARIO As Integer = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))

            Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(_TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            CargarCombo(ddlAreaElaboro, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")


            ddlUsuarioElaboro.Items.Clear()
            ddlUsuarioElaboro.Items.Insert(0, New ListItem("-Seleccione Área-", "-1"))

            If ddlAreaElaboro.Items.Count > 0 Then
                Dim li As ListItem
                li = ddlAreaElaboro.Items.FindByValue(_ID_UNIDAD_ADM_USUARIO.ToString)
                If li IsNot Nothing Then
                    ddlAreaElaboro.SelectedValue = _ID_UNIDAD_ADM_USUARIO.ToString
                    cargarUsuarioElaboro()
                End If
            End If

        Catch ex As Exception
            EscribirError(ex, "rblEstructuraElaboro_SelectedIndexChanged")
        Finally
            ''If Con IsNot Nothing AndAlso Con.Estado = True Then
            ''    Con.Cerrar()
            ''End If
            ''Con = Nothing
        End Try
    End Sub

    Protected Sub ddlAreaElaboro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAreaElaboro.SelectedIndexChanged
        cargarUsuarioElaboro()
    End Sub

   

    Private Sub ddlTipoEntidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoEntidad.SelectedIndexChanged
        'Codigo MOdificado por Julio Cesar Vieyra Tena el 18/10/2012
        ' ddlTipoEntidad_CargarDatos()
        Dim TIPO_ENTIDAD As Integer
        If ddlTipoEntidad.SelectedIndex > 0 Then
            TIPO_ENTIDAD = CInt(ddlTipoEntidad.SelectedValue)
            cargarDDLEntidades(CInt(TIPO_ENTIDAD))
            rowSubEntidad.Style.Add("display", "none")
        Else
            ddlTipoEntidad.Items.Clear()
            ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
        End If
        ViewState("TIPO_ENTIDAD") = ddlTipoEntidad.SelectedValue.ToString
        Session("ddlTipoEntidad.SelectedValue") = ddlTipoEntidad.SelectedValue
    End Sub
    Public Sub cargarDDLEntidades(ByVal id_t_entidad As Integer)
        Dim con1 As New OracleConexion
        con1 = Nothing
        con1 = New OracleConexion()
        Try
            Dim descripcion As String = Nothing
            Try
                dsEntidad = con1.Datos(" SELECT * FROM osiris.BDV_C_ENTIDAD where ID_T_ENT=" & id_t_entidad & " and VIG_FLAG=1 and CVE_ID_ENT > 0 order by DESC_ENT")
            Catch ex As Exception
            Finally
                con1.Cerrar()
                con1 = Nothing
            End Try
            ddlEntidad.DataValueField = "CVE_ID_ENT"
            ddlEntidad.DataTextField = "DESC_ENT"
            ddlEntidad.DataSource = dsEntidad
            ddlEntidad.DataBind()
            If dsEntidad.Tables(0).Rows.Count > 0 Then
                ddlEntidad.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
                ddlEntidad.Enabled = True
            Else
                ddlEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
                ddlEntidad.Enabled = False
            End If
        Catch ex As Exception
        Finally
            If Not con1 Is Nothing Then
                con1.Cerrar()
            End If
            con1 = Nothing
        End Try
    End Sub

    Private Sub ddlTipoDocumento_Load(sender As Object, e As EventArgs) Handles ddlTipoDocumento.Load

    End Sub
    Private Sub ddlTipoDocumento_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoDocumento.SelectedIndexChanged
        Try
            '-------------------------------------------------------
            ' 1 - Oficio Externo
            ' 2 - Dictamen
            ' 3 - Atenta Nota
            ' 4 - Oficio Interno
            '-------------------------------------------------------

            btnGuardarGenerarOficios.Visible = False
            pnlGenerarDocumento.Style.Add("display", "none")
            rblFirmaSIE.Enabled = True

            If ddlTipoDocumento.SelectedIndex > 0 Then

                btnGuardarGenerarOficios.Visible = True
                pnlGenerarDocumento.Style.Add("display", "block")

                CargarCombo(ddlIncumplimiento, LogicaNegocioSICOD.BusinessRules.BDA_IRREGULARIDAD.ConsultarIrregularidad, "T_IRREGULARIDAD", "ID_IRREGULARIDAD")

                'NHM INI
                lblMultiplesAfores.Visible = True
                chkMultiplesAfores.Visible = True
                ddlCargoDestinatario.Enabled = True
                txtDestinatario.Text = ""
                'NHM FIN

                If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo OrElse CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then

                    ddlTipoEntidad.Enabled = True
                    ddlEntidad.Enabled = True
                    ddlSubentidad.Enabled = True
                    pnlDocRelacionado.Visible = True

                    LoadCargoDestinatarios(False)
                    
                Else

                    LoadCargoDestinatarios(True)

                    '---------------------------------------------
                    '   Seleccionar tipo de entidad "Interno"
                    '---------------------------------------------
                    Dim T_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("T_TIPO_ENTIDAD_CONSAR")
                    Dim ID_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_CONSAR")

                    Dim T_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("T_ENTIDAD_CONSAR")
                    Dim ID_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR")

                    If ddlTipoEntidad.Items.Contains(New System.Web.UI.WebControls.ListItem(T_TIPO_ENTIDAD_CONSAR, ID_TIPO_ENTIDAD_CONSAR)) Then

                        ddlTipoEntidad.SelectedValue = ID_TIPO_ENTIDAD_CONSAR
                        Session("ddlTipoEntidad.SelectedValue") = ID_TIPO_ENTIDAD_CONSAR
                        '---------------------------------------------
                        '   Carga datos del DropDown de Entidad y Carga "CONSAR"
                        '---------------------------------------------
                        ddlTipoEntidad_SelectedIndexChanged(Nothing, Nothing)
                        'ddlTipoEntidad_CargarDatos()
                        If ddlEntidad.Items.Contains(New System.Web.UI.WebControls.ListItem(T_ENTIDAD_CONSAR, ID_ENTIDAD_CONSAR)) Then

                            ddlEntidad.SelectedValue = ID_ENTIDAD_CONSAR
                            ddlEntidad_SelectedIndexChanged(Nothing, Nothing)
                            Session("ddlEntidad.SelectedValue") = ID_ENTIDAD_CONSAR
                            '---------------------------------------------
                            '   Deshabilita botones de tipo de entidad y entidad   
                            '---------------------------------------------
                            ddlTipoEntidad.Enabled = False
                            ddlEntidad.Enabled = False
                            ddlSubentidad.Enabled = False
                        End If
                    End If

                    pnlDocRelacionado.Visible = False

                End If

                '------------------------------------------------
                ' 
                '------------------------------------------------
                'Dim dtareas As DataTable
                If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo Then

                    pnlLnkOficioExterno.Visible = False
                    '' *********************************************
                    '' COMENTADO POR JORGE RANGEL  16/AGO/2012
                    '' *********************************************
                    'dtareas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaDGA(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
                    'CargarCombo(ddlArea, dtareas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")


                    rblFirmaSIE.SelectedValue = "1"
                    'rblFirmaSIE.Enabled = False
                    rblFirmaSIE_SelectedIndexChanged(Nothing, Nothing)


                Else

                    rblFirmaSIE.SelectedValue = "1"
                    'rblFirmaSIE.Enabled = True
                    rblFirmaSIE_SelectedIndexChanged(Nothing, Nothing)

                    '------------------------------------------------
                    ' Sólo si es dictámen mostrar los controles para cargar oficio externo.
                    '------------------------------------------------
                    If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then
                        pnlLnkOficioExterno.Visible = True
                    Else
                        pnlLnkOficioExterno.Visible = False
                    End If
                    '' *********************************************
                    '' COMENTADO POR JORGE RANGEL  16/AGO/2012
                    '' *********************************************
                    ''------------------------------------------------
                    '' Sólo si es dictámen mostrar los controles para cargar oficio externo.
                    ''------------------------------------------------
                    'If PREVIOUS_TIPO_DOCUMENTO = TIPO_DOCUMENTO.OFICIO_EXTERNO Then
                    '    dtareas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
                    '    CargarCombo(ddlArea, dtareas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")
                    'End If

                End If

                PREVIOUS_TIPO_DOCUMENTO = CInt(ddlTipoDocumento.SelectedValue)
                '' *********************************************
                '' COMENTADO POR JORGE RANGEL  16/AGO/2012
                '' *********************************************
                'If ddlArea.Items.Count = 0 Then
                '    txtDestinatario.Text = String.Empty
                '    destinatarioKey.Value = ""
                'Else
                '    Dim li As ListItem
                '    li = ddlArea.Items.FindByValue(ID_UNIDAD_ADM_USUARIO.ToString)
                '    If li IsNot Nothing Then ddlArea.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
                'End If

            End If

            If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Interno OrElse CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Atenta_Nota OrElse CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then

                rblFirmaSIE.SelectedValue = "0"
                rblFirmaSIE.Enabled = False
                rblFirmaSIE_SelectedIndexChanged(Nothing, Nothing)

            End If


            'NHM INI
            If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then

                lblMultiplesAfores.Visible = False
                chkMultiplesAfores.Visible = False

                LoadCargoDestinatarios(True)

                '---------------------------------------------
                '   Seleccionar tipo de entidad "Interno"
                '---------------------------------------------
                Dim T_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("T_TIPO_ENTIDAD_CONSAR")
                Dim ID_TIPO_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_CONSAR")

                Dim T_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("T_ENTIDAD_CONSAR")
                Dim ID_ENTIDAD_CONSAR As String = WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR")

                'NHM INI

                Dim destinatarioDictamnen As String
                destinatarioDictamnen = BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("DESTINATARIO_DICTAMEN"))).ToString()
                txtDestinatario.Text = destinatarioDictamnen

                Dim T_FUNCION As String = String.Empty
                Dim ID_FUNCION As String = String.Empty

                Dim texto As String
                texto = BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("CARGO_DESTINATARIO"))).ToString()
                Dim array1 As String() = texto.Split(New Char() {","c})

                If Not IsNothing(array1) Then
                    If array1.Count > 1 Then
                        Dim array2 As String() = array1(0).Split(New Char() {"="c})
                        Dim array3 As String() = array1(1).Split(New Char() {"="c})
                        If Not IsNothing(array2) And Not IsNothing(array3) Then
                            If array2.Count > 1 And array3.Count > 1 Then
                                ID_FUNCION = array2(1)
                                T_FUNCION = array3(1)
                            End If
                        End If
                    End If
                End If

                If ddlCargoDestinatario.Items.Contains(New System.Web.UI.WebControls.ListItem(T_FUNCION, ID_FUNCION)) Then
                    ddlCargoDestinatario.SelectedValue = ID_FUNCION

                    ddlCargoDestinatario.Enabled = False
                    If Request.QueryString("Modificar") IsNot Nothing Then
                        ddlCargoDestinatario.Enabled = True
                    End If

                End If
                'NHM FIN

                If ddlTipoEntidad.Items.Contains(New System.Web.UI.WebControls.ListItem(T_TIPO_ENTIDAD_CONSAR, ID_TIPO_ENTIDAD_CONSAR)) Then

                    ddlTipoEntidad.SelectedValue = ID_TIPO_ENTIDAD_CONSAR
                    Session("ddlTipoEntidad.SelectedValue") = ID_TIPO_ENTIDAD_CONSAR
                    '---------------------------------------------
                    '   Carga datos del DropDown de Entidad y Carga "CONSAR"
                    '---------------------------------------------
                    ddlTipoEntidad_SelectedIndexChanged(Nothing, Nothing)
                    'ddlTipoEntidad_CargarDatos()
                    If ddlEntidad.Items.Contains(New System.Web.UI.WebControls.ListItem(T_ENTIDAD_CONSAR, ID_ENTIDAD_CONSAR)) Then

                        ddlEntidad.SelectedValue = ID_ENTIDAD_CONSAR
                        ddlEntidad_SelectedIndexChanged(Nothing, Nothing)
                        Session("ddlEntidad.SelectedValue") = ID_ENTIDAD_CONSAR
                        '---------------------------------------------
                        '   Deshabilita botones de tipo de entidad y entidad   
                        '---------------------------------------------
                        ddlTipoEntidad.Enabled = False
                        ddlEntidad.Enabled = False

                        If Request.QueryString("Modificar") IsNot Nothing Then
                            ddlTipoEntidad.Enabled = True
                            ddlEntidad.Enabled = True
                        End If

                    End If
                End If
            End If
            'NHM FIN


        Catch ex As Exception
            'EscribirError(ex, "ddlTipoDocumento_SelectedIndexChanged")
            EventLogWriter.EscribeEntrada(ex.ToString & "ddlTipoDocumento_SelectedIndexChanged", EventLogEntryType.Error)
        End Try
    End Sub


#Region "Agregar/quitar usuarios firma/rubrica"
    Private Sub agregarFirma()
        Try
            If lstUsuariosFirma.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lstUsuariosFirma.SelectedItem
                If Not lstFirmas.Items.Contains(item) Then
                    lstFirmas.Items.Add(item)
                End If
                item.Selected = False
                lstUsuariosFirma.Items.Remove(item)
            End If

        Catch ex As Exception
            modalMensaje("Debe seleccionar un Usuario")
        End Try
    End Sub

    Protected Sub btnAgregarFirma_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAgregarFirma.Click
        agregarFirma()
    End Sub


    Private Sub quitarFirma()
        Try
            If lstFirmas.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lstFirmas.SelectedItem
                lstUsuariosFirma.Items.Add(item)
                item.Selected = False
                lstFirmas.Items.Remove(item)
            Else
                modalMensaje("Debe seleccionar una Firma")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub btnQuitarFirma_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnQuitarFirma.Click
        quitarFirma()
    End Sub

    Private Sub agregarRubrica()
        Try
            If lstUsuariosRubrica.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lstUsuariosRubrica.SelectedItem
                If Not lstRubricas.Items.Contains(item) Then
                    lstRubricas.Items.Add(item)
                End If
                item.Selected = False
                lstUsuariosRubrica.Items.Remove(item)
            Else
                modalMensaje("Debe seleccionar un Usuario")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAgregarRubrica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAgregarRubrica.Click
        agregarRubrica()
    End Sub

    Private Sub quitarRubrica()
        Try
            If lstRubricas.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lstRubricas.SelectedItem
                lstUsuariosRubrica.Items.Add(item)
                item.Selected = False
                lstRubricas.Items.Remove(item)

            Else
                modalMensaje("Debe seleccionar una rúbrica")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub btnQuitarRubrica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnQuitarRubrica.Click
        quitarRubrica()
    End Sub
#End Region

#Region "Agregar/quitar usuarios copia"
    Private Sub agregarConCopia()
        Try
            If lstPersonal.SelectedIndex >= 0 Then
                Dim item As System.Web.UI.WebControls.ListItem = lstPersonal.SelectedItem
                If Not lstConCopia.Items.Contains(item) Then
                    lstConCopia.Items.Add(item)
                End If
                item.Selected = False
                lstPersonal.Items.Remove(item)
                lblErrores.Text = "<ul>"
                lblErroresPopup.Text = String.Empty
                lblErrores.Text = String.Empty
            End If

        Catch ex As Exception

            modalMensaje("Debe seleccionar una Persona")
        End Try
    End Sub
    Protected Sub btnAgregarConCopia_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAgregarConCopia.Click
        agregarConCopia()
    End Sub

    Private Sub quitarConCopia()
        Try
            If lstConCopia.SelectedIndex >= 0 Then
                Dim item As System.Web.UI.WebControls.ListItem = lstConCopia.SelectedItem
                lstPersonal.Items.Add(item)
                item.Selected = False
                lstConCopia.Items.Remove(item)
                lblErrores.Text = "<ul>"
                lblErroresPopup.Text = String.Empty
                lblErrores.Text = String.Empty
            End If

        Catch ex As Exception

            modalMensaje("Debe seleccionar una Rubrica")
        End Try
    End Sub
    Protected Sub btnQuitarConCopia_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnQuitarConCopia.Click
        quitarConCopia()
    End Sub

#End Region

    Protected Sub chkMultiplesAfores_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkMultiplesAfores.CheckedChanged
        If chkMultiplesAfores.Checked Then

            lblFormatoword.Text = "Abrir formato de los oficios múltiples en blanco:"
            LimpiarControles()
            ddlAreaRubrica.SelectedIndex = 0
            ddlAreaElaboro.SelectedIndex = 0
            pnlDatosBasicos.Visible = False
            pnlMultipleAfore.Visible = True
            'pnlGenerarDocumento.Visible = True

            pnlGenerarDocumento.Style.Add("display", "block")

            'ddlClasificacion.Enabled = True
            btnGuardarGenerarOficios.Visible = True
            'CargarCombo(ddlClasificacion, LogicaNegocioSICOD.BusinessRules.BDA_CLASIFICACION_OFICIO.getAll(), "T_CLASIFICACION", "ID_CLASIFICACION")
            CargarCombo(ddlDirigido, LogicaNegocioSICOD.BusinessRules.BDA_FUNCION.ConsultarFuncion, "T_FUNCION", "ID_FUNCION")
            CargarListBox(lstPersonal, LogicaNegocioSICOD.BusinessRules.BDA_FUNCION.ConsultarFuncion, "T_FUNCION", "ID_FUNCION")
            LlenarGridAfores()
            '' *********************************************
            '' COMENTADO POR JORGE RANGEL  16/AGO/2012
            '' *********************************************
            ''iBtnEnviarNotificacion.Visible = False
            pnlDocRelacionado.Visible = False

            ddlTipoDocumento.Visible = False
            lblTipoDocumento.Visible = False

            '----------------------------------------------
            ' ddlAreas sólo con DGAs para arriba (ya que multiples oficios es para oficios externos únicamente).
            '----------------------------------------------
            '' *********************************************
            '' COMENTADO POR JORGE RANGEL  16/AGO/2012
            '' *********************************************
            'Dim dtareas As DataTable
            'dtareas = BusinessRules.BDS_C_AREA.ConsultarJerarquiaDGA(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
            'CargarCombo(ddlArea, dtareas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

            'If ddlArea.Items.Count = 0 Then
            '    txtDestinatario.Text = String.Empty
            '    destinatarioKey.Value = ""
            'Else
            '    Dim li As ListItem
            '    li = ddlArea.Items.FindByValue(ID_UNIDAD_ADM_USUARIO.ToString)
            '    If li IsNot Nothing Then ddlArea.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
            'End If

            CargarDatosDefault()

        Else

            lblFormatoword.Text = "Abrir formato del oficio en blanco:"
            pnlDatosBasicos.Visible = True
            pnlMultipleAfore.Visible = False
            'pnlGenerarDocumento.Visible = False
            pnlGenerarDocumento.Style.Add("display", "none")
            btnGuardarGenerarOficios.Visible = False
            If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo Or CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then
                pnlDocRelacionado.Visible = True
            End If
            ddlTipoDocumento.Visible = True
            lblTipoDocumento.Visible = True
            CargarDatosIniciales()
            CargarDatosDefault()

            If ISATENCION Then

                Me.ddlTipoDocumento.SelectedValue = Request.QueryString("Atencion").ToString()
                Me.ddlTipoDocumento.Enabled = False
                Me.ddlTipoDocumento_SelectedIndexChanged(Nothing, Nothing)

            End If

        End If
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click

        Try


            'NHM INI - Replica funcionalida del botón: btnGuardarGenerarOficios, solo que aqui el archivo no es obligatorio
            If FILEMERGEPATH <> "" Then


                'NHM INI
                'If Not (System.IO.Path.GetExtension(FILEMERGEPATH) = ".doc" Or System.IO.Path.GetExtension(FILEMERGEPATH) = ".docx") Then
                If Not (System.IO.Path.GetExtension(FILEMERGEPATH) = ".docx") Then


                    AsyncFileUp.ClearFileFromPersistedStore()

                    'NHM INI
                    'modalMensaje("El archivo no es un tipo de archivo válido (doc, docx)")
                    modalMensaje("El archivo no es un tipo de archivo válido (docx)")

                    Exit Sub
                ElseIf System.IO.Path.GetExtension(FILEMERGEPATH).ToLower() = ".doc" Then

                    AsyncFileUp.ClearFileFromPersistedStore()

                    Dim msgDOC As String = "Estimado usuario, el aplicativo en este apartado solo permite adjuntar archivos con "
                    msgDOC = msgDOC & "extensión .docx, usted está tratando de subir un archivo tipo .doc, "
                    msgDOC = msgDOC & "le recomendamos guardar su archivo en formato .docx y volver a intentar adjuntarlo."
                    modalMensaje(msgDOC)

                    Exit Sub


                End If

            End If



            Dim continua As Boolean = False
            'NHM FIN

            'NHM INI
            GENERAR_NUM_OFICO = False
            'NHM FIN

            If Not chkMultiplesAfores.Checked Then

                If Not ValidaDestinatario() Then

                    Throw New ApplicationException("Debe seleccionar un destinatario CONSAR de la lista proporcionada")

                End If
                'NHM INI - Replica funcionalida del botón: btnGuardarGenerarOficios
                'Guardar()
                continua = Guardar()
                'NHM FIN
            Else
                'NHM INI - Replica funcionalida del botón: btnGuardarGenerarOficios
                'GuardarMultiplesAfores()
                continua = GuardarMultiplesAfores()
                'NHM FIN
            End If

            'NHM INI - Replica funcionalida del botón: btnGuardarGenerarOficios
            'FileFlag = False

            'Agrega sigueinte código:
            If continua Then

                If ISATENCION Then ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

                If FILEMERGEPATH <> "" Then

                    'NHM - Agrega valiación, porque despues de Guardar y generar oficio, ya no se muestra ese botón, 
                    'entonces solo pueden dar clic en este botón y les debe permitir actualizar el archivo word              
                    If lblNumeroOficio.Text.Trim = "NO DEFINIDO" Or lblNumeroOficio.Text.Trim = String.Empty Then

                        GenerarDocumentosOficios_NO_DEFINIDO()
                    Else
                        GenerarDocumentosOficios()
                    End If

                End If
              


            End If
            'NHM FIN


        Catch ex As ApplicationException

            modalMensaje(ex.Message, , "ERROR", )

        Catch ex As Exception

            EscribirError(ex, _PnombreFuncion)

        Finally

            mpeProcesa.Hide()

        End Try



    End Sub

    Private Sub btnGuardarGenerarOficios_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardarGenerarOficios.Click

        Try

            If FILEMERGEPATH = "" Then Throw New ApplicationException("Debe seleccionar un archivo a combinar")

            'NHM 
            'If Not (System.IO.Path.GetExtension(FILEMERGEPATH) = ".doc" Or System.IO.Path.GetExtension(FILEMERGEPATH) = ".docx") Then
            If Not (System.IO.Path.GetExtension(FILEMERGEPATH) = ".docx") Then

                AsyncFileUp.ClearFileFromPersistedStore()

                'NHM
                'modalMensaje("El archivo no es un tipo de archivo válido (doc, docx)")
                modalMensaje("El archivo no es un tipo de archivo válido (docx)")

                Exit Sub
            ElseIf System.IO.Path.GetExtension(FILEMERGEPATH).ToLower() = ".doc" Then

                AsyncFileUp.ClearFileFromPersistedStore()

                Dim msgDOC As String = "Estimado usuario, el aplicativo en este apartado solo permite adjuntar archivos con "
                msgDOC = msgDOC & "extensión .docx, usted está tratando de subir un archivo tipo .doc, "
                msgDOC = msgDOC & "le recomendamos guardar su archivo en formato .docx y volver a intentar adjuntarlo."
                modalMensaje(msgDOC)

                Exit Sub


            End If


            Dim continua As Boolean = False

            'NHM INI
            GENERAR_NUM_OFICO = True
            'NHM FIN

            If Not chkMultiplesAfores.Checked Then

                If Not ValidaDestinatario() Then

                    Throw New ApplicationException("Debe seleccionar un destinatario CONSAR de la lista proporcionada")

                End If

                continua = Guardar()
            Else
                continua = GuardarMultiplesAfores()
            End If



            If continua Then

                If ISATENCION Then ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

                'NHM - Agrega valiación, porque pueden dar clik primero en el botón: Guardar, y no generar el numero de oficio, 
                'entonces al dar clic en este botón y les debe permitir actualizar el archivo word     
                'GenerarDocumentosOficios()
                If lblNumeroOficio.Text.Trim = "NO DEFINIDO" Or lblNumeroOficio.Text.Trim = String.Empty Then

                    GenerarDocumentosOficios_NO_DEFINIDO()
                Else
                    GenerarDocumentosOficios()
                End If
                'NHM FIN

            End If





        Catch ex As ApplicationException

            modalMensaje(ex.Message, , "ERROR", )

        Catch ex As Exception

            EscribirError(ex, _PnombreFuncion)

        Finally

            mpeProcesa.Hide()

        End Try




    End Sub

    Protected Sub imgFormatoWord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgFormatoWord.Click

        FileFlag = False
        mpeWordFile.Show()

    End Sub

    Protected Sub iBtnSeguimiento_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnSeguimiento.Click
        Try

            Session("ID_ANIO") = ID_ANIO
            Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
            Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
            Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
            Session("ID_EXPEDIENTE") = ID_EXPEDIENTE
            Session("CODIGO_AREA") = CODIGO_AREA
            Session("NUMERO_OFICIO") = NUMERO_OFICIO
            Response.Redirect("~/App_Oficios/Seguimiento.aspx", False)
        Catch ex As Exception
            EscribirError(ex, "iBtnSeguimiento_Click")
        End Try
    End Sub

    '' *********************************************
    '' COMENTADO POR JORGE RANGEL  16/AGO/2012
    '' *********************************************
    'Protected Sub iBtnEnviarNotificacion_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnEnviarNotificacion.Click
    '    checkEnviarNotificacion()
    'End Sub

    Protected Sub iBtnAdjuntarDocumentos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnAdjuntarDocumentos.Click
        Try

            Session("ID_ANIO") = ID_ANIO
            Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
            Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
            Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
            Session("CODIGO_AREA") = CODIGO_AREA
            Session("NUMERO_OFICIO") = NUMERO_OFICIO
            Response.Redirect("~/App_Oficios/AdjuntarDocumentos.aspx", False)

        Catch ex As Exception
            EscribirError(ex, "iBtnAdjuntarDocumentos_Click")
        End Try
    End Sub

    '' *********************************************
    '' COMENTADO POR JORGE RANGEL  16/AGO/2012
    '' *********************************************
    'Protected Sub iBtnCedula_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnCedula.Click
    '    CargarCombo(ddlNotificador, BusinessRules.BDA_FIRMA_ELECTRONICA.ConsultarNotificadoresJerarquia(TOP_ID_UNIDAD_ADM_USUARIO), "NOMBRE", "USUARIO")
    '    txtFechaCedula.Text = Date.Now.ToShortDateString()
    '    txtHora.Text = Date.Now.Hour.ToString()
    '    txtMin.Text = Date.Now.Minute.ToString()
    '    txtHora.Text = Microsoft.VisualBasic.Format(CType(txtHora.Text, Integer), "00")
    '    txtMin.Text = Microsoft.VisualBasic.Format(CType(txtMin.Text, Integer), "00")
    '    lblTituloModal.Style.Add("display", "block")
    '    lblTituloModal.Text = "Datos para la Cédula Electrónica"
    '    ModalCedula.Show()
    'End Sub

    Protected Sub btnAceptarCedula_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarCedula.Click
        Try
            If ddlNotificador.SelectedIndex > 0 Then
                GenerarCedulaElectronica()
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraModalCedula", "closePopupCedula();", True)
            Else
                ModalCedula.Show()
            End If

        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
            '---------------------------------------------

        End Try
    End Sub

    Protected Sub btnAumentarMinuto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAumentarMinuto.Click
        If (CType(txtMin.Text, Integer) < 59) Then
            txtMin.Text = (CType(txtMin.Text, Integer) + 1).ToString()
        Else
            txtMin.Text = "00"
        End If
        txtMin.Text = Microsoft.VisualBasic.Format(CType(txtMin.Text, Integer), "00")
        ModalCedula.Show()
    End Sub

    Protected Sub btnAumentarHora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAumentarHora.Click
        txtHora.Text = (CType(txtHora.Text, Integer) + 1).ToString()
        If CType(txtHora.Text, Integer) > 23 Or CType(txtHora.Text, Integer) < 0 Then
            txtHora.Text = "00"
        End If
        txtHora.Text = Microsoft.VisualBasic.Format(CType(txtHora.Text, Integer), "00")
        ModalCedula.Show()
    End Sub

    Protected Sub btnDisminuirHora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisminuirHora.Click
        If CType(txtHora.Text, Integer) > 2 Then
            txtHora.Text = (CType(txtHora.Text, Integer) - 1).ToString()
        ElseIf CType(txtHora.Text, Integer) > 23 Then
            txtHora.Text = "00"
        Else
            txtHora.Text = "00"
        End If
        txtHora.Text = Microsoft.VisualBasic.Format(CType(txtHora.Text, Integer), "00")
        ModalCedula.Show()
    End Sub

    Protected Sub btnDisminuirMinuto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisminuirMinuto.Click
        If CType(txtMin.Text, Integer) > 2 Then
            txtMin.Text = (CType(txtMin.Text, Integer) - 1).ToString()
        Else
            txtMin.Text = "00"
        End If
        txtMin.Text = Microsoft.VisualBasic.Format(CType(txtMin.Text, Integer), "00")
        ModalCedula.Show()
    End Sub

    Private Sub ddlEstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEstatus.SelectedIndexChanged
        If ddlEstatus.SelectedIndex > 1 Then
            If ddlEstatus.SelectedItem.Text.Trim.ToUpper = "CANCELADO" Then
                pnlComentariosCancelacion.Visible = True
            Else
                pnlComentariosCancelacion.Visible = False
            End If
        End If
    End Sub

    Private Sub gvNumerosOficios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvNumerosOficios.RowCommand
        If e.CommandName = "Visualizar" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvNumerosOficios.Rows(index)
            Dim link As LinkButton = CType(row.Cells(2).Controls.Item(0), LinkButton)
            Dim nombreArchivo As String = link.Text
            Try
                AbreArchivoLink(link.Text)
            Catch ex As System.Threading.ThreadAbortException
                '----------------------------------
                ' Dejado a propósito en blanco, revisar AbreArchivoLink
                '----------------------------------
            Catch ex As Exception
                EscribirError(ex, "Visualizar archivo Word Multiples Afores")
            End Try
        End If
    End Sub

    Private Sub gvNumerosOficios_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvNumerosOficios.RowEditing
        gvNumerosOficios.EditIndex = e.NewEditIndex
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        If ISATENCION Then

            Session(BusinessRules.BDA_OFICIO.SessionAtencionResult) = Nothing

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

            Return

        End If

        Try

            If Not IsNothing(Request.QueryString("ie")) Then
                Response.Redirect("~/ExpedienteDetalle.aspx?ie=" & Request.QueryString("ie").ToString, False)
                Return
            End If

            Response.Redirect("~/App_Oficios/Bandeja.aspx", False)
        Catch ex As Exception
            EscribirError(ex, "Cancelar")
        End Try
    End Sub

    Protected Sub iBtnNuevo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnNuevo.Click
        Nuevo()
    End Sub

    Protected Sub chkSeDaPlazo_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSeDaPlazo.CheckedChanged
        If chkSeDaPlazo.Checked = True Then
            pnlFechasPlazo.Visible = True
        Else
            pnlFechasPlazo.Visible = False
            txtPlazo.Text = ""
            txtFechaRecepcion.Text = String.Empty
            txtFechaVencimiento.Text = String.Empty
        End If

        If txtFechaAcuse.Text = String.Empty Then txtFechaAcuse.Text = Today.ToShortDateString()

    End Sub

    Private Sub ddlEntidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEntidad.SelectedIndexChanged
        Dim con1 As New OracleConexion
        con1 = Nothing
        con1 = New OracleConexion()
        Try
            If ddlEntidad.SelectedIndex > 0 Then
                ddlSubentidad.Enabled = True
                '--------------------------
                ' Valor del ddlEntidad en la Session para ser usado por el autocomplete de Destinatarios (porque es un método Shared/Static)
                '--------------------------
                Session("ddlEntidad.SelectedValue") = ddlEntidad.SelectedValue
                'Codigo COmnetado por Julio Cesar Vieyra Tena el 18/10/2012
                'Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.ConsultarSubEntidades(CType(ddlEntidad.SelectedValue, Integer))
                'CargarCombo(ddlSubentidad, _dt, "T_ENTIDAD_CORTO", "ID_ENTIDAD")
                Dim descripcion As String = Nothing
                Try
                    dsSubEntidad = con1.Datos(" SELECT * FROM osiris.BDV_C_SUBENTIDAD where ID_T_ENT=" & Me.ddlTipoEntidad.SelectedValue & " and CVE_ID_ENT=" & Me.ddlEntidad.SelectedValue & " and VIG_FLAG=1 order by DSC_SUBENT ")
                Catch ex As Exception
                End Try
                ddlSubentidad.DataValueField = "ID_SUBENT"
                ddlSubentidad.DataTextField = "DSC_SUBENT"
                ddlSubentidad.DataSource = dsSubEntidad
                ddlSubentidad.DataBind()
                If dsSubEntidad.Tables(0).Rows.Count > 0 Then
                    rowSubEntidad.Style.Add("display", "block")
                    ddlSubentidad.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
                Else
                    rowSubEntidad.Style.Add("display", "none")
                    ddlSubentidad.Enabled = True
                End If
            End If
        Catch ex As Exception
            EscribirError(ex, "ddlEntidad_SelectedIndexChanged")
        Finally
            If Not con1 Is Nothing Then
                con1.Cerrar()
            End If
            con1 = Nothing
        End Try

    End Sub

    Protected Sub btnBuscarDocRelacionado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscarDocRelacionado.Click
        lnkTmpDocRelacionado.Text = String.Empty
        lblErrorDocRelacionadoDictamen.Text = String.Empty
        btnDocRelacionadoAceptar.Enabled = False

        If IsNumeric(txtConsecutivoDocRelacionado.Text) Then
            If ddlAreaDocRelacionado.SelectedIndex > 0 Then
                If ddlAnioDocRelacionado.SelectedIndex > 0 Then

                    ID_ANIO_DOC_RELACIONADO = CInt(ddlAnioDocRelacionado.SelectedValue)
                    ID_UNIDAD_ADM_DOC_RELACIONADO = CInt(ddlAreaDocRelacionado.SelectedValue)
                    If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo Then
                        ID_TIPO_DOCUMENTO_DOC_RELACIONADO = OficioTipo.Dictamen
                    ElseIf CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then
                        ID_TIPO_DOCUMENTO_DOC_RELACIONADO = OficioTipo.Oficio_Externo
                    End If

                    I_OFICIO_CONSECUTIVO_DOC_RELACIONADO = CInt(txtConsecutivoDocRelacionado.Text)

                    Dim dtOficio As DataTable =
                                LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(
                                                                                                ID_ANIO_DOC_RELACIONADO,
                                                                                                ID_UNIDAD_ADM_DOC_RELACIONADO,
                                                                                                ID_TIPO_DOCUMENTO_DOC_RELACIONADO,
                                                                                                I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)
                    If dtOficio.Rows.Count > 0 Then
                        '------------------------------------------
                        '  Verificar que exista el PDF del doc a relacionar
                        '------------------------------------------
                        If IsDBNull(dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN")) OrElse dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN").ToString Is String.Empty Then
                            lblErrorDocRelacionadoDictamen.Text = "Oficio no tiene PDF relacionado"
                        Else
                            lnkTmpDocRelacionado.Text = dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN").ToString
                            btnDocRelacionadoAceptar.Enabled = True
                        End If
                    Else
                        lblErrorDocRelacionadoDictamen.Text = "Oficio no existe"
                    End If
                Else
                    lblErrorDocRelacionadoDictamen.Text = "Por favor seleccione un año"
                End If
            Else
                lblErrorDocRelacionadoDictamen.Text = "Por favor seleccione una área"
            End If
        Else
            lblErrorDocRelacionadoDictamen.Text = "Por favor introduzca números en el campo de Consecutivo"
        End If

        modalDocRelacionado.Show()

    End Sub

    Protected Sub lnkOficioExternoRelacionado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkOficioExternoRelacionado.Click
        '----------------------------------------
        ' Si apunta a un oficio externo, abrir el archivo (el PDF)
        ' si el texto es "Buscar" abrir modal.
        '----------------------------------------
        If lnkOficioExternoRelacionado.Text = "Buscar" Then
            ' If BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then
            '----------------------------------------
            '     Abrir modal para selección de Oficio Externo
            '----------------------------------------
            CargarCombo(ddlAreaDocRelacionado, BusinessRules.BDS_C_AREA.GetAreaOficios(CInt(rblEstructuraArea.SelectedValue)), "DSC_COMPOSITE", "ID_UNIDAD_ADM")
            'Dim dtAreaDeUsuario As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO)
            'Dim areaDeUsuario As String = String.Empty
            'If dtAreaDeUsuario.Rows.Count > 0 Then
            '    areaDeUsuario = dtAreaDeUsuario.Rows(0)("ID_UNIDAD_ADM").ToString
            'End If
            'ddlAreaDocRelacionado.SelectedValue = areaDeUsuario

            ddlAreaDocRelacionado.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
            pnlDocRelacionadoTitulo.Text = "Vincular Oficio Externo"

            CargarCombo(ddlAnioDocRelacionado, LogicaNegocioSICOD.BusinessRules.BDA_ANIO.ConsultarAnio, "CICLO", "CICLO")
            If ddlAnioDocRelacionado.Items.IndexOf(New System.Web.UI.WebControls.ListItem(Today.Year.ToString)) > 0 Then ddlAnioDocRelacionado.SelectedValue = Today.Year.ToString

            txtConsecutivoDocRelacionado.Text = String.Empty
            lnkTmpDocRelacionado.Text = String.Empty
            lblErrorDocRelacionadoDictamen.Text = String.Empty
            btnDocRelacionadoAceptar.Enabled = False
            modalDocRelacionado.Show()
            'Else
            '    modalMensaje("Necesita el archivo PDF para relacionar Oficio Externo", , "INFORMACIÓN")
            'End If
        Else
            '----------------------------------------
            ' Abrir documento
            '----------------------------------------
            AbreArchivoLink(lnkOficioExternoRelacionado.Text)
        End If
    End Sub

    Protected Sub chkDictaminado_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkDictaminado.CheckedChanged

        If chkDictaminado.Checked AndAlso lnkDictamenRelacionado.Text Is String.Empty Then

            '----------------------------------------
            ' Revisar que documento tenga PDF relacionado.
            '----------------------------------------
            If BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then
                If BusinessRules.BDA_OFICIO.ConsultarTieneAcuse(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) Then
                    '----------------------------------------
                    '     Abrir modal para selección de Oficio Externo
                    '----------------------------------------
                    CargarCombo(ddlAreaDocRelacionado, BusinessRules.BDS_C_AREA.GetAreaOficios(CInt(rblEstructuraArea.SelectedValue)), "DSC_COMPOSITE", "ID_UNIDAD_ADM")

                    ddlAreaDocRelacionado.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
                    CargarCombo(ddlAnioDocRelacionado, LogicaNegocioSICOD.BusinessRules.BDA_ANIO.ConsultarAnio, "CICLO", "CICLO")
                    If ddlAnioDocRelacionado.Items.IndexOf(New System.Web.UI.WebControls.ListItem(Today.Year.ToString)) > 0 Then ddlAnioDocRelacionado.SelectedValue = Today.Year.ToString

                    txtConsecutivoDocRelacionado.Text = String.Empty
                    lnkTmpDocRelacionado.Text = String.Empty
                    lblErrorDocRelacionadoDictamen.Text = String.Empty
                    pnlDocRelacionadoTitulo.Text = "Vincular Dictamen"
                    btnDocRelacionadoAceptar.Enabled = False
                    modalDocRelacionado.Show()
                Else
                    modalMensaje("Necesita el acuse de respuesta para relacionar Dictamen", , "INFORMACIÓN")
                End If
            Else
                modalMensaje("Necesita el archivo PDF para relacionar Dictamen", , "INFORMACIÓN")
            End If
        ElseIf Not chkDictaminado.Checked AndAlso lnkDictamenRelacionado.Text IsNot String.Empty Then
            '----------------------------------------
            'quitar dictamen relacionado
            '----------------------------------------
            lnkDictamenRelacionado.Text = String.Empty
            chkDictaminado.Checked = False
            btnDeleteDictamen.Visible = False
        End If

    End Sub

    Protected Sub btnDocRelacionadoAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocRelacionadoAceptar.Click
        If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo Then
            lnkDictamenRelacionado.Text = lnkTmpDocRelacionado.Text
            btnDeleteDictamen.Visible = True
        Else
            lnkOficioExternoRelacionado.Text = lnkTmpDocRelacionado.Text
            btnDeleteOficioExterno.Visible = True
        End If
    End Sub

    Protected Sub btnDocRelacionadoCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocRelacionadoCancelar.Click
        If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo Then
            chkDictaminado.Checked = False
        End If
    End Sub

    Protected Sub lnkDictamenRelacionado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkDictamenRelacionado.Click
        '----------------------------------------
        ' Abrir documento
        '----------------------------------------
        AbreArchivoLink(lnkDictamenRelacionado.Text)
    End Sub

    Protected Sub btnDeleteDictamen_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDeleteDictamen.Click
        lnkDictamenRelacionado.Text = String.Empty
        chkDictaminado.Checked = False
        btnDeleteDictamen.Visible = False
    End Sub

    Protected Sub btnDeleteOficioExterno_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDeleteOficioExterno.Click
        lnkOficioExternoRelacionado.Text = "Buscar"
        btnDeleteOficioExterno.Visible = False
    End Sub

    Protected Sub ddlClasificacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlClasificacion.SelectedIndexChanged
        If ddlClasificacion.SelectedIndex > 0 AndAlso CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Externo Then
            If ddlClasificacion.SelectedItem.Text.Contains("Incumplimiento") OrElse ddlClasificacion.SelectedItem.Text.Contains("Otros - Otros") Then
                ddlIncumplimiento.Visible = True
                lblIncumplimiento.Visible = True
            Else
                ddlIncumplimiento.Visible = False
                lblIncumplimiento.Visible = False

            End If
        ElseIf ddlClasificacion.SelectedIndex > 0 AndAlso CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then

            'NHM INI
            'ddlIncumplimiento.Visible = True
            'lblIncumplimiento.Visible = True
            ddlIncumplimiento.Visible = False
            lblIncumplimiento.Visible = False
            'NHM FIN
        Else
            ddlIncumplimiento.Visible = False
            lblIncumplimiento.Visible = False
        End If
    End Sub

    Protected Sub btnBandeja_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBandeja.Click
        Response.Redirect("~/App_Oficios/Bandeja.aspx")
    End Sub


    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        For Each row As DataGridItem In dgMultiplesAfores.Items
            Dim chk As CheckBox = TryCast(row.FindControl("chkSeleccion"), CheckBox)
            chk.Checked = TryCast(sender, CheckBox).Checked
        Next

    End Sub
#End Region

#Region "Metodos y funciones"

    Private Sub cargarUsuariosRubrica()
        Try
            If ddlAreaRubrica.SelectedIndex > 0 Then
                CargarListBox(lstUsuariosRubrica, BusinessRules.BDS_USUARIO.GetAllPorArea(CInt(ddlAreaRubrica.SelectedItem.Value), CInt(rblEstructuraRubricas.SelectedValue)), "NOMBRECOMPLETO", "USUARIO")
            End If
        Catch ex As Exception
            EscribirError(ex, "Cargar controles ddlElaboro, lstUsuariosFirma, lstUsuariosRubrica")
        End Try


    End Sub

    Private Sub cargarUsuarioElaboro()
        Try
            If ddlAreaElaboro.SelectedIndex > 0 Then
                CargarCombo(
                            ddlUsuarioElaboro, BusinessRules.BDS_USUARIO.GetAllPorArea(
                            CType(ddlAreaElaboro.SelectedItem.Value, Integer),
                            CInt(rblEstructuraElaboro.SelectedValue)), "NOMBRECOMPLETO", "USUARIO")
                If ddlUsuarioElaboro.Items.Count = 1 Then ddlUsuarioElaboro.Items(0).Text = "-Seleccione Área-"
            End If
        Catch ex As Exception
            EscribirError(ex, "cargarUsuarioElaboro")
        End Try
    End Sub

    Private Sub deshabilitaControlesLlaveOficio()
        '---------------------------------------------
        ' Deshabilita DropDown de área, año, tipo de documento
        '---------------------------------------------
        ddlArea.Enabled = False
        ddlTipoDocumento.Enabled = False
        ddlAño.Enabled = False
        rblEstructuraArea.Enabled = False

        '---------------------------------------------
        ' Deshabilita múltiples afores
        '---------------------------------------------
        'chkMultiplesAfores.Visible = False
        'lblMultiplesAfores.Visible = False

        trMultiplesAfores.Visible = False

    End Sub

    'Private Sub ddlTipoEntidad_CargarDatos()
    '    Try
    '        If ddlTipoEntidad.SelectedIndex > 0 Then
    '            ddlEntidad.Enabled = True
    '            CargarCombo(ddlEntidad, LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.ConsultarEntidadesPorTipo(CType(ddlTipoEntidad.SelectedValue, Integer)), "T_ENTIDAD_CORTO", "ID_ENTIDAD")
    '            ddlSubentidad.Items.Clear()
    '            Session("ddlEntidad.SelectedValue") = ""
    '        End If
    '    Catch ex As Exception
    '        EscribirError(ex, "ddlTipoEntidad_CargarDatos")
    '    End Try
    'End Sub

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

    Private Sub checkEnviarNotificacion()
        Try
            Dim dtEstatus As DataTable = BusinessRules.BDA_OFICIO.ConsultarEstatusOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            If dtEstatus.Rows(0)("T_ESTATUS").ToString().Trim.ToUpper = "CANCELADO" Or
                        dtEstatus.Rows(0)("T_ESTATUS").ToString().Trim.ToUpper = "CONCLUIDO" Then
                Throw New ApplicationException("No se puede envíar la notificación porque el Estatus del Oficio no lo permite")
            Else

                '------------------------------------------------
                ' Revisar usuario debe poder notificar
                '------------------------------------------------
                If BusinessRules.BDS_USUARIO.ConsultarUsuarioPuedeNotificar(USUARIO) Then

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
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "Enviar Notificacion")
        End Try
    End Sub

    ''' <summary>
    ''' Revisa si el archivo remoto existe en el Servidor
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="credentials"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
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

    ''' <summary>
    ''' Esconde todos los controles excepto el sumario de oficios múltiples creados.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub hideEverything()
        pnlArea.Visible = False
        pnlDatosBasicos.Visible = False
        pnlFechas.Visible = False
        pnlFechasPlazo.Visible = False
        pnlClasificacion.Visible = False
        pnlAsunto.Visible = False
        pnlFirmas.Visible = False
        pnlDocRelacionado.Visible = False
        pnlComentariosCancelacion.Visible = False
        pnlMultipleAfore.Visible = False
        'pnlGenerarDocumento.Visible = False
        pnlGenerarDocumento.Style.Add("display", "none")
        btnGuardar.Visible = False
        iBtnAdjuntarDocumentos.Visible = False
        iBtnSeguimiento.Visible = False

        '' *********************************************
        '' COMENTADO POR JORGE RANGEL  16/AGO/2012
        '' *********************************************
        'iBtnCedula.Visible = False
        'iBtnEnviarNotificacion.Visible = False

        btnCancelar.Visible = False
        Label1.Visible = False
        lblRegistro.Visible = False
        lblRegistroTag.Visible = False

    End Sub

    Private Sub Nuevo()
        '------------------------------------------------------
        ' visibilidad de elementos (checkboxes, paneles y botones)
        '------------------------------------------------------
        chkMultiplesAfores.Checked = False
        pnlMultipleAfore.Visible = False
        pnlDatosBasicos.Visible = True
        'pnlGenerarDocumento.Visible = False
        pnlGenerarDocumento.Style.Add("display", "none")
        btnGuardarGenerarOficios.Visible = False
        ddlArea.Enabled = True
        ddlTipoDocumento.Enabled = True
        ddlAño.Enabled = True
        rblEstructuraArea.Enabled = True
        txtFechaDocumento.Enabled = True
        imgCalFechaDocumento.Visible = True

        pnlDocRelacionado.Visible = False

        ID_EXPEDIENTE = 0

        '------------------------------------------------------
        ' Carga Áreas de acuerdo a estructura seleccionada
        '------------------------------------------------------
        Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))
        CargarCombo(ddlArea, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

        If ddlArea.Items.Count = 0 Then
            txtDestinatario.Text = String.Empty
            destinatarioKey.Value = ""
        Else
            Dim li As ListItem
            li = ddlArea.Items.FindByValue(ID_UNIDAD_ADM_USUARIO.ToString)
            If li IsNot Nothing Then ddlArea.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
        End If

        '------------------------------------------------------
        ' Carga Áreas de acuerdo a estructura seleccionada
        '------------------------------------------------------
        ddlUsuarioElaboro.Items.Add(New ListItem("-Seleccione Área-", "-1"))

        '------------------------------------------------------
        ' Cargar años vigentes (normalmente actual y siguiente)
        '------------------------------------------------------
        CargarCombo(ddlAño, BusinessRules.BDA_ANIO.ConsultarAnioActualYSiguiente, "CICLO", "CICLO")

        '------------------------------------------------------
        ' Limpiar controles
        '------------------------------------------------------
        LimpiarControles()
        CargarDatosDefault()

    End Sub

    Private Sub CargarDatosDefault()

        '------------------------------------------------------
        ' Establece fechas
        '------------------------------------------------------
        txtFechaDocumento.Text = DateTime.Now.ToShortDateString()

        '------------------------------------------------------
        ' Establece Año
        '------------------------------------------------------
        If ddlAño.Items.IndexOf(New System.Web.UI.WebControls.ListItem(Today.Year.ToString)) > 0 Then
            ddlAño.SelectedValue = Today.Year.ToString
        End If

        '------------------------------------------------------
        ' Establece Prioridad
        '------------------------------------------------------
        If ddlPrioridad.Items.FindByText("Normal") IsNot Nothing Then
            ddlPrioridad.SelectedValue = ddlPrioridad.Items.FindByText("Normal").Value
        End If

        '------------------------------------------------------
        ' Establece Estatus
        '------------------------------------------------------
        If ddlEstatus.Items.FindByText("En Elaboración") IsNot Nothing Then
            ddlEstatus.SelectedValue = ddlEstatus.Items.FindByText("En Elaboración").Value
        End If

        '------------------------------------------------------
        ' Establece Usuario
        '------------------------------------------------------
        Dim dtUsuario As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetOne(USUARIO)
        CargarCombo(ddlUsuarioElaboro, dtUsuario, "NOMBRECOMPLETO", "USUARIO")
        ddlUsuarioElaboro.SelectedValue = USUARIO
        lblRegistro.Text = dtUsuario(0)("NOMBRECOMPLETO").ToString

        '------------------------------------------
        ' Elaboró
        '------------------------------------------
        Dim li As ListItem
        Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
        Dim _ID_UNIDAD_ADM_USUARIO As Integer = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
        li = ddlAreaElaboro.Items.FindByValue(_ID_UNIDAD_ADM_USUARIO.ToString)
        If li IsNot Nothing Then
            ddlAreaElaboro.SelectedValue = _ID_UNIDAD_ADM_USUARIO.ToString
            cargarUsuarioElaboro()
        End If

        '------------------------------------------
        ' Rúbricas
        '------------------------------------------
        li = ddlAreaRubrica.Items.FindByValue(ID_UNIDAD_ADM_USUARIO.ToString)
        If li IsNot Nothing Then
            ddlAreaRubrica.SelectedValue = ID_UNIDAD_ADM_USUARIO.ToString
            cargarUsuariosRubrica()
        End If
    End Sub

    Private Sub CargarDatosIniciales()
        Try
            ddlEntidad.Enabled = False
            ddlSubentidad.Enabled = False
            ddlClasificacion.Enabled = False


            CargarCombo(ddlTipoDocumento, LogicaNegocioSICOD.BusinessRules.BDA_TIPO_DOCUMENTO.ConsultarTipoDocumento, "T_TIPO_DOCUMENTO", "ID_TIPO_DOCUMENTO")
            CargarCombo(ddlEstatus, LogicaNegocioSICOD.BusinessRules.BDA_ESTATUS_OFICIO.ConsultarEstatus, "T_ESTATUS", "ID_ESTATUS")
            '18/10/2012 Modificacion por Julio C. Vieyra Tena
            'CargarCombo(ddlTipoEntidad, LogicaNegocioSICOD.BusinessRules.BDA_TIPO_ENTIDAD.ConsultarTipoEntidad, "T_TIPO_ENTIDAD", "ID_TIPO_ENTIDAD")
            'Metodo para la carga de la tabla TipoEntidad en OSIRIS-----------------
            cargarTipoEntidad()
            ''''''''----------------------------------------------------------------
            CargarCombo(ddlPrioridad, LogicaNegocioSICOD.BusinessRules.BDA_PRIORIDAD.ConsultarPrioridad, "T_PRIORIDAD", "ID_PRIORIDAD")
            'CargarCombo(ddlCargoDestinatario, LogicaNegocioSICOD.BusinessRules.BDA_FUNCION.ConsultarFuncion, "T_FUNCION", "ID_FUNCION")
            LoadCargoDestinatarios(False)
            CargarCombo(ddlIncumplimiento, LogicaNegocioSICOD.BusinessRules.BDA_IRREGULARIDAD.ConsultarIrregularidad, "T_IRREGULARIDAD", "ID_IRREGULARIDAD")

            PREVIOUS_TIPO_DOCUMENTO = 0

            Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraArea.SelectedValue))
            ID_UNIDAD_ADM_USUARIO = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            TOP_ID_UNIDAD_ADM_USUARIO = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))

            dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            Dim _ID_UNIDAD_ADM_USUARIO As Integer = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            Dim _TOP_ID_UNIDAD_ADM_USUARIO As Integer = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))

            Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(_TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            CargarCombo(ddlAreaElaboro, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

            If ddlAreaElaboro.Items.Count > 0 Then
                Dim li As ListItem
                li = ddlAreaElaboro.Items.FindByValue(_ID_UNIDAD_ADM_USUARIO.ToString)
                If li IsNot Nothing Then
                    ddlAreaElaboro.SelectedValue = _ID_UNIDAD_ADM_USUARIO.ToString
                    cargarUsuarioElaboro()
                End If
            End If



            CargaAreasFirmas()

            dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraFirmas.SelectedValue))
            _ID_UNIDAD_ADM_USUARIO = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            _TOP_ID_UNIDAD_ADM_USUARIO = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraFirmas.SelectedValue))
            If ddlAreaFirmas.Items.Count > 0 Then
                Dim li As ListItem
                li = ddlAreaFirmas.Items.FindByValue(_ID_UNIDAD_ADM_USUARIO.ToString)
                If li IsNot Nothing Then
                    ddlAreaFirmas.SelectedValue = _ID_UNIDAD_ADM_USUARIO.ToString
                End If
            End If

            CargaUsuariosFirmas()


            Dim _tipoUnidad As UnidadAdministrativaTipo = CType(IIf(CInt(rblEstructuraRubricas.SelectedValue) = 1,
                                                              UnidadAdministrativaTipo.Oficial,
                                                              UnidadAdministrativaTipo.Funcional), UnidadAdministrativaTipo)
            CargarCombo(ddlAreaRubrica, LogicaNegocioSICOD.UnidadAdministrativa.GetList(_tipoUnidad, UnidadAdministrativaEstatus.Activo),
                        "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")


            '------------------------------------------------------
            ' Evento doubleclick para los listbox de firmas, rubrica, destinatario...
            '------------------------------------------------------
            Dim onDblClickScript As String = Page.ClientScript.GetPostBackEventReference(lstUsuariosFirma, "ondblclick")
            lstUsuariosFirma.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstFirmas, "ondblclick")
            lstFirmas.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstUsuariosRubrica, "ondblclick")
            lstUsuariosRubrica.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstRubricas, "ondblclick")
            lstRubricas.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstPersonal, "ondblclick")
            lstPersonal.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstConCopia, "ondblclick")
            lstConCopia.Attributes.Add("ondblclick", onDblClickScript)

        Catch ex As Exception

            modalMensaje(ex.Message)

            'EscribirError(ex, "Cargar DatosIniciales")
        End Try
    End Sub

    Private Sub CargarDatosInicialesMod()
        Try
            ddlEntidad.Enabled = False
            ddlSubentidad.Enabled = False
            ddlClasificacion.Enabled = False


            CargarCombo(ddlTipoDocumento, LogicaNegocioSICOD.BusinessRules.BDA_TIPO_DOCUMENTO.ConsultarTipoDocumento, "T_TIPO_DOCUMENTO", "ID_TIPO_DOCUMENTO")
            CargarCombo(ddlEstatus, LogicaNegocioSICOD.BusinessRules.BDA_ESTATUS_OFICIO.ConsultarEstatus, "T_ESTATUS", "ID_ESTATUS")
            '18/10/2012 Modificacion por Julio C. Vieyra Tena
            'CargarCombo(ddlTipoEntidad, LogicaNegocioSICOD.BusinessRules.BDA_TIPO_ENTIDAD.ConsultarTipoEntidad, "T_TIPO_ENTIDAD", "ID_TIPO_ENTIDAD")
            'Metodo para la carga de la tabla TipoEntidad en OSIRIS-----------------
            cargarTipoEntidad()
            ''''''''----------------------------------------------------------------
            CargarCombo(ddlPrioridad, LogicaNegocioSICOD.BusinessRules.BDA_PRIORIDAD.ConsultarPrioridad, "T_PRIORIDAD", "ID_PRIORIDAD")
            'CargarCombo(ddlCargoDestinatario, LogicaNegocioSICOD.BusinessRules.BDA_FUNCION.ConsultarFuncion, "T_FUNCION", "ID_FUNCION")
            LoadCargoDestinatarios(False)
            CargarCombo(ddlIncumplimiento, LogicaNegocioSICOD.BusinessRules.BDA_IRREGULARIDAD.ConsultarIrregularidad, "T_IRREGULARIDAD", "ID_IRREGULARIDAD")

            PREVIOUS_TIPO_DOCUMENTO = 0

            'Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraArea.SelectedValue))
            'ID_UNIDAD_ADM_USUARIO = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            'TOP_ID_UNIDAD_ADM_USUARIO = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraArea.SelectedValue))

            'dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            'Dim _ID_UNIDAD_ADM_USUARIO As Integer = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            'Dim _TOP_ID_UNIDAD_ADM_USUARIO As Integer = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))

            'Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(_TOP_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraElaboro.SelectedValue))
            'CargarCombo(ddlAreaElaboro, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

            'If ddlAreaElaboro.Items.Count > 0 Then
            '    Dim li As ListItem
            '    li = ddlAreaElaboro.Items.FindByValue(_ID_UNIDAD_ADM_USUARIO.ToString)
            '    If li IsNot Nothing Then
            '        ddlAreaElaboro.SelectedValue = _ID_UNIDAD_ADM_USUARIO.ToString
            '        cargarUsuarioElaboro()
            '    End If
            'End If



            CargaAreasFirmas()

            Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(USUARIO, CInt(rblEstructuraFirmas.SelectedValue))
            Dim _ID_UNIDAD_ADM_USUARIO = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
            Dim _TOP_ID_UNIDAD_ADM_USUARIO = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(_ID_UNIDAD_ADM_USUARIO, CInt(rblEstructuraFirmas.SelectedValue))
            If ddlAreaFirmas.Items.Count > 0 Then
                Dim li As ListItem
                li = ddlAreaFirmas.Items.FindByValue(_ID_UNIDAD_ADM_USUARIO.ToString)
                If li IsNot Nothing Then
                    ddlAreaFirmas.SelectedValue = _ID_UNIDAD_ADM_USUARIO.ToString
                End If
            End If

            CargaUsuariosFirmas()


            Dim _tipoUnidad As UnidadAdministrativaTipo = CType(IIf(CInt(rblEstructuraRubricas.SelectedValue) = 1,
                                                              UnidadAdministrativaTipo.Oficial,
                                                              UnidadAdministrativaTipo.Funcional), UnidadAdministrativaTipo)
            CargarCombo(ddlAreaRubrica, LogicaNegocioSICOD.UnidadAdministrativa.GetList(_tipoUnidad, UnidadAdministrativaEstatus.Activo),
                        "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")


            '------------------------------------------------------
            ' Evento doubleclick para los listbox de firmas, rubrica, destinatario...
            '------------------------------------------------------
            Dim onDblClickScript As String = Page.ClientScript.GetPostBackEventReference(lstUsuariosFirma, "ondblclick")
            lstUsuariosFirma.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstFirmas, "ondblclick")
            lstFirmas.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstUsuariosRubrica, "ondblclick")
            lstUsuariosRubrica.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstRubricas, "ondblclick")
            lstRubricas.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstPersonal, "ondblclick")
            lstPersonal.Attributes.Add("ondblclick", onDblClickScript)

            onDblClickScript = Page.ClientScript.GetPostBackEventReference(lstConCopia, "ondblclick")
            lstConCopia.Attributes.Add("ondblclick", onDblClickScript)

        Catch ex As Exception

            modalMensaje(ex.Message)

            'EscribirError(ex, "Cargar DatosIniciales")
        End Try
    End Sub

    Public Sub cargarTipoEntidad()
        Dim con1 As OracleConexion
        con1 = Nothing
        Try

            con1 = New OracleConexion()
            Dim descripcion As String = Nothing
            Try
                dsTipoEntidad = con1.Datos(" SELECT * FROM osiris.bdv_c_t_entidad where VIG_FLAG=1 order by DESC_T_ENT ")
            Catch ex As Exception
                modalMensaje(ex.Message)
            End Try
            ddlTipoEntidad.DataValueField = "ID_T_ENT"
            ddlTipoEntidad.DataTextField = "DESC_T_ENT"

            ddlTipoEntidad.DataSource = dsTipoEntidad
            ddlTipoEntidad.DataBind()
            If ddlTipoEntidad.Items.Count > 0 Then
                ddlTipoEntidad.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Else
                ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
            End If
        Catch ex As Exception
            modalMensaje(ex.Message)
        Finally
            con1.Cerrar()
            con1 = Nothing
        End Try
    End Sub

    Private Function ArmarNumeroOficio(ByVal pIdArea As Integer, ByVal pIdTipoDocumento As Integer, ByVal pTipoDocumento As String, ByVal pConsecutivo As Integer, ByVal pAnio As Integer, ByVal pInicialesFirma As String) As String
        Dim numeroOficio As String = String.Empty
        Dim dtPrefijo, dtCodigoArea As DataTable

        Select Case pTipoDocumento.Trim.ToUpper
            Case "OFICIO EXTERNO"
                dtPrefijo = LogicaNegocioSICOD.BusinessRules.BDA_NUMERO_OFICIO.ConsultarPrefijoNumeroOficio(pIdTipoDocumento)
                dtCodigoArea = LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.GetCodigoAreaPorUnidadAdm(pIdArea)

                If dtPrefijo.Rows.Count > 0 Then
                    numeroOficio = String.Format("{0}/{1}/{2}/{3}", dtPrefijo.Rows(0)("PREFIJO").ToString(), dtCodigoArea.Rows(0)("I_CODIGO_AREA").ToString, Format(pConsecutivo, "000").ToString(), pAnio.ToString())
                End If
            Case "DICTAMEN"
                dtPrefijo = LogicaNegocioSICOD.BusinessRules.BDA_NUMERO_OFICIO.ConsultarPrefijoNumeroOficio(pIdTipoDocumento, pIdArea)
                If dtPrefijo.Rows.Count > 0 Then
                    numeroOficio = String.Format("{0}/{1}/{2}/{3}", dtPrefijo.Rows(0)("PREFIJO").ToString(), Format(pConsecutivo, "000").ToString(), pInicialesFirma, pAnio.ToString())
                End If

            Case Else

                dtPrefijo = LogicaNegocioSICOD.BusinessRules.BDA_NUMERO_OFICIO.ConsultarPrefijoNumeroOficio(pIdTipoDocumento, pIdArea)
                If dtPrefijo.Rows.Count > 0 Then
                    numeroOficio = String.Format("{0}/{1}/{2}", dtPrefijo.Rows(0)("PREFIJO").ToString(), Format(pConsecutivo, "000").ToString(), pAnio.ToString())
                End If

        End Select
        Return numeroOficio
    End Function

    Private Function Guardar() As Boolean
        Dim continua As Boolean = True
        Dim Campos As String = String.Empty
        Dim Valores As String = String.Empty
        Dim Condicion As String = String.Empty
        Dim sql As String = String.Empty
        Dim tran As Odbc.OdbcTransaction = Nothing
        Dim Sesion As Seguridad = Nothing
        Dim Con As Conexion = Nothing

        Try
            valida_datos()

            Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO
            objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

            '----------------------------------------------
            ' Traerse el primer usuario en la lista de firmas para efectos de las iniciales.
            '----------------------------------------------
            Dim inicialesFirma As String = ""
            If lstFirmas.Items.Count > 0 Then

                Dim dtUsuarioFirma As DataTable = BusinessRules.BDS_USUARIO.GetAllPorUsuario(lstFirmas.Items(0).Value.ToString())
                inicialesFirma = dtUsuarioFirma.Rows(0)("T_INICIALES").ToString()

            End If



            If ID_EXPEDIENTE = 0 Then ID_EXPEDIENTE = BusinessRules.BDA_R_OFICIOS.ObtenerMaximoConsecutivo

            '----------------------------------------------
            ' nuevas clases de conexión y seguridad
            '----------------------------------------------
            Sesion = New Seguridad
            Con = New Conexion()

            Sesion.BitacoraInicia("GuardarOficio", Con)
            tran = Con.BeginTran()
            '----------------------------------------------
            ' 
            '----------------------------------------------

            '----------------------------------------------
            '----------------------------------------------
            Dim dtNumeroOficio As New DataTable
            dtNumeroOficio = New DataTable
            dtNumeroOficio.Columns.Add("NumeroOficio")
            dtNumeroOficio.Columns.Add("OficioReferencia1")
            dtNumeroOficio.Columns.Add("OficioReferencia2")
            dtNumeroOficio.Columns.Add("FechaReferencia1")
            dtNumeroOficio.Columns.Add("FechaReferencia2")

            objOficio.UsuarioAlta = USUARIO

            objOficio.IdTipoEntidad = CType(ddlTipoEntidad.SelectedValue, Integer)

            objOficio.IdEntidad = CInt(ddlEntidad.SelectedValue)

            If ddlSubentidad.SelectedIndex > 0 Then
                objOficio.IdSubentidad = CInt(ddlSubentidad.SelectedValue)
            Else
                objOficio.IdSubentidad = 0
            End If

            '---------------------------------------
            ' Verificar que el destinatario exista en la BD, sino, agregarlo.
            '---------------------------------------
            If txtDestinatario.Enabled Then

                Dim ds As DataSet = Con.Datos("SELECT ID_PERSONA FROM BDA_PERSONAL WHERE VIG_FLAG = 1 AND ID_ENTIDAD = " & objOficio.IdEntidad.ToString() & _
                                              " AND ltrim(rtrim((ISNULL(T_PREFIJO,'')+' '+ISNULL(T_NOMBRE,'')+' '+ISNULL(T_APELLIDO_P,'')+' '+ISNULL(T_APELLIDO_M,'')))) = '" & _
                                              txtDestinatario.Text.Trim.Replace("'", "''") & "'" _
                                              , tran, False)

                If ds.Tables(0).Rows.Count > 0 Then
                    objOficio.IdDestinatario = CInt(ds.Tables(0).Rows(0).Item(0).ToString)
                Else

                    '---------------------------------------
                    ' objeto entities.bda_personal
                    '---------------------------------------
                    Dim objPersonal = New Entities.BDA_PERSONAL
                    objPersonal.Nombre = txtDestinatario.Text.Trim.Replace("'", "''") ''HttpUtility.HtmlEncode(txtDestinatario.Text.Trim)
                    objPersonal.IdEntidad = CInt(ddlEntidad.SelectedValue)
                    objPersonal.IdTipoEntidad = CInt(ddlTipoEntidad.SelectedValue)
                    objPersonal.VigFlag = 1
                    objPersonal.UsrActualizadoPor = USUARIO

                    '---------------------------------------
                    ' Siguiente id personal
                    '---------------------------------------
                    objPersonal.IdPersona = CInt(BusinessRules.BDA_PERSONAL.GetNextIdPersonal)

                    '---------------------------------------
                    ' Insertar
                    '---------------------------------------
                    If BusinessRules.BDA_PERSONAL.Insertar(objPersonal) Then
                        objOficio.IdDestinatario = objPersonal.IdPersona
                    Else
                        Throw New ApplicationException("Error al agregar destinatario, por favor intente de nuevo")
                    End If

                    '---------------------------------------
                    ' Agregar la función del destinatario, no es crítico.
                    '---------------------------------------
                    BusinessRules.BDA_R_PERSONAL_FUNCION.Insertar(objPersonal.IdPersona, CInt(ddlCargoDestinatario.SelectedValue))

                    objPersonal = Nothing

                End If


                'If (Request.Form("destinatarioKey") = String.Empty) OrElse (Request.Form("destinatarioText").Trim().ToUpper() <> txtDestinatario.Text.Trim().ToUpper()) Then
                '    '---------------------------------------
                '    ' Usuario agregó destinatario que no se encuentra en la BD, agregarlo.
                '    ' Lógica:
                '    '   Obtener siguiente id personal THEN Insertar con datos del textbox txtDestinatario
                '    '---------------------------------------

                '    '---------------------------------------
                '    ' objeto entities.bda_personal
                '    '---------------------------------------
                '    Dim objPersonal = New Entities.BDA_PERSONAL
                '    objPersonal.Nombre = txtDestinatario.Text.Trim.Replace("'", "''") ''HttpUtility.HtmlEncode(txtDestinatario.Text.Trim)
                '    objPersonal.IdEntidad = CInt(ddlEntidad.SelectedValue)
                '    objPersonal.VigFlag = 1
                '    objPersonal.UsrActualizadoPor = USUARIO

                '    '---------------------------------------
                '    ' Siguiente id personal
                '    '---------------------------------------
                '    objPersonal.IdPersona = BusinessRules.BDA_PERSONAL.GetNextIdPersonal

                '    '---------------------------------------
                '    ' Insertar
                '    '---------------------------------------
                '    If BusinessRules.BDA_PERSONAL.Insertar(objPersonal) Then
                '        objOficio.IdDestinatario = objPersonal.IdPersona
                '    Else
                '        Throw New ApplicationException("Error al agregar destinatario, por favor intente de nuevo")
                '    End If

                '    '---------------------------------------
                '    ' Agregar la función del destinatario, no es crítico.
                '    '---------------------------------------
                '    BusinessRules.BDA_R_PERSONAL_FUNCION.Insertar(objPersonal.IdPersona, CInt(ddlCargoDestinatario.SelectedValue))

                '    objPersonal = Nothing
                'Else
                '    objOficio.IdDestinatario = CInt(HttpUtility.HtmlEncode(Request.Form("destinatarioKey")))
                'End If
            End If


            objOficio.IdClasificacion = CType(ddlClasificacion.SelectedValue, Integer)
            objOficio.IdEstatus = CType(ddlEstatus.SelectedValue, Integer)
            objOficio.IdPrioridad = CType(ddlPrioridad.SelectedValue, Integer)

            objOficio.Asunto = txtAsunto.Text.Trim.Replace("'", "''") 'HttpUtility.HtmlEncode(txtAsunto.Text.ToString().Trim)
            objOficio.Comentario = txtComentarios.Text.Trim.Replace("'", "''") 'HttpUtility.HtmlEncode(txtComentarios.Text.ToString().Trim)
            objOficio.UsuarioElaboro = ddlUsuarioElaboro.SelectedValue

            'NHM INI
            'objOficio.IdIncumplimiento = CType(IIf(ddlIncumplimiento.SelectedIndex > 0, CType(ddlIncumplimiento.SelectedValue, Integer), 0), Integer)
            objOficio.IdIncumplimiento = 0
            'NHM FIN

            objOficio.Dictaminado = 0 'Temporal, se verifica mas delante

            objOficio.IdFundReserva = 12
            objOficio.PeriodoReserva = 12
            objOficio.IdPuestoDestinatario = CType(ddlCargoDestinatario.SelectedValue, Integer)

            '-----------------------------------------
            ' 
            '-----------------------------------------
            objOficio.Plazo = CType(IIf(chkSeDaPlazo.Checked, 1, 0), Integer)
            If Not String.IsNullOrEmpty(txtPlazo.Text.Trim) Then
                objOficio.PlazoDias = CType(HttpUtility.HtmlEncode(txtPlazo.Text), Integer)
            Else
                objOficio.PlazoDias = 0
            End If
            '-----------------------------------------
            '
            '-----------------------------------------
            If Not String.IsNullOrEmpty(txtFechaRecepcion.Text.Trim) Then
                objOficio.FechaRecepcion = CDate(txtFechaRecepcion.Text)
            End If
            '-----------------------------------------
            '
            '-----------------------------------------
            If Not String.IsNullOrEmpty(txtFechaVencimiento.Text.Trim) Then
                objOficio.FechaVencimineto = CDate(txtFechaVencimiento.Text)
            End If
            '-----------------------------------------
            '
            '-----------------------------------------
            If Not String.IsNullOrEmpty(txtFechaAcuse.Text.Trim) Then
                objOficio.FechaAcuse = CDate(txtFechaAcuse.Text)
            End If

            '-----------------------------------------
            ' Fecha del oficio
            '-----------------------------------------
            If Not String.IsNullOrEmpty(txtFechaDocumento.Text) Then
                objOficio.FechaOficio = CDate(txtFechaDocumento.Text)
            End If
            '-----------------------------------------
            '
            '-----------------------------------------
            objOficio.NotifElectronica = 0
            objOficio.IsFile = 0

            '----------------------------------------------
            ' Formato de fechas
            '----------------------------------------------
            Dim fechaAcuseValidacion As String = String.Empty
            If objOficio.FechaAcuse.ToString("yyyyMMdd") = "00010101" Then
                fechaAcuseValidacion = "NULL"
            Else
                fechaAcuseValidacion = "'" & objOficio.FechaAcuse.ToString("yyyyMMdd") & "'"
            End If

            Dim fechaRecepcionValidacion As String = String.Empty
            If objOficio.FechaRecepcion.ToString("yyyyMMdd") = "00010101" Then
                fechaRecepcionValidacion = "NULL"
            Else
                fechaRecepcionValidacion = "'" & objOficio.FechaRecepcion.ToString("yyyyMMdd") & "'"
            End If

            Dim fechaVencimientoValidacion As String = String.Empty
            If objOficio.FechaVencimineto.ToString("yyyyMMdd") = "00010101" Then
                fechaVencimientoValidacion = "NULL"
            Else
                fechaVencimientoValidacion = "'" & objOficio.FechaVencimineto.ToString("yyyyMMdd") & "'"
            End If

            '---------------------------------------------
            ' Si oficio ya existe, actualizar
            '---------------------------------------------
            If isModificar Then
                '---------------------------------------
                ' Actualizar Oficio
                '---------------------------------------

                '-------------------------------------------
                ' Establece los campos llave
                '-------------------------------------------
                objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
                objOficio.IdArea = ID_UNIDAD_ADM
                objOficio.IdAnio = ID_ANIO
                objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO

                Dim dtBusquedaOficio As DataTable = ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, Con, tran)
                'Dim dtEstatus As DataTable = BusinessRules.BDA_OFICIO.ConsultarEstatusOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                '---------------------------------------------
                ' ESTATUS CONCLUIDO
                ''---------------------------------------------
                If CInt(ddlEstatus.SelectedValue) = OficioEstatus.Concluido Then
                    If IsDBNull(dtBusquedaOficio.Rows(0)("T_HYP_ACUSERESPUESTA")) OrElse
                                String.IsNullOrEmpty(dtBusquedaOficio.Rows(0)("T_HYP_ACUSERESPUESTA").ToString()) Then
                        Throw New ApplicationException("Debes tener registrado el acuse para poder concluir el documento. Verifica por favor.")
                    End If

                    If (IsDBNull(dtBusquedaOficio.Rows(0)("T_HYP_CEDULAPDF")) Or dtBusquedaOficio.Rows(0)("T_HYP_CEDULAPDF").ToString() = "") _
                            AndAlso ID_TIPO_DOCUMENTO = OficioTipo.Oficio_Externo Then
                        Throw New ApplicationException("Debes tener registrada la cédula de notificación para poder concluir el documento. Verifica por favor.")
                    End If


                    If objOficio.Plazo = 1 AndAlso objOficio.FechaRecepcion.ToString("yyyyMMdd") = "00010101" Then
                        Throw New ApplicationException("Debes tener Fecha de Recepción para concluir el Oficio.")
                    End If

                End If

                'If CInt(ddlEstatus.SelectedValue) = OficioEstatus.Enviado AndAlso _
                '    CInt(dtBusquedaOficio.Rows(0)("ID_ESTATUS").ToString()) <> OficioEstatus.Enviado Then

                '    'If dtBusquedaOficio.Rows(0)("FIRMA_SIE_FLAG").ToString() = "0" AndAlso _
                '    '    IsDBNull(dtBusquedaOficio.Rows(0)("T_HYP_ARCHIVOSCAN")) Then



                '    'End If

                'End If



                objOficio.NumeroOficio = dtBusquedaOficio.Rows(0)("T_OFICIO_NUMERO").ToString()

                'NHM INI - Agrega validación
                If GENERAR_NUM_OFICO = True And objOficio.NumeroOficio = "NO DEFINIDO" Then
                    objOficio.NumeroOficio = ArmarNumeroOficio(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ddlTipoDocumento.SelectedItem.Text.ToString(), I_OFICIO_CONSECUTIVO, ID_ANIO, inicialesFirma)

                    If objOficio.NumeroOficio = String.Empty Then Throw New ApplicationException("Número de oficio no se pudo crear")

                End If
                'NHM FIN

                '----------------------------------------------
                '
                '----------------------------------------------

                If ID_TIPO_DOCUMENTO = OficioTipo.Oficio_Externo Then
                    If chkDictaminado.Checked AndAlso lnkDictamenRelacionado.Text IsNot String.Empty Then objOficio.Dictaminado = 1
                End If

                'NHM INI - Se agrega el campo: T_OFICIO_NUMERO y el valor: objOficio.NumeroOficio
                Valores =
                            "ID_PUESTO_DESTINATARIO = {0}, ID_ENTIDAD_TIPO = {1}, ID_ESTATUS = {2}, ID_CLASIFICACION = {3}," & _
                            "USUARIO_ELABORO = '{4}', ID_PRIORIDAD = {5}, ID_INCUMPLIMIENTO = {6}, ID_DESTINATARIO = {7}, ID_SUBENTIDAD = {8}," & _
                            "F_FECHA_OFICIO = '{9}', F_FECHA_ACUSE = {10}, I_PLAZO_DIAS = {11}, F_FECHA_VENCIMIENTO = {12}, F_FECHA_RECEPCION = {13}," & _
                            "PLAZO_FLAG = {14}, T_ASUNTO = '{15}', T_COMENTARIO = '{16}', ID_ENTIDAD = {17}, FIRMA_SIE_FLAG = {18}, " & _
                            "T_OFICIO_NUMERO = '{19}'"



                Valores = String.Format(Valores,
                                        objOficio.IdPuestoDestinatario.ToString,
                                        objOficio.IdTipoEntidad.ToString,
                                        objOficio.IdEstatus.ToString,
                                        objOficio.IdClasificacion.ToString,
                                        objOficio.UsuarioElaboro,
                                        objOficio.IdPrioridad.ToString,
                                        objOficio.IdIncumplimiento.ToString,
                                        objOficio.IdDestinatario.ToString,
                                        objOficio.IdSubentidad.ToString,
                                        objOficio.FechaOficio.ToString("yyyyMMdd"),
                                        fechaAcuseValidacion,
                                        objOficio.PlazoDias.ToString,
                                        fechaVencimientoValidacion,
                                        fechaRecepcionValidacion,
                                        objOficio.Plazo.ToString,
                                        objOficio.Asunto,
                                        objOficio.Comentario,
                                        objOficio.IdEntidad,
                                        rblFirmaSIE.SelectedValue,
                                        objOficio.NumeroOficio
                                       )
                'NHM FIN

                Condicion =
                            "ID_TIPO_DOCUMENTO = " & objOficio.IdTipoDocumento.ToString & _
                            " AND ID_AREA_OFICIO = " & objOficio.IdArea.ToString & _
                            " AND ID_ANIO = " & objOficio.IdAnio.ToString & _
                            " AND I_OFICIO_CONSECUTIVO = " & objOficio.IOficioConsecutivo.ToString

                continua = Con.Actualizar(Conexion.Owner & "BDA_OFICIO", Valores, Condicion, tran)


                If continua Then
                    Dim objFirma As New LogicaNegocioSICOD.Entities.BDA_FIRMA
                    objFirma.IdAnio = ID_ANIO
                    objFirma.IdAreaOficio = ID_UNIDAD_ADM
                    objFirma.IdTipoDocumento = ID_TIPO_DOCUMENTO
                    objFirma.IOficioConsecutivo = I_OFICIO_CONSECUTIVO

                    Condicion =
                                   "ID_ANIO = " & objFirma.IdAnio.ToString & _
                                   " AND ID_AREA_OFICIO= " & objFirma.IdAreaOficio.ToString & _
                                   " AND I_OFICIO_CONSECUTIVO= " & objFirma.IOficioConsecutivo.ToString & _
                                   " AND ID_TIPO_DOCUMENTO=" & objFirma.IdTipoDocumento.ToString

                    Con.Eliminar(Conexion.Owner & "BDA_FIRMA", Condicion, tran)

                    If continua Then

                        If ddlEstatus.SelectedItem.Text.Trim.ToUpper = "CANCELADO" Then
                            objOficio.UsuarioCancelacion = USUARIO

                            objOficio.FechaCancelacion = DateTime.Now
                            objOficio.DescripcionCancelacion = txtCancelacion.Text.Trim.Replace("'", "''") 'HttpUtility.HtmlEncode(txtCancelacion.Text.Trim)

                            Dim FechaCancelacionValidacion As String = String.Empty
                            If objOficio.FechaCancelacion.ToString("yyyyMMdd") = "00010101" Then
                                FechaCancelacionValidacion = "NULL"
                            Else
                                FechaCancelacionValidacion = "'" & objOficio.FechaCancelacion.ToString("yyyyMMdd") & "'"
                            End If

                            '--------------------------------------
                            ' ActualizarOficioCancelado
                            '--------------------------------------

                            Valores = "T_DESCRIPCION_CANCELACION = '{0}', USUARIO_CANCELACION = '{1}' , ID_ESTATUS = {2}, F_FECHA_CANCELACION = {3}"
                            Valores = String.Format(Valores,
                                                    objOficio.DescripcionCancelacion,
                                                    objOficio.UsuarioCancelacion,
                                                    objOficio.IdEstatus.ToString,
                                                    FechaCancelacionValidacion)

                            Condicion =
                                        "ID_TIPO_DOCUMENTO=" & objOficio.IdTipoDocumento.ToString & _
                                        " AND ID_AREA_OFICIO=" & objOficio.IdArea.ToString & _
                                        " AND ID_ANIO=" & objOficio.IdAnio.ToString & _
                                        " AND I_OFICIO_CONSECUTIVO=" & objOficio.IOficioConsecutivo.ToString

                            continua = Con.Actualizar(Conexion.Owner & "BDA_OFICIO", Valores, Condicion, tran)



                            If Not continua Then Throw New Exception("Error en Oficio")


                            'If continua Then

                            '    'GUARDAMOS EN BITACORA cancelado
                            '    Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                            '        ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                            '    Valores = objOficio.IdArea.ToString & "," & _
                            '        objOficio.IdTipoDocumento.ToString & "," & _
                            '        objOficio.IdAnio.ToString & "," & _
                            '        objOficio.IOficioConsecutivo.ToString & ",'" & _
                            '        USUARIO & "','" & USUARIO & "','" & USUARIO & "',GETDATE(),17,'" & objOficio.DescripcionCancelacion & "'," & fechaVencimientoValidacion

                            '    continua = Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran)

                            '    If Not continua Then Throw New Exception("Error en Oficio")

                            'End If


                        End If


                        'GUARDAMOS EN BITACORA actualizacion
                        If continua Then
                            Dim _msg As String = ""
                            If ddlEstatus.SelectedItem.Text.Trim.ToUpper = "CANCELADO" Then

                                _msg = "CANCELADO: " & objOficio.DescripcionCancelacion

                            End If

                            Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                                ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                            Valores = objOficio.IdArea.ToString & "," & _
                                objOficio.IdTipoDocumento.ToString & "," & _
                                objOficio.IdAnio.ToString & "," & _
                                objOficio.IOficioConsecutivo.ToString & ",'" & _
                                USUARIO & "','" & USUARIO & "','" & USUARIO & "',GETDATE(),6,'" & _msg & "'," & fechaVencimientoValidacion

                            continua = Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran)

                        End If


                        '--------------------------------------
                        'Revisar si tiene archivo asociado
                        ' ID_TIPO_DOCUMENTO = 1 - Oficio Externo
                        ' ID_TIPO_DOCUMENTO = 2 - Dictamen
                        '--------------------------------------
                        If ID_TIPO_DOCUMENTO = OficioTipo.Oficio_Externo Then

                            '--------------------------------------
                            ' Revisar si tiene dictámen relacionado.
                            '--------------------------------------

                            sql =
                                   "   SELECT                      " +
                                   "       ID_AREA_OFICIO,         " +
                                   "       ID_TIPO_DOCUMENTO,      " +
                                   "       ID_ANIO,                " +
                                   "       I_OFICIO_CONSECUTIVO    " +
                                   "   FROM BDA_R_OFICIOS          " +
                                   "   WHERE ID_EXPEDIENTE =       " + ID_EXPEDIENTE.ToString +
                                   "   AND ID_TIPO_DOCUMENTO = 2   " +
                                   "   AND INICIAL_FLAG = 1        "
                            Dim dr As Odbc.OdbcDataReader
                            dr = Con.Consulta(sql, tran)
                            If dr.HasRows Then

                                If objOficio.Dictaminado = 0 Then
                                    '-------------------------------------------
                                    ' Tiene dictamen relacionado hasta el momento, pero acabamos de quitar el vínculo.
                                    ' Obtener nuevo ID_EXPEDIENTE y asignarlo al dictamen desvinculado.
                                    '-------------------------------------------

                                    While dr.Read
                                        ID_UNIDAD_ADM_DOC_RELACIONADO = CInt(dr("ID_AREA_OFICIO"))
                                        ID_ANIO_DOC_RELACIONADO = CInt(dr("ID_ANIO"))
                                        I_OFICIO_CONSECUTIVO_DOC_RELACIONADO = CInt(dr("I_OFICIO_CONSECUTIVO"))
                                        ID_TIPO_DOCUMENTO_DOC_RELACIONADO = CInt(dr("ID_TIPO_DOCUMENTO"))
                                        Exit While
                                    End While
                                    dr.Close()

                                    ID_EXPEDIENTE = ConsultarMaximoConsecutivoExpediente(Con, tran)

                                    Valores = "ID_EXPEDIENTE={0}, USUARIO_ASOCIO='{1}', FECH_ASOCIO={2}, INICIAL_FLAG={3}"
                                    Valores = String.Format(Valores, ID_EXPEDIENTE.ToString, USUARIO, "GETDATE()", "1")

                                    Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                                    Condicion = String.Format(Condicion, ID_UNIDAD_ADM_DOC_RELACIONADO, ID_TIPO_DOCUMENTO_DOC_RELACIONADO, ID_ANIO_DOC_RELACIONADO, I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)

                                    continua = Con.Actualizar(Conexion.Owner & "BDA_R_OFICIOS", Valores, Condicion, tran)

                                    If Not continua Then Throw New SystemException("Error Actualizando Oficio")
                                Else
                                    dr.Close()

                                End If
                            Else

                                If objOficio.Dictaminado = 1 Then
                                    dr.Close()
                                    '-------------------------------------------
                                    ' No tiene dictamen relacionado hasta el momento, pero se vinculó uno:
                                    '-------------------------------------------

                                    Valores = "ID_EXPEDIENTE={0}, USUARIO_ASOCIO='{1}', FECH_ASOCIO={2}, INICIAL_FLAG={3}"
                                    Valores = String.Format(Valores, ID_EXPEDIENTE.ToString, USUARIO, "GETDATE()", "1")

                                    Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                                    Condicion = String.Format(Condicion, ID_UNIDAD_ADM_DOC_RELACIONADO, ID_TIPO_DOCUMENTO_DOC_RELACIONADO, ID_ANIO_DOC_RELACIONADO, I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)

                                    continua = Con.Actualizar(Conexion.Owner & "BDA_R_OFICIOS", Valores, Condicion, tran)

                                    If Not continua Then Throw New ApplicationException("Error Actualizando Oficio")

                                    '-------------------------------------------
                                    ' Copiar el incumplimiento del dictámen al oficio externo presente.
                                    '-------------------------------------------

                                    sql =
                                            "SELECT ID_INCUMPLIMIENTO " +
                                            " FROM BDA_OFICIO " +
                                            " WHERE ID_ANIO  =" + ID_ANIO_DOC_RELACIONADO.ToString +
                                            " AND ID_AREA_OFICIO =" + ID_UNIDAD_ADM_DOC_RELACIONADO.ToString +
                                            " AND ID_TIPO_DOCUMENTO = " + ID_TIPO_DOCUMENTO_DOC_RELACIONADO.ToString +
                                            " AND I_OFICIO_CONSECUTIVO = " + I_OFICIO_CONSECUTIVO_DOC_RELACIONADO.ToString

                                    dr = Con.Consulta(sql, tran)
                                    Dim incumplimientoDeDictamen As Integer
                                    If (dr.HasRows) Then
                                        While (dr.Read())
                                            incumplimientoDeDictamen = CInt(dr.GetValue(0))
                                        End While
                                        dr.Close()
                                    End If

                                    Valores = "ID_INCUMPLIMIENTO={0}"
                                    Valores = String.Format(Valores, incumplimientoDeDictamen.ToString)

                                    Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                                    Condicion = String.Format(Condicion, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ID_ANIO, I_OFICIO_CONSECUTIVO)

                                    continua = Con.Actualizar(Conexion.Owner & "BDA_OFICIO", Valores, Condicion, tran)

                                    If Not continua Then Throw New ApplicationException("Error Actualizando Oficio")
                                Else
                                    dr.Close()
                                End If

                            End If

                        End If

                    Else
                        Throw New ApplicationException("Error en Oficio")

                    End If
                Else
                    Throw New ApplicationException("Error en Oficio")
                End If


                If continua Then

                    Dim drow As DataRow = dtNumeroOficio.NewRow
                    drow("NumeroOficio") = objOficio.NumeroOficio
                    drow("OficioReferencia1") = "" 'txtOficioReferencia1
                    drow("OficioReferencia2") = "" 'txtOficioReferencia2
                    drow("FechaReferencia1") = "" 'txtFechaReferencia1
                    drow("FechaReferencia2") = "" 'txtFechaReferencia2
                    dtNumeroOficio.Rows.Add(drow)

                End If

            Else

                '----------------------------------------------
                ' ESTABLECE LOS CAMPOS LLAVE
                '----------------------------------------------
                ID_ANIO = CInt(ddlAño.SelectedValue)
                objOficio.IdAnio = ID_ANIO

                ID_UNIDAD_ADM = CInt(ddlArea.SelectedValue)
                objOficio.IdArea = ID_UNIDAD_ADM

                ID_TIPO_DOCUMENTO = CInt(ddlTipoDocumento.SelectedValue)
                objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO

                'NHM INI
                'I_OFICIO_CONSECUTIVO = ConsultarMaximoConsecutivo(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, Con, tran)
                Dim updt_B_APLICA_NUM_CONSEC As Boolean = False
                updt_B_APLICA_NUM_CONSEC = get_B_APLICA_NUM_CONSEC(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, Con, tran)
                I_OFICIO_CONSECUTIVO = AplicaNumeroConsecutivo(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, Con, tran)
                'NHM FIN

                objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO


                Dim dtCodigoArea As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.GetCodigoAreaPorUnidadAdm(ID_UNIDAD_ADM)
                CODIGO_AREA = CInt(dtCodigoArea.Rows(0)("I_CODIGO_AREA"))

                '----------------------------------------------
                ' Armar número de oficio
                '----------------------------------------------
                'NHM INI - Agrega validación
                If GENERAR_NUM_OFICO = True Then
                    objOficio.NumeroOficio = ArmarNumeroOficio(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ddlTipoDocumento.SelectedItem.Text.ToString(), I_OFICIO_CONSECUTIVO, ID_ANIO, inicialesFirma)

                    If objOficio.NumeroOficio = String.Empty Then Throw New ApplicationException("Número de oficio no se pudo crear")
                Else
                    objOficio.NumeroOficio = "NO DEFINIDO"
                End If
                'NHM FIN

                Dim drow As DataRow = dtNumeroOficio.NewRow
                drow("NumeroOficio") = objOficio.NumeroOficio
                drow("OficioReferencia1") = "" 'txtOficioReferencia1
                drow("OficioReferencia2") = "" 'txtOficioReferencia2
                drow("FechaReferencia1") = "" 'txtFechaReferencia1
                drow("FechaReferencia2") = "" 'txtFechaReferencia2
                dtNumeroOficio.Rows.Add(drow)

                '--------------------------------------
                ' Insertar Datos Básicos
                '--------------------------------------
                Campos =
                            "ID_ANIO, ID_AREA_OFICIO, I_OFICIO_CONSECUTIVO," & _
                            "ID_TIPO_DOCUMENTO, T_OFICIO_NUMERO, USUARIO_ALTA," & _
                            "ID_ENTIDAD_TIPO, ID_ENTIDAD, ID_SUBENTIDAD, ID_DESTINATARIO," & _
                            "ID_CLASIFICACION, ID_ESTATUS, ID_PRIORIDAD, F_FECHA_OFICIO," & _
                            "F_FECHA_ALTA, T_ASUNTO, T_COMENTARIO, USUARIO_ELABORO," & _
                            "ID_INCUMPLIMIENTO, DICTAMINADO_FLAG, ID_FUNDRESERVA," & _
                            "I_PERIODO_RESERVA, ID_PUESTO_DESTINATARIO, PLAZO_FLAG, I_PLAZO_DIAS," & _
                            "F_FECHA_RECEPCION, F_FECHA_VENCIMIENTO, F_FECHA_ACUSE, NOTIF_ELECTRONICA_FLAG, IS_FILE_FLAG, FIRMA_SIE_FLAG"

                Valores =
                            objOficio.IdAnio.ToString & "," & _
                            objOficio.IdArea.ToString & "," & _
                            objOficio.IOficioConsecutivo.ToString & "," & _
                            objOficio.IdTipoDocumento.ToString & ",'" & _
                            objOficio.NumeroOficio.ToString & "','" & _
                            objOficio.UsuarioAlta & "'," & _
                            objOficio.IdTipoEntidad.ToString & "," & _
                            objOficio.IdEntidad.ToString & "," & _
                            objOficio.IdSubentidad.ToString & "," & _
                            objOficio.IdDestinatario.ToString & "," & _
                            objOficio.IdClasificacion.ToString & "," & _
                            objOficio.IdEstatus.ToString & "," & _
                            objOficio.IdPrioridad.ToString & ",'" & _
                            objOficio.FechaOficio.ToString("yyyyMMdd") & "'," & _
                            "GETDATE(),'" & _
                            objOficio.Asunto & "','" & _
                            objOficio.Comentario & "','" & _
                            objOficio.UsuarioElaboro & "'," & _
                            objOficio.IdIncumplimiento.ToString & "," & _
                            objOficio.Dictaminado.ToString & "," & _
                            objOficio.IdFundReserva.ToString & "," & _
                            objOficio.PeriodoReserva.ToString & "," & _
                            objOficio.IdPuestoDestinatario.ToString & "," & _
                            objOficio.Plazo.ToString & "," & _
                            objOficio.PlazoDias.ToString & "," & _
                            fechaRecepcionValidacion & "," & _
                            fechaVencimientoValidacion & "," & _
                            fechaAcuseValidacion & "," & _
                            objOficio.NotifElectronica.ToString & "," & _
                            objOficio.IsFile.ToString & "," & _
                            rblFirmaSIE.SelectedValue

                continua = Con.Insertar(Conexion.Owner & "BDA_OFICIO", Campos, Valores, tran)
                'resultado = BusinessRules.BDA_OFICIO.InsertarDatosBasicos(objOficio)

                If Not continua Then Throw New ApplicationException("Error creando Oficio")

                'NHM INI - Actualizar consecutivo 
                If updt_B_APLICA_NUM_CONSEC = True Then
                    Dim valUpd As String = " B_APLICA_NUM_CONSEC = 0 "
                    Dim condUpd As String = " ID_UNIDAD_ADM = " + ID_UNIDAD_ADM.ToString() + " and ID_ANIO = " + ID_ANIO.ToString() + " and ID_TIPO_DOCUMENTO  = " + ID_TIPO_DOCUMENTO.ToString()
                    continua = Con.Actualizar(Conexion.Owner & "BDA_C_CONSECUTIVO_OFICIOS", valUpd, condUpd, tran)
                    If Not continua Then Throw New ApplicationException("Error creando Oficio")
                End If
                'NHM FIN

                'GUARDAMOS EN BITACORA
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = objOficio.IdArea.ToString & "," & _
                    objOficio.IdTipoDocumento.ToString & "," & _
                    objOficio.IdAnio.ToString & "," & _
                    objOficio.IOficioConsecutivo.ToString & ",'" & _
                    USUARIO & "','" & USUARIO & "','" & USUARIO & "',GETDATE(),4,NULL," & fechaVencimientoValidacion

                continua = Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran)

                If Not continua Then Throw New ApplicationException("Error creando Oficio")

                '--------------------------------------
                ' Agregarse a BDA_R_OFICIOS.
                ' Obtener máx ID_Expediente.
                '--------------------------------------
                Dim obj_R_OFICIOS As New Entities.BDA_R_OFICIOS
                obj_R_OFICIOS.ID_EXPEDIENTE = ID_EXPEDIENTE

                obj_R_OFICIOS.ID_ANIO = ID_ANIO
                obj_R_OFICIOS.ID_AREA_OFICIO = ID_UNIDAD_ADM
                obj_R_OFICIOS.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
                obj_R_OFICIOS.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO
                obj_R_OFICIOS.INICIAL_FLAG = 1
                obj_R_OFICIOS.USUARIO_ASOCIO = USUARIO

                Campos =
                       "ID_ANIO, ID_AREA_OFICIO, I_OFICIO_CONSECUTIVO, ID_TIPO_DOCUMENTO, ID_EXPEDIENTE, USUARIO_ASOCIO, FECH_ASOCIO, INICIAL_FLAG"

                Valores =
                       obj_R_OFICIOS.ID_ANIO.ToString & "," & _
                       obj_R_OFICIOS.ID_AREA_OFICIO.ToString & "," & _
                       obj_R_OFICIOS.I_OFICIO_CONSECUTIVO.ToString & "," & _
                       obj_R_OFICIOS.ID_TIPO_DOCUMENTO.ToString & "," & _
                       obj_R_OFICIOS.ID_EXPEDIENTE.ToString & ",'" & _
                       obj_R_OFICIOS.USUARIO_ASOCIO & "'," & _
                       "GETDATE()," & _
                       obj_R_OFICIOS.INICIAL_FLAG.ToString

                continua = Con.Insertar(Conexion.Owner & "BDA_R_OFICIOS", Campos, Valores, tran)

                If continua Then

                    'pnlNumeroOficio.Visible = True
                    VerNumeroOficio(True)
                    iBtnNuevo.Visible = True
                Else

                    Throw New Exception("Error creando Oficio")

                End If

            End If

            If continua Then continua = GuardarFirmas(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, Con, tran)

            '--------------------------------------------------
            ' Si es dictamen, revisar si tiene oficio externo relacionado.
            '--------------------------------------------------
            If continua Then
                If ID_TIPO_DOCUMENTO = 2 Then

                    'Consultar si tiene Oficio externo relacionado.
                    sql =
                            "   SELECT                      " +
                            "       ID_AREA_OFICIO,         " +
                            "       ID_TIPO_DOCUMENTO,      " +
                            "       ID_ANIO,                " +
                            "       I_OFICIO_CONSECUTIVO    " +
                            "   FROM BDA_R_OFICIOS          " +
                            "   WHERE ID_EXPEDIENTE =       " + ID_EXPEDIENTE.ToString +
                            "   AND ID_TIPO_DOCUMENTO = 1   " +
                            "   AND INICIAL_FLAG = 1        "
                    Dim dr As Odbc.OdbcDataReader
                    dr = Con.Consulta(sql, tran)

                    If dr.HasRows Then

                        '--------------------------------------------------
                        ' Tiene oficio externo relacionado.
                        '--------------------------------------------------
                        If lnkOficioExternoRelacionado.Text = "Buscar" Then
                            '--------------------------------------------------
                            ' Pero se acaba de desvincular
                            ' obtener nuevo ID_EXPEDIENTE para desvincularlo.
                            '--------------------------------------------------

                            While dr.Read
                                ID_UNIDAD_ADM_DOC_RELACIONADO = CInt(dr("ID_AREA_OFICIO"))
                                ID_ANIO_DOC_RELACIONADO = CInt(dr("ID_ANIO"))
                                I_OFICIO_CONSECUTIVO_DOC_RELACIONADO = CInt(dr("I_OFICIO_CONSECUTIVO"))
                                ID_TIPO_DOCUMENTO_DOC_RELACIONADO = CInt(dr("ID_TIPO_DOCUMENTO"))
                                Exit While
                            End While
                            dr.Close()

                            ID_EXPEDIENTE = ConsultarMaximoConsecutivoExpediente(Con, tran)
                            Valores = "ID_EXPEDIENTE={0}, USUARIO_ASOCIO='{1}', FECH_ASOCIO={2}, INICIAL_FLAG={3}"
                            Valores = String.Format(Valores, ID_EXPEDIENTE.ToString, USUARIO, "GETDATE()", "1")

                            Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                            Condicion = String.Format(Condicion, ID_UNIDAD_ADM_DOC_RELACIONADO, ID_TIPO_DOCUMENTO_DOC_RELACIONADO, ID_ANIO_DOC_RELACIONADO, I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)

                            continua = Con.Actualizar(Conexion.Owner & "BDA_R_OFICIOS", Valores, Condicion, tran)

                            If Not continua Then Throw New SystemException("Error Actualizando Oficio")

                            '-------------------------------------------
                            ' Establecer la bandera de dictaminado del oficio externo en 0.
                            '-------------------------------------------
                            Valores = "DICTAMINADO_FLAG=0"
                            Valores = String.Format(Valores, objOficio.IdIncumplimiento)

                            Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                            Condicion = String.Format(Condicion, ID_UNIDAD_ADM_DOC_RELACIONADO, ID_TIPO_DOCUMENTO_DOC_RELACIONADO, ID_ANIO_DOC_RELACIONADO, I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)

                            continua = Con.Actualizar(Conexion.Owner & "BDA_OFICIO", Valores, Condicion, tran)

                            If Not continua Then Throw New SystemException("Error Actualizando Oficio")

                        End If
                    Else
                        dr.Close()
                        If lnkOficioExternoRelacionado.Text IsNot String.Empty AndAlso lnkOficioExternoRelacionado.Text <> "Buscar" Then
                            '--------------------------------------------------
                            ' No tiene oficio externo relacionado,
                            ' pero se acaba de vincular uno.
                            '--------------------------------------------------

                            Valores = "ID_EXPEDIENTE={0}, USUARIO_ASOCIO='{1}', FECH_ASOCIO={2}, INICIAL_FLAG={3}"
                            Valores = String.Format(Valores, ID_EXPEDIENTE.ToString, USUARIO, "GETDATE()", "1")


                            Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                            Condicion = String.Format(Condicion, ID_UNIDAD_ADM_DOC_RELACIONADO, ID_TIPO_DOCUMENTO_DOC_RELACIONADO, ID_ANIO_DOC_RELACIONADO, I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)


                            continua = Con.Actualizar(Conexion.Owner & "BDA_R_OFICIOS", Valores, Condicion, tran)

                            If Not continua Then Throw New SystemException("Error Actualizando Oficio")


                            '-------------------------------------------
                            ' Establecer incumplimiento del Oficio Externo Relacionado (actualizar con el presente).
                            ' Aprovechar para poner la bandera de dictaminado en 1.
                            '-------------------------------------------
                            Valores = "ID_INCUMPLIMIENTO={0}, DICTAMINADO_FLAG=1"
                            Valores = String.Format(Valores, objOficio.IdIncumplimiento)

                            Condicion = "ID_AREA_OFICIO={0} AND ID_TIPO_DOCUMENTO={1} AND ID_ANIO={2} AND I_OFICIO_CONSECUTIVO={3}"
                            Condicion = String.Format(Condicion, ID_UNIDAD_ADM_DOC_RELACIONADO, ID_TIPO_DOCUMENTO_DOC_RELACIONADO, ID_ANIO_DOC_RELACIONADO, I_OFICIO_CONSECUTIVO_DOC_RELACIONADO)

                            continua = Con.Actualizar(Conexion.Owner & "BDA_OFICIO", Valores, Condicion, tran)

                            If Not continua Then Throw New SystemException("Error Actualizando Oficio")

                        End If

                    End If

                End If

            End If

            If continua Then

                Session("OficiosReferencia") = dtNumeroOficio
                tran.Commit()
                Sesion.BitacoraFinalizaTransaccion(True)

                Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
                Session("ID_ANIO") = ID_ANIO
                Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
                Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO

                If isModificar AndAlso objOficio.IdEstatus <> OLD_ESTATUS Then
                    isEstatusModificado = True
                    NEW_ESTATUS = objOficio.IdEstatus
                    Session.Remove("OLD_ESTATUS")
                End If


                If ISATENCION Then

                    Session(BusinessRules.BDA_OFICIO.SessionAtencionResult) = String.Format("{0}|{1}|{2}|{3}*", _
                                                                                            ID_UNIDAD_ADM.ToString(), _
                                                                                            ID_TIPO_DOCUMENTO.ToString(), _
                                                                                            ID_ANIO.ToString(), _
                                                                                            I_OFICIO_CONSECUTIVO.ToString())


                Else
                    'NHM INI - Agrega validación
                    If objOficio.NumeroOficio = "NO DEFINIDO" Then
                        modalMensaje("La información se guardó de forma correcta. Número de Oficio: <strong>" & objOficio.NumeroOficio & "</strong>", "GuardarOK", "INFORMACIÓN", False, "Aceptar")
                    Else
                        modalMensaje("Número de Oficio: <strong>" & objOficio.NumeroOficio & "</strong>", "GuardarOK", "INFORMACIÓN", False, "Aceptar")
                    End If
                    'NHM FIN
                End If

            Else
                Throw New Exception("Error en Oficio")
            End If




        Catch ex As ApplicationException

            If tran IsNot Nothing Then

                tran.Rollback()
                Sesion.BitacoraFinalizaTransaccion(False)

            End If

            Throw New ApplicationException(ex.Message)
        Catch ex As Exception

            If tran IsNot Nothing Then

                tran.Rollback()
                Sesion.BitacoraFinalizaTransaccion(False)

            End If

            'EscribirError(ex, "Guardar Oficio")
            _PnombreFuncion = "Guardar Oficio"
            Throw New Exception(ex.Message)
        Finally
            If Not Con Is Nothing Then Con.Cerrar()

            If tran IsNot Nothing AndAlso tran.Connection IsNot Nothing AndAlso tran.Connection.State = ConnectionState.Open Then
                tran.Connection.Close()
            End If

            Con = Nothing
            tran = Nothing

        End Try

        Return continua

    End Function

    Private Sub LimpiarControles()
        ddlArea.SelectedIndex = 0
        ddlAño.SelectedIndex = 0
        ddlTipoDocumento.SelectedIndex = 0
        ddlPrioridad.SelectedIndex = 0
        ddlEstatus.SelectedIndex = 0
        ddlTipoEntidad.SelectedIndex = 0
        ddlEntidad.SelectedIndex = -1
        ddlSubentidad.SelectedIndex = -1
        ddlCargoDestinatario.SelectedIndex = 0

        txtFechaDocumento.Text = String.Empty
        txtFechaAcuse.Text = String.Empty
        chkSeDaPlazo.Checked = False
        txtPlazo.Text = String.Empty
        txtFechaRecepcion.Text = String.Empty
        txtFechaVencimiento.Text = String.Empty
        ddlClasificacion.SelectedIndex = -1
        ddlIncumplimiento.SelectedIndex = 0
        txtAsunto.Text = String.Empty
        txtComentarios.Text = String.Empty
        lstUsuariosRubrica.Items.Clear()
        lstFirmas.Items.Clear()
        lstRubricas.Items.Clear()

        chkDictaminado.Checked = False
        lstConCopia.Items.Clear()
        ddlDirigido.SelectedIndex = -1
        lstPersonal.Items.Clear()
        'lblMultiplesAfores.Visible = True
        'chkMultiplesAfores.Visible = True
        trMultiplesAfores.Visible = True
        '-----------------------------------------------
        ' Visualizar botones
        '-----------------------------------------------
        VisualizarBotones(False)
        btnGuardar.Visible = True
        '-----------------------------------------------
        ' ddl usuarios elaboró
        '-----------------------------------------------
        ddlUsuarioElaboro.Enabled = True
        ddlUsuarioElaboro.SelectedIndex = -1

        ddlEntidad.Enabled = False
        ddlSubentidad.Enabled = False

        ddlClasificacion.Enabled = False
        'pnlNumeroOficio.Visible = False
        VerNumeroOficio(False)
        pnlNumerosOficiosMultiplesAfores.Visible = False
        pnlComentariosCancelacion.Visible = False
        rblFirmaSIE.SelectedValue = "1"


        For Each dataItem As DataGridItem In dgMultiplesAfores.Items
            Dim chkSeleccion As System.Web.UI.WebControls.CheckBox = CType(dataItem.FindControl("chkSeleccion"), System.Web.UI.WebControls.CheckBox)
            Dim txtOficioReferencia1 As TextBox = CType(dataItem.FindControl("txtOficioReferencia1"), TextBox)
            Dim txtOficioReferencia2 As TextBox = CType(dataItem.FindControl("txtOficioReferencia2"), TextBox)
            Dim txtFechaReferencia1 As TextBox = CType(dataItem.FindControl("txtFechaReferencia1"), TextBox)
            Dim txtFechaReferencia2 As TextBox = CType(dataItem.FindControl("txtFechaReferencia2"), TextBox)
            chkSeleccion.Checked = False
            txtOficioReferencia1.Text = ""
            txtOficioReferencia2.Text = ""
            txtFechaReferencia1.Text = ""
            txtFechaReferencia2.Text = ""
        Next
    End Sub

    Private Sub LlenarGridAfores()
        'MODIFICADO POR JORGE RANGEL 18 OCT 2012
        Dim con1 As New OracleConexion
        con1 = Nothing
        con1 = New OracleConexion()
        Try
            'Dim dtAfores As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.ConsultarEntidadesNomCortoPorTipo(LogicaNegocioSICOD.BusinessRules.BDA_TIPO_ENTIDAD.ConsultarIdPorNombre("AFORE"))
            Dim dtAfores As DataTable = con1.Datos(" SELECT CVE_ID_ENT as ID_ENTIDAD, SIGLAS_ENT as T_ENTIDAD_CORTO FROM osiris.BDV_C_ENTIDAD where ID_T_ENT=" & _
                                                   WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_AFORE").ToString() & " and VIG_FLAG=1 and CVE_ID_ENT > 0 order by SIGLAS_ENT").Tables(0)


            dgMultiplesAfores.DataSource = dtAfores
            dgMultiplesAfores.DataBind()
        Catch ex As Exception
            EscribirError(ex, "Llenar Grid Afores")
        End Try
    End Sub

    Private Function GuardarMultiplesAfores() As Boolean

        Dim continua As Boolean = True
        Dim Campos As String = String.Empty
        Dim Valores As String = String.Empty
        Dim Condicion As String = String.Empty
        Dim tran As Odbc.OdbcTransaction = Nothing
        Dim Sesion As Seguridad = Nothing
        Dim Con As Conexion = Nothing

        Dim chkSeleccion As System.Web.UI.WebControls.CheckBox
        Dim resultado As Boolean = False
        Dim resultadoFunc As Boolean = False
        Dim strErr As List(Of String)
        strErr = New List(Of String)

        Dim hasCheckedElements As Boolean = False

        Dim CadenaAtencion As String = ""

        Try
            valida_datos()

            ID_TIPO_DOCUMENTO = OficioTipo.Oficio_Externo
            ID_ANIO = CInt(ddlAño.SelectedValue)
            ID_UNIDAD_ADM = CInt(ddlArea.SelectedValue)

            Dim dtCodigoArea As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.GetCodigoAreaPorUnidadAdm(ID_UNIDAD_ADM)
            CODIGO_AREA = CInt(dtCodigoArea.Rows(0)("I_CODIGO_AREA"))


            '----------------------------------------------
            ' Nuevas conexiones
            '----------------------------------------------
            Sesion = New Seguridad
            Con = New Conexion()

            Sesion.BitacoraInicia("GuardarMultiplesAfores", Con)
            tran = Con.BeginTran()

            '----------------------------------------------
            '----------------------------------------------
            Dim dtNumeroOficio As New DataTable
            dtNumeroOficio = New DataTable
            dtNumeroOficio.Columns.Add("NumeroOficio")
            dtNumeroOficio.Columns.Add("OficioReferencia1")
            dtNumeroOficio.Columns.Add("OficioReferencia2")
            dtNumeroOficio.Columns.Add("FechaReferencia1")
            dtNumeroOficio.Columns.Add("FechaReferencia2")

            For Each dataItem As DataGridItem In dgMultiplesAfores.Items
                chkSeleccion = CType(dataItem.FindControl("chkSeleccion"), System.Web.UI.WebControls.CheckBox)
                If chkSeleccion.Checked Then

                    hasCheckedElements = True
                    Dim objOficio As New LogicaNegocioSICOD.Entities.BDA_OFICIO

                    Dim txtOficioReferencia1 As String = CType(dataItem.FindControl("txtOficioReferencia1"), TextBox).Text
                    Dim txtOficioReferencia2 As String = CType(dataItem.FindControl("txtOficioReferencia2"), TextBox).Text
                    Dim txtFechaReferencia1 As String = CType(dataItem.FindControl("txtFechaReferencia1"), TextBox).Text
                    Dim txtFechaReferencia2 As String = CType(dataItem.FindControl("txtFechaReferencia2"), TextBox).Text

                    Dim idAfore As Integer = CType(HttpUtility.HtmlDecode(dataItem.Cells.Item(0).Text), Integer)

                    'NHM INI - Validar si aplica el numero conceuctivo o no                  
                    'I_OFICIO_CONSECUTIVO = ConsultarMaximoConsecutivo(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, Con, tran)
                    Dim updt_B_APLICA_NUM_CONSEC As Boolean = False
                    updt_B_APLICA_NUM_CONSEC = get_B_APLICA_NUM_CONSEC(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, Con, tran)
                    I_OFICIO_CONSECUTIVO = AplicaNumeroConsecutivo(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, Con, tran)
                    'NHM FIN

                    objOficio.NumeroOficio = ArmarNumeroOficio(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, "OFICIO EXTERNO", I_OFICIO_CONSECUTIVO, ID_ANIO, "")

                    Dim sql As String = String.Empty
                    sql =
                            "SELECT " +
                            " p.ID_PERSONA " +
                            " ,p.T_PREFIJO " +
                            " ,p.T_NOMBRE " +
                            " ,p.T_APELLIDO_P " +
                            " ,p.T_APELLIDO_M " +
                            " FROM BDA_PERSONAL p " +
                            " INNER JOIN BDA_R_PERSONAL_FUNCION pf ON pf.ID_PERSONA = p.ID_PERSONA " +
                            " INNER JOIN BDA_FUNCION f ON pf.ID_FUNCION = f.ID_FUNCION" +
                            " WHERE p.ID_ENTIDAD = " + idAfore.ToString +
                            " AND p.ID_TIPO_ENTIDAD = " + WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_AFORE").ToString() +
                            " AND f.ID_FUNCION = " + CType(ddlDirigido.SelectedValue, Integer).ToString +
                            " AND p.VIG_FLAG = 1"

                    Dim dtPersona As New DataTable
                    Con.ConsultaAdapter(sql, tran).Fill(dtPersona)

                    Dim idPersona As Integer = 0
                    If dtPersona.Rows.Count > 0 Then idPersona = CInt(dtPersona.Rows(0)("ID_PERSONA"))


                    objOficio.IdAnio = ID_ANIO
                    objOficio.IdArea = ID_UNIDAD_ADM
                    objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO
                    objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
                    objOficio.UsuarioAlta = USUARIO

                    'objOficio.IdTipoEntidad = 3 'ConsultarIdPorNombre("AFORE", Con, tran)
                    objOficio.IdTipoEntidad = Convert.ToInt32(WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_AFORE"))
                    objOficio.IdSubentidad = 0
                    objOficio.IdDestinatario = idPersona
                    objOficio.IdClasificacion = CType(ddlClasificacion.SelectedValue, Integer)
                    objOficio.IdEntidad = idAfore
                    objOficio.IdEstatus = CInt(ddlEstatus.SelectedValue)
                    objOficio.IdPrioridad = CInt(ddlPrioridad.SelectedValue)
                    objOficio.FechaOficio = CType(txtFechaDocumento.Text, DateTime)
                    objOficio.Asunto = txtAsunto.Text.ToString().Trim.Replace("'", "''")
                    objOficio.Comentario = txtComentarios.Text.ToString().Trim.Replace("'", "''")
                    '-------------------------------------------
                    ' Carga el Usuario que elaboró
                    '-------------------------------------------
                    objOficio.UsuarioElaboro = ddlUsuarioElaboro.SelectedValue

                    objOficio.IdIncumplimiento = CType(IIf(ddlIncumplimiento.SelectedIndex > 0, CType(ddlIncumplimiento.SelectedValue, Integer), 0), Integer)
                    objOficio.Dictaminado = 0
                    objOficio.IdFundReserva = 12
                    objOficio.PeriodoReserva = 12
                    objOficio.IdPuestoDestinatario = CType(ddlDirigido.SelectedValue, Integer)
                    objOficio.Plazo = CType(IIf(chkSeDaPlazo.Checked, 1, 0), Integer)

                    If Not txtPlazo.Text.Trim = "" Then
                        objOficio.PlazoDias = CType(txtPlazo.Text, Integer)
                    Else
                        objOficio.PlazoDias = 0
                    End If
                    If Not txtFechaRecepcion.Text.Trim = String.Empty Then
                        objOficio.FechaRecepcion = CType(txtFechaRecepcion.Text, DateTime)
                    End If
                    If Not txtFechaVencimiento.Text.Trim = String.Empty Then
                        objOficio.FechaVencimineto = CType(txtFechaVencimiento.Text, DateTime)
                    End If
                    If Not txtFechaAcuse.Text.Trim = String.Empty Then
                        objOficio.FechaAcuse = CType(txtFechaAcuse.Text, DateTime)
                    End If
                    objOficio.NotifElectronica = 0
                    objOficio.IsFile = 0

                    'resultado = BusinessRules.BDA_OFICIO.InsertarDatosBasicos(objOficio)
                    '-------------------------------------------------
                    ' Formatear Fechas
                    '-------------------------------------------------

                    Dim fechaAcuseValidacion As String = String.Empty
                    If objOficio.FechaAcuse.ToString("yyyyMMdd") = "00010101" Then
                        fechaAcuseValidacion = "NULL"
                    Else
                        fechaAcuseValidacion = "'" & objOficio.FechaAcuse.ToString("yyyyMMdd") & "'"
                    End If

                    Dim fechaRecepcionValidacion As String = String.Empty
                    If objOficio.FechaRecepcion.ToString("yyyyMMdd") = "00010101" Then
                        fechaRecepcionValidacion = "NULL"
                    Else
                        fechaRecepcionValidacion = "'" & objOficio.FechaRecepcion.ToString("yyyyMMdd") & "'"
                    End If

                    Dim fechaVencimientoValidacion As String = String.Empty
                    If objOficio.FechaVencimineto.ToString("yyyyMMdd") = "00010101" Then
                        fechaVencimientoValidacion = "NULL"
                    Else
                        fechaVencimientoValidacion = "'" & objOficio.FechaVencimineto.ToString("yyyyMMdd") & "'"
                    End If

                    Campos =
                                "ID_ANIO, ID_AREA_OFICIO, I_OFICIO_CONSECUTIVO," & _
                                "ID_TIPO_DOCUMENTO, T_OFICIO_NUMERO, USUARIO_ALTA," & _
                                "ID_ENTIDAD_TIPO, ID_ENTIDAD, ID_SUBENTIDAD, ID_DESTINATARIO," & _
                                "ID_CLASIFICACION, ID_ESTATUS, ID_PRIORIDAD, F_FECHA_OFICIO," & _
                                "F_FECHA_ALTA, T_ASUNTO, T_COMENTARIO, USUARIO_ELABORO," & _
                                "ID_INCUMPLIMIENTO, DICTAMINADO_FLAG, ID_FUNDRESERVA," & _
                                "I_PERIODO_RESERVA, ID_PUESTO_DESTINATARIO, PLAZO_FLAG, I_PLAZO_DIAS," & _
                                "F_FECHA_RECEPCION, F_FECHA_VENCIMIENTO, F_FECHA_ACUSE, NOTIF_ELECTRONICA_FLAG, IS_FILE_FLAG, FIRMA_SIE_FLAG"

                    Valores =
                                objOficio.IdAnio.ToString & "," & _
                                objOficio.IdArea.ToString & "," & _
                                objOficio.IOficioConsecutivo.ToString & "," & _
                                objOficio.IdTipoDocumento.ToString & ",'" & _
                                objOficio.NumeroOficio.ToString & "','" & _
                                objOficio.UsuarioAlta & "'," & _
                                objOficio.IdTipoEntidad.ToString & "," & _
                                objOficio.IdEntidad.ToString & "," & _
                                objOficio.IdSubentidad.ToString & "," & _
                                objOficio.IdDestinatario.ToString & "," & _
                                objOficio.IdClasificacion.ToString & "," & _
                                objOficio.IdEstatus.ToString & "," & _
                                objOficio.IdPrioridad.ToString & ",'" & _
                                objOficio.FechaOficio.ToString("yyyyMMdd") & "'," & _
                                "GETDATE(),'" & _
                                objOficio.Asunto & "','" & _
                                objOficio.Comentario & "','" & _
                                objOficio.UsuarioElaboro & "'," & _
                                objOficio.IdIncumplimiento.ToString & "," & _
                                objOficio.Dictaminado.ToString & "," & _
                                objOficio.IdFundReserva.ToString & "," & _
                                objOficio.PeriodoReserva.ToString & "," & _
                                objOficio.IdPuestoDestinatario.ToString & "," & _
                                objOficio.Plazo.ToString & "," & _
                                objOficio.PlazoDias.ToString & "," & _
                                fechaRecepcionValidacion & "," & _
                                fechaVencimientoValidacion & "," & _
                                fechaAcuseValidacion & "," & _
                                objOficio.NotifElectronica.ToString & "," & _
                                objOficio.IsFile.ToString & "," & _
                                rblFirmaSIE.SelectedValue

                    continua = Con.Insertar(Conexion.Owner & "BDA_OFICIO", Campos, Valores, tran)


                    If continua Then

                        'NHM INI - Actualizar consecutivo 
                        If updt_B_APLICA_NUM_CONSEC = True Then
                            Dim valUpd As String = " B_APLICA_NUM_CONSEC = 0 "
                            Dim condUpd As String = " ID_UNIDAD_ADM = " + ID_UNIDAD_ADM.ToString() + " and ID_ANIO = " + ID_ANIO.ToString() + " and ID_TIPO_DOCUMENTO  = " + ID_TIPO_DOCUMENTO.ToString()
                            continua = Con.Actualizar(Conexion.Owner & "BDA_C_CONSECUTIVO_OFICIOS", valUpd, condUpd, tran)
                            If Not continua Then Throw New ApplicationException("Error creando Oficio")
                        End If
                        'NHM FIN

                        'GUARDAMOS EN BITACORA
                        Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                            ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                        Valores = objOficio.IdArea.ToString & "," & _
                            objOficio.IdTipoDocumento.ToString & "," & _
                            objOficio.IdAnio.ToString & "," & _
                            objOficio.IOficioConsecutivo.ToString & ",'" & _
                            USUARIO & "','" & USUARIO & "','" & USUARIO & "',GETDATE(),4,NULL," & fechaVencimientoValidacion

                        continua = Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran)

                    End If







                    CadenaAtencion &= String.Format("{0}|{1}|{2}|{3}*", _
                                                    ID_UNIDAD_ADM.ToString(), _
                                                    ID_TIPO_DOCUMENTO.ToString(), _
                                                    ID_ANIO.ToString(), _
                                                    I_OFICIO_CONSECUTIVO.ToString())


                    If continua Then

                        continua = GuardarFirmas(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, Con, tran)

                        If continua Then

                            Dim resultadoCopia As Boolean = False
                            Dim objCopia As LogicaNegocioSICOD.Entities.BDA_COPIA
                            Dim drow As DataRow = dtNumeroOficio.NewRow
                            drow("NumeroOficio") = objOficio.NumeroOficio
                            drow("OficioReferencia1") = txtOficioReferencia1
                            drow("OficioReferencia2") = txtOficioReferencia2
                            drow("FechaReferencia1") = txtFechaReferencia1
                            drow("FechaReferencia2") = txtFechaReferencia2
                            dtNumeroOficio.Rows.Add(drow)

                            Campos = "ID_PERSONA, ID_ANIO, I_OFICIO_CONSECUTIVO, ID_TIPO_DOCUMENTO, ID_AREA_OFICIO, T_NOMBRE_LEGACY"

                            For Each item As System.Web.UI.WebControls.ListItem In lstConCopia.Items
                                Dim dtCopia As DataTable = ConsultarPersonalPorFuncion(idAfore, CType(item.Value, Integer), Con, tran, CInt(WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_AFORE").ToString()))
                                Dim _Puesto As String = item.Text.Trim

                                Dim idPersonaCC As String = "NULL"
                                If dtCopia.Rows.Count > 0 Then idPersonaCC = dtCopia.Rows(0)("ID_PERSONA").ToString()

                                objCopia = New LogicaNegocioSICOD.Entities.BDA_COPIA
                                'objCopia.ID_PERSONA = idPersonaCC
                                objCopia.ID_ANIO = CType(ddlAño.SelectedValue, Integer)
                                objCopia.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO
                                objCopia.ID_DOCUMENTO_TIPO = ID_TIPO_DOCUMENTO
                                objCopia.ID_AREA_OFICIO = ID_UNIDAD_ADM
                                objCopia.T_NOMBRE_LEGACY = _Puesto

                                Valores =
                                            idPersonaCC + "," +
                                            objCopia.ID_ANIO.ToString + "," +
                                            objCopia.I_OFICIO_CONSECUTIVO.ToString + "," +
                                            objCopia.ID_DOCUMENTO_TIPO.ToString + "," +
                                            objCopia.ID_AREA_OFICIO.ToString + ", '" +
                                            objCopia.T_NOMBRE_LEGACY + "'"

                                continua = Con.Insertar(Conexion.Owner & "BDA_COPIA", Campos, Valores, tran)

                                If Not continua Then Throw New ApplicationException("Error guardando copia para afore " + HttpUtility.HtmlDecode(dataItem.Cells(1).Text))
                                ''resultadoCopia = LogicaNegocioSICOD.BusinessRules.BDA_COPIA.Insertar(objCopia)


                                'If dtCopia.Rows.Count > 0 Then
                                '    Dim idPersonaCC As Integer = CType(dtCopia.Rows(0)("ID_PERSONA"), Integer)
                                '    objCopia = New LogicaNegocioSICOD.Entities.BDA_COPIA
                                '    objCopia.ID_PERSONA = idPersonaCC
                                '    objCopia.ID_ANIO = CType(ddlAño.SelectedValue, Integer)
                                '    objCopia.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO
                                '    objCopia.ID_DOCUMENTO_TIPO = ID_TIPO_DOCUMENTO
                                '    objCopia.ID_AREA_OFICIO = ID_UNIDAD_ADM

                                '    Valores =
                                '                objCopia.ID_PERSONA.ToString + "," +
                                '                objCopia.ID_ANIO.ToString + "," +
                                '                objCopia.I_OFICIO_CONSECUTIVO.ToString + "," +
                                '                objCopia.ID_DOCUMENTO_TIPO.ToString + "," +
                                '                objCopia.ID_AREA_OFICIO.ToString

                                '    continua = Con.Insertar(Conexion.Owner & "BDA_COPIA", Campos, Valores, tran)

                                '    If Not continua Then Throw New ApplicationException("Error guardando copia para afore " + dataItem.Cells(1).Text)
                                '    'resultadoCopia = LogicaNegocioSICOD.BusinessRules.BDA_COPIA.Insertar(objCopia)

                                'Else
                                '    Throw New ApplicationException("No existe una persona asociada al cargo selccionado para Con Copia, para la AFORE " & dataItem.Cells.Item(1).Text)
                                'End If
                            Next

                            If continua Then
                                '--------------------------------------
                                ' Agregarse a BDA_R_OFICIOS.
                                ' Obtener máx ID_Expediente.
                                '--------------------------------------
                                Dim obj_R_OFICIOS As New Entities.BDA_R_OFICIOS
                                obj_R_OFICIOS.ID_EXPEDIENTE = ConsultarMaximoConsecutivoExpediente(Con, tran)

                                obj_R_OFICIOS.ID_ANIO = ID_ANIO
                                obj_R_OFICIOS.ID_AREA_OFICIO = ID_UNIDAD_ADM
                                obj_R_OFICIOS.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
                                obj_R_OFICIOS.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO
                                obj_R_OFICIOS.INICIAL_FLAG = 1
                                obj_R_OFICIOS.USUARIO_ASOCIO = USUARIO

                                Campos =
                                       "ID_ANIO, ID_AREA_OFICIO, I_OFICIO_CONSECUTIVO," & _
                                       "ID_TIPO_DOCUMENTO, ID_EXPEDIENTE, USUARIO_ASOCIO," & _
                                       "FECH_ASOCIO, INICIAL_FLAG"

                                Valores =
                                       obj_R_OFICIOS.ID_ANIO.ToString & "," & _
                                       obj_R_OFICIOS.ID_AREA_OFICIO.ToString & "," & _
                                       obj_R_OFICIOS.I_OFICIO_CONSECUTIVO.ToString & "," & _
                                       obj_R_OFICIOS.ID_TIPO_DOCUMENTO.ToString & "," & _
                                       obj_R_OFICIOS.ID_EXPEDIENTE.ToString & ",'" & _
                                       obj_R_OFICIOS.USUARIO_ASOCIO & "'," & _
                                       "GETDATE()," & _
                                       obj_R_OFICIOS.INICIAL_FLAG.ToString

                                continua = Con.Insertar(Conexion.Owner & "BDA_R_OFICIOS", Campos, Valores, tran)
                            End If

                        Else
                            Throw New ApplicationException("Error guardando firmas para la AFORE " & HttpUtility.HtmlDecode(dataItem.Cells.Item(1).Text))
                        End If
                    Else
                        Throw New ApplicationException("Error guardando para la Afore " & HttpUtility.HtmlDecode(dataItem.Cells.Item(1).Text))
                    End If
                    'Else
                    '    Throw New ApplicationException("No existe una persona asociada al cargo seleccionado, para la AFORE " & dataItem.Cells.Item(1).Text)
                    'End If
                End If
            Next

            If hasCheckedElements Then

                Session("OficiosReferencia") = dtNumeroOficio
                tran.Commit()
                Sesion.BitacoraFinalizaTransaccion(True)
                Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
                Session("ID_ANIO") = ID_ANIO
                Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
                Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
                isMultiplesOficios = True
                isMultiplesOficiosConArchivos = False

                If ISATENCION Then

                    Session(BusinessRules.BDA_OFICIO.SessionAtencionResult) = CadenaAtencion

                Else

                    modalMensaje("Registros guardados exitosamente.", "GuardarMultiplesOK", "INFORMACIÓN", False, "Aceptar")

                End If


            Else
                modalMensaje("No se seleccionaron AFOREs.")

            End If


        Catch ex As ApplicationException
            'modalMensaje(ex.Message)
            continua = False
            Throw New ApplicationException(ex.Message)
        Catch ex As Exception
            continua = False
            _PnombreFuncion = "Guardar Oficio Múltiples Afores"
            'EscribirError(ex, "Guardar Oficio Múltiples Afores")
            Throw New Exception(ex.Message)
        Finally
            If Not continua AndAlso tran IsNot Nothing Then
                tran.Rollback()
                Sesion.BitacoraFinalizaTransaccion(False)
            End If
            If Not Con Is Nothing Then
                Con.Cerrar()
                Con = Nothing
            End If
            If Not tran Is Nothing Then tran = Nothing

        End Try
        Return continua
    End Function

    Private Sub LlenarGridNumerosOficios()

        Dim dtNumeroOficio As DataTable = CType(Session("OficiosReferencia"), DataTable)
        If dtNumeroOficio.Rows.Count > 0 Then

            pnlNumerosOficiosMultiplesAfores.Visible = True
            Dim oficios As String = String.Empty
            For Each dr As DataRow In dtNumeroOficio.Rows
                oficios &= "'" & dr("NumeroOficio").ToString() & "',"
            Next
            oficios = oficios.Substring(0, oficios.Length - 1)
            Dim dtOficios As DataTable = BusinessRules.BDA_OFICIO.ConsultarDatosFormatoOficioMultiplePorNumeros(oficios)
            Dim encrip As New YourCompany.Utils.Encryption.Encryption64

            Dim biblioteca As String = encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")
            Dim nombreArchivo As String = String.Empty

            Dim con1 As New OracleConexion
            Dim dtAfores As DataTable = con1.Datos("SELECT CVE_ID_ENT, DESC_ENT, SIGLAS_ENT FROM osiris.BDV_C_ENTIDAD WHERE ID_T_ENT = " & _
                                                   WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_AFORE").ToString() & " AND VIG_FLAG = 1").Tables(0)
            If Not con1 Is Nothing Then
                con1.Cerrar()
            End If

            For Each dr As DataRow In dtOficios.Rows

                Dim drE As DataRow = dtAfores.Select("CVE_ID_ENT=" & dr("ID_ENTIDAD").ToString())(0)
                dr("T_ENTIDAD_LARGO") = drE("DESC_ENT").ToString()
                dr("T_ENTIDAD_CORTO") = drE("SIGLAS_ENT").ToString()

                nombreArchivo = dr("T_HYP_ARCHIVOWORD").ToString()
                If nombreArchivo.Contains(biblioteca) Then
                    nombreArchivo = nombreArchivo.Substring(nombreArchivo.IndexOf(biblioteca), nombreArchivo.Length - nombreArchivo.IndexOf(biblioteca))
                End If
                If nombreArchivo.Contains("/") Then
                    nombreArchivo = nombreArchivo.Substring(nombreArchivo.IndexOf("/"), nombreArchivo.Length - nombreArchivo.IndexOf("/"))
                    nombreArchivo = nombreArchivo.Substring(1, nombreArchivo.Length - 1)
                End If
                dr("T_HYP_ARCHIVOWORD") = nombreArchivo
                dr.AcceptChanges()
            Next
            dtNumeroOficio.AcceptChanges()
            gvNumerosOficios.DataSource = dtOficios
            gvNumerosOficios.DataBind()

            Session("OficiosReferencia") = Nothing
        End If
    End Sub

    Private Function GuardarFirmas(ByVal pAnio As Integer, ByVal pIdArea As Integer, ByVal pIdTipoDocumento As Integer, ByVal pConsecutivo As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Boolean
        Dim continua As Boolean = True
        Dim Campos As String = String.Empty
        Dim Valores As String = String.Empty
        Dim Condicion As String = String.Empty

        Dim objFirma As LogicaNegocioSICOD.Entities.BDA_FIRMA

        Campos = "USUARIO, ID_ANIO, ID_AREA_OFICIO, I_OFICIO_CONSECUTIVO, ID_TIPO_DOCUMENTO, RUBRICA_FLAG"

        For Each item As System.Web.UI.WebControls.ListItem In lstFirmas.Items
            objFirma = New LogicaNegocioSICOD.Entities.BDA_FIRMA()

            Dim resultadoFirma As Boolean = False
            Dim dtUsuarioFirmas As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetAllPorUsuario(item.Value.ToString())
            objFirma.Usuario = item.Value.ToString()

            objFirma.IdAnio = pAnio
            objFirma.IdAreaOficio = pIdArea
            objFirma.IOficioConsecutivo = pConsecutivo
            objFirma.IdTipoDocumento = pIdTipoDocumento
            objFirma.EsRubrica = 0

            Valores =
                        "'" & objFirma.Usuario & "'," & _
                        objFirma.IdAnio & "," & _
                        objFirma.IdAreaOficio & "," & _
                        objFirma.IOficioConsecutivo.ToString & "," & _
                        objFirma.IdTipoDocumento.ToString & "," & _
                        objFirma.EsRubrica.ToString

            If Not con.Insertar(Conexion.Owner & "BDA_FIRMA", Campos, Valores, tran) Then
                continua = False
                Exit For
            End If
            'resultadoFirma = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.Insertar(objFirma)

        Next

        If continua Then
            For Each itemR As System.Web.UI.WebControls.ListItem In lstRubricas.Items

                Dim resultadoRubrica As Boolean = False
                Dim dtUsuarioRubricas As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetAllPorUsuario(itemR.Value.ToString())
                objFirma = New LogicaNegocioSICOD.Entities.BDA_FIRMA()

                objFirma.Usuario = itemR.Value.ToString()

                objFirma.IdAnio = pAnio
                objFirma.IdAreaOficio = pIdArea
                objFirma.IOficioConsecutivo = pConsecutivo
                objFirma.IdTipoDocumento = pIdTipoDocumento
                objFirma.EsRubrica = 1

                Valores =
                           "'" & objFirma.Usuario & "'," & _
                           objFirma.IdAnio & "," & _
                           objFirma.IdAreaOficio & "," & _
                           objFirma.IOficioConsecutivo.ToString & "," & _
                           objFirma.IdTipoDocumento.ToString & "," & _
                           objFirma.EsRubrica.ToString

                If Not con.Insertar(Conexion.Owner & "BDA_FIRMA", Campos, Valores, tran) Then
                    continua = False
                    Exit For
                End If


            Next
        End If
        Return continua
    End Function

    Private Sub GenerarDocumentosOficios()

        Dim numeroOficio As String = String.Empty
        Dim directorGeneral As String = String.Empty
        Dim encrip As New YourCompany.Utils.Encryption.Encryption64
        Dim ruta As String = String.Empty
        Dim rutaCopia As String = String.Empty
        Dim oficios As String = String.Empty
        Dim spath As String = String.Empty
        Dim fl As String = String.Empty
        Dim fileName As String = String.Empty

        Dim firma As String = ""

        Try

            '------------------------------------------
            ' Define objeto WordprocessingDocument para cargar el archivo Word
            '------------------------------------------
            Dim oDoc As WordprocessingDocument

            Dim pDtOficios As DataTable
            pDtOficios = New DataTable()
            pDtOficios = CType(Session("OficiosReferencia"), DataTable)

            If pDtOficios.Rows.Count > 0 Then

                Dim count As Integer = 0

                '---------------------------------------------
                ' lista de numeros de oficios
                '---------------------------------------------              
                For Each dr As DataRow In pDtOficios.Rows
                    If BusinessRules.BDA_OFICIO.ConsultarExisteOficioMultiplePorNumero(dr("NumeroOficio").ToString()) Then
                        oficios &= "'" & dr("NumeroOficio").ToString() & "',"
                    Else
                        Throw New ApplicationException("Error con oficio, datos para crear archivo word faltantes")
                    End If
                Next
                oficios = oficios.Substring(0, oficios.Length - 1)
                '------------------------------------------
                ' Obtén los oficios creados para combinar con el doc adjunto.
                '------------------------------------------
                Dim dtOficios As DataTable = BusinessRules.BDA_OFICIO.ConsultarDatosFormatoOficioMultiplePorNumeros(oficios)
                Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO

                If dtOficios.Rows.Count > 0 Then

                    '------------------------------------------
                    ' Copia el archivo adjuntado a la ruta temporal
                    '------------------------------------------
                    Dim randomClass As Random = New Random()
                    rutaCopia = Path.GetTempPath.ToString() & Format(randomClass.Next(1000), "0000") & "__" & Format(CODIGO_AREA, "000").ToString() & ".docx"
                    Try
                        If File.Exists(rutaCopia) Then File.Delete(rutaCopia)
                    Catch ex As Exception
                        '------------------------------------------
                        ' Excepción vacía.
                        '------------------------------------------
                    End Try

                    'If fileUp.HasFile Then fileUp.SaveAs(rutaCopia)
                    'fileUp.Dispose()

                    IO.File.Move(FILEMERGEPATH, rutaCopia)

                    Threading.Thread.Sleep(500)

                    For Each row As DataRow In dtOficios.Rows

                        firma = ""
                        Dim dtTagFirma As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.ConsultarDatosFirmaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                        If dtTagFirma.Rows.Count > 0 Then
                            firma = dtTagFirma.Rows(0)("NOMBRE").ToString().Trim
                        End If

                        'NHM INI
                        Dim tipoDoc As String = "WRD_"

                        Select Case ID_TIPO_DOCUMENTO

                            Case OficioTipo.Oficio_Externo
                                tipoDoc = tipoDoc & "EX_"
                            Case OficioTipo.Oficio_Interno
                                tipoDoc = tipoDoc & "IN_"
                            Case OficioTipo.Atenta_Nota
                                tipoDoc = tipoDoc & "AN_"
                            Case OficioTipo.Dictamen
                                tipoDoc = tipoDoc & "DI_"
                        End Select


                        'fileName =
                        '            "WRD_EX_" & _
                        '            Format(CODIGO_AREA, "000").ToString() & "_" & _
                        '            Format(row("I_OFICIO_CONSECUTIVO"), "0000").ToString() & "_" & _
                        '            ID_ANIO.ToString & _
                        '            ".docx"


                        fileName =
                                   tipoDoc & _
                                   Format(CODIGO_AREA, "000").ToString() & "_" & _
                                   Format(row("I_OFICIO_CONSECUTIVO"), "0000").ToString() & "_" & _
                                   ID_ANIO.ToString & _
                                   ".docx"

                        'NHM FIN

                        '------------------------------------------
                        ' Obtén la ruta temporal a la cual vamos a copiar el archivo adjuntado.
                        '------------------------------------------
                        ruta = Path.GetTempPath.ToString() & fileName

                        Try
                            If File.Exists(ruta) Then File.Delete(ruta)
                        Catch ex As Exception
                            '------------------------------------------
                            ' Excepción vacía.
                            '------------------------------------------
                        End Try

                        '---------------------------------------------
                        ' Copiar archivo
                        '---------------------------------------------
                        File.Copy(rutaCopia, ruta, True)

                        '------------------------------------------
                        ' Abre el archivo guardado en la ruta temporal como objeto tipo WordprocessingDocument
                        '------------------------------------------
                        'oDoc = WordprocessingDocument.Open(ruta, True)

                        '------------------------------------------
                        ' Reemplaza los campos variables del documento con aquellos del oficio presente en el loop
                        '------------------------------------------
                        Dim values As New Dictionary(Of String, String)
                        'NHM INI
                        'values("#frase#") = """" & BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS"))).ToString & """"
                        Dim frase2Lineas As String
                        frase2Lineas = BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS"))).ToString
                        frase2Lineas = frase2Lineas + "<br/>" + BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS2"))).ToString
                        values("#frase#") = frase2Lineas
                        'NHM FIN
                        values("#dtm_fecha_oficio_1#") = CType(row("F_FECHA_OFICIO"), DateTime).ToString("dd \de MMMM \de yyyy")
                        values("#dtm_fecha_oficio_2#") = CType(row("F_FECHA_OFICIO"), DateTime).ToString("dd \de MMMM \de yyyy")
                        values("#id_Oficio_Numero#") = row("T_OFICIO_NUMERO").ToString()
                        values("#txt_Asunto#") = row("T_ASUNTO").ToString()

                        values("#firma#") = firma

                        '------------------------------------------
                        ' Creates a TextInfo based on the "en-US" culture.
                        '------------------------------------------
                        Dim myTI As TextInfo = New CultureInfo("es-MX", False).TextInfo
                        'NHM INI
                        'values("#nombre_Area#") = myTI.ToTitleCase(row("DSC_UNIDAD_ADM").ToString().ToLower)
                        values("#nombre_Area#") = row("ALIAS_UNIDAD_ADM").ToString()
                        'NHM FIN

                        Dim areasJerarquia As String = String.Empty
                        Dim dtAreasJerarquia As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaBottomUp(CInt(row("ID_UNIDAD_ADM")))
                        For Each TableRow As DataRow In dtAreasJerarquia.Rows
                            areasJerarquia += TableRow("DSC_UNIDAD_ADM").ToString & vbCrLf
                        Next

                        values("#txt_jerarq#") = areasJerarquia

                        values("#txt_A#") = row("NOMBRE").ToString()
                        values("#txt_Directorio_Puesto#") = row("T_FUNCION").ToString()


                        '' *****************************************************************
                        '' Obtenemos datos DE ENTIDAD de Osiris
                        'values("#txt_Entidad_Largo#") = row("T_ENTIDAD_LARGO").ToString()
                        values("#txt_Entidad_Largo#") = " "
                        Dim con1 As New OracleConexion
                        Try

                            Dim dt As DataTable = con1.Datos("SELECT DESC_ENT FROM osiris.BDV_C_ENTIDAD WHERE ID_T_ENT = " & _
                                                             row("ID_ENTIDAD_TIPO").ToString() & " AND CVE_ID_ENT = " & _
                                                             row("ID_ENTIDAD").ToString()).Tables(0)

                            If dt.Rows.Count > 0 Then
                                values("#txt_Entidad_Largo#") = dt.Rows(0)("DESC_ENT").ToString().Trim
                            End If

                        Catch ex As Exception

                        Finally
                            If Not con1 Is Nothing Then
                                con1.Cerrar()
                            End If

                        End Try


                        values("#txt_Entidad_Direccion_1#") = " "
                        values("#txt_Entidad_Direccion_2#") = " "
                        values("#txt_Entidad_CP#") = " "
                        values("#txt_Entidad_Ciudad#") = " "
                        values("#txt_Entidad_Estado#") = " "

                        Dim con As New Conexion(Conexion.BD.SICOD)
                        Try
                            'Dim sql As String = " SELECT A.T_DIRECCION,A.T_COLONIA,A.T_CP,C.T_POBLACION,ED.T_ESTADO " & _
                            '" FROM BDA_DIRECCION_ENTIDAD A INNER JOIN BDA_ENTIDAD B ON A.ID_ENTIDAD = B.ID_ENTIDAD " & _
                            '" LEFT OUTER JOIN BDA_POBLACION C ON A.ID_POBLACION = C.ID_POBLACION " & _
                            '" LEFT OUTER JOIN BDA_ESTADO ED ON A.ID_ESTADO = ED.ID_ESTADO " & _
                            '" WHERE A.DOMICILIO_NOTIFICACIONES_FLAG = 1 AND A.VIG_FLAG = 1 " & _
                            '" AND B.ID_T_ENT = " & row("ID_ENTIDAD_TIPO").ToString() & " AND B.CVE_ID_ENT = " & row("ID_ENTIDAD").ToString()
                            Dim sql As String = " SELECT A.T_DIRECCION,A.T_COLONIA,A.T_CP,C.T_POBLACION,ED.T_ESTADO " & _
                            " FROM BDA_DIRECCION_ENTIDAD A " & _
                            " LEFT OUTER JOIN BDA_POBLACION C ON A.ID_POBLACION = C.ID_POBLACION " & _
                            " LEFT OUTER JOIN BDA_ESTADO ED ON A.ID_ESTADO = ED.ID_ESTADO " & _
                            " WHERE A.DOMICILIO_NOTIFICACIONES_FLAG = 1 AND A.VIG_FLAG = 1 " & _
                            " AND A.ID_T_ENT = " & row("ID_ENTIDAD_TIPO").ToString() & " AND A.CVE_ID_ENT = " & row("ID_ENTIDAD").ToString()
                            Dim dt As DataTable = con.Datos(sql, False).Tables(0)

                            If dt.Rows.Count > 0 Then

                                If Not String.IsNullOrEmpty(Trim(dt.Rows(0)("T_DIRECCION").ToString())) Then
                                    values("#txt_Entidad_Direccion_1#") = dt.Rows(0)("T_DIRECCION").ToString()
                                End If

                                values("#txt_Entidad_Direccion_2#") = dt.Rows(0)("T_COLONIA").ToString()
                                If String.IsNullOrEmpty(values("#txt_Entidad_Direccion_2#")) Then
                                    values("#txt_Entidad_Direccion_2#") = " "
                                End If

                                values("#txt_Entidad_CP#") = dt.Rows(0)("T_CP").ToString()

                                values("#txt_Entidad_Ciudad#") = dt.Rows(0)("T_POBLACION").ToString()
                                values("#txt_Entidad_Estado#") = dt.Rows(0)("T_ESTADO").ToString()

                            End If



                            'values("#txt_Entidad_Direccion_1#") = " "
                            'If Not String.IsNullOrEmpty(Trim(row("T_DIRECCION").ToString())) Then
                            '    values("#txt_Entidad_Direccion_1#") = row("T_DIRECCION").ToString()
                            'End If

                            'values("#txt_Entidad_Direccion_2#") = row("T_COLONIA").ToString()
                            'If String.IsNullOrEmpty(values("#txt_Entidad_Direccion_2#")) Then
                            '    values("#txt_Entidad_Direccion_2#") = " "
                            'End If

                            'values("#txt_Entidad_CP#") = row("T_CP").ToString()

                            'values("#txt_Entidad_Ciudad#") = row("T_POBLACION").ToString()
                            'values("#txt_Entidad_Estado#") = row("T_ESTADO").ToString()



                        Catch ex As Exception

                        Finally

                            If Not con Is Nothing Then
                                con.Cerrar()
                            End If

                        End Try




                        '' ******************************************************************


                        values("#dtm_Ref_3#") = " "
                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("FechaReferencia1").ToString()) Then
                            values("#dtm_Ref_3#") = pDtOficios.Rows(count)("FechaReferencia1").ToString()
                        End If

                        values("#txt_RefOficio_1#") = " "

                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("OficioReferencia1").ToString()) Then
                            values("#txt_RefOficio_1#") = pDtOficios.Rows(count)("OficioReferencia1").ToString()
                        End If

                        values("#dtm_Ref_4#") = " "
                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("FechaReferencia2").ToString()) Then
                            values("#dtm_Ref_4#") = pDtOficios.Rows(count)("FechaReferencia2").ToString()
                        End If

                        values("#txt_RefOficio_2#") = " "
                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("OficioReferencia2").ToString()) Then
                            values("#txt_RefOficio_2#") = pDtOficios.Rows(count)("OficioReferencia2").ToString()
                        End If

                        values("#txt_Copia_1#") = " "
                        values("#txt_Directorio_Puesto_1#") = " "
                        values("#txt_Copia_2#") = " "
                        values("#txt_Directorio_Puesto_2#") = " "

                        Dim dtCopia As DataTable = BusinessRules.BDA_COPIA.ConsultarDatosCopiaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, CInt(row("I_OFICIO_CONSECUTIVO")))
                        If dtCopia.Rows.Count > 0 Then
                            values("#txt_Copia_1#") = dtCopia.Rows(0)("NOMBRE").ToString() & " .-"
                            values("#txt_Directorio_Puesto_1#") = dtCopia.Rows(0)("PUESTO").ToString()
                            values("#txt_Entidad_Largo_F1#") = row("T_ENTIDAD_LARGO").ToString()
                            values("#para_conocimiento_1#") = " para su conocimiento."
                        Else
                            values("#txt_Copia_1#") = " "
                            values("#txt_Directorio_Puesto_1#") = " "
                            values("#txt_Entidad_Largo_F1#") = " "
                            values("#para_conocimiento_1#") = " "
                        End If

                        If dtCopia.Rows.Count > 1 Then
                            values("#txt_Copia_2#") = dtCopia.Rows(1)("NOMBRE").ToString() & " .-"
                            values("#txt_Directorio_Puesto_2#") = dtCopia.Rows(1)("PUESTO").ToString()
                            values("#txt_Entidad_Largo_F2#") = row("T_ENTIDAD_LARGO").ToString()
                            values("#para_conocimiento_2#") = " para su conocimiento."
                        Else
                            values("#txt_Copia_2#") = " "
                            values("#txt_Directorio_Puesto_2#") = " "
                            values("#txt_Entidad_Largo_F2#") = " "
                            values("#para_conocimiento_2#") = " "
                        End If

                        '------------------------------------------
                        ' Maneja al Director General del área asociada al documento.
                        '------------------------------------------
                        directorGeneral = " "
                        Dim dtDirectorGeneral As DataTable = BusinessRules.BDS_USUARIO.ConsultarDirectorGeneralPorArea(ID_UNIDAD_ADM)
                        If dtDirectorGeneral.Rows.Count > 0 Then
                            directorGeneral = dtDirectorGeneral.Rows(0)("T_PREFIJO").ToString & " " & dtDirectorGeneral.Rows(0)("NOMBRE").ToString()
                        End If
                        If Not String.IsNullOrEmpty(directorGeneral) Then
                            values("#director_general#") = directorGeneral
                        End If

                        '------------------------------------------
                        ' Obten iniciales del usuario que elaboró.
                        '------------------------------------------
                        Dim dtUsuarioElaboro As DataTable = BusinessRules.BDS_USUARIO.GetOne(row("USUARIO_ELABORO").ToString)
                        values("#firmados#") = dtUsuarioElaboro(0)("T_INICIALES").ToString

                        '------------------------------------------
                        ' Maneja las rubricas asociadas al documento.
                        '------------------------------------------
                        Dim rubricas As String = String.Empty
                        Dim dtRubricas As DataTable = BusinessRules.BDA_FIRMA.ConsultarRubricasPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, CInt(row("I_OFICIO_CONSECUTIVO")))
                        For Each rubrica As DataRow In dtRubricas.Rows
                            'NHM INI - cambia orden de acuerdo al nivel del usuario
                            'rubricas &= rubrica("T_INICIALES").ToString + "/"
                            rubricas = rubricas + rubrica("T_INICIALES").ToString + "/"
                            'NHM FIN
                        Next
                        If Not String.IsNullOrEmpty(rubricas) Then rubricas = rubricas.Substring(0, rubricas.LastIndexOf("/"))
                        'NHM INI - cambia orden de acuerdo al nivel, en teoria el que lo elabora tiene el nivel mas bajo
                        'values("#firmados#") &= "/" & rubricas.ToUpper
                        values("#firmados#") = rubricas.ToUpper & "/" & values("#firmados#")
                        'NHM FIN

                        'https://docx.codeplex.com/downloads/get/330961
                        Dim dc As Novacode.DocX
                        dc = Novacode.DocX.Load(ruta)

                        For Each pair As KeyValuePair(Of String, String) In values
                            dc.ReplaceText(pair.Key, pair.Value)
                        Next



                        dc.Save()

                        '------------------------------------------
                        ' Subir archivo a Sharepoint
                        '------------------------------------------
                        subirArchivo(fileName)

                        count = count + 1

                        Try
                            '------------------------------------------
                            ' Borra el archivo almacenado en la ruta temporal
                            '------------------------------------------
                            File.Delete(ruta)

                        Catch ex As Exception
                            '------------------------------------------------------
                            ' Excepción vacía (para no interrumpir aplicación)
                            '------------------------------------------------------
                        End Try

                        '------------------------------------------
                        ' Asociar el archivo al oficio
                        '------------------------------------------
                        objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

                        Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(CType(ddlAño.SelectedValue, Integer), _
                                                                                                              CType(ddlArea.SelectedValue, Integer), _
                                                                                                              CType(row("ID_TIPO_DOCUMENTO"), Integer), _
                                                                                                              CType(row("I_OFICIO_CONSECUTIVO"), Integer))

                        Dim fechaVencimientoValidacion As String = "NULL"
                        If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                            fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                        End If

                        objOficio.ArchivoWord = fileName
                        objOficio.IdAnio = CType(ddlAño.SelectedValue, Integer)
                        objOficio.IdArea = CType(ddlArea.SelectedValue, Integer)
                        objOficio.IdTipoDocumento = CType(row("ID_TIPO_DOCUMENTO"), Integer)
                        objOficio.IOficioConsecutivo = CType(row("I_OFICIO_CONSECUTIVO"), Integer)

                        objOficio.UsuarioElaboro = USUARIO
                        objOficio.Comentario = fechaVencimientoValidacion

                        LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoWord(objOficio)


                    Next

                    File.Delete(rutaCopia)


                Else
                    Throw New ApplicationException("Error al generar archivo word de los oficios")
                End If
            End If
        Catch ex As ApplicationException
            'modalMensaje(ex.Message)
            Throw New ApplicationException(ex.Message)
        Catch ex As Exception

            '_PnombreFuncion = "Generar Oficios para multiples Afores"
            _PnombreFuncion = "GenerarDocumentosOficios"
            'EscribirError(ex, "Generar Oficios para multiples Afores")
            Throw New Exception(ex.Message)
        End Try
    End Sub



    Private Sub GenerarDocumentosOficios_NO_DEFINIDO()

        Dim numeroOficio As String = String.Empty
        Dim directorGeneral As String = String.Empty
        Dim encrip As New YourCompany.Utils.Encryption.Encryption64
        Dim ruta As String = String.Empty
        Dim rutaCopia As String = String.Empty
        Dim oficios As String = String.Empty
        Dim spath As String = String.Empty
        Dim fl As String = String.Empty
        Dim fileName As String = String.Empty

        Dim firma As String = ""

        Try

            '------------------------------------------
            ' Define objeto WordprocessingDocument para cargar el archivo Word
            '------------------------------------------
            Dim oDoc As WordprocessingDocument

            Dim pDtOficios As DataTable
            pDtOficios = New DataTable()
            pDtOficios = CType(Session("OficiosReferencia"), DataTable)

            If pDtOficios.Rows.Count > 0 Then

                Dim count As Integer = 0

                'NHM INI - se quita esta funcionalidad para  registros que aun no tienen definido un número de oficio sicod
                ''---------------------------------------------
                '' lista de numeros de oficios
                ''---------------------------------------------              
                'For Each dr As DataRow In pDtOficios.Rows
                '    If BusinessRules.BDA_OFICIO.ConsultarExisteOficioMultiplePorNumero_NO_DEFINIDO(1, 1, 1, 1) Then
                '        oficios &= "'" & dr("NumeroOficio").ToString() & "',"
                '    Else
                '        Throw New ApplicationException("Error con oficio, datos para crear archivo word faltantes")
                '    End If
                'Next
                'oficios = oficios.Substring(0, oficios.Length - 1)
                'NHM FIN

                '------------------------------------------
                ' Obtén los oficios creados para combinar con el doc adjunto.
                '------------------------------------------
                'NHM  INI
                'Dim dtOficios As DataTable = BusinessRules.BDA_OFICIO.ConsultarDatosFormatoOficioMultiplePorNumeros(oficios)
                Dim dtOficios As DataTable = BusinessRules.BDA_OFICIO.ConsultarDatosFormatoOficioMultiplePorNumeros_NO_DEFINIDO(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ID_ANIO, I_OFICIO_CONSECUTIVO)
                'NHM FIN
                Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO

                If dtOficios.Rows.Count > 0 Then

                    '------------------------------------------
                    ' Copia el archivo adjuntado a la ruta temporal
                    '------------------------------------------
                    Dim randomClass As Random = New Random()
                    rutaCopia = Path.GetTempPath.ToString() & Format(randomClass.Next(1000), "0000") & "__" & Format(CODIGO_AREA, "000").ToString() & ".docx"
                    Try
                        If File.Exists(rutaCopia) Then File.Delete(rutaCopia)
                    Catch ex As Exception
                        '------------------------------------------
                        ' Excepción vacía.
                        '------------------------------------------
                    End Try

                    'If fileUp.HasFile Then fileUp.SaveAs(rutaCopia)
                    'fileUp.Dispose()

                    IO.File.Move(FILEMERGEPATH, rutaCopia)

                    Threading.Thread.Sleep(500)

                    For Each row As DataRow In dtOficios.Rows

                        firma = ""
                        Dim dtTagFirma As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.ConsultarDatosFirmaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                        If dtTagFirma.Rows.Count > 0 Then
                            firma = dtTagFirma.Rows(0)("NOMBRE").ToString().Trim
                        End If

                        'NHM INI
                        Dim tipoDoc As String = "WRD_"

                        Select Case ID_TIPO_DOCUMENTO

                            Case OficioTipo.Oficio_Externo
                                tipoDoc = tipoDoc & "EX_"
                            Case OficioTipo.Oficio_Interno
                                tipoDoc = tipoDoc & "IN_"
                            Case OficioTipo.Atenta_Nota
                                tipoDoc = tipoDoc & "AN_"
                            Case OficioTipo.Dictamen
                                tipoDoc = tipoDoc & "DI_"
                        End Select

                        'fileName =
                        '            "WRD_EX_" & _
                        '            Format(CODIGO_AREA, "000").ToString() & "_" & _
                        '            Format(row("I_OFICIO_CONSECUTIVO"), "0000").ToString() & "_" & _
                        '            ID_ANIO.ToString & _
                        '            ".docx"

                        fileName =
                                   tipoDoc & _
                                   Format(CODIGO_AREA, "000").ToString() & "_" & _
                                   Format(row("I_OFICIO_CONSECUTIVO"), "0000").ToString() & "_" & _
                                   ID_ANIO.ToString & _
                                   ".docx"

                        'NHM FIN

                        '------------------------------------------
                        ' Obtén la ruta temporal a la cual vamos a copiar el archivo adjuntado.
                        '------------------------------------------
                        ruta = Path.GetTempPath.ToString() & fileName

                        Try
                            If File.Exists(ruta) Then File.Delete(ruta)
                        Catch ex As Exception
                            '------------------------------------------
                            ' Excepción vacía.
                            '------------------------------------------
                        End Try

                        '---------------------------------------------
                        ' Copiar archivo
                        '---------------------------------------------
                        File.Copy(rutaCopia, ruta, True)

                        '------------------------------------------
                        ' Abre el archivo guardado en la ruta temporal como objeto tipo WordprocessingDocument
                        '------------------------------------------
                        'oDoc = WordprocessingDocument.Open(ruta, True)

                        '------------------------------------------
                        ' Reemplaza los campos variables del documento con aquellos del oficio presente en el loop
                        '------------------------------------------
                        Dim values As New Dictionary(Of String, String)
                        'NHM INI
                        'values("#frase#") = """" & BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS"))).ToString & """"
                        Dim frase2Lineas As String
                        frase2Lineas = BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS"))).ToString
                        frase2Lineas = frase2Lineas + "<br/>" + BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS2"))).ToString
                        values("#frase#") = frase2Lineas
                        'NHM FIN
                        values("#dtm_fecha_oficio_1#") = CType(row("F_FECHA_OFICIO"), DateTime).ToString("dd \de MMMM \de yyyy")
                        values("#dtm_fecha_oficio_2#") = CType(row("F_FECHA_OFICIO"), DateTime).ToString("dd \de MMMM \de yyyy")
                        values("#id_Oficio_Numero#") = row("T_OFICIO_NUMERO").ToString()
                        values("#txt_Asunto#") = row("T_ASUNTO").ToString()

                        values("#firma#") = firma

                        '------------------------------------------
                        ' Creates a TextInfo based on the "en-US" culture.
                        '------------------------------------------
                        Dim myTI As TextInfo = New CultureInfo("es-MX", False).TextInfo
                        'NHM INI
                        'values("#nombre_Area#") = myTI.ToTitleCase(row("DSC_UNIDAD_ADM").ToString().ToLower)
                        values("#nombre_Area#") = row("ALIAS_UNIDAD_ADM").ToString()
                        'NHM FIN

                        Dim areasJerarquia As String = String.Empty
                        Dim dtAreasJerarquia As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaBottomUp(CInt(row("ID_UNIDAD_ADM")))
                        For Each TableRow As DataRow In dtAreasJerarquia.Rows
                            areasJerarquia += TableRow("DSC_UNIDAD_ADM").ToString & vbCrLf
                        Next

                        values("#txt_jerarq#") = areasJerarquia

                        values("#txt_A#") = row("NOMBRE").ToString()
                        values("#txt_Directorio_Puesto#") = row("T_FUNCION").ToString()


                        '' *****************************************************************
                        '' Obtenemos datos DE ENTIDAD de Osiris
                        'values("#txt_Entidad_Largo#") = row("T_ENTIDAD_LARGO").ToString()
                        values("#txt_Entidad_Largo#") = " "
                        Dim con1 As New OracleConexion
                        Try

                            Dim dt As DataTable = con1.Datos("SELECT DESC_ENT FROM osiris.BDV_C_ENTIDAD WHERE ID_T_ENT = " & _
                                                             row("ID_ENTIDAD_TIPO").ToString() & " AND CVE_ID_ENT = " & _
                                                             row("ID_ENTIDAD").ToString()).Tables(0)

                            If dt.Rows.Count > 0 Then
                                values("#txt_Entidad_Largo#") = dt.Rows(0)("DESC_ENT").ToString().Trim
                            End If

                        Catch ex As Exception

                        Finally
                            If Not con1 Is Nothing Then
                                con1.Cerrar()
                            End If

                        End Try


                        values("#txt_Entidad_Direccion_1#") = " "
                        values("#txt_Entidad_Direccion_2#") = " "
                        values("#txt_Entidad_CP#") = " "
                        values("#txt_Entidad_Ciudad#") = " "
                        values("#txt_Entidad_Estado#") = " "

                        Dim con As New Conexion(Conexion.BD.SICOD)
                        Try
                            'Dim sql As String = " SELECT A.T_DIRECCION,A.T_COLONIA,A.T_CP,C.T_POBLACION,ED.T_ESTADO " & _
                            '" FROM BDA_DIRECCION_ENTIDAD A INNER JOIN BDA_ENTIDAD B ON A.ID_ENTIDAD = B.ID_ENTIDAD " & _
                            '" LEFT OUTER JOIN BDA_POBLACION C ON A.ID_POBLACION = C.ID_POBLACION " & _
                            '" LEFT OUTER JOIN BDA_ESTADO ED ON A.ID_ESTADO = ED.ID_ESTADO " & _
                            '" WHERE A.DOMICILIO_NOTIFICACIONES_FLAG = 1 AND A.VIG_FLAG = 1 " & _
                            '" AND B.ID_T_ENT = " & row("ID_ENTIDAD_TIPO").ToString() & " AND B.CVE_ID_ENT = " & row("ID_ENTIDAD").ToString()
                            Dim sql As String = " SELECT A.T_DIRECCION,A.T_COLONIA,A.T_CP,C.T_POBLACION,ED.T_ESTADO " & _
                            " FROM BDA_DIRECCION_ENTIDAD A " & _
                            " LEFT OUTER JOIN BDA_POBLACION C ON A.ID_POBLACION = C.ID_POBLACION " & _
                            " LEFT OUTER JOIN BDA_ESTADO ED ON A.ID_ESTADO = ED.ID_ESTADO " & _
                            " WHERE A.DOMICILIO_NOTIFICACIONES_FLAG = 1 AND A.VIG_FLAG = 1 " & _
                            " AND A.ID_T_ENT = " & row("ID_ENTIDAD_TIPO").ToString() & " AND A.CVE_ID_ENT = " & row("ID_ENTIDAD").ToString()
                            Dim dt As DataTable = con.Datos(sql, False).Tables(0)

                            If dt.Rows.Count > 0 Then

                                If Not String.IsNullOrEmpty(Trim(dt.Rows(0)("T_DIRECCION").ToString())) Then
                                    values("#txt_Entidad_Direccion_1#") = dt.Rows(0)("T_DIRECCION").ToString()
                                End If

                                values("#txt_Entidad_Direccion_2#") = dt.Rows(0)("T_COLONIA").ToString()
                                If String.IsNullOrEmpty(values("#txt_Entidad_Direccion_2#")) Then
                                    values("#txt_Entidad_Direccion_2#") = " "
                                End If

                                values("#txt_Entidad_CP#") = dt.Rows(0)("T_CP").ToString()

                                values("#txt_Entidad_Ciudad#") = dt.Rows(0)("T_POBLACION").ToString()
                                values("#txt_Entidad_Estado#") = dt.Rows(0)("T_ESTADO").ToString()

                            End If



                            'values("#txt_Entidad_Direccion_1#") = " "
                            'If Not String.IsNullOrEmpty(Trim(row("T_DIRECCION").ToString())) Then
                            '    values("#txt_Entidad_Direccion_1#") = row("T_DIRECCION").ToString()
                            'End If

                            'values("#txt_Entidad_Direccion_2#") = row("T_COLONIA").ToString()
                            'If String.IsNullOrEmpty(values("#txt_Entidad_Direccion_2#")) Then
                            '    values("#txt_Entidad_Direccion_2#") = " "
                            'End If

                            'values("#txt_Entidad_CP#") = row("T_CP").ToString()

                            'values("#txt_Entidad_Ciudad#") = row("T_POBLACION").ToString()
                            'values("#txt_Entidad_Estado#") = row("T_ESTADO").ToString()



                        Catch ex As Exception

                        Finally

                            If Not con Is Nothing Then
                                con.Cerrar()
                            End If

                        End Try




                        '' ******************************************************************


                        values("#dtm_Ref_3#") = " "
                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("FechaReferencia1").ToString()) Then
                            values("#dtm_Ref_3#") = pDtOficios.Rows(count)("FechaReferencia1").ToString()
                        End If

                        values("#txt_RefOficio_1#") = " "

                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("OficioReferencia1").ToString()) Then
                            values("#txt_RefOficio_1#") = pDtOficios.Rows(count)("OficioReferencia1").ToString()
                        End If

                        values("#dtm_Ref_4#") = " "
                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("FechaReferencia2").ToString()) Then
                            values("#dtm_Ref_4#") = pDtOficios.Rows(count)("FechaReferencia2").ToString()
                        End If

                        values("#txt_RefOficio_2#") = " "
                        If Not String.IsNullOrEmpty(pDtOficios.Rows(count)("OficioReferencia2").ToString()) Then
                            values("#txt_RefOficio_2#") = pDtOficios.Rows(count)("OficioReferencia2").ToString()
                        End If

                        values("#txt_Copia_1#") = " "
                        values("#txt_Directorio_Puesto_1#") = " "
                        values("#txt_Copia_2#") = " "
                        values("#txt_Directorio_Puesto_2#") = " "

                        Dim dtCopia As DataTable = BusinessRules.BDA_COPIA.ConsultarDatosCopiaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, CInt(row("I_OFICIO_CONSECUTIVO")))
                        If dtCopia.Rows.Count > 0 Then
                            values("#txt_Copia_1#") = dtCopia.Rows(0)("NOMBRE").ToString() & " .-"
                            values("#txt_Directorio_Puesto_1#") = dtCopia.Rows(0)("PUESTO").ToString()
                            values("#txt_Entidad_Largo_F1#") = row("T_ENTIDAD_LARGO").ToString()
                            values("#para_conocimiento_1#") = " para su conocimiento."
                        Else
                            values("#txt_Copia_1#") = " "
                            values("#txt_Directorio_Puesto_1#") = " "
                            values("#txt_Entidad_Largo_F1#") = " "
                            values("#para_conocimiento_1#") = " "
                        End If

                        If dtCopia.Rows.Count > 1 Then
                            values("#txt_Copia_2#") = dtCopia.Rows(1)("NOMBRE").ToString() & " .-"
                            values("#txt_Directorio_Puesto_2#") = dtCopia.Rows(1)("PUESTO").ToString()
                            values("#txt_Entidad_Largo_F2#") = row("T_ENTIDAD_LARGO").ToString()
                            values("#para_conocimiento_2#") = " para su conocimiento."
                        Else
                            values("#txt_Copia_2#") = " "
                            values("#txt_Directorio_Puesto_2#") = " "
                            values("#txt_Entidad_Largo_F2#") = " "
                            values("#para_conocimiento_2#") = " "
                        End If

                        '------------------------------------------
                        ' Maneja al Director General del área asociada al documento.
                        '------------------------------------------
                        directorGeneral = " "
                        Dim dtDirectorGeneral As DataTable = BusinessRules.BDS_USUARIO.ConsultarDirectorGeneralPorArea(ID_UNIDAD_ADM)
                        If dtDirectorGeneral.Rows.Count > 0 Then
                            directorGeneral = dtDirectorGeneral.Rows(0)("T_PREFIJO").ToString & " " & dtDirectorGeneral.Rows(0)("NOMBRE").ToString()
                        End If
                        If Not String.IsNullOrEmpty(directorGeneral) Then
                            values("#director_general#") = directorGeneral
                        End If

                        '------------------------------------------
                        ' Obten iniciales del usuario que elaboró.
                        '------------------------------------------
                        Dim dtUsuarioElaboro As DataTable = BusinessRules.BDS_USUARIO.GetOne(row("USUARIO_ELABORO").ToString)
                        values("#firmados#") = dtUsuarioElaboro(0)("T_INICIALES").ToString

                        '------------------------------------------
                        ' Maneja las rubricas asociadas al documento.
                        '------------------------------------------
                        Dim rubricas As String = String.Empty
                        Dim dtRubricas As DataTable = BusinessRules.BDA_FIRMA.ConsultarRubricasPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, CInt(row("I_OFICIO_CONSECUTIVO")))
                        For Each rubrica As DataRow In dtRubricas.Rows
                            'NHM INI - cambia orden de acuerdo al nivel del usuario
                            'rubricas &= rubrica("T_INICIALES").ToString + "/"
                            rubricas = rubricas + rubrica("T_INICIALES").ToString + "/"
                            'NHM FIN
                        Next
                        If Not String.IsNullOrEmpty(rubricas) Then rubricas = rubricas.Substring(0, rubricas.LastIndexOf("/"))
                        'NHM INI - cambia orden de acuerdo al nivel, en teoria el que lo elabora tiene el nivel mas bajo
                        'values("#firmados#") &= "/" & rubricas.ToUpper
                        values("#firmados#") = rubricas.ToUpper & "/" & values("#firmados#")
                        'NHM FIN

                        'https://docx.codeplex.com/downloads/get/330961
                        Dim dc As Novacode.DocX
                        dc = Novacode.DocX.Load(ruta)

                        For Each pair As KeyValuePair(Of String, String) In values
                            dc.ReplaceText(pair.Key, pair.Value)
                        Next



                        dc.Save()

                        '------------------------------------------
                        ' Subir archivo a Sharepoint
                        '------------------------------------------
                        subirArchivo(fileName)

                        count = count + 1

                        Try
                            '------------------------------------------
                            ' Borra el archivo almacenado en la ruta temporal
                            '------------------------------------------
                            File.Delete(ruta)

                        Catch ex As Exception
                            '------------------------------------------------------
                            ' Excepción vacía (para no interrumpir aplicación)
                            '------------------------------------------------------
                        End Try

                        '------------------------------------------
                        ' Asociar el archivo al oficio
                        '------------------------------------------
                        objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

                        Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(CType(ddlAño.SelectedValue, Integer), _
                                                                                                              CType(ddlArea.SelectedValue, Integer), _
                                                                                                              CType(row("ID_TIPO_DOCUMENTO"), Integer), _
                                                                                                              CType(row("I_OFICIO_CONSECUTIVO"), Integer))

                        Dim fechaVencimientoValidacion As String = "NULL"
                        If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                            fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                        End If

                        objOficio.ArchivoWord = fileName
                        objOficio.IdAnio = CType(ddlAño.SelectedValue, Integer)
                        objOficio.IdArea = CType(ddlArea.SelectedValue, Integer)
                        objOficio.IdTipoDocumento = CType(row("ID_TIPO_DOCUMENTO"), Integer)
                        objOficio.IOficioConsecutivo = CType(row("I_OFICIO_CONSECUTIVO"), Integer)

                        objOficio.UsuarioElaboro = USUARIO
                        objOficio.Comentario = fechaVencimientoValidacion

                        LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoWord(objOficio)


                    Next

                    File.Delete(rutaCopia)


                Else
                    Throw New ApplicationException("Error al generar archivo word de los oficios")
                End If
            End If
        Catch ex As ApplicationException
            'modalMensaje(ex.Message)
            Throw New ApplicationException(ex.Message)
        Catch ex As Exception

            '_PnombreFuncion = "Generar Oficios para multiples Afores"
            _PnombreFuncion = "GenerarDocumentosOficios"
            'EscribirError(ex, "Generar Oficios para multiples Afores")
            Throw New Exception(ex.Message)
        End Try
    End Sub

    'Finds merge fields into the XML document
    Private Function GetFieldName(ByVal pField As DocumentFormat.OpenXml.Wordprocessing.SimpleField) As String
        Dim attr As DocumentFormat.OpenXml.OpenXmlAttribute = pField.GetAttribute("instr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        Dim strFieldname As String = String.Empty
        Dim instruction As String = attr.Value

        Dim instructionRegEx As Regex = _
            New Regex( _
                "^[\s]*MERGEFIELD[\s]+(?<name>[#\w]*){1}" + _
                    "[\s]*(\\\*[\s]+(?<Format>[\w]*){1})?" + _
                    "[\s]*(\\b[\s]+[""]?(?<PreText>[^\\]*){1})?" + _
                    "[\s]*(\\f[\s]+[""]?(?<PostText>[^\\]*){1})?", _
                RegexOptions.Compiled Or _
                RegexOptions.CultureInvariant Or _
                RegexOptions.ExplicitCapture Or _
                RegexOptions.IgnoreCase Or _
                RegexOptions.IgnorePatternWhitespace Or _
                RegexOptions.Singleline)

        If (Not String.IsNullOrEmpty(instruction)) Then
            Dim m As Match = instructionRegEx.Match(instruction)
            If (m.Success) Then
                strFieldname = m.Groups("name").ToString.Trim
            End If
        End If

        Return strFieldname
    End Function

    Private Function subirArchivo(ByVal NombreSharepoint As String) As String

        Dim FilePath As String = String.Empty

        Try

            '--------------------------------------------------------------------------
            ' Conectar con Sharepoint
            '--------------------------------------------------------------------------
            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim objSP As New Clases.nsSharePoint.FuncionesSharePoint

            objSP.ServidorSharePoint = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
            objSP.Biblioteca = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")
            objSP.Usuario = WebConfigurationManager.AppSettings("UsuarioSp").ToString()
            objSP.Password = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
            objSP.Dominio = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
            objSP.RutaArchivo = System.IO.Path.GetTempPath()
            objSP.NombreArchivo = NombreSharepoint
            '--------------------------------------------------------------------------
            ' Carga el archivo (directorio temporal - nombre de archivo
            '--------------------------------------------------------------------------
            If Not objSP.UploadFileToSharePoint() Then Throw New ApplicationException("Error cargando archivo a Sharepoint")

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            NombreSharepoint = String.Empty
        Finally

            If File.Exists(FilePath) Then File.Delete(FilePath)

        End Try
        Return NombreSharepoint
    End Function

    Private Sub CargarOficio()

        'chkMultiplesAfores.Visible = False
        'lblMultiplesAfores.Visible = False
        trMultiplesAfores.Visible = False
        'Dim dtOficio As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
        Dim dtOficio As DataTable = Oficio.GetByKey(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

        '------------------------------------------------------
        ' Carga todos los años
        '------------------------------------------------------
        CargarCombo(ddlAño, LogicaNegocioSICOD.BusinessRules.BDA_ANIO.ConsultarAnio, "CICLO", "CICLO")

        Dim index As Integer = 0
        If dtOficio.Rows.Count > 0 Then

            '-----------------------------------------------
            ' Selecciona el año del oficio
            '-----------------------------------------------
            ddlAño.SelectedValue = ID_ANIO.ToString
            '-----------------------------------------------
            ' Selecciona el área del oficio
            '-----------------------------------------------
            CargarCombo(ddlArea, BusinessRules.BDS_C_AREA.GetOne(ID_UNIDAD_ADM), "DSC_COMPOSITE", "ID_UNIDAD_ADM")
            ddlArea.SelectedValue = ID_UNIDAD_ADM.ToString

            '-------------------------------------------------------
            ' Tipos de documentos/llave
            ' 1 - Oficio Externo
            ' 2 - Dictamen
            ' 3 - Atenta Nota
            ' 4 - Oficio Interno
            '-------------------------------------------------------
            ddlTipoDocumento.SelectedValue = ID_TIPO_DOCUMENTO.ToString
            ddlTipoDocumento_SelectedIndexChanged(Nothing, Nothing)


            ' si es al oficio ya tiene documento word, no se muestra el boton parea generar
            btnGuardarGenerarOficios.Visible = IsDBNull(dtOficio.Rows.Item(0)("T_HYP_ARCHIVOWORD"))
         

            '-------------------------------------------------------

            '-------------------------------------------------------
            ' Consultar número de expediente.
            '-------------------------------------------------------
            ID_EXPEDIENTE = ConsultarNumeroExpediente(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            '-----------------------------------------------
            ' Cargar todos los tipos de clasificación
            '-----------------------------------------------
            'CargarCombo(ddlClasificacion, BusinessRules.BDA_CLASIFICACION_OFICIO.getAll(), "T_CLASIFICACION", "ID_CLASIFICACION")



            If ID_TIPO_DOCUMENTO = OficioTipo.Oficio_Externo Then
                '--------------------------------------------
                ' Verificar si tiene dictámen relacionado
                '--------------------------------------------
                pnlDocRelacionado.Visible = True
                pnlLnkDictamen.Visible = True
                pnlLnkOficioExterno.Visible = False
                Dim dtConsultarDictamen = BusinessRules.BDA_R_OFICIOS.ConsultarDictamenDeOficioExterno(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                If dtConsultarDictamen.Rows.Count > 0 Then
                    Dim dtDictamenDeOficioExterno As DataTable =
                            LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(
                                                                                            CInt(dtConsultarDictamen.Rows(0)("ID_ANIO")),
                                                                                            CInt(dtConsultarDictamen.Rows(0)("ID_AREA_OFICIO")),
                                                                                            CInt(dtConsultarDictamen.Rows(0)("ID_TIPO_DOCUMENTO")),
                                                                                            CInt(dtConsultarDictamen.Rows(0)("I_OFICIO_CONSECUTIVO"))
                                                                                            )
                    If Not IsDBNull(dtDictamenDeOficioExterno.Rows(0)("T_HYP_ARCHIVOSCAN")) OrElse
                            Not dtDictamenDeOficioExterno.Rows(0)("T_HYP_ARCHIVOSCAN").ToString = String.Empty Then
                        lnkDictamenRelacionado.Text = dtDictamenDeOficioExterno.Rows(0)("T_HYP_ARCHIVOSCAN").ToString
                        btnDeleteDictamen.Visible = True
                        ddlIncumplimiento.Enabled = False
                        chkDictaminado.Checked = True

                    Else
                        lnkDictamenRelacionado.Text = ""
                        chkDictaminado.Checked = False
                        chkDictaminado.Enabled = False
                    End If
                Else
                    lnkDictamenRelacionado.Text = ""
                    chkDictaminado.Checked = False
                End If

            ElseIf ID_TIPO_DOCUMENTO = OficioTipo.Dictamen Then
                '--------------------------------------------
                ' Verificar si tiene oficio externo relacionado
                '--------------------------------------------
                pnlDocRelacionado.Visible = True
                pnlLnkOficioExterno.Visible = True
                pnlLnkDictamen.Visible = False
                Dim dtConsultarOficioExterno = BusinessRules.BDA_R_OFICIOS.ConsultarOficioExternoDeDictamen(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                If dtConsultarOficioExterno.Rows.Count > 0 Then
                    Dim dtOficioExternoDeDictamen As DataTable =
                            LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(
                                                                                            CInt(dtConsultarOficioExterno.Rows(0)("ID_ANIO")),
                                                                                            CInt(dtConsultarOficioExterno.Rows(0)("ID_AREA_OFICIO")),
                                                                                            CInt(dtConsultarOficioExterno.Rows(0)("ID_TIPO_DOCUMENTO")),
                                                                                            CInt(dtConsultarOficioExterno.Rows(0)("I_OFICIO_CONSECUTIVO"))
                                                                                            )
                    If Not IsDBNull(dtOficioExternoDeDictamen.Rows(0)("T_HYP_ARCHIVOSCAN")) OrElse
                            dtOficioExternoDeDictamen.Rows(0)("T_HYP_ARCHIVOSCAN").ToString = String.Empty Then
                        lnkOficioExternoRelacionado.Text = dtOficioExternoDeDictamen.Rows(0)("T_HYP_ARCHIVOSCAN").ToString
                        btnDeleteOficioExterno.Visible = True
                        ddlIncumplimiento.Enabled = False
                    Else
                        lnkOficioExternoRelacionado.Text = "Buscar"
                        lnkOficioExternoRelacionado.Enabled = False
                    End If
                Else

                    lnkOficioExternoRelacionado.Text = "Buscar"
                End If
            Else
                pnlLnkDictamen.Visible = False
                pnlLnkOficioExterno.Visible = False
            End If

            '-----------------------------------------------
            ' Obtener número de oficio
            '-----------------------------------------------
            NUMERO_OFICIO = dtOficio.Rows.Item(0)("T_OFICIO_NUMERO").ToString()
            lblNumeroOficio.Text = NUMERO_OFICIO

            'NHM INI - si no ha generado el numero de ofico, entonces lo debe mostrar
            If lblNumeroOficio.Text.Trim = "NO DEFINIDO" Or lblNumeroOficio.Text.Trim = String.Empty Then
                btnGuardarGenerarOficios.Visible = True
            End If
            'NHM FIN

            '-----------------------------------------------
            ' Obtener código de área de la unidad adm
            '-----------------------------------------------

            'If CODIGO_AREA = 0 Then
            Dim dtCodigoArea As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.GetCodigoAreaPorUnidadAdm(ID_UNIDAD_ADM)
            CODIGO_AREA = CInt(dtCodigoArea.Rows(0)("I_CODIGO_AREA"))
            'End If

            If Not IsDBNull(dtOficio.Rows.Item(0)("T_DESCRIPCION_CANCELACION")) Then
                txtCancelacion.Text = HttpUtility.HtmlDecode(dtOficio.Rows.Item(0)("T_DESCRIPCION_CANCELACION").ToString())
            End If


            ''==========================================================================================================
            If Not IsDBNull(dtOficio.Rows.Item(0)("ID_ENTIDAD_TIPO")) Then

                '-----------------------------------------------
                ' Cargar ddl de tipo de entidad
                '-----------------------------------------------
                ddlTipoEntidad.SelectedValue = dtOficio.Rows.Item(0)("ID_ENTIDAD_TIPO").ToString()
                Session("ddlTipoEntidad.SelectedValue") = ddlTipoEntidad.SelectedValue
                ddlEntidad.Enabled = True
                '-----------------------------------------------
                '
                '-----------------------------------------------
                'CargarCombo(ddlEntidad, BusinessRules.BDA_ENTIDAD.ConsultarEntidadesPorTipo(CInt(ddlTipoEntidad.SelectedValue)), "T_ENTIDAD_CORTO", "ID_ENTIDAD")
                'MODIFICADO POR JORGE RANGEL 18 OCT 2012
                'CargarCombo(ddlEntidad, BusinessRules.BDA_ENTIDAD.getOne(CInt(dtOficio.Rows.Item(0)("ID_ENTIDAD"))), "T_ENTIDAD_CORTO", "ID_ENTIDAD")
                ddlTipoEntidad_SelectedIndexChanged(Nothing, Nothing)

                ddlEntidad.SelectedValue = dtOficio.Rows.Item(0)("ID_ENTIDAD").ToString()
                Session("ddlEntidad.SelectedValue") = ddlEntidad.SelectedValue

                '-----------------------------------------------
                ' Cargo del destinatario
                '-----------------------------------------------
                ddlCargoDestinatario.SelectedValue = dtOficio.Rows.Item(0)("ID_PUESTO_DESTINATARIO").ToString()
                '-----------------------------------------------
                ' Destinatario
                '-----------------------------------------------

                ddlSubentidad.Enabled = True
                'MODIFICADO POR JORGE RANGEL 18 OCT 2012
                'Dim _dt As DataTable = BusinessRules.BDA_ENTIDAD.ConsultarSubEntidades(CInt(ddlEntidad.SelectedValue))
                'CargarCombo(ddlSubentidad, _dt, "T_ENTIDAD_CORTO", "ID_ENTIDAD")
                ddlEntidad_SelectedIndexChanged(Nothing, Nothing)

                'rowSubEntidad.Style.Add("display", CStr(IIf(_dt.Rows.Count < 1, "none", "block")))

                If Not IsDBNull(dtOficio.Rows.Item(0)("ID_DESTINATARIO")) AndAlso CInt(dtOficio.Rows.Item(0)("ID_DESTINATARIO")) > 0 Then
                    Dim dtPersona As DataTable = BusinessRules.BDA_PERSONAL.ConsultarPersonalPorIdPersona(CInt(dtOficio.Rows.Item(0)("ID_DESTINATARIO")))

                    destinatarioKey.Value = CInt(dtOficio.Rows.Item(0)("ID_DESTINATARIO")).ToString()


                    txtDestinatario.Text = Trim(
                                                dtPersona.Rows.Item(0)("T_PREFIJO").ToString & " " &
                                                dtPersona.Rows.Item(0)("T_NOMBRE").ToString & " " & _
                                                dtPersona.Rows.Item(0)("T_APELLIDO_P").ToString & " " & _
                                                 dtPersona.Rows.Item(0)("T_APELLIDO_M").ToString)
                    aceDestinatario.ContextKey = dtPersona.Rows.Item(0)("ID_PERSONA").ToString

                    'destinatarioKey.Value = aceDestinatario.ContextKey
                ElseIf Not IsDBNull(dtOficio.Rows.Item(0)("T_DESTINATARIO")) Then

                    txtDestinatario.Text = dtOficio.Rows.Item(0)("T_DESTINATARIO").ToString
                    txtDestinatario.Enabled = False
                    destinatarioKey.Value = "0"
                Else
                    txtDestinatario.Text = ddlCargoDestinatario.SelectedItem.Text
                    destinatarioKey.Value = "0"
                End If
                destinatarioText.Value = txtDestinatario.Text
                '-----------------------------------------------
                '-----------------------------------------------
                ddlSubentidad.SelectedValue = dtOficio.Rows.Item(0)("ID_SUBENTIDAD").ToString()
                '-----------------------------------------------
                '-----------------------------------------------

            End If
            ''==========================================================================================================



            '-----------------------------------------------
            ' Cargar los tipos de clasificación para el área
            '-----------------------------------------------
            ddlArea_SelectedIndexChanged(Nothing, Nothing)

            ddlClasificacion.SelectedValue = dtOficio.Rows.Item(0)("ID_CLASIFICACION").ToString()
            '-----------------------------------------------
            '-----------------------------------------------
            ddlEstatus.SelectedValue = dtOficio.Rows.Item(0)("ID_ESTATUS").ToString()
            OLD_ESTATUS = CInt(ddlEstatus.SelectedValue)
            If OLD_ESTATUS = 6 OrElse OLD_ESTATUS = 7 Then
                btnDeleteDictamen.Visible = False
                btnDeleteOficioExterno.Visible = False
            End If

            If OLD_ESTATUS = OficioEstatus.Cancelado Then
                pnlComentariosCancelacion.Visible = True
            End If

            '-----------------------------------------------
            ' Prioridad
            '-----------------------------------------------
            ddlPrioridad.SelectedValue = dtOficio.Rows.Item(0)("ID_PRIORIDAD").ToString()

            '-----------------------------------------------
            ' Fecha de Oficio
            '-----------------------------------------------
            txtFechaDocumento.Text = CType(dtOficio.Rows.Item(0)("F_FECHA_OFICIO"), DateTime).ToString("dd/MM/yyyy")

            '-----------------------------------------------
            ' Asunto
            '-----------------------------------------------
            txtAsunto.Text = HttpUtility.HtmlDecode(dtOficio.Rows.Item(0)("T_ASUNTO").ToString())

            '-----------------------------------------------
            ' Comentario
            '-----------------------------------------------
            txtComentarios.Text = HttpUtility.HtmlDecode(dtOficio.Rows.Item(0)("T_COMENTARIO").ToString())

            '-----------------------------------------------
            ' OANH - Usuario Elaboró
            '-----------------------------------------------
            Dim usuarioelaboro As String = dtOficio.Rows.Item(0)("USUARIO_ELABORO").ToString()

            If Not IsDBNull(usuarioelaboro) AndAlso Not String.IsNullOrEmpty(usuarioelaboro) Then

                'OANH - LLENAMOS COMBO DE AREAS DEL USUARIO QUE ELABORO.
                Dim dtUnidadUsuario = BusinessRules.BDS_C_AREA.ConsultarUnidadDeUsuario(usuarioelaboro, CInt(rblEstructuraArea.SelectedValue))
                Dim idUnidadAdmUsuarioElaboro = CInt(dtUnidadUsuario(0)("ID_UNIDAD_ADM"))
                Dim topIdUnidadAdmUsuarioElaboro As Integer = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(idUnidadAdmUsuarioElaboro, CInt(rblEstructuraArea.SelectedValue))

                Dim dtAreas As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaUnidadAdm(topIdUnidadAdmUsuarioElaboro, CInt(rblEstructuraElaboro.SelectedValue))
                CargarCombo(ddlAreaElaboro, dtAreas, "DSC_COMPOSITE", "ID_UNIDAD_ADM")

                If ddlAreaElaboro.Items.Count > 0 Then
                    Dim li As ListItem
                    li = ddlAreaElaboro.Items.FindByValue(idUnidadAdmUsuarioElaboro.ToString)
                    If li IsNot Nothing Then
                        ddlAreaElaboro.SelectedValue = idUnidadAdmUsuarioElaboro.ToString
                    End If
                End If

                'OANH - LLENAMOS COMBO CON EL USUARIO QUE ELABORO.
                Dim dtUsuario As DataTable = BusinessRules.BDS_USUARIO.GetOne(usuarioelaboro)
                CargarCombo(ddlUsuarioElaboro, dtUsuario, "NOMBRECOMPLETO", "USUARIO")
                ddlUsuarioElaboro.SelectedValue = usuarioelaboro

            End If

            '-----------------------------------------------
            ' Registró
            '-----------------------------------------------
            If Not IsDBNull(dtOficio.Rows.Item(0)("USUARIO_ALTA")) AndAlso Not String.IsNullOrEmpty(dtOficio.Rows.Item(0)("USUARIO_ALTA").ToString) Then
                Dim dtUsuarioRegistro As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetOne(dtOficio.Rows.Item(0)("USUARIO_ALTA").ToString())
                lblRegistro.Text = dtUsuarioRegistro(0)("NOMBRECOMPLETO").ToString
            End If

            '-----------------------------------------------
            ' Incumplimiento
            '-----------------------------------------------
            ddlIncumplimiento.SelectedValue = dtOficio.Rows.Item(0)("ID_INCUMPLIMIENTO").ToString()


            '-----------------------------------------------
            ' Plazo
            '-----------------------------------------------
            Dim HayPlazo As Boolean = False
            If Not IsDBNull(dtOficio.Rows.Item(0)("PLAZO_FLAG")) Then
                HayPlazo = CBool(dtOficio.Rows.Item(0)("PLAZO_FLAG"))
            End If
            If HayPlazo Then
                chkSeDaPlazo.Checked = True
                pnlFechasPlazo.Visible = True
            Else
                chkSeDaPlazo.Checked = False
                pnlFechasPlazo.Visible = False
            End If


            '-----------------------------------------------
            'Días de plazo
            '-----------------------------------------------
            txtPlazo.Text = dtOficio.Rows.Item(0)("I_PLAZO_DIAS").ToString()
            '-----------------------------------------------
            '-----------------------------------------------
            If Not IsDBNull(dtOficio.Rows.Item(0)("F_FECHA_RECEPCION")) Then
                txtFechaRecepcion.Text = CType(dtOficio.Rows.Item(0)("F_FECHA_RECEPCION"), DateTime).ToString("dd/MM/yyyy")
            End If
            '-----------------------------------------------
            '-----------------------------------------------
            If Not IsDBNull(dtOficio.Rows.Item(0)("F_FECHA_VENCIMIENTO")) Then
                txtFechaVencimiento.Text = CType(dtOficio.Rows.Item(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("dd/MM/yyyy")
            End If
            '-----------------------------------------------
            '-----------------------------------------------
            If Not IsDBNull(dtOficio.Rows.Item(0)("F_FECHA_ACUSE")) Then
                txtFechaAcuse.Text = CType(dtOficio.Rows.Item(0)("F_FECHA_ACUSE"), DateTime).ToString("dd/MM/yyyy")
            End If


            Me.rblFirmaSIE.SelectedValue = CType(dtOficio.Rows.Item(0)("FIRMA_SIE_FLAG"), Integer).ToString()


            '' EL BOTON ENVIAR SE HABILITA SOLO SI NO ES FIRMADO POR SIE, TIENE EL PDF ARRIBA, EL DESTINATARIO ES DE CONSAR Y 
            '' SU ESTATUS LO PERMITE (no enviado, no concluido, no cancelado)
            btnEnviar.Visible = dtOficio.Rows.Item(0)("FIRMA_SIE_FLAG").ToString() = "0" AndAlso _
                Not IsDBNull(dtOficio.Rows.Item(0)("T_HYP_ARCHIVOSCAN")) AndAlso _
                dtOficio.Rows.Item(0)("ID_ENTIDAD").ToString = WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR") AndAlso _
                dtOficio.Rows.Item(0)("ID_ENTIDAD_TIPO").ToString = WebConfigurationManager.AppSettings("ID_TIPO_ENTIDAD_CONSAR") AndAlso _
                dtOficio.Rows.Item(0)("ID_ESTATUS").ToString <> "6" AndAlso _
                dtOficio.Rows.Item(0)("ID_ESTATUS").ToString <> "7" AndAlso _
                dtOficio.Rows.Item(0)("ID_ESTATUS").ToString <> "14"



        Else
            LimpiarControles()
            deshabilitaModificacionOficio()
        End If
        '-----------------------------------------------
        ' Usuarios de firmado
        '-----------------------------------------------
        Dim dtFirma As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.ConsultarDatosFirmaRubricaPorOficio(CType(dtOficio.Rows.Item(0)("ID_ANIO"), Integer), CType(dtOficio.Rows.Item(0)("ID_AREA_OFICIO"), Integer), CType(dtOficio.Rows.Item(0)("ID_TIPO_DOCUMENTO"), Integer), CType(dtOficio.Rows.Item(0)("I_OFICIO_CONSECUTIVO"), Integer))
        For Each dr As DataRow In dtFirma.Rows
            If CType(dr("RUBRICA_FLAG"), Integer) = 0 Then
                Dim item As System.Web.UI.WebControls.ListItem
                item = New System.Web.UI.WebControls.ListItem
                item.Text = dr("NOMBRE").ToString
                item.Value = dr("USUARIO").ToString
                lstFirmas.Items.Add(item)
                lstUsuariosFirma.Items.Remove(item)
            Else
                Dim item As System.Web.UI.WebControls.ListItem
                item = New System.Web.UI.WebControls.ListItem
                item.Text = dr("NOMBRE").ToString
                item.Value = dr("USUARIO").ToString
                lstRubricas.Items.Add(item)
                lstUsuariosRubrica.Items.Remove(item)
            End If
        Next
        VisualizarBotones(True)
        iBtnNuevo.Visible = False
        rblEstructuraArea.Visible = False

    End Sub

    Private Function ControlValido(ByVal ctrl As WebControl) As String

        Try
            If TypeOf ctrl Is TextBox Then
                Dim tb As TextBox = CType(ctrl, TextBox)
                If tb.Text = "" Then

                    Throw New ApplicationException("Asunto no puede ir vacío")
                Else
                    Dim rev As RegularExpressionValidator = CType(Me.FindControl("RegularExpressionValidator" & tb.ID.Replace("txt", "")), RegularExpressionValidator)
                    If Not Regex.IsMatch(tb.Text, rev.ValidationExpression) Then Throw New ApplicationException(rev.ErrorMessage)
                End If
            ElseIf TypeOf ctrl Is DropDownList Then
                Dim ddl As DropDownList = CType(ctrl, DropDownList)
                Dim rfv As RequiredFieldValidator = CType(Me.FindControl("RequiredFieldValidator" & ddl.ID.Replace("ddl", "")), RequiredFieldValidator)
                If ddl.SelectedValue = rfv.InitialValue Then
                    Throw New ApplicationException(rfv.ErrorMessage)
                End If
            ElseIf TypeOf ctrl Is ListBox Then
                Dim lst As ListBox = CType(ctrl, ListBox)
                Dim rfv As RequiredFieldValidator = CType(Me.FindControl("RequiredFieldValidator" & lst.ID.Replace("lst", "")), RequiredFieldValidator)
                If lst.SelectedValue = rfv.InitialValue Then
                    Throw New ApplicationException(rfv.ErrorMessage)
                End If
            Else
                Throw New ApplicationException("Error")
            End If

            Return String.Empty

        Catch ex As ApplicationException
            Return ex.Message
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Sub valida_datos()

        If txtAsunto.Text.Length > 255 Then Throw New ApplicationException(RegularExpressionValidatorAsunto.ErrorMessage)

        If txtComentarios.Text.Length > 255 Then Throw New ApplicationException(RegularExpressionValidatorComentarios.ErrorMessage)

        If ddlArea.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el área")

        If ddlAño.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el año")

        Dim strTemp As String = ControlValido(txtFechaDocumento)
        If Not strTemp Is String.Empty Then Throw New ApplicationException(strTemp)

        If txtFechaDocumento.Text.Trim = "" Then Throw New ApplicationException("Debe seleccionar la fecha de documento")

        strTemp = ControlValido(txtAsunto)
        If Not strTemp Is String.Empty Then Throw New ApplicationException(strTemp)

        If txtAsunto.Text.Trim = "" Then Throw New ApplicationException("Debe capturar el asunto")

        If ddlEstatus.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el estatus")

        If Not String.IsNullOrEmpty(txtFechaAcuse.Text) Then
            If Not DateTime.TryParse(txtFechaAcuse.Text, New Date) Then Throw New ApplicationException("Error en Fecha de Acuse")
        End If

        If Not String.IsNullOrEmpty(txtFechaDocumento.Text) Then
            If Not DateTime.TryParse(txtFechaDocumento.Text, New Date) Then Throw New ApplicationException("Error en Fecha de Documento")
        End If

        If Not String.IsNullOrEmpty(txtFechaRecepcion.Text) Then
            If Not DateTime.TryParse(txtFechaRecepcion.Text, New Date) Then Throw New ApplicationException("Error en Fecha de Recepción")
        End If

        If Not String.IsNullOrEmpty(txtFechaVencimiento.Text) Then
            If Not DateTime.TryParse(txtFechaVencimiento.Text, New Date) Then Throw New ApplicationException("Error en Fecha de Vencimiento")
        End If

        If ddlPrioridad.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar la prioridad")

        If ddlUsuarioElaboro.SelectedValue = "-1" Then Throw New ApplicationException("Debe seleccionar quien elaboró")


        '
        'If lstFirmas.Items.Count = 0 Then Throw New ApplicationException("Debe seleccionar al menos una firma")
        If rblFirmaSIE.SelectedValue = "1" Then

            If lstFirmas.Items.Count = 0 Then Throw New ApplicationException("Debe seleccionar al menos una firma")

        ElseIf rblFirmaSIE.SelectedValue = "0" And CInt(ddlTipoDocumento.SelectedValue) = LogicaNegocioSICOD.OficioTipo.Dictamen Then

            If lstFirmas.Items.Count = 0 Then Throw New ApplicationException("Debe seleccionar al menos una firma")

        End If


        If rblFirmaSIE.SelectedValue = "1" Then

            If lstRubricas.Items.Count = 0 Then Throw New ApplicationException("Debe seleccionar al menos una rúbrica ")

        End If


        If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Dictamen Then
            'NHM INI
            'If ddlIncumplimiento.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el incumplimiento")
            'NHMI FIN
        End If

        If ddlClasificacion.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar clasificación")

        If chkSeDaPlazo.Checked AndAlso (txtPlazo.Text.Length = 0 OrElse Not IsNumeric(txtPlazo.Text)) Then Throw New ApplicationException("Debe seleccionar días de plazo o formato equivocado")

        If chkSeDaPlazo.Checked AndAlso txtFechaVencimiento.Text.Length = 0 Then Throw New ApplicationException("Debe seleccionar fecha de vencimiento")

        If chkMultiplesAfores.Checked Then

            If ddlDirigido.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar a quien va dirigido")

            Dim seleccion As Boolean = False
            For Each dataItem As DataGridItem In dgMultiplesAfores.Items
                Dim chkSeleccion As System.Web.UI.WebControls.CheckBox = CType(dataItem.FindControl("chkSeleccion"), System.Web.UI.WebControls.CheckBox)
                If chkSeleccion.Checked Then
                    seleccion = True
                    Exit For
                End If
            Next

            If Not seleccion Then Throw New ApplicationException("Debe seleccionar al menos un Afore")

        Else
            If ddlTipoEntidad.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el tipo de entidad ")

            If ddlEntidad.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar la entidad")

            If ddlCargoDestinatario.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el cargo del destinatario")

            If txtDestinatario.Text Is String.Empty Then Throw New ApplicationException("Debe seleccionar/escribir el destinatario")

            If ddlTipoDocumento.SelectedIndex <= 0 Then Throw New ApplicationException("Debe seleccionar el tipo de documento")

        End If

    End Sub

    Private Sub EnviarNotificacion(ByVal nUsuario As String)

        '' VER CODIGO EN LA BANDEJA DE ENTRADA

        'Try
        '    '------------------------------------------
        '    '   CONSULTAR OFICIO
        '    '------------------------------------------
        '    Dim dtOficio As DataTable = BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

        '    '------------------------------------------
        '    '   DESTINATARIOS
        '    '------------------------------------------
        '    Dim dtFirmaElectronica As DataTable = BusinessRules.BDA_FIRMA_ELECTRONICA.ConsultarFirmaElectronicaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
        '    If dtFirmaElectronica.Rows.Count = 0 Then Throw New ApplicationException("No existen Usuarios con firma electrónica para el oficio")

        '    '------------------------------------------
        '    'Correo
        '    '------------------------------------------
        '    Dim dsTexto As DataSet = BusinessRules.BDA_CORREO_NOTIFICACION.ConsultarCorreoNotificacion(ID_UNIDAD_ADM)
        '    If dsTexto.Tables(0).Rows.Count = 0 Then Throw New ApplicationException("No existe la configuración de email para el área asignada al oficio.")

        '    '------------------------------------------
        '    ' ASUNTO
        '    '------------------------------------------
        '    Dim asuntoCorreo As String = "Se notifica Oficio No. " & dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString()
        '    asuntoCorreo = asuntoCorreo.Replace("/", "\/")

        '    '------------------------------------------
        '    ' Notificador
        '    '------------------------------------------
        '    Dim dtUsuario As DataTable = BusinessRules.BDS_USUARIO.GetAllPorUsuario(USUARIO)
        '    Dim nombreNotificador As String = dtUsuario(0)("NOMBRE").ToString & " " & dtUsuario(0)("APELLIDOS").ToString

        '    '------------------------------------------
        '    ' TO (Destinatarios y copias)
        '    '------------------------------------------
        '    Dim lstDestinatarios As New List(Of String)

        '    For Each dr As DataRow In dtFirmaElectronica.Rows

        '        If Not IsDBNull(dr("T_EMAIL")) Then
        '            lstDestinatarios.Add(dr("T_EMAIL").ToString.Trim)
        '        End If
        '    Next

        '    '------------------------------------------
        '    ' Cuerpo del correo
        '    '------------------------------------------
        '    Dim cuerpoCorreo As New StringBuilder

        '    For Each drFirma As DataRow In dtFirmaElectronica.Rows
        '        cuerpoCorreo.Append(drFirma("T_NOMBRE").ToString() & " " & drFirma("T_APELLIDO_P").ToString() & " " & drFirma("T_APELLIDO_M").ToString() + "\r")
        '    Next


        '    'cuerpoCorreo.Append("\r" & dtTexto(0)("TEXTO").ToString())
        '    'cuerpoCorreo.Append(" así como en el artículo 134, fracción I del Código Fiscal de la Federación; artículos 114, 115, 116, 118, 119, 120, 121 y 122 del Reglamento de la Ley de los Sistemas de Ahorro para el Retiro, publicado en el Diario Oficial de la Federación el 24 de agosto de 2009; 336, 337 y 339 de las DISPOSICIONES de carácter general en materia de operaciones de los sistemas de ahorro para el retiro se les notifica el oficio " & lblNumeroOficio.Text.Trim & " anexo.")
        '    cuerpoCorreo.Append("\r" & dsTexto.Tables(0)(0)("TEXTO").ToString().Replace("#NUM_OFICIO#", lblNumeroOficio.Text.Trim))
        '    cuerpoCorreo.Append("\r\rAtentamente,\r\r\r\r")
        '    cuerpoCorreo.Append(nombreNotificador + "\r\r\r\r")

        '    Dim lstCC As New List(Of String)
        '    'For Each dr As DataRow In dtTexto.Rows
        '    '    If Not IsDBNull(dr("CORREO")) Then
        '    '        lstCC.Add(dr("CORREO").ToString.Trim)
        '    '    End If
        '    '    If Not IsDBNull(dr("CC")) Then
        '    '        cuerpoCorreo.Append(dr("CC").ToString & "\r\r")
        '    '    End If
        '    'Next
        '    For Each dr As DataRow In dsTexto.Tables(1).Rows
        '        If Not IsDBNull(dr("CC_CORREO")) Then
        '            lstCC.Add(dr("CC_CORREO").ToString.Trim)
        '        End If
        '        If Not String.IsNullOrEmpty(dr("CC_TEXTO").ToString()) Then
        '            cuerpoCorreo.Append(dr("CC_TEXTO").ToString & "\r\r")
        '        End If
        '    Next

        '    Dim strCuerpoCorreo As String = cuerpoCorreo.ToString()
        '    strCuerpoCorreo = strCuerpoCorreo.Replace("{", "")
        '    strCuerpoCorreo = strCuerpoCorreo.Replace("}", "")
        '    strCuerpoCorreo = strCuerpoCorreo.Replace("/", "\/")

        '    '------------------------------------------
        '    ' Documentos Adjuntos
        '    '------------------------------------------
        '    Dim lstAdjuntos As New List(Of String)

        '    Dim ruta As String = String.Empty

        '    Dim nombreArchivoPDF As String = dtOficio.Rows(0)("T_HYP_ARCHIVOSCAN").ToString()
        '    Dim nombreArchivoCedula As String = dtOficio.Rows(0)("T_HYP_CEDULAPDF").ToString()
        '    Dim nombreArchivoCedulaDigital As String = dtOficio.Rows(0)("T_CEDULADIGITAL").ToString()
        '    Dim nombreArchivoFirmaDigital As String = dtOficio.Rows(0)("T_HYP_FIRMADIGITAL").ToString()
        '    Dim nombreAnexoUno As String = String.Empty
        '    Dim nombreAnexoDos As String = String.Empty

        '    '------------------------------------------
        '    ' Obtener nombre de anexo uno si existe
        '    '------------------------------------------
        '    If Not IsDBNull(dtOficio.Rows(0)("T_ANEXO_UNO")) AndAlso Not String.IsNullOrEmpty(dtOficio.Rows(0)("T_ANEXO_UNO").ToString) Then

        '        nombreAnexoUno = dtOficio.Rows(0)("T_ANEXO_UNO").ToString()

        '        If nombreAnexoUno.Contains("@") Then
        '            nombreAnexoUno = nombreAnexoUno.Substring(nombreAnexoUno.IndexOf("@") + 1)
        '        Else
        '            Throw New ApplicationException("Archivo Anexo 1 existe pero no está encriptado, por favor encriptar")
        '        End If

        '    End If

        '    '------------------------------------------
        '    ' Obtener nombre de anexo dos si existe
        '    '------------------------------------------
        '    If Not IsDBNull(dtOficio.Rows(0)("T_ANEXO_DOS")) AndAlso Not String.IsNullOrEmpty(dtOficio.Rows(0)("T_ANEXO_DOS").ToString) Then

        '        nombreAnexoDos = dtOficio.Rows(0)("T_ANEXO_DOS").ToString()

        '        If nombreAnexoDos.Contains("@") Then
        '            nombreAnexoDos = nombreAnexoDos.Substring(nombreAnexoDos.IndexOf("@") + 1)
        '        Else
        '            Throw New ApplicationException("Archivo Anexo 1 existe pero no está encriptado, por favor encriptar")
        '        End If

        '    End If

        '    Dim encrip As New YourCompany.Utils.Encryption.Encryption64
        '    If Not CBool(dtOficio.Rows(0)("IS_FILE_FLAG")) Then

        '        Dim biblioteca As String = encrip.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")

        '        '-------------------------------------------------
        '        ' Descargar Cedula Digital a directorio temporal
        '        ' agregar a lista de adjuntos.
        '        '-------------------------------------------------
        '        If Not BajarArchivo(nombreArchivoCedulaDigital) Then Throw New ApplicationException("Error adjuntando archivo Cédula Digital. Posiblemente no exista.")
        '        ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreArchivoCedulaDigital

        '        lstAdjuntos.Add(ruta)
        '        '-------------------------------------------------
        '        ' Descargar Archivo Firma Digital a directorio temporal
        '        ' agregar a alista de adjuntos
        '        '-------------------------------------------------
        '        ruta = Server.MapPath(Me.Request.ApplicationPath) + AppSettings("TEMP_PATH").ToString.Replace("/", "")
        '        If Not BajarArchivo(nombreArchivoFirmaDigital) Then Throw New ApplicationException("Error adjuntando archivo Firma Digital. Posiblemente no exista.")

        '        ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreArchivoFirmaDigital
        '        lstAdjuntos.Add(ruta)

        '        '-------------------------------------------------
        '        ' Descargar Archivo Anexo 1 a dir temporal
        '        ' agregar a lista de adjuntos
        '        '-------------------------------------------------
        '        If Not nombreAnexoUno = String.Empty Then

        '            ruta = Server.MapPath(Me.Request.ApplicationPath) & AppSettings("TEMP_PATH").ToString.Replace("/", "")
        '            If Not BajarArchivo(nombreAnexoUno) Then Throw New ApplicationException("Error adjuntando archivo Anexo 1. Posiblemente no exista.")
        '            ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreAnexoUno
        '            lstAdjuntos.Add(ruta)
        '        End If

        '        '-------------------------------------------------
        '        ' Descargar Archivo Anexo 2 a dir temporal
        '        ' agregar a lista de adjuntos
        '        '-------------------------------------------------
        '        If Not nombreAnexoDos = String.Empty Then
        '            ruta = Server.MapPath(Me.Request.ApplicationPath) & AppSettings("TEMP_PATH").ToString.Replace("/", "")
        '            If Not BajarArchivo(nombreAnexoDos) Then Throw New ApplicationException("Error adjuntando archivo Anexo 2. Posiblemente no exista.")
        '            ruta = Request.Url.GetLeftPart(UriPartial.Authority) & AppSettings("TEMP_PATH").ToString & nombreAnexoDos
        '            lstAdjuntos.Add(ruta)
        '        End If

        '    Else
        '        If nombreArchivoCedulaDigital.Contains("#") AndAlso nombreArchivoCedulaDigital.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
        '            nombreArchivoCedulaDigital = nombreArchivoCedulaDigital.Substring(0, nombreArchivoCedulaDigital.IndexOf("#"))
        '            lstAdjuntos.Add(nombreArchivoCedulaDigital)
        '        End If

        '        If nombreArchivoFirmaDigital.Contains("#") AndAlso nombreArchivoFirmaDigital.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
        '            nombreArchivoFirmaDigital = nombreArchivoFirmaDigital.Substring(0, nombreArchivoFirmaDigital.IndexOf("#"))
        '            lstAdjuntos.Add(nombreArchivoFirmaDigital)
        '        End If

        '        If Not nombreAnexoUno = String.Empty Then
        '            If nombreAnexoUno.Contains("#") AndAlso nombreAnexoUno.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
        '                nombreAnexoUno = nombreAnexoUno.Substring(0, nombreAnexoUno.IndexOf("#"))
        '                lstAdjuntos.Add(nombreAnexoUno)
        '            End If
        '        End If

        '        If Not nombreAnexoDos = String.Empty Then
        '            If nombreAnexoDos.Contains("#") AndAlso nombreAnexoDos.ToLower.Contains(AppSettings("FILES_PATH").ToLower.ToString) Then
        '                nombreAnexoDos = nombreAnexoDos.Substring(0, nombreAnexoDos.IndexOf("#"))
        '                lstAdjuntos.Add(nombreAnexoDos)
        '            End If
        '        End If

        '    End If

        '    '---------------------------------------
        '    ' Compone la cadena de destinatarios
        '    '---------------------------------------
        '    Dim destinatariosString As String = String.Empty
        '    If lstDestinatarios.Count > 0 Then
        '        For Each item As String In lstDestinatarios
        '            destinatariosString += item + ";"
        '        Next
        '        destinatariosString = destinatariosString.Substring(0, destinatariosString.LastIndexOf(";"))
        '    End If

        '    '---------------------------------------
        '    ' Compone la cadena de destinatarios de copia
        '    '---------------------------------------
        '    Dim destinatariosCopiaString As String = String.Empty
        '    If lstCC.Count > 0 Then
        '        For Each item As String In lstCC
        '            destinatariosCopiaString += item + ";"
        '        Next
        '        destinatariosCopiaString = destinatariosCopiaString.Substring(0, destinatariosCopiaString.LastIndexOf(";"))
        '    End If

        '    Dim openOutlookScript As New StringBuilder
        '    openOutlookScript.Append("var theApp;")
        '    openOutlookScript.Append("var theMailItem;")
        '    openOutlookScript.Append("var subject = '" + asuntoCorreo + "';")
        '    openOutlookScript.Append("var msg = '" + strCuerpoCorreo + "';")
        '    openOutlookScript.Append("var to = '" + destinatariosString + "';")
        '    openOutlookScript.Append("var cc = '" + destinatariosCopiaString + "';")
        '    openOutlookScript.Append("try {")
        '    openOutlookScript.Append("var theApp = new ActiveXObject(""Outlook.Application"");")
        '    openOutlookScript.Append("var theMailItem = theApp.CreateItem(0);")
        '    openOutlookScript.Append("theMailItem.to = to;")
        '    openOutlookScript.Append("theMailItem.Subject = (subject);")
        '    openOutlookScript.Append("theMailItem.Body = (msg);")
        '    openOutlookScript.Append("theMailItem.CC = (cc);")

        '    '---------------------------------------
        '    ' Compone la cadena de adjuntos
        '    '---------------------------------------
        '    Dim adjuntosString As String = String.Empty
        '    For Each item As String In lstAdjuntos
        '        '---------------------------------------
        '        ' Escapa la cadena si usas archivos locales al servidor, (C:\dummy.txt -> C:\\dummy.txt, \\CONSARFILE -> \\\\CONSARFILE)
        '        ' si son archivos de url, no modificar.
        '        '---------------------------------------
        '        If CBool(dtOficio.Rows(0)("IS_FILE_FLAG")) Then item = item.Replace("\", "\\")
        '        openOutlookScript.Append("theMailItem.Attachments.Add(""" + item + """);")
        '    Next

        '    openOutlookScript.Append("theMailItem.display();")
        '    openOutlookScript.Append("} catch (err) { alert(err + ' Intente de nuevo.'); }")

        '    ViewState("TempSCRIPTOUTLOOK") = openOutlookScript.ToString

        '    modalMensaje("Archivos adjuntados, abrir Outlook?", "CorreoNotificacion", "Notificación", True)

        'Catch ex As ApplicationException
        '    modalMensaje(ex.Message, , "Error", False, "Aceptar")
        'Catch ex As Exception
        '    EscribirError(ex, "Enviar Notificación")
        'End Try

    End Sub

    Private Sub VisualizarBotones(ByVal pVisible As Boolean)
        pnlBotonesImagen.Visible = pVisible
        '' *********************************************
        '' COMENTADO POR JORGE RANGEL  16/AGO/2012
        '' *********************************************
        'iBtnEnviarNotificacion.Visible = pVisible
        'iBtnCedula.Visible = pVisible

        iBtnAdjuntarDocumentos.Visible = pVisible
        'iBtnSeguimiento.Visible = pVisible
        iBtnNuevo.Visible = pVisible
    End Sub

#Region "Modal y Postback de modal"
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

        If showCancelButton Then BtnCancelarModal.Style.Add("display", "block") Else BtnCancelarModal.Style.Add("display", "none")
        'BtnCancelarModal.Visible = showCancelButton

        BtnCancelarModal.Text = CancelButtonText
        ModalPopupExtenderErrores.Show()
    End Sub

    Protected Sub BtnModalOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnModalOk.Click
        If lblModalPostBack.Text = "GuardarOK" Then

            If Not IsNothing(Request.QueryString("ie")) Then
                Response.Redirect("~/ExpedienteDetalle.aspx?ie=" & Request.QueryString("ie").ToString, False)
                Return
            End If

            Response.Redirect("~/App_Oficios/Registro.aspx?modificar=1")

        ElseIf lblModalPostBack.Text = "CorreoCambioEstatus" Then

            Dim dtCorreoAviso As DataTable = BusinessRules.BDA_R_ESTATUS_CORREO_AVISO.ConsultarCorreo(NEW_ESTATUS)

            Dim _T_DSC_ENTIDAD_CORTO As String = "" 'BusinessRules.BDA_ENTIDAD.ConsultarEntidadNomCorto(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
            Dim _T_OFICIO_NUMERO As String = BusinessRules.BDA_OFICIO.ObtenerT_NumeroOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
            Dim _T_CORREO_ASUNTO As String = dtCorreoAviso(0)("T_CORREO_ASUNTO").ToString.Replace("#T_OFICIO_NUMERO", _T_OFICIO_NUMERO)
            Dim _T_CORREO_CUERPO As String = dtCorreoAviso(0)("T_CORREO_CUERPO").ToString.Replace("#T_OFICIO_NUMERO", _T_OFICIO_NUMERO)
            Dim _USUARIO_NOMBRE As String = BusinessRules.BDS_USUARIO.GetNombreCompleto(USUARIO)
            Dim _dtElaboroRegistro As DataTable = BusinessRules.BDA_OFICIO.GetUsuariosElaboroRegistro(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
            Dim _dtFirmasRubricas As DataTable = BusinessRules.BDA_FIRMA.ConsultarDatosFirmaRubricaPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
            Dim destinatarios As New StringBuilder

            For Each row As DataRow In _dtElaboroRegistro.Rows
                If Not IsDBNull(row("USUARIO_ELABORO")) Then
                    destinatarios.Append(row("USUARIO_ELABORO").ToString & "@consar.gob.mx;")
                End If
                If Not IsDBNull(row("USUARIO_ALTA")) Then
                    destinatarios.Append(row("USUARIO_ELABORO").ToString & "@consar.gob.mx;")
                End If
            Next

            For Each row As DataRow In _dtFirmasRubricas.Rows
                If Not IsDBNull(row("USUARIO")) Then
                    destinatarios.Append(row("USUARIO").ToString & "@consar.gob.mx;")
                End If
            Next

            _T_CORREO_ASUNTO = _T_CORREO_ASUNTO.Replace("#DSC_ENTIDAD_CORTO", _T_DSC_ENTIDAD_CORTO)
            _T_CORREO_CUERPO = _T_CORREO_CUERPO.Replace("#DSC_ENTIDAD_CORTO", _T_DSC_ENTIDAD_CORTO)

            _T_CORREO_ASUNTO = _T_CORREO_ASUNTO.Replace("#USUARIO_NOMBRE", _USUARIO_NOMBRE)
            _T_CORREO_CUERPO = _T_CORREO_CUERPO.Replace("#USUARIO_NOMBRE", _USUARIO_NOMBRE)

            Dim openOutlookScript As New StringBuilder
            openOutlookScript.Append("var theApp;")
            openOutlookScript.Append("var theMailItem;")
            openOutlookScript.Append("var subject = '" + _T_CORREO_ASUNTO + "';")
            openOutlookScript.Append("var msg = '" + _T_CORREO_CUERPO + "';")
            openOutlookScript.Append("var to = '';")
            openOutlookScript.Append("try {")
            openOutlookScript.Append("var theApp = new ActiveXObject(""Outlook.Application"");")
            openOutlookScript.Append("var theMailItem = theApp.CreateItem(0);")
            openOutlookScript.Append("theMailItem.to = '" + destinatarios.ToString + "';")
            openOutlookScript.Append("theMailItem.Subject = (subject);")
            openOutlookScript.Append("theMailItem.Body = (msg);")
            openOutlookScript.Append("theMailItem.display();")
            openOutlookScript.Append("} catch (err) { alert(err + ' Intente de nuevo.'); }")

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Outlook", openOutlookScript.ToString, True)
            Session.Remove("NEW_ESTATUS")

        ElseIf lblModalPostBack.Text = "CorreoNotificacion" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Outlook", ViewState("TempSCRIPTOUTLOOK").ToString, True)
            '---------------------------------------------
            'actualizar estatus de la notificación
            '---------------------------------------------
            If BusinessRules.BDA_OFICIO.ActualizarNotificacionPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, False) > 0 Then
                modalMensaje("Notificación Generada")
            End If

        ElseIf lblModalPostBack.Text = "GuardarMultiplesOK" Then
            Response.Redirect("~/App_Oficios/Registro.aspx", False)

        ElseIf lblModalPostBack.Text = "RegresaBandeja" Then
            Response.Redirect("~/App_Oficios/Bandeja.aspx", False)

        End If
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
            If Not Perfil.Autorizado("App_Oficios/Registro.aspx") Then
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

        If Not IsNothing(Session("IdOficioSISAN")) Then Return True

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

    Private Sub GenerarCedulaElectronica()

        '' VER CODIGO EN LA BANDEJA

        'Try
        '    Dim dtOficio As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioEntidad(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
        '    Dim dtFirma As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA.ConsultarDatosFirmaCargoPorOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
        '    Dim dtDestinatarios As DataTable

        '    If Not IsDBNull(dtOficio.Rows(0)("ID_ENTIDAD")) Then
        '        dtDestinatarios = LogicaNegocioSICOD.BusinessRules.BDA_FIRMA_ELECTRONICA.ConsultarFirmaElectronicaPorEntidad(CType(dtOficio.Rows(0)("ID_ENTIDAD"), Integer))

        '        If dtDestinatarios.Rows.Count = 0 Then Throw New ApplicationException("No existen Destinatarios vigentes para la entidad")
        '    Else
        '        Throw New ApplicationException("Error en la entidad relacionada el oficio")
        '    End If

        '    Dim ruta As String = String.Empty
        '    Dim hora As String = String.Empty
        '    Dim fecha As String = String.Empty

        '    Dim fileName As String =
        '                                "CNE_EX_" & _
        '                                Format(CODIGO_AREA, "000").ToString + "_" & _
        '                                Format(I_OFICIO_CONSECUTIVO, "0000").ToString() & "_" & _
        '                                ID_ANIO.ToString & _
        '                                ".pdf"



        '    '------------------------------------------
        '    ' Obtén la ruta temporal a la cual vamos a copiar el archivo adjuntado.
        '    '------------------------------------------
        '    Dim randomClass As Random = New Random()
        '    ruta = Path.GetTempPath.ToString() & fileName

        '    hora = String.Format(" {0} horas con {1} minutos ", txtHora.Text.Trim, txtMin.Text.Trim)
        '    fecha = CType(txtFechaCedula.Text, DateTime).ToLongDateString()
        '    Dim strDestinatarios As StringBuilder
        '    strDestinatarios = New StringBuilder

        '    For Each dr As DataRow In dtDestinatarios.Rows
        '        strDestinatarios.Append(String.Format("al C. {0}, con dirección de correo electrónico {1}, ", dr("NOMBRE").ToString(), dr("T_EMAIL").ToString()))
        '    Next

        '    Dim dtUsuarioNotificador As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetOne(ddlNotificador.SelectedValue)
        '    Dim strUsuarioNotificador = String.Empty
        '    If dtUsuarioNotificador.Rows.Count > 0 Then
        '        strUsuarioNotificador = dtUsuarioNotificador(0)("T_PREFIJO").ToString & "  " & dtUsuarioNotificador(0)("NOMBRECOMPLETO").ToString
        '    End If

        '    Dim pdfTemplate As String = Server.MapPath("~/Plantillas/plantilla_CedulaElectronica.pdf")

        '    ' open the reader
        '    Dim reader As iTextSharp.text.pdf.PdfReader = New iTextSharp.text.pdf.PdfReader(pdfTemplate)
        '    Dim size As iTextSharp.text.Rectangle = reader.GetPageSizeWithRotation(1)
        '    Dim document As iTextSharp.text.Document = New iTextSharp.text.Document(size)

        '    'open the writer
        '    Dim fs As FileStream = New FileStream(ruta, FileMode.Create, FileAccess.Write)
        '    Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs)
        '    writer.SpaceCharRatio = iTextSharp.text.pdf.PdfWriter.NO_SPACE_CHAR_RATIO
        '    document.Open()

        '    Dim arialNormal As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9.0F, iTextSharp.text.Font.NORMAL)
        '    Dim arialBold As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9.0F, iTextSharp.text.Font.BOLD)
        '    Dim arialItalic As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9.0F, iTextSharp.text.Font.ITALIC)
        '    Dim arialBoldItalic As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9.0F, iTextSharp.text.Font.BOLDITALIC)
        '    Dim arialUnderline As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9.0F, iTextSharp.text.Font.UNDERLINE)
        '    Dim arialBoldUnderline As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 9.0F, iTextSharp.text.Font.UNDERLINE)

        '    Dim phrase As iTextSharp.text.Phrase = New iTextSharp.text.Phrase


        '    Dim text As String = "En la Ciudad de México, Distrito Federal, siendo las «txt_Hora»  del «txt_Fecha», " & _
        '                "el que suscribe envía por Correo Electrónico a través del Sistema de Información Electrónica (SIE) de manera simultánea " & _
        '                "«txt_Destinatarios» autorizados por «txt_Entidad_Largo», personalidad que tienen debidamente acreditada y reconocida ante " & _
        '                "esta Comisión Nacional del Sistema de Ahorro para el Retiro, en el Registro de Usuarios Autorizados para ser notificados " & _
        '                "de los actos administrativos a que se refiere el artículo 345 del Título Octavo de las "
        '    'Hora
        '    text = text.Replace("«txt_Hora»", hora)
        '    'Fecha
        '    text = text.Replace("«txt_Fecha»", fecha)
        '    'entidad largo
        '    text = text.Replace("«txt_Entidad_Largo»", dtOficio.Rows(0)("T_ENTIDAD_LARGO").ToString().ToUpper.Trim)

        '    'destinatarios
        '    text = text.Replace("«txt_Destinatarios»", strDestinatarios.ToString())

        '    Dim c1 As New iTextSharp.text.Chunk(text, arialNormal)


        '    text = "DISPOSICIONES de carácter general " & _
        '                "en materia de operaciones de los sistemas de ahorro para el retiro,"
        '    Dim c2 As New iTextSharp.text.Chunk(text, arialItalic)

        '    text = "publicadas en el Diario Oficial de la Federación el " & _
        '                "día 30 de julio del 2010, en relación con el artículo 149 de las "
        '    Dim c3 As New iTextSharp.text.Chunk(text, arialNormal)

        '    text = "DISPOSICIONES de carácter general en materia financiera " & _
        '                "de los sistemas de ahorro para el retiro"
        '    Dim c4 As New iTextSharp.text.Chunk(text, arialItalic)

        '    text = ", publicadas en el Diario Oficial de la Federación el día 31 de diciembre de 2010, " & _
        '                "y con fundamento en los artículos 111 de la Ley de los Sistemas de Ahorro para el Retiro, así como en el artículo 134, " & _
        '                "fracción I del Código Fiscal de la Federación; artículos 114, 115, 116, 118, 119, 120, 121 y 122 del Reglamento de la Ley " & _
        '                "de los Sistemas de Ahorro para el Retiro, publicado en el Diario Oficial de la Federación el 24 de agosto de 2009; 336, " & _
        '                "337 y 339 de las "
        '    Dim c5 As New iTextSharp.text.Chunk(text, arialNormal)

        '    text = "DISPOSICIONES de carácter general en materia de operaciones de los sistemas de ahorro para el retiro;"
        '    Dim c6 As New iTextSharp.text.Chunk(text, arialItalic)

        '    text = " 2° fracción III, del "
        '    Dim c7 As New iTextSharp.text.Chunk(text, arialNormal)

        '    text = "Reglamento Interior de la Comisión Nacional del Sistema de Ahorro para el Retiro "
        '    Dim c8 As New iTextSharp.text.Chunk(text, arialItalic)

        '    text = " publicado en el Diario " & _
        '            "Oficial de la Federación del 21 de julio de 2008; se notifica el documento digital consistente en el oficio número " & _
        '            "«id_Oficio_Numero», de fecha «txt_fecha_oficio», suscrito por «txt_Prefijo» «txt_Cargo» de la Comisión Nacional del Sistema de " & _
        '            "Ahorro para el Retiro, en los términos que se indican en el referido oficio, y firmado de conformidad con lo establecido por " & _
        '            "el artículo 351 de las "


        '    'fecha oficio
        '    text = text.Replace("«txt_fecha_oficio»", CType(dtOficio.Rows(0)("F_FECHA_OFICIO"), DateTime).ToLongDateString)

        '    'prefijo
        '    text = text.Replace("«txt_Prefijo»", dtFirma.Rows(0)("T_PREFIJO").ToString())

        '    'Cargo
        '    text = text.Replace("«txt_Cargo»", dtFirma.Rows(0)("T_CARGO").ToString())

        '    'Numero Oficio
        '    text = text.Replace("«id_Oficio_Numero»", dtOficio.Rows(0)("T_OFICIO_NUMERO").ToString())
        '    Dim c9 As New iTextSharp.text.Chunk(text, arialNormal)

        '    text = "DISPOSICIONES de carácter general en materia de operaciones de los sistemas de ahorro para el retiro."
        '    Dim c10 As New iTextSharp.text.Chunk(text, arialItalic)

        '    phrase.Add(c1)
        '    phrase.Add(c2)
        '    phrase.Add(c3)
        '    phrase.Add(c4)
        '    phrase.Add(c5)
        '    phrase.Add(c6)
        '    phrase.Add(c7)
        '    phrase.Add(c8)
        '    phrase.Add(c9)
        '    phrase.Add(c10)

        '    ' text = text.Replace(Environment.NewLine, String.Empty).Replace("  ", String.Empty)

        '    '---------------------------------------------------
        '    ' Posicionamiento del texto
        '    '---------------------------------------------------
        '    'Párrafo principal
        '    Dim cb As iTextSharp.text.pdf.PdfContentByte = writer.DirectContent
        '    Dim ct As iTextSharp.text.pdf.ColumnText = New iTextSharp.text.pdf.ColumnText(cb)
        '    ct.SetSimpleColumn(phrase, 70, 525, 530, 36, 13, iTextSharp.text.Element.ALIGN_JUSTIFIED)
        '    ct.Go()

        '    ' Destinatario:
        '    text = dtOficio.Rows(0)("T_ENTIDAD_LARGO").ToString().ToUpper.Trim
        '    ct = New iTextSharp.text.pdf.ColumnText(cb)
        '    ct.SetSimpleColumn(New iTextSharp.text.Phrase(New iTextSharp.text.Chunk(text, arialNormal)),
        '                       70, 549, 530, 36, 13, iTextSharp.text.Element.ALIGN_LEFT)
        '    ct.Go()

        '    'Frase del año
        '    text = BusinessRules.BDS_C_PARAMETROS.getValor(CInt(ConfigurationManager.AppSettings("FRASE_PLANTILLAS_OFICIOS")))
        '    ct = New iTextSharp.text.pdf.ColumnText(cb)
        '    ct.SetSimpleColumn(New iTextSharp.text.Phrase(New iTextSharp.text.Chunk(text, arialNormal)),
        '                       70, 650, 530, 36, 13, iTextSharp.text.Element.ALIGN_RIGHT)
        '    ct.Go()

        '    'Áreas
        '    Dim areasJerarquia As String = String.Empty
        '    Dim dtAreasJerarquia As DataTable = BusinessRules.BDS_C_AREA.ConsultarJerarquiaBottomUp(CInt(dtOficio.Rows(0)("ID_AREA_OFICIO")))
        '    For Each TableRow As DataRow In dtAreasJerarquia.Rows
        '        areasJerarquia &= TableRow("DSC_UNIDAD_ADM").ToString & Environment.NewLine
        '    Next
        '    ct = New iTextSharp.text.pdf.ColumnText(cb)
        '    ct.SetSimpleColumn(New iTextSharp.text.Phrase(New iTextSharp.text.Chunk(areasJerarquia, arialNormal)),
        '                       70, 650, 530, 36, 13, iTextSharp.text.Element.ALIGN_LEFT)
        '    ct.Go()

        '    'Notificador
        '    text = strUsuarioNotificador
        '    ct = New iTextSharp.text.pdf.ColumnText(cb)
        '    ct.SetSimpleColumn(New iTextSharp.text.Phrase(New iTextSharp.text.Chunk(text, arialNormal)),
        '                       70, 92, 530, 36, 13, iTextSharp.text.Element.ALIGN_CENTER)
        '    ct.Go()

        '    Dim Page As iTextSharp.text.pdf.PdfImportedPage = writer.GetImportedPage(reader, 1)
        '    cb.AddTemplate(Page, 0, 0)

        '    ' Cerrar documentos para que se apliquen los cambios
        '    document.Close()
        '    fs.Close()

        '    writer.Close()
        '    reader.Close()

        '    ''--------------------------------------------------
        '    '' Sube a Sharepoint
        '    ''--------------------------------------------------
        '    Dim encrip As New YourCompany.Utils.Encryption.Encryption64
        '    Dim objSP As Clases.nsSharePoint.FuncionesSharePoint
        '    objSP = New Clases.nsSharePoint.FuncionesSharePoint(encrip.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR"), WebConfigurationManager.AppSettings("UsuarioSp"), encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR"), encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR"))
        '    objSP.Biblioteca = encrip.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")
        '    objSP.RutaArchivo = Path.GetTempPath()
        '    objSP.NombreArchivo = fileName
        '    objSP.UploadFileToSharePoint()

        '    Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO
        '    objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

        '    objOficio.ArchivoCedulaPDF = fileName
        '    objOficio.IdAnio = CType(dtOficio.Rows(0)("ID_ANIO"), Integer)
        '    objOficio.IdArea = CType(dtOficio.Rows(0)("ID_AREA_OFICIO"), Integer)
        '    objOficio.IdTipoDocumento = CType(dtOficio.Rows(0)("ID_TIPO_DOCUMENTO"), Integer)
        '    objOficio.IOficioConsecutivo = CType(dtOficio.Rows(0)("I_OFICIO_CONSECUTIVO"), Integer)
        '    Dim resultado As Boolean = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaPDF(objOficio)
        '    If resultado Then modalMensaje("Se generó la Cédula Electrónica exitosamente", "INFORMACION")

        '    ''--------------------------------------------------
        '    '' Borra archivos temporales.
        '    ''--------------------------------------------------
        '    Try
        '        'If File.Exists(rutaPDF) Then File.Delete(rutaPDF)

        '        If File.Exists(ruta) Then File.Delete(ruta)

        '    Catch ex As Exception
        '        'EscribirError(ex, "Generar cedula electronica, borrar archivo temporal")
        '    End Try


        '    ''--------------------------------------------------
        '    '' End
        '    ''--------------------------------------------------
        'Catch ex As ApplicationException
        '    modalMensaje(ex.Message)
        'Catch ex As Exception
        '    EscribirError(ex, "Generar cedula electronica - Registro")
        'End Try

    End Sub

    Private Shared Function AgregaDiasPlazo(ByVal fechaInicio As Date, ByVal diasPlazo As Integer, ByVal listaAsuetos As List(Of Date)) As Date

        Dim countAllDays As Integer = -1
        Dim countBusinessDays As Integer = 0

        Do
            countAllDays += 1

            If fechaInicio.AddDays(countAllDays).DayOfWeek <> DayOfWeek.Saturday _
                               AndAlso fechaInicio.AddDays(countAllDays).DayOfWeek <> DayOfWeek.Sunday _
                                       AndAlso Not listaAsuetos.Contains(fechaInicio.AddDays(countAllDays)) Then

                countBusinessDays += 1

            End If

        Loop While countBusinessDays <= diasPlazo

        Return fechaInicio.AddDays(countAllDays)

    End Function

    <Services.WebMethod()> _
    Public Shared Function ObtenerDestinatarioOficios(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim colResultado As New List(Of String)
        Dim sql As String = String.Empty
        Dim dt As DataTable

        Try

            If Not HttpContext.Current.Session("ddlEntidad.SelectedValue") Is Nothing Then

                Dim iEntidadValue As Integer = CInt(HttpContext.Current.Session("ddlEntidad.SelectedValue"))

                If iEntidadValue = 0 Then Exit Try

                If iEntidadValue = CInt(WebConfigurationManager.AppSettings("ID_ENTIDAD_CONSAR").ToString()) Then
                    dt = LogicaNegocioSICOD.BusinessRules.BDA_PERSONAL.ConsultarPersonalEntidadCONSAR(prefixText)
                Else
                    dt = LogicaNegocioSICOD.BusinessRules.BDA_PERSONAL.ConsultarPersonalPorEntidad(iEntidadValue, CInt(HttpContext.Current.Session("ddlTipoEntidad.SelectedValue")), prefixText)
                End If

                'Dim dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_PERSONAL.ConsultarPersonalPorEntidad(iEntidadValue, prefixText)

                If dt.Rows.Count > 0 Then

                    For Each Fila As DataRow In dt.Rows
                        Dim str As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Fila("NOMBRE").ToString, Fila("ID_PERSONA").ToString)
                        colResultado.Add(Trim(str))
                    Next
                End If
            End If

        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion ObtenerDestinatario: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return colResultado.ToArray
    End Function

    ''' <summary>
    ''' Agrega los días del plazo a la fecha del acuse para generar la fecha de vencimiento
    ''' </summary>
    ''' <param name="plazoDias"></param>
    ''' <param name="fechaAcuse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()> _
    Public Shared Function GetPlazoDate(ByVal plazoDias As String, ByVal fechaAcuse As String) As Array
        Dim d(1) As String
        If IsNumeric(plazoDias) Then

            If fechaAcuse = String.Empty Then fechaAcuse = Today.ToShortDateString()

            '---------------------------------------
            ' Obtén la lista de días festivos del año en curso
            '---------------------------------------
            If HttpContext.Current.Session("Dias_Festivos_Table") Is Nothing Then
                HttpContext.Current.Session("Dias_Festivos_Table") = LogicaNegocioSICOD.BusinessRules.BDV_C_DIA_FESTIVO.ConsultarDiasFestivos(Today.Year)
            End If

            '---------------------------------------
            ' De la tabla obtener cada dia, mes y año y convertir a lista de fechas
            '---------------------------------------
            Dim dt As DataTable = TryCast(HttpContext.Current.Session("Dias_Festivos_Table"), DataTable)
            Dim listaAsuetosAnio As New List(Of Date)
            For Each row As DataRow In dt.Rows
                listaAsuetosAnio.Add(New Date(CInt(row("CICLO")), CInt(row("MES")), CInt(row("DIA"))))
            Next

            d(0) = AgregaDiasPlazo(DateTime.Parse(fechaAcuse), CInt(plazoDias), listaAsuetosAnio).ToShortDateString
            d(1) = fechaAcuse
        End If

        Return d

    End Function

    Private Sub deshabilitaModificacionOficio()
        PanelAreaNotEnable()
        pnlDatosBasicos.Enabled = False
        pnlFechas.Enabled = False
        pnlFechasPlazo.Enabled = False
        pnlFirmas.Enabled = False
        pnlComentariosCancelacion.Enabled = False
        pnlMultipleAfore.Enabled = False
        pnlGenerarDocumento.Style.Add("display", "none")


        btnGuardar.Visible = False

        Dim Perfil As New Perfil
        If Perfil.FuncionPerfil(42) Then
            btnGuardar.Visible = True
        End If

        btnGuardarGenerarOficios.Visible = False
        btnEnviar.Visible = False
        lblTitulo.Text = "Documento de sólo lectura"
        pnlLnkDictamen.Enabled = False
        pnlLnkOficioExterno.Enabled = False
    End Sub

    Protected Sub AbreArchivoLink(ByVal NombreArchivo As String, Optional ByVal IsMachote As Boolean = False)

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


                    ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
                    Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")

                    If IsMachote Then

                        ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerMachotes"), "webCONSAR")
                        Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibraryMachotes"), "webCONSAR")

                    End If

                    usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
                    passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                    Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")


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
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoLink")
        End Try
    End Sub

    Protected Sub AbreArchivoFileSystem(ByVal rutaArchivo As String)

        Dim filename As String = String.Empty
        Dim Archivo() As Byte = Nothing
        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Try

            filename = "attachment; filename=" & Server.UrlPathEncode(rutaArchivo)
            Archivo = cliente.DownloadData(rutaArchivo)
            Dim tipo_arch As String = Path.GetExtension(rutaArchivo)

            Select Case tipo_arch
                Case ".zip"
                    Response.ContentType = "application/x-zip-compressed"
                Case ".pdf"
                    Response.ContentType = "application/pdf"
                Case ".csv"
                    Response.ContentType = "text/csv"
                Case ".doc"
                    Response.ContentType = "application/doc"
                Case ".docx"
                    Response.ContentType = "application/docx"
                Case ".xls"
                    Response.ContentType = "application/xls"
                Case ".xlsx"
                    Response.ContentType = "application/xlsx"
                Case ".png"
                    Response.ContentType = "image/png"
                Case "gif"
                    Response.ContentType = "image/gif"
                Case ".jpg"
                    Response.ContentType = "image/jpeg"
                Case ".jpeg"
                    Response.ContentType = "image/jpeg"
                Case ".txt"
                    Response.ContentType = "application/txt"
                Case ".ppt"
                    Response.ContentType = "application/vnd.ms-project"
                Case ".pptx"
                    Response.ContentType = "application/vnd.ms-project"
                Case ".bmp"
                    Response.ContentType = "image/bmp"
                Case ".tif"
                    Response.ContentType = "image/tiff"
                Case ".sbm", ".sbmx"
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

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
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

    Private Function ConsultarNumeroExpediente(ByVal pIdAnio As Integer, ByVal pIdUnidad As Integer, ByVal pIdTipoDocumento As Integer, ByVal pConsecutivo As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Integer
        Dim sql As String = String.Empty
        Dim returnValue As Integer = 0
        Try
            sql =
                    "   SELECT                                      " +
                    "       ID_EXPEDIENTE                           " +
                    "   FROM " + Conexion.Owner + "BDA_R_OFICIOS    " +
                    "   WHERE                                       " +
                    "   ID_AREA_OFICIO=                             " + pIdUnidad.ToString +
                    "   AND ID_TIPO_DOCUMENTO=                      " + pIdTipoDocumento.ToString +
                    "   AND ID_ANIO=                                " + pIdAnio.ToString +
                    "   AND I_OFICIO_CONSECUTIVO=                   " + pConsecutivo.ToString

            Dim dr As Odbc.OdbcDataReader = con.Consulta(sql, tran)
            If (dr.HasRows) Then
                While (dr.Read())
                    returnValue = CInt(dr.GetValue(0))
                    Exit While
                End While
                dr.Close()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return returnValue

    End Function

    Private Function ConsultarNumeroExpediente(ByVal pIdAnio As Integer, ByVal pIdUnidad As Integer, ByVal pIdTipoDocumento As Integer, ByVal pConsecutivo As Integer) As Integer
        Dim sql As String = String.Empty
        Dim con As Conexion = Nothing
        Dim returnValue As Integer = 0
        Try
            con = New Conexion
            sql =
                    "   SELECT                                          " +
                    "       ID_EXPEDIENTE                               " +
                    "   FROM " + Conexion.Owner + "BDA_R_OFICIOS       " +
                    "   WHERE                                           " +
                    "   ID_AREA_OFICIO=                                 " + pIdUnidad.ToString +
                    "   AND ID_TIPO_DOCUMENTO=                          " + pIdTipoDocumento.ToString +
                    "   AND ID_ANIO=                                    " + pIdAnio.ToString +
                    "   AND I_OFICIO_CONSECUTIVO=                       " + pConsecutivo.ToString

            Dim dr As Odbc.OdbcDataReader = con.Consulta(sql)
            If (dr.HasRows) Then
                While (dr.Read())
                    returnValue = CInt(dr.GetValue(0))
                    Exit While
                End While
                dr.Close()
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If con IsNot Nothing Then con.Cerrar()
            con = Nothing
        End Try

        Return returnValue
    End Function

    Private Function ConsultarOficioPorLlave(ByVal pIdAnio As Integer, ByVal pIdUnidad As Integer, ByVal pIdTipoDocumento As Integer, ByVal pConsecutivo As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As DataTable
        Dim sql As String = String.Empty
        Dim dt As DataTable = Nothing
        Try

            sql =
                    "SELECT [ID_AREA_OFICIO] " +
                    " ,[ID_TIPO_DOCUMENTO]" +
                    " ,[ID_ANIO]" +
                    " ,[I_OFICIO_CONSECUTIVO]" +
                    " ,[ID_PUESTO_DESTINATARIO]" +
                    " ,[ID_ENTIDAD_TIPO]" +
                    " ,[ID_ESTATUS]" +
                    " ,[ID_CLASIFICACION]" +
                    " ,[USUARIO_ELABORO]" +
                    " ,[ID_PRIORIDAD]" +
                    " ,[ID_INCUMPLIMIENTO]" +
                    " ,[ID_DESTINATARIO]" +
                    " ,[ID_SUBENTIDAD]" +
                    " ,[USUARIO_CANCELACION]" +
                    " ,[USUARIO_ALTA]" +
                    " ,[ID_NOTIFICADOR]" +
                    " ,[I_OFICIO_ANT]" +
                    " ,[T_OFICIO_NUMERO]" +
                    " ,[F_FECHA_OFICIO]" +
                    " ,[F_FECHA_ALTA]" +
                    " ,[F_FECHA_ACUSE]" +
                    " ,[I_PLAZO_DIAS]" +
                    " ,[F_FECHA_VENCIMIENTO]" +
                    " ,[F_FECHA_RECEPCION]" +
                    " ,[PLAZO_FLAG]" +
                    " ,[T_ASUNTO]" +
                    " ,[T_COMENTARIO]" +
                    " ,[I_PERIODO_RESERVA]" +
                    " ,[ID_FUNDRESERVA]" +
                    " ,[ID_FUNDCONFIDEN]" +
                    " ,[NOTIF_ELECTRONICA_FLAG]" +
                    " ,[ARCHIVADO_FLAG]" +
                    " ,[T_ARCHIVADO]" +
                    " ,[I_OFICIO_RELACIONADO]" +
                    " ,[DICTAMINADO_FLAG]" +
                    " ,[T_HYP_ARCHIVOSCAN]" +
                    " ,[T_HYP_ARCHIVOWORD]" +
                    " ,[T_HYP_CEDULAPDF]" +
                    " ,[T_HYP_FIRMADIGITAL]" +
                    " ,[T_CEDULADIGITAL]" +
                    " ,[T_HYP_RESPUESTAOFICIO]" +
                    " ,[T_HYP_ACUSERESPUESTA]" +
                    " ,[T_HYP_EXPEDIENTE]" +
                    " ,[I_NUM_DICTAMEN]" +
                    " ,[F_FECHA_DICTAMEN]" +
                    " ,[T_ANEXO_UNO]" +
                    " ,[T_ANEXO_DOS]" +
                    " ,[COPIA_FLAG]" +
                    " ,[F_FECHA_NOTIFICACION]" +
                    " ,[F_FECHA_CANCELACION]" +
                    " ,[T_DESCRIPCION_CANCELACION]" +
                    " ,[ID_OFICIO]" +
                    " ,IS_FILE_FLAG" +
                    " ,FIRMA_SIE_FLAG" +
                    " FROM " + Conexion.Owner + "BDA_OFICIO" +
                    " WHERE ID_AREA_OFICIO = " + pIdUnidad.ToString +
                    " AND ID_TIPO_DOCUMENTO = " + pIdTipoDocumento.ToString +
                    " AND ID_ANIO = " + pIdAnio.ToString +
                    " AND I_OFICIO_CONSECUTIVO = " + pConsecutivo.ToString

            Dim ds As New DataSet
            con.ConsultaAdapter(sql, tran).Fill(ds, "dfds")
            Return ds.Tables(0)

        Catch ex As Exception

        End Try
        Return dt
    End Function

    Private Function get_B_APLICA_NUM_CONSEC(ByVal pIdAnio As Integer, ByVal pIdUnidad As Integer, ByVal pIdTipoDocumento As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Boolean

        Dim aplicaNumConsecutvio As Boolean = False


        Dim sql As String = " select top(1) isnull(B_APLICA_NUM_CONSEC,0) as B_APLICA_NUM_CONSEC,  NUM_CONSECUTIVO " + _
                            " from BDA_C_CONSECUTIVO_OFICIOS " + _
                            " where ID_UNIDAD_ADM = " + pIdUnidad.ToString() + _
                            " and ID_ANIO =  " + pIdAnio.ToString() + _
                            " and ID_TIPO_DOCUMENTO = " + pIdTipoDocumento.ToString() + _
                            " order by F_FECH_MODIFICA desc "

        Dim dr As Odbc.OdbcDataReader = con.Consulta(sql, tran)
        If (dr.HasRows) Then
            While (dr.Read())
                aplicaNumConsecutvio = Convert.ToBoolean((dr.GetValue(0)))
                Exit While
            End While
            dr.Close()
        End If

        Return aplicaNumConsecutvio

    End Function

    Private Function AplicaNumeroConsecutivo(ByVal pIdAnio As Integer, ByVal pIdUnidad As Integer, ByVal pIdTipoDocumento As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Integer

        Dim aplicaNumConsecutvio As Boolean
        Dim returnValue As Integer = 0


        Dim sql As String = " select top(1) isnull(B_APLICA_NUM_CONSEC,0) as B_APLICA_NUM_CONSEC,  NUM_CONSECUTIVO " + _
                            " from BDA_C_CONSECUTIVO_OFICIOS " + _
                            " where ID_UNIDAD_ADM = " + pIdUnidad.ToString() + _
                            " and ID_ANIO =  " + pIdAnio.ToString() + _
                            " and ID_TIPO_DOCUMENTO = " + pIdTipoDocumento.ToString() + _
                            " order by F_FECH_MODIFICA desc "

        Dim dr As Odbc.OdbcDataReader = con.Consulta(sql, tran)
        If (dr.HasRows) Then
            While (dr.Read())
                aplicaNumConsecutvio = Convert.ToBoolean((dr.GetValue(0)))
                returnValue = CInt(dr.GetValue(1))
                Exit While
            End While
            dr.Close()
        End If

        If aplicaNumConsecutvio = False Then
            returnValue = ConsultarMaximoConsecutivo(pIdAnio, pIdUnidad, pIdTipoDocumento, con, tran)
        End If

        Return returnValue

    End Function

    Private Function ConsultarMaximoConsecutivo(ByVal pIdAnio As Integer, ByVal pIdUnidad As Integer, ByVal pIdTipoDocumento As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Integer
        Dim returnValue As Integer = 0

        '' Obtenemos el COnsecutivo Inicial
        Dim Inicial As Int32 = BusinessRules.BDA_C_CONSECUTIVO_OFICIOS.GetByKey(pIdAnio, pIdUnidad, pIdTipoDocumento)

        Dim sql As String =
                            "SELECT " +
                          " ISNULL(MAX(I_OFICIO_CONSECUTIVO),0) + 1 AS MAXIMO " +
                          " FROM " + Conexion.Owner + "BDA_OFICIO" +
                          " WHERE ID_ANIO = " + ID_ANIO.ToString +
                          " AND ID_AREA_OFICIO = " + ID_UNIDAD_ADM.ToString +
                          " AND ID_TIPO_DOCUMENTO = " + ID_TIPO_DOCUMENTO.ToString

        Dim dr As Odbc.OdbcDataReader = con.Consulta(sql, tran)
        If (dr.HasRows) Then
            While (dr.Read())
                returnValue = CInt(dr.GetValue(0))
                Exit While
            End While
            dr.Close()
        End If

        '' SI NO SE HAN GENERADO OFICIOS Y EXISTE UN VALOR INICIAL, SE TOMA ÉSTE ULTIMO
        If returnValue <= 1 And Inicial > 0 Then returnValue = Inicial

        Return returnValue
    End Function

    Private Function ConsultarMaximoConsecutivoExpediente(ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Integer
        Dim sql As String = String.Empty
        Dim returnValue As Integer = 0

        Try
            sql =
                   "SELECT " +
                   " ISNULL(MAX(ID_EXPEDIENTE),0) + 1 AS MAXIMO " +
                   " FROM " + Conexion.Owner + "BDA_R_OFICIOS"
            Dim dr As Odbc.OdbcDataReader = con.Consulta(sql, tran)
            If dr.HasRows Then
                While dr.Read
                    returnValue = CInt(dr("MAXIMO"))
                End While
            End If

        Catch ex As Exception

        Finally

        End Try
        Return returnValue

    End Function

    Private Function ConsultarIdPorNombre(ByVal pNombre As String, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction) As Integer

        Dim sql As String = String.Empty
        Dim returnValue As Integer = 0

        sql = "SELECT " +
                      " t.ID_TIPO_ENTIDAD " +
                      " FROM BDA_TIPO_ENTIDAD t " +
                      " INNER JOIN osiris.BDV_C_T_ENTIDAD vt ON t.ID_TIPO_ENTIDAD = vt.ID_T_ENT " +
                      " AND vt.VIG_FLAG = 1" +
                      " WHERE LTRIM(RTRIM(t.T_TIPO_ENTIDAD)) = '" + pNombre + "'"

        Dim dr As Odbc.OdbcDataReader = con.Consulta(sql, tran)
        If dr.HasRows Then
            While dr.Read
                returnValue = CInt(dr.GetValue(0))
            End While
        End If
        Return returnValue
    End Function

    Private Function ConsultarPersonalPorFuncion(ByVal pIdEntidad As Integer, ByVal pIdFuncion As Integer, ByVal con As Conexion, ByVal tran As Odbc.OdbcTransaction, ByVal pIdTipoEntidad As Integer) As DataTable

        Dim sql As String = String.Empty

        sql =
                "SELECT " +
                " p.ID_PERSONA " +
                " ,p.T_PREFIJO " +
                " ,p.T_NOMBRE " +
                " ,p.T_APELLIDO_P " +
                " ,p.T_APELLIDO_M " +
                " FROM BDA_PERSONAL p " +
                " INNER JOIN BDA_R_PERSONAL_FUNCION pf ON pf.ID_PERSONA = p.ID_PERSONA " +
                " INNER JOIN BDA_FUNCION f ON pf.ID_FUNCION = f.ID_FUNCION" +
                " WHERE p.ID_ENTIDAD = " + pIdEntidad.ToString +
                " AND p.ID_TIPO_ENTIDAD = " + pIdTipoEntidad.ToString() +
                " AND f.ID_FUNCION = " + pIdFuncion.ToString +
                " AND p.VIG_FLAG = 1"

        Dim ds As New DataSet
        con.ConsultaAdapter(sql, tran).Fill(ds)
        Return ds.Tables(0)
    End Function

    Private Sub VerNumeroOficio(ByVal Flag As Boolean)

        lblNumeroOficioTag.Visible = Flag
        lblNumeroOficio.Visible = Flag

        If Flag Then

            TablaNumOficio.Style.Item("border-bottom-style") = "ridge"
            TablaNumOficio.Style.Item("border-top-style") = "ridge"
            TablaNumOficio.Style.Item("border-left-style") = "ridge"
            TablaNumOficio.Style.Item("border-right-style") = "ridge"
            ddlEstatus.CssClass = "txt_gral_oscuro"
            ddlEstatus.Font.Bold = True

        Else

            TablaNumOficio.Style.Item("border") = "0"
            ddlEstatus.CssClass = "txt_gral"
            ddlEstatus.Font.Bold = False

        End If


    End Sub

    Protected Sub AsyncFileUpFinish(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs)

        FILEMERGEPATH = ""

        If e.state = AjaxControlToolkit.AsyncFileUploadState.Success Then

            Dim randomClass As Random = New Random(Now.Millisecond)

            Dim Path As String = System.IO.Path.GetTempPath() & Format(randomClass.Next(1000), "0000") & AsyncFileUp.FileName

            AsyncFileUp.SaveAs(Path)

            FILEMERGEPATH = Path

            FileFlag = False

        End If

        mpeProcesa.Hide()

    End Sub


#End Region

    Protected Sub iBtnBitacora_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnBitacora.Click

        'Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1", True)
        Session("NUMERO_OFICIO") = NUMERO_OFICIO
        Response.Redirect("ConsultaHistorial.aspx?Type=of", False)

    End Sub

    Private Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged

        Dim _TOP As Integer = BusinessRules.BDS_C_AREA.ConsultarTopUnidadAdm(CInt(ddlArea.SelectedValue), CInt(rblEstructuraElaboro.SelectedValue))
        TOP_AREA_SEL = _TOP

        CargarCombo(ddlClasificacion, BusinessRules.BDA_CLASIFICACION_OFICIO.getPorAreaEstructura(TOP_AREA_SEL, 2), "T_CLASIFICACION", "ID_CLASIFICACION")
        ddlClasificacion.Enabled = True

        CargarCombo(ddlTema, BusinessRules.BDA_C_TEMAS.GetTemasByArea(TOP_AREA_SEL, 1), "DSC_TEMA", "ID_TEMA")

    End Sub

    Private Sub ddlTema_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTema.SelectedIndexChanged

        Dim dt As DataTable = BusinessRules.BDA_C_TEMAS.GetMachotesByAreaTema(TOP_AREA_SEL, 1, CInt(ddlTema.SelectedValue))

        grvMachotes.DataSource = dt
        grvMachotes.DataBind()

        grvMachotes.SelectedIndex = -1

        mpeWordFile.Show()

    End Sub

    Private Sub btnAceptarWordFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarWordFile.Click

        FileFlag = True

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "TraeArchivo", "click_Archivo();", True)


        'Dim _archivo As String = String.Empty

        'For Each row As GridViewRow In grvMachotes.Rows

        '    If row.RowState = DataControlRowState.Normal Then

        '    ElseIf row.RowState = DataControlRowState.Selected Then
        '        _archivo = row.Cells(1).Text
        '        Exit For
        '    End If

        'Next

        'If _archivo = String.Empty Then
        '    mpeWordFile.Show()
        '    Exit Sub
        'End If

        'Try

        '    mpeWordFile.Hide()
        '    AbreArchivoLink(_archivo)

        'Catch ex As Threading.ThreadAbortException
        '    '---------------------------------------------
        '    ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
        '    '---------------------------------------------
        'Catch ex As Exception
        '    EscribirError(ex, "imgFormatoWord_Click")

        'End Try


    End Sub

    Private Sub grvMachotes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvMachotes.RowCommand

        If e.CommandName = "Selecciona" Then

            grvMachotes.SelectedRowStyle.BackColor = System.Drawing.Color.FromArgb(208, 114, 95)
            grvMachotes.SelectedRowStyle.Font.Bold = True
            grvMachotes.SelectedRowStyle.ForeColor = System.Drawing.Color.White
            grvMachotes.SelectedIndex = Convert.ToInt32(e.CommandArgument)
            INDEXMACHOTE = grvMachotes.SelectedIndex

            mpeWordFile.Show()

        End If


    End Sub

    Private Sub grvMachotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvMachotes.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim _ib As LinkButton = CType(e.Row.Cells(0).Controls(0), LinkButton)
            _ib.CommandArgument = e.Row.RowIndex.ToString()

        End If

    End Sub

    Private Sub linkbotonArchivo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkbotonArchivo.Click

        'If Not FileFlag Then
        '    Exit Sub
        'End If



        Dim _archivo As String = String.Empty

        For Each row As GridViewRow In grvMachotes.Rows

            If row.RowIndex = grvMachotes.SelectedIndex Then

                _archivo = row.Cells(1).Text
                Exit For

            End If


        Next

        If _archivo = String.Empty And FileFlag Then
            mpeWordFile.Show()
            Exit Sub
        End If

        Try

            mpeWordFile.Hide()

            If FileFlag Then

                AbreArchivoLink(_archivo, True)

            End If



        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "imgFormatoWord_Click")

        Finally

            FileFlag = False

        End Try

    End Sub

    Private Sub VerificaRevivirOficio()

        'USUARIO
        Dim Con As New Conexion()
        Dim oDs As New DataSet
        Dim SQL As String = ""

        Try

            SQL = "SELECT ID_NIVEL FROM " & Conexion.Owner & "BDA_R_USUARIO_NIVEL WHERE USUARIO = '" & USUARIO & "' AND ID_T_UNIDAD_ADM = 1"
            oDs = Con.Datos(SQL, False)

            ' Los niveles mayores o iguales a DIRECTOR DE AREA pueden revivir el oficio
            If CInt(oDs.Tables(0).Rows(0)("ID_NIVEL").ToString()) <= 5 Then

                ddlEstatus.Enabled = True
                btnGuardar.Visible = True

            End If



        Catch ex As Exception

        End Try



    End Sub

    Private Sub PanelAreaNotEnable()

        ddlEstatus.Enabled = False

        Dim Perfil As New Perfil
        If Perfil.FuncionPerfil(42) Then
            ddlEstatus.Enabled = True
        End If


        chkMultiplesAfores.Enabled = False
        ddlTipoDocumento.Enabled = False
        rblEstructuraArea.Enabled = False
        ddlArea.Enabled = False
        ddlPrioridad.Enabled = False
        ddlAño.Enabled = False


    End Sub

#Region "Firmas"

    Private Sub rblFirmaSIE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblFirmaSIE.SelectedIndexChanged

        lstFirmas.Items.Clear()
        CargaUsuariosFirmas()

    End Sub

    Private Sub rblEstructuraFirmas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblEstructuraFirmas.SelectedIndexChanged

        CargaAreasFirmas()
        CargaUsuariosFirmas()

    End Sub

    Private Sub CargaAreasFirmas()

        Dim Con As Conexion = Nothing
        Try
            Con = New Conexion()


            Dim _tipoUnidad As UnidadAdministrativaTipo = CType(IIf(CInt(rblEstructuraFirmas.SelectedValue) = 1, _
                                                              UnidadAdministrativaTipo.Oficial, _
                                                              UnidadAdministrativaTipo.Funcional), UnidadAdministrativaTipo)
            CargarCombo(ddlAreaFirmas, LogicaNegocioSICOD.UnidadAdministrativa.GetList(_tipoUnidad, UnidadAdministrativaEstatus.Activo), _
                        "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")



        Catch ex As Exception
            EscribirError(ex, "rblEstructuraFirmas_SelectedIndexChanged")
        Finally
            If Con IsNot Nothing AndAlso Con.Estado Then Con.Cerrar()
            Con = Nothing
        End Try

    End Sub

    Private Sub ddlAreaFirmas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAreaFirmas.SelectedIndexChanged

        CargaUsuariosFirmas()

    End Sub

    Private Sub CargaUsuariosFirmas()

        Select Case CInt(rblFirmaSIE.SelectedValue)

            Case 1
                CargarListBox(lstUsuariosFirma, UsuariosFirmaSIE, "NOMBRECOMPLETO", "USUARIO")

            Case 0
                CargarListBox(lstUsuariosFirma, BusinessRules.BDS_USUARIO.GetAllPorArea(CInt(ddlAreaFirmas.SelectedItem.Value), CInt(rblEstructuraFirmas.SelectedValue)), "NOMBRECOMPLETO", "USUARIO")

        End Select

    End Sub

    Private Function UsuariosFirmaSIE() As DataTable

        Dim _dt As New DataTable

        Dim J As New DataTable
        J = BusinessRules.BDS_USUARIO.GetAllConFirma(CInt(ddlAreaFirmas.SelectedValue), CInt(rblEstructuraFirmas.SelectedValue))
        Dim K As New DataSet
        Dim con As Conexion

        con = New Conexion(Conexion.BD.BD_FirmaElectronica)
        Dim Sql As String = "   SELECT DISTINCT LTRIM(RTRIM(EMAIL)) collate SQL_Latin1_General_CP1_CI_AI FROM " & Conexion.Owner & "BDS_C_USR_SIE where VIG_FLAG=1"
        K = con.Datos(Sql, False)
        Dim u As New ArrayList
        For i = 0 To K.Tables(0).Rows.Count - 1
            If Not String.IsNullOrEmpty(K.Tables(0).Rows(i).Item(0).ToString.Trim) Then u.Add(K.Tables(0).Rows(i).Item(0))
        Next

        _dt = J.Clone
        For i = 0 To J.Rows.Count - 1
            If u.Contains(J.Rows(i).Item("MAIL_SAR").ToString.Trim) Then
                _dt.ImportRow(J.Rows(i))
            End If
        Next
        '


        Return _dt

    End Function

#End Region

#Region "Envío de Documento a oficialia SICOD-Entrada"

    Private Sub btnEnviar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnviar.Click

        Try

            Dim con As New Conexion
            Dim ds As DataSet = con.Datos("SELECT USUARIO FROM BDS_USUARIO WHERE VIG_FLAG = 1 " & _
                                  "AND LTRIM(RTRIM(ISNULL(NOMBRE, '') + ' ' + ISNULL(APELLIDOS, ''))) = '" & _
                                  txtDestinatario.Text.Trim.Replace("'", "''") & "'", False)

            If ds.Tables(0).Rows.Count = 0 Then Throw New ApplicationException("No se ha podido encontrar un destinatario válido")

            hdnDestinatario.Value = ds.Tables(0).Rows(0).Item(0).ToString()


            Dim dvUsuarios As DataView = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetAll().DefaultView
            dvUsuarios.RowFilter = "VIG_FLAG = 1 AND ID_T_UNIDAD_ADM = " & rblEstructuraArea.SelectedValue & " AND USUARIO <> '" & hdnDestinatario.Value & "'"
            dvUsuarios.Sort = "NOMBRECOMPLETO"

            grvFuncionarios.DataSource = dvUsuarios.ToTable()
            grvFuncionarios.DataBind()




            mpeEnvia.Show()




        Catch ex As ApplicationException

            modalMensaje(ex.Message, , "ERROR", )

        Catch ex As Exception

            EscribirError(ex, _PnombreFuncion)

        End Try



    End Sub

    Private Sub btnAceptaEnvia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptaEnvia.Click


        Try

            Dim con As New Conexion

            Dim _usuariosCC As New List(Of Documento.UsuarioCC)
            Dim _envíoNotificacion As Boolean = False
            Dim _actualizaArchivo As Boolean = False
            Dim _actualizaEstado As Boolean = False
            Dim _generaAcuse As Boolean = False
            Dim _folio As Decimal = 0
            Dim _TipoDocto As Decimal = 6
            Dim _TipoDoctoDSC As String = "Oficio"
            If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Atenta_Nota Then
                _TipoDoctoDSC = "Atenta Nota"
                _TipoDocto = 2
            End If


            ' Determinamos usuarios de copia
            Dim chk As CheckBox
            For Each row As GridViewRow In grvFuncionarios.Rows

                chk = CType(row.FindControl("chkFuncionario"), CheckBox)

                If chk.Checked Then

                    _usuariosCC.Add(New Documento.UsuarioCC(row.Cells(2).Text.Trim))

                End If

            Next


            ' Datos del oficio
            Dim drOficio As DataRow = Oficio.GetByKey(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO).Rows(0)



            Dim Nombre As String = ""
            Dim APaterno As String = ""
            Dim AMaterno As String = ""
            Dim Archivo As String = ""
            Dim IdArchivo As String = ""
            Dim Fecha_Reg As String = Format(Now, "yyyy-MM-dd HH:mm:ss")
            Dim nivelFirmante As String = ""

            Archivo = drOficio.Item("T_HYP_ARCHIVOSCAN").ToString()


            If lstFirmas.Items.Count > 0 Then

                ' Obtenemos nombre y apellidos del usuario que firma el documento
                Dim drUsuarioFirmo As DataRow = BusinessRules.BDS_USUARIO.GetOne(lstFirmas.Items(0).Value).Rows(0)

                Nombre = drUsuarioFirmo.Item("NOMBRE").ToString()
                Dim Apellidos As String() = drUsuarioFirmo.Item("APELLIDOS").ToString().Trim.Split(" "c)
                If Apellidos.Length > 0 Then APaterno = Apellidos(0)
                If Apellidos.Length > 1 Then AMaterno = Apellidos(1)
                If Nombre.Length > 40 Then Nombre = Nombre.Substring(0, 40)
                If APaterno.Length > 40 Then APaterno = APaterno.Substring(0, 40)
                If AMaterno.Length > 40 Then AMaterno = AMaterno.Substring(0, 40)

                '' BUSCAMOS NIVEL DEL FIRMANTE PARA COLOCAR COMO CARGO
                Dim ds2 As DataSet = con.Datos("SELECT b.DSC_NIVEL FROM BDA_R_USUARIO_NIVEL a INNER JOIN BDA_C_NIVEL b ON a.ID_NIVEL = b.ID_NIVEL " & _
                                  "WHERE a.ID_T_UNIDAD_ADM = 2 AND a.USUARIO = '" & _
                                  lstFirmas.Items(0).Value & "'", False)



                If ds2.Tables(0).Rows.Count > 0 Then nivelFirmante = ds2.Tables(0).Rows(0).Item(0).ToString()


            End If


            '' BUSCAMOS DESTINATARIO, QUE DEBE SER UN USUARIO ACTUAL DEL SISTEMA
            Dim destinatario As String = hdnDestinatario.Value



            ''1.- se guarda el pdf en sharepoint de SICOD
            SubeSharepointOficioSICOD(System.IO.Path.GetTempPath(), BajaSharepointOficio(Archivo))


            ''2.1.- Cambia el estatus del oficio a CONCLUÍDO para permitir la creacion del folio y expediente
            _actualizaEstado = Oficio.UpdateStatusOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, 6) = 1


            ''2.- se invoca SP de creacion de documento
            _folio = Documento.Create(ddlEntidad.SelectedItem.Text.Trim, destinatario, Fecha_Reg, _
                                    lblNumeroOficio.Text, CDate(drOficio.Item("F_FECHA_OFICIO").ToString).ToString("dd/MM/yyyy"), _
                                    Now.ToString("dd/MM/yyyy"), _TipoDocto, Nombre, APaterno, AMaterno, nivelFirmante, _
                                    txtAsunto.Text.Trim, lblNumeroOficio.Text, 0, Archivo, USUARIO, _
                                    New List(Of Documento.Anexos), _usuariosCC)


            If _folio = 0 Then Throw New ApplicationException("No se pudo crear el documento SICOD")


            ' '' ''3.- Cambia el estatus del oficio a ENVIADO Y BLOQUEA LA MODIFICACION
            ' ''_actualizaEstado = Oficio.UpdateStatusOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, 14) = 1


            ''4.- se actualiza metadata del archivo pdf en sharepoint
            Try

                Dim act_documento = New LeerSharep

                Dim dtListaDoctos As DataTable = act_documento.LeerBibliotecaDoctos

                For Each _row As DataRow In dtListaDoctos.Rows

                    If _row("nomArch").ToString.ToUpper = Archivo.ToUpper Then

                        IdArchivo = _row("idArch").ToString()

                    End If

                Next

                _actualizaArchivo = act_documento.Act_Datos_Archivo(CInt(_folio), txtDestinatario.Text, Fecha_Reg, lblNumeroOficio.Text, CInt(IdArchivo))

            Catch ex As Exception

                EventLogWriter.EscribeEntrada(" Envío Oficio-SICOD: No se ha podido acceder a Sharepoint:" + ex.Message.ToString(), EventLogEntryType.Error)

            End Try



            ''5.- se envía correo aviso
            _envíoNotificacion = EnviaCorreoNotificacionSICOD(CInt(_folio), Now.ToString("dd/MM/yyyy"), _TipoDoctoDSC, destinatario, _usuariosCC)


            '' Generamos cadena para sello digital
            'FOLIO||NUMERO OFICIO||DESTINATARIO||USUARIO QUE ENVIA||ASUNTO||ID UNIDAD||ID TIPO DOCTO||AÑO||CONSECUTIVO
            Dim cadenaSello As String = "||" & _folio.ToString & "|" & _
                lblNumeroOficio.Text & "|" & _
                txtDestinatario.Text.Trim & "|" & _
                USUARIO & "|" & _
                txtAsunto.Text.Trim & "|" & _
                ID_UNIDAD_ADM.ToString & "|" & _
                ID_TIPO_DOCUMENTO.ToString & "|" & _
                ID_ANIO.ToString & "|" & _
                I_OFICIO_CONSECUTIVO.ToString & "||"

            Dim firmante As String = Nombre & " " & APaterno & " " & AMaterno
            _generaAcuse = GeneraAcuse(CInt(_folio), txtDestinatario.Text.Trim, cadenaSello, firmante.Trim)



            Dim mensaje As String = "El Oficio se ha enviado con el número de folio " & _folio.ToString() & "<br />"

            If Not _envíoNotificacion Then mensaje &= ", No se pudo notificar por correo electrónico"
            If Not _actualizaArchivo Then mensaje &= " , No se pudo asociar el documento con el folio"
            If Not _actualizaEstado Then mensaje &= ", No se pudo actualizar el estado del oficio"
            If Not _generaAcuse Then mensaje &= ", No se pudo generar el acuse de recibo de forma automática"

            modalMensaje(mensaje, "RegresaBandeja", "INFORMACIÓN", False, "Aceptar")

        Catch ex As ApplicationException

            modalMensaje(ex.Message, , "ERROR", )

        Catch ex As Exception

            EscribirError(ex, _PnombreFuncion)

        End Try


    End Sub

    Private Function BajaSharepointOficio(ByVal NombreOficio As String) As String

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


        Try


            usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
            passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
            ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
            Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
            Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")

            cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)

            Dim url As String = String.Empty
            url = ServSharepoint & "/" & Biblioteca & "/" & NombreOficio
            urlEncode = Server.UrlPathEncode(url)
            Archivo = cliente.DownloadData(ResolveUrl(urlEncode))

            filename = NombreOficio

            ' creamos el archivo
            Dim _str As FileStream = IO.File.Create(System.IO.Path.GetTempPath() & filename)
            _str.Write(Archivo, 0, Archivo.Length)
            _str.Close()


        Catch ex As Exception
            Throw New ApplicationException("Hubo un error sustituyendo el documento. ")
        End Try

        Return filename

    End Function

    Private Sub SubeSharepointOficioSICOD(ByVal Ruta As String, ByVal NombreArchivo As String)

        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim objSP As New Clases.nsSharePoint.FuncionesSharePoint

        objSP.ServidorSharePoint = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("SharePointServer"), "webCONSAR")
        objSP.Biblioteca = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("DocLibrary"), "webCONSAR")
        objSP.Usuario = WebConfigurationManager.AppSettings("UsuarioSp").ToString()
        objSP.Password = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
        objSP.Dominio = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
        objSP.RutaArchivo = Ruta
        objSP.NombreArchivo = NombreArchivo

        If Not objSP.UploadFileToSharePoint() Then Throw New ApplicationException("Error Cargando Archivo a Sharepoint")


    End Sub

    Private Function EnviaCorreoNotificacionSICOD(ByVal IdFolio As Integer, ByVal fech_recepcion As String, ByVal TipoDoctoDSC As String, _
                                                  ByVal Destinatario As String, ByVal CC As List(Of Documento.UsuarioCC)) As Boolean

        Dim Exito As Boolean = True
        Dim ds As New DataSet
        Dim dsTemplate As New DataSet
        Dim cont As Integer = 0
        Dim correo As New Correo
        Dim user As New US
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim dom As String = System.Web.Configuration.WebConfigurationManager.AppSettings("SPDomain")
        Dim usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("usuarioCorreo")
        Dim passEnc As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncCorreo"), "webCONSAR")
        Dim fromCorreo As String = System.Web.Configuration.WebConfigurationManager.AppSettings("CorreoEnviar")
        Dim puertoCorreo As Integer = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings("puertoCorreo"))

        Try

            Dim liOAD As New List(Of OAD)
            Dim liUS As New List(Of US)

            Destinatarios(liUS, liOAD, Destinatario, CC)

            ds = correo.ConsultaUsCorreo(liUS, 3)
            dsTemplate = correo.ConsultaTemplateCorreo(3)

            Dim objCorreo As New System.Net.Mail.MailMessage()
            objCorreo.From = New System.Net.Mail.MailAddress(fromCorreo)
            objCorreo.Subject = dsTemplate.Tables(0).Rows(0)("DSC_SUBJECT_CORREO").ToString()


            objCorreo.Body = dsTemplate.Tables(0).Rows(0)("DSC_CUERPO_CORREO").ToString().Replace("#ID_FOLIO#", IdFolio.ToString()).Replace("#FECH_RECEPCION#", Now.ToString("dd/MM/yyyy")).Replace("#DSC_REFERENCIA#", lblNumeroOficio.Text).Replace("#DSC_NUM_OFICIO#", lblNumeroOficio.Text).Replace("#DSC_T_DOC#", TipoDoctoDSC).Replace("#DSC_REMITENTE#", ddlEntidad.SelectedItem.Text.Trim).Replace("#DSC_ASUNTO#", txtAsunto.Text)
            objCorreo.IsBodyHtml = True


            For Each row As DataRow In ds.Tables(0).Rows
                For cont = 0 To liOAD.Count - 1
                    If row.ItemArray(1).ToString() = liOAD(cont).usuario Then
                        If liOAD(cont).usuario = Destinatario And Not objCorreo.To.Contains(New System.Net.Mail.MailAddress(liOAD(cont).email)) Then
                            objCorreo.To.Add(liOAD(cont).email)
                        Else
                            objCorreo.CC.Add(liOAD(cont).email)
                        End If
                    End If
                Next
            Next

            If objCorreo.To.Count > 0 Or objCorreo.CC.Count > 0 Then
                Dim smtpCliente As New System.Net.Mail.SmtpClient(dom, puertoCorreo)
                smtpCliente.Credentials = New System.Net.NetworkCredential(usuario, passEnc)
                smtpCliente.Send(objCorreo)
            End If


        Catch ex As Exception

            Exito = False
            EventLogWriter.EscribeEntrada("EnviaNotificacion, Registro SICOD: " & ex.ToString, EventLogEntryType.Error)

        Finally

            user = Nothing

        End Try

        Return Exito

    End Function

    Private Sub Destinatarios(ByRef liUS As List(Of US), ByRef liOAD As List(Of OAD), ByVal Destinatario As String, ByVal CC As List(Of Documento.UsuarioCC))

        Directorio.tnomb = "1"

        Dim user As New US
        Dim consultaAD As New Directorio

        user = New US()
        user.usuario = Destinatario
        liUS.Add(user)

        For Each copia As Documento.UsuarioCC In CC

            user = New US()
            user.usuario = copia.Usuario
            liUS.Add(user)

        Next

        liOAD = consultaAD.ObtenerLista(liUS)


    End Sub

#End Region

    Private Sub VerificaVisualizaEstructura()

        Dim perfil As New Perfil

        Dim _mostrarOf As Boolean = perfil.FuncionPerfil(40)

        'tr_EstructuraArea.Visible = _mostrar
        'tr_EstructuraElaboro.Visible = _mostrar
        'tr_EstructuraFirmas.Visible = _mostrar
        'tr_EstructuraRubricas.Visible = _mostrar

        If _mostrarOf Then

            rblEstructuraArea.SelectedValue = "1"
            rblEstructuraElaboro.SelectedValue = "1"
            rblEstructuraFirmas.SelectedValue = "1"
            rblEstructuraRubricas.SelectedValue = "1"

        End If


    End Sub

    Private Sub AsyncFileUp_UploadedFileError(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs) Handles AsyncFileUp.UploadedFileError

        modalMensaje("Error: " & e.statusMessage)

    End Sub

    Private Function ValidaDestinatario() As Boolean

        'ddlTipoEntidad 

        If CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Atenta_Nota Or CInt(ddlTipoDocumento.SelectedValue) = OficioTipo.Oficio_Interno Then
            Return LogicaNegocioSICOD.BusinessRules.BDA_PERSONAL.ConsultarPersonalEntidadCONSAR_CoincidenciaExacta(txtDestinatario.Text.Trim).Rows.Count > 0

        Else

            Return True

        End If

    End Function

    Private Function GeneraAcuse(ByVal IdFolio As Integer, ByVal Destinatario As String, ByVal cadenaSello As String, ByVal Firmante As String) As Boolean

        Dim resultado As Boolean = False

        Try


            Dim selloDigital As String = Fun_Generales.sellaDoc(cadenaSello)


            Dim ruta As String = String.Empty
            Dim archivo As String = ""

            ' definimos nombre del archivo
            Select Case ID_TIPO_DOCUMENTO
                Case OficioTipo.Oficio_Externo
                    archivo = "EX"
                Case OficioTipo.Oficio_Interno
                    archivo = "IN"
                Case OficioTipo.Atenta_Nota
                    archivo = "AN"
                Case OficioTipo.Dictamen
                    archivo = "DI"
            End Select


            archivo = "ACU_" & archivo & "_" & _
                        Format(CODIGO_AREA, "000").ToString + "_" & _
                        Format(I_OFICIO_CONSECUTIVO, "0000").ToString() & "_" & _
                        ID_ANIO.ToString & ".pdf"

            ' definimos ruta temporal
            ruta = Path.GetTempPath.ToString() & archivo


            ' Obtenemos la ruta del template
            Dim pdfTemplate As String = Server.MapPath("~/Plantillas/plantilla_AcuseElectronico.pdf")


            ' Comenzamos todo

            ' open the reader
            Dim reader As iTextSharp.text.pdf.PdfReader = New iTextSharp.text.pdf.PdfReader(pdfTemplate)
            Dim size As iTextSharp.text.Rectangle = reader.GetPageSizeWithRotation(1)
            Dim document As iTextSharp.text.Document = New iTextSharp.text.Document(size)


            'open the writer
            Dim fs As FileStream = New FileStream(ruta, FileMode.Create, FileAccess.Write)
            Dim writer As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs)
            writer.SpaceCharRatio = iTextSharp.text.pdf.PdfWriter.NO_SPACE_CHAR_RATIO
            document.Open()


            Dim arialNormal As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Arial", 10.0F, iTextSharp.text.Font.NORMAL)
            Dim leading As Single = 13.0F
            Dim phrase As iTextSharp.text.Phrase = New iTextSharp.text.Phrase

            'Dim ci As CultureInfo = New CultureInfo("es-MX")

            Dim texto As String = ""
            Dim fecha As String = Now.ToLongDateString & " " & Now.ToShortTimeString
            fecha = StrConv(fecha, vbProperCase)


            Dim Dr As Odbc.OdbcDataReader = Nothing
            Dim Conecta = New Conexion()

            Try


                Dr = Conecta.Consulta("SELECT VALOR FROM " & Conexion.Owner & "BDS_C_PARAMETROS WHERE PARAMETRO='TEXTO_ACUSE_ENVIA'")
                Dr.Read()
                texto = Convert.ToString(Dr.Item("VALOR"))

                If texto.Trim = "" Then Throw New NotImplementedException("Falta el parametro")

            Catch ex As Exception

                texto = "Acuse de recibo electrónico generado por el sistema SICOD. Folio: #ID_FOLIO# - Oficio: #NUM_OFICIO# - Destinatario: #DESTINATARIO# - Firma: #FIRMA# - Asunto: #ASUNTO#"

            Finally

                If Not Dr Is Nothing Then
                    If Not Dr.IsClosed Then
                        Dr.Close()
                    End If
                End If
                If Not Conecta Is Nothing Then
                    Conecta.Cerrar()
                    Conecta = Nothing
                End If

            End Try

            texto = texto.Replace("#ID_FOLIO#", IdFolio.ToString)
            texto = texto.Replace("#NUM_OFICIO#", NUMERO_OFICIO)
            texto = texto.Replace("#DESTINATARIO#", Destinatario)
            texto = texto.Replace("#ASUNTO#", txtAsunto.Text.Trim)
            texto = texto.Replace("#FIRMA#", Firmante)


            Dim fechaTexto As New iTextSharp.text.Chunk(fecha, arialNormal)
            Dim cuerpoTexto As New iTextSharp.text.Chunk(texto, arialNormal)

            phrase.Add(cuerpoTexto)

            ' Parrafo de texto
            Dim cb As iTextSharp.text.pdf.PdfContentByte = writer.DirectContent
            Dim ct As iTextSharp.text.pdf.ColumnText = New iTextSharp.text.pdf.ColumnText(cb)
            ct.SetSimpleColumn(phrase, 70, 515, 530, 36, leading, iTextSharp.text.Element.ALIGN_JUSTIFIED)
            ct.Go()

            ' Fecha
            ct = New iTextSharp.text.pdf.ColumnText(cb)
            ct.SetSimpleColumn(New iTextSharp.text.Phrase(fechaTexto), 70, 549, 530, 13, leading, iTextSharp.text.Element.ALIGN_RIGHT)
            ct.Go()



            ' Frase Cadena cifrada
            ct = New iTextSharp.text.pdf.ColumnText(cb)
            ct.SetSimpleColumn(New iTextSharp.text.Phrase("Sello Digital:", arialNormal), 70, 115, 530, 13, leading, iTextSharp.text.Element.ALIGN_LEFT)
            ct.Go()
            ' Cadena cifrada
            ct = New iTextSharp.text.pdf.ColumnText(cb)
            ct.SetSimpleColumn(New iTextSharp.text.Phrase(selloDigital, arialNormal), 70, 100, 530, 26, leading, iTextSharp.text.Element.ALIGN_LEFT)
            ct.Go()
            ' Frase Cadena original
            ct = New iTextSharp.text.pdf.ColumnText(cb)
            ct.SetSimpleColumn(New iTextSharp.text.Phrase("Cadena Original:", arialNormal), 70, 70, 530, 26, leading, iTextSharp.text.Element.ALIGN_LEFT)
            ct.Go()
            ' Cadena original
            ct = New iTextSharp.text.pdf.ColumnText(cb)
            ct.SetSimpleColumn(New iTextSharp.text.Phrase(cadenaSello, arialNormal), 70, 55, 530, 13, leading, iTextSharp.text.Element.ALIGN_LEFT)
            ct.Go()


            Dim Page As iTextSharp.text.pdf.PdfImportedPage = writer.GetImportedPage(reader, 1)
            cb.AddTemplate(Page, 0, 0)

            ' Cerrar documentos para que se apliquen los cambios
            document.Close()
            fs.Close()

            writer.Close()
            reader.Close()


            '--------------------------------------------------
            ' Sube a Sharepoint
            '--------------------------------------------------
            Dim encrip As New YourCompany.Utils.Encryption.Encryption64
            Dim objSP As Clases.nsSharePoint.FuncionesSharePoint
            objSP = New Clases.nsSharePoint.FuncionesSharePoint(encrip.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR"), WebConfigurationManager.AppSettings("UsuarioSp"), encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR"), encrip.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR"))
            objSP.Biblioteca = encrip.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")
            objSP.RutaArchivo = Path.GetTempPath()
            objSP.NombreArchivo = archivo

            If Not objSP.UploadFileToSharePoint() Then

                Throw New Exception("No se subió el archivo a Sharepoint")

            End If


            ' actualizamos oficio con el acuse 
            Dim objOficio As New LogicaNegocioSICOD.Entities.BDA_OFICIO
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
            objOficio.ArchivoAcuse = archivo

            If Not BusinessRules.BDA_OFICIO.ActualizarArchivoAcuse(objOficio) Then
                Throw New Exception("Error guardando el archivo")
            End If

            objOficio.FechaAcuse = Now
            If Not BusinessRules.BDA_OFICIO.ActualizarFechaAcuse(objOficio) Then
                Throw New Exception("Error actualizando fecha del acuse, intente de nuevo")
            End If

            If Not BusinessRules.BDA_OFICIO.ActualizarCadenasAcuse(objOficio, cadenaSello, selloDigital) Then
                Throw New Exception("Error actualizando sello digital, intente de nuevo")
            End If


            resultado = True

        Catch ex As Exception

            resultado = False

        End Try

        Return resultado

    End Function

    Private Sub LoadCargoDestinatarios(ByVal EsConsar As Boolean)

        CargarCombo(ddlCargoDestinatario, LogicaNegocioSICOD.BusinessRules.BDA_FUNCION.ConsultarFuncion(EsConsar), "T_FUNCION", "ID_FUNCION")

    End Sub

End Class