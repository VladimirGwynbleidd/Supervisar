<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoFormato.aspx.vb" Inherits="SEPRIS.CatalogoFormato" %>
<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>
<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css" >
        .cssEnabled {
            disabled:disabled;
        }
        .cssOculto {
            display:none;
        }

        .cssError {
            font-size:15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo de Formatos</label>
                </div>
                <div style="text-align: left; width: 90%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" Visible="false"  />
                    &nbsp;
                    <asp:Button ID="btnPersonalizaColumnas" runat="server" Text="Personalizar Columnas" Width="150px" />
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="90%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" AllowSorting="True" runat="server" DataKeyNames="N_ID_FORMATO, N_ID_DOCUMENTO, I_ID_AREA, N_FLAG_VIG" ColumnasCongeladas="3" UnicoEnPantalla="false" AutoGenerateColumns="false" 
                    Width="100%" HabilitaSeleccion="SimpleCliente" HiddenFieldSeleccionSimple="hfSelectedValue" HabilitaScroll="true" HeigthScroll="400" WidthScroll="75">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_FORMATO" HeaderStyle-Width="20px" ItemStyle-Width="20px" 
                            SortExpression="N_ID_FORMATO" >
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Nombre del formato" DataField="T_NOM_FORMATO" SortExpression="T_NOM_FORMATO" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="300px" ItemStyle-Width="300px"/>
                        <asp:BoundField HeaderText="Tipo de documento" DataField="T_NOM_DOCUMENTO" SortExpression="T_NOM_DOCUMENTO" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="300px" ItemStyle-Width="300px"/>
                        <asp:TemplateField HeaderText="Archivo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkArchivo" runat="server" Text='<%# Bind("T_NOM_MACHOTE_ORI")%>'
                                    OnClick="MostrarArchivo" CommandArgument='<%# Bind("T_NOM_MACHOTE_ACTUAL")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Área" DataField="T_DSC_AREA" SortExpression="T_DSC_AREA" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="200px" ItemStyle-Width="200px"/>    
                        <asp:BoundField HeaderText="Fecha de registro" DataField="F_FECH_REGISTRO" SortExpression="F_FECH_REGISTRO" ItemStyle-CssClass="Wrap" HeaderStyle-Width="180px" ItemStyle-Width="180px" />
                        <asp:BoundField HeaderText="Usuario registro" DataField="T_DSC_NOMBRE_COMPLETO" SortExpression="T_DSC_NOMBRE_COMPLETO" ItemStyle-CssClass="Wrap" HeaderStyle-Width="180px" ItemStyle-Width="180px" />
                        <asp:BoundField HeaderText="Fecha de actualización" DataField="F_FECH_MODIFICACION" SortExpression="F_FECH_MODIFICACION" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="180px" ItemStyle-Width="180px"  />
                        <asp:BoundField HeaderText="Usuario actualizo" DataField="T_DSC_NOMBRE_COMPLETO_ACT" SortExpression="T_DSC_NOMBRE_COMPLETO_ACT" ItemStyle-CssClass="Wrap" HeaderStyle-Width="180px" ItemStyle-Width="180px" />
                        
                        <asp:TemplateField HeaderText="Estatus" SortExpression="N_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_VIG"))%>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>

                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" AlternateText="No existen registros para la consulta" ImageAlign="Middle" ImageUrl="../Imagenes/no EXISTEN.gif" />
                </div>

                <asp:HiddenField ID="hfGridView1SV_gvConsulta" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH_gvConsulta" runat="server" ClientIDMode="Static" />
                <br />
                <br />
                <asp:Image ID="imgOK" runat="server" />
                <label class="txt_gral">
                    Vigente</label>
                <asp:Image ID="imgERROR" runat="server" />
                <label class="txt_gral">
                    No vigente</label>
                <br />
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
            </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnAgregar" />
                <asp:PostBackTrigger ControlID="btnModificar" />
                <asp:PostBackTrigger ControlID="btnEliminar" />
                <asp:PostBackTrigger ControlID="btnPersonalizaColumnas" />
                <asp:PostBackTrigger ControlID="btnExportaExcel" />
            </Triggers>
        </asp:UpdatePanel>

            <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Formatos" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr runat="server" id="trId">
                            <td align="right">
                                <label class="txt_gral">Identificador</label>
                            </td>
                            <td align="left" colspan="5">
                                <asp:TextBox ID="txtIdFormato" runat="server" ReadOnly="true" onkeydown="return false;" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Nombre del Formato</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNomFormato" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lbltxtNomFormato" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Formato / Machote</label>
                            </td>
                            <td align="left">
                                <asp:FileUpload ID="fuFomato" runat="server" Width="200px" ClientIDMode="Static" />
                                <asp:ImageButton ID="imgfuFomato" runat="server" ClientIDMode="Static" ImageUrl="/Imagenes/Delete.png" OnClientClick="return HabilitafileUpload();"/>
                                <asp:LinkButton ID="lnkfuFomato" runat="server" ClientIDMode="Static" ></asp:LinkButton>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblfuFomato" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Tipo de Documento</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlDocumentos" runat="server" Width="200px"></asp:DropDownList>
                                <asp:HiddenField ID="hfTipoDocumento" runat="server" Value="0" />
                                <asp:Label ID="lblTipoDocumento" runat="server" Text=""  Font-Size="15" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblddlDocumentos" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Área</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlArea" runat="server" Width="200px"></asp:DropDownList>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblddlArea" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Estatus</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlEstatusVig" runat="server" Width="200px"></asp:DropDownList>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblddlEstatusVig" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlBotones" runat="server">
                    <table>
                        <tr>
                            <td colspan="2">
                                <label class="txt_gral">
                                    *Datos Obligatorios</label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClientClick="Deshabilita(this);" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRegresar" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>

            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
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

            <div id="divConfirmacionM2B2A" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgM2B2A" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeUnBotonUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button runat="server" ID="btnSalirM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnContinuarM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();
                    MensajeDosBotonesUnaAccionLoad();

                });

                function validanumero(e) {
                    tecla = (document.all) ? e.keyCode : e.which;
                    if (tecla == 8) return true;
                    patron = /\d/;
                    // patron = /[-][d]*.?[d]*/;
                    // patron = /[-][d]/;
                    return patron.test(String.fromCharCode(tecla));
                }


                function AquiMuestroMensaje() {

                    MensajeUnBotonNoAccion();

                };


                function ConfirmacionEliminar() {

                    MensajeDosBotonesUnaAccion();

                };


                function MensajeFinalizar() {
                    MensajeUnBotonUnaAccion();
                }

                function MensajeConfirmacion() {
                    MensajeDosBotonesUnaAccion();
                }

                function MuestraPdf(chkSub) {
                    if (chkSub.checked) {
                        $("#trPdf").removeClass("cssOculto");
                        $("#trPdf2").removeClass("cssOculto");
                        $("#trPdf3").removeClass("cssOculto");
                    } else {
                        $("#trPdf").addClass("cssOculto");
                        $("#trPdf2").addClass("cssOculto");
                        $("#trPdf3").addClass("cssOculto");
                    }
                    return true;
                }


                function HabilitaTxt(chkSub, txtCaja) {
                    if (chkSub.checked) {
                        $("#" + txtCaja).removeAttr("disabled");
                    } else {
                        $("#" + txtCaja).attr('disabled', 'disabled')
                    }
                    return true;
                }

                function HabilitafileUpload() {
                    $("#imgfuFomato").hide();
                    $("#lnkfuFomato").hide();
                    $("#fuFomato").removeClass("cssOculto");

                    return false;
                }

            </script>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnConsulta" />
            <asp:PostBackTrigger ControlID="btnAceptar" />
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnRegresar" />
            <asp:PostBackTrigger ControlID="imgfuFomatoDoc" />
            <asp:PostBackTrigger ControlID="lnkfuFomatoDoc" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server"/>

    <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
</asp:Content>
