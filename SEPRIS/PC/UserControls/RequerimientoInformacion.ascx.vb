Public Class RequerimientoInformacion
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
            Return Session("ID_USR")
        End Get
    End Property

    Public ReadOnly Property EstatusPC
        Get
            Dim PC As New Entities.PC(Folio)
            PC = Session("PC")
            Return PC.IdEstatus
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Inicializar(Folio)
    End Sub

    Public Sub Inicializar(iFolio)
        Dim dtDocumentos As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(iFolio)
        Dim dvDocumentos As DataView = dtDocumentos.DefaultView
        gvReqInformac.DataSource = dtDocumentos
        gvReqInformac.DataBind()

        If EstatusPC >= 9 Then
            gvReqInformac.Columns(7).Visible = False
            gvReqInformac.Columns(8).Visible = False
        End If
    End Sub

    Protected Sub gvReqInformac_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReqInformac.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim fechaAcuse As String = ConexionSICOD.ObtenerFechaAcuse(e.Row.Cells(3).Text)

            If fechaAcuse <> String.Empty Then

                If e.Row.Cells(5).Text = String.Empty Or e.Row.Cells(5).Text = "&nbsp;" Then
                    Dim DiasHab As Integer = 0

                    Dim NewDate As Date = fechaAcuse

                    While DiasHab < BandejaPC.ObtenerDiasVencimiento(81)
                        NewDate = CDate(NewDate).AddDays(1).ToString("dd/MM/yyyy")
                        If Not (NewDate.DayOfWeek = DayOfWeek.Sunday Or NewDate.DayOfWeek = DayOfWeek.Saturday Or ConexionSICOD.IsFestivo(NewDate.ToString("yyyy-MM-dd")) = "Si") Then
                            DiasHab = DiasHab + 1
                        End If
                    End While

                    Entities.RequerimientoPC.ActualizarDatos(New List(Of String)({"F_FECH_ESTIMADA"}), New List(Of Object)({NewDate.ToString("yyyyMMdd")}), Int32.Parse(gvReqInformac.DataKeys(e.Row.RowIndex).Value))
                    e.Row.Cells(5).Text = NewDate.ToString("dd/MM/yyyy")
                End If


                Entities.RequerimientoPC.ActualizarDatos(New List(Of String)({"F_FECH_ACUSE"}), New List(Of Object)({Date.ParseExact(fechaAcuse, "dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd")}), Int32.Parse(gvReqInformac.DataKeys(e.Row.RowIndex).Value))
                e.Row.Cells(4).Text = ConexionSICOD.ObtenerFechaAcuse(e.Row.Cells(3).Text)

            End If
        End If
    End Sub

End Class