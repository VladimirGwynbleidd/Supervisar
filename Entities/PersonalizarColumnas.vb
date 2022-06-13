Imports System
Imports System.Web
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

'***********************************************************************************************************
' Fecha Creación:       29 Julio 2013
' Codificó:             Jorge Alberto Rangel Ruiz
' Empresa:              Softtek
' Descripción           Clase para objetos, métodos y funciones para el proceso de personalización de columnas
'***********************************************************************************************************

''' <summary>
''' Clase para objetos, métodos y funciones para el proceso de personalización de columnas
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class PersonalizarColumnas

#Region "Clases"

    ''' <summary>
    ''' Clase para la tabla de relacion de componentes Gridview y páginas donde se alojan
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class GridView

#Region "Propiedades"

        ''' <summary>
        ''' Identificador del GridView
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Identificador As Integer

        ''' <summary>
        ''' Descripción del GridView
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descripcion As String

        ''' <summary>
        ''' Descripción de la Página
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pagina As String

        ''' <summary>
        ''' Descripción de la vista asociada al GridView
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Vista As String


#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal identificadorGridView As Integer, ByVal descripcionGridView As String, _
                       ByVal descripcionPagina As String, ByVal descripcionVista As String)

            Me.Identificador = identificadorGridView
            Me.Descripcion = descripcionGridView
            Me.Pagina = descripcionPagina
            Me.Vista = descripcionVista

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para la tabla de columnas pertenecentes a un GridView
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Columnas

#Region "Propiedades"

        ''' <summary>
        ''' Identificador del GridView
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GridView As Integer

        ''' <summary>
        ''' Identificador de la columna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Identificador As Integer

        ''' <summary>
        ''' Descripción de la columna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descripcion As String

        ''' <summary>
        ''' Descripcion del Texto de la columna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Texto As String

        ''' <summary>
        ''' Vigencia de la columna (solo aplica para cuando se obtiene como parte de las columnas asignadas a un usuario)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Vigencia As Boolean

#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal identificadorGridView As Integer, ByVal identificadorColumna As Integer, _
                       ByVal descripcionColumna As String, ByVal descripcionTexto As String, ByVal vigenciaFlag As Boolean)

            Me.GridView = identificadorGridView
            Me.Identificador = identificadorColumna
            Me.Descripcion = descripcionColumna
            Me.Texto = descripcionTexto
            Me.Vigencia = vigenciaFlag

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para la tabla de relacion de columnas de un GridView de un Usuario
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class UsuarioGridViewColumnas

#Region "Propiedades"

        ''' <summary>
        ''' Identificador del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Usuario As String

        ''' <summary>
        ''' Objeto GridView
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GridView As PersonalizarColumnas.GridView

        ''' <summary>
        ''' Lista de objetos Columnas del GridView en cuestión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Columnas As List(Of PersonalizarColumnas.Columnas)

#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal usuario As String, ByVal identificadorGridView As Integer)

            Me.Usuario = usuario
            Me.GridView = ObtenerGridView(identificadorGridView)
            Me.Columnas = ObtenerUsuarioColumnasGridView(usuario, identificadorGridView)

        End Sub

#End Region

    End Class


#End Region

