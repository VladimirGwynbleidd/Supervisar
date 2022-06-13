<%@ Control Language="vb" ClassName="Irregularidades" AutoEventWireup="false" CodeBehind="Irregularidades.ascx.vb" Inherits="SEPRIS.Irregularidades" %>
<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>

<%@ Register Src="Irregularidad.ascx" TagName="Irregularidad" TagPrefix="uc2" %>

<%@ Register Src="IrregularidadCompleta.ascx" TagName="IrregularidadCompleta" TagPrefix="uc3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/PC/UserControls/RequerimientoInformacion.ascx" TagPrefix="uc1" TagName="RequerimientoInformacion" %>

<br />
<div class="txt_gral" style="font-weight: bold">
    Irregularidades identificadas:
</div>
<br />
<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <div style="text-align: left; width: 100%; padding-bottom: 5px;">
            <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel"  visible="False" />
            &nbsp;
            <asp:Button ID="btnPersonalizarColumnas" runat="server" Text="Personalizar Columnas"
                Style="width: 120px;" visible="False"/>
        </div>
        <br />
        <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="I_ID_IRREGULARIDAD,Row#" HabilitaSeleccion="SimpleCliente"
            Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValueIrregularidad"
            HeigthScroll="0" UnicoEnPantalla="True" WidthScroll="0" SelectedMultiRows="false"
            AllowPaging="true">

            <Columns>
                <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkElemento" runat="server" AutoPostBack="false" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="ID" DataField="Row#" SortExpression="Row#" Visible="true">
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />

                </asp:BoundField>

                <asp:BoundField HeaderText="ID" DataField="I_ID_IRREGULARIDAD" SortExpression="I_ID_IRREGULARIDAD" Visible="false">
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Fecha irregularidad" DataField="F_FECH_IRREGULARIDAD" SortExpression="F_FECH_IRREGULARIDAD">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Proceso" DataField="DESC_PROCESO" SortExpression="DESC_PROCESO">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Subproceso" DataField="DESC_SUBPROCESO" SortExpression="DESC_SUBPROCESO">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Conducta sancionable" DataField="DESC_CONDUCTA" SortExpression="DESC_CONDUCTA">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Irregularidad" DataField="DESC_IRREGULARIDAD" SortExpression="DESC_IRREGULARIDAD">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Participante" DataField="DESC_PARTICIPANTE" SortExpression="DESC_PARTICIPANTE">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Gravedad" DataField="DESC_GRAVEDAD" SortExpression="DESC_GRAVEDAD">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Comentarios" DataField="T_DSC_COMENTARIO" SortExpression="T_DSC_COMENTARIO">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>

                <%--<asp:BoundField HeaderText="Corregida" DataField="T_DSC_CORREGIDO" SortExpression="T_DSC_CORREGIDO"></asp:BoundField>--%>
                <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <img id="MainContent_ctl00_imgCompleta" src="<%# If(Eval("B_COMPLETA").ToString() = "True", "../Imagenes/aceptado.png", "../Imagenes/ERROR.jpg")%>" style="height: 15px; width: 15px;">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Fecha corrección" DataField="F_FECH_CORRECCION" SortExpression="F_FECH_CORRECCION">
                    <ItemStyle Width="100px" />
                    <HeaderStyle Width="100px" />
                </asp:BoundField>
            </Columns>
        </cc1:CustomGridView>
        <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
            alt="No existen Registos para la Consulta" />

        &nbsp;<asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />

        <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfSelectedValueIrregularidad" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfSelectedRowIrregularidad" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfTipoVentana" runat="server" ClientIDMode="Static" Value="-1" />
        <br />
        <br />
        <div>
            <table width="250px" border="0" style="align-content: center">
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
                </tr>
            </table>
        </div>
        <br />
        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClientClick="AbrirPop(0); return false;" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" OnClientClick="AbrirPop(1); return false;" />
        &nbsp;&nbsp;&nbsp
        <asp:Button ID="btnCompletar" runat="server" Text="Completar" OnClientClick="CompletarIrregulardad(); return false;" />
        &nbsp;&nbsp;&nbsp;
    </ContentTemplate>
</asp:UpdatePanel>
<wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server" />


<div id="divAgregarIrregularidad" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    <uc2:Irregularidad ID="Irregularidad1" runat="server" BotonesVisibles="false" />
                </div>
            </td>
        </tr>
    </table>
</div>


<div id="divCompletarIrregularidad" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    <%--<uc1:RequerimientoInformacion runat="server" ID="RequerimientoInformacion" />--%>
                    <uc3:IrregularidadCompleta ID="IrregularidadCompleta1" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</div>

<div id="errorIrregularidad" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left"><a href="Actividad.vb">Actividad.vb</a>
                <div class="MensajeModal-UI">
                    <ul id="errUl">
                    </ul>
                </div>
            </td>
        </tr>
    </table>
</div>

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

    function SetCheck(row) {
        var chk = $(row).find(":checkbox")[0];
        if (chk != undefined) {
            chk.checked = true;
            uncheckOthers(chk);
        }
    }

    function AbrirPop(Accion) {
        CargaProcesosIrregularidad()
        if (Accion == 1) {
            if ($("#hfSelectedRowIrregularidad").val() != "-1") {
                if (!CargarDatosModificar($("#hfSelectedRowIrregularidad").val(), $("#hfSelectedValueIrregularidad").val())) {
                    $('#divTextoAlerta').html('No se puede modificar una irregularidad ya completada');
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
                    $("#divAgregarIrregularidad").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 400,
                        width: 600,
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
                                ActualizarIrregularidad($("#hfSelectedValueIrregularidad").val());
                                $(this).dialog("close");
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
            else {
                $('#divTextoAlerta').html('Seleccione una irregularidad');
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
                return false;
            }
        }
        if (Accion == 0) {
            limpia();
            $("#divAgregarIrregularidad").dialog({
                resizable: false,
                autoOpen: true,
                height: 400,
                width: 600,
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
                        GuardarIrregularidad();
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    }


    function AltaIrregulardad() {

        $("#divAgregarIrregularidad").dialog({
            resizable: false,
            autoOpen: true,
            height: 400,
            width: 600,
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
                    GuardarIrregularidad();
                    $(this).dialog("close");
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
    }


    function CompletarIrregulardad() {
        $("#MainContent_ValidationSummary1").hide();
        if ($("#hfSelectedRowIrregularidad").val() == "-1") {
            $('#divTextoAlerta').html('Seleccione una irregularidad para completar');
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
            return false;
        }
        else {
            
            var chk = CargarDatosIrregularidad($("#hfSelectedRowIrregularidad").val(), $("#hfSelectedValueIrregularidad").val())
            if (chk) {
                $("#divCompletarIrregularidad").dialog({
                    resizable: false,
                    autoOpen: true,
                    height: 450,
                    width: 600,
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
                            //$(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                            CompletarIrregularidad(this);
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
            else {
                $('#divTextoAlerta').html('Ésta irregularidad ya está completada');
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
        }
    }
</script>
