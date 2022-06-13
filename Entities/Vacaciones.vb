'- Fecha de creación: 02/03/2014
'- Fecha de modificación:  ##/##/####
'- Nombre del Responsable: Rafael Rodriguez
'- Empresa: Softtek
'- Clase de Registro Agenda

Public Class Vacaciones

    Private Tabla As String = "BDS_D_GR_VACACIONES"

#Region "Propiedades"
    Public Property Id As Integer
    Public Property IdUsuario As String
    Public Property InicioPeriodo As Date
    Public Property FinPeriodo As Date
    Public Property DiasAsignados As Integer
    Public Property DiasConsumidos As Integer
    Public Property Vigente As Boolean
#End Region

#Region "Constructores"
    Sub New()

    End Sub

    Sub New(ByVal id As Integer, ByVal idUsuario As String, ByVal inicioPeriodo As Date, ByVal finPeriodo As Date,
                          ByVal diasAsignados As Integer, ByVal diasConsumidos As Integer, ByVal vigente As Boolean)
        Me.Id = id
        Me.IdUsuario = idUsuario
        Me.InicioPeriodo = inicioPeriodo
        Me.FinPeriodo = finPeriodo
        Me.DiasAsignados = diasAsignados
        Me.DiasConsumidos = diasConsumidos
        Me.Vigente = vigente

    End Sub

#End Region

#Region "Consultas"

    ''' <summary>
    ''' Obtiene una lista de las vacaciones vigentes y activas para un usuario
    ''' </summary>
    ''' <param name="idUsuario">Id de usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtnenPeriodosActivos(ByVal idUsuario As String) As List(Of Vacaciones)
        Dim lstObjVacaciones As New List(Of Vacaciones)

        Dim strQuery As String = "SELECT * FROM " + Tabla + _
                                " WHERE T_ID_USUARIO = '" + idUsuario + "' " + _
                                " AND GETDATE() BETWEEN F_FECH_INICIO_PERIODO AND F_FECH_FIN_PERIODO " + _
                                " AND B_FLAG_VIG = 1 " + _
                                " ORDER BY F_FECH_INICIO_PERIODO ASC "

        Dim conexion As Conexion.SQLServer = Nothing

        Try
            conexion = New Conexion.SQLServer()
            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstObjVacaciones.Add(CreaVacaciones(dr))
            Next

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return lstObjVacaciones
    End Function

#End Region

#Region "Persistencia"

    ''' <summary>
    ''' Actualiza un periodo de vacaiones en transaccion
    ''' </summary>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="tran">Objeto transaccion</param>
    ''' <param name="bitac">Objeto Bitacora</param>
    ''' <remarks></remarks>
    Public Sub ActualizaVacaciones(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, ByVal bitac As Conexion.Bitacora)
        Dim resultado As Boolean = False
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstCamposCondicion As New List(Of String)
        Dim lstValoresCondicion As New List(Of Object)

        lstCampos.Add("T_ID_USUARIO") : lstValores.Add(Me.IdUsuario)
        lstCampos.Add("F_FECH_INICIO_PERIODO") : lstValores.Add(Me.InicioPeriodo)
        lstCampos.Add("F_FECH_FIN_PERIODO") : lstValores.Add(Me.FinPeriodo)
        lstCampos.Add("N_NUM_DIAS_ASIGNADOS") : lstValores.Add(Me.DiasAsignados)
        lstCampos.Add("N_NUM_DIAS_CONSUMIDOS") : lstValores.Add(Me.DiasConsumidos)
        lstCampos.Add("B_FLAG_VIG") : lstValores.Add(Me.Vigente)

        lstCamposCondicion.Add("N_ID_VACACIONES") : lstValoresCondicion.Add(Me.Id)

        Try
            resultado = con.ActualizarConTransaccion(Tabla, lstCampos, lstValores, lstCamposCondicion, lstValoresCondicion, tran)
            bitac.ActualizarConTransaccion(Tabla, lstCampos, lstValores, lstCamposCondicion, lstValoresCondicion, resultado, "")
        Catch ex As Exception
            resultado = False
            bitac.ActualizarConTransaccion(Tabla, lstCampos, lstValores, lstCamposCondicion, lstValoresCondicion, resultado, ex.Message)
            Throw ex
        End Try

    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Crea un objeto vacaciones dado un datarow de la tabla
    ''' </summary>
    ''' <param name="dr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreaVacaciones(ByVal dr As DataRow) As Vacaciones
        Return New Vacaciones(Convert.ToInt32(dr("N_ID_VACACIONES")),
                              dr.Item("T_ID_USUARIO").ToString,
                              Convert.ToDateTime(dr("F_FECH_INICIO_PERIODO")),
                              Convert.ToDateTime(dr("F_FECH_FIN_PERIODO")),
                              Convert.ToInt32(dr("N_NUM_DIAS_ASIGNADOS")),
                              Convert.ToInt32(dr("N_NUM_DIAS_CONSUMIDOS")),
                              Convert.ToBoolean(dr("B_FLAG_VIG")))
    End Function



#End Region

End Class
