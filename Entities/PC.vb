Imports System.Data.SqlClient

Public Class PC

    Public Property Identificador As Integer
    Public Property FechaRegistro As Date
    Public Property Remitente As String
    Public Property NumOficio As String
    Public Property FechaDocumento As Date
    Public Property FechaRecepcion As Date
    Public Property IdArea As Integer
    Public Property Area As String
    Public Property Asunto As String
    Public Property NombreFirmante As String
    Public Property PaternoFirmante As String
    Public Property MaternoFirmante As String
    Public Property Referencia As String
    Public Property TipoDocumento As String
    Public Property IdEstatus As Integer
    Public Property IdEstatusAnt As Integer
    Public Property IdPaso As Integer
    'Public Property [N_ID_FOLIO_SISAN]
    Public Property FolioSupervisar As String
    Public Property IdTipoEntidad As Integer
    Public Property IdEntidad As Integer
    Public Property IdSubEntidad As Integer
    Public Property IdSicod As Integer
    'Public Property [B_IS_SUBENTIDAD]
    Public Property NumPC As Integer
    Public Property IdProceso As Integer
    Public Property IdSubproceso As Integer
    Public Property Descripcion As String
    Public Property NumPCInterno As String
    Public Property FechaVencimiento As Date
    Public Property Cumple As Boolean?
    Public Property DescripcionNoCuemple As String
    'Public Property [N_ID_SUB_PERFIL]
    'Public Property [I_ID_PC_CUMPLE]
    Public Property IdResolucion As Integer
    Public Property DescripcionPaso As String
    Public Property Existe As Boolean = False
    Public Property DescripcionResolucion As String
    Public Property ComentariosAdicionales As String
    Public Property FolioSisan As String
    Public Property Supervisores As List(Of KeyValuePair(Of String, String))
    Public Property Inspectores As List(Of KeyValuePair(Of String, String))


    Public Sub New()

    End Sub

    Public Sub New(ByVal Folio As Integer)
        Me.Identificador = Folio
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
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_FOLIO") : listValores.Add(Me.Identificador)
            'listCampos.Add("N_ID_SICOD") : listValores.Add(Me.Identificador)

            Try


                Existe = conexion.BuscarUnRegistro("BDS_D_PC_PROGRAMA_CORRECCION", listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR("BDS_D_PC_PROGRAMA_CORRECCION", listCampos, listValores)

                    If dr.Read() Then

                        Me.NumOficio = CStr(dr("T_DSC_NUM_OFICIO"))
                        Me.FechaDocumento = CDate(dr("F_FECH_DOC"))
                        Me.FechaRecepcion = CDate(dr("F_FECH_RECEPCION"))
                        Me.Area = CStr(dr("T_DSC_UNIDAD_ADM"))
                        Me.Asunto = CStr(dr("T_DSC_ASUNTO"))
                        Me.NombreFirmante = CStr(dr("T_DSC_NOMB_FIRMNT"))
                        Me.PaternoFirmante = CStr(dr("T_DSC_AP_PAT_FIRMNT"))
                        Me.MaternoFirmante = CStr(dr("T_DSC_AP_MAT_FIRMNT"))
                        Me.Referencia = CStr(dr("T_DSC_REFERENCIA"))
                        Me.TipoDocumento = CStr(dr("T_DSC_T_DOC"))
                        Me.IdEstatus = CInt(dr("I_ID_ESTATUS"))
                        Me.IdEstatusAnt = IIf(IsDBNull(dr("I_ID_ESTATUS_ANT")), 0, dr("I_ID_ESTATUS_ANT"))
                        Me.IdPaso = CInt(dr("N_ID_PASO"))
                        'Public Property [N_ID_FOLIO_SISAN]
                        Me.FolioSupervisar = CStr(dr("I_ID_FOLIO_SUPERVISAR"))
                        If IsDBNull(dr("ID_ENTIDAD")) Then
                            Me.IdEntidad = 0
                        Else
                            Me.IdEntidad = CInt(dr("ID_ENTIDAD"))
                        End If

                        'Me.IdEntidad = IIf(IsDBNull(dr("ID_ENTIDAD")), 0, CInt(dr("ID_ENTIDAD")))

                        If IsDBNull(dr("I_ID_TIPO_ENTIDAD")) Then
                            Me.IdTipoEntidad = 0
                        Else
                            Me.IdTipoEntidad = CInt(dr("I_ID_TIPO_ENTIDAD"))
                        End If

                        If IsDBNull(dr("I_ID_SUBENTIDAD")) Then
                            Me.IdSubEntidad = 0
                        Else
                            Me.IdSubEntidad = CInt(dr("I_ID_SUBENTIDAD"))
                        End If

                        'Me.IdTipoEntidad = IIf(IsDBNull(dr("I_ID_TIPO_ENTIDAD")), 0, CInt(dr("I_ID_TIPO_ENTIDAD")))
                        Me.IdSicod = CInt(dr("N_ID_SICOD"))
                        Me.IdArea = CInt(dr("I_ID_AREA"))
                        'Public Property [B_IS_SUBENTIDAD]
                        'Me.NumPC = CStr(dr("I_NUM_PROGRAMA_CORRECION"))
                        If Not IsDBNull(dr("I_ID_PC_CUMPLE")) Then
                            Me.Cumple = dr("I_ID_PC_CUMPLE")
                        Else
                            Me.Cumple = False
                        End If

                        Me.IdProceso = CInt(dr("I_ID_PROCESO"))
                        Me.IdSubproceso = CInt(dr("I_ID_SUBPROCESO"))
                        Me.Descripcion = CStr(dr("T_DSC_DESCRIPCION"))
                        If IsDBNull(dr("T_DSC_MOTIVO_NO")) Then
                            Me.DescripcionNoCuemple = Nothing
                        Else
                            Me.DescripcionNoCuemple = CStr(dr("T_DSC_MOTIVO_NO"))
                        End If
                        'Me.DescripcionNoCuemple = CStr(dr("T_DSC_MOTIVO_NO"))
                        Me.NumPCInterno = CStr(dr("T_NUM_INTERNO"))
                        Me.FechaVencimiento = CDate(dr("F_FECH_VENC"))
                        If IsDBNull(dr("I_ID_RESOLUCION")) Then
                            Me.IdResolucion = 0
                        Else
                            Me.IdResolucion = CInt(dr("I_ID_RESOLUCION"))
                        End If
                        'Public Property [N_ID_SUB_PERFIL]
                        'Public Property [I_ID_PC_CUMPLE]
                        'Public Property [I_ID_RESOLUCION]

                        Me.DescripcionResolucion = IIf(IsDBNull(dr("T_DSC_RESOLUCION")), "", CStr(dr("T_DSC_RESOLUCION").ToString()))
                        Me.ComentariosAdicionales = IIf(IsDBNull(dr("T_DSC_COMENTARIOS")), "", CStr(dr("T_DSC_COMENTARIOS").ToString()))
                        Me.FolioSisan = IIf(IsDBNull(dr("T_ID_FOLIO_SISAN")), "", CStr(dr("T_ID_FOLIO_SISAN").ToString()))

                    End If

                    If Not dr.IsClosed Then dr.Close()
                    dr = conexion.ConsultarRegistrosDR("BDS_C_PC_PASOS", New List(Of String)(New String() {"N_ID_PASO"}), New List(Of Object)(New Object() {Me.IdPaso}))
                    If dr.Read() Then
                        Me.DescripcionPaso = CStr(dr("T_DSC_PASO"))
                    End If

                    Supervisores = ObtenerSupervisores()
                    Inspectores = ObtenerInspectores()

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

    Public Function Consecutivo(IdArea As Integer, Programa As String)

        Dim conex As Conexion.SQLServer
        Dim datos As DataSet
        Dim Consec As String = ""


        conex = New Conexion.SQLServer()

        Dim dr As SqlClient.SqlDataReader = Nothing

        Try
            Dim conexion As New Conexion.SQLServer()
            Dim data As DataTable


            data = conexion.ConsultarDT("Exec [dbo].[ObtenerConsecutivoXArea] " & IdArea.ToString() & ", " & Programa)

            conexion.CerrarConexion()

            If data.Rows.Count > 0 Then
                Consec = data.Rows(0).ItemArray(0).ToString()
            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If
            If dr IsNot Nothing Then
                If Not dr.IsClosed Then
                    dr.Close() : dr = Nothing
                End If
            End If

        End Try
        Return Consec
    End Function

    Private Function ObtenerSupervisores() As List(Of KeyValuePair(Of String, String))

        Dim conexion As New Conexion.SQLServer()
        Dim dsSupervisor As DataSet
        Dim list As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))

        dsSupervisor = conexion.ConsultarDS("SELECT	R.T_ID_USUARIO,	ISNULL(U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX,'')	NOMBRE FROM	[dbo].[BDS_R_PC_SUPERVISOR_PC] R LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON R.T_ID_USUARIO = U.T_ID_USUARIO WHERE R.N_ID_FOLIO=" + Me.Identificador.ToString())

        For Each row As Data.DataRow In dsSupervisor.Tables(0).Rows
            list.Add(New KeyValuePair(Of String, String)(row("T_ID_USUARIO").ToString(), row("NOMBRE").ToString()))
        Next

        conexion.CerrarConexion()


        Return list
    End Function


    Private Function ObtenerInspectores() As List(Of KeyValuePair(Of String, String))

        Dim conexion As New Conexion.SQLServer()
        Dim dsInspector As DataSet
        Dim list As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))

        dsInspector = conexion.ConsultarDS("SELECT U.T_ID_USUARIO, ISNULL(I.T_DSC_NOMBRE + ' ' + I.T_DSC_APELLIDO + ' ' + I.T_DSC_APELLIDO_AUX, U.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_R_PC_INSPECTOR_PC] U LEFT JOIN [dbo].[BDS_C_GR_USUARIO] I  ON I.T_ID_USUARIO = U.T_ID_USUARIO WHERE U.N_ID_FOLIO =" & Me.Identificador.ToString() & " ORDER by NOMBRE")



        For Each row As Data.DataRow In dsInspector.Tables(0).Rows
            list.Add(New KeyValuePair(Of String, String)(row("T_ID_USUARIO").ToString(), row("NOMBRE").ToString()))
        Next

        conexion.CerrarConexion()


        Return list


    End Function


End Class
