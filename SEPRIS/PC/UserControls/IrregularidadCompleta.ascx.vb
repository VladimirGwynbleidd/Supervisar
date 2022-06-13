Public Class IrregularidadCompleta
    Inherits System.Web.UI.UserControl

    Property Irreg = 0

    Public Sub Inicializar()
        ViewState("Irreg") = CInt(Request.QueryString("Irreg"))
        TxtNumIrregularidad.Text = 0
        Llena_Datos()
    End Sub


    Private Sub Llena_Datos()
        Dim aDatos As String()
        aDatos = cIrregularidades.CargaDatosCompletar(Irreg)

        txtFecIrregularidad.Text = aDatos(1)
        TxtProceso.Text = ConexionSISAN.TraeDescProcesosxID(CInt(aDatos(2)))
        TxtSubproceso.Text = ConexionSISAN.TraeDescSubprocesosxID(CInt(aDatos(3)))
        TxtConducta.Text = ConexionSISAN.TraeDescConductaxID(CInt(aDatos(4)))
        txtDescIrregularidad.Text = aDatos(5)

    End Sub


    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs)
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstCamposCond As New List(Of String)
        Dim lstValorCond As New List(Of Object)

        With lstCampos
            .Add("T_DSC_CORREGIDO")
            .Add("T_DSC_COMOCORRIGE")
            .Add("F_FECH_CORRECCION")
        End With
        With lstValores
            .Add(ddlCorreccion.Text)
            .Add(TxtComoCorrigeIrregularidad.Text)
            '.Add(Mid(TxtFecCorreccion.Text, 7, 4) & Mid(TxtFecCorreccion.Text, 3, 4) & Mid(TxtFecCorreccion.Text, 1, 2))
            .Add(Date.ParseExact(TxtFecCorreccion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        End With

        With lstCamposCond
            .Add("I_ID_IRREGULARIDAD")
        End With

        With lstValorCond
            .Add(Irreg)
        End With
        cIrregularidades.GuardaCompletarIrregularidad(lstCampos, lstValores, lstCamposCond, lstValorCond)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Fin de Guardado", "AvisoGuardarDatos();", True)
    End Sub
End Class