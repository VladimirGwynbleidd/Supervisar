
'- Fecha de creación:11/07/2013
'- Fecha de modificación: 08/05/2014
'- Nombre del Responsable: ARGC1
'- Empresa: Softtek
'- contiene los metodos que son parte del negocio del carrusel de asignaciones


Imports Conexion
Imports Utilerias

Public Class Carrusel


#Region "Variables"
    Public Shared Property FtoFecha As String = "dd/MM/yyyy"
#End Region


#Region "Propiedades"
    Private Shared ReadOnly Property Tabla As String
        Get
            Return "BDS_R_GR_USUARIO_CARRUSEL"
        End Get
    End Property




#End Region


    ''' <summary>
    ''' Indica si el usuario recibe asignaciones o no
    ''' </summary>
    ''' <param name="usuario">id de usuario </param>
    ''' <returns>Bolean</returns>
    ''' <remarks></remarks>
    Public Shared Function EstaENCarrusel(ByVal usuario As String) As Boolean
        Dim flag As Boolean = False
        Dim con As SQLServer = Nothing
        Dim dt As DataTable = Nothing

        Dim bitacora As New Bitacora("Indica si el usuario recibe asignaciones o no", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Dim Sql As String = String.Empty
        Try
            con = New SQLServer
            dt = New DataTable

            Sql = String.Format(" SELECT T_ID_USUARIO FROM {0} WHERE T_ID_USUARIO='{1}'", Tabla, usuario)

            dt = con.ConsultarDT(Sql)

            bitacora.ConsultarDT(Sql, True, "")
            If dt.Rows.Count = 0 Then
                flag = False
            Else
                flag = True

            End If
        Catch ex As Exception
            flag = False
            bitacora.ConsultarDT(Sql, flag, ex.ToString)
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            Try : bitacora.Finalizar(flag) : Catch ex As Exception : End Try
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
            dt.Dispose()
        End Try

        Return flag
    End Function

    ''' <summary>
    ''' Agrega un usuario a la tabla de carrusel
    ''' </summary>
    ''' <param name="usuario">id de usuario </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertaEnCarusel(ByVal usuario As String) As Boolean
        Dim flag As Boolean = False
        Dim con As SQLServer = Nothing

        Dim listCampos As List(Of String) = New List(Of String)
        Dim listValores As List(Of Object) = New List(Of Object)

        Dim bitacora As New Bitacora("Agrega un usuario a la tabla de carrusel", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            con = New SQLServer
            listCampos.Add("T_ID_USUARIO") : listValores.Add(usuario)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(0)
            listCampos.Add("N_FLAG_TURNO") : listValores.Add(0)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(DateTime.Now)

            flag = con.Insertar(Tabla, listCampos, listValores)
            bitacora.Insertar(Tabla, listCampos, listValores, flag, "")

        Catch ex As Exception
            flag = False
            bitacora.Insertar(Tabla, listCampos, listValores, flag, ex.ToString)
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            Try : bitacora.Finalizar(flag) : Catch ex As Exception : End Try

            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
        End Try

        Return flag
    End Function


    ''' <summary>
    ''' Obtiene la lista de usuarios del sistema y los valores que tengan el el carrusel
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Shared Function ObtenerUsuarios() As DataTable

        Dim con As SQLServer = Nothing
        Dim oDs As DataTable = Nothing
        Dim Sql As String = String.Empty
        Dim bitacora As New Bitacora("Obtiene la lista de usuarios del sistema y los valores que tengan en el carrusel", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim flagBitacora As Boolean = True
        Try
            con = New SQLServer
            oDs = New DataTable
            'Modificación 08/05/2014 Se obtiene el nombre completo del usuario
            Sql = String.Format("SELECT U.T_ID_USUARIO, U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX AS NOMBRE,ISNULL(C.N_NUM_ORDEN,0) N_NUM_ORDEN, ISNULL(C.N_FLAG_VIG,0) N_FLAG_VIG  FROM BDS_C_GR_USUARIO U LEFT JOIN {0} C ON C.T_ID_USUARIO = U.T_ID_USUARIO WHERE U.N_FLAG_VIG=1 " & _
                                " ORDER BY CASE WHEN C.N_NUM_ORDEN = 0 THEN 0 END, C.N_NUM_ORDEN", Tabla)

            oDs = con.ConsultarDT(Sql)
            bitacora.ConsultarDT(Sql, True, "")

        Catch ex As Exception
            flagBitacora = False
            bitacora.ConsultarDT(Sql, flagBitacora, ex.ToString)
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            Try : bitacora.Finalizar(flagBitacora) : Catch ex As Exception : End Try
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
        End Try

        Return oDs

    End Function



    ''' <summary>
    ''' Obtiene el siguiente usuario en el orden de recepción de asignaciones según algunas condiciones 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerSiguienteAsignacion() As String
        Dim usuarioNext As String = String.Empty

        Dim con As SQLServer = Nothing
        Dim reader As SqlClient.SqlDataReader = Nothing
        Dim bitacora As New Bitacora("Obtiene la lista de usuarios del sistema y los valores que tengan el el carrusel", "", "")
        Dim flagBitacora As Boolean = True
        Try
            con = New SQLServer

            Dim parametros(0) As SqlClient.SqlParameter

            parametros(0) = New SqlClient.SqlParameter("@USUARIO_RETURN", SqlDbType.VarChar, 16)
            parametros(0).Direction = ParameterDirection.Output

            reader = con.EjecutarSPConsultaDR("SPS_SIS_TURNO_USUARIO", parametros)

            If reader.Read() Then
                usuarioNext = CStr(reader(0))

            End If

            bitacora.EjecutarSPConsultaDR("SPS_SIS_TURNO_USUARIO", True, "")
        Catch ex As Exception
            flagBitacora = False
            bitacora.EjecutarSPConsultaDR("SPS_SIS_TURNO_USUARIO", flagBitacora, ex.ToString)
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            Try : bitacora.Finalizar(flagBitacora) : Catch ex As Exception : End Try

            If reader IsNot Nothing Then
                If Not reader.IsClosed Then
                    reader.Close() : reader = Nothing
                End If
            End If
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
        End Try
        
        Return usuarioNext
    End Function


    ''' <summary>
    ''' Activa o desactiva la propuedad recibe de un usuario en el carrusel
    ''' </summary>
    ''' <param name="usuario">id de usuario </param>
    ''' <param name="recibe">true/false </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RecibeChanged(ByRef usuario As String, ByVal recibe As Boolean) As Boolean
        Dim flag As Boolean = False
        Dim con As SQLServer = Nothing

        Try
            con = New SQLServer

            Dim listCampos As List(Of String) = New List(Of String)
            Dim listValores As List(Of Object) = New List(Of Object)
            Dim listCamposCond As List(Of String) = New List(Of String)
            Dim listValoresCond As List(Of Object) = New List(Of Object)


            listCamposCond.Add("T_ID_USUARIO")
            listValoresCond.Add(usuario)

            If recibe = True Then
                listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
                listCampos.Add("F_FECH_INI_VIG") : listValores.Add(DateTime.Now)
            Else
                listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
                listCampos.Add("N_NUM_ORDEN") : listValores.Add(0)
                listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(DateTime.Now)
            End If


            If con.Actualizar(Tabla, listCampos, listValores, listCamposCond, listValoresCond) Then

                If recibe = False Then
                    '------------------------------------------------------
                    ' Hacer reacomodo del orden
                    '-----------------------------------------------------

                    Try

                        Dim params(0) As SqlClient.SqlParameter
                        params(0) = New SqlClient.SqlParameter("@EXITO", SqlDbType.Bit)
                        params(0).Direction = ParameterDirection.Output

                        Dim reader As SqlClient.SqlDataReader = con.EjecutarSPConsultaDR("SPS_SIS_ORDENA_USUARIOS", params)

                        If reader.Read() Then
                            If CInt(reader("EXITO")) = 1 Then
                                flag = True
                            Else
                                flag = False

                            End If
                        End If

                    Catch ex As Exception
                        ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
                        'Throw ex
                        flag = False
                    End Try


                Else
                    '----------------------------------------------------------------------------------------------
                    ' Buscar el ultimi orden, si nadie recibe, poner automaticamente el orden en 1 para el usuario
                    '----------------------------------------------------------------------------------------------

                    listCampos.Clear()
                    listValores.Clear()
                    Dim oDs As New DataSet

                    Dim Sql = String.Format("SELECT max(N_NUM_ORDEN)as MAX_ORDEN FROM {0} U  WHERE N_FLAG_VIG = 1 AND N_NUM_ORDEN <> 0", Tabla)

                    oDs = con.ConsultarDS(Sql)

                    If oDs IsNot Nothing Then

                        listCampos.Add("N_NUM_ORDEN")

                        If IsDBNull(oDs.Tables(0).Rows(0)("MAX_ORDEN")) Then
                            listValores.Add(1)

                        Else
                            Dim nesxOrder = CInt(oDs.Tables(0).Rows(0)("MAX_ORDEN")) + 1
                            listValores.Add(nesxOrder)

                        End If

                        If con.Actualizar(Tabla, listCampos, listValores, listCamposCond, listValoresCond) Then
                            flag = True
                        Else
                            flag = False
                        End If
                    End If
                End If

            Else

                flag = False
            End If

        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
        End Try

        Return flag
    End Function


    ''' <summary>
    ''' Activa o desactiva la propuedad recibe de un usuario en el carrusel
    ''' </summary>
    ''' <param name="UsuarioActual">id de usuario actual </param>
    ''' <param name="command">comando invocado </param>
    ''' <param name="PosicionActual">pocision actual que tiene el usuario </param>
    ''' <returns></returns>
    Public Shared Function ActualizaOrden(ByVal UsuarioActual As String, ByVal command As String, ByVal PosicionActual As Int16) As Boolean
        Dim flag As Boolean = False
        Dim con As SQLServer = Nothing
        Dim Tran As SqlClient.SqlTransaction = Nothing
        Dim sql As String = String.Empty
        Try

            con = New SQLServer
            Dim listCampos As List(Of String) = New List(Of String)
            Dim listValores As List(Of Object) = New List(Of Object)
            Dim listCamposCond As List(Of String) = New List(Of String)
            Dim listValoresCond As List(Of Object) = New List(Of Object)

            listCampos.Add("N_NUM_ORDEN")
            listCamposCond.Add("T_ID_USUARIO")

            If command = "Subir" Then

                '----------------------------------------------------------------
                'restar posicion actual en 1
                '>si es igual a cero no hacer nada
                ' buscar quién tiene esa posición
                'si alguien la tiene intercambiar y volver a cargar grid.
                'si nadie la tiene obtener la posición mas baja y asignar esa.
                '----------------------------------------------------------------

                Dim usuarioSiguiente As String = Nothing
                Dim posicionSiguiente As Integer = Nothing

                If PosicionActual = 0 Then
                    Tran = con.BeginTransaction()
                    '--------------------------------------------------------
                    '        No tiene posición asignada aún.
                    '--------------------------------------------------------

                    sql = String.Format("SELECT MAX(N_NUM_ORDEN) as MAX_ORDEN FROM {0} WHERE N_FLAG_VIG = 1", Tabla)

                    Dim dr As SqlClient.SqlDataReader = con.ConsultarDRConTransaccion(sql, Tran)

                    If dr.Read() Then

                        If Not IsDBNull(dr("MAX_ORDEN")) Then
                            '--------------------------------------------------------
                            '   Si hay posición qué asignarle
                            '--------------------------------------------------------
                            posicionSiguiente = CInt(dr("MAX_ORDEN")) + 1
                            listValores.Add(posicionSiguiente)

                        Else
                            '--------------------------------------------------------
                            '   No hay posiciones asignadas, iniciando con 1
                            '--------------------------------------------------------

                            listValores.Add(1)

                        End If

                    End If

                    If dr IsNot Nothing Then
                        dr.Close() : dr = Nothing
                    End If

                    '---------------------------------------------------'
                    '       ejecuta Actualización
                    '----------------------------------------------------

                    If listValores.Count > 0 Then

                        listValoresCond.Add(UsuarioActual)

                        If (con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCond, listValoresCond, Tran)) Then
                            Try : Tran.Commit() : Catch ex As Exception : End Try
                            flag = True
                        Else
                            Try : Tran.Rollback() : Catch ex As Exception : End Try
                            flag = False

                        End If

                    End If


                ElseIf PosicionActual - 1 > 0 Then

                    Tran = con.BeginTransaction()
                    '--------------------------------------------------------
                    '   Usuario ya tiene posición asignada.
                    '   Selecciona el Usuario de la posición siguiente.
                    '--------------------------------------------------------

                    sql = String.Format("SELECT N_NUM_ORDEN, T_ID_USUARIO FROM {0} WHERE N_NUM_ORDEN = {1} ", Tabla, PosicionActual - 1)


                    Dim dr As SqlClient.SqlDataReader = con.ConsultarDRConTransaccion(sql, Tran)

                    If dr.Read() Then

                        If Not IsDBNull(dr("T_ID_USUARIO")) And Not IsDBNull(dr("N_NUM_ORDEN")) Then
                            usuarioSiguiente = dr("T_ID_USUARIO").ToString
                            posicionSiguiente = CInt(dr("N_NUM_ORDEN").ToString)
                            Try : dr.Close() : Catch ex As Exception : End Try


                            '--------------------------------------------------------
                            '   Cambiando el usuario actual por el siguiente. Haciendo switch de posiciones.
                            '   Orden siguiente asginarlo al usuario actual
                            '--------------------------------------------------------

                            listValoresCond.Add(UsuarioActual)
                            listValores.Add(posicionSiguiente)

                            If con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCond, listValoresCond, Tran) Then

                                '------limpia listas----------
                                listValores.Clear()
                                listValoresCond.Clear()
                                '-----------------------------


                                '--------------------------------------------------------
                                '   Orden actual asignarlo al usuario siguiente
                                '--------------------------------------------------------


                                listValores.Add(PosicionActual)
                                listValoresCond.Add(usuarioSiguiente)

                                If con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCond, listValoresCond, Tran) Then
                                    Try : Tran.Commit() : Catch ex As Exception : End Try
                                    flag = True
                                Else
                                    Try : Tran.Rollback() : Catch ex As Exception : End Try
                                    flag = False
                                End If

                            Else
                                Try : Tran.Rollback() : Catch ex As Exception : End Try
                                flag = False
                            End If

                        Else
                            Try : Tran.Rollback() : Catch ex As Exception : End Try
                            flag = False

                        End If

                    Else
                        Try : dr.Close() : Catch ex As Exception : End Try

                    End If

                ElseIf PosicionActual = 1 Then

                    ''-----CUANDO TIENE EL ORDEN=1 YA NO HAY PARA DONDE SUBIR
                    flag = True

                End If

            ElseIf command = "Bajar" Then
                Tran = con.BeginTransaction()

                Dim usuarioAnterior As String = Nothing
                Dim posicionAnterior As Integer = Nothing

                If PosicionActual = 0 Then

                    '--------------------------------------------------------
                    '   obtener la posicion mas alta y asignarle la siguiente.
                    '--------------------------------------------------------
                    sql = String.Format("SELECT MAX(N_NUM_ORDEN) as MAX_ORDEN FROM {0} WHERE N_FLAG_VIG = 1", Tabla)


                    Dim dr As SqlClient.SqlDataReader = con.ConsultarDRConTransaccion(sql, Tran)
                    If dr.Read() Then
                        If Not IsDBNull(dr("MAX_ORDEN")) Then
                            posicionAnterior = CInt(dr("MAX_ORDEN")) + 1
                            listValores.Add(posicionAnterior)
                        Else
                            listValores.Add(1)
                        End If
                    Else
                        listValores.Add(1)
                    End If
                    dr.Close()

                    '--------ejecutar actualizacion----------'

                    listValoresCond.Add(UsuarioActual)

                    If (con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCond, listValoresCond, Tran)) Then
                        Try : Tran.Commit() : Catch ex As Exception : End Try

                        flag = True

                    Else
                        Try : Tran.Rollback() : Catch ex As Exception : End Try
                        flag = False
                    End If


                Else

                    '-----------------------------------'
                    '---si la posicion no es 0----------'

                    sql = String.Format("SELECT N_NUM_ORDEN, T_ID_USUARIO FROM {0} WHERE N_NUM_ORDEN = {1} AND N_FLAG_VIG = 1", Tabla, PosicionActual + 1)

                    Dim dr As SqlClient.SqlDataReader = con.ConsultarDRConTransaccion(sql, Tran)

                    If dr.Read() Then


                        If Not IsDBNull(dr("T_ID_USUARIO")) And Not IsDBNull(dr("N_NUM_ORDEN")) Then
                            usuarioAnterior = dr("T_ID_USUARIO").ToString
                            posicionAnterior = CInt(dr("N_NUM_ORDEN").ToString)
                            Try : dr.Close() : Catch ex As Exception : End Try
                            '--------------------------------------------------------
                            '   Cambiando el usuario actual por el siguiente. Haciendo switch de posiciones.
                            '   Orden siguiente asginarlo al usuario actual
                            '--------------------------------------------------------

                            listValores.Add(posicionAnterior)
                            listValoresCond.Add(UsuarioActual)

                            If con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCond, listValoresCond, Tran) Then

                                '------limpia listas----------'
                                listValores.Clear()
                                listValoresCond.Clear()
                                '-----------------------------'
                                '--------------------------------------------------------
                                '   Orden actual asignarlo al usuario siguiente
                                '--------------------------------------------------------

                                listValores.Add(PosicionActual)
                                listValoresCond.Add(usuarioAnterior)

                                If con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCond, listValoresCond, Tran) Then
                                    Try : Tran.Commit() : Catch ex As Exception : End Try

                                    flag = True

                                Else
                                    Try : Tran.Rollback() : Catch ex As Exception : End Try
                                    flag = False
                                End If

                            Else
                                Try : Tran.Rollback() : Catch ex As Exception : End Try
                                flag = False
                            End If

                        Else
                            flag = False

                        End If
                    Else

                        '---- NO EXISTEN USUARIOS DEBAJO DE ÉL

                        Try : dr.Close() : Catch ex As Exception : End Try
                        Try : Tran.Rollback() : Catch ex As Exception : End Try
                        flag = True

                    End If
                End If

            End If

        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
        End Try

        Return flag

    End Function


