<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="Principal.aspx.vb" Inherits="SEPRIS.Principal" %>

<%@ Register Src="/Controles/wucNotificaciones.ascx" TagName="ucNotificaciones" TagPrefix="ucNotificaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ucNotificaciones:ucNotificaciones runat="server" ID="Notificaciones" EsVistaPrevia="false" />
    <table width="100%">
				<tr>
					<td valign="top" align="center">
						<img height="424" alt="" src="Imagenes\fondo_consar.jpeg" width="536"/>
					</td>
				</tr>
			</table>
</asp:Content>
