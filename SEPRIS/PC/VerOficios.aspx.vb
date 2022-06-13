Public Class VerOficios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("IdOficioSEPRIS") = Request.QueryString("folio").Replace("-", "/")
    End Sub

End Class