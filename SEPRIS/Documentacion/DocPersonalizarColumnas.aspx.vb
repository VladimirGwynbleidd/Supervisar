Public Class DocPersonalizarColumnas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnManualUsuario_Click(sender As Object, e As EventArgs) Handles btnManualUsuario.Click
        lblTituloManualUsuario.Text = "Manual de Usuario de Personalizar Columnas"
        pnlUsuario.Visible = True
        pnlPrincipal.Visible = False
        pnlTecnico.Visible = False
    End Sub


    Protected Sub btnManualTecnico_Click(sender As Object, e As EventArgs) Handles btnManualTecnico.Click
        lblTituloManualTecnico.Text = "Manual Técnico de Catalogo de Personalizar Columnas"
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = False
        pnlTecnico.Visible = True
    End Sub

    Protected Sub btnRegresarUusario_Click(sender As Object, e As EventArgs) Handles btnRegresarUusario.Click
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = True
        pnlTecnico.Visible = False
    End Sub

    Private Sub btnRegresarTecnico_Click(sender As Object, e As System.EventArgs) Handles btnRegresarTecnico.Click
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = True
        pnlTecnico.Visible = False
    End Sub

End Class