Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Diagnostics
Imports System.Text

Public Class SQLServer

    Private Conx As New SqlConnection
    Public Property ObtenerQuery As String

    Public Sub New()

        Dim strConx As String

        Try
            If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                strConx = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("DesaSeprisDSNenc"))
            Else
                strConx = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("ProdSeprisDSNenc"))
            End If
            Dim arrConx As Array
            arrConx = strConx.Split(CChar(";"))
            strConx = String.Empty
            For count As Integer = 0 To arrConx.Length - 1
                If Not arrConx.GetValue(count).ToString.Contains("Driver") Then
                    If Not arrConx.GetValue(count).ToString.Contains("Address") Then
                        strConx = strConx & arrConx.GetValue(count).ToString & ";"
                    End If
                End If
            Next
            strConx = strConx.Substring(0, strConx.Length - 1)
            Conx.ConnectionString = strConx
            Conx.Open()
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en el método New de la clase ConexionSql.", ex)
        End Try
    End Sub

    Public Sub New(ByVal cadenaConexion As String)

        Dim strConx As String
        Try
            strConx = Utilerias.Cifrado.DescifrarAES(cadenaConexion)
            Dim arrConx As Array
            arrConx = strConx.Split(CChar(";"))
            strConx = String.Empty
            For count As Integer = 0 To arrConx.Length - 1
                If Not arrConx.GetValue(count).ToString.Contains("Driver") Then
                    If Not arrConx.GetValue(count).ToString.Contains("Address") Then
                        strConx = strConx & arrConx.GetValue(count).ToString & ";"
                    End If
                End If
            Next
            strConx = strConx.Substring(0, strConx.Length - 1)
            Conx.ConnectionString = strConx
            Conx.Open()
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en el método New de la clase ConexionSql.", ex)
        End Try
    End Sub

    Public Sub CerrarConexion()
        If Not Conx Is Nothing Then
            If Conx.State = ConnectionState.Open Then
                Try
                    Conx.Close()
                Catch ex As Exception
                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
                    Throw New Exception("Ocurrió un error en el método CerrarConexion de la clase ConexionSql.", ex)
                End Try
            End If
        End If
    End Sub

    Public Function EstadoConexion() As Boolean
        Dim estado As Boolean = False
        Try
            If Conx.State = ConnectionState.Open Then
                estado = True
            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EstadoConexion de la clase ConexionSql.", ex)
        End Try
        Return estado
    End Function

    Public Function BeginTransaction() As SqlTransaction
        Dim tran As SqlTransaction = Nothing
        Try
            tran = Conx.BeginTransaction
            Return tran
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función BeginTransaction de la clase ConexionSql.", ex)
        End Try
        Return tran
    End Function

    Public Function Columnas(ByVal tabla As String) As String
        Dim strColumnas As String = String.Empty
        Dim query As String = String.Format("sp_columns {0}", tabla)

        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandTimeout = 0
                Using dr As SqlDataReader = cmm.ExecuteReader
                    If dr.HasRows Then
                        While dr.Read()
                            strColumnas += dr.Item(3).ToString & " ,"
                        End While
                    End If
                End Using
            End Using
            strColumnas = strColumnas.Substring(0, strColumnas.Length - 1)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función Columnas de la clase ConexionSql.", ex)
        End Try
        Return strColumnas

    End Function

    Public Function ColumnasConTransaccion(ByVal tabla As String, ByVal transaccion As SqlTransaction) As String
        Dim strColumnas As String = String.Empty
        Dim query As String = String.Format("sp_columns {0}", tabla)

        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandTimeout = 0
                Using dr As SqlDataReader = cmm.ExecuteReader
                    If dr.HasRows Then
                        While dr.Read()
                            strColumnas += dr.Item(3).ToString & " ,"
                        End While
                    End If
                End Using
            End Using
            strColumnas = strColumnas.Substring(0, strColumnas.Length - 1)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función Columnas de la clase ConexionSql.", ex)
        End Try
        Return strColumnas

    End Function

