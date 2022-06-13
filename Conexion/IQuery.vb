Imports System
Imports System.Data
Imports System.Data.SqlClient


Interface IQuery


    Function BuscarUnRegistro(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean

    Function BuscarUnRegistro(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function ConsultarRegistrosDR(ByVal tabla As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal campoOrder As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDS(ByVal tabla As String) As DataSet

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal campoOrder As String) As DataSet

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDT(ByVal tabla As String) As DataTable

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal campoOrder As String) As DataTable

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataSet

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataSet

    Function ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataTable

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataTable

    Function ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As SqlDataReader

    Function ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataSet

    Function ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String) As DataTable

    Function ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal transaccion As SqlTransaction) As DataTable

    Function ConsultarDR(ByVal pQuery As String) As SqlDataReader

    Function ConsultarDR(ByVal pQuery As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function ConsultarDS(ByVal pQuery As String) As DataSet

    Function ConsultarDS(ByVal pQuery As String, ByVal transaccion As SqlTransaction) As DataSet

    Function ConsultarDT(ByVal pQuery As String) As DataTable

    Function ConsultarDT(ByVal pQuery As String, ByVal transaccion As SqlTransaction) As DataTable


#Region "Métodos para insertar"

    Function Insertar(ByVal tabla As String, ByVal valores As String) As Boolean

    Function Insertar(ByVal tabla As String, ByVal valores As String, ByVal transaccion As SqlTransaction) As Boolean

    Function InsertarConTransaccion(ByVal tabla As String, ByVal valores As String) As Boolean

    Function Insertar(ByVal tabla As String, ByVal valores As List(Of Object)) As Boolean

    Function Insertar(ByVal tabla As String, ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function InsertarConTransaccion(ByVal tabla As String, ByVal valores As List(Of Object)) As Boolean

    Function Insertar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object)) As Boolean

    Function Insertar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function InsertarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object)) As Boolean

    Function Insertar(ByVal tabla As String, ByVal pCampo As String, ByVal pValor As String) As Integer

    Function Insertar(ByVal tabla As String, ByVal pCampo As String, ByVal pValor As String, ByVal transaccion As SqlTransaction) As Integer

    Function Insertar(ByVal pDataTable As DataTable, ByVal tabla As String, ByVal transaccion As SqlTransaction) As Boolean

    Function Insertar(ByVal pDataTable As DataTable, ByVal tabla As String) As Boolean

#End Region

#Region "Métodos para actualizar"

    Function Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object)) As Boolean

    Function ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object)) As Boolean

    Function ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean

    Function Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal pCampoCondicion As List(Of String), ByVal pValorCondicion As List(Of Object)) As Boolean

    Function Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal pCampoCondicion As List(Of String), ByVal pValorCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function Actualizar(ByVal tabla As String, ByVal pCampo As String, ByVal pValor As String) As Boolean

    Function Actualizar(ByVal tabla As String, ByVal pCampo As String, ByVal pValor As String, ByVal transaccion As SqlTransaction) As Boolean
#End Region

#Region "Métodos para eliminar"

    Function Eliminar(ByVal tabla As String) As Boolean

    Function Eliminar(ByVal tabla As String, ByVal transaccion As SqlTransaction) As Boolean

    Function EliminarConTransaccion(ByVal tabla As String) As Boolean

    Function Eliminar(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean

    Function Eliminar(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function EliminarConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean

#End Region

#Region "Métodos para ejecutar stored procedures"

    Function EjecutarSP(ByVal pNombreSP As String) As Boolean

    Function EjecutarSP(ByVal pNombreSP As String, ByVal transaccion As SqlTransaction) As Boolean

    Function EjecutarSPConTransaccion(ByVal pNombreSP As String) As Boolean

    Function EjecutarSPConsultaDR(ByVal pNombreSP As String) As SqlDataReader

    Function EjecutarSPConsultaDR(ByVal pNombreSP As String, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function EjecutarSPConsultaDS(ByVal pNombreSP As String) As DataSet

    Function EjecutarSPConsultaDS(ByVal pNombreSP As String, ByVal transaccion As SqlTransaction) As DataSet

    Function EjecutarSPConsultaDT(ByVal pNombreSP As String) As DataTable

    Function EjecutarSPConsultaDT(ByVal pNombreSP As String, ByVal transaccion As SqlTransaction) As DataTable

    Function EjecutarSP(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object)) As Boolean

    Function EjecutarSP(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As Boolean

    Function EjecutarSPConTransaccion(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object)) As Boolean

    Function EjecutarSP(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter) As Boolean

    Function EjecutarSP(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter, ByVal transaccion As SqlTransaction) As Boolean

    Function EjecutarSPConTransaccion(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter) As Boolean

    Function EjecutarSPConsultaDR(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object)) As SqlDataReader

    Function EjecutarSPConsultaDR(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As SqlDataReader

    Function EjecutarSPConsultaDS(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object)) As DataSet

    Function EjecutarSPConsultaDS(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As DataSet

    Function EjecutarSPConsultaDT(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object)) As DataTable

    Function EjecutarSPConsultaDT(ByVal pNombreSP As String, ByVal pParametros As Array, ByVal valores As List(Of Object), ByVal transaccion As SqlTransaction) As DataTable

    Function EjecutarSPConsultaDR(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter) As SqlDataReader

    Function EjecutarSPConsultaDR(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter, ByVal transaccion As SqlTransaction) As SqlDataReader

    Function EjecutarSPConsultaDS(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter) As DataSet

    Function EjecutarSPConsultaDS(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter, ByVal transaccion As SqlTransaction) As DataSet

    Function EjecutarSPConsultaDT(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter) As DataTable

    Function EjecutarSPConsultaDT(ByVal pNombreSP As String, ByVal pParametros() As SqlParameter, ByVal transaccion As SqlTransaction) As DataTable
#End Region


    Function Ejecutar(ByVal pQuery As String, ByVal transaccion As SqlTransaction) As Boolean

    Function Ejecutar(ByVal pQuery As String) As Boolean


End Interface
