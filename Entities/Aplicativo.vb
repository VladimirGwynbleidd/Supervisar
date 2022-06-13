'- Fecha de creación: 21/11/2013
'- Fecha de modificación:  NA
'- Nombre del Responsable: Rafael Rodriguez Sanchez
'- Empresa: Softtek
'- Clase para Catalogo de Aplicativo

<Serializable()>
Public Class Aplicativo

    Private Tabla As String = "BDS_C_GR_APLICATIVO"

#Region "Propiedades"

    Public Property Identificador As Integer
    Public Property Clave As String
    Public Property Acronimo As String
    Public Property Descripcion As String
    Public Property Ing_Responsable As String
    Public Property Ing_Backup As String
    Public Property Vigente As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date?
    Public Property Existe As Boolean = False

#End Region

#Region "Constructores"
    Public Sub New()

    End Sub

    Public Sub New(ByVal idAplicativo As Integer)
        Me.Identificador = idAplicativo

        CargarDatos()
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los datos del Aplicativo tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_APLICATIVO") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(Tabla, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(Tabla, listCampos, listValores)

                    If dr.Read() Then

                        Me.Clave = CStr(dr("T_CVE_APLICATIVO"))
                        Me.Acronimo = CStr(dr("T_DSC_ACRONIMO_APLICATIVO"))
                        Me.Descripcion = CStr(dr("T_DSC_APLICATIVO"))
                        Me.Ing_Responsable = CStr(dr("T_ID_INGENIERO_RESPONSABLE"))
                        Me.Ing_Backup = CStr(dr("T_ID_INGENIERO_BACKUP"))
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
    ''' Obtiene todos los registros de la entidad logica 
    ''' Adicionando el nombre completo de los ingenieros
    ''' </summary>
    ''' <returns>Dataview con los registros</returns>
    ''' <remarks></remarks>
    Public Function ObtenerTodosConNombreResponsables() As DataView
        Dim strQuery As String = "SELECT A.N_ID_APLICATIVO, A.T_CVE_APLICATIVO, A.T_DSC_ACRONIMO_APLICATIVO, A.T_DSC_APLICATIVO, " + _
                                 "    A.T_ID_INGENIERO_RESPONSABLE, A.T_ID_INGENIERO_BACKUP, A.B_FLAG_VIG, A.F_FECH_INI_VIG, A.F_FECH_FIN_VIG, " + _
                                 "    U_IR.T_DSC_NOMBRE + ' ' + U_IR.T_DSC_APELLIDO + ' ' + U_IR.T_DSC_APELLIDO_AUX AS NOMBRE_RESPONSABLE, " + _
                                 "    U_IB.T_DSC_NOMBRE + ' ' + U_IB.T_DSC_APELLIDO + ' ' + U_IB.T_DSC_APELLIDO_AUX AS NOMBRE_BACKUP " + _
                                 "FROM " + Tabla + " A " + _
                                 "LEFT JOIN BDS_C_GR_USUARIO U_IR ON U_IR.T_ID_USUARIO = A.T_ID_INGENIERO_RESPONSABLE " + _
                                 "LEFT JOIN BDS_C_GR_USUARIO U_IB ON U_IB.T_ID_USUARIO = A.T_ID_INGENIERO_BACKUP "

        Dim conexion As New Conexion.SQLServer
        Dim dv As New DataView()

        Try
            dv = conexion.ConsultarDT(strQuery).DefaultView
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

        Return dv
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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_APLICATIVO) + 1) N_ID_APLICATIVO FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_APLICATIVO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_APLICATIVO"))
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
    ''' Agrega el aplicativo al catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nuevo Aplicativo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_APLICATIVO") : listValores.Add(Me.Identificador)
            listCampos.Add("T_CVE_APLICATIVO") : listValores.Add(Me.Clave)
            listCampos.Add("T_DSC_ACRONIMO_APLICATIVO") : listValores.Add(Me.Acronimo)
            listCampos.Add("T_DSC_APLICATIVO") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_ID_INGENIERO_RESPONSABLE") : listValores.Add(Me.Ing_Responsable)
            listCampos.Add("T_ID_INGENIERO_BACKUP") : listValores.Add(Me.Ing_Backup)
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
    ''' Actualiza un Aplicativo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Aplicativo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_CVE_APLICATIVO") : listValores.Add(Me.Clave)
            listCampos.Add("T_DSC_ACRONIMO_APLICATIVO") : listValores.Add(Me.Acronimo)
            listCampos.Add("T_DSC_APLICATIVO") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_ID_INGENIERO_RESPONSABLE") : listValores.Add(Me.Ing_Responsable)
            listCampos.Add("T_ID_INGENIERO_BACKUP") : listValores.Add(Me.Ing_Backup)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_APLICATIVO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                If resultado And Me.Identificador <> 0 Then
                    ActualizarNivelesServicio()
                End If
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

            Bitacora.Finalizar(resultado)

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    Private Sub ActualizarNivelesServicio()
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT N_ID_NIVELES_SERVICIO FROM BDS_C_GR_NIVELES_SERVICIO WHERE N_ID_APLICATIVO = " & Me.Identificador
            Dim dvNiveles = conexion.ConsultarDT(query)
            Dim objNiveles As NivelServicio

            For Each fila As DataRow In dvNiveles.Rows
                objNiveles = New NivelServicio(fila.Item(0).ToString)
                objNiveles.IngenieroResponsable = Me.Ing_Responsable
                objNiveles.IngenieroBackup = Me.Ing_Backup
                objNiveles.Actualizar()
            Next
        Catch ex As Exception

            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Sub

    ''' <summary>
    ''' Termina la vigencia de un registro
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar Aplicativo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(False)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_APLICATIVO") : listValoresCondicion.Add(Me.Identificador)

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
