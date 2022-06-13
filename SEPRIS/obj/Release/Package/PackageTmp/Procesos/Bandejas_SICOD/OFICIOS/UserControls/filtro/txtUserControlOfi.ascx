<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="txtUserControlOfi.ascx.vb" Inherits="SEPRIS.txtUserControlOfi" %>
<table class="TablaFiltro" height="25" id="txtFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 160px">
                <asp:TextBox ID="TextBox1" class="txt_gral" Width="135px" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hdnType" runat="server" Value="0" />
            </div>
        </td>
        <td align="right" class="txt_gral_blanco">
            <div style="width: 73px">
                <asp:Label ID="Label2" runat="server"></asp:Label>
                &nbsp;
            </div>
        </td>
        <td>
            <div style="width: 232px">
                <asp:TextBox ID="TextBox2" class="txt_gral" Width="135px" runat="server"></asp:TextBox>
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button onmouseover="style.backgroundColor='#87100C'" onmouseout="style.backgroundColor='#D56D62'"
                    ID="btnDelete" CssClass="botones_x" runat="server" Text="x" />
            </div>
        </td>
    </tr>
</table>
