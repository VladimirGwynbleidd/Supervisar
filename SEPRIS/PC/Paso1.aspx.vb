Imports System.Web.Configuration
Imports System.Data
Imports System.Web.UI.WebControls

Public Class Paso1
    Inherits System.Web.UI.Page

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property
    Public ReadOnly Property Folio_SICOD
        Get
            Return Session("ID_FOLIO_SICOD")
        End Get
    End Property



    Public Property FolioPC As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Calculo_Fecha_DiasHabiles(15)
            DetalleSICOD1.Inicializar()
            Supervisor1.Inicializar()
            'lblFolio.Text = "Folio: " + Folio.ToString()
            lblFolio.Text = "Paso 1 : Recepción de Programa de corrección"

        End If

    End Sub


    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubprocesos(Proceso As String) As List(Of ListItem)
        Dim dsSubprocesos As DataSet = Subproceso.ObtenerVigentesPorProceso(Proceso)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("I_ID_SUBPROCESO").ToString(),
                          .Text = row("T_DSC_DESCRIPCION").ToString()})

        Next

        Return subprocesos
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSupervisores(Subproceso As String) As List(Of ListItem)
        Dim dsSubprocesos As DataSet = Supervisor.ObtenerVigentesPorSubproceso(Subproceso, 0)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})

        Next

        Return subprocesos
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubEntidades(Entidad As Integer) As List(Of ListItem)
        Dim subEntidades As New List(Of ListItem)()

        For Each row As Data.DataRow In ConexionSICOD.ObtenerSubEntidadesAFORE(Entidad).Rows
            subEntidades.Add(New ListItem() With {
                          .Value = row("ID_SUBENT").ToString(),
                          .Text = row("SGL_SUBENT").ToString()})

        Next

        Return subEntidades
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function AsignarPC(Folio As Integer,
                                    TipoEntidad As String,
                                    Entidad As String,
                                    NumeroPcInterno As String,
                                    FechaVencimiento As String,
                                    Proceso As Integer,
                                    SubProceso As Integer,
                                    Descripcion As String,
                                    SubEntidad As String,
                                    Area As String,
                                    IdArea As Integer,
                                    ddlSubEntidad As String,
                                    Supervisor As String) As String


        Dim res As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)
        Dim ReultadoEntidad() As String = Split(Entidad, ",")
        Dim ResultadoTipoEntidad() As String = Split(TipoEntidad, ",")
        Dim ResultadoSubEntidad() As String = Split(ddlSubEntidad, ",")

        Dim FolioSupervisar As String
        Dim FolioReturn As String = ""

        Dim myDate As Date = Date.Now()

        Dim mes As String = Month(myDate).ToString("D2")
        Dim año As String = myDate.ToString("yy")

        Dim ent As String = ReultadoEntidad(0)
        Dim tipoEnt As String = ResultadoTipoEntidad(0)
        Dim IdSubEntidad As String

        If (ResultadoSubEntidad(0) <> "undefined") Then
            IdSubEntidad = ResultadoSubEntidad(0)
            SubEntidad = IdSubEntidad
        Else
            IdSubEntidad = 0
        End If

        Dim dtEnt As DataTable = ConexionSICOD.ObtenerEntidadesAFOREArea(IdArea)
        Dim dvEnt As DataView = New DataView(dtEnt)
        dvEnt.RowFilter = "(CVE_ID_ENT = " + ent.ToString() + ")"
        Dim strEnt As String = dvEnt(0)(3).ToString()
        If Split(strEnt, " ").Count > 1 Then
            If Split(strEnt, " ")(1) = "BANORTE" Then
                strEnt = Split(strEnt, " ")(1)
            Else
                strEnt = Split(strEnt, " ")(0)
            End If
        End If

        If Split(strEnt, "-").Count > 1 Then
            strEnt = Split(strEnt, "-")(1)
        Else
            strEnt = Split(strEnt, "-")(0)
        End If

        If strEnt = "CITIBANAMEX" Then
            strEnt = "BANAMEX"
        End If

        strEnt = strEnt.Trim()

        Dim pc As New Entities.PC()

        FolioSupervisar = pc.Consecutivo(IdArea, "PC") + "/PC/" + Area + "/" + strEnt + "/" + mes + año
        FolioSupervisar = FolioSupervisar.Replace(" ", "")

        FolioReturn += FolioSupervisar

        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(1)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(1)
        ListaCampos.Add("I_ID_FOLIO_SUPERVISAR") : ListaValores.Add(FolioSupervisar)
        ListaCampos.Add("T_NUM_INTERNO") : ListaValores.Add(NumeroPcInterno)
        ListaCampos.Add("F_FECH_VENC") : ListaValores.Add(Date.ParseExact(FechaVencimiento, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        ListaCampos.Add("I_ID_PROCESO") : ListaValores.Add(Proceso)
        ListaCampos.Add("I_ID_SUBPROCESO") : ListaValores.Add(SubProceso)
        ListaCampos.Add("T_DSC_DESCRIPCION") : ListaValores.Add(Descripcion)
        ListaCampos.Add("ID_ENTIDAD") : ListaValores.Add(ent)
        ListaCampos.Add("I_ID_AREA") : ListaValores.Add(IdArea)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
        ListaCampos.Add("I_ID_TIPO_ENTIDAD") : ListaValores.Add(tipoEnt)
        ListaCampos.Add("I_ID_SUBENTIDAD") : ListaValores.Add(IdSubEntidad)

        res = BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        Dim Result() As String = Split(SubEntidad, ",")
        Dim ListaCampos_1 As New List(Of String)
        Dim ListaValores_1 As New List(Of Object)

        Dim subEnts As DataTable = ConexionSICOD.ObtenerSubEntidadesAFORE(ReultadoEntidad(0))

        If (Result(0) <> "undefined") Then
            Dim FolioSupervisarSub As String
            For Each val As String In Result
                Dim subentsf As DataView = New DataView(subEnts)
                subentsf.RowFilter = "(ID_SUBENT = " + val.ToString() + ")"
                Dim strSubE As String = subentsf(0)(4).ToString()
                Try
                    strSubE = Split(strSubE, "(")(1)
                    strSubE = Replace(strSubE, ")", "")
                Catch ex As Exception

                End Try

                FolioSupervisarSub = FolioSupervisar + "/" + strSubE
                'Mostrar solo folio supervisar sub entidad cuando se identifica que se selecciono una subentidad
                'FolioReturn += "</br>" + FolioSupervisarSub
                FolioReturn = FolioSupervisarSub
                ListaCampos_1.Add("N_ID_FOLIO") : ListaValores_1.Add(Folio)
                ListaCampos_1.Add("ID_ENTIDAD") : ListaValores_1.Add(ReultadoEntidad(0))
                ListaCampos_1.Add("N_ID_SUB_ENTIDAD") : ListaValores_1.Add(val)
                ListaCampos_1.Add("I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD") : ListaValores_1.Add(FolioSupervisarSub)
                BandejaPC.GuardarRel_Entidad_SubEntidad_PC(ListaCampos_1, ListaValores_1)
                ListaCampos_1.Clear()
                ListaValores_1.Clear()
            Next
        End If

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

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.IdentificadorUsuario
            Notifica.NotificarCorreo(97)

        Catch ex As Exception

        End Try


        Return FolioReturn


    End Function


    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ConexionSICOD.EliminarPC(Folio_SICOD)
        BandejaPC.EliminarPC(Folio)
        Response.Redirect("~/BandejaSICOD.aspx", True)
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
    End Sub

    Protected Sub Calculo_Fecha_DiasHabiles(DiasAtencion As Integer)
        'Dim dif As Integer
        'Dim i As Integer
        'Dim fecha_inicial As String = "01/11/2018"
        ''Dim Fecha As Date = CDate(fecha_inicial).GetDateTimeFormats()


        'DateAdd(DateInterval.Day, DiasAtencion, Fecha)
        'MsgBox("Dias habiles = " & Fecha.DayOfWeek & ", " & Fecha, MsgBoxStyle.Information, "Resultado")


        'For i = 0 To DiasAtencion
        '    CDate(fecha_inicial).ToString("d")
        'Next

        'Dim fecha_final As String = "5/11/2018"
        'dif = Val(CDate(fecha_final)) - Val(CDate(fecha_inicial)) + 1
        'For i = 0 To dif - 1
        'If Weekday(System.DateTime.FromOADate(CDate(fecha_inicial).ToOADate + i), FirstDayOfWeek.Monday) = 6 Or Weekday(System.DateTime.FromOADate(CDate(fecha_inicial).ToOADate + i), FirstDayOfWeek.Monday) = 7 Then
        '    dif = dif - 1
        'End If
        'Next i
        'MsgBox("Dias habiles = " & dif, MsgBoxStyle.Information, "Resultado")
    End Sub

End Class