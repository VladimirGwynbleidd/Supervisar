Public Class AltaIrregularidadesOPI
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("I_ID_OPI")
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



    <System.ComponentModel.Browsable(False)>
    Public Property BotonesVisibles As Boolean
        Get
            Return pnlBotones.Visible
        End Get
        Set(value As Boolean)
            pnlBotones.Visible = value
        End Set
    End Property

    Public Sub Inicializar()
        'Dim IdArea As Integer = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        ddlProceso.DataTextField = "DESC_PROCESO"
        ddlProceso.DataValueField = "ID_PROCESO"
        ddlProceso.DataSource = ConexionSISAN.ObtenerProcesos()
        ddlProceso.DataBind()
        ddlProceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        LblError.Visible = False
        LblIrregularidades.Visible = False
        CargarIrregularidades()
        DivModificaIrreg.Visible = False
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
            DivModificaIrreg.Visible = True
        End If

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

        ddlConducta.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
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

        With lstValores
            .Add(Folio)
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
        Entities.IrregularidadOPI.Guardar(lstCampos, lstValores)

        Return True
    End Function

    Protected Sub btnAgregarIrregularidad_Click(sender As Object, e As EventArgs) Handles btnAgregarIrregularidad.Click
        If Valida_Datos() Then
            Call GuardarIrregularidad()
            LblError.Visible = False
            LblIrregularidades.Visible = True
            Call Limpia_Datos()
            Response.Redirect("../OPI/DetalleOPI.aspx")
        Else
            LblError.Visible = True
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

    Private Sub CargarIrregularidades()
        gvReqInformac.DataSource = Entities.IrregularidadOPI.ObtenerTodas(Folio)
        gvReqInformac.DataBind()

        If gvReqInformac.Rows.Count = 0 Then
            gvReqInformac.Visible = True
            gvReqInformac.Visible = False
        Else
            gvReqInformac.Visible = False
            gvReqInformac.Visible = True
        End If
    End Sub

    Protected Sub btnModificarIrregularidad_Click(sender As Object, e As EventArgs) Handles btnModificarIrregularidad.Click
        If Valida_Datos() Then
            Call ModificandoIrregularidad()
            LblError.Visible = False
            LblIrregularidades.Visible = True
            Call Limpia_Datos()
            Response.Redirect("../OPI/DetalleOPI.aspx")
        Else
            LblError.Visible = True
        End If
        DivAltaIrreg.Visible = True
        DivModificaIrreg.Visible = False
    End Sub

    Protected Sub btnCancelIrreg_Click(sender As Object, e As EventArgs) Handles btnCancelIrreg.Click
        btnAgregarIrregularidad.Visible = True
        btnModificarIrregularidad.Visible = False
        btnCancelIrreg.Visible = False
        '        pnlBotonesAlta.Visible = True
        '       pnlBotonesModificar.Visible = False
        Call Limpia_Datos()
        DivAltaIrreg.Visible = True
        DivModificaIrreg.Visible = False
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
            .Add(Folio)
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
        Entities.IrregularidadOPI.Actualizar(lstCampos, lstValores, lstCondicion, lstValorCondicion)

        Return True
    End Function

End Class