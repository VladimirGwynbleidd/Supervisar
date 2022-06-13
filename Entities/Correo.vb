'- Fecha de creación: 26/07/2013
'- Fecha de modificación:  15/11/1013
'- Nombre del Responsable: Rafael Rodríguez Sánchez
'- Empresa: Softtek
'- Clase correo que incluye propiedades y metodos para su administracion
'- Se Agrego funcionalidad para copia de Correo

<Serializable()> _
Public Class Correo

    Private TablaCorreo As String = "BDS_C_GR_CORREO"
    Private TablaPerfilCorreo As String = "BDS_R_GR_PERFIL_CORREO"
    Private TablaExclusion As String = "BDS_R_GR_USUARIO_PERFIL_CORREO"
    Private TablaUsuarioPerfil As String = "BDS_R_GR_USUARIO_PERFIL"
    Private TablaUsuarioCopiaCorreo As String = "DBS_R_GR_USUARIO_COPIA_CORREO"


#Region "Propiedades"
    Public Property Identificador As Integer

    Public Property Descripcion As String

    Public Property Asunto As String

    Public Property Cuerpo As String

    Public Property Vigencia As Boolean

    Public Property InicioVigencia As Date

    Public Property FinVigencia As Date

    Public Property Existe As Boolean = False

#End Region

#Region "Constructores"
    Public Sub New()

    End Sub

    Public Sub New(ByVal id As Integer)
        Me.Identificador = id
        CargarDatos()
    End Sub

    Public Sub New(ByVal id As Integer, ByVal descripcion As String, ByVal asunto As String, ByVal cuerpo As String,
                   ByVal vigencia As Boolean, ByVal inicioVigencias As Date, Optional ByVal finVigencia As Date = Nothing)
        Me.Identificador = id
        Me.Descripcion = descripcion
        Me.Asunto = asunto
        Me.Cuerpo = cuerpo
        Me.Vigencia = vigencia
        Me.InicioVigencia = InicioVigencia
        If Not IsNothing(finVigencia) Then
            Me.FinVigencia = finVigencia
        End If
    End Sub
