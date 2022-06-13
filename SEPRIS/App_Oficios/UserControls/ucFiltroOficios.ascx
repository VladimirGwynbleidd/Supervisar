<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucFiltroOficios.ascx.vb" Inherits="SICOD.ucFiltroOficios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table id="ucFiltroContainer" style="height: 25px; width: 812px; background-color: #7FA6A1;">
    <tr>
        <td>
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
<asp:HiddenField ID="hdn_Session" runat="server" Value="" />
