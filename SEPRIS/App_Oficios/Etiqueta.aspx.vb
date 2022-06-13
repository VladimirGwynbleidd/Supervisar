Option Strict On
Option Explicit On
Imports LogicaNegocioSICOD
Imports ControlErrores
Imports SICOD.Generales
Imports Microsoft.Office.Interop
Imports System.Text
Imports System.IO
Imports DocumentFormat.OpenXml.Packaging

Public Class Etiqueta
    Inherits System.Web.UI.Page

    Public Enum TIPO_DOCUMENTO
        OFICIO_EXTERNO = 1
        DICTAMEN = 2
        ATENTA_NOTA = 3
        OFICIO_INTERNO = 4
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            '-----------------------------------------
            'Carga datos iniciales de la página
            '-----------------------------------------
            pnDestinatario.Visible = False
            pnFormatosEtiquetas.Visible = False


            CargarDatosIniciales()

        End If
    End Sub

    ''' <summary>
    ''' Funcion que carga los combobox para poder hacer la busqueda del oficio
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDatosIniciales()

        Try
            CargarCombo(ddlArea, LogicaNegocioSICOD.BusinessRules.BDS_C_AREA.GetAreaOficios(2), "DSC_COMPOSITE", "ID_UNIDAD_ADM")
            CargarCombo(ddlAnio, LogicaNegocioSICOD.BusinessRules.BDA_ANIO.ConsultarAnio, "CICLO", "CICLO")
            'CargarCombo(ddlEntidad, LogicaNegocioSICOD.BusinessRules.BDA_ENTIDAD.ConsultarEntidadesEtiqueta, "T_ENTIDAD_CORTO", "ID_ENTIDAD")
            txtFechaEnvio.Text() = CStr(Date.Today)

        Catch ex As Exception
            EscribirError(ex, "Cargar DatosIniciales")
        End Try

    End Sub

    ''' <summary>
    ''' boton cancelar la busqueda del oficio para etiquetas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        CargarDatosIniciales()
    End Sub

    ''' <summary>
    ''' boton abrir: consulta un oficio y muestra los datos del destinatario de ese oficio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAbrir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbrir.Click

        Try

            If Not IsNumeric(txtOficioConsecutivo.Text) Then Throw New ApplicationException("Debe introducir números en Número Consecutivo")

            If (BusinessRules.BDA_OFICIO.ConsultarOficioExistente(CInt(ddlAnio.SelectedValue), CInt(ddlArea.SelectedValue), TIPO_DOCUMENTO.OFICIO_EXTERNO, CInt(txtOficioConsecutivo.Text)) = 1) Then

                Dim data As DataTable = BusinessRules.BDA_ETIQUETA.ConsultarDatosEtiqueta(CInt(ddlAnio.SelectedValue), CInt(ddlArea.SelectedValue), TIPO_DOCUMENTO.OFICIO_EXTERNO, CInt(txtOficioConsecutivo.Text))

                If data.Rows.Count = 0 Then
                    Session("Datos_Etiqueta") = Nothing
                    Throw New ApplicationException("No se encontró Oficio")
                Else

                    CargarEtiqueta(data)
                    Session("Datos_Etiqueta") = data
                End If
            Else
                Throw New ApplicationException("El oficio no existe")
            End If
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "btnAbrir_Click")
        End Try

    End Sub

    ''' <summary>
    ''' Permite ver a los destinatarios ya en formato para etiqueta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDatosFormatoEtiqueta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDatosFormatoEtiqueta.Click

        Dim dtDatosSesion As DataTable = CType(Session("Datos_Etiqueta"), DataTable)
        dtDatosSesion.Rows.Clear()

        Try
            If chkDestinatarios.Visible Then

                AgregarRow(dtDatosSesion)

            End If

            pnDestinatario.Visible = False
            pnlEtiqueta.Visible = False
            pnFormatosEtiquetas.Visible = True

            gwEtiquetasFormatos.DataSource = dtDatosSesion
            gwEtiquetasFormatos.DataBind()

        Catch ex As Exception
            EscribirError(ex, "Ver formato y etiqueta")
        End Try

    End Sub

    Protected Sub btnCancelarDestinatario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelarDestinatario.Click
        pnDestinatario.Visible = False
        pnlEtiqueta.Visible = True
        CargarDatosIniciales()
    End Sub



    ''' <summary>
    ''' Metodo para llenar con los datos de la consulta los controles necesarios
    ''' </summary>
    ''' <param name="dtEtiqueta"></param>
    ''' <remarks></remarks>
    Private Sub CargarEtiqueta(ByVal dtEtiqueta As DataTable)

        Try

            If dtEtiqueta.Rows.Count > 0 Then

                textOficio.Text() = dtEtiqueta.Rows(0)("T_OFICIO_NUMERO").ToString()
                textDomicilio.Text() = dtEtiqueta.Rows(0)("T_DIRECCION").ToString()
                textDomicilio.Enabled = False
                textCP.Text() = dtEtiqueta.Rows(0)("T_CP").ToString()
                textCP.Enabled = False
                textColonia.Text() = dtEtiqueta.Rows(0)("T_COLONIA").ToString()
                textColonia.Enabled = False


                'Generales.CargarListBox(lstPersonalEntidad, dtEtiqueta, "NOMBRE_PERSONA", "ID_PERSONA")
                chkDestinatarios.DataSource = dtEtiqueta
                chkDestinatarios.DataTextField = "NOMBRE_PERSONA"
                chkDestinatarios.DataValueField = "ID_PERSONA"
                chkDestinatarios.DataBind()

                chkDestinatarios.Items(0).Selected = True


                'lstPersonalEntidad.SelectedValue = CStr(dtEtiqueta.Rows(0)("ID_PERSONA"))
                ddlEntidad.SelectedValue = CStr(dtEtiqueta.Rows(0)("ID_ENTIDAD"))


                CargarCombo(ddlEstado, LogicaNegocioSICOD.BusinessRules.BDA_ESTADO.ConsultaEstadoOficio(CInt(dtEtiqueta.Rows(0)("ID_ESTADO").ToString())), "T_ESTADO", "ID_ESTADO")

                If (CInt(dtEtiqueta.Rows(0)("ID_ESTADO")) = 0) Then
                    ddlEstado.SelectedValue = CStr(-1)
                    ddlEstado.Enabled = False

                    CargarCombo(ddlPoblacion, LogicaNegocioSICOD.BusinessRules.BDA_POBLACION.ConsultarPoblacionEstado(CInt(dtEtiqueta.Rows(0)("ID_ESTADO").ToString())), "T_POBLACION", "ID_POBLACION")
                    ddlPoblacion.SelectedValue = CStr(-1)
                    ddlPoblacion.Enabled = False
                Else
                    ddlEstado.SelectedValue = CStr(dtEtiqueta.Rows(0)("ID_ESTADO"))
                    ddlEstado.Enabled = False

                    CargarCombo(ddlPoblacion, LogicaNegocioSICOD.BusinessRules.BDA_POBLACION.ConsultarPoblacionEstado(CInt(dtEtiqueta.Rows(0)("ID_ESTADO").ToString())), "T_POBLACION", "ID_POBLACION")
                    ddlPoblacion.SelectedValue = CStr(dtEtiqueta.Rows(0)("ID_POBLACION"))
                    ddlPoblacion.Enabled = False

                End If

                pnlEtiqueta.Visible = False
                pnDestinatario.Visible = True

            Else
                'Aqui valida cuando no tiene datos mandar mensaje modal
                Throw New ApplicationException("No se tienen datos disponibles para este oficio")
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "Consulta de datos para etiqueta")
        End Try
    End Sub

    ''' <summary>
    ''' Agrega un renglon con datos del destinatario
    ''' </summary>
    ''' <param name="dtEtiqueta"></param>
    ''' <remarks></remarks>
    Private Sub AgregarRow(ByVal dtEtiqueta As DataTable)

        Try

            For Each lstDestinatario As ListItem In chkDestinatarios.Items

                If lstDestinatario.Selected Then

                    Dim dr As DataRow = dtEtiqueta.NewRow()

                    dr.Item("T_OFICIO_NUMERO") = textOficio.Text()
                    dr.Item("T_DIRECCION") = textDomicilio.Text()
                    dr.Item("T_CP") = textCP.Text()
                    dr.Item("T_COLONIA") = textColonia.Text()
                    dr.Item("T_ENTIDAD_CORTO") = ddlEntidad.SelectedItem.Text()
                    dr.Item("NOMBRE_PERSONA") = lstDestinatario.Text  'lstPersonalEntidad.SelectedItem.Text()
                    dr.Item("ID_PERSONA") = CType(lstDestinatario.Value, Integer)
                    dr.Item("T_ESTADO") = ddlEstado.SelectedItem.Text()
                    dr.Item("ID_ESTADO") = CType(ddlEstado.SelectedValue.ToString, Integer)
                    dr.Item("T_POBLACION") = ddlPoblacion.SelectedItem.Text()
                    dr.Item("ID_POBLACION") = CType(ddlPoblacion.SelectedValue.ToString, Integer)

                    dtEtiqueta.Rows.Add(dr)

                End If

            Next

        Catch ex As Exception
            EscribirError(ex, "Agregar un nuevo row para etiqueta")
        End Try
    End Sub

    Public Function CuentaSeleccionado() As Integer
        Dim contador As Integer = 0
        Dim chkSeleccion As CheckBox
        For Each dataItem As DataGridItem In gwEtiquetasFormatos.Items
            chkSeleccion = CType(dataItem.FindControl("chkSeleccion"), CheckBox)
            If chkSeleccion.Checked Then
                contador = contador + 1
            End If

        Next
        Return contador
    End Function

    ''' <summary>
    ''' Muestra cuantos destinatarios fueron seleccionados para ser impresos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEtiquetas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEtiquetas.Click
        Dim chkSeleccion As CheckBox
        Dim position As Integer = 1
        Dim hasSelected As Boolean = False

        Try

            Dim plantilla As String = Server.MapPath("~/Plantillas/Plantilla_Etiquetas.docx")
            Dim randomClass As Random = New Random()
            Dim ruta As String = Path.GetTempPath.ToString() & "__" & Format(randomClass.Next(10000), "00000") & "_etiquetas.docx"

            Try
                If File.Exists(ruta) Then File.Delete(ruta)

            Catch ex As Exception
                modalMensaje("Error abriendo documento temporal")
            End Try

            File.Copy(plantilla, ruta, True)

            Dim oDoc As WordprocessingDocument
            oDoc = WordprocessingDocument.Open(ruta, True)

            Dim values As New Dictionary(Of String, String)
            values("txt_Etiqueta") = String.Empty

            Dim tRow As New TableRow
            For Each dataItem As DataGridItem In gwEtiquetasFormatos.Items

                chkSeleccion = CType(dataItem.FindControl("chkSeleccion"), CheckBox)

                If chkSeleccion.Checked Then
                    hasSelected = True

                    values("txt_Etiqueta") &= Trim(dataItem.Cells(0).Text.Replace("&nbsp;", "")) & "\n"
                    values("txt_Etiqueta") &= Trim(dataItem.Cells(1).Text.Replace("&nbsp;", "")) & "\n"
                    values("txt_Etiqueta") &= Trim(dataItem.Cells(2).Text.Replace("&nbsp;", "")) & "\n"
                    values("txt_Etiqueta") &= Trim(dataItem.Cells(3).Text.Replace("&nbsp;", "")) & " " & Trim(dataItem.Cells(4).Text.Replace("&nbsp;", "")) & "\n"
                    values("txt_Etiqueta") &= Trim(dataItem.Cells(5).Text.Replace("&nbsp;", "")) & " " & Trim(dataItem.Cells(6).Text.Replace("&nbsp;", ""))

                End If

            Next

            If Not hasSelected Then Throw New ApplicationException("Debe seleccionar un registro")

            Dim objMainDoc As DocumentFormat.OpenXml.OpenXmlElement
            objMainDoc = oDoc.MainDocumentPart.Document()

            loopElementosXML(values, objMainDoc)

            '---------------------------------------
            'Guarda parte del documento
            '---------------------------------------

            oDoc.MainDocumentPart.Document.Save()
            oDoc.Close()
            oDoc.Dispose()
            Threading.Thread.Sleep(300)

            AbreArchivoFileSystem(ruta)

            Try
                If File.Exists(ruta) Then File.Delete(ruta)

            Catch ex As Exception
                '------------------------------------------
                ' Excepción vacía.
                '------------------------------------------
            End Try

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "Error al pasar datos a formato etiquetas")
        End Try
    End Sub

    Private Sub AbreArchivoFileSystem(ByVal ruta As String)

        Dim filename As String = String.Empty
        Dim Archivo() As Byte = Nothing
        Dim cliente As System.Net.WebClient = New System.Net.WebClient

        Try

            filename = "attachment; filename=" & Server.UrlPathEncode(ruta)
            Archivo = cliente.DownloadData(ruta)
            Dim tipo_arch As String = Path.GetExtension(ruta)

            Select Case tipo_arch
                Case ".zip"
                    Response.ContentType = "application/x-zip-compressed"
                Case ".pdf"
                    Response.ContentType = "application/pdf"
                Case ".csv"
                    Response.ContentType = "text/csv"
                Case ".doc"
                    Response.ContentType = "application/doc"
                Case ".docx"
                    Response.ContentType = "application/docx"
                Case ".xls"
                    Response.ContentType = "application/xls"
                Case ".xlsx"
                    Response.ContentType = "application/xlsx"
                Case ".png"
                    Response.ContentType = "image/png"
                Case "gif"
                    Response.ContentType = "image/gif"
                Case ".jpg"
                    Response.ContentType = "image/jpeg"
                Case ".jpeg"
                    Response.ContentType = "image/jpeg"
                Case ".txt"
                    Response.ContentType = "application/txt"
                Case ".ppt"
                    Response.ContentType = "application/vnd.ms-project"
                Case ".pptx"
                    Response.ContentType = "application/vnd.ms-project"
                Case ".bmp"
                    Response.ContentType = "image/bmp"
                Case ".tif"
                    Response.ContentType = "image/tiff"
                Case ".sbm", ".sbmx"
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

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún deja descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoFileSystem")
        End Try
    End Sub

    Private Sub loopElementosXML(ByVal values As Dictionary(Of String, String), ByVal element As DocumentFormat.OpenXml.OpenXmlElement)

        Dim list() As DocumentFormat.OpenXml.Wordprocessing.SimpleField
        list = element.Descendants(Of DocumentFormat.OpenXml.Wordprocessing.SimpleField)().ToArray

        For Each objField In list

            Dim strFieldName As String = GetFieldName(objField)
            If Not String.IsNullOrEmpty(strFieldName) Then
                'check if we have a value for this merge field
                If values.ContainsKey(strFieldName) AndAlso Not String.IsNullOrEmpty(values(strFieldName)) Then
                    'Find the XML placeholder
                    Dim strRunProp As String = String.Empty
                    For Each objRP As DocumentFormat.OpenXml.Wordprocessing.RunProperties In objField.Descendants(Of DocumentFormat.OpenXml.Wordprocessing.RunProperties)()
                        strRunProp = objRP.OuterXml
                        Exit For
                    Next
                    Dim objRun As New DocumentFormat.OpenXml.Wordprocessing.Run
                    If Not String.IsNullOrEmpty(strRunProp) Then
                        objRun.Append(New DocumentFormat.OpenXml.Wordprocessing.RunProperties(strRunProp))
                    End If

                    Dim split As String() = values(strFieldName).Split(New String() {"\n"}, System.StringSplitOptions.None)
                    Dim first As Boolean = True
                    For Each s As String In split
                        If Not first Then
                            objRun.Append(New DocumentFormat.OpenXml.Wordprocessing.Break)
                        End If
                        first = False
                        objRun.Append(New DocumentFormat.OpenXml.Wordprocessing.Text(s))
                    Next

                    'replace the merge field with the value
                    objField.Parent.ReplaceChild(Of DocumentFormat.OpenXml.Wordprocessing.SimpleField)(objRun, objField)
                End If
            End If
        Next
    End Sub


    'Finds merge fields into the XML document
    Private Function GetFieldName(ByVal pField As DocumentFormat.OpenXml.Wordprocessing.SimpleField) As String
        Dim attr As DocumentFormat.OpenXml.OpenXmlAttribute = pField.GetAttribute("instr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main")
        Dim strFieldname As String = String.Empty
        Dim instruction As String = attr.Value

        Dim instructionRegEx As Regex = _
            New Regex( _
                "^[\s]*MERGEFIELD[\s]+(?<name>[#\w]*){1}" + _
                    "[\s]*(\\\*[\s]+(?<Format>[\w]*){1})?" + _
                    "[\s]*(\\b[\s]+[""]?(?<PreText>[^\\]*){1})?" + _
                    "[\s]*(\\f[\s]+[""]?(?<PostText>[^\\]*){1})?", _
                RegexOptions.Compiled Or _
                RegexOptions.CultureInvariant Or _
                RegexOptions.ExplicitCapture Or _
                RegexOptions.IgnoreCase Or _
                RegexOptions.IgnorePatternWhitespace Or _
                RegexOptions.Singleline)

        If (Not String.IsNullOrEmpty(instruction)) Then
            Dim m As Match = instructionRegEx.Match(instruction)
            If (m.Success) Then
                strFieldname = m.Groups("name").ToString.Trim
            End If
        End If

        Return strFieldname
    End Function

    Protected Sub btnOficialia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOficialia.Click
        Try
            CreaDocumentoOficialia()
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "Error al crear documento de oficialia")
        End Try
    End Sub

    ''' <summary>
    ''' valida el estado y carga la lista de poblaciones dependiendo del estado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlEstado_SelectedIndexChanged1(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEstado.SelectedIndexChanged
        Try
            If ddlEstado.SelectedIndex > -1 Then
                CargarCombo(ddlPoblacion, LogicaNegocioSICOD.BusinessRules.BDA_POBLACION.ConsultarPoblacionEstado(CType(ddlEstado.SelectedValue, Integer)), "T_POBLACION", "ID_POBLACION")
            Else
                ddlPoblacion.Items.Insert(0, New ListItem("Indefinido", "-1"))
            End If
        Catch ex As Exception
            EscribirError(ex, "Cargar combo población")
        End Try
    End Sub

    Protected Sub btnCancelarEtiquetas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelarEtiquetas.Click

        Try
            If Not IsNumeric(txtOficioConsecutivo.Text) Then Throw New ApplicationException("Debe seleccionar un número en el Consecutivo del Oficio")
            pnFormatosEtiquetas.Visible = False
            pnDestinatario.Visible = True
            CargarEtiqueta(BusinessRules.BDA_ETIQUETA.ConsultarDatosEtiqueta(CInt(ddlAnio.SelectedValue), CInt(ddlArea.SelectedValue), TIPO_DOCUMENTO.OFICIO_EXTERNO, CInt(txtOficioConsecutivo.Text)))

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "btnCancelarEtiquetas_Click")
        End Try
    End Sub

    Protected Sub CreaDocumentoOficialia()

        Dim oDoc As WordprocessingDocument
        Dim ruta As String = String.Empty
        Dim chkSeleccion As CheckBox
        Dim hasSelected As Boolean = False
        Try

            Dim spath As String = Server.MapPath("~/Plantillas/DocumentoOficialia2.docx")

            Dim numDestinatario As Integer = 1

            Dim dtResponsableArea As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.ConsultarDirectorGeneralPorArea(CInt(ddlArea.SelectedValue))
            Dim dtUsuarioResponsable As DataTable = LogicaNegocioSICOD.BusinessRules.BDS_USUARIO.GetAllPorUsuario(Session("Usuario").ToString())


            Dim randomClass As Random = New Random()
            ruta = Path.GetTempPath.ToString() & "__" & Format(randomClass.Next(10000), "00000") & "_FormatoOficialia.docx"
            '--------------------------------------------------
            ' Atrapa el error si no se puede borrar el archivo temporal.
            '--------------------------------------------------
            Try
                If File.Exists(ruta) Then File.Delete(ruta)
            Catch ex As Exception
                '------------------------------------------
                ' Excepción vacía.
                '------------------------------------------
            End Try

            File.Copy(spath, ruta, True)

            Dim docText As String = Nothing
            oDoc = WordprocessingDocument.Open(ruta, True)
            Dim values As New Dictionary(Of String, String)
            For Each dataItem As DataGridItem In gwEtiquetasFormatos.Items

                chkSeleccion = CType(dataItem.FindControl("chkSeleccion"), CheckBox)
                If chkSeleccion.Checked Then
                    hasSelected = True
                    If (numDestinatario = 1) Then
                        values("txtFecha") = CType(txtFechaEnvio.Text, DateTime).ToLongDateString()
                        values("NumeroOficio") = textOficio.Text.Replace("&nbsp;", " ")
                        values("Area") = ddlArea.SelectedItem.Text.Replace("&nbsp;", " ")
                        values("Destinatario") = dataItem.Cells(2).Text.Replace("&nbsp;", " ")
                        values("Entidad") = dataItem.Cells(1).Text.Replace("&nbsp;", " ")
                        values("Direccion") = dataItem.Cells(3).Text & vbCrLf & " CP " & dataItem.Cells(5).Text & vbCrLf + dataItem.Cells(4).Text & " , " & dataItem.Cells(6).Text

                        If (dtResponsableArea.Rows.Count > 0) Then
                            values("ResponsableArea") = dtResponsableArea.Rows(0)("NOMBRE").ToString().Replace("&nbsp;", " ")
                        Else
                            values("ResponsableArea") = "No tiene responsable de área definido"
                        End If
                        values("Responsable") = dtUsuarioResponsable.Rows(0)("NOMBRECOMPLETO").ToString().Replace("&nbsp;", " ")

                        '--------------------------------------
                        ' Los demás campos en vacío
                        '--------------------------------------
                        values("txtFecha2") = " "
                        values("NumeroOficio2") = " "
                        values("Area2") = " "
                        values("Destinatario2") = " "
                        values("Entidad2") = " "
                        values("Direccion2") = " "
                        values("ResponsableArea2") = " "
                        values("Responsable2") = " "
                        numDestinatario = numDestinatario + 1

                    ElseIf (numDestinatario = 2) Then
                        values("txtFecha2") = CType(txtFechaEnvio.Text, DateTime).ToLongDateString()
                        values("NumeroOficio2") = textOficio.Text.ToString()

                        values("Area2") = ddlArea.SelectedItem.Text.Replace("&nbsp;", " ")
                        values("Destinatario2") = dataItem.Cells(2).Text.Replace("&nbsp;", " ")
                        values("Entidad2") = dataItem.Cells(1).Text.Replace("&nbsp;", " ")
                        values("Direccion2") = dataItem.Cells(3).Text + vbCrLf + " CP " + dataItem.Cells(5).Text + vbCrLf + dataItem.Cells(4).Text + " , " + dataItem.Cells(6).Text

                        If (dtResponsableArea.Rows.Count > 0) Then
                            values("ResponsableArea2") = dtResponsableArea.Rows(0)("NOMBRE").ToString().Replace("&nbsp;", " ")
                        Else
                            values("ResponsableArea2") = "No tiene responsable de área definido"
                        End If

                        values("Responsable2") = dtUsuarioResponsable.Rows(0)("NOMBRECOMPLETO").ToString().Replace("&nbsp;", " ")
                        numDestinatario = numDestinatario + 1
                    End If

                End If
            Next

            If Not hasSelected Then Throw New ApplicationException("Debe seleccionar un registro")

            Dim objMainDoc As DocumentFormat.OpenXml.OpenXmlElement
            objMainDoc = oDoc.MainDocumentPart.Document()

            loopElementosXML(values, objMainDoc)
            oDoc.MainDocumentPart.Document.Save()

            oDoc.Close()
            oDoc.Dispose()
            Threading.Thread.Sleep(300)

            AbreArchivoFileSystem(ruta)

            Try
                If File.Exists(ruta) Then File.Delete(ruta)

            Catch ex As Exception
                '------------------------------------------
                ' Excepción vacía.
                '------------------------------------------
            End Try

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "CreaDocumentoOficialia")
        End Try

    End Sub

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
        BtnModalOk.Text = AcceptButtonText
        ModalPopupExtenderErrores.Show()
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Try

            chkDestinatarios.Visible = True
            txtFechaEnvio.Text() = CStr(Date.Today)
            textDomicilio.Enabled = True
            textCP.Enabled = True
            textColonia.Enabled = True
            ddlEstado.Enabled = True
            ddlPoblacion.Enabled = True
            lblEntidad.Visible = True
            ddlEntidad.Visible = True
            ddlEntidad.Enabled = False
            Dim idEstado As Integer = CType(ddlEstado.SelectedValue, Integer)


            Dim dvDestinatarios As New DataView '= LogicaNegocioSICOD.BusinessRules.BDA_PERSONAL.ConsultarPersonalEtiquetaPorEntidad(CInt(ddlEntidad.SelectedValue)).DefaultView

            If dvDestinatarios.Count > 1 Then

                chkDestinatarios.DataSource = dvDestinatarios
                chkDestinatarios.DataTextField = "NOMBRE_PERSONA"
                chkDestinatarios.DataValueField = "ID_PERSONA"
                chkDestinatarios.DataBind()

            Else
                modalMensaje("No existe mas personal para la entidad.")

            End If

            CargarCombo(ddlEstado, LogicaNegocioSICOD.BusinessRules.BDA_ESTADO.ConsultarEstados(), "T_ESTADO", "ID_ESTADO")
            ddlEstado.SelectedValue = idEstado.ToString()

        Catch ex As Exception
            EscribirError(ex, "No se agrego el destinatario")
        End Try
    End Sub

End Class