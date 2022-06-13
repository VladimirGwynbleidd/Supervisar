Public Class ValidaDocumentos

    Public Function ValidaDocumentosVisitaPaso(ByVal idVisita As Integer, ByVal idPaso As Integer, documentos As List(Of Object)) As Integer
        Dim lbRes As Boolean = False
        Dim icCodError As Integer

        Try
            If ConsultaDocumentosObligatoriosSinCargarVisita() Then
                Dim objEtiqueta As New Entities.EtiquetaError(2139) 'Código de confirmación si los documentos están correctos
                icCodError = objEtiqueta.Identificador
            Else
                Dim objEtiqueta As New Entities.EtiquetaError(2140) 'Código de confirmación si los documentos están incorrectos
                icCodError = objEtiqueta.Identificador
            End If

        Catch ex As Exception
            icCodError = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ValidaDocumentosVisitaPaso_ValidaDocumentos", "")
        Finally

        End Try

        Return icCodError

    End Function

    ''' <summary>
    ''' Busca si hay documentos obligatorios sin cargar en el paso actual de la visita
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConsultaDocumentosObligatoriosSinCargarVisita(Optional idPaso As Integer = 1,
                                                       Optional iBanderaObligatorio As Integer = 1,
                                                       Optional iTipoDocumento As Integer = -1,
                                                       Optional idVisita As Integer = Constantes.Todos,
                                                       Optional idDocumento As Integer = Constantes.Todos) As Boolean
        Dim lstDocMin As List(Of Documento.DocumentoMini)

        lstDocMin = AccesoBD.ObtenerDocumentosObligatorios(idVisita, idPaso, idDocumento, iBanderaObligatorio, iTipoDocumento)

        If lstDocMin.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
