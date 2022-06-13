Public Class CatalogoErrores
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

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
        Else
            If pnlRegistro.Visible Then
                CustomGridView1.DataSource = CustomGridView1.DataSourceSession
                CustomGridView1.DataBind()
                CustomGridView1.CargaSeleccion()
            End If
        End If
    End Sub

    Public Function ObtenerPaginacion() As Integer
        Return CInt(Conexion.SQLServer.Parametro.ObtenerValor("Paginación Errores"))
    End Function

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_ERROR", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 5)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_ERROR", ucFiltro.DataValueType.StringType, False, True, False, , , , 100)
        ucFiltro1.AddFilter("Escenario", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_ESCENARIO", ucFiltro.DataValueType.StringType, False, True, False, , , , 100)
        ucFiltro1.LoadDDL("CatalogoErrores.aspx")

    End Sub

    Private Sub CargarCatalogo()
        Dim er As New Entities.EtiquetaError
        Dim dv As DataView = er.ObtenerTodos
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

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub

        End If


        Try

            Utilerias.Generales.ValidaCamposCapturaHTML(txtDescripcion)
            Utilerias.Generales.ValidaCamposCapturaHTML(txtEscenario)

        Catch ex As ApplicationException

            Mensaje = ex.Message
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub

        End Try


        btnAceptarM2B1A.CommandArgument = "btnAceptar"

        Dim etiquetaError As New Entities.EtiquetaError(txtClave.Text)

        If Not etiquetaError.Existe Then
            Dim errores As New Entities.EtiquetaError(119)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(120)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub


    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        Dim parametros As String = "N_FLAG_VIG=1"

        Dim tipoImagen As String = Conexion.SQLServer.Parametro.ObtenerValor("Tipos de Imagen Error")
        parametros += " AND N_ID_TIPO_IMAGEN IN (" + tipoImagen + ")"

        hfSelectedValue.Value = "-1"
        lblTituloRegistro.Text = "Alta de Error"
        imgUnBotonNoAccion.ImageUrl = String.Empty

        Dim etiquetaError As New Entities.EtiquetaError
        Dim imagen As New Entities.Imagen

        txtClave.Text = etiquetaError.ObtenerSiguienteIdentificador()
        txtDescripcion.Text = String.Empty
        txtEscenario.Text = String.Empty

        Dim dv As DataView = imagen.ObtenerTodos()
        dv.RowFilter = parametros

        CustomGridView1.DataSource = dv.ToTable()
        CustomGridView1.DataBind()

        pnlRegistro.Visible = True
        pnlConsulta.Visible = False

        trImagenActual.Visible = False
        trImagenCatalogo.Style.Value = "display:block"

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
            Dim errores As New Entities.EtiquetaError(123)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Error"
        Dim parametros As String = "N_FLAG_VIG=1"

        Dim tipoImagen As String = Conexion.SQLServer.Parametro.ObtenerValor("Tipos de Imagen Error")
        parametros += " AND N_ID_TIPO_IMAGEN IN (" + tipoImagen + ")"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim etiquetaError As New Entities.EtiquetaError(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_ERROR").ToString()))
                Dim imagen As New Entities.Imagen()

                If Not etiquetaError.Vigente Then
                    Dim errores As New Entities.EtiquetaError(124)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                txtClave.Text = etiquetaError.Identificador
                txtDescripcion.Text = etiquetaError.Descripcion
                txtEscenario.Text = etiquetaError.Escenario

                Dim dv As DataView = imagen.ObtenerTodos()
                dv.RowFilter = parametros

                CustomGridView1.DataSource = dv.ToTable()
                CustomGridView1.DataBind()

                For Each rowImagen As GridViewRow In CustomGridView1.Rows
                    If etiquetaError.Imagen.Identificador = CustomGridView1.DataKeys(rowImagen.RowIndex)("N_ID_IMAGEN") Then
                        Dim elementoImagen As CheckBox = TryCast(rowImagen.FindControl("chkElemento"), CheckBox)
                        elementoImagen.Checked = True
                        hfSelectedValue.Value = rowImagen.RowIndex.ToString()
                    End If
                Next
                CustomGridView1.CargaSeleccion()

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False


                trImagenActual.Visible = False
                trImagenCatalogo.Style.Value = "display:block"

                Exit For

            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(121)
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

                Dim objError As New Entities.EtiquetaError(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_ERROR").ToString()))
                If Not objError.Vigente Then
                    errores = New Entities.EtiquetaError(124)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next
        errores = New Entities.EtiquetaError(122)
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

                Dim etiquetaError As New Entities.EtiquetaError(txtClave.Text)

                etiquetaError.Descripcion = txtDescripcion.Text
                etiquetaError.Escenario = txtEscenario.Text



                For Each row As GridViewRow In CustomGridView1.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then
                        etiquetaError.Imagen = New Entities.Imagen(CustomGridView1.DataKeys(row.RowIndex)("N_ID_IMAGEN"))
                        Exit For
                    End If
                Next


                If Not etiquetaError.Existe Then
                    etiquetaError.Agregar()

                Else
                    etiquetaError.Actualizar()

                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim etiquetaError As New Entities.EtiquetaError(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_ERROR").ToString()))

                        etiquetaError.Baja()

                        Exit For

                    End If
                Next

                CargarCatalogo()

        End Select

        imgUnBotonNoAccion.ImageUrl = String.Empty

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
        e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Error"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim etiquetaError As New Entities.EtiquetaError(CInt(gvConsulta.DataKeys(index)("N_ID_ERROR").ToString()))

        txtClave.Text = etiquetaError.Identificador
        txtDescripcion.Text = etiquetaError.Descripcion
        txtEscenario.Text = etiquetaError.Escenario

        Dim img As New Entities.Imagen(etiquetaError.Imagen.Identificador)

        imgActual.ImageUrl = "~/Imagenes/Errores/" + img.Ruta

        trImagenActual.Visible = True
        trImagenCatalogo.Style.Value = "display:none"


        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub



    Protected Sub cvDescripcion_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDescripcion.ServerValidate
        If txtDescripcion.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(3)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False

        End If
    End Sub

    Protected Sub cvEscenario_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvEscenario.ServerValidate
        If txtEscenario.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(4)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
            Exit Sub

        End If


        Dim imagenSeleccionada As Boolean = False
        For Each row As GridViewRow In CustomGridView1.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                imagenSeleccionada = True
                Exit For
            End If
        Next

        If Not imagenSeleccionada Then

            Dim errores As New Entities.EtiquetaError(5)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

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
            btnExportaExcel.Visible = True
        Else
            gvConsulta.Visible = False
            pnlNoExiste.Visible = True
            btnExportaExcel.Visible = False
        End If
    End Sub

    Protected Sub CustomGridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles CustomGridView1.Sorting
        CustomGridView1.Ordenar(e)
    End Sub

    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)

        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"
        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Errores", referencias)
    End Sub

    Protected Sub gvConsulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataBind()
    End Sub


    Protected Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub
End Class