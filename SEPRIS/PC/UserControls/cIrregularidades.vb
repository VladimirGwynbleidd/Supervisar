Public Class cIrregularidades


    Public Shared Function GuardaIrregularidad(lstCampos As List(Of String), lstValores As List(Of Object)) As Boolean
        Dim Conexion As New Conexion.SQLServer()

        Try
            Conexion.Insertar("BDS_R_PC_IRREGULARIDAD", lstCampos, lstValores)
            Conexion.CerrarConexion()

            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function

    Public Shared Function GuardaCompletarIrregularidad(lstCampos As List(Of String), lstValores As List(Of Object), lstCamposCond As List(Of String), lstValoresCond As List(Of Object)) As Boolean
        Dim Conexion As New Conexion.SQLServer()

        Try
            Conexion.Actualizar("BDS_R_PC_IRREGULARIDAD", lstCampos, lstValores, lstCamposCond, lstValoresCond)
            Conexion.CerrarConexion()
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function


    Public Shared Function CargaDatosCompletar(IDIrregularidad As Integer) As String()
        Dim Conexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim strSql As String
        Dim aCampos As String()
        Dim FechaCorrecion As String
        Dim DescripcionCorrige As String
        Dim DSC_Corregido As String
        Dim Comentario As String
        Dim Completa As String

        strSql = "Select N_ID_FOLIO, F_FECH_IRREGULARIDAD, I_ID_PROCESO, I_ID_SUBPROCESO, I_ID_CONDUCTA, I_ID_IRREGULARIDAD_POR_SANCIONAR, " &
            "T_DSC_COMENTARIO, T_DSC_CORREGIDO, T_DSC_COMOCORRIGE, F_FECH_CORRECCION, T_DSC_COMENTARIO, B_COMPLETA from BDS_R_PC_IRREGULARIDAD Where I_ID_IRREGULARIDAD = " & IDIrregularidad
        dt = Conexion.ConsultarDT(strSql)

        If IsDBNull(dt.Rows(0).Item("F_FECH_CORRECCION")) Then
            FechaCorrecion = ""
        Else
            FechaCorrecion = dt.Rows(0).Item("F_FECH_CORRECCION").ToString()
        End If

        If IsDBNull(dt.Rows(0).Item("T_DSC_COMOCORRIGE")) Then
            DescripcionCorrige = ""
        Else
            DescripcionCorrige = dt.Rows(0).Item("T_DSC_COMOCORRIGE").ToString()
        End If

        If IsDBNull(dt.Rows(0).Item("T_DSC_CORREGIDO")) Then
            DSC_Corregido = ""
        Else
            DSC_Corregido = dt.Rows(0).Item("T_DSC_CORREGIDO").ToString()
        End If

        If IsDBNull(dt.Rows(0).Item("T_DSC_COMENTARIO")) Then
            Comentario = ""
        Else
            Comentario = dt.Rows(0).Item("T_DSC_COMENTARIO").ToString()
        End If

        If IsDBNull(dt.Rows(0).Item("B_COMPLETA")) Then
            Completa = ""
        Else
            Completa = dt.Rows(0).Item("B_COMPLETA").ToString()
        End If

        aCampos = {
                   dt.Rows(0).Item("N_ID_FOLIO").ToString,
                   dt.Rows(0).Item("F_FECH_IRREGULARIDAD").ToString.Substring(0, 10),
                   dt.Rows(0).Item("I_ID_PROCESO").ToString,
                   dt.Rows(0).Item("I_ID_SUBPROCESO").ToString,
                   dt.Rows(0).Item("I_ID_CONDUCTA").ToString,
                   dt.Rows(0).Item("I_ID_IRREGULARIDAD_POR_SANCIONAR").ToString,
                   dt.Rows(0).Item("T_DSC_COMENTARIO").ToString,
                   DescripcionCorrige,
                   FechaCorrecion,
                   DSC_Corregido,
                   Comentario,
                   Completa
                  }
        Conexion.CerrarConexion()
        Return aCampos

    End Function
End Class
