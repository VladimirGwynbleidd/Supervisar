Imports System.Web.Configuration
Imports Entities
Imports System.IO

Public Class CatalogoFormato
    Inherits System.Web.UI.Page

    Public Property Mensaje As String
    Const IdentificadroGrid As Integer = 5

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            CargarFiltros()
            CargarCatalogo()
            CargarImagenesEstatus()
            CargarCombos()

            Session("fuFomato") = Nothing

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
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If
        Else
            'Respaldar fuFomato
            If Session("fuFomato") Is Nothing And fuFomato.FileBytes.Length > 0 Then
                Session("fuFomato") = fuFomato
            ElseIf Not Session("fuFomato") Is Nothing And fuFomato.FileBytes.Length <= 0 Then
                fuFomato = TryCast(Session("fuFomato"), FileUpload)
            ElseIf fuFomato.FileBytes.Length > 0 Then
                Session("fuFomato") = fuFomato
            End If
        End If

        HabilitaCajas()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        If Not IsNothing(Page) Then
            If Not IsNothing(Page.ClientScript) Then
                For argument As Integer = 0 To gvConsulta.Rows.Count - 1
                    Page.ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
                Next
            End If
        End If

        gvConsulta.ArmaMultiScript()
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
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Formato, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Formato, "", 0)
        End If

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaBitDataSourceN, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, 1)
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_FORMATO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)

        ucFiltro1.AddFilter("Fecha de registro", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECH_REGISTRO", ucFiltro.DataValueType.StringType, False, False, True, False, False, Date.Today.AddDays(-7), 50)
        ucFiltro1.AddFilter("Fecha de actualización", ucFiltro.AcceptedControls.Calendar, Nothing, "", "F_FECH_MODIFICACION", ucFiltro.DataValueType.StringType, False, False, True, False, False, Date.Today.AddDays(-7), 50)

        ucFiltro1.AddFilter("Nombre del Formato", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(5), "T_NOM_FORMATO", "N_ID_FORMATO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Documento", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(4), "T_NOM_MACHOTE_ORI", "T_NOM_MACHOTE_ORI", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Usuario Registro", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(3), "NOM_USUARIO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Usuario Actualizo", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(2), "NOM_USUARIO", "T_ID_USUARIO_MOD", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Tipo de documento", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(1), "T_NOM_DOCUMENTO", "N_ID_DOCUMENTO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        If Not IsNothing(objUsuario) Then
            If objUsuario.IdArea <> Constantes.AREA_VJ And objUsuario.IdArea <> Constantes.AREA_PR Then
                Dim lrRowView As DataView = dtDatosFiltro.Tables(0).DefaultView
                lrRowView.RowFilter = "I_ID_AREA = " & objUsuario.IdArea

                ucFiltro1.AddFilter("Área", ucFiltro.AcceptedControls.DropDownList, lrRowView, "DscArea", "I_ID_AREA", ucFiltro.DataValueType.StringType, False, False, False, True, True, objUsuario.IdArea, 50)
            Else
                ucFiltro1.AddFilter("Área", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "DscArea", "I_ID_AREA", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
            End If
        End If

        ucFiltro1.LoadDDL("CatalogoFormatos.aspx")

    End Sub

    Private Sub CargarCatalogo()
        Dim area As New Entities.Area
        Dim dt As DataTable
        Dim objFormato As New Entities.Formato
        objFormato.N_ID_DOCUMENTO = Constantes.Todos
        objFormato.N_ID_FORMATO = Constantes.Todos
        objFormato.I_ID_AREA = Constantes.Todos

        dt = objFormato.getFormatos()

        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                Dim dv As DataView = dt.DefaultView

                Dim consulta As String = "1=1"

                For Each filtro In ucFiltro1.getFilterSelection
                    consulta += " AND " + filtro
                Next

                dv.RowFilter = consulta

                gvConsulta.DataSourceSession = dv.ToTable
                gvConsulta.DataSource = dv.ToTable
                gvConsulta.DataBind()
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
        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim objPasos As New Paso
        Dim objDocumentos As New Documento
        Dim dtDatosFiltro As DataSet

        If Not IsNothing(objUsuario) Then
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Formato, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Formato, "", 0)
        End If

        Dim dt As DataTable = objDocumentos.getDocumentos(-1, Constantes.Vigencia.Vigente)

        Utilerias.Generales.CargarCombo(ddlDocumentos, dt, "T_NOM_DOCUMENTO_CAT", "N_ID_DOCUMENTO")
        Utilerias.Generales.CargarCombo(ddlArea, dtDatosFiltro.Tables(0), "DscArea", "I_ID_AREA")
        Utilerias.Generales.CargarCombo(ddlEstatusVig, Utilerias.Generales.VigenciaBitDataSourceSU(), "Vigencia", "B_FLAG_VIG")
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de nuevo Formato"
        ''Id por default para especificar que se dara de alta un nuevo documento
        txtIdFormato.Text = "0"
        trId.Visible = False
        btnAceptar.CommandArgument = "Insertar"

        txtNomFormato.Text = ""

        lnkfuFomato.Text = ""

        ddlEstatusVig.SelectedValue = "-1"

        pnlRegistro.Visible = True
        pnlConsulta.Visible = False

        Session("fuFomato") = Nothing

        ddlDocumentos.SelectedValue = "-1"
        ddlArea.SelectedValue = "-1"

        HabilitaCajas()
    End Sub

    Private Function HayRegistroSeleccionado() As Boolean

        Dim haySeleccion As Boolean = False

        For Each row As GridViewRow In gvConsulta.Rows

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

        lblTituloRegistro.Text = "Modificación de Formato"

        If Not HayRegistroSeleccionado() Then
            Dim errores As New Entities.EtiquetaError(23)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim liIdFormato As Integer = 0

                If Int32.TryParse(gvConsulta.DataKeys(row.RowIndex)("N_ID_FORMATO").ToString(), liIdFormato) Then
                    Dim objFormato As New Entities.Formato(liIdFormato)

                    If objFormato.ExisteFormato Then
                        Session("fuFomato") = Nothing

                        If Not IsNothing(objFormato) Then
                            CargaFormulario(objFormato)

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
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim liIdFormato As Integer = 0
                Dim liIdEstatus As Integer = 0

                If Int32.TryParse(gvConsulta.DataKeys(row.RowIndex)("N_ID_FORMATO").ToString(), liIdFormato) Then
                    If Not Int32.TryParse(gvConsulta.DataKeys(row.RowIndex)("N_FLAG_VIG").ToString(), liIdEstatus) Then
                        liIdEstatus = 1
                    End If

                    Session("fuFomato") = Nothing

                    If liIdEstatus = Constantes.Falso Then
                        Mensaje = "Este formato ya a sido eliminado."
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
        Next

        Mensaje = "Selecciona un formato."
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
                Session("fuFomato") = Nothing

            Case "btnAceptar"
                GuardaActualizaFormato()

            Case "btnEliminar"

                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim liIdFormato As Integer = 0

                        If Int32.TryParse(gvConsulta.DataKeys(row.RowIndex)("N_ID_FORMATO").ToString(), liIdFormato) Then

                            Dim objFormato As New Entities.Formato(liIdFormato)

                            If objFormato.ExisteFormato Then
                                If Not IsNothing(objFormato) Then
                                    objFormato.N_FLAG_VIG = Constantes.Falso
                                    objFormato.InsertarActualizarFormato()
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

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

            Dim linkArchivo As LinkButton = CType(e.Row.FindControl("lnkArchivo"), LinkButton)

            If Not IsNothing(linkArchivo) Then
                ObtenSM().RegisterPostBackControl(linkArchivo)
            End If
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Formato"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))


        Dim liIdFormato As Integer = 0
        Dim liIdEstatusVig As Integer = 0

        If Int32.TryParse(gvConsulta.DataKeys(index)("N_ID_FORMATO").ToString(), liIdFormato) Then
            Dim objFormato As New Entities.Formato(liIdFormato)

            If objFormato.ExisteFormato Then

                If Not IsNothing(objFormato) Then
                    Session("fuFomato") = Nothing

                    CargaFormulario(objFormato)

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
        If gvConsulta.Rows.Count() > 0 Then
            gvConsulta.Visible = True
            pnlNoExiste.Visible = False
        Else
            gvConsulta.Visible = False
            pnlNoExiste.Visible = True
        End If
    End Sub

    Private Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting

        gvConsulta.Ordenar(e)

    End Sub

    Private Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcel.Click

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)

        If Not IsNothing(dt.Columns("N_FLAG_VIG")) Then dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"
        If Not IsNothing(dt.Columns("T_NOM_MACHOTE_ORI")) Then dt.Columns("T_NOM_MACHOTE_ORI").ColumnName = "Archivo"

        utl.ExportaGrid(dt, gvConsulta, "Catalogo de Formatos", referencias)

        If Not IsNothing(dt.Columns("Estatus")) Then dt.Columns("Estatus").ColumnName = "N_FLAG_VIG"
        If Not IsNothing(dt.Columns("Archivo")) Then dt.Columns("Archivo").ColumnName = "T_NOM_MACHOTE_ORI"
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = IdentificadroGrid
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

    Protected Sub btnPersonalizaColumnas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPersonalizaColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = IdentificadroGrid
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
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

        If txtNomFormato.Text.Trim().Length < 1 Then
            lbHayError = True
            AgregarError(2151, txtNomFormato.ID, lstErrores, txtNomFormato.Parent)
        Else
            QuitarError(txtNomFormato.ID, txtNomFormato.Parent)
        End If


        ''Valida el machote solo si es archivo word
        If Not fuFomato.HasFile And lnkfuFomato.Text.Trim() = "" Then
            lbHayError = True
            AgregarError(2153, fuFomato.ID, lstErrores, fuFomato.Parent)
        Else
            ''Validar el tamanio
            If fuFomato.HasFile Then
                Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
                Dim lsExtArchivo As String = System.IO.Path.GetExtension(fuFomato.FileName)
                If fuFomato.FileBytes.Length > liLimiteArchivoCarga Then
                    lbHayError = True
                    AgregarError(2153, fuFomato.ID, lstErrores, fuFomato.Parent, "El archivo sobrepasa los " & (liLimiteArchivoCarga / 1024 / 1024).ToString() & " Mb permitidos, comuniquese al area de sistemas")
                Else
                    QuitarError(fuFomato.ID, fuFomato.Parent)
                End If
            End If
        End If


        ''Respaldar el fileupload
        If Session("fuFomato") Is Nothing And fuFomato.FileBytes.Length > 0 Then
            Session("fuFomato") = fuFomato
        ElseIf Not Session("fuFomato") Is Nothing And fuFomato.FileBytes.Length <= 0 Then
            fuFomato = TryCast(Session("fuFomato"), FileUpload)
        ElseIf fuFomato.FileBytes.Length > 0 Then
            Session("fuFomato") = fuFomato
        End If

        If ddlDocumentos.SelectedValue = "-1" Then
            lbHayError = True
            AgregarError(2172, ddlDocumentos.ID, lstErrores, ddlDocumentos.Parent)
        Else
            QuitarError(ddlDocumentos.ID, ddlDocumentos.Parent)
        End If

        If ddlArea.SelectedValue = "-1" Then
            lbHayError = True
            AgregarError(2173, ddlArea.ID, lstErrores, ddlArea.Parent)
        Else
            QuitarError(ddlArea.ID, ddlArea.Parent)
        End If

        If ddlEstatusVig.SelectedValue = "-1" Then
            lbHayError = True
            AgregarError(2174, ddlEstatusVig.ID, lstErrores, ddlEstatusVig.Parent)
        Else
            QuitarError(ddlEstatusVig.ID, ddlEstatusVig.Parent)
        End If

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

    Protected Sub lnkfuFomato_Click(sender As Object, e As EventArgs)
        Dim lnkLink As LinkButton = CType(sender, LinkButton)
        If Not IsNothing(lnkLink) Then
            Try

                If IsNothing(Session("fuFomato")) Then
                    ''El nombre real del documento en el sharepoint debe de llegar en el comandArgument
                    Dim Shp As New Utilerias.SharePointManager
                    Shp.NombreArchivo = lnkLink.CommandArgument

                    ConfigurarSharePointSepris(Shp)

                    Shp.VisualizarArchivoSepris(lnkLink.Text)
                Else
                    Dim cliente As System.Net.WebClient = New System.Net.WebClient
                    Dim urlEncode As String = String.Empty
                    Dim filename As String = String.Empty
                    Dim Archivo() As Byte = Nothing
                    Dim NombreArchivo As String = String.Empty

                    Dim lsIdBtnFileUp As String = lnkLink.ID.Replace("lnk", "")
                    If Not Session(lsIdBtnFileUp) Is Nothing Then
                        Dim btnFileUploader As FileUpload = TryCast(Session(lsIdBtnFileUp), FileUpload)
                        If Not btnFileUploader Is Nothing Then
                            NombreArchivo = btnFileUploader.FileName
                            Archivo = btnFileUploader.FileBytes
                            filename = "attachment; filename=" & NombreArchivo
                        End If
                    End If

                    Try
                        If Not Archivo Is Nothing Then

                            Dim tipo_arch As String = NombreArchivo.Substring(NombreArchivo.LastIndexOf(".") + 1)

                            Select Case tipo_arch
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
                                Case "txt"
                                    Response.ContentType = "application/txt"
                                Case "ppt"
                                    Response.ContentType = "application/vnd.ms-project"
                                Case "pptx"
                                    Response.ContentType = "application/vnd.ms-project"
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
                        Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error)
                    Catch ex As Threading.ThreadAbortException
                        '---------------------------------------------
                        ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
                        '---------------------------------------------
                    Catch ex As Exception
                        Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error)
                    End Try
                End If
            Catch ex As Exception
                'Se comento porque manda erroraun descargando el archivo de forma correcta
                Mensaje = "Ocurrio un error al recuperar el archivo."
            End Try
        End If
    End Sub

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

    Private Sub GuardaActualizaFormato()
        Dim objFormato As New Entities.Formato
        Dim Shp As New Utilerias.SharePointManager

        If fuFomato.HasFile Then
            ConfigurarSharePointSepris(Shp)

            '---------------------------------------
            ' Guarda el archivo en Sharepoint
            '---------------------------------------

            ''Obtiene nombre real del documento a como quedar en sharepoint
            Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(fuFomato.FileName)

            ''Guarda el archivo en el servidor de APP
            Dim lsRutaTemp As String = Path.GetTempPath()

            ''Lo elimina si existe
            EliminaArchivoTemporal(lsRutaTemp & fuFomato.FileName)

            Try
                fuFomato.SaveAs(lsRutaTemp & fuFomato.FileName)
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento("Faltan permisos para CREAR el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                    ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
            End Try

            If Not File.Exists(lsRutaTemp & fuFomato.FileName) Then
                Mensaje = "No se pudo guardar temporalmente el documento en Servidor Web."
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                Exit Sub
            End If

            Shp.RutaArchivo = lsRutaTemp
            Shp.NombreArchivoOri = fuFomato.FileName

            If Not Shp.UploadFileToSharePoint() Then
                ''Elimina el archivo en el servidor de APP
                EliminaArchivoTemporal(lsRutaTemp & fuFomato.FileName)
                Mensaje = "No se pudo guardar el documento en SharePoint."
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                Exit Sub
            Else
                ''Elimina el archivo en el servidor de APP
                EliminaArchivoTemporal(lsRutaTemp & fuFomato.FileName)
            End If
        End If


        ''Si se cargo correctamente el archivo se da de alta el nuevo formato
        objFormato.N_ID_FORMATO = txtIdFormato.Text
        objFormato.T_NOM_FORMATO = txtNomFormato.Text

        If fuFomato.HasFile Then
            objFormato.T_NOM_MACHOTE_ORI = fuFomato.FileName
            objFormato.T_NOM_MACHOTE_ACTUAL = Shp.NombreArchivo
        Else
            objFormato.T_NOM_MACHOTE_ORI = lnkfuFomato.Text
            objFormato.T_NOM_MACHOTE_ACTUAL = lnkfuFomato.CommandArgument
        End If

        objFormato.N_FLAG_VIG = ddlEstatusVig.SelectedValue

        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) 'se obtiene de la session
        If Not IsNothing(usuario) Then
            If txtIdFormato.Text = "0" Then 'SI ESTA INSERTANDO
                objFormato.T_ID_USUARIO = usuario.IdentificadorUsuario
            Else
                objFormato.T_ID_USUARIO_MOD = usuario.IdentificadorUsuario
            End If
        End If

        objFormato.I_ID_AREA = ddlArea.SelectedValue
        objFormato.N_ID_DOCUMENTO = ddlDocumentos.SelectedValue


        If objFormato.InsertarActualizarFormato() Then
            CargarCatalogo()
            btnAceptarM2B1A.CommandArgument = "btnCancelar"
            btnAceptarM2B1A_Click(btnAceptarM2B1A, New EventArgs)
        Else
            Mensaje = "No se pudo Insertar/Actualizar el formato."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If
    End Sub

    Private Sub EliminaArchivoTemporal(lsRutaTemp As String)
        If File.Exists(lsRutaTemp) Then
            Try
                File.Delete(lsRutaTemp)
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento("Faltan permisos para borrar el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                        ex.ToString(), EventLogEntryType.Error, "CatalogoFormato.aspx.vb, EliminaArchivoTemporal", "")
            End Try
        End If

    End Sub
    Public Sub HabilitaCajas()

        If fuFomato.HasFile Or lnkfuFomato.Text <> "" Then
            If lnkfuFomato.Text = "" Then
                lnkfuFomato.Text = fuFomato.FileName
            End If

            lnkfuFomato.Attributes.Remove("class")
            imgfuFomato.Attributes.Remove("class")
            fuFomato.Attributes.Add("class", "cssOculto")
        Else
            lnkfuFomato.Text = ""
            lnkfuFomato.Attributes.Add("class", "cssOculto")
            imgfuFomato.Attributes.Add("class", "cssOculto")
            fuFomato.Attributes.Remove("class")
        End If
    End Sub

    Private Sub CargaFormulario(objFormato As Formato)
        txtIdFormato.Text = objFormato.N_ID_FORMATO
        txtNomFormato.Text = objFormato.T_NOM_FORMATO

        If objFormato.T_NOM_MACHOTE_ORI <> "" Then
            lnkfuFomato.Text = objFormato.T_NOM_MACHOTE_ORI
        Else
            lnkfuFomato.Text = ""
        End If


        ddlEstatusVig.SelectedValue = objFormato.N_FLAG_VIG

        ddlArea.SelectedValue = objFormato.I_ID_AREA
        ddlDocumentos.SelectedValue = objFormato.N_ID_DOCUMENTO
    End Sub

End Class