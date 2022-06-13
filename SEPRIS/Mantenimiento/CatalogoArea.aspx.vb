Imports Entities

Public Class CatalogoArea
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


        Dim usuarios As New Entities.Usuario
        Dim dvUsuariosVigentes As DataView = usuarios.ObtenerTodos
        dvUsuariosVigentes.RowFilter = "N_FLAG_VIG = 1"

        Dim dtResponsables As DataTable = dvUsuariosVigentes.ToTable()
        Dim dtBackup As DataTable = dtResponsables.Copy()

        dtResponsables.Columns("T_ID_USUARIO").ColumnName = "T_ID_SUBDIRECTOR"
        dtBackup.Columns("T_ID_USUARIO").ColumnName = "T_ID_BACKUP"

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaBitDataSource, "Vigencia", "B_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_AREA", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Área", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_AREA", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 50)
        ucFiltro1.AddFilter("Coordinador Responsable", ucFiltro.AcceptedControls.DropDownList, dtResponsables, "NOMBRE_COMPLETO", "T_ID_SUBDIRECTOR", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.AddFilter("Coordinador Backup", ucFiltro.AcceptedControls.DropDownList, dtBackup, "NOMBRE_COMPLETO", "T_ID_BACKUP", ucFiltro.DataValueType.StringType, False, True, False)
        ucFiltro1.LoadDDL("CatalogoArea.aspx")


    End Sub

    Private Sub CargarCatalogo()
        Dim area As New Entities.Area
        Dim dv As DataView = area.ObtenerTodosConNombreResponsables
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



        Dim area As New Entities.Area(txtClave.Text)

        If Not area.Existe Then
            Dim errores As New Entities.EtiquetaError(1137)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1138)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Private Sub CargarCombos(Optional ByVal esConsulta As Boolean = False)
        Dim filtro As String = " WHERE N_FLAG_VIG = 1 " 'Carga solamente usuarios vigentes
        If esConsulta Then filtro = "" 'Carga todos los usuario para consulta
        Dim lstUsuarios As List(Of Usuario) = Usuario.UsuariosSoloDatosGetCustom(filtro)
        For Each User As Usuario In lstUsuarios
            User.Nombre = User.Nombre + " " + User.Apellido + " " + User.ApellidoAuxiliar
        Next
        Dim dt As DataTable = Utilerias.Generales.ConvertToDataTable(lstUsuarios)
        Utilerias.Generales.CargarComboOrdenado(ddlCoordinadorResponsable, dt, "Nombre", "IdentificadorUsuario")
        Utilerias.Generales.CargarComboOrdenado(ddlCoordinadorBackup, dt, "Nombre", "IdentificadorUsuario")
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Área"

        Dim area As New Entities.Area
        CargarCombos()

        txtClave.Text = area.ObtenerSiguienteIdentificador()

        txtDescripcion.Text = String.Empty

        ddlCoordinadorResponsable.SelectedIndex = "-1"
        ddlCoordinadorBackup.SelectedIndex = "-1"


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
            Dim errores As New Entities.EtiquetaError(1141)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Área"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        CargarCombos()

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim area As New Entities.Area(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_AREA").ToString()))

                If Not area.Vigente Then
                    Dim errores As New Entities.EtiquetaError(1142)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                Mensaje = ""

                txtClave.Text = area.Identificador
                txtDescripcion.Text = area.Descripcion

                Dim objSubdirector As New Usuario(area.IdSubdirector)
                Dim objBackup As New Usuario(area.IdBackup)

                If objSubdirector.Vigente Then
                    ddlCoordinadorResponsable.SelectedValue = area.IdSubdirector
                Else
                    ddlCoordinadorResponsable.SelectedValue = "-1"
                    Mensaje += "<li>Coordinador Responsable</li>"
                End If

                If objBackup.Vigente Then
                    ddlCoordinadorBackup.SelectedValue = area.IdBackup
                Else
                    ddlCoordinadorBackup.SelectedValue = "-1"
                    Mensaje += "<li>Coordinador Backup</li>"
                End If

                If Mensaje <> "" Then
                    Dim errores As New Entities.EtiquetaError(2093)
                    Mensaje = errores.Descripcion.Replace("[LISTA]", Mensaje)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
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

        Dim errores As New Entities.EtiquetaError(1139)
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

                Dim area As New Entities.Area(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_AREA").ToString()))
                If Not area.Vigente Then
                    errores = New Entities.EtiquetaError(1142)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next

        errores = New Entities.EtiquetaError(1140)
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

                Dim area As New Entities.Area(txtClave.Text)
                area.Descripcion = txtDescripcion.Text
                area.IdSubdirector = ddlCoordinadorResponsable.SelectedValue
                area.IdBackup = ddlCoordinadorBackup.SelectedValue

                If Not area.Existe Then
                    area.Agregar()
                Else
                    area.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim area As New Entities.Area(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_AREA").ToString()))

                        area.Baja()


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

        lblTituloRegistro.Text = "Consulta de Área"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim area As New Entities.Area(CInt(gvConsulta.DataKeys(index)("N_ID_AREA").ToString()))

        CargarCombos(True)

        txtClave.Text = area.Identificador
        txtDescripcion.Text = area.Descripcion
        ddlCoordinadorResponsable.SelectedValue = area.IdSubdirector
        ddlCoordinadorBackup.SelectedValue = area.IdBackup

        Dim objSubdirector As New Usuario(area.IdSubdirector)
        Dim objBackup As New Usuario(area.IdBackup)

        If Not objSubdirector.Vigente Then
            Mensaje += "<li>Coordinador Responsable</li>"
        End If

        If Not objBackup.Vigente Then
            Mensaje += "<li>Coordinador Backup</li>"
        End If

        If Mensaje <> "" Then
            Dim errores As New Entities.EtiquetaError(2093)
            Mensaje = errores.Descripcion.Replace("[LISTA]", Mensaje)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub

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

        utl.ExportaGrid(dt, gvConsulta, "Catalogo de Área", referencias)

    End Sub



    Protected Sub cvArea_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvArea.ServerValidate

        If txtDescripcion.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1113)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If


    End Sub

    Protected Sub cvddlCoordinadorResponsable_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlCoordinadorResponsable.ServerValidate

        If ddlCoordinadorResponsable.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1114)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If


    End Sub

    Protected Sub cvddlCoordinadorBackup_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlCoordinadorBackup.ServerValidate

        If ddlCoordinadorBackup.SelectedIndex = 0 Then
            Dim errores As New Entities.EtiquetaError(1115)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf ddlCoordinadorBackup.SelectedValue = ddlCoordinadorResponsable.SelectedValue Then
            Dim errores As New Entities.EtiquetaError(1001)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub


End Class