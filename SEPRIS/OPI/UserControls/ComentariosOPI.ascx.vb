Public Class ComentariosOPI
    Inherits System.Web.UI.UserControl

    Public Property ValComentarios
        Get
            Return txtComentariosOPI.Text
        End Get
        Set(value)
            txtComentariosOPI.Text = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class