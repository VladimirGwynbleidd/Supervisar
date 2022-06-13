<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BandejaIrregularidades.ascx.vb" Inherits="SEPRIS.BandejaIrregularidades" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<asp:UpdatePanel ID="upnlIrregInfo" runat="server">
    <ContentTemplate>
        <br />
        <br />
        <div class="txt_gral" style="font-weight:normal">
            Irregularidades Identificadas
        </div>  
        <div id="divReqInfo">
            <br />

            <cc1:CustomGridView ID="gvReqInformac" runat="server" DataKeyNames="N_ID_FOLIO" HabilitaSeleccion="SimpleCliente"
                Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValueRequerimiento"
                HeigthScroll="0" UnicoEnPantalla="True" WidthScroll="0" SelectedMultiRows="false"
                AllowPaging="true" ShowHeaderWhenEmpty="true">

                <Columns>
                    <asp:BoundField HeaderText="" DataField="I_ID_IRREGULARIDAD" SortExpression="I_ID_IRREGULARIDAD" Visible="true">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Fecha de irregularidad" DataField="F_FECH_IRREGULARIDAD" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_IRREGULARIDAD">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Proceso" DataField="DESC_PROCESO" SortExpression="DESC_PROCESO">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="SubProceso" DataField="DESC_SUBPROCESO" SortExpression="DESC_SUBPROCESO">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Conducta" DataField="DESC_CONDUCTA" SortExpression="DESC_CONDUCTA">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                   <asp:BoundField HeaderText="Irregularidad" DataField="T_DSC_IRREGULARIDAD_POR_SANCIONAR" SortExpression="T_DSC_IRREGULARIDAD_POR_SANCIONAR">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Comentarios" DataField="T_DSC_COMENTARIO" SortExpression="T_DSC_COMENTARIO">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnModificarIrreg" runat="server" ImageUrl="~/Imagenes/edit.png" Width="25px" CausesValidation="false" CommandName="Modificar"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEliminarIrreg" runat="server" ImageUrl="~/Imagenes/pencil-delete.png" Width="25px" CausesValidation="false" CommandName="Eliminar"/>
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
                    Dias: <asp:TextBox ID="txtDiasProrroga" runat="server"></asp:TextBox>
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
        debugger;
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
                    var nDias = $("#<%=txtDiasProrroga.ClientID%>").val();
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
            data: '{IDReq: ' + nID + ', iDias: ' + nDias + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false, 
            success: function (data) {
                return true;
            },
            failure: function () {
                alert("Error al cargar información de los requerimientos");
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
                alert("Error al cargar información de los requerimientos");
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
                alert("Error al cargar información de los requerimientos");
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
