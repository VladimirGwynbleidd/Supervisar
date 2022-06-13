<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="IrregularidadCompleta.ascx.vb" Inherits="SEPRIS.IrregularidadCompleta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<link href="../Styles/Site.css" rel="stylesheet" />

<div>
    <table>
        <tr>
            <td>
                <asp:Label ID="LblNumIrregularidad" runat="server" Text="Número de irregularidad" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label1" runat="server" Text="*" CssClass="txt_gral"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtNumIrregularidad" runat="server" Width="150px" disabled="true" CssClass="txt_gral"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblFecIrregularidad" runat="server" Text="Fecha Irregularidad" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label3" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtFecIrregularidad" runat="server" Width="150px" disabled="true" CssClass="txt_gral"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblProceso" runat="server" Text="Proceso" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label4" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtProceso" runat="server" Width="377px" disabled="true" CssClass="txt_gral"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblSubproceso" runat="server" Text="Subproceso" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label5" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtSubproceso" runat="server" Width="377px" disabled="true" CssClass="txt_gral"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblConducta" runat="server" Text="Conducta Sancionable" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label6" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtConducta" runat="server" Width="377px" disabled="true" CssClass="txt_gral"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Irregularidad" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label7" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtDescIrregularidad" runat="server" Width="377px" disabled="true" CssClass="txt_gral"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="La irregularidad se ha corregido" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label9" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlCorreccion" runat="server" CssClass="txt_gral">
                    <asp:ListItem Selected="True" Value=""> --Selecciona una opción-- </asp:ListItem>
                    <asp:ListItem Value="Total"> Total </asp:ListItem>
                    <asp:ListItem Value="Parcial"> Parcial </asp:ListItem>
                    <asp:ListItem Value="Nula"> Nula </asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label10" runat="server" Text="¿Como corrige el PC la irregularidad identificada?" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label11" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtComoCorrigeIrregularidad" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                    TextMode="MultiLine" MaxLength="500"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr class="ddlCorreccion">
            <td>
                <asp:Label ID="Label12" runat="server" Text="Fecha de corrección" CssClass="txt_gral"></asp:Label>
                <asp:Label ID="Label13" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtFecCorreccion" runat="server" Width="150px" CssClass="txt_gral"></asp:TextBox>
                <ajx:CalendarExtender ID="calEx" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalFechaRecepcion"
                    TargetControlID="TxtFecCorreccion" CssClass="teamCalendar" />
                <asp:Image ImageAlign="Bottom" ID="imgCalFechaRecepcion" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
            </td>
        </tr>
    </table>
</div>





<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=ddlCorreccion.ClientID%>").change(function () {
            if($("#<%=ddlCorreccion.ClientID%>").val() == "Nula"){
                $(".ddlCorreccion").hide();
                $("#<%=TxtFecCorreccion.ClientID%>").val('');
            }
            else {
                $(".ddlCorreccion").show();
            }
        });
    });

    function CargarDatosIrregularidad(Num, Irregularidad) {
        var ret = false;
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerDatosIrregularidad",
            data: '{Irregularidad: ' + Irregularidad + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                console.log(data);
                if (data.d[9] == "False") {
    
                    $("#<%=TxtNumIrregularidad.ClientID%>").val(Num);
                    $("#<%=txtFecIrregularidad.ClientID%>").val(data.d[1])
                    $("#<%=TxtProceso.ClientID%>").val(data.d[2])
                    $("#<%=TxtSubproceso.ClientID%>").val(data.d[3])
                    $("#<%=TxtConducta.ClientID%>").val(data.d[4])
                    $("#<%=TxtConducta.ClientID%>").attr("title", "");
                    $("#<%=TxtConducta.ClientID%>").attr("title", data.d[4]);
                    $("#<%=txtDescIrregularidad.ClientID%>").val(data.d[5])
                    $("#<%=txtDescIrregularidad.ClientID%>").attr("title", "");
                    $("#<%=txtDescIrregularidad.ClientID%>").attr("title", data.d[5]);
                    $("#<%=TxtComoCorrigeIrregularidad.ClientID%>").val('')
                    $("#<%=TxtFecCorreccion.ClientID%>").val('')
                    $("#<%=ddlCorreccion.ClientID%>").val('')
                    ret = true;
                }
                else {
                    ret = false;
                    <%--$("#<%=TxtNumIrregularidad.ClientID%>").val(Num);
                    $("#<%=txtFecIrregularidad.ClientID%>").val(data.d[1])
                    $("#<%=TxtProceso.ClientID%>").val(data.d[2])
                    $("#<%=TxtSubproceso.ClientID%>").val(data.d[3])
                    $("#<%=TxtConducta.ClientID%>").val(data.d[4])
                    $("#<%=txtDescIrregularidad.ClientID%>").val(data.d[5])

                    $("#<%=TxtComoCorrigeIrregularidad.ClientID%>").val(data.d[6])
                    $("#<%=TxtComoCorrigeIrregularidad.ClientID%>").attr('disabled', 'disabled');

                    $("#<%=TxtFecCorreccion.ClientID%>").val(data.d[7])
                    $("#<%=TxtFecCorreccion.ClientID%>").attr('disabled', 'disabled');

                    $("#<%=ddlCorreccion.ClientID%>").val(data.d[8])
                    $("#<%=ddlCorreccion.ClientID%>").attr('disabled', 'disabled');

                    $("#MainContent_ctl00_IrregularidadCompleta1_imgCalFechaRecepcion").hide();--%>
                    //$("#MainContent_ctl00_IrregularidadCompleta1_imgCalFechaRecepcion").css('opacity', '0.4');
                }
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar los subprocesos');
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
                ret = false;
            }
        });
        return ret;
    }


    function CompletarIrregularidad(dlg) {
        var err = 0;
        $("#errUl").empty();
        if ($("#<%=ddlCorreccion.ClientID%>").val() == "") {
            err++;
            $("#errUl").append("<li>El tipo de corrección es obligatoria.</li>")
            if ($("#<%=TxtFecCorreccion.ClientID%>").val() == "") {
                err++;
                $("#errUl").append("<li>La fecha de correción es obligatoria.</li>")
            }
        }
        else {
            if ($("#<%=ddlCorreccion.ClientID%>").val() != "Nula") {
                if ($("#<%=TxtFecCorreccion.ClientID%>").val() == "") {
                    err++;
                    $("#errUl").append("<li>La fecha de correción es obligatoria.</li>")
                }
            }
        }

        if ($("#<%=TxtComoCorrigeIrregularidad.ClientID%>").val() == "") {
            err++;
            $("#errUl").append("<li>El como corrige el PC la irregularidad es obligatorio.</li>")
        }

        if (err == 0) {
            $(dlg).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/CompletarIrregularidad",
                data: '{Irregularidad: ' + $("#hfSelectedValueIrregularidad").val() + ', TipoCorreccion: "' + $("#<%=ddlCorreccion.ClientID%>").val() + '", Comentarios: "' + $("#<%=TxtComoCorrigeIrregularidad.ClientID%>").val() + '", Fecha: "' + $("#<%=TxtFecCorreccion.ClientID%>").val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $(location).attr('href', './DetallePC.aspx');
                },
                failure: function () {
                    $('#divTextoAlerta').html('Error al actualizar la irregularidad');
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
        else {
            $("#errorIrregularidad").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 400,
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
                    }
                }
            });
        }
    }

</script>
<asp:HiddenField ID="hdIrregularidad" runat="server" />

