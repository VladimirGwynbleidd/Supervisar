<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="Notificaciones.aspx.vb" Inherits="SEPRIS.Notificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitulosWebProyectos">
                Opciones Mensajes de Notificación
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnBandeja" Text="Mensajes" Width="170px" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnConfiguraEstilos" Text="Configuraci&oacute;n de Estilos"
                    Width="170px" />
            </td>
        </tr>
    </table>
</asp:Content>