#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los datos de Servicio tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_CORREO") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(TablaCorreo, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(TablaCorreo, listCampos, listValores)

                    If dr.Read() Then
                        Me.Descripcion = CStr(dr("T_DSC_CORREO"))
                        Me.Asunto = CStr(dr("T_DSC_SUBJECT_CORREO"))
                        Me.Cuerpo = CStr(dr("T_DSC_CUERPO_CORREO"))
                        Me.Vigencia = Convert.ToBoolean(dr("N_FLAG_VIG"))
                        Me.InicioVigencia = Convert.ToDateTime(dr("F_FECH_INI_VIG"))

                        If Not IsDBNull(dr("F_FECH_FIN_VIG")) Then
                            Me.FinVigencia = Convert.ToDateTime(dr("F_FECH_FIN_VIG"))
                        Else
                            Me.FinVigencia = Nothing
                        End If

                    End If

                End If
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Finally
                If dr IsNot Nothing Then
                    If Not dr.IsClosed Then
                        dr.Close() : dr = Nothing
                    End If
                End If
            End Try

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Sub


    ''' <summary>
    ''' Crea un correo a partir de un datarrow 
    ''' </summary>
    ''' <param name="dr">DataRow de la tabla BDS_C_GR_CORREO</param>
    ''' <returns>Un correo</returns>
    ''' <remarks></remarks>
    Private Function CreaCorreo(ByVal dr As DataRow) As Correo
        Dim fechaFin As Date = Nothing
        If Not IsDBNull(dr.Item("F_FECH_FIN_VIG")) Then
            fechaFin = Convert.ToDateTime(dr.Item("F_FECH_FIN_VIG")).Date
        End If

        Return New Correo(Convert.ToInt32(dr.Item("N_ID_CORREO")),
                          dr.Item("T_DSC_CORREO").ToString(),
                          dr.Item("T_DSC_SUBJECT_CORREO").ToString(),
                          dr.Item("T_DSC_CUERPO_CORREO").ToString(),
                          Convert.ToBoolean(dr.Item("N_FLAG_VIG")),
                          Convert.ToDateTime(dr.Item("F_FECH_INI_VIG")).Date)
    End Function



    ''' <summary>
    ''' Obtiene los correos que el perfil tiene asignados
    ''' </summary>
    ''' <param name="idPerfil">Id del perfil</param>
    ''' <returns>Lista de correos</returns>
    ''' <remarks></remarks>
    Public Function ObtenerCorreosPerfil(ByVal idPerfil As Integer) As List(Of Correo)
        Dim lstCorreo As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG FROM {0} C " + _
                                               "INNER JOIN {1} PC ON PC.N_ID_CORREO = C.N_ID_CORREO AND PC.N_ID_PERFIL = {2} " + _
                                               "WHERE C.N_FLAG_VIG = 1 ", TablaCorreo, TablaPerfilCorreo, idPerfil.ToString)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreo.Add(CreaCorreo(dr))
            Next

            Return lstCorreo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function


    'Funcion SobreCargada para Condicionar sobre perfil y Area Correos RRA
    Public Function ObtenerCorreosPerfil(ByVal idPerfil As Integer, ByVal idArea As Integer) As List(Of Correo)
        Dim lstCorreo As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG FROM {0} C " + _
                                               "INNER JOIN {1} PC ON PC.N_ID_CORREO = C.N_ID_CORREO AND PC.N_ID_PERFIL = {2} " + _
                                               "WHERE C.N_FLAG_VIG = 1 and PC.I_ID_AREA= {3} ORDER BY C.N_ID_CORREO", TablaCorreo, TablaPerfilCorreo, idPerfil.ToString, idArea.ToString)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreo.Add(CreaCorreo(dr))
            Next

            Return lstCorreo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Obtiene los correos que se pueden asignar al perfil
    ''' </summary>
    ''' <param name="idPerfil">Id del perfil</param>
    ''' <returns>Lista de Correos</returns>
    ''' <remarks></remarks>
    Public Function ObtenerCorreosNoEnPerfil(ByVal idPerfil As Integer) As List(Of Correo)
        Dim lstCorreo As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG FROM {0} C " + _
                                               "WHERE C.N_FLAG_VIG = 1 and C.N_ID_CORREO not in ( " + _
                                               "SELECT C.N_ID_CORREO FROM {0} C " + _
                                               "INNER JOIN {1} PC ON PC.N_ID_CORREO = C.N_ID_CORREO AND PC.N_ID_PERFIL = {2} " + _
                                               "WHERE C.N_FLAG_VIG = 1 ) ORDER BY C.N_ID_CORREO", TablaCorreo, TablaPerfilCorreo, idPerfil.ToString)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreo.Add(CreaCorreo(dr))
            Next

            Return lstCorreo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function


    'Sobrecarga de FUNCION para Agregar area RRA
    Public Function ObtenerCorreosNoEnPerfil(ByVal idPerfil As Integer, ByVal idArea As Integer) As List(Of Correo)
        Dim lstCorreo As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG FROM {0} C " + _
                                               "WHERE C.N_FLAG_VIG = 1 and C.N_ID_CORREO not in ( " + _
                                               "SELECT C.N_ID_CORREO FROM {0} C " + _
                                               "INNER JOIN {1} PC ON PC.N_ID_CORREO = C.N_ID_CORREO AND PC.N_ID_PERFIL = {2} " + _
                                               "WHERE C.N_FLAG_VIG = 1 AND PC.I_ID_AREA = {3} ) ", TablaCorreo, TablaPerfilCorreo, idPerfil.ToString, idArea.ToString)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreo.Add(CreaCorreo(dr))
            Next

            Return lstCorreo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function


    ''' <summary>
    ''' Actualiza los correos asignados de un Perfil
    ''' </summary>
    ''' <param name="lstAgrega">Lista de Correos a asignar</param>
    ''' <param name="lstQuita">Lista de Correos a quitar</param>
    ''' <param name="idPerfil">Id del Perfil</param>
    ''' <remarks></remarks>
    ''' Se MODIFICO PARA AGREGAR EL AREA A LA LISTA DE CORREO YA QUE NO EXISTE EN CORREO DICHO PARAMETRO
    Public Sub GuardaCorreoPerfil(ByVal lstAgrega As List(Of Correo), ByVal lstQuita As List(Of Correo), ByVal idPerfil As Integer, ByVal idArea As Integer)
        Try
            If lstAgrega.Count > 0 Then
                AgregaCorreosPerfil(idPerfil, lstAgrega, idArea)
            End If

            If lstQuita.Count > 0 Then
                QuitaCorreosPerfil(idPerfil, lstQuita, idArea)
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Agrega correos a un perfil
    ''' </summary>
    ''' <param name="idPerfil">Id del Perfil</param>
    ''' <param name="lstAgrega">Lista de Correos a agregar</param>
    ''' <remarks></remarks>
    Public Sub AgregaCorreosPerfil(ByVal idPerfil As Integer, ByVal lstAgrega As List(Of Correo), ByVal idArea As Integer)
        Dim lstCampos As New List(Of String)
        lstCampos.Add("N_ID_PERFIL")
        lstCampos.Add("N_ID_CORREO")
        lstCampos.Add("I_ID_AREA")

        Dim lstValores As New List(Of Object)
        Dim inserta As Boolean

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualizar los Correos por perfil" & "(" & idPerfil.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try
            For Each mail As Correo In lstAgrega
                lstValores.Clear()
                lstValores.Add(idPerfil)
                lstValores.Add(mail.Identificador)
                lstValores.Add(idArea)
                Try
                    inserta = conexion.Insertar(TablaPerfilCorreo, lstCampos, lstValores)
                    bitacora.Insertar(TablaPerfilCorreo, lstCampos, lstValores, inserta, "")
                Catch ex As Exception
                    inserta = False
                    bitacora.Insertar(TablaPerfilCorreo, lstCampos, lstValores, inserta, ex.ToString())
                    Throw ex
                End Try
            Next
        Catch ex As Exception
            Throw ex
        Finally
            conexion.CerrarConexion()
            bitacora.Finalizar(inserta)
        End Try

    End Sub

    ''' <summary>
    ''' Quita correos de un perfil
    ''' </summary>
    ''' <param name="idPerfil">Id del perfil</param>
    ''' <param name="lstQuita">Lista de correos a quitar</param>
    ''' <remarks></remarks>
    Public Sub QuitaCorreosPerfil(ByVal idPerfil As Integer, ByVal lstQuita As List(Of Correo), ByVal idArea As Integer)
        Dim lstCampos As New List(Of String)
        lstCampos.Add("N_ID_PERFIL")
        lstCampos.Add("N_ID_CORREO")
        lstCampos.Add("I_ID_AREA")

        Dim lstValores As New List(Of Object)
        Dim elimino As Boolean

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualizar los Correos por perfil", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            For Each mail As Correo In lstQuita
                EliminaExclucionUsuario(idPerfil, mail.Identificador)

                'Elimina de la tabla de relacion
                lstValores.Clear()
                lstValores.Add(idPerfil)
                lstValores.Add(mail.Identificador)
                lstValores.Add(idArea)

                Try
                    elimino = conexion.Eliminar(TablaPerfilCorreo, lstCampos, lstValores)
                    bitacora.Eliminar(TablaPerfilCorreo, lstCampos, lstValores, elimino, "")
                Catch ex As Exception
                    elimino = False
                    bitacora.Eliminar(TablaPerfilCorreo, lstCampos, lstValores, elimino, ex.ToString)
                    Throw ex
                End Try
            Next
        Catch ex As Exception
            Throw ex
        Finally
            conexion.CerrarConexion()
            bitacora.Finalizar(elimino)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina los usuarios Excluidos de un Correo para un perfil
    ''' </summary>
    ''' <param name="idPerfil">Id del Perfil</param>
    ''' <param name="idCorreo">Id del Correo</param>
    ''' <remarks></remarks>
    Public Sub EliminaExclucionUsuario(ByVal idPerfil As Integer, ByVal idCorreo As Integer)
        Dim strQueryExcluidos As String = String.Format("SELECT T_ID_USUARIO FROM {0} WHERE N_ID_PERFIL = {1} " + _
                                                        "AND N_ID_CORREO = {2} ", TablaExclusion, idPerfil.ToString, idCorreo.ToString)
        Dim lstCampos As New List(Of String)
        lstCampos.Add("N_ID_PERFIL")
        lstCampos.Add("N_ID_CORREO")

        Dim lstValores As New List(Of Object)
        Dim elimino As Boolean

        Dim conexion As New Conexion.SQLServer

        Dim dt As DataTable = conexion.ConsultarDT(strQueryExcluidos)

        'Si existen usuarios excluidos del correo, se eliminan los registros
        If dt.Rows.Count > 0 Then
            Dim bitacora As New Conexion.Bitacora("Elimina Usuarios Excluidos", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Try
                lstValores.Add(idPerfil)
                lstValores.Add(idCorreo)
                elimino = conexion.Eliminar(TablaExclusion, lstCampos, lstValores)
                bitacora.Eliminar(TablaExclusion, lstCampos, lstValores, elimino, "")
            Catch ex As Exception
                elimino = False
                bitacora.Eliminar(TablaExclusion, lstCampos, lstValores, elimino, ex.ToString)
                Throw ex
            Finally
                conexion.CerrarConexion()
                bitacora.Finalizar(elimino)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los correos que recibe un usuario basado en su perfil
    ''' Sin las exclusiones que pueda tener
    ''' </summary>
    ''' <param name="idUsuario">Id de usuario</param>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <returns>Lista de correos</returns>
    ''' <remarks></remarks>
    Public Function ObtenerCorreosUsuario(ByVal idUsuario As String, ByVal idPerfil As Integer) As List(Of Correo)
        Dim lstCorreos As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG FROM {0} UP " + _
                                               "INNER JOIN {1} PC ON PC.N_ID_PERFIL = UP.N_ID_PERFIL " + _
                                               "INNER JOIN {2} C ON C.N_ID_CORREO = PC.N_ID_CORREO AND C.N_FLAG_VIG = 1 " + _
                                               "WHERE UP.T_ID_USUARIO = '{3}' AND UP.N_ID_PERFIL = {4} " + _
                                               "AND C.N_ID_CORREO NOT IN ( SELECT UPC.N_ID_CORREO FROM {5} UPC " + _
                                               "WHERE UPC.T_ID_USUARIO = UP.T_ID_USUARIO AND UPC.N_ID_PERFIL = UP.N_ID_PERFIL ) ",
                                               TablaUsuarioPerfil, TablaPerfilCorreo, TablaCorreo, idUsuario, idPerfil.ToString, TablaExclusion)

        Dim conexion As New Conexion.SQLServer()

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreos.Add(CreaCorreo(dr))
            Next

            Return lstCorreos

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Devuelve los correos en los que esta excluido un usuario, de acuerdo a su perfil
    ''' </summary>
    ''' <param name="idUsuario">Id de usuario</param>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <returns>Lista de correos</returns>
    ''' <remarks></remarks>
    Public Function ObtenerCorreosExcluyeUsuario(ByVal idUsuario As String, ByVal idPerfil As Integer) As List(Of Correo)
        Dim lstCorreos As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG FROM {0} C " + _
                                               "INNER JOIN {1} UPC ON C.N_ID_CORREO = UPC.N_ID_CORREO " + _
                                               "AND UPC.T_ID_USUARIO = '{2}' AND N_ID_PERFIL = {3} " + _
                                               "WHERE C.N_FLAG_VIG = 1 ", TablaCorreo, TablaExclusion, idUsuario, idPerfil.ToString)

        Dim conexion As New Conexion.SQLServer()

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreos.Add(CreaCorreo(dr))
            Next

            Return lstCorreos

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Borra las excluciones de correo para un usuario
    ''' </summary>
    ''' <param name="idUsuario">Id de Usuario</param>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="lstAgregados">Lista de correos</param>
    ''' <remarks></remarks>
    Public Sub BorrarExclusion(ByVal idUsuario As String, ByVal idPerfil As Integer, ByVal lstAgregados As List(Of Correo))
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim elimino As Boolean

        lstCampos.Add("T_ID_USUARIO")
        lstCampos.Add("N_ID_PERFIL")
        lstCampos.Add("N_ID_CORREO")

        Dim conexion As New Conexion.SQLServer()
        Dim bitacora As New Conexion.Bitacora("Borra una exclusion de correo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try
            For Each mail As Correo In lstAgregados
                lstValores.Clear()
                lstValores.Add(idUsuario)
                lstValores.Add(idPerfil)
                lstValores.Add(mail.Identificador)
                elimino = conexion.Eliminar(TablaExclusion, lstCampos, lstValores)
                bitacora.Eliminar(TablaExclusion, lstCampos, lstValores, elimino, "")
            Next
        Catch ex As Exception
            elimino = False
            bitacora.Eliminar(TablaExclusion, lstCampos, lstValores, elimino, ex.ToString)
            Throw ex
        Finally
            conexion.CerrarConexion()
            bitacora.Finalizar(elimino)
        End Try

    End Sub

    ''' <summary>
    ''' Agrega una exclucion de correo a un usuario
    ''' </summary>
    ''' <param name="idUsuario">Id de Usuario</param>
    ''' <param name="idPerfil">Id Perfil</param>
    ''' <param name="lstExcluidos">Lista de correos excluidos</param>
    ''' <remarks></remarks>
    Public Sub AgregarExclusion(ByVal idUsuario As String, ByVal idPerfil As Integer, ByVal lstExcluidos As List(Of Correo))
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim inserto As Boolean
        Dim sqlWhere = " WHERE T_ID_USUARIO = '" + idUsuario + "' "

        lstCampos.Add("T_ID_USUARIO")
        lstCampos.Add("N_ID_PERFIL")
        lstCampos.Add("N_ID_CORREO")
        lstCampos.Add("N_FLAG_VIG")
        lstCampos.Add("F_FECH_INI_VIG")

        Dim conexion As New Conexion.SQLServer()
        Dim bitacora As New Conexion.Bitacora("Agrega una exclusion de correo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try
            Dim entUsuario As New Usuario()
            entUsuario = Usuario.UsuariosSoloDatosGetCustom(sqlWhere)(0)
            For Each mail As Correo In lstExcluidos
                lstValores.Clear()
                lstValores.Add(entUsuario.IdentificadorUsuario)
                lstValores.Add(idPerfil)
                lstValores.Add(mail.Identificador)
                lstValores.Add(1)
                lstValores.Add(Date.Now.Date)
                inserto = conexion.Insertar(TablaExclusion, lstCampos, lstValores)
                bitacora.Insertar(TablaExclusion, lstCampos, lstValores, inserto, "")
            Next

        Catch ex As Exception
            inserto = False
            bitacora.Insertar(TablaExclusion, lstCampos, lstValores, inserto, ex.ToString)
            Throw ex
        Finally
            conexion.CerrarConexion()
            bitacora.Finalizar(inserto)
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza los correos asignados a un usuario
    ''' </summary>
    ''' <param name="idUsuario">Id de Usuario</param>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="lstAgregados">Lista de Correos Agregados</param>
    ''' <param name="lstExcluidos">Lista de Correos Excluidos</param>
    ''' <remarks></remarks>
    Public Sub GuardaExclusion(ByVal idUsuario As String, ByVal idPerfil As Integer, ByVal lstAgregados As List(Of Correo), ByVal lstExcluidos As List(Of Correo))
        Try
            BorrarExclusion(idUsuario, idPerfil, lstAgregados)

            AgregarExclusion(idUsuario, idPerfil, lstExcluidos)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene los correos a los cuales un usuario tiene asignada copia
    ''' </summary>
    ''' <param name="idUsuario">Identifdicador del Usuario</param>
    ''' <returns>Lista de correos</returns>
    ''' <remarks></remarks>
    Public Function ObtenerCorreosCopia(ByVal idUsuario As String) As List(Of Correo)
        Dim lstCorreos As New List(Of Correo)

        Dim strQuery As String = String.Format("SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO,  " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG " + _
                                               "FROM {0} C " + _
                                               "INNER JOIN {1} UCC ON UCC.T_ID_USUARIO = '{2}' AND UCC.N_ID_CORREO = C.N_ID_CORREO AND UCC.N_FLAG_VIG = 1 " + _
                                               "WHERE C.N_FLAG_VIG = 1 ", TablaCorreo, TablaUsuarioCopiaCorreo, idUsuario)
        Dim conexion As New Conexion.SQLServer()

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreos.Add(CreaCorreo(dr))
            Next

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return lstCorreos

    End Function

    ''' <summary>
    ''' Obtiene la lista de correos a los cuales el usuario no tiene asiganada una copia
    ''' </summary>
    ''' <param name="idUsuario">Identifdicador del Usuario</param>
    ''' <returns>Lista de correos</returns>
    ''' <remarks></remarks>
    Public Function ObtenerCorreosDisponibleCopia(ByVal idUsuario As String) As List(Of Correo)
        Dim lstCorreos As New List(Of Correo)

        Dim strQuery As String = String.Format("WITH EXCLUIDOS (N_ID_CORREO) AS ( " + _
                                               "    SELECT C.N_ID_CORREO FROM {0} C " + _
                                               "    INNER JOIN {1} CC ON CC.N_ID_CORREO = C.N_ID_CORREO AND CC. T_ID_USUARIO = '{2}'" + _
                                               "    WHERE C.N_FLAG_VIG = 1 )" + _
                                               "SELECT C.N_ID_CORREO, C.T_DSC_CORREO, C.T_DSC_SUBJECT_CORREO, C.T_DSC_CUERPO_CORREO, " + _
                                               "C.N_FLAG_VIG, C.F_FECH_INI_VIG, C.F_FECH_FIN_VIG " + _
                                               "FROM {0} C " + _
                                               "LEFT OUTER JOIN EXCLUIDOS E on E.N_ID_CORREO = C.N_ID_CORREO " + _
                                               "WHERE C.N_FLAG_VIG = 1 AND E.N_ID_CORREO IS NULL", TablaCorreo, TablaUsuarioCopiaCorreo, idUsuario)
        Dim conexion As New Conexion.SQLServer()

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                lstCorreos.Add(CreaCorreo(dr))
            Next

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return lstCorreos
    End Function

    ''' <summary>
    ''' Agrega copias de correo a usuarios
    ''' </summary>
    ''' <param name="idUsuario">Identificador Usuario</param>
    ''' <param name="lstAgrega">Lista de Correos a agregar copia</param>
    ''' <remarks></remarks>
    Public Sub AgregarCopias(ByVal idUsuario As String, ByVal lstAgrega As List(Of Correo))
        Dim lstCampos As New List(Of String)
        lstCampos.Add("T_ID_USUARIO")
        lstCampos.Add("N_ID_CORREO")
        lstCampos.Add("N_FLAG_VIG")
        lstCampos.Add("F_FECH_INI_VIG")

        Dim lstValores As New List(Of Object)
        Dim inserta As Boolean

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualizar las Copias de Correos", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try
            For Each mail As Correo In lstAgrega
                lstValores.Clear()
                lstValores.Add(idUsuario)
                lstValores.Add(mail.Identificador)
                lstValores.Add(1)
                lstValores.Add(Date.Now)
                Try
                    inserta = conexion.Insertar(TablaUsuarioCopiaCorreo, lstCampos, lstValores)
                    bitacora.Insertar(TablaUsuarioCopiaCorreo, lstCampos, lstValores, inserta, "")
                Catch ex As Exception
                    inserta = False
                    bitacora.Insertar(TablaUsuarioCopiaCorreo, lstCampos, lstValores, inserta, ex.ToString())
                    Throw ex
                End Try
            Next
        Catch ex As Exception
            Throw ex
        Finally
            conexion.CerrarConexion()
            bitacora.Finalizar(inserta)
        End Try

    End Sub

    ''' <summary>
    ''' Quita copias de correo para un usuario
    ''' </summary>
    ''' <param name="idUsuario">Identificador de Usuario</param>
    ''' <param name="lstQuita">Lista de Correo a quitar copia</param>
    ''' <remarks></remarks>
    Public Sub QuitarCopias(ByVal idUsuario As String, ByVal lstQuita As List(Of Correo))
        Dim lstCampos As New List(Of String)
        lstCampos.Add("T_ID_USUARIO")
        lstCampos.Add("N_ID_CORREO")

        Dim lstValores As New List(Of Object)
        Dim elimino As Boolean

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualizar las Copias de Correos", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            For Each mail As Correo In lstQuita

                'Elimina de la tabla de relacion
                lstValores.Clear()
                lstValores.Add(idUsuario)
                lstValores.Add(mail.Identificador)
                Try
                    elimino = conexion.Eliminar(TablaUsuarioCopiaCorreo, lstCampos, lstValores)
                    bitacora.Eliminar(TablaUsuarioCopiaCorreo, lstCampos, lstValores, elimino, "")
                Catch ex As Exception
                    elimino = False
                    bitacora.Eliminar(TablaUsuarioCopiaCorreo, lstCampos, lstValores, elimino, ex.ToString)
                    Throw ex
                End Try
            Next
        Catch ex As Exception
            Throw ex
        Finally
            conexion.CerrarConexion()
            bitacora.Finalizar(elimino)
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza las copias de correos asignados de un usuario
    ''' </summary>
    ''' <param name="lstAgrega">Lista de Correos a asignar</param>
    ''' <param name="lstQuita">Lista de Correos a quitar</param>
    ''' <param name="idUsuario">Id del Usuario</param>
    ''' <remarks></remarks>
    Public Sub GuardaCopiaCorreo(ByVal lstAgrega As List(Of Correo), ByVal lstQuita As List(Of Correo), ByVal idUsuario As String)
        Try
            If lstAgrega.Count > 0 Then
                AgregarCopias(idUsuario, lstAgrega)
            End If

            If lstQuita.Count > 0 Then
                QuitarCopias(idUsuario, lstQuita)
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

#End Region
End Class
