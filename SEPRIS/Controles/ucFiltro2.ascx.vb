Imports System.Globalization

<Themeable(True)>
Public Class ucFiltro2
    Inherits System.Web.UI.UserControl

    Public Event Filtrar As EventHandler
    Protected WithEvents btnFiltrar As Button

    Public Property Valor As Object

    Public Property Mensaje As String

    ' UC = User Control
    Private ancho As String
    ''' <summary>
    ''' Permite obtener o asignar el ancho del filtro
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Width As String
        Get
            Return ancho
        End Get
        Set(ByVal value As String)
            ancho = value
            ucFiltroContainer.Style.Add("width", value)
        End Set
    End Property

    ''' <summary>
    ''' Enumeración para los controls aceptados por el UC
    ''' </summary>
    ''' <remarks></remarks>
    <Flags()> Public Enum AcceptedControls As Integer
        TextBox = 0
        DropDownList = 1
        Calendar = 2
        RadioButton = 3
        CheckBox = 4
        DropDownListR = 5
    End Enum

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private _ddlList As New List(Of String)

    ''' <summary>
    ''' Enumeración para seleccionar el tipo de salida (entrecomillado o no)
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DataValueType As Integer
        IntegerType = 0
        StringType = 1
        BoolType = -1
        RangeType = 2
    End Enum

    ''' <summary>
    ''' Variable Dummy para implementación futura
    ''' </summary>
    ''' <remarks></remarks>
    Dim controlID As String = "ucFiltro2"

    ''' <summary>
    ''' Variable para Redibujar controles
    ''' </summary>
    ''' <remarks></remarks>
    Shared createAgain As Boolean = False

    ''' <summary>
    ''' Instancia de clase auxiliar para guardar las opciones de inicio por cada filtro
    ''' </summary>
    ''' <remarks></remarks>
    Shared criteriosSeleccion As New List(Of CriterioSeleccion2)

    ''' <summary>
    ''' Variable asociada a propiedad para guardar la ClientID del botón que disparará la extracción del filtraje seleccionado.
    ''' </summary>
    ''' <remarks></remarks>
    Shared _setSelectionButton As String
    ''' <summary>
    ''' Consecutivo para controlar los controles guardados en la variable de sesión
    ''' </summary>
    ''' <remarks></remarks>
    Shared consSessionID As Integer = 0

    ''' <summary>
    ''' Nombre del botón encargado de disparar la extracción los filtros seleccionados
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectionButton() As String
        Get
            Return _setSelectionButton
        End Get
        Set(ByVal value As String)
            _setSelectionButton = value
        End Set
    End Property

    Private _ucTemplateControlPath As String = "ASP.controles_filtro2"
    ''' <summary>
    ''' Propiedad que guarda el prefijo del nombre de los controles en tiempo de ejecución
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SessionID() As String
        Get
            Return _ucTemplateControlPath
        End Get
        Set(ByVal value As String)
            _ucTemplateControlPath = value
        End Set
    End Property

    Private _ucPath As String = "~/Controles/filtro2/"
    ''' <summary>
    ''' Propiedad que guarda la ruta de los controles
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ucPath() As String
        Get
            Return _ucPath
        End Get
        Set(ByVal value As String)
            _ucPath = value
        End Set
    End Property

    Private campoFixedTemp As String = SessionID + "_CambioFixed"
    ''' <summary>
    ''' Almacena el Texto de un filtro cuyo boton "X" fue eliminado
    ''' al quedar solo con el filtro Solo Míos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property CambiaCampoFixed() As String
        Get
            If IsNothing(Session(campoFixedTemp)) Then
                Session(campoFixedTemp) = ""
            End If
            Return Session(campoFixedTemp).ToString()
        End Get
        Set(ByVal value As String)
            Session(campoFixedTemp) = value
        End Set
    End Property

    ''' <summary>
    ''' Init del control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim ctrl As Control = GetPostBackControl(Me.Page)
        'Checa si el postback es lanzado por el dropdown
        'o
        'si createAgain es verdadero
        'lo cual quiere decir que la llamada se hizo mientras el control estaba activo

        If (ctrl IsNot Nothing AndAlso ctrl.ClientID = ddlAgregar.ClientID) _
                OrElse (ctrl IsNot Nothing AndAlso ctrl.ClientID = _setSelectionButton) _
                    OrElse createAgain Then
            'Debe de establecerse antes de la llamada a CreateUserControl
            createAgain = True
            CreateUserControl()
        End If

    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    ''' <summary>
    ''' Método público para agregar un nuevo filtro
    ''' </summary>
    ''' <param name="Nombre">Texto del Label que aparece a la izquierda del control</param>
    ''' <param name="TipoControl">Valor del tipo AcceptedControls (int) para delimitar los controles aceptados</param>
    ''' <param name="FuenteDatos">DataSource del control</param>
    ''' <param name="DataTextField">Campo de la Fuente de Datos que se mostrará al usuario</param>
    ''' <param name="DataValueField">Campo de la Fuente de Datos que el usuario seleccionará</param>
    ''' <param name="DataValueType">Tipo de DataValueType para formato de salida (Integer o String)</param>
    ''' <param name="isCalendarSingle">En controles tipo Calendar si presentará un solo control Calendar</param>
    ''' <param name="isTextBoxLike">En controles tipo TextBox si la salida será en formato LIKE '%x%'</param>
    ''' <remarks>Las enumeraciones son del tipo entero</remarks>
    Public Sub AddFilter(ByVal Nombre As String,
                        ByVal TipoControl As AcceptedControls,
                        Optional ByVal FuenteDatos As Object = Nothing,
                        Optional ByVal DataTextField As String = "",
                        Optional ByVal DataValueField As String = "",
                        Optional ByVal DataValueType As DataValueType = DataValueType.IntegerType,
                        Optional ByVal isCalendarSingle As Boolean = False,
                        Optional ByVal isTextBoxLike As Boolean = False,
                        Optional ByVal isTextBoxRange As Boolean = False,
                        Optional ByVal isDefault As Boolean = False,
                        Optional ByVal isFixed As Boolean = False,
                        Optional ByVal initValue As Object = Nothing,
                        Optional ByVal maxLength As Integer = 50,
                        Optional ByVal isDdlRange As Boolean = False)

        '-------------------------------------------------
        'Checar si elemento ya existe en el dropdownlist
        '-------------------------------------------------
        If ddlAgregar.Items.Count > 1 Then
            Dim li As ListItem = ddlAgregar.Items.FindByText(Nombre)
            If li IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim cs As New CriterioSeleccion2
        cs.Nombre = Nombre
        cs.Tipo = TipoControl
        cs.Source = FuenteDatos
        cs.DataTextField = DataTextField
        cs.DataValueField = DataValueField
        cs.isCalendarSingle = isCalendarSingle
        cs.isTextBoxLike = isTextBoxLike
        cs.DataValueType = DataValueType
        cs.IsTextBoxRange = isTextBoxRange
        cs.IsFixed = isFixed
        cs.InitValue = initValue
        cs.MaxLength = maxLength
        cs.IsDdlRange = isDdlRange
        criteriosSeleccion.Add(cs)



        If isDefault Then
            Select Case cs.Tipo
                Case AcceptedControls.TextBox
                    CreateTextbox(cs.Nombre, cs.DataValueField, cs.DataValueType, cs.isTextBoxLike, cs.IsTextBoxRange, isFixed, cs.MaxLength)
                Case AcceptedControls.DropDownList
                    CreateDropDownList(cs.Nombre, cs.Source, cs.DataTextField, cs.DataValueField, cs.DataValueType, isFixed, CStr(initValue))
                Case AcceptedControls.DropDownListR
                    CreateDropDownListR(cs.Nombre, cs.Source, cs.DataTextField, cs.DataValueField, cs.DataValueType, isFixed, CStr(initValue), cs.IsDdlRange)
                Case AcceptedControls.Calendar
                    CreateCalendar(cs.Nombre, cs.DataValueField, isCalendarSingle, isDefault, isFixed, CDate(initValue))
                Case AcceptedControls.RadioButton
                    CreateRadioButton(cs.Nombre, cs.Source, cs.DataValueField, cs.DataTextField, cs.DataValueType, isFixed)
                Case AcceptedControls.CheckBox
                    CreateCheckBox(cs.Nombre, cs.Source, cs.DataValueField, cs.DataTextField, cs.DataValueType, isFixed, CInt(initValue))
            End Select
        Else
            _ddlList.Add(Nombre)
        End If

    End Sub

    ''' <summary>
    ''' Método público para agregar un nuevo filtro en la parte inferior a la altura del boton filtar
    ''' </summary>
    ''' <param name="Nombre">Texto del Label que aparece a la izquierda del control</param>
    ''' <param name="TipoControl">Valor del tipo AcceptedControls (int) para delimitar los controles aceptados</param>
    ''' <param name="FuenteDatos">DataSource del control</param>
    ''' <param name="DataTextField">Campo de la Fuente de Datos que se mostrará al usuario</param>
    ''' <param name="DataValueField">Campo de la Fuente de Datos que el usuario seleccionará</param>
    ''' <param name="DataValueType">Tipo de DataValueType para formato de salida (Integer o String)</param>
    ''' <param name="isCalendarSingle">En controles tipo Calendar si presentará un solo control Calendar</param>
    ''' <param name="isTextBoxLike">En controles tipo TextBox si la salida será en formato LIKE '%x%'</param>
    ''' <remarks>Las enumeraciones son del tipo entero</remarks>
    Public Sub AddFilterBottom(ByVal Nombre As String,
                        ByVal TipoControl As AcceptedControls,
                        Optional ByVal FuenteDatos As Object = Nothing,
                        Optional ByVal DataTextField As String = "",
                        Optional ByVal DataValueField As String = "",
                        Optional ByVal DataValueType As DataValueType = DataValueType.IntegerType,
                        Optional ByVal isCalendarSingle As Boolean = False,
                        Optional ByVal isTextBoxLike As Boolean = False,
                        Optional ByVal isTextBoxRange As Boolean = False,
                        Optional ByVal isDefault As Boolean = False,
                        Optional ByVal isFixed As Boolean = False,
                        Optional ByVal initValue As Object = Nothing)


        Dim cs As New CriterioSeleccion2
        cs.Nombre = Nombre
        cs.Tipo = TipoControl
        cs.Source = FuenteDatos
        cs.DataTextField = DataTextField
        cs.DataValueField = DataValueField
        cs.isCalendarSingle = isCalendarSingle
        cs.isTextBoxLike = isTextBoxLike
        cs.DataValueType = DataValueType
        cs.IsTextBoxRange = isTextBoxRange
        cs.IsFixed = isFixed
        cs.InitValue = initValue
        criteriosSeleccion.Add(cs)



        If isDefault Then
            Select Case cs.Tipo
                'Case AcceptedControls.TextBox
                '    CreateTextbox(cs.Nombre, cs.DataValueField, cs.DataValueType, cs.isTextBoxLike, cs.IsTextBoxRange, isFixed)
                'Case AcceptedControls.DropDownList
                '    CreateDropDownList(cs.Nombre, cs.Source, cs.DataTextField, cs.DataValueField, cs.DataValueType, isFixed, CStr(initValue))
                'Case AcceptedControls.Calendar
                '    CreateCalendar(cs.Nombre, cs.DataValueField, isCalendarSingle, isDefault, isFixed, CDate(initValue))
                'Case AcceptedControls.RadioButton
                '    CreateRadioButton(cs.Nombre, cs.Source, cs.DataValueField, cs.DataTextField, cs.DataValueType, isFixed)
                Case AcceptedControls.CheckBox
                    CreateCheckBoxButtom(cs.Nombre, cs.Source, cs.DataValueField, cs.DataTextField, cs.DataValueType, isFixed, CInt(initValue))
            End Select
        Else
            _ddlList.Add(Nombre)
        End If

    End Sub


    ''' <summary>
    ''' Método Inicial del Control
    ''' </summary>
    ''' <param name="NombreSessionPreviewFilters">Nombre de la variable de sesion donde se almacenaran los valores previamente seleccionados del control para la página padre</param>
    ''' <remarks></remarks>
    Public Sub LoadDDL(ByVal NombreSessionPreviewFilters As String)
        hdn_Session.Value = NombreSessionPreviewFilters
        'SelectionButton = NombreSessionPreviewFilters
        For Each item As String In _ddlList
            ddlAgregar.Items.Add(item)
        Next
        SortDDL(ddlAgregar)

        CargaValoresFiltro()

        'Si existe un filtro Solo Míos y solo existe un filtro ademas de el, a este ultimo se le quita el boton "X"
        DeshabilitaEliminacionSoloMios()
    End Sub

    Private Sub instatiateCriterio()

    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un DropDownList
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucDropDownList_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucDropDownList As ddlUserControl2 = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, ddlUserControl2)
        removeControl(ucDropDownList)
    End Sub

    Protected Sub ucDropDownListR_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucDropDownListR As ddlUserControlR2 = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, ddlUserControlR2)
        removeControl(ucDropDownListR)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un TextBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucTextBox_onPostClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucTextBox As txtUserControl2 = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, txtUserControl2)
        removeControl(ucTextBox)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un Calendar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucCalendar_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucCalendar As cldUserControl2 = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, cldUserControl2)
        removeControl(ucCalendar)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un RadioButton
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucRadioButton_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucRadioButton As rbUserControl2 = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, rbUserControl2)
        removeControl(ucRadioButton)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar del CheckList
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucCheckList_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucCheckList As chkUserControl2 = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, chkUserControl2)
        removeControl(ucCheckList)
    End Sub


    ''' <summary>
    ''' Método para eliminar control del ucFiltro (control principal) y de la sesión. Regresa el control al DropDown ddlAgregar
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub removeControl(ByVal obj As Object)

        Dim MyType As Type = obj.GetType

        Select Case obj.ToString
            Case _ucTemplateControlPath & "_cldusercontrol_ascx"
                PlaceHolder1.Controls.Remove(DirectCast(obj, cldUserControl2))
                Session.Remove(DirectCast(obj, cldUserControl2).SessionID)
                ddlAgregar.Items.Add(DirectCast(obj, cldUserControl2).labelText)
            Case _ucTemplateControlPath & "_rbusercontrol_ascx"
                PlaceHolder1.Controls.Remove(DirectCast(obj, rbUserControl2))
                Session.Remove(DirectCast(obj, rbUserControl2).SessionID)
                ddlAgregar.Items.Add(DirectCast(obj, rbUserControl2).labelText)
            Case _ucTemplateControlPath & "_txtusercontrol_ascx"
                PlaceHolder1.Controls.Remove(DirectCast(obj, txtUserControl2))
                Session.Remove(DirectCast(obj, txtUserControl2).SessionID)
                ddlAgregar.Items.Add(DirectCast(obj, txtUserControl2).labelText)
            Case _ucTemplateControlPath & "_ddlusercontrol_ascx"
                PlaceHolder1.Controls.Remove(DirectCast(obj, ddlUserControl2))
                Session.Remove(DirectCast(obj, ddlUserControl2).SessionID)
                ddlAgregar.Items.Add(DirectCast(obj, ddlUserControl2).labelText)
            Case _ucTemplateControlPath & "_ddlusercontrolr_ascx"
                PlaceHolder1.Controls.Remove(DirectCast(obj, ddlUserControlR2))
                Session.Remove(DirectCast(obj, ddlUserControlR2).SessionID)
                ddlAgregar.Items.Add(DirectCast(obj, ddlUserControlR2).labelText)
            Case _ucTemplateControlPath & "_chkusercontrol_ascx"
                PlaceHolder1.Controls.Remove(DirectCast(obj, chkUserControl2))
                Session.Remove(DirectCast(obj, chkUserControl2).SessionID)
                ddlAgregar.Items.Add(DirectCast(obj, chkUserControl2).labelText)

        End Select

        SortDDL(ddlAgregar)

        'Si existe el filtro de Solo Míos, debe existir siempre un filtro ademas de el
        'por lo que si solo queda uno se oculta su boton de eliminacion
        DeshabilitaEliminacionSoloMios()

    End Sub

    ''' <summary>
    ''' Crea un nuevo UC tipo TextBox
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="isTextBoxLike"></param>
    ''' <remarks></remarks>
    Private Sub CreateTextbox(
                             ByVal name As String,
                             ByVal DataValueField As String,
                             ByVal DataValueType As DataValueType,
                             ByVal isTextBoxLike As Boolean,
                             ByVal isTextBoxRange As Boolean,
                             ByVal isFixed As Boolean,
                             ByVal maxLength As Integer)
        'Crea instancia del control
        Dim ucTextBox As txtUserControl2 = DirectCast(LoadControl(ucPath & "txtUserControl2.ascx"), txtUserControl2)


        '-------------------------------------------
        'Establece las propiedades
        '-------------------------------------------
        ucTextBox.labelText = name
        ucTextBox.Text1 = ""
        ucTextBox.Text2 = ""
        ucTextBox.SessionID = ucTextBox.TemplateControl.ToString & consSessionID.ToString
        ucTextBox.DataValueField = DataValueField
        ucTextBox.isTextBoxLike = isTextBoxLike
        ucTextBox.isTextBoxRange = isTextBoxRange
        ucTextBox.DataValueType = DataValueType
        ucTextBox.ID = ucTextBox.SessionID
        ucTextBox.isFixed = isFixed
        ucTextBox.maxLength = maxLength

        'Crea el evento eliminar asociado al botón eliminar
        AddHandler ucTextBox.btnPostClk, AddressOf Me.ucTextBox_onPostClk

        'Añade el control al PlaceHolder y a la sesión
        addControl(ucTextBox, ucTextBox.SessionID)

    End Sub

    ''' <summary>
    ''' Crea un nuevo UC tipo DropDownList
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="source"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataValueType"></param>
    ''' <remarks></remarks>
    Private Sub CreateDropDownList(
                                  ByVal name As String,
                                  ByVal source As Object,
                                  ByVal DataTextField As String,
                                  ByVal DataValueField As String,
                                  ByVal DataValueType As DataValueType,
                                  ByVal isFixed As Boolean,
                                  ByVal initValue As String)

        'Crea instancia del control
        Dim ucDropDownList As ddlUserControl2 = DirectCast(LoadControl(ucPath & "ddlUserControl2.ascx"), ddlUserControl2)

        'Establece las propiedades
        ucDropDownList.DataValueType = DataValueType
        ucDropDownList.labelText = name
        ucDropDownList.source = source
        ucDropDownList.DataValueField = DataValueField
        ucDropDownList.DataTextField = DataTextField
        ucDropDownList.Bind()
        ucDropDownList.SessionID = ucDropDownList.TemplateControl.ToString & consSessionID.ToString
        ucDropDownList.ID = ucDropDownList.SessionID

        ucDropDownList.isFixed = isFixed
        ucDropDownList.selectedValue = initValue

        If Not initValue Is Nothing AndAlso initValue IsNot String.Empty Then
            ucDropDownList.selectedValue = initValue
        End If

        'Añade el control al PlaceHolder y a la sesión.
        addControl(ucDropDownList, ucDropDownList.SessionID)
    End Sub

    Private Sub CreateDropDownListR(
                                   ByVal name As String,
                                   ByVal source As Object,
                                   ByVal DataTextField As String,
                                   ByVal DataValueField As String,
                                   ByVal DataValueType As DataValueType,
                                   ByVal isFixed As Boolean,
                                   ByVal initValue As String,
                                   ByVal isDdlRange As Boolean)

        'Crea instancia del control
        Dim ucDropDownListR As ddlUserControlR2 = DirectCast(LoadControl(ucPath & "ddlUserControlR2.ascx"), ddlUserControlR2)

        'Establece las propiedades
        ucDropDownListR.DataValueType = DataValueType
        ucDropDownListR.labelText = name
        ucDropDownListR.source = source
        ucDropDownListR.DataValueField = DataValueField
        ucDropDownListR.DataTextField = DataTextField
        ucDropDownListR.Bind()
        ucDropDownListR.SessionID = ucDropDownListR.TemplateControl.ToString & consSessionID.ToString
        ucDropDownListR.ID = ucDropDownListR.SessionID

        ucDropDownListR.isFixed = isFixed
        ucDropDownListR.selectedValue = initValue

        If Not initValue Is Nothing AndAlso initValue IsNot String.Empty Then
            ucDropDownListR.selectedValue = initValue
        End If

        'Añade el control al PlaceHolder y a la sesión.
        addControl(ucDropDownListR, ucDropDownListR.SessionID)
    End Sub
    ''' <summary>
    ''' Crea un nuevo UC tipo Calendar (rango de fechas)
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="isCalendarSingle"></param>
    ''' <remarks></remarks>

    Private Sub CreateCalendar(ByVal name As String,
                                    ByVal DataValueField As String,
                                    ByVal isCalendarSingle As Boolean,
                                    Optional ByVal isDefault As Boolean = False,
                                    Optional ByVal isFixed As Boolean = False,
                                    Optional ByVal initValue As Date = #12:00:00 AM#)

        '----------------------------------------------
        'Crea instancia del control
        '----------------------------------------------
        Dim ucCalendar As cldUserControl2 = DirectCast(LoadControl(ucPath & "cldUserControl2.ascx"), cldUserControl2)

        '----------------------------------------------
        'Establece las propiedades
        '----------------------------------------------
        ucCalendar.labelText = name
        ucCalendar.SessionID = ucCalendar.TemplateControl.ToString & consSessionID.ToString
        ucCalendar.FechaInicio = ""
        ucCalendar.fechaFin = ""
        ucCalendar.DataValueField = DataValueField
        ucCalendar.isCalendarSingle = isCalendarSingle
        ucCalendar.ID = ucCalendar.SessionID
        ucCalendar.isFixed = isFixed

        '----------------------------------------------
        'Crea el evento eliminar asociado al botón eliminar
        '----------------------------------------------
        AddHandler ucCalendar.btnPostClk, AddressOf Me.ucDropDownList_onDeleteClk

        If Not initValue = Date.MinValue Then
            '------------------------------------------------
            ' Si initValueDate es mayor que hoy, ponerla como fecha de fin.
            '------------------------------------------------
            If initValue > Today.Date Then
                ucCalendar.FechaInicio = Date.Today.ToString("dd/MM/yyyy")
                ucCalendar.fechaFin = initValue.ToString("dd/MM/yyyy")
            ElseIf initValue < Today.Date Then
                '------------------------------------------------
                ' Si initValueDate es menor que hoy, ponerla como fecha de inicio.
                '------------------------------------------------
                ucCalendar.FechaInicio = initValue.ToString("dd/MM/yyyy")
                ucCalendar.fechaFin = Date.Today.ToString("dd/MM/yyyy")
            Else
                '------------------------------------------------
                ' Si es igual a hoy, poner ambas fechas al día presente.
                '------------------------------------------------
                ucCalendar.FechaInicio = Date.Today.ToString("dd/MM/yyyy")
                ucCalendar.fechaFin = Date.Today.ToString("dd/MM/yyyy")
            End If
        Else
            ucCalendar.FechaInicio = Date.Today.ToString("dd/MM/yyyy")
            ucCalendar.fechaFin = Date.Today.ToString("dd/MM/yyyy")
        End If

        '----------------------------------------------
        'Añade el control al PlaceHolder y a la sesión
        '----------------------------------------------
        addControl(ucCalendar, ucCalendar.SessionID)

    End Sub

    ''' <summary>
    ''' Crea un nuevo UC tipo RadioButtonList
    ''' </summary>
    ''' <param name="Nombre"></param>
    ''' <param name="Source"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="DataValueType"></param>
    ''' <remarks></remarks>
    Private Sub CreateRadioButton(
                                 ByVal Nombre As String,
                                 ByVal Source As Object,
                                 ByVal DataValueField As String,
                                 ByVal DataTextField As String,
                                 ByVal DataValueType As DataValueType,
                                 ByVal isFixed As Boolean)

        'Crea instancia del control
        Dim ucRadioButton As rbUserControl2 = DirectCast(LoadControl(ucPath & "rbUserControl2.ascx"), rbUserControl2)

        'Establece las propiedades
        ucRadioButton.labelText = Nombre
        ucRadioButton.source = Source
        ucRadioButton.DataValueField = DataValueField
        ucRadioButton.DataTextField = DataTextField
        ucRadioButton.DataValueType = DataValueType
        ucRadioButton.bind()
        ucRadioButton.initSelectedItem()
        ucRadioButton.SessionID = ucRadioButton.TemplateControl.ToString & consSessionID.ToString
        ucRadioButton.ID = ucRadioButton.SessionID
        ucRadioButton.isFixed = isFixed

        'Crea el evento eliminar asociado al botón eliminar
        AddHandler ucRadioButton.btnPostClk, AddressOf Me.ucRadioButton_onDeleteClk

        'Añade el control al PlaceHolder y a la sesión
        addControl(ucRadioButton, ucRadioButton.SessionID)
    End Sub



    ''' <summary>
    ''' Crea un nuevo UC tipo CheckBox en la parte Inferior
    ''' </summary>
    ''' <param name="Nombre"></param>
    ''' <param name="Source"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="DataValueType"></param>
    ''' <remarks></remarks>
    Private Sub CreateCheckBoxButtom(
                                 ByVal Nombre As String,
                                 ByVal Source As Object,
                                 ByVal DataValueField As String,
                                 ByVal DataTextField As String,
                                 ByVal DataValueType As DataValueType,
                                 ByVal isFixed As Boolean,
                                 ByVal initValue As Integer)

        'Crea instancia del control
        Dim ucCheckBox As chkButtomUserControl2 = DirectCast(LoadControl(ucPath & "chkButtomUserControl2.ascx"), chkButtomUserControl2)

        'Establece las propiedades
        ucCheckBox.labelText = Nombre
        ucCheckBox.source = Source
        ucCheckBox.DataValueField = DataValueField
        ucCheckBox.DataTextField = DataTextField
        ucCheckBox.DataValueType = DataValueType
        ucCheckBox.bind()
        ucCheckBox.initSelectedItem()
        ucCheckBox.SessionID = ucCheckBox.TemplateControl.ToString & consSessionID.ToString
        ucCheckBox.ID = ucCheckBox.SessionID
        ucCheckBox.isFixed = isFixed
        ucCheckBox.selectedValue = CBool(initValue)

        'Crea el evento eliminar asociado al botón eliminar
        AddHandler ucCheckBox.btnPostClk, AddressOf Me.ucCheckList_onDeleteClk

        'Añade el control al PlaceHolder y a la sesión
        addControlButtom(ucCheckBox, ucCheckBox.SessionID)
    End Sub



    ''' <summary>
    ''' Crea un nuevo UC tipo CheckBox
    ''' </summary>
    ''' <param name="Nombre"></param>
    ''' <param name="Source"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="DataValueType"></param>
    ''' <remarks></remarks>
    Private Sub CreateCheckBox(
                                 ByVal Nombre As String,
                                 ByVal Source As Object,
                                 ByVal DataValueField As String,
                                 ByVal DataTextField As String,
                                 ByVal DataValueType As DataValueType,
                                 ByVal isFixed As Boolean,
                                 ByVal initValue As Integer)

        'Crea instancia del control
        Dim ucCheckBox As chkUserControl2 = DirectCast(LoadControl(ucPath & "chkUserControl2.ascx"), chkUserControl2)

        'Establece las propiedades
        ucCheckBox.labelText = Nombre
        ucCheckBox.source = Source
        ucCheckBox.DataValueField = DataValueField
        ucCheckBox.DataTextField = DataTextField
        ucCheckBox.DataValueType = DataValueType
        ucCheckBox.bind()
        ucCheckBox.initSelectedItem()
        ucCheckBox.SessionID = ucCheckBox.TemplateControl.ToString & consSessionID.ToString
        ucCheckBox.ID = ucCheckBox.SessionID
        ucCheckBox.isFixed = isFixed
        ucCheckBox.selectedValue = CBool(initValue)

        'Crea el evento eliminar asociado al botón eliminar
        AddHandler ucCheckBox.btnPostClk, AddressOf Me.ucCheckList_onDeleteClk

        'Añade el control al PlaceHolder y a la sesión
        addControl(ucCheckBox, ucCheckBox.SessionID)
    End Sub


    ''' <summary>
    ''' Añade el control al PlaceHolder2 y a la sesión en la parte inferior
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="SessionID"></param>
    ''' <remarks></remarks>
    Private Sub addControlButtom(ByVal obj As Control, ByVal SessionID As String)

        'Dim Cell As New TableCell
        'Cell.Controls.Add(obj)

        'TableRow2.Cells.Add(Cell)

        PlaceHolder2.Controls.Add(obj)

        'Agrega el control a la variable de sesion
        Session.Add(SessionID, obj)

        'Aumenta el contador de controles dentro de la variable de sesión
        consSessionID += 1

        ' Set createAgain = true
        createAgain = True
    End Sub

    ''' <summary>
    ''' Añade el control al PlaceHolder y a la sesión
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="SessionID"></param>
    ''' <remarks></remarks>
    Private Sub addControl(ByVal obj As Control, ByVal SessionID As String)

        ' Agrega el control al PlaceHolder
        PlaceHolder1.Controls.Add(obj)

        'Agrega el control a la variable de sesion
        Session.Add(SessionID, obj)

        'Aumenta el contador de controles dentro de la variable de sesión
        consSessionID += 1

        ' Set createAgain = true
        createAgain = True
    End Sub

    ''' <summary>
    ''' Establece si la str es una fecha válida
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FechaValida(ByVal str As String, ByVal culture As CultureInfo, ByVal style As DateTimeStyles) As Boolean
        Try
            DateTime.Parse(str, culture, style)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Verifica que la fecha de inicio se previa a la fecha de fin.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function VerificaFechas(ByVal strFechaInicial As String, ByVal strFechaFinal As String, ByVal culture As CultureInfo, ByVal style As DateTimeStyles) As Boolean

        Try
            '--------------------------------------------------
            'Verifica que las fechas del periodo sean validas
            '--------------------------------------------------
            If FechaValida(strFechaInicial, culture, style) And FechaValida(strFechaFinal, culture, style) Then
                Dim FechaInicial, FechaFinal As Date
                FechaInicial = DateTime.Parse(strFechaInicial, culture, style)
                FechaFinal = DateTime.Parse(strFechaFinal, culture, style)


                If FechaInicial <= FechaFinal Then
                    Return True
                Else


                End If

            End If

            Return False
        Catch ex As SystemException
            Throw ex
            Return False
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Función que prepara los valores seleccionados para su extracción.
    ''' </summary>
    ''' <returns>'Lista de string que contiene los valores seleccionados.</returns>
    ''' <remarks>El formateo de la string de salida se lleva a cabo aquí.</remarks>
    Public Function getFilterSelection() As List(Of String)

        Dim SelValues As New List(Of String)
        Dim FiltroParaSesion As New Dictionary(Of String, String)

        Try
            If PlaceHolder1 IsNot Nothing Then
                For Each Control As Control In PlaceHolder1.Controls
                    Select Case Control.TemplateControl.ToString
                        Case SessionID & "_txtusercontrol2_ascx"
                            Dim ucTextBox As txtUserControl = TryCast(Control, txtUserControl)
                            ucTextBox.FetchValues()
                            Dim currentSelValue As String = String.Empty

                            If ucTextBox.Text1 IsNot String.Empty Then
                                If ucTextBox.isTextBoxLike Then
                                    currentSelValue = ucTextBox.DataValueField & " LIKE '%" & ucTextBox.Text1 & "%'"
                                Else
                                    If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                        currentSelValue = ucTextBox.DataValueField & "=" & ucTextBox.Text1 & ""
                                    Else
                                        currentSelValue = ucTextBox.DataValueField & "='" & ucTextBox.Text1 & "'"
                                    End If
                                End If

                                If ucTextBox.isTextBoxRange Then
                                    If ucTextBox.Text2 IsNot String.Empty Then
                                        If ucTextBox.isTextBoxLike Then
                                            currentSelValue = ucTextBox.DataValueField & " BETWEEN '%" & ucTextBox.Text1 & "%' AND '%" & ucTextBox.Text2 & "%'"
                                        Else
                                            If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                                currentSelValue = ucTextBox.DataValueField & " BETWEEN " & ucTextBox.Text1 & " AND " & ucTextBox.Text2
                                            Else
                                                currentSelValue = ucTextBox.DataValueField & " BETWEEN '" & ucTextBox.Text1 & "' AND '" & ucTextBox.Text2 & "'"
                                            End If

                                        End If
                                    End If
                                End If

                                SelValues.Add(currentSelValue)
                                FiltroParaSesion.Add(ucTextBox.labelText, ucTextBox.Text1 & "|" & ucTextBox.Text2)

                            End If

                        Case SessionID & "_ddlusercontrol2_ascx"
                            Dim ucDropDownList As ddlUserControl = TryCast(Control, ddlUserControl)

                            ucDropDownList.FetchValues()

                            If ucDropDownList.DataValueType = DataValueType.IntegerType Then
                                SelValues.Add(ucDropDownList.DataValueField & "=" & ucDropDownList.selectedValue)
                            ElseIf ucDropDownList.DataValueType = DataValueType.StringType Then
                                SelValues.Add(ucDropDownList.DataValueField & "='" & ucDropDownList.selectedValue & "'")
                                'Se agrega validacion para los tipos boleanos
                                'Se utilizará cuando sea un filtro para de vigencia
                            ElseIf ucDropDownList.DataValueType = DataValueType.BoolType Then
                                If ucDropDownList.selectedValue <> "-1" Then
                                    SelValues.Add(ucDropDownList.DataValueField & "=" & ucDropDownList.selectedValue)
                                End If
                            End If

                            FiltroParaSesion.Add(ucDropDownList.labelText, ucDropDownList.selectedValue)

                        Case SessionID & "_ddlusercontrolr2_ascx"
                            Dim ucDropDownListR As ddlUserControlR = TryCast(Control, ddlUserControlR)

                            ucDropDownListR.FetchValues()

                            If ucDropDownListR.DataValueType = DataValueType.RangeType Then
                                If ucDropDownListR.selectedValue <> "-1" And ucDropDownListR.selectedValue2 <> "-1" Then
                                    SelValues.Add(ucDropDownListR.DataValueField & ">=" & ucDropDownListR.selectedValue)
                                    SelValues.Add(ucDropDownListR.DataValueField & "<=" & ucDropDownListR.selectedValue2)
                                End If
                            End If

                            FiltroParaSesion.Add(ucDropDownListR.labelText, ucDropDownListR.selectedValue)

                        Case SessionID & "_cldusercontrol2_ascx"
                            Dim ucCalendar As cldUserControl = TryCast(Control, cldUserControl)
                            ucCalendar.FetchValues()
                            '---------------------------------------------
                            '---------------------------------------------
                            Dim culture As CultureInfo
                            culture = CultureInfo.CreateSpecificCulture("es-MX")
                            '---------------------------------------------
                            '---------------------------------------------
                            Dim styles As DateTimeStyles
                            styles = DateTimeStyles.None
                            '---------------------------------------------
                            '---------------------------------------------
                            If ucCalendar.isCalendarSingle Then
                                If ucCalendar.FechaInicio.Length > 0 Then

                                    If FechaValida(ucCalendar.FechaInicio, culture, styles) Then
                                        SelValues.Add(ucCalendar.DataValueField & " = '" & Convert.ToDateTime(ucCalendar.FechaInicio).ToString("yyyy/MM/dd") & "'")
                                        FiltroParaSesion.Add(ucCalendar.labelText, ucCalendar.FechaInicio & "|")
                                    End If

                                End If

                            Else

                                If ucCalendar.FechaInicio.Length > 0 AndAlso ucCalendar.fechaFin.Length > 0 Then
                                    If VerificaFechas(ucCalendar.FechaInicio, ucCalendar.fechaFin, culture, styles) Then
                                        'SelValues.Add(
                                        '                ucCalendar.DataValueField & " BETWEEN '" & _
                                        '                Convert.ToDateTime(ucCalendar.FechaInicio).ToString("yyyy/MM/dd") & _
                                        '                "' AND '" & _
                                        '                Convert.ToDateTime(ucCalendar.fechaFin).ToString("yyyy/MM/dd") & "'")}
                                        SelValues.Add(
                                                        ucCalendar.DataValueField & " >= '" &
                                                        Convert.ToDateTime(ucCalendar.FechaInicio).ToString("yyyy/MM/dd") & " 12:00:00 am" &
                                                        "' AND " &
                                                         ucCalendar.DataValueField & " <= '" &
                                                        Convert.ToDateTime(ucCalendar.fechaFin).ToString("yyyy/MM/dd") & " 11:59:59 pm'")
                                        FiltroParaSesion.Add(ucCalendar.labelText, ucCalendar.FechaInicio & "|" & ucCalendar.fechaFin)
                                    End If
                                End If
                            End If
                        Case SessionID & "_rbusercontrol2_ascx"
                            Dim ucRadioButton As rbUserControl = TryCast(Control, rbUserControl)
                            ucRadioButton.FetchValues()
                            If ucRadioButton.selectedValue IsNot String.Empty Then
                                If ucRadioButton.DataValueType = DataValueType.IntegerType Then
                                    SelValues.Add(ucRadioButton.DataValueField & "=" & ucRadioButton.selectedValue)
                                Else
                                    SelValues.Add(ucRadioButton.DataValueField & "='" & ucRadioButton.selectedValue & "'")
                                End If
                                FiltroParaSesion.Add(ucRadioButton.labelText, ucRadioButton.selectedValue)
                            End If
                        Case SessionID & "_chkusercontrol2_ascx"
                            Dim ucCheckBox As chkUserControl = TryCast(Control, chkUserControl)
                            ucCheckBox.FetchValues()

                            SelValues.Add(ucCheckBox.DataValueField & "=" & Convert.ToInt16(ucCheckBox.selectedValue))
                            FiltroParaSesion.Add(ucCheckBox.labelText, ucCheckBox.selectedValue.ToString())


                            'If ucCheckBox.selectedValue IsNot String.Empty Then
                            '    If ucCheckList.DataValueType = DataValueType.IntegerType Then
                            '        SelValues.Add(ucCheckList.DataValueField & "=" & ucCheckList.selectedValue)
                            '    Else
                            '        SelValues.Add(ucCheckList.DataValueField & "='" & ucCheckList.selectedValue & "'")
                            '    End If
                            'End If

                    End Select
                Next

            End If

            If TableRow2.Cells.Count > 0 Then


                For Each Control As TableCell In TableRow2.Cells

                    Select Case Control.Controls(0).TemplateControl.ToString()
                        Case SessionID & "_chkbuttomusercontrol2_ascx"

                            Dim ucCheckBox As chkButtomUserControl = TryCast(Control.Controls(0), chkButtomUserControl)
                            ucCheckBox.FetchValues()
                            SelValues.Add(ucCheckBox.DataValueField & "=" & Convert.ToInt16(ucCheckBox.selectedValue))
                            FiltroParaSesion.Add(ucCheckBox.labelText, ucCheckBox.selectedValue.ToString())
                    End Select

                Next


            End If

            Session(hdn_Session.Value) = Nothing
            Session(hdn_Session.Value) = FiltroParaSesion


        Catch ex As Exception
            Throw ex
        End Try
        Return SelValues
    End Function

    ''' <summary>
    ''' Obtiene el control que realizó el PostBack
    ''' </summary>
    ''' <param name="thePage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPostBackControl(ByVal thePage As Page) As Control
        Dim myControl As Control = Nothing
        Dim ctrlName As String = thePage.Request.Params.Get("__EVENTTARGET")
        If ((ctrlName IsNot Nothing) And (ctrlName <> String.Empty)) Then
            myControl = thePage.FindControl(ctrlName)
        Else
            For Each Item As String In thePage.Request.Form
                Dim c As Control = thePage.FindControl(Item)
                If (TypeOf (c) Is System.Web.UI.WebControls.DropDownList) Or (TypeOf (c) Is Button) Then
                    myControl = c
                End If
            Next

        End If
        Return myControl
    End Function

    ''' <summary>
    ''' createAgain se estableció como true en el método Pre_init cuando se seleccionó del dropdown una opción
    ''' este campo se utiliza para verificar si el UC está en la página antes del llamado, si es así, se agrega
    ''' al Control de Jerarquías.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CreateUserControl()
        Try
            If createAgain AndAlso PlaceHolder1 IsNot Nothing Then
                If Session.Count > 0 Then
                    PlaceHolder1.Controls.Clear()
                    TableRow2.Cells.Clear()
                    For i As Integer = 0 To Session.Count - 1
                        If Session(i) IsNot Nothing Then
                            Select Case Session(i).ToString()
                                Case SessionID & "_txtusercontrol2_ascx"
                                    Dim ucTextBox As txtUserControl = TryCast(LoadControl(ucPath & "txtUserControl2.ascx"), txtUserControl)

                                    ucTextBox.ID = DirectCast(Session(i), txtUserControl).ID
                                    ucTextBox.SessionID = DirectCast(Session(i), txtUserControl).SessionID
                                    ucTextBox.labelText = DirectCast(Session(i), txtUserControl).labelText
                                    ucTextBox.isTextBoxLike = DirectCast(Session(i), txtUserControl).isTextBoxLike
                                    ucTextBox.isTextBoxRange = DirectCast(Session(i), txtUserControl).isTextBoxRange
                                    ucTextBox.maxLength = DirectCast(Session(i), txtUserControl).maxLength

                                    ucTextBox.DataValueField = DirectCast(Session(i), txtUserControl).DataValueField

                                    'Si el campo esta marcado como que se forzo a fijo por el filtro solo mio
                                    'indica que es campo no es fijo, por lo cual se asigna como no fijo
                                    If ucTextBox.ID.ToString = CambiaCampoFixed Then
                                        ucTextBox.isFixed = False
                                        DirectCast(Session(i), txtUserControl2).isFixed = False
                                    Else
                                        ucTextBox.isFixed = DirectCast(Session(i), txtUserControl2).isFixed
                                    End If

                                    AddHandler ucTextBox.btnPostClk, AddressOf ucTextBox_onPostClk
                                    PlaceHolder1.Controls.Add(ucTextBox)
                                    Exit Select
                                Case SessionID & "_ddlusercontrol2_ascx"
                                    Dim ucDropDownList As ddlUserControl2 = TryCast(LoadControl(ucPath & "ddlUserControl2.ascx"), ddlUserControl2)


                                    ucDropDownList.DataValueType = DirectCast(Session(i), ddlUserControl2).DataValueType
                                    ucDropDownList.ID = DirectCast(Session(i), ddlUserControl2).ID
                                    ucDropDownList.SessionID = DirectCast(Session(i), ddlUserControl2).SessionID
                                    ucDropDownList.source = DirectCast(Session(i), ddlUserControl2).source
                                    ucDropDownList.DataTextField = DirectCast(Session(i), ddlUserControl2).DataTextField
                                    ucDropDownList.DataValueField = DirectCast(Session(i), ddlUserControl2).DataValueField
                                    ucDropDownList.Bind()
                                    ucDropDownList.labelText = DirectCast(Session(i), ddlUserControl2).labelText

                                    'Si el campo esta marcado como que se forzo a fijo por el filtro solo mio
                                    'indica que es campo no es fijo, por lo cual se asigna como no fijo
                                    If ucDropDownList.ID.ToString = CambiaCampoFixed Then
                                        ucDropDownList.isFixed = False
                                        DirectCast(Session(i), ddlUserControl).isFixed = False
                                    Else
                                        ucDropDownList.isFixed = DirectCast(Session(i), ddlUserControl2).isFixed
                                    End If

                                    ucDropDownList.selectedValue = DirectCast(Session(i), ddlUserControl2).selectedValue

                                    AddHandler ucDropDownList.btnPostClk, AddressOf ucDropDownList_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucDropDownList)
                                    Exit Select
                                Case SessionID & "_ddlusercontrolr2_ascx"
                                    Dim ucDropDownListR As ddlUserControlR2 = TryCast(LoadControl(ucPath & "ddlUserControlR2.ascx"), ddlUserControlR2)


                                    ucDropDownListR.DataValueType = DirectCast(Session(i), ddlUserControlR2).DataValueType
                                    ucDropDownListR.ID = DirectCast(Session(i), ddlUserControlR2).ID
                                    ucDropDownListR.SessionID = DirectCast(Session(i), ddlUserControlR2).SessionID
                                    ucDropDownListR.source = DirectCast(Session(i), ddlUserControlR2).source
                                    ucDropDownListR.DataTextField = DirectCast(Session(i), ddlUserControlR2).DataTextField
                                    ucDropDownListR.DataValueField = DirectCast(Session(i), ddlUserControlR2).DataValueField
                                    ucDropDownListR.Bind()
                                    ucDropDownListR.labelText = DirectCast(Session(i), ddlUserControlR2).labelText

                                    'Si el campo esta marcado como que se forzo a fijo por el filtro solo mio
                                    'indica que es campo no es fijo, por lo cual se asigna como no fijo
                                    If ucDropDownListR.ID.ToString = CambiaCampoFixed Then
                                        ucDropDownListR.isFixed = False
                                        DirectCast(Session(i), ddlUserControlR2).isFixed = False
                                    Else
                                        ucDropDownListR.isFixed = DirectCast(Session(i), ddlUserControlR2).isFixed
                                    End If

                                    ucDropDownListR.selectedValue = DirectCast(Session(i), ddlUserControlR2).selectedValue

                                    AddHandler ucDropDownListR.btnPostClk, AddressOf ucDropDownListR_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucDropDownListR)
                                    Exit Select

                                Case SessionID & "_cldusercontrol2_ascx"
                                    Dim ucCalendar As cldUserControl = TryCast(LoadControl(ucPath & "cldUserControl2.ascx"), cldUserControl)

                                    ucCalendar.ID = DirectCast(Session(i), cldUserControl2).ID
                                    ucCalendar.SessionID = DirectCast(Session(i), cldUserControl2).SessionID
                                    ucCalendar.fechaFin = DirectCast(Session(i), cldUserControl2).fechaFin
                                    ucCalendar.FechaInicio = DirectCast(Session(i), cldUserControl2).FechaInicio

                                    ucCalendar.labelText = DirectCast(Session(i), cldUserControl2).labelText
                                    ucCalendar.isCalendarSingle = DirectCast(Session(i), cldUserControl2).isCalendarSingle
                                    ucCalendar.DataValueField = DirectCast(Session(i), cldUserControl2).DataValueField

                                    'Si el campo esta marcado como que se forzo a fijo por el filtro solo mio
                                    'indica que es campo no es fijo, por lo cual se asigna como no fijo
                                    If ucCalendar.ID.ToString = CambiaCampoFixed Then
                                        ucCalendar.isFixed = False
                                        DirectCast(Session(i), cldUserControl2).isFixed = False
                                    Else
                                        ucCalendar.isFixed = DirectCast(Session(i), cldUserControl2).isFixed
                                    End If

                                    AddHandler ucCalendar.btnPostClk, AddressOf ucCalendar_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucCalendar)
                                    Exit Select
                                Case SessionID & "_rbusercontrol2_ascx"
                                    Dim ucRadioButton As rbUserControl2 = TryCast(LoadControl(ucPath & "rbUserControl2.ascx"), rbUserControl2)

                                    ucRadioButton.ID = DirectCast(Session(i), rbUserControl2).ID
                                    ucRadioButton.SessionID = DirectCast(Session(i), rbUserControl2).SessionID
                                    ucRadioButton.source = DirectCast(Session(i), rbUserControl2).source
                                    ucRadioButton.DataValueField = DirectCast(Session(i), rbUserControl2).DataValueField
                                    ucRadioButton.DataTextField = DirectCast(Session(i), rbUserControl2).DataTextField
                                    ucRadioButton.bind()
                                    ucRadioButton.labelText = DirectCast(Session(i), rbUserControl2).labelText
                                    ucRadioButton.DataValueType = DirectCast(Session(i), rbUserControl2).DataValueType

                                    'Si el campo esta marcado como que se forzo a fijo por el filtro solo mio
                                    'indica que es campo no es fijo, por lo cual se asigna como no fijo
                                    If ucRadioButton.ID.ToString = CambiaCampoFixed Then
                                        ucRadioButton.isFixed = False
                                        DirectCast(Session(i), rbUserControl2).isFixed = False
                                    Else
                                        ucRadioButton.isFixed = DirectCast(Session(i), rbUserControl2).isFixed
                                    End If

                                    AddHandler ucRadioButton.btnPostClk, AddressOf ucRadioButton_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucRadioButton)
                                    Exit Select
                                Case SessionID & "_chkusercontrol2_ascx"
                                    Dim ucCheckList As chkUserControl = TryCast(LoadControl(ucPath & "chkUserControl2.ascx"), chkUserControl)

                                    ucCheckList.ID = DirectCast(Session(i), chkUserControl2).ID
                                    ucCheckList.SessionID = DirectCast(Session(i), chkUserControl2).SessionID
                                    ucCheckList.source = DirectCast(Session(i), chkUserControl2).source
                                    ucCheckList.DataValueField = DirectCast(Session(i), chkUserControl2).DataValueField
                                    ucCheckList.DataTextField = DirectCast(Session(i), chkUserControl2).DataTextField
                                    ucCheckList.bind()
                                    ucCheckList.labelText = DirectCast(Session(i), chkUserControl2).labelText
                                    ucCheckList.DataValueType = DirectCast(Session(i), chkUserControl2).DataValueType

                                    'Si el campo esta marcado como que se forzo a fijo por el filtro solo mio
                                    'indica que es campo no es fijo, por lo cual se asigna como no fijo
                                    If ucCheckList.ID.ToString = CambiaCampoFixed Then
                                        ucCheckList.isFixed = False
                                        DirectCast(Session(i), chkUserControl2).isFixed = False
                                    Else
                                        ucCheckList.isFixed = DirectCast(Session(i), chkUserControl2).isFixed
                                    End If

                                    ucCheckList.selectedValue = DirectCast(Session(i), chkUserControl2).selectedValue

                                    AddHandler ucCheckList.btnPostClk, AddressOf ucCheckList_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucCheckList)
                                    Exit Select

                                Case SessionID & "_chkbuttomusercontrol2_ascx"
                                    Dim ucCheckList As chkButtomUserControl2 = TryCast(LoadControl(ucPath & "chkButtomUserControl2.ascx"), chkButtomUserControl2)

                                    ucCheckList.ID = DirectCast(Session(i), chkButtomUserControl2).ID
                                    ucCheckList.SessionID = DirectCast(Session(i), chkButtomUserControl2).SessionID
                                    ucCheckList.source = DirectCast(Session(i), chkButtomUserControl2).source
                                    ucCheckList.DataValueField = DirectCast(Session(i), chkButtomUserControl2).DataValueField
                                    ucCheckList.DataTextField = DirectCast(Session(i), chkButtomUserControl2).DataTextField
                                    ucCheckList.bind()
                                    ucCheckList.labelText = DirectCast(Session(i), chkButtomUserControl2).labelText
                                    ucCheckList.DataValueType = DirectCast(Session(i), chkButtomUserControl2).DataValueType
                                    ucCheckList.isFixed = DirectCast(Session(i), chkButtomUserControl2).isFixed
                                    ucCheckList.selectedValue = DirectCast(Session(i), chkButtomUserControl2).selectedValue

                                    AddHandler ucCheckList.btnPostClk, AddressOf ucCheckList_onDeleteClk
                                    'PlaceHolder1.Controls.Add(ucCheckList)

                                    Dim Cell As New TableCell
                                    Cell.Controls.Add(ucCheckList)
                                    TableRow2.Cells.Add(Cell)


                                    Exit Select





                            End Select
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Evento SelectedIndexChanged del DropDownList que contiene las opciones
    ''' Supervisa el agregado de controles al PlaceHolder
    ''' apoyándose en la clase auxiliar criterioSeleccion que guarda la info de configuración.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlAgregar_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAgregar.SelectedIndexChanged
        Dim result As CriterioSeleccion2 = criteriosSeleccion.Find(AddressOf FindNombre)
        If result IsNot Nothing Then
            Select Case result.Tipo
                Case AcceptedControls.TextBox
                    CreateTextbox(result.Nombre, result.DataValueField, result.DataValueType, result.isTextBoxLike, result.IsTextBoxRange, result.IsFixed, result.MaxLength)
                Case AcceptedControls.DropDownList
                    CreateDropDownList(result.Nombre, result.Source, result.DataTextField, result.DataValueField, result.DataValueType, result.IsFixed, CStr(result.InitValue))
                Case AcceptedControls.DropDownListR
                    CreateDropDownListR(result.Nombre, result.Source, result.DataTextField, result.DataValueField, result.DataValueType, result.IsFixed, CStr(result.InitValue), result.IsDdlRange)
                Case AcceptedControls.Calendar
                    CreateCalendar(result.Nombre, result.DataValueField, result.isCalendarSingle, , result.IsFixed)
                Case AcceptedControls.RadioButton
                    CreateRadioButton(result.Nombre, result.Source, result.DataValueField, result.DataTextField, result.DataValueType, result.IsFixed)
                Case AcceptedControls.CheckBox
                    CreateCheckBox(result.Nombre, result.Source, result.DataValueField, result.DataTextField, result.DataValueType, result.IsFixed, CInt(result.InitValue))
            End Select
            ddlAgregar.Items.Remove(ddlAgregar.SelectedItem.Text)
            SortDDL(ddlAgregar)
        End If

        'Si se elimino el boton "X" de un filtro debido a la existencia del Filtro Solo Mio
        'Al agregar un filtro mas se devuelve el boton "X" al que se le habia quitado
        HabilitaEliminacionSoloMios()
    End Sub

    ''' <summary>
    ''' Funcion auxiliar para recorrer en bucle la Lista criteriosSeleccion
    ''' </summary>
    ''' <param name="cs"></param>
    ''' <returns>boolean</returns>
    ''' <remarks></remarks>
    Private Function FindNombre(ByVal cs As CriterioSeleccion2) As Boolean
        If cs.Nombre = ddlAgregar.SelectedItem.Text Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Método para ordernar alfabéticamente un DropDownList
    ''' </summary>
    ''' <param name="objDDL"></param>
    ''' <remarks></remarks>
    Private Sub SortDDL(ByRef objDDL As DropDownList)
        Dim textList As ArrayList = New ArrayList()
        Dim valueList As ArrayList = New ArrayList()

        For Each li As ListItem In objDDL.Items

            textList.Add(li.Text)
        Next
        textList.Sort()

        For Each item As Object In textList

            Dim value As String = objDDL.Items.FindByText(item.ToString()).Value
            valueList.Add(value)
        Next
        objDDL.Items.Clear()

        For i As Integer = 0 To textList.Count - 1 Step 1
            Dim objItem As ListItem = New ListItem(textList(i).ToString(), valueList(i).ToString())
            objDDL.Items.Add(objItem)
        Next
    End Sub

    Public Sub resetSession()
        If PlaceHolder1 IsNot Nothing AndAlso Session.Count > 0 Then
            For Each Control As Control In PlaceHolder1.Controls
                Select Case Control.TemplateControl.ToString
                    Case SessionID & "_txtusercontrol2_ascx"
                        Dim ucTextBox As txtUserControl2 = TryCast(Control, txtUserControl2)
                        Session.Remove(ucTextBox.SessionID)

                    Case SessionID & "_ddlusercontrol2_ascx"
                        Dim ucDropDownList As ddlUserControl2 = TryCast(Control, ddlUserControl2)
                        Session.Remove(ucDropDownList.SessionID)

                    Case SessionID & "_ddlusercontrolr2_ascx"
                        Dim ucDropDownListR As ddlUserControlR2 = TryCast(Control, ddlUserControlR2)
                        Session.Remove(ucDropDownListR.SessionID)

                    Case SessionID & "_cldusercontrol2_ascx"
                        Dim ucCalendar As cldUserControl2 = TryCast(Control, cldUserControl2)
                        Session.Remove(ucCalendar.SessionID)

                    Case SessionID & "_rbusercontrol2_ascx"
                        Dim ucRadioButton As rbUserControl2 = TryCast(Control, rbUserControl2)
                        Session.Remove(ucRadioButton.SessionID)

                    Case SessionID & "_chkusercontrol2_ascx"
                        Dim ucCheckList As chkUserControl2 = TryCast(Control, chkUserControl2)
                        Session.Remove(ucCheckList.SessionID)

                End Select
            Next
        End If

        'If PlaceHolder2 IsNot Nothing AndAlso Session.Count > 0 Then
        '    For Each Control As Control In PlaceHolder2.Controls
        '        Select Case Control.TemplateControl.ToString        
        '            Case SessionID & "_chkbuttomusercontrol_ascx"
        '                Dim ucCheckList As chkUserControl = TryCast(Control, chkUserControl)
        '                Session.Remove(ucCheckList.SessionID)

        '        End Select
        '    Next
        'End If

        If TableRow2.Cells.Count > 0 Then
            For Each Control As TableCell In TableRow2.Cells
                Select Case Control.Controls(0).TemplateControl.ToString()
                    Case SessionID & "_chkbuttomusercontrol2_ascx"
                        Dim ucCheckBox As chkButtomUserControl2 = TryCast(Control.Controls(0), chkButtomUserControl2)
                        Session.Remove(ucCheckBox.SessionID)
                End Select
            Next
            TableRow2.Cells.Clear()
        End If

        PlaceHolder1.Controls.Clear()
        PlaceHolder2.Controls.Clear()
        consSessionID = 0
        criteriosSeleccion.Clear()

        Dim toRemove As New List(Of String)
        For Each item As String In Session.Keys
            If item.Contains(_ucTemplateControlPath) Then
                toRemove.Add(item)
            End If
        Next

        For Each item In toRemove
            Session.Remove(item)
        Next

    End Sub

    Private Sub CargaValoresFiltro()

        If IsNothing(Session(hdn_Session.Value)) Then Exit Sub
        Dim _filtros As Dictionary(Of String, String) = CType(Session(hdn_Session.Value), Dictionary(Of String, String))


        For Each _filtro As String In _filtros.Keys

            Try

                Me.ddlAgregar.SelectedValue = _filtro
                Me.ddlAgregar_SelectedIndexChanged(Nothing, Nothing)

            Catch ex As Exception
                Console.Write("No está")
            End Try

        Next


        For Each _control As Control In PlaceHolder1.Controls
            Select Case _control.TemplateControl.ToString

                Case SessionID & "_txtusercontrol2_ascx"
                    Dim ucTextBox As txtUserControl2 = TryCast(_control, txtUserControl2)
                    If _filtros.ContainsKey(ucTextBox.labelText) Then
                        ucTextBox.Text1 = _filtros(ucTextBox.labelText).Split(CChar("|"))(0)
                        ucTextBox.Text2 = _filtros(ucTextBox.labelText).Split(CChar("|"))(1)

                    End If

                Case SessionID & "_ddlusercontrol2_ascx"
                    Dim ucDropDownList As ddlUserControl2 = TryCast(_control, ddlUserControl2)
                    If _filtros.ContainsKey(ucDropDownList.labelText) Then
                        ucDropDownList.selectedValue = _filtros(ucDropDownList.labelText)
                    End If

                Case SessionID & "_ddlusercontrolr2_ascx"
                    Dim ucDropDownListR As ddlUserControlR2 = TryCast(_control, ddlUserControlR2)
                    If _filtros.ContainsKey(ucDropDownListR.labelText) Then
                        ucDropDownListR.selectedValue = _filtros(ucDropDownListR.labelText)
                    End If

                Case SessionID & "_cldusercontrol2_ascx"
                    Dim ucCalendar As cldUserControl2 = TryCast(_control, cldUserControl2)
                    If _filtros.ContainsKey(ucCalendar.labelText) Then
                        ucCalendar.FechaInicio = _filtros(ucCalendar.labelText).Split(CChar("|"))(0)
                        ucCalendar.fechaFin = _filtros(ucCalendar.labelText).Split(CChar("|"))(1)
                    End If

                Case SessionID & "_rbusercontrol2_ascx"
                    Dim ucRadioButton As rbUserControl2 = TryCast(_control, rbUserControl2)
                    If _filtros.ContainsKey(ucRadioButton.labelText) Then
                        ucRadioButton.selectedValue = _filtros(ucRadioButton.labelText)
                    End If

                Case SessionID & "_chkusercontrol2_ascx"
                    Dim ucCheckButton As chkUserControl2 = TryCast(_control, chkUserControl2)
                    If _filtros.ContainsKey(ucCheckButton.labelText) Then
                        'ucCheckButton.selectedValue = _filtros(ucCheckButton.labelText)
                    End If


            End Select


        Next

        For Each Control As TableCell In TableRow2.Cells
            Select Case Control.Controls(0).TemplateControl.ToString()
                Case SessionID & "_chkbuttomusercontrol2_ascx"
                    Dim ucCheckBox As chkButtomUserControl2 = TryCast(Control.Controls(0), chkButtomUserControl2)
                    If _filtros.ContainsKey(ucCheckBox.labelText) Then
                        'ucCheckButton.selectedValue = _filtros(ucCheckButton.labelText)
                    End If
            End Select
        Next

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFiltrar.Click

        Dim lstErrores As New List(Of Entities.EtiquetaError)
        Dim fecha As DateTime = DateTime.Now
        Dim valor As Boolean = False
        'Validaciones 
        If PlaceHolder1.Controls.Count > 0 Then

            For Each cntrl In PlaceHolder1.Controls
                Select Case cntrl.TemplateControl.ToString()
                    Case SessionID & "_cldusercontrol2_ascx"
                        Dim ucCalendar As cldUserControl = TryCast(cntrl, cldUserControl)
                        'If ucCalendar.FechaInicio > fecha Then

                        '    Dim script As String = "<script type='text/javascript'> Validacion();</script>"
                        'Else


                        'End If

                        ucCalendar.FetchValues()
                        If Not ucCalendar.isCalendarSingle Then
                            If Not FechaValida(ucCalendar.fechaFin, New CultureInfo("es-Mx"), DateTimeStyles.None) Then
                                Dim errores As New Entities.EtiquetaError(46)
                                errores.Descripcion += " fecha fin"
                                lstErrores.Add(errores)
                            End If
                        End If

                        If Not FechaValida(ucCalendar.FechaInicio, New CultureInfo("es-Mx"), DateTimeStyles.None) Then
                            Dim errores As New Entities.EtiquetaError(46)
                            errores.Descripcion += " fecha fin"
                            lstErrores.Add(errores)
                        End If

                    Case SessionID & "_txtusercontrol2_ascx"
                        Dim ucTextBox As txtUserControl = TryCast(cntrl, txtUserControl)
                        ucTextBox.FetchValues()
                        If Not String.IsNullOrWhiteSpace(ucTextBox.Text1) Then
                            If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                If Not EsEntero(ucTextBox.Text1) Then
                                    Dim errores As New Entities.EtiquetaError(47)
                                    errores.Descripcion = errores.Descripcion.Replace("{0}", ucTextBox.labelText)
                                    lstErrores.Add(errores)
                                End If
                            End If
                        Else
                            Dim errores As New Entities.EtiquetaError(48)
                            errores.Descripcion += " " + ucTextBox.labelText
                            lstErrores.Add(errores)
                        End If

                    Case SessionID & "_ddlusercontrol2_ascx"
                        Dim ucDDL As ddlUserControl = TryCast(cntrl, ddlUserControl)
                        ucDDL.FetchValues()
                        If ucDDL.selectedValue = "-1" Or ucDDL.selectedValue = Nothing Then
                            If ucDDL.DataValueType <> DataValueType.BoolType Then
                                Dim errores As New Entities.EtiquetaError(49)
                                errores.Descripcion += " " + ucDDL.labelText
                                lstErrores.Add(errores)
                            End If
                        End If

                    Case SessionID & "_ddlusercontrolr2_ascx"
                        Dim ucDDL As ddlUserControlR = TryCast(cntrl, ddlUserControlR)
                        ucDDL.FetchValues()
                        If ucDDL.selectedValue = "-1" Or ucDDL.selectedValue = Nothing Then
                            If ucDDL.DataValueType <> DataValueType.RangeType Then
                                Dim errores As New Entities.EtiquetaError(49)
                                errores.Descripcion += " " + ucDDL.labelText
                                lstErrores.Add(errores)
                            End If
                        End If
                End Select
            Next
        End If

        If lstErrores.Count = 0 Then
            RaiseEvent Filtrar(sender, e)
        Else
            Mensaje = ""
            Dim primero As Boolean = True
            For Each er As Entities.EtiquetaError In lstErrores
                If primero Then
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & er.Imagen.Ruta
                    primero = False
                End If
                Mensaje += er.Descripcion + "<br />"
            Next

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Filtro incorrecto", "MuestraVerificacionFiltro();", True)
        End If

    End Sub

    ''' <summary>
    ''' Valida que una cadena sea Entera
    ''' </summary>
    ''' <param name="cadena"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EsEntero(ByVal cadena As String) As Boolean
        Dim valido As Boolean = False
        Dim itmp As Integer

        valido = Integer.TryParse(cadena, itmp)

        Return valido
    End Function

    ''' <summary>
    ''' Si existe el filtro de Solo Míos, debe existir siempre un filtro ademas de el
    ''' por lo que si solo queda uno se oculta su boton de eliminacion
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeshabilitaEliminacionSoloMios()
        If TableRow2.Cells.Count > 0 Then

            If PlaceHolder1.Controls.Count = 1 Then

                Dim tmpControl As Control = PlaceHolder1.Controls(0)

                Select Case tmpControl.TemplateControl.ToString()
                    Case SessionID & "_cldusercontrol2_ascx"
                        Dim ucCalendar As cldUserControl2 = TryCast(PlaceHolder1.Controls(0), cldUserControl2)
                        If ucCalendar.isFixed = False Then
                            ucCalendar.isFixed = True
                            CambiaCampoFixed = ucCalendar.ID.ToString()
                        End If
                    Case SessionID & "_rbusercontrol2_ascx"
                        Dim ucRadioButton As rbUserControl2 = TryCast(PlaceHolder1.Controls(0), rbUserControl2)
                        If ucRadioButton.isFixed = False Then
                            ucRadioButton.isFixed = True
                            CambiaCampoFixed = ucRadioButton.ID.ToString()
                        End If
                    Case SessionID & "_txtusercontrol2_ascx"
                        Dim ucTextBox As txtUserControl2 = TryCast(PlaceHolder1.Controls(0), txtUserControl2)
                        If ucTextBox.isFixed = False Then
                            ucTextBox.isFixed = True
                            CambiaCampoFixed = ucTextBox.ID.ToString()
                        End If
                    Case SessionID & "_ddlusercontrol2_ascx"
                        Dim ucDDL As ddlUserControl2 = TryCast(PlaceHolder1.Controls(0), ddlUserControl2)
                        If ucDDL.isFixed = False Then
                            ucDDL.isFixed = True
                            CambiaCampoFixed = ucDDL.ID.ToString()
                        End If
                    Case SessionID & "_ddlusercontrolr2_ascx"
                        Dim ucDDL As ddlUserControlR2 = TryCast(PlaceHolder1.Controls(0), ddlUserControlR2)
                        If ucDDL.isFixed = False Then
                            ucDDL.isFixed = True
                            CambiaCampoFixed = ucDDL.ID.ToString()
                        End If
                    Case SessionID & "_chkusercontrol2_ascx"
                        Dim ucCheckBox As chkUserControl2 = TryCast(PlaceHolder1.Controls(0), chkUserControl2)
                        If ucCheckBox.isFixed = False Then
                            ucCheckBox.isFixed = True
                            CambiaCampoFixed = ucCheckBox.ID.ToString()
                        End If
                End Select

            End If

        End If
    End Sub

    ''' <summary>
    ''' Si se agrega un control extra al obligatorio que acompaña al Filtro Solo Míos
    ''' Se muestra el boton "X" del los filtros
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HabilitaEliminacionSoloMios()
        If PlaceHolder1.Controls.Count > 0 Then
            Dim tmpControl As Control = PlaceHolder1.Controls(0)

            Select Case tmpControl.TemplateControl.ToString()
                Case SessionID & "_cldusercontrol2_ascx"
                    Dim ucCalendar As cldUserControl2 = TryCast(PlaceHolder1.Controls(0), cldUserControl2)
                    If CambiaCampoFixed = ucCalendar.ID.ToString() Then
                        ucCalendar.isFixed = False
                        CambiaCampoFixed = Nothing
                    End If
                Case SessionID & "_rbusercontrol2_ascx"
                    Dim ucRadioButton As rbUserControl2 = TryCast(PlaceHolder1.Controls(0), rbUserControl2)
                    If CambiaCampoFixed = ucRadioButton.ID.ToString() Then
                        ucRadioButton.isFixed = False
                        CambiaCampoFixed = Nothing
                    End If
                Case SessionID & "_txtusercontrol2_ascx"
                    Dim ucTextBox As txtUserControl2 = TryCast(PlaceHolder1.Controls(0), txtUserControl2)
                    If CambiaCampoFixed = ucTextBox.ID.ToString() Then
                        ucTextBox.isFixed = False
                        CambiaCampoFixed = Nothing
                    End If
                Case SessionID & "_ddlusercontrol2_ascx"
                    Dim ucDDL As ddlUserControl2 = TryCast(PlaceHolder1.Controls(0), ddlUserControl2)
                    If CambiaCampoFixed = ucDDL.ID.ToString() Then
                        ucDDL.isFixed = False
                        CambiaCampoFixed = Nothing
                    End If
                Case SessionID & "_ddlusercontrolr2_ascx"
                    Dim ucDDL As ddlUserControlR2 = TryCast(PlaceHolder1.Controls(0), ddlUserControlR2)
                    If CambiaCampoFixed = ucDDL.ID.ToString() Then
                        ucDDL.isFixed = False
                        CambiaCampoFixed = Nothing
                    End If
                Case SessionID & "_chkusercontrol2_ascx"
                    Dim ucCheckBox As chkUserControl2 = TryCast(PlaceHolder1.Controls(0), chkUserControl2)
                    If CambiaCampoFixed = ucCheckBox.ID.ToString() Then
                        ucCheckBox.isFixed = False
                        CambiaCampoFixed = Nothing
                    End If
            End Select
        End If
    End Sub

End Class

''' <summary>
''' Clase Auxiliar para guardar las opciones de configuración de cada filtro.
''' </summary>
''' <remarks></remarks>
Friend Class CriterioSeleccion2

    Property DataValueType As ucFiltro.DataValueType

    Private _Nombre As String = ""
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

    Private _Source As Object = Nothing
    Public Property Source() As Object
        Get
            Return _Source
        End Get
        Set(ByVal value As Object)
            _Source = value

        End Set
    End Property
    Private _Tipo As ucFiltro2.AcceptedControls
    Public Property Tipo As ucFiltro2.AcceptedControls
        Get
            Return _Tipo
        End Get
        Set(ByVal value As ucFiltro2.AcceptedControls)
            _Tipo = value
        End Set
    End Property

    Private _DataTextField As String
    Public Property DataTextField() As String
        Get
            Return _DataTextField
        End Get
        Set(ByVal value As String)
            _DataTextField = value
        End Set
    End Property

    Private _DataValueField As String
    Public Property DataValueField() As String
        Get
            Return _DataValueField
        End Get
        Set(ByVal value As String)
            _DataValueField = value
        End Set
    End Property
    Private _isCalendarSingle As Boolean
    Public Property isCalendarSingle() As Boolean
        Get
            Return _isCalendarSingle
        End Get
        Set(ByVal value As Boolean)
            _isCalendarSingle = value
        End Set
    End Property
    Private _isTextBoxLike As Boolean
    Public Property isTextBoxLike As Boolean
        Get
            Return _isTextBoxLike
        End Get
        Set(ByVal value As Boolean)
            _isTextBoxLike = value
        End Set
    End Property
    Private _isTextBoxRange As Boolean
    Public Property IsTextBoxRange() As Boolean
        Get
            Return _isTextBoxRange
        End Get
        Set(ByVal value As Boolean)
            _isTextBoxRange = value
        End Set
    End Property

    Private _isDdlRange As Boolean
    Public Property IsDdlRange() As Boolean
        Get
            Return _isDdlRange
        End Get
        Set(ByVal value As Boolean)
            _isDdlRange = value
        End Set
    End Property

    Private _isFixed As Boolean
    Public Property IsFixed() As Boolean
        Get
            Return _isFixed
        End Get
        Set(ByVal value As Boolean)
            _isFixed = value
        End Set
    End Property
    Private _initValue As Object
    Public Property InitValue() As Object
        Get
            Return _initValue
        End Get
        Set(ByVal value As Object)
            _initValue = value
        End Set
    End Property

    Private _maxLength As Integer
    Public Property MaxLength() As Integer
        Get
            Return _maxLength
        End Get
        Set(ByVal value As Integer)
            _maxLength = value
        End Set
    End Property


End Class