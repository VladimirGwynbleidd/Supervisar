<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoSubmenus.aspx.vb" Inherits="SEPRIS.DocCatalogoSubmenus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                       Documentacón de Catálogo de Submenús</label>
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
                            Consulta detalle de registro de submenú</p>
                        <p class="txt_gral justificado">
                            1. Accede a la bandeja de Catálogo de Submenús (Seguridad  Catálogo de Submenús).</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_62b9bb36.jpg" alt="" />
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral justificado">
                            2. Doble clic sobre un registro para ver el detalle del mensaje.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_m3dff8630.jpg" alt="" />
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral justificado">
                            3. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Agregar submenú</p>
                        <p class="txt_gral justificado">
                            1. Accede a la bandeja de Catálogo de Submenús (Seguridad  Catálogo de Submenús).</p>
                        <p class="txt_gral justificado">
                            2. Hacer clic sobre el botón Agregar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_62b9bb36.jpg" alt="" />
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral justificado">
                            3. Capturar los campos obligatorios del formulario y da clic en el botón Aceptar</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_1808e48c.jpg" alt="" />
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral justificado">
                            4. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_m13369760.jpg" alt="" />
                            <br />
                            Figura 5
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Modificar submenú</p>
                        <p class="txt_gral justificado">
                            1. Accede a la bandeja de Catálogo de Submenús (Seguridad  Catálogo de Submenús).</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_62b9bb36.jpg" alt="" />
                            <br />
                            Figura 6
                        </p>
                        <p class="txt_gral justificado">
                            2. Selecciona el registro que se vaya a modificar y da clic el botón Modificar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_589faa21.jpg" alt="" />
                            <br />
                            Figura 7
                        </p>
                        <p class="txt_gral justificado">
                            3. Modificar los campos que se desee y dar clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_mc475630.jpg" alt="" />
                            <br />
                            Figura 8
                        </p>
                        <p class="txt_gral justificado">
                            4. Confirmar Acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_3ae4de2d.jpg" alt="" />
                            <br />
                            Figura 9
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.
                        </p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Eliminar submenú</p>
                        <p class="txt_gral justificado">
                            1. Accede a la bandeja de Catálogo de Submenús (Seguridad  Catálogo de Submenús).</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_m2d3cca9e.jpg" alt="" />
                            <br />
                            Figura 10
                        </p>
                        <p class="txt_gral justificado">
                            2. Selecciona el registro que se vaya a modificar y da clic el botón Eliminar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_m57b521b4.jpg" alt="" />
                            <br />
                            Figura 11
                        </p>
                        <p class="txt_gral justificado">
                            3. Dar clic en el botón Eliminar.</p>
                        <p class="txt_gral justificado">
                            4. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23e3431c9171361d_html_m7986be58.jpg" alt="" />
                            <br />
                            Figura 12
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.</p>
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
                        <p class="txt_gral justificado">
                            Este módulo abarca funcionales como lo son:
                            <ul>
                                <li>Catálogo de menús</li>
                                <li>Catálogo de submenús</li>
                                <li>Catálogo de perfiles de menú</li>
                            </ul>
                            <p class="SubTitulosWebProyectos">
                                Implementar requerimientos
                            </p>
                            <ul>
                                <li>Implementar patrón funcional de GridView</li>
                                <li>Implementa patrón funcional de Mensaje modal.</li>
                                <li>Filtros dinámicos.</li>
                                <li>Bitácora.</li>
                                <li>Script de base de datos.- para implementar el conjunto de patrones funcionales es
                                    necesario contar con las siguientes tablas en la base de datos:</li>
                                    <br />
                                <ul class="circle">
                                    <li>BDS_C_GR_MENU</li>
                                    <li>BDS_R_GR_MENU_SUBMENU_PAGINA</li>
                                    <li>BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA</li>
                                </ul>
                                <br />
                                Estas tablas se pueden obtener del script de creación de base de datos de la ODT-02.
                                <li>Código fuente.- el código fuente de esta en la carpeta Seguridad del proyecto WEBSITE,
                                    contiene tres páginas que heredan de las clases Menu.vb, Submenu.vb y Perfil.vb</li>
                            </ul>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_4b6274d7231a.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
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
