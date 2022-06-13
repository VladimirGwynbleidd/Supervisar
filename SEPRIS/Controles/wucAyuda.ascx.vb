' Fecha de creación: 02/08/2013
' Fecha de modificación: 
' Nombre del Responsable: Rafael Rodriguez Sanchez RARS1
' Empresa: Softtek
' Control que muestra la informacion de ayuda
Imports Entities
Public Class wucAyuda
    Inherits System.Web.UI.UserControl

#Region "Propiedades"
    Public ReadOnly Property SessionID As String
        Get
            Return Me.ClientID + "entAyuda"
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then

        End If

    End Sub

#Region "Eventos Controles"
    Protected Sub imbAyuda_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbAyuda.Click
        Dim si As New SiteInterno
        Dim menu As Integer = Convert.ToInt32(Session(si.SessionCurrentMenu))
        Dim submenu As Integer = Convert.ToInt32(Session(si.SessionCurrentSubMenu))
        Dim rutaPagina As String = Session(si.SessionCurrentURLSubMenu).ToString

        Dim filtro As String = CreaFiltroIndice(menu, submenu)
        Dim entAyuda As New Ayuda()
        Dim dtIndice As DataTable = entAyuda.ObtenerIndice(filtro)

        If dtIndice.Rows.Count = 1 Then
            CargaInformacion(menu, submenu, dtIndice.Rows(0).Item("N_ID_AYUDA"))
        ElseIf dtIndice.Rows.Count > 1 Then
            pnlInicioAyuda.Visible = False
            pnlIndice.Visible = True
            pnlInformacionAyuda.Visible = False
            lblIndice.Text = "Temas de Ayuda"
            CreaIndice(dtIndice)
        Else
            CargaInformacion(menu, submenu, -1)
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Ayuda", "DialogoAyuda();", True)
    End Sub

    Protected Sub imbInicioAyuda_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbInicioAyuda.Click
        MuestraInicio()
    End Sub

    Protected Sub imbIndiceAyuda_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbIndiceAyuda.Click
        pnlInicioAyuda.Visible = False
        pnlIndice.Visible = True
        pnlInformacionAyuda.Visible = False
        lblIndice.Text = "Índice"

        ObtenerIndiceAyuda()

    End Sub

    Protected Sub btnBuscarAyuda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscarAyuda.Click
        pnlInicioAyuda.Visible = False
        pnlIndice.Visible = True
        pnlInformacionAyuda.Visible = False

        Dim filtro As String = String.Format(" AND (MSPA.T_DSC_TITULO LIKE '%{0}%' OR MSPA.T_DSC_CONTENIDO LIKE '%{0}%') ", txtBuscarAyuda.Text)
        Dim entAyuda As New Ayuda()
        Dim dtIndice As DataTable = entAyuda.ObtenerIndice(filtro)
        CreaIndice(dtIndice)

        If dtIndice.Rows.Count > 0 Then            
            lblIndice.Text = "Resultados Encontrados"
        Else
            lblIndice.Text = "No se encontraron coincidencias para la búsqueda: """ + txtBuscarAyuda.Text + """"
        End If

        txtBuscarAyuda.Text = ""

    End Sub

    Protected Sub trvIndice_SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles trvIndice.SelectedNodeChanged
        CargarElementodeIndice(trvIndice.SelectedNode)
    End Sub

    Protected Sub lnbMenu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnbMenu.Click
        pnlInicioAyuda.Visible = False
        pnlIndice.Visible = True
        pnlInformacionAyuda.Visible = False

        Dim entAyuda As Ayuda = TryCast(ViewState(SessionID), Ayuda)
        ObtenerIndiceAyuda(entAyuda.IdentificadorMenu)

        Dim entMenu As New Menu(entAyuda.IdentificadorMenu)
        lblIndice.Text = entMenu.Descripcion
    End Sub

    Protected Sub lnbSubmenu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnbSubmenu.Click
        pnlInicioAyuda.Visible = False
        pnlIndice.Visible = True
        pnlInformacionAyuda.Visible = False

        Dim entAyuda As Ayuda = TryCast(ViewState(SessionID), Ayuda)
        ObtenerIndiceAyuda(entAyuda.IdentificadorMenu, entAyuda.IdentificadorSubmenu)

        Dim entSubmenu As New SubMenu(entAyuda.IdentificadorMenu, entAyuda.IdentificadorSubmenu)
        lblIndice.Text = entSubmenu.Descripcion

    End Sub

    Protected Sub lnbAyudaPadre_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnbAyudaPadre.Click
        Dim entAyuda As Ayuda = TryCast(ViewState(SessionID), Ayuda)
        If Not IsNothing(entAyuda) Then
            CargaInformacion(entAyuda.IdentificadorMenu, entAyuda.IdentificadorSubmenu, entAyuda.IdenticadorPadre)
        Else
            MuestraInicio()
        End If
    End Sub

    Protected Sub lnbAyuda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnbAyuda.Click
        Dim entAyuda As Ayuda = TryCast(ViewState(SessionID), Ayuda)
        If Not IsNothing(entAyuda) Then
            CargaInformacion(entAyuda.IdentificadorMenu, entAyuda.IdentificadorSubmenu, entAyuda.IdentificadorAyuda)
        Else
            MuestraInicio()
        End If
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Muestra la información de la Ayuda
    ''' </summary>
    ''' <param name="idMenu">Id Menu</param>
    ''' <param name="idSubmenu">Id Submenu</param>
    ''' <param name="idAyuda">Id Ayuda</param>
    ''' <remarks></remarks>
    Private Sub CargaInformacion(ByVal idMenu As Integer, ByVal idSubmenu As Integer, ByVal idAyuda As Integer)
        Dim entAyuda As New Ayuda(idMenu, idSubmenu, idAyuda)
        lblTituloAyuda.Text = entAyuda.Titulo
        lblContenudoAyuda.Text = entAyuda.Contenido

        If Not String.IsNullOrWhiteSpace(entAyuda.Titulo) Then
            pnlInicioAyuda.Visible = False
            pnlIndice.Visible = False
            pnlInformacionAyuda.Visible = True
            CreaBreadCrumb(idMenu, idSubmenu, idAyuda)
            ViewState(SessionID) = entAyuda
        Else
            MuestraInicio()
        End If

    End Sub

    ''' <summary>
    ''' Dibuja el Breabscrumb para la pantalla de contenido
    ''' </summary>
    ''' <param name="idMenu">Id Menu</param>
    ''' <param name="idSubmenu">Id Submenu</param>
    ''' <param name="idAyuda">Id Ayuda</param>
    ''' <remarks></remarks>
    Private Sub CreaBreadCrumb(ByVal idMenu As Integer, ByVal idSubmenu As Integer, ByVal idAyuda As Integer)
        Dim entMenu As New Menu(idMenu)
        Dim entSubmenu As New SubMenu(idMenu, idSubmenu)
        Dim entAyuda As New Ayuda(idMenu, idSubmenu, idAyuda)

        lnbMenu.Text = entMenu.Descripcion
        lnbSubmenu.Text = entSubmenu.Descripcion

        lnbAyuda.Text = entAyuda.Titulo

        If entAyuda.IdenticadorPadre <> 0 Then
            Dim entAyudaPadre As New Ayuda(idMenu, idSubmenu, entAyuda.IdenticadorPadre)
            If entAyudaPadre.Vigente = True Then
                lnbAyudaPadre.Text = entAyudaPadre.Titulo
                lnbAyudaPadre.Visible = True
                lblAyudaPadre.Visible = True
            Else
                lnbAyudaPadre.Visible = False
                lblAyudaPadre.Visible = False
            End If
        Else
            lnbAyudaPadre.Visible = False
            lblAyudaPadre.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' Muestra la pantalla de Inicio de Ayuda
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraInicio()
        lblContenidoInicioAyuda.Text = Conexion.SQLServer.Parametro.ObtenerValor("InicioAyudaContenido")
        pnlInicioAyuda.Visible = True
        pnlIndice.Visible = False
        pnlInformacionAyuda.Visible = False
    End Sub

    ''' <summary>
    ''' Obtiene el indice de la ayuda
    ''' </summary>
    ''' <param name="idMenu"></param>
    ''' <param name="idSubmenu"></param>
    ''' <remarks></remarks>
    Private Sub ObtenerIndiceAyuda(Optional ByVal idMenu As Integer = -1, Optional ByVal idSubmenu As Integer = -1)
        Dim filtro As String = CreaFiltroIndice(idMenu, idSubmenu)

        Dim entAyuda As New Ayuda()
        Dim dtIndice As DataTable = entAyuda.ObtenerIndice(filtro)
        CreaIndice(dtIndice)
    End Sub

    ''' <summary>
    ''' Crea el Filtro para la ayuda
    ''' </summary>
    ''' <param name="idMenu"></param>
    ''' <param name="idSubmenu"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreaFiltroIndice(Optional ByVal idMenu As Integer = -1, Optional ByVal idSubmenu As Integer = -1) As String
        Dim filtro As String = ""
        If idMenu <> -1 Then
            filtro += " AND MSPA.N_ID_MENU = " + idMenu.ToString
            If idSubmenu <> -1 Then
                filtro += " AND MSPA.N_ID_SUBMENU = " + idSubmenu.ToString
            End If
            filtro += " "
        End If
        Return filtro
    End Function

    ''' <summary>
    ''' Crea el indice 
    ''' </summary>
    ''' <param name="dtIndice">Datatable para generar indice</param>
    ''' <remarks></remarks>
    Private Sub CreaIndice(ByVal dtIndice As DataTable)
        trvIndice.Nodes.Clear()
        'Almacena el numero de menus en el arbol
        Dim menu As Integer = -1
        'Almacena el numero de submenus en el menu actual
        Dim submenu As Integer
        'Almacena el numero de ayudas en el submenu actual
        Dim ayuda As Integer

        For Each dr As DataRow In dtIndice.Rows
            If trvIndice.Nodes.Count = 0 OrElse dr.Item("N_ID_MENU") <> trvIndice.Nodes(menu).Value Then
                menu += 1
                submenu = 0
                ayuda = 0
                Dim nodoMenu As New TreeNode(dr.Item("T_DSC_MENU"), dr.Item("N_ID_MENU"))
                Dim nodoSubmenu As New TreeNode(dr.Item("T_DSC_SUBMENU"), dr.Item("N_ID_SUBMENU"))
                Dim nodoAyuda As New TreeNode(dr.Item("T_DSC_TITULO"), dr.Item("N_ID_AYUDA"))
                nodoSubmenu.ChildNodes.Add(nodoAyuda)
                nodoMenu.ChildNodes.Add(nodoSubmenu)
                trvIndice.Nodes.Add(nodoMenu)
            ElseIf dr.Item("N_ID_MENU") = trvIndice.Nodes(menu).Value And dr.Item("N_ID_SUBMENU") = trvIndice.Nodes(menu).ChildNodes(submenu).Value Then
                If Convert.ToInt32(dr.Item("N_ID_AYUDA_PADRE")) = 0 Then
                    ayuda += 1
                    Dim nodoAyuda As New TreeNode(dr.Item("T_DSC_TITULO"), dr.Item("N_ID_AYUDA"))
                    trvIndice.Nodes(menu).ChildNodes(submenu).ChildNodes.Add(nodoAyuda)
                Else
                    Dim inserto As Boolean = False

                    For index = 0 To trvIndice.Nodes(menu).ChildNodes(submenu).ChildNodes.Count - 1
                        If trvIndice.Nodes(menu).ChildNodes(submenu).ChildNodes(index).Value = dr.Item("N_ID_AYUDA_PADRE") Then
                            Dim nodoAyudaPadre As New TreeNode(dr.Item("T_DSC_TITULO"), dr.Item("N_ID_AYUDA"))
                            trvIndice.Nodes(menu).ChildNodes(submenu).ChildNodes(index).ChildNodes.Add(nodoAyudaPadre)
                            inserto = True
                        End If
                    Next
                    If inserto = False Then
                        ayuda += 1
                        Dim nodoAyuda As New TreeNode(dr.Item("T_DSC_TITULO"), dr.Item("N_ID_AYUDA"))
                        trvIndice.Nodes(menu).ChildNodes(submenu).ChildNodes.Add(nodoAyuda)
                    End If
                End If

            ElseIf dr.Item("N_ID_MENU") = trvIndice.Nodes(menu).Value And dr.Item("N_ID_SUBMENU") <> trvIndice.Nodes(menu).ChildNodes(submenu).Value Then
                submenu += 1
                ayuda = 0
                Dim nodoSubmenu As New TreeNode(dr.Item("T_DSC_SUBMENU"), dr.Item("N_ID_SUBMENU"))
                Dim nodoAyuda As New TreeNode(dr.Item("T_DSC_TITULO"), dr.Item("N_ID_AYUDA"))
                nodoSubmenu.ChildNodes.Add(nodoAyuda)
                trvIndice.Nodes(menu).ChildNodes.Add(nodoSubmenu)
            End If
        Next
        trvIndice.ExpandAll()
    End Sub

    ''' <summary>
    ''' Obtiene el padre del nodo y agrega su valor a la lista lstMenu
    ''' </summary>
    ''' <param name="nodo">nodo de indice</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerPadre(ByVal nodo As TreeNode) As TreeNode
        If IsNothing(nodo.Parent) Then
            Return nodo
        Else
            Dim nodoPadre As TreeNode = ObtenerPadre(nodo.Parent)
            lstMenu.Add(Convert.ToInt32(nodoPadre.Value))
            Return nodo
        End If
    End Function

    Private lstMenu As List(Of Integer)
    ''' <summary>
    ''' Obtiene la informacion del nodo selecionado para mostrar su informacion
    ''' </summary>
    ''' <param name="nodo">nodo del indice</param>
    ''' <remarks></remarks>
    Private Sub CargarElementodeIndice(ByVal nodo As TreeNode)
        lstMenu = New List(Of Integer)
        ObtenerPadre(nodo)

        lstMenu.Add(Convert.ToInt32(nodo.Value))

        Select Case lstMenu.Count
            Case 1
                ObtenerIndiceAyuda(lstMenu(0))
            Case 2
                ObtenerIndiceAyuda(lstMenu(0), lstMenu(1))
            Case 3
                CargaInformacion(lstMenu(0), lstMenu(1), lstMenu(2))
            Case 4
                CargaInformacion(lstMenu(0), lstMenu(1), lstMenu(3))
            Case Else
                MuestraInicio()
        End Select

    End Sub

#End Region

End Class