Public Class DocComponenteAyuda
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnManualUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnManualUsuario.Click
        lblTituloManualUsuario.Text = "Manual de Usuario de Componente de Ayuda"
        pnlUsuario.Visible = True
        pnlPrincipal.Visible = False
        pnlTecnico.Visible = False
    End Sub


    Protected Sub btnManualTecnico_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnManualTecnico.Click
        lblTituloManualTecnico.Text = "Manual Técnico de Componente de Ayuda"
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = False
        pnlTecnico.Visible = True
    End Sub

    Protected Sub btnRegresarUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresarUsuario.Click
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = True
        pnlTecnico.Visible = False
    End Sub

    Private Sub btnRegresarTecnico_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresarTecnico.Click
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = True
        pnlTecnico.Visible = False
    End Sub

End Class