Public Class ClasificacionOPI
    Inherits System.Web.UI.UserControl

    Public Property ComboVisible
        Get
            Return Session("ComboVisibleClasificacion")
        End Get
        Set(value)
            Session("ComboVisibleClasificacion") = value
        End Set
    End Property
    Public Property ValClasificacion
        Get
            Return ddlClasificacionOPI.SelectedValue
        End Get
        Set(value)
            txtClasificacionOPI.Text = value
        End Set
    End Property

    Public ReadOnly Property ValIndexClasificacion
        Get
            Return ddlClasificacionOPI.SelectedIndex
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Inicializar()
        End If
    End Sub

    Public Sub Inicializar()
        If ComboVisible Then
            LblAsterico.Style("Visibility") = "visible"
            DivTxtClasifOPI.Visible = False
            DivComboClasifOPI.Visible = True
            'DivTxtClasifOPI.Style("Display") = "none"
            'DivComboClasifOPI.Style("Visibility") = "visible"
        Else
            LblAsterico.Style("Display") = "none"
            DivComboClasifOPI.Visible = False
            DivTxtClasifOPI.Visible = True
            'DivComboClasifOPI.Style("Display") = "none"
            'DivTxtClasifOPI.Style("Visibility") = "visible"
        End If
    End Sub
    Public Sub ActClasificacion(clasificaion As String)

        If Not clasificaion Is Nothing AndAlso Not String.IsNullOrEmpty(clasificaion) Then
            ddlClasificacionOPI.SelectedValue = clasificaion
        End If

    End Sub
End Class