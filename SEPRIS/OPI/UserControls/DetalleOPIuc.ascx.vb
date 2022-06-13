Imports System.Net
Imports System.Web.Configuration

Public Class DetalleOPIuc
    Inherits System.Web.UI.UserControl
    Dim enc As New YourCompany.Utils.Encryption.Encryption64

    Private _id_OPI As Integer
    Public Property I_ID_OPI() As Integer
        Get
            Return _id_OPI
        End Get
        Set(ByVal value As Integer)
            _id_OPI = value
        End Set
    End Property
    Public Property FechaPI
        Get
            Return txtFechaPI.Text
        End Get
        Set(value)
            txtFechaPI.Text = value
        End Set
    End Property
    Public Property ProcesoPO
        Get
            Return dplProcesoPO.SelectedValue
        End Get
        Set(value)
            dplProcesoPO.SelectedValue = value
        End Set
    End Property
    Public Property SubprocesoPO
        Get
            Return ddlSubproceso.SelectedValue
        End Get
        Set(value)
            ddlSubproceso.SelectedValue = value
        End Set
    End Property
    Public Property DescripcionOPI
        Get
            Return txtDescOPI.Text
        End Get
        Set(value)
            txtDescOPI.Text = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Session("I_ID_OPI")) Then
                Dim usuario As New Entities.Usuario()
                usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

                cargadllTiposEntidades()
                cargarEntidadesV2()
                CargadlProceso(usuario.IdArea)
                MapeaCamposDetalle()
                DesHabilita()
            Else
                Response.Redirect("~/OPI/BandejaOPI.aspx")
            End If

        End If

    End Sub

    Private Sub MapeaCamposDetalle()
        Dim _opiDetalle As OPI_Incumplimiento
        Dim _opiFuncionalidad As New Registro_OPI
        Dim _item As ListItem

        I_ID_OPI = Session("I_ID_OPI")

        _opiDetalle = _opiFuncionalidad.GetOPIDetail(I_ID_OPI)

        ddlTipoEntidad.SelectedValue = _opiDetalle.I_ID_TIPO_ENTIDAD
        TxtddlTipoEntidad.Text = ddlTipoEntidad.SelectedItem.Text
        dplEntidad.SelectedValue = _opiDetalle.I_ID_ENTIDAD
        TxtdplEntidad.Text = dplEntidad.SelectedItem.Text
        If _opiDetalle.I_ID_SUBENTIDAD.Count > 0 Then
            trSubentidad.Visible = True
            llenachkSubEntidades(_opiDetalle.I_ID_SUBENTIDAD)
        End If

        If _opiDetalle.I_ID_SUBENTIDAD_SB > 0 Then
            trSubentidad1.Visible = True

            Dim dsSubEntidades As New DataSet
            Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
            Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
            Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")


            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            Dim dv As DataView = ConexionSICOD.ObtenerSubEntidadesAFORE(_opiDetalle.I_ID_ENTIDAD).DefaultView
            dv.RowFilter = "ID_SUBENT = " + _opiDetalle.I_ID_SUBENTIDAD_SB.ToString()
            txtSubEntidad.Text = dv(0)("DSC_SUBENT")

        End If


        dplProcesoPO.SelectedValue = _opiDetalle.I_ID_PROCESO_POSIBLE_INC
        TxtdplProcesoPO.Text = dplProcesoPO.SelectedItem.Text

        CargaddlSubProceso(_opiDetalle.I_ID_PROCESO_POSIBLE_INC)
        ddlSubproceso.SelectedValue = _opiDetalle.I_ID_SUBPROCESO
        TxtddlSubproceso.Text = ddlSubproceso.SelectedItem.Text

        txtFechaPI.Text = _opiDetalle.F_FECH_POSIBLE_INC.ToString("dd/MM/yyyy")
        If Not IsNothing(_opiDetalle.T_ID_SUPERVISORES) AndAlso _opiDetalle.T_ID_SUPERVISORES.Count > 0 Then
            For Each item In _opiDetalle.T_ID_SUPERVISORES.Keys
                _item = New ListItem(_opiDetalle.T_ID_SUPERVISORES(item).ToString(), item.ToString())
                lstSupervisores.Items.Add(_item)
                SupervisorOPI.agregaItem(_item)
            Next
            _item = Nothing
        End If

        If Not IsNothing(_opiDetalle.T_ID_INSPECTORES) AndAlso _opiDetalle.T_ID_INSPECTORES.Count > 0 Then
            For Each item In _opiDetalle.T_ID_INSPECTORES.Keys
                _item = New ListItem(_opiDetalle.T_ID_INSPECTORES(item).ToString(), item.ToString())
                lstInspectores.Items.Add(_item)
                InspectoresOPI.agregaItem(_item)
            Next
        End If

        txtDescOPI.Text = _opiDetalle.T_DSC_POSIBLE_INC
        'ComentariosOPI.ValComentarios = _opiDetalle.T_OBSERVACIONES_OPI.Trim()

    End Sub

    Protected Sub llenachkSubEntidades(_lstSubentidades As List(Of Integer))

        'Verifica que se haya seleccionado un elemento en el combobox
        If Not (dplEntidad.SelectedValue <> "" AndAlso dplEntidad.SelectedValue <> "-1") Then chkSubEntidad.Items.Clear() : Exit Sub

        Dim usuarioActual As New Entities.Usuario()
        usuarioActual = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        Dim dsSubEntidades As New DataSet
        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        If Not IsNothing(usuarioActual) Then

            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            Dim lsIdEntidad As String = ""

            lsIdEntidad = dplEntidad.SelectedValue '.Substring(1)

            dsSubEntidades = proxySICOD.GetSubEntidadesComplete(lsIdEntidad)
            Dim _dvfoundRows As DataView 'DataRow
            Dim _sFilter As String

            For Each se In _lstSubentidades
                _sFilter &= se.ToString() & ","
            Next

            _sFilter = "ID_SUBENT IN (" & _sFilter.Substring(0, _sFilter.Length - 1) & ")"

            _dvfoundRows = dsSubEntidades.Tables(0).AsDataView '.Select(_sFilter)

            _dvfoundRows.RowFilter = _sFilter
            If _dvfoundRows.Table.Rows.Count > 0 Then
                chkSubEntidad.DataSource = _dvfoundRows
                chkSubEntidad.DataTextField = "SGL_SUBENT"
                chkSubEntidad.DataValueField = "ID_SUBENT"
                chkSubEntidad.DataBind()

                For i = 0 To chkSubEntidad.Items.Count - 1
                    chkSubEntidad.Items(i).Selected = True
                Next
            End If

        End If

    End Sub

    Private Sub cargadllTiposEntidades()

        Dim dsEntidades As WR_SICOD.Diccionario()

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Try
            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))

            dsEntidades = proxySICOD.GetTipoEntidades

            'Modificacion CAGC INCIDENCIA N* 68
            Dim list = From p In dsEntidades
                       Select p

            'Codigo anterior
            '
            'Dim list = From p In dsEntidades
            '           Where p.Key = 1 _
            '           Or p.Key = 2 _
            '           Or p.Key = 3 _
            '           Or p.Key = 4 _
            '           Or p.Key = 7 _
            '           Or p.Key = 17
            '           Select p

            If dsEntidades IsNot Nothing Then
                ddlTipoEntidad.DataSource = list
                ddlTipoEntidad.DataTextField = "value"
                ddlTipoEntidad.DataValueField = "key"
                ddlTipoEntidad.DataBind()
                ddlTipoEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
            End If

        Catch ex As Exception
            catch_cone(ex, "cargadllTiposEntidades()")
        End Try
    End Sub

    Public Sub cargarEntidadesV2()

        Dim dsEntidades As New DataSet

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim vecTipoEntidades As New List(Of Integer)

        Dim usuarioActual As New Entities.Usuario()
        usuarioActual = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(7)
        vecTipoEntidades.Add(12)
        vecTipoEntidades.Add(2)
        vecTipoEntidades.Add(3)
        vecTipoEntidades.Add(4)
        vecTipoEntidades.Add(17)

        Try
            If Not IsNothing(usuarioActual) Then
                Dim mycredentialCache As CredentialCache = New CredentialCache()
                Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
                Dim proxySICOD As New WR_SICOD.ws_SICOD
                proxySICOD.Credentials = credentials
                proxySICOD.ConnectionGroupName = "SEPRIS"
                Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

                dplEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))

                For Each ent As Integer In vecTipoEntidades
                    dsEntidades = proxySICOD.GetEntidadesComplete(ent)
                    For Each ltTable As DataTable In dsEntidades.Tables
                        For Each lrRow As DataRow In ltTable.Rows
                            Dim objEntidadSicod As New Entities.EntidadSicod
                            If Int32.TryParse(lrRow("cve_id_ent"), objEntidadSicod.ID) Then
                                objEntidadSicod.ID = CInt(objEntidadSicod.ID.ToString())
                                objEntidadSicod.DSC = lrRow("siglas_ent").ToString().Trim()

                                ''Diferenciar por el descripcion
                                Dim cont As Integer = (From e In lstEntidadesSicod Where e.ID = objEntidadSicod.ID Or e.DSC = objEntidadSicod.DSC).Count()

                                If cont < 1 Then
                                    ''No se agrega procesar para financiero
                                    If usuarioActual.IdArea = Constantes.AREA_VF And objEntidadSicod.ID = 1 And objEntidadSicod.DSC = "PROCESAR" Then
                                        Continue For
                                    Else
                                        lstEntidadesSicod.Add(objEntidadSicod)
                                    End If
                                End If
                            End If
                        Next
                    Next
                Next

                Dim lstEntidadesOrdenada = From l In lstEntidadesSicod Distinct Order By l.DSC

                dplEntidad.Enabled = False
                If dsEntidades IsNot Nothing Then
                    If dsEntidades.Tables(0).Rows.Count > 0 Then
                        dplEntidad.DataSource = lstEntidadesOrdenada
                        dplEntidad.DataTextField = "DSC"
                        dplEntidad.DataValueField = "ID"
                        dplEntidad.DataBind()
                        dplEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
                        dplEntidad.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            catch_cone(ex, "cargaddlEntidades()")
        End Try
    End Sub

    Public Sub CargadlProceso(_area As Integer)
        dplProcesoPO.DataTextField = "T_DSC_DESCRIPCION"
        dplProcesoPO.DataValueField = "I_ID_PROCESO"
        dplProcesoPO.DataSource = ObtenerProcesosVigentes(_area) 'ConexionSISAN.ObtenerProcesos()
        dplProcesoPO.DataBind()
        dplProcesoPO.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
    End Sub

    Public Sub CargaddlSubProceso(_idProceso As Integer)
        ddlSubproceso.DataTextField = "text"
        ddlSubproceso.DataValueField = "value"
        ddlSubproceso.DataSource = ObtenerSubprocesos(_idProceso)
        ddlSubproceso.DataBind()
        ddlSubproceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
    End Sub
    Protected Sub dplProcesoPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dplProcesoPO.SelectedIndexChanged

        If Not (dplProcesoPO.SelectedValue <> "" AndAlso dplProcesoPO.SelectedValue <> "-1") Then Exit Sub

        ddlSubproceso.Items.Clear()
        SupervisorOPI.ClearList()
        InspectoresOPI.ClearList()

        ddlSubproceso.DataSource = ObtenerSubprocesos(dplProcesoPO.SelectedValue)
        ddlSubproceso.DataTextField = "text"
        ddlSubproceso.DataValueField = "value"
        ddlSubproceso.DataBind()

        If ddlSubproceso.Items.Count() <= 0 Then
            ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
        Else
            ddlSubproceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        End If

    End Sub
    Protected Sub ddlSubproceso_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubproceso.SelectedIndexChanged

        If Not (ddlSubproceso.SelectedValue <> "" AndAlso ddlSubproceso.SelectedValue <> "-1") Then Exit Sub

        SupervisorOPI.Id_Proceso = dplProcesoPO.SelectedValue
        SupervisorOPI.I_ID_SUBPROCESO = ddlSubproceso.SelectedValue
        SupervisorOPI.RefreshList()

        InspectoresOPI.Id_Proceso = dplProcesoPO.SelectedValue
        InspectoresOPI.I_ID_SUBPROCESO = ddlSubproceso.SelectedValue
        InspectoresOPI.RefreshList()

    End Sub
    Public Shared Function ObtenerProcesosVigentes(Area As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT I_ID_PROCESO, T_DSC_DESCRIPCION FROM [dbo].[BDS_C_GR_PROCESO] WHERE B_FLAG_VIGENTE = 1 AND I_ID_AREA =" + Area.ToString())

        conexion.CerrarConexion()

        Return data

    End Function

    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        Clases.EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
    End Sub

    Public Function ObtenerSubprocesos(Proceso As String) As List(Of ListItem)
        Dim dsSubprocesos As DataSet = Subproceso.ObtenerVigentesPorProceso(Proceso)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("I_ID_SUBPROCESO").ToString(),
                          .Text = row("T_DSC_DESCRIPCION").ToString()})

        Next

        Return subprocesos
    End Function
    Public Sub Habilita()
        trFecha.Disabled = False
        imgFechaPI.Visible = True
        dplProcesoPO.Visible = True
        TxtdplProcesoPO.Visible = False
        ddlSubproceso.Visible = True
        TxtddlSubproceso.Visible = False
        txtDescOPI.Enabled = True
        trSupervisores.Visible = True
        trInspectores.Visible = True
        lstSupervisores.Enabled = False
        lstInspectores.Enabled = False

        SupervisorOPI.Id_Proceso = dplProcesoPO.SelectedValue
        SupervisorOPI.I_ID_SUBPROCESO = ddlSubproceso.SelectedValue
        SupervisorOPI.RefreshList()

        InspectoresOPI.Id_Proceso = dplProcesoPO.SelectedValue
        InspectoresOPI.I_ID_SUBPROCESO = ddlSubproceso.SelectedValue
        InspectoresOPI.RefreshList()
        trViewListas.Visible = False
    End Sub
    Public Sub DesHabilita()
        trFecha.Disabled = True
        imgFechaPI.Visible = False
        dplProcesoPO.Visible = False
        TxtdplProcesoPO.Visible = True
        ddlSubproceso.Visible = False
        TxtddlSubproceso.Visible = True
        txtDescOPI.Enabled = False
        trSupervisores.Visible = False
        trInspectores.Visible = False
        lstSupervisores.Enabled = True
        lstInspectores.Enabled = True
        trViewListas.Visible = True
    End Sub
    Function GetSupervisoresSel() As Hashtable
        Dim lstSupervisores As New Hashtable
        If SupervisorOPI.GetSupervisoresSeleccionados().Count() > 0 Then
            For Each item In SupervisorOPI.GetSupervisoresSeleccionados()
                'lstSupervisores.Add(item.Value)
                Try
                    lstSupervisores.Add(item.Value, item.Text)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            Next
        End If
        Return lstSupervisores
    End Function
    Function GetInspectoresSel() As Hashtable
        'Dim lstInspectores As New List(Of String)
        Dim lstInspectores As New Hashtable
        If InspectoresOPI.GetInspectoresSeleccionados().Count() > 0 Then
            For Each item In InspectoresOPI.GetInspectoresSeleccionados()
                'lstInspectores.Add(item.Value)
                Try
                    lstInspectores.Add(item.Value, item.Text)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            Next

        End If
        Return lstInspectores
    End Function
End Class