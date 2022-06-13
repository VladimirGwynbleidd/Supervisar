Imports System.Data.SqlClient

''' <summary>
''' Almacena todo el expediente de una visita en particular
''' </summary>
''' <remarks>AGC</remarks>
Public Class Expediente
    Public Property lstDocumentos As List(Of Documento)
    Public Property NumMaxVersiones As Integer
    Public Property HayDocumentos As Boolean
    Public Property IdVisita As Integer
    Public Property IdUsuario As String
    Public Property IdPaso As Integer
    Public Property IdDocumento As Integer = Constantes.Todos
    Public Property IdVigencia As Integer = Constantes.Todos
    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks>AGC</remarks>
    Public Sub New(Optional piIdVigencia As Integer = Constantes.Vigencia.NoConsideraVigencia,
                    Optional piIdPaso As Integer = Constantes.Todos,
                    Optional piIdVisita As Integer = Constantes.Todos,
                    Optional piIdUsuario As String = "",
                    Optional piIdDocumento As Integer = Constantes.Todos)
        Me.IdVisita = piIdVisita
        Me.IdUsuario = piIdUsuario
        Me.IdPaso = piIdPaso
        Me.IdDocumento = piIdDocumento
        Me.IdVigencia = piIdVigencia
        
        Me.RefreszarDocumentosExpediente()
    End Sub

    ''' <summary>
    ''' Llena o refresca la lista de documentos cargados
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreszarDocumentosExpediente()
        Me.lstDocumentos = llenarExpedienteDocumentos(Me.IdVigencia, Me.IdPaso, Me.IdVisita, Me.IdUsuario, Me.IdDocumento)
        HayDocumentos = IIf(Me.lstDocumentos.Count > 0, True, False)
    End Sub

    Public Sub RefreszarDocumentosExpedienteSubVisita(ByVal idSubV As Integer)
        Me.lstDocumentos = llenarExpedienteDocumentos(Me.IdVigencia, Me.IdPaso, idSubV, Me.IdUsuario, Me.IdDocumento)
        HayDocumentos = IIf(Me.lstDocumentos.Count > 0, True, False)
    End Sub

    ''' <summary>
    ''' Llena los documentos desde la BD
    ''' </summary>
    ''' <param name="piIdVigencia"></param>
    ''' <param name="piIdPaso"></param>
    ''' <param name="piIdVisita"></param>
    ''' <param name="piIdUsuario"></param>
    ''' <param name="piIdDocumento"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function llenarExpedienteDocumentos(Optional piIdVigencia As Integer = Constantes.Vigencia.NoConsideraVigencia,
                                                Optional piIdPaso As Integer = Constantes.Todos,
                                                Optional piIdVisita As Integer = Constantes.Todos,
                                                Optional piIdUsuario As String = "",
                                                Optional piIdDocumento As Integer = Constantes.Todos) As List(Of Documento)
        Dim dr As SqlDataReader = Nothing
        Dim lstDocs As New List(Of Documento)
        Dim con As Conexion.SQLServer = Nothing

        Dim visita As New Visita()
        visita = AccesoBD.getDetalleVisita(piIdVisita, -1)
        Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp As String = Nothing

            If Not IsNothing(visita) Then
                If ((Not piIdVisita.Equals(-1)) And Convert.ToDateTime(visita.FechaRegistro).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                    sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2")
                Else
                    sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2_V17")
                End If
            End If

            Dim sqlParameter(4) As SqlParameter
            sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
            sqlParameter(1) = New SqlParameter("@I_ID_USUARIO", piIdUsuario)
            sqlParameter(2) = New SqlParameter("@I_ID_PASO", piIdPaso)
            sqlParameter(3) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)
            sqlParameter(4) = New SqlParameter("@ID_VIGENCIA", piIdVigencia)

            dr = con.EjecutarSPConsultaDR(sp, sqlParameter)
            lstDocs = CType(dr.ToList(Of Documento)(), Global.System.Collections.Generic.List(Of Documento))
        Catch ex As Exception
            dr = Nothing
            lstDocs = New List(Of Documento)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarDocumentos", "")
        Finally
            If dr IsNot Nothing Then
                If Not dr.IsClosed Then
                    dr.Close() : dr = Nothing
                End If
            End If
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return lstDocs
    End Function


    Public Function ConsultaDocumentos(ByVal idVisitaMC As Integer,
                                       Optional piIdVigencia As Integer = Constantes.Vigencia.NoConsideraVigencia,
                                                Optional piIdPaso As Integer = Constantes.Todos,
                                                Optional piIdVisita As Integer = Constantes.Todos,
                                                Optional piIdUsuario As String = "",
                                                Optional piIdDocumento As Integer = Constantes.Todos) As DataTable
        Dim dr As DataTable = Nothing
        Dim lstDocs As New List(Of Documento)
        Dim con As Conexion.SQLServer = Nothing

        Dim visita As New Visita()
        visita = AccesoBD.getDetalleVisita(idVisitaMC, -1)
        Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp As String = Nothing

            If Not IsNothing(visita) Then
                If (Convert.ToDateTime(visita.FechaRegistro).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                    sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2")
                Else
                    sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2_V17")
                End If
            End If


            Dim sqlParameter(4) As SqlParameter
            sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
            sqlParameter(1) = New SqlParameter("@I_ID_USUARIO", piIdUsuario)
            sqlParameter(2) = New SqlParameter("@I_ID_PASO", piIdPaso)
            sqlParameter(3) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)
            sqlParameter(4) = New SqlParameter("@ID_VIGENCIA", piIdVigencia)

            dr = con.EjecutarSPConsultaDT(sp, sqlParameter)

        Catch ex As Exception
            dr = Nothing
            lstDocs = New List(Of Documento)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarDocumentos", "")
        Finally
            If dr IsNot Nothing Then
                dr = Nothing
            End If
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return dr
    End Function

    Public Function ConsultaNombreDocumentos(Optional piIdPaso As Integer = Constantes.Todos,
                                             Optional piIdVisita As Integer = Constantes.Todos,
                                             Optional piIdDocumento As Integer = Constantes.Todos,
                                             Optional piVersionDocto As Integer = 0) As String
        Dim dr As DataTable = Nothing
        Dim lstDocs As New List(Of Documento)
        Dim con As Conexion.SQLServer = Nothing
        Dim nombreDocto As String = ""

        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_NOMBRE_DOCUMENTOS")


            Dim sqlParameter(3) As SqlParameter
            sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
            sqlParameter(1) = New SqlParameter("@I_ID_PASO", piIdPaso)
            sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)
            sqlParameter(3) = New SqlParameter("@ID_VERSION", piVersionDocto)

            dr = con.EjecutarSPConsultaDT(sp, sqlParameter)

            If dr.Rows.Count > 0 Then
                nombreDocto = dr.Rows(0)("T_NOM_DOCUMENTO_CAT").ToString
            End If

        Catch ex As Exception
            dr = Nothing
            lstDocs = New List(Of Documento)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ConsultaNombreDocumentos", "")
        Finally
            If dr IsNot Nothing Then
                dr = Nothing
            End If
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return nombreDocto
    End Function
End Class