#Region "Periodes de no asignacion"



    ''' <summary>
    ''' Obtiene los periodode de asignacion para un usuario
    ''' </summary>
    ''' <param name="usuario">id del usuario </param>
    ''' <returns>Dataset</returns>
    Public Shared Function ObtienePeriodosDeNoAsignacion(ByVal usuario As String) As DataTable
        Dim con As SQLServer = Nothing
        Dim dt As DataTable = Nothing

        Try
            con = New SQLServer
            dt = New DataTable

            Dim Sql = "SELECT T_ID_USUARIO," +
                      " CONVERT(varchar(10), [F_FECH_INI_VIG], 103) AS F_FECH_INI_VIG" +
                      ",CONVERT(varchar(10), [F_FECH_FIN_VIG], 103) AS F_FECH_FIN_VIG " +
                      " FROM BDS_R_GR_USUARIO_CARRUSEL_PERIODO_NO_ASIGNADO " +
                      " WHERE T_ID_USUARIO='{0}'"

            Sql = String.Format(Sql, usuario)

            dt = con.ConsultarDT(Sql)
        Catch ex As Exception

        Finally
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If

        End Try
        
        Return dt

    End Function

    ''' <summary>
    ''' Inserta periodo de NO asignacion a usuario
    ''' </summary>
    ''' <param name="FechaIni">fecha de inicio fin del periodo</param>
    '''  <param name="FechaFin">fecha de fin del periodo </param>
    ''' <param name="usuario">id del usuario </param>
    ''' <remarks></remarks>
    Public Shared Function AgregarPeriodo(ByVal FechaIni As String, ByVal FechaFin As String, ByVal usuario As String) As Boolean
        Dim con As SQLServer = Nothing
        Dim flag As Boolean = False
        Try
            con = New SQLServer

            Dim listCampos As List(Of String) = New List(Of String)
            Dim listValores As List(Of Object) = New List(Of Object)

            listCampos.Add("T_ID_USUARIO") : listValores.Add(usuario)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(FechaIni)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(FechaFin)

            If con.Insertar("BDS_R_GR_USUARIO_CARRUSEL_PERIODO_NO_ASIGNADO", listCampos, listValores) Then

                flag = True
            Else
                flag = False

            End If

        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If
        End Try

        Return flag
    End Function



    ''' <summary>
    ''' Verifica que las fechas del periodo sean validas
    ''' </summary>
    ''' <param name="FechaIni">fecha de inicio fin del periodo</param>
    '''  <param name="FechaFin">fecha de fin del periodo </param>
    ''' <param name="usuario">id del usuario </param>
    Public Function VerificaFechas(ByVal FechaIni As String, ByVal FechaFin As String, ByVal usuario As String) As Boolean

        Dim FechaInicial, FechaFinal As Date

        If FechaValida(FechaIni) And FechaValida(FechaFin) Then
            FechaInicial = DateTime.ParseExact(FechaIni, FtoFecha, Globalization.CultureInfo.InvariantCulture)
            FechaFinal = DateTime.ParseExact(FechaFin, FtoFecha, Globalization.CultureInfo.InvariantCulture)
            If FechaInicial <= FechaFinal And FechaInicial >= Date.Today Then

                If RevisaNoTraslape(FechaIni, FechaFin, usuario) Then
                    Return True
                Else
                    Return False
                End If

            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function


    ''' <summary>
    ''' Función que verifica no exista traslape entre los periodos seleccionados.
    ''' False: periodo traslapado
    ''' </summary>
    ''' <param name="FechaIni">fecha de inicio fin del periodo</param>
    '''  <param name="FechaFin">fecha de fin del periodo </param>
    ''' <param name="usuario">id del usuario </param>
    ''' <remarks></remarks>
    Public Shared Function RevisaNoTraslape(ByVal FechaIni As String, ByVal FechaFin As String, ByVal usuario As String) As Boolean
        Dim con As SQLServer = Nothing
        Dim dr As SqlClient.SqlDataReader = Nothing
        Dim traslapeFlag As Boolean = False
        Try
            con = New SQLServer
            Dim sql As String =
                "SELECT COUNT(*) FROM [dbo].[BDS_R_GR_USUARIO_CARRUSEL_PERIODO_NO_ASIGNADO] " & _
                "WHERE " & _
                "((CONVERT(smalldatetime,  '{0}', 103) BETWEEN  F_FECH_INI_VIG AND F_FECH_FIN_VIG) OR " & _
                "(CONVERT(smalldatetime,  '{1}', 103) BETWEEN  F_FECH_INI_VIG AND F_FECH_FIN_VIG) OR " & _
                "(F_FECH_INI_VIG BETWEEN CONVERT(smalldatetime,  '{2}', 103)  AND CONVERT(smalldatetime,  '{3}', 103)) OR " & _
                "(F_FECH_FIN_VIG BETWEEN CONVERT(smalldatetime,  '{4}', 103)  AND CONVERT(smalldatetime,  '{5}', 103))) AND " & _
                "T_ID_USUARIO = '{6}'  "
            sql = String.Format(sql,
                                FechaIni,
                                FechaFin,
                                FechaIni,
                                FechaFin,
                                FechaIni,
                                FechaFin,
                                usuario)

            dr = con.ConsultarDR(sql)

            If dr.Read() Then

                If dr(0).ToString <> "0" Then
                    traslapeFlag = True
                    Return False
                Else
                    traslapeFlag = False
                    Return True

                End If

            Else
                traslapeFlag = False
                Return traslapeFlag
            End If

        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If dr IsNot Nothing Then
                If Not dr.IsClosed Then
                    dr.Close() : dr = Nothing
                End If
            End If

            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If

        End Try
        Return False
    End Function


    ''' <summary>
    ''' Verifica que la fecha ingresada sea válida
    ''' </summary>
    ''' <param name="fecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FechaValida(ByVal fecha As String) As Boolean
        Try
            DateTime.ParseExact(fecha, FtoFecha, Globalization.CultureInfo.InvariantCulture)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    ''' <summary>
    ''' Elimina un periodo de asignación
    ''' </summary>
    ''' <param name="FechaIni">fecha de inicio fin del periodo</param>
    '''  <param name="FechaFin">fecha de fin del periodo </param>
    ''' <param name="usuario">id del usuario </param>
    ''' <returns></returns>
    Public Shared Function EliminaPeriodo(ByVal usuario As String, ByVal FechaIni As String, ByVal FechaFin As String) As Boolean
        Dim con As SQLServer = Nothing
        Dim flag As Boolean = False
        Try

            con = New SQLServer
            Dim condicion As String = "T_ID_USUARIO='{0}' AND F_FECH_INI_VIG = CONVERT(smalldatetime,'{1}',103) AND F_FECH_FIN_VIG = CONVERT(smalldatetime, '{2}', 103)"
            condicion = String.Format(condicion,
                                      usuario,
                                      FechaIni,
                                      FechaFin)

            flag = con.Ejecutar("DELETE FROM BDS_R_GR_USUARIO_CARRUSEL_PERIODO_NO_ASIGNADO WHERE " & condicion)

        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If con IsNot Nothing Then
                con.CerrarConexion() : con = Nothing
            End If

        End Try

        Return flag
    End Function



#End Region

End Class
