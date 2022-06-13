Public Class Inspector
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property SubProcesoView
        Get
            Return Session("SUB_PROCESO")
        End Get
    End Property

    Public ReadOnly Property PC As Entities.PC
        Get
            Return DirectCast(Session("PC"), Entities.PC)
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Select Case PC.IdEstatus
                Case 2
                    CargarInpectores()
                    pnlAgregarInspector.Enabled = True

                Case Else
                    CargarInpectoresSeleccion()
                    pnlAgregarInspector.Enabled = False

            End Select
        End If
    End Sub

    Public Sub Inicializar()
        Select Case PC.IdEstatus
            Case 2
                CargarInpectores()
                pnlAgregarInspector.Enabled = True

            Case Else
                CargarInpectoresSeleccion()
                pnlAgregarInspector.Enabled = False

        End Select
    End Sub

    Protected Function ObtenerInspector_SubProceso() As List(Of ListItem)
        Dim ListInspectores As New List(Of ListItem)()
        Dim dsSubProcesos As DataSet = Inspector.ObtenerVigentesPorSubproceso(SubProcesoView, 0)
        For Each row As Data.DataRow In dsSubProcesos.Tables(0).Rows
            ListInspectores.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next

        Return ListInspectores
    End Function

    Public Sub CargarInpectoresSeleccion()
        Dim ListInspectoresSelec As New List(Of ListItem)()
        Dim dsInspectores As DataSet = Inspector.ObtenerVigentesPorFolio(Folio)

        For Each row As Data.DataRow In dsInspectores.Tables(0).Rows
            ListInspectoresSelec.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next

        lstInspectorSeleccionado.DataSource = ListInspectoresSelec
        lstInspectorSeleccionado.DataTextField = "Text"
        lstInspectorSeleccionado.DataValueField = "Value"
        lstInspectorSeleccionado.DataBind()

        Dim ListInspectores As New List(Of ListItem)()
        Dim dsSubProcesos As DataSet = Inspector.ObtenerVigentesPorSubproceso(PC.IdSubproceso, 0)
        For Each row As Data.DataRow In dsSubProcesos.Tables(0).Rows
            ListInspectores.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next

        For Each InpAsing As ListItem In ListInspectoresSelec
            ListInspectores.Remove(InpAsing)
        Next

        If (ListInspectores.Count <> 0) Then
            lstInspectorDisponible.DataSource = ListInspectores
            lstInspectorDisponible.DataTextField = "Text"
            lstInspectorDisponible.DataValueField = "Value"
            lstInspectorDisponible.DataBind()
            lstInspectorSeleccionado.DataSource = ""
        Else
            lstInspectorDisponible.DataSource = ""
            lstInspectorDisponible.DataBind()

        End If




    End Sub

    Public Shared Function ObtenerInspectoresFolioPC(Folio As Integer) As List(Of ListItem)
        Dim ListInspectoresSelec As New List(Of ListItem)()
        Dim dsInspectores As DataSet = Inspector.ObtenerVigentesPorFolio(Folio)

        For Each row As Data.DataRow In dsInspectores.Tables(0).Rows
            ListInspectoresSelec.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next

        Return ListInspectoresSelec
    End Function

    Protected Sub CargarInpectores()

        Dim ListInspectores As New List(Of ListItem)()
        Dim dsSubProcesos As DataSet = Inspector.ObtenerVigentesPorSubproceso(PC.IdSubproceso, 0)
        For Each row As Data.DataRow In dsSubProcesos.Tables(0).Rows
            ListInspectores.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next

        lstInspectorDisponible.DataSource = ListInspectores
        lstInspectorDisponible.DataTextField = "Text"
        lstInspectorDisponible.DataValueField = "Value"
        lstInspectorDisponible.DataBind()

    End Sub


End Class