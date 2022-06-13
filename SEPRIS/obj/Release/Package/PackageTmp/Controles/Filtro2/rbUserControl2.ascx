<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="rbUserControl2.ascx.vb"
    Inherits="SEPRIS.rbUserControl2" %>
<table class="TablaFiltro" height="18" id="txtFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco" style="width: 180px;">
            <div style="width: 180; display:none;">
                <asp:Label ID="Label1" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 468px">
                <asp:RadioButtonList ID="ucRadioButton" class="txt_gral_blanco" runat="server" AutoPostBack="False" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" SkinID="botones_x" runat="server" Text="x" />
            </div>
        </td>
    </tr>
</table>
