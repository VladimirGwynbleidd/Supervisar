<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="Catalogos.aspx.vb" Inherits="SEPRIS.Catalogos" %>

<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <br />
        <center>
            <asp:Label runat="server" ID="lblTitulo" Text="Catálogos" CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label></center>
        <br />
        <table cellspacing="0" cellpadding="0" border="0" style="width: 80%; height: 400px;
            text-align: center; background-attachment: scroll;">
            <tr>
                <td style="vertical-align: top">
                    <center>
                        <% SubMenu()%></center>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
