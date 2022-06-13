'- Fecha de creación:29/07/2013
'- Fecha de modificación:  NA
'- Nombre del Responsable: Julio Cesar Vieyra Tena
'- Empresa: Softtek
'- Catalogo de Parametros

Public Class CatalogoParametros
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
        End If

    End Sub

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, , "N_ID_PARAMETRO", ucFiltro.DataValueType.IntegerType, , , , , , , 3)
        ucFiltro1.AddFilter("Parámetro", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_PARAMETRO", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_OBS_PARAMETRO", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Valor", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_VALOR", ucFiltro.DataValueType.StringType, False, True, False, , , , 4000)
        ucFiltro1.LoadDDL("CatalogoParametros.aspx")

    End Sub

    Private Sub CargarCatalogo()
        Dim parametro As New Entities.Parametros()
        Dim dv As DataView = parametro.ObtenerTodos()
        Dim consulta As String = "1=1"

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        dv.RowFilter = consulta
        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()
        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Page.Validate("Forma")
        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If



        Try

            Utilerias.Generales.ValidaCamposCapturaHTML(txtParametro)
            Utilerias.Generales.ValidaCamposCapturaHTML(txtDescripcion)
            Utilerias.Generales.ValidaCamposCapturaHTML(txtValor)


        Catch ex As ApplicationException

            Mensaje = ex.Message
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub

        End Try



        btnAceptarM2B1A.CommandArgument = "btnAceptar"

        Dim parametro As New Entities.Parametros(txtIdParametro.Text, txtParametro.Text, txtDescripcion.Text, txtValor.Text)

        If Not parametro.Existe Then
            Dim errores As New Entities.EtiquetaError(131)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(132)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub


    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        Dim parametro As New Entities.Parametros()
        lblTituloRegistro.Text = "Alta de Parámetro"
        txtIdParametro.Text = parametro.ObtenerSiguienteIdentificador
        txtParametro.Text = String.Empty
        txtDescripcion.Text = String.Empty
        txtValor.Text = String.Empty
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
            Dim errores As New Entities.EtiquetaError(135)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Parámetro"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim parametros As New Entities.Parametros(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PARAMETRO").ToString()))

                If Not parametros.Vigente Then
                    Dim errores As New Entities.EtiquetaError(136)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    txtParametro.Text = parametros.Parametro
                    txtDescripcion.Text = parametros.DescripcionParametro
                    txtValor.Text = parametros.ValorParametro
                    txtIdParametro.Text = gvConsulta.DataKeys(row.RowIndex)("N_ID_PARAMETRO").ToString()
                    pnlRegistro.Visible = True
                    pnlConsulta.Visible = False
                End If
               
                Exit For

            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(133)
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

                Dim parametros As New Entities.Parametros(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PARAMETRO").ToString()))
                If Not parametros.Vigente Then
                    errores = New Entities.EtiquetaError(136)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next


        errores = New Entities.EtiquetaError(134)
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Mensaje = errores.Descripcion
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

                Dim parametro As New Entities.Parametros(CInt(txtIdParametro.Text), txtParametro.Text, txtDescripcion.Text, txtValor.Text)

                If Not parametro.Existe Then
                    parametro.Agregar()
                Else
                    parametro.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then
                        Dim parametro As New Entities.Parametros(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PARAMETRO").ToString()))
                        parametro.Baja()
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
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

        End If

    End Sub


    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Parámetro"
        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))
        Dim parametro As New Entities.Parametros(CInt(gvConsulta.DataKeys(index)("N_ID_PARAMETRO").ToString()))
        txtParametro.Text = parametro.Parametro
        txtDescripcion.Text = parametro.DescripcionParametro
        txtValor.Text = parametro.ValorParametro
        txtIdParametro.Text = gvConsulta.DataKeys(index)("N_ID_PARAMETRO").ToString()
        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True
        pnlConsulta.Visible = False

    End Sub


    Protected Sub cvDescripcion_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDescripcion.ServerValidate

        If txtDescripcion.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(6)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub
    Protected Sub cvParametro_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvParametro.ServerValidate

        If txtParametro.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(9)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub
    Protected Sub cvValor_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvValor.ServerValidate

        If txtValor.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(10)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub



    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Parámetros", referencias)
    End Sub

    Private Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub
End Class