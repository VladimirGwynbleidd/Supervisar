Public Class Area

    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property IdSubdirector As String
    Public Property Subdirector As String
    Public Property IdBackup As String
    Public Property Backup As String
    Public Property Existe As Boolean = False
    Public Property Vigente As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(ByVal idArea)
        Me.Identificador = idArea
        CargarDatos()
    End Sub

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Return conexion.ConsultarRegistrosDT("BDS_C_GR_AREA").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function


   Public Shared Function ObtenerDescripcionArea(area As Integer) As String
      Dim strQuery As String = "SELECT [T_DSC_AREA] FROM [dbo].[BDS_C_GR_AREAS] WHERE [I_ID_AREA] = " & area & ""

      Dim conexion As New Conexion.SQLServer
      Dim descripcionarea As String

      Try
         descripcionarea = conexion.EjecutarQuery(strQuery)
      Catch ex As Exception
         Throw ex
      Finally
         If Not IsNothing(conexion) Then
            conexion.CerrarConexion()
         End If
      End Try

      Return descripcionarea
   End Function

   ''' <summary>
   ''' Obtiene todos los registros de la entidad logica 
   ''' Adicionando el nombre completo de los ingenieros
   ''' </summary>
   ''' <returns>Dataview con los registros</returns>
   ''' <remarks></remarks>
   Public Function ObtenerTodosConNombreResponsables() As DataView
        Dim strQuery As String = "SELECT A.N_ID_AREA, A.T_DSC_AREA, " + _
                                 "    A.T_ID_SUBDIRECTOR, A.T_ID_BACKUP, A.B_FLAG_VIG, A.F_FECH_INI_VIG, A.F_FECH_FIN_VIG, " + _
                                 "    U_IR.T_DSC_NOMBRE + ' ' + U_IR.T_DSC_APELLIDO + ' ' + U_IR.T_DSC_APELLIDO_AUX AS NOMBRE_RESPONSABLE, " + _
                                 "    U_IB.T_DSC_NOMBRE + ' ' + U_IB.T_DSC_APELLIDO + ' ' + U_IB.T_DSC_APELLIDO_AUX AS NOMBRE_BACKUP " + _
                                 "FROM BDS_C_GR_AREA A " + _
                                 "LEFT JOIN BDS_C_GR_USUARIO U_IR ON U_IR.T_ID_USUARIO = A.T_ID_SUBDIRECTOR " + _
                                 "LEFT JOIN BDS_C_GR_USUARIO U_IB ON U_IB.T_ID_USUARIO = A.T_ID_BACKUP "

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


    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_AREA") : listValores.Add(Me.Identificador)

            Try


                Existe = conexion.BuscarUnRegistro("BDS_C_GR_AREA", listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR("BDS_C_GR_AREA", listCampos, listValores)

                    If dr.Read() Then

                        Me.Descripcion = CStr(dr("T_DSC_AREA"))
                        Me.IdSubdirector = CStr(dr("T_ID_SUBDIRECTOR"))
                        Me.IdBackup = CStr(dr("T_ID_BACKUP"))
                        Me.Vigente = CBool(dr("B_FLAG_VIG"))



                        dr = conexion.ConsultarRegistrosDR("BDS_C_GR_USUARIO", New List(Of String) From {"T_ID_USUARIO"}, New List(Of Object) From {Me.IdSubdirector})

                        If dr.Read() Then
                            Me.Subdirector = CStr(dr("T_DSC_NOMBRE")) & " " & CStr(dr("T_DSC_APELLIDO"))
                        End If

                        dr = conexion.ConsultarRegistrosDR("BDS_C_GR_USUARIO", New List(Of String) From {"T_ID_USUARIO"}, New List(Of Object) From {Me.IdBackup})

                        If dr.Read Then
                            Me.Backup = CStr(dr("T_DSC_NOMBRE")) & " " & CStr(dr("T_DSC_APELLIDO"))
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

    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_AREA) + 1) N_ID_AREA FROM BDS_C_GR_AREA")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_AREA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_AREA"))
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

            listCampos.Add("N_ID_AREA") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_AREA") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_ID_SUBDIRECTOR") : listValores.Add(Me.IdSubdirector)
            listCampos.Add("T_ID_BACKUP") : listValores.Add(Me.IdBackup)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_AREA", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_AREA", listCampos, listValores, resultado, "Error al registrar nueva area")

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

            listCampos.Add("T_DSC_AREA") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_ID_SUBDIRECTOR") : listValores.Add(Me.IdSubdirector)
            listCampos.Add("T_ID_BACKUP") : listValores.Add(Me.IdBackup)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_AREA") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_AREA", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_AREA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar área")

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

            listCampos.Add("B_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_AREA") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_AREA", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_AREA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar área")

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
