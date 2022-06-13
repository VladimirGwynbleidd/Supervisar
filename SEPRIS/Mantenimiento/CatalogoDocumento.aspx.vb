Imports Entities
Imports System.Web.Configuration

Public Class CatalogoDocumento
    Inherits System.Web.UI.Page

    Public Property Mensaje As String
    Const IdentificadroGrid As Integer = 4

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            CargarFiltros()
            CargarCatalogo()
            CargarImagenesEstatus()
            CargarCombos()

            Session("fuFomatoDoc") = Nothing

            ''Validar solo lectura para areas diferentes a VJ y Precidencia
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID) 'se obtiene de la session
            If Not IsNothing(usuario) Then
                If (usuario.IdArea <> Constantes.AREA_VJ And usuario.IdArea <> Constantes.AREA_PR) Or usuario.IdentificadorPerfilActual = Constantes.PERFIL_SOLO_LEC Then
                    btnModificar.Visible = False
                    btnAgregar.Visible = False
                    btnEliminar.Visible = False
                End If
            End If

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = IdentificadroGrid
                PersonalizaColumnas.GridViewPersonalizar = gvConsultaDocs
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If
        Else
            'Respaldar fuFomatoDoc
            'If Session("fuFomatoDoc") Is Nothing And fuFomatoDoc.FileBytes.Length > 0 Then
            '    Session("fuFomatoDoc") = fuFomatoDoc
            'ElseIf Not Session("fuFomatoDoc") Is Nothing And fuFomatoDoc.FileBytes.Length <= 0 Then
            '    fuFomatoDoc = TryCast(Session("fuFomatoDoc"), FileUpload)
            'ElseIf fuFomatoDoc.FileBytes.Length > 0 Then
            '    Session("fuFomatoDoc") = fuFomatoDoc
            'End If
        End If

        HabilitaCajas()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        If Not IsNothing(Page) Then
            If Not IsNothing(Page.ClientScript) Then
                For argument As Integer = 0 To gvConsultaDocs.Rows.Count - 1
                    Page.ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
                Next
            End If
        End If

        gvConsultaDocs.ArmaMultiScript()
        MyBase.Render(writer)

    End Sub

    Private Sub buscaControles(lstControles As ControlCollection)
        ''NOMBRE DE LOS CONTROLES
        Dim lsIdFileUpload As String = ""

        ''BUSCA TODOS LOS CONTROLES DENTRO DE LA LISTA
        For Each lcControl As Control In lstControles
            ''COMPARA SI EL OBJETO QUE ECONTRO ES UN FILE UPLOAD
            If TypeOf lcControl Is FileUpload Then
                lsIdFileUpload = lcControl.ID
                Dim fileUpload As FileUpload = TryCast(Session(lsIdFileUpload), FileUpload)
                If Not fileUpload Is Nothing Then
                    Try
                        'BOTON IMAGEN QUE ELIMINA EL ARCHIVO CARGADO
                        Dim lstBtnImgCancel = From lcBntImg In lstControles Where lcBntImg.ID = "img" & lsIdFileUpload Select lcBntImg
                        Dim btnImgCancel As ImageButton = lstBtnImgCancel(0)
                        If Not btnImgCancel Is Nothing Then
                            btnImgCancel.Visible = True
                        End If

                        'BOTON LINK QUE MUESTRA EL ARCHIVO CARGADO
                        Dim lstBtnLnkFileUpload = From lcBntLnk In lstControles Where lcBntLnk.ID = "lnk" & lsIdFileUpload Select lcBntLnk
                        Dim btnLnkFileUpload As LinkButton = lstBtnLnkFileUpload(0)
                        If Not btnLnkFileUpload Is Nothing Then
                            btnLnkFileUpload.Text = fileUpload.FileName
                            btnLnkFileUpload.Visible = True
                        End If
                        fileUpload.Visible = False
                    Catch ex As Exception
                        Console.Out.Write(ex.Message)
                    End Try
                End If
            End If

            ''HACE UNA NUEVA LLAMADA SI ESE CONTROL TIENE MAS CONTROLES HIJOS
            If lcControl.Controls.Count > 0 Then
                buscaControles(lcControl.Controls)
            End If
        Next
    End Sub

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim dtDatosFiltro As DataSet

        If Not IsNothing(objUsuario) Then
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.DocumentosCat, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.DocumentosCat, "", 0)
        End If

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaBitDataSourceN, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("Heredar", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.HeredaSiNo, "Hereda", "N_FLAG_HEREDA", ucFiltro.DataValueType.BoolType, False, True, False, False, False, -1)
        ucFiltro1.AddFilter("Heredar Subvisitas", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.HeredaSiNoSbV, "Hereda", "N_FLAG_HEREDA_ENTRE_SBVISITA", ucFiltro.DataValueType.BoolType, False, True, False, False, False, -1)

        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_DOCUMENTO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        'ucFiltro1.AddFilter("Nombre del documento", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_NOM_DOCUMENTO_CAT", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 50)
        ucFiltro1.AddFilter("Nombre del documento", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "T_NOM_DOCUMENTO_CAT", "N_ID_DOCUMENTO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Fecha de registro", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, False, True, False, False, Date.Today.AddDays(-7), 50)
        ucFiltro1.AddFilter("Fecha de actualización", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECH_MODIFICACION", ucFiltro.DataValueType.StringType, False, False, True, False, False, Date.Today.AddDays(-7), 50)
        ucFiltro1.AddFilter("Fecha fin de vigencia", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECH_FIN_VIG", ucFiltro.DataValueType.StringType, False, False, True, False, False, Date.Today.AddDays(-7), 50)

        'ucFiltro1.AddFilter("Documento", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_NOM_MACHOTE_ORI", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 50)
        ucFiltro1.AddFilter("Documento", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(1), "T_NOM_MACHOTE_ORI", "T_NOM_MACHOTE_ORI", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Usuario Registro", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 16)
        ucFiltro1.AddFilter("Usuario Registro", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(2), "NOM_USUARIO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Usuario Actualizo", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_ID_USUARIO_MOD", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 16)
        ucFiltro1.AddFilter("Usuario Actualizo", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(3), "NOM_USUARIO", "T_ID_USUARIO_MOD", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Paso en que se solicita", ucFiltro.AcceptedControls.TextBox, Nothing, "", "I_ID_PASO_INI", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
      ucFiltro1.AddFilter("Rango de Pasos", ucFiltro.AcceptedControls.DropDownListR, dtDatosFiltro.Tables(4), "DSC_PASO", "I_ID_PASO_INI", ucFiltro.DataValueType.RangeType, False, False, False, , False, "", 50)

        ucFiltro1.LoadDDL("CatalogoDocumento.aspx")

    End Sub

    Private Sub CargarCatalogo()
        Dim area As New Entities.Area
        Dim dt As DataTable
        dt = AccesoBD.consultarDocumentosUsuario("", , , Constantes.Vigencia.NoConsideraVigencia)

        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                Dim dv As DataView = dt.DefaultView

                Dim consulta As String = "1=1"

                For Each filtro In ucFiltro1.getFilterSelection
                    consulta += " AND " + filtro
                Next

                dv.RowFilter = consulta

                gvConsultaDocs.DataSourceSession = dv.ToTable
                gvConsultaDocs.DataSource = dv.ToTable
                gvConsultaDocs.DataBind()
                MuestraGridView()
                btnExportaExcel.Visible = True
            Else
                btnExportaExcel.Visible = False
            End If
        Else
            btnExportaExcel.Visible = False
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        If ValidaInformacion() Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"

        If btnAceptar.CommandArgument = "Insertar" Then
            Dim errores As New Entities.EtiquetaError(1137)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1138)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Private Sub CargarCombos(Optional ByVal esConsulta As Boolean = False)
        Dim objPasos As New Paso

        Dim dt As DataTable = objPasos.getPasos()
        Utilerias.Generales.CargarCombo(ddlPasoIni, dt, "DESCRIP", "I_ID_PASO")
        Utilerias.Generales.CargarCombo(ddlPasoFin, dt, "DESCRIP", "I_ID_PASO")
        'Utilerias.Generales.CargarCombo(ddlPasoIniPdf, dt, "DESCRIP", "I_ID_PASO")
        'Utilerias.Generales.CargarCombo(ddlPasoFinPdf, dt, "DESCRIP", "I_ID_PASO")

        dt = Nothing
        Dim objDoc As New Documento

        dt = objDoc.getTipoDocumento()
        Utilerias.Generales.CargarCombo(ddlTipoDoc, dt, "DESCRIP", "ID")

        Utilerias.Generales.CargarCombo(ddlEstatusVig, Utilerias.Generales.VigenciaBitDataSourceSU(), "Vigencia", "B_FLAG_VIG")
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Documento"
        ''Id por default para especificar que se dara de alta un nuevo documento
        txtIdDoc.Text = "0"
        trId.Visible = False
        btnAceptar.CommandArgument = "Insertar"

        txtNomDoc.Text = ""
        txtNomCorto.Text = ""
        ddlTipoDoc.SelectedValue = "-1"
        txtOrden.Text = "0"

        'lnkfuFomatoDoc.Text = ""

        txtFechaIniVig.Text = ""
        chkVersiones.Checked = False
        txtNumVersiones.Text = "1"
        'txtNumVersiones.Enabled = False

        ddlPasoIni.SelectedValue = "-1"
        ddlPasoFin.SelectedValue = "-1"

        chkHeredar.Checked = False
        chkHeredarSub.Checked = False
        chkNomenclatura.Checked = False
        chkObligatorio.Checked = False
        chkSicod.Checked = False
        chkPdf.Checked = False
        chkConfirm.Checked = False

        'ddlPasoIniPdf.SelectedValue = "-1"
        'ddlPasoFinPdf.SelectedValue = "-1"

        ddlEstatusVig.SelectedValue = "-1"

        pnlRegistro.Visible = True
        pnlConsulta.Visible = False

        'Session("fuFomatoDoc") = Nothing

        HabilitaCajas()
    End Sub

    Private Function HayRegistroSeleccionado() As Boolean

        Dim haySeleccion As Boolean = False

        For Each row As GridViewRow In gvConsultaDocs.Rows

            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)

            If elemento.Checked Then

                haySeleccion = True
                Exit For

            End If

        Next

        If Not haySeleccion Then
            Dim errores As New Entities.EtiquetaError(1141)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Documento"

        If Not HayRegistroSeleccionado() Then
            Dim errores As New Entities.EtiquetaError(23)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsultaDocs.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim liIdDoc As Integer = 0

                If Int32.TryParse(gvConsultaDocs.DataKeys(row.RowIndex)("N_ID_DOCUMENTO").ToString(), liIdDoc) Then
                    Dim objExpediente As New Expediente(Constantes.Vigencia.NoConsideraVigencia, , , , liIdDoc)
                    Dim objDocumento As New Documento

                    If objExpediente.HayDocumentos Then
                        objDocumento = objExpediente.lstDocumentos().FirstOrDefault()
                        'Session("fuFomatoDoc") = Nothing

                        If Not IsNothing(objDocumento) Then
                            txtIdDoc.Text = objDocumento.N_ID_DOCUMENTO
                            txtNomDoc.Text = objDocumento.T_NOM_DOCUMENTO_CAT
                            txtNomCorto.Text = objDocumento.T_NOM_CORTO
                            ddlTipoDoc.SelectedValue = objDocumento.N_ID_TIPO_DOCUMENTO_ORI

                            'If objDocumento.T_NOM_MACHOTE_ORI <> "" Then
                            '    lnkfuFomatoDoc.Text = objDocumento.T_NOM_MACHOTE_ORI
                            '    lnkfuFomatoDoc.CommandArgument = objDocumento.T_NOM_MACHOTE_ACTUAL
                            'Else
                            '    lnkfuFomatoDoc.Text = ""
                            '    lnkfuFomatoDoc.CommandArgument = ""
                            'End If

                            txtFechaIniVig.Text = objDocumento.F_FECH_INI_VIG.ToString("dd/MM/yyyy")
                            chkVersiones.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_VERSIONADO)
                            txtNumVersiones.Text = objDocumento.N_NUM_VERSIONES
                            txtOrden.Text = objDocumento.N_NUM_ORDEN
                            ddlPasoIni.SelectedValue = objDocumento.I_ID_PASO_INI
                            ddlPasoFin.SelectedValue = objDocumento.I_ID_PASO_FIN

                            chkHeredar.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_HEREDA)
                            chkHeredarSub.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_HEREDA_ENTRE_SBVISITA)
                            chkNomenclatura.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_APLICA_NOMENCLATURA)
                            chkObligatorio.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_OBLI)
                            chkSicod.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_SICOD)
                            'chkPdf.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_PDF)
                            chkConfirm.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_CONFIRMACION)

                            'If chkPdf.Checked Then
                            '    ddlPasoIniPdf.SelectedValue = objDocumento.I_ID_PASO_PDF_INI
                            '    ddlPasoFinPdf.SelectedValue = objDocumento.I_ID_PASO_PDF_FIN
                            'End If

                            ddlEstatusVig.SelectedValue = objDocumento.N_FLAG_VIG

                            HabilitaCajas()
                            trId.Visible = True
                            btnAceptar.CommandArgument = "Modificar"
                        End If
                    End If

                End If

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False

                Exit For
            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(1139)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        btnAceptarM2B1A_Click(sender, e)
    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click

        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        Dim errores
        For Each row As GridViewRow In gvConsultaDocs.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim liIdDoc As Integer = 0

                If Int32.TryParse(gvConsultaDocs.DataKeys(row.RowIndex)("N_ID_DOCUMENTO").ToString(), liIdDoc) Then
                    Dim objExpediente As New Expediente(Constantes.Vigencia.NoConsideraVigencia, , , , liIdDoc)
                    Dim objDocumento As New Documento

                    If objExpediente.HayDocumentos Then
                        objDocumento = objExpediente.lstDocumentos().FirstOrDefault()
                        'Session("fuFomatoDoc") = Nothing

                        If Not IsNothing(objDocumento) Then
                            If objDocumento.N_FLAG_VIG = Constantes.Falso Then
                                Mensaje = "Este documento ya a sido eliminado."
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                                Exit Sub
                            Else
                                errores = New Entities.EtiquetaError(1140)
                                Mensaje = errores.Descripcion
                                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
        Next

        Mensaje = "Selecciona un documento."
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        Exit Sub
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"
                pnlControles.Enabled = True
                pnlBotones.Visible = True
                pnlRegresar.Visible = False
                pnlRegistro.Visible = False
                pnlConsulta.Visible = True
                'Session("fuFomatoDoc") = Nothing

            Case "btnAceptar"
                GuardaActualizaDocumento()

            Case "btnEliminar"

                For Each row As GridViewRow In gvConsultaDocs.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim liIdDoc As Integer = 0

                        If Int32.TryParse(gvConsultaDocs.DataKeys(row.RowIndex)("N_ID_DOCUMENTO").ToString(), liIdDoc) Then
                            Dim objExpediente As New Expediente(Constantes.Vigencia.NoConsideraVigencia, , , , liIdDoc)
                            Dim objDocumento As New Documento

                            If objExpediente.HayDocumentos Then
                                objDocumento = objExpediente.lstDocumentos().FirstOrDefault()

                                If Not IsNothing(objDocumento) Then
                                    objDocumento.N_FLAG_VIG = Constantes.Falso
                                    objDocumento.InsertarActualizarDocumento()
                                End If
                            End If
                        End If
                        Exit For
                    End If
                Next

                CargarCatalogo()
        End Select

    End Sub

    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/OK.gif"
        Else
            Return "~/Imagenes/Error.gif"
        End If

    End Function

    Public Function ObtenSM() As ScriptManager
        Return CType(Page.Master.FindControl("SM"), ScriptManager)
    End Function

    Protected Sub gvConsultaDocs_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsultaDocs.RowCreated
        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

            'Dim linkArchivo As LinkButton = CType(e.Row.FindControl("lnkArchivo"), LinkButton)

            'If Not IsNothing(linkArchivo) Then
            '    ObtenSM().RegisterPostBackControl(linkArchivo)
            'End If
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Documento"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))


        Dim liIdDoc As Integer = 0

        If Int32.TryParse(gvConsultaDocs.DataKeys(index)("N_ID_DOCUMENTO").ToString(), liIdDoc) Then
            Dim objExpediente As New Expediente(Constantes.Vigencia.NoConsideraVigencia, , , , liIdDoc)
            Dim objDocumento As New Documento

            If objExpediente.HayDocumentos Then
                objDocumento = objExpediente.lstDocumentos().FirstOrDefault()

                If Not IsNothing(objDocumento) Then
                    'Session("fuFomatoDoc") = Nothing

                    txtIdDoc.Text = objDocumento.N_ID_DOCUMENTO
                    txtNomDoc.Text = objDocumento.T_NOM_DOCUMENTO_CAT
                    txtNomCorto.Text = objDocumento.T_NOM_CORTO
                    ddlTipoDoc.SelectedValue = objDocumento.N_ID_TIPO_DOCUMENTO_ORI
                    txtOrden.Text = objDocumento.N_NUM_ORDEN

                    'If objDocumento.T_NOM_MACHOTE_ORI <> "" Then
                    '    lnkfuFomatoDoc.Text = objDocumento.T_NOM_MACHOTE_ORI
                    'Else
                    '    lnkfuFomatoDoc.Text = ""
                    'End If

                    txtFechaIniVig.Text = objDocumento.F_FECH_INI_VIG.ToString("dd/MM/yyyy")
                    chkVersiones.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_VERSIONADO)
                    txtNumVersiones.Text = objDocumento.N_NUM_VERSIONES

                    ddlPasoIni.SelectedValue = objDocumento.I_ID_PASO_INI
                    ddlPasoFin.SelectedValue = objDocumento.I_ID_PASO_FIN

                    chkHeredar.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_HEREDA)
                    chkHeredarSub.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_HEREDA_ENTRE_SBVISITA)
                    chkNomenclatura.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_APLICA_NOMENCLATURA)
                    chkObligatorio.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_OBLI)
                    chkSicod.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_SICOD)
                    'chkPdf.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_PDF)
                    chkConfirm.Checked = ConvertIntegerToBoolean(objDocumento.N_FLAG_CONFIRMACION)

                    'If chkPdf.Checked Then
                    '    ddlPasoIniPdf.SelectedValue = objDocumento.I_ID_PASO_PDF_INI
                    '    ddlPasoFinPdf.SelectedValue = objDocumento.I_ID_PASO_PDF_FIN
                    'End If

                    ddlEstatusVig.SelectedValue = objDocumento.N_FLAG_VIG

                    trId.Visible = True
                    pnlRegistro.Visible = True
                    pnlControles.Enabled = False
                    pnlBotones.Visible = False
                    pnlRegresar.Visible = True
                    pnlConsulta.Visible = False

                    HabilitaCajas()
                End If
            End If

        End If
    End Sub

    Private Sub MuestraGridView()
        If gvConsultaDocs.Rows.Count() > 0 Then
            gvConsultaDocs.Visible = True
            pnlNoExiste.Visible = False
        Else
            gvConsultaDocs.Visible = False
            pnlNoExiste.Visible = True
        End If
    End Sub

    Private Sub gvConsultaDocs_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsultaDocs.Sorting

        gvConsultaDocs.Ordenar(e)

    End Sub

    Private Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcel.Click

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsultaDocs.DataSourceSession, DataTable)

        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"
        dt.Columns("N_FLAG_HEREDA").ColumnName = "Heredar"
        dt.Columns("N_FLAG_HEREDA_ENTRE_SBVISITA").ColumnName = "Heredar Subvisitas"

        utl.ExportaGrid(dt, gvConsultaDocs, "Catalogo de Documentos", referencias)

        dt.Columns("Estatus").ColumnName = "N_FLAG_VIG"
        dt.Columns("Heredar").ColumnName = "N_FLAG_HEREDA"
        dt.Columns("Heredar Subvisitas").ColumnName = "N_FLAG_HEREDA_ENTRE_SBVISITA"
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = IdentificadroGrid
        PersonalizaColumnas.GridViewPersonalizar = gvConsultaDocs
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsultaDocs.DataSource = gvConsultaDocs.DataSourceSession
        gvConsultaDocs.DataBind()
    End Sub

    Protected Sub btnPersonalizaColumnas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPersonalizaColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = IdentificadroGrid
        PersonalizaColumnas.GridViewPersonalizar = gvConsultaDocs
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Private Function ConvertIntegerToBoolean(liBooleanInteger As Integer) As Boolean
        If liBooleanInteger = Constantes.Verdadero Then
            Return True
        ElseIf liBooleanInteger = Constantes.Falso Then
            Return False
        End If

        Throw New Exception("No es posible convertir a booleano el numero proporcionado.")
    End Function

    Private Function ConvertBooleanToInteger(liBooleanInteger As Boolean) As Integer
        If liBooleanInteger Then
            Return Constantes.Verdadero
        Else
            Return Constantes.Falso
        End If
    End Function

    Private Function ValidaInformacion() As Boolean
        Dim lbHayError As Boolean = False
        Dim lstErrores As New List(Of String)

        If txtNomDoc.Text.Trim().Length < 1 Then
            lbHayError = True
            AgregarError(2151, txtNomDoc.ID, lstErrores, txtNomDoc.Parent)
        Else
            QuitarError(txtNomDoc.ID, txtNomDoc.Parent)
        End If

        If txtNomCorto.Text.Trim().Length < 1 Then
            lbHayError = True
            AgregarError(2171, txtNomCorto.ID, lstErrores, txtNomCorto.Parent)
        Else
            QuitarError(txtNomCorto.ID, txtNomCorto.Parent)
        End If

        If ddlPasoIni.SelectedValue = "-1" Then
            lbHayError = True
            AgregarError(2158, ddlPasoIni.ID, lstErrores, ddlPasoIni.Parent)
        Else
            QuitarError(ddlPasoIni.ID, ddlPasoIni.Parent)

            If ddlPasoFin.SelectedValue = "-1" Then
                lbHayError = True
                AgregarError(2159, ddlPasoFin.ID, lstErrores, ddlPasoFin.Parent)
            Else
                QuitarError(ddlPasoFin.ID, ddlPasoFin.Parent)

                ''si ambos ya estan seleccinados
                Dim liPasoIni As Integer = ddlPasoIni.SelectedValue
                Dim liPasoFin As Integer = ddlPasoFin.SelectedValue
                If liPasoIni > liPasoFin Then
                    lbHayError = True
                    AgregarError(2160, ddlPasoFin.ID, lstErrores, ddlPasoFin.Parent)
                Else
                    QuitarError(ddlPasoFin.ID, ddlPasoFin.Parent)
                End If
            End If
        End If


        If ddlTipoDoc.SelectedValue = "-1" Then
            lbHayError = True
            AgregarError(2152, ddlTipoDoc.ID, lstErrores, ddlTipoDoc.Parent)
        Else
            QuitarError(ddlTipoDoc.ID, ddlTipoDoc.Parent)
        End If

        ''Valida el machote solo si es archivo word
        'If ddlTipoDoc.SelectedValue = Constantes.TipoArchivo.WORD Then
        '    If Not fuFomatoDoc.HasFile And lnkfuFomatoDoc.Text.Trim() = "" Then
        '        lbHayError = True
        '        AgregarError(2153, fuFomatoDoc.ID, lstErrores, fuFomatoDoc.Parent)
        '    Else
        '        ''Validar el tamanio
        '        If fuFomatoDoc.HasFile Then
        '            Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
        '            Dim lsExtArchivo As String = System.IO.Path.GetExtension(fuFomatoDoc.FileName)
        '            If fuFomatoDoc.FileBytes.Length > liLimiteArchivoCarga Then
        '                lbHayError = True
        '                AgregarError(2153, fuFomatoDoc.ID, lstErrores, fuFomatoDoc.Parent, "El archivo sobrepasa los " & (liLimiteArchivoCarga / 1024 / 1024).ToString() & " Mb permitidos, comuniquese al area de sistemas")
        '            Else
        '                ''Valida el tipo de archivo
        '                If (ddlTipoDoc.SelectedValue = Constantes.TipoArchivo.WORD And (lsExtArchivo <> ".doc" And lsExtArchivo <> ".docx")) Then
        '                    lbHayError = True
        '                    AgregarError(2153, fuFomatoDoc.ID, lstErrores, fuFomatoDoc.Parent, "Archivo WORD no válido.")
        '                Else
        '                    QuitarError(fuFomatoDoc.ID, fuFomatoDoc.Parent)
        '                End If
        '            End If
        '        End If
        '    End If
        'End If

        If txtFechaIniVig.Text.Trim().Length < 1 Then
            lbHayError = True
            AgregarError(2154, txtFechaIniVig.ID, lstErrores, txtFechaIniVig.Parent)
        Else
            Dim lbDateAux As Date
            If Not Date.TryParse(txtFechaIniVig.Text.Trim(), lbDateAux) Then
                Dim ldVecFecha As String() = txtFechaIniVig.Text.Trim().Split("/")

                If ldVecFecha.Length = 3 Then
                    Try
                        Dim ldFecha As Date = New Date(ldVecFecha(2), ldVecFecha(1), ldVecFecha(0))
                        QuitarError(txtFechaIniVig.ID, txtFechaIniVig.Parent)
                    Catch ex As Exception
                        lbHayError = True
                        AgregarError(2154, txtFechaIniVig.ID, lstErrores, txtFechaIniVig.Parent)
                    End Try
                Else
                    lbHayError = True
                    AgregarError(2154, txtFechaIniVig.ID, lstErrores, txtFechaIniVig.Parent)
                End If
            Else
                QuitarError(txtFechaIniVig.ID, txtFechaIniVig.Parent)
            End If
        End If

        If chkVersiones.Checked Then
            Dim liNumVer As Integer = 0
            If Not Int32.TryParse(txtNumVersiones.Text, liNumVer) Then
                lbHayError = True
                AgregarError(2155, txtNumVersiones.ID, lstErrores, txtNumVersiones.Parent)
            Else
                If liNumVer < 1 Or liNumVer > 5 Then
                    lbHayError = True
                    AgregarError(2155, txtNumVersiones.ID, lstErrores, txtNumVersiones.Parent)
                Else
                    QuitarError(txtNumVersiones.ID, txtNumVersiones.Parent)
                End If
            End If
        Else
            txtNumVersiones.Text = "1"  ''Si no hay versiones se predefine 1
            QuitarError(txtNumVersiones.ID, txtNumVersiones.Parent)
        End If

        'If chkPdf.Checked Then
        '    If ddlPasoIniPdf.SelectedValue = "-1" Then
        '        lbHayError = True
        '        AgregarError(2156, ddlPasoIniPdf.ID, lstErrores, ddlPasoIniPdf.Parent)
        '    Else
        '        QuitarError(ddlPasoIniPdf.ID, ddlPasoIniPdf.Parent)

        '        If ddlPasoFinPdf.SelectedValue = "-1" Then
        '            lbHayError = True
        '            AgregarError(2157, ddlPasoFinPdf.ID, lstErrores, ddlPasoFinPdf.Parent)
        '        Else
        '            QuitarError(ddlPasoFinPdf.ID, ddlPasoFinPdf.Parent)

        '            ''si ambos ya estan seleccinados
        '            Dim liPasoIni As Integer = ddlPasoIniPdf.SelectedValue
        '            Dim liPasoFin As Integer = ddlPasoFinPdf.SelectedValue
        '            If liPasoIni > liPasoFin Then
        '                lbHayError = True
        '                AgregarError(2160, ddlPasoFinPdf.ID, lstErrores, ddlPasoFinPdf.Parent)
        '            Else
        '                QuitarError(ddlPasoFinPdf.ID, ddlPasoFinPdf.Parent)
        '            End If
        '        End If
        '    End If
        'Else
        '    QuitarError(ddlPasoIniPdf.ID, ddlPasoIniPdf.Parent)
        '    QuitarError(ddlPasoFinPdf.ID, ddlPasoFinPdf.Parent)
        'End If

        Dim liOrden As Integer

        If Not Int32.TryParse(txtOrden.Text, liOrden) Then
            txtOrden.Text = "0"
        End If

        If ddlEstatusVig.SelectedValue = "-1" Then
            lbHayError = True
            AgregarError(2161, ddlEstatusVig.ID, lstErrores, ddlEstatusVig.Parent)
        Else
            QuitarError(ddlEstatusVig.ID, ddlEstatusVig.Parent)
        End If

        ''Respaldar el fileupload
        'If Session("fuFomatoDoc") Is Nothing And fuFomatoDoc.FileBytes.Length > 0 Then
        '    Session("fuFomatoDoc") = fuFomatoDoc
        'ElseIf Not Session("fuFomatoDoc") Is Nothing And fuFomatoDoc.FileBytes.Length <= 0 Then
        '    fuFomatoDoc = TryCast(Session("fuFomatoDoc"), FileUpload)
        'ElseIf fuFomatoDoc.FileBytes.Length > 0 Then
        '    Session("fuFomatoDoc") = fuFomatoDoc
        'End If

        Mensaje = "<ul>"
        For Each lsError As String In lstErrores
            Mensaje &= "<li>" & lsError & "</li>"
        Next
        Mensaje &= "</ul>"

        Return lbHayError
    End Function

    ''' <summary>
    ''' Agrega un error al formulario
    ''' </summary>
    ''' <param name="piIdError"></param>
    ''' <param name="idObjeto"></param>
    ''' <param name="lstErrores"></param>
    ''' <remarks></remarks>
    Private Sub AgregarError(piIdError As Integer, idObjeto As String,
                             Optional ByRef lstErrores As List(Of String) = Nothing,
                             Optional parent As Object = Nothing, Optional psDescripError As String = "")

        If psDescripError <> "" Then
            Dim lbError As Label

            If Not IsNothing(parent) Then
                lbError = parent.FindControl("lbl" & idObjeto)
                If Not IsNothing(lbError) Then
                    'lbError.Text = psDescripError
                    lbError.Visible = True
                End If
            End If

            If Not IsNothing(lstErrores) Then
                ''lstErrores.Add("ERROR[" & piIdError & "]: " & psDescripError)
                lstErrores.Add(psDescripError)
            End If
        Else
            Dim objError As New Entities.EtiquetaError(piIdError)
            Dim lbError As Label

            If Not IsNothing(parent) Then
                lbError = parent.FindControl("lbl" & idObjeto)
                If Not IsNothing(lbError) Then
                    'lbError.Text = objError.Descripcion
                    lbError.Visible = True
                End If
            End If

            If Not IsNothing(lstErrores) Then
                lstErrores.Add(objError.Descripcion)
            End If
        End If
    End Sub

    Private Sub QuitarError(idObjeto As String, parent As Object)
        Dim lbError As Label
        If Not IsNothing(parent) Then
            lbError = parent.FindControl("lbl" & idObjeto)
            If Not IsNothing(lbError) Then
                lbError.Visible = False
            End If
        End If
    End Sub

    Private Function ObtenerTamMaximoArch() As Integer
        'Obtener el maximo permitido
        Dim liLimiteArchivoCarga As Integer
        Try
            liLimiteArchivoCarga = CInt(WebConfigurationManager.AppSettings("LimiteTamArchivo").ToString())
        Catch
            liLimiteArchivoCarga = 49152000
        End Try
        Return liLimiteArchivoCarga
    End Function

    Private Sub ConfigurarSharePointSepris(ByRef Shp As Utilerias.SharePointManager)
        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS").ToString()
    End Sub

    'Protected Sub lnkfuFomatoDoc_Click(sender As Object, e As EventArgs)
    '    Dim lnkLink As LinkButton = CType(sender, LinkButton)
    '    If Not IsNothing(lnkLink) Then
    '        Try

    '            If IsNothing(Session("fuFomatoDoc")) Then
    '                ''El nombre real del documento en el sharepoint debe de llegar en el comandArgument
    '                Dim Shp As New Utilerias.SharePointManager
    '                Shp.NombreArchivo = lnkLink.CommandArgument

    '                ConfigurarSharePointSepris(Shp)

    '                Shp.VisualizarArchivoSepris()
    '            Else
    '                Dim cliente As System.Net.WebClient = New System.Net.WebClient
    '                Dim urlEncode As String = String.Empty
    '                Dim filename As String = String.Empty
    '                Dim Archivo() As Byte = Nothing
    '                Dim NombreArchivo As String = String.Empty

    '                Dim lsIdBtnFileUp As String = lnkLink.ID.Replace("lnk", "")
    '                If Not Session(lsIdBtnFileUp) Is Nothing Then
    '                    Dim btnFileUploader As FileUpload = TryCast(Session(lsIdBtnFileUp), FileUpload)
    '                    If Not btnFileUploader Is Nothing Then
    '                        NombreArchivo = btnFileUploader.FileName
    '                        Archivo = btnFileUploader.FileBytes
    '                        filename = "attachment; filename=" & NombreArchivo
    '                    End If
    '                End If

    '                Try
    '                    If Not Archivo Is Nothing Then

    '                        Dim tipo_arch As String = NombreArchivo.Substring(NombreArchivo.LastIndexOf(".") + 1)

    '                        Select Case tipo_arch
    '                            Case "pdf"
    '                                Response.ContentType = "application/pdf"
    '                            Case "csv"
    '                                Response.ContentType = "text/csv"
    '                            Case "doc"
    '                                Response.ContentType = "application/doc"
    '                            Case "docx"
    '                                Response.ContentType = "application/docx"
    '                            Case "xls"
    '                                Response.ContentType = "application/xls"
    '                            Case "xlsx"
    '                                Response.ContentType = "application/xlsx"
    '                            Case "txt"
    '                                Response.ContentType = "application/txt"
    '                            Case "ppt"
    '                                Response.ContentType = "application/vnd.ms-project"
    '                            Case "pptx"
    '                                Response.ContentType = "application/vnd.ms-project"
    '                            Case Else
    '                                Response.ContentType = "application/octet-stream"
    '                        End Select

    '                        Response.AddHeader("content-disposition", filename)

    '                        Response.BinaryWrite(Archivo)

    '                        Response.End()
    '                        '---------------------------------------------
    '                        ' No usamos HttpContext.Current.ApplicationInstance.CompleteRequest()
    '                        ' porque en archivos de texto (txt, csv, etc...) agregaba al final el código de la página.
    '                        '---------------------------------------------
    '                    End If
    '                Catch ex As ApplicationException
    '                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error)
    '                Catch ex As Threading.ThreadAbortException
    '                    '---------------------------------------------
    '                    ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
    '                    '---------------------------------------------
    '                Catch ex As Exception
    '                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error)
    '                End Try
    '            End If
    '        Catch ex As Exception
    '            'Se comento porque manda erroraun descargando el archivo de forma correcta
    '            Mensaje = "Ocurrio un error al recuperar el archivo."
    '        End Try
    '    End If
    'End Sub

    Protected Sub MostrarArchivo(sender As Object, e As EventArgs)
        Dim lnkLink As LinkButton = CType(sender, LinkButton)
        If Not IsNothing(lnkLink) Then
            Try
                ''El nombre real del documento en el sharepoint debe de llegar en el comandArgument
                ''Si no llega ahi, buscar el archivo mediante el nombre que hay en la propiedad text
                If lnkLink.CommandArgument.Length > 0 Then
                    Dim Shp As New Utilerias.SharePointManager
                    Shp.NombreArchivo = lnkLink.CommandArgument

                    ConfigurarSharePointSepris(Shp)

                    Shp.VisualizarArchivoSepris(lnkLink.Text)

                ElseIf lnkLink.Text.Length > 0 Then
                    Dim Shp As New Utilerias.SharePointManager
                    Shp.NombreArchivo = lnkLink.Text

                    ConfigurarSharePointSepris(Shp)

                    Shp.VisualizarArchivoSepris(lnkLink.Text)
                End If
            Catch ex As Exception
                'Se comento porque manda erroraun descargando el archivo de forma correcta
                'Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
                'Utilerias.ControlErrores.EscribirEvento("Ocurrio un error al recuperar el archivo.", EventLogEntryType.Error, "SEPRIS", ex.Message)
                Mensaje = "Ocurrio un error al recuperar el archivo."
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos();", True)
            End Try
        End If
    End Sub

    Private Sub GuardaActualizaDocumento()
        Dim objDocumento As New Documento
        Dim Shp As New Utilerias.SharePointManager

        'If fuFomatoDoc.HasFile Then
        '    ConfigurarSharePointSepris(Shp)

        '    '---------------------------------------
        '    ' Guarda el archivo en Sharepoint
        '    '---------------------------------------
        '    Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(fuFomatoDoc.FileName)
        '    Shp.BinFile = fuFomatoDoc.FileBytes

        '    If Not Shp.CargarDocumentoSepris() Then
        '        Mensaje = "No se pudo guardar el documento en SharePoint."
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        '        Exit Sub
        '    End If
        'End If

        ''Si se cargo correctamente el archivo se da de alta el nuevo documento
        objDocumento.N_ID_DOCUMENTO = txtIdDoc.Text
        objDocumento.T_NOM_DOCUMENTO_CAT = txtNomDoc.Text
        objDocumento.T_NOM_CORTO = txtNomCorto.Text
        objDocumento.N_ID_TIPO_DOCUMENTO_ORI = ddlTipoDoc.SelectedValue

        'If fuFomatoDoc.HasFile Then
        '    objDocumento.T_NOM_MACHOTE_ORI = fuFomatoDoc.FileName
        '    objDocumento.T_NOM_MACHOTE_ACTUAL = Shp.NombreArchivo
        'Else
        '    objDocumento.T_NOM_MACHOTE_ORI = lnkfuFomatoDoc.Text
        '    objDocumento.T_NOM_MACHOTE_ACTUAL = lnkfuFomatoDoc.CommandArgument
        'End If
        
        objDocumento.F_FECH_INI_VIG = CDate(txtFechaIniVig.Text)
        objDocumento.N_FLAG_VERSIONADO = ConvertBooleanToInteger(chkVersiones.Checked)
        objDocumento.N_NUM_VERSIONES = txtNumVersiones.Text
        objDocumento.I_ID_PASO_INI = ddlPasoIni.SelectedValue
        objDocumento.I_ID_PASO_FIN = ddlPasoFin.SelectedValue
        objDocumento.N_FLAG_HEREDA = ConvertBooleanToInteger(chkHeredar.Checked)
        objDocumento.N_FLAG_HEREDA_ENTRE_SBVISITA = ConvertBooleanToInteger(chkHeredarSub.Checked)
        objDocumento.N_FLAG_APLICA_NOMENCLATURA = ConvertBooleanToInteger(chkNomenclatura.Checked)
        objDocumento.N_FLAG_OBLI = ConvertBooleanToInteger(chkObligatorio.Checked)
        objDocumento.N_FLAG_SICOD = ConvertBooleanToInteger(chkSicod.Checked)
        objDocumento.N_FLAG_CONFIRMACION = ConvertBooleanToInteger(chkConfirm.Checked)
        objDocumento.N_FLAG_PDF = ConvertBooleanToInteger(chkPdf.Checked)
        'objDocumento.I_ID_PASO_PDF_INI = ddlPasoFinPdf.SelectedValue
        'objDocumento.I_ID_PASO_PDF_FIN = ddlPasoFinPdf.SelectedValue
        objDocumento.N_NUM_ORDEN = txtOrden.Text
        objDocumento.N_FLAG_VIG = ddlEstatusVig.SelectedValue

        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) 'se obtiene de la session
        If Not IsNothing(usuario) Then
            objDocumento.T_ID_USUARIO = usuario.IdentificadorUsuario
        End If



        If objDocumento.InsertarActualizarDocumento() Then
            CargarCatalogo()
            btnAceptarM2B1A.CommandArgument = "btnCancelar"
            btnAceptarM2B1A_Click(btnAceptarM2B1A, New EventArgs)
        Else
            Mensaje = "No se pudo Insertar/Actualizar el documento."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If
    End Sub

    Public Sub HabilitaCajas()
        If chkVersiones.Checked Then
            txtNumVersiones.Attributes.Remove("disabled")
            txtNumVersiones.Enabled = True
        Else
            txtNumVersiones.Enabled = False
        End If

        If chkPdf.Checked Then
            trPdf.Attributes.Remove("class")
            trPdf2.Attributes.Remove("class")
            trPdf3.Attributes.Remove("class")
        Else
            trPdf.Attributes.Remove("class")
            trPdf2.Attributes.Remove("class")
            trPdf3.Attributes.Remove("class")
            trPdf.Attributes.Add("class", "cssOculto")
            trPdf2.Attributes.Add("class", "cssOculto")
            trPdf3.Attributes.Add("class", "cssOculto")
        End If

        'If fuFomatoDoc.HasFile Or lnkfuFomatoDoc.Text <> "" Then
        '    If lnkfuFomatoDoc.Text = "" Then
        '        lnkfuFomatoDoc.Text = fuFomatoDoc.FileName
        '    End If

        '    lnkfuFomatoDoc.Attributes.Remove("class")
        '    imgfuFomatoDoc.Attributes.Remove("class")
        '    fuFomatoDoc.Attributes.Add("class", "cssOculto")
        'Else
        '    lnkfuFomatoDoc.Text = ""
        '    lnkfuFomatoDoc.Attributes.Add("class", "cssOculto")
        '    imgfuFomatoDoc.Attributes.Add("class", "cssOculto")
        '    fuFomatoDoc.Attributes.Remove("class")
        'End If
    End Sub
End Class