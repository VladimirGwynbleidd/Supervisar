<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="chkButtomUserControl.ascx.vb" Inherits="SEPRIS.chkButtomUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table class="TablaFiltro" height="18" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 50px; text-align:left;">
                <asp:Label ID="Label1" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 400px; text-align:left;">
                <asp:CheckBox ID="ucCheckBox" runat="server" />
            </div>
        </td>
    </tr>
</table>
