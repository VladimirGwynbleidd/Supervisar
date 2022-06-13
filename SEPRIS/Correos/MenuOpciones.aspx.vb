Public Class MenuOpciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCorreos_Click(sender As Object, e As EventArgs) Handles btnCorreos.Click
        Response.Redirect("CatalogoMensajes.aspx", True)
    End Sub

    Protected Sub bCorreoPerfil_Click(sender As Object, e As EventArgs) Handles bCorreoPerfil.Click
        Response.Redirect("CorreosPerfil.aspx", True)
    End Sub

    Protected Sub btnExpeciones_Click(sender As Object, e As EventArgs) Handles btnExpeciones.Click
        Response.Redirect("ExclusionCorreoUsuario.aspx", True)
    End Sub

End Class