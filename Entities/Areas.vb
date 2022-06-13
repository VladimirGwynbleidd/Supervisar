Public Class Areas

    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property Nombre As String
    Public Property Vigente As Boolean
    Public Property Existe As Boolean

    Public Sub New()

    End Sub

    Public Sub New(ByVal idArea)
        Me.Identificador = idArea
    End Sub

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Return conexion.ConsultarRegistrosDT("BDS_C_GR_AREAS").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    'RRA obtiene areas vigentes
    Public Function ObtenerTodosActivos() As DataSet
        Dim conexion As New Conexion.SQLServer
        Dim condicion As New List(Of String)
        Dim ValorCondicion As New List(Of Object)

        condicion.Add("B_FLAG_VIGENTE")
        ValorCondicion.Add(1)

        Try
            Return conexion.ConsultarRegistrosDS("BDS_C_GR_AREAS", condicion, valorcondicion)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

    End Function

    Public Function ObtenerAreasVisitaConjunta_IdVisita(ByVal idVisita As String) As DataSet
        Dim conexion As New Conexion.SQLServer
        Dim condicion As New List(Of String)
        Dim ValorCondicion As New List(Of Object)

        condicion.Add("I_ID_VISITA")
        ValorCondicion.Add(idVisita)

        Try
            Return conexion.ConsultarRegistrosDS("BDS_R_VISITAS_CONJUNTAS", condicion, ValorCondicion)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

    End Function

    'RRA Areas Disponibles sin contar la de sesion
    Public Function AreasActivas_SinSesion(ByVal idAreaSesion As Integer) As DataSet
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "SELECT I_ID_AREA,T_DSC_AREA FROM BDS_C_GR_AREAS WHERE B_FLAG_VIGENTE = 1 AND I_ID_AREA NOT IN(34, 37, " + idAreaSesion.ToString + ")"
        Try
            Return conexion.ConsultarDS(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    'RRA Extrae OBJETO VISITA que pertenecen a la area seleccioanda
    Public Function ObjVisita_AreaSeleccion(ByVal idAreaSeleccion As String) As DataSet
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "SELECT N_ID_OBJETO_VISITA as ID_OBJVIS, T_DSC_OBJETO_VISITA as DSC FROM [dbo].[BDS_C_GR_OBJETO_VISITA] WHERE N_FLAG_VIG = 1 AND I_ID_AREA in (" + idAreaSeleccion + ")"
        Try
            Return conexion.ConsultarDS(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    'RRA Extrae usuarios que pertenecen a la area seleccioanda
    Public Function Usuarios_AreaSeleccion(ByVal idAreaSeleccion As String) As DataSet
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "SELECT U.T_ID_USUARIO, U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX as NOMBRE FROM BDS_C_GR_USUARIO U " & _
                              "INNER JOIN BDS_R_GR_USUARIO_PERFIL UP ON UP.T_ID_USUARIO = U.T_ID_USUARIO " & _
                              "INNER JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = UP.N_ID_PERFIL " & _
                              "INNER JOIN BDS_C_GR_AREAS A ON A.I_ID_AREA = UP.I_ID_AREA " & _
                              "WHERE P.N_ID_PERFIL IN(2,3) AND A.I_ID_AREA IN(" + idAreaSeleccion + ") AND A.B_FLAG_VIGENTE=1 AND P.N_FLAG_VIG=1  AND u.N_FLAG_VIG=1 AND up.N_FLAG_VIG=1"
        Try
            Return conexion.ConsultarDS(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    'RRA Inserta nuevas Visitas Conjuntas mediante SP 
    Public Sub InsertaElimina_VisitasConjuntas(ByVal idVisita As String, ByVal cadenaAreas As String)
        Dim conexion As New Conexion.SQLServer
        Dim NombreSP As String = "SpID_BDS_GRL_INSERTA_ELIMINA_VISITA_CONJUNTA"
        Try
            Dim sqlParameter(1) As System.Data.SqlClient.SqlParameter
            sqlParameter(0) = New System.Data.SqlClient.SqlParameter("@ID_VISITA", idVisita)
            sqlParameter(1) = New System.Data.SqlClient.SqlParameter("@CADENA_SPLIT", cadenaAreas)

            conexion.EjecutarSP(NombreSP, sqlParameter)
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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(I_ID_AREA) + 1) I_ID_AREA FROM BDS_C_GR_AREAS")

            If dr.Read() Then

                If IsDBNull(dr("I_ID_AREA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("I_ID_AREA"))
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


            Dim bitacora As New Conexion.Bitacora("Registro de nueva area" & "(" & Me.Identificador.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("I_ID_AREA") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_AREA") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIGENTE") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_AREAS", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_AREAS", listCampos, listValores, resultado, "Error al registrar nueva area")

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


            Dim bitacora As New Conexion.Bitacora("Actualización de área" & "(" & Me.Identificador.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("I_ID_AREA") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIGENTE") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("I_ID_AREA") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_AREAS", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_AREAS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar área")

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


            Dim bitacora As New Conexion.Bitacora("Borrar area" & "(" & Me.Identificador.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIGENTE") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("I_ID_AREA") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_AREAS", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_AREAS", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar área")

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

    ''' <summary>
    ''' Obtiene los dsatos de ua area
    ''' </summary>
    ''' <remarks>agc</remarks>
    Public Sub CargarDatos()
        Dim conexion As New Conexion.SQLServer

        Try
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing
            Dim dr2 As SqlClient.SqlDataReader = Nothing

            listCampos.Add("I_ID_AREA") : listValores.Add(Me.Identificador)

            Try
                Existe = conexion.BuscarUnRegistro("BDS_D_VS_CONSECUTIVOS_AREAS", listCampos, listValores)

                If Existe Then
                    dr = conexion.ConsultarRegistrosDR("BDS_D_VS_CONSECUTIVOS_AREAS", listCampos, listValores)
                    If dr.Read() Then
                        Me.Descripcion = CStr(dr("T_DSC_AREA"))
                    End If

                    If dr IsNot Nothing Then
                        If Not dr.IsClosed Then
                            dr.Close() : dr = Nothing
                        End If
                    End If

                    dr2 = conexion.ConsultarRegistrosDR("BDS_C_GR_AREAS", listCampos, listValores)
                    If dr2.Read() Then
                        Me.Nombre = CStr(dr2("T_DSC_AREA"))
                        If Me.Descripcion.Trim().Length < 1 Then
                            Me.Descripcion = Me.Nombre.Substring(0, 2)
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
                If dr2 IsNot Nothing Then
                    If Not dr2.IsClosed Then
                        dr2.Close() : dr2 = Nothing
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
End Class
