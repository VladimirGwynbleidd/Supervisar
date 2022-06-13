Public Class PosibleIncumplimiento
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property ValPosibIncumplim
        Get
            'Return txtPosibleIncump.Text
            'Return HddFldPosibIncump.Value
            Return rdbPosibIncumpl.SelectedValue
        End Get
    End Property

    Public ReadOnly Property ValMotivoNOProced
        Get
            Return txtMotivNOProced.Text
        End Get
    End Property

    Public WriteOnly Property ValActivaCampos
        Set(value)
            txtMotivNOProced.Enabled = value
            ObjCaracteresNO.Visible = value
            rdbPosibIncumpl.Enabled = False
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivaCampos", "JsActivaCampos('" & value & "');", True)
        End Set
    End Property

    Public WriteOnly Property AsignaPosibIncumpl
        Set(value)
            'txtPosibleIncump.Text = value
            'rdbPosibIncumpl.SelectedValue = value
            'HddFldPosibIncump.Value = value
            If value = True Then
                rdbPosibIncumpl.Items(0).Selected = True
                rdbPosibIncumpl.Items(1).Selected = False
                DivMotivNOProced.Visible = False
            Else
                rdbPosibIncumpl.Items(0).Selected = False
                rdbPosibIncumpl.Items(1).Selected = True
                DivMotivNOProced.Visible = True
            End If
        End Set
    End Property

    Public WriteOnly Property AsignaMotivoNOProcedencia
        Set(value)
            txtMotivNOProced.Text = value
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ActivaCampos", "JsAsignaPosincump();", True)
        End Set
    End Property

    Protected Sub rdbPosibIncumpl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdbPosibIncumpl.SelectedIndexChanged
        If rdbPosibIncumpl.SelectedValue = "1" Then
            DivMotivNOProced.Visible = False
        Else
            DivMotivNOProced.Visible = True
        End If
    End Sub
End Class