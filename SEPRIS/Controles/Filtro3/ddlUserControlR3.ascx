<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ddlUserControlR3.ascx.vb"
    Inherits="SEPRIS.ddlUserControlR3" %>
<table class="TablaFiltro" height="18" id="ddlCustomControlFilter" border="0" cellspacing="0">
    <tr>
        <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label1" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
        <td>
            <div style="width: 469px; text-align:right;">
               <asp:Label ID="Label2" runat="server" class="txt_gral_blanco" Text="Del: " EnableTheming="false"></asp:Label>
                <asp:DropDownList ID="ucDropDownList" class="txt_gral" runat="server" Style="margin-left: 0px"
                    Width="420px">
                </asp:DropDownList>
            </div>
        </td>
       </tr>
   <tr>
      <td class="txt_gral_blanco">
            <div style="width: 180px">
                <asp:Label ID="Label4" runat="server" EnableTheming="false"></asp:Label>
            </div>
        </td>
       <td>
            <div style="width: 469px; text-align:right;">
               <asp:Label ID="Label3" runat="server" class="txt_gral_blanco" Text="al:" EnableTheming="false"></asp:Label>
                <asp:DropDownList ID="ucDropDownList2" class="txt_gral" runat="server" Style="margin-left: 0px"
                    Width="420px">
                </asp:DropDownList>
            </div>
        </td>
        <td class="ucfilter_btnclose">
            <div style="width: 25px">
                <asp:Button ID="btnDelete" runat="server" Text="x" SkinID="botones_x"  CausesValidation="false"/>
            </div>
        </td>
    </tr>
</table>
