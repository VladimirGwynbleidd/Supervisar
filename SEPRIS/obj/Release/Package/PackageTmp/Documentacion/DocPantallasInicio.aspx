<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocPantallasInicio.aspx.vb" Inherits="SEPRIS.DocPantallasInicio_aspx_" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Pantallas de Inicio</label>
                </div>
                <br />
                <br />
                <%--  <asp:Button ID="btnManualUsuario" runat="server" Text="Manual Usuario" />--%>
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
                            <p class="SubTitulosWebProyectos">
                                Acerca de
                                <br />
                                <br />
                                Requerimientos:
                            </p>
                            <ul>
                                <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_C_GR_PARAMETRO
                                    y dos registros en esta. El primer registro el campo T_DSC_PARAMETRO deberá tener
                                    el texto “Titulo” y en el campo T_DSC_VALOR el texto que corresponda al título completo
                                    del sistema. El segundo registro deberá tener en el campo T_DSC_PARAMETRO el texto
                                    “DescripcionSistema” y en el campo T_DSC_VALOR el texto que corresponda a la descripción
                                    del sistema. </li>
                            </ul>
                            El script de la tabla BDS_C_GR_PARAMETRO se pueden obtener del script de creación
                            de base de datos de la ODT-02.
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_47a2efca.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            <ul>
                                <li>Código fuente.- Este patrón funcional está compuesto por una página que se encuentra
                                    en raíz del proyecto WEBSITE. </li>
                            </ul>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_m6f1b6cae.png"
                                    alt="" />
                                <br />
                                Figura 2
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Requerimientos mínimos
                                <br>
                                <br>
                                Requerimientos:</p>
                            <ul>
                                <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_C_GR_PARAMETRO.
                                    El patrón funcional codificado leerá los registros que tengan el texto “REQUERIMIENTOS”
                                    en el campo T_DSC_PARAMETRO y mostrará el valor que tenga en la columna T_DSC_VALOR
                                    de la tabla. </li>
                            </ul>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_m72daca54.png"
                                    alt="" />
                                <br />
                                Figura 3
                            </p>
                            <ul>
                                <li>Código fuente.- Este patrón funcional está compuesto por una página Requerimientos.aspx
                                    que se encuentra en raíz del proyecto WEBSITE. </li>
                            </ul>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_15f342c1.png"
                                    alt="" />
                                <br />
                                Figura 4
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Manual de usuario
                                <br />
                                <br />
                                Requerimientos:
                                <ul>
                                    <li>Código fuente.- Este patrón funcional está compuesto por una página Manual.aspx
                                        que se encuentra en raíz del proyecto WEBSITE.</li>
                                    <li>Manual.- El manual de usuario deberá estar dentro del proyecto. Nota: El evento
                                    onclick del botón de descarga del manual deberá hacer referencia a la ubicación
                                    y nombre del manual.</li>
                                </ul>
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_m7b0f107b.png"
                                        alt="" />
                                    <br />
                                    Figura 5
                                </p>
                                <p class="SubTitulosWebProyectos">
                                    Responsables
                                    <br />
                                    <br />
                                    Requerimientos
                                </p>
                                <ul>
                                    <li>Script de base de datos.- El patrón funcional requiere de la tabla BDS_C_GR_PARAMETRO.
                                        El cual lee de los registros tengan el texto “MESA AYUDA TECNICA” y “MESA AYUDA
                                        FUNCIONAL” en el campo T_DSC_PARAMETRO y mostrará el valor que tenga en la columna
                                        T_DSC_VALOR de la tabla. El campo T_DSC_VALOR deberá tener el nombre, puesto, correo
                                        electrónico, y teléfono de contacto del responsable en ese orden. </li>
                                </ul>
                                Ejemplo: INSERT INTO BDS_C_GR_PARAMETRO (N_ID_PARAMETRO, T_DSC_PARAMETRO, T_OBS_PARAMETRO,
                                T_DSC_VALOR, N_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG) VALUES (11, 'MESA AYUDA
                                FUNCIONAL', 'DASDA', 'Álvaro Cruz García, Responsable del Sistema ,acruz@consar.gob.mx,3000-2515',
                                1, GETDATE(), NULL)
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_25ebe1a1.png"
                                        alt="" />
                                    <br />
                                    Figura 6
                                </p>
                                <ul>
                                    <li>Código fuente.- Este patrón funcional está compuesto por una página Responsables.aspx
                                        que se encuentra en raíz del proyecto WEBSITE. </li>
                                </ul>
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_17113a053e4051f0_html_m5b9eabc8.png"
                                        alt="" />
                                    <br />
                                    Figura 7
                                </p>
                                <br />
                                <br />
                    </div>
                    <asp:Button ID="btnRegresarTecnico" runat="server" Text="Regresar" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <%--   <asp:PostBackTrigger ControlID="btnManualUsuario" />--%>
            <asp:PostBackTrigger ControlID="btnManualTecnico" />
            <asp:PostBackTrigger ControlID="btnRegresarUusario" />
            <asp:PostBackTrigger ControlID="btnRegresarTecnico" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
