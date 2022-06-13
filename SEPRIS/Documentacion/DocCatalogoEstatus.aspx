<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoEstatus.aspx.vb" Inherits="SEPRIS.DocCatalogoEstatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación Catálogo de Estatus</label>
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
                        <p class="txt_gral SubTitulosWebProyectos">
                            Consultar detalle de estatus
                        </p>
                        <p class="txt_gral justificado">
                            1. Accede a la bandeja de Catálogo de estatus (Mantenimiento  Catálogo de estatus).</p>
                        <p class="txt_gral justificado">
                            2. Doble clic sobre un registro para ver el detalle del mensaje.
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m663c87d2.jpg"
                                    width="567" />
                                <br />
                                Figura 1
                            </p>
                            <p>
                            </p>
                            <p class="txt_gral justificado">
                                Esto nos envía a la pantalla de mensajes de correo
                            </p>
                            <p class="txt_gral justificado">
                                3. Fin.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_47c36c9c.jpg"
                                    width="567" />
                                <br />
                                Figura 2
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Agregar estatus
                            </p>
                            <p class="txt_gral justificado">
                                1.Accede a la bandeja de Catálogo de estatus (Mantenimiento  Catálogo de estatus).
                            </p>
                            <p class="txt_gral justificado">
                                2. Dar clic sobre el botón Agregar</p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m7d9302a9.jpg"
                                    width="567" />
                                <br />
                                Figura 3
                            </p>
                            <p class="txt_gral justificado">
                                3. Capturar los campos obligatorios, en el caso de la imagen, seleccionar una del
                                grid.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_30c5dcd3.jpg"
                                    width="567" />
                                <br />
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m57697114.jpg" />
                                <br />
                                Figura 4
                            </p>
                            <p class="txt_gral justificado">
                                4. Dar clic en el botón Aceptar.
                            </p>
                            <p class="txt_gral justificado">
                                5. Confirmar Acción.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m32378021.jpg" />
                                <br />
                                Figura 5
                            </p>
                            <p class="txt_gral justificado">
                                6. Fin.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m23f17455.jpg" />
                                <br />
                                Figura 6
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Modificar estatus
                            </p>
                            <p class="txt_gral justificado">
                                1. Accede a la bandeja de Catálogo de estatus (Mantenimiento  Catálogo de estatus).</p>
                            <p class="txt_gral justificado">
                                2. Seleccionar un registro del grid y dar clic en el botón Modificar.</p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m716ed1d0.jpg" />
                                <br />
                                Figura 7
                            </p>
                            <p class="txt_gral justificado">
                                3. Se puede actualizar la descripción o la imagen que tiene asociada.</p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_4b4a0c8a.jpg" />
                                <br />
                                Figura 8
                            </p>
                            <p class="txt_gral justificado">
                                4. Clic en el botón Aceptar.</p>
                            <p class="txt_gral justificado">
                                5. Confirmar acción.</p>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_5b1c36a1.jpg"
                                    alt="" />
                                <br />
                                Figura 9
                            </p>
                            <p class="txt_gral justificado">
                                6. Fin.</p>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_3a213f43.jpg"
                                    alt="" />
                                <br />
                                Figura 10
                            </p>
                            <br />
                            <p class="SubTitulosWebProyectos">
                                Eliminar estatus.</p>
                            <p class="txt_gral justificado">
                                1. Accede a la bandeja de Catálogo de estatus (Mantenimiento  Catálogo de estatus).
                            </p>
                            <p class="txt_gral justificado">
                                2. Seleccionar un registro del grid y dar clic en el botón Eliminar.
                            </p>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m65bb34fc.jpg"
                                    alt="" />
                                <br />
                                Figura 11
                            </p>
                            <p class="txt_gral justificado">
                                3. Confirmar acción.
                            </p>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_m281300bf.jpg"
                                    alt="" />
                                <br />
                                Figura 12
                            </p>
                            <p class="txt_gral justificado">
                                4. Fin.
                            </p>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_81846839fac8c0da_html_4a9692b8.jpg"
                                    alt="" />
                                <br />
                                Figura 13
                            </p>
                    </div>
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" Width="110px"/>
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


                           <li> Script de base de datos.- el patrón funcional requiere de las siguientes tablas:</li>
                            <ul class="circle">
                                <li>BDS_C_GR_ESTATUS </li>
                                <li>BDS_C_GR_IMAGEN</li>
                            </ul>
                            <p class="txt_gral justificado">
                                Estas tablas se pueden obtener del script de creación de base de datos de la ODT-02.
                            </p>
                            <li>Código fuente.- Este patrón funcional está compuesto por una página CatalogoEstatus.aspx
                                que se encuentra en la carpeta Mantenimiento del proyecto WEBSITE, que hereda de
                                la clase Estatus.vb que se encuentra en el proyecto Entities.</li>
                        </ul>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_04f254s655ed9w233a_html_4dascb47ffe0112.png" alt="" />
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
