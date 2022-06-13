
Public Class MensajeModal
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnModalOk.OnClientClick = String.Format("fnClickOK('{0}','{1}')", btnModalOk.UniqueID, "")
        btnModalCancelar.OnClientClick = String.Format("fnClickCancelar('{0}','{1}')", btnModalCancelar.UniqueID, "")
    End Sub

    Public Enum MensajeModalBotones
        Confirmacion = 1
        AceptarCancelar = 2
    End Enum


#Region "Propiedades"

    Private _tituloBackColor As System.Drawing.Color = Drawing.Color.Gray
    Public Property TituloBackColor() As System.Drawing.Color
        Get
            Return _tituloBackColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            _tituloBackColor = value
            tdTitulo.Style.Add("background-color", System.Drawing.ColorTranslator.ToHtml(_tituloBackColor))
        End Set
    End Property


    Private _backColor As System.Drawing.Color = Drawing.Color.LightGray
    Public Property BackColor() As System.Drawing.Color
        Get
            Return _backColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            _backColor = value
            pnlPopup.BackColor = _backColor
        End Set
    End Property

    Private _foreColor As System.Drawing.Color = Drawing.Color.Black
    Public Property ForeColor() As System.Drawing.Color
        Get
            Return _foreColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            _foreColor = value
            pnlPopup.ForeColor = _foreColor
        End Set
    End Property

    Public ReadOnly Property PostBackOption() As String
        Get
            Return lblPostBackOption.Text
        End Get
    End Property

    Private _width As Unit = Unit.Pixel(500)
    Public Property Width() As Unit
        Get
            Return _width
        End Get
        Set(ByVal value As Unit)
            _width = value
        End Set
    End Property

    Private _backgroundCssClass As String
    Public Property BackgroundCssClass() As String
        Get
            Return _backgroundCssClass
        End Get
        Set(ByVal value As String)
            _backgroundCssClass = value
            mpext.BackgroundCssClass = _backgroundCssClass
        End Set
    End Property

    Private _cssClass As String
    Public Property CssClass() As String
        Get
            Return _cssClass
        End Get
        Set(ByVal value As String)
            _cssClass = value
            pnlPopup.CssClass = _cssClass
        End Set
    End Property


#End Region

