Public Class NotificacionesBandeja
    Inherits System.Web.UI.Page

    '***********************************************************************************************************
    ' Fecha Creación:       21 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Pantalla inicial de notificaciones (Bandeja)
    '***********************************************************************************************************
    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            CargaFiltros()
            CargaDatos()

        End If

    End Sub

#Region "Acciones de los botones"

    Private Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        ' Redireccionamos a la pantalla para agregar un nuevo mensaje
        Response.Redirect("NotificacionesDatos.aspx", False)

    End Sub

    Private Sub btnModificar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificar.Click

        ViewState("Seleccionado") = grvNotificaciones.SelectedIndex.ToString

        ' Verificamos que se haya seleccionado algo
        If ViewState("Seleccionado") = -1 Then

            Dim errores As New Entities.EtiquetaError(23)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub

        End If

        Dim notificacion As Entities.Notificaciones.NotificacionPantalla = Entities.Notificaciones.NotificacionesPantallaGetOne(CInt(grvNotificaciones.SelectedRow.Cells(1).Text))

        If Not notificacion.Vigente Then
            Dim errores As New Entities.EtiquetaError(154)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub
        End If


        ' Redireccionamos a la pantalla para modificar el mensaje seleccionado
        Response.Redirect("NotificacionesDatos.aspx?idmensaje=" & grvNotificaciones.SelectedRow.Cells(1).Text, False)

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Dim errores As Entities.EtiquetaError

        ' Procesamos elemento seleccionado
        ViewState("Seleccionado") = grvNotificaciones.SelectedIndex.ToString

        ' Verificamos que se haya seleccionado algo
        If ViewState("Seleccionado") = -1 Then

            errores = New Entities.EtiquetaError(23)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub

        End If


        Dim notificacion As Entities.Notificaciones.NotificacionPantalla = Entities.Notificaciones.NotificacionesPantallaGetOne(CInt(grvNotificaciones.SelectedRow.Cells(1).Text))

        If Not notificacion.Vigente Then

            errores = New Entities.EtiquetaError(154)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub

        End If


        ViewState("Seleccionado") = grvNotificaciones.SelectedRow.Cells(1).Text
        ' Solicitamos confirmacion para eliminar
        errores = New Entities.EtiquetaError(155)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosConfirmacion", "MostrarConfirmacion();", True)
        
    End Sub

    Private Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim datos As DataTable = TryCast(grvNotificaciones.DataSourceSession, DataTable)
        datos.Columns("Vigente").ColumnName = "Estatus"

        utl.ExportaGrid(datos, grvNotificaciones, "Mensajes de Notificacion", referencias)

    End Sub

    Private Sub Filtros_Filtrar(ByVal sender As Object, ByVal e As System.EventArgs) Handles Filtros.Filtrar

        ' Obtenemos datos
        CargaDatos()

    End Sub

    Private Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM2B1A.Click

        Dim errores As Entities.EtiquetaError
        ' realizamos operación de eliminado (lógico)
        If Not Entities.Notificaciones.EliminaNotificacionPantalla(CInt(ViewState("Seleccionado"))) Then

            ' Si no se pudo eliminar
            errores = New Entities.EtiquetaError(29)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub

        End If

        ' Mostramos mensaje de confirmacion
        errores = New Entities.EtiquetaError(156)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosAceptar", "MostrarAceptar();", True)

    End Sub

    Private Sub btnAceptarM1B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM1B1A.Click

        ' Recargamos datos
        CargaDatos()

    End Sub

#End Region

#Region "Métodos Generales"

    ''' <summary>
    ''' Método para cargar datos de las notificaciones existentes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDatos()

        Dim dt = Entities.Notificaciones.NotificacionesPantallaGetCustom(GetFiltro())

        Dim datos As New DataTable
        datos.Columns.Add("Identificador")
        datos.Columns.Add("Titulo")
        datos.Columns.Add("Vigente")
        datos.AcceptChanges()

        For Each elemento As Entities.Notificaciones.NotificacionPantalla In dt

            Dim dr As DataRow = datos.NewRow
            dr.Item("Identificador") = elemento.Identificador.ToString()
            dr.Item("Titulo") = elemento.Titulo
            dr.Item("Vigente") = elemento.Vigente
            datos.Rows.Add(dr)

        Next
        datos.AcceptChanges()

        'Me.grvNotificaciones.DataSource = Entities.Notificaciones.NotificacionesPantallaGetCustom(GetFiltro())
        Me.grvNotificaciones.DataSource = datos
        Me.grvNotificaciones.DataBind()
        MuestraGridViewImagen()

    End Sub

    ''' <summary>
    ''' Método para definir y cargar los filtros
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFiltros()

        Filtros.resetSession()

        ' Obtenemos datos de notificaciones
        Dim datos As List(Of Entities.Notificaciones.NotificacionPantalla) = Entities.Notificaciones.NotificacionesPantallaGetAll()


        Filtros.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)

        Dim claves = From p In datos Select New With {.N_ID_NOTIFICACION_PANTALLA = p.Identificador, .Identificador = p.Identificador}
        Filtros.AddFilter("Clave  ", SEPRIS.ucFiltro.AcceptedControls.DropDownList, claves, "Identificador", "N_ID_NOTIFICACION_PANTALLA", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing)

        Dim nombres = From p In datos Select New With {.N_ID_NOTIFICACION_PANTALLA = p.Identificador, .Descripcion = p.Titulo}
        Filtros.AddFilter("Nombre     ", SEPRIS.ucFiltro.AcceptedControls.DropDownList, nombres, "Descripcion", "N_ID_NOTIFICACION_PANTALLA", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing)

        Filtros.LoadDDL("NotificacionesBandeja")

    End Sub

    ''' <summary>
    ''' Método para determinar la imagen de vigencia a mostrar
    ''' </summary>
    ''' <param name="Vigente"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ImagenVigencia(ByVal Vigente As String) As String
        If CBool(Vigente) Then
            Return "/imagenes/vigente.gif"
        Else
            Return "/imagenes/no_vigente.gif"
        End If
    End Function

    ''' <summary>
    ''' Método para obtener el filtro de búsqueda con los elementos seleccionados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFiltro() As String

        Dim filtros = Me.Filtros.getFilterSelection
        Dim where = "WHERE 1=1 "
        For Each filtro As String In filtros
            where += " AND " + filtro
        Next

        Return where

    End Function

#End Region

    ''' <summary>
    ''' Dependiendo si el GridView no tiene registros muestra una imagen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraGridViewImagen()
        If grvNotificaciones.Rows.Count() > 0 Then
            grvNotificaciones.Visible = True
            pnlNoExiste.Visible = False
        Else
            grvNotificaciones.Visible = False
            pnlNoExiste.Visible = True
        End If
    End Sub

    Private Sub grvNotificaciones_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grvNotificaciones.Sorting

        grvNotificaciones.Ordenar(e)

    End Sub

End Class