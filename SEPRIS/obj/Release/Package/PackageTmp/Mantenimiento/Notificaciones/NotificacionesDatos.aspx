<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="NotificacionesDatos.aspx.vb" Inherits="SEPRIS.NotificacionesDatos"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="ucFiltro" %>
<%@ Register Src="/Controles/wucNotificaciones.ascx" TagName="ucNotificacionesPrueba"
    TagPrefix="ucNotificacionesPrueba" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hidSourceID" runat="server" />

    <table width="800px" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" class="TitulosWebProyectos">
                <asp:Label runat="server" ID="lblTitulo" Text="Alta de Mensaje" EnableTheming="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="txt_gral" style="width: 15%; text-align: left">
                * Nombre Mensaje
            </td>
            <td style="width: 85%">
                <asp:TextBox runat="server" ID="txtNombre" Width="95%" CssClass="txt_gral" MaxLength="250"
                    ValidationGroup="Notificaciones"></asp:TextBox>
                <asp:CustomValidator ID="cvNombre" ControlToValidate="txtNombre" ValidationGroup="Notificaciones"
                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server" ID="upMensaje">
        <ContentTemplate>
            <table width="800px" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="txt_gral" style="width: 15%; text-align: left">
                        * Mensaje
                    </td>
                    <td style="width: 85%">
                        <asp:TextBox runat="server" ID="txtMensaje" CssClass="txt_gral" Width="95%" TextMode="MultiLine"
                            Height="250px" ValidationGroup="Notificaciones" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" 
                           onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                        <asp:CustomValidator ID="cvMensaje" ControlToValidate="txtMensaje" ValidationGroup="Notificaciones"
                            ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnTitulo" Text="Titulo" Width="55px" ToolTip="[t][/t]"
                            ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnLetra1" Text="Letra 1" Width="55px" ToolTip="[l1][/l1]"
                            ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnLetra2" Text="Letra 2" Width="55px" ToolTip="[l2][/l2]"
                            ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnLetra3" Text="Letra 3" Width="55px" ToolTip="[l3][/l3]"
                            ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnVinculo1" Text="V&iacute;nculo 1" Width="70px"
                            ToolTip="[h=www.algo.com][/h]" ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnVinculo2" Text="V&iacute;nculo2" Width="70px" ToolTip="[h][/h]"
                            ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnRecuadro" Text="Recuadro" Width="60px" ToolTip="[re][/re]"
                            ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnArchivoAdjunto" Text="Archivo Adjunto" Width="100px"
                            ToolTip="[d][/d]" ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />&nbsp;
                        <asp:Button runat="server" ID="btnVistaPrevia" Text="Vista Previa" Width="90px" ClientIDMode="Static" 
                            OnClientClick = "SetSource(this.id)" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnVistaPrevia" />
        </Triggers>
    </asp:UpdatePanel>
    <table width="800px" cellpadding="0" cellspacing="0">
        <tr>
            <td class="txt_gral" style="width: 15%; text-align: left">
                Archivo Adjunto
            </td>
            <td style="width: 85%">
                <asp:FileUpload runat="server" ID="fulArchivoAdjunto" CssClass="txt_gral" Style="width: 95%" />
                <br />
                <asp:Button ID="btnArchivo" runat="server" SkinID="botonArchivo" Visible="False"
                    Width="447px" ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />
                <asp:Button ID="btnReemplazaArchivo" runat="server" CssClass="botones" Text="Reemplazar Archivo"
                    Visible="False" Width="110px" ClientIDMode="Static" OnClientClick = "SetSource(this.id)" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel runat="server" ID="upNotificacionesMensaje">
                    <ContentTemplate>
                        <table width="800px" runat="server" id="tblUsuarios">
                            <tr>
                                <td colspan="3" align="right">
                                    <ucFiltro:ucFiltro runat="server" ID="Filtros" Width="800px" />
                                </td>
                            </tr>
                            <tr>
                                <td runat="server" id="celdaAsignados" style="width: 370px; text-align: center; vertical-align: top;">
                                    <cc1:CustomGridView ID="grvAsignados" runat="server" Width="360px" SkinID="SeleccionMultiple">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkSeleccion" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Usuario" HeaderText="Clave" />
                                            <asp:BoundField DataField="Nombre" HeaderText="Usuario" />
                                        </Columns>
                                    </cc1:CustomGridView>
                                </td>
                                <td runat="server" align="center" id="celdaBotones" style="width: 60px; text-align: center;
                                    vertical-align: middle;">
                                    <asp:Button runat="server" ID="btnDesasignaUsuario" Text="&gt;&gt;" Width="58px"
                                        Height="21px" ClientIDMode="Static" OnClientClick = "SetSource(this.id)"  />
                                    <br />
                                    <br />
                                    <asp:Button runat="server" ID="btnAsignaUsuario" Text="&lt;&lt;" Width="58px" Height="21px" 
                                        ClientIDMode="Static" OnClientClick = "SetSource(this.id)"  />
                                </td>
                                <td runat="server" id="celdaNoAsignados" style="width: 370px; text-align: center;
                                    vertical-align: top;">
                                    <cc1:CustomGridView ID="grvNoAsignados" runat="server" Width="360px" SkinID="SeleccionMultiple" ToolTipHabilitado="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkSeleccionNoAsignado" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IdentificadorUsuario" HeaderText="Clave" />
                                            <asp:BoundField DataField="Nombre" HeaderText="Usuario" />
                                        </Columns>
                                    </cc1:CustomGridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtFechaInicial" />
                        <asp:AsyncPostBackTrigger ControlID="txtFechaFinal" />
                        
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" OnClientClick="Deshabilita(this); SetSource(this.id);" 
                    ClientIDMode="Static" />
                &nbsp;
                <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CausesValidation="false" 
                    ClientIDMode="Static" OnClientClick = "SetSource(this.id)"  />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" /> 
    <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" /> 
    <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
    <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <asp:Label runat="server" ID="lblError" Text="" CssClass="MensajeModal-UI" EnableTheming="false"></asp:Label>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Notificaciones"
                                    CssClass="MensajeModal-UI" EnableTheming="false"/>
                           </div>
                        </td>
                    </tr>
                </table>

    </div>
    <div id="divMensajeUnBotonUnaAccion" style="display: none" clientidmode="Static">
        <p>
            <asp:Label runat="server" ID="lblConfirmaSalida" Text=""></asp:Label>
        </p>
        <table align="center" border="0" cellpadding="3" cellspacing="7" style="left: 33px"
            width="100%" runat="server" id="TablaFechas" class="txt_gral">
            <tr>
                <td class="style19">
                    <asp:Label ID="lblFechaInicial" runat="server" CssClass="txt_gral" Text="Fecha de Inicio: "></asp:Label>
                </td>
                <td align="left" class="style145">
                    *
                </td>
                <td style="width: 450px" align="left">
                    <asp:TextBox ID="txtFechaInicial" runat="server" Height="15px" MaxLength="12" Width="135px"
                        CssClass="txt_gral" ClientIDMode="Static"></asp:TextBox>
                    <cc2:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="imgFechaInicial" TargetControlID="txtFechaInicial"
                        ClientIDMode="Static">
                    </cc2:CalendarExtender>
                    <asp:Image ID="imgFechaInicial" runat="server" ImageUrl="/Imagenes/Calendar.GIF"
                        src="/Imagenes/Calendar.GIF" Style="cursor: hand" ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td class="style20">
                    <asp:Label ID="lblFechaFinal" runat="server" CssClass="txt_gral" Text="Fecha Fin: "></asp:Label>
                    &nbsp;
                </td>
                <td align="left" class="style18">
                    *
                </td>
                <td style="width: 277px; text-align: left;" align="right" height="30">
                    <asp:TextBox ID="txtFechaFinal" runat="server" Height="15px" MaxLength="12" Width="135px"
                        CssClass="txt_gral" ClientIDMode="Static"></asp:TextBox>
                    <cc2:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" Enabled="True"
                        Format="dd/MM/yyyy" PopupButtonID="imgFechaFinal" TargetControlID="txtFechaFinal"
                        ClientIDMode="Static">
                    </cc2:CalendarExtender>
                    <asp:Image ID="imgFechaFinal" runat="server" ImageUrl="/Imagenes/Calendar.GIF" src="/Imagenes/Calendar.GIF"
                        Style="cursor: hand" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" OnClientClick = "SetSource(this.id)"  />
    <ucNotificacionesPrueba:ucNotificacionesPrueba runat="server" ID="ucNotificaciones"
        EsVistaPrevia="true" />
    <script type="text/javascript">

        $(function () {

            MensajeUnBotonNoAccionLoad();
            MensajeUnBotonUnaAccionLoad();


        });

        function MostrarMensaje() {

            MensajeUnBotonNoAccion();

        }

        function MostrarMensajeAccion() {

            MensajeUnBotonUnaAccion();

        }

        function SetSource(SourceID) {
            var hidSourceID = document.getElementById("<%=hidSourceID.ClientID%>");
            hidSourceID.value = SourceID;
        }


    </script>
</asp:Content>
