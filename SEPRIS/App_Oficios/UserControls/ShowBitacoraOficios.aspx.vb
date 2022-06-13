Public Class ShowBitacoraOficios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ctrShowBitacora.ucSB_MostrarBotonCerrarAfuera = True
            ctrShowBitacora.ucSB_MostrarBotonExportar = False

            Try

                CargaDatos()

            Catch ex As Exception

                ctrShowBitacora.MuestraErrores(ex.Message)

            End Try

        End If

    End Sub

    Private Sub CargaDatos()

        Dim _Docto As String = Request.QueryString("doc")
        Dim _tipoDoc As ucShowBitacora.ucSB_TipoDocumentoBitacora
        Dim _dt As New DataTable


        ctrShowBitacora.ucSB_NumeroDocumento = _Docto

        Select Case Request.QueryString("tipo")

            Case "0"
                _tipoDoc = ucShowBitacora.ucSB_TipoDocumentoBitacora.Entrada_Folio
                _dt = LogicaNegocioSICOD.BusinessRules.BDA_BITACORA.ConsultaMovimientoBitacoraFolio(Convert.ToInt32(_Docto))

            Case "1"
                _tipoDoc = ucShowBitacora.ucSB_TipoDocumentoBitacora.Salida_Oficio

                With LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorNumero("'" & _Docto & "'").Rows(0)

                    _dt = LogicaNegocioSICOD.BusinessRules.BDA_BITACORA.ConsultaMovimientoBitacoraOficio(
                        Convert.ToInt32(.Item("ID_AREA_OFICIO").ToString()),
                        Convert.ToInt32(.Item("ID_TIPO_DOCUMENTO").ToString()),
                        Convert.ToInt32(.Item("ID_ANIO").ToString()),
                        Convert.ToInt32(.Item("I_OFICIO_CONSECUTIVO").ToString()))
                End With

            Case Else
                Throw New NullReferenceException("El tipo de documento no es válido")

        End Select

        ctrShowBitacora.ucSB_TipoDocumento = _tipoDoc
        ctrShowBitacora.ucSB_Datos = _dt

        ctrShowBitacora.ucSB_AbrirBitacora()

    End Sub

End Class