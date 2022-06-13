Public Class Bitacora
    Inherits System.Web.UI.Page
    Public Property Mensaje As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarFiltros()
            CargarBitacora()
            CargarImagenesEstatus()
        Else
            gvConsulta.DataSource = gvConsulta.DataSourceSession
            gvConsulta.DataBind()
            gvConsulta.CargaSeleccion()
        End If
    End Sub
    Public Function ObtenerPaginacion() As Integer
        Return CInt(Conexion.SQLServer.Parametro.ObtenerValor("Paginación Bitácora"))
    End Function
    Private Sub CargarFiltros()
        ucFiltro1.resetSession()

        ucFiltro1.AddFilter("Fecha", ucFiltro.AcceptedControls.Calendar, , , "F_FECH_BITACORA", ucFiltro.DataValueType.StringType, , , True, True, True, DateTime.Now.ToString("dd/MM/yyyy"))
        Dim objUsuario As New Entities.Usuario
        ucFiltro1.AddFilter("Usuario", ucFiltro.AcceptedControls.DropDownList, objUsuario.ObtenerTodos.ToTable, "T_ID_USUARIO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Procedimiento", ucFiltro.AcceptedControls.TextBox, , , "PROCEDIMIENTO", ucFiltro.DataValueType.StringType, , True)
        'ucFiltro1.AddFilter("Procedimiento", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "PROCEDIMIENTO", "PROCEDIMIENTO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.LoadDDL("Bitacora.aspx")
    End Sub
    Private Sub CargarBitacora()
        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next
        Dim entBitacora As New Entities.Bitacora
        Dim dv As DataView = entBitacora.ObtenerTodos
        dv.RowFilter = consulta
        gvConsulta.DataSource = dv.ToTable()
        gvConsulta.DataBind()
        If gvConsulta.Rows.Count = 0 Then
            pnlNoExiste.Visible = True
            gvConsulta.Visible = False
        Else
            pnlNoExiste.Visible = False
            gvConsulta.Visible = True
        End If
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
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultar.Click
        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim bitacora As New Entities.Bitacora(gvConsulta.DataKeys(row.RowIndex)("N_ID_BITACORA"))
                txtProcedimiento.Text = bitacora.Procedimiento
                txtFecha.Text = bitacora.Fecha
                txtLinea.Text = bitacora.Linea
                txtFuncion.Text = bitacora.Funcion
                txtTabla.Text = bitacora.Tabla
                Mensaje = String.Empty
                tblMensaje.Visible = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Detalle", "MensajeBitacora();", True)
                Exit For
            End If
        Next
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
            Dim errores As New Entities.EtiquetaError(157)
            Mensaje = errores.Descripcion
            tblMensaje.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "MensajeBitacora();", True)
        End If
        Return haySeleccion
    End Function
    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarBitacora()
    End Sub
    Private Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)
        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("RESULTADO").ColumnName = "RESULTADO"
        utl.ExportaGrid(dt, gvConsulta, "Bitácora de acciones", referencias)
    End Sub
    Protected Sub gvConsulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataBind()
    End Sub
End Class