<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoParametros.aspx.vb" Inherits="SEPRIS.DocCatalogoParametros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación Catálogo de Parámetros</label>
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
                            Consultar detalle de registro de parámetro</p>
                        <p class="txt_gral justificado">
                            1. Accede al Catálogo de Parámetros (Seguridad  Catálogo de Parámetros)
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_43d08f6.jpg"
                                alt="" />
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral justificado">
                            2. Haz doble clic sobre un registro del grid para ver el detalle.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_m1d68709b.jpg"
                                alt="" />
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral justificado">
                            3. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Agregar parámetro</p>
                        <p class="txt_gral justificado">
                            1. Accede al Catálogo de Parámetros (Seguridad  Catálogo de Parámetros)
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_43d08f6.jpg"
                                alt="" />
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral justificado">
                            2. Haz clic sobre el botón Agregar.</p>
                        <p class="txt_gral justificado">
                            3. Captura los campos requeridos y da clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_71ce9f25.jpg"
                                alt="" />
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral justificado">
                            4. Confirma acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_m43229dc1.jpg"
                                alt="" />
                            <br />
                            Figura 5
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Modificar parámetro</p>
                        <p class="txt_gral justificado">
                            1. Accede al Catálogo de Parámetros (Seguridad  Catálogo de Parámetros)
                        </p>
                        <p class="txt_gral justificado">
                            2. Selecciona un registro y da clic en el botón Modificar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_62e86767.jpg"
                                alt="" />
                            <br />
                            Figura 6
                        </p>
                        <p class="txt_gral justificado">
                            3. Modifica los campos necesarios y da clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_5de16d8f.jpg"
                                alt="" />
                            <br />
                            Figura 7
                        </p>
                        <p class="txt_gral justificado">
                            4. Confirma acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_m35d2573.jpg"
                                alt="" />
                            <br />
                            Figura 8
                        </p>
                        <p class="txt_gral justificado">
                            5. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Eliminar parámetro.</p>
                        <p class="txt_gral justificado">
                            1. Accede al Catálogo de Parámetros (Seguridad  Catálogo de Parámetros)
                        </p>
                        <p class="txt_gral justificado">
                            2. Selecciona un registro y da clic en el botón Eliminar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_62e86767.jpg"
                                alt="" />
                            <br />
                            Figura 9
                        </p>
                        <p class="txt_gral justificado">
                            3. Confirma acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_504c29dd14b85023_html_499ed670.jpg"
                                alt="" />
                            <br />
                            Figura 10
                        </p>
                        <p class="txt_gral justificado">
                            4. Fin.</p>
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
                            <li>Bitácora.</li>
                            <li>Implementar catálogo de errores.</li>
                            <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_C_GR_PARAMETRO</li>
                            <br />
                       
                            Esta tabla se puede obtener del script de creación de base de datos de la ODT-02.
                            <br />
                            <br />
                            <li>Código fuente.- Este patrón funcional está compuesto por una página CatalogoParametros.aspx
                                que se encuentra en la carpeta Mantenimiento del proyecto WEBSITE, que hereda de
                                la clase Parametros.vb que se encuentra en el proyecto Entities.</li>
                        </ul>
                        <br />
                        <br />
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_171ii7651f0_html_m5b93easeabcq.png"
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
