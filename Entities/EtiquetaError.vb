Public Class EtiquetaError


    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property Escenario As String
    Public Property Existe As Boolean = False
    Public Property Imagen As Imagen
    Public Property Vigente As Boolean = False


    Public Sub New()

    End Sub

    Public Sub New(ByVal idError)
        Me.Identificador = idError

        CargarDatos()



    End Sub

    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("N_ID_ERROR") : listValores.Add(Me.Identificador)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_ERROR", listCampos, listValores)

            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_ERROR", listCampos, listValores)

                If dr.Read() Then

                    Me.Descripcion = CStr(dr("T_DSC_ERROR"))
                    Me.Escenario = CStr(dr("T_DSC_ESCENARIO"))
                    Me.Imagen = New Imagen(CInt(dr("N_ID_IMAGEN")))
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

            Return conexion.ConsultarRegistrosDT("BDS_C_GR_ERROR").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_ERROR) + 1) N_ID_ERROR FROM BDS_C_GR_ERROR")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_ERROR")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_ERROR"))
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

    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Try


            Dim bitacora As New Conexion.Bitacora("Registro de nueva etiqueta de error" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_ERROR") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_ERROR") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_ESCENARIO") : listValores.Add(Me.Escenario)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Imagen.Identificador)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_ERROR", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_ERROR", listCampos, listValores, resultado, "Error al registrar nueva etiqueta de error")

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


            Dim bitacora As New Conexion.Bitacora("Actualización de etiqueta de error" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_ERROR") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_ESCENARIO") : listValores.Add(Me.Escenario)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Imagen.Identificador)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_ERROR") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_ERROR", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_ERROR", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar etiqueta de error")

            bitacora.Finalizar(resultado)

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally



            If Not IsNothing(Conexion) Then
                Conexion.CerrarConexion()
            End If



        End Try

        Return resultado

    End Function

    Public Function Baja() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Try


            Dim bitacora As New Conexion.Bitacora("Borrar error" & "(" & Me.Identificador & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_ERROR") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_ERROR", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_ERROR", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar error")

            bitacora.Finalizar(resultado)

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally



            If Not IsNothing(Conexion) Then
                Conexion.CerrarConexion()
            End If



        End Try

        Return resultado

    End Function
End Class
