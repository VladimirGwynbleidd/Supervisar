

' Fecha de creación: 29/07/2013
' Fecha de modificación: 
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' contiene los metodos que son parte del negocio del catalogo de Estatus


Public Class Estatus

    Public Property Identificador As String
    Public Property IdentificadorImagen As String
    Public Property Descripcion As String
    Public Property Existe As Boolean = False
    Public Property Vigente As Boolean = False

#Region "Constructores"
    Public Sub New()

    End Sub

    Public Sub New(ByVal idEstatus)
        Me.Identificador = idEstatus

        CargarDatos()
    End Sub
#End Region
  


    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_ESTATUS") : listValores.Add(Me.Identificador)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_ESTATUS", listCampos, listValores)

            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_ESTATUS", listCampos, listValores)

                If dr.Read() Then

                    Me.Descripcion = CStr(dr("T_DSC_ESTATUS"))
                    Me.IdentificadorImagen = CStr(dr("N_ID_IMAGEN"))
                    Me.Vigente = CBool(dr("N_FLAG_VIG"))

                End If

            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Sub

    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_ESTATUS) + 1) N_ID_ESTATUS FROM BDS_C_GR_ESTATUS")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_ESTATUS")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_ESTATUS"))
                End If

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function


    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataView
            ' dv = conexion.ConsultarRegistrosDT("BDS_C_GR_ESTATUS").DefaultView
            dv = conexion.ConsultarDT("SELECT E.N_ID_ESTATUS, E.T_DSC_ESTATUS, E.N_FLAG_VIG, I.T_DSC_RUTA_IMAGEN FROM BDS_C_GR_ESTATUS E JOIN dbo.BDS_C_GR_IMAGEN I ON I.N_ID_IMAGEN = E.N_ID_IMAGEN ").DefaultView
            dv.Sort = "N_ID_ESTATUS"
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    Public Shared Function ObtenerTodosSolicitud() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try
            Dim query As String = "SELECT N_ID_ESTATUS_SERVICIO, T_DSC_ESTATUS_SERVICIO FROM BDS_C_GR_ESTATUS_SERVICIO WHERE B_FLAG_ESTATUS_SOLICITUD = 1 " & _
                                  "AND N_ID_ESTATUS_SERVICIO NOT IN (1, 10) AND B_FLAG_VIG = 1"
            Dim dv As New DataTable
            dv = conexion.ConsultarDT(query)
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function


    Public Shared Function ObtenerTodosServicio() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            dv = conexion.ConsultarDT("SELECT N_ID_ESTATUS_SERVICIO, T_DSC_ESTATUS_SERVICIO FROM BDS_C_GR_ESTATUS_SERVICIO WHERE B_FLAG_ESTATUS_SOLICITUD = 0  AND B_FLAG_VIG = 1")
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Registra un ESTATUS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Registro de nueva estatus" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("N_ID_ESTATUS") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_ESTATUS") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.IdentificadorImagen)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_ESTATUS", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_ESTATUS", listCampos, listValores, resultado, "")

        Catch ex As Exception
            resultado = False
            bitacora.Insertar("BDS_C_GR_ESTATUS", listCampos, listValores, resultado, "Error al guardar estatus")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally

            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
            listValores = Nothing
            listCampos = Nothing
        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Actualiza un menú
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de estatus" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, _
                                              CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try

            listCampos.Add("T_DSC_ESTATUS") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.IdentificadorImagen)


            listCamposCondicion.Add("N_ID_ESTATUS") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_ESTATUS", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_ESTATUS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_ESTATUS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar menú")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Elimina un ESTATUS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar estatus" & "(" & Me.Identificador.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try
            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_ESTATUS") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_ESTATUS", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_ESTATUS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_ESTATUS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar menú")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If

        End Try

        Return resultado

    End Function
End Class
