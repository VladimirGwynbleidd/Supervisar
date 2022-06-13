<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocMenuPerfiles.aspx.vb" Inherits="SEPRIS.DocMenuPerfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                       Documentación de Catálogo de Perfiles de Menú</label>
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
                            Consultar Catálogo de perfiles de Menú.</p>
                        <p class="txt_gral justificado">
                           1. Accede al Catálogo Perfiles de Menú. (Menú Seguridad  Catálogo Perfiles de Menú).</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m362e3068.jpg" alt="" />
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral justificado">
                            2. Seleccionar un perfil y un menú para ver los submenús del menú asociados y no
                            asociados al perfil.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m6c1a83dd.jpg" alt="" />
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral justificado">
                            3. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Asignar submenú a perfil</p>
                        <p class="txt_gral justificado">
                            1. Accede al Catálogo Perfiles de Menú. (Menú Seguridad  Catálogo Perfiles de Menú).</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m362e3068.jpg" alt="" />
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral justificado">
                            2. Seleccionar un perfil y un menú para ver los submenús del menú asociados y no
                            asociados al perfil.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m6c1a83dd.jpg" alt="" />
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral justificado">
                            3. Selecciona del grid de la derecha (Submenús Disponibles para agregar) los submenús
                            que se vayan a asociar al perfil y da clic en el botón.
                             <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m37cd5b98.jpg" alt="" />
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_5c0dcef0.jpg" alt="" />
                            <br />
                            Figura 5
                        </p>
                        <p class="txt_gral justificado">
                            4. Da clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_24accdbe.jpg" alt="" />
                            <br />
                            Figura 6
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_37107a10.jpg" alt="" />
                            <br />
                            Figura 7
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Desasignar submenú a perfil.</p>
                        <p class="txt_gral justificado">
                            1. Accede al Catálogo Perfiles de Menú. (Menú Seguridad  Catálogo Perfiles de Menú).
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m362e3068.jpg" alt="" />
                            <br />
                            Figura 8
                        </p>
                        <p class="txt_gral justificado">
                            2. Seleccionar un perfil y un menú para ver los submenús del menú asociados y no
                            asociados al perfil.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m6c1a83dd.jpg" alt="" />
                            <br />
                            Figura 9
                        </p>
                        <p class="txt_gral justificado">
                            3. Selecciona del grid de la Izquierda (Submenús actualmente usados por el menú)
                            los submenús que se vayan a desligar del perfil y da clic en el botón
                             <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_m73fb0eaa.jpg" alt="" />.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_5df07b20.jpg" alt="" />
                            <br />
                            Figura 10
                        </p>
                        <p class="txt_gral justificado">
                            4. Da clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_4b6274d7.jpg" alt="" />
                            <br />
                            Figura 11
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17e70b3c468cef72_html_37107a10.jpg" alt="" />
                            <br />
                            Figura 12
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
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
