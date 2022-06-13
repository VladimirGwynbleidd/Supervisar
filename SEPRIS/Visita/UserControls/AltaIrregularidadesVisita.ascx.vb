Public Class AltaIrregularidadesVisita
    Inherits System.Web.UI.UserControl
    Public ReadOnly Property Folio
        Get
            Return Session("ID_VISITA")
        End Get
    End Property
    Public ReadOnly Property IdEstatusActual
        Get
            Dim visita As Visita = CType(Session("DETALLE_VISITA_V17"), Visita)
            Dim idEstatus As Integer
            If Not IsNothing(visita) Then
                idEstatus = visita.IdEstatusActual
            Else
                idEstatus = 0
            End If
            Return idEstatus
        End Get
    End Property
    Public ReadOnly Property IdPasoActual
        Get
            Dim visita As Visita = CType(Session("DETALLE_VISITA_V17"), Visita)
            Dim idEstatus As Integer
            If Not IsNothing(visita) Then
                idEstatus = visita.IdPasoActual
            Else
                idEstatus = 0
            End If
            Return idEstatus
        End Get
    End Property
    Public WriteOnly Property ActivaAltaIrregularidad
        Set(value)
            divAltaIrregularidades.Visible = value
        End Set
    End Property
    Public WriteOnly Property ActivaBandejaIrregularidad
        Set(value)
            DivBandejaIrregularidad.Visible = value
        End Set
    End Property
    Public ReadOnly Property puObjUsuario As Entities.Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Else
                Return Nothing
            End If
        End Get
    End Property
    <System.ComponentModel.Browsable(False)>
    Public Property BotonesVisibles As Boolean
        Get
            Return pnlBotones.Visible
        End Get
        Set(value As Boolean)
            pnlBotones.Visible = value
        End Set
    End Property
    Public Sub Inicializar(FolioVisita As Integer)
        'Dim IdArea As Integer = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        ddlProceso.DataTextField = "DESC_PROCESO"
        ddlProceso.DataValueField = "ID_PROCESO"
        ddlProceso.DataSource = ConexionSISAN.ObtenerProcesos()
        ddlProceso.DataBind()
        ddlProceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        ddlProceso.Enabled = True
        ddlSubProceso.Enabled = True
        ddlConducta.Enabled = True
        ddlIrregularidad.Enabled = True

        HiddenFieldFolioVisita.Value = FolioVisita

        LblError.Visible = False
        LblIrregularidades.Visible = False
        CargarIrregularidades(FolioVisita)

        'DivModificaIrregVisita.Visible = False
        Dim visita As Visita = CType(Session("DETALLE_VISITA_V17"), Visita)
        If Not IsNothing(visita) Then
            'If visita.IdPasoActual = 17 And visita.IdEstatusActual = 12 Then
            '    pnlBotones.Enabled = False
            '    divAltaIrregularidades.Visible = False
            'End If

            Select Case visita.IdPasoActual
                Case 17
                    If visita.IdEstatusActual = 12 Then
                        pnlBotones.Enabled = False
                        divAltaIrregularidades.Visible = False
                    End If

                    Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP, Constantes.PERFIL_SOLO_CARGA
                            If Not EsAreaOperativa(puObjUsuario.IdArea) Then
                                divAltaIrregularidades.Visible = False
                            End If
                    End Select
            End Select
        End If

    End Sub
    Protected Sub ddlProcesoAlta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProceso.SelectedIndexChanged
        If Not (ddlProceso.SelectedValue <> "" AndAlso ddlProceso.SelectedValue <> "-1") Then Exit Sub

        Dim dtSubprocesos As DataTable = ConexionSISAN.ObtenerSubprocesos(ddlProceso.SelectedValue)

        With ddlSubProceso
            .Items.Clear()
            .DataSource = dtSubprocesos
            .DataTextField = "DESC_SUBPROCESO"
            .DataValueField = "ID_SUBPROCESO"
            .DataBind()
        End With

        ddlSubProceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        If HiddenFieldSubProceso.Value <> "" Then
            ddlSubProceso.SelectedValue = HiddenFieldSubProceso.Value
            HiddenFieldSubProceso.Value = ""
            ddlSubProcesoAlta_SelectedIndexChanged(sender, e)
        End If
        If HiddenFieldModificar.Value = "S" Then
            DivAltaIrreg.Visible = False
            DivModificaIrregVisita.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Loader", "jsRemoveWindowLoad()", True)
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Loader", "jsRemoveWindowLoad()", True)

    End Sub
    Protected Sub ddlSubProcesoAlta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubProceso.SelectedIndexChanged
        Dim iTEM As ListItem


        If Not (ddlSubProceso.SelectedValue <> "" AndAlso ddlSubProceso.SelectedValue <> "-1") Then Exit Sub

        Dim dtConducta As DataTable = ConexionSISAN.ObtenerConducta(ddlProceso.SelectedValue, ddlSubProceso.SelectedValue)

        With ddlConducta
            .Items.Clear()
            dtConducta = ConexionSISAN.ObtenerConducta(ddlProceso.SelectedValue, ddlSubProceso.SelectedValue)
            For i = 0 To dtConducta.Rows.Count - 1
                iTEM = New ListItem(Left(dtConducta.Rows(i).Item("DESC_CONDUCTA").ToString, 115), dtConducta.Rows(i).Item("ID_CONDUCTA").ToString)
                iTEM.Attributes.Add("Title", dtConducta.Rows(i).Item("DESC_CONDUCTA").ToString)
                .Items.Add(iTEM)
            Next
            .Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        End With

      'ddlConducta.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
      If HiddenFieldConducta.Value <> "" Then
            ddlConducta.SelectedValue = HiddenFieldConducta.Value
            HiddenFieldConducta.Value = ""
            ddlConducta_SelectedIndexChanged(sender, e)
        End If
    End Sub
    Protected Sub ddlConducta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlConducta.SelectedIndexChanged
        If Not (ddlConducta.SelectedValue <> "" AndAlso ddlConducta.SelectedValue <> "-1") Then Exit Sub

        Dim dt As New DataTable
        Dim i As Integer = 0
        Dim iTEM As ListItem

        With ddlIrregularidad
            .Items.Clear()
            dt = ConexionSISAN.ObtenerIrregularidades(ddlProceso.SelectedValue, ddlSubProceso.SelectedValue, ddlConducta.SelectedValue)
            For i = 0 To dt.Rows.Count - 1
                iTEM = New ListItem(Left(dt.Rows(i).Item("DESC_IRREGULARIDAD").ToString, 115), dt.Rows(i).Item("ID_IRREGULARIDAD").ToString)
                iTEM.Attributes.Add("Title", dt.Rows(i).Item("DESC_IRREGULARIDAD").ToString)
                .Items.Add(iTEM)
            Next
            .Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        End With

        If HiddenFieldIrregularidad.Value <> "" Then
            ddlIrregularidad.SelectedValue = HiddenFieldIrregularidad.Value
            HiddenFieldIrregularidad.Value = ""
        End If

    End Sub
    Public Function GuardarIrregularidad() As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        With lstCampos
            .Add("N_ID_FOLIO")
            .Add("F_FECH_IRREGULARIDAD")
            .Add("I_ID_PROCESO")
            '.Add("PROCESO")
            .Add("I_ID_SUBPROCESO")
            '.Add("SUBPROCESO")
            .Add("I_ID_CONDUCTA")
            '.Add("CONDUCTA")
            .Add("I_ID_IRREGULARIDADES")
            .Add("T_DSC_COMENTARIO")
        End With

        'HiddenFieldFolioVisita.Value
        '.Add(Folio)
        With lstValores
            .Add(HiddenFieldFolioVisita.Value)
            .Add(Date.ParseExact(txtFecIrregularidad.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd"))
            .Add(ddlProceso.SelectedValue)
            '.Add(ddlProceso.SelectedItem)
            .Add(ddlSubProceso.SelectedValue)
            '.Add(ddlSubProceso.SelectedItem)
            .Add(ddlConducta.SelectedValue)
            '.Add(ddlConducta.SelectedItem)
            .Add(ddlIrregularidad.SelectedValue)
            .Add(TxtComentarios.Text)
        End With
        Entities.IrregularidadVisita.Guardar(lstCampos, lstValores)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Loader", "jsRemoveWindowLoad()", True)
        Return True
    End Function
    Protected Sub btnAgregarIrregularidad_Click(sender As Object, e As EventArgs) Handles btnAgregarIrregularidad.Click
        If Valida_Datos() Then
            Call GuardarIrregularidad()
            LblError.Visible = False
            LblIrregularidades.Visible = True
            Call Limpia_Datos()
            Response.Redirect("../Procesos/DetalleVisita_V17.aspx")
        Else
            LblError.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Loader", "jsRemoveWindowLoad()", True)
        End If
    End Sub
    Private Sub Limpia_Datos()
        txtFecIrregularidad.Text = ""
        ddlConducta.SelectedIndex = -1
        ddlSubProceso.SelectedIndex = -1
        ddlProceso.SelectedIndex = -1
        ddlIrregularidad.SelectedIndex = -1
        TxtComentarios.Text = ""
        HiddenFieldIDIrregularidad.Value = ""
        HiddenFieldSubProceso.Value = ""
        HiddenFieldConducta.Value = ""
        HiddenFieldIrregularidad.Value = ""
    End Sub
    Private Function Valida_Datos() As Boolean
        Dim blnSinError As Boolean = True
        If txtFecIrregularidad.Text = "" Then
            blnSinError = False
        ElseIf ddlProceso.SelectedValue = -1 Then
            blnSinError = False
        ElseIf ddlSubProceso.SelectedValue = -1 Then
            blnSinError = False
        ElseIf ddlConducta.SelectedValue = -1 Then
            blnSinError = False
        ElseIf ddlIrregularidad.SelectedValue = -1 Then
            blnSinError = False
        ElseIf TxtComentarios.Text = "" Then
            blnSinError = False
        End If
        Return blnSinError
    End Function
    Private Sub CargarIrregularidades(FolioVisita As Integer)
        gvReqInformacVisita.DataSource = Entities.IrregularidadVisita.ObtenerTodas(FolioVisita)
        gvReqInformacVisita.DataBind()

        If gvReqInformacVisita.Rows.Count = 0 Then
            gvReqInformacVisita.Visible = True
            gvReqInformacVisita.Visible = False
        Else
            gvReqInformacVisita.Visible = False
            gvReqInformacVisita.Visible = True
        End If

        If puObjUsuario.IdArea = 37 Then
            gvReqInformacVisita.Columns(8).Visible = False 'columna con boton EDITAR
            gvReqInformacVisita.Columns(9).Visible = False 'columna con boton ELIMINAR
        End If
    End Sub
    Protected Sub btnModificarIrregularidad_Click(sender As Object, e As EventArgs) Handles btnModificarIrregularidad.Click
        If Valida_Datos() Then
            Call ModificandoIrregularidad()
            LblError.Visible = False
            LblIrregularidades.Visible = True
            Call Limpia_Datos()
            Response.Redirect("../Procesos/DetalleVisita_V17.aspx")
        Else
            LblError.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Loader", "jsRemoveWindowLoad()", True)
        End If
        'DivAltaIrreg.Visible = True
        'DivModificaIrregVisita.Visible = False
    End Sub
    Protected Sub btnCancelIrreg_Click(sender As Object, e As EventArgs) Handles btnCancelIrreg.Click
        btnAgregarIrregularidad.Visible = True
        'btnModificarIrregularidad.Visible = False
        'btnCancelIrreg.Visible = False
        '        pnlBotonesAlta.Visible = True
        '       pnlBotonesModificar.Visible = False
        Call Limpia_Datos()
        DivAltaIrreg.Visible = True
        DivModificaIrregVisita.Visible = False
    End Sub
    Public Function ModificandoIrregularidad() As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstCondicion As New List(Of String)
        Dim lstValorCondicion As New List(Of Object)

        With lstCampos
            .Add("N_ID_FOLIO")
            .Add("F_FECH_IRREGULARIDAD")
            .Add("I_ID_PROCESO")
            '.Add("PROCESO")
            .Add("I_ID_SUBPROCESO")
            '.Add("SUBPROCESO")
            .Add("I_ID_CONDUCTA")
            '.Add("CONDUCTA")
            .Add("I_ID_IRREGULARIDADES")
            .Add("T_DSC_COMENTARIO")
        End With

        With lstValores
            .Add(HiddenFieldFolioVisita.Value)
            .Add(Date.ParseExact(txtFecIrregularidad.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd"))
            .Add(ddlProceso.SelectedValue)
            '.Add(ddlProceso.SelectedItem)
            .Add(ddlSubProceso.SelectedValue)
            '.Add(ddlSubProceso.SelectedItem)
            .Add(ddlConducta.SelectedValue)
            '.Add(ddlConducta.SelectedItem)
            .Add(ddlIrregularidad.SelectedValue)
            .Add(TxtComentarios.Text)
        End With

        With lstCondicion
            .Add("I_ID_IRREGULARIDAD")
        End With

        With lstValorCondicion
            .Add(HiddenFieldIDIrregularidad.Value)
        End With
        Entities.IrregularidadVisita.Actualizar(lstCampos, lstValores, lstCondicion, lstValorCondicion)

        Return True
    End Function
    Private Function EsAreaOperativa(idAreaActual As Integer) As Boolean
        If (From objA As Entities.Area In Constantes.GetAreasOperativas() Where objA.Identificador = idAreaActual Select objA).Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Sub ModificaIds(idVisita As Integer)

        'If Not HiddenFieldIDIrregularidad.ID.Contains("_" & idVisita.ToString()) Then HiddenFieldIDIrregularidad.ID = HiddenFieldIDIrregularidad.ID & "_" & idVisita.ToString()
        If Not ddlProceso.CssClass.Contains("_" & idVisita.ToString()) Then ddlProceso.CssClass = ddlProceso.ID & "_" & idVisita.ToString()
        If Not ddlSubProceso.CssClass.Contains("_" & idVisita.ToString()) Then ddlSubProceso.CssClass = ddlSubProceso.ID & "_" & idVisita.ToString()
        If Not ddlConducta.CssClass.Contains("_" & idVisita.ToString()) Then ddlConducta.CssClass = ddlConducta.ID & "_" & idVisita.ToString()
        If Not ddlIrregularidad.CssClass.Contains("_" & idVisita.ToString()) Then ddlIrregularidad.CssClass = ddlIrregularidad.ID & "_" & idVisita.ToString()

        If Not txtFecIrregularidad.CssClass.Contains("_" & idVisita.ToString()) Then txtFecIrregularidad.CssClass = txtFecIrregularidad.ID & "_" & idVisita.ToString()
        If Not TxtComentarios.CssClass.Contains("_" & idVisita.ToString()) Then TxtComentarios.CssClass = TxtComentarios.ID & "_" & idVisita.ToString()
        'If Not TxtComentarios.Attributes.Equals(TxtComentarios.ID & "_" & idVisita.ToString()) Then TxtComentarios.Attributes.Add("class", TxtComentarios.ID & "_" & idVisita.ToString())

    End Sub

    Protected Sub btnModificarIrreg_Click(sender As Object, e As ImageClickEventArgs)
        Dim stringIdIrregularidad As String = HiddenFieldIDIrregularidad.Value

        Dim dt As New DataTable
        Dim query As String = ""
        Dim conexion As New Conexion.SQLServer()
        Dim cIrreg As New Entities.IrregularidadVisita
        Dim strCadena As String = ""

        query = "Select * FROM [dbo].[BDS_R_VI_IRREGULARIDAD] WHERE I_ID_IRREGULARIDAD = " + stringIdIrregularidad
        dt = conexion.ConsultarDT(query)

        With dt.Rows(0)
            strCadena = .Item("F_FECH_IRREGULARIDAD").ToString.Substring(0, 10) & "|"
            strCadena = strCadena & .Item("I_ID_PROCESO").ToString & "|"
            strCadena = strCadena & .Item("I_ID_SUBPROCESO").ToString & "|"
            strCadena = strCadena & .Item("I_ID_CONDUCTA").ToString & "|"
            strCadena = strCadena & .Item("I_ID_IRREGULARIDADES").ToString & "|"
            strCadena = strCadena & .Item("T_DSC_COMENTARIO").ToString & "|"
            strCadena = strCadena & .Item("I_ID_IRREGULARIDAD").ToString & "|"
        End With


    End Sub
End Class