#Region "Métodos de consulta"

    ''' <summary>
    ''' Método para obtener un Objeto GridView de personalizacion de columnas
    ''' </summary>
    ''' <param name="IdentificadorGridView"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerGridView(ByVal identificadorGridView As Integer) As PersonalizarColumnas.GridView

        Dim resultado As PersonalizarColumnas.GridView = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtGridView As DataTable = conexion.ConsultarDT("SELECT N_ID_GRIDVIEW, T_DSC_GRIDIVIEW, T_DSC_PAGINA, T_DSC_VISTA " & _
                                                                     " FROM BDS_C_GR_GRIDVIEW WHERE N_ID_GRIDVIEW = " & _
                                                                     identificadorGridView.ToString)


            If dtGridView.Rows.Count > 0 Then

                With dtGridView.Rows(0)

                    resultado = New PersonalizarColumnas.GridView(CInt(.Item("N_ID_GRIDVIEW").ToString), _
                                                                  .Item("T_DSC_GRIDIVIEW").ToString, _
                                                                  .Item("T_DSC_PAGINA").ToString, _
                                                                  .Item("T_DSC_VISTA").ToString)


                End With

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
    ''' Método para obtener la lista de columnas de un gridview de personalizacion
    ''' </summary>
    ''' <param name="IdentificadorGridView">Identifcador del GridView en Tabla</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerColumnasGridView(ByVal identificadorGridView As Integer) As List(Of PersonalizarColumnas.Columnas)

        Dim resultado As New List(Of PersonalizarColumnas.Columnas)

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtColumnas As DataTable = conexion.ConsultarDT("SELECT N_ID_GRIDVIEW, N_ID_COLUMNA, T_DSC_COLUMNA, T_DSC_COLUMNA_TEXTO " & _
                                                                     " FROM BDS_R_GR_GRIDVIEW_COLUMNAS WHERE N_ID_GRIDVIEW = " & _
                                                                     identificadorGridView.ToString)

            For Each renglon As DataRow In dtColumnas.Rows

                resultado.Add(New PersonalizarColumnas.Columnas(CInt(renglon.Item("N_ID_GRIDVIEW").ToString), _
                                                                CInt(renglon.Item("N_ID_COLUMNA").ToString), _
                                                                renglon.Item("T_DSC_COLUMNA").ToString, _
                                                                renglon.Item("T_DSC_COLUMNA_TEXTO"), True))

            Next


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
    ''' Método para obtener la lista de columnas de un GridView configuradas para un usuario determinado
    ''' </summary>
    ''' <param name="Usuario">Identificador del usuario de quien se requieren las columnas</param>
    ''' <param name="IdentificadorGridView">Identifcador del GridView en Tabla</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerUsuarioColumnasGridView(ByVal usuario As String, ByVal identificadorGridView As Integer) As List(Of PersonalizarColumnas.Columnas)

        Dim resultado As New List(Of PersonalizarColumnas.Columnas)

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtColumnas As DataTable = conexion.ConsultarDT("SELECT a.N_ID_GRIDVIEW, a.N_ID_COLUMNA, a.N_FLAG_VIG, b.T_DSC_COLUMNA, b.T_DSC_COLUMNA_TEXTO " &
                                                                     " FROM BDS_R_GR_USUARIO_GRIDVIEW_COLUMNAS a INNER JOIN BDS_R_GR_GRIDVIEW_COLUMNAS b " &
                                                                     " ON a.N_ID_GRIDVIEW = b.N_ID_GRIDVIEW and a.N_ID_COLUMNA = b.N_ID_COLUMNA " &
                                                                     " WHERE a.T_ID_USUARIO = '" &
                                                                     usuario & "' AND a.N_ID_GRIDVIEW = " & identificadorGridView.ToString)


            Dim valorx, valory As String


            For Each renglon As DataRow In dtColumnas.Rows


                valorx = renglon.Item("T_DSC_COLUMNA_TEXTO").ToString
                valory = renglon.Item("T_DSC_COLUMNA_TEXTO").ToString
                If valorx = "PC Cumple" Then
                    renglon.Item("T_DSC_COLUMNA_TEXTO") = "PC Cumple?"
                End If
                If valory = "Area" Then
                    renglon.Item("T_DSC_COLUMNA_TEXTO") = "Área"
                End If






                resultado.Add(New PersonalizarColumnas.Columnas(CInt(renglon.Item("N_ID_GRIDVIEW").ToString), _
                                                                CInt(renglon.Item("N_ID_COLUMNA").ToString), _
                                                                renglon.Item("T_DSC_COLUMNA").ToString, _
                                                                renglon.Item("T_DSC_COLUMNA_TEXTO").ToString, _
                                                                CBool(renglon.Item("N_FLAG_VIG"))))

            Next


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
    ''' Método que determina si un usuario tiene configuradas columnas para un GridView determinado
    ''' </summary>
    ''' <param name="Usuario">usuario a consultar</param>
    ''' <param name="IdentificadorGridView">Identificador del GridView a consultar</param>
    ''' <returns>True si tiene columnas, False si no tiene</returns>
    ''' <remarks></remarks>
    Public Shared Function UsuarioTieneColumnas(ByVal usuario As String, ByVal identificadorGridView As Integer) As Boolean

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtColumnas As DataTable = conexion.ConsultarDT("Select N_ID_GRIDVIEW, N_ID_COLUMNA, N_FLAG_VIG " & _
                                                                     " FROM BDS_R_GR_USUARIO_GRIDVIEW_COLUMNAS WHERE T_ID_USUARIO = '" & _
            usuario & "' AND N_ID_GRIDVIEW = " & identificadorGridView.ToString)

            Return dtColumnas.Rows.Count > 0

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function


#End Region

#Region "Métodos Persistencia"

    ''' <summary>
    ''' Método que guarda configuracion de columnas para un usuario y gridview determinado por primera vez
    ''' </summary>
    ''' <param name="Usuario">Identificador de Usuario</param>
    ''' <param name="IdentificadorGridView">Identificador de GridView</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConfiguraColumnasPrimeraVez(ByVal usuario As String, ByVal identificadorGridView As Integer) As Boolean

        Dim conexion As New Conexion.SQLServer

        Try

            Dim sqlInsercion As String = "INSERT INTO BDS_R_GR_USUARIO_GRIDVIEW_COLUMNAS" & _
                " (T_ID_USUARIO, N_ID_GRIDVIEW, N_ID_COLUMNA, N_FLAG_VIG) " & _
                " SELECT '" & usuario & "', N_ID_GRIDVIEW, N_ID_COLUMNA, 1 " & _
                " FROM BDS_R_GR_GRIDVIEW_COLUMNAS WHERE N_ID_GRIDVIEW = " & identificadorGridView.ToString

            Return conexion.Ejecutar(sqlInsercion)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método que actualiza la configuracion de columnas para un usuario y gridview determinado
    ''' </summary>
    ''' <param name="UsuarioColumnas">Objeto del tipo Entities.PersonalizarColumnas.UsuarioGridViewColumnas que contiene los datos necesarios</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GuardaColumnasGridViewUsuario(ByVal usuarioColumnas As Entities.PersonalizarColumnas.UsuarioGridViewColumnas) As Boolean

        Dim conexion As New Conexion.SQLServer
        Dim transaccion As Data.SqlClient.SqlTransaction

        ' Iniciamos la transaccion
        transaccion = conexion.BeginTransaction

        Dim bitacora As New Conexion.Bitacora("Actualización de Personalizacion Columnas ", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim resultado As Boolean = False

        Dim Campo As New List(Of String)
        Dim Valor As New List(Of Object)

        Dim CampoCondicion As New List(Of String)
        Dim ValorCondicion As New List(Of Object)

        ' Agregamos los campos a actualizar
        Campo.Add("N_FLAG_VIG")

        ' Agregamos el campo llave
        CampoCondicion.Add("T_ID_USUARIO")
        CampoCondicion.Add("N_ID_GRIDVIEW")
        CampoCondicion.Add("N_ID_COLUMNA")

        For Each columna As PersonalizarColumnas.Columnas In usuarioColumnas.Columnas

            Valor.Clear()
            ValorCondicion.Clear()

            ' Asignamos vigencia
            Valor.Add(IIf(columna.Vigencia = True, "1", "0"))

            ' Asignamos condiciones
            ValorCondicion.Add(usuarioColumnas.Usuario)
            ValorCondicion.Add(usuarioColumnas.GridView.Identificador)
            ValorCondicion.Add(columna.Identificador)

            ' actualizamos
            resultado = conexion.ActualizarConTransaccion("BDS_R_GR_USUARIO_GRIDVIEW_COLUMNAS", Campo, Valor, CampoCondicion, ValorCondicion, transaccion)
            bitacora.ActualizarConTransaccion("BDS_R_GR_USUARIO_GRIDVIEW_COLUMNAS", Campo, Valor, CampoCondicion, ValorCondicion, resultado, "")


            If Not resultado Then

                Exit For

            End If

        Next

        Select Case resultado

            Case True
                transaccion.Commit()

            Case False
                transaccion.Rollback()

        End Select


        bitacora.Finalizar(resultado)

        Return resultado

    End Function

#End Region



End Class
