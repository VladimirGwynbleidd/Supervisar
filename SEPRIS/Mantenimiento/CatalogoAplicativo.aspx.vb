Imports Entities
Public Class CatalogoAplicativo
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
        ucFiltro1.AddFilter("ID", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_APLICATIVO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_CVE_APLICATIVO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 5)
        ucFiltro1.AddFilter("Acrónimo", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_ACRONIMO_APLICATIVO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 20)
        ucFiltro1.AddFilter("Descripción Aplicativo", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_APLICATIVO", ucFiltro.DataValueType.StringType, False, True)

        Dim lstUsuarios As List(Of Usuario) = Usuario.UsuariosSoloDatosGetCustom(" WHERE N_FLAG_VIG = 1 AND B_FLAG_ES_INGENIERO = 1 ")
        Dim lstIdUsuarios = From s In lstUsuarios Select New With {.T_ID_INGENIERO_RESPONSABLE = s.IdentificadorUsuario, .NOMBRE_RESPONSABLE = s.Nombre + " " + s.Apellido + " " + s.ApellidoAuxiliar}
        ucFiltro1.AddFilter("Ingeniero Responsable", ucFiltro.AcceptedControls.DropDownList, lstIdUsuarios, "NOMBRE_RESPONSABLE", "T_ID_INGENIERO_RESPONSABLE", ucFiltro.DataValueType.StringType, False, False, False, False, False)
        Dim lstIdUsuariosBK = From s In lstUsuarios Select New With {.T_ID_INGENIERO_BACKUP = s.IdentificadorUsuario, .NOMBRE_BACKUP = s.Nombre + " " + s.Apellido + " " + s.ApellidoAuxiliar}
        ucFiltro1.AddFilter("Ingeniero Backup", ucFiltro.AcceptedControls.DropDownList, lstIdUsuariosBK, "NOMBRE_BACKUP", "T_ID_INGENIERO_BACKUP", ucFiltro.DataValueType.StringType, False, False, False, False, False)
        ucFiltro1.LoadDDL("CatalogoAplicativo.aspx")

    End Sub

    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim entAplicativo As New Aplicativo
        Dim dv As DataView = entAplicativo.ObtenerTodosConNombreResponsables()

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

    Private Sub CargaDDL(Optional ByVal esConsulta As Boolean = False)
        Dim filtro As String = " WHERE N_FLAG_VIG = 1 AND B_FLAG_ES_INGENIERO = 1 "
        If esConsulta Then filtro = " WHERE B_FLAG_ES_INGENIERO = 1 "
        Dim lstUsuarios As List(Of Usuario) = Usuario.UsuariosSoloDatosGetCustom(filtro)
        For Each User As Usuario In lstUsuarios
            User.Nombre = User.Nombre + " " + User.Apellido + " " + User.ApellidoAuxiliar
        Next
        Dim dt As DataTable = Utilerias.Generales.ConvertToDataTable(lstUsuarios)
        Utilerias.Generales.CargarComboOrdenado(ddlIngResponsable, dt, "Nombre", "IdentificadorUsuario")
        Utilerias.Generales.CargarComboOrdenado(ddlIngBackup, dt, "Nombre", "IdentificadorUsuario")
    End Sub

#End Region

#Region "Eventos Controles"

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Aplicativo"

        Dim entApliactivo As New Aplicativo()

        txtId.Text = entApliactivo.ObtenerSiguienteIdentificador.ToString
        txtClave.Text = String.Empty
        txtAcronimo.Text = String.Empty
        txtAplicativo.Text = String.Empty

        CargaDDL()

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
        Dim entAplicativo As New Aplicativo(Convert.ToInt32(txtId.Text))

        If Not entAplicativo.Existe Then
            Dim errores As New Entities.EtiquetaError(1133)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1134)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Aplicativo"

        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(1059)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim entAplicativo As New Aplicativo(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_APLICATIVO").ToString()))

        If Not entAplicativo.Vigente Then
            Dim errores As New Entities.EtiquetaError(1060)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        Else
            Mensaje = ""
            txtId.Text = entAplicativo.Identificador
            txtClave.Text = entAplicativo.Clave
            txtAcronimo.Text = entAplicativo.Acronimo
            txtAplicativo.Text = entAplicativo.Descripcion

            CargaDDL()

            Dim ingResponsable As New Entities.Usuario(entAplicativo.Ing_Responsable)
            Dim ingBackup As New Entities.Usuario(entAplicativo.Ing_Backup)

            If ingResponsable.Vigente Then
                ddlIngResponsable.SelectedValue = entAplicativo.Ing_Responsable
            Else
                ddlIngResponsable.SelectedValue = "-1"
                Mensaje += "<li>Ingeniero Responsable</li>"
            End If

            If ingBackup.Vigente Then
                ddlIngBackup.SelectedValue = entAplicativo.Ing_Backup
            Else
                ddlIngBackup.SelectedValue = "-1"
                Mensaje += "<li>Ingeniero Backup</li>"
            End If

            If Mensaje <> "" Then
                Dim errores As New Entities.EtiquetaError(2092)
                Mensaje = errores.Descripcion.Replace("[LISTA]", Mensaje)
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            End If
            
            pnlRegistro.Visible = True
            pnlConsulta.Visible = False
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(1135)
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
            errores = New Entities.EtiquetaError(1059)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim entAplicativo As New Aplicativo(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_APLICATIVO").ToString()))

        If Not entAplicativo.Vigente Then
            errores = New Entities.EtiquetaError(1060)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        errores = New Entities.EtiquetaError(1136)
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

                Dim entAplicativo As New Aplicativo(Convert.ToInt32(txtId.Text))
                entAplicativo.Clave = txtClave.Text
                entAplicativo.Acronimo = txtAcronimo.Text
                entAplicativo.Descripcion = txtAplicativo.Text
                entAplicativo.Ing_Responsable = ddlIngResponsable.SelectedValue.ToString
                entAplicativo.Ing_Backup = ddlIngBackup.SelectedValue.ToString

                If Not entAplicativo.Existe Then
                    entAplicativo.Identificador = entAplicativo.ObtenerSiguienteIdentificador()
                    entAplicativo.Agregar()
                Else
                    entAplicativo.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                Dim entAplicativo As New Aplicativo(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_APLICATIVO").ToString()))
                entAplicativo.Baja()

                CargarCatalogo()

        End Select

    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Aplicativo"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim entAplicativo As New Aplicativo(CInt(gvConsulta.DataKeys(index)("N_ID_APLICATIVO").ToString()))

        Mensaje = ""

        txtId.Text = entAplicativo.Identificador.ToString
        txtClave.Text = entAplicativo.Clave
        txtAcronimo.Text = entAplicativo.Acronimo
        txtAplicativo.Text = entAplicativo.Descripcion

        CargaDDL(True)

        ddlIngResponsable.SelectedValue = entAplicativo.Ing_Responsable
        ddlIngBackup.SelectedValue = entAplicativo.Ing_Backup

        Dim objResponsable As New Usuario(entAplicativo.Ing_Responsable)
        Dim objBackup As New Usuario(entAplicativo.Ing_Backup)

        If Not objResponsable.Vigente Then
            Mensaje += "<li>Ingeniero Responsable</li>"
        End If

        If Not objBackup.Vigente Then
            Mensaje += "<li>Ingeniero Backup</li>"
        End If

        If Mensaje <> "" Then
            Dim errores As New Entities.EtiquetaError(2092)
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

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Aplicativo", referencias)
    End Sub

    Private Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

#End Region

#Region "Metodos"
    ''' <summary>
    ''' Obtiene la rutra de la imagen
    ''' </summary>
    ''' <param name="imagen"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerImagen(ByVal imagen As String) As String

        Return "~/Imagenes/Errores/" + imagen

    End Function

    ''' <summary>
    ''' Obtiene Imagen de estatus
    ''' </summary>
    ''' <param name="estatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function
    
#End Region

#Region "Validaciones"
    Private Sub cvClave_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvClave.ServerValidate
        Dim iTemp As Integer
        If txtClave.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1061)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf Not Integer.TryParse(txtClave.Text.Trim(), iTemp) Then
            Dim errores As New Entities.EtiquetaError(1062)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvAcronimo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvAcronimo.ServerValidate
        If txtAcronimo.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1063)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvAplicativo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvAplicativo.ServerValidate
        If txtAplicativo.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(1058)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvddlIngResponsable_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlIngResponsable.ServerValidate
        If ddlIngResponsable.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(1064)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

    Private Sub cvddlIngBackup_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlIngBackup.ServerValidate
        If ddlIngBackup.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(1065)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf ddlIngBackup.SelectedValue = ddlIngResponsable.SelectedValue Then
            Dim errores As New Entities.EtiquetaError(1066)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub

#End Region

End Class