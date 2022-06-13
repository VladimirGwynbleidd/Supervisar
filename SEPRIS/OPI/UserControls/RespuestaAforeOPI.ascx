<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RespuestaAforeOPI.ascx.vb" Inherits="SEPRIS.RespuestaAforeOPI" %>
<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <table width="95%" style="align-content:center">
            <tr>
                <td style="height: 35px; width: 15%; text-align: left;">
                    <asp:Label ID="LblClasificacion" runat="server" Text="Tipo de respuesta AFORE" CssClass="txt_gral"></asp:Label>
                    <asp:Label ID="LblProceso" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                    <asp:Label ID="LblAsterico" runat="server" Text="*" CssClass="txt_gral"></asp:Label>
                </td>
                <td style="height: 35px; width: 85%; text-align: left;">
                    <div id="DivComboClasifOPI" style="visibility:visible" runat="server">
                        <asp:DropDownList ID="ddlRespuestaAforeOPI" runat="server" Width="218px" Height="16px" CssClass="txt_gral">
<%--                            <asp:ListItem Value="0">--Selecciona una opción--</asp:ListItem>
                            <asp:ListItem>Respuesta de requerimiento</asp:ListItem>
                            <asp:ListItem>Prórroga de entrega de información</asp:ListItem>
                            <asp:ListItem>No hay respuesta en tiempo establecido</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div id="DivTxtClasifOPI" style="visibility:visible" runat="server" >
                        <asp:TextBox ID="txtRespuestaAforeOPI" readOnly="true" runat="server" Width="250px" CssClass="txt_gral"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

