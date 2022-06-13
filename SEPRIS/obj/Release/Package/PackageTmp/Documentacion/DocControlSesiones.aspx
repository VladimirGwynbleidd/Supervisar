<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocControlSesiones.aspx.vb" Inherits="SEPRIS.DocControlSessiones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Control de Sesiones</label>
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
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" Width="110px" />
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
                            <li>Implementar patrón funcional Master Page.</li>
                            <li>Implementar patrón funcional Mensaje Modal.</li>
                            <li>Dos registros en la tabla de BDS_C_GR_PARAMETRO.</li>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3e04ec2399ea3053zza2x_html_1f6b2f4a.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            <li>Código fuente.- este patrón consiste en un control de usuario llamado wucVencimientoSesion.ascx
                                que se encuentra en la carpeta Controles dentro del proyecto WEBSITE.</li>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_53e04ec2mjhgea3053zza2x_html_1f6b2f4a.png"
                                    alt="" />
                                <br />
                                Figura 2
                            </p>
                            <br />
                            Para el implementar completamente el contron es necesario agregar la referencia
                            al control de sesiones en SiteInterno.Master
                            <br />
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_53e04ec2m8765ea3053z77a2x_html_1f6b2f4a.png"
                                    alt="" />
                                <br />
                                Figura 3
                            </p>
                            <br />
                            En el div Id=”Pie” agregar las líneas encerradas en el cuadro rojo.
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_53e04ec2222211773z77a2x_html_1f6b2f4a.png"
                                    alt="" />
                                <br />
                                Figura 4
                            </p>
                            <p>
                                Para los botones que se agregaron en el paso anterior programar la lógica deseada
                                para tales eventos, ya sea continuar la sesión o terminar ésta.
                            </p>
                        </ul>
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
