Option Explicit On
Option Strict On

Imports Clases


Public Class Generales

    Public Shared Sub CargarCombo(ByVal pCombo As DropDownList, ByVal pDt As DataTable, ByVal pCampoTexto As String, ByVal pCampoValor As String)
        Try
            pCombo.DataSource = pDt
            pCombo.DataTextField = pCampoTexto
            pCombo.DataValueField = pCampoValor
            pCombo.DataBind()
            pCombo.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
        Catch ex As Exception
            Throw ex
        End Try
        
    End Sub

    Public Shared Sub CargarListBox(ByVal pListBox As ListBox, ByVal pDt As DataTable, ByVal pCampoTexto As String, ByVal pCampoValor As String)
        Try
            pListBox.DataSource = pDt
            pListBox.DataTextField = pCampoTexto
            pListBox.DataValueField = pCampoValor
            pListBox.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub EscribirError(ByVal pEx As Exception, ByVal pNombreFuncion As String)
        'ControlErrores.nsControlErrores.ControlErrores.EscribirEvento("Función " & pNombreFuncion & ": " & pEx.Message, EventLogEntryType.Error, "SICOD_-OFICIOS", "")
        'HttpContext.Current.Response.Redirect("~\PaginaErrorSICOD.aspx")
    End Sub

    ''Calcula la fecha para asignar una fecha habil, validando que no sea dia festivo y fin de semana
    Public Shared Function AsignaNuevaFechaHabil(ByVal totalDias As Integer, ByRef fechaSeleccionada As Date, ByVal autoridad As Integer) As String

        Try
            '2012-10-11
            Dim fechaFormateada As String
            fechaFormateada = fechaSeleccionada.ToString("yyyy/MM/dd")
            Dim tablaFechaValida As New DataTable
            Dim Con As Clases.Conexion = New Clases.Conexion()
            Dim ConSIPAJ As Clases.Conexion = New Clases.Conexion(Clases.Conexion.BD.SIPAJ)
            Dim dtCalendario As DataTable = ConSIPAJ.Datos("SELECT ID_CALENDARIO FROM " & Clases.Conexion.OwnerSIPAJ & "BDA_C_AUTORIDAD WHERE ID_AUTORIDAD = " & autoridad, False).Tables(0)
            tablaFechaValida = ConSIPAJ.Datos("SELECT dbo.FechaLimite(" & totalDias & ",'" & fechaFormateada & "', " & dtCalendario.Rows(0)("ID_CALENDARIO").ToString & ")", False).Tables(0)
            Return tablaFechaValida.Rows(0).ItemArray(0).ToString()
        Catch
            Return ""
        End Try

    End Function

    ''Calcula la fecha para asignar una nueva fecha habil validando que no sea dia festivo ni fin de semana, quita días al calendario
    Public Shared Function AsignaNuevaFechaHabilNeg(ByVal totalDias As Integer, ByRef fechaSeleccionada As Date, ByVal autoridad As Integer) As String

        Try
            '2012-10-11
            Dim fechaFormateada As String
            fechaFormateada = fechaSeleccionada.ToString("yyyy/MM/dd")
            Dim tablaFechaValida As New DataTable
            Dim Con As Clases.Conexion = New Clases.Conexion()
            Dim ConSIPAJ As Clases.Conexion = New Clases.Conexion(Clases.Conexion.BD.SIPAJ)
            Dim dtCalendario As DataTable = ConSIPAJ.Datos("SELECT ID_CALENDARIO FROM " & Clases.Conexion.OwnerSIPAJ & "BDA_C_AUTORIDAD WHERE ID_AUTORIDAD = " & autoridad, False).Tables(0)
            tablaFechaValida = ConSIPAJ.Datos("SELECT dbo.FechaLimiteNeg(" & totalDias & ",'" & fechaFormateada & "', " & dtCalendario.Rows(0)("ID_CALENDARIO").ToString & ")", False).Tables(0)
            Return tablaFechaValida.Rows(0).ItemArray(0).ToString()
        Catch
            Return ""
        End Try

    End Function

    ''Valida si la fecha seleccionada es dia habil
    Public Shared Function FechaHabilValida(ByVal fechaDiaHabil As String) As Boolean
        Dim esFechaValida As Boolean = False
        If IsDate(fechaDiaHabil) Then
            If (Weekday(CType(fechaDiaHabil, Date)) = 1) Or (Weekday(CType(fechaDiaHabil, Date)) = 7) Then
                esFechaValida = True
            End If
        End If
        Return esFechaValida
    End Function

    ''Compara si es día Festivo; De acuerdo a la Tabla de Dias Festivos
    Public Shared Function DiasFestivosFechaValida(ByRef fechaDiaHabil As String, ByVal ID_AUTORIDAD As String) As Boolean
        Try
            Dim Con As Clases.Conexion = New Clases.Conexion()
            Dim ConSIPAJ As Clases.Conexion = New Clases.Conexion(Clases.Conexion.BD.SIPAJ)
            Dim dtDias As New DataTable()
            Dim esFechaValida As Boolean = False
            Dim dtDia As New DataTable()

            dtDia = ConSIPAJ.Datos("SELECT FECH_DIA_FESTIVO, DSC_DIA_FESTIVO FROM " & Clases.Conexion.Owner & "BDA_C_DIA_FESTIVO WHERE  ID_CALENDARIO = (SELECT ID_CALENDARIO FROM BDA_C_AUTORIDAD WHERE ID_AUTORIDAD = " & ID_AUTORIDAD & " AND VIG_FLAG = 1 ) AND VIG_FLAG = 1", True).Tables(0)
            For Each RowDias As DataRow In dtDia.Rows
                If Convert.ToDateTime(RowDias("FECH_DIA_FESTIVO")) = Convert.ToDateTime(fechaDiaHabil) Then
                    esFechaValida = True
                    Return esFechaValida
                End If
            Next
            Return esFechaValida
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Shared Function ExtraeHoraMinuto(ByVal fehcaNotConsar As String) As String()
        Dim fechaConsar As String() = fehcaNotConsar.Split(New Char() {" "c})
        Dim horaMinuto As String() = fechaConsar(1).Split(New Char() {":"c})
        Return horaMinuto
    End Function

    Public Shared Function ObtenParametroFechas(ByVal descripcionParametro As String) As Integer
        Dim Valor As Integer = 0
        Try
            Dim Con As Clases.Conexion = New Clases.Conexion()
            Dim ConSIPAJ As Clases.Conexion = New Clases.Conexion(Clases.Conexion.BD.SIPAJ)
            Dim dtParametro As DataTable = ConSIPAJ.Datos("SELECT VALOR FROM " & Clases.Conexion.OwnerSIPAJ & "BDS_C_PARAMETRO WHERE PARAMETRO = '" & descripcionParametro & "' AND VIG_FLAG = 1 ", False).Tables(0)
            Valor = Convert.ToInt32(dtParametro.Rows(0).ItemArray(0).ToString())
            Return Valor
        Catch
            Return Valor
        End Try

    End Function

    Public Shared Function SafeSqlLiteral(ByVal inputSQL As String) As String

        Return inputSQL.Replace("'", "''").Replace("--", "").Replace("[", "[[]").Replace("%", "[%]")

    End Function


End Class
