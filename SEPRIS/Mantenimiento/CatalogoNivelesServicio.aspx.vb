Public Class CatalogoServicios
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
            CargarNivelServicioesEstatus()
            If Session("BajaUsuario") <> Nothing Then
                Session("BajaUsuario") = Nothing
                Dim errores As New Entities.EtiquetaError(2091)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            End If
        End If

    End Sub

    Public Function ObtenerPaginacion() As Integer
        Return CInt(Conexion.SQLServer.Parametro.ObtenerValor("Paginación Niveles de Servicio"))
    End Function

    Private Sub CargarNivelServicioesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarCombos(Optional ByVal Consulta As Boolean = False)

        Dim filtro As String = "B_FLAG_VIG = 1"

        If Consulta Then filtro = ""

        Dim tipoServicio As New Entities.TipoServicio
        Dim dvTipoServicio As DataView = tipoServicio.ObtenerTodos()
        dvTipoServicio.RowFilter = filtro

        Dim servicio As New Entities.Servicio
        Dim dvServicio As DataView = servicio.ObtenerTodos()
        dvServicio.RowFilter = filtro

        Dim aplicativo As New Entities.Aplicativo
        Dim dvAplicativo As DataView = aplicativo.ObtenerTodos()
        dvAplicativo.RowFilter = filtro

        Dim area As New Entities.Area
        Dim dvArea As DataView = area.ObtenerTodos()
        dvArea.RowFilter = filtro

        Dim flujo As New Entities.Flujo
        Dim dvFlujo As DataView = flujo.ObtenerTodos()
        dvFlujo.RowFilter = filtro

        If Not Consulta Then filtro = "N_FLAG_VIG = 1"
        Dim usuarios As New Entities.Usuario
        Dim dvUsuarios As DataView = usuarios.ObtenerTodos()
        dvUsuarios.RowFilter = filtro

        Utilerias.Generales.CargarComboOrdenado(ddlTipoServicio, dvTipoServicio.ToTable, "T_DSC_TIPO_SERVICIO", "N_ID_TIPO_SERVICIO")
        Utilerias.Generales.CargarComboOrdenado(ddlServicio, dvServicio.ToTable(), "T_DSC_SERVICIO", "N_ID_SERVICIO")
        Utilerias.Generales.CargarComboOrdenado(ddlAplicativo, dvAplicativo.ToTable(), "T_DSC_APLICATIVO", "N_ID_APLICATIVO")
        Utilerias.Generales.CargarComboOrdenado(ddlArea, dvArea.ToTable(), "T_DSC_AREA", "N_ID_AREA")
        Utilerias.Generales.CargarComboOrdenado(ddlFlujo, dvFlujo.ToTable(), "T_DSC_FLUJO", "N_ID_FLUJO")
        Utilerias.Generales.CargarComboOrdenado(ddlResponsable, dvUsuarios.ToTable(), "NOMBRE_COMPLETO", "T_ID_USUARIO")
        Utilerias.Generales.CargarComboOrdenado(ddlBackup, dvUsuarios.ToTable(), "NOMBRE_COMPLETO", "T_ID_USUARIO")

    End Sub

    Private Sub CargarFiltros()




        ucFiltro1.resetSession()


        Dim tipoServicio As New Entities.TipoServicio
        Dim servicio As New Entities.Servicio
        Dim aplicativo As New Entities.Aplicativo
        Dim area As New Entities.Area
        Dim flujo As New Entities.Flujo
        Dim usuarios As New Entities.Usuario
        Dim dvUsuarios As DataView = usuarios.ObtenerTodos
        dvUsuarios.RowFilter = "N_FLAG_VIG=1"
        Dim dtResponsables As DataTable = dvUsuarios.ToTable()
        Dim dtBackup As DataTable = dtResponsables.Copy()

        dtResponsables.Columns("T_ID_USUARIO").ColumnName = "T_ID_INGENIERO_RESPONSABLE"
        dtBackup.Columns("T_ID_USUARIO").ColumnName = "T_ID_INGENIERO_BACKUP"


        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_NIVELES_SERVICIO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Tipo de Servicio", ucFiltro.AcceptedControls.DropDownList, tipoServicio.ObtenerTodos().ToTable(), "T_DSC_TIPO_SERVICIO", "N_ID_TIPO_SERVICIO", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Servicio", ucFiltro.AcceptedControls.DropDownList, servicio.ObtenerTodos().ToTable(), "T_DSC_SERVICIO", "N_ID_SERVICIO", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Aplicativo", ucFiltro.AcceptedControls.DropDownList, aplicativo.ObtenerTodos().ToTable(), "T_DSC_APLICATIVO", "N_ID_APLICATIVO", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Área", ucFiltro.AcceptedControls.DropDownList, area.ObtenerTodos().ToTable(), "T_DSC_AREA", "N_ID_AREA", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Flujo", ucFiltro.AcceptedControls.DropDownList, flujo.ObtenerTodos().ToTable(), "T_DSC_FLUJO", "N_ID_FLUJO", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Ingeniero Responsable", ucFiltro.AcceptedControls.DropDownList, dtResponsables, "NOMBRE_COMPLETO", "T_ID_INGENIERO_RESPONSABLE", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Ingeniero Backup", ucFiltro.AcceptedControls.DropDownList, dtBackup, "NOMBRE_COMPLETO", "T_ID_INGENIERO_BACKUP", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_NIVELES_SERVICIO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 100)
        ucFiltro1.AddFilter("Tiempo de ejecución", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_NUM_TIEMPO_EJECUCION", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 4)
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSourceBit, "Vigencia", "B_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)

        ucFiltro1.LoadDDL("CatalogoNivelesServicio.aspx")


    End Sub

    Private Sub CargarCatalogo()
        Dim nivelServicio As New Entities.NivelServicio
        Dim dv As DataView = nivelServicio.ObtenerTodos
        Dim consulta As String = "1=1"

        For Each filtro In ucFiltro1.getFilterSelection

            consulta += " AND " + filtro

        Next

        dv.RowFilter = consulta

        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()
        MuestraGridView()
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub

        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"


        Dim NivelServicio As New Entities.NivelServicio(txtClave.Text)

        If Not NivelServicio.Existe Then
            Dim errores As New Entities.EtiquetaError(1147)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1148)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Nivel de Servicio"

        Dim nivelServicio As New Entities.NivelServicio

        txtClave.Text = nivelServicio.ObtenerSiguienteIdentificador()
        CargarCombos()
        txtDescripcion.Text = String.Empty
        txtTiempoEjecucion.Text = String.Empty

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
            Dim errores As New Entities.EtiquetaError(1151)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Nivel de Servicio"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Mensaje = ""
                Dim NivelServicio As New Entities.NivelServicio(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_NIVELES_SERVICIO").ToString()))

                If Not NivelServicio.Vigente Then
                    Dim errores As New Entities.EtiquetaError(1152)
                    Mensaje = errores.Descripcion
                    imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                CargarCombos()

                txtClave.Text = NivelServicio.Identificador
                Dim objTipoServicio As New Entities.TipoServicio(NivelServicio.IdentificadorTipoServicio)
                If objTipoServicio.Vigente Then
                    ddlTipoServicio.SelectedValue = NivelServicio.IdentificadorTipoServicio
                Else
                    Mensaje += "<li>Tipo de Servicio</li>"
                End If

                Dim objServicio As New Entities.Servicio(NivelServicio.IdentificadorServicio)
                If objServicio.Vigente Then
                    ddlServicio.SelectedValue = NivelServicio.IdentificadorServicio
                Else
                    Mensaje += "<li>Servicio</li>"
                End If

                Dim objAplicativo As New Entities.Aplicativo(NivelServicio.IdentificadorAplicativo)
                If objAplicativo.Vigente Then
                    ddlAplicativo.SelectedValue = NivelServicio.IdentificadorAplicativo
                Else
                    Mensaje += "<li>Aplicativo</li>"
                End If
                If objAplicativo.Identificador <> 0 And (Not ObtenerVigenciaIngenieros(objAplicativo)) Then
                    Session("BajaUsuario") = "Baja"
                    Response.Redirect("CatalogoNivelesServicio.aspx")
                End If
                If ddlAplicativo.SelectedValue <> "0" And ddlAplicativo.SelectedValue <> "-1" Then
                    ddlResponsable.Enabled = False
                    ddlBackup.Enabled = False
                End If

                Dim objArea As New Entities.Area(NivelServicio.IdentificadorArea)
                If objArea.Vigente Then
                    ddlArea.SelectedValue = NivelServicio.IdentificadorArea
                Else
                    Mensaje += "<li>Área</li>"
                End If

                Dim objFlujo As New Entities.Flujo(NivelServicio.IdentificadorFlujo)
                If objFlujo.Vigente Then
                    ddlFlujo.SelectedValue = NivelServicio.IdentificadorFlujo
                Else
                    Mensaje += "<li>Flujo</li>"
                    ddlFlujo.Enabled = True
                End If

                Dim objResponsable As New Entities.Usuario(NivelServicio.IngenieroResponsable)
                If objResponsable.Vigente Then
                    ddlResponsable.SelectedValue = NivelServicio.IngenieroResponsable
                Else
                    Mensaje += "<li>Ingeniero Responsable</li>"
                End If

                Dim objBackup As New Entities.Usuario(NivelServicio.IngenieroBackup)
                If objBackup.Vigente Then
                    ddlBackup.SelectedValue = NivelServicio.IngenieroBackup
                Else
                    Mensaje += "<li>Ingeniero Backup</li>"
                End If

                txtDescripcion.Text = NivelServicio.Descripcion
                txtTiempoEjecucion.Text = NivelServicio.TiempoEjecucion

                If Mensaje <> "" Then
                    Dim errores As New Entities.EtiquetaError(2090)
                    Mensaje = errores.Descripcion.Replace("[LISTA]", Mensaje)
                    imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                End If

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False

                Exit For

            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"


        Dim errores As New Entities.EtiquetaError(1149)
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

                Dim nivelServicio As New Entities.NivelServicio(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_NIVELES_SERVICIO").ToString()))

                If Not NivelServicio.Vigente Then
                    errores = New Entities.EtiquetaError(1152)
                    Mensaje = errores.Descripcion
                    imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next

        errores = New Entities.EtiquetaError(1150)
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


                Dim nivelServicio As New Entities.NivelServicio(txtClave.Text)
                
                nivelServicio.IdentificadorTipoServicio = ddlTipoServicio.SelectedValue
                nivelServicio.IdentificadorServicio = ddlServicio.SelectedValue
                nivelServicio.IdentificadorAplicativo = ddlAplicativo.SelectedValue
                nivelServicio.IdentificadorArea = ddlArea.SelectedValue
                nivelServicio.IdentificadorFlujo = ddlFlujo.SelectedValue
                nivelServicio.IngenieroResponsable = ddlResponsable.SelectedValue
                nivelServicio.IngenieroBackup = ddlBackup.SelectedValue
                nivelServicio.Descripcion = txtDescripcion.Text
                nivelServicio.TiempoEjecucion = txtTiempoEjecucion.Text



                If Not NivelServicio.Existe Then
                    NivelServicio.Agregar()
                Else
                    NivelServicio.Actualizar()
                End If


                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim NivelServicio As New Entities.NivelServicio(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_NIVELES_SERVICIO").ToString()))

                        NivelServicio.Baja()


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

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated

        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then

            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

        End If


    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Nivel de Servicio"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim NivelServicio As New Entities.NivelServicio(CInt(gvConsulta.DataKeys(index)("N_ID_NIVELES_SERVICIO").ToString()))

        CargarCombos(True)

        txtClave.Text = NivelServicio.Identificador
        ddlTipoServicio.SelectedValue = NivelServicio.IdentificadorTipoServicio
        ddlServicio.SelectedValue = NivelServicio.IdentificadorServicio
        ddlAplicativo.SelectedValue = NivelServicio.IdentificadorAplicativo
        ddlArea.SelectedValue = NivelServicio.IdentificadorArea
        ddlFlujo.SelectedValue = NivelServicio.IdentificadorFlujo
        ddlResponsable.SelectedValue = NivelServicio.IngenieroResponsable
        ddlResponsable.SelectedValue = NivelServicio.IngenieroResponsable
        ddlBackup.SelectedValue = NivelServicio.IngenieroBackup
        txtDescripcion.Text = NivelServicio.Descripcion
        txtTiempoEjecucion.Text = NivelServicio.TiempoEjecucion

        Dim objTipoServicio As New Entities.TipoServicio(NivelServicio.IdentificadorTipoServicio)
        If Not objTipoServicio.Vigente Then
            Mensaje += "<li>Tipo de Servicio</li>"
        End If

        Dim objServicio As New Entities.Servicio(NivelServicio.IdentificadorServicio)
        If Not objServicio.Vigente Then
            Mensaje += "<li>Servicio</li>"
        End If

        Dim objAplicativo As New Entities.Aplicativo(NivelServicio.IdentificadorAplicativo)
        If Not objAplicativo.Vigente Then
            Mensaje += "<li>Aplicativo</li>"
        End If
        If Not ObtenerVigenciaIngenieros(objAplicativo) Then
            Mensaje += "<li>Al menos un ingeniero del aplicativo seleccionado se encuentra con estado de no vigente, debe modificar el aplicativo en el catálogo de aplicativos</li>"
        End If

        Dim objArea As New Entities.Area(NivelServicio.IdentificadorArea)
        If Not objArea.Vigente Then
            Mensaje += "<li>Área</li>"
        End If

        Dim objFlujo As New Entities.Flujo(NivelServicio.IdentificadorFlujo)
        If Not objFlujo.Vigente Then
            Mensaje += "<li>Flujo</li>"
        End If

        Dim objResponsable As New Entities.Usuario(NivelServicio.IngenieroResponsable)
        If Not objResponsable.Vigente Then
            Mensaje += "<li>Ingeniero Responsable</li>"
        End If

        Dim objBackup As New Entities.Usuario(NivelServicio.IngenieroBackup)
        If Not objBackup.Vigente Then
            Mensaje += "<li>Ingeniero Backup</li>"
        End If

        If Mensaje <> "" Then
            Dim errores As New Entities.EtiquetaError(2090)
            Mensaje = errores.Descripcion.Replace("[LISTA]", Mensaje)
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub

    Private Function ObtenerVigenciaIngenieros(ByRef objAplicativo As Entities.Aplicativo) As Boolean
        Dim resultado As Boolean = True
        Dim objIngenieroR As New Entities.Usuario(objAplicativo.Ing_Responsable)
        Dim objIngenieroB As New Entities.Usuario(objAplicativo.Ing_Backup)
        If Not (objIngenieroB.Vigente And objIngenieroR.Vigente) Then
            resultado = False
        End If
        Return resultado
    End Function

    ''' <summary>
    ''' Dependiendo si el GridView no tiene registros muestra un NivelServicio
    ''' </summary>
    ''' <remarks></remarks>
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
        dt.Columns("B_FLAG_VIG").ColumnName = "Estatus"


        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Niveles de Servicio", referencias)

    End Sub

    Protected Sub cvTipoServicio_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvTipoServicio.ServerValidate
        If ddlTipoServicio.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1153)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvServicio_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvServicio.ServerValidate
        If ddlServicio.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1154)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvAplicativo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvAplicativo.ServerValidate
        If ddlAplicativo.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1155)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvArea_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvArea.ServerValidate
        If ddlArea.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1156)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvFlujo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvFlujo.ServerValidate
        If ddlFlujo.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1157)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvResponsable_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvResponsable.ServerValidate
        If ddlResponsable.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1158)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvBackup_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvBackup.ServerValidate
        If ddlBackup.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1159)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf ddlResponsable.SelectedIndex = ddlBackup.SelectedIndex Then
            Dim errores As New Entities.EtiquetaError(1172)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvDescripcion_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDescripcion.ServerValidate
        If txtDescripcion.Text = "" Then
            Dim errores As New Entities.EtiquetaError(1160)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub cvTiempoEjecucion_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvTiempoEjecucion.ServerValidate
        If txtTiempoEjecucion.Text = "" And ddlFlujo.SelectedItem.ToString <> "Especial" Then
            Dim errores As New Entities.EtiquetaError(1161)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Protected Sub ddlAplicativo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAplicativo.SelectedIndexChanged
        If ddlAplicativo.SelectedValue <> "0" And ddlAplicativo.SelectedValue <> "-1" Then
            Dim sAplicativo As New Entities.Aplicativo(ddlAplicativo.SelectedValue)
            If Not ObtenerVigenciaIngenieros(sAplicativo) Then
                ddlAplicativo.SelectedValue = hfValorSeleccionado.Value.ToString
                Dim errores As New Entities.EtiquetaError(2091)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                Exit Sub
            End If
            ddlResponsable.SelectedValue = sAplicativo.Ing_Responsable
            ddlResponsable.Enabled = False
            ddlBackup.SelectedValue = sAplicativo.Ing_Backup
            ddlBackup.Enabled = False
        Else
            ddlResponsable.SelectedIndex = 0
            ddlResponsable.Enabled = True
            ddlBackup.SelectedIndex = 0
            ddlBackup.Enabled = True
        End If
    End Sub

    Protected Sub ddlTipoServicio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoServicio.SelectedIndexChanged
        If ddlTipoServicio.SelectedValue = "0" Then
            ddlFlujo.SelectedValue = "3"
            ddlFlujo.Enabled = False
        Else
            If ddlFlujo.SelectedValue = "3" Then ddlFlujo.SelectedIndex = 0
            ddlFlujo.Enabled = True
        End If
    End Sub

    Protected Sub ddlFlujo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFlujo.SelectedIndexChanged
        If ddlFlujo.SelectedValue = "3" Then
            ddlTipoServicio.SelectedValue = "0"
            ddlTipoServicio.Enabled = False
        Else
            If ddlTipoServicio.SelectedValue = "0" Then ddlTipoServicio.SelectedIndex = 0
            ddlTipoServicio.Enabled = True
        End If

        If ddlFlujo.SelectedValue = "2" Then
            txtTiempoEjecucion.Text = "1"
            txtTiempoEjecucion.Enabled = False
        Else
            If Not txtTiempoEjecucion.Enabled Then
                txtTiempoEjecucion.Enabled = True
                txtTiempoEjecucion.Text = ""
            End If
        End If
    End Sub

    Protected Sub gvConsulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataBind()
    End Sub

End Class