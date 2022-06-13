Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports Utilerias
Imports System.Reflection
Imports System.ComponentModel

Public Class ConexionSISAN

    Public Property ObtenerQuery As String

    'Select ID_PROCESO, DESC_PROCESO from dbo.BSA_C_PROCESO where VIG_FLAG = 1 order by DESC_PROCESO
    'Select ID_SUBPROCESO, DESC_SUBPROCESO from dbo.BSA_C_SUBPROCESO where VIG_FLAG = 1 And ID_PROCESO = 56
    'Select ID_CONDUCTA, DESC_CONDUCTA from dbo.BSA_C_CONDUCTA where VIG_FLAG = 1 And ID_PROCESO = 56 And ID_SUBPROCESO = 50

    Public Shared Function ObtenerFechaSancion(FolioSISAN As String) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        Dim strDescripcion As String = ""

        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT ISNULL( CONVERT (VARCHAR,F_FECH_ENVIA_SANCIONES,111), '') AS F_FECH_ENVIA_SANCIONES from [dbo].[BSA_IRREGULARIDAD] WHERE T_FOLIO = '" + FolioSISAN + "'")
        If data.Rows.Count > 0 Then
            strDescripcion = data.Rows(0).Item("F_FECH_ENVIA_SANCIONES")
        End If

        conexion.CerrarConexion()

        Return strDescripcion

    End Function

    Public Shared Function ObtenerFechaSancionFiltro() As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        Dim strDescripcion As String = ""

        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT ISNULL( CONVERT (VARCHAR,F_FECH_ENVIA_SANCIONES,103), '') AS F_FECH_ENVIA_SANCIONES from [dbo].[BSA_IRREGULARIDAD]")

        conexion.CerrarConexion()

        Return data

    End Function


    Public Shared Function ObtenerIDFechaAcuse(Irregularidad As String) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim Iddocumento As Integer
        Dim datasupport As DataTable ''En esta tabla se guardan los datos con el verdadero I_ID_DOCUMENTO,I_ID_OFICIO Y F_FECH_ACUSE
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        datasupport = conexion.ConsultarDT("SELECT I_ID_DOCUMENTO,I_ID_OFICIO,ISNULL(CONVERT(varchar, F_FECH_ACUSE,103),'') AS F_FECH_ACUSE FROM [dbo].[BSA_DATOS_ENV_OF_SICOD] WHERE I_ID_IRREGULARIDAD = '" + Irregularidad.ToString + "'")


    End Function


    Public Shared Function ObtenerFechaAcuse(Irregularidad As String) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable

        Dim strDescripcion As String

        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT ISNULL(CONVERT(varchar, F_FECH_ACUSE,103),'') AS F_FECH_ACUSE FROM [dbo].[BSA_DATOS_ENV_OF_SICOD] WHERE I_ID_IRREGULARIDAD = '" + Irregularidad.ToString + "'")


        If data.Rows.Count > 0 Then
            strDescripcion = data.Rows(0).Item("F_FECH_ACUSE")
        End If

        conexion.CerrarConexion()

        Return strDescripcion

    End Function

    Public Shared Function ObtenerProcesos() As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("Select ID_PROCESO, DESC_PROCESO from dbo.BSA_C_PROCESO where VIG_FLAG = 1 order by DESC_PROCESO")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerSubprocesos(Proceso As Integer) As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("Select ID_SUBPROCESO, DESC_SUBPROCESO from dbo.BSA_C_SUBPROCESO where VIG_FLAG = 1 And ID_PROCESO = " + Proceso.ToString())

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerConducta(Proceso As Integer, Subproceso As Integer) As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("Select ID_CONDUCTA, DESC_CONDUCTA from dbo.BSA_C_CONDUCTA where VIG_FLAG = 1 And ID_PROCESO = " + Proceso.ToString() + " And ID_SUBPROCESO = " + Subproceso.ToString())

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerIrregularidades(Proceso As Integer, Subproceso As Integer, Conducta As Integer) As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("select ID_IRREGULARIDAD, DESC_IRREGULARIDAD from BSA_C_IRREGULARIDAD where ID_PROCESO = " + Proceso.ToString() + " AND ID_SUBPROCESO = " + Subproceso.ToString() + " AND ID_CONDUCTA= " + Conducta.ToString() + " AND VIG_FLAG  = 1")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function TraeDescProcesosxID(ID As Integer) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        Dim strDescripcion As String
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("Select ID_PROCESO, DESC_PROCESO from dbo.BSA_C_PROCESO where VIG_FLAG = 1 and ID_Proceso = " & ID & " order by DESC_PROCESO")

        conexion.CerrarConexion()

        strDescripcion = data.Rows(0).Item("Desc_Proceso").ToString()

        Return strDescripcion
    End Function

    Public Shared Function TraeDescSubprocesosxID(IDSubProceso As Integer) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        Dim strDescripcion As String
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("Select ID_SUBPROCESO, DESC_SUBPROCESO from dbo.BSA_C_SUBPROCESO where VIG_FLAG = 1 And ID_SUBPROCESO = " & IDSubProceso)

        conexion.CerrarConexion()

        strDescripcion = data.Rows(0).Item("Desc_Subproceso").ToString()

        Return strDescripcion
    End Function

    Public Shared Function TraeDescConductaxID(IDConducta As Integer) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        Dim strDescripcion As String
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("Select ID_CONDUCTA, DESC_CONDUCTA from dbo.BSA_C_CONDUCTA where VIG_FLAG = 1 And ID_CONDUCTA = " & IDConducta)

        conexion.CerrarConexion()

        strDescripcion = data.Rows(0).Item("Desc_Conducta").ToString()

        Return strDescripcion
    End Function

    Public Shared Function TraeDescIrregularidadxID(IDIrregularidad As Integer) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        Dim strDescripcion As String
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If
        data = conexion.ConsultarDT("select ID_IRREGULARIDAD, DESC_IRREGULARIDAD from BSA_C_IRREGULARIDAD where ID_IRREGULARIDAD = " & IDIrregularidad)

        conexion.CerrarConexion()

        strDescripcion = data.Rows(0).Item("DESC_IRREGULARIDAD").ToString()

        Return strDescripcion
    End Function

    Public Shared Function ObtenerDescripciones(Proceso As Integer, Subproceso As Integer, Conducta As Integer, Irregularidad As Integer, Participante As Integer, Gravedad As Integer) As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT P.DESC_PROCESO, SP.DESC_SUBPROCESO, C.DESC_CONDUCTA, I.DESC_IRREGULARIDAD, PA.DESC_PARTICIPANTE, G.DSC_GRAVEDAD " +
                                  " FROM BSA_C_PROCESO P" +
                                  " JOIN BSA_C_SUBPROCESO SP ON P.ID_PROCESO=SP.ID_PROCESO" +
                                  " JOIN BSA_C_CONDUCTA C ON SP.ID_PROCESO = C.ID_PROCESO AND SP.ID_SUBPROCESO= C.ID_SUBPROCESO" +
                                  " JOIN BSA_C_IRREGULARIDAD I ON I.ID_PROCESO = C.ID_PROCESO And I.ID_SUBPROCESO= C.ID_SUBPROCESO And I.ID_CONDUCTA = C.ID_CONDUCTA" +
                                  " JOIN BSA_C_PARTICIPANTE PA ON I.ID_PARTICIPANTE = PA.ID_PARTICIPANTE" +
                                  " JOIN BDS_C_GRAVEDAD G ON I.ID_GRAVEDAD = G.ID_GRAVEDAD" +
                                  " WHERE P.ID_PROCESO = " +
                                  " AND SP.ID_SUBPROCESO = " +
                                  " AND C.ID_CONDUCTA = " +
                                  " AND I.ID_IRREGULARIDAD =" +
                                  " AND PA.ID_PARTICIPANTE = " +
                                  " AND G.ID_GRAVEDAD = ")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function Registrar() As Boolean

        Dim USUARIOS = WebConfigurationManager.AppSettings("UsuarioSisan")
        Dim PASSWORDS = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncSisan").ToString())

        Dim Registro As New wsSisanReg.RegistroExterno
        Dim credentialsS As System.Net.NetworkCredential = New System.Net.NetworkCredential(USUARIOS, PASSWORDS, "ADCONSAR")
        Registro.Credentials = credentialsS

        Return False

    End Function

    Public Shared Function ObtenerSancion(FolioSISAN As String) As DataTable

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT * from [dbo].[BSA_IRREGULARIDAD] WHERE T_FOLIO = '" + FolioSISAN + "'")

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerEstatus(IdEstatus As Integer) As String

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If
        data = conexion.ConsultarDT("SELECT * from [dbo].[BSA_C_STATUS_REGISTRO] WHERE I_ID_ESTATUS = " + IdEstatus.ToString())
        conexion.CerrarConexion()

        Return data.Rows(0)("T_DSC_STATUS")

    End Function

    Public Shared Function ObtenerObservaciones(IdIrregularida As Integer) As DataTable

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If
        data = conexion.ConsultarDT("SELECT T_ID_USUARIO_REMITENTE, T_OBSERVACIONES FROM [dbo].[BSA_HISTORIAL] WHERE i_id_irregularidad = " + IdIrregularida.ToString() + " order by i_id_historial desc")
        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerNombre(usuario As String) As String

        If usuario = String.Empty Then
            Return String.Empty
        End If

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If
        data = conexion.ConsultarDT("SELECT NOMBRE + ' ' + APELLIDOS FROM [dbo].[BDS_USUARIO] WHERE USUARIO  = '" + usuario + "'")
        conexion.CerrarConexion()

        Return data.Rows(0)(0)

    End Function

    Public Shared Function ObtenerDocumentos(IdIrregularida As Integer) As DataTable

        Dim conexion As Conexion.SQLServer = Nothing
        Dim iddata As DataTable
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        iddata = conexion.ConsultarDT("SELECT I_ID_OFICIO,ISNULL(CONVERT(varchar, F_FECH_ACUSE,103),'') AS F_FECH_ACUSE FROM [dbo].[BSA_DATOS_ENV_OF_SICOD] WHERE I_ID_IRREGULARIDAD = '" + IdIrregularida.ToString() + "'")

        data = conexion.ConsultarDT("SELECT I_ID_DOCUMENTO,I_ID_TIPO_DOCUMENTO,  T_NOMBRE_DOCUMENTO,  I_ID_IRREGULARIDAD FROM dbo.BSA_R_DOCUMENTO WHERE I_ID_IRREGULARIDAD = '" + IdIrregularida.ToString() + "' AND B_IS_SICOD = 1 ORDER BY I_ID_DOCUMENTO DESC")

        conexion.CerrarConexion()

        data.Columns.Add(New DataColumn("F_FECH_ACUSE", Type.GetType("System.String")))

        If iddata.Rows.Count > 0 Then
            For i As Integer = 0 To data.Rows.Count - 1
                Dim drfech As DataRow = data.Rows(i)
                Dim valorIncial As Integer
                Dim valorFinal As Integer

                valorIncial = iddata.Rows.Count

                'drFechaAcuse = iddata.Rows(i)("F_FECH_ACUSE")
                If (valorIncial <> valorFinal) Then
                    If (iddata.Rows(i)("F_FECH_ACUSE") Is Nothing) Then

                        If drfech("I_ID_TIPO_DOCUMENTO") = "1" Then  'Elimina la fila si el tipo d edocumento es 1'
                            drfech.Delete()
                        End If
                    Else
                        drfech("F_FECH_ACUSE") = iddata.Rows(i)("F_FECH_ACUSE")
                        If drfech("I_ID_TIPO_DOCUMENTO") = "1" Then  'Elimina la fila si el tipo d edocumento es 1'
                            drfech.Delete()
                        End If
                    End If
                End If


                valorFinal = valorFinal + 1
            Next
        Else
            For i As Integer = 0 To data.Rows.Count - 1
                Dim drfech As DataRow = data.Rows(i)
                drfech("F_FECH_ACUSE") = ""
                If drfech("I_ID_TIPO_DOCUMENTO") = "1" Then  'Elimina la fila si el tipo d edocumento es 1'
                    drfech.Delete()
                End If
            Next

        End If

        Return data

    End Function
End Class
