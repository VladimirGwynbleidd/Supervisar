<Serializable()> _
Public Class Paquete


    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property Existe As Boolean = False
    Public Property Imagen As Imagen
    Public Property Vigente As Boolean = False


    Public Sub New()

    End Sub

    Public Sub New(ByVal idPaquete)
        Me.Identificador = idPaquete

        CargarDatos()



    End Sub

    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("N_ID_GRUPO_SERVICIO") : listValores.Add(Me.Identificador)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores)

            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores)

                If dr.Read() Then

                    Me.Descripcion = CStr(dr("T_DSC_GRUPO_SERVICIO"))
                    Me.Imagen = New Imagen(CInt(dr("N_ID_IMAGEN")))
                    Me.Vigente = CBool(dr("B_FLAG_VIG"))

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

            Return conexion.ConsultarRegistrosDT("BDS_C_GR_GRUPO_SERVICIOS").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function ObtenerTodosImagen() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try
            Dim dtPaquete As DataTable = conexion.ConsultarDT("SELECT N_ID_GRUPO_SERVICIO, T_DSC_GRUPO_SERVICIO, (IM.T_DSC_RUTA_IMAGEN + '?ID=' + CONVERT(varchar(3),GS.N_ID_GRUPO_SERVICIO)) AS IMAGEN " & _
                                                             "FROM BDS_C_GR_GRUPO_SERVICIOS GS INNER JOIN BDS_C_GR_IMAGEN IM ON GS.N_ID_IMAGEN = IM.N_ID_IMAGEN WHERE GS.B_FLAG_VIG = 1")
            Return dtPaquete

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function
    Public Shared Function PaqueteSoloDatosGetCustom(ByVal where As String) As List(Of Paquete)

        Dim resultado As New List(Of Paquete)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtPaquete As DataTable = conexion.ConsultarDT("SELECT N_ID_GRUPO_SERVICIO, T_DSC_GRUPO_SERVICIO, N_ID_IMAGEN, " & _
                                                               "B_FLAG_VIG FROM BDS_C_GR_GRUPO_SERVICIOS " & where & " ORDER BY N_ID_GRUPO_SERVICIO")

            For Each renglon As DataRow In dtPaquete.Rows

                resultado.Add(New Paquete With {.Identificador = renglon("N_ID_GRUPO_SERVICIO").ToString, _
                                                .Descripcion = renglon("T_DSC_GRUPO_SERVICIO").ToString, _
                                                .Imagen = New Imagen(renglon("N_ID_IMAGEN")), _
                                                .Vigente = CBool(renglon("B_FLAG_VIG"))})

            Next

            Return resultado

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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_GRUPO_SERVICIO) + 1) N_ID_GRUPO_SERVICIO FROM BDS_C_GR_GRUPO_SERVICIOS")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_GRUPO_SERVICIO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_GRUPO_SERVICIO"))
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


            Dim bitacora As New Conexion.Bitacora("Registro de nuevo paquete de servicios", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_GRUPO_SERVICIO") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_GRUPO_SERVICIO") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Imagen.Identificador)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores, resultado, "Error al registrar nuevo paquete de servicio")

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


            Dim bitacora As New Conexion.Bitacora("Actualización de paquete de servicio", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_GRUPO_SERVICIO") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_ID_IMAGEN") : listValores.Add(Me.Imagen.Identificador)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_GRUPO_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar etiqueta de error")

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


            Dim bitacora As New Conexion.Bitacora("Borrar error", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_GRUPO_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_GRUPO_SERVICIOS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar error")

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

    Public Function ActualizarNivelesServicio(agregados As List(Of Entities.NivelServicio), eliminados As List(Of Entities.NivelServicio)) As Boolean

        'Dim strQuery As String
        Dim campos As List(Of Object)
        Dim condicion As List(Of String)
        Dim valores As List(Of Object)

        Dim conexion As New Conexion.SQLServer
        Try

            For Each agregado As Entities.NivelServicio In agregados
                campos = New List(Of Object)
                campos.Add(Me.Identificador)
                campos.Add(agregado.Identificador)

                conexion.Insertar("BDS_R_GR_GRUPO_SERVICIOS", campos)
            Next

            For Each eliminado As Entities.NivelServicio In eliminados
                condicion = New List(Of String)
                valores = New List(Of Object)

                condicion.Add("N_ID_GRUPO_SERVICIO") : valores.Add(Me.Identificador)
                condicion.Add("N_ID_NIVELES_SERVICIO") : valores.Add(eliminado.Identificador)

                conexion.Eliminar("BDS_R_GR_GRUPO_SERVICIOS", condicion, valores)
            Next


        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try







        Return True
    End Function

    Public Function ObtenerNivelesServicio() As List(Of Entities.NivelServicio)


        Dim lstNivelesServicio As New List(Of NivelServicio)

        Dim strQuery As String = "SELECT S.N_ID_NIVELES_SERVICIO, S.T_DSC_NIVELES_SERVICIO " & _
                                "FROM BDS_C_GR_GRUPO_SERVICIOS G " & _
                                "JOIN BDS_R_GR_GRUPO_SERVICIOS GS ON G.N_ID_GRUPO_SERVICIO = GS.N_ID_GRUPO_SERVICIO " & _
                                "JOIN bDS_C_GR_NIVELES_SERVICIO S ON GS.N_ID_NIVELES_SERVICIO = S.N_ID_NIVELES_SERVICIO " & _
                                "WHERE G.N_ID_GRUPO_SERVICIO =  " & Me.Identificador


        Dim conexion As New Conexion.SQLServer
        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim entNivelServicio As New NivelServicio()
                entNivelServicio.Identificador = dr.Item("N_ID_NIVELES_SERVICIO")
                entNivelServicio.Descripcion = dr.Item("T_DSC_NIVELES_SERVICIO")
                lstNivelesServicio.Add(entNivelServicio)
            Next

            Return lstNivelesServicio

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function



    Public Function ObtenerNivelesServicioDisponibles() As List(Of Entities.NivelServicio)

        Dim lstNivelesServicio As New List(Of NivelServicio)

        Dim strQuery As String = "SELECT N_ID_NIVELES_SERVICIO, T_DSC_NIVELES_SERVICIO " & _
                                "FROM BDS_C_GR_NIVELES_SERVICIO " & _
                                "WHERE N_ID_NIVELES_SERVICIO NOT IN " & _
                                "( " & _
                                "Select S.N_ID_NIVELES_SERVICIO " & _
                                "FROM BDS_C_GR_GRUPO_SERVICIOS G " & _
                                "JOIN BDS_R_GR_GRUPO_SERVICIOS GS ON G.N_ID_GRUPO_SERVICIO = GS.N_ID_GRUPO_SERVICIO " & _
                                "JOIN BDS_C_GR_NIVELES_SERVICIO S ON GS.N_ID_NIVELES_SERVICIO = S.N_ID_NIVELES_SERVICIO " & _
                                "WHERE G.N_ID_GRUPO_SERVICIO = " & Me.Identificador & _
                                " )"

        If Me.Identificador = 0 Then
            strQuery += " AND N_ID_FLUJO = 3"
        Else
            strQuery += " AND N_ID_FLUJO <> 3"
        End If

        Dim conexion As New Conexion.SQLServer
        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim entNivelServicio As New NivelServicio()
                entNivelServicio.Identificador = dr.Item("N_ID_NIVELES_SERVICIO")
                entNivelServicio.Descripcion = dr.Item("T_DSC_NIVELES_SERVICIO")
                lstNivelesServicio.Add(entNivelServicio)
            Next

            Return lstNivelesServicio

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function
    Public Function ObtenerServiciosTooltip() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try
            Dim query As String = String.Empty
            query = "SELECT GS.N_ID_GRUPO_SERVICIO, NV.N_ID_APLICATIVO, NV.N_ID_NIVELES_SERVICIO, " & _
                    "AP.T_DSC_APLICATIVO, SR.T_DSC_SERVICIO FROM BDS_R_GR_GRUPO_SERVICIOS GS " & _
                    "INNER JOIN BDS_C_GR_NIVELES_SERVICIO NV ON NV.N_ID_NIVELES_SERVICIO = GS.N_ID_NIVELES_SERVICIO " & _
                    "INNER JOIN BDS_C_GR_SERVICIOS SR ON SR.N_ID_SERVICIO = NV.N_ID_SERVICIO " & _
                    "INNER JOIN BDS_C_GR_APLICATIVO AP ON AP.N_ID_APLICATIVO = NV.N_ID_APLICATIVO " & _
                    "WHERE GS.N_ID_GRUPO_SERVICIO = {0}"
            query = String.Format(query, Me.Identificador)
            Dim dtPaquete As DataTable = conexion.ConsultarDT(query)
            Return dtPaquete
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

End Class
