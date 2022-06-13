'- Fecha de creación: 29/07/2013
'- Nombre del Responsable: Rafael Rodríguez Sánchez
'- Empresa: Softtek
'- Permite agregar o remover exclusion de correos a de los usuarios

Imports Entities

Public Class ExclusionCorreoUsuario
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargaUsuarios()
        End If

    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvEnvian.ArmaMultiScript()
    End Sub


#Region "Carga Datos"

    ''' <summary>
    ''' Carga los usuario al DropDownList ddlUsuario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaUsuarios()
        Dim strWhere As String = " WHERE N_FLAG_VIG = 1 "

        Dim lstUsuarios As List(Of Usuario) = Usuario.UsuariosSoloDatosGetCustom(strWhere)

        Utilerias.Generales.CargarCombo(ddlUsuario, lstUsuarios, "IdentificadorUsuario", "IdentificadorUsuario")

    End Sub

    ''' <summary>
    ''' Carga los perfiles en el DropDownList ddlPerfil
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaPerfiles()
        If ddlUsuario.SelectedValue <> "-1" Then

            Dim entUsuario As New Usuario()

            entUsuario.CargarDatos(ddlUsuario.SelectedValue)

            Dim listPerfiles = From s In entUsuario.Perfiles Where s.EsVigente = True Select New With {.Descripcion = s.Descripcion, .IdentificadorPerfil = s.IdentificadorPerfil}

            Utilerias.Generales.CargarCombo(ddlPerfil, listPerfiles, "Descripcion", "IdentificadorPerfil")

            If listPerfiles.Count > 1 Then
                lblPerfil.Visible = True
                ddlPerfil.Visible = True
                pnlCorreos.Style.Add("display", "none")
            Else
                ddlPerfil.SelectedIndex = 1
                lblPerfil.Visible = False
                ddlPerfil.Visible = False
                CargaCorreos()
            End If
        Else
            pnlCorreos.Style.Add("display", "none")
            btnMostrarMensaje.Visible = False
            btnCancelar.Visible = False
            imgDatos.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Carga los correos en los GridViews
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaCorreos()
        If ddlUsuario.SelectedValue <> "-1" Then
            pnlCorreos.Style.Add("display", "block")
            btnMostrarMensaje.Visible = True
            btnCancelar.Visible = True

            Dim entCorreo As New Correo()

            Dim lstEnvian As List(Of Correo) = entCorreo.ObtenerCorreosUsuario(ddlUsuario.SelectedValue, Convert.ToInt32(ddlPerfil.SelectedValue))

            gvEnvian.DataSource = lstEnvian
            gvEnvian.DataBind()

            Dim lstExcluidos As List(Of Correo) = entCorreo.ObtenerCorreosExcluyeUsuario(ddlUsuario.SelectedValue, Convert.ToInt32(ddlPerfil.SelectedValue))

            gvExcluidos.DataSource = lstExcluidos
            gvExcluidos.DataBind()

            MuestraImagenes()
        Else
            pnlCorreos.Style.Add("display", "none")
            btnMostrarMensaje.Visible = False
            btnCancelar.Visible = False
            imgDatos.Visible = False
        End If
        

    End Sub

#End Region

