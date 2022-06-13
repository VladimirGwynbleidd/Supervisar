Public Class PersonalizarColumnas
    Inherits System.Web.UI.UserControl


    '***********************************************************************************************************
    ' Fecha Creación:       03-03-2016
    ' Codificó:             AGC
    ' Empresa:              Softtek
    ' Descripción           Control de usuario para las personalizacion de columnas de GridViews
    '***********************************************************************************************************

#Region "Objetos de trabajo"

    Private IdGridView As Integer = 0
    Private GridViewActual As GridView = Nothing
    Private UsuarioActual As String = ""

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad para asignar el Identificador del gridview a personalizar
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property IdentificadorGridView As Integer
        Set(ByVal value As Integer)
            IdGridView = value
            hdnIdGridView.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Control GridView a Personalizar
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property GridViewPersonalizar As GridView
        Set(ByVal value As GridView)
            GridViewActual = value
        End Set
    End Property

    ''' <summary>
    ''' Usuario actual del sistema
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property Usuario As String
        Set(ByVal value As String)
            UsuarioActual = value
        End Set
    End Property

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Clase para argumentos del evento FinPersonalizacion
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PersonalizarColumnasEvent
        Inherits EventArgs

        Public IdentificadorGridView As Integer

        Public Sub New()
            MyBase.New()

        End Sub

    End Class

    ''' <summary>
    ''' Evento para ser atrapado en el control padre NECESARIO PARA DE AQUI INVOCAR EL MÉTODO GuardarPersonalizacion()
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="sender"></param>
    ''' <remarks></remarks>
    Public Event FinPersonalizacion(ByVal e As PersonalizarColumnasEvent, ByVal sender As Object)

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' operaciones en caso de ser necesarias

    End Sub

#Region "Botones de Asignación"

    Protected Sub btnPasarVerUno_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPasarUnoVer.Click

        Dim listaSeleccionados As New List(Of UI.WebControls.ListItem)

        ' pasamos a los nuevos asignados
        For indice = 0 To lstSinAsignar.Items.Count - 1

            If lstSinAsignar.Items(indice).Selected Then

                lstAsignado.Items.Add(lstSinAsignar.Items(indice))
                listaSeleccionados.Add(lstSinAsignar.Items(indice))

            End If

        Next indice


        ' quitamos de desasignados
        For Each item As UI.WebControls.ListItem In listaSeleccionados

            lstSinAsignar.Items.Remove(item)

        Next

        lstSinAsignar.ClearSelection()
        lstAsignado.ClearSelection()

        lstSinAsignar.SelectedIndex = -1
        lstAsignado.SelectedIndex = -1

    End Sub

    Protected Sub btnPasarVerTodos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPasarTodosVer.Click

        Dim total As Integer

        ' Pasamos todos los elementos para ser asignados
        For total = 0 To lstSinAsignar.Items.Count - 1
            lstAsignado.Items.Add(lstSinAsignar.Items(total))
        Next total

        lstSinAsignar.Items.Clear()


    End Sub

    Protected Sub btnPasarOcultarUno_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPasarUnoOcultar.Click

        Dim listaSeleccionados As New List(Of UI.WebControls.ListItem)

        If lstAsignado.SelectedIndex >= 0 Then
            lstSinAsignar.Items.Add(lstAsignado.Items(lstAsignado.SelectedIndex))
            lstAsignado.Items.RemoveAt(lstAsignado.SelectedIndex)
        End If


        ' pasamos a los nuevos desasignados
        For indice = 0 To lstAsignado.Items.Count - 1

            If lstAsignado.Items(indice).Selected Then

                lstSinAsignar.Items.Add(lstAsignado.Items(indice))
                listaSeleccionados.Add(lstAsignado.Items(indice))

            End If

        Next indice

        ' quitamos de asignados
        For Each item As UI.WebControls.ListItem In listaSeleccionados

            lstAsignado.Items.Remove(item)

        Next

        lstSinAsignar.ClearSelection()
        lstAsignado.ClearSelection()


        lstAsignado.SelectedIndex = -1
        lstSinAsignar.SelectedIndex = -1

    End Sub

    Protected Sub btnPasarOcultarTodos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPasarTodosOcultar.Click

        Dim total As Integer

        ' Pasamos todos los elementos para ser no asignados
        For total = 0 To lstAsignado.Items.Count - 1
            lstSinAsignar.Items.Add(lstAsignado.Items(total))
        Next total

        lstAsignado.Items.Clear()

    End Sub

#End Region

#Region "Métodos Publicos"

    ''' <summary>
    ''' Método para cargar datos y mostrar el control
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Mostrar()

        ValidaAsignaciones()

        VerificaColumnasGridViewUsuario()

        LlenaListas(CargaDatos())

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosPersonalizacion", "MostramosPersonalizaColumnasControl();", True)

    End Sub


    ''' <summary>
    ''' Método que permite personalizar las columnas visibles del GridView 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Personalizar()

        ValidaAsignaciones()

        Dim columnas As List(Of Entities.PersonalizarColumnas.Columnas) = CargaDatos()

        ' obtenemos las columnas visibles
        Dim columnasAsignados As IEnumerable(Of Entities.PersonalizarColumnas.Columnas) = From columna In columnas Where columna.Vigencia = True

        ' obtenemos las columnas no visibles
        Dim columnasNoAsignados As IEnumerable(Of Entities.PersonalizarColumnas.Columnas) = From columna In columnas Where columna.Vigencia = False

        ' procesamos columnas visibles
        If columnasAsignados.Any Then
            For Each columna As Entities.PersonalizarColumnas.Columnas In columnasAsignados
                GridViewActual.Columns(columna.Identificador).Visible = True
            Next
        End If

        ' procesamos columnas NO visibles
        If columnasNoAsignados.Any Then
            For Each columna As Entities.PersonalizarColumnas.Columnas In columnasNoAsignados
                GridViewActual.Columns(columna.Identificador).Visible = False
            Next
        End If
    End Sub

#End Region

#Region "Métodos Privados"

    ''' <summary>
    ''' Método que llena las listas de asign ados y no asignados
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LlenaListas(ByVal listaColumnas As List(Of Entities.PersonalizarColumnas.Columnas))

        ' obtenemos las columnas visibles
        Dim columnasAsignados As IEnumerable(Of Entities.PersonalizarColumnas.Columnas) = From columna In listaColumnas Where columna.Vigencia = True

        ' obtenemos las columnas no visibles
        Dim columnasNoAsignados As IEnumerable(Of Entities.PersonalizarColumnas.Columnas) = From columna In listaColumnas Where columna.Vigencia = False

        lstAsignado.Items.Clear()
        lstAsignado.DataSource = columnasAsignados
        lstAsignado.DataValueField = "Identificador"
        lstAsignado.DataTextField = "Texto"
        lstAsignado.DataBind()

        lstSinAsignar.Items.Clear()
        lstSinAsignar.DataSource = columnasNoAsignados
        lstSinAsignar.DataValueField = "Identificador"
        lstSinAsignar.DataTextField = "Texto"
        lstSinAsignar.DataBind()

    End Sub

    ''' <summary>
    ''' Método para carga de datos según el identificador del GridView proporcionado
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CargaDatos() As List(Of Entities.PersonalizarColumnas.Columnas)

        Return Entities.PersonalizarColumnas.ObtenerUsuarioColumnasGridView(UsuarioActual, IdGridView)

    End Function

    ''' <summary>
    ''' Método que valida la asignacion de los elementos necesarios para que funcione el control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ValidaAsignaciones()

        If IsNothing(GridViewActual) OrElse IdGridView = 0 OrElse UsuarioActual.Trim = "" Then

            Throw New NullReferenceException("Se deben asignar todas las propiedades del control")

        End If

    End Sub

    ''' <summary>
    ''' Método encargado de validar si un usuario tiene columnas asignadas para el gridview en cuestion
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub VerificaColumnasGridViewUsuario()

        ' Si el usuario no tiene columnas configuradas, asigna todas las columnas como visibles (primera vez)
        If Not Entities.PersonalizarColumnas.UsuarioTieneColumnas(UsuarioActual, IdGridView) Then

            If Not Entities.PersonalizarColumnas.ConfiguraColumnasPrimeraVez(UsuarioActual, IdGridView) Then

                Throw New Exception("No se han guardado las columnas")

            End If

        End If

    End Sub

#End Region

#Region "Acciones de los botones generales"

    ''' <summary>
    ''' Método que guarda la personalizacion de columnas para el grid
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GuardarPersonalizacion()

        ValidaAsignaciones()

        Dim listaColumnas As New List(Of Entities.PersonalizarColumnas.Columnas)

        ' obtenemos lista de columnas para ser visibles (asignadas)
        For Each renglon As UI.WebControls.ListItem In lstAsignado.Items

            listaColumnas.Add(New Entities.PersonalizarColumnas.Columnas(IdGridView, CInt(renglon.Value), "", "", 1))

        Next

        ' obtenemos lista de columnas para ser NO visibles (No asignadas)
        For Each renglon As UI.WebControls.ListItem In lstSinAsignar.Items

            listaColumnas.Add(New Entities.PersonalizarColumnas.Columnas(IdGridView, CInt(renglon.Value), "", "", 0))

        Next

        Dim columnas As New Entities.PersonalizarColumnas.UsuarioGridViewColumnas
        columnas.Usuario = UsuarioActual
        columnas.GridView = Entities.PersonalizarColumnas.ObtenerGridView(IdGridView)
        columnas.Columnas = listaColumnas

        ' guardamos personalizacion en base de datos
        If Not Entities.PersonalizarColumnas.GuardaColumnasGridViewUsuario(columnas) Then

            Throw New Exception("No fué posible guardar los personalización")

        End If


        ' Personalizamos grid view
        Personalizar()

    End Sub

    Private Sub btnGuardarPersonalizacion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardarPersonalizacion.Click

        ' lanzamos evento para que se llenen nuevamente variables y se invoque método GuardarPersonalizacion()
        Dim parametroEvento As New PersonalizarColumnasEvent
        parametroEvento.IdentificadorGridView = CInt(hdnIdGridView.Value)
        RaiseEvent FinPersonalizacion(parametroEvento, Nothing)

    End Sub

    Private Sub btnCancelarPersonalizacion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelarPersonalizacion.Click

        ' el simple invocado debe cerrar la ventana modal

    End Sub

#End Region



End Class