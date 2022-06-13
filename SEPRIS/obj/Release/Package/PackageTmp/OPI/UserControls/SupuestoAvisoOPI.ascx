<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SupuestoAvisoOPI.ascx.vb" Inherits="SEPRIS.SupuestoAvisoOPI" %>
<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <table width="95%">
            <tr>
                <td style="height: 35px; width: 15%; text-align: left;">
                    <asp:Label ID="LblSupuestoAviso" runat="server" Text="Supuesto del aviso de conocimiento" CssClass="txt_gral"></asp:Label>
                    <asp:Label ID="LblProceso" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                    <asp:Label ID="LblAsterico" runat="server" Text="*" CssClass="txt_gral"></asp:Label>
                </td>
                <td style="height: 35px; width: 85%; text-align: left;">
                    <div id="ComboSupuestoOPI" style="visibility:visible" runat="server">
                        <asp:DropDownList ID="ddlSupuestoAvisoOPI" runat="server" Width="72%" Height="18px" CssClass="txt_gral">
                        </asp:DropDownList>
                    </div>
                    <div id="divSupuestoAviso" runat ="server" visible="false" >
                        <asp:Label ID="lbl_Aviso" runat ="server" Text=""></asp:Label> 
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>