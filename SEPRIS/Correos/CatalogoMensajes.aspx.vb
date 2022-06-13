
' Fecha de creación: 23/07/2013
' Fecha de modificación:  
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' Página de catálogo de mensajes


Imports Negocio
Imports System.Xml
Imports System.Windows.Forms

Public Class CatalogoMensajes
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
            CargaMarcadores()

        End If
    End Sub

    Private Sub CargarFiltros()
        ucFiltro1.resetSession()

        Utilerias.Generales.VigenciaDataSource()

        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim dtDatosFiltro As DataSet

        If Not IsNothing(objUsuario) Then
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Correos, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Correos, "", 0)
        End If

        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource(), "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, False, False, True, True, -1)
        'ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_CORREO", ucFiltro.DataValueType.IntegerType, False, False, False, False, , , 4)
        'ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_CORREO", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "T_DSC_CORREO", "N_ID_CORREO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.LoadDDL("CatalogoMensajes.aspx")
        btnFiltrar_Click(Nothing, Nothing)
    End Sub

    Private Sub CargarCatalogo()
        Dim correo As New Correo
        Dim dv As DataView = correo.ObtenerTodos
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

    Private Sub CargaMarcadores()

        Dim correo As New Correo
        CGVMarcadores.DataSource = correo.LlenaMarcadores()
        CGVMarcadores.DataBind()


    End Sub

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub




    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/OK.gif"
        Else
            Return "~/Imagenes/Error.gif"
        End If

    End Function

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        lblTituloRegistro.Text = "Registro de Mensajes de Correo "

        Dim correo As New Correo

        txtClave.Text = correo.ObtenerSiguienteIdentificador()
        txtDescripcion.Text = String.Empty
        txtMensaje.Text = String.Empty
        txtAsunto.Text = String.Empty
        imgUnBotonNoAccion.ImageUrl = ""


        pnlRegistro.Visible = True
        pnlConsulta.Visible = False
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        lblTituloRegistro.Text = "Modificación de Mensajes de Correo"

        If gvConsulta.SelectedIndex = "-1" Then

            Mensaje = "Favor de Seleccionar un registro"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)

            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim correo As New Correo(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_CORREO").ToString()))

                If Not correo.EsVigente Then
                    Mensaje = "Registro solo de lectura"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


                txtClave.Text = correo.IdentificadorCorreo
                txtDescripcion.Text = correo.Descripcion
                txtAsunto.Text = correo.Asunto
                txtMensaje.Text = correo.Mensaje

                pnlRegistro.Visible = True
                pnlConsulta.Visible = False

                Exit For

            End If
        Next
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        Dim errores As Entities.EtiquetaError
        If gvConsulta.SelectedIndex = "-1" Then

            errores = New Entities.EtiquetaError(162)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)

            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim correo As New Correo(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_CORREO").ToString()))

                If Not correo.EsVigente Then
                    errores = New Entities.EtiquetaError(163)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                Exit For

            End If
        Next

      

        errores = New Entities.EtiquetaError(161)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        btnAceptarM2B1A_Click(sender, e)

    End Sub

    Protected Sub btnAceptarM2B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True

                pnlBotones.Visible = True
                btnAceptar.Visible = True

                pnlRegresar.Visible = False

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True

                imgUnBotonNoAccion.ImageUrl = ""

            Case "btnAceptar"

                Dim correo As New Correo(txtClave.Text)
                correo.Descripcion = txtDescripcion.Text
                correo.Asunto = txtAsunto.Text
                correo.Mensaje = txtMensaje.Text.Replace("##", "<").Replace("|", "/").Replace("**", ">")

                If Not correo.Existe Then
                    correo.Agregar()
                Else
                    correo.Actualizar()
                End If

                CargarCatalogo()

                btnFiltrar_Click(sender, e)
                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim correo As New Correo(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_CORREO").ToString()))

                        correo.Baja()
                        btnFiltrar_Click(sender, e)
                        Exit For

                    End If
                Next

        End Select
        imgUnBotonNoAccion.ImageUrl = ""
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(160)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub

        Else
            Try

                Utilerias.Generales.ValidaCamposCapturaHTML(txtDescripcion)
                Utilerias.Generales.ValidaCamposCapturaHTML(txtMensaje)
                Utilerias.Generales.ValidaCamposCapturaHTML(txtAsunto)

            Catch ex As ApplicationException

                Mensaje = ex.Message
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                Exit Sub
            End Try

        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"


        Dim correo As New Correo(txtClave.Text)

        If Not correo.Existe Then
            Dim errores As New Entities.EtiquetaError(158)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(159)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub



    Protected Sub gvConsulta_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvConsulta.RowCreated
        e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Mensajes de Correo"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim correo As New Correo(CInt(gvConsulta.DataKeys(index)("N_ID_CORREO").ToString()))


        txtClave.Text = correo.IdentificadorCorreo
        txtDescripcion.Text = correo.Descripcion
        txtAsunto.Text = correo.Asunto
        txtMensaje.Text = correo.Mensaje

        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = True
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

        btnAceptar.Visible = False
    End Sub

    Protected Sub cvDescripcion_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvDescripcion.ServerValidate

        If txtDescripcion.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(11)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

        End If
    End Sub

    Protected Sub cvAsunto_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvAsunto.ServerValidate
        If txtAsunto.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(12)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

        End If
    End Sub

    Protected Sub cvMensaje_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvMensaje.ServerValidate
        If txtMensaje.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(13)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False

            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

        End If
    End Sub

    Protected Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)

        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"
        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Mensajes de Correo", referencias)
    End Sub

    Protected Sub btnRegresarCorreos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresarCorreos.Click
        Response.Redirect("~/Correos/MenuOpciones.aspx")
    End Sub

    Protected Sub BtnAgregaAsunto_Click(sender As Object, e As ImageClickEventArgs) Handles BtnAgregaAsunto.Click
        For Each row As GridViewRow In CGVMarcadores.Rows

            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim valorGrid = CGVMarcadores.DataKeys(row.RowIndex)("T_DSC_ETIQUETA").ToString()

                If Not txtposicionText.Text = "" Then
                    txtAsunto.Text = txtAsunto.Text.Insert(Convert.ToInt32(txtposicionText.Text), valorGrid)
                End If
                Exit For

            End If
        Next
    End Sub

    Protected Sub BtnAgregaCuerpo_Click(sender As Object, e As ImageClickEventArgs) Handles BtnAgregaCuerpo.Click

        For Each row As GridViewRow In CGVMarcadores.Rows

            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim valorGrid = CGVMarcadores.DataKeys(row.RowIndex)("T_DSC_ETIQUETA").ToString()
                If Not txtposicionTextArea.Text = "" Then
                    Try
                        txtMensaje.Text = txtMensaje.Text.Insert(Convert.ToInt32(txtposicionTextArea.Text), valorGrid)
                    Catch ex As Exception
                        txtMensaje.Text = txtMensaje.Text & " " & valorGrid
                    End Try
                End If
                Exit For
            End If
        Next
    End Sub

End Class