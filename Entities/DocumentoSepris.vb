Imports System.Data.SqlClient

Public Class DocumentoSepris
    Private _I_ID_REGISTRO As Integer
    Private _I_ID_VISITA As Integer
    Private _I_ID_PASO As Integer
    Private _T_DSC_NOMBRE_ARCHIVO As String
    Private _F_FECH_REGISTRO As DateTime
    Private _T_DSC_COMENTARIO As String
    Private _N_NUM_DOCUMENTO As Integer
    Private _N_FLAG_VIG As Integer
#Region "Propiedades"
    Public Property I_ID_REGISTRO As System.Int32
        Get
            Return _I_ID_REGISTRO
        End Get

        Set(value As System.Int32)
            _I_ID_REGISTRO = value
        End Set
    End Property
    Public Property I_ID_VISITA As System.Int32?
        Get
            Return _I_ID_VISITA
        End Get

        Set(value As System.Int32?)
            _I_ID_VISITA = value
        End Set
    End Property
    Public Property I_ID_PASO As System.Int32
        Get
            Return _I_ID_PASO
        End Get

        Set(value As System.Int32)
            _I_ID_PASO = value
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
    Public Property F_FECH_REGISTRO As System.DateTime
        Get
            Return _F_FECH_REGISTRO
        End Get

        Set(value As System.DateTime)
            _F_FECH_REGISTRO = value
        End Set
    End Property
    Public Property T_DSC_COMENTARIO As System.String
        Get
            Return _T_DSC_COMENTARIO
        End Get

        Set(value As System.String)
            _T_DSC_COMENTARIO = value
        End Set
    End Property
    Public Property N_NUM_DOCUMENTO As System.Int32
        Get
            Return _N_NUM_DOCUMENTO
        End Get

        Set(value As System.Int32)
            _N_NUM_DOCUMENTO = value
        End Set
    End Property
    Public Property N_FLAG_VIG As System.Int32
        Get
            Return _N_FLAG_VIG
        End Get

        Set(value As System.Int32)
            _N_FLAG_VIG = value
        End Set
    End Property
