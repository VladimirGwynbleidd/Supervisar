Public Class PruebaSISAN
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim USUARIOS = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSisan")
        Dim PASSWORDS = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSisan").ToString())

        Dim Registro As New wsSisanReg.RegistroExterno
        Dim credentialsS As System.Net.NetworkCredential = New System.Net.NetworkCredential(USUARIOS, PASSWORDS, "ADCONSAR")
        Registro.Credentials = credentialsS
        Registro.ConnectionGroupName = "SEPRIS"

        Dim subentidad As Integer = -1 'En caso de no enviar
        Dim sistema As String = "SEPRIS" 'sistema 2 sepris
        Dim tipoEntidad As Integer = 1
        Dim entidad As Integer = 578
        Dim clasificacion As Integer = 1 'PAC
        Dim participante As Integer = 0 ' Viene la irregularidad
        Dim oficioSICOD As String = "CONSAR/DI/VF/DVF/090/JBS/2018" 'sistema 2 sepris
        Dim usuario As String = "krosales"

        'Dim resultado As wsSisanReg.WsResultado = Registro.CallRegistroExterno("FOL. REF", usuario, sistema, tipoEntidad, entidad, subentidad, 1, clasificacion, "ninguno", Date.Now, Date.Now, oficioSICOD, "prueba", usuario, "ninguno")
        'Dim resultado As wsSisanReg.WsResultado = Registro.RegistroIncidencias(0, 1000, 2164, Date.Now, "PRUEBA DE WS")
        Dim resultado As wsSisanReg.WsResultado = Registro.enviarSancion(2164, usuario, "PRUEBA SANCION")
        '4851

        If resultado.isError Then
            Dim mensajes As String() = resultado.lstMensajes
            'presentar errores

        Else
            Dim folioSISAN As String = resultado.Folio
            'Almacenar el folio en un campo de PC


        End If


    End Sub
End Class