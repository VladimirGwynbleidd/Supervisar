''' <summary>
''' Clase que da de alta una visita
''' </summary>
''' <remarks></remarks>
Public Class Registro
    Public objVisita As Visita

    Public Sub New()
        objVisita = New Visita
        objVisita.FolioVisita = ""
    End Sub



    Public Function registrarVisitaPadre(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = False

        Try
            objVisita.FechaRegistro = DateTime.Today
            objVisita.IdTipoEntidad = 0
            objVisita.DscTipoEntidad = ""
            objVisita.IdEstatusActual = 1 ' 1 - Registrado - BDS_C_GR_ESTATUS_CAT
            objVisita.IdPasoActual = 1 ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            objVisita.EsCancelada = False
            objVisita.MotivoCancelacion = String.Empty
            objVisita.FechaCancela = Nothing
            objVisita.IdSubentidad = 0 ''En la visita padre se deja vacia la sub entidad
            objVisita.DscSubentidad = "" ''En la visita padre se deja vacia la sub entidad
         objVisita.Fecha_AcuerdoVul = Date.MinValue

            Dim psIdVisita As String = ""
            objVisita.IdVisitaGenerado = AccesoBD.registrarVisita(objVisita, con, tran, psIdVisita)

            objVisita.FolioVisita = psIdVisita
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, registrarVisitaPadre", "")
            regExitoso = False
        End Try
        If objVisita.IdVisitaGenerado > 0 Then
            regExitoso = True
        Else
            regExitoso = False
        End If
        Return regExitoso
    End Function

    Public Function registrarSupervisores(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            For Each supervisor In objVisita.LstSupervisoresAsignados
                If AccesoBD.registrarSupervisor(objVisita.IdVisitaGenerado, supervisor, con, tran) = False Then
                    regExitoso = False
                End If
            Next

        Catch ex As Exception
            regExitoso = False
        End Try
        Return regExitoso
    End Function

    Public Function registrarInspectores(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            For Each inspector In objVisita.LstInspectoresAsignados
                If AccesoBD.registrarInspector(objVisita.IdVisitaGenerado, inspector, con, tran) = False Then
                    regExitoso = False
                End If
            Next
        Catch ex As Exception
            regExitoso = False
        End Try
        Return regExitoso
    End Function

    Public Function registrarPasoUno(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            Dim Paso As New PasoProcesoVisita()

            Paso.IdVisitaGenerado = objVisita.IdVisitaGenerado
            Paso.IdPaso = objVisita.IdPasoActual ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            Paso.FechaInicio = objVisita.FechaRegistro
            Paso.FechaFin = Nothing
            Paso.EsNotificado = False
            Paso.IdAreaNotificada = Constantes.AREA_SN
            Paso.IdUsuarioNotificado = Nothing
            Paso.EmailUsuarioNotificado = Nothing
            Paso.TieneProrroga = False
            Paso.FechaNotifica = Nothing

            If AccesoBD.registrarPaso(Paso, con, tran) = False Then
                regExitoso = False
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, registrarPasoUno", "")
            regExitoso = False
        End Try

        Return regExitoso
    End Function

    Public Function registrarEstatusPaso(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try

            Dim estatusPaso As New EstatusPaso()
            Dim PasosProV As New PasoProcesoVisita()
            estatusPaso.IdVisitaGenerado = objVisita.IdVisitaGenerado
            estatusPaso.IdPaso = objVisita.IdPasoActual ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            estatusPaso.IdEstatus = objVisita.IdEstatusActual ' 1 - Registrado - BDS_C_GR_ESTATUS_CAT
            estatusPaso.FechaRegistro = objVisita.FechaRegistro
            estatusPaso.IdUsuario = objVisita.Usuario.IdentificadorUsuario
            estatusPaso.Comentarios = objVisita.ComentariosIniciales
            estatusPaso.EsRegistro = Constantes.Verdadero
            estatusPaso.TipoComentario = "USUARIO"
            '' PasosProV.IdMovimiento = pimo

            If AccesoBD.registrarEstatusPaso(PasosProV, estatusPaso, con, tran) = False Then
                regExitoso = False
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, registrarEstatusPaso", "")
            regExitoso = False
        End Try

        Return regExitoso
    End Function

    Public Function registrarObjetoVisitaReg(psLstObjetoVisita As List(Of Integer), con As Conexion.SQLServer, tran As SqlClient.SqlTransaction) As Boolean
        Dim lsObjVisita As String = ""

        If psLstObjetoVisita.Count > 0 Then
            For Each liObjVisita In psLstObjetoVisita
                lsObjVisita &= liObjVisita.ToString() & "|"
            Next

            If lsObjVisita.Trim.Length > 1 Then
                lsObjVisita = lsObjVisita.Substring(0, lsObjVisita.Length - 1)
                Return AccesoBD.registrarObjetoVisita(objVisita.IdVisitaGenerado, lsObjVisita, con, tran)
            End If
        End If

        Return True
    End Function

    Public Function InsertaUsuarios(IdentificadorUsuario As String, psSupervisor As String, psLstInspector As List(Of String)) As Boolean
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim objUsuario As Entities.Usuario

        Try
            objUsuario = New Entities.Usuario(IdentificadorUsuario)
            If Not objUsuario.Existe Then
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(IdentificadorUsuario)
                If datosUsuario.Count > 0 Then
                    If Not IsNothing(datosUsuario.Item("nombre")) Then
                        objUsuario.Nombre = datosUsuario.Item("nombre").ToString()
                    End If

                    If Not IsNothing(datosUsuario.Item("apellidos")) Then
                        objUsuario.Apellido = datosUsuario.Item("apellidos").ToString()
                    End If

                    If Not IsNothing(datosUsuario.Item("mail")) Then
                        objUsuario.Mail = datosUsuario.Item("mail").ToString()
                    End If

                    objUsuario.IdentificadorPerfilActual = 2
                    objUsuario.AgregarUsuarioVO()
                End If
            End If

            objUsuario = New Entities.Usuario(psSupervisor)
            If Not objUsuario.Existe Then
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psSupervisor)
                If datosUsuario.Count > 0 Then
                    If Not IsNothing(datosUsuario.Item("nombre")) Then
                        objUsuario.Nombre = datosUsuario.Item("nombre").ToString()
                    End If

                    If Not IsNothing(datosUsuario.Item("apellidos")) Then
                        objUsuario.Apellido = datosUsuario.Item("apellidos").ToString()
                    End If

                    If Not IsNothing(datosUsuario.Item("mail")) Then
                        objUsuario.Mail = datosUsuario.Item("mail").ToString()
                    End If

                    objUsuario.IdentificadorPerfilActual = 2
                    objUsuario.AgregarUsuarioVO()
                End If
            End If

            For Each lsInspector As String In psLstInspector
                objUsuario = New Entities.Usuario(lsInspector)
                If Not objUsuario.Existe Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(lsInspector)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("nombre")) Then
                            objUsuario.Nombre = datosUsuario.Item("nombre").ToString()
                        End If

                        If Not IsNothing(datosUsuario.Item("apellidos")) Then
                            objUsuario.Apellido = datosUsuario.Item("apellidos").ToString()
                        End If

                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            objUsuario.Mail = datosUsuario.Item("mail").ToString()
                        End If

                        objUsuario.IdentificadorPerfilActual = 2
                        objUsuario.AgregarUsuarioVO()
                    End If
                End If
            Next
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, InsertaUsuarios", "")
            Return False
        End Try

        Return True
    End Function


    Public Function InsertaUsuario(IdentificadorUsuario As String) As Entities.Usuario
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim objUsuario As Entities.Usuario

        Try
            objUsuario = New Entities.Usuario(IdentificadorUsuario)
            If Not objUsuario.Existe Then
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(IdentificadorUsuario)
                If datosUsuario.Count > 0 Then
                    If Not IsNothing(datosUsuario.Item("nombre")) Then
                        objUsuario.Nombre = datosUsuario.Item("nombre").ToString()
                    End If

                    If Not IsNothing(datosUsuario.Item("apellidos")) Then
                        objUsuario.Apellido = datosUsuario.Item("apellidos").ToString()
                    End If

                    If Not IsNothing(datosUsuario.Item("mail")) Then
                        objUsuario.Mail = datosUsuario.Item("mail").ToString()
                    End If

                    objUsuario.IdentificadorPerfilActual = 2
                    objUsuario.AgregarUsuarioVO()
                End If
            End If
        Catch
            Return New Entities.Usuario
        End Try

        Return objUsuario
    End Function

End Class
