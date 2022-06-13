Public Class CatalogoImagen
    Inherits System.Web.UI.Page


    Public Property Mensaje As String

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim Form As HtmlForm = Page.Form
        If Form.Enctype.Length = 0 Then
            Form.Enctype = "multipart/form-data"
        End If
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        For argument As Integer = 0 To gvConsulta.Rows.Count - 1
            ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            CargarFiltros()
            CargarCatalogo()
            CargarImagenesEstatus()

        End If

    End Sub

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        Dim tipoImagen As New Entities.TipoImagen
        Dim dvTiposImagen As DataView = tipoImagen.ObtenerTodos

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_IMAGEN", ucFiltro.DataValueType.IntegerType, False, False, False, False, , , 3)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_IMAGEN", ucFiltro.DataValueType.StringType, False, True, False, , , , 100)
        ucFiltro1.AddFilter("Tipo de Imagen", ucFiltro.AcceptedControls.DropDownList, dvTiposImagen.ToTable, "T_DSC_TIPO_IMAGEN", "N_ID_TIPO_IMAGEN", ucFiltro.DataValueType.StringType, False, False, True, False)
        ucFiltro1.LoadDDL("CatalogoImagen.aspx")


    End Sub

    Private Sub CargarCatalogo()
        Dim imagen As New Entities.Imagen
        Dim dv As DataView = imagen.ObtenerTodos
        Dim consulta As String = "1=1"

        For Each filtro In ucFiltro1.getFilterSelection

            consulta += " AND " + filtro

        Next

        dv.RowFilter = consulta

        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()
        MuestraGridViewImagen()
    End Sub

    Private Sub CargarCombos(Optional ByVal esConsulta As Boolean = False)
        Dim tipoImagen = New Entities.TipoImagen
        Dim dvTipoImagen As DataView = tipoImagen.ObtenerTodos
        If Not esConsulta Then dvTipoImagen.RowFilter = "B_FLAG_VIG = 1"
        Utilerias.Generales.CargarComboOrdenado(ddlTipoImagen, dvTipoImagen.ToTable, "T_DSC_TIPO_IMAGEN", "N_ID_TIPO_IMAGEN")
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub

        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"

        Session("fuimagen") = fuImagen

        Dim imagen As New Entities.Imagen(txtClave.Text)

        If Not imagen.Existe Then
            Dim errores As New Entities.EtiquetaError(125)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(126)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Private Sub SubirImagen(ByVal fileName)

        fuImagen.SaveAs(fileName)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Imagen"
        cvFileUpload.Enabled = True

        Dim imagen As New Entities.Imagen
        CargarCombos()

        txtClave.Text = imagen.ObtenerSiguienteIdentificador()
        txtDescripcion.Text = String.Empty

        pnlRegistro.Visible = True
        pnlConsulta.Visible = False


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
            Dim errores As New Entities.EtiquetaError(129)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Imagen"
        cvFileUpload.Enabled = False

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If
        CargarCombos()
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim imagen As New Entities.Imagen(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_IMAGEN").ToString()))

                If Not imagen.Vigente Then
                    Dim errores As New Entities.EtiquetaError(130)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                Dim objTipoImagen As New Entities.TipoImagen(imagen.Tipo)

                If Not objTipoImagen.Vigente Then
                    ddlTipoImagen.SelectedValue = "-1"
                    Dim errores As New Entities.EtiquetaError(175)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                Else
                    ddlTipoImagen.SelectedValue = imagen.Tipo
                End If

                txtClave.Text = imagen.Identificador
                txtDescripcion.Text = imagen.Descripcion
                trImagenActual.Visible = True
                imgActual.ImageUrl = "~/Imagenes/Errores/" + imagen.Ruta

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False

                Exit For

            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(127)
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

        Dim errores As Entities.EtiquetaError
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim imagen As New Entities.Imagen(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_IMAGEN").ToString()))

                If Not imagen.Vigente Then
                    errores = New Entities.EtiquetaError(130)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next
        errores = New Entities.EtiquetaError(128)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click


        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True
                trImagenActual.Visible = False
                trImagen.Visible = True

                pnlBotones.Visible = True
                pnlRegresar.Visible = False

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True



            Case "btnAceptar"

                Dim path As String = Server.MapPath("/Imagenes/Errores/")

                Dim archivo As FileUpload = CType(Session("fuimagen"), FileUpload)

                Dim fileName As String = txtClave.Text + "Error" + System.IO.Path.GetExtension(archivo.FileName)

                Dim imagen As New Entities.Imagen(txtClave.Text)
                imagen.Descripcion = txtDescripcion.Text
                imagen.Tipo = ddlTipoImagen.SelectedValue

                'SubirImagen(path + fileName)
                If archivo.HasFile Then
                    imagen.Ruta = fileName
                    archivo.SaveAs(path + fileName)
                End If

                If Not imagen.Existe Then
                    imagen.Agregar()
                Else
                    imagen.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim imagen As New Entities.Imagen(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_IMAGEN").ToString()))

                        imagen.Baja()


                        Exit For

                    End If
                Next

                CargarCatalogo()

        End Select

    End Sub

    Public Function ObtenerImagen(ByVal imagen As String) As String

        Return "~/Imagenes/Errores/" + imagen

    End Function

    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/OK.gif"
        Else
            Return "~/Imagenes/Error.gif"
        End If

    End Function

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated

        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then

            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

        End If


    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Imagen"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim imagen As New Entities.Imagen(CInt(gvConsulta.DataKeys(index)("N_ID_IMAGEN").ToString()))

        CargarCombos(True)

        txtClave.Text = imagen.Identificador
        txtDescripcion.Text = imagen.Descripcion
        ddlTipoImagen.SelectedValue = imagen.Tipo

        trImagenActual.Visible = True
        trImagen.Visible = False
        imgActual.ImageUrl = "~/Imagenes/Errores/" + imagen.Ruta

        Dim objTipoImagen As New Entities.TipoImagen(imagen.Tipo)

        If Not objTipoImagen.Vigente Then
            Dim errores As New Entities.EtiquetaError(175)
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

    Protected Sub cvDescripcion_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDescripcion.ServerValidate

        If txtDescripcion.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False

        End If

    End Sub


    Protected Sub cvTipoImagen_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvTipoImagen.ServerValidate
        If ddlTipoImagen.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(58)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvFileUpload_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvFileUpload.ServerValidate

        If Not fuImagen.HasFile Then
            Dim errores As New Entities.EtiquetaError(2)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False

        End If

    End Sub

    ''' <summary>
    ''' Dependiendo si el GridView no tiene registros muestra una imagen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraGridViewImagen()
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
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"
        dt.Columns("T_DSC_RUTA_IMAGEN").ColumnName = "Imagen"

        utl.ExportaGrid(dt, gvConsulta, "Catalogo de Imagenes", referencias)

    End Sub

End Class

