Public Class Actividad



    Public Shared Function ObtenerTodas(FolioSICOD As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = "select ROW_NUMBER() OVER(ORDER BY F_FECH_INI_VIG ASC) AS Row#, a.N_ID_FOLIO_SICOD, a.I_ID_ACTIVIDAD, " &
                "A.T_DSC_ACTIVIDAD, A.T_ID_USUARIO, A.F_FECH_ENTREGA, A.I_ID_ESTATUS, AC.T_ID_USUARIO as T_ID_USUARIO_MOD, " &
                "AC.F_FECH_COMENTARIO, AP.F_FECH_ENTREGA AS F_FECH_ENTREGA_PRO FROM [BDS_D_PC_ACTIVIDAD] A " &
                "left join BDS_R_PC_ACTIVIDAD_COMENTARIO AC on A.I_ID_ACTIVIDAD=AC.I_ID_ACTIVIDAD " &
                "left join BDS_R_PC_ACTIVIDAD_PRORROGA AP on A.I_ID_ACTIVIDAD=AP.I_ID_ACTIVIDAD  " &
                "Where A.N_ID_FOLIO_SICOD = " & FolioSICOD & " order by ROW_NUMBER() OVER(ORDER BY F_FECH_INI_VIG ASC) ASC "

        data = conexion.ConsultarDT(query)

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function GuardaAltaActividad(lstCampos As List(Of String), lstValores As List(Of Object)) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Try
            conexion.Insertar("BDS_D_PC_ACTIVIDAD", lstCampos, lstValores)
            conexion.CerrarConexion()
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function

    Public Shared Function GuardaModifActividad(lstCampos As List(Of String), lstValores As List(Of Object), strNuevoEstatus As String) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Dim strQuery As String


        Try
            conexion.Insertar("BDS_R_PC_ACTIVIDAD_COMENTARIO", lstCampos, lstValores)
            strQuery = "Update BDS_D_PC_ACTIVIDAD set [I_ID_ESTATUS] = '" & strNuevoEstatus & "' Where I_ID_ACTIVIDAD = " & lstValores(0)
            conexion.Ejecutar(strQuery)
            conexion.CerrarConexion()
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function

    Public Shared Function GuardaAltaProrrogaActividad(lstCampos As List(Of String), lstValores As List(Of Object), strNuevoEstatus As String) As Boolean
        Dim Conexion As New Conexion.SQLServer()
        Dim strQuery As String

        Try
            Conexion.Insertar("BDS_R_PC_ACTIVIDAD_PRORROGA", lstCampos, lstValores)

            strQuery = "Update BDS_D_PC_ACTIVIDAD set [I_ID_ESTATUS] = '" & strNuevoEstatus & "' Where I_ID_ACTIVIDAD = " & lstValores(0)
            Conexion.Ejecutar(strQuery)
            Conexion.CerrarConexion()
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function

    Public Shared Function TraUltFolActividad(FolioSICOD As Integer)
        Dim Conexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim nID As Long
        Dim strSql As String

        strSql = "Select Max(I_ID_ACTIVIDAD) + 1 as UltFolAct from BDS_D_PC_ACTIVIDAD Where N_ID_FOLIO_SICOD = " & FolioSICOD
        dt = Conexion.ConsultarDT(strSql)
        nID = dt.Rows(0).Item("UltFolAct")
        Conexion.CerrarConexion()
        Return nID
    End Function

    Public Shared Function TraUltFolActividadComentario(FolioSICOD As Integer, IDActividad As Integer)
        Dim Conexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim nID As Long
        Dim strSql As String

        strSql = "Select Max([I_ID_COMENTARIO) + 1 as UltFolComent from BDS_R_PC_ACTIVIDAD_COMENTARIO Where N_ID_FOLIO_SICOD = " & _
            FolioSICOD & " and I_ID_ACTIVIDAD = " & IDActividad
        dt = Conexion.ConsultarDT(strSql)
        nID = dt.Rows(0).Item("UltFolComent")
        Conexion.CerrarConexion()
        Return nID
    End Function

    Public Shared Function TraUltFolActividadProrroga(FolioSICOD As Integer, IDActividad As Integer)
        Dim Conexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim nID As Long
        Dim strSql As String

        strSql = "Select Max([I_ID_PRORROGA) + 1 as UltFolProrroga from BDS_R_PC_ACTIVIDAD_PRORROGA Where N_ID_FOLIO_SICOD = " & _
            FolioSICOD & " and I_ID_ACTIVIDAD = " & IDActividad
        dt = Conexion.ConsultarDT(strSql)
        nID = dt.Rows(0).Item("UltFolProrroga")
        Conexion.CerrarConexion()
        Return nID
    End Function


    Public Shared Function CargaDatosModificacion(IDActividad As Integer) As String()
        Dim Conexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim strSql As String
        Dim aCampos As String()

        strSql = "Select T_DSC_ACTIVIDAD,CASE WHEN AP.F_FECH_ENTREGA IS NULL THEN A.F_FECH_ENTREGA ELSE AP.F_FECH_ENTREGA END as F_FECH_ENTREGA, A.I_ID_ACTIVIDAD, I_ID_ESTATUS " &
            "from BDS_D_PC_ACTIVIDAD A left join [dbo].[BDS_R_PC_ACTIVIDAD_PRORROGA] AP on A.I_ID_ACTIVIDAD=AP.I_ID_ACTIVIDAD Where A.I_ID_ACTIVIDAD = " & IDActividad
        dt = Conexion.ConsultarDT(strSql)
        aCampos = {dt.Rows(0).Item("T_DSC_ACTIVIDAD").ToString,
                   dt.Rows(0).Item("F_FECH_ENTREGA").ToString.Substring(0, 10),
                   dt.Rows(0).Item("I_ID_ACTIVIDAD").ToString,
                   dt.Rows(0).Item("I_ID_ESTATUS").ToString}
        Conexion.CerrarConexion()
        Return aCampos

    End Function

    Public Shared Function CargaDatosProrroga(IDActividad As Integer) As String()
        Dim Conexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim strSql As String
        Dim aCampos As String()

        strSql = "Select T_DSC_ACTIVIDAD, F_FECH_ENTREGA from BDS_D_PC_ACTIVIDAD Where I_ID_ACTIVIDAD = " & IDActividad
        dt = Conexion.ConsultarDT(strSql)
        aCampos = {dt.Rows(0).Item("T_DSC_ACTIVIDAD").ToString, dt.Rows(0).Item("F_FECH_ENTREGA").ToString.Substring(0, 10)}
        Conexion.CerrarConexion()
        Return aCampos

    End Function

End Class
