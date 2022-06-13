<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="chkUserControl3.ascx.vb" Inherits="SEPRIS.chkUserControl3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table class="TablaFiltro" height="18" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 469px">
                <asp:CheckBox ID="ucCheckBox" runat="server" />
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x" SkinID="botones_x" CausesValidation="false" />
            </div>
        </td>
    </tr>
</table>
