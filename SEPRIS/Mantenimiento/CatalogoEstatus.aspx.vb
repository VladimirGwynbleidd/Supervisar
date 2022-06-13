' Fecha de creación: 28/07/2013
' Fecha de modificación: 
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' Página del catálogo de estatus


Public Class CatalogoEstatus
    Inherits Page

    Public Property Mensaje As String
    Public Property idEstatus As String

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)

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
                gvImagenes.DataSource = gvImagenes.DataSourceSession
                gvImagenes.DataBind()
                gvImagenes.CargaSeleccion()
            End If
        End If
    End Sub

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function

    Private Sub CargarCatalogo()

        Dim estatus As New Entities.Estatus
        gvConsulta.DataSourceSession = estatus.ObtenerTodos().ToTable
        gvConsulta.DataSource = estatus.ObtenerTodos().ToTable
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count > 0 Then
            btnExportaExcel.Visible = True
            gvConsulta.Visible = True
            pnlNoExiste.Visible = False
        Else
            btnExportaExcel.Visible = False
            gvConsulta.Visible = False
            pnlNoExiste.Visible = True
        End If

    End Sub

    Private Sub CargarFiltros()


        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource(), "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, False, False, True, True, -1)
        ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_ESTATUS", ucFiltro.DataValueType.IntegerType, False, False, False, True, , , 3)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_ESTATUS", ucFiltro.DataValueType.StringType, False, True, False, , , , 100)
        ucFiltro1.LoadDDL("CatalogoEstatus.aspx")

        btnFiltrar_Click(Nothing, Nothing)

    End Sub



    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub

            'ElseIf gvImagenes.SelectedIndex = "-1" Then

            '    Mensaje = "Favor de Seleccionar un registro"
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            '    Exit Sub

        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"


        Dim estatus As New Entities.Estatus(txtClave.Text)

        If Not estatus.Existe Then
            Mensaje = "¿Está seguro que desea agregar el registro?"
        Else
            Mensaje = "¿Está seguro que desea modificar el registro?"
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub



    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        Dim estatus As New Entities.Estatus
        Dim dv As DataView = estatus.ObtenerTodos
        Dim consulta As String = "1=1"

        For Each filtro In ucFiltro1.getFilterSelection

            consulta += " AND " + filtro

        Next

        dv.RowFilter = consulta

        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count > 0 Then
            btnExportaExcel.Visible = True
            gvConsulta.Visible = True
            pnlNoExiste.Visible = False
        Else
            btnExportaExcel.Visible = False
            gvConsulta.Visible = False
            pnlNoExiste.Visible = True
        End If


    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click
        hfSelectedValue.Value = "-1"
        lblTituloRegistro.Text = "Alta de Estatus"

        Dim estatus As New Entities.Estatus

        txtClave.Text = estatus.ObtenerSiguienteIdentificador()
        txtDescripcion.Text = String.Empty


        CargarCatalogoImagenes()


        pnlRegistro.Visible = True
        pnlConsulta.Visible = False

        trImagenActual.Visible = False

        trImagenCatalogo.Visible = True

        trImagenCatalogo.Style.Value = "display:block"



    End Sub


    Private Sub CargarCatalogoImagenes()

        Dim imagen As New Entities.Imagen
        Dim dv As New DataView
        dv = imagen.ObtenerTodos()
        dv.RowFilter = "N_FLAG_VIG=1"

        gvImagenes.DataSourceSession = dv.ToTable
        gvImagenes.DataSource = dv.ToTable
        gvImagenes.DataBind()

        gvImagenes.Visible = True

        'gvImagenes.CargaSeleccion()

    End Sub

    Private Function HayRegistroSeleccionado() As Boolean

        Dim haySeleccion As Boolean = False

        If gvConsulta.SelectedIndex = "-1" Then
            Mensaje = "Favor de Seleccionar un Registro"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            haySeleccion = False
        Else
            haySeleccion = True
        End If



        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim estatus As New Entities.Estatus(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_ESTATUS").ToString()))

                If Not estatus.Vigente Then
                    Mensaje = "Registro solo de lectura"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


                Dim imagen As New Entities.Imagen()

                Dim dv As DataView = imagen.ObtenerTodos()
                dv.RowFilter = "N_FLAG_VIG=1"

                gvImagenes.DataSource = dv.ToTable()
                gvImagenes.DataBind()

                For Each rowImagen As GridViewRow In gvImagenes.Rows
                    If estatus.IdentificadorImagen = gvImagenes.DataKeys(rowImagen.RowIndex)("N_ID_IMAGEN") Then
                        Dim elementoImagen As CheckBox = TryCast(rowImagen.FindControl("chkElemento"), CheckBox)
                        elementoImagen.Checked = True
                        hfSelectedValue.Value = rowImagen.RowIndex.ToString
                    End If
                Next

                gvImagenes.CargaSeleccion()

                txtClave.Text = estatus.Identificador
                txtDescripcion.Text = estatus.Descripcion
                idEstatus = estatus.Identificador

                trImagenActual.Visible = False
                trImagenCatalogo.Visible = True
                trImagenCatalogo.Style.Value = "display:block"

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False


                'CargarCatalogoImagenes()
                lblTituloRegistro.Text = "Modificación de Estatus"

                Exit For

            End If
        Next

    End Sub

    Public Function verificaSeleccionado(ByVal idImagen As Object, ByVal idEstatus As Object) As Boolean

        'If lblTituloRegistro.Text = "Modificación de Estatus" Then

        '    Return False

        If idEstatus IsNot Nothing Then

            Dim estatus As New Entities.Estatus(idEstatus)

            If estatus.IdentificadorImagen = idImagen Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If


    End Function

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"


        Mensaje = "Al abandonar la pantalla se perderán los cambios. ¿Desea continuar?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        btnAceptarM2B1A_Click(sender, e)

        imgUnBotonNoAccion.ImageUrl = String.Empty
    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click

        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim estatus As New Entities.Estatus(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_ESTATUS").ToString()))

                If Not estatus.Vigente Then
                    Mensaje = "Registro solo de lectura"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next

        Mensaje = "¿Está seguro que desea eliminar el registro?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click


        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True
                trImagenActual.Visible = False
                ' trImagen.Visible = True

                pnlBotones.Visible = True
                pnlRegresar.Visible = False

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True
                imgUnBotonNoAccion.ImageUrl = String.Empty


            Case "btnAceptar"

                Dim estatus As New Entities.Estatus(txtClave.Text)
                estatus.Descripcion = txtDescripcion.Text

                For Each row As GridViewRow In gvImagenes.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then
                        estatus.IdentificadorImagen = gvImagenes.DataKeys(row.RowIndex)("N_ID_IMAGEN").ToString()
                        Exit For
                    End If

                Next


                If Not estatus.Existe Then
                    estatus.Agregar()
                    btnFiltrar_Click(sender, e)
                Else
                    estatus.Actualizar()
                    btnFiltrar_Click(sender, e)
                End If


                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim estatus As New Entities.Estatus(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_ESTATUS").ToString()))

                        If estatus.Baja() Then
                            btnFiltrar_Click(sender, e)
                        End If

                        Exit For

                    End If
                Next

                CargarCatalogo()

        End Select

    End Sub

    Public Function ObtenerImagen(ByVal imagen As String) As String

        ' Dim img As New Entities.Imagen(imagen)

        Return "~/Imagenes/Errores/" + imagen

    End Function

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Estatus"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim estatus As New Entities.Estatus(CInt(gvConsulta.DataKeys(index)("N_ID_ESTATUS").ToString()))

        Dim img As New Entities.Imagen(estatus.IdentificadorImagen)

        imgActual.ImageUrl = "~/Imagenes/Errores/" + img.Ruta

        txtClave.Text = estatus.Identificador
        txtDescripcion.Text = estatus.Descripcion

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
            Dim errores As New Entities.EtiquetaError(22)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

        End If

        Dim imagenSeleccionada As Boolean = False
        For Each row As GridViewRow In gvImagenes.Rows
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

    
    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)

        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"
        dt.Columns("T_DSC_RUTA_IMAGEN").ColumnName = "Imagen"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Estatus", referencias)
    End Sub

    Protected Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub


End Class