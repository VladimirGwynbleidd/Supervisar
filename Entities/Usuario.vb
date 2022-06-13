'- Fecha de modificación:  15/11/1013
'- Nombre del Responsable: Rafael Rodriguez RARS1
'- Empresa: Softtek
'- Se agrego la propiedad Fecha Inicio

Imports System
Imports System.Web
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient

<Serializable()> _
Public Class Usuario


#Region "Propiedades"

    Public Shared Property SessionID As String = "USUARIO_SESS"

    Public Property IdentificadorUsuario As String
    Public Property Nombre As String = String.Empty
    Public Property Apellido As String = String.Empty
    Public Property ApellidoAuxiliar As String = String.Empty
    Public Property Telefono As String = String.Empty
    Public Property Mail As String = String.Empty
    Public Property Vigente As Boolean = False
    'Public Property Ingeniero As Boolean = False
    'Public Property Autorizador As Boolean = False
    Public Property Existe As Boolean = False
    Public Property InicioVigencia As DateTime
    Public Property FinVigencia As DateTime
    Public Property Perfiles As List(Of Perfil)
    Public Property Contrasena As String = String.Empty
    Public Property ContrasenaSHA512 As String = String.Empty
    Public Property Restablecer As Boolean = False
    Public Property CambiarContrasenia As Boolean = False
    Public Property IdentificadorPerfilActual As Integer
    Public Property FechaIngreso As Date
    'Public Property RFC As String = String.Empty
    'NHM Inicia
    Public Property IdArea As Integer
    Public Property DescArea As String
    Public Property AbreArea As String
    'NHM Fin

#End Region

#Region "Constructores"

    Public Sub New(ByVal identificador As String)
        Me.IdentificadorUsuario = identificador
        CargarDatos()
    End Sub

    Public Sub New()

    End Sub

    Public Sub New(ByVal identificadorUsuario As String, ByVal nombre As String, ByVal apellido As String, _
                   ByVal apellidoAuxiliar As String, ByVal esVigente As Boolean, ByVal inicioVigencia As DateTime, _
                   ByVal finVigencia As DateTime, ByVal perfiles As List(Of Perfil))

        Me.IdentificadorUsuario = identificadorUsuario
        Me.Nombre = nombre
        Me.Apellido = apellido
        Me.ApellidoAuxiliar = apellidoAuxiliar

        Me.Vigente = esVigente
        Me.InicioVigencia = inicioVigencia
        Me.FinVigencia = finVigencia
        Me.Perfiles = perfiles

    End Sub

