Public Class InspectoresOPI
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

    End Sub

    Public Sub ClearList()
        lstInspectorDisponible.Items.Clear()
        lstInspectorSeleccionado.Items.Clear()
    End Sub

    Public Sub RefreshList()
        Dim InspectorisListAsignado As List(Of ListItem) = ObtenerInspector_Proceso()
        lstInspectorDisponible.DataSource = InspectorisListAsignado
        lstInspectorDisponible.DataTextField = "Text"
        lstInspectorDisponible.DataValueField = "Value"
        lstInspectorDisponible.DataBind()
    End Sub


    Protected Function ObtenerInspector_Proceso() As List(Of ListItem)
        Dim ListInspectores As New List(Of ListItem)()
        Dim dsProcesos As DataSet = cInspectorOPI.ObtenerVigentesPorProceso(Id_Proceso, I_ID_SUBPROCESO)
        For Each row As Data.DataRow In dsProcesos.Tables(0).Rows
            ListInspectores.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next


        Return ListInspectores
    End Function

    Public Function GetInspectoresSeleccionados() As List(Of ListItem)

        Dim _inspectores As New List(Of ListItem)()

        For Each item As ListItem In lstInspectorSeleccionado.Items
            _inspectores.Add(item)
        Next

        Return _inspectores

    End Function

    Protected Sub imgAsignarInspector_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarInspector.Click

        Dim _selecteditems = lstInspectorDisponible.GetSelectedIndices

        If _selecteditems.Count = 0 Then Exit Sub

        If Not lstInspectorSeleccionado.Items.Contains(lstInspectorDisponible.SelectedItem) Then
            Dim _indx As Integer
            _indx = lstInspectorDisponible.SelectedIndex
            lstInspectorSeleccionado.Items.Add(New ListItem(text:=lstInspectorDisponible.SelectedItem.Text, value:=lstInspectorDisponible.SelectedItem.Value))
            lstInspectorDisponible.SelectedItem.Selected = False
            lstInspectorDisponible.Items.RemoveAt(_indx)
        End If

        If lstInspectorSeleccionado.Items.Count <= 0 Then
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoShow")
        Else
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoHide")
        End If

    End Sub

    Protected Sub imgDesasignarInspector_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarInspector.Click

        Dim listaSeleccionados As Integer

        listaSeleccionados = lstInspectorSeleccionado.GetSelectedIndices.Count

        If listaSeleccionados = 0 Then Exit Sub

        If Not lstInspectorDisponible.Items.Contains(lstInspectorSeleccionado.SelectedItem) Then
            Dim _indx As Integer
            _indx = lstInspectorSeleccionado.SelectedIndex
            lstInspectorDisponible.Items.Add(New ListItem(text:=lstInspectorSeleccionado.SelectedItem.Text, value:=lstInspectorSeleccionado.SelectedItem.Value))
            lstInspectorSeleccionado.SelectedItem.Selected = False
            lstInspectorSeleccionado.Items.RemoveAt(_indx)
        End If

        If lstInspectorSeleccionado.Items.Count <= 0 Then
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoShow")
        Else
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoHide")
        End If
    End Sub
    Public Sub agregaItem(item As ListItem)
        lstInspectorSeleccionado.Items.Add(item)
    End Sub
End Class