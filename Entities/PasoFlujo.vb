'- Fecha de creación: 21/01/2014
'- Fecha de modificación:  NA
'- Nombre del Responsable: Iván Rivera Martiñón
'- Empresa: Softtek
'- Clase para Paso de Flujo

<Serializable()>
Public Class PasoFlujo
    Private Tabla As String = "BDS_C_GR_PASOS_SEGUIMIENTO"

#Region "Propiedades"

    Public Property Identificador As Integer
    Public Property Descripcion As String
    Public Property Orden As String = ""
    Public Property Estatus As EstatusServicio
    Public Property ColorActual As String
    Public Property ColorAnterior As String
    Public Property ColorPosterior As String
    Public Property ImagenVisible As Boolean
    Public Property ImagenInactiva As Imagen
    Public Property Imagen As Imagen
    Public Property Vigente As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date?
    Public Property Existe As Boolean = False

#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idPaso As Integer)
        Me.Identificador = idPaso

        CargarDatos()
    End Sub


#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los datos de Función tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_PASO") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(Tabla, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(Tabla, listCampos, listValores)

                    If dr.Read() Then
                        Me.Descripcion = CStr(dr("T_DSC_PASO_TOOLTIP"))
                        Me.ColorActual = CStr(dr("T_DSC_COLOR_HEX_ACTUAL"))
                        Me.ColorAnterior = CStr(dr("T_DSC_COLOR_HEX_ANTES"))
                        Me.ColorPosterior = CStr(dr("T_DSC_COLOR_HEX_POSTERIOR"))
                        Me.Vigente = Convert.ToBoolean(dr("B_FLAG_VIG"))
                        Me.InicioVigencia = Convert.ToDateTime(dr("F_FECH_INI_VIG"))
                        Me.Estatus = New EstatusServicio(CInt(dr("N_ID_ESTATUS_SERVICIO")))
                        Me.ImagenVisible = Convert.ToBoolean(dr("B_FLAG_VISIBLE"))
                        Me.Imagen = New Imagen(CInt(dr("N_ID_IMAGEN")))
                        Me.ImagenInactiva = New Imagen(CInt(dr("N_ID_IMAGEN_INACTIVA")))

                        If Not IsDBNull(dr("F_FECH_FIN_VIG")) Then
                            Me.FinVigencia = Convert.ToDateTime(dr("F_FECH_FIN_VIG"))
                        Else
                            Me.FinVigencia = Nothing
                        End If

                    End If

                End If
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Finally
                If dr IsNot Nothing Then
                    If Not dr.IsClosed Then
                        dr.Close() : dr = Nothing
                    End If
                End If
            End Try

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Sub

    ''' <summary>
    ''' Obtiene todos los registros del catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer
        Dim sqlQuery As String = "SELECT PF.*, ES.T_DSC_ESTATUS_SERVICIO FROM " & Tabla & " PF INNER JOIN BDS_C_GR_ESTATUS_SERVICIO ES ON PF.N_ID_ESTATUS_SERVICIO = ES.N_ID_ESTATUS_SERVICIO "

        Try

            Return conexion.ConsultarDT(sqlQuery).DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Obtiene el siguiente identificador del catalogo
    ''' </summary>
    ''' <returns>Identificador siguiente</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_PASO) + 1) N_ID_PASO FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_PASO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_PASO"))
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

    ''' <summary>
    ''' Agrega la Función al catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nuevo Paso de Flujo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_PASO") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_PASO_TOOLTIP") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VISIBLE") : listValores.Add(Me.ImagenVisible)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Imagen.Identificador)
            listCampos.Add("N_ID_IMAGEN_INACTIVA") : listValores.Add(Me.ImagenInactiva.Identificador)
            listCampos.Add("T_DSC_COLOR_HEX_ANTES") : listValores.Add(Me.ColorAnterior)
            listCampos.Add("T_DSC_COLOR_HEX_ACTUAL") : listValores.Add(Me.ColorActual)
            listCampos.Add("T_DSC_COLOR_HEX_POSTERIOR") : listValores.Add(Me.ColorPosterior)
            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.Estatus.Identificador)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            Try
                resultado = conexion.Insertar(Tabla, listCampos, listValores)
                bitacora.Insertar(Tabla, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message.ToString)
                Throw ex
            End Try

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            bitacora.Finalizar(resultado)

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Actualiza un Paso de Flujo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Paso de Flujo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_PASO_TOOLTIP") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VISIBLE") : listValores.Add(Me.ImagenVisible)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Imagen.Identificador)
            listCampos.Add("N_ID_IMAGEN_INACTIVA") : listValores.Add(Me.ImagenInactiva.Identificador)
            listCampos.Add("T_DSC_COLOR_HEX_ANTES") : listValores.Add(Me.ColorAnterior)
            listCampos.Add("T_DSC_COLOR_HEX_ACTUAL") : listValores.Add(Me.ColorActual)
            listCampos.Add("T_DSC_COLOR_HEX_POSTERIOR") : listValores.Add(Me.ColorPosterior)
            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.Estatus.Identificador)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_PASO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
                Throw ex
            End Try

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            bitacora.Finalizar(resultado)

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Termina la vigencia de un registro
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar Paso de Flujo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(False)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_PASO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
                Throw ex
            End Try

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            bitacora.Finalizar(resultado)

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function
#End Region
End Class
