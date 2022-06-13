Public Class BitacoraVisita
    Public Shared Function ObtenerEntradas(Query As String) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try ' SE MODIFICO A LA CONSULTA
            Return conexion.ConsultarDT("SELECT BVE.F_FECH_REGISTRO as F_FECH_REGISTRO, BVE.T_ID_USUARIO as T_ID_USUARIO,BVE.T_DSC_COMENTARIOS as T_DSC_COMENTARIO
				  ,BVE.I_ID_PASO as I_ID_PASO,BPM.T_DSC_MOVIMIENTO as T_DSC_MOVIMIENTO,BPM.I_ID_MOVIMIENTO as I_ID_MOVIMIENTO 				 
				   FROM BDS_D_VS_ESTATUS_PASO AS BVE
				   INNER JOIN BDS_C_GR_PASOS_MOVIMIENTO_V17 AS BPM ON BPM.I_ID_MOVIMIENTO = BVE.I_ID_MOVIMIENTO 
                 " + Query + " ORDER BY BVE.I_ID_MOVIMIENTO ")

            'Return conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_C_PC_BITACORA] " + Query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Shared Function ObtenerUsuarios(Visita As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try ' SE MODIFICO A LA CONSULTA
         Return conexion.ConsultarDT("SELECT DISTINCT([T_ID_USUARIO]) FROM [dbo].[BDS_D_VS_ESTATUS_PASO] WHERE [I_ID_VISITA] =" + Visita.ToString() + " ORDER BY T_ID_USUARIO")
         'Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_USUARIO) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_USUARIO")
      Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Shared Function ObtenerPasos(Visita As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
         Return conexion.ConsultarDT("SELECT DISTINCT(CGPV.I_ID_PASO)  FROM  [dbo].[BDS_D_VS_PASOS_PROCESO_VISITA] AS PPV
                                         INNER JOIN [dbo].[BDS_C_GR_PASOS_V17] AS CGPV ON PPV.I_ID_PASO = CGPV.I_ID_PASO
                                         INNER JOIN [dbo].[BDS_D_VS_VISITA] AS BDVV ON PPV.I_ID_VISITA =BDVV.I_ID_VISITA 
                                         WHERE BDVV.I_ID_VISITA=" + Visita.ToString() + " ORDER BY I_ID_PASO")
         'Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_PASO) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_PASO")
      Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
   Public Shared Function ObtenerAccion(Folio As Integer) As DataTable
      Dim conexion As New Conexion.SQLServer
      Try
         'QUEDA PENDIENTE EL CAMPO DE DESCRIPCION T_DSC_ACCION -OJOS-
         Return conexion.ConsultarDT("SELECT DISTINCT(BPM.T_DSC_MOVIMIENTO) as T_DSC_MOVIMIENTO FROM BDS_D_VS_ESTATUS_PASO AS BVE INNER JOIN BDS_C_GR_PASOS_MOVIMIENTO_V17 AS BPM ON BPM.I_ID_MOVIMIENTO = BVE.I_ID_MOVIMIENTO WHERE I_ID_VISITA=" + Folio.ToString() + " ORDER BY BPM.T_DSC_MOVIMIENTO")
         'Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_ACCION) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_ACCION")
      Catch ex As Exception
         Throw ex
      Finally
         If Not IsNothing(conexion) Then
            conexion.CerrarConexion()
         End If
      End Try
   End Function

   Public Shared Function ObtenerComentarios(Folio As Integer) As DataTable
      Dim conexion As New Conexion.SQLServer
      Try
            'QUEDA PENDIENTE EL CAMPO DE DESCRIPCION T_DSC_ACCION -OJOS-
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_COMENTARIOS) FROM [dbo].[BDS_D_VS_ESTATUS_PASO] WHERE T_ID_TIPO_COMENTARIO = 'USUARIO' AND I_ID_MOVIMIENTO >= 0 AND I_ID_VISITA=" + Folio.ToString() + "")
            'Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_ACCION) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_ACCION")
        Catch ex As Exception
         Throw ex
      Finally
         If Not IsNothing(conexion) Then
            conexion.CerrarConexion()
         End If
      End Try
   End Function

   Public Shared Sub AgregarEntrada(Folio As Integer, Usuario As String, Paso As String, Accion As String, Optional Comentarios As String = Nothing)
        Dim conexion As New Conexion.SQLServer
        'Dim query As String = "INSERT INTO [dbo].[BDS_V_VISITA_BITACORA] ([N_ID_FOLIO],[F_FECH_REGISTRO],[T_DSC_USUARIO],[T_DSC_PASO],[T_DSC_ACCION], [T_DSC_COMENTARIOS]) " &
        '"VALUES (" & Folio & ",GETDATE(),'" & Usuario & "','" & Paso & "','" & Accion & "', '" & Comentarios & "') SELECT 0 AS ExisteError"
        Dim query As String = "INSERT INTO [dbo].[BDS_C_PC_BITACORA] ([N_ID_FOLIO],[F_FECH_REGISTRO],[T_DSC_USUARIO],[T_DSC_PASO],[T_DSC_ACCION], [T_DSC_COMENTARIOS]) " &
            "VALUES (" & Folio & ",GETDATE(),'" & Usuario & "','" & Paso & "','" & Accion & "', '" & Comentarios & "') SELECT 0 AS ExisteError"
        Try
            conexion.ConsultarDT(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Sub
End Class
