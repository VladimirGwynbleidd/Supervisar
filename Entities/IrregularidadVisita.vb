Public Class IrregularidadVisita
    Public Shared Function ObtenerTodas(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String
        query = "SELECT ROW_NUMBER() OVER(ORDER BY F_FECH_IRREGULARIDAD ASC) AS Row#, " &
                "I_ID_IRREGULARIDAD, N_ID_FOLIO, format(F_FECH_IRREGULARIDAD, 'dd/MM/yyyy', 'en-US' ) as F_FECH_IRREGULARIDAD, " &
                "I_ID_PROCESO, I_ID_SUBPROCESO, I_ID_CONDUCTA, " &
                "I_ID_IRREGULARIDADES, T_DSC_COMENTARIO " &
                "FROM BDS_R_VI_IRREGULARIDAD " &
                "Where N_ID_FOLIO = " & Folio
        data = conexion.ConsultarDT(query)
        conexion.CerrarConexion()

        data.Columns.Add("DESC_PROCESO")
        data.Columns.Add("DESC_SUBPROCESO")
        data.Columns.Add("DESC_CONDUCTA")
        data.Columns.Add("DESC_IRREGULARIDAD")
        'data.Columns.Add("DESC_PARTICIPANTE")
        'data.Columns.Add("DESC_GRAVEDAD")

        For index As Integer = 0 To data.Rows.Count - 1
            Dim descripciones As DataTable = ObtenerDescripciones(data(index)("I_ID_PROCESO"), data(index)("I_ID_SUBPROCESO"), data(index)("I_ID_CONDUCTA"), data(index)("I_ID_IRREGULARIDADES"))
            If descripciones.Rows.Count > 0 Then
                data(index)("DESC_PROCESO") = descripciones(0)("DESC_PROCESO")
                data(index)("DESC_SUBPROCESO") = descripciones(0)("DESC_SUBPROCESO")
                data(index)("DESC_CONDUCTA") = descripciones(0)("DESC_CONDUCTA")
                data(index)("DESC_IRREGULARIDAD") = descripciones(0)("DESC_IRREGULARIDAD")
            Else
                data(index)("DESC_PROCESO") = ""
                data(index)("DESC_SUBPROCESO") = ""
                data(index)("DESC_CONDUCTA") = ""

            End If
        Next

        data.AcceptChanges()

        Return data
    End Function
    Public Shared Function ObtenerDescripciones(Proceso As Integer, Subproceso As Integer, Conducta As Integer, irregularidad As Integer) As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If System.Web.Configuration.WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(System.Web.Configuration.WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(System.Web.Configuration.WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT P.DESC_PROCESO, SP.DESC_SUBPROCESO, C.DESC_CONDUCTA, I.DESC_IRREGULARIDAD, '' as DESC_PARTICIPANTE, '' as DSC_GRAVEDAD " +
                                    " FROM BSA_C_PROCESO P" +
                                    " JOIN BSA_C_SUBPROCESO SP ON P.ID_PROCESO=SP.ID_PROCESO" +
                                    " JOIN BSA_C_CONDUCTA C ON SP.ID_PROCESO = C.ID_PROCESO AND SP.ID_SUBPROCESO= C.ID_SUBPROCESO" +
                                    " JOIN BSA_C_IRREGULARIDAD I ON I.ID_PROCESO = C.ID_PROCESO And I.ID_SUBPROCESO= C.ID_SUBPROCESO And I.ID_CONDUCTA = C.ID_CONDUCTA" +
                                    " WHERE P.ID_PROCESO = " + Proceso.ToString() +
                                    " AND SP.ID_SUBPROCESO = " + Subproceso.ToString() +
                                    " AND C.ID_CONDUCTA = " + Conducta.ToString() +
                                    "AND I.ID_IRREGULARIDAD =" + irregularidad.ToString())
        conexion.CerrarConexion()
        Return data
    End Function
    Public Shared Function Guardar(lstCampos As List(Of String), lstValores As List(Of Object)) As Boolean
        Dim cnnConexion As New Conexion.SQLServer()
        Try
            cnnConexion.Insertar("BDS_R_VI_IRREGULARIDAD", lstCampos, lstValores)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function Actualizar(lstCampos As List(Of String), lstValores As List(Of Object), lstCamposCondicion As List(Of String), lstValoresCondicion As List(Of Object)) As Boolean
        Dim cnnConexion As New Conexion.SQLServer()
        Try
            cnnConexion.Actualizar("BDS_R_VI_IRREGULARIDAD", lstCampos, lstValores, lstCamposCondicion, lstValoresCondicion)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ExistenIrregularidades(Folio As Integer) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String
        Dim blnExisten As Boolean = False

        query = "SELECT count(*) FROM BDS_R_VI_IRREGULARIDAD " &
                "Where N_ID_FOLIO = " & Folio
        data = conexion.ConsultarDT(query)

        If data.Rows(0).Item(0).ToString > 0 Then
            blnExisten = True
        Else
            blnExisten = False
        End If

        conexion.CerrarConexion()

        Return blnExisten
    End Function

    Public Class GridView
        Public Property Numero As Integer
        Public Property Fecha As String
        Public Property Proceso As String
        Public Property Subproceso As String
        Public Property Conducta As String
        Public Property Participante As String
        Public Property Gravedad As String
        Public Property Comentarios As String
    End Class
End Class
