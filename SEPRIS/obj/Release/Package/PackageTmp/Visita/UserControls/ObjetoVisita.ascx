<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ObjetoVisita.ascx.vb" Inherits="SEPRIS.ObjetoVisita" %>
<asp:UpdatePanel ID="up1" runat="Server">
    <ContentTemplate>
        <table style="width: 80%">
            <tr>
                <td colspan="4" style="text-align: left;" class="auto-style3">Objeto de la visita:</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;"></td>
                <td style="width: 10%">&nbsp;</td>
                <td style="text-align: left; width: 40%">Seleccionado(s)
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="vertical-align: middle;">
                    <asp:ListBox ID="lstObjetosDisponibles" runat="server" Width="95%" Height="110px"></asp:ListBox>
                </td>
                <td style="vertical-align: bottom; text-align: center;">
                    <asp:ImageButton ID="imgAsignar" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif"
                        OnClick="imgAsignar_Click" />
                </td>
                <td rowspan="2" style="vertical-align: bottom;">
                    <asp:ListBox ID="lstObjetosSeleccionados" runat="server" Width="95%" Height="110px"></asp:ListBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: center;">
                    <asp:ImageButton ID="imgDesasignar" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif"
                        OnClick="imgDesasignar_Click" />
                </td>
            </tr>
            <tr id="OtroObjVisita" runat="server" visible="false">
                <td style="height: 35px; width: 25%; text-align: left;">Especificar:</td>
                <td></td>
                <td style="height: 35px; width: 80%; text-align: left;" colspan="2">
                    <asp:TextBox ID="txtObjetoVisita" runat="server" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)" onkeyup="validaLimiteLongitud(this,500,'lblConttxtObjetoVisita')" TextMode="MultiLine" Width="80%" MaxLength="500"></asp:TextBox>
                    <div id="ast13" runat="server" class="AsteriscoHide">*</div>
                </td>
            </tr>
        </table>

    </ContentTemplate>
</asp:UpdatePanel>
