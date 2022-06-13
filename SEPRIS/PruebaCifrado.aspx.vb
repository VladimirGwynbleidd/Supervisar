Public Class PruebaCifrado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCifrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCifrar.Click

        Select Case rdOpciones.SelectedValue

            Case "AES"
                txtSalida.Text = Utilerias.Cifrado.CifrarAES(txtEntrada.Text)

            Case "SHA512"
                txtSalida.Text = Utilerias.Cifrado.CifrarSHA512(txtEntrada.Text)

        End Select

    End Sub

    Protected Sub rdOpciones_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdOpciones.SelectedIndexChanged

        btnCifrar.Enabled = True
        btnDescifrar.Enabled = True

        Select Case rdOpciones.SelectedValue

            Case "AES"


            Case "SHA512"
                btnDescifrar.Enabled = False

        End Select

        btnCifrar_Click(sender, e)

    End Sub

    Protected Sub btnDescifrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDescifrar.Click

        Select Case rdOpciones.SelectedValue

            Case "AES"
                txtSalida.Text = Utilerias.Cifrado.DescifrarAES(txtEntrada.Text)

            Case "SHA512"
                txtSalida.Text = ""

        End Select

    End Sub

    Protected Sub imgCandado_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCandado.Click

        If Utilerias.Cifrado.ValidarKey() Then
            imgCandado.ImageUrl = "~/Imagenes/candado4.jpg"
        Else
            imgCandado.ImageUrl = "~/Imagenes/candado5.jpg"
        End If

    End Sub
End Class