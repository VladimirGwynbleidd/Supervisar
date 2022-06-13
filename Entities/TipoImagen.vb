'- Fecha de creación: 11/02/2014
'- Fecha de modificación:  NA
'- Nombre del Responsable: Rivera Martiñón Iván
'- Empresa: Softtek
'- Clase para Catálogo De Tipo de Imagen

<Serializable()>
Public Class TipoImagen

    Private Tabla As String = "BDS_C_GR_TIPO_IMAGEN"

#Region "Propiedades"

    Public Property Identificador As Integer
    Public Property Descripcion As String
    Public Property Vigente As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date?
    Public Property Existe As Boolean = False

#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idTipoImagen As Integer)
        Me.Identificador = idTipoImagen

        CargarDatos()
    End Sub


#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los datos de Tipo de Imagen tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_TIPO_IMAGEN") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(Tabla, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(Tabla, listCampos, listValores)

                    If dr.Read() Then
                        Me.Descripcion = CStr(dr("T_DSC_TIPO_IMAGEN"))
                        Me.Vigente = Convert.ToBoolean(dr("B_FLAG_VIG"))
                        Me.InicioVigencia = Convert.ToDateTime(dr("F_FECH_INI_VIG"))

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

        Try

            Return conexion.ConsultarRegistrosDT(Tabla).DefaultView

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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_TIPO_IMAGEN) + 1) N_ID_TIPO_IMAGEN FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_TIPO_IMAGEN")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_TIPO_IMAGEN"))
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
    ''' Agrega el Tipo de Imagen al catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nuevo Tipo de Imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_TIPO_IMAGEN") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_TIPO_IMAGEN") : listValores.Add(Me.Descripcion)
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
    ''' Actualiza un Tipo de Tipo de Imagen
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Tipo de Imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_TIPO_IMAGEN") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_TIPO_IMAGEN") : listValoresCondicion.Add(Me.Identificador)

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
        Dim bitacora As New Conexion.Bitacora("Borrar Tipo de Imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(False)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_TIPO_IMAGEN") : listValoresCondicion.Add(Me.Identificador)

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