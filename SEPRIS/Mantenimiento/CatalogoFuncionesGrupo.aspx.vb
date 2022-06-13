Imports Entities

Public Class CatalogoFuncionesGrupo
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargaGrupos()
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
    Private Sub CargaGrupos()
        Dim entGrupo As New Grupo
        Dim dvGrupo As DataView = entGrupo.ObtenerTodos
        dvGrupo.RowFilter = "B_FLAG_VIG=1"

        Utilerias.Generales.CargarCombo(ddlGrupo, dvGrupo.ToTable, "T_DSC_GRUPO", "N_ID_GRUPO")

    End Sub

    ''' <summary>
    ''' Carga los correos en los GridViews
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFunciones()
        If ddlGrupo.SelectedValue <> "-1" Then
            pnlFuncion.Style.Add("display", "block")
            btnMostrarMensaje.Visible = True
            btnCancelar.Visible = True

            Dim entGrupo As New Grupo

            gvAsignados.DataSource = entGrupo.ObtenerFunciones(Convert.ToInt32(ddlGrupo.SelectedValue))
            gvAsignados.DataBind()

            gvDisponibles.DataSource = entGrupo.ObtenerFuncionesDisponibles(Convert.ToInt32(ddlGrupo.SelectedValue))
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlFuncion.Style.Add("display", "none")
            btnMostrarMensaje.Visible = False
            btnCancelar.Visible = False
        End If
    End Sub
#End Region

#Region "Eventos de Controles"
    Protected Sub ddlPerfil_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrupo.SelectedIndexChanged
            CargaFunciones()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If ddlGrupo.SelectedValue <> "-1" Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(80)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    lblMensaje.Text = errores.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"
                ddlGrupo.SelectedValue = "-1"
                CargaFunciones()
        End Select


    End Sub

    Protected Sub btnMostrarMensaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMostrarMensaje.Click
        Page.Validate("Forma")

        If Page.IsValid() Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"
            Dim errores As New EtiquetaError(81)
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
        Dim errores As New EtiquetaError(82)
        imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        Mensaje = errores.Descripcion
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvAsignados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of Funcion) = TryCast(gvAsignados.DataSourceSession, List(Of Funcion))
            Dim lstDisponibles As List(Of Funcion) = TryCast(gvDisponibles.DataSourceSession, List(Of Funcion))
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
            Dim errores As New Entities.EtiquetaError(83)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvDisponibles.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of Funcion) = TryCast(gvAsignados.DataSourceSession, List(Of Funcion))
            Dim lstDisponibles As List(Of Funcion) = TryCast(gvDisponibles.DataSourceSession, List(Of Funcion))
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
            Dim errores As New Entities.EtiquetaError(83)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Private Sub btnExportaExcelAsigandos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelAsigandos.Click
        Dim lstUsadas As List(Of Funcion) = TryCast(gvAsignados.DataSourceSession, List(Of Funcion))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entFuncion As Funcion In lstUsadas
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entFuncion.Identificador
            dr.Item("Descripcion") = entFuncion.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Grupo: " + ddlGrupo.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvAsignados, "Funciones actualmente usadas por el grupo", lstComentarios)
    End Sub

    Private Sub btnExportaExcelDisponibles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelDisponibles.Click
        Dim lstDisponibles As List(Of Funcion) = TryCast(gvDisponibles.DataSourceSession, List(Of Funcion))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entFuncion As Funcion In lstDisponibles
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entFuncion.Identificador
            dr.Item("Descripcion") = entFuncion.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Grupo: " + ddlGrupo.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvDisponibles, "Funciones disponibles para agregar", lstComentarios)
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
            pnlFuncion.Visible = False
        Else
            imgDatos.Visible = False
            pnlFuncion.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los registros agregados a gvAsignados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of Funcion)
        Dim entGrupo As New Grupo()
        Dim lstUsados As List(Of Funcion) = entGrupo.ObtenerFunciones(Convert.ToInt32(ddlGrupo.SelectedValue))

        Dim lstAgregados As New List(Of Funcion)

        For Each gvrow As GridViewRow In gvAsignados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstUsados.Count - 1
                If lstUsados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregados.Add(New Funcion(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of Funcion)
        Dim entGrupo As New Grupo()
        Dim lstDisponibles As List(Of Funcion) = entGrupo.ObtenerFuncionesDisponibles(Convert.ToInt32(ddlGrupo.SelectedValue))

        Dim lstEliminados As New List(Of Funcion)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New Funcion(Convert.ToInt32(gvrow.Cells(1).Text)))
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

            Dim entGrupo As New Grupo()

            entGrupo.ActualizaFunciones(lstAgregar, lstQuitar, Convert.ToInt32(ddlGrupo.SelectedValue))

            CargaFunciones()
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(84)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub
#End Region

#Region "Validadores"
    Private Sub cvPerfil_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvGrupo.ServerValidate
        If ddlGrupo.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(80)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        End If
    End Sub
#End Region

End Class