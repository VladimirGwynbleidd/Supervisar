<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoDocumento.aspx.vb" Inherits="SEPRIS.CatalogoDocumento" %>
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
                        Catálogo de Documentos</label>
                </div>
                <div style="text-align: left; width: 90%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" Visible="false"  />
                    &nbsp;
                    <asp:Button ID="btnPersonalizaColumnas" runat="server" Text="Personalizar Columnas" Width="150px" />
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="90%" />
                <br />
                <cc1:CustomGridView ID="gvConsultaDocs" AllowSorting="True" runat="server" DataKeyNames="N_ID_DOCUMENTO" ColumnasCongeladas="3" UnicoEnPantalla="false" AutoGenerateColumns="false" 
                    Width="100%" HabilitaSeleccion="SimpleCliente" HiddenFieldSeleccionSimple="hfSelectedValue" HabilitaScroll="true" HeigthScroll="400" WidthScroll="75">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_DOCUMENTO" HeaderStyle-Width="20px" ItemStyle-Width="20px" 
                            SortExpression="N_ID_DOCUMENTO" >
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Nombre del documento" DataField="T_NOM_DOCUMENTO_CAT" SortExpression="T_NOM_DOCUMENTO_CAT" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="300px" ItemStyle-Width="300px" />
                        <asp:BoundField HeaderText="Nombre corto" DataField="T_NOM_CORTO" SortExpression="T_NOM_CORTO" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="250px" ItemStyle-Width="250px"/>
                        <asp:BoundField HeaderText="Fecha de registro" DataField="F_FECH_REGISTRO" SortExpression="F_FECH_REGISTRO" ItemStyle-CssClass="Wrap" HeaderStyle-Width="180px" ItemStyle-Width="180px" />
                        <asp:BoundField HeaderText="Fecha de actualización" DataField="F_FECH_MODIFICACION" SortExpression="F_FECH_MODIFICACION" ItemStyle-CssClass="Wrap"  HeaderStyle-Width="180px" ItemStyle-Width="180px"  />
                        <asp:BoundField HeaderText="Fecha fin de vigencia" DataField="F_FECH_FIN_VIG" SortExpression="F_FECH_FIN_VIG" ItemStyle-CssClass="Wrap" />
                        
                        <%--<asp:TemplateField HeaderText="Documento">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkArchivo" runat="server" Text='<%# Bind("T_NOM_MACHOTE_ORI")%>'
                                    OnClick="MostrarArchivo" CommandArgument='<%# Bind("T_NOM_MACHOTE_ACTUAL")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:BoundField HeaderText="Usuario Registro" DataField="T_ID_USUARIO" SortExpression="T_ID_USUARIO" ItemStyle-CssClass="Wrap" />
                        <asp:BoundField HeaderText="Usuario Actualizo" DataField="T_ID_USUARIO_MOD" SortExpression="T_ID_USUARIO_MOD" ItemStyle-CssClass="Wrap" />
                        <asp:BoundField HeaderText="Paso en que se solicita" DataField="I_ID_PASO_INI" SortExpression="I_ID_PASO_INI" >
                            <ItemStyle CssClass="Wrap" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Estatus" SortExpression="N_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_VIG"))%>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Heredar" SortExpression="N_FLAG_HEREDA">
                            <ItemTemplate>
                                <asp:Image ID="imagenHerencia" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_HEREDA"))%>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Heredar Subvisitas" SortExpression="N_FLAG_HEREDA_ENTRE_SBVISITA">
                            <ItemTemplate>
                                <asp:Image ID="imagenHerenciaSub" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_HEREDA_ENTRE_SBVISITA"))%>' />
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                            <HeaderStyle Width="100px" />
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Documentos" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr runat="server" id="trId">
                            <td align="right">
                                <label class="txt_gral">Identificador</label>
                            </td>
                            <td align="left" colspan="5">
                                <asp:TextBox ID="txtIdDoc" runat="server" ReadOnly="true" onkeydown="return false;" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Nombre del documento</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNomDoc" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lbltxtNomDoc" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Nombre corto</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNomCorto" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lbltxtNomCorto" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Tipo de documento</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlTipoDoc" runat="server" Width="200px"></asp:DropDownList>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblddlTipoDoc" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td align="right">
                                <label class="txt_gral">Formato / Machote</label>
                            </td>
                            <td align="left">
                                <asp:FileUpload ID="fuFomatoDoc" runat="server" Width="200px" ClientIDMode="Static" />
                                <asp:ImageButton ID="imgfuFomatoDoc" runat="server" ClientIDMode="Static" ImageUrl="/Imagenes/Delete.png" OnClientClick="return HabilitafileUpload();"/>
                                <asp:LinkButton ID="lnkfuFomatoDoc" runat="server" ClientIDMode="Static" ></asp:LinkButton>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblfuFomatoDoc" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Fecha de inicio de vigencia</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFechaIniVig" runat="server" Width="200px"></asp:TextBox>

                                <%--<cc1:CalendarExtender ID="caltxtFechaIniVig" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario1"
                                    TargetControlID="txtFechaIniVig" CssClass="teamCalendar" />--%>
                                <cc1:CalendarExtender runat="server" ID="caltxtFechaIniVig" TargetControlID="txtFechaIniVig"
                                    Format="dd/MM/yyyy" PopupButtonID="imgCalendario1" DaysModeTitleFormat="d/M/yyyy"
                                    TodaysDateFormat="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgCalendario1" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                                    ImageAlign="Bottom" Height="16px" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lbltxtFechaIniVig" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
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
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Orden de lista</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtOrden" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lbltxtOrden" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td align="right">
                                <label class="txt_gral">Permitir carga de varios archivos</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkVersiones" runat="server" onclick="return HabilitaTxt(this, 'txtNumVersiones');"/>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblchkVersiones" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                            <td align="center" id="tdNumArchUno">
                                <label class="txt_gral">Cuantos archivos? (Max. 5)</label>
                            </td>
                            <td align="left" id="tdNumArchDos" style="width:50px;">
                                <asp:TextBox ID="txtNumVersiones" ClientIDMode="Static" Enabled="false"  runat="server" Width="100%" MaxLength="1"></asp:TextBox>
                            </td>
                            <td align="left" id="tdNumArchTres">
                                <asp:Label ID="lbltxtNumVersiones" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Paso inicial para habilitar el documento</label>
                            </td>
                            <td align="left"colspan="4">
                                <asp:DropDownList ID="ddlPasoIni" runat="server" Width="100%"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblddlPasoIni" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2"><label class="txt_gral"> al paso </label></td>
                            <td colspan="2">&nbsp;</td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td align="left" colspan="4">
                                <asp:DropDownList ID="ddlPasoFin" runat="server" Width="100%"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblddlPasoFin" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Validar como obligatorio</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkObligatorio" runat="server" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkObligatorio" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Heredar este documento (Visita -> Subvisitas)</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkHeredar" runat="server" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkHeredar" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Heredar este documento (Subvisita -> Subvisitas)</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkHeredarSub" runat="server" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkHeredarSub" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Utilizar nomenclatura</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkNomenclatura" runat="server" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkNomenclatura" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Permitir buscar este archivo en SICOD</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkSicod" runat="server" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkSicod" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">Solicitar confirmación de carga completa? <br />(Afecta a todos los documentos del paso)</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkConfirm" runat="server" />
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkConfirm" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr  style="display:none;">
                            <td align="right">
                                <label class="txt_gral">Permitir version de este archivo en PDF</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkPdf" runat="server" onclick="return MuestraPdf(this);"/>
                            </td>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblchkPdf" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr id="trPdf" clientidmode="Static" runat="server" class="cssOculto">
                            <td align="right">
                                <label class="txt_gral">Paso inicial para habilitar la version en PDF</label>
                            </td>
                            <td align="left" colspan="4">
                                <asp:DropDownList ID="ddlPasoIniPdf" runat="server" Width="100%"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblddlPasoIniPdf" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
                            </td>
                        </tr>
                        <tr id="trPdf2" clientidmode="Static" runat="server" class="cssOculto">
                            <td align="right" colspan="2"><label class="txt_gral"> al paso </label></td>
                            <td colspan="2">&nbsp;</td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr id="trPdf3" clientidmode="Static" runat="server" class="cssOculto">
                            <td>&nbsp;</td>
                            <td align="left" colspan="4">
                                <asp:DropDownList ID="ddlPasoFinPdf" runat="server" Width="100%"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblddlPasoFinPdf" runat="server" Text=" *" Visible="false" Font-Size="13" Font-Bold="true" ForeColor="Red" ></asp:Label>
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
                    $("#imgfuFomatoDoc").hide();
                    $("#lnkfuFomatoDoc").hide();
                    $("#fuFomatoDoc").removeClass("cssOculto");

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
