<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ddlUserControl.ascx.vb"
    Inherits="SEPRIS.ddlUserControl" %>
<table class="TablaFiltro" height="18" id="ddlCustomControlFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 469px;">
                <asp:DropDownList ID="ucDropDownList" class="txt_gral" runat="server" Style="margin-left: 0px"
                    Width="420px">
                </asp:DropDownList>
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x" SkinID="botones_x" />
            </div>
        </td>
    </tr>
</table>
