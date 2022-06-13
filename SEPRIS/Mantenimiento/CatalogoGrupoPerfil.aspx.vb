Imports Entities

Public Class CatalogoGrupoPerfil
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargaPerfiles()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvAsignados.ArmaMultiScript()
    End Sub

#Region "Carga Datos"
    ''' <summary>
    ''' Carga los grupos en el DropDownList ddlGrupo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaPerfiles()
        Dim entPerfil As New Perfil
        Dim dvPerfil As DataView = entPerfil.ObtenerTodos
        dvPerfil.RowFilter = "N_FLAG_VIG=1"

        Utilerias.Generales.CargarCombo(ddlPerfil, dvPerfil.ToTable, "T_DSC_PERFIL", "N_ID_PERFIL")

    End Sub

    ''' <summary>
    ''' Carga los correos en los GridViews
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaGrupos()

        If ddlPerfil.SelectedValue <> "-1" Then
            pnlGrupos.Style.Add("display", "block")
            btnMostrarMensaje.Visible = True
            btnCancelar.Visible = True

            Dim entPerfil As New Perfil

            gvAsignados.DataSource = entPerfil.ObtenerGrupos(Convert.ToInt32(ddlPerfil.SelectedValue))
            gvAsignados.DataBind()

            gvDisponibles.DataSource = entPerfil.ObtenerGruposDisponibles(Convert.ToInt32(ddlPerfil.SelectedValue))
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlGrupos.Style.Add("display", "none")
            btnMostrarMensaje.Visible = False
            btnCancelar.Visible = False
        End If

    End Sub
#End Region

#Region "Eventos de Controles"
    Protected Sub ddlPerfil_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPerfil.SelectedIndexChanged
        CargaGrupos()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If ddlPerfil.SelectedValue <> "-1" Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(85)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    lblMensaje.Text = errores.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"
                ddlPerfil.SelectedValue = "-1"
                CargaGrupos()
        End Select


    End Sub

    Protected Sub btnMostrarMensaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMostrarMensaje.Click
        Page.Validate("Forma")

        If Page.IsValid() Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            Dim errores As New EtiquetaError(86)
            imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Guardar", "LevantaVentanaConfirma();", True)
        Else
            lblMensaje.Text = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New EtiquetaError(87)
        imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        Mensaje = errores.Descripcion
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvAsignados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of Grupo) = TryCast(gvAsignados.DataSourceSession, List(Of Grupo))
            Dim lstDisponibles As List(Of Grupo) = TryCast(gvDisponibles.DataSourceSession, List(Of Grupo))
            For Each gvRow In lstGVRow
                For index = 0 To lstUsados.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstUsados(index).Identificador Then
                        lstDisponibles.Add(lstUsados(index))
                        lstUsados.Remove(lstUsados(index))
                        Exit For
                    End If
                Next
            Next

            gvAsignados.DataSource = lstUsados
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstDisponibles
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(88)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvDisponibles.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of Grupo) = TryCast(gvAsignados.DataSourceSession, List(Of Grupo))
            Dim lstDisponibles As List(Of Grupo) = TryCast(gvDisponibles.DataSourceSession, List(Of Grupo))
            For Each gvRow In lstGVRow
                For index = 0 To lstDisponibles.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstDisponibles(index).Identificador Then
                        lstUsados.Add(lstDisponibles(index))
                        lstDisponibles.Remove(lstDisponibles(index))
                        Exit For
                    End If
                Next
            Next

            gvAsignados.DataSource = lstUsados
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstDisponibles
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(88)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Private Sub btnExportaExcelAsigandos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelAsigandos.Click
        Dim lstDisponibles As List(Of Grupo) = TryCast(gvAsignados.DataSourceSession, List(Of Grupo))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entGrupo As Grupo In lstDisponibles
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entGrupo.Identificador
            dr.Item("Descripcion") = entGrupo.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Perfil: " + ddlPerfil.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvAsignados, "Grupos actualmente usados por el perfil", lstComentarios)
    End Sub

    Private Sub btnExportaExcelDisponibles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelDisponibles.Click
        Dim lstDisponibles As List(Of Grupo) = TryCast(gvDisponibles.DataSourceSession, List(Of Grupo))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entGrupo As Grupo In lstDisponibles
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entGrupo.Identificador
            dr.Item("Descripcion") = entGrupo.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Perfil: " + ddlPerfil.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvDisponibles, "Grupos disponibles para agregar", lstComentarios)
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
            pnlGrupos.Visible = False
        Else
            imgDatos.Visible = False
            pnlGrupos.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los registros agregados a gvAsignados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of Grupo)
        Dim entPerfil As New Perfil()
        Dim lstUsados As List(Of Grupo) = entPerfil.ObtenerGrupos(Convert.ToInt32(ddlPerfil.SelectedValue))

        Dim lstAgregados As New List(Of Grupo)

        For Each gvrow As GridViewRow In gvAsignados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstUsados.Count - 1
                If lstUsados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregados.Add(New Grupo(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of Grupo)
        Dim entPerfil As New Perfil()
        Dim lstDisponibles As List(Of Grupo) = entPerfil.ObtenerGruposDisponibles(Convert.ToInt32(ddlPerfil.SelectedValue))

        Dim lstEliminados As New List(Of Grupo)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New Grupo(Convert.ToInt32(gvrow.Cells(1).Text)))
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

            Dim entPerfil As New Perfil()

            entPerfil.ActualizaFunciones(lstAgregar, lstQuitar, Convert.ToInt32(ddlPerfil.SelectedValue))

            CargaGrupos()
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(89)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub
#End Region

#Region "Validadores"
    Private Sub cvPerfil_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvPerfil.ServerValidate
        If ddlPerfil.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(85)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        End If
    End Sub
#End Region
End Class