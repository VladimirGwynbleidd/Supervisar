'- Fecha de creación: 16/07/2013
'- Nombre del Responsable: Rafael Rodríguez Sánchez
'- Empresa: Softtek
'- GridView personalizado que incluye funcion de scroll, columnas fijas, ordenamiento, seleccion simple y multiple
'V2.
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page


<DefaultProperty("Text"), ToolboxData("<{0}:CustomGridView runat=server></{0}:CustomGridView>")>
Public Class CustomGridView
    Inherits GridView

    ''' <summary>
    ''' Los tipos de seleccion que pueden existir en el CustomGridView
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoSeleccion
        No = 0
        Simple = 1
        Multiple = 2
        SimpleCliente = 3
        MultipleCliente = 4
    End Enum

    Private Property activaDataBind As Boolean = True
    ''' <summary>
    ''' Establece si se decea que se realice el DataSource y DataBind en cada PostBack
    ''' Se utiliza cuando se usa con los filtros dinamicos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DataBindEnPostBack As Boolean
        Get
            Return activaDataBind
        End Get
        Set(ByVal value As Boolean)
            activaDataBind = value
        End Set
    End Property

    ''' <summary>
    ''' Determina si solo existe un CustomGridView en la pantalla
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UnicoEnPantalla As Boolean = True

    ''' <summary>
    ''' Determina si se mostrara el Mensaje de ToolTip en las filas del GridView
    ''' Por defecto esta habilitado y se muestra si se coloca texto en la propiedad ToolTip
    ''' Si se coloca texto en la propiedad ToolTip y esta esta en false, no se mostrara el ToolTip
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ToolTipHabilitado As Boolean = True

    ''' <summary>
    ''' Indica el valor por defecto del Hidden Field a utilizar para seleccion simple
    ''' Se debe utilizar cuando hay mas de un customgrid en pantalla con seleccion simple
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HiddenFieldSeleccionSimple As String = "hfSelectedValue_" & Me.ID


    Private ToolTipMensaje As String

    Public Overrides Property ToolTip As String
        Get
            'Return MyBase.ToolTip
            Return ToolTipMensaje
        End Get
        Set(ByVal value As String)
            'MyBase.ToolTip = value
            ToolTipMensaje = value
        End Set
    End Property

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)

        If UnicoEnPantalla Then
            Dim cs As ClientScriptManager = Me.Page.ClientScript
            cs.RegisterStartupScript(GetType(CustomGridView), "script_" + Me.ID.ToString, GeneraScript())
        Else
            GeneraMultiScript()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            If DataBindEnPostBack Then
                DataSource = DataSourceSession
                DataBind()
            End If
            If HabilitaSeleccion = TipoSeleccion.MultipleCliente Or HabilitaSeleccion = TipoSeleccion.SimpleCliente Then
                CargaSeleccion()
            End If
        End If
    End Sub

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)>
    Property Text() As String
        Get
            Dim s As String = CStr(ViewState("Text"))
            If s Is Nothing Then
                Return "[" & Me.ID & "]"
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("Text") = Value
        End Set
    End Property

    Protected Overrides Sub OnRowDataBound(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        MyBase.OnRowDataBound(e)

        If e.Row.RowType = DataControlRowType.DataRow Then

            If HabilitaSeleccion = TipoSeleccion.Simple Or HabilitaSeleccion = TipoSeleccion.Multiple Then
                If e.Row.Cells(0).Controls.Count > 1 Then
                    Dim chkTmp As CheckBox = CType(e.Row.Cells(0).Controls.Item(1), CheckBox)

                    ''Cambio de autoPostback a Flase
                    chkTmp.AutoPostBack = False

                    AddHandler chkTmp.CheckedChanged, AddressOf chkSelect_CheckedChanged
                    If HabilitaSeleccion = TipoSeleccion.Simple Then
                        Dim script As String = "uncheckOthers(this);"
                        chkTmp.Attributes.Add("onclick", script)
                    End If
                End If
            End If

            If HabilitaSeleccion = TipoSeleccion.SimpleCliente Then
                If e.Row.Cells(0).Controls.Count > 1 Then
                    Dim chkTmp As CheckBox = CType(e.Row.Cells(0).Controls.Item(1), CheckBox)
                    chkTmp.Attributes.Add("onclick", "ControlaSeleccionSimple(this); " +
                                          "if(this.checked){ " + HiddenFieldSeleccionSimple + ".value = " + Me.Rows.Count.ToString + ";}else{" + HiddenFieldSeleccionSimple + ".value = -1}")

                    chkTmp.Attributes.Add("class", "controlSeleccion")
                End If
            End If

            If HabilitaSeleccion = TipoSeleccion.MultipleCliente Then
                If e.Row.Cells(0).Controls.Count > 1 Then
                    Dim chkTmp As CheckBox = CType(e.Row.Cells(0).Controls.Item(1), CheckBox)

                    chkTmp.Attributes.Add("onclick", "ControlaMultiSeleccion(this);")
                End If
            End If

            If ColumnasCongeladas > 0 Then

                For index = 0 To ColumnasCongeladas - 1
                    Dim td As TableCell = e.Row.Cells(index)
                    td.CssClass = "CongelaColumna"
                Next

            End If

        End If

    End Sub

    ''' <summary>
    ''' Devuelve o asigna el tipo de seleccion del CustomGridView
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HabilitaSeleccion As TipoSeleccion

    ''' <summary>
    ''' Devuelve o asigna el índice del registro seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Property SelectedIndex As Integer
        Get
            If HabilitaSeleccion = TipoSeleccion.No Or HabilitaSeleccion = Nothing Then
                Return MyBase.SelectedIndex
            ElseIf HabilitaSeleccion = TipoSeleccion.Simple Or HabilitaSeleccion = TipoSeleccion.SimpleCliente Then
                Dim index As Integer = -1
                Dim icont As Integer = 0
                For Each row As GridViewRow In Me.Rows
                    Dim chkTmp As CheckBox = CType(row.Cells(0).Controls.Item(1), CheckBox)
                    If chkTmp.Checked Then
                        index = icont
                    End If
                    icont += 1
                Next
                Return index
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Integer)
            MyBase.SelectedIndex = value
        End Set
    End Property

    Private multiIndices As List(Of Integer)
    ''' <summary>
    ''' Devuelve una lista de los índices seleccionados, si no existe ninguna seleccionado regresa la lista vacía
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MultiSelectedIndex As List(Of Integer)
        Get
            Dim lstIndex As New List(Of Integer)
            If HabilitaSeleccion <> TipoSeleccion.No Or HabilitaSeleccion <> Nothing Then
                Dim icont As Integer = 0
                For Each row As GridViewRow In Me.Rows
                    Dim chkTmp As CheckBox = CType(row.Cells(0).Controls.Item(1), CheckBox)
                    If chkTmp.Checked Then
                        lstIndex.Add(icont)
                    End If
                    icont += 1
                Next
            End If
            Return lstIndex
        End Get
    End Property

    ''' <summary>
    ''' Devuleve o asigna si es que el CustomGridView tendra Scroll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HabilitaScroll As Boolean

    ''' <summary>
    ''' Devuelve o asigna el numero de columnas congeladas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ColumnasCongeladas As Integer

    ''' <summary>
    ''' Devuelve o asigna el ancho cuando se habilita el scroll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WidthScroll As Integer

    ''' <summary>
    ''' Devuelve o asigna el alto cuando se habilita el Scroll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HeigthScroll As Integer

    ''' <summary>
    ''' Identificador del nombre de sesion donde se guarda la informacion del Databind
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SessionId As String
        Get
            Return "GridViewData" + Me.ClientID
        End Get
    End Property

    ''' <summary>
    ''' Asigna el conjunto de datos al gridview y lo almacena en session en el campo de la propiedad SessionId
    ''' El conjunto de datos devuelto lo obtiene de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DataSourceSession As Object
        Get
            Return ViewState(SessionId)
        End Get
        Set(ByVal value As Object)
            ViewState(SessionId) = value
        End Set
    End Property

    ''' <summary>
    ''' Se Agrega la funcionalidad de guardar el valor asignado en DataSourceSession
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Property DataSource As Object
        Get
            Return MyBase.DataSource
        End Get
        Set(ByVal value As Object)
            MyBase.DataSource = value
            DataSourceSession = value
        End Set
    End Property

    ''' <summary>
    ''' Devuelve la fila seleccionada
    ''' Si se utiliza en multiselect solo regresa la primera
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property SelectedRow As System.Web.UI.WebControls.GridViewRow
        Get
            'Se coloca bloque try ya que es posible que no exista seleccion en el gridview
            Try
                For Each gvRow As GridViewRow In Me.Rows
                    Dim chk As CheckBox = CType(gvRow.Cells(0).Controls.Item(1), CheckBox)
                    If chk.Checked Then
                        Return gvRow
                    End If
                Next
            Catch ex As Exception

            End Try

            Return MyBase.SelectedRow
        End Get
    End Property

    ''' <summary>
    ''' Devuelve lista de filas seleccionadas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SelectedMultiRows As List(Of GridViewRow)
        Get
            Dim lstGvRows As New List(Of GridViewRow)
            'Se coloca bloque try ya que es posible que no exista seleccion en el gridview
            Try
                For Each gvRow As GridViewRow In Me.Rows
                    Dim chk As CheckBox = CType(gvRow.Cells(0).Controls.Item(1), CheckBox)
                    If chk.Checked Then
                        lstGvRows.Add(gvRow)
                    End If
                Next
            Catch ex As Exception

            End Try
            Return lstGvRows
        End Get
    End Property

    ''' <summary>
    ''' LLave para almacenar en sesion los scripts
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SessionScriptFunction As String
        Get
            Return "CustomGridViewScriptFunctions" & Me.ID
        End Get
    End Property

    ''' <summary>
    ''' LLave para almacenar en sesion la funcion FunctionReady
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SessionFunctionReady As String
        Get
            Return "CustomGridViewFunctionReady" & Me.ID
        End Get
    End Property

    ''' <summary>
    ''' LLave para almacenar en sesion la funcion PageLoad
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SessionFunctionPageLoad As String
        Get
            Return "CustomGridViewFunctionPageLoad" & Me.ID
        End Get
    End Property

    ''' <summary>
    ''' LLave para almacenar en sesion los scripts de seleccion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SessionScriptSeleccion As String
        Get
            Return "CustomGridViewScriptSeleccion" & Me.ID
        End Get
    End Property

    ''' <summary>
    ''' Ordena el datasource del CustomGridView y lo muestra
    ''' El datasource debe ser un datatable de lo contrario de debe crear uno personalizado
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub Ordenar(ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)

        If Me.DataSourceSession IsNot Nothing Then

            Dim sortExpression As String
            Dim sortDirection As String

            If ViewState("SortExpression") Is Nothing Then
                sortExpression = ""
            Else
                sortExpression = ViewState("SortExpression").ToString()
            End If

            If ViewState("SortDirection") Is Nothing Then
                sortDirection = "ASC"
            Else
                sortDirection = ViewState("SortDirection").ToString()
            End If

            If sortExpression = e.SortExpression Then
                ViewState("SortDirection") = IIf(sortDirection = "ASC", "DESC", "ASC")
            Else
                ViewState("SortExpression") = e.SortExpression
                ViewState("SortDirection") = "ASC"
            End If

            Dim dt As DataTable = CType(Me.DataSourceSession, DataTable)
            Dim dv As DataView = New DataView(dt)
            dv.Sort = e.SortExpression + " " + ViewState("SortDirection").ToString

            Me.DataSource = dv.ToTable
            Me.DataBind()

        End If
    End Sub

    ''' <summary>
    ''' Genera el Script inicial que tendra la pantalla
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GeneraScript() As String
        Dim script As String = "<script type=""text/javascript"">"
        Dim scriptFunctions As String = ""

        Dim functionJQuery = " $(function () { "


        Dim functionPageLoad As String = " function pageLoad(sender, args) { " +
            " if (args.get_isPartialLoad()) { "

        Dim functionReady As String = " $(document).ready(function () { "

        If HabilitaScroll Then
            functionReady += " gridviewScroll" & Me.ID & "(); "

            functionPageLoad += " gridviewScroll" & Me.ID & "(); "

            scriptFunctions += " function gridviewScroll" & Me.ID & "() { $('#" + Me.ClientID + "').gridviewScroll({ "

            If WidthScroll > 0 Then
                If WidthScroll < 100 Then
                    scriptFunctions += "width: (($(window).width()) * ." & WidthScroll.ToString() & "),"
                ElseIf WidthScroll = 100 Then
                    scriptFunctions += "width: (($(window).width()) * .1),"
                Else
                    scriptFunctions += "width:" + WidthScroll.ToString + ","
                End If
            Else
                scriptFunctions += "width: ($(window).width() - (($(window).width()) * .19)),"
            End If

            If HeigthScroll > 0 Then
                scriptFunctions += "height:" + HeigthScroll.ToString + ","
            Else
                scriptFunctions += "height: 200,"
            End If

            If ColumnasCongeladas > 0 Then
                scriptFunctions += "freezesize: " + ColumnasCongeladas.ToString() + ","
            End If

            scriptFunctions += "startVertical: $('#hfGridView1SV_" & Me.ID & "').val(), " +
                "startHorizontal: $('#hfGridView1SH_" & Me.ID & "').val(), " +
                "onScrollVertical: function (delta) {  $('#hfGridView1SV_" & Me.ID & "').val(delta); }, " +
                "onScrollHorizontal: function (delta) { $('#hfGridView1SH_" & Me.ID & "').val(delta); }, " +
                "arrowsize: 30, " +
                "varrowtopimg: ""/Imagenes/arrowvt.png"", varrowbottomimg: ""/Imagenes/arrowvb.png"", " +
                "harrowleftimg: ""/Imagenes/arrowhl.png"", harrowrightimg: ""/Imagenes/arrowhr.png"" " +
                "}); }"
        End If

        If HabilitaSeleccion = TipoSeleccion.Simple Then
            scriptFunctions += " function uncheckOthers(chk) {  " +
                "	var elm = document.getElementsByTagName('input');  " +
                "	for (var i = 0; i < elm.length; i++) {  " +
                "		if (elm.item(i).type == 'checkbox' && elm.item(i) != chk)  " +
                "			elm.item(i).checked = false;  " +
                "	}  " +
                "}  "
        End If

        If HabilitaSeleccion = TipoSeleccion.SimpleCliente Then
            scriptFunctions += " function ControlaSeleccionSimple(chk) {  " +
                "	if (chk.checked) {  " +
                "		$('.controlSeleccion input:checkbox').each(function (index) { " +
                "			if (this != chk) {  " +
                "				$(this).closest('tr').children('td').each(function (idx) {  " +
                "					$(this).removeClass('RowSeleccionado');  " +
                "				});  " +
                "				$(this).attr('checked', false); }  " +
                "		});  " +
                "		$(chk).closest('tr').children('td').each(function (index) {  " +
                "			$(this).addClass('RowSeleccionado');  " +
                "		})  " +
                "	} else {  " +
                "		$(chk).closest('tr').children('td').each(function (idx) {   " +
                "			$(this).removeClass('RowSeleccionado'); " +
                "		});  " +
                "	}  " +
                "}  "

        End If

        If HabilitaSeleccion = TipoSeleccion.MultipleCliente Then
            scriptFunctions += " function ControlaMultiSeleccion(chk) {  " +
                "	if (chk.checked) {  " +
                "		$(chk).closest('tr').children('td').each(function (index) { " +
                "			$(this).addClass('RowSeleccionado');  " +
                "		});  " +
                "	} else {  " +
                "		$(chk).closest('tr').children('td').each(function (index) {   " +
                "			$(this).removeClass('RowSeleccionado');   " +
                "		});  " +
                "	}  " +
                "} "
        End If

        functionPageLoad += " } } "
        functionJQuery += " }); "
        functionReady += " }); "

        script += functionJQuery + functionPageLoad + functionReady + scriptFunctions + "</script>"

        Return script
    End Function

    ''' <summary>
    ''' Genera los scripts cunado es mas de un CustomGridView
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GeneraMultiScript()

        Dim scriptFunctions As String = ""
        Dim functionPageLoad As String = ""
        Dim functionReady As String = ""
        Dim scriptSeleccion = ""

        If Not IsNothing(Me.Page.Session(SessionScriptFunction)) Then
            scriptFunctions = Me.Page.Session(SessionScriptFunction).ToString
        End If

        If Not IsNothing(Me.Page.Session(SessionFunctionReady)) Then
            functionReady = Me.Page.Session(SessionFunctionReady).ToString
        End If

        If Not IsNothing(Me.Page.Session(SessionFunctionPageLoad)) Then
            functionPageLoad = Me.Page.Session(SessionFunctionPageLoad)
        End If

        If Not IsNothing(Me.Page.Session(SessionScriptSeleccion)) Then
            scriptSeleccion = Me.Page.Session(SessionScriptSeleccion)
        End If

        If HabilitaScroll Then
            functionReady += " gridviewScroll" & Me.ID & "(); "

            functionPageLoad += " gridviewScroll" & Me.ID & "(); "

            scriptFunctions += "  $('#" + Me.ClientID + "').gridviewScroll({ "

            If WidthScroll > 0 Then
                If WidthScroll < 100 Then
                    scriptFunctions += "width: (($(window).width()) * ." & WidthScroll.ToString() & "),"
                ElseIf WidthScroll = 100 Then
                    scriptFunctions += "width: (($(window).width()) * .1),"
                Else
                    scriptFunctions += "width:" + WidthScroll.ToString + ","
                End If
            Else
                scriptFunctions += "width: ($(window).width() - (($(window).width()) * .19)),"
            End If

            If HeigthScroll > 0 Then
                scriptFunctions += "height:" + HeigthScroll.ToString + ","
            Else
                scriptFunctions += "height: 200,"
            End If

            If ColumnasCongeladas > 0 Then
                scriptFunctions += "freezesize: " + ColumnasCongeladas.ToString() + ","
            End If

            scriptFunctions += "startVertical: $('#hfGridView1SV_" & Me.ID & "').val(), " +
                "startHorizontal: $('#hfGridView1SH_" & Me.ID & "').val(), " +
                "onScrollVertical: function (delta) {  $('#hfGridView1SV_" & Me.ID & "').val(delta); }, " +
                "onScrollHorizontal: function (delta) { $('#hfGridView1SH_" & Me.ID & "').val(delta); }, " +
                "arrowsize: 30, " +
                "varrowtopimg: ""/Imagenes/arrowvt.png"", varrowbottomimg: ""/Imagenes/arrowvb.png"", " +
                "harrowleftimg: ""/Imagenes/arrowhl.png"", harrowrightimg: ""/Imagenes/arrowhr.png"" " +
                "}); "
        End If

        If HabilitaSeleccion = TipoSeleccion.Simple Then
            scriptSeleccion += " function uncheckOthers(chkRen) {  " +
                "	var elm = document.getElementsByTagName('input');  " +
                "	for (var i = 0; i < elm.length; i++) {  " +
                "		if (elm.item(i).type == 'checkbox' && elm.item(i) != chkRen){" +
                "		    if(elm.item(i).checked == true){ " +
                "			    elm.item(i).checked = false; return; " +
                "		    }" +
                "		}  " +
                "	}  " +
                "}  "
        End If

        If HabilitaSeleccion = TipoSeleccion.SimpleCliente Then
            scriptSeleccion += " function ControlaSeleccionSimple(chk) {  " +
                "	if (chk.checked) {  " +
                "		$('.controlSeleccion input:checkbox').each(function (index) { " +
                "			if (this != chk) {  " +
                "				$(this).closest('tr').children('td').each(function (idx) {  " +
                "					$(this).removeClass('RowSeleccionado');  " +
                "				});  " +
                "				$(this).attr('checked', false); }  " +
                "		});  " +
                "		$(chk).closest('tr').children('td').each(function (index) {  " +
                "			$(this).addClass('RowSeleccionado');  " +
                "		})  " +
                "	} else {  " +
                "		$(chk).closest('tr').children('td').each(function (idx) {   " +
                "			$(this).removeClass('RowSeleccionado'); " +
                "		});  " +
                "	}  " +
                "}  "

        End If

        If HabilitaSeleccion = TipoSeleccion.MultipleCliente Then
            scriptSeleccion += " function ControlaMultiSeleccion(chk) {  " +
                "	if (chk.checked) {  " +
                "		$(chk).closest('tr').children('td').each(function (index) { " +
                "			$(this).addClass('RowSeleccionado');  " +
                "		});  " +
                "	} else {  " +
                "		$(chk).closest('tr').children('td').each(function (index) {   " +
                "			$(this).removeClass('RowSeleccionado');   " +
                "		});  " +
                "	}  " +
                "} "
        End If

        Me.Page.Session(SessionScriptFunction) = scriptFunctions
        Me.Page.Session(SessionFunctionReady) = functionReady
        Me.Page.Session(SessionFunctionPageLoad) = functionPageLoad
        Me.Page.Session(SessionScriptSeleccion) = scriptSeleccion

    End Sub

    ''' <summary>
    ''' Une los multiples scripts de los diversos CustomGridViews en uno solo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ArmaMultiScript()
        Dim script As String = "<script type=""text/javascript"">"
        Dim scriptFunctions As String = " function gridviewScroll" & Me.ID & "() { "
        Dim functionJQuery = " $(function () { "
        Dim functionPageLoad As String = " function pageLoad(sender, args) { " +
            " if (args.get_isPartialLoad()) { "
        Dim scriptSeleccion As String = ""

        Dim functionReady As String = " $(document).ready(function () { "

        If Not IsNothing(Me.Page.Session(SessionScriptFunction)) Then
            scriptFunctions += Me.Page.Session(SessionScriptFunction).ToString
        End If

        If Not IsNothing(Me.Page.Session(SessionFunctionReady)) Then
            functionReady += " gridviewScroll" & Me.ID & "(); "
        End If

        If Not IsNothing(Me.Page.Session(SessionFunctionPageLoad)) Then
            functionPageLoad += " gridviewScroll" & Me.ID & "(); "
        End If

        If Not IsNothing(Me.Page.Session(SessionScriptSeleccion)) Then
            scriptSeleccion += Me.Page.Session(SessionScriptSeleccion)
        End If


        functionPageLoad += " } } "
        functionJQuery += " }); "
        functionReady += " }); "
        scriptFunctions += " }"

        If Page.IsPostBack Then
            script += functionJQuery + functionPageLoad + functionReady + scriptFunctions + scriptSeleccion + "</script>"
        Else
            script += functionJQuery + functionPageLoad + functionReady + scriptFunctions + scriptSeleccion + "</script>"
            'If Me.ID.Contains("gvConsultaDocs") Then
            '    script += functionJQuery + functionPageLoad + " gridviewScroll" & Me.ID & "(); " + scriptFunctions + scriptSeleccion + "</script>"
            'Else

            'End If
        End If

        Me.Page.Session(SessionFunctionPageLoad) = Nothing
        Me.Page.Session(SessionScriptFunction) = Nothing
        Me.Page.Session(SessionFunctionReady) = Nothing
        Me.Page.Session(SessionScriptSeleccion) = Nothing

        Me.Page.ClientScript.RegisterStartupScript(GetType(CustomGridView), "script", script)
    End Sub

    Public Function RetornaArmaMultiScript() As String
        GeneraMultiScript()

        Dim script As String = ""
        Dim scriptFunctions As String = " function gridviewScroll" & Me.ID & "() { "
        Dim functionJQuery = " $(function () { "
        Dim functionPageLoad As String = " function pageLoad(sender, args) { " +
            " if (args.get_isPartialLoad()) { "
        Dim scriptSeleccion As String = ""

        Dim functionReady As String = " $(document).ready(function () { "

        If Not IsNothing(Me.Page.Session(SessionScriptFunction)) Then
            scriptFunctions += Me.Page.Session(SessionScriptFunction).ToString
        End If

        If Not IsNothing(Me.Page.Session(SessionFunctionReady)) Then
            functionReady += " gridviewScroll" & Me.ID & "(); "
        End If

        If Not IsNothing(Me.Page.Session(SessionFunctionPageLoad)) Then
            functionPageLoad += " gridviewScroll" & Me.ID & "(); "
        End If

        If Not IsNothing(Me.Page.Session(SessionScriptSeleccion)) Then
            scriptSeleccion += Me.Page.Session(SessionScriptSeleccion)
        End If


        functionPageLoad += " } } "
        functionJQuery += " }); "
        functionReady += " }); "
        scriptFunctions += " }"

        script += functionJQuery + functionPageLoad + functionReady + scriptFunctions + scriptSeleccion

        Me.Page.Session(SessionFunctionPageLoad) = Nothing
        Me.Page.Session(SessionScriptFunction) = Nothing
        Me.Page.Session(SessionFunctionReady) = Nothing
        Me.Page.Session(SessionScriptSeleccion) = Nothing

        Return script
    End Function

    ''' <summary>
    ''' Coloca estilo a la fila seleccionada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub chkSelect_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.RowCommand
        Dim chkEvent As CheckBox = TryCast(sender, CheckBox)

        If Not IsNothing(chkEvent) Then
            Dim chkValor As Boolean = chkEvent.Checked

            If HabilitaSeleccion = TipoSeleccion.Simple Then
                For Each gvRow As GridViewRow In Me.Rows
                    Dim chk As CheckBox = CType(gvRow.Cells(0).Controls.Item(1), CheckBox)

                    If chkValor = True Then
                        chk.Checked = False
                    End If

                Next
                chkEvent.Checked = chkValor
            End If

            For Each gvRow As GridViewRow In Me.Rows
                Dim chk As CheckBox = CType(gvRow.Cells(0).Controls.Item(1), CheckBox)
                If chk.Checked Then
                    For index = 0 To gvRow.Cells.Count - 1
                        gvRow.Cells(index).CssClass = Me.Columns(index).ItemStyle.CssClass + " " + gvRow.Cells(index).CssClass + " RowSeleccionado"
                    Next
                Else
                    For index = 0 To gvRow.Cells.Count - 1
                        gvRow.Cells(index).CssClass = Me.Columns(index).ItemStyle.CssClass + " " + gvRow.Cells(index).CssClass.Replace("RowSeleccionado", "")
                    Next
                End If

            Next
        End If

    End Sub

    ''' <summary>
    ''' Realiza la carga los estilos de los registros seleccionados
    ''' Usar siempre que exista la propiedad HabilitaSeleccion en diferente de No
    ''' Se debe usar en cada pantalla cundo sea postback
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaSeleccion()

        Dim hfSelectedValue As HiddenField = Page.Master.FindControl("MainContent").FindControl(HiddenFieldSeleccionSimple)

        If Not IsNothing(hfSelectedValue) Then
            Dim setectedItem As Integer = Convert.ToInt32(hfSelectedValue.Value)

            If setectedItem > -1 Then
                Dim chk As CheckBox = CType(Me.Rows(setectedItem).Cells(0).Controls.Item(1), CheckBox)
                chk.Checked = True
            End If
        End If

        For Each gvRow As GridViewRow In Me.Rows
            Dim chk As CheckBox = CType(gvRow.Cells(0).Controls.Item(1), CheckBox)
            If chk.Checked Then
                If Me.ColumnasCongeladas > 0 AndAlso (HabilitaSeleccion = TipoSeleccion.MultipleCliente _
                                                      OrElse HabilitaSeleccion = TipoSeleccion.SimpleCliente) Then
                    For index = 0 To ColumnasCongeladas - 1
                        gvRow.Cells(index).CssClass = Me.Columns(index).ItemStyle.CssClass + " " + gvRow.Cells(index).CssClass + " RowSeleccionado"
                    Next
                Else
                    For index = 0 To gvRow.Cells.Count - 1
                        gvRow.Cells(index).CssClass = Me.Columns(index).ItemStyle.CssClass + " " + gvRow.Cells(index).CssClass + " RowSeleccionado"
                    Next
                End If
            Else
                For index = 0 To gvRow.Cells.Count - 1
                    gvRow.Cells(index).CssClass = Me.Columns(index).ItemStyle.CssClass + " " + gvRow.Cells(index).CssClass.Replace("RowSeleccionado", "")
                Next
            End If
        Next

    End Sub

    ''' <summary>
    ''' Asigna Imagen al encabezado dependiendo del ordenamiento
    ''' Asigna el ToolTip a las filas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowCreated
        If Me.AllowSorting = True Then
            If e.Row.RowType = DataControlRowType.Header Then

                If Not IsNothing(ViewState("SortExpression")) OrElse Not IsNothing(ViewState("SortDirection")) Then
                    Dim sortExpression As String = ViewState("SortExpression").ToString()
                    Dim sortDirection As String = ViewState("SortDirection").ToString()

                    For Each celda As TableCell In e.Row.Cells
                        If Not celda.HasControls() Then
                            Continue For
                        End If

                        Dim lbSort As LinkButton = TryCast(celda.Controls(0), LinkButton)

                        If lbSort Is Nothing Then
                            Continue For
                        End If

                        If lbSort.CommandArgument = sortExpression Then
                            Dim imageSort As New Image()
                            imageSort.ImageAlign = ImageAlign.AbsMiddle
                            imageSort.Width = 20

                            If sortDirection = "ASC" Then
                                imageSort.ImageUrl = "~/Imagenes/asc.png"
                            Else
                                imageSort.ImageUrl = "~/Imagenes/desc.png"
                            End If
                            celda.Controls.Add(imageSort)
                        End If

                    Next
                End If

            End If
        End If

        If Not String.IsNullOrWhiteSpace(ToolTip) AndAlso ToolTipHabilitado Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("onMouseOver", "tooltip.show('" + ToolTip + "');hideTimerId = setTimeout(""tooltip.hide('" + e.Row.ClientID + "','" & e.Row.DataItemIndex + 1 & "', '#C7F4F8','#44698D',0,100)"",3000);")
                e.Row.Attributes.Add("onMouseOut", "tooltip.hide('" + e.Row.ClientID + "','" & e.Row.DataItemIndex + 1 & "', 'transparent','#44698D',0,100);clearTimeout(hideTimerId);")
            End If
        End If

    End Sub

End Class
