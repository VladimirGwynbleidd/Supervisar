Public Class ObjetoVisita
    Inherits System.Web.UI.UserControl
    Public Property txtOtro
        Get
            Return txtObjetoVisita.Text
        End Get
        Set(value)
            txtObjetoVisita.Text = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    RefreshList()
        'End If 
    End Sub
    Public Sub ClearList()
        lstObjetosDisponibles.Items.Clear()
        lstObjetosSeleccionados.Items.Clear()
    End Sub
    Public Sub cargarObjetoVisita(objUsuario As Entities.Usuario)
        Dim dt As DataTable = Nothing
        Dim objAccesoBD As New AccesoBD

        dt = objAccesoBD.ObtenObjetoVisita(objUsuario.IdArea)

        If dt.Rows.Count > 0 Then
            lstObjetosDisponibles.DataValueField = "ID"
            lstObjetosDisponibles.DataTextField = "DSC"
            lstObjetosDisponibles.DataSource = dt
            lstObjetosDisponibles.DataBind()

        End If

    End Sub
    Public Function GetObjetosSeleccionados() As List(Of ListItem)

        Dim objetos As New List(Of ListItem)()

        For Each item As ListItem In lstObjetosSeleccionados.Items
            objetos.Add(item)
        Next

        Return objetos

    End Function

    Protected Sub imgAsignar_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignar.Click

        Dim _selecteditems = lstObjetosDisponibles.GetSelectedIndices

        If _selecteditems.Count = 0 Then Exit Sub

        If Not lstObjetosSeleccionados.Items.Contains(lstObjetosDisponibles.SelectedItem) Then
            Dim _indx As Integer
            _indx = lstObjetosDisponibles.SelectedIndex
            lstObjetosSeleccionados.Items.Add(New ListItem(text:=lstObjetosDisponibles.SelectedItem.Text, value:=lstObjetosDisponibles.SelectedItem.Value))
            lstObjetosDisponibles.SelectedItem.Selected = False
            lstObjetosDisponibles.Items.RemoveAt(_indx)
        End If
        VerificaOtroSeleccionado()

    End Sub

    Protected Sub imgDesasignar_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignar.Click
        Dim listaSeleccionados As Integer

        listaSeleccionados = lstObjetosSeleccionados.GetSelectedIndices.Count

        If listaSeleccionados = 0 Then Exit Sub

        If Not lstObjetosDisponibles.Items.Contains(lstObjetosSeleccionados.SelectedItem) Then
            Dim _indx As Integer
            _indx = lstObjetosSeleccionados.SelectedIndex
            lstObjetosDisponibles.Items.Add(New ListItem(text:=lstObjetosSeleccionados.SelectedItem.Text, value:=lstObjetosSeleccionados.SelectedItem.Value))
            lstObjetosSeleccionados.SelectedItem.Selected = False
            lstObjetosSeleccionados.Items.RemoveAt(_indx)
        End If
        VerificaOtroSeleccionado()
    End Sub
    Protected Sub VerificaOtroSeleccionado()
        Dim isOtro As Boolean = False
        Dim x = lstObjetosSeleccionados.Items

        For Each item As ListItem In lstObjetosSeleccionados.Items
            If item.Text = "Otro" Then
                isOtro = True
            End If
        Next
        If isOtro Then
            OtroObjVisita.Visible = True
        Else
            OtroObjVisita.Visible = False
            txtObjetoVisita.Text = ""
        End If
    End Sub
End Class