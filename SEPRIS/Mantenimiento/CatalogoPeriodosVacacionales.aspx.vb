Imports Entities

Public Class CatalogoPeriodosVacacionales
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
            If Session("BajaUsuario") <> Nothing Then
                Session("BajaUsuario") = Nothing
                Dim errores As New Entities.EtiquetaError(2096)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
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
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_VACACIONES", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        Dim objUsuario As New Usuario
        Dim dvUsuarios As DataView = objUsuario.ObtenerTodos
        ucFiltro1.AddFilter("Usuario", ucFiltro.AcceptedControls.DropDownList, dvUsuarios.ToTable, "NOMBRE_COMPLETO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Fecha de inicio del periodo", ucFiltro.AcceptedControls.Calendar, , , "F_FECH_INICIO_PERIODO", ucFiltro.DataValueType.StringType, True)
        ucFiltro1.AddFilter("Fecha de fin del periodo", ucFiltro.AcceptedControls.Calendar, , , "F_FECH_FIN_PERIODO", ucFiltro.DataValueType.StringType, True)
        ucFiltro1.AddFilter("Días asignados al usuario", ucFiltro.AcceptedControls.TextBox, , , "N_NUM_DIAS_ASIGNADOS", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.AddFilter("Días consumidos por el usuario", ucFiltro.AcceptedControls.TextBox, , , "N_NUM_DIAS_CONSUMIDOS", ucFiltro.DataValueType.IntegerType)
        ucFiltro1.LoadDDL("CatalogoPeriodosVacacionales.aspx")

    End Sub

    Private Sub CargarCombos(Optional ByVal esConsulta As Boolean = False)
        Dim objUsuario As New Usuario
        Dim dvUsuarios As DataView = objUsuario.ObtenerTodos
        If Not esConsulta Then
            dvUsuarios.RowFilter = "N_FLAG_VIG = 1"
        End If
        Utilerias.Generales.CargarComboOrdenado(ddlUsuario, dvUsuarios.ToTable, "NOMBRE_COMPLETO", "T_ID_USUARIO")
    End Sub

    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim objPerido As New PeriodoVacacional
        Dim dv As DataView = objPerido.ObtenerTodos()

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

        lblTituloRegistro.Text = "Alta de Periodo Vacacional"
        CargarCombos()
        Dim objPeriodo As New PeriodoVacacional
        imgFec3.Visible = True
        imgFech.Visible = True
        txtID.Text = objPeriodo.ObtenerSiguienteIdentificador.ToString
        ddlUsuario.SelectedValue = "-1"
        txtFechaIni.Text = String.Empty
        txtFechaFin.Text = String.Empty
        txtDiasAsignados.Text = String.Empty
        txtDiasConsumidos.Text = String.Empty
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
        Dim objPeriodo As New PeriodoVacacional(Convert.ToInt32(txtID.Text))

        If Not objPeriodo.Existe Then
            Dim errores As New Entities.EtiquetaError(2090)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(2091)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub
    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Perido Vacacional"
        CargarCombos()
        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(2094)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objPeriodo As New PeriodoVacacional(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_VACACIONES").ToString()))

        If Not objPeriodo.Vigente Then
            Dim errores As New Entities.EtiquetaError(2095)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        Else
            imgFec3.Visible = True
            imgFech.Visible = True
            txtID.Text = objPeriodo.Identificador
            Dim objUsuario As New Usuario(objPeriodo.Usuario)
            If objUsuario.Vigente Then
                ddlUsuario.SelectedValue = objPeriodo.Usuario
            Else
                Session("BajaUsuario") = "Baja"
                Response.Redirect("CatalogoPeriodosVacacionales.aspx")
            End If

            txtFechaIni.Text = objPeriodo.FechaInicioPeriodo.ToString("dd/MM/yyyy")
            txtFechaFin.Text = objPeriodo.FechaFinPeriodo.ToString("dd/MM/yyyy")
            txtDiasAsignados.Text = objPeriodo.DiasAsignados.ToString
            txtDiasConsumidos.Text = objPeriodo.DiasConsumidos.ToString
            pnlRegistro.Visible = True
            pnlConsulta.Visible = False
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(1092)
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
            errores = New Entities.EtiquetaError(2094)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objPeriodo As New PeriodoVacacional(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_VACACIONES").ToString()))

        If Not objPeriodo.Vigente Then
            errores = New Entities.EtiquetaError(2095)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        errores = New Entities.EtiquetaError(2093)
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

                Dim objPeriodo As New PeriodoVacacional(Convert.ToInt32(txtID.Text))
                objPeriodo.Usuario = ddlUsuario.SelectedValue
                objPeriodo.FechaInicioPeriodo = Convert.ToDateTime(txtFechaIni.Text.Trim & " 09:00:00 a.m.")
                objPeriodo.FechaFinPeriodo = Convert.ToDateTime(txtFechaFin.Text.Trim & " 07:00:00 p.m.")
                objPeriodo.DiasAsignados = CInt(txtDiasAsignados.Text.Trim)
                objPeriodo.DiasConsumidos = CInt(txtDiasConsumidos.Text.Trim)

                If Not objPeriodo.Existe Then
                    objPeriodo.Identificador = objPeriodo.ObtenerSiguienteIdentificador()
                    objPeriodo.Agregar()
                Else
                    objPeriodo.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                Dim objPeriodo As New PeriodoVacacional(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_VACACIONES").ToString()))
                objPeriodo.Baja()

                CargarCatalogo()

        End Select

    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Periodo Vacacional"
        CargarCombos(True)
        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim objPeriodo As New PeriodoVacacional(CInt(gvConsulta.DataKeys(index)("N_ID_VACACIONES").ToString()))

        txtID.Text = objPeriodo.Identificador.ToString
        ddlUsuario.SelectedValue = objPeriodo.Usuario
        txtFechaIni.Text = objPeriodo.FechaInicioPeriodo.ToString("dd/MM/yyyy")
        txtFechaFin.Text = objPeriodo.FechaFinPeriodo.ToString("dd/MM/yyyy")
        txtDiasAsignados.Text = objPeriodo.DiasAsignados.ToString
        txtDiasConsumidos.Text = objPeriodo.DiasConsumidos.ToString
        imgFec3.Visible = False
        imgFech.Visible = False
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

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Peridos Vacacionales", referencias)
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

    

#End Region


    Protected Sub cvUsuario_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvUsuario.ServerValidate
        If ddlUsuario.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(2097)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Protected Sub cvFechaIni_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvFechaIni.ServerValidate
        Dim dateTemp As DateTime
        If txtFechaIni.Text.Trim = "" Then
            Dim errores As New Entities.EtiquetaError(2098)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf Not DateTime.TryParse(txtFechaIni.Text.Trim, dateTemp) Then
            Dim errores As New Entities.EtiquetaError(2099)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    Protected Sub cvFechaFin_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvFechaFin.ServerValidate
        Dim dateTemp As DateTime
        If txtFechaFin.Text.Trim = "" Then
            Dim errores As New Entities.EtiquetaError(2100)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf Not DateTime.TryParse(txtFechaIni.Text.Trim, dateTemp) Then
            Dim errores As New Entities.EtiquetaError(2101)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf txtFechaFin.Text.Trim <> "" And ddlUsuario.SelectedValue <> "-1" Then
            Dim valido As Boolean = True
            Dim objPeriodo As New PeriodoVacacional(CInt(txtID.Text.Trim))
            Dim dvPeriodos = objPeriodo.ObtenerTodos
            dvPeriodos.RowFilter = "T_ID_USUARIO = '" & ddlUsuario.SelectedItem.ToString & "'"
            Dim dt As DataTable = dvPeriodos.ToTable
            If dt.Rows.Count > 0 Then
                Dim fechaIni As DateTime = Convert.ToDateTime(txtFechaIni.Text.Trim)
                Dim fechaFin As DateTime = Convert.ToDateTime(txtFechaFin.Text.Trim)
                If fechaFin < fechaIni Then
                    Dim errores As New Entities.EtiquetaError(2102)
                    source.ErrorMessage = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    args.IsValid = False
                    Exit Sub
                End If
                For i = 0 To dt.Rows.Count - 1
                    Dim objTemp As New PeriodoVacacional(CInt(dt.Rows(i)("N_ID_VACACIONES").ToString()))
                    If ((fechaIni >= objTemp.FechaInicioPeriodo And fechaIni <= objTemp.FechaFinPeriodo) Or _
                        (fechaFin >= objTemp.FechaInicioPeriodo And fechaFin <= objTemp.FechaFinPeriodo) Or _
                        (objTemp.FechaInicioPeriodo >= fechaIni And objTemp.FechaInicioPeriodo <= fechaFin) Or _
                        (objTemp.FechaFinPeriodo >= fechaIni And objTemp.FechaFinPeriodo <= fechaFin)) And _
                        (objPeriodo.Identificador <> objTemp.Identificador) Then
                        valido = False
                        Exit For
                    End If
                Next
            End If
            If Not valido Then
                Dim errores As New Entities.EtiquetaError(2103)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                cvFechaIni.IsValid = False
                Exit Sub
            End If
        End If

    End Sub

    Protected Sub cvDiasAsignados_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDiasAsignados.ServerValidate
        If txtDiasAsignados.Text.Trim = "" Then
            Dim errores As New Entities.EtiquetaError(2104)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf Not Regex.IsMatch(txtDiasAsignados.Text.Trim, "^\d{1,2}$") Then
            Dim errores As New Entities.EtiquetaError(2105)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf Not CInt(txtDiasAsignados.Text.Trim) > 0 Then
            Dim errores As New Entities.EtiquetaError(2106)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Protected Sub cvDiasCunsumidos_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDiasCunsumidos.ServerValidate
        If txtDiasConsumidos.Text.Trim = "" Then
            Dim errores As New Entities.EtiquetaError(2107)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf Not Regex.IsMatch(txtDiasConsumidos.Text.Trim, "^\d{1,2}$") Then
            Dim errores As New Entities.EtiquetaError(2108)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        ElseIf txtDiasAsignados.Text.Trim <> "" And (CInt(txtDiasAsignados.Text.Trim) < CInt(txtDiasConsumidos.Text.Trim)) Then
            Dim errores As New Entities.EtiquetaError(2109)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If
    End Sub
End Class