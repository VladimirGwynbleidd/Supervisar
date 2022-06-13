<%@ Control Language="vb" ClassName="RequerimientoInformacion" AutoEventWireup="false" CodeBehind="RequerimientoInformacion.ascx.vb" Inherits="SEPRIS.RequerimientoInformacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<asp:UpdatePanel ID="upnlReqInfo" runat="server">
    <ContentTemplate>
        <br />
        <br />
        <div class="txt_gral" style="font-weight:normal">
            Requerimientos de información
        </div>  
        <div id="divReqInfo">
            <br />

            <cc1:CustomGridView ID="gvReqInformac" runat="server" DataKeyNames="I_ID_REQUERIMIENTO" HabilitaSeleccion="SimpleCliente"
                Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValueRequerimiento"
                HeigthScroll="0" UnicoEnPantalla="True" WidthScroll="0" SelectedMultiRows="false"
                AllowPaging="true" ShowHeaderWhenEmpty="true">

                <Columns>
                    <asp:BoundField HeaderText="Número de requerimiento" DataField="R_REQUERIMIENTO" SortExpression="I_ID_REQUERIMIENTO">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Número de requerimiento" DataField="I_ID_REQUERIMIENTO" SortExpression="I_ID_REQUERIMIENTO" Visible="false">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Estatus" DataField="DESC_ESTATUS" SortExpression="DESC_ESTATUS">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Núm. Oficio SICOD" DataField="T_FOLIO_SICOD" SortExpression="T_FOLIO_SICOD" >
                         </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de acuse" DataField="F_FECH_ACUSE" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_ACUSE">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Fecha estimada de entrega de información" DataField="F_FECH_ESTIMADA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_ESTIMADA">
                        <ItemStyle Width="200px" />
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>



                    <asp:BoundField HeaderText="Fecha real de respuesta" DataField="F_FECH_REAL" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_REAL">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnProrroga" runat="server" ImageUrl="~/Imagenes/solicitarProrroga.png" Width="25px" CausesValidation="false" CommandName="Adjuntar" OnClientClick='<%# "Prorroga(" + DataBinder.Eval(Container, "RowIndex", "{0}") + "," + Eval("I_ID_REQUERIMIENTO").ToString() + ");"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEntregado" runat="server" ImageUrl="~/Imagenes/aprobarProrroga.png" Width="25px" CausesValidation="false" OnClientClick='<%# "Entregado(" + DataBinder.Eval(Container, "RowIndex", "{0}") + "," + Eval("I_ID_REQUERIMIENTO").ToString() + "); return false;"%>' />
                            <asp:ImageButton ID="btnNoEntregado" runat="server" ImageUrl="~/Imagenes/RechazarProrroga.png" Width="25px" CausesValidation="false" OnClientClick='<%# "NoEntregado(" + DataBinder.Eval(Container, "RowIndex", "{0}") + "," + Eval("I_ID_REQUERIMIENTO").ToString() + "); return false;"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                   
                </Columns>
                <EmptyDataTemplate>
                    <div style="text-align:center;">
                        <img id="Noexisten" runat="server" src="~/Imagenes/No%20Existen.gif" alt="No existen Registos para la Consulta" />
                    </div>
                </EmptyDataTemplate> 
            </cc1:CustomGridView>
        </div>
        <br />
        <br />
        <asp:HiddenField ID="hfSelectedValueRequerimiento" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfTipoGuardar" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfFecEstEntrega" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfFecRealEntrega" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfIDRequerimiento" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
        <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
        <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
    </ContentTemplate>

    <Triggers>
        <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
        <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
        <asp:PostBackTrigger ControlID="btnConsulta" />
    </Triggers>
</asp:UpdatePanel>



<div id="divSolicitudProrroga" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    Dias: <asp:TextBox ID="txtDiasProrroga2" runat="server"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
</div>


<div id="divSolicitudEntregado" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    Fecha Real de Entrega: <asp:TextBox ID="TxtNvaFecRealEntrega" runat="server"></asp:TextBox>
                            <ajx:calendarextender ID="calFechaPI" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFechaPI"
                                TargetControlID="TxtNvaFecRealEntrega" CssClass="teamCalendar" />
                            <asp:Image ID="imgFechaPI" runat="server" ImageUrl="~/Imagenes/Calendar.gif" Width="16px"
                                ImageAlign="Bottom" Height="16px" />
                </div>
            </td>
        </tr>
    </table>
</div>


<script type="text/javascript">
    //function SolicitaFecRealEntrega() {
    //    //Levanta la modal
    //    var url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';
    //    var winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
    //    window.showModalDialog(url, "SICOD", winprops);
    //}

    function Prorroga(RowIndex, nIDReq) {
        
        $("#<%=hfSelectedValueRequerimiento.ClientID%>").val(RowIndex);
        $("#divSolicitudProrroga").dialog({
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
                "Aceptar": function () {
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                    var nDias = $("#<%=txtDiasProrroga2.ClientID%>").val();
                    GuardarProrrogaReq(nIDReq, nDias);
                    $(location).attr('href', './DetallePC.aspx');
                },
                "Cancelar": function () {
                    $('#hfTipoGuardar').val("");
                    $(this).dialog("close");
                }
            }
        });
    }


    function GuardarProrrogaReq(nID, nDias) {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardandoProrroga",  
            data: '{IDReq: ' + nID + ', iDias: ' + nDias + ', Usuario:"<%=Usuario%>"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false, 
            success: function (data) {
                return true;
            },
            failure: function () {
                $('#divTextoAlerta').html('Error al cargar la información de los requerimientos');
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


    function Entregado(RowIndex, nIDReq) {
        $("#<%=hfSelectedValueRequerimiento.ClientID%>").val(RowIndex);
        var dNvaFecha;
        $("#divSolicitudEntregado").dialog({
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
                "Aceptar": function () {
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                    var dNvaFecha = $("#<%=TxtNvaFecRealEntrega.ClientID%>").val();
                    GuardarEntregado(nIDReq, dNvaFecha);
                    $(location).attr('href', './DetallePC.aspx');
                },
                "Cancelar": function () {
                    $('#hfTipoGuardar').val("");
                    $(this).dialog("close");
                }
            }
        });
    }


    function GuardarEntregado(nID, dNvaFecha) {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardandoEntregado",
            data: '{IDReq: ' + nID + ', dNvaFecha: "' + dNvaFecha + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $(location).attr('href', './DetallePC.aspx');
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar la información de los requerimientos');
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

    function NoEntregado(RowIndex, nIDReq) {
        $("#<%=hfSelectedValueRequerimiento.ClientID%>").val(RowIndex);

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardandoNoEntregado",
            data: '{IDReq: ' + nIDReq + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $(location).attr('href', './DetallePC.aspx');
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar la información de los requerimientos');
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




    //function CargarDatosRequerimiento(Num, Requerimiento) {
    //    $.ajax({
    //        type: "POST",
    //        url: "RequerimientoInformacion.ascx/CargarDatosRequerimiento",
    //        data: '{IDRequerimiento: ' + Requerimiento + ', NumRow:' + Num + '}',
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (data) {
    //            console.log(data);
    //            $("#hfIDRequerimiento").val(Num);
    //            $("#hfFecRealEntrega").val(data.d[4]);
    //            $("#hfFeEstEntrega").val(data.d[3]);
    //        },

    //        failure: function () {
    //            alert("Error al cargar información de los requerimientos");
    //        }
    //    });
    //}


</script>