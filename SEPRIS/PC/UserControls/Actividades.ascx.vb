Public Class Actividades
    Inherits System.Web.UI.UserControl

    Public Property Mensaje As String

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property


    Public ReadOnly Property Usuario_Act As String
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        End Get
    End Property


    Public Property IDActividad As Integer
    Public Property IDActividadReal As Integer

    Public Sub Inicializar()

        If Not Page.IsPostBack Then

            CargarFiltros()
            btnFiltarClick(Nothing, Nothing)

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 7
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If

        End If
    End Sub

    Public Sub InicializarR()

        If Not Page.IsPostBack Then
            CargarFiltros()
            btnFiltarClick(Nothing, Nothing)

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 7
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
                gvConsulta.Columns(0).Visible = False
                btnAgregar.Visible = False
                btnModificar2.Visible = False
                btnProrrogar.Visible = False
            End If
        End If

    End Sub


    Private Sub GuardaCamposProrroga()
        Dim lstCamposPro As New List(Of String)
        Dim lstValoresPro As New List(Of Object)

        With lstCamposPro
            .Add("I_ID_ACTIVIDAD")
            .Add("T_DSC_PRORROGA")
            .Add("I_NUM_DIA_PRORROGA")
            .Add("T_ID_USUARIO")
            .Add("F_FECH_PRORROGA")
            .Add("F_FECH_ENTREGA")
        End With

        With lstValoresPro
            .Add(TxtIDActividadProReal.Text)
            .Add(TxtMotivoProrroga.Text)
            .Add(TxtDiasProrroga.Text)
            .Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            .Add(Now().ToString("yyyy/MM/dd"))
            .Add(Mid(TxtFecEntrProrroga.Text, 8, 4) & Mid(TxtFecEntrProrroga.Text, 4, 4) & Mid(TxtFecEntrProrroga.Text, 2, 2))
        End With
        Actividad.GuardaAltaProrrogaActividad(lstCamposPro, lstValoresPro, "En Proceso")

    End Sub

    ''' <summary>
    ''' Realiza la carga inicial de los filtros dinamicos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarFiltros()

        ucFiltro3.SessionID = "Actividades"
        ucFiltro3.resetSession()

        Dim consulta As String = "1=1"

        Dim objEstatus() = {New With {.DSC_APROBADO = "Completa", .B_FLAG_APROBADO = 0},
                           New With {.DSC_APROBADO = "Incompleta", .B_FLAG_APROBADO = 1},
                           New With {.DSC_APROBADO = "En proceso", .B_FLAG_APROBADO = 2},
                           New With {.DSC_APROBADO = "Prórrogada", .B_FLAG_APROBADO = 3}}

        ucFiltro3.AddFilter("Estatus", ucFiltro3.AcceptedControls.DropDownList, objEstatus, "DSC_APROBADO", "B_FLAG_APROBADO", ucFiltro3.DataValueType.IntegerType, , , , True, True)



        ucFiltro3.AddFilter("Consecutivo              ", ucFiltro3.AcceptedControls.TextBox, Nothing, "I_ID_ACTIVIDAD", "I_ID_ACTIVIDAD", ucFiltro3.DataValueType.IntegerType, , , , , , , 10)
        ucFiltro3.AddFilter("Actividad a entregar     ", ucFiltro3.AcceptedControls.TextBox, Nothing, "T_DSC_ACTIVIDAD", "T_DSC_ACTIVIDAD", ucFiltro3.DataValueType.StringType)
        ucFiltro3.AddFilter("Fecha de entrega inicial ", ucFiltro3.AcceptedControls.Calendar, Nothing, "F_FECH_ENTREGA", "F_FECH_ENTREGA", ucFiltro3.DataValueType.StringType, False, , True)
        ucFiltro3.AddFilter("Fecha de entrega actual  ", ucFiltro3.AcceptedControls.Calendar, Nothing, "F_FECH_ENTREGA_PRO", "F_FECH_ENTREGA_PRO", ucFiltro3.DataValueType.StringType, False, , True)
        ucFiltro3.AddFilter("Usuario registró         ", ucFiltro3.AcceptedControls.TextBox, Nothing, "T_ID_USUARIO", "T_ID_USUARIO", ucFiltro3.DataValueType.StringType)
        ucFiltro3.AddFilter("Usuario modificó         ", ucFiltro3.AcceptedControls.TextBox, Nothing, "T_ID_USUARIO_MOD", "T_ID_USUARIO_MOD", ucFiltro3.DataValueType.StringType)
        ucFiltro3.AddFilter("Fecha de modificación    ", ucFiltro3.AcceptedControls.Calendar, Nothing, "F_FECH_COMENTARIO", "F_FECH_COMENTARIO", ucFiltro3.DataValueType.StringType, False, , True)

        ucFiltro3.LoadDDL("Actividades.ascx")


    End Sub

    Protected Sub btnFiltarClick(sender As Object, e As EventArgs) Handles ucFiltro3.Filtrar

        Dim consulta As String = "WHERE 1=1 "
        For Each filtro In ucFiltro3.getFilterSelection

            consulta += "AND " + filtro

        Next


        gvConsulta.DataSource = Actividad.ObtenerTodas(Folio)
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If

        ucFiltro3.LoadDDL("Actividades.ascx")

    End Sub



    Private Sub btnPersonalizarColumnas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizarColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 7
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 7
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Session("ID_FOLIO") = CInt(gvConsulta.DataKeys(index)("ID_FOLIO").ToString())
        Response.Redirect("~/PC/Paso1.aspx", False)

    End Sub

    Public Function ObtenerImagenEstatus(ByVal strEstatus As String) As String
        Select Case strEstatus
            Case "Completa"
                Return "~/Imagenes/aceptado.png"
            Case "Incompleta"
                Return "~/Imagenes/ERROR.jpg"
            Case "En Proceso"
                Return "~/Images/ORIENTACION.png"
            Case "Prorrogada"
                Return "~/Images/PREVENTIVO.jpg"
            Case Else
                Return Nothing
        End Select
    End Function

    Private Sub btnAceptarM2B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarM2B1A.Click

    End Sub

    Protected Sub BuscaRegistroSeleccionado()
        For Each row As GridViewRow In gvConsulta.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim rdbRow As RadioButton = TryCast(row.Cells(0).FindControl("rdbElemento"), RadioButton)
                If rdbRow.Checked Then
                    IDActividad = row.Cells(1).Text
                    Exit For
                End If
            End If
        Next
    End Sub


    Protected Sub btnModificar2_Click(sender As Object, e As EventArgs) Handles btnModificar2.Click
        Dim aDatos As String()

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                IDActividadReal = CInt(gvConsulta.DataKeys(row.RowIndex)(0).ToString())
                IDActividad = CInt(gvConsulta.Rows(row.RowIndex).Cells(1).Text)
                aDatos = Actividad.CargaDatosModificacion(IDActividadReal)
                TxtNumActividad.Text = IDActividad
                TxtNumActividadReal.Text = IDActividadReal
                TxtDescActividadMod.Text = aDatos(0)
                TxtFecEstimEntregMod.Text = aDatos(1)
                TxtComentariosMod.Text = String.Empty
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Modificar actividades", "ModificaActividad();", True)

                Return
            End If
        Next
    End Sub

    Protected Sub btnProrrogar_Click(sender As Object, e As EventArgs) Handles btnProrrogar.Click
        Dim aDatos As String()

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                IDActividadReal = CInt(gvConsulta.DataKeys(row.RowIndex)(0).ToString())
                IDActividad = CInt(gvConsulta.Rows(row.RowIndex).Cells(1).Text)
                aDatos = Actividad.CargaDatosProrroga(IDActividadReal)
                TxtIDActividadPro.Text = IDActividad
                TxtIDActividadProReal.Text = IDActividadReal
                TxtDescActividadPro.Text = aDatos(0)
                TxtFecEstEntregaPro.Text = aDatos(1)
                TxtComentariosMod.Text = String.Empty
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Prorrogar actividades", "ProrrogarActividad();", True)

                Return
            End If
        Next
    End Sub

    Protected Sub gvConsulta_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvConsulta.RowCreated

        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then
            Dim Actividad As Integer = CInt(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("I_ID_ACTIVIDAD"))
            Dim row_number As Integer = CInt(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("Row#"))
            e.Row.Attributes.Add("onclick", "GuardaValorGrid(" + Actividad.ToString() + "," + row_number.ToString() + ");")
        End If

    End Sub
End Class