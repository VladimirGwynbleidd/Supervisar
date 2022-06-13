<%@ Page Title="" Language="vb" Debug="true" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoMensajes.aspx.vb" Inherits="SEPRIS.CatalogoMensajes" %>

<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="txtposicionText" ClientIDMode="Static" runat="server" style="display:none" ></asp:TextBox>
    <asp:TextBox ID="txtposicionTextArea" ClientIDMode="Static" runat="server" style="display:none" ></asp:TextBox>

    <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:UpdatePanel runat="server" ID="upnlConsulta">
            <ContentTemplate>
                <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                    <div align="center" style="padding: 20px 20px 15px 20px">
                        <asp:Label ID="lblTitulo" runat="server" Text="Catálogo de Mensajes de Correo " CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label>
                    </div>
                    <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                        <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    </div>

                    <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                    <br />
                    <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="N_ID_CORREO" Width="100%" AllowSorting="true" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkElemento" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="15px" />
                                <HeaderStyle Width="15px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Clave" DataField="N_ID_CORREO" SortExpression="N_ID_CORREO">
                                <ItemStyle Width="50px" />
                                <HeaderStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Descripción" DataField="T_DSC_CORREO" SortExpression="T_DSC_CORREO" ItemStyle-CssClass="Wrap" />
                            <asp:TemplateField HeaderText="Estatus" SortExpression="N_FLAG_VIG">
                                <ItemTemplate>
                                    <asp:Image ID="imagEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_VIG"))  %>' />
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>
                        </Columns>

                    </cc1:CustomGridView>

                    <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                        <asp:Image ID="Image1" runat="server"
                            AlternateText="No existen registros para la consulta" ImageAlign="Middle"
                            ImageUrl="../Imagenes/no EXISTEN.gif" />
                    </div>
                    <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />

                    <div align="center" style="padding: 20px 20px 15px 20px">
                        <asp:Image ID="imgOK" runat="server" />
                        <label class="txt_gral">Vigente</label>
                        <asp:Image ID="imgERROR" runat="server" />
                        <label class="txt_gral">No vigente</label>
                    </div>
                    <div align="center" style="padding: 20px 20px 15px 20px">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                        &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
                        &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
                        &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnRegresarCorreos" runat="server" Text="Regresar" />
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                    <div align="center" style="padding: 20px 20px 15px 20px">
                        <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Registro de mensajes de Correo" EnableTheming="false"></asp:Label>
                    </div>

                    <asp:Panel ID="pnlControles" runat="server">
                        <table>
                            <tr>
                                <td align="right">
                                    <label class="txt_gral">Clave:</label></td>
                                <td align="left">
                                    <asp:TextBox ID="txtClave" runat="server" CssClass="txt_solo_lectura"
                                        Enabled="false" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <label class="txt_gral">Descripción*:</label></td>
                                <td align="left">
                                    <asp:TextBox ID="txtDescripcion" runat="server"
                                        CssClass="txt_gral" Width="300px" MaxLength="255"></asp:TextBox>
                                    <asp:CustomValidator ID="cvDescripcion" ControlToValidate="txtDescripcion" ValidationGroup="Forma" ValidateEmptyText="true"
                                        runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <!--COMIENZO el menu de los marcadores-->
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <label class="txt_gral">Asunto*:</label></td>
                                                    <td align="left">                                                        
                                                        <asp:TextBox ID="txtAsunto" runat="server" onblur="PosicionText();" ClientIDMode="Static"
                                                            CssClass="txt_gral" Width="300px" MaxLength="255"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvAsunto" ControlToValidate="txtAsunto" ValidationGroup="Forma" ValidateEmptyText="true"
                                                            runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="BtnAgregaAsunto" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <label class="txt_gral">Cuerpo*:</label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtMensaje" runat="server" onblur="ReemplazaEspeciales();" ClientIDMode="Static"
                                                            CssClass="txt_gral" Width="300px" MaxLength="400" TextMode="MultiLine" Height="100"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvMensaje" ControlToValidate="txtMensaje" ValidationGroup="Forma" ValidateEmptyText="true"
                                                            runat="server" EnableClientScript="true" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="BtnAgregaCuerpo" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                        <td>
                                            <br />
                                            <div style="width: 330px; height: 150px; overflow: auto;">
                                                <cc1:CustomGridView ID="CGVMarcadores" SkinID="SinTooltip" runat="server" DataKeyNames="T_DSC_ETIQUETA" AllowSorting="true" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkElemento" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15px" />
                                                            <HeaderStyle Width="15px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Descripcion" DataField="T_DSC_MARCADOR">
                                                            <ItemStyle Width="300px" />
                                                            <HeaderStyle Width="300px" />
                                                        </asp:BoundField>
                                                    </Columns>

                                                </cc1:CustomGridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <!--FIN el menu de los marcadores-->
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlBotones" runat="server">
                        <table>
                            <tr>
                                <td align="left" colspan="2">
                                    <label class="txt_gral">*Datos Obligatorios</label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClientClick="ReemplazaEspeciales();Deshabilita(this);" />
                                    &nbsp;&nbsp;&nbsp;
                         <asp:Button ID="btnCancelar" runat="server"
                             Text="Cancelar" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlRegresar" runat="server" Visible="false">
                        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" />
                    </asp:Panel>

                </asp:Panel>

                <div id="divMensajeUnBotonNoAccion" style="display: none">
                    <table width="100%">
                        <tr>
                            <td style="width: 50px; text-align: center; vertical-align: top">
                                <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                            </td>
                            <td style="text-align: left">
                                <div class="MensajeModal-UI">
                                    <%= Mensaje%>
                                    <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" EnableTheming="false" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divConfirmacionM2B2A" style="display: none">
                    <table width="100%">
                        <tr>
                            <td style="width: 50px; text-align: center; vertical-align: top">
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
                            <td style="width: 50px; text-align: center; vertical-align: top">
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
                            <td style="width: 50px; text-align: center; vertical-align: top">
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
                    
                    //function devPos() {
                    //    input = document.getElementById('txtMensaje');
                    //    if (typeof document.selection != 'undefined' && document.selection && typeof input.selectionStart == 'undefined') {

                    //        var str = document.selection.createRange();
                    //        stored_range = str;
                    //        stored_range.moveToElementText(input);
                    //        stored_range.setEndPoint('EndToEnd', str);
                    //        input.selectionStart = stored_range.text.length - str.text.length;
                    //        input.selectionEnd = input.selectionStart + str.text.length;
                    //        alert(input.selectionStart - input.selectionEnd)
                    //        alert(input.selectionEnd - input.selectionStart)
                    //        $("#txtposicionTextArea").val(input.selectionStart);
                    //    } else if (typeof input.selectionStart != 'undefined') {
                    //        //alert(input.selectionStart);
                    //        $("#txtposicionTextArea").val(input.selectionStart);
                    //    }
                    //}

                    function ReemplazaEspeciales() {

                        PosicionTextArea();

                        //devPos();

                        var mens = document.getElementById("txtMensaje").value.replace(/</g, "##");
                        mens = mens.replace(/\//g, "|");
                        mens = mens.replace(/>/g, "**");
                        document.getElementById("txtMensaje").value = "";
                        document.getElementById("txtMensaje").value = mens;
                    }

                    /*RRA*/

                    function PosicionText() {

                        var iCaretPos = 0;
                        var oField = document.getElementById("txtAsunto");
                        // IE Support
                        if (document.selection) {

                            // Set focus on the element
                            oField.focus();

                            // To get cursor position, get empty selection range
                            var oSel = document.selection.createRange();

                            // Move selection start to 0 position
                            oSel.moveStart('character', -oField.value.length);

                            // The caret position is selection length
                            iCaretPos = oSel.text.length;
                        }

                            // Firefox support
                        else if (oField.selectionStart || oField.selectionStart == '0')
                            iCaretPos = oField.selectionStart;

                        // Return results
                        $("#txtposicionText").val(iCaretPos);
                    }

                      function PosicionTextArea() {
                        var iCaretPos = 0;
                        var oField = document.getElementById("txtMensaje");

                        // IE Support
                        if (document.selection) {

                            // Set focus on the element
                            oField.focus();

                            // To get cursor position, get empty selection range
                            var oSel = document.selection.createRange();

                            // Move selection start to 0 position
                            oSel.moveStart('character', -oField.value.length);
                            oSel.select();
                            // The caret position is selection length
                            iCaretPos = oSel.text.length;
                        }

                            // Firefox support
                        else if (oField.selectionStart || oField.selectionStart == '0')
                            iCaretPos = oField.selectionStart;
                        // Return results
                        $("#txtposicionTextArea").val(iCaretPos);

                    }

                    /*-----------------------*/


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

                </script>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
                <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
                <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
                <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
                <asp:PostBackTrigger ControlID="btnConsulta" />
                <asp:PostBackTrigger ControlID="btnAceptar" />
                <asp:PostBackTrigger ControlID="btnExportaExcel" />
                <asp:PostBackTrigger ControlID="btnRegresar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

</asp:Content>
