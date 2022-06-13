<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocMensajeModal.aspx.vb" Inherits="SEPRIS.DocMensajeModal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Mensaje Modal</label>
                </div>
                <br />
                <br />
                <asp:Button ID="btnManualUsuario" runat="server" Text="Manual Usuario" />
                <br />
                <br />
                <asp:Button ID="btnManualTecnico" runat="server" Text="Manual Técnico" />
            </asp:Panel>
            <asp:Panel ID="pnlUsuario" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualUsuario" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalUsuario" runat="server">
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlTecnico" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualTecnico" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalTecnico" runat="server">
                    <div style="text-align: left;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Requerimientos:
                        </p>
                        <ul>
                            <li>Implementar patrón funcional de Master Page </li>
                            <li>Debe existir la referencia a la librería jquery-ui-1.10.3.custom.js (http://jqueryui.com/download/)
                                y jquery-ui-1.10.3.custom.min.js (http://jqueryui.com/themeroller/) con el tema
                                Smoothness, así mismo, a la libreria de jqueryjquery-1.9.1.js (versión actual al
                                momento del desarrollo el presente componente).</li>
                            <li>Cada Mensaje está compuesto de dos llamadas, una en la carga de la página:
                                <ul class="circle">
                                    <li>método "$(document).ready(function() { XXXXXXX });" o "$(function() { XXXXXXX });"
                                        en javascript </li>
                                    <li>y la segunda donde se desea mostrar el mensaje. </li>
                                </ul>
                                <li>Los botones de acción (que realicen postbacks deben incluir la etiqueta ClientIDMode="Static").</li>
                                <li>Los elementos div deben tener la etiqueta style="display: none"</li>
                                <li>Lea la descripción y observaciones de cada mensaje que desee implementar el archivo
                                    MensajeModal.js que se encuentra en la carpeta de Scripts del proyecto.</li>
                                <br />
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3e04ec239fgh43113053_html_142df4z.png"
                                        alt="" />
                                    <br />
                                    Figura 1
                                </p>
                        </ul>
                        <br />
                    </div>
                    <asp:Button ID="btnRegresarTecnico" runat="server" Text="Regresar" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnManualUsuario" />
            <asp:PostBackTrigger ControlID="btnManualTecnico" />
            <asp:PostBackTrigger ControlID="btnRegresarUusario" />
            <asp:PostBackTrigger ControlID="btnRegresarTecnico" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
