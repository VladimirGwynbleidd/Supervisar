' Fecha de creación:17/07/2013
' Fecha de modificación:  
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' Descrición del nuevo componente o de la modificación


Public Class Manual
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("Login.aspx")
    End Sub
End Class