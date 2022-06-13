'- Fecha de creación: 01/03/2014
'- Fecha de modificación:  ##/##/####
'- Nombre del Responsable: Rafael Rodriguez
'- Empresa: Softtek
'- Clase para agendar las tareas a los ingenieros

Imports Entities

Public Class Agendar

    ''' <summary>
    ''' Verifica si el dia de la semana de una fecha es sabado o domingo
    ''' </summary>
    ''' <param name="fecha">Fecha a validar</param>
    ''' <returns>True si es fin de semana, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Shared Function EsFinDeSemana(ByVal fecha As Date) As Boolean
        Dim finSemana As Boolean = False
        Dim dia As Integer = Weekday(fecha)
        'Si dia es domingo = 1 ó sabado = 7 es fin de semana
        If dia = 1 Or dia = 7 Then
            finSemana = True
        Else
            finSemana = False
        End If
        Return finSemana
    End Function

    ''' <summary>
    ''' Valida si una fecha dada es un dia habil
    ''' </summary>
    ''' <param name="fecha">Fecha a validar</param>
    ''' <returns>True si es dia habil, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Shared Function EsDiaHabil(ByVal fecha As Date) As Boolean
        'Se valida si el dia es fin de semana
        Dim finSemana As Boolean = EsFinDeSemana(fecha)
        Dim habil As Boolean = False
        If finSemana = False Then
            Dim strTabla = "BDS_C_GR_DIAS_FESTIVOS"
            Dim lstCampoCondicion As New List(Of String)
            Dim lstValoresCondicion As New List(Of Object)
            lstCampoCondicion.Add("F_FECH_DIA_FESTIVO") : lstValoresCondicion.Add(fecha)
            lstCampoCondicion.Add("B_FLAG_VIG") : lstValoresCondicion.Add(1)

            Dim con As Conexion.SQLServer = Nothing
            Try
                con = New Conexion.SQLServer
                Dim existe As Boolean = con.BuscarUnRegistro(strTabla, lstCampoCondicion, lstValoresCondicion)
                If existe Then
                    habil = False
                Else
                    habil = True
                End If
            Catch ex As Exception
                habil = False
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
                Throw New Exception("Error al determinar si es dia habil")
            Finally
                If Not IsNothing(con) Then
                    con.CerrarConexion()
                    con = Nothing
                End If
            End Try
        Else
            habil = False
        End If
        Return habil
    End Function

    ''' <summary>
    ''' Determina si una hora se encuentra en el periodo de atencion
    ''' </summary>
    ''' <param name="fecha">Fecha con la hora a validar</param>
    ''' <returns>True si es periodo de antencion, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Shared Function EsHoraLaboral(ByVal fecha As Date) As Boolean
        Dim horaLaboral As Boolean = False

        If (fecha.Hour >= 9 And fecha.Hour <= 13) Or (fecha.Hour >= 16 And fecha.Hour <= 18) Then
            horaLaboral = True
        Else
            horaLaboral = False
        End If

        Return horaLaboral
    End Function

    ''' <summary>
    ''' Valia si se encuentra disponible un periodo para asignacion de tareas de un ingeniero
    ''' </summary>
    ''' <param name="ingeniero">Ingeniero</param>
    ''' <param name="fechainicial">Fecha inicial de validacion</param>
    ''' <param name="fechaFinal">Fecha final de validacion</param>
    ''' <param name="Actualizacion">Indica si la validacion es para la actualizacion de un registro de agenda</param>
    ''' <param name="idRegistroAgenda">Id de registro de agenda, cuando es una validacion de actualizacion de registro de agenda</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidaPeriodo(ByVal ingeniero As String, ByVal fechainicial As DateTime, ByVal fechaFinal As DateTime,
                                  Optional ByVal Actualizacion As Boolean = False, Optional ByVal idRegistroAgenda As Integer = 0) As Boolean
        Dim disponible As Boolean = False
        fechaFinal = fechaFinal.AddHours(-1)
        Dim strQuery As String = "SELECT * FROM BDS_D_TI_AGENDA " + _
                                " WHERE T_ID_INGENIERO = '" + ingeniero + "' " + _
                                " AND F_FECH_FECHA_HORA_TAREA BETWEEN '" + fechainicial.ToString("yyyy-MM-dd HH:mm") + "' AND '" + fechaFinal.ToString("yyyy-MM-dd HH:mm") + "'"
        If Actualizacion Then
            strQuery += " AND N_ID_REGISTRO_AGENDA <> " + idRegistroAgenda.ToString
        End If
        Dim con As Conexion.SQLServer = Nothing
        Try
            con = New Conexion.SQLServer
            Dim dt As DataTable = con.ConsultarDT(strQuery)
            If dt.Rows.Count > 0 Then
                disponible = False
            Else
                disponible = True
            End If
        Catch ex As Exception
            disponible = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw New Exception("Error al Validar el periodo")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
        Return disponible
    End Function

    ''' <summary>
    ''' Valida si se encuentra disponible el horario seleccionado para la tarea
    ''' </summary>
    ''' <param name="objRegistroAgenda">Objeto RegistroAgenda</param>
    ''' <param name="Actualizacion">Indica si se validara para actualizacion</param>
    ''' <returns>True si el horario esta disponible, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Shared Function ValidaHorarioDisponible(ByVal objRegistroAgenda As RegistroAgenda, Optional ByVal Actualizacion As Boolean = False) As Boolean
        Dim disponible As Boolean = True

        If objRegistroAgenda.Ciclica Then
            Dim fechaInicial As DateTime = objRegistroAgenda.FechIniReg
            Dim fechaFinal As New DateTime(objRegistroAgenda.FechIniReg.Year, objRegistroAgenda.FechIniReg.Month, objRegistroAgenda.FechIniReg.Day, _
                                           objRegistroAgenda.FechFinReg.Hour, objRegistroAgenda.FechFinReg.Minute, objRegistroAgenda.FechFinReg.Second)

            Do While (fechaFinal < objRegistroAgenda.FechFinReg)
                Dim dia As Integer = Weekday(fechaInicial)

                Select Case dia
                    Case 2 'Lunes
                        If objRegistroAgenda.Lunes Then
                            disponible = ValidaPeriodo(objRegistroAgenda.IngenieroSolicta, fechaInicial, fechaFinal, Actualizacion, objRegistroAgenda.Id)
                        End If
                    Case 3 'Martes
                        If objRegistroAgenda.Martes Then
                            disponible = ValidaPeriodo(objRegistroAgenda.IngenieroSolicta, fechaInicial, fechaFinal, Actualizacion, objRegistroAgenda.Id)
                        End If
                    Case 4 'Miercoles
                        If objRegistroAgenda.Miercoles Then
                            disponible = ValidaPeriodo(objRegistroAgenda.IngenieroSolicta, fechaInicial, fechaFinal, Actualizacion, objRegistroAgenda.Id)
                        End If
                    Case 5 'Jueves
                        If objRegistroAgenda.Jueves Then
                            disponible = ValidaPeriodo(objRegistroAgenda.IngenieroSolicta, fechaInicial, fechaFinal, Actualizacion, objRegistroAgenda.Id)
                        End If
                    Case 6 'Viernes
                        If objRegistroAgenda.Viernes Then
                            disponible = ValidaPeriodo(objRegistroAgenda.IngenieroSolicta, fechaInicial, fechaFinal, Actualizacion, objRegistroAgenda.Id)
                        End If
                End Select

                If Not disponible Then
                    Exit Do
                End If

                fechaInicial = fechaInicial.AddDays(1)
                fechaFinal = fechaFinal.AddDays(1)
            Loop

        Else
            disponible = ValidaPeriodo(objRegistroAgenda.IngenieroSolicta, objRegistroAgenda.FechIniReg, objRegistroAgenda.FechFinReg, Actualizacion, objRegistroAgenda.Id)
        End If

        Return disponible
    End Function

    ''' <summary>
    ''' Borra los registros de Agenda de una tarea de Registro Agenda
    ''' </summary>
    ''' <param name="objRegistroAgenda">Objeto RegistroAgenda</param>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <param name="b">Objeto Bitacora</param>
    ''' <remarks></remarks>
    Public Sub BorraTareasAgenda(ByVal objRegistroAgenda As RegistroAgenda, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, ByVal b As Conexion.Bitacora)
        Dim objAgenda As New Agenda
        Dim dvAgenda As DataView = objAgenda.ObtenerTodos(con, tran)
        Dim consulta As String = " N_ID_REGISTRO_AGENDA = " + objRegistroAgenda.Id.ToString + " AND T_ID_INGENIERO = '" + objRegistroAgenda.IngenieroSolicta + "'"

        dvAgenda.RowFilter = consulta

        For Each dr As DataRow In dvAgenda.ToTable.Rows
            objAgenda.Eliminar(dr.Item("N_ID_AGENDA"), con, b, tran)
        Next

    End Sub

    ''' <summary>
    ''' Obtiene el numerio de dias de duracion de la tarea ya sea ciclica o continua
    ''' De igual manera obtiene el numerio de dias de vacaciones dependiendo de las fechas capturadas o el valor enviado
    ''' </summary>
    ''' <param name="FechaFinIntermedia">Fecha final de calculo de dias</param>
    ''' <returns>Numero de dias</returns>
    ''' <remarks></remarks>
    Public Shared Function NumeroDiasSeleccionados(ByVal objRegistroAgenda As RegistroAgenda, Optional ByVal FechaFinIntermedia As Date = Nothing) As Int32
        Dim dias As Integer = 0
        Dim FechaInicio As Date
        Dim FechaFin As Date
        If objRegistroAgenda.Ciclica Then
            FechaInicio = objRegistroAgenda.FechIniReg
            If FechaFinIntermedia <> Nothing Then
                FechaFin = FechaFinIntermedia
            Else
                FechaFin = objRegistroAgenda.FechFinReg
            End If

            Do
                Dim dia As Integer = Weekday(FechaInicio)

                Select Case dia
                    Case 2  'Lunes
                        If objRegistroAgenda.Lunes Then
                            If Agendar.EsDiaHabil(FechaInicio) Then
                                dias += 1
                            End If
                        End If
                    Case 3 'Martes
                        If objRegistroAgenda.Martes Then
                            If Agendar.EsDiaHabil(FechaInicio) Then
                                dias += 1
                            End If
                        End If
                    Case 4  'Miercoles
                        If objRegistroAgenda.Miercoles Then
                            If Agendar.EsDiaHabil(FechaInicio) Then
                                dias += 1
                            End If
                        End If
                    Case 5  'Jueves
                        If objRegistroAgenda.Jueves Then
                            If Agendar.EsDiaHabil(FechaInicio) Then
                                dias += 1
                            End If
                        End If
                    Case 6  'Viernes
                        If objRegistroAgenda.Viernes Then
                            If Agendar.EsDiaHabil(FechaInicio) Then
                                dias += 1
                            End If
                        End If
                End Select

                FechaInicio = FechaInicio.AddDays(1)

            Loop While FechaInicio < FechaFin
        Else
            FechaInicio = objRegistroAgenda.FechIniReg
            If Not FechaFinIntermedia = Nothing Then
                FechaFin = FechaFinIntermedia
            Else
                FechaFin = objRegistroAgenda.FechFinReg
            End If

            Do
                If Agendar.EsDiaHabil(FechaInicio) Then
                    dias += 1
                End If
                FechaInicio = FechaInicio.AddDays(1)
            Loop While FechaInicio < FechaFin
        End If
        Return dias
    End Function

    ''' <summary>
    ''' Almacena los registros de Agenda de un dia
    ''' </summary>
    ''' <param name="ObjRegistroAgenda">Objeto RegistroAgenda</param>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <param name="bitac">Objeto Bitacora</param>
    ''' <param name="fechaInicial">Fecha y hora inicial, en caso de omitirlo se utilizara la fecha y hora de ObjRegistroAgenda</param>
    ''' <param name="fechaFinal">Fecha y hora final, en caso de omitirlo se utilizara la fecha y hora de ObjRegistroAgenda</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GuardaAgenda(ByVal ObjRegistroAgenda As RegistroAgenda, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, ByVal bitac As Conexion.Bitacora,
                                  Optional ByVal fechaInicial As DateTime = Nothing, Optional ByVal fechaFinal As DateTime = Nothing) As Boolean
        Dim guardo As Boolean = False

        If fechaInicial = Nothing Then
            fechaInicial = ObjRegistroAgenda.FechIniReg
        End If
        If fechaFinal = Nothing Then
            fechaFinal = ObjRegistroAgenda.FechFinReg
        End If

        Dim objAgenda As New Agenda
        objAgenda.Ingeniero = ObjRegistroAgenda.IngenieroSolicta
        objAgenda.IdTipoTarea = Agenda.TipoTarea.RegistroAgenda
        objAgenda.IdRegistroAgendaFK = ObjRegistroAgenda.Id
        objAgenda.TareaAgenda = ObjRegistroAgenda.NotaRegistro
        objAgenda.FechaHoraTarea = fechaInicial

        Do
            If objAgenda.FechaHoraTarea.Hour = 0 AndAlso Not EsDiaHabil(objAgenda.FechaHoraTarea) Then
                objAgenda.FechaHoraTarea = objAgenda.FechaHoraTarea.AddDays(1)
            Else
                If EsHoraLaboral(objAgenda.FechaHoraTarea) Then
                    objAgenda.Identificador = objAgenda.ObtenerSiguienteIdentificador(con, tran)
                    guardo = objAgenda.AgregarAgendaServicios(con, tran, bitac)
                End If

                objAgenda.FechaHoraTarea = objAgenda.FechaHoraTarea.AddHours(1)
            End If

        Loop While (objAgenda.FechaHoraTarea < fechaFinal) And guardo

        Return guardo
    End Function

    ''' <summary>
    ''' Guarda un registro agenda y las tareas correspondientes en agenda
    ''' </summary>
    ''' <param name="ObjRegistroAgenda"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GuardarTarea(ByVal ObjRegistroAgenda As RegistroAgenda, ByVal EsVacaciones As Boolean) As Boolean
        Dim actualizacion As Boolean = If(ObjRegistroAgenda.Id > 0, True, False)
        Dim guardo As Boolean = True

        If ValidaHorarioDisponible(ObjRegistroAgenda, actualizacion) Then
            Dim con As Conexion.SQLServer = Nothing
            Dim bitacora As Conexion.Bitacora = Nothing
            Dim tran As SqlClient.SqlTransaction = Nothing
            Try
                con = New Conexion.SQLServer()

                tran = con.BeginTransaction()

                Dim MensajeBitacora As String = "{0} {1} Agenda {2}"

                If actualizacion Then
                    If ObjRegistroAgenda.Aprobado Then
                        MensajeBitacora = "Guarda Registro Agenda {0} a partir del ID número {1} de la tabla BDS_D_TI_AGENDA"
                        MensajeBitacora = String.Format(MensajeBitacora, ObjRegistroAgenda.Id, New Agenda().ObtenerSiguienteIdentificador.ToString)
                        bitacora = New Conexion.Bitacora(MensajeBitacora, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
                    Else
                        MensajeBitacora = String.Format(MensajeBitacora, "Actualiza", "Registro", ObjRegistroAgenda.Id)
                        bitacora = New Conexion.Bitacora(MensajeBitacora, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
                    End If
                    ObjRegistroAgenda.Modificar(con, bitacora, tran)
                    BorraTareasAgenda(ObjRegistroAgenda, con, tran, bitacora)
                Else
                    MensajeBitacora = String.Format(MensajeBitacora, "Guarda", "Registro", ObjRegistroAgenda.ObtenerSiguienteIdentificador)
                    bitacora = New Conexion.Bitacora(MensajeBitacora, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
                    ObjRegistroAgenda.Agregar(con, bitacora, tran)
                End If

                Dim diasVacaciones As Integer = 0

                'Si el registro esta aprobado se guarda en agenda y se actualizan las vacaciones
                If ObjRegistroAgenda.Aprobado Then
                    If ObjRegistroAgenda.Ciclica Then
                        Dim fechaInicial As DateTime = ObjRegistroAgenda.FechIniReg
                        Dim fechaFinal As New DateTime(ObjRegistroAgenda.FechIniReg.Year, ObjRegistroAgenda.FechIniReg.Month, ObjRegistroAgenda.FechIniReg.Day, _
                                                       ObjRegistroAgenda.FechFinReg.Hour, ObjRegistroAgenda.FechFinReg.Minute, ObjRegistroAgenda.FechFinReg.Second)

                        Do While (fechaFinal < ObjRegistroAgenda.FechFinReg) And guardo
                            Dim dia As Integer = Weekday(fechaInicial)

                            Select Case dia
                                Case 2  'Lunes
                                    If ObjRegistroAgenda.Lunes Then
                                        guardo = GuardaAgenda(ObjRegistroAgenda, con, tran, bitacora, fechaInicial, fechaFinal)
                                        diasVacaciones += 1
                                    End If
                                Case 3 'Martes
                                    If ObjRegistroAgenda.Martes Then
                                        guardo = GuardaAgenda(ObjRegistroAgenda, con, tran, bitacora, fechaInicial, fechaFinal)
                                        diasVacaciones += 1
                                    End If
                                Case 4  'Miercoles
                                    If ObjRegistroAgenda.Miercoles Then
                                        guardo = GuardaAgenda(ObjRegistroAgenda, con, tran, bitacora, fechaInicial, fechaFinal)
                                        diasVacaciones += 1
                                    End If
                                Case 5  'Jueves
                                    If ObjRegistroAgenda.Jueves Then
                                        guardo = GuardaAgenda(ObjRegistroAgenda, con, tran, bitacora, fechaInicial, fechaFinal)
                                        diasVacaciones += 1
                                    End If
                                Case 6  'Viernes
                                    If ObjRegistroAgenda.Viernes Then
                                        guardo = GuardaAgenda(ObjRegistroAgenda, con, tran, bitacora, fechaInicial, fechaFinal)
                                        diasVacaciones += 1
                                    End If
                            End Select

                            fechaInicial = fechaInicial.AddDays(1)
                            fechaFinal = fechaFinal.AddDays(1)

                        Loop

                    Else
                        guardo = GuardaAgenda(ObjRegistroAgenda, con, tran, bitacora)
                        'Si es vacaciones se obtienen el numero de dias
                        If EsVacaciones Then
                            Dim FechaInicio As Date = ObjRegistroAgenda.FechIniReg
                            Dim fechaFin As Date = ObjRegistroAgenda.FechFinReg

                            Do
                                If EsDiaHabil(FechaInicio) Then
                                    diasVacaciones += 1
                                End If
                                FechaInicio = FechaInicio.AddDays(1)
                            Loop While FechaInicio < fechaFin
                        End If
                    End If

                    If EsVacaciones Then
                        Dim objVacaciones As New Vacaciones()
                        Dim lstObjVacaciones As List(Of Vacaciones) = objVacaciones.ObtnenPeriodosActivos(ObjRegistroAgenda.IngenieroSolicta)
                        Select Case lstObjVacaciones.Count
                            Case 0
                                Throw New Exception("No existen periodos de vacaciones")
                            Case 1
                                lstObjVacaciones(0).DiasConsumidos = lstObjVacaciones(0).DiasConsumidos + diasVacaciones
                                lstObjVacaciones(0).ActualizaVacaciones(con, tran, bitacora)
                            Case 2
                                Dim DiasDisponibles As Integer = lstObjVacaciones(0).DiasAsignados - lstObjVacaciones(0).DiasConsumidos
                                If DiasDisponibles >= diasVacaciones Then
                                    lstObjVacaciones(0).DiasConsumidos = lstObjVacaciones(0).DiasConsumidos + diasVacaciones
                                    diasVacaciones = 0
                                Else
                                    lstObjVacaciones(0).DiasConsumidos = lstObjVacaciones(0).DiasConsumidos + DiasDisponibles
                                    diasVacaciones -= DiasDisponibles
                                End If

                                lstObjVacaciones(0).ActualizaVacaciones(con, tran, bitacora)

                                If diasVacaciones > 0 Then
                                    lstObjVacaciones(1).DiasConsumidos = lstObjVacaciones(1).DiasConsumidos + diasVacaciones
                                    lstObjVacaciones(1).ActualizaVacaciones(con, tran, bitacora)
                                End If
                            Case Else
                                Dim numero As Integer = lstObjVacaciones.Count - 1
                                Dim DiasDisponibles As Integer = lstObjVacaciones(numero - 1).DiasAsignados - lstObjVacaciones(numero - 1).DiasConsumidos
                                If DiasDisponibles >= diasVacaciones Then
                                    lstObjVacaciones(numero - 1).DiasConsumidos = lstObjVacaciones(numero - 1).DiasConsumidos + diasVacaciones
                                    diasVacaciones = 0
                                Else
                                    lstObjVacaciones(numero - 1).DiasConsumidos = lstObjVacaciones(numero - 1).DiasConsumidos + DiasDisponibles
                                    diasVacaciones -= DiasDisponibles
                                End If

                                lstObjVacaciones(numero - 1).ActualizaVacaciones(con, tran, bitacora)

                                If diasVacaciones > 0 Then
                                    lstObjVacaciones(numero).DiasConsumidos = lstObjVacaciones(numero).DiasConsumidos + diasVacaciones
                                    lstObjVacaciones(numero).ActualizaVacaciones(con, tran, bitacora)
                                End If

                        End Select
                    End If
                End If

            Catch ex As Exception
                guardo = False
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Finally
                If Not IsNothing(tran) Then
                    If guardo Then
                        tran.Commit()
                    Else
                        tran.Rollback()
                    End If
                    tran.Dispose()
                End If
                If Not IsNothing(bitacora) Then
                    Try : bitacora.Finalizar(guardo) : Catch ex As Exception : End Try
                End If
                If Not IsNothing(con) Then
                    con.CerrarConexion()
                    con = Nothing
                End If
            End Try

        End If

        Return guardo
    End Function

    ''' <summary>
    ''' Da de baja logica un registro agenda y elimina las tareas de agenda
    ''' </summary>
    ''' <param name="objRegistroAgenda">Objeto Registro Agenda</param>
    ''' <returns>True si la baja fue exitosa, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Function BajaRegistroAgenda(ByVal objRegistroAgenda As RegistroAgenda) As Boolean
        Dim Elimino As Boolean = False

        Dim con As Conexion.SQLServer = Nothing
        Dim bitac As Conexion.Bitacora = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing
        Try
            con = New Conexion.SQLServer()
            bitac = New Conexion.Bitacora("Baja de Registro Agenda " & objRegistroAgenda.Id, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            tran = con.BeginTransaction()

            objRegistroAgenda.Baja(con, bitac, tran)

            BorraTareasAgenda(objRegistroAgenda, con, tran, bitac)

            Elimino = True
        Catch ex As Exception
            Elimino = False
        Finally
            If Elimino Then
                Try : tran.Commit() : Catch : Elimino = False : End Try
            Else
                Try : tran.Rollback() : Catch : End Try
            End If
            If Not IsNothing(bitac) Then
                Try : bitac.Finalizar(Elimino) : Catch : End Try
            End If

            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return Elimino
    End Function

End Class
