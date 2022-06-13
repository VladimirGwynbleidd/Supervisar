Public Class MotivoOpi1
    Inherits System.Web.UI.UserControl
    Public Property ValMotivo1
        Get
            Return txtMotivo1.Text
        End Get
        Set(value)
            txtMotivo1.Text = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class