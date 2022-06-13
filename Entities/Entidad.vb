Public Class Entidad
    Public Shared Function ObtenerPorFolio(Folio As String) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT [N_ID_SUB_ENTIDAD] FROM [dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC]WHERE [N_ID_FOLIO]=" + Folio)
        Return data
    End Function


    Public Shared Function ObtenerSubFoliosPorFolio(Folio As String) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT [N_ID_RELACION], [I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD] FROM [dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC] WHERE [B_ISRESOLUCION] = 0 AND [N_ID_FOLIO]=" + Folio)
        Return data
    End Function

    Public Shared Function ObtenerSubFoliosPorFolioComplete(Folio As String) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT [N_ID_RELACION], [I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD] FROM [dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC] WHERE [N_ID_FOLIO]=" + Folio)
        Return data
    End Function

    Public Shared Function ObtenerSubFolios(Folio As String) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT [N_ID_SUB_ENTIDAD], [I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD] FROM [dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC] WHERE [N_ID_FOLIO]=" + Folio)
        Return data
    End Function

    Public Shared Function ValidarSubFoliosPorFolio(Folio As String) As Boolean

        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT [N_ID_SUB_ENTIDAD],[I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD] FROM [dbo].[BDS_R_PC_ENTIDAD_SUB_ENTIDAD_PC]WHERE [N_ID_FOLIO]=" + Folio)
        If (data.Tables(0).Rows.Count = 0) Then
            Return False
        Else
            Return True
        End If
    End Function
End Class
