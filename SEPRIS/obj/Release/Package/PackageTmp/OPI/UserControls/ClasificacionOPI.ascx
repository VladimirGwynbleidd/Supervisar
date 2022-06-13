<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClasificacionOPI.ascx.vb" Inherits="SEPRIS.ClasificacionOPI" %>
<style type="text/css">
    .auto-style1 {
        height: 35px;
        width: 300px;
    }
</style>
<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <table style="width:600px">
            <tr>
                <td style="height: 35px; width:30%; text-align: left;">
                    <asp:Label ID="LblClasificacion" runat="server" Text="Clasificación " CssClass="txt_gral"></asp:Label>
                    <asp:Label ID="LblProceso" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                    <asp:Label ID="LblAsterico" runat="server" Text="" CssClass="txt_gral"><samp style="color: red; font-size: 1.3em"><b>&nbsp;*</b></samp></asp:Label>
                </td>
                <td style="text-align: left;" class="auto-style1">
                    <div id="DivComboClasifOPI" style="visibility:visible" Width="300px" runat="server">
                        <asp:DropDownList ID="ddlClasificacionOPI" cssclass="txt_gral" runat="server" Width="218px" Height="16px">
                            <asp:ListItem Value="0">--Selecciona una opción--</asp:ListItem>
                            <asp:ListItem>Aviso de conocimiento</asp:ListItem>
                            <asp:ListItem>Requerimiento de información</asp:ListItem>
                            <asp:ListItem>Oficio de Observaciones</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="DivTxtClasifOPI"   Width="300px" runat="server" >
                        <asp:TextBox ID="txtClasificacionOPI" Enabled="false"  ReadOnly="true" runat="server" Width="250px" CssClass="txt_gral"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>