Imports System.Data

Public Class Paso
    Private _idPaso As Integer
    Private _dscPaso As String
    Private _numDiasMin As Integer
    Private _numDiasMax As Integer
    Private _numDiasActualesEnVisita As Integer
    Private _ldFechaInicio As DateTime

    Public Property ExistePaso As Boolean
    Public Property EnProrroga As Integer
    Public Property EstatusPaso As Integer

    Public Property TieneProrroga As Boolean

    Public Property IdPaso() As Integer
        Get
            Return _idPaso
        End Get
        Set(ByVal value As Integer)
            _idPaso = value
        End Set
    End Property

    Public Property DscPaso() As String
        Get
            Return _dscPaso
        End Get
        Set(ByVal value As String)
            _dscPaso = value
        End Set
    End Property


    Public Property NumDiasMin() As Integer
        Get
            Return _numDiasMin
        End Get
        Set(ByVal value As Integer)
            _numDiasMin = value
        End Set
    End Property

    Public Property NumDiasMax() As Integer
        Get
            Return _numDiasMax
        End Get
        Set(ByVal value As Integer)
            _numDiasMax = value
        End Set
    End Property

    Public Property NumDiasActualesEnVisita() As Integer
        Get
            Return _numDiasActualesEnVisita
        End Get
        Set(ByVal value As Integer)
            _numDiasActualesEnVisita = value
        End Set
    End Property

    Public Property FechaInicioEnVisita() As DateTime
        Get
            Return _ldFechaInicio
        End Get
        Set(ByVal value As DateTime)
            _ldFechaInicio = value
        End Set
    End Property

    Public Function getPasos() As DataTable
        Dim con As Conexion.SQLServer = Nothing
        Dim dt As DataTable

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_CATALOGO_PASOS_V17")

            dt = con.EjecutarSPConsultaDT(sp)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "paso.vb, getPasos", "")
            dt = New DataTable
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return dt

    End Function


    Public Sub cargaPasoVisita(idVisita As Integer, piIdPaso As Integer, piIdArea As Integer)
        Dim con As Conexion.SQLServer = Nothing

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp As String = Nothing

            'Validar la fecha de registro de la visita
            Dim objVisita As New Visita()
            objVisita = AccesoBD.getDetalleVisita(idVisita, piIdArea)

            Dim fechaRegVisita As Date = CDate(objVisita.FechaRegistro.ToString())
            Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

            If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") > Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                If piIdArea = 36 Then
                    sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA_VF_V17")
                Else
                    sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA_V17")
                End If
            Else
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA")
            End If

            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", piIdPaso))

            Dim dt As DataTable
            dt = con.EjecutarSPConsultaDT(sp, SqlParameters.ToArray)
            ExistePaso = False

            If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        ExistePaso = True
                        For Each lrRow As DataRow In dt.Rows
                            Me.IdPaso = lrRow("I_ID_PASO")
                            Me.NumDiasActualesEnVisita = lrRow("DIAS_TRANSCURRIDOS_PASO_ACTUAL")
                            Me.NumDiasMin = lrRow("I_NUM_DURACION_MINIMA")
                            If Not (DateTime.TryParse(lrRow("F_FECH_INI_PASO"), Me.FechaInicioEnVisita)) Then
                                Me.FechaInicioEnVisita = DateTime.Now
                            End If
                            Me.NumDiasMax = lrRow("I_NUM_DURACION_MAXIMA")
                            Me.TieneProrroga = lrRow("B_FLAG_TIENE_PRORROGA")
                        Next
                    End If
                End If
            Else
                If piIdArea = 36 Then
                    If Not IsNothing(dt) Then
                        If dt.Rows.Count > 0 Then
                            ExistePaso = True
                            For Each lrRow As DataRow In dt.Rows
                                Me.IdPaso = lrRow("I_ID_PASO")
                                Me.NumDiasActualesEnVisita = lrRow("DIAS_TRANSCURRIDOS_PASO_ACTUAL")
                                Me.NumDiasMin = lrRow("I_NUM_DURACION_MINIMA_VF")
                                If Not (DateTime.TryParse(lrRow("F_FECH_INI_PASO"), Me.FechaInicioEnVisita)) Then
                                    Me.FechaInicioEnVisita = DateTime.Now
                                End If
                                Me.NumDiasMax = lrRow("I_NUM_DURACION_MAXIMA_VF")
                                Me.TieneProrroga = lrRow("B_FLAG_TIENE_PRORROGA")
                            Next
                        End If
                    End If
                Else
                    If Not IsNothing(dt) Then
                        If dt.Rows.Count > 0 Then
                            ExistePaso = True
                            For Each lrRow As DataRow In dt.Rows
                                Me.IdPaso = lrRow("I_ID_PASO")
                                Me.NumDiasActualesEnVisita = lrRow("DIAS_TRANSCURRIDOS_PASO_ACTUAL")
                                Me.NumDiasMin = lrRow("I_NUM_DURACION_MINIMA")
                                If Not (DateTime.TryParse(lrRow("F_FECH_INI_PASO"), Me.FechaInicioEnVisita)) Then
                                    Me.FechaInicioEnVisita = DateTime.Now
                                End If
                                Me.NumDiasMax = lrRow("I_NUM_DURACION_MAXIMA")
                                Me.TieneProrroga = lrRow("B_FLAG_TIENE_PRORROGA")
                            Next
                        End If
                    End If
                End If
            End If



        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try
    End Sub

    Public Sub cargaPasoActualVisita(idVisita As Integer)
        Dim con As Conexion.SQLServer = Nothing

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp As String = Nothing

            'Validar la fecha de registro de la visita
            Dim objVisita As New Visita()
            objVisita = AccesoBD.getDetalleVisita(idVisita, 35)

            Dim fechaRegVisita As Date = CDate(objVisita.FechaRegistro.ToString())
            Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

            If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") > Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA_V17")
            Else
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA")
            End If

            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))

            Dim dt As DataTable
            dt = con.EjecutarSPConsultaDT(sp, SqlParameters.ToArray)
            ExistePaso = False

            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    ExistePaso = True
                    For Each lrRow As DataRow In dt.Rows
                        Me.IdPaso = lrRow("I_ID_PASO")
                        Me.NumDiasActualesEnVisita = lrRow("DIAS_TRANSCURRIDOS_PASO_ACTUAL")
                        Me.NumDiasMin = lrRow("I_NUM_DURACION_MINIMA")
                        If Not (DateTime.TryParse(lrRow("F_FECH_INI_PASO"), Me.FechaInicioEnVisita)) Then
                            Me.FechaInicioEnVisita = DateTime.Now
                        End If
                        Me.NumDiasMax = lrRow("I_NUM_DURACION_MAXIMA")
                        Me.TieneProrroga = lrRow("B_FLAG_TIENE_PRORROGA")
                    Next
                End If
            End If


        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try
    End Sub
End Class
