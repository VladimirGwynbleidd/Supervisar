Imports Entities

Public Class CatalogoPasosFlujo
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvImagen.ArmaMultiScript()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        For argument As Integer = 0 To gvConsulta.Rows.Count - 1
            ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
        Next

        MyBase.Render(writer)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarImagenesEstatus()
            CargarFiltros()
            CargarCatalogo()

        Else
            If pnlRegistro.Visible Then
                gvImagen.DataSource = gvImagen.DataSourceSession
                gvImagen.DataBind()
                gvImagen.CargaSeleccion()
                gvImagenInactiva.DataSource = gvImagenInactiva.DataSourceSession
                gvImagenInactiva.DataBind()
                gvImagenInactiva.CargaSeleccion()
            End If
        End If
    End Sub


#Region "Carga Datos"

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarCombos(Optional ByVal esConsulta As Boolean = False)
        Dim estatusSeervicio As New EstatusServicio
        Dim dvEstatusServicio As DataView = estatusSeervicio.ObtenerTodos()
        If Not esConsulta Then dvEstatusServicio.RowFilter = "B_FLAG_VIG = 1"
        Utilerias.Generales.CargarComboOrdenado(ddlEstatusServicio, dvEstatusServicio.ToTable, "T_DSC_ESTATUS_SERVICIO", "N_ID_ESTATUS_SERVICIO")
    End Sub

    Private Sub CargarFiltros()
        Dim estatusServicio As New EstatusServicio
        Dim dvEstatusServicio As DataView = estatusServicio.ObtenerTodos
        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSourceBit, "Vigencia", "B_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_PASO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Descripción Paso (Tooltip)", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_PASO_TOOLTIP", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Estatus asociado al paso", ucFiltro.AcceptedControls.DropDownList, dvEstatusServicio.ToTable, "T_DSC_ESTATUS_SERVICIO", "N_ID_ESTATUS_SERVICIO", ucFiltro.DataValueType.StringType, False, True, False)
        Dim tipo1 = New With {Key .B_FLAG_VISIBLE = 1, .descripcion = "Si"}
        Dim tipo2 = New With {Key .B_FLAG_VISIBLE = 0, .descripcion = "No"}
        Dim lstAutorizacion = New With {tipo1, tipo2}
        ucFiltro1.AddFilter("Imagen activa durante el flujo", ucFiltro.AcceptedControls.CheckBox, lstAutorizacion, "descripcion", "B_FLAG_VISIBLE", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.LoadDDL("CatalogoPasosFlujo.aspx")
    End Sub

    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim objPasoFlujo As New PasoFlujo
        Dim dv As DataView = objPasoFlujo.ObtenerTodos()

        dv.RowFilter = consulta

        gvConsulta.DataSource = dv.ToTable()
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If

    End Sub

    Private Sub CargaGridImagenes()

        Dim parametros As String = "N_FLAG_VIG=1"
        Dim tipoImagen As String = Conexion.SQLServer.Parametro.ObtenerValor("Tipos de Imagen Pasos de Flujo")

        If tipoImagen <> String.Empty Then
            parametros += " AND N_ID_TIPO_IMAGEN IN (" + tipoImagen + ")"
        End If


        Dim imagen As New Entities.Imagen
        Dim dv As DataView = imagen.ObtenerTodos()
        dv.RowFilter = parametros

        gvImagen.DataSource = dv.ToTable()
        gvImagenInactiva.DataSource = dv.ToTable
        gvImagen.DataBind()
        gvImagenInactiva.DataBind()
    End Sub

#End Region

#Region "Eventos Controles"

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Paso de Flujo"
        CargarCombos()
        Dim objPasoFlujo As New PasoFlujo()

        hfSelectedValueG1.Value = -1
        hfSelectedValueG2.Value = -1

        ibtnColorActual.Visible = True
        ibtnColorAnterior.Visible = True
        ibtnColorPosterior.Visible = True
        txtID.Text = objPasoFlujo.ObtenerSiguienteIdentificador.ToString
        txtPaso.Text = String.Empty
        txtColorActual.Text = String.Empty
        txtColorAnterior.Text = String.Empty
        txtColorPosterior.Text = String.Empty
        chkImagenActiva.Checked = False
        CargaGridImagenes()
        pnlRegistro.Visible = True
        pnlConsulta.Visible = False
        trImagenActual.Visible = False
        trImagenInactiva.Visible = False
        trImagenCatalogo.Style.Value = "display:block"
        trImagenInactivaCatalogo.Style.Value = "display:block"

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"
        Dim objPasoFlujo As New PasoFlujo(Convert.ToInt32(txtID.Text))

        If Not objPasoFlujo.Existe Then
            Dim errores As New Entities.EtiquetaError(1008)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1009)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Paso de Flujo"

        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1012)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objPasoFlujo As New PasoFlujo(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_PASO").ToString()))

        If Not objPasoFlujo.Vigente Then
            Dim errores As New Entities.EtiquetaError(1013)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        Else
            ibtnColorActual.Visible = True
            ibtnColorAnterior.Visible = True
            ibtnColorPosterior.Visible = True
            CargarCombos()
            txtID.Text = objPasoFlujo.Identificador
            txtPaso.Text = objPasoFlujo.Descripcion
            txtColorActual.Text = objPasoFlujo.ColorActual
            txtColorAnterior.Text = objPasoFlujo.ColorAnterior
            txtColorPosterior.Text = objPasoFlujo.ColorPosterior
            chkImagenActiva.Checked = objPasoFlujo.ImagenVisible
            CargaGridImagenes()

            For Each rowImagen As GridViewRow In gvImagen.Rows
                If objPasoFlujo.Imagen.Identificador = gvImagen.DataKeys(rowImagen.RowIndex)("N_ID_IMAGEN") Then
                    Dim elementoImagen As CheckBox = TryCast(rowImagen.FindControl("chkElemento"), CheckBox)
                    elementoImagen.Checked = True
                    hfSelectedValueG1.Value = rowImagen.RowIndex.ToString()
                End If
            Next
            For Each rowImagen As GridViewRow In gvImagenInactiva.Rows
                If objPasoFlujo.ImagenInactiva.Identificador = gvImagenInactiva.DataKeys(rowImagen.RowIndex)("N_ID_IMAGEN") Then
                    Dim elementoImagen As CheckBox = TryCast(rowImagen.FindControl("chkElemento"), CheckBox)
                    elementoImagen.Checked = True
                    hfSelectedValueG2.Value = rowImagen.RowIndex.ToString()
                End If
            Next
            gvImagen.CargaSeleccion()
            gvImagenInactiva.CargaSeleccion()

            Dim objEstatusAsociado As New EstatusServicio(objPasoFlujo.Estatus.Identificador)
            If objEstatusAsociado.Vigente Then
                ddlEstatusServicio.SelectedValue = objPasoFlujo.Estatus.Identificador
            Else
                ddlEstatusServicio.SelectedValue = "-1"
                Dim errores As New Entities.EtiquetaError(2094)
                Mensaje = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            End If

            pnlRegistro.Visible = True
            pnlConsulta.Visible = False

            trImagenActual.Visible = False
            trImagenInactiva.Visible = False
            trImagenCatalogo.Style.Value = "display:block"
            trImagenInactivaCatalogo.Style.Value = "display:block"
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(1010)
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

        Dim errores
        If gvConsulta.SelectedIndex = -1 Then
            errores = New Entities.EtiquetaError(1012)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objPasoFlujo As New PasoFlujo(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_PASO").ToString()))

        If Not objPasoFlujo.Vigente Then
            errores = New Entities.EtiquetaError(1013)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        errores = New Entities.EtiquetaError(1011)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)

    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True
                pnlBotones.Visible = True
                pnlRegresar.Visible = False

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True

            Case "btnAceptar"

                Dim objPasoFlujo As New PasoFlujo(Convert.ToInt32(txtID.Text))
                objPasoFlujo.Descripcion = txtPaso.Text
                objPasoFlujo.ColorActual = txtColorActual.Text
                objPasoFlujo.ColorAnterior = txtColorAnterior.Text
                objPasoFlujo.ColorPosterior = txtColorPosterior.Text
                objPasoFlujo.Estatus = New EstatusServicio(ddlEstatusServicio.SelectedValue)
                objPasoFlujo.ImagenVisible = chkImagenActiva.Checked
                objPasoFlujo.Imagen = New Entities.Imagen(gvImagen.DataKeys(gvImagen.SelectedIndex)("N_ID_IMAGEN"))
                objPasoFlujo.ImagenInactiva = New Entities.Imagen(gvImagenInactiva.DataKeys(gvImagenInactiva.SelectedIndex)("N_ID_IMAGEN"))

                If Not objPasoFlujo.Existe Then
                    objPasoFlujo.Identificador = objPasoFlujo.ObtenerSiguienteIdentificador()
                    objPasoFlujo.Agregar()
                Else
                    objPasoFlujo.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                Dim objPasoFlujo As New PasoFlujo(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_PASO").ToString()))
                objPasoFlujo.Baja()

                CargarCatalogo()

        End Select

    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Paso de Flujo"
        CargarCombos(True)
        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim objPasoFlujo As New PasoFlujo(CInt(gvConsulta.DataKeys(index)("N_ID_PASO").ToString()))

        ibtnColorActual.Visible = False
        ibtnColorAnterior.Visible = False
        ibtnColorPosterior.Visible = False
        txtID.Text = objPasoFlujo.Identificador.ToString
        txtPaso.Text = objPasoFlujo.Descripcion
        ddlEstatusServicio.SelectedValue = objPasoFlujo.Estatus.Identificador
        txtColorActual.Text = objPasoFlujo.ColorActual
        txtColorAnterior.Text = objPasoFlujo.ColorAnterior
        txtColorPosterior.Text = objPasoFlujo.ColorPosterior
        chkImagenActiva.Checked = objPasoFlujo.ImagenVisible
        imgActual.ImageUrl = "~/Imagenes/Errores/" + objPasoFlujo.Imagen.Ruta
        imgInactiva.ImageUrl = "~/Imagenes/Errores/" + objPasoFlujo.ImagenInactiva.Ruta
        trImagenActual.Visible = True
        trImagenInactiva.Visible = True
        trImagenCatalogo.Style.Value = "display:none"
        trImagenInactivaCatalogo.Style.Value = "display:none"

        If Not objPasoFlujo.Estatus.Vigente Then
            Dim errores As New Entities.EtiquetaError(2094)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If


        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("B_FLAG_VISIBLE").ColumnName = "Imagen activa durante el flujo"
        dt.Columns("B_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Pasos de Flujo", referencias)
    End Sub

    Private Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

#End Region

#Region "Metodos"
    Public Function ObtenerImagen(ByVal imagen As String) As String

        Return "~/Imagenes/Errores/" + imagen

    End Function


    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function
    Public Function ObtenerImagenVisible(ByVal visible As Boolean) As String
        If visible Then
            Return "Sí"
        Else
            Return "No"
        End If
    End Function

    Public Function ValidarColor(ByVal color As String) As Boolean
        Dim valid As Boolean = False
        If color.Length = 6 Then
            For Each caracter As Char In color.ToUpper.ToCharArray
                Select Case caracter
                    Case CChar("A") To CChar("F")
                        valid = True
                    Case CChar("0") To CChar("9")
                        valid = True
                    Case Else
                        Return False
                End Select
            Next
        End If
        Return valid
    End Function
#End Region

#Region "Validaciones"

    Private Sub cvPaso_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvPaso.ServerValidate
        If txtPaso.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1003)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvImagen_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvImagen.ServerValidate
        If gvImagen.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1007)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvImagenInactiva_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvImagenInactiva.ServerValidate
        If gvImagenInactiva.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(2086)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvColorAnterior_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvColorAnterior.ServerValidate
        If txtColorAnterior.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1004)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf Not ValidarColor(txtColorAnterior.Text.Trim) Then
            Dim errores As New Entities.EtiquetaError(1014)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvColorActual_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvColorActual.ServerValidate
        If txtColorActual.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1005)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf Not ValidarColor(txtColorActual.Text.Trim) Then
            Dim errores As New Entities.EtiquetaError(1015)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvColorPosterior_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvColorPosterior.ServerValidate
        If txtColorPosterior.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1006)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf Not ValidarColor(txtColorPosterior.Text.Trim) Then
            Dim errores As New Entities.EtiquetaError(1016)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvEstatusServicio_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvEstatusServicio.ServerValidate
        If ddlEstatusServicio.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(1002)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub
#End Region

End Class