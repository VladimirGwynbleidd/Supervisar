Imports System.Data.SqlClient

Public Class FileSepris

    Private _I_ID_VISITA As Integer
    Private _I_ID_PASO As Integer
    Private _N_ID_DOCUMENTO As Integer
    Private _T_NOM_DOCUMENTO As String
    Private _T_NOM_DOCUMENTO_ORI As String
    Private _N_ID_VERSION As Integer
    Private _T_DSC_COMENTARIO As String
    Private _N_ID_TIPO_DOCUMENTO As Integer
    Private _T_ID_USUARIO As String
    Private _I_ID_ESTATUS As Integer
    Private _N_ID_DOCUMENTO_PASO As Integer
    Private _NUM_VERSIONES As Integer
#Region "Propiedades"
    Public Property I_ID_VISITA As Integer
        Get
            Return IIf(IsNothing(_I_ID_VISITA), 0, _I_ID_VISITA)
        End Get
        Set(value As Integer)
            _I_ID_VISITA = value
        End Set
    End Property

    Public Property I_ID_PASO As Integer
        Get
            Return IIf(IsNothing(_I_ID_PASO), 0, _I_ID_PASO)
        End Get
        Set(value As Integer)
            _I_ID_PASO = value
        End Set
    End Property

    Public Property N_ID_DOCUMENTO As Integer
        Get
            Return IIf(IsNothing(_N_ID_DOCUMENTO), 0, _N_ID_DOCUMENTO)
        End Get
        Set(value As Integer)
            _N_ID_DOCUMENTO = value
        End Set
    End Property

    Public Property T_NOM_DOCUMENTO As String
        Get
            Return IIf(IsNothing(_T_NOM_DOCUMENTO), "", _T_NOM_DOCUMENTO)
        End Get
        Set(value As String)
            _T_NOM_DOCUMENTO = value
        End Set
    End Property

    Public Property T_NOM_DOCUMENTO_ORI As String
        Get
            Return IIf(IsNothing(_T_NOM_DOCUMENTO_ORI), "", _T_NOM_DOCUMENTO_ORI)
        End Get
        Set(value As String)
            _T_NOM_DOCUMENTO_ORI = value
        End Set
    End Property

    Public Property N_ID_VERSION As Integer
        Get
            Return IIf(IsNothing(_N_ID_VERSION), 0, _N_ID_VERSION)
        End Get
        Set(value As Integer)
            _N_ID_VERSION = value
        End Set
    End Property

    Public Property T_DSC_COMENTARIO As String
        Get
            Return IIf(IsNothing(_T_DSC_COMENTARIO), "", _T_DSC_COMENTARIO)
        End Get
        Set(value As String)
            _T_DSC_COMENTARIO = value
        End Set
    End Property

    Public Property N_ID_TIPO_DOCUMENTO As Integer
        Get
            Return IIf(IsNothing(_N_ID_TIPO_DOCUMENTO), 0, _N_ID_TIPO_DOCUMENTO)
        End Get
        Set(value As Integer)
            _N_ID_TIPO_DOCUMENTO = value
        End Set
    End Property

    Public Property T_ID_USUARIO As String
        Get
            Return IIf(IsNothing(_T_ID_USUARIO), "", _T_ID_USUARIO)
        End Get
        Set(value As String)
            _T_ID_USUARIO = value
        End Set
    End Property

    Public Property I_ID_ESTATUS As Integer
        Get
            Return IIf(IsNothing(_I_ID_ESTATUS), 0, _I_ID_ESTATUS)
        End Get
        Set(value As Integer)
            _I_ID_ESTATUS = value
        End Set
    End Property

    Public Property N_ID_DOCUMENTO_PASO As Integer
        Get
            Return IIf(IsNothing(_N_ID_DOCUMENTO_PASO), 0, _N_ID_DOCUMENTO_PASO)
        End Get
        Set(value As Integer)
            _N_ID_DOCUMENTO_PASO = value
        End Set
    End Property

    Public Property NUM_VERSIONES As Integer
        Get
            Return IIf(IsNothing(_NUM_VERSIONES), 0, _NUM_VERSIONES)
        End Get
        Set(value As Integer)
            _NUM_VERSIONES = value
        End Set
    End Property
    Public Property T_FOLIO_SICOD As String
