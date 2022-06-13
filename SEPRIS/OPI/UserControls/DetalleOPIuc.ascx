<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DetalleOPIuc.ascx.vb" Inherits="SEPRIS.DetalleOPIuc" %>
<%@ Register Src="~/OPI/UserControls/ComentariosOPI.ascx" TagPrefix="uc1" TagName="ComentariosOPI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/OPI/UserControls/SupervisorOPI.ascx" TagPrefix="uc1" TagName="SupervisorOPI" %>
<%@ Register Src="~/OPI/UserControls/InspectoresOPI.ascx" TagPrefix="uc1" TagName="InspectoresOPI" %>


<asp:UpdatePanel runat="server" ID="upTabla">
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="imgRegistrarVisita" />--%>
        <%--<asp:AsyncPostBackTrigger ControlID="GridPrincipal" />--%>
    </Triggers>

    <ContentTemplate>
        <!-- Tabla para capturar los datos -->
        <table id="tbDatosCaptura" runat="server" style="width: 95%; margin-top: 20px;">
            <tr>
                <td style="height: 35px; width: 15%; text-align: left;">
                    <asp:Label runat="server" ID="lblTipoEntidad"
                        CssClass="txt_gral" Text="Tipo Entidad :"></asp:Label>
                </td>
                <td style="height: 35px; width: 85%; text-align: left;" colspan="2">
                    <asp:DropDownList ID="ddlTipoEntidad" runat="server" Width="50%" CssClass="txt_gral" Visible="false"></asp:DropDownList>
                    <asp:TextBox ID="TxtddlTipoEntidad" runat="server" Width="50%" value="asdasds" ReadOnly="true" disabled="disabled" Visible="true"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" clientidmode="Static">
                <td style="height: 35px; width: 15%; text-align: left;">
                    <asp:Label runat="server" ID="lblEntidad"
                        CssClass="txt_gral" Text="Entidad :"></asp:Label>
                </td>
                <td style="height: 35px; width: 85%; text-align: left;">
                    <asp:DropDownList ID="dplEntidad" runat="server" Width="50%" CssClass="txt_gral" Visible="false"></asp:DropDownList>
                    <asp:TextBox ID="TxtdplEntidad" runat="server" Width="50%" value="asdasds" ReadOnly="true" disabled="disabled" Visible="true"></asp:TextBox>
                </td>
            </tr>

            <tr id="trSubentidad1" runat="server" clientidmode="Static" visible="false">
                <td style="height: 35px; width: 25%; text-align: left;">
                    <asp:Label runat="server" ID="lblSE1"
                        CssClass="txt_gral" Text="Sub Entidad :"></asp:Label>
                </td>
                <td style="height: 35px; width: 40%; text-align: left;">
                    <asp:TextBox ID="txtSubEntidad" runat="server" Width="50%" value="" ReadOnly="true" disabled="disabled" Visible="true"></asp:TextBox>

                </td>
                <td style="text-align: left;"></td>
            </tr>


            <tr id="trSubentidad" runat="server" clientidmode="Static" visible="false">
                <td style="height: 35px; width: 15%; text-align: left;">
                    <asp:Label runat="server" ID="lblSE"
                        CssClass="txt_gral" Text="Sub Entidad :"></asp:Label>
                </td>
                <td style="text-align: left; width: 85%">
                    <asp:CheckBoxList ID="chkSubEntidad" ClientIDMode="Static" Visible="true"
                        CssClass="txt_gral" runat="server">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr runat="server" id="trFecha">
                <td style="height: 35px; width: 30%; text-align: left;">
                    Fecha de posible irregularidad :</td>
                <td style="height: 35px; width: 70%; text-align: left;" colspan="2">
                    <asp:TextBox ID="txtFechaPI" runat="server" Width="150px"></asp:TextBox>
                    <ajx:CalendarExtender ID="calFechaPI" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFechaPI"
                        TargetControlID="txtFechaPI" CssClass="teamCalendar" />
                    <asp:Image ID="imgFechaPI" runat="server" ImageUrl="../../Imagenes/Calendar.GIF" Width="16px"
                        ImageAlign="Bottom" Height="16px" />
                </td>
            </tr>

            <tr runat="server" id="trProceso">
                <td style="height: 35px; width: 30%; text-align: left;">
                    Proceso :</td>
                <td style="height: 35px; width: 70%; text-align: left;" colspan="2">
                    <asp:DropDownList ID="dplProcesoPO" runat="server" AutoPostBack="True" Width="50%" OnSelectedIndexChanged="dplProcesoPO_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="TxtdplProcesoPO" runat="server" Width="50%" value="asdasds" ReadOnly="true" disabled="disabled" Visible="true"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="trSunproceso">
                <td style="height: 35px; width: 30%; text-align: left;">
                    Subproceso :</td>
                <td style="height: 35px; width: 70%; text-align: left;" colspan="2">
                    <asp:DropDownList ID="ddlSubproceso" runat="server" AutoPostBack="True" Width="50%" OnSelectedIndexChanged="ddlSubproceso_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="TxtddlSubproceso" runat="server" Width="50%" value="asdasds" ReadOnly="true" disabled="disabled" Visible="true"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" clientidmode="Static">
                <td style="text-align: left;" class="auto-style1">
                    Descripción :</td>
                <td style="text-align: left;" colspan="2" class="auto-style2" rowspan="2">
                    <asp:TextBox ID="txtDescOPI" runat="server" onkeypress="ReemplazaCEspeciales(this.id)"
                        onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"
                        onkeyup="validaLimiteLongitud(this,300,'lblConttxtObjetoOPI')"
                        TextMode="MultiLine" Width="100%" MaxLength="300" Height="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
            </tr>
            <tr id="OtroObjVisitaDsc" runat="server" clientidmode="Static">
                <td style="width: 25%; text-align: left;">&nbsp;</td>
                <td style="width: 75%; text-align: right;" colspan="3">
                    <div id="divConttxtObjetoOPI" runat="server" style="width: 100%; text-align: right; float: left;">
                        <asp:Label runat="server" ID="lbltxtObjetoOPI" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                        <asp:Label runat="server" ID="lblConttxtObjetoOPI" CssClass="txt_gral" Text="500" ClientIDMode="Static"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="trSupervisores" runat="server" clientidmode="Static">
                <td style="width: 100%;" colspan="4">
                    <%--<div id="ast12" runat="server" class="AsteriscoShow">*</div>--%>
                    <uc1:SupervisorOPI runat="server" ID="SupervisorOPI" />
                </td>
            </tr>
            <tr id="trInspectores" runat="server" clientidmode="Static">
                <td style="width: 100%;" colspan="4">
                    <uc1:InspectoresOPI runat="server" ID="InspectoresOPI" />
                    <%--<div id="ast14" runat="server" class="AsteriscoHide">*</div>--%>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <br />
                    <br />
                    <br />
                </td>
            </tr>

            <tr>
                <td class="auto-style4"></td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
            </tr>
            <tr runat="server" id="trViewListas">
                <td style="width: 95%" colspan="2">
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: start; width: 12%">
                                <asp:Label runat="server" ID="lblSupervisor"
                                    CssClass="txt_gral" Text="Supervisor(es) :"></asp:Label>
                            </td>
                            <td rowspan="2" style="vertical-align: middle; width: 28%">
                                <asp:ListBox ID="lstSupervisores" runat="server" Width="100%" Enabled="false"
                                    CssClass="txt_gral" Height="110px" disabled="disabled"></asp:ListBox>
                            </td>
                            <td style="text-align: left; width: 12%">
                                <asp:Label runat="server" ID="lblInspector"
                                    CssClass="txt_gral" Text="Inspector(es) :"></asp:Label>
                            </td>
                            <td rowspan="2" style="vertical-align: bottom; width: 28%">
                                <asp:ListBox ID="lstInspectores" runat="server" Width="100%" Enabled="false"
                                    CssClass="txt_gral" Height="110px" disabled="disabled"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
            </tr>
            <tr>
                <td class="auto-style4" style="width: 95%" colspan="2">
                    <%--<uc1:ComentariosOPI runat="server" ID="ComentariosOPI" />--%>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
