Imports System.Net
Imports System.Web.Configuration
Imports Clases

Public Class Supervisor1
    Inherits System.Web.UI.UserControl
    Dim enc As New YourCompany.Utils.Encryption.Encryption64

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property AbrArea
        Get
            Return Session("ABR_AREA")
        End Get
    End Property

    Public ReadOnly Property SubProcesoView
        Get
            Return Session("SUB_PROCESO")
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
            Return Session("ID_USR")
        End Get
    End Property

    Public ReadOnly Property IdArea
        Get
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID)
            Return usuario.IdArea
        End Get
    End Property

    Public ReadOnly Property PC As Entities.PC
        Get
            Return DirectCast(Session("PC"), Entities.PC)
        End Get
    End Property

    Public ReadOnly Property FechaRecepcion
        Get
            Return IIf(IsNothing(Session("FechaRecepcion")), "01/01/1900", Session("FechaRecepcion"))
        End Get
    End Property


    Public Sub Inicializar()

        ddlSubprocesoP2.Attributes.Add("onchange", "ObtenerListaSupervisores()")
        ddlEntidad.Attributes.Add("onchange", "ObtenerSubEntidades()")
        'chkSubentidades.Attributes.Add("onclick", "ObtenerSubEntidades()")
        cargadllTiposEntidades(IdArea)
        Utilerias.Generales.CargarComboOrdenadoOriginalRows(ddlProcesoP2, Proceso.ObtenerPcVigentes(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea), "T_DSC_DESCRIPCION", "I_ID_PROCESO")
        Utilerias.Generales.CargarComboOrdenadoOriginalRows(ddlEntidad, ConexionSICOD.ObtenerEntidadesAFOREArea(IdArea), "SIGLAS_ENT", "CVE_ID_ENT")

        Dim DiasHab As Integer = 0

        Dim NewDate As Date = FechaRecepcion

        While DiasHab < BandejaPC.ObtenerDiasVencimiento
            NewDate = CDate(NewDate).AddDays(1).ToString("dd/MM/yyyy")
            If Not (NewDate.DayOfWeek = DayOfWeek.Sunday Or NewDate.DayOfWeek = DayOfWeek.Saturday Or ConexionSICOD.IsFestivo(NewDate.ToString("yyyy-MM-dd")) = "Si") Then
                DiasHab = DiasHab + 1
            End If
        End While

        If Not IsNothing(PC) Then
            PC.FechaVencimiento = NewDate.ToString("dd/MM/yyyy")
        End If

        txtFechaVencimiento.Text = NewDate.ToString("dd/MM/yyyy")


        If TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea = Constantes.AREA_VF Then
            trSubentidadesCheck.Visible = True
            trSubentidadesList.Visible = True
        End If



    End Sub

    Public Sub Cargar()

        Inicializar()

        txtNumInterno.Text = PC.NumPCInterno
        txtFechaVencimiento.Text = PC.FechaVencimiento.ToString("dd/MM/yyyy")
        txtDescripcion.Text = PC.Descripcion
        ddlEntidad.SelectedValue = PC.IdEntidad
        ddlTipoEntidad.SelectedValue = PC.IdTipoEntidad
        ddlProcesoP2.SelectedValue = PC.IdProceso

        Utilerias.Generales.CargarComboOrdenado(ddlSubprocesoP2, ObtenerSubProceso(PC.IdProceso), "Text", "Value")
        Utilerias.Generales.CargarComboOrdenado(ddlSubEntidad, ObtenerSubEntidades(PC.IdEntidad), "Text", "Value")
        cargadllTiposEntidades(PC.IdArea)
        Utilerias.Generales.CargarComboOrdenadoOriginalRows(ddlEntidad, ConexionSICOD.ObtenerEntidadesAFOREArea(PC.IdArea), "SIGLAS_ENT", "CVE_ID_ENT")

        ddlSubprocesoP2.SelectedValue = PC.IdSubproceso
        Session("SUB_PROCESO") = PC.IdSubproceso

        If PC.IdSubEntidad > 0 Then
            ObtenerSubEntidades(PC.IdEntidad.ToString())
            ddlSubEntidad.SelectedValue = PC.IdSubEntidad
        End If
        trSubentidadesList.Visible = PC.IdSubEntidad > 0

        Dim SupervisoresListAsignado As List(Of ListItem) = ObtenerSupervisoresFolio()
        lstSupervisorSeleccionado.DataSource = SupervisoresListAsignado
        lstSupervisorSeleccionado.DataTextField = "Text"
        lstSupervisorSeleccionado.DataValueField = "Value"
        lstSupervisorSeleccionado.DataBind()

        Dim SupervisorDisponible As List(Of ListItem) = ObtenerSupervisores()
        For Each SupAsing As ListItem In SupervisoresListAsignado
            SupervisorDisponible.Remove(SupAsing)
        Next

        If (SupervisorDisponible.Count <> 0) Then
            lstSupervisorDisponible.DataSource = SupervisorDisponible
            lstSupervisorDisponible.DataTextField = "Text"
            lstSupervisorDisponible.DataValueField = "Value"
        End If

        Select Case Integer.Parse(PC.IdEstatus)
            Case 0
                pnlEntidad.Enabled = True
                pnlAgregarSupervisor.Enabled = True
                pnlEntidad.Enabled = True
            Case 2
                pnlEntidad.Enabled = False
                pnlAgregarSupervisor.Enabled = False
                pnlDescripcion.Enabled = False
                ddlSubprocesoP2.Attributes.Add("onchange", "LlenarSupervisorInspector()")

            Case Else
                pnlEntidad.Enabled = False
                pnlAgregarSupervisor.Enabled = False
                pnlEntidad.Enabled = False
        End Select

    End Sub
    Private Sub cargadllTiposEntidades(_id_area As Integer)
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim dsEntidades As WR_SICOD.Diccionario()

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Try
            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            Dim list As IEnumerable(Of WR_SICOD.Diccionario) = Nothing
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))

            dsEntidades = proxySICOD.GetTipoEntidades

            Select Case _id_area

                Case 35 Or 1
                    list = From p In dsEntidades
                           Where p.Key = 1 _
                           Or p.Key = 7 _
                           Or p.Key = 12
                           Select p

                Case 1
                    list = From p In dsEntidades
                           Where p.Key = 1 _
                           Or p.Key = 7 _
                           Or p.Key = 12
                           Select p
                Case 36
                    ''PCMT: el requerimento solicita SIEFORE pero no la devuelve el catálogo
                    list = From p In dsEntidades
                           Where p.Key = 1 Or p.Key = 2 _
                           Or p.Key = 3 Or p.Key = 4 _
                           Or p.Key = 17 _
                           Or p.Key = 7 _
                           Or p.Key = 12
                           Select p
            End Select

            If dsEntidades IsNot Nothing Then
                ddlTipoEntidad.DataSource = list
                ddlTipoEntidad.DataTextField = "value"
                ddlTipoEntidad.DataValueField = "key"
                ddlTipoEntidad.DataBind()
                ddlTipoEntidad.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
            End If

        Catch ex As Exception
            catch_cone(ex, "cargadllTiposEntidades()")
        End Try
    End Sub
    Protected Sub ddlTipoEntidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoEntidad.SelectedIndexChanged
        'Dim _idTE As Integer = ddlTipoEntidad.SelectedValue
        'Utilerias.Generales.CargarComboOrdenadoOriginalRows(ddlEntidad, ConexionSICOD.ObtenerEntidadesAFOREArea(_idTE), "SIGLAS_ENT", "CVE_ID_ENT")

        If Not (ddlTipoEntidad.SelectedValue <> "" AndAlso ddlTipoEntidad.SelectedValue <> "-1") Then
            ddlEntidad.Enabled = False
            Exit Sub
        End If
        Dim usuario As New Entities.Usuario()
        Dim _idTE As Integer = ddlTipoEntidad.SelectedValue

        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        ddlEntidad.Enabled = True

        'RN38
        If ddlTipoEntidad.SelectedItem.Text = "SB" OrElse
           ddlTipoEntidad.SelectedItem.Text = "SIAC" OrElse
            ddlTipoEntidad.SelectedItem.Text = "SIPS" OrElse
            ddlTipoEntidad.SelectedItem.Text = "SIAV" Then
            trSubentidadesList.Visible = True
        Else
            trSubentidadesList.Visible = False
        End If

        Dim dsEntidades As DataSet

        Dim UsuarioWs As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")


        Dim mycredentialCache As CredentialCache = New CredentialCache()
        Dim credentials As NetworkCredential = New NetworkCredential(UsuarioWs, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"
        Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

        ddlEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))


        dsEntidades = proxySICOD.GetEntidadesComplete(_idTE)

        Dim lstEntidadesOrdenada = From l In lstEntidadesSicod Distinct Order By l.DSC


        ddlEntidad.Enabled = False
        If dsEntidades IsNot Nothing Then
            If dsEntidades.Tables(0).Rows.Count > 0 Then
                ddlEntidad.DataSource = dsEntidades.Tables(0)
                ddlEntidad.DataTextField = "SIGLAS_ENT"
                ddlEntidad.DataValueField = "CVE_ID_ENT"
                ddlEntidad.DataBind()
                ddlEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
                ddlEntidad.Enabled = True
            End If
        End If
    End Sub
    Protected Sub ObtenerSubEntidades(Entidad As String)

        Dim SubEntidadesRel As DataSet = Entities.Entidad.ObtenerPorFolio(Folio)
        Dim SubEntidades As DataTable = ConexionSICOD.ObtenerSubEntidadesAFORE(Entidad)
        Dim ResultadoSubEntidades As StringBuilder = New StringBuilder()

        For Each row_1 As DataRow In SubEntidadesRel.Tables(0).Rows

            For Each row As Data.DataRow In SubEntidades.Rows
                If (row_1("N_ID_SUB_ENTIDAD") = row("ID_SUBENT")) Then
                    ResultadoSubEntidades.AppendLine("<tr><td style='text-align:justify'>" + row("DSC_SUBENT").ToString() + "<td/><tr/>")
                End If
            Next

        Next

        ltl_SubEntidad.Text = ResultadoSubEntidades.ToString()
    End Sub


    Protected Function ObtenerSupervisoresFolio() As List(Of ListItem)
        Dim dsSupervisor As DataSet = Supervisor.ObtenerPorFolio(Folio)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSupervisor.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})

        Next

        Return subprocesos
    End Function

    Public Shared Function ObtenerSupervisoresFolioPC(Folio As Integer) As List(Of ListItem)
        Dim dsSupervisor As DataSet = Supervisor.ObtenerPorFolio(Folio)


        Dim ListSupervioresSelec As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSupervisor.Tables(0).Rows
            ListSupervioresSelec.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})

        Next

        Return ListSupervioresSelec
    End Function


    Protected Function ObtenerSupervisores() As List(Of ListItem)
        Dim dsSubprocesos As DataSet = Supervisor.ObtenerVigentesPorSubproceso(SubProcesoView, 0)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})

        Next

        Return subprocesos
    End Function


    Protected Function ObtenerSubProceso(Proceso As Integer) As List(Of ListItem)

        Dim dsSubprocesos As DataSet = Subproceso.ObtenerVigentesPorProceso(Proceso)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("I_ID_SUBPROCESO").ToString(),
                          .Text = row("T_DSC_DESCRIPCION").ToString()})

        Next

        Return subprocesos
    End Function
    Protected Function ObtenerSubEntidades(Entidad As Integer) As List(Of ListItem)
        Dim subEntidades As New List(Of ListItem)()

        For Each row As Data.DataRow In ConexionSICOD.ObtenerSubEntidadesAFORE(Entidad).Rows
            subEntidades.Add(New ListItem() With {
                          .Value = row("ID_SUBENT").ToString(),
                          .Text = row("SGL_SUBENT").ToString()})

        Next

        Return subEntidades
    End Function
    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
    End Sub
End Class
