Imports LogicaNegocioSICOD.Entities
Imports LogicaNegocioSICOD.BusinessRules

Public Class PersonalizarColumasOficios
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnCancelar.Attributes.Add("onclick", "CierraModalPersonzalizar(0)")
        lblAccines.Text = ""
    End Sub

    ''' <summary>
    ''' Evento que almacena los datos de las listas; Actualiza el estado VIG_FLAG
    ''' </summary>   
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click

        Try

            Dim objCamposConsulta As New LogicaNegocioSICOD.Entities.BDA_CAMPOS_CONSULTA()
            objCamposConsulta.USUARIO = Session("Usuario").ToString
            objCamposConsulta.ID_CONSULTA = Session("tipoConsulta")
            If lbxAsignado.Items.Count > 0 Then
                For totalCampos = 0 To lbxAsignado.Items.Count - 1
                    objCamposConsulta.ID_CAMPOS = lbxAsignado.Items(totalCampos).Value
                    objCamposConsulta.VIG_FLAG = True
                    LogicaNegocioSICOD.BusinessRules.BDA_CAMPOS_CONSULTA.Update(objCamposConsulta)
                Next
            End If

            If lbxSinAsignar.Items.Count > 0 Then
                For totalCampos = 0 To lbxSinAsignar.Items.Count - 1
                    objCamposConsulta.ID_CAMPOS = lbxSinAsignar.Items(totalCampos).Value
                    objCamposConsulta.VIG_FLAG = False
                    LogicaNegocioSICOD.BusinessRules.BDA_CAMPOS_CONSULTA.Update(objCamposConsulta)
                Next
            End If
            lblAccines.Text = " Los datos fueron almacenados exitosamente "
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "CierraModal", "CierraModalPersonzalizar(1)", True)

        Catch ex As Exception
            lblAccines.Text = "Error: " + ex.Message
        End Try

    End Sub

#Region "Botones de Asignación"
    ''' <summary>
    ''' Eventos para traspasar de un List Box a otro List Box
    ''' </summary>   
    Protected Sub btnPasarVerUno_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnPasarUnoVer.Click
        If lbxSinAsignar.SelectedIndex >= 0 Then
            lbxAsignado.Items.Add(lbxSinAsignar.Items(lbxSinAsignar.SelectedIndex))
            lbxSinAsignar.Items.RemoveAt(lbxSinAsignar.SelectedIndex)
        End If
        lbxSinAsignar.SelectedIndex = -1
        lbxAsignado.SelectedIndex = -1
    End Sub

    Protected Sub btnPasarVerTodos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnPasarTodosVer.Click
        Dim total As Integer
        For total = 0 To lbxSinAsignar.Items.Count - 1
            lbxAsignado.Items.Add(lbxSinAsignar.Items(total))
        Next total
        lbxSinAsignar.Items.Clear()
    End Sub

    Protected Sub btnPasarOcultarUno_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnPasarUnoOcultar.Click
        If lbxAsignado.SelectedIndex >= 0 Then
            lbxSinAsignar.Items.Add(lbxAsignado.Items(lbxAsignado.SelectedIndex))
            lbxAsignado.Items.RemoveAt(lbxAsignado.SelectedIndex)
        End If
        lbxAsignado.SelectedIndex = -1
        lbxSinAsignar.SelectedIndex = -1

    End Sub

    Protected Sub btnPasarOcultarTodos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnPasarTodosOcultar.Click
        Dim total As Integer
        For total = 0 To lbxAsignado.Items.Count - 1
            lbxSinAsignar.Items.Add(lbxAsignado.Items(total))
        Next total
        lbxAsignado.Items.Clear()
    End Sub

