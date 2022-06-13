Public Class SupervisorOPI
    Inherits System.Web.UI.UserControl
    Private _proceso As Integer
    Public Property Id_Proceso() As Integer
        Get
            Return _proceso
        End Get
        Set(ByVal value As Integer)
            _proceso = value
        End Set
    End Property

    Private _id_subproceso As Integer
    Public Property I_ID_SUBPROCESO() As Integer
        Get
            Return _id_subproceso
        End Get
        Set(ByVal value As Integer)
            _id_subproceso = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    RefreshList()
        'End If
    End Sub
    Public Sub ClearList()
        lstSupervisorDisponible.Items.Clear()
        lstSupervisorSeleccionado.Items.Clear()
    End Sub

    Public Sub RefreshList()
        Dim SupervisoresListAsignado As List(Of ListItem) = ObtenerSupervisores()
        lstSupervisorDisponible.DataSource = SupervisoresListAsignado
        lstSupervisorDisponible.DataTextField = "Text"
        lstSupervisorDisponible.DataValueField = "Value"
        lstSupervisorDisponible.DataBind()
    End Sub

    Protected Function ObtenerSupervisores() As List(Of ListItem)

        Dim dsSupervisores As DataSet = cSupervisorOPI.ObtenerVigentesPorSubproceso(Id_Proceso, I_ID_SUBPROCESO)
        Dim supervisores As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSupervisores.Tables(0).Rows
            supervisores.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next

        Return supervisores

    End Function


    Public Function GetSupervisoresSeleccionados() As List(Of ListItem)

        Dim supervisores As New List(Of ListItem)()

        For Each item As ListItem In lstSupervisorSeleccionado.Items
            supervisores.Add(item)
        Next

        Return supervisores

    End Function

    Protected Sub imgAsignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarSupervisor.Click

        Dim _selecteditems = lstSupervisorDisponible.GetSelectedIndices

        If _selecteditems.Count = 0 Then Exit Sub

        If Not lstSupervisorSeleccionado.Items.Contains(lstSupervisorDisponible.SelectedItem) Then
            Dim _indx As Integer
            _indx = lstSupervisorDisponible.SelectedIndex
            lstSupervisorSeleccionado.Items.Add(New ListItem(text:=lstSupervisorDisponible.SelectedItem.Text, value:=lstSupervisorDisponible.SelectedItem.Value))
            lstSupervisorDisponible.SelectedItem.Selected = False
            lstSupervisorDisponible.Items.RemoveAt(_indx)
        End If

        If lstSupervisorSeleccionado.Items.Count <= 0 Then
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoShow")
        Else
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoHide")
        End If

    End Sub

    Protected Sub imgDesasignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarSupervisor.Click
        Dim listaSeleccionados As Integer

        listaSeleccionados = lstSupervisorSeleccionado.GetSelectedIndices.Count

        If listaSeleccionados = 0 Then Exit Sub

        If Not lstSupervisorDisponible.Items.Contains(lstSupervisorSeleccionado.SelectedItem) Then
            Dim _indx As Integer
            _indx = lstSupervisorSeleccionado.SelectedIndex
            lstSupervisorDisponible.Items.Add(New ListItem(text:=lstSupervisorSeleccionado.SelectedItem.Text, value:=lstSupervisorSeleccionado.SelectedItem.Value))
            lstSupervisorSeleccionado.SelectedItem.Selected = False
            lstSupervisorSeleccionado.Items.RemoveAt(_indx)
        End If

        If lstSupervisorSeleccionado.Items.Count <= 0 Then
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoShow")
        Else
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoHide")
        End If
    End Sub
    Public Sub agregaItem(item As ListItem)
        lstSupervisorSeleccionado.Items.Add(item)
    End Sub
End Class