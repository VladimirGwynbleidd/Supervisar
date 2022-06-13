Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Web.Configuration
Imports System.Text.RegularExpressions

Public Class Generales


    ''' <summary>
    ''' Método que regresa el Datasource para el fintro de Vigencia, campo DataText = "Vigencia", campo DataValue = "N_FLAG_VIG"
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function VigenciaDataSource() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Vigencia")
        data.Columns.Add("N_FLAG_VIG")

        data.Rows.Add({"Vigente", 1})
        data.Rows.Add({"No Vigente", 0})
        data.Rows.Add({"Todos", -1})

        Return data

    End Function

    Public Shared Function VigenciaBitDataSource() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Vigencia")
        data.Columns.Add("B_FLAG_VIG")

        data.Rows.Add({"Vigente", 1})
        data.Rows.Add({"No Vigente", 0})
        data.Rows.Add({"Todos", -1})

        Return data

    End Function

    Public Shared Function HeredaSiNo() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Hereda")
        data.Columns.Add("N_FLAG_HEREDA")

        data.Rows.Add({"Si", 1})
        data.Rows.Add({"No", 0})
        data.Rows.Add({"Todos", -1})

        Return data

    End Function

    Public Shared Function HeredaSiNoSbV() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Hereda")
        data.Columns.Add("N_FLAG_HEREDA_ENTRE_SBVISITA")

        data.Rows.Add({"Si", 1})
        data.Rows.Add({"No", 0})
        data.Rows.Add({"Todos", -1})

        Return data

    End Function

    Public Shared Function VigenciaBitDataSourceN() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Vigencia")
        data.Columns.Add("N_FLAG_VIG")

        data.Rows.Add({"Vigente", 1})
        data.Rows.Add({"No Vigente", 0})
        data.Rows.Add({"Todos", -1})

        Return data

    End Function

    ''' <summary>
    ''' Método que regresa el Datasource para el fintro de Vigencia, campo DataText = "Vigencia", campo DataValue = "B_FLAG_VIG"
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function VigenciaDataSourceBit() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Vigencia")
        data.Columns.Add("B_FLAG_VIG")

        data.Rows.Add({"Vigente", 1})
        data.Rows.Add({"No Vigente", 0})
        data.Rows.Add({"Todos", -1})

        Return data

    End Function

    Public Shared Function VigenciaBitDataSourceSU() As DataTable

        Dim data As New DataTable()

        data.Columns.Add("Vigencia")
        data.Columns.Add("B_FLAG_VIG")

        data.Rows.Add({"Vigente", 1})
        data.Rows.Add({"No Vigente", 0})

        Return data

    End Function

    ''' <summary>
    ''' Método que carga un DropDownList"
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CargarCombo(ByVal control As DropDownList, ByVal source As Object, ByVal campoTexto As String, ByVal campoValor As String)
        Try

            control.Items.Clear()
            control.DataSource = source
            control.DataTextField = campoTexto
            control.DataValueField = campoValor
            control.DataBind()
            control.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Método que carga un DropDownList y ordena sus elementos"
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CargarComboOrdenado(ByVal control As DropDownList, ByVal source As DataTable, ByVal campoTexto As String, ByVal campoValor As String)
        Try

            Dim dv As New DataView(source, "", campoTexto, DataViewRowState.Added)

            control.Items.Clear()
            control.DataSource = dv
            control.DataTextField = campoTexto
            control.DataValueField = campoValor
            control.DataBind()
            control.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Método que carga un DropDownList y ordena sus elementos"
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CargarComboOrdenado(ByVal control As DropDownList, ByVal source As List(Of ListItem), ByVal campoTexto As String, ByVal campoValor As String)
        Try

            control.Items.Clear()
            control.DataSource = source
            control.DataTextField = campoTexto
            control.DataValueField = campoValor
            control.DataBind()
            control.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Método que carga un DropDownList y ordena sus elementos"
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CargarComboOrdenadoOriginalRows(ByVal control As DropDownList, ByVal source As DataTable, ByVal campoTexto As String, ByVal campoValor As String)
        Try

            Dim dv As New DataView(source, "", campoTexto, DataViewRowState.OriginalRows)

            control.Items.Clear()
            control.DataSource = dv
            control.DataTextField = campoTexto
            control.DataValueField = campoValor
            control.DataBind()
            control.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    ''' <summary>
    ''' Método que valida la entrada capturada en un control TextBox (evitar scripting)
    ''' </summary>
    ''' <param name="controlValidar">Control de tipo Textbox a validar</param>
    ''' <remarks>Se emplea básicamente en aquellas páginas donde explicitamente se deshabilitó la validación de script</remarks>
    Public Shared Sub ValidaCamposCapturaHTML(ByVal controlValidar As TextBox)

        If controlValidar.Text.ToLower.Contains("<script>") Then

            Throw New ApplicationException("Texto potencialmente peligroso y no válido, verifique por favor")

        End If

    End Sub

    ''' <summary>
    ''' Convert a IEnumerable(of T) to a DataTable.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="list"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertToDataTable(Of T)(ByVal list As IEnumerable(Of T)) As DataTable

        Dim table As New DataTable()
        If Not IsNothing(list) And list.Count > 0 Then
            Dim fields() As PropertyInfo = GetType(T).GetProperties(BindingFlags.Public Or BindingFlags.Instance)
            For Each field As PropertyInfo In fields
                Dim tipo As Type = GetCoreType(field.PropertyType)
                table.Columns.Add(field.Name, tipo)
            Next
            For Each item As T In list
                Dim row As DataRow = table.NewRow()
                For i As Integer = 0 To fields.Count - 1
                    row(i) = fields(i).GetValue(item, Nothing)
                Next

                table.Rows.Add(row)
            Next
        End If

        Return table
    End Function

    ''' <summary>
    ''' Determine of specified type is nullable
    ''' </summary>
    ''' <param name="T">Tipo</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsNullable(ByVal T As Type) As Boolean
        Return Not (T.IsValueType) OrElse (T.IsGenericType AndAlso T.GetGenericTypeDefinition() Is GetType(Nullable(Of )))
    End Function

    ''' <summary>
    ''' Return underlying type if type is Nullable otherwise return the type
    ''' </summary>
    ''' <param name="t">tipo a evaluar</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCoreType(ByVal t As Type) As Type
        If Not IsNothing(t) And IsNullable(t) Then
            If Not t.IsValueType Then
                Return t
            Else
                Return Nullable.GetUnderlyingType(t)
            End If
        Else
            Return t
        End If
    End Function
    Public Shared Function ObtenerNombreArchivo(Nombre_Original As String) As String

        Dim Nombre_Archivo As String = ""

        Dim existe As String = ConsultaDocumentoNombre(Nombre_Original)
        If Not String.IsNullOrEmpty(existe) Then
            Dim Div_nombreArchivo() As String = Nombre_Original.Split(".")
            Nombre_Archivo = ""
            For cuenta As Int16 = 0 To Div_nombreArchivo.Count - 2
                Nombre_Archivo = Nombre_Archivo & Div_nombreArchivo(cuenta)
            Next
            Nombre_Archivo = Nombre_Archivo & "[" & Date.Now().ToString("yyyyMMddHHmmss") & "]"
            Nombre_Archivo = Nombre_Archivo & "." & Div_nombreArchivo(Div_nombreArchivo.Count - 1)

            Return Nombre_Archivo

        Else
            Return Nombre_Original
        End If
    End Function
    Public Shared Function ConsultaDocumentoNombre(ByVal nombre As String) As String
        Try
            Dim Shp As New Utilerias.SharePointManager
            Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
            Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
            Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
            Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
            Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS").ToString()
            Shp.NombreArchivo = nombre
            Shp.RowLimit = "100000"
            Dim id As String = Shp.ObtenerIdArchivo
            Return id

        Catch ex As Exception
            Return ""
        End Try
    End Function

    'Elimina todos los caracteres que contegan acentos y dierecis, cambiandolos por el mismo caracter 
    Public Shared Function Acentos(ByVal Cadena As String) As String
        Dim i As Integer
        Dim CadenaAux As String = String.Empty

        For i = 1 To Len(Cadena)
            Select Case Mid(Cadena, i, 1)
                Case "Á"
                    CadenaAux += "A"
                Case "É"
                    CadenaAux += "E"
                Case "Í"
                    CadenaAux += "I"
                Case "Ó"
                    CadenaAux += "O"
                Case "Ú"
                    CadenaAux += "U"
                Case "Ä"
                    CadenaAux += "A"
                Case "Ë"
                    CadenaAux += "E"
                Case "Ï"
                    CadenaAux += "I"
                Case "Ö"
                    CadenaAux += "O"
                Case "Ü"
                    CadenaAux += "U"
                Case "À"
                    CadenaAux += "A"
                Case "È"
                    CadenaAux += "E"
                Case "Ì"
                    CadenaAux += "I"
                Case "Ò"
                    CadenaAux += "O"
                Case "Ù"
                    CadenaAux += "U"
                Case "á"
                    CadenaAux += "a"
                Case "é"
                    CadenaAux += "e"
                Case "í"
                    CadenaAux += "i"
                Case "ó"
                    CadenaAux += "o"
                Case "ú"
                    CadenaAux += "u"
                Case "ä"
                    CadenaAux += "a"
                Case "ë"
                    CadenaAux += "e"
                Case "ï"
                    CadenaAux += "i"
                Case "ö"
                    CadenaAux += "o"
                Case "ü"
                    CadenaAux += "u"
                Case "à"
                    CadenaAux += "a"
                Case "è"
                    CadenaAux += "e"
                Case "ì"
                    CadenaAux += "i"
                Case "ò"
                    CadenaAux += "o"
                Case "ù"
                    CadenaAux += "u"
                Case Else
                    CadenaAux += Mid(Cadena, i, 1)
            End Select
        Next
        Return CadenaAux
    End Function


    Public Shared Function AlfaNumericoEspacios(ByVal str As String) As Boolean
        Dim lsCad As String = "^[a-zA-Z0-9!Ñ@#$%^&*()-_=+;:'""|~`<>?/{}]+[a-zA-Z0-9!@#$%^&*()-_=+;:'""|~`<>?/{}\s]*$"
        If Regex.IsMatch(str, lsCad) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function AcentosSepris(ByVal Cadena As String) As String
        Dim i As Integer
        Dim CadenaAux As String = String.Empty

        For i = 1 To Len(Cadena)
            Select Case Mid(Cadena, i, 1)
                Case "Ä"
                    CadenaAux += "A"
                Case "Ë"
                    CadenaAux += "E"
                Case "Ï"
                    CadenaAux += "I"
                Case "Ö"
                    CadenaAux += "O"
                Case "Ü"
                    CadenaAux += "U"
                Case "ä"
                    CadenaAux += "a"
                Case "ë"
                    CadenaAux += "e"
                Case "ï"
                    CadenaAux += "i"
                Case "ö"
                    CadenaAux += "o"
                Case "ü"
                    CadenaAux += "u"
                Case Else
                    CadenaAux += Mid(Cadena, i, 1)
            End Select
        Next
        Return CadenaAux
    End Function

    Public Shared Sub ConfigurarServerMail(ByRef mail As Mail)
        mail.ServidorMail = WebConfigurationManager.AppSettings("MailServer").ToString()
        mail.Dominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
        mail.DireccionRemitente = WebConfigurationManager.AppSettings("MailCuenta").ToString()
        mail.Usuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
        mail.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass").ToString())
        mail.EsHTML = True
    End Sub
End Class
