<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="rbUserControlOficios.ascx.vb"
    Inherits="SICOD.rbUserControlOficios" %>
<table class="TablaFiltro" height="25" id="txtFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 465px">
                <asp:RadioButtonList ID="ucRadioButton" class="txt_gral_blanco" runat="server" AutoPostBack="False">
                </asp:RadioButtonList>
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
