
Imports System.Data.SqlClient

Public Class Registro_OPI
    Inherits OPI_Incumplimiento
    Const _tblSubentidad = "BDS_D_OPI_REL_SUBENTIDADES"
    Const _tblSupervisores = "BDS_D_OPI_SUPERVISORES_ASIGNADOS"
    Const _tblIspectores = "BDS_D_OPI_INSPECTORES_ASIGNADOS"
    Const _tblComentario = "BDS_D_OPI_COMENTARIOS_PASOS"

    ''' <summary>
    ''' Inserta un registro
    ''' </summary>
    ''' <returns>True si se incerto correctamente, de lo contrario false</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function AgregarOPI(ByRef FolioOPI) As Boolean

        Dim _save As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listCamposSub As New List(Of String)

        Dim listCamposSupervisores As New List(Of String)
        Dim listValoresSupervisores As New List(Of Object)

        Dim listCamposInspectores As New List(Of String)
        Dim listValoresInspectores As New List(Of String)

        Dim listValores As New List(Of Object)
        Dim listValoresSub As New List(Of Object)
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing

        Dim lstCampoCond As New List(Of String)
        Dim lstValCondicion As New List(Of Object)
        Dim _id_OPI As Integer
        Dim _id_OPI_Identity As Integer
        Dim IdArea As Integer = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea

        _id_OPI = ObtenerSiguienteIdentificador()

        Me.T_ID_FOLIO = _id_OPI.ToString("000") & Me.T_ID_FOLIO 'Me.I_ID_OPI.ToString("000") & Me.T_ID_FOLIO
        FolioOPI = Me.T_ID_FOLIO
        listCampos.Add("T_ID_FOLIO") : listValores.Add(Me.T_ID_FOLIO)
        listCampos.Add("N_ID_PASO") : listValores.Add(Me.N_ID_PASO)
        listCampos.Add("N_ID_SUBPASO") : listValores.Add(Me.N_ID_SUBPASO)
        listCampos.Add("I_ID_TIPO_ENTIDAD") : listValores.Add(Me.I_ID_TIPO_ENTIDAD)
        listCampos.Add("I_ID_ENTIDAD") : listValores.Add(Me.I_ID_ENTIDAD)
        listCampos.Add("F_FECH_POSIBLE_INC") : listValores.Add(Me.F_FECH_POSIBLE_INC)
        listCampos.Add("I_ID_PROCESO_POSIBLE_INC") : listValores.Add(Me.I_ID_PROCESO_POSIBLE_INC)
        listCampos.Add("I_ID_SUBPROCESO") : listValores.Add(Me.I_ID_SUBPROCESO)
        listCampos.Add("T_DSC_POSIBLE_INC") : listValores.Add(Me.T_DSC_POSIBLE_INC)
        listCampos.Add("T_OBSERVACIONES_OPI") : listValores.Add(Me.T_OBSERVACIONES_OPI)
        listCampos.Add("I_ID_ESTATUS") : listValores.Add(Me.I_ID_ESTATUS)
        listCampos.Add("I_ID_AREA") : listValores.Add(Me.I_ID_AREA)
        listCampos.Add("F_FECH_PASO_ACTUAL") : listValores.Add(DateTime.Today)
        listCampos.Add("N_ID_PASO_ANT") : listValores.Add(0)
        listCampos.Add("I_ID_SUBENTIDAD") : listValores.Add(-1)

        If Not Me.T_DSC_CLASIFICACION Is Nothing AndAlso Not String.IsNullOrEmpty(Me.T_DSC_CLASIFICACION) Then
            listCampos.Add("T_DSC_CLASIFICACION") : listValores.Add(Me.T_DSC_CLASIFICACION)
        End If
        If Not Me.T_DSC_RESP_AFORE Is Nothing AndAlso Not String.IsNullOrEmpty(Me.T_DSC_RESP_AFORE) Then
            listCampos.Add("T_DSC_RESP_AFORE") : listValores.Add(Me.T_DSC_RESP_AFORE)
        End If
        If Not Me.F_FECH_ESTIM_ENTREGA Is Nothing AndAlso Not String.IsNullOrEmpty(Me.F_FECH_ESTIM_ENTREGA) Then
            listCampos.Add("F_FECH_ESTIM_ENTREGA") : listValores.Add(Me.F_FECH_ESTIM_ENTREGA)
        End If
        If Not Me.F_FECH_ACUSE_DOCTO Is Nothing AndAlso Not String.IsNullOrEmpty(Me.F_FECH_ACUSE_DOCTO) Then
            listCampos.Add("F_FECH_ACUSE_DOCTO") : listValores.Add(Me.F_FECH_ACUSE_DOCTO)
        End If

        Try
            conexion = New Conexion.SQLServer
            tran = conexion.BeginTransaction()
            bitacora = New Conexion.Bitacora("Alta de registro posible incumplimiento", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            _save = conexion.InsertarConTransaccion(Tabla, listCampos, listValores, tran, _id_OPI_Identity)
            If _id_OPI_Identity > 0 Then
                Me.I_ID_OPI = _id_OPI_Identity
            Else
                _save = False
            End If

            Dim _sUsuario As String = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario

            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

            'registra en la bitacora
            Dim paso As Integer = "1"

            'aqui guarda 
            BitacoraOPI.AgregarEntrada(I_ID_OPI, _sUsuario, BitacoraOPI.ObtenerDCSPaso(paso), "Registro de oficio", Me.T_OBSERVACIONES_OPI)

            If _save AndAlso Not Me.I_ID_SUBENTIDAD Is Nothing AndAlso Me.I_ID_SUBENTIDAD.Count > 0 Then

                For i = 0 To Me.I_ID_SUBENTIDAD.Count - 1
                    _id_OPI += 1
                    'listValores.Item(0) = _id_OPI 'listValores.Item(0) + 1
                    'listValores.Item(1) = Me.T_ID_FOLIO & "/SB" & Me.I_ID_SUBENTIDAD(i).ToString()
                    listValores.Item(0) = Me.T_ID_FOLIO & "/SB" & Me.I_ID_SUBENTIDAD(i).ToString()
                    _save = conexion.InsertarConTransaccion(Tabla, listCampos, listValores, tran, _id_OPI_Identity)

                    bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

                    If _save And Me.T_ID_SUPERVISORES.Count > 0 Then
                        listCamposSub.Clear()
                        listValoresSub.Clear()
                        listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(_id_OPI_Identity)
                        'listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(listValores.Item(0))
                        listCamposSub.Add("T_ID_SUPERVISOR_ASIGNADO") : listValoresSub.Add("")
                        For Each sup In Me.T_ID_SUPERVISORES.Keys
                            listValoresSub.Item(1) = sup.ToString()
                            _save = _save And conexion.InsertarConTransaccion(_tblSupervisores, listCamposSub, listValoresSub, tran)
                            bitacora.Insertar(_tblSupervisores, listCamposSub, listValoresSub, _save, "")
                        Next
                    End If

                    If _save And Me.T_ID_INSPECTORES.Count > 0 Then
                        listCamposSub.Clear()
                        listValoresSub.Clear()
                        listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(_id_OPI_Identity)
                        'listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(listValores.Item(0))
                        listCamposSub.Add("T_ID_INSPECTOR_ASIGNADO") : listValoresSub.Add("")
                        For Each ins In Me.T_ID_INSPECTORES.Keys
                            listValoresSub.Item(1) = ins.ToString()
                            _save = _save And conexion.InsertarConTransaccion(_tblIspectores, listCamposSub, listValoresSub, tran)
                            bitacora.Insertar(_tblIspectores, listCamposSub, listValoresSub, _save, "")
                        Next
                    End If
                Next

            End If

            If _save And Me.T_ID_SUPERVISORES.Count > 0 Then
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
                listCampos.Add("T_ID_SUPERVISOR_ASIGNADO") : listValores.Add("")
                For Each sup In Me.T_ID_SUPERVISORES.Keys
                    listValores.Item(1) = sup.ToString()
                    _save = _save And conexion.InsertarConTransaccion(_tblSupervisores, listCampos, listValores, tran)
                    bitacora.Insertar(_tblSupervisores, listCampos, listValores, _save, "")
                Next

            End If
            If _save And Me.T_ID_INSPECTORES.Count > 0 Then
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
                listCampos.Add("T_ID_INSPECTOR_ASIGNADO") : listValores.Add("")

                For Each ins In Me.T_ID_INSPECTORES.Keys
                    listValores.Item(1) = ins.ToString()
                    _save = _save And conexion.InsertarConTransaccion(_tblIspectores, listCampos, listValores, tran)
                    bitacora.Insertar(_tblIspectores, listCampos, listValores, _save, "")
                Next

            End If

            If _save Then
                _save = _save And conexion.EjecutarConTransaccion("UPDATE [BDS_D_PC_OPI_CONSECUTIVOS] SET I_CONSECUTIVO = " & _id_OPI &
                                                                  " WHERE I_ID_AREA = " & IdArea.ToString() & " AND PROGRAMA = 'OPI' " &
                                                                  " AND YEAR(F_AÑO_PRESENTE) = YEAR(GETDATE())", tran) '.ActualizarConTransaccion("BDS_D_PC_OPI_CONSECUTIVOS", listCampos, listValores, lstCampoCond, lstValCondicion, tran)
            End If

        Catch ex As Exception
            _save = False
            tran.Rollback()
            conexion.Ejecutar("declare @max as int select @max  = Max(I_ID_OPI) from [BDS_D_OPI_INCUMPLIMIENTO] DBCC CHECKIDENT ('BDS_D_OPI_INCUMPLIMIENTO',RESEED,@max)")
            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally

            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(_save) : Catch : End Try
            End If
            If Not IsNothing(tran) Then
                If _save Then
                    'Registro exitoso
                    tran.Commit()
                    conexion.Ejecutar("declare @max as int select @max  = Max(I_ID_OPI) from [BDS_D_OPI_INCUMPLIMIENTO] DBCC CHECKIDENT ('BDS_D_OPI_INCUMPLIMIENTO',RESEED,@max)")
                Else
                    tran.Rollback()
                    conexion.Ejecutar("declare @max as int select @max  = Max(I_ID_OPI) from [BDS_D_OPI_INCUMPLIMIENTO] DBCC CHECKIDENT ('BDS_D_OPI_INCUMPLIMIENTO',RESEED,@max)")
                End If
                tran.Dispose()
            End If

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return _save

    End Function

    Public Function Agregar(ByRef strFoliosOPi As String) As Boolean

        Dim _save As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listCamposSub As New List(Of String)

        Dim listCamposSupervisores As New List(Of String)
        Dim listValoresSupervisores As New List(Of Object)

        Dim listCamposInspectores As New List(Of String)
        Dim listValoresInspectores As New List(Of String)

        Dim listValores As New List(Of Object)
        Dim listValoresSub As New List(Of Object)
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing

        Dim lstCampoCond As New List(Of String)
        Dim lstValCondicion As New List(Of Object)
        Dim _id_OPI As Integer
        Dim _id_OPI_Identity As Integer
        Dim IdArea As Integer = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea

        _id_OPI = ObtenerSiguienteIdentificador()

        'Me.I_ID_OPI = _id_OPI 'ObtenerSiguienteIdentificador()
        Me.T_ID_FOLIO = _id_OPI.ToString("000") & Me.T_ID_FOLIO 'Me.I_ID_OPI.ToString("000") & Me.T_ID_FOLIO
        strFoliosOPi = Me.T_ID_FOLIO
        'listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
        listCampos.Add("T_ID_FOLIO") : listValores.Add(Me.T_ID_FOLIO)
        listCampos.Add("N_ID_PASO") : listValores.Add(Me.N_ID_PASO)
        listCampos.Add("N_ID_SUBPASO") : listValores.Add(Me.N_ID_SUBPASO)
        listCampos.Add("I_ID_TIPO_ENTIDAD") : listValores.Add(Me.I_ID_TIPO_ENTIDAD)
        listCampos.Add("I_ID_ENTIDAD") : listValores.Add(Me.I_ID_ENTIDAD)
        listCampos.Add("F_FECH_POSIBLE_INC") : listValores.Add(Me.F_FECH_POSIBLE_INC)
        listCampos.Add("I_ID_PROCESO_POSIBLE_INC") : listValores.Add(Me.I_ID_PROCESO_POSIBLE_INC)
        listCampos.Add("I_ID_SUBPROCESO") : listValores.Add(Me.I_ID_SUBPROCESO)
        listCampos.Add("T_DSC_POSIBLE_INC") : listValores.Add(Me.T_DSC_POSIBLE_INC)
        listCampos.Add("T_OBSERVACIONES_OPI") : listValores.Add(Me.T_OBSERVACIONES_OPI)
        listCampos.Add("I_ID_ESTATUS") : listValores.Add(Me.I_ID_ESTATUS)
        listCampos.Add("I_ID_AREA") : listValores.Add(Me.I_ID_AREA)
        listCampos.Add("F_FECH_PASO_ACTUAL") : listValores.Add(DateTime.Today)
        listCampos.Add("N_ID_PASO_ANT") : listValores.Add(0)
        listCampos.Add("I_ID_SUBENTIDAD") : listValores.Add(I_ID_SUBENTIDAD_SB)

        If Not Me.T_DSC_CLASIFICACION Is Nothing AndAlso Not String.IsNullOrEmpty(Me.T_DSC_CLASIFICACION) Then
            listCampos.Add("T_DSC_CLASIFICACION") : listValores.Add(Me.T_DSC_CLASIFICACION)
        End If
        If Not Me.T_DSC_RESP_AFORE Is Nothing AndAlso Not String.IsNullOrEmpty(Me.T_DSC_RESP_AFORE) Then
            listCampos.Add("T_DSC_RESP_AFORE") : listValores.Add(Me.T_DSC_RESP_AFORE)
        End If
        If Not Me.F_FECH_ESTIM_ENTREGA Is Nothing AndAlso Not String.IsNullOrEmpty(Me.F_FECH_ESTIM_ENTREGA) Then
            listCampos.Add("F_FECH_ESTIM_ENTREGA") : listValores.Add(Me.F_FECH_ESTIM_ENTREGA)
        End If
        If Not Me.F_FECH_ACUSE_DOCTO Is Nothing AndAlso Not String.IsNullOrEmpty(Me.F_FECH_ACUSE_DOCTO) Then
            listCampos.Add("F_FECH_ACUSE_DOCTO") : listValores.Add(Me.F_FECH_ACUSE_DOCTO)
        End If

        Try
            conexion = New Conexion.SQLServer
            tran = conexion.BeginTransaction()
            bitacora = New Conexion.Bitacora("Alta de registro posible incumplimiento", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            _save = conexion.InsertarConTransaccion(Tabla, listCampos, listValores, tran, _id_OPI_Identity)
            If _id_OPI_Identity > 0 Then
                Me.I_ID_OPI = _id_OPI_Identity
            Else
                _save = False
            End If

            Dim _sUsuario As String = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario

            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

            'registra en la bitacora
            Dim paso As Integer = "1"

            'aqui guarda 
            BitacoraOPI.AgregarEntrada(I_ID_OPI, _sUsuario, BitacoraOPI.ObtenerDCSPaso(paso), "Registro de oficio", Me.T_OBSERVACIONES_OPI)

            If _save AndAlso Not Me.I_ID_SUBENTIDAD Is Nothing AndAlso Me.I_ID_SUBENTIDAD.Count > 0 Then

                For i = 0 To Me.I_ID_SUBENTIDAD.Count - 1
                    _id_OPI += 1
                    'listValores.Item(0) = _id_OPI 'listValores.Item(0) + 1
                    'listValores.Item(1) = Me.T_ID_FOLIO & "/SB" & Me.I_ID_SUBENTIDAD(i).ToString()
                    'listValores.Item(0) = Me.T_ID_FOLIO & "/SB" & Me.I_ID_SUBENTIDAD(i).ToString()
                    listValores.Item(0) = _id_OPI.ToString("000") + Me.T_ID_FOLIO.Substring(3, Me.T_ID_FOLIO.Length - 3) & "/SB" & Me.I_ID_SUBENTIDAD(i).ToString()
                    strFoliosOPi = strFoliosOPi & ", " & _id_OPI.ToString("000") + Me.T_ID_FOLIO.Substring(3, Me.T_ID_FOLIO.Length - 3) & "/SB" & Me.I_ID_SUBENTIDAD(i).ToString()
                    _save = conexion.InsertarConTransaccion(Tabla, listCampos, listValores, tran, _id_OPI_Identity)

                    bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

                    If _save And Me.T_ID_SUPERVISORES.Count > 0 Then
                        listCamposSub.Clear()
                        listValoresSub.Clear()
                        listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(_id_OPI_Identity)
                        'listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(listValores.Item(0))
                        listCamposSub.Add("T_ID_SUPERVISOR_ASIGNADO") : listValoresSub.Add("")
                        For Each sup In Me.T_ID_SUPERVISORES.Keys
                            listValoresSub.Item(1) = sup.ToString()
                            _save = _save And conexion.InsertarConTransaccion(_tblSupervisores, listCamposSub, listValoresSub, tran)
                            bitacora.Insertar(_tblSupervisores, listCamposSub, listValoresSub, _save, "")
                        Next
                    End If

                    If _save And Me.T_ID_INSPECTORES.Count > 0 Then
                        listCamposSub.Clear()
                        listValoresSub.Clear()
                        listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(_id_OPI_Identity)
                        'listCamposSub.Add("I_ID_OPI") : listValoresSub.Add(listValores.Item(0))
                        listCamposSub.Add("T_ID_INSPECTOR_ASIGNADO") : listValoresSub.Add("")
                        For Each ins In Me.T_ID_INSPECTORES.Keys
                            listValoresSub.Item(1) = ins.ToString()
                            _save = _save And conexion.InsertarConTransaccion(_tblIspectores, listCamposSub, listValoresSub, tran)
                            bitacora.Insertar(_tblIspectores, listCamposSub, listValoresSub, _save, "")
                        Next
                    End If
                Next

            End If

            If _save And Me.T_ID_SUPERVISORES.Count > 0 Then
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
                listCampos.Add("T_ID_SUPERVISOR_ASIGNADO") : listValores.Add("")
                For Each sup In Me.T_ID_SUPERVISORES.Keys
                    listValores.Item(1) = sup.ToString()
                    _save = _save And conexion.InsertarConTransaccion(_tblSupervisores, listCampos, listValores, tran)
                    bitacora.Insertar(_tblSupervisores, listCampos, listValores, _save, "")
                Next

            End If
            If _save And Me.T_ID_INSPECTORES.Count > 0 Then
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
                listCampos.Add("T_ID_INSPECTOR_ASIGNADO") : listValores.Add("")

                For Each ins In Me.T_ID_INSPECTORES.Keys
                    listValores.Item(1) = ins.ToString()
                    _save = _save And conexion.InsertarConTransaccion(_tblIspectores, listCampos, listValores, tran)
                    bitacora.Insertar(_tblIspectores, listCampos, listValores, _save, "")
                Next

            End If

            If _save Then
                _save = _save And conexion.EjecutarConTransaccion("UPDATE [BDS_D_PC_OPI_CONSECUTIVOS] SET I_CONSECUTIVO = " & _id_OPI &
                                                                  " WHERE I_ID_AREA = " & IdArea.ToString() & " AND PROGRAMA = 'OPI' " &
                                                                  " AND YEAR(F_AÑO_PRESENTE) = YEAR(GETDATE())", tran) '.ActualizarConTransaccion("BDS_D_PC_OPI_CONSECUTIVOS", listCampos, listValores, lstCampoCond, lstValCondicion, tran)
            End If

        Catch ex As Exception
            _save = False
            tran.Rollback()
            conexion.Ejecutar("declare @max as int select @max  = Max(I_ID_OPI) from [BDS_D_OPI_INCUMPLIMIENTO] DBCC CHECKIDENT ('BDS_D_OPI_INCUMPLIMIENTO',RESEED,@max)")
            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally

            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(_save) : Catch : End Try
            End If
            If Not IsNothing(tran) Then
                If _save Then
                    'Registro exitoso
                    tran.Commit()
                    conexion.Ejecutar("declare @max as int select @max  = Max(I_ID_OPI) from [BDS_D_OPI_INCUMPLIMIENTO] DBCC CHECKIDENT ('BDS_D_OPI_INCUMPLIMIENTO',RESEED,@max)")
                Else
                    tran.Rollback()
                    conexion.Ejecutar("declare @max as int select @max  = Max(I_ID_OPI) from [BDS_D_OPI_INCUMPLIMIENTO] DBCC CHECKIDENT ('BDS_D_OPI_INCUMPLIMIENTO',RESEED,@max)")
                End If
                tran.Dispose()
            End If

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return _save

    End Function

    Public Function Guardar(_id_OPI As Integer, listCampos As List(Of String), listValores As List(Of Object), Optional _comentarios_Paso As String = Nothing) As Boolean

        Dim _save As Boolean = False
        'Dim listCampos As New List(Of String)
        Dim lstCampoCond As New List(Of String)
        Dim _lstChildFields As New List(Of String)
        'Dim listValores As New List(Of Object)
        Dim lstValCondicion As New List(Of Object)
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing


        lstCampoCond.Add("I_ID_OPI") : lstValCondicion.Add(_id_OPI)

        Try
            conexion = New Conexion.SQLServer
            tran = conexion.BeginTransaction()

            Dim _pasoNo As Int16
            Dim _subpaso As Int16
            Dim _estatus As Int16

            _pasoNo = listCampos.IndexOf("N_ID_PASO")
            _pasoNo = listValores(_pasoNo).ToString()

            _subpaso = listCampos.IndexOf("N_ID_SUBPASO")
            _subpaso = listValores(_subpaso).ToString()

            _estatus = listCampos.IndexOf("I_ID_ESTATUS")
            _estatus = listValores(_estatus).ToString()

            bitacora = New Conexion.Bitacora("Registro del Paso " & _pasoNo & " Subpaso " & _subpaso & " del posible incumplimiento", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            _save = conexion.ActualizarConTransaccion(Me.Tabla, listCampos, listValores, lstCampoCond, lstValCondicion, tran)

            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

            If _save AndAlso Not String.IsNullOrEmpty(_comentarios_Paso) Then
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(_id_OPI)
                listCampos.Add("N_ID_PASO") : listValores.Add(_pasoNo)
                listCampos.Add("I_ID_ESTATUS") : listValores.Add(_estatus)
                listCampos.Add("T_DSC_COMENTARIOS") : listValores.Add(_comentarios_Paso.Trim())
                _save = _save And conexion.InsertarConTransaccion(_tblComentario, listCampos, listValores, tran)
                bitacora.Insertar(_tblComentario, listCampos, listValores, _save, "")

            End If


        Catch ex As Exception
            _save = False
            tran.Rollback()
            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(_save) : Catch : End Try
            End If
            If Not IsNothing(tran) Then
                If _save Then
                    'Registro exitoso
                    tran.Commit()
                Else
                    tran.Rollback()
                End If
                tran.Dispose()
            End If
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return _save

    End Function
    'EC-Juan.Jose.Velazquez
    Public Function ActualizarEstatus(_id_OPI As Integer, listCampos As List(Of String), listValores As List(Of Object), Optional _comentarios_Paso As String = Nothing) As Boolean

        Dim _save As Boolean = False
        'Dim listCampos As New List(Of String)
        Dim lstCampoCond As New List(Of String)
        Dim _lstChildFields As New List(Of String)
        'Dim listValores As New List(Of Object)
        Dim lstValCondicion As New List(Of Object)
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing


        lstCampoCond.Add("I_ID_OPI") : lstValCondicion.Add(_id_OPI)

        Try
            conexion = New Conexion.SQLServer
            tran = conexion.BeginTransaction()

            Dim _pasoNo As Int16
            Dim _subpaso As Int16
            Dim _estatus As Int16

            _pasoNo = listCampos.IndexOf("N_ID_PASO")
            _pasoNo = listValores(_pasoNo).ToString()

            _subpaso = listCampos.IndexOf("N_ID_SUBPASO")
            _subpaso = listValores(_subpaso).ToString()

            _estatus = listCampos.IndexOf("I_ID_ESTATUS")
            _estatus = listValores(_estatus).ToString()

            bitacora = New Conexion.Bitacora("Cancelación de OPI en Paso " & _pasoNo & " Subpaso " & _subpaso, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            _save = conexion.ActualizarConTransaccion(Me.Tabla, listCampos, listValores, lstCampoCond, lstValCondicion, tran)

            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

        Catch ex As Exception
            _save = False
            tran.Rollback()
            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(_save) : Catch : End Try
            End If
            If Not IsNothing(tran) Then
                If _save Then
                    'Registro exitoso
                    tran.Commit()
                Else
                    tran.Rollback()
                End If
                tran.Dispose()
            End If
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return _save

    End Function
    Public Function ActualizarOPI() As Boolean

        Dim _save As Boolean = False
        Dim listCampos As New List(Of String)
        Dim lstCampoCond As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim lstValCondicion As New List(Of Object)
        'Dim tran As SqlClient.SqlTransaction = Nothing
        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing

        lstCampoCond.Add("I_ID_OPI") : lstValCondicion.Add(Me.I_ID_OPI)

        listCampos.Add("F_FECH_POSIBLE_INC") : listValores.Add(Me.F_FECH_POSIBLE_INC)
        listCampos.Add("I_ID_PROCESO_POSIBLE_INC") : listValores.Add(Me.I_ID_PROCESO_POSIBLE_INC)
        listCampos.Add("I_ID_SUBPROCESO") : listValores.Add(Me.I_ID_SUBPROCESO)
        listCampos.Add("T_DSC_POSIBLE_INC") : listValores.Add(Me.T_DSC_POSIBLE_INC)
        If Not Me.T_DSC_CLASIFICACION Is Nothing AndAlso Not String.IsNullOrEmpty(Me.T_DSC_CLASIFICACION) Then
            listCampos.Add("T_DSC_CLASIFICACION") : listValores.Add(Me.T_DSC_CLASIFICACION)
        End If

        Try
            conexion = New Conexion.SQLServer
            'tran = conexion.BeginTransaction()

            bitacora = New Conexion.Bitacora("Actualización de OPI en Paso ", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            _save = conexion.Actualizar(Me.Tabla, listCampos, listValores, lstCampoCond, lstValCondicion)

            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, "")

            If _save And Me.T_ID_SUPERVISORES.Count > 0 Then

                Dim CampoCondicion As New List(Of String)
                Dim ValorCondicion As New List(Of Object)

                CampoCondicion.Add("I_ID_OPI")
                ValorCondicion.Add(Me.I_ID_OPI)

                'Elimina los supervisores actuales antes de agregar
                conexion.Eliminar(_tblSupervisores, CampoCondicion, ValorCondicion)
                bitacora.EliminarConTransaccion(_tblSupervisores, CampoCondicion, ValorCondicion, _save, "")

                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
                listCampos.Add("T_ID_SUPERVISOR_ASIGNADO") : listValores.Add("")
                For Each sup In Me.T_ID_SUPERVISORES.Keys
                    listValores.Item(1) = sup.ToString()
                    _save = _save And conexion.Insertar(_tblSupervisores, listCampos, listValores)
                    bitacora.Insertar(_tblSupervisores, listCampos, listValores, _save, "")
                Next

            End If
            If _save And Me.T_ID_INSPECTORES.Count > 0 Then
                Dim CampoCondicion As New List(Of String)
                Dim ValorCondicion As New List(Of Object)

                CampoCondicion.Add("I_ID_OPI")
                ValorCondicion.Add(Me.I_ID_OPI)

                'Elimina los inspectores actuales antes de agregar
                conexion.Eliminar(_tblIspectores, CampoCondicion, ValorCondicion)
                'bitacora.Eliminar(_tblSupervisores, CampoCondicion, ValorCondicion, _save, "")

                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("I_ID_OPI") : listValores.Add(Me.I_ID_OPI)
                listCampos.Add("T_ID_INSPECTOR_ASIGNADO") : listValores.Add("")

                For Each ins In Me.T_ID_INSPECTORES.Keys
                    listValores.Item(1) = ins.ToString()
                    _save = _save And conexion.Insertar(_tblIspectores, listCampos, listValores)
                    bitacora.Insertar(_tblIspectores, listCampos, listValores, _save, "")
                Next

            End If

        Catch ex As Exception
            _save = False
            'tran.Rollback()
            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(_save) : Catch : End Try
            End If
            'If Not IsNothing(tran) Then
            '    If _save Then
            '        'Registro exitoso
            '        tran.Commit()
            '    Else
            '        tran.Rollback()
            '    End If
            '    tran.Dispose()
            'End If
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return _save

    End Function
    Public Function ActualizarSISAN(ListaCampos, ListaValores, listCamposCondicion, listValoresCondicion)
        Dim conexion As New Conexion.SQLServer()

        Return conexion.Actualizar("[dbo].[BDS_D_OPI_INCUMPLIMIENTO]", ListaCampos, ListaValores, listCamposCondicion, listValoresCondicion)
    End Function

    Public Function GuardarHistoricoProrrogas(strTabla As String, listCampos As List(Of String), listValores As List(Of Object)) As Boolean
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing
        Dim _save As Boolean

        Try
            conexion = New Conexion.SQLServer
            tran = conexion.BeginTransaction()
            bitacora = New Conexion.Bitacora("Alta en histórico de prorrogas", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            _save = conexion.InsertarConTransaccion(strTabla, listCampos, listValores, tran)
        Catch ex As Exception
            _save = False
            tran.Rollback()
            bitacora.Insertar(Me.Tabla, listCampos, listValores, _save, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(_save) : Catch : End Try
            End If
            If Not IsNothing(tran) Then
                If _save Then
                    'Registro exitoso
                    tran.Commit()
                Else
                    tran.Rollback()
                End If
                tran.Dispose()
            End If
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

        Return _save

    End Function

    Public Function GetOPIDetail(Id_Opi As Integer) As OPI_Incumplimiento
        Dim _opi As OPI_Incumplimiento
        Dim _lstaux As New Hashtable
        Dim _lstauxInt As New List(Of Integer)

        Dim ds As DataSet = Nothing
        Dim _tblAux As DataTable

        Dim con As Conexion.SQLServer = Nothing
        Try
            con = New Conexion.SQLServer

            Dim sp = "spS_BDS_OPI_GET_OPI_DETAIL"

            Dim parametro(0) As SqlParameter
            parametro(0) = New SqlParameter("@I_ID_OPI", Id_Opi)

            ds = con.EjecutarSPConsultaDS(sp, parametro)

            If ds.Tables(0).Rows.Count > 0 Then
                _opi = New OPI_Incumplimiento

                ''Trae los datos generales de la opi
                _tblAux = ds.Tables(0)
                _opi.I_ID_OPI = Id_Opi

                For Each row In _tblAux.Rows
                    _opi.T_ID_FOLIO = IIf(IsDBNull(row("T_ID_FOLIO")), String.Empty, row("T_ID_FOLIO"))
                    _opi.N_ID_PASO = row("N_ID_PASO")
                    _opi.N_ID_SUBPASO = IIf(IsDBNull(row("N_ID_SUBPASO")), 0, row("N_ID_SUBPASO"))
                    _opi.I_ID_TIPO_ENTIDAD = row("I_ID_TIPO_ENTIDAD")
                    _opi.I_ID_ENTIDAD = row("I_ID_ENTIDAD")
                    _opi.F_FECH_POSIBLE_INC = row("F_FECH_POSIBLE_INC")
                    _opi.I_ID_PROCESO_POSIBLE_INC = row("I_ID_PROCESO_POSIBLE_INC")
                    _opi.I_ID_SUBPROCESO = IIf(IsDBNull(row("I_ID_SUBPROCESO")), 0, row("I_ID_SUBPROCESO"))
                    _opi.T_DSC_POSIBLE_INC = row("T_DSC_POSIBLE_INC")
                    _opi.T_OBSERVACIONES_OPI = row("T_OBSERVACIONES_OPI")
                    _opi.F_FECH_REGISTRO = IIf(IsDBNull(row("F_FECH_REGISTRO")), Nothing, row("F_FECH_REGISTRO"))
                    _opi.T_DSC_CLASIFICACION = IIf(IsDBNull(row("T_DSC_CLASIFICACION")), String.Empty, row("T_DSC_CLASIFICACION"))
                    _opi.I_ID_ESTATUS = IIf(IsDBNull(row("I_ID_ESTATUS")), 0, row("I_ID_ESTATUS"))
                    _opi.T_DSC_ESTATUS = IIf(IsDBNull(row("T_DSC_ESTATUS")), String.Empty, row("T_DSC_ESTATUS"))
                    _opi.I_ID_SUPUESTO = IIf(IsDBNull(row("I_ID_SUPUESTO")), 0, row("I_ID_SUPUESTO"))
                    _opi.T_DSC_RESP_AFORE = IIf(IsDBNull(row("T_DSC_RESP_AFORE")), String.Empty, row("T_DSC_RESP_AFORE"))
                    _opi.B_EXISTE_IRREG = IIf(IsDBNull(row("B_EXISTE_IRREG")), False, row("B_EXISTE_IRREG"))
                    _opi.T_DSC_JUST_NO_IRREG = IIf(IsDBNull(row("T_DSC_JUST_NO_IRREG")), String.Empty, row("T_DSC_JUST_NO_IRREG"))
                    _opi.B_IRREG_STD = IIf(IsDBNull(row("B_IRREG_STD")), False, row("B_IRREG_STD"))
                    _opi.F_FECH_ESTIM_ENTREGA = IIf(IsDBNull(row("F_FECH_ESTIM_ENTREGA")), Nothing, row("F_FECH_ESTIM_ENTREGA"))
                    _opi.T_DSC_PROC_POSIB_INCUMP = IIf(IsDBNull(row("T_DSC_PROC_POSIB_INCUMP")), String.Empty, row("T_DSC_PROC_POSIB_INCUMP"))
                    _opi.T_DSC_MOTIV_NO_PROC = IIf(IsDBNull(row("T_DSC_MOTIV_NO_PROC")), String.Empty, row("T_DSC_MOTIV_NO_PROC"))
                    _opi.I_ID_AREA = IIf(IsDBNull(row("I_ID_AREA")), 0, row("I_ID_AREA"))
                    _opi.T_DSC_AREA = IIf(IsDBNull(row("T_DSC_AREA")), String.Empty, row("T_DSC_AREA"))
                    _opi.F_FECH_PASO_ACTUAL = IIf(IsDBNull(row("F_FECH_PASO_ACTUAL")), Nothing, row("F_FECH_PASO_ACTUAL"))
                    _opi.T_COMENTARIOS_PASOS = IIf(IsDBNull(row("T_DSC_COMENTARIOS_PASO_ACTUAL")), String.Empty, row("T_DSC_COMENTARIOS_PASO_ACTUAL"))
                    _opi.N_ID_PASO_ANT = IIf(IsDBNull(row("N_ID_PASO_ANT")), 0, row("N_ID_PASO_ANT"))
                    _opi.F_FECH_ACUSE_DOCTO = IIf(IsDBNull(row("F_FECH_ACUSE_DOCTO")), Nothing, row("F_FECH_ACUSE_DOCTO"))
                    _opi.B_POSIBLE_INC = IIf(IsDBNull(row("B_PROCEDE")), False, row("B_PROCEDE"))
                    _opi.B_PROCEDE = IIf(IsDBNull(row("B_PROCEDE")), False, row("B_PROCEDE"))
                    _opi.F_FECH_REUNION = IIf(IsDBNull(row("F_FECH_REUNION")), Nothing, row("F_FECH_REUNION"))
                    _opi.F_FECH_REUNION_REAL = IIf(IsDBNull(row("F_FECH_REUNION_REAL")), Nothing, row("F_FECH_REUNION_REAL"))
                    _opi.T_ID_FOLIO_SISAN = IIf(IsDBNull(row("T_ID_FOLIO_SISAN")), Nothing, row("T_ID_FOLIO_SISAN"))
                    _opi.I_ID_SUBENTIDAD_SB = IIf(IsDBNull(row("I_ID_SUBENTIDAD")), -1, row("I_ID_SUBENTIDAD"))
                    _opi.I_ID_ESTATUS_ANT = IIf(IsDBNull(row("I_ID_ESTATUS_ANT")), 0, row("I_ID_ESTATUS_ANT"))
                Next

                ''SUBENTIDADES
                _tblAux = ds.Tables(1)
                If _tblAux.Rows.Count > 0 Then
                    For Each se In _tblAux.Rows
                        _lstauxInt.Add(Integer.Parse(se("I_ID_SUBENTIDAD").ToString()))
                    Next
                End If

                _opi.I_ID_SUBENTIDAD = _lstauxInt

                ''SUPERVISORES
                _tblAux = ds.Tables(2)

                If _tblAux.Rows.Count > 0 Then
                    For Each sup In _tblAux.Rows
                        _lstaux.Add(sup("T_ID_SUPERVISOR_ASIGNADO").ToString, sup("NOMBRE_SUP").ToString())
                    Next
                End If
                _opi.T_ID_SUPERVISORES = _lstaux

                _lstaux = New Hashtable
                ''INSPECTORES
                _tblAux = ds.Tables(3)
                If _tblAux.Rows.Count > 0 Then
                    For Each ins In _tblAux.Rows
                        _lstaux.Add(ins("T_ID_INSPECTOR_ASIGNADO").ToString(), ins("NOMBRE_INSP").ToString())
                    Next
                End If
                _opi.T_ID_INSPECTORES = _lstaux

            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Registro_OPI, GetOPIDetail", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try


        Return _opi

    End Function



    ''' <summary>
    ''' Obtiene el siguiente id
    ''' </summary>
    ''' <returns>Entero con el siguiente identificador</returns>
    ''' <remarks></remarks>
    Private Function ObtenerSiguienteIdentificador() As Integer
        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer
        Dim IdArea As Integer = CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        Try

            'Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(I_ID_OPI) + 1) ID FROM " & Me.Tabla)
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(I_CONSECUTIVO) + 1) ID FROM [BDS_D_PC_OPI_CONSECUTIVOS]" &
                                                                     " WHERE I_ID_AREA = " & IdArea & " AND PROGRAMA = 'OPI' " &
                                                                     " AND YEAR(F_AÑO_PRESENTE) = YEAR(GETDATE())")
            If dr.Read() Then
                If IsDBNull(dr("ID")) Then
                    resultado = 1
                Else
                    resultado = Convert.ToInt32(dr("ID"))
                End If
            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return resultado
    End Function

    Public Shared Function ObtenerDiasVencimiento() As Integer
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE N_ID_PARAMETRO = '82'")
        conexion.CerrarConexion()
        Return Integer.Parse(data.Rows(0)("T_DSC_VALOR").ToString())

    End Function

    Public Function ObtenerNuevoFolioOPI(strFolio As String) As String
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT MAX(T_ID_FOLIO) MAXVALOR from BDS_D_OPI_INCUMPLIMIENTO Where T_ID_FOLIO like '%" & strFolio & "%'")
        conexion.CerrarConexion()

        'Return String.Format("{000}", Integer.Parse(data.Rows(0)("MAXVALOR").ToString())) & strFolio
        Return data.Rows(0)("MAXVALOR").ToString

    End Function


End Class
