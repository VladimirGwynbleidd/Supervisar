<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="AltaSolicitud.aspx.vb" Inherits="SEPRIS.AltaSolicitud" ValidateRequest="false" %>

<%@ Register Assembly="CustomGridView" Namespace="CustomGridView" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            MensajeDosBotonesUnaAccionLoad();
            MensajeUnBotonNoAccionLoad();
        });

        function LevantaVentanaConfirma() {
            MensajeDosBotonesUnaAccion();
        }

        function MuestraMensajeUnBotonNoAccion() {
            MensajeUnBotonNoAccion();
            return false;
        }
        function ResetScroll() {
            $('<%= acServicios.ClientID%>').scrollTop(0);
        }
        function ItemSelected() {
            __doPostBack('<%= acServicios.ClientID%>', "ItemSelected");
        }       
    </script>
    <style type="text/css">
        .txt_gral
        {
            margin-left: 0px;
        }
        #grupo_seleccionado
        {
            border: 5px solid #426939;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />
    <asp:UpdatePanel ID="udpSolicitudes" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hdfImagen" ClientIDMode="Static" Value="-1" />
            <div style="visibility: hidden;">
                <asp:Button runat="server" ID="lnkImagen" ClientIDMode="Static" />
            </div>
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Alta Solicitud</label>
            </div>
            <table style="text-align: left; width: 1000px; border-collapse: collapse">
                <tr>
                    <td style="text-align: left; width: 175px; border-left: 1px solid black; border-top: 1px solid black;
                        padding-left: 15px; padding-top: 15px;">
                        <label class="txt_gral">
                            Usuario que registra la solicitud:</label>
                    </td>
                    <td style="text-align: left; width: 300px; border-top: 1px solid black; padding-top: 15px;">
                        <asp:TextBox ID="txtUsuarioRegistro" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                    </td>
                    <td style="text-align: left; width: 225px; border-top: 1px solid black; padding-top: 15px;">
                        <label class="txt_gral">
                            Usuario para quien se realiza la solicitud:</label>
                    </td>
                    <td style="text-align: left; width: 300px; border-top: 1px solid black; border-right: 1px solid black;
                        padding-top: 15px; padding-right: 15px;">
                        <asp:TextBox ID="txtUsuarioSolicitud" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="border-left: 1px solid black; padding-left: 15px;">
                        <label class="txt_gral">
                            Extensión*:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtExtension" runat="server" MaxLength="6"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexExtension" ControlToValidate="txtExtension"
                            runat="server" ErrorMessage="*Ingrese solo números" ForeColor="Red" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                        <label class="txt_gral">
                            Funcionario que autoriza*:</label>
                    </td>
                    <td style="border-right: 1px solid black; padding-right: 15px;">
                        <asp:DropDownList ID="ddlFuncionario" runat="server" CssClass="txt_gral" Width="100%">
                        </asp:DropDownList>
                    </td>
                </tr>
                
                <tr id="trCarrusel" runat="server">
                    <td colspan="4" align="center" style="border-left: 1px solid black; border-top: 0px; border-right: 1px solid black; padding-bottom: 15px;">
                        <br />
                        <asp:Panel ID="pnlCarrusel" runat="server" Visible="true">
                        </asp:Panel>
                        <br />
                    </td>
                </tr>
                <tr id="trBusqueda" runat="server">
                    <td style="border-left: 1px solid black; padding-left: 15px;">
                        <label class="txt_gral">
                            Búsqueda de servicios:</label>
                    </td>
                    <td colspan="3" style="border-right: 1px solid black;">
                        <asp:TextBox ID="txtServicios" runat="server" CssClass="txt_gral" Width="810px" onkeyup="ResetScroll()"></asp:TextBox>
                        <asp:AutoCompleteExtender runat="server" TargetControlID="txtServicios" MinimumPrefixLength="4" CompletionListItemCssClass="txt_gral" 
                            ID="acServicios" OnClientItemSelected="ItemSelected"
                            ServicePath="WebService.asmx" ServiceMethod="ObtenerNiveles" CompletionSetCount="20" CompletionListCssClass="lista_autocomplete"
                            EnableCaching="true" UseContextKey="True" CompletionInterval="0" CompletionListHighlightedItemCssClass="itemHighlighted">   
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="border-left: 1px solid black; border-bottom: 1px solid black; border-right: 1px solid black;">
                         <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <div id="servicios" style="width: 100%;">
                            <table style="width: 100%; border-collapse: collapse">
                                <tr>
                                    <td style="width: 45%;">
                                        <label class="txt_gral">
                                            <b>SERVICIOS DISPONIBLES PARA AGREGAR A LA SOLICITUD</b>
                                        </label>
                                    </td>
                                    <td style="width: 10%;">
                                    </td>
                                    <td style="width: 45%;">
                                        <label class="txt_gral">
                                            <b>SERVICIOS AGREGADOS A LA SOLICITUD</b></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <div id="serviciosDisponibles" style="text-align: left; vertical-align: top;">
                                            <asp:Panel ID="pnlApp" runat="server">
                                                <fieldset style="text-align: center; vertical-align: middle;">
                                                    <legend>
                                                        <label class="txt_gral">
                                                            <b>Aplicativo</b>
                                                        </label>
                                                    </legend>
                                                    <br />
                                                    <asp:DropDownList ID="ddlAplicativoDisp" DataValueField="N_ID_APLICATIVO" DataTextField="T_DSC_ACRONIMO_APLICATIVO"
                                                        runat="server" CssClass="txt_gral" AutoPostBack="true" Width="300px">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <br />
                                                </fieldset>
                                            </asp:Panel>
                                            <cc1:CustomGridView ID="gvDisponibles" runat="server" Width="100%" WidthScroll="450"
                                                HeigthScroll="400" SkinID="SeleccionMultipleCliente" HabilitaScroll="true" UnicoEnPantalla="false"
                                                ToolTipHabilitado="false" DataKeyNames="N_ID_NIVELES_SERVICIO">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkElemento" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Clave" DataField="N_ID_NIVELES_SERVICIO" />
                                                    <asp:BoundField HeaderText="Aplicativo" DataField="T_DSC_ACRONIMO_APLICATIVO" />
                                                    <asp:BoundField HeaderText="Servicio" DataField="T_DSC_SERVICIO" />
                                                </Columns>
                                            </cc1:CustomGridView>
                                            <div style="text-align: center;">
                                                <asp:Image ID="imgNoDisp" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="botones">
                                            <asp:Button ID="btnRemueve" runat="server" Text="<" />
                                            <br />
                                            <br />
                                            <asp:Button ID="btnAgrega" runat="server" Text=">" />
                                        </div>
                                        <br />
                                        <br />
                                        <div id="Div1">
                                            <asp:Button ID="btnRemueveTodos" runat="server" Text="<<" />
                                            <br />
                                            <br />
                                            <asp:Button ID="btnAgregaTodos" runat="server" Text=">>" />
                                        </div>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <div id="serviciosAgregados" style="text-align: left; vertical-align: top;">
                                            <cc1:CustomGridView ID="gvAgregados" runat="server" Width="100%" WidthScroll="450"
                                                HeigthScroll="400" SkinID="SeleccionMultipleCliente" HabilitaScroll="true" UnicoEnPantalla="false"
                                                ToolTipHabilitado="false">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkElemento" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Clave" DataField="N_ID_NIVELES_SERVICIO" />
                                                    <asp:BoundField HeaderText="Aplicativo" DataField="T_DSC_ACRONIMO_APLICATIVO" />
                                                    <asp:BoundField HeaderText="Servicio" DataField="T_DSC_SERVICIO" />
                                                </Columns>
                                            </cc1:CustomGridView>
                                            <div style="text-align: center;">
                                                <asp:Image ID="imgNoAgr" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="notas">
                            <br />
                            <p style="text-align: left; padding: 0px; margin: 0px;">
                                <label class="txt_gral">
                                    Notas complementarias:</label>
                            </p>
                            <p style="padding: 0px; margin: 0px; text-align: left; vertical-align: top; text-align: left;">
                                <asp:TextBox ID="txtNota" runat="server" ClientIDMode="Static" Height="100px" onkeyup="ContarCaracteres($('#hfLimiteCharsCuerpo').val(),'txtNota','lblCharsCuerpo')"
                                    Style="text-align: left" TextMode="MultiLine" Width="1000px" cols="5000000" MaxLength="3000"
                                    CssClass="txt_gral" Columns="50000" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" 
                                   onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                            </p>
                            <p style="padding: 0px; margin: 0px; text-align: right; width: 100%;">
                                <asp:Label ID="lblCharsCuerpo" runat="server" ClientIDMode="Static" CssClass="txt_gral"></asp:Label>
                            </p>
                            <p style="padding: 0px; margin: 0px; text-align: right; width: 100%;">
                                <asp:RegularExpressionValidator ID="regExTamañoCuerpo" runat="server" ControlToValidate="txtNota"
                                    CssClass="txt_gral" Display="Dynamic" ErrorMessage="El campo Nota no puede medir mas de 2000 caracteres"
                                    Style="color: #FF0000" ValidationExpression="^[\s\S]{1,2000}$" ValidationGroup="Main"> Error: Tamaño máximo 2000 caracteres</asp:RegularExpressionValidator>
                            </p>
                            <br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label class="txt_gral">
                            Adjuntar archivo complementario:</label>
                    </td>
                    <td>
                        <asp:FileUpload ID="fupDocSolicitud" runat="server" CssClass="txt_gral" Width="100%" />
                    </td>
                    <td>
                        <label class="txt_gral">
                            Servicio asociado al archivo:</label>
                    </td>
                    <td style="text-align: right;">
                        <table style="width: 100%; border-collapse: collapse">
                            <tr>
                                <td style="width: 80%; text-align: left;">
                                    <asp:DropDownList ID="ddlServicios" runat="server" CssClass="txt_gral" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%; text-align: right">
                                    <asp:Button ID="btnAgregarDoc" runat="server" Text="Agregar" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView runat="server" ID="gvConsultaSolicitudes" DataKeyNames="NomDoc" Width="100%"
                            Style="margin-top: 0px">
                            <Columns>
                                <asp:BoundField HeaderText="Servicio asociado al archivo" DataField="DescServ" SortExpression="DescServ">
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Nombre del archivo" DataField="NomDoc" SortExpression="NomDoc">
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Acción">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbDescargar" runat="server" Height="20px" CausesValidation="False"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Descargar"
                                            ImageUrl="~/Imagenes/download.png" ToolTip="Descargar" />
                                        <asp:ImageButton ID="imbBorrar" runat="server" Height="20px" CausesValidation="False"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Borrar"
                                            ImageUrl="~/Imagenes/icono_corregir.jpg" ToolTip="Borrar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                    <HeaderStyle Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <p style="text-align: center; width: 100%;">
                            <asp:Image ID="imgNoArch" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                        </p>
                    </td>
                </tr>
            </table>
            <p style="text-align: center; width: 800px;">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" Width="80px" />
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="btnEnviar" Text="Guardar y Enviar" Width="120px" />
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="btnCancelar" CausesValidation="false" Text="Cancelar"
                    Width="80px" />
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="Limpiar" Text="Limpiar" CausesValidation="false" />
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="Button1" Text="PDF (BORRAR PRUEBA)" Visible="false" />
            </p>
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="../Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="txt_gral" />
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
            <asp:Button runat="server" ID="btnSalirM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnContinuarM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
            <asp:HiddenField ID="hfLimiteCharsCuerpo" runat="Server" ClientIDMode="Static" Value="2000" />
            <asp:HiddenField runat="server" ID="hfTextComboBusqueda" ClientIDMode="Static" Value="" />
            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();
                    MensajeDosBotonesUnaAccionLoad();
                    ContarCaracteres($("#hfLimiteCharsCuerpo").val(), "txtNota", "lblCharsCuerpo");

                });

                function ContarCaracteres(maxchar, txtId, lblId) {

                    if ($("#" + txtId).length && $("#" + lblId).length) {

                        var x = $("#" + txtId).val();
                        x = x.replace(/\u000d\u000a/g, '\u000a').replace(/\u000a/g, '\u000d\u000a');

                        var remaningChar = maxchar - x.length;
                        $("#" + lblId).html("Caracteres restantes: " + remaningChar);
                        return true;
                    }
                }

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
                
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnGuardar" />
            <asp:PostBackTrigger ControlID="btnAgregarDoc" />
            <asp:PostBackTrigger ControlID="gvConsultaSolicitudes" />
            <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
