Imports System.Data
Imports System.Web.Configuration
Imports Clases
Imports AccesoDatos
Imports System.IO
Imports LogicaNegocioSICOD
Imports SICOD.Generales
Imports System.Net

Public Class VincularDocumentosSICOD
    Inherits System.Web.UI.Page

    'Indica el Total de Registros de la Consulta
    Private SqlTotalDatosInicio As String = "SELECT  Count(*) TotalRegistros FROM (  "
    Private SqlTotalDatosFin As String = " )  as UnionedResultTotal  "

    'Indica el Tope de la Consulta
    Private SqlTopInicio As String = "SELECT TOP " + System.Web.Configuration.WebConfigurationManager.AppSettings("TopConsulta") + " * FROM (  "
    Private SqlTopFin As String = " )  as UnionedResult ORDER BY ID_FOLIO "

    Private Const Modulo As String = "BANDEJA DE ENTRADA"

#Region "Propiedades de la página"

    Public Property USUARIO() As String
        Get
            Return ViewState("Usuario").ToString
        End Get
        Set(ByVal value As String)
            ViewState("Usuario") = value
        End Set
    End Property

    Public Property ID_UNIDAD_ADM() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM") = value
        End Set
    End Property

    Public Property ID_ANIO() As Integer
        Get
            Return CInt(ViewState("ID_ANIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO") = value
        End Set
    End Property

    Public Property ID_TIPO_DOCUMENTO() As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOCUMENTO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOCUMENTO") = value
        End Set
    End Property

    Public Property I_OFICIO_CONSECUTIVO() As Integer
        Get
            Return CInt(ViewState("I_OFICIO_CONSECUTIVO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("I_OFICIO_CONSECUTIVO") = value
        End Set
    End Property

    Public Property ID_EXPEDIENTE() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE") = value
        End Set
    End Property
    Public Property ID_FOLIO_TEMP() As Integer
        Get
            Return CInt(ViewState("ID_FOLIO_TEMP"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_FOLIO_TEMP") = value
        End Set
    End Property

    Public Property NOMBRE_ARCHIVO_TEMP() As String
        Get
            Return ViewState("NOMBRE_ARCHIVO_TEMP").ToString
        End Get
        Set(ByVal value As String)
            ViewState("NOMBRE_ARCHIVO_TEMP") = value
        End Set
    End Property

    Public Property NUMERO_OFICIO As String
        Get
            Return ViewState("NUMERO_OFICIO").ToString
        End Get
        Set(ByVal value As String)
            ViewState("NUMERO_OFICIO") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '-----------------------------------------------
        ' Verificar Sesión y Perfil de usuario
        '-----------------------------------------------
        verificaSesion()
        verificaPerfil()

        If Not IsPostBack Then
            If Session("Usuario") Is Nothing Then logOut()
            USUARIO = Session("Usuario").ToString

            '-----------------------------------------------
            ' Trae variables de sesión de bandeja y establece propiedades de la página en el Viewstate
            '-----------------------------------------------
            ID_UNIDAD_ADM = CInt(Session("ID_UNIDAD_ADM"))
            ID_ANIO = CInt(Session("ID_ANIO"))
            ID_TIPO_DOCUMENTO = CInt(Session("ID_TIPO_DOCUMENTO"))
            I_OFICIO_CONSECUTIVO = CInt(Session("I_OFICIO_CONSECUTIVO"))
            ID_EXPEDIENTE = CInt(Session("ID_EXPEDIENTE"))
            NUMERO_OFICIO = Session("NUMERO_OFICIO").ToString

            lblTitulo.Text = "Asociar documentos a <strong>" & NUMERO_OFICIO & "</strong>"
        End If
    End Sub

    Protected Sub BtnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnFiltrar.Click
        Try
            If FiltrosAsociar.GetWhereQuery().StartsWith("-1,") Then
                EventLogWriter.EscribeEntrada("Error al consultar " & FiltrosAsociar.GetWhereQuery().Substring(3).ToString(), EventLogEntryType.Error)
            Else
                If Session("filtros") = "" Then
                    Dim sqlGrid As String = ActualizaGrid(True)
                    If GridPrincipal.Rows.Count < 1 Then
                        Throw New ApplicationException("No hay Registros")
                        btnAsociar.Visible = False
                    Else
                        btnAsociar.Visible = True
                    End If

                Else

                    If Session("fec_Rec2") <> "" Then Throw New ApplicationException("La Fecha Final de Recepción no puede ser menor a la Fecha Inicio de Recepción. ")


                    If Session("fec_docto") <> "" Then Throw New ApplicationException("Debe seleccionar la Fecha Documento")


                    If Session("fec_docto1") <> "" Then Throw New ApplicationException("La Fecha Documento debe ser válida con el formato DD/MM/AAAA.")


                    If Session("fec_Rec") <> "" And Session("fec_Rec1") <> "" Then
                        Throw New ApplicationException("Debe seleccionar la Fecha Recepción Doc.")

                    Else
                        If Session("fec_Rec") <> "" Then
                            Throw New ApplicationException("Debe seleccionar la Fecha Inicial Recepción Doc.")
                        Else
                            If Session("fec_Rec3") <> "" Then Throw New ApplicationException("La Fecha Inicial de Recepción Doc. debe ser válida con el formato DD/MM/AAAA.")
                        End If

                        If Session("fec_Rec1") <> "" Then
                            Throw New ApplicationException("Debe seleccionar la Fecha Final Recepción Doc.")
                        Else
                            If Session("fec_Rec4") <> "" Then
                                Throw New ApplicationException("La Fecha Final de Recepción Doc. debe ser válida con el formato DD/MM/AAAA.")
                            End If
                        End If

                    End If

                    If Session("Tdocto") <> "" Then Throw New ApplicationException("Debe seleccionar el Tipo de Documento")

                    If Session("fArea") <> "" Then Throw New ApplicationException("Debe seleccionar el Área")

                    If Session("destinatario") <> "" Then Throw New ApplicationException("Debe seleccionar el Destinatario")

                    If Session("rdbRecib") <> "" Then Throw New ApplicationException("Debe seleccionar Recibido o No Recibido")

                    If Session("refere1") <> "" Then Throw New ApplicationException("Debe ingresar una referencia")

                    If Session("refere") <> "" Then Throw New ApplicationException("La referencia debe ser valida")

                    'by guillermo.banda; Nuevos Filtros
                    If Session("folio") <> "" Then Throw New ApplicationException("Debe Ingresar el Numero de Folio")

                    If Session("Oficio") <> "" Then Throw New ApplicationException("Debe Ingresar el Oficio")

                    If Session("Remitente") <> "" Then Throw New ApplicationException("Debe Ingresar el Remitente")

                    If Session("Asunto") <> "" Then Throw New ApplicationException("Debe Ingresar el Asunto")

                    If Session("Nombre") <> "" Then Throw New ApplicationException("Debe Ingresar el Nombre")

                    If Session("FechaRegistro") <> "" Then Throw New ApplicationException("Debe Ingresar la Fecha de Registro")

                    If Session("FechaLimite") <> "" Then Throw New ApplicationException("Debe Ingresar la Fecha Limite")

                    If Session("AtendidaStatus") = "AtendidaStatus" Then Throw New ApplicationException("Debe seleccionar un estatus de Atención")

                End If


            End If
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Formato de Fechas incorrecto " & ex.ToString(), EventLogEntryType.Error)
        Finally
            Session("filtros") = Nothing
            Session("fec_docto") = Nothing
            Session("fec_Rec") = Nothing
            Session("Tdocto") = Nothing
            Session("fArea") = Nothing
            Session("destinatario") = Nothing
            Session("rdbRecib") = Nothing
            Session("refere1") = Nothing
            Session("refere") = Nothing
            'by guillermo.banda; Nuevos Filtros
            Session("folio") = Nothing
            Session("Oficio") = Nothing
            Session("Remitente") = Nothing
            Session("AtendidaStatus") = Nothing


        End Try
    End Sub

    Public Function ActualizaGrid(ByVal usarFiltro As Boolean) As String

        Dim usuario As String = Session("Usuario")
        Dim Con As Conexion = Nothing
        Dim ds As New DataSet
        Dim dsA As New DataSet
        Dim liARCH As List(Of ARCH) = New List(Of ARCH)()
        Dim y As Int16 = 0
        Dim leerShare As New LeerSharep
        'Arma SQL
        Dim sqlGrid As String = String.Empty
        Dim sqlGrid1 As String = String.Empty
        Dim Sesion As Seguridad = Nothing

        sqlGrid = Clases.BandejaE.ConsultaInicialRelacionOficios

        If Session("Perfil").ToString() = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioAdmon") Then

            'sqlGrid &= " WHERE DESTINATARIO IN ('O', 'C') "
            sqlGrid &= " WHERE DESTINATARIO = 'O' "

        ElseIf Session("Perfil").ToString() = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioJArea") Then

            'sqlGrid &= " WHERE DESTINATARIO IN ('O', 'C') AND ID_UNIDAD_ADM = " & Session("UnidadAdm").ToString()
            sqlGrid &= " WHERE DESTINATARIO = 'O' AND ID_UNIDAD_ADM = " & Session("UnidadAdm").ToString()

        ElseIf Session("Perfil").ToString() = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioAsistente") Then

            'sqlGrid &= " WHERE DESTINATARIO IN ('O', 'C') AND ID_UNIDAD_ADM = " & Session("UnidadAdm").ToString()
            sqlGrid &= " WHERE DESTINATARIO = 'O' AND ID_UNIDAD_ADM = " & Session("UnidadAdm").ToString()

        Else

            'sqlGrid &= " WHERE DESTINATARIO IN ('O', 'C') AND USUARIO = '" & usuario & "' "
            sqlGrid &= " WHERE DESTINATARIO = 'O' AND USUARIO = '" & usuario & "' "

        End If

        sqlGrid &= FiltrosAsociar.GetWhereQuery()
        ''sqlGrid &= " AND ID_FOLIO NOT IN (SELECT ID_FOLIO FROM BDA_R_EXPEDIENTE_INFO_DOC WHERE VIG_FLAG = 1) "

        Try
            Con = New Conexion()
            Sesion = New Seguridad

            ''Agrega las cadenas de Top Registros y Total de Registros
            SqlTopInicio &= sqlGrid & SqlTopFin & " "
            SqlTotalDatosInicio &= sqlGrid & SqlTotalDatosFin

            SqlTopInicio &= SqlTotalDatosInicio


            Sesion.BitacoraInicia("Filtro de búsqueda ", Con)

            ds = Con.Datos(SqlTopInicio, True)
            Sesion.BitacoraFinalizaTransaccion(True)
            Sesion.BitacoraFinaliza(True)

            Dim i As Int16 = 0
            Dim x As Int16 = 0

            ds.Tables(0).Columns.Add(New DataColumn("ANEXO", System.Type.GetType("System.String")))
            ds.Tables(0).Columns.Add(New DataColumn("NOM_ARCH", System.Type.GetType("System.String")))
            ds.Tables(0).Columns.Add(New DataColumn("NOMBRE_ARCHIVO", System.Type.GetType("System.String")))
            ds.Tables(0).Columns.Add(New DataColumn("ID_ARCHIVO", System.Type.GetType("System.String")))

            For a = 0 To ds.Tables(0).Rows.Count - 1
                Dim oad = New ARCH()
                oad.folio = ds.Tables(0).Rows(a).ItemArray(0) '.ToString()
                liARCH.Add(oad)
                oad = Nothing
            Next

            Try

                leerShare.LeerFoliosDoctos(liARCH, "")

            Catch ex As Exception
                EventLogWriter.EscribeEntrada("Error de SharePoint al consultar: " & ex.ToString(), EventLogEntryType.Error)
            End Try

            For Each row As DataRow In ds.Tables(0).Rows

                dsA = Con.Datos("select ID_ANEXO from BDA_R_DOC_ANEXO where Id_folio = " & row("ID_FOLIO").ToString().Trim() & "", False)
                If dsA.Tables(0).Rows.Count > 0 Then
                    x = 1
                Else
                    x = 0
                End If

                ds.Tables(0).Rows(i)("ANEXO") = x
                ds.AcceptChanges()
                If liARCH.Item(i).nomArch <> Nothing Then
                    If liARCH.Item(i).nomArch.ToString().Length > 30 Then

                        ID = ds.Tables(0).Rows(i)(0).ToString()
                        y = i
                        For cont = 0 To 1
                            If y <= ds.Tables(0).Rows.Count - 1 Then
                                If ID = ds.Tables(0).Rows(i)(0).ToString And ID = ds.Tables(0).Rows(y)(0).ToString() And i <> y Then
                                    ds.Tables(0).Rows(y)("NOM_ARCH") = liARCH.Item(y - 1).nomArch.ToString().Substring(0, 20)
                                    ds.AcceptChanges()
                                Else
                                    ds.Tables(0).Rows(i)("NOM_ARCH") = liARCH.Item(i).nomArch.ToString().Substring(0, 20)
                                    ds.AcceptChanges()
                                End If
                            End If
                            y += 1
                        Next
                    Else
                        ID = ds.Tables(0).Rows(i)(0).ToString()
                        y = i
                        For cont = 0 To 1
                            If y <= ds.Tables(0).Rows.Count - 1 Then
                                If ID = ds.Tables(0).Rows(i)(0).ToString And ID = ds.Tables(0).Rows(y)(0).ToString() And i <> y Then
                                    ds.Tables(0).Rows(y)("NOM_ARCH") = liARCH.Item(y - 1).nomArch.ToString()
                                    ds.AcceptChanges()

                                Else
                                    ds.Tables(0).Rows(i)("NOM_ARCH") = liARCH.Item(i).nomArch.ToString()
                                    ds.AcceptChanges()
                                End If
                            End If
                            y += 1
                        Next

                    End If
                    y = i
                    For cont = 0 To 1
                        If y <= ds.Tables(0).Rows.Count - 1 Then
                            If ID = ds.Tables(0).Rows(i)(0).ToString And ID = ds.Tables(0).Rows(y)(0).ToString() And i <> y Then
                                ds.Tables(0).Rows(y)("NOMBRE_ARCHIVO") = liARCH.Item(y - 1).nomArch.ToString()
                                ds.AcceptChanges()
                                ds.Tables(0).Rows(y)("ID_ARCHIVO") = liARCH.Item(y - 1).idArch.ToString()
                                ds.AcceptChanges()
                            Else
                                ds.Tables(0).Rows(i)("NOMBRE_ARCHIVO") = liARCH.Item(i).nomArch.ToString()
                                ds.AcceptChanges()
                                ds.Tables(0).Rows(i)("ID_ARCHIVO") = liARCH.Item(i).idArch.ToString()
                                ds.AcceptChanges()
                            End If
                        End If
                        y += 1
                    Next

                End If
                i += 1
            Next

            'Obtiene el total de los registros
            Dim rowTotal As DataRow = ds.Tables(1).Rows(ds.Tables(1).Rows.Count - 1)
            ViewState("TotalRegistrosConsulta") = CType(rowTotal("TotalRegistros"), Int32)

            Session("Consulta_Asociados" & Modulo) = ds.Tables(0).DefaultView

            'Dim objDataset As New DataSet()
            'objDataset = AgregaFiltroEstatusAtendida(ds.Tables(0)).DataSet
            'ds = New DataSet()
            'ds = objDataset

            'Llena Grid
            GridPrincipal.PageIndex = 0
            GridPrincipal.SelectedIndex = -1
            GridPrincipal.DataSource = ds.Tables(0).DefaultView
            GridPrincipal.DataBind()

            If GridPrincipal.Rows.Count = 0 Then
                GridPrincipal.Visible = False
                btnAsociar.Visible = False
                Throw New ApplicationException("No hay registros")
            Else
                GridPrincipal.Visible = True
                btnAsociar.Visible = True

                If CType(ViewState("TotalRegistrosConsulta").ToString(), Int32) > 600 Then
                    modalMensaje("La consulta contiene:  " & CType(ViewState("TotalRegistrosConsulta").ToString(), Int32) & " registros , y solo muestra 600 ")
                End If

            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "ActualizaGrid")
        Finally
            If Not Con Is Nothing Then
                Con.Cerrar()
            End If
            Con = Nothing
            Session("tusaurio") = Nothing
        End Try

        Imagen_procesando.Style.Add("display", "none")
        GRID.Style.Add("visibility", "visible")

        Return sqlGrid
    End Function

    Protected Sub btnAsociar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAsociar.Click
        Dim obj_R_Oficio_Doc As New Entities.BDA_R_OFICIO_DOC
        obj_R_Oficio_Doc.ID_ANIO = ID_ANIO
        obj_R_Oficio_Doc.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
        obj_R_Oficio_Doc.ID_AREA_OFICIO = ID_UNIDAD_ADM
        obj_R_Oficio_Doc.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO
        obj_R_Oficio_Doc.USUARIO_ASOCIO = USUARIO

        Dim Campos As String
        Dim Valores As String
        Dim Sesion As Seguridad = Nothing
        Dim Con As Conexion = Nothing
        Dim tran As Odbc.OdbcTransaction = Nothing
        Dim Dr As Odbc.OdbcDataReader = Nothing

        Try

            Sesion = New Seguridad

            Con = New Conexion()

            Sesion.BitacoraInicia("Vincula documentos SICOD ", Con)
            tran = Con.BeginTran()

            Dim fechaVencimientoValidacion As String = "NULL"
            Dim _dtO As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, _
                                                  ID_UNIDAD_ADM, _
                                                  ID_TIPO_DOCUMENTO, _
                                                  I_OFICIO_CONSECUTIVO)

            fechaVencimientoValidacion = "NULL"
            If Not IsDBNull(_dtO.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                fechaVencimientoValidacion = "'" & CType(_dtO.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
            End If

            '------------------------------------
            ' Revisar si oficio ya tiene docs relacionados, obtener expediente.
            '------------------------------------
            Dim id_expediente As Integer = BusinessRules.BDA_R_OFICIO_DOC.ConsultarExpedienteOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            If id_expediente = 0 Then
                '------------------------------------
                ' obtener máximo consecutivo
                '------------------------------------
                obj_R_Oficio_Doc.ID_EXPEDIENTE = BusinessRules.BDA_R_OFICIO_DOC.ObtenerMaximoConsecutivo()
            Else
                obj_R_Oficio_Doc.ID_EXPEDIENTE = id_expediente
            End If


            ''*********************************************************************************************
            '' VERIFICAMOS SI EL OFICIO TIENE EXPEDIENTE
            Dim Id_Expediente_Ext As Integer = 0
            Dim Id_Expediente_Ext_current As Integer = 0
            Dim Id_Expediente_Ext_old As Integer = 0

            Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByOFICIO(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ID_ANIO, I_OFICIO_CONSECUTIVO)
            If _dt.Rows.Count > 0 Then Id_Expediente_Ext = Convert.ToInt32(_dt.Rows(0)("ID_EXPEDIENTE").ToString())
            ''*********************************************************************************************



            For Each item As GridViewRow In GridPrincipal.Rows

                Dim chb As CheckBox = TryCast(item.FindControl("chkSeleccionar"), CheckBox)
                If chb.Checked Then
                    '---------------------------------------
                    ' Revisar que no pertenezca ya a otro expediente.
                    '---------------------------------------
                    obj_R_Oficio_Doc.ID_FOLIO = CInt(TryCast(item.Cells(3).FindControl("lblFolio"), Label).Text)
                    obj_R_Oficio_Doc.USUARIO = TryCast(item.Cells(12).FindControl("lblUsuario"), Label).Text
                    obj_R_Oficio_Doc.ORIGINAL_FLAG = item.Cells(14).Text

                    Dim dtFolioExiste As DataTable = BusinessRules.BDA_R_OFICIO_DOC.ConsultarExiste(obj_R_Oficio_Doc.ID_EXPEDIENTE,
                                                                                                    obj_R_Oficio_Doc.ID_FOLIO,
                                                                                                    obj_R_Oficio_Doc.USUARIO,
                                                                                                    obj_R_Oficio_Doc.ORIGINAL_FLAG)

                    If dtFolioExiste.Rows.Count > 0 Then
                        Throw New ApplicationException("Folio: " & obj_R_Oficio_Doc.ID_FOLIO.ToString() & " de Usuario: " & obj_R_Oficio_Doc.USUARIO & " ya está asignado a otro Oficio")
                    End If

                    Dim dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_INFO_DOC.GetByFOLIO(obj_R_Oficio_Doc.ID_FOLIO)
                    If dt.Rows.Count > 0 Then
                        Id_Expediente_Ext_current = Convert.ToInt32(dt.Rows(0)("ID_EXPEDIENTE").ToString())
                        If Id_Expediente_Ext_old = 0 Then Id_Expediente_Ext_old = Id_Expediente_Ext_current
                    End If

                    If Id_Expediente_Ext_old <> Id_Expediente_Ext_current Then
                        Throw New ApplicationException("Los Folios seleccionados pertenecen a expedientes diferentes ")
                    End If

                End If
            Next

            If Id_Expediente_Ext > 0 And Id_Expediente_Ext_current > 0 And Id_Expediente_Ext <> Id_Expediente_Ext_current Then
                Throw New ApplicationException("El oficio y los folios pertenecen a expedientes diferentes ")
            ElseIf Id_Expediente_Ext = 0 And Id_Expediente_Ext_current > 0 Then

                Id_Expediente_Ext = Id_Expediente_Ext_current
                '' guardamos oficio en el expediente
                Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                Valores = Id_Expediente_Ext.ToString & "," & ID_UNIDAD_ADM.ToString & "," & ID_TIPO_DOCUMENTO.ToString & "," & ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If


                'GUARDAMOS EN BITACORA PARA OFICIO
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = ID_UNIDAD_ADM.ToString & "," & _
                    ID_TIPO_DOCUMENTO.ToString & "," & _
                    ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ",'" & _
                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & Id_Expediente_Ext.ToString() & "'," & fechaVencimientoValidacion

                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If


            ElseIf Id_Expediente_Ext = 0 Then
                '' creamos nuevo expediente (nadie tiene expediente)
                Dr = Con.Consulta("SELECT ISNULL(MAX(ID_EXPEDIENTE),0)+1 AS ID_EXPEDIENTE FROM " & Conexion.Owner & "BDA_C_EXPEDIENTE", tran)
                Dr.Read()
                Id_Expediente_Ext = Convert.ToInt32(Dr.Item("ID_EXPEDIENTE"))
                Dr.Close()

                Campos = "ID_EXPEDIENTE,DSC_EXPEDIENTE,USUARIO_CREACION,VIG_FLAG,FECH_INI_VIG"
                Valores = Id_Expediente_Ext.ToString & ", '', '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_C_EXPEDIENTE", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo crearse el expediente ")
                End If

                Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                Valores = Id_Expediente_Ext.ToString & "," & ID_UNIDAD_ADM.ToString & "," & ID_TIPO_DOCUMENTO.ToString & "," & ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If

                'GUARDAMOS EN BITACORA PARA OFICIO
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = ID_UNIDAD_ADM.ToString & "," & _
                    ID_TIPO_DOCUMENTO.ToString & "," & _
                    ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ",'" & _
                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & Id_Expediente_Ext.ToString() & "'," & fechaVencimientoValidacion

                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If

            End If

            For Each item As GridViewRow In GridPrincipal.Rows
                Dim chb As CheckBox = TryCast(item.FindControl("chkSeleccionar"), CheckBox)
                If chb.Checked Then
                    obj_R_Oficio_Doc.ID_FOLIO = CInt(TryCast(item.Cells(3).FindControl("lblFolio"), Label).Text)
                    obj_R_Oficio_Doc.USUARIO = TryCast(item.Cells(12).FindControl("lblUsuario"), Label).Text
                    obj_R_Oficio_Doc.ORIGINAL_FLAG = item.Cells(14).Text

                    BusinessRules.BDA_R_OFICIO_DOC.Upsert(obj_R_Oficio_Doc)


                    'If LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_INFO_DOC.GetByKey(Id_Expediente_Ext, obj_R_Oficio_Doc.ID_FOLIO).Rows.Count = 0 Then
                    If Not Con.BusquedaReg("BDA_R_EXPEDIENTE_INFO_DOC", "VIG_FLAG = 1 and ID_EXPEDIENTE = " & Id_Expediente_Ext.ToString() & _
                                    " AND ID_FOLIO = " & obj_R_Oficio_Doc.ID_FOLIO.ToString(), tran) Then

                        Campos = "ID_EXPEDIENTE,ID_FOLIO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                        Valores = Id_Expediente_Ext.ToString & ", " & obj_R_Oficio_Doc.ID_FOLIO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                        If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_INFO_DOC", Campos, Valores, tran) Then
                            Throw New ApplicationException("No pudo vincularse el folio al expediente ")
                        End If


                        Campos = "ID_FOLIO, USUARIO_SISTEMA, ID_MOVIMIENTO, DESCRIPCION, USUARIO_ORIGEN, USUARIO_DESTINO"
                        Valores = obj_R_Oficio_Doc.ID_FOLIO.ToString + ", '" & Session("Usuario") & "', 13, 'AGREGADO AL EXPEDIENTE Num. " & Id_Expediente_Ext.ToString() & "', '" & Session("Usuario") & "', '" & Session("Usuario") & "'"
                        If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA", Campos, Valores, tran) Then
                            Throw New ApplicationException("No pudo vincularse el folio al expediente ")
                        End If


                    End If

                    item.CssClass &= " related_row "

                End If
            Next

            tran.Commit()
            Sesion.BitacoraFinaliza(True)
            Sesion.BitacoraFinaliza(True)

            modalMensaje("Documentos asociados con éxito")
        Catch ex As ApplicationException
            tran.Rollback()
            Sesion.BitacoraFinaliza(False)
            Sesion.BitacoraFinaliza(False)
            modalMensaje(ex.Message)
        Catch ex As Exception
            tran.Rollback()
            Sesion.BitacoraFinaliza(False)
            Sesion.BitacoraFinaliza(False)
            EscribirError(ex, "btnAsociar_Click")
        Finally
            If Not Con Is Nothing Then
                Con.Cerrar()
                Con = Nothing
            End If

            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
    End Sub

#Region "ModalPopup"
    Private Sub modalMensaje(
                                ByVal mensaje As String, Optional ByVal PostBackCall As String = "",
                                    Optional ByVal Titulo As String = "ALERTA",
                                        Optional ByVal showCancelButton As Boolean = False,
                                            Optional ByVal AcceptButtonText As String = "Aceptar",
                                                Optional ByVal CancelButtonText As String = "Cancelar"
                                                                                                )

        lblErroresTitulo.Style.Add("display", "block")
        lblErroresTitulo.Text = Titulo
        lblErroresPopup.Text = "<ul><li>" & mensaje & "</li></ul>"
        lblErroresPopup.Style.Add("display", "block")
        lblModalPostBack.Text = PostBackCall
        BtnModalOk.Text = AcceptButtonText
        BtnCancelarModal.Visible = showCancelButton
        BtnCancelarModal.Text = CancelButtonText
        ModalPopupExtenderErrores.Show()
    End Sub
    Protected Sub BtnModalOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnModalOk.Click
        If lblModalPostBack.Text = "" Then

        End If
    End Sub
#End Region

#Region "Funciones llamadas desde página aspx"

    ''' <summary>
    ''' Agrega el Estatus en la tabla (Norma, Atendida,Vencida, etc.)
    ''' </summary>
    ''' <param name="objDataSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AgregaFiltroEstatusAtendida(ByVal objDataSet As DataTable) As DataTable
        Try

            Dim dif As Integer
            Dim i As Integer
            Dim fecha_inicial As String = String.Empty
            Dim fecha_final As String = String.Empty
            Dim fechaHoy As Date = DateTime.Now
            Dim esNegativo As Boolean = False
            Dim objDataView As New DataView()

            If Not objDataSet.Columns.Contains("ATENDIDA") Then objDataSet.Columns.Add(New DataColumn("ATENDIDA", System.Type.GetType("System.String")))
            If Not objDataSet.Columns.Contains("TOTAL_DIAS") Then objDataSet.Columns.Add(New DataColumn("TOTAL_DIAS", System.Type.GetType("System.String")))

            For Each Row In objDataSet.Rows
                Try

                    fecha_inicial = fechaHoy.ToString("dd/MM/yyyy")
                    fecha_final = DirectCast(Row("FECHA_LIMITE"), Date).ToString("dd/MM/yyyy")
                    dif = DateDiff(DateInterval.Day, CType(fecha_inicial, Date), CType(fecha_final, Date))

                    If dif < 0 Then
                        dif = dif * (-1)
                        esNegativo = True
                    End If

                    For i = 0 To dif - 1
                        If Weekday(System.DateTime.FromOADate(CDate(fecha_inicial).ToOADate + i), DayOfWeek.Monday) = 6 Or Weekday(System.DateTime.FromOADate(CDate(fecha_inicial).ToOADate + i), DayOfWeek.Monday) = 7 Then
                            dif = dif - 1
                        End If
                    Next i

                    If Row("ESTATUS_ATENDIDA").ToString() = "0" Then
                        Row("TOTAL_DIAS") = dif
                    End If
                Catch
                End Try

                If Row("ESTATUS_ATENDIDA").ToString() = "1" Then
                    Row("ATENDIDA") = "Atendido"
                ElseIf IsDBNull(Row("FECH_INICIO")) And Row("ESTATUS_ATENDIDA").ToString() = "0" Then
                    Row("ATENDIDA") = "Normal"
                ElseIf (fechaHoy < fecha_final) And dif > 5 Then
                    Row("ATENDIDA") = "Normal"
                ElseIf esNegativo And (fechaHoy >= fecha_final) Then
                    Row("ATENDIDA") = "Vencido"
                ElseIf (fechaHoy < fecha_final) Or dif <= 5 Then
                    Row("ATENDIDA") = "Por Vencer"
                End If

            Next

            If Session("AtendidaStatus") <> Nothing Then

                objDataView = New DataView(objDataSet)
                objDataView.RowFilter = "ATENDIDA  Like '%" + Session("AtendidaStatus").ToString() + "%'"
                Dim dtTabla As New DataTable()
                dtTabla = CType(objDataView.ToTable, DataTable)
                objDataSet = New DataTable()
                objDataSet = dtTabla
                ViewState("TotalRegistrosConsulta") = objDataSet.Rows.Count
            Else
                objDataSet.AcceptChanges()
            End If


            Return objDataSet

        Catch ex As Exception

            Return New DataTable()
        End Try
    End Function

    Public Function ImagenFechaLimite(ByVal Estatus_Atender As String)
        Try
            Dim imagenRegreso As String = ""

            If Estatus_Atender = "Atendido" Then
                imagenRegreso = "~/imagenes/ATENDIDO.png"
            ElseIf Estatus_Atender = "Normal" Then
                imagenRegreso = "~/imagenes/statusNormal.png"
            ElseIf Estatus_Atender = "Por Vencer" Then
                imagenRegreso = "~/imagenes/PREVENTIVO.png"
            ElseIf Estatus_Atender = "Vencido" Then
                imagenRegreso = "~/imagenes/VENCIDO.png"
            End If
            Return imagenRegreso
        Catch ex As Exception
        End Try
        Return ""

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Estatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ImagenEstatusCopia(ByVal Estatus As String) As String
        Try
            If Estatus = "O" Then
                Return "~/imagenes/original.gif"
            ElseIf Estatus = "C" Then
                Return "~/imagenes/copia.gif"
            End If
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion ImagenEstatus: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function


    ''' <summary>
    ''' Consulta el folio para identidicar si ya hay duplicados o asociados
    ''' </summary>
    ''' <param name="Folio_Asociado"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FolioAsociados(ByVal Folio_Asociado As String)
        Try
            Dim imagenRegreso As String = ""

            If Folio_Asociado = "0" Then
                imagenRegreso = "~/imagenes/Duplicado.png"
            ElseIf Folio_Asociado = "1" Then
                imagenRegreso = "~/imagenes/Asociado.png"
            End If

            Return imagenRegreso
        Catch ex As Exception
        End Try
        Return ""

    End Function

#End Region

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
            If Not Perfil.Autorizado("App_Oficios/Registro.aspx") Then
                logout = True
            End If

        Catch ex As Exception
            EscribirError(ex, "verificaPerfil")
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

    Private Sub logOut()
        If Request.Browser.EcmaScriptVersion.Major >= 1 Then
            Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
            Response.End()
        Else
            Response.Redirect("~/logout.aspx")
        End If
    End Sub

    Private Function verificaUsuario() As Boolean
        Return BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, USUARIO)
    End Function

#End Region

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Session("ID_ANIO") = ID_ANIO
        Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
        Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
        Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
        Session("ID_EXPEDIENTE") = ID_EXPEDIENTE
        Response.Redirect("~/App_Oficios/Seguimiento.aspx", False)
    End Sub

    Private Sub GridPrincipal_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridPrincipal.RowCommand

        Try
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            With GridPrincipal
                NOMBRE_ARCHIVO_TEMP = .DataKeys(index)("NOMBRE_ARCHIVO").ToString()
            End With

            Select Case e.CommandName
                Case "verPDFFolio"
                    If Not String.IsNullOrEmpty(NOMBRE_ARCHIVO_TEMP) Then
                        AbreArchivoLink(NOMBRE_ARCHIVO_TEMP)
                    End If
            End Select
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "GridPrincipal_RowCommand")
        End Try

    End Sub
    Protected Sub AbreArchivoLink(ByVal NombreArchivo As String)

        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim usuario As String
        Dim passwd As String
        Dim ServSharepoint As String
        Dim Dominio As String
        Dim Biblioteca As String
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim urlEncode As String = String.Empty
        Try

            Dim Archivo() As Byte = Nothing

            Dim filename As String = "attachment; filename=" & Server.UrlPathEncode(NombreArchivo)
            Try
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
                passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServer"), "webCONSAR")
                Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
                Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibrary"), "webCONSAR")

                cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)

                Dim Url As String = ServSharepoint & "/" & Biblioteca & "/" & NombreArchivo
                urlEncode = Server.UrlPathEncode(Url)
                Archivo = cliente.DownloadData(ResolveUrl(urlEncode))

            Catch ex As Exception
                Throw New ApplicationException("Hubo un error abriendo el documento")
            End Try

            If Not Archivo Is Nothing Then

                Dim dotPosicion As Integer = NombreArchivo.LastIndexOf(".")

                Dim tipo_arch As String = NombreArchivo.Substring(dotPosicion + 1)

                Select Case tipo_arch
                    Case "zip"
                        Response.ContentType = "application/x-zip-compressed"
                    Case "pdf"
                        Response.ContentType = "application/pdf"
                    Case "csv"
                        Response.ContentType = "text/csv"
                    Case "doc"
                        Response.ContentType = "application/doc"
                    Case "docx"
                        Response.ContentType = "application/docx"
                    Case "xls"
                        Response.ContentType = "application/xls"
                    Case "xlsx"
                        Response.ContentType = "application/xlsx"
                    Case "png"
                        Response.ContentType = "image/png"
                    Case "gif"
                        Response.ContentType = "image/gif"
                    Case "jpg"
                        Response.ContentType = "image/jpeg"
                    Case "jpeg"
                        Response.ContentType = "image/jpeg"
                    Case "txt"
                        Response.ContentType = "application/txt"
                    Case "ppt"
                        Response.ContentType = "application/vnd.ms-project"
                    Case "pptx"
                        Response.ContentType = "application/vnd.ms-project"
                    Case "bmp"
                        Response.ContentType = "image/bmp"
                    Case "tif"
                        Response.ContentType = "image/tiff"
                    Case "sbm"
                        Response.ContentType = "application/octet-stream"
                    Case Else
                        Response.ContentType = "application/octet-stream"
                End Select

                Response.AddHeader("content-disposition", filename)

                Response.BinaryWrite(Archivo)

                Response.End()
                '---------------------------------------------
                ' No usamos HttpContext.Current.ApplicationInstance.CompleteRequest()
                ' porque en archivos de texto (txt, csv, etc...) agregaba al final el código de la página.
                '---------------------------------------------
            End If
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoLink")
        End Try
    End Sub

    Private Sub GridPrincipal_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridPrincipal.RowDataBound
        e.Row.Cells(14).Visible = False
        If e.Row.RowIndex Mod 2 = 1 Then e.Row.CssClass = "tr_odd"
    End Sub
End Class