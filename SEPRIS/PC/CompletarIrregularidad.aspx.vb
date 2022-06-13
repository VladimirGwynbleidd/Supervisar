Public Class CompletarIrregularidad
    Inherits System.Web.UI.Page

    'Public ReadOnly Property Folio
    '    Get
    '        Return Session("ID_FOLIO")
    '    End Get
    'End Property

    'Private Irreg As Integer
    Public ReadOnly Property Irreg
        Get
            Return ViewState("Irreg")
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If CInt(Request.QueryString("Irreg")) > 0 Then
                Call Inicializar()
            End If
        End If
    End Sub

    Public Sub Inicializar()

        ViewState("Irreg") = CInt(Request.QueryString("Irreg"))
        TxtNumIrregularidad.Text = Irreg
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

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
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
            .Add(Mid(TxtFecCorreccion.Text, 7, 4) & Mid(TxtFecCorreccion.Text, 3, 4) & Mid(TxtFecCorreccion.Text, 1, 2))
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