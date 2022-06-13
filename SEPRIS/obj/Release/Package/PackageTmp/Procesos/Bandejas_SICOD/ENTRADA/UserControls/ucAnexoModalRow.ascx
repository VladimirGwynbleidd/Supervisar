<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucAnexoModalRow.ascx.vb"
    Inherits="SEPRIS.ucAnexoModalRow" %>
<div style="width: 660px; text-align: center">
    <strong>
        <asp:Label ID="lblFolio" runat="server" Width="660px"></asp:Label>
    </strong>
</div>
<table style="text-align: center" class="txt_gral" width="660px">
    <tr>
        <td style="width: 110px">
            Carpeta
        </td>
        <td style="width: 110px">
            CD/DVD
        </td>
        <td style="width: 110px">
            Sobre Cerrado
        </td>
        <td style="width: 110px">
            Paquete
        </td>
        <td style="width: 110px">
            Revistas
        </td>
        <td style="width: 110px">
            Otros
        </td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" id="chk_Carpeta" runat="server" disabled="disabled" />
        </td>
        <td>
            <input type="checkbox" id="chk_CD" runat="server" disabled="disabled" />
        </td>
        <td>
            <input type="checkbox" id="chk_Sobre" runat="server" disabled="disabled" />
        </td>
        <td>
            <input type="checkbox" id="chk_Paq" runat="server" disabled="disabled" />
        </td>
        <td>
            <input type="checkbox" id="chk_Rev" runat="server" disabled="disabled" />
        </td>
        <td>
            <table width="110px">
                <tr>
                    <td style="width: 70px" align="right">
                        <input type="checkbox" id="chk_Otro" runat="server" disabled="disabled" />
                    </td>
                    <td style="width: 40px">
                        <asp:Image ID="imgOtros" runat="server" Visible="false" Width="16px" Height="16px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdn_CheckValues" runat="server" />
<asp:HiddenField ID="hdnFolio" runat="server" />