#End Region

    Protected Function UsuarioTieneColumnas(ByVal Usuario As String, ByVal tipoConsulta As Integer) As Boolean
        Return LogicaNegocioSICOD.BusinessRules.BDA_CAMPOS_CONSULTA.ConsultaUsuarioTieneColumnasAsignadas(tipoConsulta, Usuario)
    End Function

    ''' <summary>
    ''' Carga las listas de Asignar y NoAsignado
    ''' </summary>
    Public Sub LoadListasPersonalizadas(ByVal Usuario As String, ByVal tipoConsulta As Integer)
        Try

            Session("tipoConsulta") = tipoConsulta

            If Not UsuarioTieneColumnas(Usuario, tipoConsulta) Then

                '-----------------------------------------------------------
                ' Cargar todas las columnas para este grid en la columna de 'ver'
                ' Insertar de una vez en la tabla de relaciones Campos_Consulta con vig_flag = 0
                '-----------------------------------------------------------
                Dim objCamposConsulta As New LogicaNegocioSICOD.Entities.BDA_CAMPOS_CONSULTA()
                objCamposConsulta.USUARIO = Session("Usuario").ToString
                objCamposConsulta.ID_CONSULTA = tipoConsulta
                objCamposConsulta.VIG_FLAG = True
                '-----------------------------------------------------------
                ' Insertar
                '-----------------------------------------------------------
                Dim dt As DataTable = ReturnColumnasPorConsulta(tipoConsulta)
                For Each row As DataRow In dt.Rows
                    objCamposConsulta.ID_CAMPOS = row("ID_CAMPOS")
                    LogicaNegocioSICOD.BusinessRules.BDA_CAMPOS_CONSULTA.Insert(objCamposConsulta)
                Next
            End If

            Dim dtView As DataView = New DataView

            dtView = ReturnColumnasPersonalizada(Usuario, tipoConsulta)
            dtView.Sort = "DSC_CAMPOS"

            dtView.RowFilter = "vig_flag = 1"
            lbxAsignado.Items.Clear()
            lbxAsignado.DataSource = dtView
            lbxAsignado.DataValueField = "ID_CAMPOS"
            lbxAsignado.DataTextField = "DSC_CAMPOS"
            lbxAsignado.DataBind()

            dtView.RowFilter = "vig_flag = 0"
            lbxSinAsignar.Items.Clear()
            lbxSinAsignar.DataSource = dtView
            lbxSinAsignar.DataValueField = "ID_CAMPOS"
            lbxSinAsignar.DataTextField = "DSC_CAMPOS"
            lbxSinAsignar.DataBind()

        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' Consulta a la Base de Datos de acuerdo al Usuario y Numero de Consulta 
    ''' </summary>
    ''' <returns>Regresa un DataView con los campos consultados</returns>
    ''' <remarks></remarks>
    Function ReturnColumnasPersonalizada(ByVal Usuario As String, ByVal tipoConsulta As Integer) As DataView
        Try
            Dim dtTable As DataTable = New DataTable
            Dim dtView As DataView
            dtTable = LogicaNegocioSICOD.BusinessRules.BDA_CAMPOS_CONSULTA.ConsultaListaCampos(tipoConsulta, Usuario)
            dtView = New DataView(dtTable)
            Return dtView
        Catch ex As Exception
            Return New DataView
        End Try

    End Function


    Function ReturnColumnasPorConsulta(ByVal tipoConsulta As Integer) As DataTable
        Try
            Dim dtTable As DataTable = New DataTable
            dtTable = LogicaNegocioSICOD.BusinessRules.BDA_CAMPOS_CONSULTA.ConsultaListaConsulta(tipoConsulta)
            Return dtTable
        Catch ex As Exception
            Return New DataTable
        End Try

    End Function

    ''' <summary>
    ''' Valida los campos de la lista SinAsignar y Regresa un ListBox
    ''' </summary>
    ''' <returns>Regresa un ListBox</returns>
    ''' <remarks></remarks>
    Public Function PerzonalizaColumasOcultasDataGrid() As ListBox
        Return lbxSinAsignar
    End Function
    Public Function PerzonalizaColumasVisiblesDataGrid() As ListBox
        Return lbxAsignado
    End Function

End Class