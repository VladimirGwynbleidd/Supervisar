Imports Entities

Public Class ReasignaSupervisores

    Inherits System.Web.UI.Page

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property AbrArea
        Get
            Return Session("ABR_AREA")
        End Get
    End Property

    Public ReadOnly Property SubProcesoView
        Get
            Return Session("SUB_PROCESO")
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
            Return Session("ID_USR")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            InicializarReasigna()
        End If
    End Sub

    Public Sub InicializarReasigna()
        ddlSubprocesoP2.Attributes.Add("onchange", "ObtenerListaSupervisores()")
        Utilerias.Generales.CargarComboOrdenadoOriginalRows(ddlProcesoP2, Proceso.ObtenerPcVigentes(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea), "T_DSC_DESCRIPCION", "I_ID_PROCESO")

        N_Folio.Value = Folio

        hdnUsuario.Value = Usuario

        prcs.Visible = False

        Dim SupervisoresListAsignado As List(Of ListItem) = ObtenerSupervisoresFolio()
        lstSupervisorSeleccionado.DataSource = SupervisoresListAsignado
        lstSupervisorSeleccionado.DataTextField = "Text"
        lstSupervisorSeleccionado.DataValueField = "Value"
        lstSupervisorSeleccionado.DataBind()

        Dim SupervisorDisponible As List(Of ListItem) = ObtenerSupervisores()
        For Each SupAsing As ListItem In SupervisoresListAsignado
            SupervisorDisponible.Remove(SupAsing)
        Next

        If (SupervisorDisponible.Count <> 0) Then
            lstSupervisorDisponible.DataSource = SupervisorDisponible
            lstSupervisorDisponible.DataTextField = "Text"
            lstSupervisorDisponible.DataValueField = "Value"
            lstSupervisorDisponible.DataBind()
        End If

    End Sub

    Protected Function ObtenerSupervisoresFolio() As List(Of ListItem)
        Dim dsSupervisor As DataSet = Supervisor.ObtenerPorFolio(Folio)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSupervisor.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})

        Next

        Return subprocesos
    End Function


    Protected Function ObtenerSupervisores() As List(Of ListItem)
        Dim dsSubprocesos As DataSet = Supervisor.ObtenerVigentesPorSubproceso(SubProcesoView, 0)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})

        Next

        Return subprocesos
    End Function


    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function Reasignar(Folio As Integer, Proceso As Integer, SubProceso As Integer, Usuario As String, Supervisor As String) As Boolean
        BandejaPC.EliminarSupervisores(Folio)


        Dim Result2() As String = Split(Supervisor, ",")
        Dim ListaCampos_Supervisor As New List(Of String)
        Dim ListaValores_Supervisor As New List(Of Object)

        If (Result2(0) <> "undefined" Or Result2(0) <> "") Then
            For Each val As String In Result2
                ListaCampos_Supervisor.Add("N_ID_FOLIO") : ListaValores_Supervisor.Add(Folio)
                ListaCampos_Supervisor.Add("T_ID_USUARIO") : ListaValores_Supervisor.Add(val)
                BandejaPC.AsignarSupervidores(ListaCampos_Supervisor, ListaValores_Supervisor)
                ListaCampos_Supervisor.Clear()
                ListaValores_Supervisor.Clear()
            Next
        End If

        Dim res As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)
        ListaCampos.Add("I_ID_PROCESO") : ListaValores.Add(Proceso)
        ListaCampos.Add("I_ID_SUBPROCESO") : ListaValores.Add(SubProceso)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        res = BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        Dim DetallePC As DetallePC = New DetallePC()
        DetallePC.GuardarBitacota(Folio, Usuario, "1", "Se reasignaron supervisores.", " ")

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usr As New Entities.Usuario()
            usr = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usr.Nombre + " " + usr.Apellido + " " + usr.ApellidoAuxiliar
            Notifica.NotificarCorreo(99)
        Catch ex As Exception

        End Try

        Return True

    End Function

End Class