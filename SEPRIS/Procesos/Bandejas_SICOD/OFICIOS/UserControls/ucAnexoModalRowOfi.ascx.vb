Public Class ucAnexoModalRowOfi
    Inherits System.Web.UI.UserControl


#Region "Propiedades"

    ''' <summary>
    ''' Permite asignar la imagen que se presentara al lado del checkbox "otros"
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property ImageURL As String
        Set(ByVal value As String)
            imgOtros.ImageUrl = value
        End Set
        Get
            Return imgOtros.ImageUrl
        End Get
    End Property


    ''' <summary>
    ''' Asigna el texto a mostrar cuando se encuentre seleccionado el check "Otros"
    ''' </summary>
    ''' <value></value>
    Public Property OTROS_DSC As String
        Set(ByVal value As String)
            imgOtros.ToolTip = value
        End Set
        Get
            Return imgOtros.ToolTip
        End Get
    End Property

    ''' <summary>
    ''' Asigna/Obtiene el FOLIO
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ID_FOLIO As String
        Get
            Return hdnFolio.Value
        End Get
        Set(ByVal value As String)
            hdnFolio.Value = value
            lblFolio.Text = "Folio:&nbsp&nbsp;" & value
        End Set
    End Property

    ''' <summary>
    ''' Asigna/Obtiene los valores de los check en un string formato 0|0|0|0|0|0
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public Property CHECK_VALUES As String
        Set(ByVal value As String)
            hdn_CheckValues.Value = value
            CargaDatos()
        End Set
        Get
            Return hdn_CheckValues.Value
        End Get
    End Property


#End Region

    Private Sub CargaDatos()

        Dim _indice As Integer = 0
        For Each _check As String In hdn_CheckValues.Value.Split("|")

            Select Case _indice

                Case 0
                    chk_Carpeta.Checked = _check = "1"

                Case 1
                    chk_CD.Checked = _check = "1"

                Case 2
                    chk_Sobre.Checked = _check = "1"

                Case 3
                    chk_Paq.Checked = _check = "1"

                Case 4
                    chk_Rev.Checked = _check = "1"

                Case 5
                    chk_Otro.Checked = _check = "1"
                    imgOtros.Visible = _check = "1"


                Case Else
                    Console.WriteLine("No considerado")

            End Select

            _indice += 1

        Next

    End Sub

End Class