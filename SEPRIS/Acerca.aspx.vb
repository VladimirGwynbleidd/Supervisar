
' Fecha de creación: 16/07/2013
' Fecha de modificación:  
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' Página Acerca


Public Class Acerca
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblTitulo.Text = Conexion.SQLServer.Parametro.ObtenerValor("Titulo")
        lblDescripcion.Text = Conexion.SQLServer.Parametro.ObtenerValor("Acerca de")

    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("Login.aspx")
    End Sub
End Class