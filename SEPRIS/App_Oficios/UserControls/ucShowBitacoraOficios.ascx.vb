Public Class ucShowBitacoraOficios
    Inherits System.Web.UI.UserControl

#Region "Propiedades Publicas"

#Region "Enumerador Tipo Documento"

    Public Enum ucSB_TipoDocumentoBitacora
        Entrada_Folio
        Salida_Oficio
    End Enum

#End Region

    Private _tipoDocto As ucSB_TipoDocumentoBitacora
    Public WriteOnly Property ucSB_TipoDocumento As ucSB_TipoDocumentoBitacora
        Set(ByVal value As ucSB_TipoDocumentoBitacora)
            _tipoDocto = value
        End Set
    End Property

    Private _numDocto As String = ""
    Public WriteOnly Property ucSB_NumeroDocumento As String
        Set(ByVal value As String)
            _numDocto = value
        End Set
    End Property

    Private _data As DataTable
    Public WriteOnly Property ucSB_Datos As DataTable
        Set(ByVal value As DataTable)
            _data = value
            If hdnSession.Value = "" Then
                Dim _rdn = New Random(Now.Millisecond)
                hdnSession.Value = "SSHistorialPP_" & _rdn.Next(999999).ToString
            End If
            Session(hdnSession.Value) = _data
        End Set
    End Property

    Public WriteOnly Property ucSB_MostrarBotonExportar As Boolean
        Set(ByVal value As Boolean)
            If value Then
                BtnExportarInferior.Style.Add("display", "block")
            Else
                BtnExportarInferior.Style.Add("display", "none")
            End If
        End Set
    End Property

    Public WriteOnly Property ucSB_MostrarBotonCerrarAfuera As Boolean
        Set(ByVal value As Boolean)
            If value Then
                btnCerrarAfuera.Style.Add("display", "block")
                btnCerrar.Style.Add("display", "none")
            Else
                btnCerrarAfuera.Style.Add("display", "none")
                btnCerrar.Style.Add("display", "block")
            End If
        End Set
    End Property

#End Region

    Public Sub ucSB_AbrirBitacora()

        Try

            Dim col1 As Web.UI.WebControls.BoundField = grvHistorial.Columns(1)
            Dim col7 As Web.UI.WebControls.BoundField = grvHistorial.Columns(7)


            Select Case _tipoDocto

                Case ucSB_TipoDocumentoBitacora.Entrada_Folio
                    Me.lblTitulo.Text = "Folio Número "

                    col1.HeaderText = "Fecha Registro"
                    col1.DataField = "FECH_REGISTRO"
                    col1.SortExpression = "FECH_REGISTRO"

                    col7.HeaderText = "Fecha Limite"
                    col7.DataField = "FECHA_LIMITE"
                    col7.SortExpression = "FECHA_LIMITE"

                Case ucSB_TipoDocumentoBitacora.Salida_Oficio
                    Me.lblTitulo.Text = "Oficio Número "

                    col1.HeaderText = "Fecha Alta"
                    col1.DataField = "F_FECHA_ALTA"
                    col1.SortExpression = "F_FECHA_ALTA"

                    col7.HeaderText = "Fecha Vencimiento"
                    col7.DataField = "FECHA_VENCIMIENTO"
                    col7.SortExpression = "FECHA_VENCIMIENTO"


                Case Else
                    Throw New NullReferenceException("Tipo de documento no válido para bitácora")

            End Select

            Me.lblTitulo.Text &= _numDocto

            Me.grvHistorial.DataSource = _data
            Me.grvHistorial.DataBind()



            If _data.Rows.Count = 0 Then

                pnlImagenNoExisten.Style.Add("display", "block")
                pnlGrid.Visible = False

            End If


            mpeShowComentario.Show()


        Catch ex As Exception

            MuestraErrores(ex.Message)

        End Try





    End Sub

    Protected Sub BtnExportarInferior_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExportarInferior.Click
        Try

            Dim col1 As Web.UI.WebControls.BoundField = grvHistorial.Columns(1)
            Dim col7 As Web.UI.WebControls.BoundField = grvHistorial.Columns(7)

            Dim Reference As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)
            Reference.Add("SICOD")
            Reference.Add("DOCUMENTO " & Me.lblTitulo.Text)
            Reference.Add(Session("Usuario"))
            Reference.Add(DateTime.Today.ToString("dd/MM/yyyy"))


            Dim HeaderColumns As System.Collections.Generic.List(Of String()) = New System.Collections.Generic.List(Of String())
            HeaderColumns.Add({"Usuario Registró", "USUARIO_SISTEMA_NOM"})
            HeaderColumns.Add({col1.HeaderText, col1.DataField})
            HeaderColumns.Add({"Tipo de Movimiento", "MOVIMIENTO"})
            HeaderColumns.Add({"Descripción Movimiento", "DESCRIPCION"})
            HeaderColumns.Add({"Usuario Origen", "USUARIO_ORIGEN_NOM"})
            HeaderColumns.Add({"Usuario Destino", "USUARIO_DESTINO_NOM"})
            HeaderColumns.Add({"Fecha Movimiento", "FECH_MOVIMIENTO"})
            HeaderColumns.Add({col7.HeaderText, col7.DataField})

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = "BITACORA"
            export.TableName = "Histórico de Movimientos"
            export.HeaderColor = "5D7370"
            export.HeaderForeColor = "FFFFFF"
            export.CellForeColor = "000000"
            export.ShowGridLines = False
            export.Reference = Reference
            export.HeaderColumns = HeaderColumns
            export.DataSource = CType(Session(hdnSession.Value), DataTable)
            export.CreatePackage("BITACORA")


        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        End Try
    End Sub

    Public Sub MuestraErrores(ByVal MsgError As String)

        BtnExportarInferior.Visible = False
        pnlGrid.Style.Add("display", "none")
        pnlImagenNoExisten.Style.Add("display", "none")
        pnlErrores.Style.Add("display", "block")
        lblErrores.Text = MsgError

        mpeShowComentario.Show()

    End Sub

    'Public Event CerrarAfuera()
    'Private Sub btnCerrarAfuera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCerrarAfuera.Click

    '    'RaiseEvent CerrarAfuera()
    '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "cierraVentana", "Cierrame();", True)

    'End Sub


End Class