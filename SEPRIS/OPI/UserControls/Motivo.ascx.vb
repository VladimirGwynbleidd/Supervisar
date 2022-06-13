'Sye-Juan.Jose.Velazquez
Public Class motivo
    Inherits System.Web.UI.UserControl
    Public Property ValMotivo
        Get
            Return txtMotivo.Text
        End Get
        Set(value)
            txtMotivo.Text = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class