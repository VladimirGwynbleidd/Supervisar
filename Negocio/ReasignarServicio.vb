'- Fecha de creación: 01/08/2014
'- Fecha de modificación:  ##/##/####
'- Nombre del Responsable: Rafael Rodriguez
'- Empresa: Softtek
'- Clase para reasignar servicios

Imports Entities

Public Class ReasignarServicio

    Public Enum Tipo
        Reasignar = 1
        Reagendar = 2
    End Enum

    ''' <summary>
    ''' Reagenda un nivel de servicio ya sea para el mismo ingeniero o a uno diferente(reasignacion)
    ''' </summary>
    ''' <param name="Folio">Folio de Solicitud</param>
    ''' <param name="idNivelServicio">Id Servicio</param>
    ''' <param name="tipoReagenda">Indica si es reagendado o reasignacion</param>
    ''' <param name="IngenieroResponsable">Usuario al que se reasigna</param>
    ''' <returns>True si se actualizo correctamente</returns>
    ''' <remarks>Si es reasignacion se debe especificar el usuario a reagendar</remarks>
    Public Function Reagendar(Folio As String, idNivelServicio As Integer, ByVal tipoReagenda As Tipo, Optional ByVal IngenieroResponsable As String = "N/A") As Boolean
        Dim reasigno As Boolean
        Dim fechIni As DateTime
        Dim fechFin As DateTime
        Dim fechaValidar As DateTime
        Dim objAgenda As New Agenda()
        Dim IngenieroActual As String = String.Empty
        Dim nomTarea As String = String.Empty
        Dim idRegistroAgenda As Integer = 0
        Dim objNivel As New NivelServicio(idNivelServicio)
        Dim objSol As New Solicitud(Folio)
        Dim idSolicitud As Integer = objSol.Identificador


        Dim dtDatosAg As DataTable = objAgenda.ObtenerDatosAgenda(idSolicitud, idNivelServicio)

        If dtDatosAg.Rows.Count > 0 Then
            IngenieroActual = dtDatosAg.Rows(0).Item("T_ID_INGENIERO").ToString
            idRegistroAgenda = Convert.ToInt32(dtDatosAg.Rows(0).Item("N_ID_REGISTRO_AGENDA"))
            nomTarea = dtDatosAg.Rows(0).Item("T_DSC_TAREA_AGENDA").ToString
        End If

        Dim con As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing

        Try
            con = New Conexion.SQLServer()
            bitacora = New Conexion.Bitacora("Se reasigna el nivel de servicio " & idNivelServicio & " de la solicitud " & Folio, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
            tran = con.BeginTransaction()

            'ELIMINAR DE LA AGENDA
            objAgenda.EliminarGrupoTareas(IngenieroActual, idSolicitud, idNivelServicio, con, bitacora, tran)

            'Dependiendo el tipo de reasignado se insertaran al nuevo ingeniero o al actual
            If tipoReagenda = Tipo.Reasignar Then
                objAgenda.Ingeniero = IngenieroResponsable
            ElseIf tipoReagenda = Tipo.Reagendar Then
                objAgenda.Ingeniero = IngenieroActual
            End If


            'INSERTAR CON EL NUEVO INGENIERO
            If objNivel.TiempoEjecucion = 1 Then
                'SERVICIO ESPECIAL
                DeterminarFechaInicio(objAgenda, fechIni, con, tran)
                For hora As Integer = 1 To objNivel.TiempoEjecucion
                    objAgenda.Identificador = objAgenda.ObtenerSiguienteIdentificador(con, tran)
                    objAgenda.FechaHoraTarea = fechIni
                    objAgenda.IdTipoTarea = 1
                    objAgenda.IdSolicitud = idSolicitud
                    objAgenda.IdNivelServicio = idNivelServicio
                    objAgenda.IdRegistroAgendaFK = idRegistroAgenda
                    objAgenda.TareaAgenda = "TAREA REASIGNADA - " & nomTarea
                    objAgenda.AgregarAgendaServicios(con, tran, bitacora)
                Next

            Else
                'SERVICIO NORMAL CON FECHA DE TERMINO ESTABLECIDA------>
                DeterminarFechaInicio(objAgenda, fechIni, con, tran)
                For hora As Integer = 1 To objNivel.TiempoEjecucion
                    objAgenda.Identificador = objAgenda.ObtenerSiguienteIdentificador(con, tran)
                    objAgenda.FechaHoraTarea = fechIni
                    objAgenda.IdTipoTarea = 1
                    objAgenda.IdSolicitud = idSolicitud
                    objAgenda.IdNivelServicio = idNivelServicio
                    objAgenda.IdRegistroAgendaFK = idRegistroAgenda
                    objAgenda.TareaAgenda = "TAREA REASIGNADA - " & nomTarea
                    objAgenda.AgregarAgendaServicios(con, tran, bitacora)
                    'SE INSERTARÁ HORA POR HORA Y OBVIAMENTE CADA HORA SE VALIDA
                    fechaValidar = fechIni.AddHours(1)
                    fechIni = ValidaFechaFinal(fechaValidar, objAgenda.Ingeniero, con, tran)
                    fechFin = fechIni
                Next
                objSol.NivelServicio = idNivelServicio
                objSol.FechaLimite = fechFin
                objSol.ActualizarServicioAgenda(con, bitacora, tran)
                '<------SERVICIO NORMAL CON FECHA DE TERMINO ESTABLECIDA
            End If

            reasigno = True

        Catch ex As Exception
            reasigno = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If Not IsNothing(tran) Then
                If reasigno Then
                    tran.Commit()
                Else
                    tran.Rollback()
                End If
                tran.Dispose()
            End If
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(reasigno) : Catch ex As Exception : End Try
            End If
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try

        Dim enviaCorreo As New Notificacion

        If reasigno AndAlso tipoReagenda = Tipo.Reasignar Then
            enviaCorreo.CorreoNotificacionReasignacion(19, idSolicitud, idNivelServicio, IngenieroResponsable, IngenieroActual)
        ElseIf reasigno AndAlso tipoReagenda = Tipo.Reagendar Then
            enviaCorreo.CorreoNotificacionReasignacion(20, idSolicitud, idNivelServicio, IngenieroActual)
        End If

        Return reasigno

    End Function


#Region "ObtenerFechas"

    Public Shared Function ValidaFechaFinalDefinitiva(ByVal fechaValidar As DateTime, ByVal ingeniero As String) As DateTime
        Dim hora As Integer = fechaValidar.Hour
        Dim dia As Date = fechaValidar.Date
        'VALIDA LA FECHA SELECCIONADA Y REGRESA UNA HORA Y DIA CORRECTOS PARA ESTABLECERLOS COMO FECHA INICIAL Y POSTERIORMENTE VALIDA CONTRA LA AGENDA
        ValidaDiasHoras(dia, hora)
        Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
        Dim fechaValida As DateTime = DateTime.Parse(fechaTemp)
        fechaValida = ValidaAgenda(fechaValida, ingeniero)
        Return fechaValida
    End Function

    Public Shared Sub DeterminarFechaInicio(ByRef objAgenda As Agenda, ByRef fechIni As DateTime)
        Dim dt As New DataTable
        Dim dtTemp As New DataTable
        Dim fechaF1 As DateTime? = Nothing
        Dim fech As String = String.Empty
        'OBTENER FECHA EN QUE SE LIBERA INGENIERO
        fechaF1 = ValidaFechaFinalDefinitiva(DateTime.Now.ToString("yyyy/MM/dd") & " " & DateTime.Now.AddHours(1).Hour.ToString & ":00:00", objAgenda.Ingeniero)
        fechIni = ValidaAgenda(fechaF1, objAgenda.Ingeniero)
    End Sub

    Public Shared Sub DeterminarFechaInicio(ByRef objAgenda As Agenda, ByRef fechIni As DateTime,
                                            ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction)
        Dim fechaF1 As DateTime? = Nothing
        'OBTENER FECHA EN QUE SE LIBERA ING_RESP
        fechaF1 = ValidaFechaFinal(DateTime.Now.ToString("yyyy/MM/dd") & " " & DateTime.Now.AddHours(1).Hour.ToString & ":00:00", objAgenda.Ingeniero, con, tran)
        fechIni = ValidaAgenda(fechaF1, objAgenda.Ingeniero, con, tran)
    End Sub

    Public Shared Function ValidaFechaFinalDefinitiva(ByVal fechaValidar As DateTime, ByVal ingeniero As String,
                                                      ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As DateTime
        Dim hora As Integer = fechaValidar.Hour
        Dim dia As Date = fechaValidar.Date
        'VALIDA LA FECHA SELECCIONADA Y REGRESA UNA HORA Y DIA CORRECTOS PARA ESTABLECERLOS COMO FECHA INICIAL Y POSTERIORMENTE VALIDA CONTRA LA AGENDA
        ValidaDiasHorasFinal(dia, hora)
        Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
        Dim fechaValida As DateTime = DateTime.Parse(fechaTemp)
        fechaValida = ValidaAgenda(fechaValida, ingeniero, con, tran)
        Return fechaValida
    End Function

    Public Shared Function ValidaFechaFinal(ByVal fechaValidar As DateTime, ByVal ingeniero As String,
                                            ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As DateTime
        Dim hora As Integer = fechaValidar.Hour
        Dim dia As Date = fechaValidar.Date
        'VALIDA LA FECHA SELECCIONADA Y REGRESA UNA HORA Y DIA CORRECTOS PARA ESTABLECERLOS COMO FECHA INICIAL Y POSTERIORMENTE VALIDA CONTRA LA AGENDA
        ValidaDiasHoras(dia, hora)
        Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
        Dim fechaValida As DateTime = DateTime.Parse(fechaTemp)
        fechaValida = ValidaAgenda(fechaValida, ingeniero, con, tran)
        Return fechaValida
    End Function

    Public Shared Function ValidaAgenda(ByVal fecha As DateTime, ByVal ingeniero As String) As DateTime
        Dim hora As Integer
        Dim dia As Date
        Dim EstaDisponibleAg As Boolean = Agenda.FechaDisponibleAgenda(fecha, ingeniero)
        Do While Not EstaDisponibleAg
            hora = DateTime.Parse(fecha).AddHours(1).Hour
            dia = DateTime.Parse(fecha).Date
            ValidaDiasHoras(dia, hora)
            Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
            fecha = DateTime.Parse(fechaTemp)
            EstaDisponibleAg = Agenda.FechaDisponibleAgenda(fecha, ingeniero)
        Loop
        Return fecha
    End Function

    Public Shared Function ValidaAgenda(ByVal fecha As DateTime, ByVal ingeniero As String,
                                        ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As DateTime
        Dim hora As Integer
        Dim dia As Date
        Dim EstaDisponibleAg As Boolean = Agenda.FechaDisponibleAgenda(fecha, ingeniero, con, tran)
        Do While Not EstaDisponibleAg
            hora = DateTime.Parse(fecha).AddHours(1).Hour
            dia = DateTime.Parse(fecha).Date
            ValidaDiasHoras(dia, hora)
            Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
            fecha = DateTime.Parse(fechaTemp)
            EstaDisponibleAg = Agenda.FechaDisponibleAgenda(fecha, ingeniero, con, tran)
        Loop
        Return fecha
    End Function

    Public Shared Sub ValidaDiasHorasFinal(ByRef diaFin As Date, ByRef horaFin As Integer)
        If horaFin > 14 And horaFin < 16 Then
            horaFin = 17
        ElseIf horaFin > 19 Then
            horaFin = 9
            diaFin = diaFin.AddDays(1)
        End If

        Dim EsDiaHabil As Boolean = Negocio.Agendar.EsDiaHabil(diaFin)
        Do While Not EsDiaHabil
            diaFin = diaFin.AddDays(1)
            EsDiaHabil = Negocio.Agendar.EsDiaHabil(diaFin)
        Loop
    End Sub

    Public Shared Sub ValidaDiasHoras(ByRef diaIni As Date, ByRef horaIni As Integer)
        If horaIni > 13 And horaIni < 16 Then
            horaIni = 16
        ElseIf horaIni > 18 And horaIni < 24 Then
            horaIni = 9
            diaIni = diaIni.AddDays(1)
        ElseIf horaIni >= 0 And horaIni < 9 Then
            horaIni = 9
        End If

        Dim EsDiaHabil As Boolean = Negocio.Agendar.EsDiaHabil(diaIni)
        Do While Not EsDiaHabil
            diaIni = diaIni.AddDays(1)
            EsDiaHabil = Negocio.Agendar.EsDiaHabil(diaIni)
        Loop
    End Sub
#End Region

End Class
