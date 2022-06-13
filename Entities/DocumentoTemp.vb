Imports System.Data.SqlClient
Public Class DocumentoTemp
    Private _I_ID_DOCUMENTO As Integer    
    Private _T_DSC_NOMBRE_ARCHIVO As String    

    Public Property I_ID_DOCUMENTO As System.Int32
        Get
            Return _I_ID_DOCUMENTO
        End Get

        Set(value As System.Int32)
            _I_ID_DOCUMENTO = value
        End Set
    End Property
    Public Property T_DSC_NOMBRE_ARCHIVO As System.String
        Get
            Return _T_DSC_NOMBRE_ARCHIVO
        End Get

        Set(value As System.String)
            _T_DSC_NOMBRE_ARCHIVO = value
        End Set
    End Property

    Public Function InsertarDocumentoTemporal() As Integer
        Dim con As New Conexion.SQLServer
        Dim idDocumentoTemp As Integer = 0
        Dim dataReader As SqlDataReader = Nothing
        Try
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_DOCUMENTO_TEMP")
            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)

            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_NOMBRE_ARCHIVO", T_DSC_NOMBRE_ARCHIVO))
            dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

            While dataReader.Read()
                idDocumentoTemp = Convert.ToInt32(dataReader("I_ID_DOCUMENTO"))
            End While

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarVisita", "")
        Finally
            If Not IsNothing(dataReader) Then
                dataReader.Close()
                dataReader = Nothing
            End If
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
        Return idDocumentoTemp
    End Function

    Sub EliminarDocumentosTemporales(idOficios As Integer, idActa As Integer)
        Dim con As New Conexion.SQLServer
        Dim idDocumentoTemp As Integer = 0        
        Try
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ELIMINA_DOCUMENTO_TEMP")
            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)

            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idOficios", idOficios))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idActa", idActa))
            con.EjecutarSP(sp, SqlParameters.ToArray)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarVisita", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try        
    End Sub

End Class
