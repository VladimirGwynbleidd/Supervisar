Imports Clases

Public Class PruebaOSIRIS


    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim con1 As New OracleConexion()
        Dim lsQuery As String = ""
        lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON S.ID_T_ENT = TE.ID_T_ENT UNION SELECT F.ID_SUBENT,F.SGL_SUBENT DSC_SUBENT,F.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD_F JOIN osiris.BDV_C_T_ENTIDAD TE ON F.ID_T_ENT = TE.ID_T_ENT"

        Dim dt As DataSet = con1.Datos(lsQuery)

        GridView1.DataSource = dt
        GridView1.DataBind()

    End Sub
End Class