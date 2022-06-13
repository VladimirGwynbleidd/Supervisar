Imports LogicaNegocioSICOD
Imports ControlErrores
Imports SICOD.Generales
Imports Clases

Public Class ConsultaHistorial
    Inherits System.Web.UI.Page

#Region "VARIABLES"
    Private Const Modulo As String = "HISTORIAL"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '-----------------------------------------------
        ' Cache, no permitir que guarde.
        '-----------------------------------------------
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

        '-----------------------------------------------
        ' Verificar Sesión
        '-----------------------------------------------
        verificaSesion()

        If Not IsPostBack Then

            Try

                Me.lblTitulo.Text &= Session("NUMERO_OFICIO").ToString()
                CargaDatos()

            Catch ex As Exception

                EscribirError(ex, "Cargar Historial")

            End Try


        End If

    End Sub

#Region "Verificar Sesión y perfil de Usuario"
    Private Sub verificaSesion()
        Dim logout As Boolean = False
        Dim Sesion As Seguridad = Nothing
        Try
            Sesion = New Seguridad
            'Verifica la sesion de usuario
            Select Case Sesion.ContinuarSesionAD()
                Case -1
                    logout = True
                Case 0, 3
                    logout = True
            End Select
        Catch ex As Exception
            EscribirError(ex, "verificaSesion")
        Finally
            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx", False)
            End If
        End If
    End Sub

    'Private Sub verificaPerfil()
    '    Dim logout As Boolean = False
    '    Dim Perfil As Perfil = Nothing
    '    Try
    '        Perfil = New Perfil
    '        'Verifica que el usuario este autorizado para ver esta página
    '        If Not Perfil.Autorizado("App_Oficios/ConsultaHistorial.aspx") Then
    '            logout = True
    '        End If

    '    Catch ex As Exception
    '        EscribirError(ex, "verificaPerfil")
    '    Finally
    '        If Not Perfil Is Nothing Then
    '            Perfil.CerrarCon()
    '            Perfil = Nothing
    '        End If
    '    End Try
    '    If logout Then
    '        If Request.Browser.EcmaScriptVersion.Major >= 1 Then
    '            Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
    '            Response.End()
    '        Else
    '            Response.Redirect("~/logout.aspx")
    '        End If
    '    End If
    'End Sub

    Private Sub logOut()
        If Request.Browser.EcmaScriptVersion.Major >= 1 Then
            Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
            Response.End()
        Else
            Response.Redirect("~/logout.aspx", False)
        End If
    End Sub

    'Private Function verificaUsuario() As Boolean

    '    If Session("PERFIL_ASISTENTE") Is Nothing Then Session("PERFIL_ASISTENTE") = BusinessRules.BDS_C_PERFIL.ConsultarPerfilPorNombre("ASISTENTE")

    '    If CInt(Session("perfil")) = CInt(Session("PERFIL_ASISTENTE")) Then
    '        Dim dtUsuarios As DataTable = BusinessRules.BDA_R_USUARIO_ASISTENTE.getUsuarios(USUARIO)
    '        Dim list As New List(Of String)
    '        If dtUsuarios.Rows.Count > 0 Then
    '            For Each row As DataRow In dtUsuarios.Rows
    '                list.Add(row("USUARIO").ToString())
    '            Next
    '            If BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, list) Then
    '                Return True
    '            End If
    '        End If
    '    End If

    '    Return BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, USUARIO)
    'End Function

#End Region

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click

        Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1", False)

    End Sub

    Private Sub CargaDatos()

        Dim dtConsulta As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_BITACORA.ConsultaMovimientoBitacoraOficio(CInt(Session("ID_UNIDAD_ADM")), _
                                                                                                                CInt(Session("ID_TIPO_DOCUMENTO")), _
                                                                                                                CInt(Session("ID_ANIO")), _
                                                                                                                CInt(Session("I_OFICIO_CONSECUTIVO")))

        Session("ConsultaHistoricoOficio" & Modulo) = dtConsulta
        grvHistorial.DataSource = dtConsulta
        grvHistorial.DataBind()

    End Sub

    Protected Sub BtnExportarInferior_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExportarInferior.Click
        Try

            Dim Reference As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)
            Reference.Add("SICOD OFICIOS")
            Reference.Add("OFICIO " & Session("NUMERO_OFICIO").ToString())
            Reference.Add(Session("Usuario"))
            Reference.Add(DateTime.Today.ToString("dd/MM/yyyy"))


            Dim HeaderColumns As System.Collections.Generic.List(Of String()) = New System.Collections.Generic.List(Of String())
            HeaderColumns.Add({"Usuario Registró", "USUARIO_SISTEMA_NOM"})
            HeaderColumns.Add({"Fecha Alta", "F_FECHA_ALTA"})
            HeaderColumns.Add({"Tipo de Movimiento", "MOVIMIENTO"})
            HeaderColumns.Add({"Descripción Movimiento", "DESCRIPCION"})
            HeaderColumns.Add({"Usuario Origen", "USUARIO_ORIGEN_NOM"})
            HeaderColumns.Add({"Usuario Destino", "USUARIO_DESTINO_NOM"})
            HeaderColumns.Add({"Fecha Movimiento", "FECH_MOVIMIENTO"})
            HeaderColumns.Add({"Fecha Vencimiento", "FECHA_VENCIMIENTO"})

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = "BITACORA OFICIOS"
            export.TableName = "Histórico de Movimientos"
            export.HeaderColor = "5D7370"
            export.HeaderForeColor = "FFFFFF"
            export.CellForeColor = "000000"
            export.ShowGridLines = False
            export.Reference = Reference
            export.HeaderColumns = HeaderColumns
            export.DataSource = CType(Session("ConsultaHistoricoOficio" & Modulo), DataTable)
            export.CreatePackage("BITACORA")




        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion BtnExportarInferior_Click(): " & ex.ToString(), EventLogEntryType.Error)
        End Try
    End Sub

    ''Protected Sub btnMuestrame_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMuestrame.Click

    ''    ctrShowBitacora.ucSB_TipoDocumento = ucShowBitacora.ucSB_TipoDocumentoBitacora.Salida_Oficio
    ''    ctrShowBitacora.ucSB_NumeroDocumento = Session("NUMERO_OFICIO").ToString()
    ''    ctrShowBitacora.ucSB_Datos = CType(Session("ConsultaHistoricoOficio" & Modulo), DataTable)
    ''    ctrShowBitacora.ucSB_AbrirBitacora()

    ''End Sub

End Class