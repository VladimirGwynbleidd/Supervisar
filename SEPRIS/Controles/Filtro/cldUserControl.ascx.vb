Imports AjaxControlToolkit

Public Class cldUserControl
    Inherits System.Web.UI.UserControl

    Public Delegate Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event btnPostClk As btnDelete_Click

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent btnPostClk(sender, e)
    End Sub

    Private _isCalendarSingle As Boolean
    Public Property isCalendarSingle() As Boolean
        Get
            Return _isCalendarSingle
        End Get
        Set(ByVal value As Boolean)
            _isCalendarSingle = value
            TxtFecSolFin.Visible = Not (_isCalendarSingle)
            imgFec2.Visible = Not (_isCalendarSingle)
        End Set
    End Property


    Private _SessionID As String
    Public Property SessionID() As String
        Get
            Return _SessionID
        End Get
        Set(ByVal value As String)
            _SessionID = value
        End Set
    End Property

    Public Property labelText() As String
        Get
            Return Label1.Text
        End Get
        Set(ByVal value As String)
            Label1.Text = value
        End Set
    End Property

    Private _typeOfControl As String
    Public Property TypeOfControl() As String
        Get
            Return _typeOfControl
        End Get
        Set(ByVal value As String)
            _typeOfControl = value
        End Set
    End Property

    Private _fechaInicio As String
    Public Property FechaInicio() As String
        Get
            Return _fechaInicio
        End Get
        Set(ByVal value As String)
            _fechaInicio = value
            TxtFecSolIni.Text = _fechaInicio
        End Set
    End Property

    Private _fechaFin As String
    Public Property fechaFin() As String
        Get
            Return _fechaFin
        End Get
        Set(ByVal value As String)
            _fechaFin = value
            TxtFecSolFin.Text = _fechaFin
        End Set
    End Property

    Private _dataValueField As String
    Public Property DataValueField() As String
        Get
            Return _dataValueField
        End Get
        Set(ByVal value As String)
            _dataValueField = value
        End Set
    End Property

    Private _isFixed As Boolean
    Public Property isFixed() As Boolean
        Get
            Return _isFixed
        End Get
        Set(ByVal value As Boolean)
            _isFixed = value
            btnDelete.Visible = Not _isFixed
        End Set
    End Property

    Public Sub FetchValues()
        _fechaInicio = TxtFecSolIni.Text
        _fechaFin = TxtFecSolFin.Text
    End Sub

    Protected Sub TxtFecSolIni_TextChanged(sender As Object, e As EventArgs) _
        Handles TxtFecSolIni.TextChanged, TxtFecSolFin.TextChanged
        FetchValues()
    End Sub

End Class