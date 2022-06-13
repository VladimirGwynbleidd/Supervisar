Imports Clases
Public Class Folio_s_Consecutivo
    Inherits System.Web.UI.Page
    Private Const FORFECHA As String = "yyyy/MM/dd HH:mm:ss"
    Private Const CVECATALOGO As String = "ID_INI_FOLIO"
    Private TABLA As String = Conexion.Owner & "BDA_VIEW_INI_FOLIOS"
    Private Const PAGINA As String = "FolioConsecutivo.aspx"
   
    ' Variables para la generacion de EventLog
    Private sModulo As String = ConfigurationSettings.AppSettings("EventLogModuloSecurity")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        verificaSesion()
        verificaPerfil()
        Dim Con As New Conexion()
        Dim Dr As Odbc.OdbcDataReader
        Dim SQL As String
        Dim Clave As String
        Dim Tipo_Doc As String
        Dim Area As String

        Try
            If Not IsPostBack Then
                Clave = Request.Item("Id")
                If Clave <> "-1" Then
                    'Edición del registro
                    'Llena los campos con los datos del registro


                    SQL = "SELECT * FROM " & TABLA & " WHERE " & CVECATALOGO & "=" & Clave & ""
                    Dr = Con.Consulta(SQL)
                    Dr.Read()
                    TxtClave.Text = Clave

                    Me.TextBox1.Text = Dr.Item("Anio").ToString
                    Me.TextBox2.Text = Dr.Item("Inicial").ToString
                    Area = Dr.Item("DSC_UNIDAD_ADM").ToString
                    Tipo_Doc = Dr.Item("DOCUMENTO").ToString

                    CargaComboOpciones("ID_TIPO_DOCUMENTO,T_TIPO_DOCUMENTO", "", Me.DropDownList2, "BDA_TIPO_DOCUMENTO")
                    CargaComboOpciones("ID_UNIDAD_ADM, DSC_UNIDAD_ADM", "", Me.DropDownList1, "BDA_C_UNIDAD_ADM")


                    EliminaRepetidosCombo(Me.DropDownList1)
                    EliminaRepetidosCombo(Me.DropDownList2)

                    Me.DropDownList2.SelectedValue = CInt(Dr.Item("TIPO_DOC").ToString)
                    Me.DropDownList1.SelectedValue = CInt(Dr.Item("ID_UNIDAD_ADM").ToString)


                    Dr.Close()
                Else

                    'Registro nuevo
                    'Obtiene la clave del registro más grande y le suma 1
                    SQL = "SELECT ISNULL(MAX(" & CVECATALOGO & "),0)+1 AS MAXIMO FROM " & TABLA & ""
                    Dr = Con.Consulta(SQL)
                    Dr.Read()
                    TxtClave.Text = Convert.ToString(Dr.Item("MAXIMO"))
                    Dr.Close()

                    CargaComboOpciones("ID_TIPO_DOCUMENTO,T_TIPO_DOCUMENTO", "", Me.DropDownList2, "BDA_TIPO_DOCUMENTO")
                    CargaComboOpciones("ID_UNIDAD_ADM,DSC_UNIDAD_ADM", "", Me.DropDownList1, "BDA_C_UNIDAD_ADM")
                    EliminaRepetidosCombo(Me.DropDownList1)
                    EliminaRepetidosCombo(Me.DropDownList2)
                    Me.TextBox1.Text = Now.Date.Year.ToString



                End If
            End If


        Catch ex As Exception
            catch_cone(ex, "Page_Load")
        Finally
            Con.Cerrar()
            Con = Nothing
        End Try
    End Sub

    Public Function TraeTipoDoc(ByVal Folio As Integer) As String
        Dim Doc As String
        Try
            Dim Con As New Conexion()
            Dim Dr As Odbc.OdbcDataReader
            Dr = Con.Consulta("SELECT T_TIPO_DOCUMENTO FROM BDA_TIPO_DOCUMENTO WHERE ID_TIPO_DOCUMENTO = " & Folio & "")
            Dr.Read()
            Doc = Dr.Item(0).ToString

        Catch
            Doc = Nothing
        End Try
        Return Doc
    End Function

    Public Function TraeUnidadAdm5(ByVal IDUNIDADADM As Integer, ByVal PREFIJO As String) As String
        Dim Doc As String
        Try
            Dim Con As New Conexion()
            Dim Dr As Odbc.OdbcDataReader
            Dr = Con.Consulta("SELECT DSC_UNIDAD_ADM FROM BDA_C_UNIDAD_ADM WHERE ID_UNIDAD_ADM = " & IDUNIDADADM & " AND PREFIJO_UNIDAD_ADM='" & PREFIJO & "' ")
            Dr.Read()
            Doc = Dr.Item(0).ToString

        Catch
            Doc = Nothing
        End Try
        Return Doc
    End Function
    Public Function TraeTipoDoc2() As Integer
        Dim Doc As String
        Try
            Dim Con As New Conexion()
            Dim Dr As Odbc.OdbcDataReader
            Dr = Con.Consulta("SELECT ID_TIPO_DOCUMENTO FROM BDA_TIPO_DOCUMENTO WHERE T_TIPO_DOCUMENTO = '" & Me.DropDownList2.Text & "'", Nothing)
            Dr.Read()
            Doc = Dr.Item(0).ToString

        Catch
            Doc = Nothing
        End Try
        Return Doc
    End Function

    Public Function TraeUnidadAdm() As Integer
        Dim Doc As Integer
        Try
            Dim Con As New Conexion()
            Dim Dr As Odbc.OdbcDataReader
            Dr = Con.Consulta("SELECT ID_UNIDAD_ADM FROM BDA_C_UNIDAD_ADM WHERE DSC_UNIDAD_ADM = '" & Me.DropDownList1.Text & "'")
            Dr.Read()
            Doc = CInt(Dr.Item(0).ToString)

        Catch
            Doc = Nothing
        End Try
        Return Doc
    End Function

    Public Function TraeUnidadAdm2() As String
        Dim Doc As String
        Try
            Dim Con As New Conexion()
            Dim Dr As Odbc.OdbcDataReader
            Dr = Con.Consulta("SELECT PREFIJO_UNIDAD_ADM FROM BDA_C_UNIDAD_ADM WHERE DSC_UNIDAD_ADM = '" & Me.DropDownList1.Text & "'")
            Dr.Read()
            Doc = Dr.Item(0).ToString

        Catch
            Doc = Nothing
        End Try
        Return Doc
    End Function

    Public Sub EliminaRepetidosCombo(ByVal oCombo As System.Web.UI.WebControls.DropDownList)
        Dim iElementos, iIndices As Int32
        Try
            For iElementos = 0 To oCombo.Items.Count - 2
                For iIndices = oCombo.Items.Count - 1 To iElementos + 1 Step -1
                    If oCombo.Items(iElementos).ToString = oCombo.Items(iIndices).ToString Then
                        oCombo.Items.RemoveAt(iIndices)
                    End If
                Next
            Next
        Catch
        End Try
    End Sub

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
            catch_cone(ex, "verificaSesion")
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
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub

    Private Sub verificaPerfil()
        Dim logout As Boolean = False
        Dim Perfil As Perfil = Nothing
        Try
            Perfil = New Perfil
            'Verifica que el usuario este autorizado para ver esta página
            If Not Perfil.Autorizado("Security/seg_c_areas.aspx") Then
                logout = True
            End If
            'Verifica que el usuario pueda agregar o modificar
            'If Not (Perfil.FuncionPerfil(18) Or Perfil.FuncionPerfil(19)) Then
            '    logout = True
            'End If
        Catch ex As Exception
            catch_cone(ex, "verificaPerfil")
        Finally
            If Not Perfil Is Nothing Then
                Perfil.CerrarCon()
                Perfil = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub
    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
        Response.Redirect("~\PaginaErrorSICOD.aspx")
    End Sub

    Protected Sub cmdAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAceptar.Click
       

        Try

            Dim funciones As New Fun_Generales
            Dim Vacios As Boolean = False
            Dim MsgVacios As String = ""

            'Verifica que el campo clave sea numérico
            If Not funciones.Numerico(TxtClave.Text) Then
                MsgVacios = "Campo Clave Inválido\n"
                Vacios = True
            End If
            If Not funciones.Numerico(TextBox2.Text) Then
                MsgVacios = "Campo Clave Inválido\n"
                Vacios = True
            End If
            If Vacios Then
                'Manda una alerta de que algún campo no es válido
                MsgBox1.ShowMessage(MsgVacios)
            Else
                If Request.Item("Id") = "-1" Then
                    ' OBTENGO EL AREA ADMINISTRATIVA

                   
                    MsgBox1.ShowConfirmation("Esta seguro que desea guardar el registro ?", "NUEVO", True, False)

                Else
                    MsgBox1.ShowConfirmation("Esta seguro que desea actualizar el registro ?", "ACTUALIZAR", True, False)
                End If
            End If




        Catch

        End Try




    End Sub
    Private Sub MsgBox1_YesChoosed(ByVal sender As Object, ByVal Key As String) Handles MsgBox1.YesChoosed
        Dim Con As New Conexion()
        Dim Sesion As Seguridad = Nothing
        Dim Funciones As New Fun_Generales
        Dim Exito As Boolean = True
        Dim Err As Integer
        Dim Tran As Odbc.OdbcTransaction

        Try
            Sesion = New Seguridad
            Exito = True
            If Exito Then
                Select Case Key
                    Case "NUEVO"
                        'Dim area As String
                        'Dim unidad_adm As Integer
                        'Dim tipo_doc As Integer

                        'unidad_adm = TraeUnidadAdm()
                        'area = TraeUnidadAdm2()
                        'tipo_doc = TraeTipoDoc2()


                        Tran = Con.BeginTran()

                        Dim Campos As String = " ID_UNIDAD_ADM, TIPO_DOC, ANIO,INICIAL,VIG_FLAG "
                        Dim Valores As String = Me.DropDownList1.SelectedValue & ", " & Me.DropDownList2.SelectedValue & ", '" & Me.TextBox1.Text & "', " & Me.TextBox2.Text & ",1"

                        'Inserta el registro en la BD
                        Exito = Con.Insertar("BDA_INI_FOLIOS", Campos, Valores, Tran)

                        If Exito Then
                            Sesion.bitacora(4, "SE AGREGO UNA NUEVA INICIALIZADOR " & TxtClave.Text)
                            Tran.Commit()
                        Else
                            Sesion.bitacora(4, "ERROR NUEVA INICIALIZADOR " & TxtClave.Text)
                            Tran.Rollback()
                        End If


                    Case "ACTUALIZAR"
                        'Dim area As String
                        'Dim unidad_adm As Integer
                        'Dim tipo_doc As Integer

                        'unidad_adm = TraeUnidadAdm()
                        'area = TraeUnidadAdm2()
                        'tipo_doc = TraeTipoDoc2()

                        Tran = Con.BeginTran()

                        Dim Valores As String = "ID_UNIDAD_ADM = " & Me.DropDownList1.SelectedValue & ",TIPO_DOC = " & Me.DropDownList2.SelectedValue & ",ANIO = '" & Me.TextBox1.Text & "',INICIAL = " & Me.TextBox2.Text & ""
                        Dim Condicion As String = "ID_INI_FOLIO = " & TxtClave.Text

                        Exito = Con.Actualizar("BDA_INI_FOLIOS", Valores, Condicion, Tran)

                        If Exito Then
                            Sesion.bitacora(5, "SE MODIFICO UN INI_FOLIO CON CODIGO " & TxtClave.Text)
                            Tran.Commit()
                        Else
                            Sesion.bitacora(5, "ERROR SE MODIFICO UN INI_FOLIO CON CODIGO " & TxtClave.Text)
                            Tran.Rollback()
                        End If

                End Select
            Else
                MsgBox1.ShowMessage("Error al guardar los datos, intente nuevamente.")
            End If

            Select Case Err
                Case 0
                    Exito = True
                Case 1
                    MsgBox1.ShowMessage("El registro ya existe.")
                    Exito = False
                Case 2
                    MsgBox1.ShowMessage("Error al guardar los datos, intente nuevamente.")
                    Exito = False
            End Select

            If Exito Then
                Response.Redirect(PAGINA, False)
            End If

        Catch ex As Exception
            catch_cone(ex, "msgbox1yeschoosed")
        Finally
            Sesion.CerrarCon()
            Sesion = Nothing
            Con.Cerrar()
            Con = Nothing

        End Try
    End Sub
    Private Sub CargaComboOpciones(ByVal sCampo As String, ByVal sValor As String, ByVal ddl As DropDownList, ByVal Tabla1 As String)
        Dim Con As New Conexion()
        Dim oDs As New DataSet
        Dim sSql As String = "SELECT " & sCampo & " FROM " & Conexion.Owner & Tabla1

        Try

            oDs = Con.Datos(sSql, False)
            If oDs.Tables(0).Rows.Count > 0 Then
                With ddl
                    .DataSource = oDs
                    .DataTextField = oDs.Tables(0).Columns.Item(1).ToString
                    .DataValueField = oDs.Tables(0).Columns.Item(0).ToString
                    .DataBind()
                    .Visible = True
                End With
                ddl.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Else
                With ddl
                    .Items.Insert(0, "No hay opciones disponibles")
                    .Visible = True
                End With
            End If

        Catch ex As Exception
            Dim lMensaje As String = ex.ToString()
        Finally

            Con.Cerrar()
            Con = Nothing
        End Try

    End Sub
End Class