#End Region

    Public Sub CargarDatos()
        CargarDatos(Me.IdentificadorUsuario)
    End Sub

    Public Sub CargarDatosAD()
        Try
            Dim datos As Dictionary(Of String, String) = Conexion.ActiveDirectory.ObtenerUsuario(Me.IdentificadorUsuario)

            If Not IsNothing(datos) Then

                Me.Nombre = datos.Item("nombre")

                Dim tmpApellidos = datos.Item("apellidos").Split()

                Me.Apellido = tmpApellidos(0)

                Me.ApellidoAuxiliar = String.Empty

                For i = 1 To (tmpApellidos.Length - 1)
                    Me.ApellidoAuxiliar += tmpApellidos(i) + " "
                Next


                Me.Telefono = datos.Item("telefono")
                Me.Mail = datos.Item("mail")

            End If
        Catch ex As Exception

        End Try

    End Sub





    Public Sub CargarDatos(ByVal identificadorUsuario As String)

        Me.IdentificadorUsuario = identificadorUsuario

        Dim conex As Conexion.SQLServer
        Dim datos As DataSet
        Dim dsMenus As DataSet


        conex = New Conexion.SQLServer()

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim dr As SqlClient.SqlDataReader = Nothing

        listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)


        Try
            Dim parametro(0) As SqlParameter
            parametro(0) = New SqlParameter("@T_ID_USUARIO", Me.IdentificadorUsuario)

            Existe = conex.BuscarUnRegistro("BDS_C_GR_USUARIO", listCampos, listValores)
            If Existe Then
                dr = conex.ConsultarRegistrosDR("BDS_C_GR_USUARIO", listCampos, listValores)
                If dr.Read() Then
                    'Me.RFC = CStr(dr("T_DSC_RFC"))
                    'Me.Ingeniero = CBool(dr("B_FLAG_ES_INGENIERO"))
                    'Me.Autorizador = CBool(dr("B_FLAG_ES_AUTORIZADOR"))
                End If
            End If

            If dr IsNot Nothing Then
                If Not dr.IsClosed Then
                    dr.Close() : dr = Nothing
                End If
            End If

            datos = conex.EjecutarSPConsultaDS("ConsultarUsuario", parametro)
            If datos.Tables(0).Rows.Count > 0 Then

                Me.Existe = True
                Me.Nombre = datos.Tables(0).Rows(0)("T_DSC_NOMBRE").ToString().ToString()

                If Not IsDBNull(datos.Tables(0).Rows(0)("T_DSC_APELLIDO")) Then
                    Me.Apellido = datos.Tables(0).Rows(0)("T_DSC_APELLIDO").ToString()
                Else
                    Me.Apellido = ""
                End If

                If Not IsDBNull(datos.Tables(0).Rows(0)("T_DSC_APELLIDO_AUX")) Then
                    Me.ApellidoAuxiliar = datos.Tables(0).Rows(0)("T_DSC_APELLIDO_AUX").ToString()
                Else
                    Me.ApellidoAuxiliar = ""
                End If

                If Not IsDBNull(datos.Tables(0).Rows(0)("T_DSC_TELEFONO")) Then
                    Me.Telefono = datos.Tables(0).Rows(0)("T_DSC_TELEFONO")
                Else
                    Me.Telefono = ""
                End If

                If Not IsDBNull(datos.Tables(0).Rows(0)("T_DSC_MAIL")) Then
                    Me.Mail = datos.Tables(0).Rows(0)("T_DSC_MAIL")
                Else
                    Me.Mail = ""
                End If

                Me.Vigente = CBool(datos.Tables(0).Rows(0)("N_FLAG_VIG"))

                Me.CambiarContrasenia = CBool(datos.Tables(0).Rows(0)("N_FLAG_CONTRASENA"))

                'Me.FechaIngreso = Convert.ToDateTime(datos.Tables(0).Rows(0)("F_FECH_INGRESO"))

                'NHM Inicia
                If Not IsDBNull(datos.Tables(0).Rows(0)("F_FECH_INI_VIG")) Then
                    Me.FechaIngreso = Convert.ToDateTime(datos.Tables(0).Rows(0)("F_FECH_INI_VIG"))

                Else
                    Me.FechaIngreso = DateTime.Now
                End If
                'NHM Fin

            End If

            Dim listPerfil As New List(Of Perfil)

            For indexPerfil = 0 To datos.Tables(1).Rows.Count - 1

                Dim perfil1 As New Perfil

                perfil1.IdentificadorPerfil = datos.Tables(1).Rows(indexPerfil)("N_ID_PERFIL").ToString().ToString()
                perfil1.Descripcion = datos.Tables(1).Rows(indexPerfil)("T_DSC_PERFIL").ToString().ToString()
                perfil1.EsVigente = datos.Tables(1).Rows(indexPerfil)("N_FLAG_VIG").ToString().ToString()
                perfil1.InicioVigencia = datos.Tables(1).Rows(indexPerfil)("F_FECH_INI_VIG").ToString().ToString()
                perfil1.InicioVigencia = datos.Tables(1).Rows(indexPerfil)("F_FECH_INI_VIG").ToString().ToString()
                ' perfil1.AbreArea = datos.Tables(1).Rows(indexPerfil)("T_ABR_AREA").ToString().ToString() 'MMOB SE COMENTÓ

                ' Sye/ alejandro.guevara | Lla columna "T_DSC_AREA" no aparece en BD
                perfil1.DescArea = datos.Tables(1).Rows(indexPerfil)("T_DSC_AREA").ToString().ToString()

                If indexPerfil = 0 Then Me.IdentificadorPerfilActual = perfil1.IdentificadorPerfil

                'NHM Inicia
                Me.IdArea = datos.Tables(1).Rows(indexPerfil)("I_ID_AREA").ToString().ToString()

                '' Me.AbreArea = perfil1.AbreArea 'MMOB SE COMENTÓ
                Me.DescArea = perfil1.DescArea
                'NHM Fin

                Dim parametro1(0) As SqlParameter
                parametro1(0) = New SqlParameter("N_ID_PERFIL", datos.Tables(1).Rows(indexPerfil)("N_ID_PERFIL").ToString().ToString())

                dsMenus = conex.EjecutarSPConsultaDS("ConsultarMenu", parametro1)

                Dim listMenus As New List(Of Menu)

                For indexMenu = 0 To dsMenus.Tables(0).Rows.Count - 1

                    Dim menu1 As New Menu

                    menu1.IdentificadorMenu = dsMenus.Tables(0).Rows(indexMenu)("N_ID_MENU").ToString().ToString()
                    menu1.Descripcion = dsMenus.Tables(0).Rows(indexMenu)("T_DSC_MENU").ToString().ToString()
                    menu1.EsVigente = dsMenus.Tables(0).Rows(indexMenu)("N_FLAG_VIG").ToString().ToString()
                    menu1.InicioVigencia = dsMenus.Tables(0).Rows(indexMenu)("F_FECH_INI_VIG").ToString().ToString()

                    Dim listSubMenus As New List(Of SubMenu)

                    For indexSubMenuRow = 0 To dsMenus.Tables(indexMenu + 1).Rows.Count - 1

                        Dim submenu1 As New SubMenu
                        submenu1.IdentificadorSubMenu = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_ID_SUBMENU").ToString().ToString()
                        submenu1.Descripcion = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_SUBMENU").ToString().ToString()
                        submenu1.UrlPagina = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_URL_PAGINA").ToString().ToString()
                        submenu1.DescripcionPagina = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_PAGINA").ToString().ToString()
                        submenu1.EsVigente = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_FLAG_VIG").ToString().ToString()
                        submenu1.InicioVigencia = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("F_FECH_INI_VIG").ToString().ToString()
                        'submenu1.FinVigencia = dsMenus.Tables(indexSubMenu).Rows(indexSubMenuRow)("F_FECH_FIN_VIG").ToString().ToString()
                        submenu1.Orden = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_NUM_ORDEN").ToString().ToString()

                        listSubMenus.Add(submenu1)

                    Next

                    menu1.SubMenus = listSubMenus
                    'Next

                    listMenus.Add(menu1)



                Next

                perfil1.Menus = listMenus

                listPerfil.Add(perfil1)

            Next

            Me.Perfiles = listPerfil

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If
            If dr IsNot Nothing Then
                If Not dr.IsClosed Then
                    dr.Close() : dr = Nothing
                End If
            End If

        End Try


    End Sub

    ''' <summary>
    ''' Carga los datos de un usuario externo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaDatosExterno()
        Dim conex As Conexion.SQLServer
        Dim dsMenus As DataSet

        conex = New Conexion.SQLServer()

        Try
            'Obtengo Id de Perfil
            Dim lstCampos As New List(Of String)
            lstCampos.Add("N_ID_PERFIL")
            Dim lstCamposCondicion As New List(Of String)
            Dim lstCamposValores As New List(Of Object)
            lstCamposCondicion.Add("T_DSC_PERFIL") : lstCamposValores.Add("Externo")
            lstCamposCondicion.Add("N_FLAG_VIG") : lstCamposValores.Add(1)
            Dim dt As DataTable = conex.ConsultarRegistrosDT(lstCampos, "BDS_C_GR_PERFIL", lstCamposCondicion, lstCamposValores)
            Dim idPerfilExterno As Integer = -1
            If dt.Rows.Count > 0 Then
                Me.IdentificadorPerfilActual = Convert.ToInt32(dt.Rows(0)(0))
            End If

            Me.Perfiles = New List(Of Perfil)

            Me.Perfiles.Add(New Perfil(Me.IdentificadorPerfilActual))

            Dim parametro1(0) As SqlParameter
            parametro1(0) = New SqlParameter("N_ID_PERFIL", Me.IdentificadorPerfilActual.ToString())

            dsMenus = conex.EjecutarSPConsultaDS("ConsultarMenu", parametro1)

            Dim listMenus As New List(Of Menu)

            For indexMenu = 0 To dsMenus.Tables(0).Rows.Count - 1

                Dim menu1 As New Menu

                menu1.IdentificadorMenu = dsMenus.Tables(0).Rows(indexMenu)("N_ID_MENU").ToString().ToString()
                menu1.Descripcion = dsMenus.Tables(0).Rows(indexMenu)("T_DSC_MENU").ToString().ToString()
                menu1.EsVigente = dsMenus.Tables(0).Rows(indexMenu)("N_FLAG_VIG").ToString().ToString()
                menu1.InicioVigencia = dsMenus.Tables(0).Rows(indexMenu)("F_FECH_INI_VIG").ToString().ToString()

                Dim listSubMenus As New List(Of SubMenu)

                For indexSubMenuRow = 0 To dsMenus.Tables(indexMenu + 1).Rows.Count - 1

                    Dim submenu1 As New SubMenu
                    submenu1.IdentificadorSubMenu = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_ID_SUBMENU").ToString().ToString()
                    submenu1.Descripcion = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_SUBMENU").ToString().ToString()
                    submenu1.UrlPagina = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_URL_PAGINA").ToString().ToString()
                    submenu1.DescripcionPagina = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_PAGINA").ToString().ToString()
                    submenu1.EsVigente = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_FLAG_VIG").ToString().ToString()
                    submenu1.InicioVigencia = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("F_FECH_INI_VIG").ToString().ToString()
                    'submenu1.FinVigencia = dsMenus.Tables(indexSubMenu).Rows(indexSubMenuRow)("F_FECH_FIN_VIG").ToString().ToString() 
                    submenu1.Orden = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_NUM_ORDEN").ToString().ToString()

                    listSubMenus.Add(submenu1)

                Next

                menu1.SubMenus = listSubMenus
                'Next

                listMenus.Add(menu1)
            Next


        Catch ex As Exception

            Throw ex
        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If
        End Try

    End Sub

    Public Function EsValido(ByVal usuario, ByVal contrasena)

        contrasena = Utilerias.Cifrado.CifrarSHA512(contrasena)

        Dim conex As New Conexion.SQLServer

        Dim Campos As New List(Of String)
        Dim Valores As New List(Of Object)

        Campos.Add("T_ID_USUARIO") : Valores.Add(usuario)
        Campos.Add("T_DSC_CONTRASENA") : Valores.Add(contrasena)

        Try

            Return conex.BuscarUnRegistro("BDS_C_GR_USUARIO", Campos, Valores)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try


    End Function

    Public Function EstaRegistrado(ByVal usuario, ByVal mail)

        Dim conex As New Conexion.SQLServer

        Dim Campos As New List(Of String)
        Dim Valores As New List(Of Object)

        Campos.Add("T_ID_USUARIO") : Valores.Add(usuario)
        Campos.Add("T_DSC_MAIL") : Valores.Add(mail)


        Try

            Return conex.BuscarUnRegistro("BDS_C_GR_USUARIO", Campos, Valores)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try



    End Function

    Public Sub CargarDatosPorMail(ByVal Mail As String)

        Dim conex As New Conexion.SQLServer
        Dim parametro1(0) As SqlParameter

        parametro1(0) = New SqlParameter("@T_DSC_MAIL", Mail)

        Try

            Dim reader As SqlDataReader = conex.EjecutarSPConsultaDR("UsuarioPorMail", parametro1)

            If reader.Read() Then

                Me.IdentificadorUsuario = CStr(reader("T_ID_USUARIO"))
                CargarDatos()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try



    End Sub

    Public Function ActualizarContrasenaAnonimo() As Boolean

        Dim sessionAnonimo As String = "0000000000"
        Dim identificadorAnonimo As String = "anonimo"


        Dim conex As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Contraseña" & "(" & Me.IdentificadorUsuario.ToString() & ")", sessionAnonimo, identificadorAnonimo)


        Dim resultado As Boolean

        Try

            GenerarContrasena()

            Dim parametro1(0) As SqlParameter

            Dim Campo As New List(Of String)
            Dim Valor As New List(Of Object)

            Dim CampoCondicion As New List(Of String)
            Dim ValorCondicion As New List(Of Object)


            Campo.Add("T_DSC_CONTRASENA") : Valor.Add(Me.ContrasenaSHA512)
            Campo.Add("N_FLAG_CONTRASENA") : Valor.Add(1)

            CampoCondicion.Add("T_ID_USUARIO") : ValorCondicion.Add(Me.IdentificadorUsuario)

            resultado = conex.Actualizar("BDS_C_GR_USUARIO", Campo, Valor, CampoCondicion, ValorCondicion)
            bitacora.Actualizar("BDS_C_GR_USUARIO", Campo, Valor, CampoCondicion, ValorCondicion, resultado, "")

            bitacora.Finalizar(resultado)

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try



    End Function

    Public Function ActualizarContrasena() As Boolean



        Dim conex As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Contraseña" & "(" & Me.IdentificadorUsuario.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim resultado As Boolean

        Try


            GenerarContrasena()

            Dim parametro1(0) As SqlParameter

            Dim Campo As New List(Of String)
            Dim Valor As New List(Of Object)

            Dim CampoCondicion As New List(Of String)
            Dim ValorCondicion As New List(Of Object)


            Campo.Add("T_DSC_CONTRASENA") : Valor.Add(Me.ContrasenaSHA512)
            Campo.Add("N_FLAG_CONTRASENA") : Valor.Add(1)

            CampoCondicion.Add("T_ID_USUARIO") : ValorCondicion.Add(Me.IdentificadorUsuario)

            resultado = conex.Actualizar("BDS_C_GR_USUARIO", Campo, Valor, CampoCondicion, ValorCondicion)
            bitacora.Actualizar("BDS_C_GR_USUARIO", Campo, Valor, CampoCondicion, ValorCondicion, resultado, "")

            bitacora.Finalizar(resultado)

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try





    End Function

    Public Sub RegistrarSesion()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposBuscar As New List(Of String)
            Dim listValoresBuscar As New List(Of Object)

            listCamposBuscar.Add("T_ID_USUARIO") : listValoresBuscar.Add(Me.IdentificadorUsuario)
            listCamposBuscar.Add("T_ID_SESION") : listValoresBuscar.Add(System.Web.HttpContext.Current.Session.SessionID)

            listCampos.Add("T_ID_SESION") : listValores.Add(System.Web.HttpContext.Current.Session.SessionID)
            listCampos.Add("T_DSC_DIRECCION_IP") : listValores.Add(System.Web.HttpContext.Current.Request.UserHostAddress)
            listCampos.Add("F_FECH_INI") : listValores.Add(System.DateTime.Now)
            listCampos.Add("F_FECH_FIN") : listValores.Add(System.DateTime.Now)
            listCampos.Add("N_NUM_TIEMPO_VIDA") : listValores.Add(20)
            listCampos.Add("N_FLAG_ACTIVO") : listValores.Add(1)
            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)

            If conexion.BuscarUnRegistro("BDS_D_RG_SESION", listCamposBuscar, listValoresBuscar) Then
                conexion.Actualizar("BDS_D_RG_SESION", listCampos, listValores, listCamposBuscar, listValoresBuscar)
            Else
                conexion.Insertar("BDS_D_RG_SESION", listCampos, listValores)
            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Sub

    Public Sub RegistrarSesionSisvig(psSesionSisvig As String)

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposBuscar As New List(Of String)
            Dim listValoresBuscar As New List(Of Object)

            listCamposBuscar.Add("T_ID_USUARIO") : listValoresBuscar.Add(Me.IdentificadorUsuario)
            listCamposBuscar.Add("T_ID_SESION") : listValoresBuscar.Add(psSesionSisvig)

            listCampos.Add("T_ID_SESION") : listValores.Add(psSesionSisvig)
            listCampos.Add("T_DSC_DIRECCION_IP") : listValores.Add("127.0.0.1")
            listCampos.Add("F_FECH_INI") : listValores.Add(System.DateTime.Now)
            listCampos.Add("F_FECH_FIN") : listValores.Add(System.DateTime.Now)
            listCampos.Add("N_NUM_TIEMPO_VIDA") : listValores.Add(20)
            listCampos.Add("N_FLAG_ACTIVO") : listValores.Add(1)
            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)

            If conexion.BuscarUnRegistro("BDS_D_RG_SESION", listCamposBuscar, listValoresBuscar) Then
                conexion.Actualizar("BDS_D_RG_SESION", listCampos, listValores, listCamposBuscar, listValoresBuscar)
            Else
                conexion.Insertar("BDS_D_RG_SESION", listCampos, listValores)
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Sub

    Public Sub ActualizarSesion()

        Dim conexion As New Conexion.SQLServer
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("T_ID_SESION") : listValores.Add(System.Web.HttpContext.Current.Session.SessionID)
            listCampos.Add("T_DSC_DIRECCION_IP") : listValores.Add(System.Web.HttpContext.Current.Request.UserHostAddress)
            listCampos.Add("F_FECH_INI") : listValores.Add(System.DateTime.Now)
            listCampos.Add("F_FECH_FIN") : listValores.Add(System.DateTime.Now)
            listCampos.Add("N_NUM_TIEMPO_VIDA") : listValores.Add(20)
            listCampos.Add("N_FLAG_ACTIVO") : listValores.Add(1)
            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)
            conexion.Actualizar("BDS_D_RG_SESION", listCampos, listValores)

        Catch ex As Exception

            Throw ex


        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Sub

    Public Sub FinalizarSesion()

        Dim conexion As New Conexion.SQLServer
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try

            listCamposCondicion.Add("T_ID_SESION") : listValoresCondicion.Add(System.Web.HttpContext.Current.Session.SessionID)
            'listCampos.Add("T_DSC_DIRECCION_IP") : listValores.Add(System.Web.HttpContext.Current.Request.UserHostAddress)
            'listCampos.Add("F_FECH_INI") : listValores.Add(System.DateTime.Now)
            listCampos.Add("F_FECH_FIN") : listValores.Add(System.DateTime.Now)
            'listCampos.Add("N_NUM_TIEMPO_VIDA") : listValores.Add(20)
            listCampos.Add("N_FLAG_ACTIVO") : listValores.Add(0)
            'listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)
            conexion.Actualizar("BDS_D_RG_SESION", listCampos, listValores, listCamposCondicion, listValoresCondicion)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Sub

    Public Shared Function UsuariosSoloDatosGetCustom(ByVal where As String) As List(Of Usuario)

        Dim resultado As New List(Of Usuario)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtUsuarios As DataTable = conexion.ConsultarDT("SELECT T_ID_USUARIO, T_DSC_NOMBRE, T_DSC_APELLIDO, T_DSC_APELLIDO_AUX, " & _
                                                               "T_DSC_TELEFONO, T_DSC_MAIL, N_FLAG_VIG FROM BDS_C_GR_USUARIO " & _
                                                               where & " ORDER BY T_ID_USUARIO")

            For Each renglon As DataRow In dtUsuarios.Rows

                resultado.Add(New Usuario With {.IdentificadorUsuario = renglon("T_ID_USUARIO").ToString, _
                                                .Nombre = renglon("T_DSC_NOMBRE").ToString, _
                                                .Apellido = renglon("T_DSC_APELLIDO").ToString, _
                                                .ApellidoAuxiliar = renglon("T_DSC_APELLIDO_AUX").ToString, _
                                                .Vigente = CBool(renglon("N_FLAG_VIG")), _
                                                .Telefono = renglon("T_DSC_TELEFONO").ToString, _
                                                .Mail = renglon("T_DSC_MAIL").ToString})



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

    Public Function ObtenerAutorizadores() As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim dt As New DataTable
        Dim query As String
        Try
            query = "SELECT T_ID_USUARIO, T_DSC_NOMBRE, T_DSC_APELLIDO, T_DSC_APELLIDO_AUX, T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + " & _
                    "T_DSC_APELLIDO_AUX AS T_DSC_NOMBRE_COMPLETO FROM BDS_C_GR_USUARIO " & _
                    "WHERE N_FLAG_VIG = 1 And B_FLAG_ES_AUTORIZADOR = 1 "
            dt = conexion.ConsultarDT(query)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim campos As New List(Of String)
         campos.Add("U.T_ID_USUARIO")
         campos.Add("U.T_DSC_NOMBRE")
         campos.Add("U.T_DSC_APELLIDO")
         campos.Add("U.T_DSC_APELLIDO_AUX")
         campos.Add("U.T_DSC_CONTRASENA")
         campos.Add("U.T_DSC_TELEFONO")
         campos.Add("U.T_DSC_MAIL")
         campos.Add("U.N_FLAG_VIG")
         campos.Add("U.F_FECH_INI_VIG")
         campos.Add("U.F_FECH_FIN_VIG")
         campos.Add("U.N_FLAG_CONTRASENA")
            'campos.Add("F_FECH_INGRESO")
            'campos.Add("B_FLAG_ES_INGENIERO")
            'campos.Add("B_FLAG_ES_AUTORIZADOR")
            'campos.Add("T_DSC_RFC")

         campos.Add("U.T_DSC_NOMBRE + ' ' + isnull(U.T_DSC_APELLIDO, '') + ' ' + isnull(U.T_DSC_APELLIDO_AUX, '') AS NOMBRE_COMPLETO")
         campos.Add("A.T_DSC_AREA")
         campos.Add("P.T_DSC_PERFIL")

         Return conexion.ConsultarRegistrosDT(campos, "BDS_C_GR_USUARIO U JOIN BDS_R_GR_USUARIO_PERFIL UP ON UP.T_ID_USUARIO = U.T_ID_USUARIO JOIN BDS_C_GR_AREAS A ON A.I_ID_AREA = UP.I_ID_AREA JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = UP.N_ID_PERFIL", "T_ID_USUARIO").DefaultView

            'Return conexion.ConsultarRegistrosDT("BDS_C_GR_USUARIO", "T_ID_USUARIO").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function


    Public Shared Function ObtenerTodosFiltro() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim campos As New List(Of String)
         campos.Add("U.T_ID_USUARIO")
         campos.Add("U.T_DSC_NOMBRE")
         campos.Add("U.T_DSC_APELLIDO")
         campos.Add("U.T_DSC_APELLIDO_AUX")
         campos.Add("U.T_DSC_CONTRASENA")
         campos.Add("U.T_DSC_TELEFONO")
         campos.Add("U.T_DSC_MAIL")
         campos.Add("U.N_FLAG_VIG")
         campos.Add("U.F_FECH_INI_VIG")
         campos.Add("U.F_FECH_FIN_VIG")
         campos.Add("U.N_FLAG_CONTRASENA")
            'campos.Add("F_FECH_INGRESO")
            'campos.Add("B_FLAG_ES_INGENIERO")
            'campos.Add("B_FLAG_ES_AUTORIZADOR")
            'campos.Add("T_DSC_RFC")

         campos.Add("U.T_DSC_NOMBRE + ' ' + isnull(U.T_DSC_APELLIDO, '') + ' ' + isnull(U.T_DSC_APELLIDO_AUX, '') AS NOMBRE_COMPLETO")
         campos.Add("A.T_DSC_AREA")
         campos.Add("P.T_DSC_PERFIL")

         Return conexion.ConsultarRegistrosDT(campos, "BDS_C_GR_USUARIO U JOIN BDS_R_GR_USUARIO_PERFIL UP ON UP.T_ID_USUARIO = U.T_ID_USUARIO JOIN BDS_C_GR_AREAS A ON A.I_ID_AREA = UP.I_ID_AREA JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = UP.N_ID_PERFIL", "T_ID_USUARIO")

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Shared Function ObtenerIngenieros() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try
            Dim query As String
            query = "SELECT T_ID_USUARIO, T_DSC_NOMBRE + ' ' + isnull(T_DSC_APELLIDO, '') + ' ' + isnull(T_DSC_APELLIDO_AUX, '') AS NOMBRE_COMPLETO "
            query += "FROM BDS_C_GR_USUARIO "
            query += "WHERE B_FLAG_ES_INGENIERO = 1 AND N_FLAG_VIG = 1"
            Return conexion.ConsultarDT(query)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Function

    Public Sub GenerarContrasena()

        Contrasena = System.Web.Security.Membership.GeneratePassword(10, 1)
        ContrasenaSHA512 = Utilerias.Cifrado.CifrarSHA512(Contrasena)

    End Sub


    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer


        Try


            Dim trans As SqlClient.SqlTransaction = conexion.BeginTransaction



            Dim bitacora As New Conexion.Bitacora("Registro de nuevo usuario" & "(" & Me.IdentificadorUsuario.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)
            listCampos.Add("T_DSC_NOMBRE") : listValores.Add(Me.Nombre)
            listCampos.Add("T_DSC_APELLIDO") : listValores.Add(Me.Apellido)
            listCampos.Add("T_DSC_APELLIDO_AUX") : listValores.Add(Me.ApellidoAuxiliar)
            listCampos.Add("T_DSC_TELEFONO") : listValores.Add(Me.Telefono)
            listCampos.Add("T_DSC_MAIL") : listValores.Add(Me.Mail)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)
            'listCampos.Add("F_FECH_INGRESO") : listValores.Add(Me.FechaIngreso)
            'listCampos.Add("B_FLAG_ES_INGENIERO") : listValores.Add(Me.Ingeniero)
            'listCampos.Add("B_FLAG_ES_AUTORIZADOR") : listValores.Add(Me.Autorizador)
            'listCampos.Add("T_DSC_RFC") : listValores.Add(Me.RFC)

            resultado = conexion.InsertarConTransaccion("BDS_C_GR_USUARIO", listCampos, listValores, trans)
            bitacora.InsertarConTransaccion("BDS_C_GR_USUARIO", listCampos, listValores, resultado, "Error al registrar nuevo usuario")

            If resultado Then

                listCampos.Clear()
                listValores.Clear()

                listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)
                listCampos.Add("N_ID_PERFIL") : listValores.Add(Me.Perfiles(0).IdentificadorPerfil)
                listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
                listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)
                'NHM inicia
                listCampos.Add("I_ID_AREA") : listValores.Add(Me.IdArea)
                'NHM fin

                resultado = conexion.InsertarConTransaccion("BDS_R_GR_USUARIO_PERFIL", listCampos, listValores, trans)
                bitacora.InsertarConTransaccion("BDS_R_GR_USUARIO_PERFIL", listCampos, listValores, resultado, "Error al registrar nuevo usuario")

            End If

            If resultado Then
                trans.Commit()
                bitacora.Finalizar(True)
            Else
                trans.Rollback()
                bitacora.Finalizar(False)
            End If

            If Me.Restablecer Then
                ActualizarContrasena()
            End If

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False


        Dim conexion As New Conexion.SQLServer

        Try


            Dim trans As SqlClient.SqlTransaction = conexion.BeginTransaction

            Dim bitacora As New Conexion.Bitacora("Actualización de usuario" & "(" & Me.IdentificadorUsuario.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_NOMBRE") : listValores.Add(Me.Nombre)
            listCampos.Add("T_DSC_APELLIDO") : listValores.Add(Me.Apellido)
            listCampos.Add("T_DSC_APELLIDO_AUX") : listValores.Add(Me.ApellidoAuxiliar)
            listCampos.Add("T_DSC_TELEFONO") : listValores.Add(Me.Telefono)
            listCampos.Add("T_DSC_MAIL") : listValores.Add(Me.Mail)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)
            'listCampos.Add("F_FECH_INGRESO") : listValores.Add(Me.FechaIngreso)
            'listCampos.Add("B_FLAG_ES_INGENIERO") : listValores.Add(Me.Ingeniero)
            'listCampos.Add("B_FLAG_ES_AUTORIZADOR") : listValores.Add(Me.Autorizador)
            'listCampos.Add("T_DSC_RFC") : listValores.Add(Me.RFC)

            listCamposCondicion.Add("T_ID_USUARIO") : listValoresCondicion.Add(Me.IdentificadorUsuario)

            resultado = conexion.ActualizarConTransaccion("BDS_C_GR_USUARIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, trans)
            bitacora.ActualizarConTransaccion("BDS_C_GR_USUARIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar usuario")

            If resultado Then

                listCampos.Clear()
                listValores.Clear()
                listCamposCondicion.Clear()
                listValoresCondicion.Clear()

                listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdentificadorUsuario)
                listCampos.Add("N_ID_PERFIL") : listValores.Add(Me.Perfiles(0).IdentificadorPerfil)
                listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)
                'NHM inicia
                listCampos.Add("I_ID_AREA") : listValores.Add(Me.IdArea)
                'NHM fin

                listCamposCondicion.Add("T_ID_USUARIO") : listValoresCondicion.Add(Me.IdentificadorUsuario)
                listCamposCondicion.Add("N_ID_PERFIL") : listValoresCondicion.Add(Me.IdentificadorPerfilActual)

                resultado = conexion.ActualizarConTransaccion("BDS_R_GR_USUARIO_PERFIL", listCampos, listValores, listCamposCondicion, listValoresCondicion, trans)
                bitacora.ActualizarConTransaccion("BDS_R_GR_USUARIO_PERFIL", listCampos, listValores, resultado, "Error al registrar nuevo usuario")

            End If

            If resultado Then
                trans.Commit()
                bitacora.Finalizar(True)
            Else
                trans.Rollback()
                bitacora.Finalizar(False)
            End If

            If Me.Restablecer Then
                ActualizarContrasena()
            End If

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Borrar usuario" & "(" & Me.IdentificadorUsuario.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("T_ID_USUARIO") : listValoresCondicion.Add(Me.IdentificadorUsuario)

            resultado = conexion.Actualizar("BDS_C_GR_USUARIO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_USUARIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar usuasrio")

            bitacora.Finalizar(resultado)

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    Public Function ObtenerIngenierosAgenda() As DataView
        Dim resultado As New DataTable
        resultado.Columns.Add("T_ID_INGENIERO")
        resultado.Columns.Add("NombreING")

        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty
        Try
            sql = "SELECT A.T_ID_INGENIERO ,A.T_ID_INGENIERO + ' - ' + B.T_DSC_NOMBRE + ' ' + B.T_DSC_APELLIDO  NombreING" + Chr(13)
            sql += "FROM  BDS_D_TI_AGENDA A, BDS_C_GR_USUARIO B" + Chr(13)
            sql += "WHERE A.T_ID_INGENIERO = B.T_ID_USUARIO" + Chr(13)
            sql += "GROUP BY A.T_ID_INGENIERO ,A.T_ID_INGENIERO + ' - ' + B.T_DSC_NOMBRE + ' ' + B.T_DSC_APELLIDO" + Chr(13)

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(sql)

            While dr.Read()
                Dim row As DataRow
                row = resultado.NewRow()
                row("T_ID_INGENIERO") = dr("T_ID_INGENIERO")
                row("NombreING") = dr("NombreING")
                resultado.Rows.Add(row)
            End While

            Return resultado.AsDataView
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    Public Function ObtenerDatosAgendaIng(ByVal IdIng As String, ByVal Mes As Integer, ByVal Anio As Integer, ByVal TipoBusqueda As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty
        Try
            Dim parametro As New Parametros(46)

            sql = "SELECT N_ID_REGISTRO_AGENDA," + Chr(13)
            If parametro.ValorParametro = 0 Then
                sql += "      F_FECH_FECHA_HORA_TAREA," + Chr(13)
            Else
                sql += "      CAST(CONVERT(VARCHAR,F_FECH_FECHA_HORA_TAREA,101) AS DATETIME)  F_FECH_FECHA_HORA_TAREA," + Chr(13)
            End If

            sql += "      T_DSC_TAREA_AGENDA," + Chr(13)
            sql += "      N_ID_TIPO_TAREA" + Chr(13)
            sql += "FROM BDS_D_TI_AGENDA" + Chr(13)
            sql += "WHERE T_ID_INGENIERO = '" + IdIng + "'" + Chr(13)
            sql += "AND N_ID_CONSECUTIVO_SOPORTE IS NULL "
            sql += "      AND MONTH(F_FECH_FECHA_HORA_TAREA) = " + Mes.ToString() + Chr(13)
            sql += "      AND YEAR(F_FECH_FECHA_HORA_TAREA) = " + Anio.ToString() + Chr(13)
            If TipoBusqueda = 1 Then
                sql += "      AND N_ID_TIPO_TAREA = 2" + Chr(13)
                sql += "      AND N_ID_REGISTRO_AGENDA IN(SELECT N_ID_REGISTRO_AGENDA FROM BDS_D_TI_REGISTRO_AGENDA B" + Chr(13)
                sql += "                                 WHERE B.N_ID_TIPO_ACTIVIDAD is not null" + Chr(13)
                sql += "                                       AND B.N_ID_TIPO_AUSENCIA is null)" + Chr(13)
            ElseIf TipoBusqueda = 2 Then
                sql += "      AND N_ID_TIPO_TAREA = 2" + Chr(13)
                sql += "      AND N_ID_REGISTRO_AGENDA IN(SELECT N_ID_REGISTRO_AGENDA FROM BDS_D_TI_REGISTRO_AGENDA B" + Chr(13)
                sql += "                                 WHERE B.N_ID_TIPO_ACTIVIDAD is null" + Chr(13)
                sql += "                                       AND B.N_ID_TIPO_AUSENCIA > 0)" + Chr(13)
            ElseIf TipoBusqueda = 3 Then
                sql += "      AND N_ID_TIPO_TAREA = 1" + Chr(13)
            End If
            If parametro.ValorParametro = 0 Then
                sql += "GROUP BY N_ID_REGISTRO_AGENDA,F_FECH_FECHA_HORA_TAREA, T_DSC_TAREA_AGENDA, N_ID_TIPO_TAREA" + Chr(13)
            Else
                sql += "GROUP BY N_ID_REGISTRO_AGENDA, CAST(CONVERT(VARCHAR,F_FECH_FECHA_HORA_TAREA,101) AS DATETIME), T_DSC_TAREA_AGENDA, N_ID_TIPO_TAREA" + Chr(13)
            End If

            sql += "ORDER BY F_FECH_FECHA_HORA_TAREA " + Chr(13)

            Dim ds As DataSet = conexion.ConsultarDS(sql)

            Return ds.Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    Public Function ObtenFolioSolAgenda(ByVal Id As Integer) As String
        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty
        Dim folio As String = String.Empty
        Try
            sql = "SELECT TOP 1 SL.N_NUM_FOLIO_ID ,SL.N_NUM_FOLIO_ANIO FROM BDS_D_GR_SOLICITUD_SERVICIO SL " & _
                    "INNER JOIN BDS_D_TI_AGENDA AG ON SL.N_ID_SOLICITUD_SERVICIO = AG.N_ID_SOLICITUD_SERVICIO " & _
                    "INNER JOIN BDS_D_TI_REGISTRO_AGENDA RG ON RG.N_ID_REGISTRO_AGENDA = AG.N_ID_REGISTRO_AGENDA " & _
                    "WHERE RG.N_ID_REGISTRO_AGENDA = " & Id.ToString()
            Dim ds As DataSet = conexion.ConsultarDS(sql)
            If ds.Tables(0).Rows.Count Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    folio = dr("N_NUM_FOLIO_ID").ToString() & "-" & dr("N_NUM_FOLIO_ANIO").ToString()
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return folio
    End Function

    Public Function ObtenDetalleAgenda(ByVal Id As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty
        Try
            sql = "SELECT	B.F_FECH_FECHA_HORA_TAREA, " + Chr(13)
            sql += "         B.T_DSC_TAREA_AGENDA," + Chr(13)
            sql += "         A.B_FLAG_CICLICA," + Chr(13)
            sql += "         A.B_FLAG_LUNES Lunes," + Chr(13)
            sql += "         A.B_FLAG_MARTES Martes," + Chr(13)
            sql += "         A.B_FLAG_MIERCOLES Miercoles," + Chr(13)
            sql += "         A.B_FLAG_JUEVES Jueves," + Chr(13)
            sql += "         A.B_FLAG_VIERNES Viernes" + Chr(13)
            sql += "FROM BDS_D_TI_REGISTRO_AGENDA A, BDS_D_TI_AGENDA B" + Chr(13)
            sql += "WHERE A.N_ID_REGISTRO_AGENDA = " + Id.ToString() + Chr(13)
            sql += "      AND A.N_ID_REGISTRO_AGENDA = B.N_ID_REGISTRO_AGENDA" + Chr(13)
            Dim ds As DataSet = conexion.ConsultarDS(sql)

            Return ds.Tables(0)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function


#Region "Para Cambio de Contrasenia"

    Public Function CambioContrasenia() As Boolean

        Dim conex As New Conexion.SQLServer
        Dim resultado As Boolean = False

        Try

            ' Validamos reglas para formación de contraseña
            ValidaContrasenia()

            ' Iniciamos bitacora
            Dim bitacora As New Conexion.Bitacora("Cambio de Contraseña" & "(" & Me.IdentificadorUsuario.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            ' ciframos contraseña
            ContrasenaSHA512 = Utilerias.Cifrado.CifrarSHA512(Me.Contrasena)


            Dim Campo As New List(Of String)
            Dim Valor As New List(Of Object)

            Dim CampoCondicion As New List(Of String)
            Dim ValorCondicion As New List(Of Object)


            Campo.Add("T_DSC_CONTRASENA") : Valor.Add(Me.ContrasenaSHA512)
            Campo.Add("N_FLAG_CONTRASENA") : Valor.Add(0)

            CampoCondicion.Add("T_ID_USUARIO") : ValorCondicion.Add(Me.IdentificadorUsuario)

            ' actualizamos
            resultado = conex.Actualizar("BDS_C_GR_USUARIO", Campo, Valor, CampoCondicion, ValorCondicion)
            bitacora.Actualizar("BDS_C_GR_USUARIO", Campo, Valor, CampoCondicion, ValorCondicion, resultado, "")

            bitacora.Finalizar(resultado)



            resultado = True

        Catch ex0 As ValidaContraseniaException

            Throw ex0

        Catch ex As Exception

            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            resultado = False

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    Private Sub ValidaContrasenia()


        ' detectamos si la contraseña es la misma que el sistema le dió
        If Me.EsValido(Me.IdentificadorUsuario, Me.Contrasena) Then

            Throw New ValidaContraseniaException("La contraseña no puede ser la misma que la actual, verifique por favor.")

        End If


        ' verificamos regla de negocio para generación de contraseña
        If Me.Contrasena.Length < 6 OrElse Me.Contrasena.Length > 30 Then

            Throw New ValidaContraseniaException("La longitud mínima para la contraseña es de 6 caracteres y la máxima de 30, verifique por favor.")

        End If


        ' verificamos caracteres válidos
        Dim regex As New System.Text.RegularExpressions.Regex("^(?=.*\d)(?=.*[A-Za-z])(?!.*[!@#\$%\^&\*\(\)\+=\|;'""{}<>\.\?\-_\\/:,~`]).{6,30}$")

        If Not regex.IsMatch(Me.Contrasena) Then

            Throw New ValidaContraseniaException("Los caracteres válidos para la contraseña son letras (mayúsculas o minúsculas) y números, y debe contener por lo menos uno de ambos, verifique por favor.")

        End If


        '' verificamos que por lo menos tenga una letra
        'Dim flagCaracter As Boolean = False

        'For Each letra As Char In Me.Contrasena

        '    flagCaracter = Char.IsLetter(letra)
        '    If flagCaracter Then Exit For

        'Next

        'If Not flagCaracter Then Throw New ValidaContraseniaException("Debe contener al menos una letra")


        '' verificamos que por lo menos tenga un numero
        'flagCaracter = False
        'For Each letra As Char In Me.Contrasena

        '    flagCaracter = Char.IsDigit(letra)
        '    If flagCaracter Then Exit For

        'Next

        'If Not flagCaracter Then Throw New ValidaContraseniaException("Debe contener al menos un dígito")


    End Sub

    ''' <summary>
    ''' Excepcion para el proceso de validacion de contraseña
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ValidaContraseniaException
        Inherits Exception

        Public Sub New(ByVal Mensaje As String)
            MyBase.New(Mensaje)
        End Sub

        Public Sub New(ByVal Message As String, ByVal InnerException As Exception)
            MyBase.New(Message, InnerException)
        End Sub

    End Class

#End Region

    Public Function DatosPantallaControles() As DataView

        Dim conex As Conexion.SQLServer

        conex = New Conexion.SQLServer()

        Try

            Return conex.EjecutarSPConsultaDT("PantallaControles").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try


    End Function

    Public Function ObtenerTodosPorArea(piIdArea As Integer) As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim lsQuery As String = ""
         lsQuery = "SELECT U.N_FLAG_CONTRASENA, U.T_ID_USUARIO, T_DSC_NOMBRE, T_DSC_APELLIDO,T_DSC_APELLIDO_AUX,T_DSC_CONTRASENA,T_DSC_TELEFONO,T_DSC_MAIL,U.N_FLAG_VIG, U.F_FECH_INI_VIG, U.F_FECH_FIN_VIG, T_DSC_NOMBRE + ' ' + isnull(T_DSC_APELLIDO, '') + ' ' + isnull(T_DSC_APELLIDO_AUX, '') AS NOMBRE_COMPLETO, A.T_DSC_AREA, P.T_DSC_PERFIL FROM BDS_C_GR_USUARIO U JOIN BDS_R_GR_USUARIO_PERFIL UP ON(U.T_ID_USUARIO = UP.T_ID_USUARIO AND I_ID_AREA = " & piIdArea.ToString() & ") JOIN BDS_C_GR_AREAS A ON A.I_ID_AREA = UP.I_ID_AREA JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = UP.N_ID_PERFIL "

            Return conexion.ConsultarDT(lsQuery).DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function AgregarUsuarioVO() As Boolean
        Dim registroExitoso As Boolean = True
        Dim conexion As New Conexion.SQLServer

        Try

            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_INS_USUARIO_SISVIG")

            Dim sqlParameter(3) As SqlParameter
            sqlParameter(0) = New SqlParameter("@T_ID_USUARIO", Me.IdentificadorUsuario)
            sqlParameter(1) = New SqlParameter("@T_ID_USUARIO_NOMBRE", Me.Nombre)
            sqlParameter(2) = New SqlParameter("@T_ID_USUARIO_AP", Me.Apellido)
            sqlParameter(3) = New SqlParameter("@I_ID_PERFIL", Me.IdentificadorPerfilActual)

            registroExitoso = conexion.EjecutarSP(sp, sqlParameter)

        Catch ex As Exception
            registroExitoso = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, AgregarUsuarioVO", "")
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

        Return registroExitoso

    End Function

    Public Shared Function BuscaSecionNueva() As Boolean
        Dim existeNueva As Boolean = False
        Dim objUsuario = TryCast(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario)
        If Not objUsuario Is Nothing Then
            Dim usuario As String = objUsuario.IdentificadorUsuario

            Dim miSession As String = System.Web.HttpContext.Current.Session.SessionID

            Dim strQuery As String = "SELECT T_ID_SESION " & _
                                     " FROM BDS_D_RG_SESION" & _
                                     " WHERE T_ID_USUARIO = '" & usuario & "'" & _
                                     " AND F_FECH_INI = (SELECT MAX(F_FECH_INI) FROM BDS_D_RG_SESION WHERE T_ID_USUARIO = '" & usuario & "')"


            Dim conexion As New Conexion.SQLServer

            Try
                Dim dt As DataTable = conexion.ConsultarDT(strQuery)

                If dt.Rows.Count > 0 Then
                    If Not IsDBNull(dt.Rows(0)("T_ID_SESION")) Then
                        If dt.Rows(0)("T_ID_SESION") <> miSession Then
                            If dt.Rows(0)("T_ID_SESION") <> SessionID Then
                                existeNueva = True
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                existeNueva = False
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

            Finally

                If Not IsNothing(conexion) Then
                    conexion.CerrarConexion()
                End If
            End Try
        End If
        Return existeNueva
    End Function

End Class
