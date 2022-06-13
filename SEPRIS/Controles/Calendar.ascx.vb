Imports Entities

Public Class Calendar
    Inherits System.Web.UI.UserControl

    Private _datasource As DataTable
    Private _Fecha As DateTime
    Private Const reporte As String = ""
    Private FechaDia As String
    Public Mensaje As String

    Public Property Datasource() As DataTable
        Get
            Return Me._datasource
        End Get
        Set(ByVal value As DataTable)
            Me._datasource = value
        End Set
    End Property

    Public Property Fecha() As DateTime
        Get
            Return Me._Fecha
        End Get
        Set(ByVal value As DateTime)
            Me._Fecha = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub DataList1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dtlmes.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If (CType(e.Item.FindControl("lblDia"), LinkButton)).Text <> String.Empty Then

                End If
                If dtlmes.Items.Count > 6 Then
                    CType(e.Item.FindControl("lblDiaDes"), Label).Visible = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub ReloadCalender()
        dtlmes.DataSource = Datasource
        dtlmes.DataBind()
    End Sub

    Protected Sub dtlmes_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dtlmes.ItemCommand
        Dim indice = e.Item.ItemIndex
        hfIdRow.Value = indice
        Dim sender As Object
        Dim e2 As EventArgs
        Select Case (hfLinkId.Value)
            Case "lnkActividad1"
                lnkActividad1_Click(sender, e2)
            Case "lnkActividad2"
                lnkActividad2_Click(sender, e2)
            Case "lnkActividad3"
                lnkActividad3_Click(sender, e2)
            Case "lnkActividad4"
                lnkActividad4_Click(sender, e2)
            Case "lnkActividad5"
                lnkActividad5_Click(sender, e2)
            Case "lnkActividad6"
                lnkActividad6_Click(sender, e2)
            Case "lnkActividad7"
                lnkActividad7_Click(sender, e2)
            Case "lnkActividad8"
                lnkActividad8_Click(sender, e2)
            Case "lnkActividad9"
                lnkActividad9_Click(sender, e2)
            Case "lnkActividad10"
                LinkButton10_Click(sender, e2)
            Case "lnkActividad11"
                LinkButton11_Click(sender, e2)
            Case "lnkActividad12"
                LinkButton12_Click(sender, e2)
        End Select

        hfIdRow.Value = ""
    End Sub

    Private Sub MostrarMensaje(ByVal nomObj As String)
        If hfIdRow.Value <> "" Then
            Dim lnk As LinkButton = CType(dtlmes.Items(hfIdRow.Value).FindControl(nomObj), LinkButton)
            Dim usuario As New Usuario
            Dim folio As String = usuario.ObtenFolioSolAgenda(Convert.ToInt32(lnk.Text.Split("-")(0)))
            Dim dtDetalle As DataTable = usuario.ObtenDetalleAgenda(Convert.ToInt32(lnk.Text.Split("-")(0)))
            Dim leyendaSolicitud As String = "<tr><td><b>SOLICITUD: " + folio + "</b></td></tr><tr><td>&nbsp;</td></tr>"
            Dim cadena As String
            cadena = "<html>" + Chr(13)
            If Not String.IsNullOrEmpty(folio) Then
                cadena += "<body> <table> " + leyendaSolicitud + " <tr><td> <b>" + dtDetalle.Rows(0)(1).ToString() + Chr(13) + "</b></td> </tr><tr><td>&nbsp;</td></tr>"
            Else
                cadena += "<body> <table> <tr><td> <b>" + dtDetalle.Rows(0)(1).ToString() + Chr(13) + "</b></td> </tr><tr><td>&nbsp;</td></tr>"
            End If
            cadena += "<tr><td> <b>Inicio:</b> " + Convert.ToDateTime(dtDetalle.Rows(0)(0).ToString()).ToLongDateString() + " a las " + Convert.ToDateTime(dtDetalle.Rows(0)(0).ToString()).ToLongTimeString()
            cadena += "<tr><td> <b>Fin:</b> " + Convert.ToDateTime(dtDetalle.Rows(dtDetalle.Rows.Count - 1)(0).ToString()).ToLongDateString() + " a las " + Convert.ToDateTime(dtDetalle.Rows(dtDetalle.Rows.Count - 1)(0)).AddHours(1).ToLongTimeString()
            cadena += "</table>"
            If dtDetalle.Rows(0)(2) Then
                cadena += "<br><table><tr><td>Ciclica: <input type=" + Chr(34) + "checkbox" + Chr(34) + _
                          "name=" + Chr(34) + "option1" + Chr(34) + "checked disabled=" + Chr(34) + _
                          "disabled" + Chr(34) + "></td><td align=" + Chr(34) + "right" + Chr(34) + ">" + dtDetalle.Columns(3).ColumnName + _
                          ": <input type=" + Chr(34) + "checkbox" + Chr(34) + "name=" + Chr(34) + "option1" + Chr(34) + "disabled=" + Chr(34) + "disabled" + Chr(34) + If(dtDetalle.Rows(0)(3), "checked", "") + "></td></tr>"
                For i As Integer = 4 To 7
                    cadena += "<tr><td></td><td align=" + Chr(34) + "right" + Chr(34) + ">" + dtDetalle.Columns(i).ColumnName + ": <input type=" + Chr(34) + "checkbox" + Chr(34) + "name=" + Chr(34) + "option1" + Chr(34) + "disabled=" + Chr(34) + "disabled" + Chr(34) + If(dtDetalle.Rows(0)(i), " checked ", "") + "></td></tr>"
                Next
                cadena += "</table></body></html>"
            End If
            Mensaje = cadena
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeUnBotonAgenda();", True)
        End If
    End Sub

    Protected Sub lnkActividad1_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad1"
        MostrarMensaje("lnkActividad1")
    End Sub

    Protected Sub lnkActividad2_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad2"
        MostrarMensaje("lnkActividad2")
    End Sub

    Protected Sub lnkActividad3_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad3"
        MostrarMensaje("lnkActividad3")
    End Sub

    Protected Sub lnkActividad4_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad4"
        MostrarMensaje("lnkActividad4")
    End Sub

    Protected Sub lnkActividad5_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad5"
        MostrarMensaje("lnkActividad5")
    End Sub

    Protected Sub lnkActividad6_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad6"
        MostrarMensaje("lnkActividad6")
    End Sub

    Protected Sub lnkActividad7_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad7"
        MostrarMensaje("lnkActividad7")
    End Sub

    Protected Sub lnkActividad8_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad8"
        MostrarMensaje("lnkActividad8")
    End Sub

    Protected Sub lnkActividad9_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad9"
        MostrarMensaje("lnkActividad9")
    End Sub

    Protected Sub LinkButton10_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad10"
        MostrarMensaje("lnkActividad10")
    End Sub

    Protected Sub LinkButton11_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad11"
        MostrarMensaje("lnkActividad11")
    End Sub

    Protected Sub LinkButton12_Click(ByVal sender As Object, ByVal e As EventArgs)
        hfLinkId.Value = "lnkActividad12"
        MostrarMensaje("lnkActividad12")
    End Sub
End Class