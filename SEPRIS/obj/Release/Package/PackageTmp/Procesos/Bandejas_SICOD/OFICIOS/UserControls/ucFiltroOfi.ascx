<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucFiltroOfi.ascx.vb" Inherits="SEPRIS.ucFiltroOfi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<style type="text/css">
    .style1
    {
        height: 24px;
    }
</style>

<table id="ucFiltroContainer" style="height: 25px; width: 812px; background-color: #7FA6A1;">
    <tr>
        <td class="style1">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </td>
        <td style="width: 150px" rowspan="2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlAgregar" Width="120px" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True">-- Agregar Criterio --</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
