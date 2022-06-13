<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="BandejaOPI.aspx.vb" Inherits="SEPRIS.BandejaOPI"  EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
    <%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
    <%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />

    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Bandeja de Oficios</label>
            </div>
            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                &nbsp;
                <asp:Button ID="btnPersonalizarColumnas" runat="server" Text="Personalizar Columnas"
                    Style="width: 130px;" />
            </div>
            <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
            <br />
            <cc1:CustomGridView ID="gvConsultaOPI" runat="server" DataKeyNames="I_ID_OPI,I_ID_ESTATUS" AllowSorting="true" AutoGenerateColumns="false" HabilitaScroll="true" HeigthScroll="400" UnicoEnPantalla="true" HeaderStyle-Font-Size="14"
                ColumnasCongeladas="1" HabilitaSeleccion="No">
                <Columns>
                    <asp:BoundField HeaderText="" DataField="I_ID_ESTATUS" Visible="false"></asp:BoundField>
                    <asp:BoundField HeaderText="IDFolio" Visible="False" DataField="I_ID_OPI" SortExpression="I_ID_OPI"></asp:BoundField>
                    <asp:BoundField HeaderText="Folio OPI" Visible="true" DataField="T_ID_FOLIO" SortExpression="T_ID_FOLIO"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-Width="200px"></asp:BoundField>
                    <asp:TemplateField HeaderText="Entidad" HeaderStyle-Width="200px">
                         <ItemTemplate>
                            <table width="100%">
                                <tr>
                                    <td style="border-right:0; border-bottom: 0">
                                        <asp:Image ID="logoImg" runat="server" />
                                    </td>
                                    <td style="border-right:0; border-bottom: 0">
                                        <asp:Label ID="lblEnt" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Fecha de incumplimiento" DataField="F_FECH_POSIBLE_INC" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_DOC"></asp:BoundField>
                    <asp:BoundField HeaderText="Área" DataField="T_DSC_AREA" Visible="true" SortExpression="T_DSC_AREA"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de registro" DataField="F_FECH_REGISTRO" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_REGISTRO"></asp:BoundField>
                    <asp:BoundField HeaderText="Clasificación" DataField="T_DSC_CLASIFICACION" SortExpression="T_DSC_CLASIFICACION"></asp:BoundField>
                    <asp:BoundField HeaderText="Paso actual" DataField="N_ID_PASO" SortExpression="N_ID_PASO"></asp:BoundField>
                    <asp:BoundField HeaderText="Días transcurridos <br /> en el paso actual" DataField="DIAS_TRANSC" SortExpression="DIAS_TRANSC" HtmlEncode="False" ></asp:BoundField>
                    <asp:TemplateField HeaderText="Supervisor(es)" SortExpression="D_SUPERVISORES">
                        <ItemTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("D_SUPERVISORES")%>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inspector(es)" SortExpression="D_INSPECTORES">
                        <ItemTemplate>
                            <asp:Literal ID="Literal2" runat="server" Text='<%# Eval("D_INSPECTORES")%>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="Proceso" DataField="PR_DSC" SortExpression="PR_DSC"></asp:BoundField>
                    <asp:BoundField HeaderText="Subproceso" DataField="T_DSC_SUBPROCESO" SortExpression="T_DSC_SUBPROCESO"></asp:BoundField>
                    <asp:BoundField HeaderText="¿Procede?" DataField="B_PROCEDE" SortExpression="B_PROCEDE"></asp:BoundField>
                    <asp:BoundField  HeaderText="Días transcurridos <br /> desde el posible incumplimiento" 
                        DataField="DIAS_HABILES" SortExpression="DIAS_HABILES" HtmlEncode="False"
                        HeaderStyle-Width="80px"></asp:BoundField>
                    <asp:BoundField HeaderText="Estatus" DataField="T_DSC_ESTATUS" SortExpression="T_DSC_ESTATUS"></asp:BoundField>
                    <asp:BoundField HeaderText="Folio SISAN"  Visible ="true" DataField="T_ID_FOLIO_SISAN" SortExpression="T_ID_FOLIO_SISAN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-Width="200px"></asp:BoundField>
                    <%--  <asp:BoundField HeaderText="Fecha de Envío a Sanciones" DataField="F_FECH_ENVIA_SANCIONES" SortExpression="F_FECH_ENVIA_SANCIONES"></asp:BoundField>--%>
                     <asp:TemplateField HeaderText="Fecha de Envío a Sanciones">
                        <ItemTemplate>
                            <table width="100%">
                                <tr>                                    
                                    <td style="border-right:0; border-bottom: 0">
                                        <asp:Label ID="F_FECH_ENVIA_SANCIONES" runat="server"/>                                         
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Fecha de Envío a Sanciones">
                        <ItemTemplate >
                            <table width="100%">
                                <tr >
                                    <td style="border-right:0; border-bottom: 0">
                                        <label ID="F_FECH_IRREGULARIDAD" runat ="server"></label>

                                    </td>
                                </tr>

                            </table>
                        </ItemTemplate>

                    </asp:TemplateField>--%>
                </Columns>
            </cc1:CustomGridView>
            <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                alt="No existen Registos para la Consulta" />
            <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
            <br />
            <br />
            <br />
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                                ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
            <script type="text/javascript">
                $(document).ready(function () {
                    $("#ddlCustomControlFilter").hide();
                });
                    

                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();

                });


                function AquiMuestroMensaje() {

                    MensajeUnBotonNoAccion();

                };


                function ConfirmacionEliminar() {

                    MensajeDosBotonesUnaAccion();

                };


                function MensajeConfirmacion() {
                    MensajeDosBotonesUnaAccion();
                }

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnConsulta" />
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnPersonalizarColumnas" />
        </Triggers>
    </asp:UpdatePanel>

    <wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server" />
</asp:Content>