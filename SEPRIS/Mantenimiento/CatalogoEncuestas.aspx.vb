Imports Entities
Public Class CatalogoEncuestas
    Inherits System.Web.UI.Page
    Public Property Mensaje As String
    Public Shared ordenAsp As New List(Of String)
    Public Shared idAsp As New List(Of String)
    Public Shared ordenOpc As New List(Of String)
    Public Shared idOpc As New List(Of String)

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
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_ENCUESTA", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_ENCUESTA", ucFiltro.DataValueType.StringType, False, True)
        ucFiltro1.LoadDDL("CatalogoEncuestas.aspx")

    End Sub

    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim objEncuesta As New Encuesta
        Dim dv As DataView = objEncuesta.ObtenerTodos()

        dv.RowFilter = consulta

        gvConsulta.DataSource = dv.ToTable()
        gvConsulta.DataBind()

        MuestraImagenes()

    End Sub

#End Region
#Region "Eventos Controles"


    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Encuesta"

        Dim objEncuesta As New Encuesta()
        Dim dtEnc As DataTable = objEncuesta.ConsultaEncuestaVigente()
        Dim errores As New Entities.EtiquetaError(1173)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta

        btnAceptarM2B1A.CommandArgument = "btnAgregar"

        If dtEnc.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else
            txtId.Text = objEncuesta.ObtenerSiguienteIdentificador.ToString
            txtEncuesta.Text = String.Empty

            gvConsultaAspectos.DataSource = objEncuesta.CargarAspectosNuevo()
            gvConsultaAspectos.DataBind()

            gvConsultaOpciones.DataSource = objEncuesta.CargarOpcionesNuevo()
            gvConsultaOpciones.DataBind()

            pnlRegistro.Visible = True
            pnlConsulta.Visible = False
            btnAceptar.Visible = True
            btnCancelar.Visible = True
            btnRegresar.Visible = False
            MuestraImagenes()
        End If
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")


        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"
        Dim objEncuesta As New Encuesta(Convert.ToInt32(txtId.Text))

        If Not objEncuesta.Existe Then
            Dim errores As New Entities.EtiquetaError(1107)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1108)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

       

    End Sub
    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Encuesta"

        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1111)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objEncuesta As New Encuesta(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_ENCUESTA").ToString()))

        If Not objEncuesta.Vigente Then
            Dim errores As New Entities.EtiquetaError(1112)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        Else
            txtId.Text = objEncuesta.Identificador
            txtEncuesta.Text = objEncuesta.Descripcion

            gvConsultaAspectos.DataSource = objEncuesta.CargarAspectosNuevo()
            gvConsultaAspectos.DataBind()

            gvConsultaOpciones.DataSource = objEncuesta.CargarOpcionesNuevo()
            gvConsultaOpciones.DataBind()

            txtId.Enabled = False
            txtEncuesta.Enabled = True
            gvConsultaAspectos.Enabled = True
            gvConsultaOpciones.Enabled = True
            btnAceptar.Visible = True
            btnCancelar.Visible = True
            btnRegresar.Visible = False

            MuestraImagenes()

            pnlRegistro.Visible = True
            pnlConsulta.Visible = False

        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(1109)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub
    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click, btnRegresar2.Click
        Response.Redirect("CatalogoEncuestas.aspx")
    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click

        btnAceptarM2B1A.CommandArgument = "btnEliminar"
        Dim dtAct As New DataTable
        Dim errores
        If gvConsulta.SelectedIndex = -1 Then
            errores = New Entities.EtiquetaError(1111)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

       

        Dim objEncuesta As New Encuesta(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_ENCUESTA").ToString()))
        dtAct = objEncuesta.ConsultaEncuestaVigente()

        If dtAct.Rows.Count > 1 Then
            If Not objEncuesta.Vigente Then
                errores = New Entities.EtiquetaError(1112)
                Mensaje = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                Exit Sub
            End If

            errores = New Entities.EtiquetaError(1110)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)

        Else
            errores = New Entities.EtiquetaError(1174)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If
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

                Dim objEncuesta As New Encuesta(Convert.ToInt32(txtId.Text))
                Dim dtEncuest As New DataTable
                objEncuesta.Descripcion = txtEncuesta.Text()
                objEncuesta.idOpc = idOpc
                objEncuesta.idAsp = idAsp
                objEncuesta.ordenAsp = ordenAsp
                objEncuesta.ordenOpc = ordenOpc

                If Not objEncuesta.Existe Then
                    dtEncuest = objEncuesta.ConsultaEncuestaVigente()
                    For Each row As DataRow In dtEncuest.Rows
                        objEncuesta.Identificador = row("N_ID_ENCUESTA")
                        objEncuesta.DesactivarEncuesta()
                    Next
                    objEncuesta.Identificador = objEncuesta.ObtenerSiguienteIdentificador()
                    objEncuesta.Agregar()
                Else
                    objEncuesta.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                Dim objEncuesta As New Encuesta(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_ENCUESTA").ToString()))
                objEncuesta.Baja()

                CargarCatalogo()
            Case "btnAgregar"
                Dim objEncuesta As New Encuesta()
                txtId.Text = objEncuesta.ObtenerSiguienteIdentificador.ToString
                txtEncuesta.Text = String.Empty

                gvConsultaAspectos.DataSource = objEncuesta.CargarAspectosNuevo()
                gvConsultaAspectos.DataBind()

                gvConsultaOpciones.DataSource = objEncuesta.CargarOpcionesNuevo()
                gvConsultaOpciones.DataBind()

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False
                MuestraImagenes()
        End Select
    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Encuestas"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim objEncuesta As New Encuesta(CInt(gvConsulta.DataKeys(index)("N_ID_ENCUESTA").ToString()))

        txtId.Text = objEncuesta.Identificador.ToString
        txtEncuesta.Text = objEncuesta.Descripcion

        gvConsultaAspectos.DataSource = objEncuesta.CargarAspectosNuevo()
        gvConsultaAspectos.DataBind()

        gvConsultaOpciones.DataSource = objEncuesta.CargarOpcionesNuevo()
        gvConsultaOpciones.DataBind()

        txtId.Enabled = False
        txtEncuesta.Enabled = False
        gvConsultaAspectos.Enabled = False
        gvConsultaOpciones.Enabled = False
        btnAceptar.Visible = False
        btnCancelar.Visible = False
        btnRegresar.Visible = True

        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

        MuestraImagenes()

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

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Encuestas", referencias)
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

    Private Sub cvTipoEncuesta_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvEncuesta.ServerValidate
        If txtEncuesta.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1116)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub
#End Region

    Protected Sub btnRegresarBandeja_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresarBandeja.Click
        Response.Redirect("~/Mantenimiento/Encuestas.aspx")
    End Sub

    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvConsultaAspectos.Rows.Count > 0 Then
            NoExistenAspectos.Visible = False
            pnlBotones.Visible = True
            pnlRegresar.Visible = False
        Else
            NoExistenAspectos.Visible = True
            pnlBotones.Visible = False
            pnlRegresar.Visible = True
        End If

        If gvConsultaOpciones.Rows.Count > 0 Then
            NoExistenOpciones.Visible = False
            pnlBotones.Visible = True
            pnlRegresar.Visible = False
        Else
            NoExistenOpciones.Visible = True
            pnlBotones.Visible = False
            pnlRegresar.Visible = True
        End If

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If
    End Sub

    Protected Sub cvAspectos_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvAspectos.ServerValidate
        Dim ordenAspectos As Boolean = False
        Dim errorIntAspect As Boolean = False
        Dim intValue As Integer
        For Each row As GridViewRow In gvConsultaAspectos.Rows
            Dim tb As TextBox = CType(row.FindControl("txbOrden"), TextBox)
            If Not tb.Text.Trim().Equals(String.Empty) Then
                ordenAspectos = True
                If Not Integer.TryParse(tb.Text, intValue) Then
                    errorIntAspect = True
                End If
            End If
        Next
        If Not ordenAspectos Then
            Dim errores As New Entities.EtiquetaError(1117) 'Error de que no se le ha asignado orden a todos los aspectos
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf errorIntAspect Then
            Dim errores As New Entities.EtiquetaError(1175)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

        ordenAsp.Clear()
        idAsp.Clear()
        For Each row As GridViewRow In gvConsultaAspectos.Rows
            Dim tb As TextBox = CType(row.FindControl("txbOrden"), TextBox)
            If Not tb.Text.Equals(String.Empty) Then
                If Not ordenAsp.Contains(tb.Text) Then
                    ordenAsp.Add(tb.Text)
                    idAsp.Add(gvConsultaAspectos.DataKeys(row.RowIndex).Value.ToString())
                Else
                    Dim errores As New Entities.EtiquetaError(1105)
                    source.ErrorMessage = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    args.IsValid = False
                End If
            End If
        Next
    End Sub

    Protected Sub cvOpciones_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvOpciones.ServerValidate
        Dim errorIntOpc As Boolean = False
        Dim ordenOpciones As Boolean = False
        Dim intValue As Integer
        For Each row As GridViewRow In gvConsultaOpciones.Rows
            Dim tb As TextBox = CType(row.FindControl("txbOrden"), TextBox)
            If Not tb.Text.Trim().Equals(String.Empty) Then
                ordenOpciones = True
                If Not Integer.TryParse(tb.Text, intValue) Then
                    errorIntOpc = True
                End If
            End If
        Next
        If Not ordenOpciones Then
            Dim errores As New Entities.EtiquetaError(1118) 'Error de que no se le ha asignado orden a todos las opciones
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf errorIntOpc Then
            Dim errores As New Entities.EtiquetaError(1176)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
        ordenOpc.Clear()
        idOpc.Clear()
        For Each row As GridViewRow In gvConsultaOpciones.Rows
            Dim tb As TextBox = CType(row.FindControl("txbOrden"), TextBox)
            If Not tb.Text.Equals(String.Empty) Then
                If Not ordenOpc.Contains(tb.Text) Then
                    ordenOpc.Add(tb.Text)
                    idOpc.Add(gvConsultaOpciones.DataKeys(row.RowIndex).Value.ToString())
                Else
                    Dim errores As New Entities.EtiquetaError(1106)
                    source.ErrorMessage = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    args.IsValid = False
                End If
            End If
        Next
    End Sub
End Class