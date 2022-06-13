<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocExportarExcel.aspx.vb" Inherits="SEPRIS.DocExportarExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Exportar a Excel</label>
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
                    <div style="text-align:justify;" class="txt_gral">
                        <p class="txt_gral Justificado">
                            Proporcionar al usuario la posibilidad de exportar los datos, presentados en un GridView, a un archivo de formato Excel.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Botón Exportar a Excel
                        </p>
                        <p class="txt_gral Justificado">
                            En ciertos GridView se tendrá un Botón asociado para exportar a un archivo de formato Excel los datos que se muestren en el. Es botón tiene un aspecto como el que se muestra en la siguiente imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_3941c0cc2ee0f6f2_html_m7e32202.png" name="Picture 1" width="105" height="37" border="0" alt="Boton">
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic en el, se mostrara un dialogo del explorador de internet, para descargar el archivo, como se muestra en la siguiente figura.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_3941c0cc2ee0f6f2_html_6977f66d.png" name="Picture 2" width="567" height="30" border="0" alt="Descargar">
                            <br />
                            Figura 2
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Archivo de Excel
                        </p>
                        <p class="txt_gral Justificado">
                            El archivo Excel contiene los registros que se muestran en el GridView, como se muestra en la siguiente imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_3941c0cc2ee0f6f2_html_6f2a3d46.png" name="Picture 3" width="567" height="297" border="0" alt="Archivo">
                            <br />
                            Figura 3
                        </p>                        
                        
                    </div>
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" Width="110px" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" />
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
                            <li>Implementar patrón funcional GridView.</li>
                            <li>Código fuente.- Este patrón funcional está hecho por una clase llamada ExportarExcel.vb</li>
                            <br />
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd4932cffdasda09d85bc_html_10d628.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            Ejemplo de implementación:
                            <ul class="none">
                                <li>1. Agregar un botón para exportar los datos
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd4za33aacffdasda09d85_html_10d628.png"
                                            alt="" />
                                        <br />
                                        Figura 2
                                    </p>
                                </li>
                                <li>2. Agregar el siguiente código en el evento clic del botón.
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd4za33a754564dasda09d85_html_10d628.png"
                                            alt="" />
                                        <br />
                                        Figura 3
                                    </p>
                                </li>
                            </ul>
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
