Imports Entities

Public Class CatalogoTipoServicio
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
            CargarImagenesEstatus()
            CargarFiltros()
            CargarCatalogo()

        Else
            If pnlRegistro.Visible Then
                gvImagen.DataSource = gvImagen.DataSourceSession
                gvImagen.DataBind()
                gvImagen.CargaSeleccion()
            End If
        End If
    End Sub


#Region "Carga Datos"

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSourceBit, "Vigencia", "B_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_TIPO_SERVICIO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_TIPO_SERVICIO", ucFiltro.DataValueType.StringType, False, True)
        ucFiltro1.LoadDDL("CatalogoTipoServicio.aspx")

    End Sub

    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim objTipoServicio As New TipoServicio
        Dim dv As DataView = objTipoServicio.ObtenerTodos()

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
        Dim tipoImagen As String = Conexion.SQLServer.Parametro.ObtenerValor("Tipos de Imagen Tipo Servicio")

        If tipoImagen <> String.Empty Then
            parametros += " AND N_ID_TIPO_IMAGEN IN (" + tipoImagen + ")"
        End If

        Dim imagen As New Entities.Imagen
        Dim dv As DataView = imagen.ObtenerTodos()
        dv.RowFilter = parametros

        gvImagen.DataSource = dv.ToTable()
        gvImagen.DataBind()
    End Sub

#End Region

#Region "Eventos Controles"

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Tipo de Servicio"

        Dim objTipoServicio As New TipoServicio()

        txtID.Text = objTipoServicio.ObtenerSiguienteIdentificador.ToString
        txtTipoServicio.Text = String.Empty
        CargaGridImagenes()
        pnlRegistro.Visible = True
        pnlConsulta.Visible = False
        trImagenActual.Visible = False
        trImagenCatalogo.Style.Value = "display:block"


    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"
        Dim objTipoServicio As New TipoServicio(Convert.ToInt32(txtID.Text))

        If Not objTipoServicio.Existe Then
            Dim errores As New Entities.EtiquetaError(1143)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1144)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Tipo de Servicio"

        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1070)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objTipoServicio As New TipoServicio(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_TIPO_SERVICIO").ToString()))

        If Not objTipoServicio.Vigente Then
            Dim errores As New Entities.EtiquetaError(1071)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        Else
            txtID.Text = objTipoServicio.Identificador
            txtTipoServicio.Text = objTipoServicio.Descripcion

            CargaGridImagenes()

            For Each rowImagen As GridViewRow In gvImagen.Rows
                If objTipoServicio.Imagen.Identificador = gvImagen.DataKeys(rowImagen.RowIndex)("N_ID_IMAGEN") Then
                    Dim elementoImagen As CheckBox = TryCast(rowImagen.FindControl("chkElemento"), CheckBox)
                    elementoImagen.Checked = True
                    hfSelectedValue.Value = rowImagen.RowIndex.ToString()
                End If
            Next
            gvImagen.CargaSeleccion()

            pnlRegistro.Visible = True
            pnlConsulta.Visible = False

            trImagenActual.Visible = False
            trImagenCatalogo.Style.Value = "display:block"
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(1145)
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
            errores = New Entities.EtiquetaError(1070)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objTipoServicio As New TipoServicio(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_TIPO_SERVICIO").ToString()))

        If Not objTipoServicio.Vigente Then
            errores = New Entities.EtiquetaError(1071)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        errores = New Entities.EtiquetaError(1146)
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

                Dim objTipoServicio As New TipoServicio(Convert.ToInt32(txtID.Text))
                objTipoServicio.Descripcion = txtTipoServicio.Text
                objTipoServicio.Imagen = New Entities.Imagen(gvImagen.DataKeys(gvImagen.SelectedIndex)("N_ID_IMAGEN"))

                If Not objTipoServicio.Existe Then
                    objTipoServicio.Identificador = objTipoServicio.ObtenerSiguienteIdentificador()
                    objTipoServicio.Agregar()
                Else
                    objTipoServicio.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                Dim objTipoServicio As New TipoServicio(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_TIPO_SERVICIO").ToString()))
                objTipoServicio.Baja()

                CargarCatalogo()

        End Select

    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Tipo de Servicio"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim objTipoServicio As New TipoServicio(CInt(gvConsulta.DataKeys(index)("N_ID_TIPO_SERVICIO").ToString()))

        txtId.Text = objTipoServicio.Identificador.ToString
        txtTipoServicio.Text = objTipoServicio.Descripcion
        imgActual.ImageUrl = "~/Imagenes/Errores/" + objTipoServicio.Imagen.Ruta

        trImagenActual.Visible = True
        trImagenCatalogo.Style.Value = "display:none"

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
        dt.Columns("B_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Tipo de Servicio", referencias)
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
#End Region

#Region "Validaciones"

    Private Sub cvTipoServicio_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvTipoServicio.ServerValidate
        If txtTipoServicio.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1072)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvImagen_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvImagen.ServerValidate
        If gvImagen.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1073)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

#End Region





    
End Class