Public Class Irregularidad



    Public Shared Function Guardar(lstCampos As List(Of String), lstValores As List(Of Object)) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Try
            conexion.Insertar("BDS_R_PC_IRREGULARIDAD", lstCampos, lstValores)
            conexion.CerrarConexion()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function Actualizar(lstCampos As List(Of String), lstValores As List(Of Object), lstCamposCondicion As List(Of String), lstValoresCondicion As List(Of Object)) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Try
            Conexion.Actualizar("BDS_R_PC_IRREGULARIDAD", lstCampos, lstValores, lstCamposCondicion, lstValoresCondicion)
            Conexion.CerrarConexion
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Shared Function ObtenerTodas(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String
        query = "Select ROW_NUMBER() OVER(ORDER BY F_FECH_IRREGULARIDAD ASC) As Row#,I.I_ID_IRREGULARIDAD, I.N_ID_FOLIO, format(I.F_FECH_IRREGULARIDAD, 'dd/MM/yyyy', 'en-US' ) as F_FECH_IRREGULARIDAD, I.I_ID_PROCESO, I.I_ID_SUBPROCESO, I.I_ID_CONDUCTA, I.I_ID_IRREGULARIDAD_POR_SANCIONAR, I.T_DSC_COMENTARIO,I.T_DSC_CORREGIDO,I.T_DSC_COMOCORRIGE,I.F_FECH_CORRECCION,I.B_COMPLETA, P.N_ID_SICOD FROM BDS_R_PC_IRREGULARIDAD I inner join BDS_D_PC_PROGRAMA_CORRECCION P  on P.N_ID_FOLIO=I.N_ID_FOLIO Where I.N_ID_FOLIO =  " & Folio.ToString()
        data = conexion.ConsultarDT(query)
        conexion.CerrarConexion()

        data.Columns.Add("DESC_PROCESO")
        data.Columns.Add("DESC_SUBPROCESO")
        data.Columns.Add("DESC_CONDUCTA")
        data.Columns.Add("DESC_IRREGULARIDAD")
        data.Columns.Add("DESC_PARTICIPANTE")
        data.Columns.Add("DESC_GRAVEDAD")

        For index As Integer = 0 To data.Rows.Count - 1
            Dim descripciones As DataTable = ObtenerDescripciones(data(index)("I_ID_PROCESO"), data(index)("I_ID_SUBPROCESO"), data(index)("I_ID_CONDUCTA"), data(index)("I_ID_IRREGULARIDAD_POR_SANCIONAR"))
            If descripciones.Rows.Count > 0 Then
                data(index)("DESC_PROCESO") = descripciones(0)("DESC_PROCESO")
                data(index)("DESC_SUBPROCESO") = descripciones(0)("DESC_SUBPROCESO")
                data(index)("DESC_CONDUCTA") = descripciones(0)("DESC_CONDUCTA")
                data(index)("DESC_IRREGULARIDAD") = descripciones(0)("DESC_IRREGULARIDAD")
                data(index)("DESC_PARTICIPANTE") = descripciones(0)("DESC_PARTICIPANTE")
                data(index)("DESC_GRAVEDAD") = descripciones(0)("DSC_GRAVEDAD")
            Else
                data(index)("DESC_PROCESO") = ""
                data(index)("DESC_SUBPROCESO") = ""
                data(index)("DESC_CONDUCTA") = ""
                data(index)("DESC_IRREGULARIDAD") = ""
                data(index)("DESC_PARTICIPANTE") = ""
                data(index)("DESC_GRAVEDAD") = ""

            End If


        Next

        data.AcceptChanges()


        Return data

    End Function

    Public Shared Function ObtenerCompletas(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String
        query = "SELECT CASE WHEN COUNT (*) = (SELECT COUNT(*) " &
                "FROM [dbo].[BDS_R_PC_IRREGULARIDAD] " &
                "where N_ID_FOLIO = " & Folio & " AND B_COMPLETA = 1) THEN 'Si' ELSE 'No' END AS RES " &
                "FROM [dbo].[BDS_R_PC_IRREGULARIDAD] where N_ID_FOLIO = " & Folio

        data = conexion.ConsultarDT(query)
        conexion.CerrarConexion()

        Return data

    End Function


    Public Shared Function ObtenerDescripciones(Proceso As Integer, Subproceso As Integer, Conducta As Integer, Irregularidad As Integer) As DataTable
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If System.Web.Configuration.WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(System.Web.Configuration.WebConfigurationManager.AppSettings("DesaSISAN").ToString())
        Else
            conexion = New Conexion.SQLServer(System.Web.Configuration.WebConfigurationManager.AppSettings("ProdSISAN").ToString())
        End If

        data = conexion.ConsultarDT("SELECT P.DESC_PROCESO, SP.DESC_SUBPROCESO, C.DESC_CONDUCTA, I.DESC_IRREGULARIDAD, PA.DESC_PARTICIPANTE, G.DSC_GRAVEDAD " +
                                    " FROM BSA_C_PROCESO P" +
                                    " JOIN BSA_C_SUBPROCESO SP ON P.ID_PROCESO=SP.ID_PROCESO" +
                                    " JOIN BSA_C_CONDUCTA C ON SP.ID_PROCESO = C.ID_PROCESO AND SP.ID_SUBPROCESO= C.ID_SUBPROCESO" +
                                    " JOIN BSA_C_IRREGULARIDAD I ON I.ID_PROCESO = C.ID_PROCESO And I.ID_SUBPROCESO= C.ID_SUBPROCESO And I.ID_CONDUCTA = C.ID_CONDUCTA" +
                                    " JOIN BSA_C_PARTICIPANTE PA ON I.ID_PARTICIPANTE = PA.ID_PARTICIPANTE" +
                                    " JOIN BDS_C_GRAVEDAD G ON I.ID_GRAVEDAD = G.ID_GRAVEDAD" +
                                    " WHERE P.ID_PROCESO = " + Proceso.ToString() +
                                    " AND SP.ID_SUBPROCESO = " + Subproceso.ToString() +
                                    " AND C.ID_CONDUCTA = " + Conducta.ToString() +
                                    " AND I.ID_IRREGULARIDAD = " + Irregularidad.ToString())

        conexion.CerrarConexion()

        Return data

    End Function
    Public Shared Function EliminarIrregularidad(lstCampos As List(Of String), lstValores As List(Of Object))
        Dim conexion As New Conexion.SQLServer()
        Try
            conexion.Eliminar("BDS_R_PC_IRREGULARIDAD", lstCampos, lstValores)
            conexion.CerrarConexion()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Class GridView
        Public Property IdIrregularidad As Integer
        Public Property Numero As Integer
        Public Property Fecha As String
        Public Property Proceso As String
        Public Property Subproceso As String
        Public Property Conducta As String
        Public Property Irregularidad As String
        Public Property Participante As String
        Public Property Gravedad As String
        Public Property Comentarios As String
    End Class

End Class
