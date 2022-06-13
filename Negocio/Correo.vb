' Fecha de creación: 23/07/2013
' Fecha de modificación:  
' Nombre del Responsable: ARGC1
' Empresa: Softtek
'contiene los metodos que son parte del negocio de la administración de correos

Imports System.Data.SqlClient
Imports Conexion
Imports Utilerias

Public Class Correo

    Property IdentificadorCorreo As Integer
    Property Descripcion As String
    Property Asunto As String
    Property Mensaje As String
    Property EsVigente As Boolean = False
    Public Property Existe As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(ByVal idCorreo)
        Me.IdentificadorCorreo = idCorreo

        CargarDatos()
    End Sub


    Public Sub New(ByVal IdentificadorCorreo As Integer, ByVal Etiquetas As Dictionary(Of String, String))


        Dim conexion As New SQLServer
        Try

            Dim dr As SqlDataReader = conexion.ConsultarDR("SELECT T_DSC_CORREO, T_DSC_SUBJECT_CORREO, T_DSC_CUERPO_CORREO, N_FLAG_VIG FROM BDS_C_GR_CORREO WHERE N_ID_CORREO = " + IdentificadorCorreo.ToString())

            If dr.Read() Then

                Me.IdentificadorCorreo = IdentificadorCorreo
                Me.Descripcion = CStr(dr("T_DSC_CORREO"))
                Me.Asunto = CStr(dr("T_DSC_SUBJECT_CORREO"))
                Me.Mensaje = CStr(dr("T_DSC_CUERPO_CORREO"))
                Me.EsVigente = CBool(dr("N_FLAG_VIG"))

                If Etiquetas.Count > 0 Then
                    For Each kvp As KeyValuePair(Of String, String) In Etiquetas
                        Me.Mensaje = Me.Mensaje.Replace(kvp.Key, kvp.Value)
                    Next
                End If


            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try





    End Sub


    Public Sub CargarDatos()

        Dim conexion As New SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_CORREO") : listValores.Add(Me.IdentificadorCorreo)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_CORREO", listCampos, listValores)

            If Existe Then

                Dim dr As SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_CORREO", listCampos, listValores)

                If dr.Read() Then

                    Me.Descripcion = CStr(dr("T_DSC_CORREO"))
                    Me.Asunto = CStr(dr("T_DSC_SUBJECT_CORREO"))
                    Me.Mensaje = CStr(dr("T_DSC_CUERPO_CORREO"))
                    Me.EsVigente = CBool(dr("N_FLAG_VIG"))
                End If

            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Sub

    Public Function LlenaMarcadores() As DataSet
        Dim conexion As New SQLServer
        Dim camposWhere As New List(Of String)
        Dim valoresWhere As New List(Of Object)

        Try
            
            camposWhere.Add("N_FLAG_VIG")
            valoresWhere.Add(1)
            Dim dv As New DataSet

            dv = conexion.ConsultarRegistrosDS("BDS_C_GR_MARCADOR", camposWhere, valoresWhere)
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Function



    Public Function ObtenerTodos() As DataView

        Dim conexion As New SQLServer

        Try

            Dim dv As New DataView
            dv = conexion.ConsultarRegistrosDT("BDS_C_GR_CORREO").DefaultView
            dv.Sort = "N_ID_CORREO"
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Obtiene el siguiente id de la tabla
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1

        Dim conexion As New SQLServer

        Try

            Dim dr As SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_CORREO) + 1) N_ID_CORREO FROM BDS_C_GR_CORREO")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_CORREO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_CORREO"))
                End If

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Registra un correo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New SQLServer
        Dim bitacora As New Bitacora("Registro de nueva correo", "", "")
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try


            listCampos.Add("N_ID_CORREO") : listValores.Add(Me.IdentificadorCorreo)
            listCampos.Add("T_DSC_CORREO") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_SUBJECT_CORREO") : listValores.Add(Me.Asunto)
            listCampos.Add("T_DSC_CUERPO_CORREO") : listValores.Add(Me.Mensaje)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_CORREO", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_CORREO", listCampos, listValores, resultado, "")

        Catch ex As Exception
            resultado = False
            bitacora.Insertar("BDS_C_GR_CORREO", listCampos, listValores, resultado, "Error al guardar correo")
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally

            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
            listValores = Nothing
            listCampos = Nothing
        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Actualiza un correo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New SQLServer
        Dim bitacora As New Bitacora("Actualización de imagen", "", "")

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try
            listCampos.Add("T_DSC_CORREO") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_SUBJECT_CORREO") : listValores.Add(Me.Asunto)
            listCampos.Add("T_DSC_CUERPO_CORREO") : listValores.Add(Me.Mensaje)
           

            listCamposCondicion.Add("N_ID_CORREO") : listValoresCondicion.Add(Me.IdentificadorCorreo)

            resultado = conexion.Actualizar("BDS_C_GR_CORREO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_CORREO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_CORREO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar correo")
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If Conexion IsNot Nothing Then
                Conexion.CerrarConexion() : Conexion = Nothing
            End If
        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Elimina un correo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New SQLServer
        Dim bitacora As New Bitacora("Borrar correo", "", "")
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try
            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_CORREO") : listValoresCondicion.Add(Me.IdentificadorCorreo)

            resultado = conexion.Actualizar("BDS_C_GR_CORREO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_CORREO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_CORREO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar correo")
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If

        End Try

        Return resultado

    End Function

End Class
