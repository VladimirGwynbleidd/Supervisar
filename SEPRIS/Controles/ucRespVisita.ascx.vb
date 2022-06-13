Public Class ucRespVisita
    Inherits System.Web.UI.UserControl

    Public Property pObjVisitaCuRespV As Visita
        Get
            If IsNothing(Session("pObjVisitaCuRespV")) Then
                Return Nothing
            Else
                Return CType(Session("pObjVisitaCuRespV"), Visita)
            End If
        End Get
        Set(value As Visita)
            Session("pObjVisitaCuRespV") = value
        End Set
    End Property

    Public Property puObjUsuario As Entities.Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Entities.Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property

    Public Property piTipoResponsable As Integer
        Get
            If IsNothing(Session("piTipoResponsable")) Then
                Return 0
            Else
                Return CType(Session("piTipoResponsable"), Integer)
            End If
        End Get
        Set(value As Integer)
            Session("piTipoResponsable") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub CargarControl()
        If Not IsNothing(pObjVisitaCuRespV) Then
            cargarUsuariosDisponibles()
            cargarUsuarioSupervisor()
            cargarUsuarioInspector()

            lsbUsuariosDisponibles.Enabled = True
            lsbUsuariosDisponibles.Visible = True

            If piTipoResponsable = Constantes.ResponsablesVisita.Supervisor Then
                imgAsignarSupervisor.Enabled = True
                imgAsignarSupervisor.Visible = True
                imgAsignarSupervisor.ImageUrl = "~/Imagenes/FlechaRojaDer.gif"
                imgDesasignarSupervisor.Enabled = True
                imgDesasignarSupervisor.Visible = True
                imgDesasignarSupervisor.ImageUrl = "~/Imagenes/FlechaRojaIzq.gif"
                lsbSupervisor.Enabled = True
                lsbSupervisor.Visible = True

                imgAsignarInspector.Visible = True
                imgAsignarInspector.Enabled = False
                imgAsignarInspector.ImageUrl = "~/Imagenes/FlechaRojaDerOculta.gif"
                imgDesasignarInspector.Visible = True
                imgDesasignarInspector.Enabled = False
                imgDesasignarInspector.ImageUrl = "~/Imagenes/FlechaRojaIzqOculta.gif"
                lsbInspectores.Visible = True
                lsbInspectores.Enabled = False
            ElseIf piTipoResponsable = Constantes.ResponsablesVisita.Inspector Then
                imgAsignarInspector.Enabled = True
                imgAsignarInspector.Visible = True
                imgAsignarInspector.ImageUrl = "~/Imagenes/FlechaRojaDer.gif"
                imgDesasignarInspector.Enabled = True
                imgDesasignarInspector.Visible = True
                imgDesasignarInspector.ImageUrl = "~/Imagenes/FlechaRojaIzq.gif"
                lsbInspectores.Enabled = True
                lsbInspectores.Visible = True

                lsbSupervisor.Enabled = False
                lsbSupervisor.Visible = True
                imgAsignarSupervisor.Visible = True
                imgAsignarSupervisor.Enabled = False
                imgAsignarSupervisor.ImageUrl = "~/Imagenes/FlechaRojaDerOculta.gif"
                imgDesasignarSupervisor.Visible = True
                imgDesasignarSupervisor.Enabled = False
                imgDesasignarSupervisor.ImageUrl = "~/Imagenes/FlechaRojaIzqOculta.gif"
            End If

            lblError.Text = ""
        End If
    End Sub

   Public Sub cargarUsuariosDisponibles()
      Dim isVisitaC As Integer = AccesoBD.EsVisitaConjunta(pObjVisitaCuRespV.IdVisitaGenerado)
      Try
         If pObjVisitaCuRespV.IdArea > 0 Then
            If isVisitaC > 0 Then
               lsbUsuariosDisponibles.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(pObjVisitaCuRespV.IdArea, -1, isVisitaC)
            Else
               lsbUsuariosDisponibles.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(pObjVisitaCuRespV.IdArea)
            End If
         End If

         lsbUsuariosDisponibles.DataTextField = "NOMBRE_COMPLETO"
         lsbUsuariosDisponibles.DataValueField = "USUARIO"
         lsbUsuariosDisponibles.DataBind()
      Catch ex As Exception
         'NHM INICIA LOG           
         Utilerias.ControlErrores.EscribirEvento("Ocurrió un error al cargar los usuarios disponibles. EXCEPTION: " + ex.ToString(), EventLogEntryType.Error)
         'NHM FIN LOG
      End Try

   End Sub

    Private Sub cargarUsuarioSupervisor()
        Try
            If Not IsNothing(pObjVisitaCuRespV) Then
                lsbSupervisor.Items.Clear()
                For Each objSupAsig As SupervisorAsignado In pObjVisitaCuRespV.LstSupervisoresAsignados
                    Dim ltItemSelec As ListItem = (From ltItem As ListItem In lsbUsuariosDisponibles.Items
                                                   Where ltItem.Value = objSupAsig.Id Select ltItem).FirstOrDefault()

                    If Not IsNothing(ltItemSelec) Then
                        lsbSupervisor.Items.Add(ltItemSelec)
                        lsbUsuariosDisponibles.Items.Remove(ltItemSelec)
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub cargarUsuarioInspector()
        Try
            If Not IsNothing(pObjVisitaCuRespV) Then
                lsbInspectores.Items.Clear()
                For Each objInspAsig As InspectorAsignado In pObjVisitaCuRespV.LstInspectoresAsignados
                    Dim ltItemSelec As ListItem = (From ltItem As ListItem In lsbUsuariosDisponibles.Items
                                                   Where ltItem.Value = objInspAsig.Id Select ltItem).FirstOrDefault()

                    If Not IsNothing(ltItemSelec) Then
                        lsbInspectores.Items.Add(ltItemSelec)
                        lsbUsuariosDisponibles.Items.Remove(ltItemSelec)
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub agregarUsuarioSupervisor()
        Try
            ''AGC se quita validacion para agregar mas de un usuario
            ''If lsbSupervisor.Items.Count = 0 Then
            If lsbUsuariosDisponibles.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbUsuariosDisponibles.SelectedItem
                If Not lsbSupervisor.Items.Contains(item) Then
                    lsbSupervisor.Items.Add(item)
                End If
                item.Selected = False
                lsbUsuariosDisponibles.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar un Usuario")
            End If
            ''Else
            'solo deja agregar un supervisor
            ''End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub agregarUsuarioInspector()
        Try
            If lsbUsuariosDisponibles.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbUsuariosDisponibles.SelectedItem
                If Not lsbInspectores.Items.Contains(item) Then
                    lsbInspectores.Items.Add(item)
                End If
                item.Selected = False
                lsbUsuariosDisponibles.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar un Usuario")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub quitarSupervisor()
        Try
            If lsbSupervisor.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbSupervisor.SelectedItem

                lsbUsuariosDisponibles.Items.Insert(0, item)
                Dim lstAnterior As New ListBox
                lstAnterior.Items.AddRange((From ltItem As ListItem In lsbUsuariosDisponibles.Items Order By ltItem.Value).ToArray())

                lsbUsuariosDisponibles.Items.Clear()
                lsbUsuariosDisponibles.Items.AddRange((From ltItem As ListItem In lstAnterior.Items Select ltItem).ToArray())

                item.Selected = False
                lsbSupervisor.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar una rúbrica")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub quitarInspector()
        Try
            If lsbInspectores.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbInspectores.SelectedItem

                lsbUsuariosDisponibles.Items.Insert(0, item)
                Dim lstAnterior As New ListBox
                lstAnterior.Items.AddRange((From ltItem As ListItem In lsbUsuariosDisponibles.Items Order By ltItem.Value).ToArray())

                lsbUsuariosDisponibles.Items.Clear()
                lsbUsuariosDisponibles.Items.AddRange((From ltItem As ListItem In lstAnterior.Items Select ltItem).ToArray())

                item.Selected = False
                lsbInspectores.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar una rúbrica")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub imgAsignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarSupervisor.Click
        agregarUsuarioSupervisor()
    End Sub

    Protected Sub imgDesasignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarSupervisor.Click
        quitarSupervisor()
    End Sub

    Protected Sub imgAsignarInspector_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarInspector.Click
        agregarUsuarioInspector()
    End Sub

    Protected Sub imgDesasignarInspector_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarInspector.Click
        quitarInspector()
    End Sub


    ''' <summary>
    ''' Retorna los usuarios seleccionados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUsuariosSeleccionados() As String
        If piTipoResponsable <> 0 Then
            If piTipoResponsable = Constantes.ResponsablesVisita.Supervisor Then
                If lsbSupervisor.Items.Count > 0 Then
                    Dim lsSupervisores As String = ""

                    For indice As Integer = 0 To lsbSupervisor.Items.Count - 1
                        lsSupervisores &= lsbSupervisor.Items(indice).Value & "|" & lsbSupervisor.Items(indice).Text & ","
                    Next
                    If lsSupervisores.Length > 0 Then
                        Return lsSupervisores.Substring(0, lsSupervisores.Length - 1)
                    End If
                End If
            ElseIf piTipoResponsable = Constantes.ResponsablesVisita.Inspector Then
                If lsbInspectores.Items.Count > 0 Then
                    Dim lsInspectores As String = ""

                    For indice As Integer = 0 To lsbInspectores.Items.Count - 1
                        lsInspectores &= lsbInspectores.Items(indice).Value & "|" & lsbInspectores.Items(indice).Text & ","
                    Next

                    If lsInspectores.Length > 0 Then
                        Return lsInspectores.Substring(0, lsInspectores.Length - 1)
                    End If
                End If
            Else
                Return ""
            End If
        Else
            Return ""
        End If

        Return ""
    End Function

    Public Function GetListaUsuariosSeleccionados() As List(Of Persona)

        Dim lstPersonas As New List(Of Persona)

        If piTipoResponsable <> 0 Then
            If piTipoResponsable = Constantes.ResponsablesVisita.Supervisor Then
                If lsbSupervisor.Items.Count > 0 Then
                    For indice As Integer = 0 To lsbSupervisor.Items.Count - 1
                        Dim objUsuario As New Entities.Usuario(lsbSupervisor.Items(indice).Value)
                        lstPersonas.Add(New Persona With {.Nombre = lsbSupervisor.Items(indice).Text, .Correo = objUsuario.Mail})
                    Next
                End If
            ElseIf piTipoResponsable = Constantes.ResponsablesVisita.Inspector Then
                If lsbInspectores.Items.Count > 0 Then
                    Dim lsInspectores As String = ""

                    For indice As Integer = 0 To lsbInspectores.Items.Count - 1
                        Dim objUsuario As New Entities.Usuario(lsbInspectores.Items(indice).Value)
                        lstPersonas.Add(New Persona With {.Nombre = lsbInspectores.Items(indice).Text, .Correo = objUsuario.Mail})
                    Next
                End If
            End If
        End If

        Return lstPersonas
    End Function

    Public Sub MuestraError(psError As String)
        lblError.Text = psError
    End Sub
End Class