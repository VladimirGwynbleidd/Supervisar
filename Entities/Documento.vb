'- Fecha de modificación:  27/03/2015
'- Nombre del Responsable: Alfonso Flores Mendoza
'- Empresa: Indra Sistemas
'- Se agrego la propiedad Fecha Inicio

Imports System
Imports System.Web
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports Utilerias
Public Class Documento
    Private BDS_C_GR_DOCUMENTO_SEDI As String = "BDS_C_GR_DOCUMENTO_SEDI"
#Region "Propiedades"
    'Tabla DOCUMENTOS
    Public Property IdDocumento As Integer
    Public Property NombreDocumento As String
    Public Property FechaFirmaDocumento As Date
    Public Property SelloDigitalDocumento As String
    Public Property NombreDocumentoSharepoint As String
    Public Property NumeroPaginas As Integer
    Public Property IdUsuarioDocumento As String
    Public Property NombreUsuarioDocumento As String
    Public Property Existe As Boolean
#End Region
#Region "Constructores"
    Public Sub New()

    End Sub
    Public Sub New(ByVal idDoc As Integer)
        Me.IdDocumento = idDoc
    End Sub
#End Region
    Function ObtenerDocumentosBD() As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing
            listCampos.Add("N_ID_DOCUMENTO") : listValores.Add(Me.IdDocumento)
            Try
                Existe = conexion.BuscarUnRegistro(BDS_C_GR_DOCUMENTO_SEDI, listCampos, listValores)
                If Existe Then
                    dr = conexion.ConsultarRegistrosDR(BDS_C_GR_DOCUMENTO_SEDI, listCampos, listValores)
                    If dr.Read() Then
                        'Me.IdDocumento = CInt(dr("N_ID_DOCUMENTO"))
                        Me.NombreDocumento = CStr(dr("S_NOMBRE_DOCUMENTO"))
                        If Not IsDBNull(dr("D_FECHA_FIRMA")) Then
                            Me.FechaFirmaDocumento = CDate(dr("D_FECHA_FIRMA"))
                        End If
                        Me.SelloDigitalDocumento = CStr(dr("S_SELLO_DIGITAL"))
                        Me.NombreDocumentoSharepoint = CStr(dr("S_NOMBRE_DOC_SHAREPOINT"))
                        Me.IdUsuarioDocumento = CStr(dr("T_ID_USUARIO"))
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
    End Function
    Public Function ObtenerDocumentoSelloDigital(ByVal Sello As String) As Documento
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            dv = conexion.EjecutarSPConsultaDT_Sello("SP_CONSULTA_DOCUMENTO_SEDI", Sello)
            If dv.Rows.Count > 0 Then
                For Each dr As DataRow In dv.Rows
                    Return New Documento(CInt(dr("N_ID_DOCUMENTO")))
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return Nothing
    End Function
    Public Function ObtenerDocumentoId() As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim dv As New DataTable
        Try
            dv = conexion.EjecutarSPConsultaDT_Id("SP_CONSULTA_DOCUMENTO_SEDI_POR_ID", Me.IdDocumento)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return dv
    End Function
    Public Function GuardarDocumento() As Boolean
        Dim conexion As New Conexion.SQLServer
        Dim resultado As Boolean = False

        Dim parametro1 As New SqlParameter
        parametro1.ParameterName = ""
        parametro1.Value = ""
        parametro1.DbType = DbType.String
        Dim parametros As New SqlParameter()
        resultado = conexion.EjecutarSP("SP_REGISTRAR_DOCUMENTO_SEDI", NombreDocumento, FechaFirmaDocumento, SelloDigitalDocumento, NombreDocumentoSharepoint, IdUsuarioDocumento)
        Return resultado
    End Function
    Public Sub GenerarSelloDigital()
        SelloDigitalDocumento = Cifrado.CifrarAES("Nombre Documento:" & NombreDocumento & "||" _
                                                  & "VPO||" _
                                                  & "Fecha Firma:" & FechaFirmaDocumento & "||" _
                                                  & "Numero Paginas:" & NumeroPaginas)
    End Sub
    Public Function ObtenerUsuario() As Boolean
        Dim resultado As Boolean = False
        Try
            Dim conexion As New Conexion.SQLServer
            Dim dv As New DataTable
            Try
                dv = conexion.EjecutarSPConsultaUsuarioDT_Id("SP_CONSULTA_USUARIO_SEDI_POR_ID", Me.IdUsuarioDocumento)
            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(conexion) Then
                    conexion.CerrarConexion()
                End If
            End Try
            NombreUsuarioDocumento = dv.Rows(0).Item(0).ToString() & " " & dv.Rows(0).Item(1).ToString() & " " & dv.Rows(0).Item(2).ToString()
            resultado = True
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        End Try
        Return resultado
    End Function
End Class