#End Region

    ''' <summary>
    ''' Da de alta y actualiza un documento en sepris
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AltaDocumento() As Boolean
        Dim con As Conexion.SQLServer = Nothing
        Dim lbRes As Boolean = False

        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REG_UPD_DOCUMENTO")


            Dim sqlParameter(11) As SqlParameter
            sqlParameter(0) = New SqlParameter("@I_ID_VISITA", Me.I_ID_VISITA)
            sqlParameter(1) = New SqlParameter("@I_ID_PASO", Me.I_ID_PASO)
            sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", Me.N_ID_DOCUMENTO)
            sqlParameter(3) = New SqlParameter("@T_NOM_DOCUMENTO", Me.T_NOM_DOCUMENTO)
            sqlParameter(4) = New SqlParameter("@T_NOM_DOCUMENTO_ORI", Me.T_NOM_DOCUMENTO_ORI)
            sqlParameter(5) = New SqlParameter("@N_ID_VERSION", Me.N_ID_VERSION)
            sqlParameter(6) = New SqlParameter("@T_DSC_COMENTARIO", Me.T_DSC_COMENTARIO)
            sqlParameter(7) = New SqlParameter("@N_ID_TIPO_DOCUMENTO", Me.N_ID_TIPO_DOCUMENTO)
            sqlParameter(8) = New SqlParameter("@T_ID_USUARIO", Me.T_ID_USUARIO)
            sqlParameter(9) = New SqlParameter("@T_ID_ESTATUS", Me.I_ID_ESTATUS)
            sqlParameter(10) = New SqlParameter("@N_ID_DOCUMENTO_PASO", Me.N_ID_DOCUMENTO_PASO)
            sqlParameter(11) = New SqlParameter("@NUM_VERSIONES", Me.NUM_VERSIONES)

            lbRes = con.EjecutarSP(sp, sqlParameter)
        Catch ex As Exception
            lbRes = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "FileSepris.vb, AltaDocumento", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return lbRes
    End Function

    ''' <summary>
    ''' Da de alta un documento de un usuario cuando no hay visita
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AltaDocumentoUsuario() As Boolean
        Dim con As Conexion.SQLServer = Nothing
        Dim lbRes As Boolean = False

        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REG_UPD_DOCUMENTO_USUARIO")


            Dim sqlParameter(7) As SqlParameter
            sqlParameter(0) = New SqlParameter("@I_ID_PASO", Me.I_ID_PASO)
            sqlParameter(1) = New SqlParameter("@N_ID_DOCUMENTO", Me.N_ID_DOCUMENTO)
            sqlParameter(2) = New SqlParameter("@T_NOM_DOCUMENTO", Me.T_NOM_DOCUMENTO)
            sqlParameter(3) = New SqlParameter("@T_NOM_DOCUMENTO_ORI", Me.T_NOM_DOCUMENTO_ORI)
            sqlParameter(4) = New SqlParameter("@N_ID_VERSION", Me.N_ID_VERSION)
            sqlParameter(5) = New SqlParameter("@T_DSC_COMENTARIO", Me.T_DSC_COMENTARIO)
            sqlParameter(6) = New SqlParameter("@N_ID_TIPO_DOCUMENTO", Me.N_ID_TIPO_DOCUMENTO)
            sqlParameter(7) = New SqlParameter("@T_ID_USUARIO", Me.T_ID_USUARIO)

            lbRes = con.EjecutarSP(sp, sqlParameter)
        Catch ex As Exception
            lbRes = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "FileSepris.vb, AltaDocumento", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return lbRes
    End Function
End Class
