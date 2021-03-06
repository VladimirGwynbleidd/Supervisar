Imports System.Globalization

<Themeable(True)>
Public Class ucFiltroOfi
    Inherits System.Web.UI.UserControl

    ' UC = User Control

    ''' <summary>
    ''' Enumeración para los controls aceptados por el UC
    ''' </summary>
    ''' <remarks></remarks>
    <Flags()> Public Enum AcceptedControls As Integer
        TextBox = 0
        DropDownList = 1
        Calendar = 2
        RadioButton = 3
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
    End Enum

    ''' <summary>
    ''' Variable Dummy para implementación futura
    ''' </summary>
    ''' <remarks></remarks>
    Dim controlID As String = "ucFiltro"

    ''' <summary>
    ''' Variable para Redibujar controles
    ''' </summary>
    ''' <remarks></remarks>
    Shared createAgain As Boolean = False

    ''' <summary>
    ''' Instancia de clase auxiliar para guardar las opciones de inicio por cada filtro
    ''' </summary>
    ''' <remarks></remarks>
    Shared criteriosSeleccion As New List(Of CriterioSeleccion)

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

    Private _ucTemplateControlPath As String = "ASP.usercontrols_filtro"
    ''' <summary>
    ''' Propiedad que guarda el prefijo del nombre de los controles en tiempo de ejecución
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ucTemplateControlPath() As String
        Get
            Return _ucTemplateControlPath
        End Get
        Set(ByVal value As String)
            _ucTemplateControlPath = value
        End Set
    End Property

    Private _ucPath As String = "~/UserControls/filtro/"
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
    Public Sub AddFilter(ByVal Nombre As String, _
                        ByVal TipoControl As AcceptedControls, _
                        Optional ByVal FuenteDatos As Object = Nothing, _
                        Optional ByVal DataTextField As String = "", _
                        Optional ByVal DataValueField As String = "", _
                        Optional ByVal DataValueType As DataValueType = DataValueType.IntegerType,
                        Optional ByVal isCalendarSingle As Boolean = False, _
                        Optional ByVal isTextBoxLike As Boolean = False,
                        Optional ByVal isTextBoxRange As Boolean = False,
                        Optional ByVal isDefault As Boolean = False,
                        Optional ByVal isFixed As Boolean = False,
                        Optional ByVal initValue As Object = Nothing)

        '-------------------------------------------------
        'Checar si elemento ya existe en el dropdownlist
        '-------------------------------------------------
        If ddlAgregar.Items.Count > 1 Then
            Dim li As ListItem = ddlAgregar.Items.FindByText(Nombre)
            If li IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim cs As New CriterioSeleccion
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
                Case AcceptedControls.TextBox
                    CreateTextbox(cs.Nombre, cs.DataValueField, cs.DataValueType, cs.isTextBoxLike, cs.IsTextBoxRange, isFixed)
                Case AcceptedControls.DropDownList
                    CreateDropDownList(cs.Nombre, cs.Source, cs.DataTextField, cs.DataValueField, cs.DataValueType, isFixed, initValue.ToString)
                Case AcceptedControls.Calendar
                    CreateCalendar(cs.Nombre, cs.DataValueField, isCalendarSingle, isDefault, isFixed, initValue)
                Case AcceptedControls.RadioButton
                    CreateRadioButton(cs.Nombre, cs.Source, cs.DataValueField, cs.DataTextField, cs.DataValueType, isFixed)
            End Select
        Else
            _ddlList.Add(Nombre)
        End If

    End Sub

    Public Sub LoadDDL()
        For Each item As String In _ddlList
            ddlAgregar.Items.Add(item)
        Next
        SortDDL(ddlAgregar)
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
        Dim ucDropDownList As ddlUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, ddlUserControl)
        removeControl(ucDropDownList)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un TextBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucTextBox_onPostClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucTextBox As txtUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, txtUserControl)
        removeControl(ucTextBox)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un Calendar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucCalendar_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucCalendar As cldUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, cldUserControl)
        removeControl(ucCalendar)
    End Sub

    ''' <summary>
    ''' Evento asociado al click del botón eliminar de un RadioButton
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ucRadioButton_onDeleteClk(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ucRadioButton As rbUserControl = DirectCast(DirectCast(sender, System.Web.UI.WebControls.Button).Parent, rbUserControl)
        removeControl(ucRadioButton)
    End Sub


    ''' <summary>
    ''' Método para eliminar control del ucFiltro (control principal) y de la sesión. Regresa el control al DropDown ddlAgregar
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub removeControl(ByVal obj As Object)
        PlaceHolder1.Controls.Remove(obj)
        Session.Remove(obj.SessionID)
        ddlAgregar.Items.Add(obj.labelText)
        SortDDL(ddlAgregar)
    End Sub


    ''' <summary>
    ''' Crea un nuevo UC tipo TextBox
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="isTextBoxLike"></param>
    ''' <remarks></remarks>
    Private Sub CreateTextbox( _
                             ByVal name As String, _
                             ByVal DataValueField As String, _
                             ByVal DataValueType As DataValueType, _
                             ByVal isTextBoxLike As Boolean,
                             ByVal isTextBoxRange As Boolean,
                             ByVal isFixed As Boolean)
        'Crea instancia del control
        Dim ucTextBox As txtUserControl = LoadControl(ucPath & "txtUserControl.ascx")

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
    Private Sub CreateDropDownList( _
                                  ByVal name As String, _
                                  ByVal source As Object, _
                                  ByVal DataTextField As String, _
                                  ByVal DataValueField As String, _
                                  ByVal DataValueType As DataValueType,
                                  ByVal isFixed As Boolean,
                                  ByVal initValue As String)

        'Crea instancia del control
        Dim ucDropDownList As ddlUserControl = LoadControl(ucPath & "ddlUserControl.ascx")

        'Establece las propiedades
        ucDropDownList.labelText = name
        ucDropDownList.source = source
        ucDropDownList.DataValueField = DataValueField
        ucDropDownList.DataTextField = DataTextField
        ucDropDownList.Bind()
        ucDropDownList.SessionID = ucDropDownList.TemplateControl.ToString & consSessionID.ToString
        ucDropDownList.ID = ucDropDownList.SessionID
        ucDropDownList.DataValueType = DataValueType
        ucDropDownList.isFixed = isFixed
        ucDropDownList.selectedValue = initValue

        If Not initValue Is Nothing AndAlso initValue IsNot String.Empty Then
            ucDropDownList.selectedValue = initValue
        End If

        'Añade el control al PlaceHolder y a la sesión.
        addControl(ucDropDownList, ucDropDownList.SessionID)
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
        Dim ucCalendar As cldUserControl = LoadControl(ucPath & "cldUserControl.ascx")

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
    Private Sub CreateRadioButton( _
                                 ByVal Nombre As String, _
                                 ByVal Source As Object, _
                                 ByVal DataValueField As String, _
                                 ByVal DataTextField As String, _
                                 ByVal DataValueType As DataValueType,
                                 ByVal isFixed As Boolean)

        'Crea instancia del control
        Dim ucRadioButton As rbUserControl = LoadControl(ucPath & "rbUserControl.ascx")

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
                    Throw New SystemException("Rango de fechas no válido.")

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
    Public Function getFilterSelection()

        Dim SelValues As New List(Of String)

        Try
            If PlaceHolder1 IsNot Nothing Then
                For Each Control As Control In PlaceHolder1.Controls
                    Select Case Control.TemplateControl.ToString
                        Case ucTemplateControlPath & "_txtusercontrol_ascx"
                            Dim ucTextBox As txtUserControl = TryCast(Control, txtUserControl)
                            ucTextBox.FetchValues()
                            Dim currentSelValue As String = String.Empty

                            If ucTextBox.Text1 IsNot String.Empty Then
                                If ucTextBox.isTextBoxLike Then
                                    currentSelValue = ucTextBox.DataValueField & " LIKE '%" & ucTextBox.Text1.Replace("'", "''") & "%'"
                                Else
                                    If ucTextBox.DataValueType = DataValueType.IntegerType Then
                                        currentSelValue = ucTextBox.DataValueField & "=" & ucTextBox.Text1 & ""
                                    Else
                                        currentSelValue = ucTextBox.DataValueField & "='" & ucTextBox.Text1.Replace("'", "''") & "'"
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

                            End If

                        Case ucTemplateControlPath & "_ddlusercontrol_ascx"
                            Dim ucDropDownList As ddlUserControl = TryCast(Control, ddlUserControl)
                            ucDropDownList.FetchValues()
                            If ucDropDownList.DataValueType = DataValueType.IntegerType Then
                                SelValues.Add(ucDropDownList.DataValueField & "=" & ucDropDownList.selectedValue)
                            Else
                                SelValues.Add(ucDropDownList.DataValueField & "='" & ucDropDownList.selectedValue & "'")
                            End If
                        Case ucTemplateControlPath & "_cldusercontrol_ascx"
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
                                    End If

                                End If

                            Else

                                If ucCalendar.FechaInicio.Length > 0 AndAlso ucCalendar.fechaFin.Length > 0 Then
                                    If VerificaFechas(ucCalendar.FechaInicio, ucCalendar.fechaFin, culture, styles) Then
                                        SelValues.Add(
                                                        ucCalendar.DataValueField & " BETWEEN '" & _
                                                        Convert.ToDateTime(ucCalendar.FechaInicio).ToString("yyyy/MM/dd") & _
                                                        "' AND '" & _
                                                        Convert.ToDateTime(ucCalendar.fechaFin).ToString("yyyy/MM/dd") & "'")
                                    End If
                                End If
                            End If
                        Case ucTemplateControlPath & "_rbusercontrol_ascx"
                            Dim ucRadioButton As rbUserControl = TryCast(Control, rbUserControl)
                            ucRadioButton.FetchValues()
                            If ucRadioButton.selectedValue IsNot String.Empty Then
                                If ucRadioButton.DataValueType = DataValueType.IntegerType Then
                                    SelValues.Add(ucRadioButton.DataValueField & "=" & ucRadioButton.selectedValue)
                                Else
                                    SelValues.Add(ucRadioButton.DataValueField & "='" & ucRadioButton.selectedValue & "'")
                                End If
                            End If
                    End Select
                Next
            End If
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
                    For i As Integer = 0 To Session.Count - 1
                        If Session(i) IsNot Nothing Then
                            Select Case Session(i).ToString()
                                Case ucTemplateControlPath & "_txtusercontrol_ascx"
                                    Dim ucTextBox As txtUserControl = TryCast(LoadControl(ucPath & "txtUserControl.ascx"), txtUserControl)

                                    ucTextBox.ID = DirectCast(Session(i), txtUserControl).ID
                                    ucTextBox.SessionID = DirectCast(Session(i), txtUserControl).SessionID
                                    ucTextBox.labelText = DirectCast(Session(i), txtUserControl).labelText
                                    ucTextBox.isTextBoxLike = DirectCast(Session(i), txtUserControl).isTextBoxLike
                                    ucTextBox.isTextBoxRange = DirectCast(Session(i), txtUserControl).isTextBoxRange

                                    ucTextBox.DataValueField = DirectCast(Session(i), txtUserControl).DataValueField
                                    ucTextBox.isFixed = DirectCast(Session(i), txtUserControl).isFixed

                                    AddHandler ucTextBox.btnPostClk, AddressOf ucTextBox_onPostClk
                                    PlaceHolder1.Controls.Add(ucTextBox)
                                    Exit Select
                                Case ucTemplateControlPath & "_ddlusercontrol_ascx"
                                    Dim ucDropDownList As ddlUserControl = TryCast(LoadControl(ucPath & "ddlUserControl.ascx"), ddlUserControl)

                                    ucDropDownList.ID = DirectCast(Session(i), ddlUserControl).ID
                                    ucDropDownList.SessionID = DirectCast(Session(i), ddlUserControl).SessionID
                                    ucDropDownList.source = DirectCast(Session(i), ddlUserControl).source
                                    ucDropDownList.DataTextField = DirectCast(Session(i), ddlUserControl).DataTextField
                                    ucDropDownList.DataValueField = DirectCast(Session(i), ddlUserControl).DataValueField
                                    ucDropDownList.Bind()
                                    ucDropDownList.labelText = DirectCast(Session(i), ddlUserControl).labelText
                                    ucDropDownList.DataValueType = DirectCast(Session(i), ddlUserControl).DataValueType
                                    ucDropDownList.isFixed = DirectCast(Session(i), ddlUserControl).isFixed
                                    ucDropDownList.selectedValue = DirectCast(Session(i), ddlUserControl).selectedValue

                                    AddHandler ucDropDownList.btnPostClk, AddressOf ucDropDownList_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucDropDownList)
                                    Exit Select
                                Case ucTemplateControlPath & "_cldusercontrol_ascx"
                                    Dim ucCalendar As cldUserControl = TryCast(LoadControl(ucPath & "cldUserControl.ascx"), cldUserControl)

                                    ucCalendar.ID = DirectCast(Session(i), cldUserControl).ID
                                    ucCalendar.SessionID = DirectCast(Session(i), cldUserControl).SessionID
                                    ucCalendar.fechaFin = DirectCast(Session(i), cldUserControl).fechaFin
                                    ucCalendar.FechaInicio = DirectCast(Session(i), cldUserControl).FechaInicio

                                    ucCalendar.labelText = DirectCast(Session(i), cldUserControl).labelText
                                    ucCalendar.isCalendarSingle = DirectCast(Session(i), cldUserControl).isCalendarSingle
                                    ucCalendar.DataValueField = DirectCast(Session(i), cldUserControl).DataValueField
                                    ucCalendar.isFixed = DirectCast(Session(i), cldUserControl).isFixed

                                    AddHandler ucCalendar.btnPostClk, AddressOf ucCalendar_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucCalendar)
                                    Exit Select
                                Case ucTemplateControlPath & "_rbusercontrol_ascx"
                                    Dim ucRadioButton As rbUserControl = TryCast(LoadControl(ucPath & "rbUserControl.ascx"), rbUserControl)

                                    ucRadioButton.ID = DirectCast(Session(i), rbUserControl).ID
                                    ucRadioButton.SessionID = DirectCast(Session(i), rbUserControl).SessionID
                                    ucRadioButton.source = DirectCast(Session(i), rbUserControl).source
                                    ucRadioButton.DataValueField = DirectCast(Session(i), rbUserControl).DataValueField
                                    ucRadioButton.DataTextField = DirectCast(Session(i), rbUserControl).DataTextField
                                    ucRadioButton.bind()
                                    ucRadioButton.labelText = DirectCast(Session(i), rbUserControl).labelText
                                    ucRadioButton.DataValueType = DirectCast(Session(i), rbUserControl).DataValueType
                                    ucRadioButton.isFixed = DirectCast(Session(i), rbUserControl).isFixed

                                    AddHandler ucRadioButton.btnPostClk, AddressOf ucRadioButton_onDeleteClk
                                    PlaceHolder1.Controls.Add(ucRadioButton)
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
        Dim result As CriterioSeleccion = criteriosSeleccion.Find(AddressOf FindNombre)
        If result IsNot Nothing Then
            Select Case result.Tipo
                Case AcceptedControls.TextBox
                    CreateTextbox(result.Nombre, result.DataValueField, result.DataValueType, result.isTextBoxLike, result.IsTextBoxRange, result.IsFixed)
                Case AcceptedControls.DropDownList
                    CreateDropDownList(result.Nombre, result.Source, result.DataTextField, result.DataValueField, result.DataValueType, result.IsFixed, result.InitValue)
                Case AcceptedControls.Calendar
                    CreateCalendar(result.Nombre, result.DataValueField, result.isCalendarSingle, , result.IsFixed)
                Case AcceptedControls.RadioButton
                    CreateRadioButton(result.Nombre, result.Source, result.DataValueField, result.DataTextField, result.DataValueType, result.IsFixed)
            End Select
            ddlAgregar.Items.Remove(ddlAgregar.SelectedItem.Text)
            SortDDL(ddlAgregar)
        End If

    End Sub

    ''' <summary>
    ''' Funcion auxiliar para recorrer en bucle la Lista criteriosSeleccion
    ''' </summary>
    ''' <param name="cs"></param>
    ''' <returns>boolean</returns>
    ''' <remarks></remarks>
    Private Function FindNombre(ByVal cs As CriterioSeleccion) As Boolean
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
                    Case ucTemplateControlPath & "_txtusercontrol_ascx"
                        Dim ucTextBox As txtUserControl = TryCast(Control, txtUserControl)
                        Session.Remove(ucTextBox.SessionID)

                    Case ucTemplateControlPath & "_ddlusercontrol_ascx"
                        Dim ucDropDownList As ddlUserControl = TryCast(Control, ddlUserControl)
                        Session.Remove(ucDropDownList.SessionID)

                    Case ucTemplateControlPath & "_cldusercontrol_ascx"
                        Dim ucCalendar As cldUserControl = TryCast(Control, cldUserControl)
                        Session.Remove(ucCalendar.SessionID)

                    Case ucTemplateControlPath & "_rbusercontrol_ascx"
                        Dim ucRadioButton As rbUserControl = TryCast(Control, rbUserControl)
                        Session.Remove(ucRadioButton.SessionID)
                End Select
            Next
        End If

        PlaceHolder1.Controls.Clear()
        consSessionID = 0

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

End Class



''' <summary>
''' Clase Auxiliar para guardar las opciones de configuración de cada filtro.
''' </summary>
''' <remarks></remarks>
Friend Class CriterioSeleccionOfi

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
    Private _Tipo As ucFiltro.AcceptedControls
    Public Property Tipo As ucFiltro.AcceptedControls
        Get
            Return _Tipo
        End Get
        Set(ByVal value As ucFiltro.AcceptedControls)
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

End Class