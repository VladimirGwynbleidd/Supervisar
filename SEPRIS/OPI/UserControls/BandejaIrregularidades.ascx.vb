Public Class BandejaIrregularidades
    Inherits System.Web.UI.UserControl

    Private ReadOnly Property Folio
        Get
            Return Session("I_ID_OPI")
        End Get
    End Property

    Public Sub Inicializar()
        CargarIrregularidades()
    End Sub

    Private Sub CargarIrregularidades()
        gvReqInformac.DataSource = Entities.IrregularidadOPI.ObtenerTodas(Folio)
        gvReqInformac.DataBind()

        If gvReqInformac.Rows.Count = 0 Then
            gvReqInformac.Visible = True
            gvReqInformac.Visible = False
        Else
            gvReqInformac.Visible = False
            gvReqInformac.Visible = True
        End If
    End Sub
End Class