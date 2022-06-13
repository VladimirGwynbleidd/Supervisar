Public Class RequerimientoPC

    Property IDRequerimiento As Integer
    Property Folio As Integer
    Property FechaAcuse As Date
    Property FechaEstimada As Date
    Property FechaRealEntrega As Date
    Property Usuario As String

    Public Shared Sub ActualizarDatos(campos As List(Of String), valores As List(Of Object), iIDRequerimiento As Integer)
        Dim conexion As New Conexion.SQLServer()
        Dim CamposCond As New List(Of String)
        Dim valoresCond As New List(Of Object)

        CamposCond.Add("I_ID_REQUERIMIENTO")
        valoresCond.Add(iIDRequerimiento)

        conexion.Actualizar("BDS_R_PC_REQUERIMIENTOS", campos, valores, CamposCond, valoresCond)

        conexion.CerrarConexion()
    End Sub

    Public Shared Sub ActualizarFolioSICOD(campos As List(Of String), valores As List(Of Object), folio As Integer)
        Dim conexion As New Conexion.SQLServer()
        Dim CamposCond As New List(Of String)
        Dim valoresCond As New List(Of Object)

        CamposCond.Add("N_ID_FOLIO")
        valoresCond.Add(folio)

        conexion.ActualizarReq("BDS_R_PC_REQUERIMIENTOS", campos, valores, CamposCond, valoresCond)

        conexion.CerrarConexion()
    End Sub

    Public Shared Sub InsertarProrroga(campos As List(Of String), valores As List(Of Object))
        Dim conexion As New Conexion.SQLServer()

        conexion.Insertar("BDS_R_PC_REQUERIMIENTOS", campos, valores)

        conexion.CerrarConexion()
    End Sub

    Public Shared Function TraerDatos(IDRequerimiento As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim strQuery As String
        Dim dtDatos As New DataTable

        strQuery = "Select * from BDS_R_PC_REQUERIMIENTOS Where I_ID_REQUERIMIENTO = " & IDRequerimiento
        dtDatos = conexion.ConsultarDT(strQuery)
        Return dtDatos
    End Function

    Public Sub Agregar()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        campos.Add("N_ID_FOLIO")
        campos.Add("T_ID_USUARIO")
        campos.Add("I_ID_ESTATUS")

        valores.Add(Folio)
        valores.Add(Usuario)
        valores.Add(1)

        conexion.Insertar("BDS_R_PC_REQUERIMIENTOS", campos, valores)

        conexion.CerrarConexion()

    End Sub

    Public Shared Function ObtenerRequerimientos(iFolio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = "SELECT ROW_NUMBER() OVER(order by I_ID_REQUERIMIENTO asc) AS  R_REQUERIMIENTO,I_ID_REQUERIMIENTO, N_ID_FOLIO, F_FECH_ACUSE, F_FECH_ESTIMADA, F_FECH_REAL, " &
            "I_ID_ESTATUS, " &
            "CASE I_ID_ESTATUS WHEN 1 THEN 'REQUERIMIENTO' WHEN 2 THEN 'PRORROGA' " &
            "WHEN 3 THEN 'ENTREGADO' WHEN 4 THEN 'NO ENTREGADO' END DESC_ESTATUS, T_FOLIO_SICOD " &
            "FROM BDS_R_PC_REQUERIMIENTOS WHERE N_ID_FOLIO = " + iFolio.ToString

        data = conexion.ConsultarDT(query)

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerDatosRequerimiento(IDRequerimiento As Integer) As String()
        Dim cnnConexion As New Conexion.SQLServer()
        Dim dt As New DataTable
        Dim nID As Long
        Dim strSql As String
        Dim aCampos As String()

        strSql = "SELECT I_ID_REQUERIMIENTO, N_ID_FOLIO, F_FECH_ACUSE, F_FECH_ESTIMADA, F_FECH_REAL, " & _
            "I_ID_ESTATUS, " & _
            "CASE I_ID_ESTATUS WHEN 1 THEN 'REQUERIMIENTO' WHEN 2 THEN 'PRORROGA' " & _
            "WHEN 3 THEN 'ENTREGADO' WHEN 4 THEN 'NO ENTREGADO' END DESC_ESTATUS " & _
            "FROM BDS_R_PC_REQUERIMIENTOS WHERE I_ID_REQUERIMIENTO = " & IDRequerimiento
        dt = cnnConexion.ConsultarDT(strSql)

        aCampos = {
                   dt.Rows(0).Item("I_ID_REQUERIMIENTO").ToString,
                   dt.Rows(0).Item("N_ID_FOLIO").ToString,
                   dt.Rows(0).Item("F_FECH_ACUSE").ToString,
                   dt.Rows(0).Item("F_FECH_ESTIMADA").ToString,
                   dt.Rows(0).Item("F_FECH_REAL").ToString,
                   dt.Rows(0).Item("I_ID_ESTATUS").ToString,
                   dt.Rows(0).Item("DESC_ESTATUS").ToString
                  }
        Return aCampos
    End Function

    Public Shared Function RequerimientosCompletos(iFolio As Integer) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String
        query = "SELECT 1 FROM BDS_R_PC_REQUERIMIENTOS WHERE  N_ID_FOLIO = " + iFolio.ToString + " and I_ID_ESTATUS IN (2,3,4)"
        data = conexion.ConsultarDT(query)
        conexion.CerrarConexion()

        If data.Rows.Count = 0 Then
            'No hay pendientes
            Return False
        Else
            Return True
        End If



    End Function
End Class
