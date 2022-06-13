'- Fecha de creación:29/07/2013
'- Fecha de modificación:  29/07/2013
'- Nombre del Responsable: Julio Cesar Vieyra Tena
'- Empresa: Softtek
'- Clase de Parametros

Public Class Parametros


#Region "Variables"
    Public Property IdentificadorParametro As Integer
    Public Property Parametro As String
    Public Property DescripcionParametro As String
    Public Property ValorParametro As String
    Public Property Existe As Boolean = False
    Public Property Vigente As Boolean = False
#End Region

#Region "Constructores"
    Public Sub New()
    End Sub
    Public Sub New(ByVal identificadorParametro As Integer, ByVal parametro As String, ByVal descripcionParametro As String, ByVal valorParametro As String)

        Me.IdentificadorParametro = identificadorParametro
        Me.Parametro = parametro
        Me.DescripcionParametro = descripcionParametro
        Me.ValorParametro = valorParametro
        Dim conexion As New Conexion.SQLServer
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        listCampos.Add("N_ID_PARAMETRO") : listValores.Add(Me.IdentificadorParametro)
        Existe = conexion.BuscarUnRegistro("BDS_C_GR_PARAMETRO", listCampos, listValores)

    End Sub
    Public Sub New(ByVal identificadorParametro As Integer)

        Me.IdentificadorParametro = identificadorParametro
        CargarDatos()

    End Sub

#End Region

#Region "Metodos"

    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            listCampos.Add("N_ID_PARAMETRO") : listValores.Add(Me.IdentificadorParametro)
            Existe = conexion.BuscarUnRegistro("BDS_C_GR_PARAMETRO", listCampos, listValores)
            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_PARAMETRO", listCampos, listValores)
                If dr.Read() Then
                    Me.IdentificadorParametro = CStr(dr("N_ID_PARAMETRO"))
                    Me.Parametro = CStr(dr("T_DSC_PARAMETRO"))
                    Me.DescripcionParametro = CStr(dr("T_OBS_PARAMETRO"))
                    Me.ValorParametro = CStr(dr("T_DSC_VALOR"))
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
    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer
        Try

            Return conexion.ConsultarRegistrosDT("BDS_C_GR_PARAMETRO", "N_ID_PARAMETRO").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Registro de nuevo Parametro", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_PARAMETRO") : listValores.Add(Me.IdentificadorParametro)
            listCampos.Add("T_DSC_PARAMETRO") : listValores.Add(Me.Parametro)
            listCampos.Add("T_OBS_PARAMETRO") : listValores.Add(Me.DescripcionParametro)
            listCampos.Add("T_DSC_VALOR") : listValores.Add(Me.ValorParametro)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_PARAMETRO", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_PARAMETRO", listCampos, listValores, resultado, "Error al registrar nuevo parametro")
            bitacora.Finalizar(resultado)


        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If


        End Try

        Return resultado

    End Function
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Actualizacion de Parametro", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)


            listCampos.Add("T_DSC_PARAMETRO") : listValores.Add(Me.Parametro)
            listCampos.Add("T_OBS_PARAMETRO") : listValores.Add(Me.DescripcionParametro)
            listCampos.Add("T_DSC_VALOR") : listValores.Add(Me.ValorParametro)


            listCamposCondicion.Add("N_ID_PARAMETRO") : listValoresCondicion.Add(Me.IdentificadorParametro)



            resultado = conexion.Actualizar("BDS_C_GR_PARAMETRO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_PARAMETRO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar parametro")
            bitacora.Finalizar(resultado)


        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If


        End Try

        Return resultado

    End Function
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Borrar Parametro", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_PARAMETRO") : listValoresCondicion.Add(Me.IdentificadorParametro)


            resultado = conexion.Actualizar("BDS_C_GR_PARAMETRO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_PARAMETRO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar parametro")
            bitacora.Finalizar(resultado)

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)



        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If


        End Try

        Return resultado

    End Function
    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_PARAMETRO) + 1) N_ID_PARAMETRO FROM BDS_C_GR_PARAMETRO")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_PARAMETRO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_PARAMETRO"))
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

#End Region



End Class
