<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="RegistrarAgenda.aspx.vb" Inherits="SEPRIS.RegistrarAgenda" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .NombreControlTD
        {
            text-align: right;
            width: 30%;
            vertical-align: top;
        }
        .ControlesTD
        {
            text-align: left;
            /*width: 20%;*/
            vertical-align: top;
        }
        .EspacioTD
        {
            text-align: left;
            width: 70%;
            vertical-align: top;
        }
        .Botones_Padding
        {
            padding-right: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />
    <asp:UpdatePanel ID="udpSolicitudes" runat="server">
        <ContentTemplate>            
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label id="lblTitulo" runat="server" class="TitulosWebProyectos">
                    Registrar en Agenda
                </label>
            </div>
            <table style="text-align: left; width: 1000px;">
                <tbody>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Tipo*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:RadioButtonList ID="rblTipo" runat="server" AutoPostBack="true" class="txt_gral"
                                RepeatDirection="Horizontal" style="width:100%">
                                <asp:ListItem Text="Actividad" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Ausencia" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CustomValidator ID="csvTipo" runat="server" ControlToValidate="rblTipo" Display="Dynamic"
                                EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbActividad" runat="server">
                    <tr id="trTipoActividad">
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Tipo Actividad*:
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlTipoActividad" runat="server" AutoPostBack="true" class="txt_gral"
                                Width="400px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="csvTipoActividad" runat="server" ControlToValidate="ddlTipoActividad"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbAusencia" runat="server" visible="false">
                    <tr id="trTipoAusencia">
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Tipo Ausencia*:
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlTipoAusencia" runat="server" AutoPostBack="true" class="txt_gral"
                                Width="400px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="csvTipoAusencia" runat="server" ControlToValidate="ddlTipoAusencia"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbAutoriza" runat="server" visible="false">
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Autoriza*:
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlFuncionario" runat="server" CssClass="txt_gral" Width="400px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="csvFuncionario" runat="server" ControlToValidate="ddlFuncionario"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbVacaciones" runat="server" visible="false">
                    <tr>
                        <td class="NombreControlTD">
                        </td>
                        <td colspan="2" style="border: 1px solid black">
                            <asp:Label ID="lblNoHayVacaciones" runat="server" Visible="false">El ingeniero no cuenta con periodos vacacionales</asp:Label>
                            <div id="TieneVacaciones" runat="server" style="width: 100%">
                                <label class="txt_gral" style="float: right;">
                                    Vacaciones
                                </label>
                                <div id="PeriodoAnterior" runat="server" style="clear: both">
                                    <label id="lblPeriodoAnterior" runat="server" class="txt_gral">
                                        Periodo de #INICIO# a #FIN#
                                    </label>
                                    <br />
                                    <label id="lblRestantesAnterior" runat="server" class="txt_gral">
                                        Restantes: #DIAS#
                                    </label>
                                </div>
                                <div id="PeriodoActual" runat="server" style="clear: both">
                                    <label id="lblPeriodoActual" runat="server" class="txt_gral">
                                        Periodo de #INICIO# a #FIN#
                                    </label>
                                    <br />
                                    <label id="lblRestanteActual" runat="server" class="txt_gral">
                                        Restantes: #DIAS#
                                    </label>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
                <tbody>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Cíclica:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:CheckBox ID="chkCiclica" runat="server" AutoPostBack="true" class="txt_gral" />
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbTiempoContinuo" runat="server">
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Fecha de inicio*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:TextBox ID="txtFechaInicioContinuo" class="txt_gral" runat="server" Width="150px" MaxLength="10"></asp:TextBox>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CalendarExtender ID="txtFechaInicioContinuo_CalendarExtender" runat="server"
                                TargetControlID="txtFechaInicioContinuo" Enabled="True" PopupButtonID="imgFec1"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:CustomValidator ID="csvFechaInicioContinuo" runat="server" ControlToValidate="txtFechaInicioContinuo"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                            <asp:Image ID="imgFec1" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                Style="cursor: hand" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" />
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Hora de inicio*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:DropDownList ID="ddlHoraInicioContinuo" runat="server" class="txt_gral" Width="100%">
                            </asp:DropDownList>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CustomValidator ID="csvHoraInicioContinuo" runat="server" ControlToValidate="ddlHoraInicioContinuo"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Fecha de fin*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:TextBox ID="txtFechaFinContinuo" class="txt_gral" runat="server" Width="150px"
                                MaxLength="10"></asp:TextBox>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CalendarExtender ID="txtFechaFinContinuo_CalendarExtender" runat="server" TargetControlID="txtFechaFinContinuo"
                                Enabled="True" PopupButtonID="imgFec2" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:CustomValidator ID="csvFechaFinContinuo" runat="server" ControlToValidate="txtFechaInicioContinuo"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                            <asp:Image ID="imgFec2" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                Style="cursor: hand" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" />
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Hora de fin*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:DropDownList ID="ddlHoraFinContinuo" runat="server" class="txt_gral" Width="100%">
                            </asp:DropDownList>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CustomValidator ID="csvHoraFinContinuo" runat="server" ControlToValidate="ddlHoraFinContinuo"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbTiempoCiclico" runat="server" visible="false">
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Fecha de inicio*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:TextBox ID="txtFechaInicioCiclico" class="txt_gral" runat="server" Width="150px"
                                MaxLength="10"></asp:TextBox>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CalendarExtender ID="txtFechaInicioCiclico_CalendarExtender" runat="server"
                                TargetControlID="txtFechaInicioCiclico" Enabled="True" PopupButtonID="imgFec3"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:CustomValidator ID="csvFechaInicioCiclico" runat="server" ControlToValidate="txtFechaInicioCiclico"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                            <asp:Image ID="imgFec3" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                Style="cursor: hand" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" />
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Fecha de fin*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:TextBox ID="txtFechaFinCiclico" class="txt_gral" runat="server" Width="150px"
                                MaxLength="10"></asp:TextBox>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CalendarExtender ID="txtFechaFinCiclico_CalendarExtender" runat="server" TargetControlID="txtFechaFinCiclico"
                                Enabled="True" PopupButtonID="imgFec4" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:CustomValidator ID="csvFechaFinCiclico" runat="server" ControlToValidate="txtFechaFinCiclico"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                            <asp:Image ID="imgFec4" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                Style="cursor: hand" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" />
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Hora de inicio*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:DropDownList ID="ddlHoraInicioCiclico" runat="server" class="txt_gral" Width="100%">
                            </asp:DropDownList>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CustomValidator ID="csvHoraInicioCiclico" runat="server" ControlToValidate="ddlHoraInicioCiclico"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Hora de fin*:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:DropDownList ID="ddlHoraFinCiclico" runat="server" class="txt_gral" Width="100%">
                            </asp:DropDownList>                            
                        </td>
                        <td class="EspacioTD">
                            <asp:CustomValidator ID="csvHoraFinCiclico" runat="server" ControlToValidate="ddlHoraFinCiclico"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Lunes:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:CheckBox ID="chkLunes" runat="server" />
                        </td>
                        <td class="EspacioTD"></td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Martes:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:CheckBox ID="chkMartes" runat="server" />
                        </td>
                        <td class="EspacioTD"></td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Miércoles:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:CheckBox ID="chkMiercoles" runat="server" />
                        </td>
                        <td class="EspacioTD"></td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Jueves:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:CheckBox ID="chkJueves" runat="server" />
                        </td>
                        <td class="EspacioTD"></td>
                    </tr>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Viernes:
                            </label>
                        </td>
                        <td class="ControlesTD">
                            <asp:CheckBox ID="chkViernes" runat="server" />
                        </td>
                        <td class="EspacioTD"></td>
                    </tr>
                </tbody>
                <tbody>
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Descripción Tarea*:
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescripcionActividad" runat="server" class="txt_gral" Width="95%"
                                TextMode="MultiLine" Height="100px" onkeyup="validaLimite(this, 1000, 'lblRestanteDescripcionActividad')"
                                onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                            <asp:CustomValidator ID="csvDescripcionActividad" runat="server" ControlToValidate="txtDescripcionActividad"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                            <div style="float: right; padding-right: 30px;">
                                <label class="txt_gral">
                                    Máx 1000 caracteres, restante
                                </label>
                                <label id="lblRestanteDescripcionActividad" clientidmode="Static" runat="server"
                                    class="txt_gral">
                                    1000
                                </label>
                            </div>
                        </td>
                    </tr>
                </tbody>
                <tbody id="tbNotasAutorizador" runat="server" visible="false">
                    <tr>
                        <td class="NombreControlTD">
                            <label class="txt_gral">
                                Notas Autorizador*:
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNotasAutorizador" runat="server" class="txt_gral" Width="95%"
                                TextMode="MultiLine" Height="100px" onkeyup="validaLimite(this, 1000, 'lblRestanteNotasAutorizador')"  onkeypress="ReemplazaCEspeciales(this.id)" 
                               onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                            <asp:CustomValidator ID="csvNotasAutorizador" runat="server" ControlToValidate="txtNotasAutorizador"
                                Display="Dynamic" EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true"
                                ValidationGroup="Forma">*</asp:CustomValidator>
                            <div style="float: right; padding-right: 30px;">
                                <label class="txt_gral">
                                    Máx 1000 caracteres, restante
                                </label>
                                <label id="lblRestanteNotasAutorizador" runat="server" clientidmode="Static" class="txt_gral">
                                    1000
                                </label>
                            </div>
                        </td>
                    </tr>
                </tbody>
                <tbody>
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="botones Botones_Padding"
                                OnClientClick="Deshabilita(this);" />
                            <asp:Button ID="btnAutorizar" runat="server" Text="Autorizar" CssClass="botones Botones_Padding"
                                OnClientClick="Deshabilita(this);" Visible="false" />
                            <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" CssClass="botones Botones_Padding"
                                OnClientClick="Deshabilita(this);" Visible="false" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botones Botones_Padding"
                                OnClientClick="Deshabilita(this);" />
                        </td>
                    </tr>
                </tbody>
            </table>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(function () {

            MensajeUnBotonNoAccionLoad();
            MensajeDosBotonesDosAccionesLoad();
            MensajeUnBotonUnaAccionLoad();
            MensajeDosBotonesUnaAccionLoad();
        });


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

        function Deshabilita(btn) {
            btn.disabled = true;
            btn.value = 'Procesando...';
            __doPostBack(btn.name, "");
        }

    </script>
</asp:Content>
