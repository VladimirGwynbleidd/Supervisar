Imports Entities

Public Class CatalogoEstatusServicio
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
        End If
    End Sub

#Region "Carga Datos"

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSourceBit, "Vigencia", "B_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_ESTATUS_SERVICIO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_ESTATUS_SERVICIO", ucFiltro.DataValueType.StringType, False, True)

        Dim tipo1 = New With {Key .B_FLAG_ESTATUS_SOLICITUD = 1, .descripcion = "Si"}
        Dim tipo2 = New With {Key .B_FLAG_ESTATUS_SOLICITUD = 0, .descripcion = "No"}
        Dim lstSolicitud = New With {tipo1, tipo2}
        ucFiltro1.AddFilter("Estatus corresponde a Solicitud", ucFiltro.AcceptedControls.CheckBox, lstSolicitud, "descripcion", "B_FLAG_ESTATUS_SOLICITUD", ucFiltro.DataValueType.IntegerType)

        ucFiltro1.LoadDDL("CatalogoEstatusSolicitud.aspx")

    End Sub

    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim objEstatusServicio As New EstatusServicio
        Dim dv As DataView = objEstatusServicio.ObtenerTodos()

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

#End Region

#Region "Eventos Controles"

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Estatus de Servicio"

        Dim objEstatusServicio As New EstatusServicio()

        txtID.Text = objEstatusServicio.ObtenerSiguienteIdentificador.ToString
        txtEstatusServicio.Text = ""
        chkSolicitud.Checked = False

        pnlRegistro.Visible = True
        pnlConsulta.Visible = False

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"
        Dim objEstatusServicio As New EstatusServicio(Convert.ToInt32(txtID.Text))

        If Not objEstatusServicio.Existe Then
            Dim errores As New Entities.EtiquetaError(1025)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1026)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Estatus de Servicio"

        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1029)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objEstatusServicio As New EstatusServicio(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_ESTATUS_SERVICIO").ToString()))

        If Not objEstatusServicio.Vigente Then
            Dim errores As New Entities.EtiquetaError(1030)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        Else
            txtID.Text = objEstatusServicio.Identificador
            txtEstatusServicio.Text = objEstatusServicio.Descripcion
            chkSolicitud.Checked = objEstatusServicio.EstatusSolicitud
            pnlRegistro.Visible = True
            pnlConsulta.Visible = False
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(1027)
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
            errores = New Entities.EtiquetaError(1029)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim entEstatusServicio As New EstatusServicio(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_ESTATUS_SERVICIO").ToString()))

        If Not entEstatusServicio.Vigente Then
            errores = New Entities.EtiquetaError(1030)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        errores = New Entities.EtiquetaError(1028)
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

                Dim objEstatusServicio As New EstatusServicio(Convert.ToInt32(txtID.Text))
                objEstatusServicio.Descripcion = txtEstatusServicio.Text
                objEstatusServicio.EstatusSolicitud = chkSolicitud.Checked

                If Not objEstatusServicio.Existe Then
                    objEstatusServicio.Identificador = objEstatusServicio.ObtenerSiguienteIdentificador()
                    objEstatusServicio.Agregar()
                Else
                    objEstatusServicio.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                Dim entEstatusSolicitud As New EstatusServicio(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_ESTATUS_SERVICIO").ToString()))
                entEstatusSolicitud.Baja()

                CargarCatalogo()

        End Select

    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Estatus de Servicio"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim objEstatusServicio As New EstatusServicio(CInt(gvConsulta.DataKeys(index)("N_ID_ESTATUS_SERVICIO").ToString()))

        txtID.Text = objEstatusServicio.Identificador.ToString
        txtEstatusServicio.Text = objEstatusServicio.Descripcion
        chkSolicitud.Checked = objEstatusServicio.EstatusSolicitud

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
        dt.Columns("B_FLAG_ESTATUS_SOLICITUD").ColumnName = "Estatus corresponde a Solicitud"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Estatus de Servicio", referencias)
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

    Public Function ObtenerMensajeSolicitud(ByVal solicitud As Boolean) As String
        If solicitud Then
            Return "Si"
        Else
            Return "No"
        End If
    End Function
#End Region

#Region "Validaciones"

    Private Sub cvEstatusServicio_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvEstatusServicio.ServerValidate
        If txtEstatusServicio.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1031)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

#End Region

End Class