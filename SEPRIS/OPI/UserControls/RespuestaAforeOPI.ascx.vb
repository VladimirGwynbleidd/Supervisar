Public Class RespuestaAforeOPI
    Inherits System.Web.UI.UserControl

    Private ParamOpiInicial As Boolean = False

    Public Property ComboVisible
        Get
            Return Session("ddlRespuestaAforeOPI")
        End Get
        Set(value)
            Session("ddlRespuestaAforeOPI") = value
        End Set
    End Property
    Public Property ValRespAfore
        Get
            Return ddlRespuestaAforeOPI.SelectedValue
        End Get
        Set(value)
            txtRespuestaAforeOPI.Text = value
        End Set
    End Property

    Public WriteOnly Property SetRespOPIInicial
        Set(value)
            ParamOpiInicial = value
        End Set
    End Property


    Public ReadOnly Property ValIndexRespuesta
        Get
            Return ddlRespuestaAforeOPI.SelectedIndex
        End Get

    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Inicializar()
        End If
    End Sub

    Public Sub Inicializar()
        If ComboVisible Then
            LblAsterico.Style("Visibility") = "visible"
            DivTxtClasifOPI.Style("Display") = "none"
            DivComboClasifOPI.Style("Visibility") = "visible"
        Else
            LblAsterico.Style("Display") = "none"
            DivComboClasifOPI.Style("Display") = "none"
            DivTxtClasifOPI.Style("Visibility") = "visible"
        End If

        ddlRespuestaAforeOPI.Items.Clear()
        ddlRespuestaAforeOPI.Items.Add("--Selecciona una opción--")
        ddlRespuestaAforeOPI.Items(0).Value = 0
        If Not ParamOpiInicial Then
            ddlRespuestaAforeOPI.Items.Add("Respuesta de requerimiento")
        Else
            ddlRespuestaAforeOPI.Items.Add("Respuesta a Oficio de Observaciones")
        End If
        ddlRespuestaAforeOPI.Items.Add("Prórroga de entrega de información")
        ddlRespuestaAforeOPI.Items.Add("No hay respuesta en tiempo establecido")
    End Sub

End Class
