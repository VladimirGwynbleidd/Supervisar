<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="BandejaPC.aspx.vb" Inherits="SEPRIS.BandejaPC" EnableEventValidation="false" %>

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
                    Bandeja de PC's</label>
            </div>
            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                &nbsp;
                <asp:Button ID="btnPersonalizarColumnas" runat="server" Text="Personalizar Columnas"
                    Style="width: 130px;" />
            </div>
          
            <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
            <br />
            <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="N_ID_FOLIO, N_ID_SICOD" AutoGenerateColumns="false" HabilitaScroll="true" HeigthScroll="400" UnicoEnPantalla="true" HeaderStyle-Font-Size="14"
                ColumnasCongeladas="1" HabilitaSeleccion="No" Width="100%">
                <Columns>
                    <asp:BoundField HeaderText="Folio" DataField="I_ID_FOLIO_SUPERVISAR" SortExpression="I_ID_FOLIO_SUPERVISAR"></asp:BoundField>
                    <asp:TemplateField HeaderText="Entidad">
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
                    <asp:BoundField DataField="ID_ENTIDAD" visible="false"></asp:BoundField>
                    <asp:TemplateField HeaderText="Sub Entidad" Visible="false" >
                        <ItemTemplate>
                            <asp:Image ID="SubEnt" ClientIDMode="Static" onclick="" ImageUrl="~/Imagenes/Subvisitas.png" name="SubEnt" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Núm. PC de Entidad" DataField="T_DSC_NUM_OFICIO" SortExpression="T_DSC_NUM_OFICIO"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de documento" DataField="F_FECH_DOC" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_DOC"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de recepción" DataField="F_FECH_RECEPCION" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_RECEPCION"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de Vencimiento" DataField="F_FECH_VENC" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_VENC"></asp:BoundField>
                    <asp:BoundField HeaderText="Área" DataField="T_DSC_AREA" SortExpression="T_DSC_AREA"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de registro" DataField="F_FECH_REGISTRO" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_REGISTRO"></asp:BoundField>
                    <asp:BoundField HeaderText="Paso actual" DataField="T_DSC_PASO" SortExpression="T_DSC_PASO"></asp:BoundField>
                    <asp:BoundField HeaderText="Estatus" DataField="T_DSC_ESTATUS" SortExpression="T_DSC_ESTATUS"></asp:BoundField>
                    <asp:BoundField HeaderText="Supervisor(es)" DataField="D_SUPERVISORES"></asp:BoundField>
                    <asp:BoundField HeaderText="Inspector(es)" DataField="D_INSPECTORES"></asp:BoundField>
                    <asp:BoundField HeaderText="Proceso" DataField="PR_DSC" SortExpression="PR_DSC"></asp:BoundField>
                    <asp:BoundField HeaderText="Subproceso" DataField="SPR_DSC" SortExpression="SPR_DSC"></asp:BoundField>
                    <asp:TemplateField HeaderText="PC Cumple?">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# If(Convert.ToInt32(Eval("I_ID_ESTATUS")) > 5, If(Convert.ToBoolean(Eval("I_ID_PC_CUMPLE")), "Si", "No"), "")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Resolución">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# If(Convert.ToInt32(Eval("I_ID_RESOLUCION")) = 1, "No Procede", If(Convert.ToInt32(Eval("I_ID_RESOLUCION")) = 2, "Procede", If(Convert.ToInt32(Eval("I_ID_RESOLUCION")) = 3, "Procede Con Plazo", If(Convert.ToInt32(Eval("I_ID_RESOLUCION")) = 4, "No Presentado", ""))))%>'/>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:BoundField HeaderText="Folio SISAN" DataField="T_ID_FOLIO_SISAN" SortExpression="T_ID_FOLIO_SISAN"></asp:BoundField>
                     <asp:TemplateField HeaderText="Fecha de Envío a Sanciones">
                        <ItemTemplate>
                            <table width="100%">
                                <tr>
                                    
                                    <td style="border-right:0; border-bottom: 0">
                                        <asp:Label ID="F_FECH_ENVIA_SANCIONES" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
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
                              <%--  <%= Mensaje%>--%>
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
                              <%--  <%= Mensaje%>--%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnSubentidad" Style="display: none" ClientIDMode="Static" />
            <script type="text/javascript">
                $(document).ready(function () {
                    $("#ddlCustomControlFilter").hide();
                });

                function FolioSubent() {

                    $('#divTextoRespuesta').html('Folio de subentidades:</br>' + "<%=ViewState("subents")%>");

                        $("#divMensajeRespuesta").dialog({
                            resizable: false,
                            autoOpen: true,
                            height: 300,
                            width: 500,
                            modal: true,
                            open:
                            function (event, ui) {
                                $(this).parent().css('z-index', 3999);
                                $(this).parent().appendTo(jQuery("form:last"));
                                $('.ui-widget-overlay').css('position', 'fixed');
                                $('.ui-widget-overlay').css('z-index', 3998);
                                $('.ui-widget-overlay').appendTo($("form:last"));
                            },
                            buttons: {
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                }
                    



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

    <div id="divMensajeRespuesta" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image2" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <div id="divTextoRespuesta"></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>


</asp:Content>