#Region "Eventos de Controles"

    Protected Sub ddlUsuario_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlUsuario.SelectedIndexChanged
        CargaPerfiles()
    End Sub

    Protected Sub ddlPerfil_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPerfil.SelectedIndexChanged
        CargaCorreos()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If (ddlUsuario.SelectedValue <> "-1" AndAlso ddlPerfil.SelectedValue <> "-1") Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(25)
                    lblMensaje.Text = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"
                Response.Redirect("~/Correos/MenuOpciones.aspx", True)
        End Select

    End Sub

    Protected Sub btnMostrarMensaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMostrarMensaje.Click
        Page.Validate("Forma")

        If Page.IsValid() Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            Dim errores As New Entities.EtiquetaError(166)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Guardar", "LevantaVentanaConfirma();", True)
        Else
            'Dim errores As New Entities.EtiquetaError(25)
            lblMensaje.Text = String.Empty
            'imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(167)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvExcluidos.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstExcluidos As List(Of Correo) = TryCast(gvExcluidos.DataSourceSession, List(Of Correo))
            Dim lstEnvian As List(Of Correo) = TryCast(gvEnvian.DataSourceSession, List(Of Correo))
            For Each gvRow In lstGVRow
                For index = 0 To lstExcluidos.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstExcluidos(index).Identificador Then
                        lstEnvian.Add(lstExcluidos(index))
                        lstExcluidos.Remove(lstExcluidos(index))
                        Exit For
                    End If
                Next
            Next

            gvExcluidos.DataSource = lstExcluidos
            gvExcluidos.DataBind()

            gvEnvian.DataSource = lstEnvian
            gvEnvian.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvEnvian.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstExcluidos As List(Of Correo) = TryCast(gvExcluidos.DataSourceSession, List(Of Correo))
            Dim lstEnvian As List(Of Correo) = TryCast(gvEnvian.DataSourceSession, List(Of Correo))
            For Each gvRow In lstGVRow
                For index = 0 To lstEnvian.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstEnvian(index).Identificador Then
                        lstExcluidos.Add(lstEnvian(index))
                        lstEnvian.Remove(lstEnvian(index))
                        Exit For
                    End If
                Next
            Next

            gvExcluidos.DataSource = lstExcluidos
            gvExcluidos.DataBind()

            gvEnvian.DataSource = lstEnvian
            gvEnvian.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnExportaExcelExcluidos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcelExcluidos.Click
        Dim lstExcluidos As List(Of Correo) = TryCast(gvExcluidos.DataSourceSession, List(Of Correo))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entSubMenu As Correo In lstExcluidos
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entSubMenu.Identificador
            dr.Item("Descripcion") = entSubMenu.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Para el Usuario: " + ddlUsuario.SelectedValue)
        lstComentarios.Add("Perfil: " + ddlPerfil.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvExcluidos, "Los siguientes correos nunca se enviarán al usuario", lstComentarios)
    End Sub

    Protected Sub btnExportaExcelEnvian_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcelEnvian.Click
        Dim lstEnvian As List(Of Correo) = TryCast(gvEnvian.DataSourceSession, List(Of Correo))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entSubMenu As Correo In lstEnvian
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entSubMenu.Identificador
            dr.Item("Descripcion") = entSubMenu.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Para el Usuario: " + ddlUsuario.SelectedValue)
        lstComentarios.Add("Perfil: " + ddlPerfil.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvEnvian, "Correos disponibles que se envían", lstComentarios)
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvEnvian.Rows.Count > 0 Then
            imgEnvian.Visible = False
        Else
            imgEnvian.Visible = True
        End If

        If gvExcluidos.Rows.Count > 0 Then
            imgExcluidos.Visible = False
        Else
            imgExcluidos.Visible = True
        End If
        If gvEnvian.Rows.Count = 0 And gvExcluidos.Rows.Count = 0 Then
            imgDatos.Visible = True
            pnlCorreos.Visible = False
        Else
            imgDatos.Visible = False
            pnlCorreos.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los registros agregados a gvExcluidos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of Correo)
        Dim entCorreo As New Correo()
        Dim lstExcluidos As List(Of Correo) = entCorreo.ObtenerCorreosExcluyeUsuario(ddlUsuario.SelectedValue, Convert.ToInt32(ddlPerfil.SelectedValue))

        Dim lstAgregadosExclucion As New List(Of Correo)

        For Each gvrow As GridViewRow In gvExcluidos.Rows
            Dim existe As Boolean = False
            For index = 0 To lstExcluidos.Count - 1
                If lstExcluidos(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregadosExclucion.Add(New Correo(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregadosExclucion
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvEnvian
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of Correo)
        Dim entCorreo As New Correo()
        Dim lstDisponibles As List(Of Correo) = entCorreo.ObtenerCorreosUsuario(ddlUsuario.SelectedValue, Convert.ToInt32(ddlPerfil.SelectedValue))

        Dim lstEliminados As New List(Of Correo)

        For Each gvrow As GridViewRow In gvEnvian.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New Correo(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstEliminados
    End Function

    ''' <summary>
    ''' Actualiza las modificaciones realizadas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Guardar()
        Try
            Dim lstExcluidos As List(Of Correo) = ObtenAgregados()

            Dim lstEnvian As List(Of Correo) = ObtenEliminados()

            Dim entCorreo As New Correo()

            entCorreo.GuardaExclusion(ddlUsuario.SelectedValue, Convert.ToInt32(ddlPerfil.SelectedValue), lstEnvian, lstExcluidos)

            CargaCorreos()
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(55)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub

#End Region

#Region "Validaciones"
    Private Sub cvPerfil_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvPerfil.ServerValidate
        If ddlPerfil.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(25)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        End If
    End Sub

    Private Sub cvUsuario_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvUsuario.ServerValidate
        If ddlUsuario.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(25)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        End If
    End Sub
#End Region

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("~/Correos/MenuOpciones.aspx")
    End Sub
End Class