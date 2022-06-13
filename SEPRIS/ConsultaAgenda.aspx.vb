Imports Entities

Public Class ConsultaAgenda
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim usuario As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
                Dim dtIngenieros As DataTable = usuario.ObtenerIngenieros
                divCalendar.Visible = False
                divDropDownList.Visible = True
                CargaDLL(dtIngenieros)
                hfIdIng.Value = usuario.IdentificadorUsuario
                calendar1.Datasource = CreateCalendar(DateTime.Now, hfIdIng.Value)
                calendar1.ReloadCalender()
                rbTodos.Checked = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function CreateCalendar(ByVal Fecha As DateTime, ByVal IdIng As String) As DataTable
        Try
            Dim dsDatos As DataTable
            Dim FecheIni As String = "01-" + Convert.ToString(Fecha.Month) + "-" + Convert.ToString(Fecha.Year)
            Dim FechaFin As String = Convert.ToString(DateTime.DaysInMonth(Fecha.Year, Fecha.Month)) + "-" + Fecha.Month.ToString() + "-" + Fecha.Year.ToString()
            lblMesAnio.Text = UCase(MonthName(Fecha.Month, False)) + " " + Fecha.Year.ToString()
            dsDatos = ObtenDatosAgendaIng(IdIng, Convert.ToString(Fecha.Month), Convert.ToString(Fecha.Year))
            Fecha = Convert.ToDateTime(FecheIni)
            Session("Fecha") = Fecha
            calendar1.Fecha = Fecha
            Dim dtCalender As DataTable = New DataTable()
            Dim c1 As DataColumn = New DataColumn("Dia")
            Dim c2 As DataColumn = New DataColumn("DiaDes")
            Dim c4 As DataColumn = New DataColumn("Actividad1")
            Dim c5 As DataColumn = New DataColumn("Actividad2")
            Dim c6 As DataColumn = New DataColumn("Actividad3")
            Dim c7 As DataColumn = New DataColumn("Actividad4")
            Dim c8 As DataColumn = New DataColumn("Actividad5")
            Dim c9 As DataColumn = New DataColumn("Actividad6")
            Dim c10 As DataColumn = New DataColumn("Actividad7")
            Dim c11 As DataColumn = New DataColumn("Actividad8")
            Dim c12 As DataColumn = New DataColumn("Actividad9")
            Dim c13 As DataColumn = New DataColumn("Actividad10")
            Dim c14 As DataColumn = New DataColumn("Actividad11")
            Dim c15 As DataColumn = New DataColumn("Actividad12")

            dtCalender.Columns.Add(c1)
            dtCalender.Columns.Add(c2)
            dtCalender.Columns.Add(c4)
            dtCalender.Columns.Add(c5)
            dtCalender.Columns.Add(c6)
            dtCalender.Columns.Add(c7)
            dtCalender.Columns.Add(c8)
            dtCalender.Columns.Add(c9)
            dtCalender.Columns.Add(c10)
            dtCalender.Columns.Add(c11)
            dtCalender.Columns.Add(c12)
            dtCalender.Columns.Add(c13)
            dtCalender.Columns.Add(c14)
            dtCalender.Columns.Add(c15)

            Dim DaysMonth As Integer = DateTime.DaysInMonth(Fecha.Year, Fecha.Month)
            Dim FirstDay As Integer = (CType(Fecha.DayOfWeek, Integer) + 1)
            Dim DaysDif As Integer = FirstDay - 1

            For x As Integer = 1 To 42
                Dim row As DataRow = dtCalender.NewRow()
                If FirstDay <= x And x - DaysDif <= DaysMonth Then
                    row("Dia") = x - DaysDif
                    Dim strValor As String = ""
                    Dim expression As String = (x - DaysDif).ToString("0#") + "/" + Fecha.Month.ToString("0#") + "/" + Fecha.Year.ToString()
                    Dim results = (From myRow In dsDatos.AsEnumerable()
                                  Where myRow.Field(Of DateTime)("F_FECH_FECHA_HORA_TAREA").ToString().Contains(expression)
                                  Select New With
                                    {
                                        .IdAgenda = myRow.Field(Of Decimal)("N_ID_REGISTRO_AGENDA").ToString(),
                                        .Fecha = myRow.Field(Of DateTime)("F_FECH_FECHA_HORA_TAREA").ToString(),
                                        .Data = myRow.Field(Of String)("T_DSC_TAREA_AGENDA")
                                    }).ToList()

                    Dim contador As Integer = 1
                    For Each rowquery In results
                        strValor = String.Format("{0} - {1}", rowquery.IdAgenda, If(rowquery.Data.Length > 18, rowquery.Data.Substring(0, 18), rowquery.Data))
                        Dim nombre As String = "Actividad" + contador.ToString()
                        row(nombre) = strValor
                        contador = contador + 1
                    Next
                Else
                    row("Dia") = String.Empty
                    row("Actividad1") = String.Empty
                    row("Actividad2") = String.Empty
                    row("Actividad3") = String.Empty
                    row("Actividad4") = String.Empty
                    row("Actividad5") = String.Empty
                    row("Actividad6") = String.Empty
                    row("Actividad7") = String.Empty
                    row("Actividad8") = String.Empty
                    row("Actividad9") = String.Empty
                    row("Actividad10") = String.Empty
                    row("Actividad11") = String.Empty
                    row("Actividad12") = String.Empty
                End If
                If x <= 7 Then
                    row("DiaDes") = ObtenDia(x)
                End If
                If Not IsDBNull(row.ItemArray(1)) Or row.ItemArray(0) <> "" Then
                    dtCalender.Rows.Add(row)
                End If
            Next
            Session("dtCalender") = dtCalender
            Return dtCalender
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ObtenSiIngEsSubDirector(ByVal id As String) As Integer
        Dim resul As Integer = 0
        Dim area As New Area
        Dim dv As DataView = area.ObtenerTodosConNombreResponsables()
        Dim dt As DataTable = dv.ToTable()
        Dim query = (From a In dt.AsEnumerable()
                    Where a.Field(Of String)("T_ID_SUBDIRECTOR") = id _
                    Or a.Field(Of String)("T_ID_BACKUP") = id _
                    And a.Field(Of Boolean)("B_FLAG_VIG") = True
                    Select New With
                    {
                        .Area = a.Field(Of Decimal)("N_ID_AREA")
                    }).ToList()

        If query.Count > 0 Then
            resul = Convert.ToInt32(query(0).Area)
        End If

        Return resul
    End Function

    Private Function ObtenDia(ByVal Dia As Integer) As String
        Dim strDia As String = String.Empty
        Select Case Dia
            Case 1
                strDia = "Domingo"
            Case 2
                strDia = "Lunes"
            Case 3
                strDia = "Martes"
            Case 4
                strDia = "Miercoles"
            Case 5
                strDia = "Jueves"
            Case 6
                strDia = "Viernes"
            Case 7
                strDia = "Sabado"
        End Select
        Return strDia
    End Function

    Private Sub CargaDLL(ByVal dt As DataTable)
        For Each row As DataRow In dt.Rows
            row(1) = row(0) + " - " + row(1)
        Next
        Utilerias.Generales.CargarCombo(ddlIngenieros, dt, "NOMBRE_COMPLETO", "T_ID_USUARIO")
    End Sub

    Protected Sub ddlIngenieros_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlIngenieros.SelectedIndexChanged
        If ddlIngenieros.SelectedValue <> "-1" Then
            hfIdIng.Value = ddlIngenieros.SelectedValue
            divCalendar.Visible = True
            calendar1.Datasource = CreateCalendar(DateTime.Now, hfIdIng.Value)
            calendar1.ReloadCalender()
        Else
            divCalendar.Visible = False
        End If
    End Sub

    Protected Sub btnMesAnt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMesAnt.Click
        Dim Mes As Integer = CType(Session("Fecha"), DateTime).Month
        Dim Fecha As DateTime

        If Mes > 1 Then
            Mes = Mes - 1
            Fecha = Convert.ToDateTime("01-" + Mes.ToString() + "-" + CType(Session("Fecha"), DateTime).Year.ToString())
        Else
            Dim Anio As Integer = CType(Session("Fecha"), DateTime).Year
            Anio = Anio - 1
            Fecha = Convert.ToDateTime("01-12-" + Anio.ToString())
        End If
        calendar1.Datasource = CreateCalendar(Fecha, hfIdIng.Value)
        calendar1.ReloadCalender()
    End Sub

    Protected Sub btnMesSig_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMesSig.Click
        Dim Mes As Integer = CType(Session("Fecha"), DateTime).Month
        Dim Fecha As DateTime

        If Mes < 12 Then
            Mes = Mes + 1
            Fecha = Convert.ToDateTime("01-" + Mes.ToString() + "-" + CType(Session("Fecha"), DateTime).Year.ToString())
        Else
            Dim Anio As Integer = CType(Session("Fecha"), DateTime).Year
            Anio = Anio + 1
            Fecha = Convert.ToDateTime("01-01-" + Anio.ToString())
        End If
        calendar1.Datasource = CreateCalendar(Fecha, hfIdIng.Value)
        calendar1.ReloadCalender()
    End Sub

    Private Function ObtenDatosAgendaIng(ByVal IdIng As String, ByVal Mes As Integer, ByVal Anio As Integer) As DataTable
        Dim objUsuario As New Usuario()
        Dim tipoBusqueda As Integer = ObtenTipoBusqueda()
        Dim tableData As DataTable = objUsuario.ObtenerDatosAgendaIng(IdIng, Mes, Anio, tipoBusqueda)
        Return tableData
    End Function

    Private Function ObtenTipoBusqueda() As Integer
        Dim Resul As Integer = 0

        If rbActividad.Checked Then
            Resul = 1
        ElseIf rbAusencia.Checked Then
            Resul = 2
        ElseIf rbServicio.Checked Then
            Resul = 3
        Else
            Resul = 4
        End If

        Return Resul
    End Function

    Protected Sub rbActividad_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbActividad.CheckedChanged
        If ddlIngenieros.SelectedValue <> "-1" Then
            divCalendar.Visible = True
            calendar1.Datasource = CreateCalendar(Session("Fecha"), hfIdIng.Value)
            calendar1.ReloadCalender()
        Else
            divCalendar.Visible = False
        End If
    End Sub

    Protected Sub rbAusencia_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbAusencia.CheckedChanged
        If ddlIngenieros.SelectedValue <> "-1" Then
            divCalendar.Visible = True
            calendar1.Datasource = CreateCalendar(Session("Fecha"), hfIdIng.Value)
            calendar1.ReloadCalender()
        Else
            divCalendar.Visible = False
        End If
    End Sub

    Protected Sub rbTodos_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbTodos.CheckedChanged
        If ddlIngenieros.SelectedValue <> "-1" Then
            divCalendar.Visible = True
            calendar1.Datasource = CreateCalendar(Session("Fecha"), hfIdIng.Value)
            calendar1.ReloadCalender()
        Else
            divCalendar.Visible = False
        End If
    End Sub

    Protected Sub rbServicio_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbServicio.CheckedChanged
        If ddlIngenieros.SelectedValue <> "-1" Then
            divCalendar.Visible = True
            calendar1.Datasource = CreateCalendar(Session("Fecha"), hfIdIng.Value)
            calendar1.ReloadCalender()
        Else
            divCalendar.Visible = False
        End If
    End Sub
End Class