#Region "Consultas"

    Public Function RegresaFecha() As DateTime
        Dim dr As SqlDataReader = Nothing
        Dim fecha As DateTime
        Try
            Using cmm As New SqlCommand("SELECT Getdate()", Conx)
                cmm.CommandTimeout = 0
                dr = cmm.ExecuteReader
                While dr.Read
                    fecha = CType(dr(0), DateTime)
                End While
                dr.Close()
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD", "")
            Throw New Exception("Ocurrió un error en la función RegresaFecha de la clase ConexionSql.", ex)
        End Try
        Return fecha

    End Function

    Public Function RegresaFecha(ByVal transaccion As SqlTransaction) As DateTime
        Dim dr As SqlDataReader = Nothing
        Dim fecha As DateTime
        Try

            Using cmm As New SqlCommand("SELECT Getdate()", Conx)
                cmm.CommandTimeout = 0
                cmm.Transaction = transaccion
                dr = cmm.ExecuteReader
                While dr.Read
                    fecha = CType(dr(0), DateTime)
                End While
                dr.Close()
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD", "")
            Throw New Exception("Ocurrió un error en la función RegresaFecha de la clase ConexionSql.", ex)
        End Try
        Return fecha

    End Function

    Public Function BuscarUnRegistro(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim resultado As Boolean

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return False
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        ObtenerQuery = query

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.HasRows Then
                        resultado = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD BuscarUnRegistro", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función BuscarUnRegistro de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function BuscarUnRegistroConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim resultado As Boolean

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return False
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using dr As SqlDataReader = cmd.ExecuteReader
                    If dr.HasRows Then
                        resultado = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD BuscarUnRegistro", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función BuscarUnRegistro de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function ConsultarRegistrosDR(ByVal tabla As String) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Then
            Return dr
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ", tabla)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Then
            Return dr
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ", tabla)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDR(ByVal tabla As String, ByVal campoOrder As String) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Then
            Return dr
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ORDER BY {1} ", tabla, campoOrder)
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Then
            Return dr
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ORDER BY {1} ", tabla, campoOrder)
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDS(ByVal tabla As String) As DataSet
        Dim ds As New DataSet

        If tabla = String.Empty Then
            Return ds
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ", tabla)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim ds As New DataSet

        If tabla = String.Empty Then
            Return ds
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ", tabla)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDS(ByVal tabla As String, ByVal campoOrder As String) As DataSet
        Dim ds As New DataSet

        If tabla = String.Empty Then
            Return ds
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ORDER BY {1}", tabla, campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim ds As New DataSet

        If tabla = String.Empty Then
            Return ds
            Exit Function
        End If
        Dim query As String = String.Format("SELECT * FROM {0} ORDER BY {1}", tabla, campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDT(ByVal tabla As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(tabla)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(tabla, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function

    Public Function ConsultarRegistrosDT(ByVal tabla As String, ByVal campoOrder As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(tabla, campoOrder)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(tabla, campoOrder, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function

    Public Function ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)
        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)
        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataSet
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim ds As New DataSet
        strQuery.Append(tabla & " WHERE ")

        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim ds As New DataSet
        strQuery.Append(tabla & " WHERE ")

        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataSet
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim ds As New DataSet

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)
        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuery As New StringBuilder("SELECT * FROM ")
        Dim ds As New DataSet

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        strQuery.Append(tabla & " WHERE ")
        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)
        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing
        Try
            ds = ConsultarRegistrosDS(tabla, camposCondicion, valoresCondicion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing
        Try
            ds = ConsultarRegistrosDSConTransaccion(tabla, camposCondicion, valoresCondicion, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(tabla, camposCondicion, valoresCondicion, campoOrder)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(tabla, camposCondicion, valoresCondicion, campoOrder, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla
        querySelect = querySelect & String.Format(" ORDER BY {0}", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla
        querySelect = querySelect & String.Format(" ORDER BY {0}", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                cmd.Transaction = transaccion
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla
        querySelect = querySelect & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla
        querySelect = querySelect & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(campos, tabla)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(campos, tabla, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(campos, tabla, campoOrder)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(campos, tabla, campoOrder, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla


        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query
        End If

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query

        End If

        querySelect = querySelect & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query

        End If

        querySelect = querySelect & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query
        End If

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla

        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query
        End If

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla


        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query
        End If
        querySelect = querySelect & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuerySelect As New StringBuilder("SELECT ")
        Dim ds As New DataSet

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        For cic As Integer = 0 To campos.Count - 1
            strQuerySelect.Append(campos(cic).ToString & ", ")
        Next
        Dim querySelect As String = strQuerySelect.ToString
        querySelect = querySelect.Substring(0, querySelect.Length - 2) & " FROM " & tabla


        If camposCondicion.Count > 0 Then
            Dim strQuery As New StringBuilder(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            Dim query As String = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
            querySelect = querySelect & query
        End If
        querySelect = querySelect & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(querySelect, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(campos, tabla, camposCondicion, valoresCondicion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(campos, tabla, camposCondicion, valoresCondicion, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(campos, tabla, camposCondicion, valoresCondicion, campoOrder)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos.Count = 0 Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(campos, tabla, camposCondicion, valoresCondicion, campoOrder, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)


        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)


        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)


        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If

        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDRConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim strQuery As New StringBuilder("SELECT ")
        Dim dr As SqlDataReader = Nothing
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dr
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)


        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If

        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                dr = cmd.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDR", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataSet
        Dim strQuery As New StringBuilder("SELECT ")
        Dim ds As New DataSet
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)

        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuery As New StringBuilder("SELECT ")
        Dim ds As New DataSet
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)

        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataSet
        Dim strQuery As New StringBuilder("SELECT ")
        Dim ds As New DataSet
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)

        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If
        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDSConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim strQuery As New StringBuilder("SELECT ")
        Dim ds As New DataSet
        Dim query As String = String.Empty

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return ds
            Exit Function
        End If

        strQuery.Append(campos & " FROM " & tabla)

        If camposCondicion.Count > 0 Then
            strQuery.Append(" WHERE ")
            For count As Integer = 0 To camposCondicion.Count - 1
                strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
            Next
            query = strQuery.ToString
            query = query.Substring(0, query.Length - 4)
        Else
            query = strQuery.ToString
        End If
        query = query & String.Format(" ORDER BY {0} ", campoOrder)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDS", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(campos, tabla, camposCondicion, valoresCondicion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(campos, tabla, camposCondicion, valoresCondicion, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDS(campos, tabla, camposCondicion, valoresCondicion, campoOrder)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarRegistrosDTConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        If tabla = String.Empty Or campos = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return dt
            Exit Function
        End If

        Try
            ds = ConsultarRegistrosDSConTransaccion(campos, tabla, camposCondicion, valoresCondicion, campoOrder, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD ConsultarRegistrosDT", "")
            Throw New Exception("Ocurrió un error en la función ConsultarRegistrosDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarDR(ByVal query As String) As SqlDataReader
        Dim dr As SqlDataReader = Nothing
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.CommandTimeout = 0
                dr = cmm.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD Consultar", "")
            Throw New Exception("Ocurrió un error en la función ConsultarDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarDRConTransaccion(ByVal query As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim dr As SqlDataReader = Nothing
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                dr = cmm.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD Consultar", "")
            Throw New Exception("Ocurrió un error en la función ConsultarDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function ConsultarDS(ByVal query As String) As DataSet
        Dim ds As New DataSet
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD Consultar", "")
            Throw New Exception("Ocurrió un error en la función ConsultarDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarDSConTransaccion(ByVal query As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim ds As New DataSet
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD Consultar", "")
            Throw New Exception("Ocurrió un error en la función ConsultarDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function ConsultarDT(ByVal query As String) As DataTable
        Dim dt As DataTable = Nothing
        Dim ds As New DataSet

        Try
            ds = ConsultarDS(query)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD Consultar", "")
            Throw New Exception("Ocurrió un error en la función ConsultarDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function ConsultarDTConTransaccion(ByVal query As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim dt As DataTable = Nothing
        Dim ds As New DataSet

        Try
            ds = ConsultarDSConTransaccion(query, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD Consultar", "")
            Throw New Exception("Ocurrió un error en la función ConsultarDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

#End Region

#Region "Métodos para insertar"

    Public Function Insertar(ByVal tabla As String, ByVal valores As String) As Boolean
        Dim resultado As Boolean

        If tabla = String.Empty Or valores = String.Empty Then
            Return False
            Exit Function
        End If

        Dim query As String = String.Format("INSERT INTO {0} VALUES ({1})", tabla, valores)
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function InsertarConTransaccion(ByVal tabla As String, ByVal valores As String, ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean

        If tabla = String.Empty Or valores = String.Empty Then
            Return False
            Exit Function
        End If

        Dim query As String = String.Format("INSERT INTO {0} VALUES ({1})", tabla, valores)
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Insertar(ByVal tabla As String, ByVal valores As List(Of Object)) As Boolean
        Dim resultado As Boolean

        If tabla = String.Empty Or valores.Count = 0 Then
            Return False
            Exit Function
        End If

        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " VALUES (")

        For count As Integer = 0 To valores.Count - 1
            strQuery.Append(valores(count).ToString & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2) & ")"

        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function InsertarConTransaccion(ByVal tabla As String, ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean

        If tabla = String.Empty Or valores.Count = 0 Then
            Return False
            Exit Function
        End If

        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " VALUES (")

        For count As Integer = 0 To valores.Count - 1
            strQuery.Append(valores(count).ToString & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2) & ")"

        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Insertar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object)) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " (")

        For reg As Integer = 0 To campos.Count - 1
            strQuery.Append(campos(reg).ToString & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2)
        query = query & ") VALUES ("

        Dim strValores As New StringBuilder

        For reg As Integer = 0 To valores.Count - 1
            strValores.Append("@" & campos(reg) & ", ")
        Next

        Dim queryValores As String = strValores.ToString
        queryValores = queryValores.Substring(0, queryValores.Length - 2) & ") select @@identity "
        query = query & queryValores

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try


        Return resultado
    End Function

    Public Function InsertarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " (")

        For reg As Integer = 0 To campos.Count - 1
            strQuery.Append(campos(reg).ToString & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2)
        query = query & ") VALUES ("

        Dim strValores As New StringBuilder

        For reg As Integer = 0 To valores.Count - 1
            strValores.Append("@" & campos(reg) & ", ")
        Next

        Dim queryValores As String = strValores.ToString
        queryValores = queryValores.Substring(0, queryValores.Length - 2) & ") select @@identity "
        query = query & queryValores

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha(transaccion).ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function InsertarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction, ByRef identity As Integer) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " (")

        For reg As Integer = 0 To campos.Count - 1
            strQuery.Append(campos(reg).ToString & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2)
        query = query & ") VALUES ("

        Dim strValores As New StringBuilder

        For reg As Integer = 0 To valores.Count - 1
            strValores.Append("@" & campos(reg) & ", ")
        Next

        Dim queryValores As String = strValores.ToString
        queryValores = queryValores.Substring(0, queryValores.Length - 2) & ") select @@identity "
        query = query & queryValores

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha(transaccion).ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next
                identity = CType(cmd.ExecuteScalar, Integer)
                resultado = True
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            identity = 0
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Insertar(ByVal tabla As String, ByVal campo As String, ByVal valor As String) As Integer
        Dim resultado As Integer

        If tabla = String.Empty Or campo = String.Empty Or valor = String.Empty Then
            Return 0
            Exit Function
        End If

        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " (")

        strQuery.Append(campo)

        Dim query As String = strQuery.ToString
        query = query & ") VALUES ("

        Dim strValores As New StringBuilder

        strValores.Append("@" & campo)

        Dim queryValores As String = strValores.ToString
        queryValores = queryValores & ")"
        query = query & queryValores
        query = query & "; SELECT CAST(scope_identity() AS int)"
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                If valor.ToUpper = "GETDATE()" Then
                    cmd.Parameters.Add("@" & campo.ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    cmd.Parameters.AddWithValue("@" & campo, valor)
                End If
                Return CType(cmd.ExecuteScalar, Integer)
            End Using

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = 0
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function InsertarConTransaccion(ByVal tabla As String, ByVal campo As String, ByVal valor As String, ByVal transaccion As SqlTransaction) As Integer
        Dim resultado As Integer

        If tabla = String.Empty Or campo = String.Empty Or valor = String.Empty Then
            Return 0
            Exit Function
        End If

        Dim strQuery As New StringBuilder("INSERT INTO " & tabla & " (")

        strQuery.Append(campo)

        Dim query As String = strQuery.ToString
        query = query & ") VALUES ("

        Dim strValores As New StringBuilder

        strValores.Append("@" & campo)

        Dim queryValores As String = strValores.ToString
        queryValores = queryValores & ")"
        query = query & queryValores
        query = query & "; SELECT CAST(scope_identity() AS int)"
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                If valor.ToUpper = "GETDATE()" Then
                    cmd.Parameters.Add("@" & campo.ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    cmd.Parameters.AddWithValue("@" & campo, valor)
                End If
                Return CType(cmd.ExecuteScalar, Integer)
            End Using

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            resultado = 0
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function InsertarConTransaccion(ByVal dataTable As DataTable, ByVal tabla As String, ByVal transaccion As SqlTransaction) As Boolean
        Dim exito As Boolean = True

        Dim bc As SqlBulkCopy = Nothing

        Try
            bc = New SqlBulkCopy(Conx, SqlBulkCopyOptions.TableLock, transaccion)
            bc.BulkCopyTimeout = 60 * 600
            bc.DestinationTableName = tabla
            bc.BatchSize = dataTable.Rows.Count
            bc.WriteToServer(dataTable)
            exito = True
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            exito = False
        Finally
            If Not bc Is Nothing Then
                bc.Close()
                bc = Nothing
            End If
        End Try

        Return exito
    End Function

    Function Insertar(ByVal dataTable As DataTable, ByVal tabla As String) As Boolean
        Dim exito As Boolean = True

        Dim bc As SqlBulkCopy = Nothing

        Try
            bc = New SqlBulkCopy(Conx)
            bc.BulkCopyTimeout = 60 * 600
            bc.DestinationTableName = tabla
            bc.BatchSize = dataTable.Rows.Count
            bc.WriteToServer(dataTable)
            exito = True
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Insertar", "")
            exito = False
        Finally
            If Not bc Is Nothing Then
                bc.Close()
                bc = Nothing
            End If
        End Try

        Return exito
    End Function

#End Region

#Region "Métodos para actualizar"


    Public Function ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        For Each field As String In campos
            strQuery.Append(field & " = @" & field & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha(transaccion).ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object)) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        For count As Integer = 0 To campos.Count - 1
            strQuery.Append(campos(count).ToString & " = @" & campos(count).ToString & ", ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2)
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        For count As Integer = 0 To campos.Count - 1
            strQuery.Append(campos(count).ToString & " = @" & campos(count).ToString & ", ")
        Next
        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 2)

        Dim strCondiciones As New StringBuilder(" WHERE ")
        For cic As Integer = 0 To camposCondicion.Count - 1
            strCondiciones.Append(camposCondicion(cic).ToString & " = @1" & camposCondicion(cic).ToString & " AND ")
        Next
        Dim queryCondiciones As String = strCondiciones.ToString
        queryCondiciones = queryCondiciones.Substring(0, queryCondiciones.Length - 4)

        query = query & queryCondiciones

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha(transaccion).ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next

                For i As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@1" & camposCondicion(i).ToString, valoresCondicion(i))
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function ActualizarReq(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal pCampoCondicion As List(Of String), ByVal pValorCondicion As List(Of Object)) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        For Each field As String In campos
            strQuery.Append(field & " = @" & field & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, strQuery.Length - 2)

        Dim strCondiciones As New StringBuilder(" WHERE 1=1 AND T_FOLIO_SICOD IS NULL ")

        For Each field As String In pCampoCondicion
            strCondiciones.Append(" AND " & field & " = @1" & field)
        Next


        Dim queryCondiciones As String = strCondiciones.ToString()
        query = query & queryCondiciones
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next

                For reg As Integer = 0 To pCampoCondicion.Count - 1
                    If pValorCondicion(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@1" & pCampoCondicion(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@1" & pCampoCondicion(reg).ToString, pValorCondicion(reg))
                    End If
                Next

                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal pCampoCondicion As List(Of String), ByVal pValorCondicion As List(Of Object)) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        For Each field As String In campos
            strQuery.Append(field & " = @" & field & ", ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, strQuery.Length - 2)

        Dim strCondiciones As New StringBuilder(" WHERE 1=1 ")

        For Each field As String In pCampoCondicion
            strCondiciones.Append(" AND " & field & " = @1" & field)
        Next


        Dim queryCondiciones As String = strCondiciones.ToString()
        query = query & queryCondiciones
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To campos.Count - 1
                    If valores(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@" & campos(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@" & campos(reg).ToString, valores(reg))
                    End If
                Next

                For reg As Integer = 0 To pCampoCondicion.Count - 1
                    If pValorCondicion(reg).ToString.ToUpper = "GETDATE()" Then
                        cmd.Parameters.Add("@1" & pCampoCondicion(reg).ToString, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                    Else
                        cmd.Parameters.AddWithValue("@1" & pCampoCondicion(reg).ToString, pValorCondicion(reg))
                    End If
                Next

                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Actualizar(ByVal tabla As String, ByVal campo As String, ByVal valor As String) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        strQuery.Append(campo & " = @" & campo)
        Dim query As String = strQuery.ToString
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                If valor.ToUpper = "GETDATE()" Then
                    cmd.Parameters.Add("@" & campo, SqlDbType.DateTime).Value = RegresaFecha().ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    cmd.Parameters.AddWithValue("@" & campo, valor)
                End If
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function ActualizarConTransaccion(ByVal tabla As String, ByVal campo As String, ByVal valor As String, ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("UPDATE " & tabla & " SET ")

        strQuery.Append(campo & " = @" & campo)
        Dim query As String = strQuery.ToString
        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                If valor.ToUpper = "GETDATE()" Then
                    cmd.Parameters.Add("@" & campo, SqlDbType.DateTime).Value = RegresaFecha(transaccion).ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    cmd.Parameters.AddWithValue("@" & campo, valor)
                End If
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Actualizar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Actualizar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function
#End Region

#Region "Métodos para eliminar"

    Public Function Eliminar(ByVal tabla As String) As Boolean
        If tabla = String.Empty Then
            Return False
            Exit Function
        End If
        Dim resultado As Boolean
        Dim query As String = String.Format("TRUNCATE TABLE {0} ", tabla)
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Eliminar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Eliminar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EliminarConTransaccion(ByVal tabla As String, ByVal transaccion As SqlTransaction) As Boolean
        If tabla = String.Empty Then
            Return False
            Exit Function
        End If
        Dim resultado As Boolean
        Dim query As String = String.Format("TRUNCATE TABLE {0} ", tabla)
        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Eliminar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Eliminar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function Eliminar(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean
        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return False
            Exit Function
        End If

        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("DELETE FROM " & tabla & " WHERE ")

        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Eliminar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Eliminar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EliminarConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        If tabla = String.Empty Or camposCondicion.Count = 0 Or valoresCondicion.Count = 0 Then
            Return False
            Exit Function
        End If

        Dim resultado As Boolean
        Dim strQuery As New StringBuilder("DELETE FROM " & tabla & " WHERE ")

        For count As Integer = 0 To camposCondicion.Count - 1
            strQuery.Append(camposCondicion(count).ToString & " = @" & camposCondicion(count).ToString & " AND ")
        Next

        Dim query As String = strQuery.ToString
        query = query.Substring(0, query.Length - 4)

        Try
            Using cmd As New SqlCommand(query, Conx)
                cmd.Transaction = transaccion
                cmd.CommandTimeout = 0
                For reg As Integer = 0 To camposCondicion.Count - 1
                    cmd.Parameters.AddWithValue("@" & camposCondicion(reg).ToString, valoresCondicion(reg))
                Next
                resultado = CType(cmd.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, Eliminar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Eliminar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function


#End Region

#Region "Métodos para ejecutar stored procedures"

    Public Function EjecutarSP(ByVal nombreSp As String) As Boolean
        Dim resultado As Boolean

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function
    Public Function EjecutarSP(ByVal nombreSp As String, ByVal nombreDocumento As String, ByVal fechaFirma As DateTime, ByVal selloDital As String, ByVal nombreDocumentoSharepoint As String, ByVal idUsuario As String) As Boolean
        Dim resultado As Boolean = False
        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.Add("@S_NOMBRE_DOCUMENTO", SqlDbType.VarChar).Value = nombreDocumento
                cmm.Parameters.Add("@D_FECHA_FIRMA", SqlDbType.DateTime).Value = fechaFirma
                cmm.Parameters.Add("@S_SELLO_DIGITAL", SqlDbType.VarChar).Value = selloDital
                cmm.Parameters.Add("@S_NOMBRE_DOC_SHAREPOINT", SqlDbType.VarChar).Value = nombreDocumentoSharepoint
                cmm.Parameters.Add("@T_ID_USUARIO", SqlDbType.VarChar).Value = idUsuario
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EjecutarSPConTransaccion(ByVal nombreSp As String, ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EjecutarSPConsultaDR(ByVal nombreSp As String) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                dr = cmm.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function EjecutarSPConsultaDRConTransaccion(ByVal nombreSp As String, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                dr = cmm.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function EjecutarSPConsultaDS(ByVal nombreSp As String) As DataSet
        Dim ds As New DataSet

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function EjecutarSPConsultaDSParametro_Sello(ByVal nombreSp As String, ByVal parametro As String) As DataSet
        Dim ds As New DataSet

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.Add("@SEDIGITAL", SqlDbType.VarChar).Value = parametro
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function
    Public Function EjecutarSPConsultaDSParametro_Id(ByVal nombreSp As String, ByVal parametro As Integer) As DataSet
        Dim ds As New DataSet
        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.Add("@ID_DOC", SqlDbType.Int).Value = parametro
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function
    Public Function EjecutarSPConsultaDSUsuario_Id(ByVal nombreSp As String, ByVal parametro As String) As DataSet
        Dim ds As New DataSet
        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar).Value = parametro
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function
    Public Function EjecutarSPConsultaDSConTransaccion(ByVal nombreSp As String, ByVal transaccion As SqlTransaction) As DataSet
        Dim ds As New DataSet

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function EjecutarSPConsultaDT(ByVal nombreSp As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDS(nombreSp)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function

    Public Function EjecutarSPConsultaDT_Sello(ByVal nombreSp As String, ByVal parametro As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDSParametro_Sello(nombreSp, parametro)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function
    Public Function EjecutarSPConsultaDT_Id(ByVal nombreSp As String, ByVal parametro As Integer) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDSParametro_Id(nombreSp, parametro)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function
    Public Function EjecutarSPConsultaUsuarioDT_Id(ByVal nombreSp As String, ByVal parametro As String) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDSUsuario_Id(nombreSp, parametro)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function
    Public Function EjecutarSPConsultaDTConTransaccion(ByVal nombreSp As String, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDSConTransaccion(nombreSp, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt

    End Function

    Public Function EjecutarSP(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object)) As Boolean
        Dim cmm As New SqlCommand
        Dim resultado As Boolean

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Parameters.Clear()
            For count As Integer = 0 To parametros.Count - 1
                cmm.Parameters.AddWithValue(parametros(count).ToString, valores(count))
            Next
            resultado = CType(cmm.ExecuteNonQuery, Boolean)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSP", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try

        Return resultado
    End Function

    Public Function EjecutarSPConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean
        Dim cmm As New SqlCommand
        Dim resultado As Boolean

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Transaction = transaccion
            cmm.Parameters.Clear()
            For count As Integer = 0 To parametros.Count - 1
                cmm.Parameters.AddWithValue(parametros(count).ToString, valores(count))
            Next
            resultado = CType(cmm.ExecuteNonQuery, Boolean)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSP", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try

        Return resultado
    End Function

    Public Function EjecutarSP(ByVal nombreSp As String, ByVal parametros() As SqlParameter) As Boolean
        Dim resultado As Boolean

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.AddRange(parametros)
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSP", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EjecutarSP(ByVal nombreSp As String, ByVal parametros() As SqlParameter, tran As SqlClient.SqlTransaction) As Boolean
        Dim resultado As Boolean

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Transaction = tran
                cmm.Parameters.AddRange(parametros)
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSP", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EjecutarSPConConexion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Transaction = transaccion
                cmm.Parameters.AddRange(parametros)
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSP", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

    Public Function EjecutarSPConsultaDR(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object)) As SqlDataReader
        Dim cmm As New SqlCommand
        Dim dr As SqlDataReader = Nothing

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Parameters.Clear()
            For count As Integer = 0 To parametros.Count - 1
                cmm.Parameters.AddWithValue(parametros(count).ToString, valores(count))
            Next
            dr = cmm.ExecuteReader
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConsultaDR", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDR de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try
        Return dr
    End Function

    Public Function EjecutarSPConsultaDRConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim cmm As New SqlCommand
        Dim dr As SqlDataReader = Nothing

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Transaction = transaccion
            cmm.Parameters.Clear()
            For count As Integer = 0 To parametros.Count - 1
                cmm.Parameters.AddWithValue(parametros(count).ToString, valores(count))
            Next
            dr = cmm.ExecuteReader
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConsultaDR", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDR de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try
        Return dr
    End Function

    Public Function EjecutarSPConsultaDS(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object)) As DataSet
        Dim cmm As New SqlCommand
        Dim ds As New DataSet

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Parameters.Clear()
            For count As Integer = 0 To parametros.Count - 1
                cmm.Parameters.AddWithValue(parametros(count).ToString, valores(count))
            Next
            Using da As New SqlDataAdapter(cmm)
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConsultaDS", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try
        Return ds
    End Function

    Public Function EjecutarSPConsultaDSConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet
        Dim cmm As New SqlCommand
        Dim ds As New DataSet

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Transaction = transaccion
            cmm.Parameters.Clear()
            For count As Integer = 0 To parametros.Count - 1
                cmm.Parameters.AddWithValue(parametros(count).ToString, valores(count))
            Next
            Using da As New SqlDataAdapter(cmm)
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConsultaDS", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try
        Return ds
    End Function

    Public Function EjecutarSPConsultaDT(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object)) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDS(nombreSp, parametros, valores)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConsultaDT", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function EjecutarSPConsultaDTConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing

        Try
            ds = EjecutarSPConsultaDSConTransaccion(nombreSp, parametros, valores, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConsultaDT", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function EjecutarSPConsultaDR(ByVal nombreSp As String, ByVal parametros() As SqlParameter) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.AddRange(parametros)
                dr = cmm.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSPConsultaDR", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function EjecutarSPConsultaDRConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal transaccion As SqlTransaction) As SqlDataReader
        Dim dr As SqlDataReader = Nothing

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Transaction = transaccion
                cmm.Parameters.AddRange(parametros)
                dr = cmm.ExecuteReader
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSPConsultaDR", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDR de la clase ConexionSql.", ex)
        End Try
        Return dr
    End Function

    Public Function EjecutarSPConsultaDS(ByVal nombreSp As String, ByVal parametros() As SqlParameter) As DataSet
        Dim ds As New DataSet

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Parameters.AddRange(parametros)
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSPConsultaDS", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function EjecutarSPConsultaDSConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal transaccion As SqlTransaction) As DataSet
        Dim ds As New DataSet

        Try
            Using cmm As New SqlCommand()
                cmm.CommandType = CommandType.StoredProcedure
                cmm.CommandText = nombreSp
                cmm.Connection = Conx
                cmm.CommandTimeout = 0
                cmm.Transaction = transaccion
                cmm.Parameters.AddRange(parametros)
                Using da As New SqlDataAdapter(cmm)
                    da.Fill(ds)
                End Using
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSPConsultaDS", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDS de la clase ConexionSql.", ex)
        End Try
        Return ds
    End Function

    Public Function EjecutarSPConsultaDT(ByVal nombreSp As String, ByVal parametros() As SqlParameter) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing
        Try
            ds = EjecutarSPConsultaDS(nombreSp, parametros)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSPConsultaDT", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function

    Public Function EjecutarSPConsultaDTConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal transaccion As SqlTransaction) As DataTable
        Dim ds As New DataSet
        Dim dt As DataTable = Nothing
        Try
            ds = EjecutarSPConsultaDSConTransaccion(nombreSp, parametros, transaccion)
            dt = ds.Tables(0)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos, EjecutarSPConsultaDT", "")
            Throw New Exception("Ocurrió un error en la función EjecutarSPConsultaDT de la clase ConexionSql.", ex)
        End Try
        Return dt
    End Function
#End Region


    Public Function EjecutarConTransaccion(ByVal query As String, ByVal transaccion As SqlTransaction) As Boolean
        Dim resultado As Boolean = False

        Try
            Using cmm As New SqlCommand(query, Conx)
                cmm.Transaction = transaccion
                cmm.CommandTimeout = 0
                resultado = CType(cmm.ExecuteNonQuery, Boolean)
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexión Sql Ejecutar", "")
            resultado = False
            Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
        End Try
        Return resultado
    End Function

   Public Function Ejecutar(ByVal query As String) As Boolean
      Dim resultado As Boolean = False

      Try
         Using cmm As New SqlCommand(query, Conx)
            cmm.CommandTimeout = 0
            resultado = CType(cmm.ExecuteNonQuery, Boolean)
         End Using
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(query + " - " + ex.ToString(), EventLogEntryType.Error, "Conexión Sql Ejecutar", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
      End Try
      Return resultado
   End Function

   Public Function EjecutarQuery(ByVal query As String) As String
      Dim resultado As String = ""

      Try
         Using cmm As New SqlCommand(query, Conx)
            cmm.CommandTimeout = 0
            resultado = CType(cmm.ExecuteScalar, String)
         End Using
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(query + " - " + ex.ToString(), EventLogEntryType.Error, "Conexión Sql Ejecutar", "")
         resultado = ""
         Throw New Exception("Ocurrió un error en la función Insertar de la clase ConexionSql.", ex)
      End Try
      Return resultado
   End Function

   Public Shared Function ObtenerDscPaso(ByVal parametro As String) As String

        Dim retorno As String = String.Empty

        Try
            Dim dt As DataTable = ObtenerDsc(parametro)
            If dt.Rows.Count > 0 Then
                retorno = dt.Rows(0)("T_DSC_PASO").ToString()
            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "SQLServer ObtenerDscPaso", "")
        End Try

        Return retorno

    End Function

    Public Shared Function ObtenerDsc(ByVal parametro As String) As DataTable

        Dim dt As DataTable = Nothing
        Dim con As Conexion.SQLServer = Nothing
        Try
            con = New Conexion.SQLServer
            dt = con.ConsultarDT("SELECT T_DSC_PASO FROM BDS_C_GR_PASOS_V17 WHERE I_ID_PASO = " + parametro)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "SQLServer ObtenerDsc", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try

        Return dt
    End Function

    Public Class Parametro

        Public Shared Function ObtenerValor(ByVal parametro As String) As String

            Dim retorno As String = String.Empty

            Try
                Dim dt As DataTable = ObtenerValores(parametro)
                If dt.Rows.Count > 0 Then
                    retorno = dt.Rows(0)("T_DSC_VALOR").ToString()
                End If
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Parametro ObtenerValor", "")
            End Try

            Return retorno

        End Function

        Public Shared Function ObtenerValores(ByVal parametro As String) As DataTable

            Dim dt As DataTable = Nothing
            Dim con As Conexion.SQLServer = Nothing
            Try
                con = New Conexion.SQLServer
                dt = con.ConsultarDT("SELECT T_DSC_VALOR FROM BDS_C_GR_PARAMETRO WHERE T_DSC_PARAMETRO = '" + parametro + "'")
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Parametro ObtenerValores", "")
            Finally
                If Not IsNothing(con) Then
                    con.CerrarConexion()
                    con = Nothing
                End If
            End Try

            Return dt
        End Function


        Public Shared Function ObtnerIdAreaPld(ByVal parametro As String) As Integer
            Dim lsValorParam As String = ""
            Dim liIdArea As Integer = 0

            lsValorParam = ObtenerValor(parametro)

            If (lsValorParam.Trim() <> "") Then
                If (Int32.TryParse(lsValorParam, liIdArea)) Then
                    Return liIdArea
                End If
            End If

            Utilerias.ControlErrores.EscribirEvento("No se encontro el id del area PLD, confirme en la table de areas y en el parametro ID_AREA_PLD.",
                                                    EventLogEntryType.Error, "Parametro ObtnerIdAreaPld", "")
            Return 34 ''SI NO SE ENCUENTRA UN VALOR RETORNAR EL ID DE OPERACIONES PARA QUE NO AFECTE EL FLUJO
        End Function

    End Class


    Public Function EjecutarSP(Of T)(ByVal nombreSp As String, ByVal item As T) As Boolean
        Dim cmm As New SqlCommand
        Dim resultado As Boolean

        Try
            cmm.Connection = Conx
            cmm.CommandTimeout = 0
            cmm.CommandText = nombreSp
            cmm.CommandType = CommandType.StoredProcedure
            cmm.Parameters.Clear()
            cmm.GetParameters(Of T)(item)

            resultado = CType(cmm.ExecuteNonQuery, Boolean)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion BD EjecutarSPConTransaccion", "")
            resultado = False
            Throw New Exception("Ocurrió  un error en la función EjecutarSPConTransaccion de la clase ConexionSql.", ex)
        Finally
            cmm.Dispose()
        End Try

        Return resultado
    End Function
End Class

