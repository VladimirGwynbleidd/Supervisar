Public Class Checklist
    Public Shared Function Preguntas() As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "SELECT [I_ID_CHECKLIST],[T_DSC_PREGUNTA] FROM [dbo].[BDS_C_GR_CHECKLIST] WHERE B_FLAG_VIGENTE = 1 ORDER BY [I_ID_CHECKLIST]"
        Try
            Return conexion.ConsultarDT(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    Public Shared Function PreguntasFolio(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "SELECT A.* FROM (SELECT PCH.[I_ID_CHECKLIST],[T_DSC_PREGUNTA],CH.B_CHECK FROM [dbo].[BDS_C_GR_CHECKLIST] PCH "
        query += "INNER JOIN [dbo].[BDS_R_PC_CHECKLIST] CH ON PCH.I_ID_CHECKLIST=CH.I_ID_CHECKLIST AND CH.N_ID_FOLIO = " & Folio.ToString() & " "
        query += "WHERE B_FLAG_VIGENTE = 1) A "
        query += "UNION "
        query += "SELECT B.* FROM (SELECT 999 AS [I_ID_CHECKLIST], T_DSC_MOTIVO_NO AS [T_DSC_PREGUNTA], I_ID_PC_CUMPLE AS B_CHECK "
        query += "FROM [dbo].[BDS_D_PC_PROGRAMA_CORRECCION] WHERE N_ID_FOLIO = " & Folio.ToString() & ") B ORDER BY [I_ID_CHECKLIST] "

        Try
            Return conexion.ConsultarDT(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
End Class
