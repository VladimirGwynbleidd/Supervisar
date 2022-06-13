Imports Clases
Imports System.Diagnostics
Imports System.IO
Imports System.IO.StreamWriter
Imports System.Net
Imports System.Web.Configuration

Public Class BandejaOficios

    Inherits System.Web.UI.Page

    Private Const ConsultaText As String = "Consultar Archivos..."
    Private Property ObjProps As PropsModal
        Get
            If IsNothing(Session("ObjProps")) Then
                Return Nothing
            Else
                Return CType(Session("ObjProps"), PropsModal)
            End If
        End Get
        Set(ByVal value As PropsModal)
            Session("ObjProps") = value
        End Set
    End Property

    Property TotalRegistros As String
        Get
            Return ViewState("TotalRegistrosConsulta").ToString()
        End Get
        Set(ByVal value As String)
            ViewState.Add("TotalRegistrosConsulta", value)
        End Set
    End Property

    Property UsuariosAsistente As String
        Get
            Return ViewState("UsuariosAsistente").ToString()
        End Get
        Set(ByVal value As String)
            ViewState.Add("UsuariosAsistente", value)
        End Set
    End Property

    Public Property ISEXPEDIENTE() As Boolean
        Get
            Return CBool(ViewState("ISEXPEDIENTE"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("ISEXPEDIENTE") = value
        End Set
    End Property


#Region "Consulta principal de la pagina"

    'Indica el Total de Registros de la Consulta
    ' Private SqlTotalDatosInicio As String = "SELECT  Count(*) TotalRegistros FROM (  "
    ' Private SqlTotalDatosFin As String = " )  as UnionedResultTotal  "

    'Indica el Tope de la Consulta
    ' Private SqlTopInicio As String = "SELECT TOP 20 * FROM (  "
    ' Private SqlTopFin As String = " )  as UnionedResult ORDER BY [ID_FOLIO], [ORIGINAL_FLAG] DESC, [ID_G_TURNADO], [FECH_INICIO] "

    'Private OrderBy As String = " ORDER BY A.ID_FOLIO"
    Private Const AbrirConexion As String = "BÚSQUEDA"
    Private Const Modulo As String = "BANDEJA DE ENTRADA"

    ' Variables para la generacion de EventLog
    Private TryCatchEventProceso As String = ConfigurationSettings.AppSettings("EventLogSource")
    Private TryCatchEventSitio As String = ConfigurationSettings.AppSettings("EventLogSitio")

    Private Const TituloDeForma As String = "BANDEJA DE ENTRADA"
    Protected WithEvents txtFolio As System.Web.UI.WebControls.TextBox
    'Private OrigenLayouts As String = String.Empty
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Sesion As New Seguridad
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        Try
            If Not IsPostBack Then
                Dim Comentario As String = "Para consultar un registro hacer Doble Click sobre el registro"
                Me.GridPrincipal.ToolTip = Comentario

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Archivo", "setTimeout('CallClickINICIO()', 300);", True)
                Dim objPropsModal As New PropsModal

                If Not IsNothing(Request.QueryString("r")) Then
                    objPropsModal.idRenGrid = Request.QueryString("r").ToString()
                    objPropsModal.idDocumento = Request.QueryString("ds").ToString()
                    objPropsModal.idVersion = Request.QueryString("vds").ToString()
                End If

                If IsNothing(Request.QueryString("idCat")) Then
                    objPropsModal.idCatDocumento = "1"
                Else
                    objPropsModal.idCatDocumento = Request.QueryString("idCat").ToString()
                End If

                If IsNothing(Request.QueryString("idArea")) Then
                    objPropsModal.idArea = "35"
                    Filtros.AgregaFiltro("Area")
                Else
                    objPropsModal.idArea = Request.QueryString("idArea").ToString()
                    Filtros.AgregaFiltro("Area")
                End If

                If IsNothing(Request.QueryString("tipo")) Then
                    objPropsModal.tipo = "1"
                    Filtros.AgregaFiltro("Tipo de Documento")
                Else
                    objPropsModal.tipo = Request.QueryString("tipo").ToString()
                    If objPropsModal.tipo = "undefined" Then
                        objPropsModal.tipo = "1"
                        Filtros.AgregaFiltro("Tipo de Documento")

                    End If
                    Filtros.AgregaFiltro("Tipo de Documento")
                End If
                If IsNothing(Request.QueryString("idFolio")) Then
                    objPropsModal.idFolio = ""
                Else
                    objPropsModal.idFolio = Request.QueryString("idFolio").ToString()
                    If objPropsModal.idFolio = "undefined" Then
                        objPropsModal.idFolio = ""
                    End If
                End If
                If IsNothing(Request.QueryString("paso")) Then
                    objPropsModal.paso = 1
                Else
                    objPropsModal.paso = Request.QueryString("paso").ToString()
                    If objPropsModal.paso = "undefined" Then
                        objPropsModal.paso = 1
                    End If
                End If

                If IsNothing(Request.QueryString("origen")) Then
                    objPropsModal.origen = "VISITA"
                Else
                    objPropsModal.origen = Request.QueryString("origen").ToString()
                    If objPropsModal.origen = "undefined" Then
                        objPropsModal.origen = "VISITA"
                    End If
                End If
                ObjProps = objPropsModal
                Filtros.LlenaDDLs("Area")
                Filtros.LlenaDDLs("Tipo de Documento")
            End If
        Catch ex As Exception
            catch_cone(ex, "Page_Load")
        Finally
            Sesion.CerrarCon()
            Sesion = Nothing
        End Try
    End Sub
    ''TootipPreuba
    Protected Sub CargaGRID_click(ByVal sender As Object, ByVal e As EventArgs)
        'ActualizaGrid(True)
        'Imagen_procesando.Style.Add("display", "none")
        'GRID.Style.Add("visibility", "visible")
    End Sub

    Public Sub ActualizaGrid(ByVal usarFiltro As Boolean)
        Dim usuario As String = Session("Usuario")
        Dim Con As Clases.Conexion = Nothing
        Dim Sesion As Seguridad = Nothing
        Dim ds As New DataSet
        Dim dsA As New DataSet
        Dim liARCH As List(Of ARCHOfi) = New List(Of ARCHOfi)()
        Dim y As ULong = 0
        Dim sqlGrid As String = String.Empty

      sqlGrid = "SELECT TOP 50 A.DSC_UNIDAD_ADM " +
      " ,A.ID_UNIDAD_ADM " +
      " ,O.T_OFICIO_NUMERO " +
      " ,O.ID_AREA_OFICIO " +
      " ,O.ID_ANIO " +
      " ,O.I_OFICIO_CONSECUTIVO " +
      " ,O.T_ASUNTO " +
      " ,O.ID_ENTIDAD " +
      " ,O.I_PLAZO_DIAS " +
      " ,O.ID_OFICIO " +
      " ,UE.USUARIO AS ELABORO " +
      " ,UA.USUARIO AS REGISTRO " +
      " ,T_TIPO_DOCUMENTO " +
      " ,(CASE WHEN ISNULL(P.DESTINATARIO, '') <> '' THEN P.DESTINATARIO ELSE (CASE WHEN O.T_DESTINATARIO IS NOT NULL THEN O.T_DESTINATARIO " +
      " ELSE ISNULL(F.T_FUNCION, '') END) END) DESTINATARIO " +
      " ,E.T_ESTATUS " +
      " ,O.ID_ESTATUS " +
      " ,CONVERT(VARCHAR(10),O.F_FECHA_NOTIFICACION,103) AS F_FECHA_NOTIFICACION" +
      " ,CONVERT(VARCHAR(10), O.F_FECHA_RECEPCION,103) AS F_FECHA_RECEPCION " +
      " ,CONVERT(VARCHAR(10), O.F_FECHA_OFICIO,103) AS F_FECHA_OFICIO " +
      " ,CONVERT(VARCHAR(10), O.F_FECHA_ACUSE,103) AS F_FECHA_ACUSE " +
      " ,CONVERT(VARCHAR(10), O.F_FECHA_VENCIMIENTO,103) AS F_FECHA_VENCIMIENTO " +
      " ,O.DICTAMINADO_FLAG " +
      " ,O.NOTIF_ELECTRONICA_FLAG " +
       " ,'' AS T_INICIALES " +
      " ,'' AS T_ENTIDAD_CORTO" +
      " ,O.ID_ENTIDAD_TIPO" +
      " ,'' AS T_LOGO " +
      " ,O.I_OFICIO_CONSECUTIVO " +
      " ,O.ID_ANIO " +
      " ,CL.ID_CLASIFICACION " +
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
      " FROM BDA_OFICIO O " +
      "" +
      " INNER JOIN BDA_C_UNIDAD_ADM A ON A.ID_UNIDAD_ADM = O.ID_AREA_OFICIO AND A.ID_T_UNIDAD_ADM = 1 " +
      " LEFT OUTER JOIN BDS_USUARIO UE ON UE.USUARIO = O.USUARIO_ELABORO " +
      " LEFT OUTER JOIN BDS_USUARIO UA ON UA.USUARIO = O.USUARIO_ALTA " +
      " LEFT OUTER JOIN BDA_TIPO_DOCUMENTO T ON T.ID_TIPO_DOCUMENTO = O.ID_TIPO_DOCUMENTO " +
      " LEFT OUTER JOIN (SELECT ISNULL(XP.T_NOMBRE,'')  + ' ' + ISNULL(XP.T_APELLIDO_P,'') + ' ' + ISNULL(XP.T_APELLIDO_M,'') AS DESTINATARIO, XP.ID_PERSONA " +
      " FROM BDA_PERSONAL XP) P ON P.ID_PERSONA=O.ID_DESTINATARIO " +
      " LEFT OUTER JOIN BDA_ESTATUS_OFICIO E ON E.ID_ESTATUS = O.ID_ESTATUS " +
      " LEFT OUTER JOIN BDA_CLASIFICACION_OFICIO CL ON CL.ID_CLASIFICACION = O.ID_CLASIFICACION " +
      " LEFT OUTER JOIN BDA_FUNCION F ON O.ID_PUESTO_DESTINATARIO = f.ID_FUNCION " +
      " WHERE O.ID_ESTATUS = 6 "

      sqlGrid &= Filtros.GetWhereQuery(ObjProps.idArea, ObjProps.tipo)
        sqlGrid &= " order by O.F_FECHA_OFICIO desc"

        Try
            Con = New Clases.Conexion()
            Sesion = New Seguridad
            ds = Con.Datos(sqlGrid, True)
            Session("ConsultaPrincipalOficios") = ds

            GridPrincipal.CurrentPageIndex = 0
            GridPrincipal.SelectedIndex = -1
            GridPrincipal.DataSource = ds.Tables(0).DefaultView
            GridPrincipal.DataBind()

            If GridPrincipal.Items.Count = 0 Then
                GridPrincipal.Visible = False
            Else
                GridPrincipal.Visible = True
            End If
        Catch ex As Exception
            catch_cone(ex, "ActualizaGrid")

        Finally
            If Not Con Is Nothing Then
                Con.Cerrar()
            End If
            Con = Nothing
            Session("tusaurio") = Nothing
            Session("Recibido") = Nothing
            Session("fechaRegistro") = Nothing
            Session("fechaLimite") = Nothing
        End Try

        Imagen_procesando.Style.Add("display", "none")
        GRID.Style.Add("visibility", "visible")
        Boton_guardar.Style.Add("visibility", "visible")

    End Sub

    Private Sub AgregarColumnasAnexo(ByVal dsConsulta As DataSet)
        Dim i As ULong = 0
        Dim x As ULong = 0
        Dim Con As Clases.Conexion = New Clases.Conexion()
        dsConsulta.Tables(0).Columns.Add(New DataColumn("ANEXO", System.Type.GetType("System.String")))
        dsConsulta.Tables(0).Columns.Add(New DataColumn("NOM_ARCH", System.Type.GetType("System.String")))
        dsConsulta.Tables(0).Columns.Add(New DataColumn("NOMBRE_ARCHIVO", System.Type.GetType("System.String")))
        dsConsulta.Tables(0).Columns.Add(New DataColumn("ID_ARCHIVO", System.Type.GetType("System.String")))
        For Each row As DataRow In dsConsulta.Tables(0).Rows
            If Con.BusquedaReg("BDA_R_DOC_ANEXO", "ID_FOLIO = " & row("ID_FOLIO").ToString().Trim()) Then
                row("ANEXO") = "Si"
            Else
                row("ANEXO") = "No"
            End If
        Next
        dsConsulta.AcceptChanges()
    End Sub
    Private Sub RowsStyle()

        If GridPrincipal.Items.Count > 0 Then
            Dim Folio As String = String.Empty
            Dim EsCopia As String = String.Empty
            Dim chkRecibido As CheckBox
            Dim chkAtendido As CheckBox
            Dim lblCorreosSIE As New Label
            Dim lnkArchivo As New LinkButton

            Dim tDocto As String
            Dim cont1 As Integer = 0
            For Each _dataItem As DataGridItem In GridPrincipal.Items
                Folio = String.Empty
                EsCopia = String.Empty
                tDocto = String.Empty
                Dim nomArch As String = String.Empty
                Dim idArch As ULong = 0
                Dim usuarioDoc As String = String.Empty

                lblCorreosSIE = CType(_dataItem.FindControl("lblCorreosSIE"), Label)
                lnkArchivo = CType(_dataItem.FindControl("lnkArchivo"), LinkButton)

                If lblCorreosSIE.Text = "No" Then
                    _dataItem.Attributes.Add("onMouseOver", "cambio(this);")
                Else
                    _dataItem.Attributes.Add("onMouseOver", "cambioSIE(this);")
                End If

                _dataItem.Attributes.Add("onMouseOut", "cambio2(this);")

                _dataItem.Attributes.Add("class", "hotspot")

                If lblCorreosSIE.Text = "Si" Then

                    _dataItem.Attributes.Add("class", "tr_odd2")

                End If

                Folio = CType(_dataItem.Cells(1).Controls(0), LinkButton).Text
                EsCopia = _dataItem.Cells(10).Text
                If _dataItem.Cells(13).Text <> "&nbsp;" Then
                    nomArch = _dataItem.Cells(13).Text
                End If
                If _dataItem.Cells(14).Text <> "&nbsp;" Then
                    idArch = Convert.ToUInt64(_dataItem.Cells(14).Text)
                End If
                tDocto = _dataItem.Cells(5).Text

                usuarioDoc = _dataItem.Cells(20).Text

                If Not Folio = String.Empty Then
                    If lblCorreosSIE.Text = "Si" Then
                        If lnkArchivo.Text <> "" Then
                            _dataItem.Attributes.Add("onDblClick", "window.parent.frames[3].window.location='Modifica.aspx?NumFolio=" & HttpUtility.UrlEncode(Folio.ToString()) & "&nombreArch= " & HttpUtility.UrlEncode(nomArch.ToString()) & "&IdArch=" & HttpUtility.UrlEncode(idArch) & "&tDocto=" & HttpUtility.UrlDecode(tDocto) & "&EsCopia=" & HttpUtility.UrlDecode(EsCopia) & "&Usuario=" & HttpUtility.UrlDecode(usuarioDoc) & " '")
                        End If
                    Else
                        _dataItem.Attributes.Add("onDblClick", "window.parent.frames[3].window.location='Modifica.aspx?NumFolio=" & HttpUtility.UrlEncode(Folio.ToString()) & "&nombreArch= " & HttpUtility.UrlEncode(nomArch.ToString()) & "&IdArch=" & HttpUtility.UrlEncode(idArch) & "&tDocto=" & HttpUtility.UrlDecode(tDocto) & "&EsCopia=" & HttpUtility.UrlDecode(EsCopia) & "&Usuario=" & HttpUtility.UrlDecode(usuarioDoc) & " '")
                    End If
                End If

                chkRecibido = _dataItem.FindControl("chkRecibido")
                chkRecibido.Attributes.Add("OnClick", "javascript:selRec(" & _dataItem.Cells(11).Text & "," & chkRecibido.ClientID & ");")
                chkAtendido = _dataItem.FindControl("chkAtendido")
                chkAtendido.Attributes.Add("OnClick", "javascript:selRec(" & _dataItem.Cells(15).Text & "," & chkAtendido.ClientID & ");")

                If _dataItem.Cells(10).Text = "C" Then 'A petición de Israel las copias también serán modificables en estos campos y tendrán sus propios campos en la tabla de copias 20110928. AS
                    cont1 += 1 'Para mostrar rel botón Guardar cuando haya sólo copias
                Else
                    cont1 += 1
                End If

                If _dataItem.Cells(11).Text = "1" Then
                    chkRecibido.Checked = True
                End If
                If _dataItem.Cells(15).Text = "1" Then
                    chkAtendido.Checked = True
                End If

            Next
        End If

    End Sub
    Private Sub Filtros_Filtrar() Handles Filtros.BtnFiltrarClick
        Try
            Session("ListaPanelVisibles") = Nothing

            lblErrores.Text = String.Empty
            lblFecDoc.Text = String.Empty
            lblFecRec.Text = String.Empty
            lblTDoc.Text = String.Empty
            lblArea.Text = String.Empty
            lblDest.Text = String.Empty
            lblRec.Text = String.Empty
            lblRefere.Text = String.Empty
            lblRegistros.Text = String.Empty

            lblFolio.Text = String.Empty
            lblOficio.Text = String.Empty
            lblRemitente.Text = String.Empty
            lblAtendidaStatus.Text = String.Empty
            lblAsunto.Text = String.Empty
            lblResponsable.Text = String.Empty
            lblFechaRegistro.Text = String.Empty
            lblFechaLimite.Text = String.Empty
            lblProvieneSIE.Text = String.Empty

            If Filtros.GetWhereQuery().StartsWith("-1,") Then
                EventLogWriter.EscribeEntrada("Error al consultar " & Filtros.GetWhereQuery().Substring(3).ToString(), EventLogEntryType.Error)
            Else
                If Session("filtros") = "" Then

                    hdnChecks.Value = ""
                    Session.Remove("ConsultaPrincipal")

                    ActualizaGrid(True)

                    If GridPrincipal.Items.Count < 1 Then
                        lblErrores.Text = String.Empty
                        btnGuardar.Visible = False
                        lblFecDoc.Text = String.Empty
                        lblFecRec.Text = String.Empty
                        lblTDoc.Text = String.Empty
                        lblArea.Text = String.Empty
                        lblDest.Text = String.Empty
                        lblRec.Text = String.Empty
                        lblRefere.Text = String.Empty
                        lblRegistros.Text = String.Empty
                        lblErroresTitulo.Text = "Alerta"
                        BtnModalOk.Style.Add("display", "block")
                        BtnContinua.Visible = False
                        ModalPopupExtenderErrores.Show()
                        lblErroresTitulo.Visible = True
                        lblErroresPopup.Visible = False

                        lblRegistros.Text = "<ul><li>No hay registros</li></ul>"
                        lblRegistros.Style.Add("display", "block")
                        lblErrores.Text = lblRegistros.Text

                        tblControles.Visible = False

                    End If
                Else
                    lblErroresTitulo.Text = "Alerta"
                    BtnModalOk.Style.Add("display", "block")
                    BtnContinua.Visible = False
                    ModalPopupExtenderErrores.Show()
                    lblErroresTitulo.Visible = True
                    lblErroresPopup.Visible = False

                    If Session("fec_Rec2") <> "" Then
                        lblFecRec.Text = "<ul><li>La Fecha Final de Recepción no puede ser menor a la Fecha Inicio de Recepción. </li></ul>"
                        lblFecRec.Style.Add("display", "block")
                        lblErrores.Text = lblFecRec.Text
                    End If

                    If Session("fec_docto") <> "" Then
                        lblFecDoc.Text = "<ul><li>Debe seleccionar la Fecha Documento </li></ul>"
                        lblFecDoc.Style.Add("display", "block")
                        lblErrores.Text = lblFecDoc.Text
                    End If

                    If Session("fec_docto1") <> "" Then

                        lblFecDoc.Text = "<ul><li>La Fecha Documento debe ser válida con el formato DD/MM/AAAA.</li></ul>"
                        lblFecDoc.Style.Add("display", "block")
                        lblErrores.Text = lblFecDoc.Text
                    End If

                    If Session("fec_Rec") <> "" And Session("fec_Rec1") <> "" Then
                        lblFecRec.Text = "<ul><li>Debe seleccionar la Fecha Recepción Doc. </li></ul>"
                        lblFecRec.Style.Add("display", "block")
                        lblErrores.Text = lblFecRec.Text
                    Else
                        If Session("fec_Rec") <> "" Then
                            lblFecRec.Text = "<ul><li>Debe seleccionar la Fecha Inicial Recepción Doc. </li></ul>"
                            lblFecRec.Style.Add("display", "block")
                            lblErrores.Text = lblFecRec.Text
                        Else
                            If Session("fec_Rec3") <> "" Then
                                lblFecRec.Text = "<ul><li>La Fecha Inicial de Recepción Doc. debe ser válida con el formato DD/MM/AAAA.</li></ul>"
                                lblFecRec.Style.Add("display", "block")
                                lblErrores.Text = lblFecRec.Text
                            End If
                        End If

                        If Session("fec_Rec1") <> "" Then
                            lblFecRec.Text &= "<ul><li>Debe seleccionar la Fecha Final Recepción Doc. </li></ul>"
                            lblFecRec.Style.Add("display", "block")
                            lblErrores.Text = lblFecRec.Text
                        Else
                            If Session("fec_Rec4") <> "" Then
                                lblFecRec.Text &= "<ul><li>La Fecha Final de Recepción Doc. debe ser válida con el formato DD/MM/AAAA.</li></ul>"
                                lblFecRec.Style.Add("display", "block")
                                lblErrores.Text = lblFecRec.Text
                            End If
                        End If

                    End If

                    If Session("Tdocto") <> "" Then
                        lblTDoc.Text = "<ul><li>Debe seleccionar el Tipo de Documento </li></ul>"
                        lblTDoc.Style.Add("display", "block")
                        lblErrores.Text = lblTDoc.Text
                    End If
                    If Session("fArea") <> "" Then

                        lblArea.Text = "<ul><li>Debe seleccionar el Área </li></ul>"
                        lblArea.Style.Add("display", "block")
                        lblErrores.Text = lblArea.Text
                    End If
                    If Session("destinatario") <> "" Then

                        lblDest.Text = "<ul><li>Debe seleccionar el Destinatario </li></ul>"
                        lblDest.Style.Add("display", "block")
                        lblErrores.Text = lblDest.Text
                    End If

                    If Session("rdbRecib") <> "" Then
                        lblRec.Text = "<ul><li>Debe seleccionar Recibido o No Recibido </li></ul>"
                        lblRec.Style.Add("display", "block")
                        lblErrores.Text = lblRec.Text
                    End If

                    If Session("refere1") <> "" Then

                        lblRefere.Text = "<ul><li>Debe ingresar una referencia </li></ul>"
                        lblRefere.Style.Add("display", "block")
                        lblErrores.Text = lblRefere.Text
                    End If

                    If Session("refere") <> "" Then

                        lblRefere.Text = "<ul><li>La referencia debe ser valida</li></ul>"
                        lblRefere.Style.Add("display", "block")
                        lblErrores.Text = lblRefere.Text
                    End If

                    If Session("folio") <> "" Then
                        lblFolio.Text = "<ul><li>Debe ingresar el número de folio</li></ul>"
                        lblFolio.Style.Add("display", "block")
                        lblErrores.Text = lblFolio.Text
                    End If

                    If Session("Oficio") <> "" Then
                        lblOficio.Text = "<ul><li>Debe ingresar el oficio</li></ul>"
                        lblOficio.Style.Add("display", "block")
                        lblErrores.Text = lblOficio.Text
                    End If

                    If Session("Remitente") <> "" Then
                        lblRemitente.Text = "<ul><li>Debe ingresar el remitente</li></ul>"
                        lblRemitente.Style.Add("display", "block")
                        lblErrores.Text = lblRemitente.Text
                    End If

                    If Session("Asunto") <> "" Then
                        lblAsunto.Text = "<ul><li>Debe ingresar el asunto</li></ul>"
                        lblAsunto.Style.Add("display", "block")
                        lblErrores.Text = lblAsunto.Text
                    End If

                    If Session("Responsable") <> "" Then
                        lblResponsable.Text = "<ul><li>Debe ingresar el responsable</li></ul>"
                        lblResponsable.Style.Add("display", "block")
                        lblErrores.Text = lblResponsable.Text
                    End If

                    If Session("FechaRegistro") <> "" Then

                        lblFechaRegistro.Text = "<ul><li>Debe seleccionar la Fecha de Registro</li></ul>"
                        lblFechaRegistro.Style.Add("display", "block")
                        lblErrores.Text = lblFechaRegistro.Text

                    End If

                    If Session("FechaLimite") <> "" Then

                        lblFechaLimite.Text = "<ul><li>Debe seleccionar la Fecha Limite</li></ul>"
                        lblFechaLimite.Style.Add("display", "block")
                        lblErrores.Text = lblFechaLimite.Text

                    End If

                    If Session("AtendidaStatus") = "AtendidaStatus" Then

                        lblAtendidaStatus.Text = "<ul><li>Debe seleccionar un estatus.</li></ul>"
                        lblAtendidaStatus.Style.Add("display", "block")
                        lblErrores.Text = lblAtendidaStatus.Text

                    End If

                    If Session("ProvieneSIE") = "SIE" Then

                        lblProvieneSIE.Text = "<ul><li>Debe seleccionar si proviene ó no el archivo de SIE</li></ul>"
                        lblProvieneSIE.Style.Add("display", "block")
                        lblErrores.Text = lblProvieneSIE.Text

                    End If

                    lblErrores.Visible = True

                End If


            End If
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Formato de Fechas incorrecto " & ex.ToString(), EventLogEntryType.Error)
            catch_cone(ex, "Error Filtrar")
        Finally
            Try
                Session("filtros") = Nothing
                Session("fec_docto") = Nothing
                Session("fec_Rec") = Nothing
                Session("Tdocto") = Nothing
                Session("fArea") = Nothing
                Session("destinatario") = Nothing
                Session("rdbRecib") = Nothing
                Session("refere1") = Nothing
                Session("refere") = Nothing

                Session("folio") = Nothing
                Session("Oficio") = Nothing
                Session("Remitente") = Nothing
                Session("AtendidaStatus") = Nothing
                Session("Asunto") = Nothing
                Session("Nombre") = Nothing
                Session("FechaRegistro") = Nothing
                Session("FechaLimite") = Nothing
                Session("ProvieneSIE") = Nothing

            Catch ex As Exception

            End Try

        End Try
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim chkRecibido As CheckBox
        Dim chkAtendido As CheckBox
        Dim folio As String
        Dim ds As New DataSet
        Dim Sesion As Seguridad = Nothing
        Dim Valores As String = String.Empty
        Dim Condicion As String = String.Empty
        Dim continua As Boolean = False
        Dim tran As Odbc.OdbcTransaction = Nothing
        Dim BanRec As Boolean = False
        Dim Con As Clases.Conexion = Nothing

        Try

            Con = New Clases.Conexion()

            'Sesion = New Seguridad
            'Sesion.BitacoraInicia("Actualizar documento recibido ", Con)

            'tran = Con.BeginTran()

            '' Obtenemod Folios a Procesar
            Dim _folios As String = ""
            Dim _foliosOrig As New ArrayList


            _foliosOrig.AddRange(hdnChecks.Value.Split(","))

            For Each _dataItem As DataGridItem In GridPrincipal.Items
                chkRecibido = _dataItem.FindControl("chkRecibido")
                chkAtendido = _dataItem.FindControl("chkAtendido")

                'folio = Convert.ToUInt64(_dataItem.Cells.Item(1).Text)
                folio = Convert.ToUInt64(CType(_dataItem.Cells.Item(1).Controls(1), LinkButton).Text)

                If (_foliosOrig.Contains(folio) AndAlso chkRecibido.Checked) Then _folios &= folio & ","


                'If chkRecibido.Checked Or chkAtendido.Checked Then
                '    ds = New DataSet
                '    folio = Convert.ToUInt64(_dataItem.Cells.Item(1).Text)

                '    ds = Con.Datos("SELECT ESTATUS_RECIBIDO, ESTATUS_ATENDIDA FROM " Clases.Conexion.Owner & "BDA_R_DOC_COPIAS WHERE ID_FOLIO = " & folio & " AND USUARIO = '" & _dataItem.Cells(20).Text & "' AND ORIGINAL_FLAG = " & _dataItem.Cells(33).Text, tran, False)

                '    If ds.Tables(0).Rows.Count > 0 Then
                '        If ds.Tables(0).Rows(0).ItemArray(0).ToString() = "1" And ds.Tables(0).Rows(0).ItemArray(1).ToString() = "1" Then
                '            Continue For
                '        End If
                '        If chkRecibido.Checked And chkAtendido.Checked Then
                '            Valores = "ESTATUS_RECIBIDO = 1, ESTATUS_ATENDIDA = 1"
                '        ElseIf chkRecibido.Checked Then
                '            Valores = "ESTATUS_RECIBIDO = 1"
                '        ElseIf chkAtendido.Checked Then
                '            Valores = "ESTATUS_ATENDIDA = 1"
                '        End If
                '        Condicion = "ID_FOLIO =" & folio & " AND USUARIO = '" & _dataItem.Cells(20).Text & "' AND ORIGINAL_FLAG = " & _dataItem.Cells(33).Text

                '        continua = Con.Actualizar(Conexion.Owner & "BDA_R_DOC_COPIAS", Valores, Condicion, tran)
                '        If Not continua Then
                '            Exit For
                '        End If

                '    End If

                'End If

            Next

            'Procesamos y levantamos ventana an caso de ser necesario
            _folios = _folios.Trim(",")

            If _folios.Length = 0 Then

                ModalMensaje("No se han seleccionado Folios")
                Exit Sub

            End If



            Dim _dt As DataTable = LogicaNegocio.BusinessRules.BDA_R_DOC_COPIAS.GetAtendidosAceptar(_folios)

            If _dt.Rows.Count = 0 Then

                ModalMensaje("Ningún folio de los seleccionados cuenta con Documentos")
                Exit Sub

            End If

            'grvAnexos.DataSource = _dt
            'grvAnexos.DataBind()

            ModalPopUpCaptura.Show()




        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Error al recibir el documento " & ex.ToString(), EventLogEntryType.Error)
        Finally

            mpeProcesa.Hide()

            If Not IsNothing(Con) Then

                Con.Cerrar()

            End If

            chkRecibido = Nothing
            chkAtendido = Nothing

        End Try
    End Sub

    Protected Sub nomArch_Click1(ByVal sender As Object, ByVal e As EventArgs)

        If ISEXPEDIENTE Then Return

        Dim lnkArch As LinkButton = CType(sender, LinkButton)
        'Dim gi As DataGridItem = CType(lnkArch.Parent.Parent, DataGridItem)

        If lnkArch.Text = ConsultaText Then

            For Each row As DataGridItem In GridPrincipal.Items

                CType(row.FindControl("chkSeleccionar"), CheckBox).Checked = False

            Next

            CType(GridPrincipal.Items(Convert.ToInt32(lnkArch.CommandArgument)).FindControl("chkSeleccionar"), CheckBox).Checked = True
            ' btnAdjuntarSIE_Click(Nothing, Nothing)

        Else

            Dim nom_archivo As String = DirectCast(sender, System.Web.UI.WebControls.LinkButton).Text 'gi.Cells(13).Text
            AbreArchivoLink(nom_archivo)

        End If


    End Sub


    Public Function ImagenEstatusCopia(ByVal Estatus As String, ByVal Bloq As String) As String
        Try
            If Estatus = "O" And Bloq = "0" Then
                Return "~/images/original.gif"
            ElseIf Estatus = "O" And Bloq = "1" Then
                Return "~/images/original_disabled.gif"
            ElseIf Estatus = "C" And Bloq = "0" Then
                Return "~/images/copia.gif"
            ElseIf Estatus = "C" And Bloq = "1" Then
                Return "~/images/copia_disabled.gif"
            ElseIf Estatus = "TO" And Bloq = "0" Then
                Return "~/images/TemplateTab2.gif"
            ElseIf Estatus = "TO" And Bloq = "1" Then
                Return "~/images/TemplateTab2_disabled.gif"
            ElseIf Estatus = "TC" And Bloq = "0" Then
                Return "~/images/TemplateTab1.gif"
            ElseIf Estatus = "TC" And Bloq = "1" Then
                Return "~/images/TemplateTab1_disabled.gif"
            End If

        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion ImagenEstatus: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function

    Public Function ImagenFechaLimite(ByVal Estatus_Atender As String)
        Try
            Dim imagenRegreso As String = ""

            If Estatus_Atender = "Atendido" Then
                imagenRegreso = "~/images/ATENDIDO.png"
            ElseIf Estatus_Atender = "Normal" Then
                imagenRegreso = "~/images/statusNormal.png"
            ElseIf Estatus_Atender = "Por Vencer" Then
                imagenRegreso = "~/images/PREVENTIVO.png"
            ElseIf Estatus_Atender = "Vencido" Then
                imagenRegreso = "~/images/VENCIDO.png"
            ElseIf Estatus_Atender = "En Espera VoBo" Then
                imagenRegreso = "~/images/question.png"
            ElseIf Estatus_Atender = "Complemento" Then
                imagenRegreso = "~/images/question.png"
            ElseIf Estatus_Atender = "En trámite" Then
                imagenRegreso = "~/images/tramite.png"
            End If
            Return imagenRegreso
        Catch ex As Exception
        End Try
        Return ""

    End Function

    Public Function ImagenEstatus(ByVal Estatus As String) As String
        Try
            If Estatus = "0" Then
                Return "~/images/ERROR.jpg"
            ElseIf Estatus = True Then
                Return "~/images/OK.jpg"
            End If
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion ImagenEstatus: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function

    Public Function FolioAsociados(ByVal Expediente As String, ByVal Folio_Asociado As String)
        Try
            Dim imagenRegreso As String = ""

            If Folio_Asociado = "0" Then
                imagenRegreso = "~/images/Asociado.png"
            ElseIf Folio_Asociado = "1" Then
                imagenRegreso = "~/images/Duplicado.png"
            End If

            Return imagenRegreso
        Catch ex As Exception
        End Try
        Return ""

    End Function


    Protected Sub AbreArchivoLink(ByVal NombreArchivo As String)
        Dim nom_archivo As String = NombreArchivo
        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim usuario As String
        Dim passwd As String
        Dim ServSharepoint As String
        Dim Dominio As String
        Dim Biblioteca As String
        Dim enc As New YourCompany.Utils.Encryption.Encryption64

        Try

            Dim Archivo() As Byte = Nothing
            Dim filename As String = "attachment; filename=" & nom_archivo
            Try
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
                passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServer"), "webCONSAR")
                Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
                Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibrary"), "webCONSAR")

                cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)

                Dim Url As String = ServSharepoint & "/" & Biblioteca & "/" & nom_archivo

                Archivo = cliente.DownloadData(ResolveUrl(Url))

            Catch ex As Exception
                EventLogWriter.EscribeEntrada("No se puede abrir el documento: " & nom_archivo, EventLogEntryType.Error)
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
        Catch ex As Exception

        End Try
    End Sub




    Public Function ExportarGrid(ByVal oDs As DataView, ByVal Grid As DataGrid) As DataTable
        Dim oTabla As DataTable = New DataTable
        Try
            Dim oColumna As DataColumn
            Dim oRenglon As DataRow



            'Crea columnas tomadas del Grid
            For Each gridColumn As DataGridColumn In Grid.Columns
                If gridColumn.Visible And gridColumn.SortExpression.Length > 0 Then
                    oColumna = New DataColumn
                    oColumna.ColumnName = gridColumn.HeaderText
                    oColumna.DataType = System.Type.GetType("System.String")
                    oColumna.DefaultValue = gridColumn.SortExpression
                    oTabla.Columns.Add(oColumna)
                End If
            Next

            If oDs.Count > 0 Then

                For Each row As DataRow In oDs.Table.Rows
                    oRenglon = oTabla.NewRow()
                    For Each column As DataColumn In oTabla.Columns
                        oRenglon(column.ColumnName) = row(Convert.ToString(column.DefaultValue))
                    Next
                    oTabla.Rows.Add(oRenglon)
                Next
            End If

            If oDs.Count > 0 Then
                For Each rowCambio As DataRow In oTabla.Rows
                    rowCambio("Anexo") = IIf(rowCambio("Anexo").ToString() = 0, "No", "Si")
                    rowCambio("Recibido") = IIf(rowCambio("Recibido").ToString() = 0, "No", "Si")
                    rowCambio("Atendido") = IIf(rowCambio("Atendido").ToString() = 0, "No", "Si")
                Next
            End If

        Catch ex As Exception
            EventLogWriter.EscribeEntrada(ex.ToString(), EventLogEntryType.Error)
        End Try
        Return oTabla
    End Function

    ''' <summary>
    ''' Trunca los registros, dejando sólo el número de registros indicado en el parámetro Integer.
    ''' </summary>
    ''' <param name="ds">Un DataSet ordenado por Folio ascendentemente.</param>
    ''' <param name="limite">El número de registros a los cuales se va a truncar el DataSet.</param>
    ''' <returns>Un DataSet ordenado por Folio ascendentemente sólo con el número de registros indicados.</returns>
    ''' <remarks>Elimina los primeros registros, dejando los últimos más recientes (suponiendo que el DataSet está ordenado ascendemente) y respetando el órden.</remarks>

    Private Function truncaRegistros(ByVal ds As DataSet, ByVal limite As Integer) As DataSet
        If ds.Tables(0).Rows.Count > limite Then
            'Mensaje error
            lblErroresTitulo.Text = "Alerta"
            BtnModalOk.Style.Add("display", "block")
            BtnContinua.Visible = False
            ModalPopupExtenderErrores.Show()
            lblErroresTitulo.Visible = True
            lblErroresPopup.Visible = False
            lblRegistros.Text = "<ul><li>Usted tiene " & ds.Tables(0).Rows.Count.ToString() & " asuntos, se están desplegando solamente los " & limite.ToString() & " más recientes. Si desea ver todos, dé clic en el botón <b>FILTRAR</b>.</li></ul>"
            lblRegistros.Style.Add("display", "block")
            lblErrores.Text = lblRegistros.Text
            'Borra registros excedentes
            For iL As Integer = 0 To ds.Tables(0).Rows.Count - limite - 1
                ds.Tables(0).Rows(iL).Delete()
            Next
            ds.Tables(0).AcceptChanges()
        End If
        Return ds
    End Function

    Protected Sub btnReasignar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReasignar.Click
        Session.Remove("Consulta_TurnadoReasignacion")
        Session("Reasignar") = "OK"

        Dim dvConsulta As New DataView()
        dvConsulta = ConsultaOpcionReasignacion()

        If dvConsulta.Count > 0 Then
            If ViewState("Folios_Repetidos") Is Nothing Then

                '' Aseguramos que ninguno de los folios seleccionados se encuentra en espera de VoBo
                Dim _enEspera As String = String.Empty
                _enEspera = ValorEnEspera(dvConsulta.ToTable)
                If Not String.IsNullOrEmpty(_enEspera) Then
                    ModalMensaje("El(los) folio(s) " & _enEspera & " no puede ser turnado, se encuentra en espera de Vo.Bo.")
                    Return
                End If


                Session("Consulta_TurnadoReasignacion") = dvConsulta.ToTable

                dvConsulta.RowFilter = "DESTINATARIO <> 'O'"

                If dvConsulta.Count > 0 Then

                    ModalMensaje("Unicamente se pueden reasignar documento originales.")
                    Return
                Else


                    dvConsulta.RowFilter = "ESTATUS_ATENDIDA = 1"

                    If dvConsulta.Count > 0 Then
                        ModalMensaje("Unicamente se pueden reasignar documento originales.")
                        Return
                    Else
                        Response.Redirect("Reasignacion.aspx", False)
                    End If



                End If

            Else

                ModalMensaje(ViewState("Folios_Repetidos").ToString())
                ViewState("Folios_Repetidos") = Nothing
                Return

            End If

        Else

            ModalMensaje("Debe seleccionar un registro")

        End If



    End Sub

    Protected Sub btnTurnar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTurnar.Click
        Session.Remove("AsociarFolios")
        Dim dvConsulta As New DataView()

        dvConsulta = ConsultaOpcionTurnado()
        Dim A As Boolean
        A = RegresaTurnar(CInt(dvConsulta.Table(0).Item(0).ToString))
        If A = False Then
            If dvConsulta.Count > 0 Then
                If ViewState("Folios_Repetidos") Is Nothing Then
                    Dim _vobo As Integer
                    Dim _enEspera As String = String.Empty

                    '' Aseguramos que ninguno de los folios seleccionados se encuentra en espera de VoBo
                    _enEspera = ValorEnEspera(dvConsulta.ToTable)
                    If Not String.IsNullOrEmpty(_enEspera) Then
                        ModalMensaje("El(los) folio(s) " & _enEspera & " no puede ser turnado, se encuentra en espera de Vo.Bo.")
                        Return
                    End If

                    '' Determinamos el estado de VoBo de los folios seleccionados
                    Try
                        _vobo = ValorVoBo(dvConsulta.ToTable)
                    Catch ex As Exception
                        ModalMensaje("No se pueden enviar a turnar folios con solicitudes de Vo.Bo. diferentes")
                        Return
                    End Try

                    Session("Consulta_TurnadoReasignacion") = dvConsulta.ToTable
                    Response.Redirect("Turnado.aspx?vobo=" & _vobo.ToString, False)

                Else
                    ModalMensaje(ViewState("Folios_Repetidos").ToString())
                    ViewState("Folios_Repetidos") = Nothing
                    Return
                End If
            End If
        Else
            ModalMensaje("No puedes Turnar este Registro")
            Return
        End If
        ModalMensaje("Debe seleccionar un registro")
    End Sub
    Protected Function RegresaTurnar(ByVal Id_Turnado As Integer) As Boolean
        Dim j As Boolean
        Try
            Dim ID_G_TURNADO As Integer
            Dim USUARIO As String
            Dim US_RESPO As String
            Dim Con As Clases.Conexion = Nothing
            Con = New Clases.Conexion()
            Dim Dr As Odbc.OdbcDataReader = Nothing
            Dr = Con.Consulta("SELECT USUARIO,USUARIO_RESPONSABLE,ID_G_TURNADO FROM " & Clases.Conexion.Owner & "BDA_VIEW_DOCUMENTOS where Id_Folio=" & Id_Turnado & " AND ID_G_TURNADO <> 0", Nothing)
            Dr.Read()
            ID_G_TURNADO = Convert.ToInt32(Dr.Item("ID_G_TURNADO"))
            USUARIO = Dr.Item("USUARIO")
            US_RESPO = Dr.Item("USUARIO_RESPONSABLE")
            Dr.Close()
            If USUARIO = US_RESPO Then
                j = True
            Else
                j = False
            End If
        Catch
            j = False
        End Try
        Return j
    End Function



    ''' <summary>
    ''' Ocultas la filas del DataGrid
    ''' </summary>
    ''' <param name="listaOcultar">Recibe un ListBox de la lista oculta de columnas</param>
    ''' <remarks></remarks>
    ''' 

    Private Sub OcultaFilasGrid(ByVal listaOcultar As ListBox, ByVal listaVisible As ListBox)

        Try
            If listaOcultar.Items.Count > 0 Then
                For contSinAsignados = 0 To listaOcultar.Items.Count - 1
                    For contPerzonalizada = 0 To GridPrincipal.Columns.Count - 1
                        If DirectCast(listaOcultar.Items(contSinAsignados).Text, String) = GridPrincipal.Columns(contPerzonalizada).HeaderText Then
                            GridPrincipal.Columns(contPerzonalizada).Visible = False
                        End If
                    Next
                Next
            End If

            If listaVisible.Items.Count > 0 Then
                For contAsignados = 0 To listaVisible.Items.Count - 1
                    For contPerzonalizada = 1 To GridPrincipal.Columns.Count - 1
                        If DirectCast(listaVisible.Items(contAsignados).Text, String) = GridPrincipal.Columns(contPerzonalizada).HeaderText Then
                            GridPrincipal.Columns(contPerzonalizada).Visible = True
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            ModalMensaje("Se genero el siguiente Error: " + ex.Message)
        End Try




    End Sub

    Function ConsultaOpcionReasignacion() As DataView


        Dim chkOpcionSeleccionar As CheckBox
        Dim arrFolio As String = ""
        Dim dvConsultaFiltro As New DataView()
        Dim dtConsultaFinal As New DataTable()
        Dim dtTablaFinal As New DataView()

        Dim cuentaRepetidos As Integer = 0

        dvConsultaFiltro = CType(Session("ConsultaPrincipalOficios"), DataSet).Tables(0).DefaultView

        For Each columna As DataColumn In dvConsultaFiltro.Table.Columns
            dtConsultaFinal.Columns.Add(New DataColumn(columna.ColumnName, GetType(String)))
        Next

        For Each Row As DataGridItem In GridPrincipal.Items

            chkOpcionSeleccionar = CType(Row.FindControl("chkSeleccionar"), CheckBox)

            If chkOpcionSeleccionar.Checked = True Then
                'dvConsultaFiltro.RowFilter = "ID_FOLIO = '" + Row.Cells(1).Text + "' AND USUARIO = '" + Row.Cells(20).Text + "' AND ORIGINAL_FLAG = '" + Row.Cells(33).Text + "'  "
                dvConsultaFiltro.RowFilter = "ID_FOLIO = '" + CType(Row.Cells(1).Controls(1), LinkButton).Text + "' AND USUARIO = '" + Row.Cells(20).Text + "' AND ORIGINAL_FLAG = '" + Row.Cells(33).Text + "'  "

                If dvConsultaFiltro.Count > 0 Then

                    dtConsultaFinal.ImportRow(dvConsultaFiltro.Item(0).Row)

                End If


            End If
        Next

        dtTablaFinal = New DataView(dtConsultaFinal)

        Return dtTablaFinal


    End Function

    Function ConsultaOpcionTurnado() As DataView

        Dim chkOpcionSeleccionar As CheckBox
        Dim arrFolio As String = ""
        Dim dvConsultaFiltro As New DataView()
        Dim dtConsultaFinal As New DataTable()
        Dim dtTablaFinal As New DataView()
        dvConsultaFiltro = CType(Session("ConsultaPrincipalOficios"), DataSet).Tables(0).DefaultView
        Dim cuentaRepetidos As Integer = 0
        For Each columna As DataColumn In dvConsultaFiltro.Table.Columns
            dtConsultaFinal.Columns.Add(New DataColumn(columna.ColumnName, GetType(String)))
        Next

        For Each Row As DataGridItem In GridPrincipal.Items
            chkOpcionSeleccionar = CType(Row.FindControl("chkSeleccionar"), CheckBox)
            If chkOpcionSeleccionar.Checked = True Then
                'dvConsultaFiltro.RowFilter = " ID_FOLIO = '" + Row.Cells(1).Text + "' AND USUARIO = '" + Row.Cells(20).Text + "' AND DESTINATARIO = '" + Row.Cells(10).Text + "'  "
                dvConsultaFiltro.RowFilter = " ID_FOLIO = '" + CType(Row.Cells(1).Controls(1), LinkButton).Text + "' AND USUARIO = '" + Row.Cells(20).Text + "' AND DESTINATARIO = '" + Row.Cells(10).Text + "'  "

                If dvConsultaFiltro.Count > 0 Then
                    dtConsultaFinal.ImportRow(dvConsultaFiltro.Item(0).Row)
                End If


            End If
        Next

        If cuentaRepetidos > 1 Then
            ViewState("Folios_Repetidos") = "Solo se permite turnar un documento con el mismo folio"
        End If

        dtTablaFinal = New DataView(dtConsultaFinal)
        Return dtTablaFinal
    End Function



    Private Sub ModalMensaje(ByVal MesajeMostrar As String)
        ModalPopupExtenderErrores.Show()
        lblErroresTitulo.Visible = True
        lblErroresPopup.Visible = False
        lblRegistros.Text = "<ul><li>" + MesajeMostrar + "</li></ul>"
        lblRegistros.Style.Add("display", "block")
    End Sub

    Function AgregaFiltroEstatusAtendida(ByVal objDataSet As DataSet) As DataSet
        Try
            Dim fecha_inicial As String = String.Empty
            Dim fecha_final As String = String.Empty
            Dim fechaHoy As Date = DateTime.Now
            Dim esNegativo As Boolean = False
            Dim objDataView As New DataView()
            Dim esDiaHabil As String = ""
            Dim dtDiasFestivos As New DataTable

            If Session("AtendidaStatus") <> Nothing Then
                Dim dtTotalReg As DataTable = (objDataSet.Tables(1))
                objDataView = New DataView(objDataSet.Tables(0))
                objDataView.RowFilter = "ATENDIDA  LIKE '%" + Session("AtendidaStatus").ToString() + "%'"
                Dim dtTabla As New DataTable()
                dtTabla = CType(objDataView.ToTable, DataTable)
                objDataSet = New DataSet()
                objDataSet.Tables.Add(dtTabla.Copy())
                objDataSet.Tables.Add(dtTotalReg.Copy())
                TotalRegistros = objDataSet.Tables(0).Rows.Count
            Else
                objDataSet.AcceptChanges()
            End If


            Return objDataSet

        Catch ex As Exception

            Return New DataSet()
        End Try


    End Function






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
            catch_cone(ex, "verificaSesion")
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
            If Not Perfil.Autorizado("BandejaEntrada.aspx") Then
                logout = True
            End If

        Catch ex As Exception
            catch_cone(ex, "verificaPerfil")
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

    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
        ModalMensaje("Error: verifica con el Administrador")
    End Sub



    Public Function EstatusNotificacion(ByVal Estatus As Object) As String
        Try
            If IsDBNull(Estatus) Then
                Return "~/Images/ERROR.gif"
            Else
                If CBool(Estatus) Then
                    Return "~/Images/OK.gif"
                Else
                    Return "~/Images/ERROR.gif"
                End If
            End If

        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion EstatusNotificacion: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function

    Public Function EstatusNotificacionTooltip(ByVal Estatus As Object) As String
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
    Private Function ValorVoBo(ByVal Datos As DataTable) As Integer

        Dim resultado As Integer = 0
        Dim ExisteVoBo As Boolean = False
        Dim ValidaVoBoCompara As Boolean = False
        Dim indice As Integer = 0

        For Each row As DataRow In Datos.Rows

            With row

                '' Validamos si podemos dar VoBo
                ExisteVoBo = LogicaNegocio.BusinessRules.BDA_TURNADO.ValidaVoBoTurnado(Convert.ToInt32(.Item("ID_G_TURNADO")),
                                                                              Convert.ToInt32(.Item("ID_FOLIO")),
                                                                              .Item("USUARIO_RESPONSABLE").ToString,
                                                                              Convert.ToInt32(.Item("ORIGINAL_FLAG")),
                                                                             .Item("USUARIO").ToString)

            End With

            '' Si es el primer registro, el valor y la comparacion deben ser iguales
            If indice = 0 Then ValidaVoBoCompara = ExisteVoBo

            '' incrementamos indice
            indice += 1

            If ValidaVoBoCompara <> ExisteVoBo Then

                Throw New Exception("VoBo's diferentes")

            End If

        Next

        If ExisteVoBo Then resultado = 1

        Return resultado

    End Function


    Private Function ValorEnEspera(ByVal Datos As DataTable) As String

        Dim resultado As String = String.Empty

        For Each row As DataRow In Datos.Rows

            With row

                If row("ESTATUS_TRAMITE").ToString = "2" Then

                    resultado &= row("ID_FOLIO").ToString & ","

                End If

            End With


        Next


        If Not String.IsNullOrEmpty(resultado) Then resultado = resultado.Trim(",")


        Return resultado

    End Function

    Private Sub ManejaExpediente()

        If ISEXPEDIENTE Then

            'BtnExportarSuperior.Visible = False
            btnReasignar.Visible = False
            btnTurnar.Visible = False
            btnQuitarTurnado.Visible = False
            btnGuardar.Visible = False
            BtnPersonalizar.Style.Add("display", "none")

            trBtnCancelar.Visible = True

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Session(LogicaNegocio.BusinessRules.BDA_C_EXPEDIENTE.SessionExpResult) = Nothing

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

    End Sub

    Private Sub handleRowDoubleClick(ByVal strRequest As String)

        Session(LogicaNegocio.BusinessRules.BDA_C_EXPEDIENTE.SessionExpResult) = strRequest

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

    End Sub
    Private Sub BtnModalOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnModalOk.Click

    End Sub
    Private Sub GridPrincipal_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridPrincipal.DeleteCommand
        Dim Index As Integer
        Try
            Index = e.Item.ItemIndex
            Session("ID_OFICIO") = Nothing
            Session("F_FECHA_OFICIO") = Nothing
            Session("CLAVE_OFICIO") = Nothing
            Session("TIPO_OFICIO") = Nothing
            Session("FECHA_VENCI") = Nothing
            Session("PLAZO") = Nothing
            Session("ID_ENTIDAD_TIPO") = Nothing
            Session("ID_ENTIDAD") = Nothing
            Session("FECHA_NOTIFICACION") = Nothing
            Session("T_INICIALES") = Nothing
            Session("ID_TIPO_DOCUMENTO") = Nothing
            Session("ID_CLASIFICACION") = Nothing
            Session("T_CLASIFICACION") = Nothing
            Session("T_HYP_ARCHIVOSCAN") = Nothing
            Session("ID_AREA_OFICIO") = Nothing
            Session("ID_ANIO") = Nothing
            Session("I_OFICIO_CONSECUTIVO") = Nothing
            Session("NOTIFICACION") = Nothing

            Session.Remove("ID_OFICIO")
            Session.Remove("CLAVE_OFICIO")
            Session.Remove("TIPO_OFICIO")
            Session.Remove("PLAZO")
            Session.Remove("FECHA_NOTIFICACION")
            Session.Remove("T_INICIALES")
            Session.Remove("ID_TIPO_DOCUMENTO")
            Session.Remove("F_FECHA_OFICIO")
            Session.Remove("ID_ENTIDAD_TIPO")
            Session.Remove("ID_ENTIDAD")
            Session.Remove("ID_CLASIFICACION")
            Session.Remove("T_CLASIFICACION")
            Session.Remove("T_HYP_ARCHIVOSCAN")
            Session.Remove("ID_AREA_OFICIO")
            Session.Remove("ID_ANIO")
            Session.Remove("I_OFICIO_CONSECUTIVO")
            Session.Remove("NOTIFICACION")

            'ID_OFICIO0.Text = e.Item.Cells(3).Text
            'CLAVE_OFICIO.Text = e.Item.Cells(2).Text
            'TIPO_OFICIO.Text = e.Item.Cells(12).Text
            'FECHA_VENCIMIENTO.Text = e.Item.Cells(31).Text
            'PLAZO_DIAS.Text = e.Item.Cells(5).Text
            'FECHA_NOTIFICACION.Text = e.Item.Cells(14).Text
            'ENTIDAD_CORTO.Text = e.Item.Cells(9).Text
            'ID_TIPO_DOCUMENTO.Text = e.Item.Cells(36).Text
            'F_FECHA_OFICIO.Text = e.Item.Cells(16).Text
            'ID_CONSAR.Text = e.Item.Cells(37).Text
            'ID_ENTIDAD.Text = e.Item.Cells(39).Text
            'ID_CLASIFICACION.Text = e.Item.Cells(38).Text
            'T_CLASIFICACION.Text = e.Item.Cells(33).Text
            'T_HYP_ARCHIVOSCAN.Text = e.Item.Cells(21).Text
            'ID_AREA_OFICIO.Text = e.Item.Cells(40).Text
            'ID_ANIO.Text = e.Item.Cells(41).Text
            'I_OFICIO_CONSECUTIVO.Text = e.Item.Cells(42).Text


            Session("ID_OFICIO") = e.Item.Cells(3).Text
            Session("CLAVE_OFICIO") = e.Item.Cells(2).Text
            Session("TIPO_OFICIO") = e.Item.Cells(12).Text
            Session("FECHA_VENCI") = e.Item.Cells(31).Text
            Session("PLAZO") = e.Item.Cells(5).Text
            Session("FECHA_NOTIFICACION") = e.Item.Cells(14).Text
            'Session("T_INICIALES") = e.Item.Cells(9).Text
            Session("ID_TIPO_DOCUMENTO") = e.Item.Cells(36).Text
            Session("F_FECHA_OFICIO") = e.Item.Cells(16).Text
            Session("ID_ENTIDAD_TIPO") = e.Item.Cells(37).Text
            Session("ID_ENTIDAD") = e.Item.Cells(39).Text
            Session("ID_CLASIFICACION") = e.Item.Cells(38).Text
            Session("T_CLASIFICACION") = e.Item.Cells(33).Text
            'Session("T_HYP_ARCHIVOSCAN") = e.Item.Cells(21).Text
            Session("ID_AREA_OFICIO") = e.Item.Cells(40).Text
            Session("ID_ANIO") = e.Item.Cells(41).Text
            Session("I_OFICIO_CONSECUTIVO") = e.Item.Cells(42).Text
            Session("NOTIFICACION") = e.Item.Cells(43).Text

            Dim cveOficio As String = e.Item.Cells(2).Text

            If IsNothing(Request.QueryString("r")) Then
                System.Web.UI.ScriptManager.RegisterStartupScript(source, source.GetType(), "cerrar", " window.close();", True)
            Else
                If ObjProps.idArea = "" Then
                    ScriptManager.RegisterStartupScript(source, source.GetType(), "cerrar", " CierraModal(" & ObjProps.idRenGrid & "," & ObjProps.idDocumento & "," & ObjProps.idVersion & ");", True)
                Else
                    ScriptManager.RegisterStartupScript(source, source.GetType(), "cerrar", " CierraModalOpiPC(" & ObjProps.idRenGrid & "," & ObjProps.idDocumento & "," & ObjProps.idVersion & ",'" & cveOficio & "');", True)
                End If
            End If

        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_DeleteCommand")
        Finally
        End Try
    End Sub

    Private Sub GridPrincipal_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GridPrincipal.ItemDataBound


        Try
            e.Item.Attributes("onDblClick") = String.Format("javascript:__doPostBack('GridPrincipal$ctl{0:00}$ctl00','')", e.Item.ItemIndex + 2) ' ClientScript.GetPostBackClientHyperlink(GridPrincipal, String.Format("$ctl{0,2}$ctl00", e.Item.ItemIndex), True)
        Catch ex As Exception
            catch_cone(ex, " GridPrincipal_ItemDataBound")

        End Try

    End Sub

    Private Sub lnkCierra_Click(sender As Object, e As System.EventArgs) Handles lnkCierra.Click

    End Sub
    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        Dim index As Integer = GridPrincipal.SelectedIndex
        Dim name As String = "PP"
        Dim country As String = "CC"
        Dim message As String = "Row Index: " & index & "\nName: " & name + "\nCountry: " & country
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('" + message + "');", True)
    End Sub

    Protected Sub GridPrincipal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridPrincipal.SelectedIndexChanged
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert();", True)

    End Sub
    Protected Sub LigarFolioSICOD()

        If ObjProps.origen = "OPI" Then
            Dim documento As New Entities.DocumentoOPI
            If ObjProps.idDocumento <= 0 Then
                documento.Folio = ObjProps.idFolio
                documento.IdDocumento = ObjProps.idCatDocumento
                documento.FolioSICOD = Session("CLAVE_OFICIO")
                documento.NombreDocumento = "" 'Session("T_HYP_ARCHIVOSCAN")
                documento.NombreDocumentoSh = "" 'Session("T_HYP_ARCHIVOSCAN")
                documento.Usuario = Session("Usuario")

                documento.AgregarLigar()
            Else
                documento.Folio = ObjProps.idFolio
                documento.IdDocumento = ObjProps.idDocumento
                documento.FolioSICOD = Session("CLAVE_OFICIO")

                documento.ActualizarFolioSICOD()
            End If
        ElseIf ObjProps.origen = "PC" Then
            Dim documento As New Entities.DocumentoPC
            If ObjProps.idDocumento <= 0 Then
                documento.Folio = ObjProps.idFolio
                documento.IdDocumento = ObjProps.idCatDocumento
                documento.FolioSICOD = Session("CLAVE_OFICIO")
                documento.NombreDocumento = "" 'Session("T_HYP_ARCHIVOSCAN")
                documento.NombreDocumentoSh = "" 'Session("T_HYP_ARCHIVOSCAN")
                documento.Usuario = Session("Usuario")

                documento.AgregarLigar()

                Dim dtRequerimientoExistente As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(ObjProps.idFolio)
                If dtRequerimientoExistente.Rows.Count = 0 Then
                    Dim req As New Entities.RequerimientoPC
                    req.Folio = ObjProps.idFolio
                    req.Usuario = Session("Usuario")
                    req.Agregar()

                    Entities.RequerimientoPC.ActualizarFolioSICOD(New List(Of String)({"T_FOLIO_SICOD"}), New List(Of Object)({Session("CLAVE_OFICIO")}), ObjProps.idFolio)
                End If
            Else
                documento.Folio = ObjProps.idFolio
                documento.IdDocumento = ObjProps.idDocumento
                documento.FolioSICOD = Session("CLAVE_OFICIO")

                documento.ActualizarFolioSICOD()

            End If
        ElseIf ObjProps.origen = "VISITA" Then
            Dim documento As New Documento
            If ObjProps.idDocumento <= 0 Then
                documento.IdVisita = ObjProps.idFolio
                documento.N_ID_DOCUMENTO = ObjProps.idCatDocumento
                documento.FolioSICOD = Session("CLAVE_OFICIO")
                documento.T_NOM_DOCUMENTO = Session("CLAVE_OFICIO")
                documento.T_NOM_DOCUMENTO_ORI = Session("CLAVE_OFICIO")
                documento.T_ID_USUARIO = Session("Usuario")
                documento.N_ID_VERSION = ObjProps.idVersion
                documento.I_ID_PASO = ObjProps.paso
                documento.AgregarLigar()

            Else
                documento.IdVisita = ObjProps.idFolio
                documento.N_ID_DOCUMENTO = ObjProps.idDocumento
                documento.FolioSICOD = Session("CLAVE_OFICIO")
                documento.T_ID_USUARIO = Session("Usuario")
                documento.ActualizarFolioSICOD()

            End If
            'Session.Remove("T_HYP_ARCHIVOSCAN")

        End If
    End Sub

    Protected Sub GridPrincipal_ItemCreated(sender As Object, e As DataGridItemEventArgs) Handles GridPrincipal.ItemCreated

    End Sub
End Class
Public Enum TipoFiltroOfi
    FechaDeDocto = 0
    FechadeRecep = 1
    TipoDeDocto = 2
    Area = 3
    Destinatario = 4
    Recibido = 5
End Enum

<Serializable>
Public Class PropsModal
    Public idRenGrid As String
    Public idDocumento As String
    Public idCatDocumento As String
    Public idVersion As String
    Public idArea As String
    Public tipo As String
    Public idFolio As String
    Public origen As String
    Public paso As String
End Class
Public Class ARCHOfi
    Private _folio As Integer
    Public Property folio() As Integer
        Set(ByVal value As Integer)
            _folio = value
        End Set
        Get
            Return _folio
        End Get

    End Property
    Private _nomArch As String
    Public Property nomArch() As String
        Set(ByVal value As String)
            _nomArch = value
        End Set
        Get
            Return _nomArch
        End Get

    End Property
    Private _idArch As String
    Public Property idArch() As String
        Set(ByVal value As String)
            _idArch = value
        End Set
        Get
            Return _idArch
        End Get

    End Property
End Class
