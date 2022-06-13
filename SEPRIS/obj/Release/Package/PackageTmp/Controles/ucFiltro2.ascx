<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucFiltro2.ascx.vb" Inherits="SEPRIS.ucFiltro2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<table id="ucFiltroContainer" style="height:25px;" border="0" cellpadding="0" cellspacing="0" class="FiltroDinamico" runat="server">
    <tr>
        <td align="left" colspan="4">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </td>
        <td></td>
    </tr>
    <tr>
        <td align="left" width="30%">
            <asp:DropDownList ID="ddlAgregar" width="180px" runat="server" AutoPostBack="True" CssClass="txt_gral">
                <asp:ListItem Selected="True">-- Agregar Criterio --</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td colspan="2" width="60%">
            <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                <asp:Table ID="Table2" runat="server">
                    <asp:TableRow ID="TableRow2" runat="server"></asp:TableRow>
                </asp:Table>
            </asp:PlaceHolder>
        </td>
        <td align="right" width="20%">
            <asp:Button ID="btnFiltrar" runat="server" CssClass="botones" Text="Filtrar" visible="true" OnClientClick="Deshabilita(this);">
            </asp:Button>          
        </td>
        <%--<td style="width:20%"></td>--%>
    </tr>
</table>
<asp:HiddenField ID="hdn_Session" runat="server" Value="" />

<div id="divMensajeVerificaFiltro" style="display: none">
    <table width="100%">
        <tr>
            <td style="width: 50px; text-align: center; vertical-align:top">
                <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px"/>
            </td>
            <td  style="text-align: left" class="MensajeModal-UI">
                <%= Mensaje %>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    $(function () {

        MensajeVerificaFiltroLoad();
 
    });

    function MuestraVerificacionFiltro() {

        MensajeVerificaFiltro();
        return false;

    }

    function Validacion12() {
        //alert("esta entrando a la validacion");
    };


</script>