Public Class PruebaServicios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim wsSISAN As New wsSisanReg.RegistroExterno
        Dim wsSICOD As New WR_SICOD.ws_SICOD

        Response.Write("SICOD: " + wsSICOD.Url)
        Response.Write("</br>")
        Response.Write("SISAN: " + wsSISAN.Url)




    End Sub

End Class