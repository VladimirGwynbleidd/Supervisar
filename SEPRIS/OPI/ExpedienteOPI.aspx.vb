Public Class ExpedienteOPI
    Inherits System.Web.UI.Page

    Public ReadOnly Property Folio
        Get
            Return Session("I_ID_OPI")
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        End Get
    End Property
    Public ReadOnly Property Area
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        End Get
    End Property

    Private _opiDetalle As New OPI_Incumplimiento

    Public ReadOnly Property T_FolioOPI
        Get
            Dim OPI As New Registro_OPI
            _opiDetalle = OPI.GetOPIDetail(Folio)
            Return _opiDetalle.T_ID_FOLIO
        End Get
    End Property
    Public ReadOnly Property PasoActual
        Get
            Dim OPI As New Registro_OPI
            _opiDetalle = OPI.GetOPIDetail(Folio)
            Return _opiDetalle.N_ID_PASO
        End Get
    End Property

    Public ReadOnly Property EstatusActual
        Get
            Dim OPI As New Registro_OPI
            _opiDetalle = OPI.GetOPIDetail(Folio)
            Return _opiDetalle.I_ID_ESTATUS
        End Get
    End Property

    Public ReadOnly Property SubPasoActual
        Get
            Dim OPI As New Registro_OPI
            _opiDetalle = OPI.GetOPIDetail(Folio)
            Return _opiDetalle.N_ID_SUBPASO
        End Get
    End Property
    Public ReadOnly Property Clasificacion
        Get
            Dim OPI As New Registro_OPI
            _opiDetalle = OPI.GetOPIDetail(Folio)
            Return _opiDetalle.T_DSC_CLASIFICACION
        End Get
    End Property
    Public Property puObjUsuario As Entities.Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Entities.Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("usuario") = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario

        If Not Page.IsPostBack Then

            CargarFiltros()

            btnActulizarGrid_Click(sender, e)

            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
            Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
            Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
            Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
            Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            'Generales.CargarCombo(ddlPuestoDestinatario, proxySICOD.GetCatalogoCargoDestinatarioOficios(False).ToList(), "Value", "Key")
            Try
                ddlPuestoDestinatario.DataSource = proxySICOD.GetCatalogoCargoDestinatarioOficios(False)
                ddlPuestoDestinatario.DataTextField = "Value"
                ddlPuestoDestinatario.DataValueField = "Key"
                ddlPuestoDestinatario.DataBind()
                ddlPuestoDestinatario.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Catch ex As Exception
                Throw ex
            End Try

            Dim Todas As DataTable = proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo).Tables(0)
            'Dim Filtrado As DataTable = Todas.Select("ID_UNIDAD_ADM IN (" + ObtenerAreasSicod(Me.Area) + ")").CopyToDataTable()
            Generales.CargarCombo(ddlAreaOficio, Todas, "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")
            Generales.CargarCombo(ddlAreaFirma, Todas, "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")
            Generales.CargarCombo(ddlAreaRubrica, Todas, "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")

        End If

    End Sub
    Public Shared Function ObtenerAreasSicod(idAreaSupervisar As String) As String
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE T_DSC_PARAMETRO = 'AREA_" + idAreaSupervisar + "'")
        conexion.CerrarConexion()
        If (data.Rows.Count > 0) Then
            Return data.Rows(0)("T_DSC_VALOR").ToString()
        Else
            Return ""
        End If
    End Function
    Private Sub CargarFiltros()

        ucFiltro1.resetSession()

        ucFiltro1.AddFilter("Paso                ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Documento           ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Documentos Adjuntos ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Núm. Oficio SICOD   ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)

        ucFiltro1.LoadDDL("ExpedienteOPI.aspx")

    End Sub

    Private Sub gvExpedientePC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvExpedientePC.RowDataBound
        Dim blnDesDocto As Boolean = False
        Dim dvDocumentos As New DataView
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim btnAgregarDocumento As ImageButton = CType(e.Row.FindControl("btnAgregarDocumento"), ImageButton)
            Dim btnReemplazarDocumento As ImageButton = CType(e.Row.FindControl("btnReemplazarDocumento"), ImageButton)
            Dim btnRegistroSICOD As Button = CType(e.Row.FindControl("btnRegistroSICOD"), Button)
            Dim btnBuscarSICOD As Button = CType(e.Row.FindControl("btnBuscarSICOD"), Button)

            btnAgregarDocumento.OnClientClick = "SubirArchivo(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", '" + e.Row.DataItem("T_PREFIJO").ToString() + "', '" + T_FolioOPI + "'); return false;"
            btnReemplazarDocumento.OnClientClick = "ReemplazarArchivo(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + "); return false;"
            'btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", -1, '" + "1" + "'); return false;"
            btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", -1, '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "'); return false;"
            Session("FolioSicod") = String.Empty

            'If Not e.Row.DataItem("B_REG_SICOD") Then
            '    btnRegistroSICOD.Visible = False
            '    btnAgregarDocumento.Visible = True
            '    'btnEliminarDocumento.Visible = False
            'Else
            '    btnRegistroSICOD.Visible = True
            'End If

            'Metodo para ocultar o mostrar los botones del grid de expediente
            'If (IIf(IsDBNull(e.Row.DataItem("B_BUSCAR_SICOD")), False, e.Row.DataItem("B_BUSCAR_SICOD")) AndAlso
            '    (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
            '    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS OrElse
            '    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP)) Then

            'btnBuscarSICOD.Visible = True
            'btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", " + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "," + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + "," + Folio + ")"
            'btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", -1, " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "')"

            'Else
            'btnBuscarSICOD.Visible = False
            'End If



            'Programa de Corrección
            'If e.Row.DataItem("I_PASO_INICIAL").ToString() = 1 And e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 1 Then
            '    btnRegistroSICOD.Visible = False
            '    btnAgregarDocumento.Visible = False
            '    btnReemplazarDocumento.Visible = False
            'End If

            'If (e.Row.DataItem("I_PASO_INICIAL").ToString() = 10 And
            'e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 And
            '_opiDetalle.B_POSIBLE_INC = True And
            '(puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS OrElse
            '    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM)) Then
            '    btnRegistroSICOD.Visible = True
            '    btnBuscarSICOD.Visible = True
            '    'btnEliminarDocumento.Visible = False
            'End If

            'If (e.Row.DataItem("I_PASO_INICIAL").ToString() = 10 And
            'e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 And
            '_opiDetalle.B_POSIBLE_INC = True And
            'e.Row.DataItem("I_ID_ESTATUS").ToString() <> 27 And
            'e.Row.DataItem("I_ID_ESTATUS").ToString() <> 28 And
            'e.Row.DataItem("I_ID_ESTATUS").ToString() <> 43 And
            'e.Row.DataItem("I_ID_ESTATUS").ToString() <> 44 And
            '(puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS OrElse
            '    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM)) Then
            '    btnRegistroSICOD.Visible = True
            '    btnBuscarSICOD.Visible = True
            '    'btnEliminarDocumento.Visible = False
            'End If


            'If (e.Row.DataItem("I_PASO_INICIAL").ToString() = 10 And e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 And _opiDetalle.B_POSIBLE_INC = True) Then
            '    btnRegistroSICOD.Visible = False
            '    btnAgregarDocumento.Visible = False
            '    'btnEliminarDocumento.Visible = False
            'End If


            If e.Row.DataItem("I_PASO_INICIAL").ToString() <> PasoActual And e.Row.DataItem("I_PASO_FINAL").ToString() <> PasoActual Then

                Dim dtArchivos As DataTable = Entities.DocumentoOPI.ObtenerArchivos(Folio, e.Row.DataItem("I_ID_DOCUMENTO").ToString())
                Dim tablaArchivos As New Table
                Dim tablaOficios As New Table

                Dim rowOficio As New TableRow
                Dim cellOficio As New TableCell


                btnRegistroSICOD.Visible = False
                btnAgregarDocumento.Visible = False
                btnReemplazarDocumento.Visible = False
                btnBuscarSICOD.Visible = False
                For Each archivo As DataRow In dtArchivos.Rows

                    If (archivo("T_DSC_NOMBRE_DOCUMENTO").ToString() <> "" Or archivo("T_FOLIO_SICOD").ToString() <> "") Then
                        Dim rowArchivo As New TableRow
                        Dim cellArchivo As New TableCell

                        Dim linkArchivo As New LinkButton

                        If (archivo("T_DSC_NOMBRE_DOCUMENTO").ToString() <> "") Then
                            linkArchivo.Text = archivo("T_DSC_NOMBRE_DOCUMENTO")
                            linkArchivo.OnClientClick = "__doPostBack('" + Button1.UniqueID + "', '" + archivo("T_DSC_NOMBRE_DOCUMENTO") + "'); return false;"
                            cellArchivo.Controls.Add(linkArchivo)
                            rowArchivo.Cells.Add(cellArchivo)
                            tablaArchivos.Rows.Add(rowArchivo)
                            'btnReemplazarDocumento.Visible = True
                        End If

                        If (archivo("T_FOLIO_SICOD").ToString() <> "") Then
                            Dim linkFolioSICOD As New LinkButton
                            linkFolioSICOD.Text = archivo("T_FOLIO_SICOD").ToString()
                            linkFolioSICOD.OnClientClick = "ConsultarOficios('" + archivo("T_FOLIO_SICOD").ToString().Replace("/", "-") + "'); return false;"
                            cellOficio.Controls.Add(linkFolioSICOD)
                            rowOficio.Cells.Add(cellOficio)
                            tablaOficios.Rows.Add(rowOficio)
                            'btnReemplazarDocumento.Visible = False
                        End If

                        btnRegistroSICOD.Visible = False
                        btnAgregarDocumento.Visible = False

                        btnBuscarSICOD.Visible = False


                    End If
                    e.Row.Cells(3).Controls.Add(tablaArchivos)
                    e.Row.Cells(6).Controls.Add(tablaOficios)
                Next
            Else
                Dim dtArchivos As DataTable = Entities.DocumentoOPI.ObtenerArchivos(Folio, e.Row.DataItem("I_ID_DOCUMENTO").ToString())


                If dtArchivos.Rows.Count > 0 Then

                    Dim tablaArchivos As New Table
                    Dim tablaOficios As New Table

                    For Each archivo As DataRow In dtArchivos.Rows

                        If (archivo("T_DSC_NOMBRE_DOCUMENTO").ToString() <> "") Then
                            Dim rowArchivo As New TableRow
                            Dim cellArchivo As New TableCell

                            Dim linkArchivo As New LinkButton

                            If (archivo("T_DSC_NOMBRE_DOCUMENTO").ToString() <> "") Then
                                Session("NombreDocumento") = archivo("T_DSC_NOMBRE_DOCUMENTO").ToString()
                                linkArchivo.Text = archivo("T_DSC_NOMBRE_DOCUMENTO")
                                linkArchivo.OnClientClick = "__doPostBack('" + Button1.UniqueID + "', '" + archivo("T_DSC_NOMBRE_DOCUMENTO") + "'); return false;"
                                cellArchivo.Controls.Add(linkArchivo)
                                rowArchivo.Cells.Add(cellArchivo)
                                tablaArchivos.Rows.Add(rowArchivo)
                                btnReemplazarDocumento.Visible = True
                                btnAgregarDocumento.Visible = False
                            End If



                            btnRegistroSICOD.Visible = False
                            btnAgregarDocumento.Visible = False

                            btnBuscarSICOD.Visible = False


                        Else

                            If Not e.Row.DataItem("B_REG_SICOD") Then
                                btnRegistroSICOD.Visible = False
                            Else
                                btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + archivo("I_ID").ToString() + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "'); return false;"
                                btnRegistroSICOD.Visible = True
                            End If



                            If Not e.Row.DataItem("B_BUSCAR_SICOD") Then
                                btnBuscarSICOD.Visible = False
                            Else
                                btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", " + archivo("I_ID").ToString() + ", " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "')"
                                btnBuscarSICOD.Visible = True
                            End If
                            btnReemplazarDocumento.Visible = False
                            'btnAgregarDocumento.Visible = True

                        End If


                        'si hay documento, no se debe permitir cargar uno nuevo
                        If EstatusActual = Constantes.ESTATUS Or EstatusActual = Constantes.CANCELAR Then
                            btnReemplazarDocumento.Visible = False
                        Else
                            'btnReemplazarDocumento.Visible = True
                        End If

                        'btnAgregarDocumento.Visible = False
                        'habilitar el boton de registro 
                        'Validar si se debe mostrar botón de registrar







                        If (archivo("T_FOLIO_SICOD").ToString() <> "") Then
                            Dim rowOficio As New TableRow
                            Dim cellOficio As New TableCell


                            If (archivo("T_FOLIO_SICOD").ToString() <> "") Then
                                Session("FolioSicod") = archivo("T_FOLIO_SICOD").ToString()
                                Dim linkFolioSICOD As New LinkButton
                                linkFolioSICOD.Text = archivo("T_FOLIO_SICOD").ToString()
                                linkFolioSICOD.OnClientClick = "ConsultarOficios('" + archivo("T_FOLIO_SICOD").ToString().Replace("/", "-") + "'); return false;"
                                cellOficio.Controls.Add(linkFolioSICOD)
                                rowOficio.Cells.Add(cellOficio)
                                tablaOficios.Rows.Add(rowOficio)
                                btnRegistroSICOD.Visible = False
                                btnBuscarSICOD.Visible = False
                            End If





                            btnRegistroSICOD.Visible = False
                            'btnAgregarDocumento.Visible = False
                            btnBuscarSICOD.Visible = False



                            'If ConexionSICOD.FolioFinalizado(archivo("T_FOLIO_SICOD").ToString()) Then
                            '    'Si el folio esta finalizado se pertimete ingresar otro archivo
                            '    btnAgregarDocumento.Visible = True
                            'End If
                        Else
                            If (Session("FolioSicod").ToString() Is Nothing) Then
                                If Not e.Row.DataItem("B_REG_SICOD") Then
                                    btnRegistroSICOD.Visible = False
                                Else
                                    btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + archivo("I_ID").ToString() + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "'); return false;"
                                    btnRegistroSICOD.Visible = True
                                End If



                                If Not e.Row.DataItem("B_BUSCAR_SICOD") Then
                                    btnBuscarSICOD.Visible = False
                                Else
                                    btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", " + archivo("I_ID").ToString() + ", " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "')"
                                    btnBuscarSICOD.Visible = True
                                End If

                            End If

                        End If




                        If ((puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS OrElse
                        puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                        puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) And
                    (e.Row.DataItem("I_PASO_INICIAL").ToString() = PasoActual)) Then


                            If ((archivo("T_DSC_NOMBRE_DOCUMENTO") = "") And (archivo("T_FOLIO_SICOD").ToString() = "")) Then

                                If (e.Row.DataItem("T_NOM_DOCUMENTO").ToString = Clasificacion) Then
                                    btnRegistroSICOD.Visible = True
                                    btnAgregarDocumento.Visible = True
                                    btnBuscarSICOD.Visible = True
                                End If
                            End If



                        End If


                        'If ((puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) AndAlso (e.Row.DataItem("I_PASO_INICIAL").ToString() = PasoActual)) Then

                        '    If (e.Row.DataItem("T_NOM_DOCUMENTO").ToString = Clasificacion) Then
                        '        btnRegistroSICOD.Visible = True
                        '        btnAgregarDocumento.Visible = True
                        '        btnBuscarSICOD.Visible = True
                        '    End If

                        'End If


                    Next

                    e.Row.Cells(3).Controls.Add(tablaArchivos)
                    e.Row.Cells(6).Controls.Add(tablaOficios)
                Else
                    btnRegistroSICOD.Visible = True
                    btnAgregarDocumento.Visible = True
                    btnBuscarSICOD.Visible = True
                    btnReemplazarDocumento.Visible = False

                    If Not e.Row.DataItem("B_REG_SICOD") Then
                        btnRegistroSICOD.Visible = False
                    Else
                        btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", -1, '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "'); return false;"
                        btnRegistroSICOD.Visible = True
                    End If



                    If Not e.Row.DataItem("B_BUSCAR_SICOD") Then
                        btnBuscarSICOD.Visible = False
                    Else
                        btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", -1, " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "')"
                        btnBuscarSICOD.Visible = True
                    End If

                End If

                If InStr(e.Row.DataItem("T_DSC_ESTATUS").ToString, "|" & EstatusActual & "|") = 0 Then
                    'btnRegistroSICOD.Visible = False
                    'btnAgregarDocumento.Visible = False
                    'btnBuscarSICOD.Visible = False

                    'btnEliminarDocumento.Visible = False
                    '                Else
                    '                   btnEliminarDocumento.Visible = False
                End If
            End If








            Dim liNumDoc As Integer = 0
            liNumDoc = CInt(DataBinder.Eval(e.Row.DataItem, "I_ID_DOCUMENTO"))


        End If

    End Sub
    Protected Sub btnActulizarGrid_Click(sender As Object, e As EventArgs) Handles ucFiltro1.Filtrar
        Dim dtDocumentos As DataTable = Entities.DocumentoOPI.ObtenerTodos
        Dim dvDocumentos As DataView = dtDocumentos.DefaultView

        gvExpedientePC.DataSource = dvDocumentos
        gvExpedientePC.DataBind()

        If Not hfFolioSICOD.Value = String.Empty Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Muestra folio", "ConsultarOficios('" + hfFolioSICOD.Value + "');", True)
            hfFolioSICOD.Value = String.Empty
        End If
    End Sub
    Public Function CumpleReq() As Boolean


    End Function

    Public Function ExpedienteValido(Optional ByVal SubPaso As Integer = 0) As Boolean

        Dim valido As Boolean = True
        Dim dvDocumentos As New DataView
        Dim dvDocumentos2 As New DataView

        Select Case PasoActual

            Case 2
                Select Case EstatusActual
                    Case 4
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 1).DefaultView
                    Case 45
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 2).DefaultView
                    Case 8
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 3).DefaultView
                    Case 9
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 4).DefaultView
                End Select
            Case 3

                Select Case EstatusActual
                    Case 8
                        If SubPaso = 1 Then
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 3).DefaultView
                        ElseIf SubPaso = 2 Then
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 5).DefaultView
                            'ammm ODT08 alcance a respuesta
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 16).DefaultView
                        End If

                    Case 9
                        'Se trae del paso 4.
                        If SubPaso = 2 Then '16082019 ammm se agrega condicion para diferenciar cuando ya se tiene respuesta
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 6).DefaultView
                            'dvDocumentos = Entities.DocumentoOPI.ObtenerVariosArchivos(Folio, "5,6").DefaultView
                            'ammm ODT08 alcance a respuesta
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 16).DefaultView
                        Else
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 3).DefaultView
                        End If
                    'ammm ODT08
                    Case 10
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 16).DefaultView

                End Select

            Case 4
                Select Case EstatusActual
                    Case 12
                        'Se cambio a paso 3
                        'Select Case SubPaso
                        '    Case 1
                        '        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 5).DefaultView
                        '    Case 2
                        '        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 6).DefaultView
                        'End Select
                        'ammm ODT08 alcance a respuesta
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 16).DefaultView
                        Return True
                End Select

            Case 7
                Select Case EstatusActual
                    Case 4, 19, 20, 21
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 7).DefaultView
                    Case 9, 23, 46
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 10).DefaultView
                End Select
            Case 8
                Select Case EstatusActual
                    Case 9
                        If SubPaso = 2 Then 'AMMM 06082019 SE AGREGA ESTA CONDICIÓN con prórroga
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 15).DefaultView
                            'ammm ODT08
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 17).DefaultView
                        Else
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 8).DefaultView
                        End If

                    Case 23
                        If SubPaso = 2 Then
                            dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 9).DefaultView
                        Else
                            If SubPaso = 1 Then
                                dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 8).DefaultView
                            End If
                        End If
                    Case Else
                        'ammm ODT08
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 17).DefaultView
                End Select
            Case 10
                Select Case EstatusActual
                    'Case 26, 31
                    '    dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 11).DefaultView
                    'Se comenta para validar que cuando no procede no se hailite documneto.
                    'Case 27
                    '    dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 11).DefaultView
                    'Se comentan esta lineas para que no valide que el documento sea obligatorio
                    'If (_opiDetalle.B_POSIBLE_INC = False) Then
                    '    If (Entities.DocumentoOPI.ObtenerVariosArchivos(Folio, "11,12").Rows.Count < 2) Then
                    '        valido = False
                    '    Else
                    '        dvDocumentos = Entities.DocumentoOPI.ObtenerVariosArchivos(Folio, "11,12").DefaultView
                    '    End If
                    'End If
                    Case 42
                        dvDocumentos = Entities.DocumentoOPI.ObtenerArchivos(Folio, 13).DefaultView
                End Select
            Case Else
                valido = True
        End Select

        If dvDocumentos.Count = 0 Or dvDocumentos Is Nothing Then
            If PasoActual = 8 And SubPaso = 3 Or PasoActual = 3 And SubPaso = 3 Or PasoActual = 10 And SubPaso = 0 Then
                valido = True
            Else
                'No se cargado ningun archivo
                valido = False
            End If
        Else

            For Each documento As DataRowView In dvDocumentos
                If (Entities.DocumentoOPI.ValidaObligatorioSICOD(documento("I_ID_DOCUMENTO"))) Then
                    If documento("T_FOLIO_SICOD").ToString() <> String.Empty OrElse documento("T_DSC_NOMBRE_DOCUMENTO").ToString() <> String.Empty Then
                        If documento("T_FOLIO_SICOD").ToString() <> String.Empty Then
                            If ConexionSICOD.FolioFinalizado(documento("T_FOLIO_SICOD")) Then
                                'OK

                            Else
                                'Tiene archivo y folio pero no esta finalizado
                                valido = False
                                Exit For
                            End If

                        End If
                    Else
                        'Tiene archivo pero no folio sicod
                        valido = False
                        Exit For
                    End If
                End If
            Next
        End If


        Return valido

    End Function


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim usuario As String
        Dim passwd As String
        Dim dominio As String
        Dim nom_archivo As String = String.Empty
        Dim biblioteca As String
        Dim ServSharepoint As String
        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim enc As New YourCompany.Utils.Encryption.Encryption64


        nom_archivo = Request("__EVENTARGUMENT")

        If nom_archivo <> "Sin imagen" Then
            Dim Archivo() As Byte = Nothing
            Dim filename As String = "attachment; filename=" & nom_archivo

            Try
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointUserSEPRIS")
                passwd = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS"))
                ServSharepoint = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerSEPRIS")
                dominio = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointDomainSEPRIS")
                biblioteca = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS-PC-OPI")

                cliente.Credentials = New System.Net.NetworkCredential(usuario, passwd, dominio)

                Dim Url As String = ServSharepoint & "/" & biblioteca & "/" & nom_archivo

                Archivo = cliente.DownloadData(ResolveUrl(Url))

            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento("No se puede abrir el documento: " & nom_archivo, EventLogEntryType.Error)
            End Try

            If Not Archivo Is Nothing Then

                Dim dotPosicion As Integer = nom_archivo.LastIndexOf(".")

                Dim tipo_arch As String = nom_archivo.Substring(dotPosicion + 1)

                Select Case tipo_arch
                    Case "zip"
                        Response.ContentType = "application/x-zip-compressed"
                    Case "pdf"
                        Response.ContentType = "application/pdf"
                    Case "csv"
                        Response.ContentType = "application/csv"
                    Case "doc"
                        Response.ContentType = "application/doc"
                    Case "docx"
                        Response.ContentType = "application/docx"
                    Case "xls"
                        Response.ContentType = "application/xls"
                    Case "xlsx"
                        Response.ContentType = "application/xlsx"
                    Case "png"
                        Response.ContentType = "application/png"
                    Case "gif"
                        Response.ContentType = "application/gif"
                    Case "jpg"
                        Response.ContentType = "application/jpg"
                    Case "csv"
                        Response.ContentType = "application/csv"
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
                End Select

                Response.AddHeader("content-disposition", filename)

                Response.BinaryWrite(Archivo)

                Response.End()
            End If

        End If

        btnActulizarGrid_Click(sender, e)
    End Sub
End Class