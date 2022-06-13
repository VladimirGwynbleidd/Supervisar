<Serializable()>
Public Class Imagen

    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property Tipo As String
    Public Property Ruta As String
    Public Property Existe As Boolean = False
    Public Property Vigente As Boolean = False

    Public Const RutaCarpeta As String = "/Imagenes/Errores/"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idImagen)
        Me.Identificador = idImagen

        CargarDatos()
    End Sub

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer
        Dim dvImagen As New DataView
        Dim strQuery As String = ("SELECT I.N_ID_IMAGEN, I.T_DSC_IMAGEN, I.N_ID_TIPO_IMAGEN, I.T_DSC_RUTA_IMAGEN, " + _
                                  "I.G_IMG_IMAGEN, I.N_FLAG_VIG, I.F_FECH_INI_VIG, I.F_FECH_FIN_VIG, " + _
                                  "TI.T_DSC_TIPO_IMAGEN FROM BDS_C_GR_IMAGEN AS I INNER JOIN " + _
                                  "BDS_C_GR_TIPO_IMAGEN AS TI ON I.N_ID_TIPO_IMAGEN = TI.N_ID_TIPO_IMAGEN")

        Try

            dvImagen = conexion.ConsultarDT(strQuery).DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return dvImagen

    End Function

    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Identificador)

            Try


                Existe = conexion.BuscarUnRegistro("BDS_C_GR_IMAGEN", listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR("BDS_C_GR_IMAGEN", listCampos, listValores)

                    If dr.Read() Then

                        Me.Descripcion = CStr(dr("T_DSC_IMAGEN"))
                        Me.Ruta = CStr(dr("T_DSC_RUTA_IMAGEN"))
                        Me.Tipo = CStr(dr("N_ID_TIPO_IMAGEN"))
                        Me.Vigente = CBool(dr("N_FLAG_VIG"))

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

    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_IMAGEN) + 1) N_ID_IMAGEN FROM BDS_C_GR_IMAGEN")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_IMAGEN")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_IMAGEN"))
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


            Dim bitacora As New Conexion.Bitacora("Registro de nueva imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_IMAGEN") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_RUTA_IMAGEN") : listValores.Add(Me.Ruta)
            listCampos.Add("N_ID_TIPO_IMAGEN") : listValores.Add(Me.Tipo)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_IMAGEN", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_IMAGEN", listCampos, listValores, resultado, "Error al registrar nueva imagen")

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


            Dim bitacora As New Conexion.Bitacora("Actualización de imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_IMAGEN") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_RUTA_IMAGEN") : listValores.Add(Me.Ruta)
            listCampos.Add("N_ID_TIPO_IMAGEN") : listValores.Add(Me.Tipo)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_IMAGEN") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_IMAGEN", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_IMAGEN", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar imagen")

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


            Dim bitacora As New Conexion.Bitacora("Borrar imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_IMAGEN") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_IMAGEN", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_IMAGEN", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar imagen")

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
End Class
