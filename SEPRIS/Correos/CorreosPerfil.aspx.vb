'- Fecha de creación: 26/07/2013
'- Nombre del Responsable: Rafael Rodríguez Sánchez
'- Empresa: Softtek
'- Permite agregar o remover correos a de los perfiles

Imports Entities
Public Class CorreosPerfil
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargaAreas()
            'CargaPerfiles()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvAsignados.ArmaMultiScript()
    End Sub

#Region "Carga Datos"
    ''' <summary>
    ''' Carga los perfiles en el DropDownList ddlPerfil
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaAreas()
        Dim area As New Areas
        Dim lstPerfil As DataSet

        lstPerfil = area.ObtenerTodosActivos()
        Utilerias.Generales.CargarCombo(ddlArea, lstPerfil, "T_DSC_AREA", "I_ID_AREA")

    End Sub

    Private Sub CargaPerfiles(ByVal valor As Integer)
        Dim entPerfil As New Perfil
        Dim lstPerfil As New List(Of Perfil)
        lstPerfil = entPerfil.ObtenerPerfilesVigentes(valor.ToString())

        Utilerias.Generales.CargarCombo(ddlPerfil, lstPerfil, "Descripcion", "IdentificadorPerfil")

    End Sub



    ''' <summary>
    ''' Carga los correos en los GridViews
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaCorreos()

        If ddlPerfil.SelectedValue <> "-1" Then
            pnlCorreos.Style.Add("display", "block")
            btnMostrarMensaje.Visible = True
            btnCancelar.Visible = True
            Dim entCorreo As New Correo
            Dim lstCorreoAsignados As List(Of Correo) = entCorreo.ObtenerCorreosPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue))
            Dim lstCorreoNoAsignado As List(Of Correo) = entCorreo.ObtenerCorreosNoEnPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue))

            gvAsignados.DataSource = lstCorreoAsignados
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstCorreoNoAsignado
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlCorreos.Style.Add("display", "none")
            btnMostrarMensaje.Visible = False
            btnCancelar.Visible = False
        End If

    End Sub
#End Region

#Region "Eventos de Controles"
    Protected Sub ddlPerfil_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPerfil.SelectedIndexChanged
        CargaCorreos()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If ddlPerfil.SelectedValue <> "-1" Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(25)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    lblMensaje.Text = errores.Descripcion
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

            Dim errores As New Entities.EtiquetaError(164)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Guardar", "LevantaVentanaConfirma();", True)
        Else
            lblMensaje.Text = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(165)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvAsignados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstExcluidos As List(Of Correo) = TryCast(gvAsignados.DataSourceSession, List(Of Correo))
            Dim lstEnvian As List(Of Correo) = TryCast(gvDisponibles.DataSourceSession, List(Of Correo))
            For Each gvRow In lstGVRow
                For index = 0 To lstExcluidos.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstExcluidos(index).Identificador Then
                        lstEnvian.Add(lstExcluidos(index))
                        lstExcluidos.Remove(lstExcluidos(index))
                        Exit For
                    End If
                Next
            Next

            gvAsignados.DataSource = lstExcluidos
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstEnvian
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvDisponibles.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstExcluidos As List(Of Correo) = TryCast(gvAsignados.DataSourceSession, List(Of Correo))
            Dim lstEnvian As List(Of Correo) = TryCast(gvDisponibles.DataSourceSession, List(Of Correo))
            For Each gvRow In lstGVRow
                For index = 0 To lstEnvian.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstEnvian(index).Identificador Then
                        lstExcluidos.Add(lstEnvian(index))
                        lstEnvian.Remove(lstEnvian(index))
                        Exit For
                    End If
                Next
            Next

            gvAsignados.DataSource = lstExcluidos
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstEnvian
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Private Sub btnExportaExcelAsigandos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelAsigandos.Click
        Dim lstExcluidos As List(Of Correo) = TryCast(gvAsignados.DataSourceSession, List(Of Correo))
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
        lstComentarios.Add("Perfil: " + ddlPerfil.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvAsignados, "Correos asignados al perfil", lstComentarios)
    End Sub

    Private Sub btnExportaExcelDisponibles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelDisponibles.Click
        Dim lstExcluidos As List(Of Correo) = TryCast(gvDisponibles.DataSourceSession, List(Of Correo))
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
        lstComentarios.Add("Perfil: " + ddlPerfil.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvDisponibles, "Correos disponibles", lstComentarios)
    End Sub

#End Region

#Region "Metodos"
    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvAsignados.Rows.Count > 0 Then
            imgAsignados.Visible = False
        Else
            imgAsignados.Visible = True
        End If

        If gvDisponibles.Rows.Count > 0 Then
            imgDisponibles.Visible = False
        Else
            imgDisponibles.Visible = True
        End If

        If gvDisponibles.Rows.Count = 0 And gvAsignados.Rows.Count = 0 Then
            imgDatos.Visible = True
            pnlCorreos.Visible = False
        Else
            imgDatos.Visible = False
            pnlCorreos.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los registros agregados a gvAsignados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of Correo)
        Dim entCorreo As New Correo()
        Dim lstAsignados As List(Of Correo) = entCorreo.ObtenerCorreosPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue))

        Dim lstAgregados As New List(Of Correo)

        For Each gvrow As GridViewRow In gvAsignados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstAsignados.Count - 1
                If lstAsignados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregados.Add(New Correo(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of Correo)
        Dim entCorreo As New Correo()
        Dim lstDisponibles As List(Of Correo) = entCorreo.ObtenerCorreosNoEnPerfil(Convert.ToInt32(ddlPerfil.SelectedValue))

        Dim lstEliminados As New List(Of Correo)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
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
            Dim lstAgregar = ObtenAgregados()
            Dim lstQuitar = ObtenEliminados()

            Dim entCorreo As New Correo()

            entCorreo.GuardaCorreoPerfil(lstAgregar, lstQuitar, Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlArea.SelectedValue))

            CargaCorreos()
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(55)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub
#End Region

#Region "Validadores"
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
#End Region

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("~/Correos/MenuOpciones.aspx")
    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlArea.SelectedIndexChanged
        Dim valor As Integer = ddlArea.SelectedValue
        CargaPerfiles(valor)
        ddlPerfil.Visible = True
        lbPerfil.Visible = True
        pnlCorreos.Style.Add("Display", "none")
    End Sub
End Class