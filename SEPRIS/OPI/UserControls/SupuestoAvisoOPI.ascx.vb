Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports Entities
Imports Utilerias
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web.Services.Protocol

Public Class SupuestoAvisoOPI
    Inherits System.Web.UI.UserControl

    Private _cboRO As Boolean
    Public Property ComboReadOnly() As Boolean
        Get
            Return _cboRO
        End Get
        Set(ByVal value As Boolean)
            _cboRO = value
        End Set
    End Property

    Dim _idSupuesto As Integer
    Public Property ValSupuestoAviso
        Get
            Return ddlSupuestoAvisoOPI.SelectedValue
        End Get
        Set(value)
            _idSupuesto = value
        End Set
    End Property

    Public ReadOnly Property ValIndexSupuestoAviso
        Get
            Return ddlSupuestoAvisoOPI.SelectedIndex
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Inicializar()
        End If
    End Sub

    Protected Sub Inicializar()
        Call Llena_ComboSupuestos()
        ComboSupuestoOPI.Disabled = Me.ComboReadOnly
        If Me.ComboReadOnly Then
            ddlSupuestoAvisoOPI.SelectedIndex = _idSupuesto
            divSupuestoAviso.Visible = True
            lbl_Aviso.Text = ddlSupuestoAvisoOPI.SelectedItem.Text
            ComboSupuestoOPI.Visible = False
        End If
    End Sub

    Private Sub Llena_ComboSupuestos()
        Dim dt As New DataTable
        Dim i As Integer = 0
        Dim iTEM As ListItem

        With ddlSupuestoAvisoOPI
            dt = ObtenerSupuestos()
            For i = 0 To dt.Rows.Count - 1
                iTEM = New ListItem(dt.Rows(i).Item("T_DSC_SUPUESTO").ToString.Substring(0, 150), dt.Rows(i).Item("I_ID_SUPUESTO").ToString)
                iTEM.Attributes.Add("Title", dt.Rows(i).Item("T_DSC_SUPUESTO").ToString)
                .Items.Add(iTEM)
            Next
            .Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        End With
    End Sub

    Public Shared Function ObtenerSupuestos() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("Select I_ID_SUPUESTO, T_DSC_SUPUESTO from [dbo].[BDS_C_OPI_SUPUESTOS] ORDER BY I_ID_SUPUESTO")

        conexion.CerrarConexion()

        Return data

    End Function

    'Private Sub Llena_ComboSupuestos()

    '    Dim dsSupuestos As WR_SICOD.Diccionario()

    '    Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
    '    Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
    '    Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

    '    Dim iTEM As ListItem

    '    Try
    '        Dim mycredentialCache As CredentialCache = New CredentialCache()
    '        Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
    '        Dim proxySICOD As New WR_SICOD.ws_SICOD
    '        proxySICOD.Credentials = credentials
    '        dsSupuestos = proxySICOD.GetTipoEntidades

    '        With ddlSupuestoAvisoOPI

    '            For i = 0 To dsSupuestos.tables(0).Rows.Count - 1
    '                iTEM = New ListItem(dt.Rows(i).Item("I_ID_SUPUESTO").ToString, dt.Rows(i).Item("T_DSC_SUPUESTO").ToString.Substring(0, 50))
    '                iTEM.Attributes.Add("Title", dt.Rows(i).Item("T_DSC_SUPUESTO").ToString)
    '                .Items.Add(iTEM)
    '            Next
    '        End With
    '        If dsSupuestos IsNot Nothing Then
    '            ddlSupuestoAvisoOPI.DataSource = List
    '            ddlTipoEntidad.DataTextField = "value"
    '            ddlTipoEntidad.DataValueField = "key"
    '            ddlTipoEntidad.DataBind()
    '            ddlTipoEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
    '        End If

    '    Catch ex As Exception
    '        catch_cone(ex, "cargadllTiposEntidades()")
    '    End Try
    'End Sub

End Class