#Region "Métodos"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As String)

        lblMessage.Text = Mensaje
        lblTitulo.Text = ""

        pnlPopup.Width = _width
        mpext.Show()
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As List(Of String))

        lblMessage.Text = "<ul>"
        For Each item In Mensaje
            lblMessage.Text &= "<li>" & item & "</li>"
        Next
        lblMessage.Text &= "</ul>"
        lblMessage.Style.Add("display", "block")

        lblTitulo.Text = ""

        Me.btnModalCancelar.Style.Add("display", "none")
        pnlPopup.Width = _width
        mpext.Show()

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="Titulo"></param>
    ''' <remarks></remarks>
    Public Sub ShowM(ByVal Mensaje As String, ByVal Titulo As String)

        lblMessage.Text = Mensaje
        lblTitulo.Text = Titulo

        Me.btnModalCancelar.Style.Add("display", "none")
        pnlPopup.Width = _width
        mpext.Show()
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As List(Of String), ByVal Titulo As String)

        lblMessage.Text = "<ul>"
        For Each item In Mensaje
            lblMessage.Text &= "<li>" & item & "</li>"
        Next
        lblMessage.Text &= "</ul>"
        lblMessage.Style.Add("display", "block")

        lblTitulo.Text = Titulo
        Me.btnModalCancelar.Style.Add("display", "none")
        pnlPopup.Width = _width
        mpext.Show()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="validaciones"></param>
    ''' <param name="Titulo"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As String, ByVal validaciones As List(Of String), ByVal Titulo As String)
        lblMessage.Text = "  " + Mensaje + "</br> " + "<ul>"
        For Each item In validaciones
            lblMessage.Text &= "<li>" & item & "</li>"
        Next
        lblMessage.Text &= "</ul>"
        lblMessage.Style.Add("display", "block")

        lblTitulo.Text = Titulo
        Me.btnModalCancelar.Style.Add("display", "none")
        pnlPopup.Width = _width
        mpext.Show()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="validaciones"></param>
    ''' <param name="Titulo"></param>
    ''' <param name="Botones"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As String, ByVal validaciones As List(Of String), ByVal Titulo As String, ByVal Botones As MensajeModalBotones)
        lblMessage.Text = "  " + Mensaje + "</br> " + "<ul>"
        For Each item In validaciones
            lblMessage.Text &= "<li>" & item & "</li>"
        Next
        lblMessage.Text &= "</ul>"
        lblMessage.Style.Add("display", "block")

        lblTitulo.Text = Titulo

        If Botones = MensajeModalBotones.Confirmacion Then
            Me.btnModalCancelar.Style.Add("display", "none")
            'Me.lblSpace.Style.Add("display", "none")
        Else
            Me.btnModalCancelar.Style.Add("display", "inline")
            'Me.lblSpace.Style.Add("display", "inline")
        End If

        pnlPopup.Width = _width
        mpext.Show()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="Titulo"></param>
    ''' <param name="Botones"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As String, ByVal Titulo As String, ByVal Botones As MensajeModalBotones)

        lblMessage.Text = Mensaje
        lblTitulo.Text = Titulo
        If Botones = MensajeModalBotones.Confirmacion Then
            Me.btnModalCancelar.Style.Add("display", "none")
        Else
            Me.btnModalCancelar.Style.Add("display", "inline")
        End If

        pnlPopup.Width = _width
        mpext.Show()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="Titulo"></param>
    ''' <param name="Botones"></param>
    ''' <remarks></remarks>

    Public Sub Show(ByVal Mensaje As List(Of String), ByVal Titulo As String, ByVal Botones As MensajeModalBotones)
        lblMessage.Text = "<ul>"
        For Each item In Mensaje
            lblMessage.Text &= "<li>" & item & "</li>"
        Next
        lblMessage.Text &= "</ul>"
        lblMessage.Style.Add("display", "block")

        lblTitulo.Text = Titulo

        If Botones = MensajeModalBotones.Confirmacion Then
            Me.btnModalCancelar.Style.Add("display", "none")
            'Me.lblSpace.Style.Add("display", "none")
        End If

        pnlPopup.Width = _width
        mpext.Show()
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="Titulo"></param>
    ''' <param name="Botones"></param>
    ''' <param name="PostBackOption"></param>
    ''' <remarks></remarks>
    Public Sub Show(ByVal Mensaje As String, ByVal Titulo As String, ByVal Botones As MensajeModalBotones, ByVal PostBackOption As String)
        lblMessage.Text = Mensaje
        lblTitulo.Text = Titulo

        If Botones = MensajeModalBotones.Confirmacion Then
            Me.btnModalCancelar.Style.Add("display", "none")
        End If

        lblPostBackOption.Text = PostBackOption

        pnlPopup.Width = _width
        mpext.Show()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="Titulo"></param>
    ''' <param name="Botones"></param>
    ''' <param name="PostBackOption"></param>
    ''' <remarks></remarks>

    Public Sub Show(ByVal Mensaje As List(Of String), ByVal Titulo As String, ByVal Botones As MensajeModalBotones, ByVal PostBackOption As String)
        lblMessage.Text = "<ul>"
        For Each item In Mensaje
            lblMessage.Text &= "<li>" & item & "</li>"
        Next
        lblMessage.Text &= "</ul>"
        lblMessage.Style.Add("display", "block")

        lblTitulo.Text = Titulo

        If Botones = MensajeModalBotones.Confirmacion Then
            Me.btnModalCancelar.Style.Add("display", "none")
        End If

        lblPostBackOption.Text = PostBackOption

        pnlPopup.Width = _width
        mpext.Show()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Hide()

        lblMessage.Text = ""
        lblTitulo.Text = ""
        mpext.Hide()
    End Sub

#End Region

#Region "Eventos"

    '----------------------------------
    ' Exponiendo el Evento click del botón aceptar
    '----------------------------------
    Public Event OkButtonPressed(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModalOk.Click
        RaiseEvent OkButtonPressed(Me, e)
    End Sub

    '----------------------------------
    ' Exponiendo el Evento click del botón cancelar
    '----------------------------------

    Public Event CancelButtonPressed(ByVal sender As Object, ByVal e As EventArgs)

    Protected Sub OnCancelButtonPressed(ByVal sender As Object, ByVal e As EventArgs) Handles btnModalCancelar.Click

        RaiseEvent CancelButtonPressed(Me, e)

    End Sub

#End Region

End Class