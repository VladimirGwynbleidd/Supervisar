<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoErrores.aspx.vb" Inherits="SEPRIS.DocCatalogoErrores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                       Documentación Catálogo de Errores</label>
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
                            Consultar catálogo de Errores</p>
                        <p class="txt_gral justificado">
                            1. Accede a Catálogo de Errores. (Mantenimiento – Catálogo de Error)</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_ma9b3d1e.jpg"
                                alt="" />
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral justificado">
                            2. Da doble clic sobre cualquier registro para ver el detalle del error.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_m1ce3c9dd.jpg"
                                alt="" />
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral justificado">
                            3. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Agregar registro de Error</p>
                        <p class="txt_gral justificado">
                            1. Accede a Catálogo de Errores. (Mantenimiento – Catálogo de Error).</p>
                        <p class="txt_gral justificado">
                            2. Haz clic en el botón Agregar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_ma9b3d1e.jpg"
                                alt="" />
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral justificado">
                            3. Capturar los campos requeridos.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_m51bab93f.jpg"
                                alt="" />
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral justificado">
                            4. Confirmar acción.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_1b6631ff.jpg"
                                alt="" />
                            <br />
                            Figura 5
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Modificar Registro de Error</p>
                        <p class="txt_gral justificado">
                            1. Accede a Catálogo de Errores. (Mantenimiento – Catálogo de Error).</p>
                        <p class="txt_gral justificado">
                            2. Selecciona el registro que se vaya a modificar y da clic en el botón Modificar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_m6718b58b.jpg"
                                alt="" />
                            <br />
                            Figura 6
                        </p>
                        <p class="txt_gral justificado">
                            3. Modifica los campos necesarios y da clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_m154618bd.jpg"
                                alt="" />
                            <br />
                            Figura 7
                        </p>
                        <p class="txt_gral justificado">
                            4. Confirmar acción.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_m35d2573.jpg"
                                alt="" />
                            <br />
                            Figura 8
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Eliminar Registro de Error</p>
                        <p class="txt_gral justificado">
                            1. Accede a Catálogo de Errores. (Mantenimiento – Catálogo de Error).</p>
                        <p class="txt_gral justificado">
                            2. Selecciona el registro que se vaya a eliminar y da clic en el botón Eliminar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_6c53d762.jpg"
                                alt="" />
                            <br />
                            Figura 9
                        </p>
                        <p class="txt_gral justificado">
                            3. Confirmar Acción.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_3bff65296815de31_html_499ed670.jpg"
                                alt="" />
                            <br />
                            Figura 10
                        </p>
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
                            Requerimientos:
                        </p>
                        <ul>
                            <li>	Implementar patrón funcional de GridView.</li>
                            <li>	Implementa patrón funcional de Mensaje modal.</li>
                            <li>	Filtros dinámicos.</li>
                            <li>	Bitácora.</li>
                            <li>Script de base de datos.- el patrón funcional requiere de las siguientes tablas:
                            </li>
                            <ul class="circle">
                                <li>BDS_C_GR_ERROR</li>
                                <li>BDS_C_GR_IMAGEN</li>
                            </ul>
                            <br />
                            
                            Estas tablas se pueden obtener del script de creación de base de datos de la ODT-02.
                            <br />
                            <br />
                            <li>Código fuente.- Este patrón funcional está compuesto por una página CatalogoErrores.aspx
                                que se encuentra en la carpeta Mantenimiento del proyecto WEBSITE, que hereda de
                                la clase EtiquetaError.vb que se encuentra en el proyecto Entities. </li>
                        </ul>
                        <br />
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a4051f0_html_m5b93easeabc8.png"
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
