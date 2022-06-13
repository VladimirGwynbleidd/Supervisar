<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Inspector.ascx.vb" Inherits="SEPRIS.Inspector" %>

<script type="text/javascript">
    var Inspector;
    function ObtenerListaInspectores() {
        $("#<%=lstInspectorDisponible.ClientID%>").empty();
        $("#<%=lstInspectorSeleccionado.ClientID%>").empty();
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerInspectores",
            data: '{Subproceso: ' + ' <%=SubProcesoView%>' + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#<%=lstInspectorDisponible.ClientID%>").html("")
                $.each(data.d, function () {
                    $("#<%=lstInspectorDisponible.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));

                });
            },
            failure: function () {

            }
        });
    }

    function ObtenerListaInspectoresPaso2(SubProceso) {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerInspectores",
            data: '{Subproceso: ' + SubProceso + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#<%=lstInspectorDisponible.ClientID%>").html("")
                $.each(data.d, function () {
                    $("#<%=lstInspectorDisponible.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));

                });
            },
            failure: function () {

            }
        });
    }

    function AgregarInspector() {
        if ($("#<%=lstInspectorDisponible.ClientID%> option:selected").length > 0) {
        $("#<%=lstInspectorSeleccionado.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstInspectorDisponible.ClientID%> option:selected").val()).text($("#<%=lstInspectorDisponible.ClientID%> option:selected").text()));
        $("#<%=lstInspectorSeleccionado.ClientID%>").val($("#<%=lstInspectorDisponible.ClientID%>").val());
        $("#<%=lstInspectorDisponible.ClientID%> option:selected").remove();
    }
    else {
            $('#divTextoAlerta').html('Selecciona un supervisor');
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

function QuitarInspector() {
    if ($("#<%=lstInspectorSeleccionado.ClientID%> option:selected").length > 0) {
            $("#<%=lstInspectorDisponible.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstInspectorSeleccionado.ClientID%> option:selected").val()).text($("#<%=lstInspectorSeleccionado.ClientID%> option:selected").text()));
            $("#<%=lstInspectorSeleccionado.ClientID%> option:selected").remove();

        }
        else {
        $('#divTextoAlerta').html('Selecciona un supervisor');
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

    function GuardarInspectorInfo() {
        GetInspectorInfo();
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardarInfoInspector",
            data: '{Folio: ' + '<%=Folio%>' +
            ', Inspectores: "' + Inspector + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar el checklist');
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

    function GuardarInspectorInfo_Status() {

        GetInspectorInfo();
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardarInspectorInfo_Status",
            data: '{Folio: ' + '<%=Folio%>' + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $(location).attr('href', './DetallePC.aspx');
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar el checklist');
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


    function GetInspectorInfo() {

        Inspector = new Array();
        $("#<%=lstInspectorSeleccionado.ClientID%> option").each(function (i, selected) {
            Inspector.push($(selected).val())

        });
    }

    function HabilitarFechaIzquierda() {
        $("#<%=ImageButton1.ClientID%>").removeAttr('disabled');
    }
    function HabilitarFechaDerecha() {
        $("#<%=btnDerecha.ClientID%>").removeAttr('disabled');
    }

</script>
<asp:Panel ID="pnlAgregarInspector" runat="server">
    <table style="width: 100%">
        <tr>
            <td colspan="4" style="height: 35px; width: 100%; text-align: left;">
                <label class="txt_gral">Inspector (es):</label></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <label class="txt_gral">Disponible(s):</label></td>
            <td></td>
            <td style="height: 35px; width: 25%; text-align: left;">
                <label class="txt_gral">Seleccionado(s):</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="lstInspectorSeleccionado" ErrorMessage="Seleccione un inspector" ForeColor="Red" ValidationGroup="ucInspector">&nbsp;</asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td style="width: 20%"></td>
            <td style="width: 30%">
                <asp:ListBox ID="lstInspectorDisponible" runat="server" CssClass="txt_gral lstinspe" Height="150px" Width="255px"></asp:ListBox>
            </td>
            <td style="width: 10%">
                <asp:ImageButton ID="btnDerecha" runat="server" ImageUrl="~/Images/FlechaRojaDer.gif" OnClientClick="AgregarInspector(); return false;" />
                <br />
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/FlechaRojaIzq.gif" OnClientClick="QuitarInspector(); return false;" />
            </td>
            <td style="width: 30%">
                <asp:ListBox ID="lstInspectorSeleccionado" runat="server" CssClass="txt_gral lstinspe" Height="150px" Width="255px"></asp:ListBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="lstInspectorSeleccionado" ErrorMessage="Seleccione un Inspector" ForeColor="Red">&nbsp;</asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</asp:Panel>
