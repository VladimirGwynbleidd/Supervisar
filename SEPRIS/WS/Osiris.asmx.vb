Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Clases

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Osiris
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function


    <WebMethod()>
    Public Function ObtenerSubEntidades(ByVal entidad As Integer) As DataTable
        Dim con1 As New OracleConexion()
        Dim lsQuery As String = ""

        entidad = entidad.ToString().Substring(1)

        lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & entidad.ToString() & ",4" & entidad.ToString() & ",5" & entidad.ToString() & ",6" & entidad.ToString() & ") and S.ID_T_ENT in (2,3,4,17) ANd S.VIG_FLAG=1 ORDER BY S.SGL_SUBENT "
        'lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & entidad.ToString() & ",4" & entidad.ToString() & ",5" & entidad.ToString() & ",6" & entidad.ToString() & ") and S.ID_T_ENT in (2,3,4,17) ANd S.VIG_FLAG=1 UNION SELECT F.ID_SUBENT,F.SGL_SUBENT DSC_SUBENT,F.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD_F F JOIN osiris.BDV_C_T_ENTIDAD TE ON(F.ID_T_ENT = TE.ID_T_ENT) where F.CVE_ID_ENT in (3" & entidad.ToString() & ",4" & entidad.ToString() & ",5" & entidad.ToString() & ",6" & entidad.ToString() & ") and F.ID_T_ENT in (2,3,4,17) ANd F.VIG_FLAG=1"

        Dim dt As DataSet = con1.Datos(lsQuery)

        Return dt.Tables(0)

    End Function


End Class