#End Region
    Public Sub New(ByVal idRegistro As Integer, ByVal idVisita As Integer, ByVal idPaso As Integer, ByVal nombreArchivo As String, ByVal numeroArchivo As Integer)
        Me._I_ID_REGISTRO = idRegistro
        Me.I_ID_VISITA = idVisita
        Me.I_ID_PASO = idPaso
        Me.T_DSC_NOMBRE_ARCHIVO = nombreArchivo
        Me.N_NUM_DOCUMENTO = numeroArchivo
    End Sub
    Public Sub New()

    End Sub
    Public Function InsertarDocumentoSepris() As Boolean
        Dim con As New Conexion.SQLServer
        Dim bitac As New Conexion.Bitacora("Reemplaza documento", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim resultado As Boolean = False

        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)

        Try
            campos.Add("I_ID_VISITA") : valores.Add(I_ID_VISITA)
            campos.Add("I_ID_PASO") : valores.Add(I_ID_PASO)
            campos.Add("T_DSC_NOMBRE_ARCHIVO") : valores.Add(T_DSC_NOMBRE_ARCHIVO)
            campos.Add("F_FECH_REGISTRO") : valores.Add(F_FECH_REGISTRO)
            campos.Add("T_DSC_COMENTARIO") : valores.Add(T_DSC_COMENTARIO)
            campos.Add("N_NUM_DOCUMENTO") : valores.Add(N_NUM_DOCUMENTO)
            campos.Add("N_FLAG_VIG") : valores.Add(N_FLAG_VIG)
            resultado = con.Insertar("BDS_D_VS_DOCUMENTOS_PASO", campos, valores)
            bitac.Actualizar("BDS_D_VS_DOCUMENTOS_PASO", campos, valores, resultado, "")

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Error al actualizar documento", EventLogEntryType.Error, "SEPRIS", "")
            resultado = False
            bitac.Actualizar("BDS_R_CONSULTA_DOCUMENTO", campos, valores, resultado, ex.ToString)
            Throw ex
        Finally
            Try : bitac.Finalizar(resultado) : Catch ex As Exception : End Try

            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try
        Return resultado
    End Function
    Public Function InsertarDocumentoSeprisInicio(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, Optional pbEsActualizacion As Boolean = False) As Boolean
        Dim bitac As New Conexion.Bitacora("Reemplaza documento", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim resultado As Boolean = False

        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)

        Try
            campos.Add("I_ID_VISITA") : valores.Add(I_ID_VISITA)
            campos.Add("I_ID_PASO") : valores.Add(I_ID_PASO)
            campos.Add("T_DSC_NOMBRE_ARCHIVO") : valores.Add(T_DSC_NOMBRE_ARCHIVO)
            campos.Add("F_FECH_REGISTRO") : valores.Add(F_FECH_REGISTRO)
            campos.Add("T_DSC_COMENTARIO") : valores.Add(T_DSC_COMENTARIO)
            campos.Add("N_NUM_DOCUMENTO") : valores.Add(N_NUM_DOCUMENTO)
            campos.Add("N_FLAG_VIG") : valores.Add(N_FLAG_VIG)

            If Not pbEsActualizacion Then
                resultado = con.InsertarConTransaccion("BDS_D_VS_DOCUMENTOS_PASO", campos, valores, tran)
            Else
                Dim camposCond As New List(Of String)
                Dim valoresCond As New List(Of Object)
                camposCond.Add("I_ID_VISITA") : valoresCond.Add(I_ID_VISITA)
                camposCond.Add("I_ID_PASO") : valoresCond.Add(I_ID_PASO)
                camposCond.Add("N_NUM_DOCUMENTO") : valoresCond.Add(N_NUM_DOCUMENTO)

                resultado = con.ActualizarConTransaccion("BDS_D_VS_DOCUMENTOS_PASO", campos, valores, camposCond, valoresCond, tran)
            End If

            bitac.Actualizar("BDS_D_VS_DOCUMENTOS_PASO", campos, valores, resultado, "")
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Error al actualizar documento", EventLogEntryType.Error, "SEPRIS", "")
            resultado = False
            bitac.Actualizar("BDS_R_CONSULTA_DOCUMENTO", campos, valores, resultado, ex.ToString)
            Throw ex
        Finally
            Try : bitac.Finalizar(resultado) : Catch ex As Exception : End Try
        End Try
        Return resultado
    End Function

    Public Function ObtenerDocumentosExpediente(ByVal idVisita As Integer) As List(Of DocumentoSepris)
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim consulta = " SELECT I_ID_REGISTRO, " & _
                            " I_ID_VISITA, " & _
                            " I_ID_PASO, " & _
                            " N_NUM_DOCUMENTO, " & _
                            " T_DSC_NOMBRE_ARCHIVO, " & _
                            " F_FECH_REGISTRO, " & _
                            " T_DSC_COMENTARIO " & _
                            " FROM dbo.BDS_D_VS_DOCUMENTOS_PASO " & _
                            " WHERE I_ID_VISITA =  " & idVisita & " AND N_FLAG_VIG = 1 ORDER BY N_NUM_DOCUMENTO , F_FECH_REGISTRO ASC"

            dv = conexion.ConsultarDT(consulta)
            If dv.Rows.Count > 0 Then
                Dim ListaDocumentos As New List(Of DocumentoSepris)
                For Each dr As DataRow In dv.Rows
                    ListaDocumentos.Add(New DocumentoSepris(CInt(dr("I_ID_REGISTRO")), CInt(dr("I_ID_VISITA")), CInt(dr("I_ID_PASO")), CStr(dr("T_DSC_NOMBRE_ARCHIVO")), CStr(dr("N_NUM_DOCUMENTO"))))
                Next
                Return ListaDocumentos
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return Nothing
    End Function
    Public Function ActualizaDocumentoInhabilita(ByVal id_visita As Integer, ByVal n_num_doc As Integer) As Boolean
        Dim registroExitoso As Boolean = True
        Dim con As Conexion.SQLServer = Nothing
        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ACTUALIZAR_INHABILITA_DOCUMENTO_EXPEDIENTE")
            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", id_visita))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@N_NUM_DOCUMENTO", n_num_doc))
            Dim dataReader As SqlDataReader
            dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)
            While dataReader.Read()
            End While

        Catch ex As Exception
            registroExitoso = False
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try
        Return registroExitoso
    End Function
End Class
