Public Class Encuestas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAspectosEvaluar.Click
        Response.Redirect("~/Mantenimiento/CatalogoAspectosEvaluar.aspx")
    End Sub

    Protected Sub btnOpcionesEvaluacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpcionesEvaluacion.Click
        Response.Redirect("~/Mantenimiento/CatalogoOpcionesEvaluacion.aspx")
    End Sub

    Protected Sub btnEncuestas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEncuestas.Click
        Response.Redirect("~/Mantenimiento/CatalogoEncuestas.aspx")
    End Sub
End Class