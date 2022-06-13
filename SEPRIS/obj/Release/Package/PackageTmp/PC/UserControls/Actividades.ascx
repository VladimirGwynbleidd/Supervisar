<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Actividades.ascx.vb" Inherits="SEPRIS.Actividades" %>

<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<%@ Register Src="~/Controles/ucFiltro3.ascx" TagName="ucFiltro3" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<link href="../Styles/Site.css" rel="stylesheet" />

<wcu:wucAyuda ID="Ayuda" runat="server" />
<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <div style="text-align: left; width: 100%; padding-bottom: 5px;">
            <asp:Button ID="btnExportaExcel" visible="false" runat="server" Text="Exportar a Excel" />
            &nbsp;
                <asp:Button ID="btnPersonalizarColumnas"  visible="false" runat="server" Text="Personalizar Columnas"
                    Style="width: 120px;" />
        </div>
        <uc1:ucFiltro3 ID="ucFiltro3" runat="server" Width="100%"  ClientIDMode="Static"  />
        <br />
        <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" DataKeyNames="I_ID_ACTIVIDAD,Row#"
            Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValue"
            HeigthScroll="0" ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0" SelectedMultiRows="false"
            AllowPaging="true" PageSize="15">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkElemento" runat="server" AutoPostBack="false" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="Consecutivo" DataField="Row#" Visible="false" SortExpression="Row#">
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="IDActividad" DataField="I_ID_ACTIVIDAD" SortExpression="I_ID_ACTIVIDAD" Visible="false">
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Actividad a entregar" DataField="T_DSC_ACTIVIDAD" SortExpression="T_DSC_ACTIVIDAD">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fecha Entrega Inicial" DataField="F_FECH_ENTREGA" SortExpression="F_FECH_ENTREGA">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fecha Entrega Actual" DataField="F_FECH_ENTREGA_PRO" SortExpression="F_FECH_ENTREGA_PRO">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Usuario Registró" DataField="T_ID_USUARIO" SortExpression="T_ID_USUARIO">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Estatus" DataField="I_ID_ESTATUS" SortExpression="I_ID_ESTATUS" Visible="false"></asp:BoundField>
                <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(Eval("I_ID_ESTATUS"))%>' Width="15px" Height="15px" />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Usuario Modificó" DataField="T_ID_USUARIO_MOD" SortExpression="T_ID_USUARIO_MOD">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fecha Modificación" DataField="F_FECH_COMENTARIO" SortExpression="F_FECH_COMENTARIO">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
            </Columns>
        </cc1:CustomGridView>
        <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
            alt="No existen Registos para la Consulta" />
        <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfIdActividad" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfTipoVentana" runat="server" ClientIDMode="Static" Value="-1" />
        <br />
        <br />
        <div>
            <table width="450px" border="0" style="align-content: center">
                <tr>
                    <td width="25%" style="align-content: center">
                        <asp:Image ID="imgCompleta" runat="server" Width="15px" Height="15px" ImageUrl="~/Imagenes/aceptado.png" />
                        <label class="txt_gral">
                            &nbsp;Completa</label>
                    </td>
                    <td width="25%" style="align-content: center">
                        <asp:Image ID="imgIncompleta" runat="server" Width="15px" Height="15px" ImageUrl="~/Imagenes/ERROR.jpg" />
                        <label class="txt_gral">
                            &nbsp;Incompleta</label>
                    </td>
                    <td width="25%" style="align-content: center">
                        <asp:Image ID="imgEnproceso" runat="server" Width="15px" Height="15px" ImageUrl="~/Images/ORIENTACION.png" />
                        <label class="txt_gral">
                            &nbsp;En proceso</label>
                    </td>
                    <td width="25%" style="align-content: center">
                        <asp:Image ID="imgProrrogada" runat="server" Width="15px" Height="15px" ImageUrl="~/Images/PREVENTIVO.jpg" />
                        <label class="txt_gral">
                            &nbsp;Prórrogada</label>
                    </td>
                </tr>
            </table>
        </div>
        <br />

        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClientClick="AltaActividad(); return false;" />
        &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnModificar2" runat="server" Text="Modificar" OnClientClick="CargaDatosActividades();return false;" />
        &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnProrrogar" runat="server" Text="Prórrogar" OnClientClick="CargaDatosProrroga(); return false;" />
        &nbsp;&nbsp;&nbsp;
            
        <div id="divMensajeDosBotonesUnaAccion" style="display: none">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                            ImageUrl="~/Imagenes/Errores/Error1.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <div id="divAltaActividades" style="display: none">
                                <table>
                                    <tr>
                                        <td colspan="4">Alta de actividades</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td>
                                            <asp:Label ID="lblDescActividad" runat="server" Text="Descripción de la actividad:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtDescActividad" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                                                TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td>
                                            <asp:Label ID="LblFecEstEntrega" runat="server" Text="Fecha estimada de entrega:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtFecEstimEntrega" runat="server" Width="150px"></asp:TextBox>
                                            <ajx:CalendarExtender ID="calEx_1" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalFechaRecepcion"
                                                TargetControlID="txtFecEstimEntrega" CssClass="teamCalendar" />
                                            <asp:Image ImageAlign="Bottom" ID="imgCalFechaRecepcion" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>

                            <div id="divModificarActividades" style="display: none">
                                <table>
                                    <tr>
                                        <td colspan="4">Modificar actividades</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblNumActividad" runat="server" Text="Número de actividad:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtNumActividad" runat="server" Width="50px" disabled="true"></asp:TextBox>
                                            <asp:TextBox ID="TxtNumActividadReal" runat="server" Width="15px" Visible="false"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblDescActividadMod" runat="server" Text="Descripción de la actividad:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtDescActividadMod" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                                                TextMode="MultiLine" MaxLength="600" onkeyup="validaLimite(this,100)" disabled="true"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblFecEstimEntregMod" runat="server" Text="Fecha estimada de entrega:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtFecEstimEntregMod" runat="server" Width="150px" disabled="true"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td>
                                            <asp:Label ID="LblEstatusMod" runat="server" Text="Estatus" CssClass="txt_gral"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTEstatusMod" runat="server" Height="20px" Width="255px" AutoPostBack="false"
                                                CssClass="txt_gral">
                                                <asp:ListItem Value="Completa">Completa</asp:ListItem>
                                                <asp:ListItem Value="Incompleta">Incompleta</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblComentariosMod" runat="server" Text="Comentarios del cambio:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtComentariosMod" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                                                TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>

                            <div id="divProrrogarActividades" style="display: none">
                                <table>
                                    <tr>
                                        <td colspan="4">Prorrogar entrega de actividades</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblIDActividadPro" runat="server" Text="Número de actividad:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtIDActividadPro" runat="server" Width="50px" disabled="true"></asp:TextBox>
                                            <asp:TextBox ID="TxtIDActividadProReal" runat="server" Width="15px" Visible="false"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblDescActividadPro" runat="server" Text="Descripción de la actividad:" CssClass="txt_gral" disabled="true"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtDescActividadPro" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                                                TextMode="MultiLine" MaxLength="500" disabled="true"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblFecEstEntregaPro" runat="server" Text="Fecha estimada de entrega:" CssClass="txt_gral" disabled="true"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtFecEstEntregaPro" runat="server" Width="150px" disabled="true"></asp:TextBox>
                                            
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td>
                                            <asp:Label ID="LblEstatusPro" runat="server" Text="Estatus" CssClass="txt_gral"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstatusPro" runat="server" Height="20px" Width="255px" AutoPostBack="true"
                                                CssClass="txt_gral" disabled="true">
                                                <asp:ListItem Value="Completa">Completa</asp:ListItem>
                                                <asp:ListItem Value="Incompleta">Incompleta</asp:ListItem>
                                                <asp:ListItem Value="Prorrogada">Prorrogada</asp:ListItem>
                                                <asp:ListItem Value="En Proceso">En Proceso</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr class="prorr">
                                        <td>*</td>
                                        <td>
                                            <asp:Label ID="LblMotivoProrroga" runat="server" Text="Motivo de prórroga:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtMotivoProrroga" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                                                TextMode="MultiLine" MaxLength="600"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr class="prorr">
                                        <td>*</td>
                                        <td>
                                            <asp:Label ID="LblDiasProrroga" runat="server" Text="Días de prórroga:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtDiasProrroga" onkeypress="return isNumberKey(event);" runat="server" Width="50px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr class="prorr">
                                        <td></td>
                                        <td>
                                            <asp:Label ID="LblFecEntrProrroga" runat="server" Text="Nueva fecha estimada de entrega:" CssClass="txt_gral"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtFecEntrProrroga" runat="server" Width="150px" disabled></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
        <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
        <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
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


