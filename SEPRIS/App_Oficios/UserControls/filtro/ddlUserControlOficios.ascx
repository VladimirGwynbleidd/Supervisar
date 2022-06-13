<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ddlUserControlOficios.ascx.vb"
    Inherits="SICOD.ddlUserControlOficios" %>
<table class="TablaFiltro" height="25" id="ddlCustomControlFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 469px">
                <asp:DropDownList ID="ucDropDownList" class="txt_gral" runat="server" Style="margin-left: 0px"
                    Width="420px">
                </asp:DropDownList>
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button onmouseover="style.backgroundColor='#87100C'" onmouseout="style.backgroundColor='#D56D62'"
                    ID="btnDelete" runat="server" Text="x" CssClass="botones_x" />
            </div>
        </td>
    </tr>
</table>
