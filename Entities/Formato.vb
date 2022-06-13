Imports System.Data.SqlClient

''' <summary>
''' AGC, Metodos que sirven para dar de alta un formato a un documento en base de datos
''' </summary>
''' <remarks></remarks>
Public Class Formato

#Region "Propiedades"

    Sub New()
        ' TODO: Complete member initialization 
    End Sub

    Private Property _N_ID_FORMATO As Integer
    Private Property _T_NOM_FORMATO As String
    Private Property _N_ID_DOCUMENTO As Integer
    Private Property _T_NOM_MACHOTE_ORI As String
    Private Property _T_NOM_MACHOTE_ACTUAL As String
    Private Property _I_ID_AREA As Integer
    Private Property _T_ID_USUARIO As String
    Private Property _F_FECH_REGISTRO As DateTime
    Private Property _T_ID_USUARIO_MOD As String
    Private Property _F_FECH_MODIFICACION As DateTime
    Private Property _N_FLAG_VIG As Integer
    Public Property ExisteFormato As Boolean

    Public Property N_FLAG_VIG As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_VIG), 0, _N_FLAG_VIG)
        End Get
        Set(value As Integer)
            _N_FLAG_VIG = value
        End Set
    End Property

    Public Property F_FECH_MODIFICACION As DateTime
        Get
            Return IIf(IsNothing(_F_FECH_MODIFICACION), DateTime.MinValue, _F_FECH_MODIFICACION)
        End Get
        Set(value As DateTime)
            _F_FECH_MODIFICACION = value
        End Set
    End Property

    Public ReadOnly Property F_FECH_MODIFICACION_F As String
        Get
            Return IIf(_F_FECH_MODIFICACION = DateTime.MinValue, "", _F_FECH_MODIFICACION.ToString("dd/MM/yyyy"))
        End Get
    End Property

    Public Property T_ID_USUARIO_MOD As String
        Get
            Return IIf(IsNothing(_T_ID_USUARIO_MOD), "", _T_ID_USUARIO_MOD)
        End Get
        Set(value As String)
            _T_ID_USUARIO_MOD = value
        End Set
    End Property


    Public Property F_FECH_REGISTRO As DateTime
        Get
            Return IIf(IsNothing(_F_FECH_REGISTRO), DateTime.MinValue, _F_FECH_REGISTRO)
        End Get
        Set(value As DateTime)
            _F_FECH_REGISTRO = value
        End Set
    End Property

    Public ReadOnly Property F_FECH_REGISTRO_F As String
        Get
            Return IIf(_F_FECH_REGISTRO = DateTime.MinValue, "", _F_FECH_REGISTRO.ToString("dd/MM/yyyy"))
        End Get
    End Property

    Public Property T_ID_USUARIO As String
        Get
            Return IIf(IsNothing(_T_ID_USUARIO), "", _T_ID_USUARIO)
        End Get
        Set(value As String)
            _T_ID_USUARIO = value
        End Set
    End Property

    Public Property I_ID_AREA As Integer
        Get
            Return IIf(IsNothing(_I_ID_AREA), 0, _I_ID_AREA)
        End Get
        Set(value As Integer)
            _I_ID_AREA = value
        End Set
    End Property

    Public Property T_NOM_MACHOTE_ACTUAL As String
        Get
            Return IIf(IsNothing(_T_NOM_MACHOTE_ACTUAL), "", _T_NOM_MACHOTE_ACTUAL)
        End Get
        Set(value As String)
            _T_NOM_MACHOTE_ACTUAL = value
        End Set
    End Property

    Public Property T_NOM_MACHOTE_ORI As String
        Get
            Return IIf(IsNothing(_T_NOM_MACHOTE_ORI), "", _T_NOM_MACHOTE_ORI)
        End Get
        Set(value As String)
            _T_NOM_MACHOTE_ORI = value
        End Set
    End Property

    Public Property T_NOM_FORMATO As String
        Get
            Return IIf(IsNothing(_T_NOM_FORMATO), "", _T_NOM_FORMATO)
        End Get
        Set(value As String)
            _T_NOM_FORMATO = value
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

    Public Property N_ID_FORMATO As Integer
        Get
            Return IIf(IsNothing(_N_ID_FORMATO), 0, _N_ID_FORMATO)
        End Get
        Set(value As Integer)
            _N_ID_FORMATO = value
        End Set
    End Property


#End Region


    Public Sub New(piIdFormato As Integer)
        Me.I_ID_AREA = -1
        Me.T_ID_USUARIO = ""
        Me.N_ID_FORMATO = piIdFormato
        Me.N_ID_DOCUMENTO = -1

        Dim dt As DataTable = getFormatos()

        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                Me.ExisteFormato = True
            End If
            For Each dtRows As DataRow In dt.Rows
                Me.T_NOM_FORMATO = dtRows("T_NOM_FORMATO").ToString()
                Me.T_NOM_MACHOTE_ORI = dtRows("T_NOM_MACHOTE_ORI").ToString()
                Me.T_NOM_MACHOTE_ACTUAL = dtRows("T_NOM_MACHOTE_ACTUAL").ToString()
                Me.N_ID_DOCUMENTO = CInt(dtRows("N_ID_DOCUMENTO").ToString())
                Me.I_ID_AREA = CInt(dtRows("I_ID_AREA").ToString())
                Me.T_ID_USUARIO = dtRows("T_ID_USUARIO").ToString()
                Me.T_ID_USUARIO_MOD = dtRows("T_ID_USUARIO_MOD").ToString()
                Me.N_FLAG_VIG = CInt(dtRows("N_FLAG_VIG").ToString())

            Next
        End If
    End Sub

    Public Function getFormatos() As DataTable
        Dim con As Conexion.SQLServer = Nothing
        Dim dt As DataTable

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_FORMATOS")

            Dim sqlParameter(3) As SqlParameter
            sqlParameter(0) = New SqlParameter("@N_ID_DOCUMENTO", Me.N_ID_DOCUMENTO)
            sqlParameter(1) = New SqlParameter("@I_ID_AREA", Me.I_ID_AREA)
            sqlParameter(2) = New SqlParameter("@T_ID_USUARIO", Me.T_ID_USUARIO)
            sqlParameter(3) = New SqlParameter("@N_ID_FORMATO", Me.N_ID_FORMATO)

            dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getFormatos", "")
            dt = New DataTable
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return dt

    End Function

    ''' <summary>
    ''' Inserta/Actualiza un formato
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function InsertarActualizarFormato() As Integer
        Dim con As Conexion.SQLServer = Nothing
        Dim liRes As Integer

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_MERGE_FORMATO")

            liRes = con.EjecutarSP(sp, Me)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, InsertarActualizarFormato", "")
            liRes = False
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return liRes
    End Function
End Class
