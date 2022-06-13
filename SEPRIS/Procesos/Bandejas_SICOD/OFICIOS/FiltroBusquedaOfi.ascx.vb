Imports Clases
Public Class FiltroBusquedaOfi
    Inherits System.Web.UI.UserControl

    Private _defaultButtonId As String
    Public Property DefaultButtonId() As String
        Get
            Return _defaultButtonId
        End Get
        Set(ByVal Value As String)
            _defaultButtonId = Value
        End Set
    End Property
    Public Property ObjProps As PropsModal
        Get
            Dim oPropsModal As New PropsModal
            If IsNothing(Session("ObjProps")) Then
                Return oPropsModal
            Else
                Return CType(Session("ObjProps"), PropsModal)
            End If
        End Get
        Set(ByVal value As PropsModal)
            Session("ObjProps") = value
        End Set
    End Property
#Region "VARIABLES"
    Dim panelVisibles As String = String.Empty
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

        Dim Sesion As New Seguridad
        Dim Perfil As New Perfil
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1
        'verificaSesion()
        'verificaPerfil()
        Dim ListaPanelVisibles As String = "Fecha Recepción Doc."
        Dim arregloPanelesVisibles As String()
        Dim x As Integer

        If Not IsPostBack Then
            Session.Remove("ListaPanelVisibles")
            Session.Remove("OcultaFechaRecpDoc")
            Session("filtros") = ""
            Session("fec_docto") = ""
            Session("fec_docto1") = ""
            Session("fec_Rec") = ""
            Session("fec_Rec1") = ""
            Session("fec_Rec2") = ""
            Session("fec_Rec3") = ""
            Session("fec_Rec4") = ""
            Session("Tdocto") = ""
            Session("fArea") = ""
            Session("destinatario") = ""
            Session("rdbRecib") = ""
            Session("refere") = ""
            Session("refere1") = ""
            Session("folio") = ""
            Session("Oficio") = ""
            Session("Remitente") = ""
            Session("Asunto") = ""
            Session("Responsable") = ""
            Session("FechaRegistro") = ""
            Session("FechaLimite") = ""
            Session("AtendidaStatus") = ""
            Session("ProvieneSIE") = ""

            LlenaDDLs()

            TxtFecDocto.Text = Nothing
            TxtFecRecIni.Text = Date.Now.AddDays(-2).ToString("dd/MM/yyyy")
            TxtFecRecFin.Text = Date.Now.ToString("dd/MM/yyyy")

            If Request.QueryString.Item("filtros") <> "" Then
                For Each filtro As String In Request.QueryString.Item("filtros").Split(","c)
                    AgregaFiltro(filtro)
                Next
            End If

            If Not Session("ListaPanelVisibles") Is Nothing Then
                ListaPanelVisibles = Session("ListaPanelVisibles")
                arregloPanelesVisibles = ListaPanelVisibles.ToString().Split("@")
                For x = 0 To arregloPanelesVisibles.Length - 1
                    If Not Session("OcultaFechaRecpDoc") Is Nothing Then
                        listFiltrosControl.Items.FindByText("Fecha Recepción Doc.").Value = "0"
                        SetBtnEliminaFiltroVisible("Fecha Recepción Doc.", False)
                    End If

                    ActCriterio(arregloPanelesVisibles(x).ToString())
                    AgregaFiltro(arregloPanelesVisibles(x).ToString())

                    AgregaTextoControlesCarga(arregloPanelesVisibles(x).ToString())
                Next
            Else
                ActCriterio(ListaPanelVisibles)
                AgregaFiltro(ListaPanelVisibles)
            End If

            'rdbRec.Attributes.Add("OnClick", "SelectMeOnlyStatusRec('" + rdbRec.ClientID + "')")
            'rdbNRec.Attributes.Add("OnClick", "SelectMeOnlyStatusRec('" + rdbNRec.ClientID + "')")

            'rbtnSIE_SI.Attributes.Add("OnClick", "SelectMeOnlySIE('" + rbtnSIE_SI.ClientID + "')")
            'rbtnSIE_NO.Attributes.Add("OnClick", "SelectMeOnlySIE('" + rbtnSIE_NO.ClientID + "')")

            ValidacionSoloUnFiltro()
        End If

        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            If Not Session("RecuperarEstadoDeControlesDeFiltro") Is Nothing Then
                If Convert.ToBoolean(Session("RecuperarEstadoDeControlesDeFiltro")) = True Then
                    Session.Remove("RecuperarEstadoDeControlesDeFiltro")

                    pnlFechaDeDocto.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlFechaDeDocto.Visible", pnlFechaDeDocto.Visible))
                    TxtFecDocto.Text = Convert.ToString(RecuperaDato("Filtro.TxtFecDocto.Text", TxtFecDocto.Text))

                    pnlRangoDeFechas.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlRangoDeFechas.Visible", pnlRangoDeFechas.Visible))
                    TxtFecRecIni.Text = Convert.ToString(RecuperaDato("Filtro.TxtFechaIni.Text", TxtFecRecIni.Text))
                    TxtFecRecFin.Text = Convert.ToString(RecuperaDato("Filtro.TxtFechaFin.Text", TxtFecRecFin.Text))

                    'pnlEstatus.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlEstatus.Visible", pnlEstatus.Visible))
                    'rdbRec.Text = Convert.ToString(RecuperaDato("Filtro.rdbRec.Text", rdbRec.Text))
                    'rdbNRec.Text = Convert.ToString(RecuperaDato("Filtro.rdbNRec.Text", rdbNRec.Text))

                    pnlTipoDeDocto.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlTipoDeDocto.Visible", pnlTipoDeDocto.Visible))
                    If Not Session("Filtro.ddlTipoDocto.Items") Is Nothing And Not Session("Filtro.ddlTipoDocto.SelectedIndex") Is Nothing Then
                        ddlTipoDocto.Items.Clear()
                        For Each item As ListItem In CType(Session("Filtro.ddlTipoDocto.Items"), ListItemCollection)
                            ddlTipoDocto.Items.Add(item)
                        Next
                        Session.Remove("Filtro.ddlTipoDocto.Items")
                        ddlTipoDocto.SelectedIndex = Convert.ToInt32(Session("Filtro.ddlTipoDocto.SelectedIndex"))
                        Session.Remove("Filtro.ddlTipoDocto.SelectedIndex")
                    End If

                    pnlArea.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlArea.Visible", pnlArea.Visible))
                    If Not Session("Filtro.ddlArea.Items") Is Nothing And Not Session("Filtro.ddlArea.SelectedIndex") Is Nothing Then
                        ddlArea.Items.Clear()
                        For Each item As ListItem In CType(Session("Filtro.ddlArea.Items"), ListItemCollection)
                            ddlArea.Items.Add(item)
                        Next
                        Session.Remove("Filtro.ddlArea.Items")
                        ddlArea.SelectedIndex = Convert.ToInt32(Session("Filtro.ddlArea.SelectedIndex"))
                        Session.Remove("Filtro.ddlArea.SelectedIndex")
                    End If

                    'pnlReferencia.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlReferencia.Visible", pnlReferencia.Visible))
                    'TxtRefere.Text = Convert.ToString(RecuperaDato("Filtro.TxtRefere.Text", TxtRefere.Text))

                    'pnlEstatus.Visible = Convert.ToBoolean(RecuperaDato("Filtro.pnlEstatus.Visible", pnlEstatus.Visible))
                    'rdbRec.Checked = Convert.ToBoolean(RecuperaDato("Filtro.rdbRec.Checked", rdbRec.Checked))
                    'rdbNRec.Checked = Convert.ToBoolean(RecuperaDato("Filtro.rdbNRec.Checked", rdbNRec.Checked))

                    If Not Session("Filtro.ddlAgregar.Items") Is Nothing And Not Session("Filtro.ddlAgregar.SelectedIndex") Is Nothing Then
                        ddlAgregar.Items.Clear()
                        For Each item As ListItem In CType(Session("Filtro.ddlAgregar.Items"), ListItemCollection)
                            ddlAgregar.Items.Add(item)
                        Next
                        Session.Remove("Filtro.ddlAgregar.Items")
                        ddlAgregar.SelectedIndex = Convert.ToInt32(Session("Filtro.ddlAgregar.SelectedIndex"))
                        Session.Remove("Filtro.ddlAgregar.SelectedIndex")
                    End If
                    If Not Session("Filtro.listFiltrosControl.Items") Is Nothing Then
                        listFiltrosControl.Items.Clear()
                        For Each item As ListItem In CType(Session("Filtro.listFiltrosControl.Items"), ListItemCollection)
                            listFiltrosControl.Items.Add(item)
                        Next
                        Session.Remove("Filtro.listFiltrosControl.Items")
                    End If
                End If
            End If
        End If
        'Guarda estado de controles de filtro

        Session("Filtro.pnlRangoDeFechas.Visible") = pnlRangoDeFechas.Visible
        Session("Filtro.TxtFecRecIni.Text") = TxtFecRecIni.Text
        Session("Filtro.TxtFecRecFin.Text") = TxtFecRecFin.Text

        Session("Filtro.pnlFechaDeDocto.Visible") = pnlFechaDeDocto.Visible
        Session("Filtro.TxtFecDocto.Text") = TxtFecDocto.Text

        Session("Filtro.pnlTipoDeDocto.Visible") = pnlTipoDeDocto.Visible
        Session("Filtro.ddlTipoDocto.Items") = ddlTipoDocto.Items
        Session("Filtro.ddlTipoDocto.SelectedIndex") = ddlTipoDocto.SelectedIndex

        Session("Filtro.pnlArea.Visible") = pnlArea.Visible
        Session("Filtro.ddlArea.SelectedIndex") = ddlArea.SelectedIndex

        Session("Filtro.pnlDestinatario.Visible") = pnlDestinatario.Visible
        Session("Filtro.ddlDestinatario.Items") = ddlDestinatario.Items
        Session("Filtro.ddlDestinatario.SelectedIndex") = ddlDestinatario.SelectedIndex

        'Session("Filtro.pnlEstatus.Visible") = pnlEstatus.Visible
        'Session("Filtro.rdbRec.Checked") = rdbRec.Checked
        'Session("Filtro.rdbNRec.Checked") = rdbNRec.Checked

        Session("Filtro.ddlAgregar.Items") = ddlAgregar.Items
        Session("Filtro.ddlAgregar.SelectedIndex") = ddlAgregar.SelectedIndex
        Session("Filtro.listFiltrosControl.Items") = listFiltrosControl.Items


        Session("Filtro.ddlStatus.Items") = ddlStatus.Items
        Session("Filtro.ddlStatus.SelectedIndex") = ddlStatus.SelectedIndex
        Session("Filtro.ddlStatus.SelectedValue") = ddlStatus.SelectedValue

        'Session("Filtro.pnlReferencia.Visible") = pnlReferencia.Visible
        'Session("Filtro.TxtFecRecIni.Text") = TxtRefere.Text

        'Session("Filtro.pnlProvieneSIE.Visible") = pnlProvieneSIE.Visible
        'Session("Filtro.rbtnSIE_SI.Checked") = rbtnSIE_SI.Checked
        'Session("Filtro.rbtnSIE_NO.Checked") = rbtnSIE_NO.Checked

        If Not ViewState("EliminaDestinatario") Is Nothing Then
            ddlAgregar.Items.Remove("Destinatario")
            BtnEliminaDestinatario.Visible = True
            ViewState("EliminaDestinatario") = Nothing
        End If


    End Sub

    Private Function RecuperaDato(ByVal clave As String, ByVal vDefault As Object) As Object
        Dim objeto As Object
        If Not Session(clave) Is Nothing Then
            objeto = Session(clave)
            Session.Remove(clave)
            Return objeto
        End If
        Return vDefault
    End Function

    Private Sub ddlAgregar_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlAgregar.SelectedIndexChanged
        If ddlAgregar.SelectedIndex > 0 Then
            AgregaFiltro(ddlAgregar.SelectedValue)
        End If
    End Sub

    Public Sub AgregaFiltro(ByVal nombreFiltro As String)
        Select Case nombreFiltro
            Case "Fecha Documento"
                TxtFecDocto.Text = Nothing
                pnlFechaDeDocto().Visible = True
                BtnEliminaFecDocto.Visible = True
            Case "Tipo de Documento"
                pnlTipoDeDocto().Visible = True
                BtnEliminaTipoDocto.Visible = True
            Case "Fecha Recepción Doc."
                TxtFecRecIni.Text = Date.Now.AddDays(-2).ToString("dd/MM/yyyy")
                TxtFecRecFin.Text = Date.Now.ToString("dd/MM/yyyy")
                pnlRangoDeFechas.Visible = True
                BtnEliminaRangoDeFechas.Visible = True
                Session("OcultaFechaRecpDoc") = Nothing
            Case "Area"
                pnlArea.Visible = True
                BtnEliminaArea.Visible = True
                listFiltrosControl.Items.Insert(5, "Destinatario")
            Case "Destinatario"
                pnlDestinatario.Visible = True
                BtnEliminaDestinatario.Visible = True
                If pnlArea.Visible = True Then
                    pnlDestinatario.Visible = True
                    If ddlArea.SelectedItem.Text.Trim() <> "-Seleccione una-" Then
                        ddlDestinatario.Enabled = True
                    Else
                        ddlDestinatario.Enabled = False
                    End If
                    ddlDestinatario.Items.Insert(0, "-Seleccione uno-")
                Else
                    ddlDestinatario.Enabled = False
                End If

                'Case "Recibido"
                '    rdbNRec.Checked = False
                '    rdbRec.Checked = False
                '    BtnEliminaEstatus.Visible = True
                '    pnlEstatus.Visible = True

                'Case "Referencia"
                '    BtnEliminaRefere.Visible = True
                '    pnlReferencia.Visible = True
                '    'by ; Se agregaron Filtro para la consulta
                'Case "Folio"
                '    txtFolio.Text = Nothing
                '    pnlFolio().Visible = True
                '    BtnEliminaFolio.Visible = True

            Case "Oficio"
                txtOficio.Text = Nothing
                pnlOficio().Visible = True
                BtnEliminaOficio.Visible = True

                'Case "Remitente"
                '    txtRemitente.Text = Nothing
                '    pnlRemitente().Visible = True
                '    BtnEliminaRemitente.Visible = True

            Case "Asunto"
                txtAsunto.Text = Nothing
                pnlAsunto().Visible = True
                BtnEliminaAsunto.Visible = True

            Case "Fecha de Registro"
                txtFechaRegistro.Text = Nothing
                pnlFechaRegistro().Visible = True
                BtnEliminaFechaRegistro.Visible = True

            Case "Fecha Limite"
                txtFechaLimite.Text = Nothing
                pnlFechaLimite().Visible = True
                BtnEliminaFechaLimite.Visible = True
            Case "Estatus"
                ddlStatus.SelectedIndex = 0
                pnlStatusAtendida().Visible = True
                BtnEliminaStatus.Visible = True

            Case Else
                Return
        End Select

        listFiltrosControl.Items.FindByText(nombreFiltro).Value = "1"
        LlenaDDLs(nombreFiltro)
        ActCriterio(nombreFiltro)



        ValidacionSoloUnFiltro()
    End Sub

    Private Sub ActCriterio(ByVal nombreFiltro)
        Dim cont As Integer
        Dim items As Integer = 0
        Try
            'Actualiza criterios
            ddlAgregar.Items.Clear()
            ddlAgregar.Items.Add("Agregar criterio")

            For Each item As ListItem In listFiltrosControl.Items
                If item.Value = "0" Then
                    ddlAgregar.Items.Add(item.Text)
                End If
            Next
            If pnlArea.Visible = True And nombreFiltro = "Destinatario" Then
                ddlAgregar.Items.Remove("Destinatario")
            End If
            If pnlArea.Visible And pnlDestinatario.Visible = False Then
                For i = 0 To ddlAgregar.Items.Count - 1
                    If ddlAgregar.Items(i).Text = "Destinatario" Then
                        items += 1
                    End If
                Next
                If items = 0 Then
                    cont = ddlAgregar.Items.Count
                    ddlAgregar.Items.Insert(cont, "Destinatario")
                    BtnEliminaDestinatario.CommandArgument = "Destinatario" 'listFiltrosControl.Items(5).Text
                End If

            End If

            SortDDL(ddlAgregar)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub EliminaFiltro(ByVal nombreFiltro As String)
        Dim cuenta0 As Integer = 0
        Dim cuenta1 As Integer = 0
        Dim ultimoFiltro As String
        For Each item As ListItem In listFiltrosControl.Items
            If item.Value = "0" Then 'Está en uso
                cuenta0 += 1
                'ultimoFiltro = item.Text
            Else 'If item.Value = "1" Then
                cuenta1 += 1
                ultimoFiltro = item.Text
            End If
        Next
        If cuenta1 = 1 Then
            listFiltrosControl.Items.FindByText(nombreFiltro).Value = "1"
            SetBtnEliminaFiltroVisible(nombreFiltro, True)
        Else

            Select Case nombreFiltro
                Case "Fecha Documento"
                    pnlFechaDeDocto().Visible = False
                    Session.Remove("TxtFecDocto")
                Case "Tipo de Documento"
                    pnlTipoDeDocto().Visible = False
                    Session.Remove("ddlTipoDocto")
                Case "Fecha Recepción Doc."
                    pnlRangoDeFechas.Visible = False
                    Session.Remove("OcultaFechaRecpDoc")
                    Session("OcultaFechaRecpDoc") = "SI"
                    Session.Remove("TxtFecRecIni")
                    Session.Remove("TxtFecRecFin")
                Case "Area"
                    pnlArea.Visible = False
                    Session.Remove("ddlArea")
                    If pnlDestinatario.Visible = False Then
                        listFiltrosControl.Items.FindByText(nombreFiltro).Value = "0"
                        SetBtnEliminaFiltroVisible(nombreFiltro, False)
                        listFiltrosControl.Items.RemoveAt(5)
                    Else
                        pnlArea.Visible = True
                    End If
                Case "Destinatario"
                    pnlDestinatario.Visible = False
                    Session.Remove("ddlDestinatario")
                    If pnlArea.Visible = False Then
                        ddlAgregar.Items.Remove("Destinatario")
                    End If
                    'Case "Recibido"
                    '    pnlEstatus.Visible = False
                    '    Session.Remove("rdbNRec")
                    '    Session.Remove("rdbRec")
                    'Case "Referencia"
                    '    pnlReferencia.Visible = False
                    '    Session.Remove("TxtRefere")

                    'by ; Se agregaron Filtro para la consulta
                    'Case "Folio"
                    '    pnlFolio().Visible = False
                    '    Session.Remove("txtFolio")
                Case "Oficio"
                    pnlOficio().Visible = False
                    Session.Remove("txtOficio")

                    'Case "Remitente"
                    '    pnlRemitente().Visible = False
                    '    Session.Remove("txtRemitente")
                Case "Asunto"
                    pnlAsunto().Visible = False
                    Session.Remove("txtAsunto")
                    'Case "Responsable"
                    '    pnlNombre().Visible = False
                    '    Session.Remove("txtNombre")
                Case "Fecha de Registro"
                    pnlFechaRegistro().Visible = False
                    Session.Remove("txtFechaRegistro")
                Case "Fecha Limite"
                    pnlFechaLimite().Visible = False
                    Session.Remove("txtFechaLimite")
                Case "Estatus"
                    pnlStatusAtendida().Visible = False
                    Session.Remove("ddlStatus")
                    'Case "Proviene SIE"
                    '    pnlProvieneSIE.Visible = False
                    '    Session.Remove("rbtnSIE_SI")
                    '    Session.Remove("rbtnSIE_NO")
                Case Else
                    Return
            End Select
            If nombreFiltro <> "Area" Then
                listFiltrosControl.Items.FindByText(nombreFiltro).Value = "0"
                SetBtnEliminaFiltroVisible(nombreFiltro, False)
            End If

        End If


        'listFiltrosControl.Items.FindByText(nombreFiltro).Value = "0"
        ActCriterio(nombreFiltro)
        LlenaDDLs()
        ValidacionSoloUnFiltro()
    End Sub

    Private Sub ValidacionSoloUnFiltro()
        'Valida que sí sólo queda un filtro, no se pueda eliminar
        Dim cuenta As Integer = 0
        Dim ultimoFiltro As String = ""
        For Each item As ListItem In listFiltrosControl.Items

            If item.Value = "1" Then
                ultimoFiltro = item.Text
                OcultaBotonElimina(ultimoFiltro, True)
                cuenta = cuenta + 1
            Else

                OcultaBotonElimina(item.Text, False)
            End If
        Next
        If cuenta = 1 Then
            OcultaBotonElimina(ultimoFiltro, False)
        End If

    End Sub

    Private Function SetBtnEliminaFiltroVisible(ByVal NombreFiltro As String, ByVal Visible As Boolean) As Boolean
        Select Case NombreFiltro
            Case "Fecha Documento"
                pnlFechaDeDocto().Visible = Visible
                If Visible = False Then
                    Session.Remove("TxtFecDocto")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Tipo de Documento"
                pnlTipoDeDocto().Visible = Visible
                If Visible = False Then
                    Session.Remove("ddlTipoDocto")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Fecha Recepción Doc."
                pnlRangoDeFechas.Visible = Visible
                If Visible = False Then
                    Session.Remove("TxtFecRecIni")
                    Session.Remove("TxtFecRecFin")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Area"
                pnlArea.Visible = Visible
                If Visible = False Then
                    Session.Remove("ddlArea")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Destinatario"
                pnlDestinatario.Visible = Visible
                If Visible = False Then
                    Session.Remove("ddlDestinatario")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Oficio"
                pnlOficio().Visible = Visible
                If Visible = False Then
                    Session.Remove("txtOficio")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Asunto"
                pnlAsunto().Visible = Visible
                If Visible = False Then
                    Session.Remove("txtAsunto")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Fecha de Registro"
                pnlFechaRegistro().Visible = Visible
                If Visible = False Then
                    Session.Remove("txtFechaRegistro")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Fecha Limite"
                pnlFechaLimite().Visible = Visible
                If Visible = False Then
                    Session.Remove("txtFechaLimite")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case "Estatus"
                pnlStatusAtendida().Visible = Visible
                If Visible = False Then
                    Session.Remove("ddlStatus")
                    RecortaCadenaPanelOculta(NombreFiltro)
                End If
            Case Else
                Return False
        End Select
        Return True
    End Function

    Private Function OcultaBotonElimina(ByVal NombreFiltro As String, ByVal Visible As Boolean) As Boolean
        Select Case NombreFiltro
            Case "Tipo de Documento"
                BtnEliminaTipoDocto.Visible = Visible
            Case "Fecha Recepción Doc."
                BtnEliminaRangoDeFechas.Visible = Visible
            Case "Area"
                BtnEliminaArea.Visible = Visible
            Case "Destinatario"
                BtnEliminaDestinatario.Visible = Visible
            Case "Oficio"
                BtnEliminaOficio.Visible = Visible
            Case "Asunto"
                BtnEliminaAsunto.Visible = Visible
            Case "Fecha de Registro"
                BtnEliminaFechaRegistro.Visible = Visible
            Case "Fecha Limite"
                BtnEliminaFechaLimite.Visible = Visible
            Case "Estatus"
                BtnEliminaStatus.Visible = Visible
            Case Else
                Return False
        End Select
        Return True
    End Function

    Public Sub LlenaDDLs(Optional ByVal actualizaNombreFiltro As String = "")

        BtnEliminaRangoDeFechas.CommandArgument = "Fecha Recepción Doc."
        ' BtnEliminaFolio.CommandArgument = "Folio"
        BtnEliminaFecDocto.CommandArgument = "Fecha Documento"
        BtnEliminaTipoDocto.CommandArgument = "Tipo de Documento"
        BtnEliminaArea.CommandArgument = "Area"
        BtnEliminaDestinatario.CommandArgument = "Destinatario"
        ' BtnEliminaEstatus.CommandArgument = "Recibido"

        BtnEliminaOficio.CommandArgument = "Oficio"

        BtnEliminaAsunto.CommandArgument = "Asunto"

        BtnEliminaFechaRegistro.CommandArgument = "Fecha de Registro"
        BtnEliminaFechaLimite.CommandArgument = "Fecha Limite"
        BtnEliminaStatus.CommandArgument = "Estatus"



        Select Case actualizaNombreFiltro
            Case "Tipo de Documento"
                'Llena DDL de Tipo de Documento

                If pnlTipoDeDocto.Visible Then
                    Dim dt As New DataTable
                    Dim Con = New Clases.Conexion()
                    Try
                        Con.ConsultaAdapter("SELECT ID_TIPO_DOCUMENTO,T_TIPO_DOCUMENTO FROM " & Clases.Conexion.Owner & "BDA_TIPO_DOCUMENTO WHERE VIG_FLAG= 1 " & IIf(Me.ObjProps.tipo = "", "", " AND ID_TIPO_DOCUMENTO=" & Me.ObjProps.tipo)).Fill(dt)
                        ddlTipoDocto.DataSource = dt
                        ddlTipoDocto.DataTextField = "T_TIPO_DOCUMENTO"
                        ddlTipoDocto.DataValueField = "ID_TIPO_DOCUMENTO"
                        ddlTipoDocto.DataBind()
                        ddlTipoDocto.Items.Insert(0, "-Seleccione uno-")

                    Catch ex As Exception
                    Finally
                        If Not Con Is Nothing Then
                            Con.Cerrar()

                        End If
                    End Try
                End If


            Case "Destinatario"
                'Llena DDL de Tipos de documento
                If pnlDestinatario.Visible Then
                    Try
                        If ddlArea.SelectedItem.Text.Trim() <> "-Seleccione una-" Then
                            buscaUs()
                        End If
                    Catch ex As Exception
                    Finally

                    End Try
                End If

            Case "Area"
                'Llena ddl de Areas   
                If pnlArea.Visible Then
                    Dim dt As New DataTable
                    Dim Con = New Clases.Conexion()


                    Try
                        Dim usuario As New Entities.Usuario()
                        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
                        'Codigo nuevo NHM:
                        If (Me.ObjProps.idArea = "" Or usuario.IdentificadorPerfilActual = 1) And usuario.IdArea = 34 Then
                            Con.ConsultaAdapter("SELECT ID_UNIDAD_ADM, DSC_UNIDAD_ADM FROM " & Clases.Conexion.Owner & "BDA_C_UNIDAD_ADM WHERE VIG_FLAG= 1 AND ID_T_UNIDAD_ADM = 2 ORDER BY I_CODIGO_AREA ASC").Fill(dt)
                        Else
                            Dim dscarea = Entities.Area.ObtenerDescripcionArea(Me.ObjProps.idArea)
                            'Dim x As String = "SELECT ID_UNIDAD_ADM, DSC_UNIDAD_ADM FROM " & Clases.Conexion.Owner & "BDA_C_UNIDAD_ADM WHERE VIG_FLAG= 1 AND ID_T_UNIDAD_ADM = 2 " & "AND [DSC_UNIDAD_ADM] = '" & dscarea & "' COLLATE Latin1_general_CI_AI ORDER BY I_CODIGO_AREA ASC"
                            'Con.ConsultaAdapter("SELECT ID_UNIDAD_ADM, DSC_UNIDAD_ADM FROM " & Clases.Conexion.Owner & "BDA_C_UNIDAD_ADM WHERE VIG_FLAG= 1 AND ID_T_UNIDAD_ADM = 2 " & "AND [DSC_UNIDAD_ADM] ='" & dscarea & "' COLLATE Latin1_general_CI_AI ORDER BY I_CODIGO_AREA ASC").Fill(dt)
                            Dim x As String = "DECLARE @codarea AS NVARCHAR(3) SET @codarea = (SELECT LEFT(I_CODIGO_AREA,LEN(I_CODIGO_AREA)-2) FROM " & Clases.Conexion.Owner & "BDA_C_UNIDAD_ADM WHERE VIG_FLAG= 1 AND ID_T_UNIDAD_ADM = 2 AND DSC_UNIDAD_ADM = '" & dscarea & "' COLLATE Latin1_general_CI_AI) " &
                            "SELECT ID_UNIDAD_ADM, DSC_UNIDAD_ADM FROM dbo.BDA_C_UNIDAD_ADM WHERE VIG_FLAG= 1 AND ID_T_UNIDAD_ADM = 2 AND I_CODIGO_AREA LIKE ''+@codarea+'%' COLLATE Latin1_general_CI_AI ORDER BY I_CODIGO_AREA ASC"
                            Con.ConsultaAdapter(x).Fill(dt)
                        End If
                        'NHM fin

                        ddlArea.DataSource = dt
                        ddlArea.DataTextField = "DSC_UNIDAD_ADM"
                        ddlArea.DataValueField = "ID_UNIDAD_ADM"
                        ddlArea.DataBind()
                        ddlArea.Items.Insert(0, "-Seleccione una-")

                    Catch ex As Exception
                    Finally
                        If Not Con Is Nothing Then
                            Con.Cerrar()
                        End If
                    End Try
                End If





        End Select

    End Sub
    Private Sub BtnEliminaFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEliminaRangoDeFechas.Click, BtnEliminaFecDocto.Click, BtnEliminaTipoDocto.Click, BtnEliminaArea.Click, BtnEliminaDestinatario.Click, BtnEliminaOficio.Click, BtnEliminaAsunto.Click, BtnEliminaStatus.Click, BtnEliminaFechaRegistro.Click, BtnEliminaFechaLimite.Click
        EliminaFiltro(CType(sender, Button).CommandArgument)
    End Sub

    'Regresa el query del where sin poner el where y empezando con " AND", siempre regresa el USUARIO
    'Si tiene error regresa -1, seguido del error. Ejmplo: "-1,Fechas incorrectas"
    Public Function GetWhereQuery(Optional idArea As String = "", Optional tipo As String = "") As String
        Dim query As String = ""

        If Not Session("ListaPanelVisibles") Is Nothing Then
            panelVisibles = Session("ListaPanelVisibles").ToString()
        End If


        If pnlFechaDeDocto.Visible Then
            Try
                If TxtFecDocto.Text <> "" Then

                    If Not panelVisibles.ToString().Contains("Fecha Documento") Then
                        panelVisibles = panelVisibles & "Fecha Documento" & "@"
                        Session("TxtFecDocto") = TxtFecDocto.Text
                    End If

                    Dim fechaDoc As String = Date.ParseExact(TxtFecDocto.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                    query &= " AND F_FECHA_OFICIO = '" & fechaDoc & "' "
                Else
                    Session("fec_docto") = "fecdoc"
                    Session("filtros") = "fecdoc"
                End If

            Catch ex As Exception

                Session("fec_docto1") = "fecdoc1"
                Session("filtros") = "fecdoc1"
            End Try
        End If

        If pnlRangoDeFechas.Visible Then
            Dim ffin As String = ""
            Session("fec_Rec") = ""
            Session("fec_Rec1") = ""
            Session("fec_Rec2") = ""
            Session("fec_Rec3") = ""
            Session("fec_Rec4") = ""
            Try

                If TxtFecRecIni.Text <> "" And TxtFecRecFin.Text <> "" Then
                    Dim fechasErroneas As Boolean = False
                    Dim d1 As Date = Nothing

                    Try
                        d1 = Date.ParseExact(TxtFecRecIni.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

                    Catch ex As Exception
                        Session("fec_Rec3") = "fecRec3"
                        Session("filtros") = "fecRec3"
                        fechasErroneas = True

                    End Try
                    Dim d2 As Date = Nothing


                    Try
                        d2 = Date.ParseExact(TxtFecRecFin.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    Catch ex As Exception
                        Session("fec_Rec4") = "fecRec4"
                        Session("filtros") = "fecRec4"
                        fechasErroneas = True
                    End Try

                    If (Not fechasErroneas And d2 < d1) Then

                        Session("fec_Rec2") = "fecRec2"
                        Session("filtros") = "fecRec2"
                    Else
                        Dim fechaD1 As String = Date.ParseExact(TxtFecRecIni.Text.Trim(), "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                        Dim fechaD2 As String = Date.ParseExact(TxtFecRecFin.Text.Trim(), "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd")
                        If (Not fechasErroneas And d2 >= d1 And d1 < Date.Now) Then

                            If Not panelVisibles.ToString().Contains("Fecha Recepción Doc.") Then
                                panelVisibles = panelVisibles & "Fecha Recepción Doc." & "@"
                                Session("TxtFecRecIni") = TxtFecRecIni.Text
                                Session("TxtFecRecFin") = TxtFecRecFin.Text
                            End If

                            query &= " AND F_FECHA_OFICIO BETWEEN " & "'" & fechaD1 & "'"


                            query &= " AND " & "'" & fechaD2 & "'"

                        ElseIf (Not fechasErroneas And d2 >= d1 And d1 > Date.Now) Then
                            If Not panelVisibles.ToString().Contains("Fecha Recepción Doc.") Then
                                panelVisibles = panelVisibles & "Fecha Recepción Doc." & "@"
                                Session("TxtFecRecIni") = TxtFecRecIni.Text
                                Session("TxtFecRecFin") = TxtFecRecFin.Text
                            End If

                            query &= " AND F_FECHA_OFICIO BETWEEN " & "'" & fechaD1 & "'"

                            query &= " AND " & "'" & fechaD2 & "'"

                        End If
                    End If
                Else
                    If TxtFecRecIni.Text = "" Then
                        Session("fec_Rec") = "fecRec"
                        Session("filtros") = "fecRec"
                    End If
                    If TxtFecRecFin.Text = "" Then
                        Session("fec_Rec1") = "fecRec1"
                        Session("filtros") = "fecRec1"
                    End If


                End If

            Catch ex As Exception

            End Try
        End If

        Try
            If pnlTipoDeDocto.Visible Then
                If ddlTipoDocto.SelectedIndex > 0 Then
                    If Not panelVisibles.ToString().Contains("Tipo de Documento") Then
                        panelVisibles = panelVisibles & "Tipo de Documento" & "@"
                        Session("ddlTipoDocto") = ddlTipoDocto.Text
                    End If
                    query &= " AND O.ID_TIPO_DOCUMENTO = '" & ddlTipoDocto.SelectedValue & "'"
                Else
                    Session("Tdocto") = "docto"
                    Session("filtros") = "docto"
                End If
            Else
                query &= " AND O.ID_TIPO_DOCUMENTO = '" & Me.ObjProps.tipo & "'"
            End If
            If pnlArea.Visible Then
                If ddlArea.SelectedIndex > 0 Then

                    If Not panelVisibles.ToString().Contains("Area") Then
                        panelVisibles = panelVisibles & "Area" & "@"
                        Session("ddlArea") = ddlArea.Text
                    End If

                    query &= " AND ID_UNIDAD_ADM = " & ddlArea.SelectedValue & ""

                Else
                    Session("fArea") = "area"
                    Session("filtros") = "area"
                End If
            End If

            If pnlDestinatario.Visible Then
                If ddlDestinatario.SelectedIndex > 0 Then

                    If Not panelVisibles.ToString().Contains("Destinatario") Then
                        panelVisibles = panelVisibles & "Destinatario" & "@"
                        Session("ddlDestinatario") = ddlDestinatario.Text
                    End If
                    query &= " AND DESTINATARIO = '" & ddlDestinatario.SelectedValue.Trim() & "'"


                    Session("dest") = ddlDestinatario.SelectedValue.Trim()
                Else
                    Session("destinatario") = "dest"
                    Session("filtros") = "dest"
                End If
            Else
                Session("dest") = Session("Usuario")
            End If

            If pnlOficio.Visible Then
                Try
                    If txtOficio.Text <> "" Then
                        If Not panelVisibles.ToString().Contains("Oficio") Then
                            panelVisibles = panelVisibles & "Oficio" & "@"
                            Session("txtOficio") = txtOficio.Text
                        End If
                        query &= " AND T_OFICIO_NUMERO LIKE '%" & txtOficio.Text.ToString().Trim() & "%'"
                    Else
                        Session("Oficio") = "Oficio"
                        Session("filtros") = "Oficio"
                    End If
                Catch ex As Exception
                    Session("Oficio") = "Oficio"
                    Session("filtros") = "Oficio"
                End Try
            End If

            If pnlAsunto.Visible Then
                Try
                    If txtAsunto.Text <> "" Then
                        If Not panelVisibles.ToString().Contains("Asunto") Then
                            panelVisibles = panelVisibles & "Asunto" & "@"
                            Session("txtAsunto") = txtAsunto.Text
                        End If
                        query &= " AND T_ASUNTO LIKE '%" & txtAsunto.Text.ToString().Trim() & "%'"
                    Else
                        Session("Asunto") = "Asunto"
                        Session("filtros") = "Asunto"
                    End If
                Catch ex As Exception
                    Session("Asunto") = "Asunto"
                    Session("filtros") = "Asunto"
                End Try
            End If


            If pnlFechaLimite.Visible Then
                Try
                    Convert.ToDateTime(txtFechaLimite.Text.Trim())
                    Dim fechaLimite As String = Date.ParseExact(txtFechaLimite.Text.Trim(), "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd")

                    If txtFechaLimite.Text <> "" Then


                        If Not panelVisibles.ToString().Contains("Fecha Limite") Then
                            panelVisibles = panelVisibles & "Fecha Limite" & "@"
                            Session("txtFechaLimite") = txtFechaLimite.Text
                        End If

                        query &= " AND F_FECHA_VENCIMIENTO = '" & fechaLimite & "'"

                    Else
                        Session("FechaLimite") = "FechaLimite"
                        Session("filtros") = "FechaLimite"
                    End If
                Catch ex As Exception
                    Session("FechaLimite") = "FechaLimite"
                    Session("filtros") = "FechaLimite"
                End Try
            End If

        Finally

        End Try

        Session("ListaPanelVisibles") = panelVisibles
        Return query

    End Function

    Public Function GetDestinatario() As String
        Dim consultaAD As New Directorio
        Dim liOAD As New List(Of OAD)
        Dim liUS As New List(Of US)
        Try
            If ddlDestinatario.SelectedValue = "" Then
                Return ""
            Else
                Dim user = New US()
                user.usuario = ddlDestinatario.SelectedValue
                liUS.Add(user)
                liOAD = consultaAD.ObtenerLista(liUS)
                Return liOAD.Item(0).nomusuario

            End If

        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub verificaSesion()
        Dim logout As Boolean = False
        Dim Sesion As Seguridad = Nothing
        Try
            Sesion = New Seguridad
            'Verifica la sesion de usuario
            Select Case Sesion.ContinuarSesionAD()
                Case -1
                    logout = True
                Case 0, 3
                    logout = True
            End Select
        Catch ex As Exception
            catch_cone(ex, "verificaSesion")
        Finally
            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub
    Private Sub verificaPerfil()
        Dim logout As Boolean = False
        Dim Perfil As Perfil = Nothing
        Try
            Perfil = New Perfil
            'Verifica que el usuario este autorizado para ver esta página
            If Not Perfil.Autorizado("BandejaEntrada.aspx") Then
                logout = True
            End If

        Catch ex As Exception
            catch_cone(ex, "verificaPerfil")
        Finally
            If Not Perfil Is Nothing Then
                Perfil.CerrarCon()
                Perfil = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub

    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
        Response.Redirect("~\PaginaErrorSICOD.aspx")
    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlArea.SelectedIndexChanged
        Try
            If ddlArea.SelectedItem.Text.Trim() = "-Seleccione una-" Then
                ddlDestinatario.Items.Clear()
                ddlDestinatario.Items.Insert(0, "-No Items-")
                ddlDestinatario.DataBind()
                BtnEliminaDestinatario.Visible = True
            Else
                If pnlDestinatario.Visible = True Then
                    ddlDestinatario.Enabled = True
                End If
                BtnEliminaDestinatario.Visible = True
                buscaUs()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub SortDDL(ByVal objDDL As DropDownList)

        Dim textList As ArrayList = New ArrayList()
        Dim valueList As ArrayList = New ArrayList()
        Dim x As Integer

        For Each listaIten As ListItem In objDDL.Items
            textList.Add(listaIten.Text)
        Next

        textList.Sort()

        For Each item As Object In objDDL.Items
            Dim valor As String = objDDL.Items.FindByText(item.ToString()).Value
            valueList.Add(valor)
        Next

        objDDL.Items.Clear()

        For x = 0 To textList.Count - 1
            ddlAgregar.Items.Add(textList(x).ToString())
        Next

    End Sub

    Protected Sub RecortaCadenaPanelOculta(ByVal nombrePanel As String)
        If Not Session("ListaPanelVisibles") Is Nothing Then
            Dim arregloPanelesVisibles As String()
            Dim ListaPanelVisibles As String = ""



            ListaPanelVisibles = Session("ListaPanelVisibles")
            arregloPanelesVisibles = ListaPanelVisibles.ToString().Split("@")
            Session.Remove("ListaPanelVisibles")
            For x = 0 To arregloPanelesVisibles.Length - 1
                If arregloPanelesVisibles(x).ToString() <> nombrePanel Then
                    panelVisibles = panelVisibles & arregloPanelesVisibles(x).ToString() & "@"
                End If
            Next

            Session("ListaPanelVisibles") = panelVisibles
        End If
    End Sub

    Sub buscaUs()

        Dim dt As New DataTable
        Dim Con = New Clases.Conexion()
        Try


            Con.ConsultaAdapter(" SELECT U.USUARIO, NOMBRE + ' ' + ISNULL(APELLIDOS, '') NOMBRE FROM " & Clases.Conexion.Owner & "BDS_USUARIO U JOIN " & Clases.Conexion.Owner & "BDA_R_USUARIO_UNIDAD_ADM A ON U.USUARIO = A.USUARIO WHERE U.VIG_FLAG = 1 AND A.VIG_FLAG = 1 AND A.ID_T_UNIDAD_ADM = 2 AND A.ID_UNIDAD_ADM = " & ddlArea.SelectedValue & " ORDER BY 2").Fill(dt)

            If dt.Rows.Count > 0 Then
                ddlDestinatario.DataSource = dt
                ddlDestinatario.DataTextField = "NOMBRE"
                ddlDestinatario.DataValueField = "USUARIO"
                ddlDestinatario.DataBind()
                ddlDestinatario.Items.Insert(0, "-Seleccione Uno-")
            Else
                ddlDestinatario.Items.Clear()
                ddlDestinatario.Items.Insert(0, "-No se encontraron destinatarios-")
                ddlDestinatario.DataBind()
            End If

        Catch ex As Exception


        Finally
            If Not Con Is Nothing Then
                Con.Cerrar()
            End If
        End Try


    End Sub
    Private Sub MsgBox(ByVal mensaje As String, ByVal pagina As System.Web.UI.Page)

        Dim scriptStr As String = "alert('" + mensaje.Replace("\\N", "\\n") + "');"

        System.Web.UI.ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "MsgBox", scriptStr, True)

    End Sub

    Private Sub AgregaTextoControlesCarga(ByVal nombreFiltro As String)

        Select Case nombreFiltro
            Case "Fecha Documento"
                TxtFecDocto.Text = IIf(Session("TxtFecDocto") Is Nothing, "", Session("TxtFecDocto"))
            Case "Tipo de Documento"
                ddlTipoDocto.Text = IIf(Session("ddlTipoDocto") Is Nothing, "-Seleccione uno-", Session("ddlTipoDocto"))
            Case "Fecha Recepción Doc."
                TxtFecRecIni.Text = IIf(Session("TxtFecRecIni") Is Nothing, "", Session("TxtFecRecIni"))
                TxtFecRecFin.Text = IIf(Session("TxtFecRecFin") Is Nothing, "", Session("TxtFecRecFin"))
            Case "Area"
                ddlArea.Text = IIf(Session("ddlArea") Is Nothing, "-Seleccione una-", Session("ddlArea"))
            Case "Destinatario"
                ddlDestinatario.Text = IIf(Session("ddlDestinatario") Is Nothing, "-Seleccione Uno-", Session("ddlDestinatario"))
                ViewState("EliminaDestinatario") = True
                'Case "Referencia"
                '    TxtRefere.Text = IIf(Session("TxtRefere") Is Nothing, "", Session("TxtRefere"))
                'Case "Recibido"
                '    If Not Session("rdbNRec") Is Nothing Then
                '        rdbNRec.Checked = Convert.ToBoolean(Session("rdbNRec").ToString())
                '    ElseIf Not Session("rdbRec") Is Nothing Then
                '        rdbRec.Checked = Convert.ToBoolean(Session("rdbRec").ToString())
                '    End If
                'Case "Folio"
                '    txtFolio.Text = IIf(Session("txtFolio") Is Nothing, "", Session("txtFolio"))
            Case "Oficio"
                txtOficio.Text = IIf(Session("txtOficio") Is Nothing, "", Session("txtOficio"))
                'Case "Remitente"
                '    txtRemitente.Text = IIf(Session("txtRemitente") Is Nothing, "", Session("txtRemitente"))
            Case "Asunto"
                txtAsunto.Text = IIf(Session("txtAsunto") Is Nothing, "", Session("txtAsunto"))
                'Case "Responsable"
                '    txtNombre.Text = IIf(Session("txtNombre") Is Nothing, "", Session("txtNombre"))
            Case "Fecha de Registro"
                txtFechaRegistro.Text = IIf(Session("txtFechaRegistro") Is Nothing, "", Session("txtFechaRegistro"))
            Case "Fecha Limite"
                txtFechaLimite.Text = IIf(Session("txtFechaLimite") Is Nothing, "", Session("txtFechaLimite"))
            Case "Estatus"
                ddlStatus.Text = IIf(Session("ddlStatus") Is Nothing, "-Seleccione Uno-", Session("ddlStatus"))
                'Case "Proviene SIE"
                '    If Not Session("rbtnSIE_SI") Is Nothing Then
                '        rbtnSIE_SI.Checked = Convert.ToBoolean(Session("rbtnSIE_SI").ToString())
                '    ElseIf Not Session("rbtnSIE_NO") Is Nothing Then
                '        rbtnSIE_NO.Checked = Convert.ToBoolean(Session("rbtnSIE_NO").ToString())
                '    End If
            Case Else
                Return
        End Select

    End Sub
    Public Shared Function ObtenerAreasSicod(idAreaSupervisar) As String
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE T_DSC_PARAMETRO = 'AREA_" + idAreaSupervisar + "'")
        conexion.CerrarConexion()
        If (data.Rows.Count > 0) Then
            Return data.Rows(0)("T_DSC_VALOR").ToString()
        Else
            Return ""
        End If
    End Function
    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBox1.CheckedChanged
        'If Session("SoloMiosV") Is Nothing Then
        '    Session("SoloMiosV") = "SoloMios"
        '    Session("SoloMiosF") = Nothing
        'Else
        '    Session("SoloMiosV") = Nothing
        '    Session("SoloMiosF") = "SoloMios"
        '    Me.CheckBox1.Checked = False
        'End If
        'If Session("SoloMiosF") Is Nothing Then
        '    Session("SoloMiosF") = "SoloMios"
        '    Session("SoloMiosV") = Nothing
        '    Me.CheckBox1.Checked = False
        'Else
        '    Session("SoloMiosF") = Nothing
        '    Session("SoloMiosV") = "SoloMios"
        '    Me.CheckBox1.Checked = True
        'End If





    End Sub

    Protected Sub ddlDestinatario_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDestinatario.SelectedIndexChanged

    End Sub


    ' Agregado por Jorge Rangel   25 Sept 2012
    Public Event BtnFiltrarClick()

    Protected Sub BtnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnFiltrar.Click

        RaiseEvent BtnFiltrarClick()

    End Sub



    Protected Sub ddlTipoDocto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoDocto.SelectedIndexChanged

    End Sub
End Class
