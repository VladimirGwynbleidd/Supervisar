<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MensajeModal.ascx.vb"
    Inherits="SEPRIS.MensajeModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<ajax:ModalPopupExtender ID="mpext" runat="server" BackgroundCssClass="modalBackground"
    TargetControlID="pnlPopup" PopupControlID="pnlPopup">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlPopup" runat="server" Style="display: none;" DefaultButton="btnModalOk">
    <table width="100%">
        <tr class="topHandle">
            <td align="left" runat="server" id="tdTitulo" class="tdTitulo">
                &nbsp;
                <asp:Label ID="lblTitulo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="middle" align="left" id="tdMensaje">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <table style="width: 230px">
                    <tr>
                        <td align="right" style="width: 115px">
                            <asp:Button ID="btnModalOk" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                CssClass="botones" />
                        </td>
                        <td>
                            <asp:Button ID="btnModalCancelar" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                CssClass="botones" />
                           <%-- &nbsp;&nbsp;&nbsp;--%>
                        </td>
                        <asp:Label ID="lblPostBackOption" Style="display: none" runat="server"></asp:Label>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<script type="text/javascript">
    function fnClickOK(sender, e) {
        __doPostBack(sender, e);
    }

    function fnClickCancelar(sender, e) {
        __doPostBack(sender, e);
    }
</script>
