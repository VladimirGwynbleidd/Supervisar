Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Sicod
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function

    <WebMethod()>
    Public Function AgregarPC(IdFolio As Integer) As Boolean

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If System.Web.Configuration.WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(System.Web.Configuration.WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(System.Web.Configuration.WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If

        data = conexion.ConsultarDT("SELECT ID_FOLIO [N_ID_SICOD], FECH_REGISTRO [F_FECH_REGISTRO], R.DSC_REMITENTE [T_DSC_REMITENTE], DSC_NUM_OFICIO [T_DSC_NUM_OFICIO], " +
                                    "FECH_DOC [F_FECH_DOC], FECH_RECEPCION [F_FECH_RECEPCION], U.DSC_UNIDAD_ADM [T_DSC_UNIDAD_ADM], DSC_ASUNTO [T_DSC_ASUNTO], " +
                                    "DSC_NOMB_FIRMNT [T_DSC_NOMB_FIRMNT], DSC_AP_PAT_FIRMNT [T_DSC_AP_PAT_FIRMNT], DSC_AP_MAT_FIRMNT [T_DSC_AP_MAT_FIRMNT], D.DSC_T_DOC [T_DSC_T_DOC], " +
                                    "INFO.ID_UNIDAD_ADM [I_ID_AREA], DSC_REFERENCIA [T_DSC_REFERENCIA] " +
                                    "FROM BDA_INFO_DOC INFO " +
                                    "JOIN BDA_C_REMITENTE R ON INFO.ID_REMITENTE=R.ID_REMITENTE " +
                                    "JOIN BDA_C_UNIDAD_ADM U ON INFO.ID_T_UNIDAD_ADM = U.ID_T_UNIDAD_ADM And INFO.ID_UNIDAD_ADM = U.ID_UNIDAD_ADM " +
                                    "JOIN BDA_C_T_DOC D ON INFO.ID_T_DOC = D.ID_T_DOC " +
                                    "WHERE ID_FOLIO = " + IdFolio.ToString())


        conexion.CerrarConexion()

        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)

        campos.Add("N_ID_SICOD") : valores.Add(data.Rows(0)("N_ID_SICOD"))
        campos.Add("F_FECH_REGISTRO") : valores.Add(data.Rows(0)("F_FECH_REGISTRO"))
        campos.Add("T_DSC_REMITENTE") : valores.Add(data.Rows(0)("T_DSC_REMITENTE"))
        campos.Add("T_DSC_NUM_OFICIO") : valores.Add(data.Rows(0)("T_DSC_NUM_OFICIO"))
        campos.Add("F_FECH_DOC") : valores.Add(data.Rows(0)("F_FECH_DOC"))
        campos.Add("F_FECH_RECEPCION") : valores.Add(data.Rows(0)("F_FECH_RECEPCION"))
        campos.Add("T_DSC_UNIDAD_ADM") : valores.Add(data.Rows(0)("T_DSC_UNIDAD_ADM"))
        campos.Add("T_DSC_ASUNTO") : valores.Add(data.Rows(0)("T_DSC_ASUNTO"))
        campos.Add("T_DSC_NOMB_FIRMNT") : valores.Add(data.Rows(0)("T_DSC_NOMB_FIRMNT"))
        campos.Add("T_DSC_AP_PAT_FIRMNT") : valores.Add(data.Rows(0)("T_DSC_AP_PAT_FIRMNT"))
        campos.Add("T_DSC_AP_MAT_FIRMNT") : valores.Add(data.Rows(0)("T_DSC_AP_MAT_FIRMNT"))
        campos.Add("T_DSC_T_DOC") : valores.Add(data.Rows(0)("T_DSC_T_DOC"))
        campos.Add("I_ID_AREA") : valores.Add(data.Rows(0)("I_ID_AREA"))
        campos.Add("T_DSC_REFERENCIA") : valores.Add(data.Rows(0)("T_DSC_REFERENCIA"))
        campos.Add("I_ID_ESTATUS") : valores.Add(0)
        conexion = New Conexion.SQLServer()
        conexion.Insertar("BDS_D_PC_PROGRAMA_CORRECCION", campos, valores)
        conexion.CerrarConexion()

        Return True

    End Function

End Class