<script type="text/javascript">
    $(document).ready(function () {
        $(".GridViewContenido .GridViewContenidoAlternate").removeAttr('onmouseover');
        $(".GridViewContenido .GridViewContenidoAlternate").attr('onmouseover', 'tooltip.show(\'Para seleccionar el registro puede hacer click sobre el registro, o la casilla.\');hideTimerId = setTimeout("tooltip.hide(\'\',\'1\', \'#C7F4F8\',\'#44698D\',0,100)",3000);')
        $(".GridViewContenido").click(function () {
            SetCheck(this);
        });
        $(".GridViewContenidoAlternate").click(function () {
            SetCheck(this);
        });
    });

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode
        return !(charCode > 31 && (charCode < 48 || charCode > 57));
    }

    function SetCheck(row) {
        var chk = $(row).find(":checkbox")[0];
        if (chk != undefined) {
            chk.checked = true;
            uncheckOthers(chk);
        }
    }

    $(function () {

        MensajeUnBotonNoAccionLoad();
        MensajeUnBotonUnaAccionLoad();
        MensajeDosBotonesUnaAccionLoad();

    });


    function AquiMuestroMensaje() {

        MensajeUnBotonNoAccion();

    };

    function ModificaFecEntrega() {
        //var NuevaFecha = jsModifica_Fecha(TxtFecEstEntregaPro, '+' + $('#TxtDiasProrroga').val(), 'd', '/');
        ////TxtFecEstEntregaPro
        ////TxtFecEntrProrroga
        //$('#TxtFecEntrProrroga').val(NuevaFecha);
    }

    //function jsModifica_Fecha(fecha, intervalo, dma, separador) {

    //    var separador = separador || "-";
    //    var arrayFecha = fecha.split(separador);
    //    var dia = arrayFecha[0];
    //    var mes = arrayFecha[1];
    //    var anio = arrayFecha[2];

    //    var fechaInicial = new Date(anio, mes - 1, dia);
    //    var fechaFinal = fechaInicial;
    //    if (dma == "m" || dma == "M") {
    //        fechaFinal.setMonth(fechaInicial.getMonth() + parseInt(intervalo));
    //    } else if (dma == "y" || dma == "Y") {
    //        fechaFinal.setFullYear(fechaInicial.getFullYear() + parseInt(intervalo));
    //    } else if (dma == "d" || dma == "D") {
    //        fechaFinal.setDate(fechaInicial.getDate() + parseInt(intervalo));
    //    } else {
    //        return fecha;
    //    }
    //    dia = fechaFinal.getDate();
    //    mes = fechaFinal.getMonth() + 1;
    //    anio = fechaFinal.getFullYear();

    //    dia = (dia.toString().length == 1) ? "0" + dia.toString() : dia;
    //    mes = (mes.toString().length == 1) ? "0" + mes.toString() : mes;

    //    return dia + "-" + mes + "-" + anio;
    //}
    function AltaActividad() {
        $('#divModificarActividades').hide();
        $('#divAltaActividades').show()
        $('#divProrrogarActividades').hide();

        $('#hfTipoVentana').val("A");
        //MensajeDosBotonesUnaAccion();
        $("#divMensajeDosBotonesUnaAccion").dialog({
            resizable: true,
            autoOpen: true,
            height: 250,
            width: 700,
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
                "Aceptar": function () {
                    if ($("#<%=TxtDescActividad.ClientID%>").val() != "" && $("#<%=txtFecEstimEntrega.ClientID%>").val() != "") {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        GudadoActividades();
                    }
                    else {
                        var alrt = '';
                        if ($("#<%=TxtDescActividad.ClientID%>").val() == "" && $("#<%=txtFecEstimEntrega.ClientID%>").val() != "") {
                            alrt='La descripción de la actividad es obligatoria.'
                        }
                        if ($("#<%=TxtDescActividad.ClientID%>").val() != "" && $("#<%=txtFecEstimEntrega.ClientID%>").val() == "") {
                            alrt = 'La fecha estimada de entrega es obligatoria.'
                        }
                        if ($("#<%=TxtDescActividad.ClientID%>").val() == "" && $("#<%=txtFecEstimEntrega.ClientID%>").val() == "") {
                            alrt = 'La descripción de la actividad y la fecha estimada de entrega son obligatorias.'
                        }
                        $('#divTextoAlerta').html(alrt);
                        $("#divAlertaGeneral").dialog({
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
                                },
                            }
                        });
                    }
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
    }

    function ModificaActividad(IdValor) {
        $('#divModificarActividades').show();
        $('#divAltaActividades').hide()
        $('#divProrrogarActividades').hide();
        $('#hfTipoVentana').val("M");
        $("#divMensajeDosBotonesUnaAccion").dialog({
            resizable: true,
            autoOpen: true,
            height: 500,
            width: 700,
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
                "Aceptar": function () {
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                    ModificarDatosActividades(IdValor);
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
    }

    function ProrrogarActividad(comp) {
        $('#divModificarActividades').hide();
        $('#divAltaActividades').hide()
        $('#divProrrogarActividades').show();
        $('#hfTipoVentana').val("P");
        $("#divMensajeDosBotonesUnaAccion").dialog({
            resizable: true,
            autoOpen: true,
            height: 500,
            width: 700,
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
                "Aceptar": function () {

                    if (comp == 1) {
                        $(this).dialog("close");
                        $('#divTextoAlerta').html('No se puede prorrogar una actividad completada');
                        $("#divAlertaGeneral").dialog({
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
                                },
                            }
                        });
                    }
                    else {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        ProrrogaDatosActividades($('#hfSelectedValue').val())
                    }
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
    }

    function ConfirmacionEliminar() {

        MensajeDosBotonesUnaAccion();

    };


    function MensajeConfirmacion() {
        MensajeDosBotonesUnaAccion();
    }


    function GudadoActividades() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardaCamposAlta_Actividades",
            data: '{Folio: ' + '<%=Folio%>' +
                  ', ActividadDes: "' + $("#<%=TxtDescActividad.ClientID%>").val() +
                  '", Usuario: "' + '<%=Usuario_Act%>' +
                  '", FechaEntrega: "' + $("#<%=txtFecEstimEntrega.ClientID%>").val() + '\"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $(location).attr('href', './DetallePC.aspx');
                $("#divMensajeDosBotonesUnaAccion").dialog("close");
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al guardar la actividad');
                $("#divAlertaGeneral").dialog({
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
                        },
                    }
                });
            }
        });
    }


    function CargaDatosActividades() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/CargaCampos_Actividades",
            data: '{IdActividad: ' + $("#<%=hfSelectedValue.ClientID%>").val() + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $("#<%=TxtNumActividad.ClientID%>").val($("#<%=hfIdActividad.ClientID%>").val());
                $("#<%=TxtDescActividadMod.ClientID%>").val(data.d[0]);
                $("#<%=TxtFecEstimEntregMod.ClientID%>").val(data.d[1]);
                $("#<%=hfSelectedValue.ClientID%>").val(data.d[2]);
                ModificaActividad(data.d[2]);
                //$("#divMensajeDosBotonesUnaAccion").dialog("close");
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar los datos de la actividad');
                $("#divAlertaGeneral").dialog({
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
                        },
                    }
                });
            }
        });
    }

  

    $("#<%=TxtDiasProrroga.ClientID%>").change(function () {
        var date = $("#<%=TxtFecEstEntregaPro.ClientID%>").val().split('/');

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/CalculaFecha",
            data: '{Fecha: "' + $("#<%=TxtFecEstEntregaPro.ClientID%>").val() + '",Dias: ' + $("#<%=TxtDiasProrroga.ClientID%>").val() + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $("#<%=TxtFecEntrProrroga.ClientID%>").val(data.d);
            },
            failure: function () {
                $('#divTextoAlerta').html('Error al calcular la fecha.');
                $("#divAlertaGeneral").dialog({
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
                        },
                    }
                });
            }
        });

        <%--date[0] = Number(date[0]) + Number($("#<%=TxtDiasProrroga.ClientID%>").val());
        date[0] = ("0" + date[0]).slice(-2);
        $("#<%=TxtFecEntrProrroga.ClientID%>").val(date.join('/'));--%>
    });

    function CargaDatosProrroga() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/CargaCampos_Actividades",
            data: '{IdActividad: ' + $("#<%=hfSelectedValue.ClientID%>").val() + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $("#<%=TxtIDActividadPro.ClientID()%>").val($("#<%=hfIdActividad.ClientID%>").val());
                $("#<%=TxtDescActividadPro.ClientID()%>").val(data.d[0]);
                $("#<%=TxtFecEstEntregaPro.ClientID%>").val(data.d[1]);
                $("#<%=hfSelectedValue.ClientID%>").val(data.d[2]);
                $("#<%=ddlEstatusPro.ClientID%>").val(data.d[3]);
                if (data.d[3] == "Completa") {
                    $(".prorr").hide();
                    ProrrogarActividad(1);
                }
                else {
                    $(".prorr").show();
                    ProrrogarActividad(0);
                }
                
                //$("#divMensajeDosBotonesUnaAccion").dialog("close");
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar los datos de la prorroga');
                $("#divAlertaGeneral").dialog({
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
                        },
                    }
                });
            }
        });
    }


    function ModificarDatosActividades(IdActividad) {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ModificaCampos_Actividades",
            data: '{IdActividad: ' + IdActividad +
                 ', Estatus: "' + $("#<%=ddlTEstatusMod.ClientID%>").val() +
                 '", Usuario: "' + '<%=Usuario_Act%>' +
                 '",Comentarios:"' + $("#<%=TxtComentariosMod.ClientID%>").val() + '\"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $(location).attr('href', './DetallePC.aspx');
                $("#divMensajeDosBotonesUnaAccion").dialog("close");
                
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al editar los datos de actividades');
                $("#divAlertaGeneral").dialog({
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
                        },
                    }
                });
            }
        });


    }

    function ProrrogaDatosActividades(IdActividad) {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardaCamposProrroga",
            data: '{IdActividad: ' + IdActividad +
                 ', MotivoProrroga: "' + $("#<%=TxtMotivoProrroga.ClientID%>").val() +
                 '", DiasProrroga: ' + $("#<%=TxtDiasProrroga.ClientID%>").val() +
                 ', FechaEntrega: "' + $("#<%=TxtFecEntrProrroga.ClientID%>").val() +
                 '", Usuario: "' + '<%=Usuario_Act%>' + '\"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $(location).attr('href', './DetallePC.aspx');
                $("#divMensajeDosBotonesUnaAccion").dialog("close");
                
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al guardar los datos de prorroga');
                $("#divAlertaGeneral").dialog({
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
                        },
                    }
                });
            }
        });


    }


    function GuardaValorGrid(Valor, NumActividad) {
        $("#<%=hfSelectedValue.ClientID%>").val(Valor);
        $("#<%=hfIdActividad.ClientID%>").val(NumActividad);

        
    }
</script>
