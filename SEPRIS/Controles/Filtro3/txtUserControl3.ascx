<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="txtUserControl3.ascx.vb"
    Inherits="SEPRIS.txtUserControl3" %>
<table class="TablaFiltro" height="18" id="txtFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 160px">
                <asp:TextBox ID="TextBox1" class="txt_gral" Width="135px" runat="server" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                <asp:HiddenField ID="hdnType" runat="server" Value="0" />
            </div>
        </td>
        <td align="right" class="txt_gral_blanco">
            <div style="width: 73px">
                <asp:Label ID="Label2" runat="server" EnableTheming="false"></asp:Label>
                &nbsp;
            </div>
        </td>
        <td>
            <div style="width: 232px">
                <asp:TextBox ID="TextBox2" class="txt_gral" Width="135px" runat="server" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" SkinID="botones_x" runat="server" Text="x" CausesValidation="false" />
            </div>
        </td>
    </tr>
</table>
