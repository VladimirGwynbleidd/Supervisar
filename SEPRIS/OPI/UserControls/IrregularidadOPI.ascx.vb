Public Class IrregularidadOPI
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property ValExisteIrreg
        Get
            Return txtExisteIrregOPI.Text
        End Get
    End Property

    Private _irr As Boolean

    Public ReadOnly Property ValJustificac
        Get
            Return txtJustificacionOPI.Text
        End Get
    End Property

    Private _irrStd As Boolean
    Public Property ValIrregStand() As Boolean
        Get
            If rdbIrregStandardOPI.Items(0).Selected Then
                _irrStd = True
            Else
                _irrStd = False
            End If

            Return _irrStd
        End Get
        Set(ByVal value As Boolean)

            If value Then
                rdbIrregStandardOPI.Items(0).Selected = True
                rdbIrregStandardOPI.Items(1).Selected = False
            Else
                rdbIrregStandardOPI.Items(1).Selected = True
                rdbIrregStandardOPI.Items(0).Selected = False
            End If

        End Set
    End Property


    Public Property ValIsIrregularidad
        Get
            Return rdbExisteIrregOPI.SelectedValue
        End Get
        Set(value)
            'rdbExisteIrregOPI.SelectedValue = value
            If value Then
                rdbExisteIrregOPI.Items(0).Selected = True
                rdbExisteIrregOPI.Items(1).Selected = False
            Else
                rdbExisteIrregOPI.Items(1).Selected = True
                rdbExisteIrregOPI.Items(0).Selected = False
            End If
            If (value = True) Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OcultaIrregularidad", "MostrarOcultar(1);", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "MuestraIrregularidad", "MostrarOcultar(0);", True)

            End If
        End Set
    End Property

    Public Property MuestraIrregStd
        Get
            Return Session("MuestraIrregStd")
        End Get
        Set(value)
            Session("MuestraIrregStd") = value
            If value Then
                IDIrregStd.Visible = True
            Else
                IDIrregStd.Visible = False
            End If
            If value Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivaCampos", "ActivaExisteIrreg('S');", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivaCampos", "ActivaExisteIrreg('N');", True)
            End If
        End Set
    End Property

    Public Property MuestraExisteIrregularidad
        Get
            Return Session("MuestraExisteIrregularidad")
        End Get
        Set(value)
            Session("MuestraExisteIrregularidad") = value
            If value Then
                ExisteIrreg.Visible = True
                IDJustNOIrreg.Visible = True
            Else
                IDJustNOIrreg.Visible = False
                ExisteIrreg.Visible = False
            End If
            If value Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivaCampos", "ActivaExisteIrreg('S');", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivaCampos", "ActivaExisteIrreg('N');", True)
            End If
        End Set
    End Property
    Public Property HabilitaCampos
        Get
            Return rdbIrregStandardOPI.Enabled()
        End Get
        Set(value)
            rdbIrregStandardOPI.Enabled = False
            rdbExisteIrregOPI.Enabled = False
            txtJustificacionOPI.Enabled = False
        End Set
    End Property

    Public Property ValJustificacion
        Get
            Return txtJustificacionOPI.Text
        End Get
        Set(value)
            txtJustificacionOPI.Text = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

        Else
            'FSW CAGC SOFTTEK Incidencia Corregida Núm 71 y 48, Documento 38
            If rdbExisteIrregOPI.Items(0).Selected = True Then
                IDJustNOIrreg.Visible = False
            End If
        End If
    End Sub


    Private Sub rdbExisteIrregOPI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdbExisteIrregOPI.SelectedIndexChanged
        Session("EXISTE_IRREG") = rdbExisteIrregOPI.SelectedValue
    End Sub
End Class