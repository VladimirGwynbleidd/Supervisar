Public Class chkButtomUserControl2
    Inherits System.Web.UI.UserControl

    Public Delegate Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event btnPostClk As btnDelete_Click
    Private _SessionID As String

    Private _DataValueType As ucFiltro.DataValueType
    Public Property DataValueType() As ucFiltro.DataValueType
        Get
            Return _DataValueType
        End Get
        Set(ByVal value As ucFiltro.DataValueType)
            _DataValueType = value
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

    Private _source As Object
    Public Property source() As Object
        Get
            Return _source
        End Get
        Set(ByVal value As Object)
            _source = value
        End Set
    End Property

    Private _selectedItem As String
    Public Property selectedItem() As String
        Get
            Return _selectedItem
        End Get
        Set(ByVal value As String)
            _selectedItem = value
        End Set
    End Property

    Private _selectedValue As Boolean
    Public Property selectedValue() As Boolean
        Get
            Return ucCheckBox.Checked
        End Get
        Set(ByVal value As Boolean)
            _selectedValue = value
            ucCheckBox.Checked = _selectedValue
        End Set
    End Property

    Public Property SessionID() As String
        Get
            Return _SessionID
        End Get
        Set(ByVal value As String)
            _SessionID = value
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

    Private _dataTextField As String
    Public Property DataTextField() As String
        Get
            Return _dataTextField
        End Get
        Set(ByVal value As String)
            _dataTextField = value
        End Set
    End Property

    Private _isFixed As Boolean
    Public Property isFixed() As Boolean
        Get
            Return _isFixed
        End Get
        Set(ByVal value As Boolean)
            _isFixed = value
        End Set
    End Property

    Public Sub bind()
        If _source IsNot Nothing Then
        End If
    End Sub

    Public Sub FetchValues()
        _selectedValue = ucCheckBox.Checked
    End Sub


    'Private Sub ucCheckBoxList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ucCheckBoxList.SelectedIndexChanged
    '    FetchValues()
    'End Sub

    Public Sub initSelectedItem()
        ucCheckBox.Checked = False
        FetchValues()

    End Sub

End Class