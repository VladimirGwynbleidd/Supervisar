Imports Clases
Imports System.Diagnostics
Imports System.IO
Imports System.IO.StreamWriter
Imports System.Net
'Imports AdministracionSISVIG


Public Class BandejaEntrada

    Inherits System.Web.UI.Page

    Private Const ConsultaText As String = "Consultar Archivos..."

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
    Private SqlTotalDatosInicio As String = "SELECT  Count(*) TotalRegistros FROM (  "
    Private SqlTotalDatosFin As String = " )  as UnionedResultTotal  "

    'Indica el Tope de la Consulta
    Private SqlTopInicio As String = "SELECT TOP 50 * FROM (  "
    Private SqlTopFin As String = " )  as UnionedResult"

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
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Archivo", "setTimeout('CallClickINICIO()', 300);", True)
            End If
        Catch ex As Exception
            catch_cone(ex, "Page_Load")
        Finally
            Sesion.CerrarCon()
            Sesion = Nothing
        End Try
    End Sub

    Protected Sub CargaGRID_click(ByVal sender As Object, ByVal e As EventArgs)
        ActualizaGrid(True)
        Imagen_procesando.Style.Add("display", "none")
        GRID.Style.Add("visibility", "visible")

    End Sub

    Public Sub ActualizaGrid(ByVal usarFiltro As Boolean)

        Dim usuario As String = Session("Usuario")
        Dim Con As Clases.Conexion = Nothing
        Dim Sesion As Seguridad = Nothing
        Dim ds As New DataSet
        Dim dsA As New DataSet
        Dim liARCH As List(Of ARCHOfi) = New List(Of ARCHOfi)()
        Dim y As ULong = 0
        ' Dim leerShare As New LeerSharep
        Dim sqlGrid As String = String.Empty


        sqlGrid = _
 "SELECT top 50 [ID_FOLIO] ," + vbCrLf + _
 "CONVERT(VARCHAR(10),[FECH_RECEPCION], 103) AS [FECH_RECEPCION]" + vbCrLf + _
 ",[ID_UNIDAD_ADM] ," + vbCrLf + _
 "[DSC_NUM_OFICIO] ," + vbCrLf + _
 "[DSC_REFERENCIA] ," + vbCrLf + _
 "TDOC.DSC_T_DOC as [DSC_T_DOC] ," + vbCrLf + _
 " SUBSTRING(R.DSC_REMITENTE, 0, 30) AS [DSC_REMITENTE]," + vbCrLf + _
 "ARCHIVO as [DSC_ARCHIVO] ," + vbCrLf + _
 "[ESTATUS_RECIBIDO] ," + vbCrLf + _
 "[USUARIO_DESTINATARIO] as [DESTINATARIO] ," + vbCrLf + _
 "'' as [ORIGINAL_FLAG] ," + vbCrLf + _
 "[ESTATUS_ATENDIDA] ," + vbCrLf + _
 "CONVERT(VARCHAR(10),[FECH_DOC], 103) AS [FECH_DOC] , " + vbCrLf + _
 "CONVERT(VARCHAR(10), [FECH_REGISTRO], 103) AS  [FECH_REGISTRO] ," + vbCrLf + _
 "[DSC_ASUNTO] ," + vbCrLf + _
 "[NUMERO_ATENCION] as [NUM_ATENCION] ," + vbCrLf + _
 "[USUARIO_SISTEMA] as [USUARIO] ," + vbCrLf + _
 "'' as [NOMBRE] ," + vbCrLf + _
 "'' as [ATENDIDA] ," + vbCrLf + _
 "'' as [DxV] ," + vbCrLf + _
 "'' as [FECHA_LIMITE] ," + vbCrLf + _
 "'' as [FECHA_LIMITE_IMG] ," + vbCrLf + _
 "[ARCHIVO_SBM] ," + vbCrLf + _
 "'' as [ARCHIVO_SIE] ," + vbCrLf + _
 "[CORREO_SIE] ," + vbCrLf + _
 "'' as [INSTRUCCIONES] ," + vbCrLf + _
 "[TURNADO_FLAG] ," + vbCrLf + _
 "'' as [ID_EXPEDIENTE] ," + vbCrLf + _
 "'' as [DUPLICADO_FLAG] ," + vbCrLf + _
 "'' as [RESPONSABLE] ," + vbCrLf + _
 "[TURNADO_FLAG] as [ID_G_TURNADO] ," + vbCrLf + _
 "'' as [USUARIO_RESPONSABLE] ," + vbCrLf + _
 "'' as [BLOQ_TURNADO] ," + vbCrLf + _
 "'' as [FECH_INICIO] ," + vbCrLf + _
 "[USUARIO_SISTEMA] ," + vbCrLf + _
 "'' as [ESTATUS_TRAMITE] " + vbCrLf + _
 "FROM [BDA_INFO_DOC] A" + vbCrLf + _
 "JOIN BDA_C_REMITENTE R ON A.ID_REMITENTE = R.ID_REMITENTE" + vbCrLf + _
 "JOIN BDA_C_T_DOC TDOC ON A.ID_T_DOC = TDOC.ID_T_DOC " + vbCrLf + _
 "WHERE 1=1 "





        sqlGrid &= Filtros.GetWhereQuery()

        Try
            Con = New Clases.Conexion()
            Sesion = New Seguridad



            If Session("ConsultaPrincipal") Is Nothing Then
                Sesion.BitacoraInicia("Filtro de búsqueda ", Con)
                ds = Con.Datos(sqlGrid, True)
                AgregarColumnasAnexo(ds)
            Else
                ds = DirectCast(Session("ConsultaPrincipal"), DataSet)
                ds.Tables(0).DefaultView.RowFilter = ""
            End If


            Session("ConsultaPrincipal") = ds

            Session("dtAnexos") = New Collections.Generic.Dictionary(Of String, String)
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
            ModalMensaje("El tiempo de Espera ha caducado favor de cerrar la ventana e intentar mas tarde")
        Finally
            If Not Con Is Nothing Then
                Con.Cerrar()
            End If
            Con = Nothing
            Session("tusaurio") = Nothing
            Session("Recibido") = Nothing
            Session("fechaRegistro") = Nothing
            Session("fechaLimite") = Nothing
            Imagen_procesando.Style.Add("display", "none")
            GRID.Style.Add("visibility", "visible")
        End Try




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

                'If lblCorreosSIE.Text = "No" Then
                '    _dataItem.Attributes.Add("onMouseOver", "cambio(this);")
                'Else
                '    _dataItem.Attributes.Add("onMouseOver", "cambioSIE(this);")
                'End If

                '_dataItem.Attributes.Add("onMouseOut", "cambio2(this);")

                '_dataItem.Attributes.Add("class", "hotspot")

                If lblCorreosSIE.Text = "Si" Then

                    _dataItem.Attributes.Add("class", "tr_odd2")


                End If


                'Folio = _dataItem.Cells(1).Text
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


    ''    Private Sub BtnFiltrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFiltrar.Click
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









    Private Sub ModalMensaje(ByVal MesajeMostrar As String)
        ModalPopupExtenderErrores.Show()
        lblErroresTitulo.Visible = True
        lblErroresPopup.Visible = False
        lblRegistros.Text = "<ul><li>" + MesajeMostrar + "</li></ul>"
        lblRegistros.Style.Add("display", "block")
    End Sub

    Private Sub GridPrincipal_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridPrincipal.DeleteCommand
        'Selecciona el registro
        Dim Index As Integer
        Try
            'NO_OFICIO.Text = e.Item.Cells(5).Text
            'FECHA_DOCUMENTO.Text = e.Item.Cells(36).Text
            'FECHA_RECEPCION.Text = e.Item.Cells(3).Text
            'ID_FOLIO.Text = e.Item.Cells(37).Text
            Index = e.Item.ItemIndex

            Session("NO_OFICIOENTRADA") = Nothing
            Session("FECHA_DOCUMENTOENTRADA") = Nothing
            Session("FECHA_RECEPCIONENTRADA") = Nothing
            Session("DOCUMENTO") = Nothing
            Session("TIPODOCENTRADA") = Nothing
            Session("ID_FOLIO_ENTRADA") = Nothing
            Session.Remove("NO_OFICIOENTRADA")
            Session.Remove("FECHA_DOCUMENTOENTRADA")
            Session.Remove("FECHA_RECEPCIONENTRADA")
            Session.Remove("DOCUMENTO")
            Session.Remove("TIPODOCENTRADA")
            Session.Remove("ID_FOLIO_ENTRADA")

            Dim doc As String = String.Empty
            Dim tipo As String = String.Empty
            tipo = traetipooficio(CInt(e.Item.Cells(37).Text))
            doc = traedoc(CInt(e.Item.Cells(37).Text))
            Session("NO_OFICIOENTRADA") = e.Item.Cells(5).Text
            Session("FECHA_DOCUMENTOENTRADA") = e.Item.Cells(36).Text
            Session("FECHA_RECEPCIONENTRADA") = e.Item.Cells(3).Text
            Session("DOCUMENTO") = doc
            Session("TIPODOCENTRADA") = tipo
            Session("ID_FOLIO_ENTRADA") = e.Item.Cells(37).Text

            System.Web.UI.ScriptManager.RegisterStartupScript(source, source.GetType(), "cerrar", " window.close();", True)
        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_DeleteCommand")
        Finally
        End Try
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










    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
        ModalMensaje("Error: verifica con el Administrador")
    End Sub




    Private Function ValorVoBo(ByVal Datos As DataTable) As Integer

        Dim resultado As Integer = 0
        Dim ExisteVoBo As Boolean = False
        Dim ValidaVoBoCompara As Boolean = False
        Dim indice As Integer = 0

        For Each row As DataRow In Datos.Rows

            With row

                '' Validamos si podemos dar VoBo
                ExisteVoBo = LogicaNegocio.BusinessRules.BDA_TURNADO.ValidaVoBoTurnado(Convert.ToInt32(.Item("ID_G_TURNADO")), _
                                                                              Convert.ToInt32(.Item("ID_FOLIO")), _
                                                                              .Item("USUARIO_RESPONSABLE").ToString, _
                                                                              Convert.ToInt32(.Item("ORIGINAL_FLAG")), _
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






    Private Sub handleRowDoubleClick(ByVal strRequest As String)

        Session(LogicaNegocio.BusinessRules.BDA_C_EXPEDIENTE.SessionExpResult) = strRequest

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

    End Sub




    Public Function traedoc(ByVal oficio As Integer) As String
        Dim a As String = String.Empty
        Dim conSICOD As New Clases.Conexion()
        Dim SQL As String = _
     "SELECT ARCHIVO FROM BDA_INFO_DOC where ID_FOLIO=" & oficio & ""
        Try
            Dim dsTDoc As DataSet
            dsTDoc = conSICOD.Datos(SQL, False)
            If dsTDoc IsNot Nothing Then
                If dsTDoc.Tables(0).Rows.Count > 0 Then
                    a = dsTDoc.Tables(0).Rows(0).Item(0)
                Else
                    a = ""
                End If
            Else
                a = ""
            End If
        Catch ex As Exception
            catch_cone(ex, "traedoc()")
        Finally
            If conSICOD IsNot Nothing Then
                conSICOD.Cerrar()
                conSICOD = Nothing
            End If
        End Try
        Return a
    End Function
    Public Function traetipooficio(ByVal oficio As Integer) As String
        Dim a As String = String.Empty
        Dim conSICOD As New Clases.Conexion()
        Dim SQL As String = _
     "SELECT ID_T_DOC FROM BDA_INFO_DOC where ID_FOLIO=" & oficio & ""
        Try
            Dim dsTDoc As DataSet
            dsTDoc = conSICOD.Datos(SQL, False)
            If dsTDoc IsNot Nothing Then
                If dsTDoc.Tables(0).Rows.Count > 0 Then
                    a = dsTDoc.Tables(0).Rows(0).Item(0)
                Else
                    a = ""
                End If
            Else
                a = ""
            End If
        Catch ex As Exception
            catch_cone(ex, "traedoc()")
        Finally
            If conSICOD IsNot Nothing Then
                conSICOD.Cerrar()
                conSICOD = Nothing
            End If
        End Try
        Return a
    End Function

    Private Sub GridPrincipal_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GridPrincipal.ItemDataBound
        Try
            e.Item.Attributes("onDblClick") = String.Format("javascript:__doPostBack('GridPrincipal$ctl{0:00}$ctl00','')", e.Item.ItemIndex + 2) ' ClientScript.GetPostBackClientHyperlink(GridPrincipal, String.Format("$ctl{0,2}$ctl00", e.Item.ItemIndex), True)
        Catch ex As Exception
            catch_cone(ex, " GridPrincipal_ItemDataBound")
        End Try
    End Sub

   
End Class


Public Enum TipoFiltro
    FechaDeDocto = 0
    FechadeRecep = 1
    TipoDeDocto = 2
    Area = 3
    Destinatario = 4
    Recibido = 5
End Enum
Public Class ARCH
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
