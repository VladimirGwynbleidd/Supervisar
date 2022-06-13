Imports Clases
Imports Entities

Public Class SubVisitas
    Inherits System.Web.UI.UserControl

    Public Property pagina As System.Web.UI.Page
    Public Property Mensaje As String

    Private Property LlenoSubvisitas As Boolean
        Get
            If IsNothing(ViewState("LlenoSubvisitas")) Then
                Return False
            Else
                Dim lbAux As Boolean = False
                If Boolean.TryParse(ViewState("LlenoSubvisitas"), lbAux) Then
                    Return lbAux
                Else
                    Return False
                End If
            End If
        End Get
        Set(value As Boolean)
            ViewState("LlenoSubvisitas") = value
        End Set
    End Property

    Public Property piIdEntidad As Integer
        Get
            If IsNothing(ViewState("piIdEntidad")) Then
                Return 0
            Else
                Return CInt(ViewState("piIdEntidad"))
            End If
        End Get
        Set(value As Integer)
            ViewState("piIdEntidad") = value
        End Set
    End Property

    Public Property psDscEntidadSb As String
        Get
            If IsNothing(ViewState("psDscEntidadSb")) Then
                Return ""
            Else
                Return ViewState("psDscEntidadSb").ToString()
            End If
        End Get
        Set(value As String)
            ViewState("psDscEntidadSb") = value
        End Set
    End Property

    Public Property psFolioVisitaPadreSb As String
        Get
            If IsNothing(ViewState("psFolioVisitaPadreSb")) Then
                Return ""
            Else
                Return ViewState("psFolioVisitaPadreSb").ToString()
            End If
        End Get
        Set(value As String)
            ViewState("psFolioVisitaPadreSb") = value
        End Set
    End Property

    Public Property piVistaPadreSb As Integer
        Get
            If IsNothing(ViewState("piVistaPadreSb")) Then
                Return 0
            Else
                Return CInt(ViewState("piVistaPadreSb"))
            End If
        End Get
        Set(value As Integer)
            ViewState("piVistaPadreSb") = value
        End Set
    End Property

    Private Property pLstTipoEntidad As List(Of TipoSubEntidad)
        Get
            Return ViewState("pLstTipoEntidadSB")
        End Get
        Set(value As List(Of TipoSubEntidad))
            ViewState("pLstTipoEntidadSB") = value
        End Set
    End Property

    Public Property TextoCheck As String
        Get
            Return chkSubVisitas.Text
        End Get
        Set(value As String)
            chkSubVisitas.Text = value
        End Set
    End Property

    Public Property psSubEntidadesSeleccionadas As List(Of Entities.TipoSubEntidad)
        Get
            If IsNothing(ViewState("psSubEntidadesSeleccionadas")) Then
                Return New List(Of Entities.TipoSubEntidad)
            Else
                Return CType(ViewState("psSubEntidadesSeleccionadas"), List(Of Entities.TipoSubEntidad))
            End If
        End Get
        Set(value As List(Of Entities.TipoSubEntidad))
            ViewState("psSubEntidadesSeleccionadas") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub chkSubVisitas_CheckedChanged(sender As Object, e As EventArgs) Handles chkSubVisitas.CheckedChanged
        chkSubVisitas.Checked = False
        If Not LlenoSubvisitas Then
            pLstTipoEntidad = New List(Of TipoSubEntidad)
            LlenaSubvisitas()
            LlenoSubvisitas = True
        End If

        If chkSubEntidad.Items.Count < 1 Then
            Mensaje = "No hay subentidades para la entidad actual. <br /><br />"
            imgAviso.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AvisoCCF();", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MuestraModal();", True)
        End If
    End Sub

    Private Sub LlenaSubvisitas()
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        If Not IsNothing(usuario) And piIdEntidad <> 0 Then
            Dim lsIdEntidad As String = piIdEntidad.ToString().Substring(1)
            Dim con1 As New OracleConexion()
            Dim lsQuery As String = ""

            If psSubEntidadesSeleccionadas.Count > 0 Then
                If lsIdEntidad = "52" Then ''TRAER PARA BANAMEX LA ENTIDAD 652 AV1
                    ''lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (1,2,3,4,17) ANd S.VIG_FLAG= CASE WHEN S.ID_SUBENT = 1 THEN S.VIG_FLAG ELSE 1 END ORDER BY S.SGL_SUBENT "
                    lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (1,2,3,4,17) AND S.VIG_FLAG=CASE WHEN S.ID_SUBENT = 1 THEN S.VIG_FLAG ELSE 1 END AND CAST(S.ID_SUBENT AS VARCHAR2(15)) || CAST(S.ID_T_ENT AS VARCHAR2(15)) NOT IN (" & psSubEntidadesSeleccionadas.ToListQuery() & ") ORDER BY S.ID_SUBENT "
                Else
                    ''lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (2,3,4,17) ANd S.VIG_FLAG=1 ORDER BY S.SGL_SUBENT "
                    lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (2,3,4,17) AND S.VIG_FLAG=1 AND CAST(S.ID_SUBENT AS VARCHAR2(15)) || CAST(S.ID_T_ENT AS VARCHAR2(15)) NOT IN (" & psSubEntidadesSeleccionadas.ToListQuery() & ") ORDER BY S.ID_SUBENT "
                End If
            Else
                If lsIdEntidad = "52" Then ''TRAER PARA BANAMEX LA ENTIDAD 652 AV1
                    ''lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (2,3,4,17) AND S.VIG_FLAG=1 ORDER BY S.ID_SUBENT "
                    lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (1,2,3,4,17) AND S.VIG_FLAG=CASE WHEN S.ID_SUBENT = 1 THEN S.VIG_FLAG ELSE 1 END ORDER BY S.ID_SUBENT "
                Else
                    lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (2,3,4,17) AND S.VIG_FLAG=1 ORDER BY S.ID_SUBENT "
                End If
            End If

            Dim dt As DataSet = con1.Datos(lsQuery)

            If dt IsNot Nothing Then
                If dt.Tables(0).Rows.Count > 0 Then
                    chkSubEntidad.DataSource = dt
                    chkSubEntidad.DataTextField = "DSC_SUBENT"
                    chkSubEntidad.DataValueField = "ID_SUBENT"
                    chkSubEntidad.DataBind()

                    Dim objTipoEntidad As TipoSubEntidad
                    pLstTipoEntidad.Clear()

                    Dim i As Integer = 0
                    For Each dtTable As DataTable In dt.Tables
                        For Each drTable As DataRow In dtTable.Rows
                            objTipoEntidad = New TipoSubEntidad With {.IdItem = i, .IdSubEntidad = drTable("ID_SUBENT"), .IdTipoEntidad = drTable("ID_T_ENT"), .DscTipoEntidad = drTable("DESC_T_ENT")}
                            pLstTipoEntidad.Add(objTipoEntidad)
                            i = i + 1
                        Next
                    Next
                Else
                    chkSubEntidad.Items.Clear()
                End If
            Else
                chkSubEntidad.Items.Clear()
            End If
        End If
    End Sub

    Protected Sub btnGeneraSubvisita_Click(sender As Object, e As EventArgs)
        Dim index As Integer = 0
        Dim idSubEntidad As Integer = 0
        Dim lsNuevaVisita As Integer = 0
        Dim lsVisitaGenerada As String = "<ul>"
        Dim objTipoSub As New TipoSubEntidad
        Dim lbGenero As Boolean = False
        Dim lsFolio As String = ""
        Dim lsAux As String = ""

        If chkSubEntidad.SelectedValue = "" Then
            lblMsg.Text = "Selecciona alguna Subentidad."
            lblMsg.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MuestraModal();", True)
            Exit Sub
        Else
            lblMsg.Visible = False
        End If

        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        Dim objAreas As New Entities.Areas(usuario.IdArea)
        objAreas.CargarDatos()

        ''Replicar la visita
        For i As Integer = 0 To chkSubEntidad.Items.Count - 1
            If chkSubEntidad.Items(i).Selected Then
                objTipoSub = (From TS As TipoSubEntidad In pLstTipoEntidad Where TS.IdItem = i And TS.IdSubEntidad = chkSubEntidad.Items(i).Value Select TS).FirstOrDefault()

                If Not IsNothing(usuario) And Not IsNothing(objTipoSub) Then

                    If psFolioVisitaPadreSb <> "" Then
                        lsFolio = psFolioVisitaPadreSb & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
                    Else
                        lsFolio = "999" & "/" & objAreas.Descripcion & "/" & psDscEntidadSb & "/" & Today.ToString("MMyy") & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
                    End If

                    lsAux = AccesoBD.registrarCopiaVisita(piVistaPadreSb, usuario.IdArea, lsNuevaVisita, Constantes.TipoCopia.SubVisita, lsFolio, chkSubEntidad.Items(i).Value, chkSubEntidad.Items(i).Text, objTipoSub.IdTipoEntidad, objTipoSub.DscTipoEntidad)

                    If lsAux <> "" Then
                        lsVisitaGenerada &= "<li>" & lsAux & "</li>"
                        lbGenero = True
                    End If
                Else
                    lsFolio = "999" & "/" & objAreas.Descripcion & "/" & psDscEntidadSb & "/" & Today.ToString("MMyy") & "/" & chkSubEntidad.Items(i).Value
                    lsAux = AccesoBD.registrarCopiaVisita(piVistaPadreSb, usuario.IdArea, lsNuevaVisita, Constantes.TipoCopia.SubVisita, lsFolio, chkSubEntidad.Items(i).Value, chkSubEntidad.Items(i).Text)

                    If lsAux <> "" Then
                        lsVisitaGenerada &= "<li>" & lsAux & "</li>"
                        lbGenero = True
                    End If
                End If
            End If
        Next

        If lbGenero Then
            Mensaje = "Se ha registrado satisfactoriamente la nueva visita: <br /><br />" & lsVisitaGenerada & "</ul>"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionSubvisitas();", True)
        Else
            Mensaje = "No se pudo registrar la visita. <br /><br />"
            imgAviso.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AvisoCCF();", True)
        End If
    End Sub


    Protected Sub btnConfirmaReg_Click(sender As Object, e As EventArgs)
        Response.Redirect("../Visita/Bandeja.aspx?sb=" & piVistaPadreSb)
    End Sub

    Public Sub HabilitaControles()
        chkSubEntidad.Enabled = True
        chkSubVisitas.Enabled = True
    End Sub
End Class