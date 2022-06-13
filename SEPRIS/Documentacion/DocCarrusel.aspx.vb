Public Class DocCarrusel
    Inherits Page

    Protected Sub btnManualUsuario_Click(sender As Object, e As EventArgs) Handles btnManualUsuario.Click
        lblTituloManualUsuario.Text = "Manual de Usuario de Carrusel de Asignaciones"
        pnlUsuario.Visible = True
        pnlPrincipal.Visible = False
        pnlTecnico.Visible = False
    End Sub


    Protected Sub btnManualTecnico_Click(sender As Object, e As EventArgs) Handles btnManualTecnico.Click
        lblTituloManualTecnico.Text = "Manual Técnico de Carrusel de Asignaciones"
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = False
        pnlTecnico.Visible = True
    End Sub

    Protected Sub btnRegresarUusario_Click(sender As Object, e As EventArgs) Handles btnRegresarUusario.Click
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = True
        pnlTecnico.Visible = False
    End Sub

    Private Sub btnRegresarTecnico_Click(sender As Object, e As EventArgs) Handles btnRegresarTecnico.Click
        pnlUsuario.Visible = False
        pnlPrincipal.Visible = True
        pnlTecnico.Visible = False
    End Sub

End Class