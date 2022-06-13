<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocBitacora.aspx.vb" Inherits="SEPRIS.DocBitacora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Consulta Bitácora</label>
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
                    <div style="text-align: left;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Consultar detalle de registro de bitácora</p>
                        <p class="txt_gral justificado">
                            1. Accede a la pantalla de Bitácora de Acciones (Menú Seguridad  Bitácora)</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a0533451f0_html_m5b9eabc8.png"
                                alt="" />
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral justificado">
                            2. Selecciona un registro del grid y da clic en el botón Consultar</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_172dd2a053345f0_html_m5b9eabc8.png"
                                alt="" />
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral justificado">
                            Un mensaje modal se muestra sobre la página con el detalle del registro.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_wqw12dd2a053345f0_html_meabc8.png"
                                alt="" />
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral justificado">
                            3. Fin.</p>
                        <br />
                        <br />
                    </div>
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
                            Requerimientos:</p>
                        <ul>
                            <li>Implementar patrón funcional de GridView</li>
                            <li>Implementa patrón funcional de Mensaje modal.</li>
                            <li>Filtros dinámicos.</li>
                            <li>Implementar catálogo de errores.</li>
                            <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_D_GR_BITACORA</li>
                            <br />
                            Esta tabla se puede obtener del script de creación de base de datos de la ODT-02.
                            <br />
                            <br />
                            <li>Código fuente.- Este patrón funcional está compuesto por una página Bitacora.aspx
                                que se encuentra en la carpeta Seguridad del proyecto WEBSITE, que hereda de la
                                clase Bitacora.vb que se encuentra en el proyecto Entities.</li>
                        </ul>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_aabf092cdea32da19bed_html_5d463618.png"
                                alt="" />
                            <br />
                            Figura 1
                        </p>
                        <br />
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
