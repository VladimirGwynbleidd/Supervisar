Public Class Irregularidades
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public Sub Inicializar()
        CargarFiltros()
        CargarIrregularidades()

        If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
            PersonalizaColumnas.IdentificadorGridView = 9
            PersonalizaColumnas.GridViewPersonalizar = gvConsulta
            PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
            PersonalizaColumnas.Personalizar()
        End If
    End Sub

    Public Sub InicializarR()
        CargarFiltros()
        CargarIrregularidades()

        If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
            PersonalizaColumnas.IdentificadorGridView = 9
            PersonalizaColumnas.GridViewPersonalizar = gvConsulta
            PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
            PersonalizaColumnas.Personalizar()
            gvConsulta.Columns(0).Visible = False
            btnAgregar.Visible = False
            btnModificar.Visible = False
            btnCompletar.Visible = False
        End If
    End Sub

    Private Sub btnPersonalizarColumnas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizarColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 9
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 9
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

    ''' <summary>
    ''' Realiza la carga inicial de los filtros dinamicos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarFiltros()

        'ucFiltro1.resetSession()

        'Dim consulta As String = "1=1"

        'Dim objEstatus() = {New With {.DSC_APROBADO = "Completa", .B_FLAG_APROBADO = 0},
        '                   New With {.DSC_APROBADO = "Incompleta", .B_FLAG_APROBADO = 1}}

        'ucFiltro1.AddFilter("Consecutivo   ", ucFiltro.AcceptedControls.TextBox, Nothing, "I_ID_ACTIVIDAD", "I_ID_ACTIVIDAD", ucFiltro.DataValueType.IntegerType, , , , , , , 10)
        'ucFiltro1.AddFilter("Fecha Irregularidad", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_IRREGULARIDAD", "F_FECH_IRREGULARIDAD", ucFiltro.DataValueType.StringType, False, , True)
        'ucFiltro1.AddFilter("Proceso", ucFiltro.AcceptedControls.TextBox, Nothing, "DESC_PROCESO", "DESC_PROCESO", ucFiltro.DataValueType.StringType, , , , , , , 50)
        'ucFiltro1.AddFilter("Subproceso", ucFiltro.AcceptedControls.TextBox, Nothing, "DESC_SUBPROCESO", "DESC_SUBPROCESO", ucFiltro.DataValueType.StringType, , , , , , , 50)
        'ucFiltro1.AddFilter("Conducta", ucFiltro.AcceptedControls.TextBox, Nothing, "DESC_CONDUCTA", "DESC_CONDUCTA", ucFiltro.DataValueType.StringType, , , , , , , 50)
        'ucFiltro1.AddFilter("Irregularidad", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_IRREGULARIDAD", "T_DSC_IRREGULARIDAD", ucFiltro.DataValueType.StringType, , , , , , , 50)
        'ucFiltro1.AddFilter("Comentarios", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_COMENTARIO", "T_DSC_COMENTARIO", ucFiltro.DataValueType.StringType, , , , , , , 50)

        ''Clasificación
        ''ucFiltro1.AddFilter("Entidads   ", ucFiltro.AcceptedControls.DropDownList, objAutoriza, "AUTORIZADOR", "T_ID_AUTORIZADOR", ucFiltro.DataValueType.StringType)
        'ucFiltro1.LoadDDL("Irregularidades.ascx")


    End Sub

    ''' <summary>
    ''' Obtiene los filtros y carga los registros de la badeja
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarIrregularidades()
        gvConsulta.DataSource = Entities.Irregularidad.ObtenerTodas(Folio)
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If
    End Sub
    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarIrregularidad(idIrregularidad As Integer) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        lstCampos.Add("I_ID_IRREGULARIDAD") : lstValores.Add(idIrregularidad)

        Return Entities.Irregularidad.EliminarIrregularidad(lstCampos, lstValores)
    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerProcesos() As List(Of ListItem)
        Dim procesos = ConexionSISAN.ObtenerProcesos()
        Dim listProceso As New List(Of ListItem)

        For Each row As DataRow In procesos.Rows
            Dim li As New ListItem
            li.Value = row.Item("ID_PROCESO")
            li.Text = row.Item("DESC_PROCESO")
            listProceso.Add(li)
        Next row

        Return listProceso
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ActualizarIrregularidad(idIrregularidad As Integer) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        lstCampos.Add("I_ID_IRREGULARIDAD") : lstValores.Add(idIrregularidad)

        Return Entities.Irregularidad.EliminarIrregularidad(lstCampos, lstValores)
    End Function


    Private Sub gvConsulta_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then
            Dim irreguaridad As Integer = CInt(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("I_ID_IRREGULARIDAD"))
            Dim row_number As Integer = CInt(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("Row#"))
            e.Row.Attributes.Add("onclick", "$('#hfSelectedValueIrregularidad').val(" + irreguaridad.ToString() + "); $('#hfSelectedRowIrregularidad').val(" + row_number.ToString() + ");")


        End If

    End Sub

End Class