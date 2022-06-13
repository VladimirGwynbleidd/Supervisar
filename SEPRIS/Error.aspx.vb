Public Class _Error
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("Exception_1")) Then
            Dim ex = CType(Session("Exception_1"), Exception)
            Session("Exception_1") = Nothing
            Label3.Text = ex.Message + ex.StackTrace
        End If
    End Sub

End Class