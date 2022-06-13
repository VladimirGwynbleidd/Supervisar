<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReasignaSupervisores.aspx.vb" Inherits="SEPRIS.ReasignaSupervisores" %>

<%@ Register Src="UserControls/Supervisor.ascx" TagName="Supervisor" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Reasigna Supervisores</title>
    <link href="/Styles/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap-3.3.4/js/bootstrap.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>
    <script src="/Scripts/MensajeModal.js" type="text/javascript"></script>

    <script type="text/javascript">
        var Supervisor;

        $(document).ready(function () {
            $("#lstSupervisorDisponible").empty();
            $("#lstSupervisorSeleccionado").empty();
            $("#ddlProcesoP2").change(function () {
                $("#lstSupervisorDisponible").empty();
                $.ajax({
                    type: "POST",
                    url: "Paso1.aspx/ObtenerSubprocesos",
                    async: false,
                    data: '{Proceso: ' + $("#ddlProcesoP2").val() + ' }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $("#ddlSubprocesoP2").html("").append('<option value="0">--Seleccionar--</option>');
                        $.each(data.d, function () {
                            $("#ddlSubprocesoP2").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                        });
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
                    }
                });
            });
        });

        function ObtenerListaSupervisores() {
            $("#lstSupervisorSeleccionado").empty();
            $("#lstSupervisorDisponible").empty();
            $.ajax({
                type: "POST",
                url: "Paso1.aspx/ObtenerSupervisores",
                async: false,
                data: '{Subproceso: ' + $("#ddlSubprocesoP2").val() + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#lstSupervisorDisponible").empty();
                    $.each(data.d, function () {
                        var valSup = this['Value']
                        var txtSup = this['Text']
                        $("#lstSupervisorDisponible").append($("<option></option>").attr("value", valSup).text(txtSup));
                    });
                },

                failure: function () {
                    $('#divTextoAlerta').html('Error al cargar los supervisores');
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
            SubProcesoValor = $("#ddlSubprocesoP2").val();
        }

        function GetSupervisores() {
            Supervisor = new Array();
            $("#lstSupervisorSeleccionado option").each(function (i, selected) {
                Supervisor.push($(selected).val())
            });
        }

        function Aceptar() {
            GetSupervisores();
            if ($("#ddlProcesoP2").val() == "-1" && ($("#ddlSubprocesoP2").val() == "0" || $("#ddlSubprocesoP2").val() == null)) {
                $('#divTextoAlerta').html('Seleccione una proceso y un subproceso');
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
                if ($("#ddlProcesoP2").val() != "-1" && ($("#ddlSubprocesoP2").val() == "0" || $("#ddlSubprocesoP2").val() == null)) {
                    $('#divTextoAlerta').html('Seleccione un proceso');
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
                    if (Supervisor.length > 0) {
                        $.ajax({
                            type: "POST",
                            async:false,
                            url: "ReasignaSupervisores.aspx/Reasignar",
                            data: '{Folio: ' + $("#N_Folio").val() +
                            ', Proceso: ' + $("#ddlProcesoP2").val() +
                            ', SubProceso: ' + $("#ddlSubprocesoP2").val() +
                            ', Usuario: "' + $("#hdnUsuario").val() +
                            '", Supervisor: "' + Supervisor + "\"}",

                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                window.top.location.href = './BandejaPC.aspx';
                            },
                            failure: function () {
                                $('#divTextoAlerta').html('Error al reasignar los supervisores');
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
                        })
                    }
                    else {
                        $('#divTextoAlerta').html('Debe seleccionar al menos un supervisor');
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
        }

        function AgregarSupervisor() {
            if ($("#lstSupervisorDisponible option:selected").length > 0) {
                $("#lstSupervisorSeleccionado").append($("<option></option>").attr("value", $("#lstSupervisorDisponible  option:selected").val()).text($("#lstSupervisorDisponible  option:selected").text()));
                $("#lstSupervisorDisponible option:selected").remove();
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

        function QuitarSupervisor() {
            if ($("#lstSupervisorSeleccionado option:selected").length > 0) {
                $("#lstSupervisorDisponible").append($("<option></option>").attr("value", $("#lstSupervisorSeleccionado  option:selected").val()).text($("#lstSupervisorSeleccionado  option:selected").text()));
                $("#lstSupervisorSeleccionado option:selected").remove();
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

        function Cerrar() {
            window.parent.$("#dvReasigna").dialog('close')
        }
    </script>

</head>
<body>
    <div>
        <form runat="server">
            <asp:ScriptManager runat="server" ID="SM" EnableScriptGlobalization="true" EnablePageMethods="true">
            </asp:ScriptManager>

            <table style="width: 100%">
                <tr>
                    <td style="height: 35px; width: 20%; text-align: left;">
                        <label class="txt_gral">Proceso:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
                    </td>
                    <td style="height: 35px; width: 30%; text-align: left;">
                        <asp:DropDownList ID="ddlProcesoP2" runat="server" Width="150px" CssClass="txt_gral"></asp:DropDownList>
                    </td>
                    <td style="height: 35px; width: 20%; text-align: left;">
                        <label class="txt_gral">Subproceso:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSubprocesoP2" runat="server" Width="150px" CssClass="txt_gral ddlSubproceso"></asp:DropDownList>
                    </td>
                </tr>
                <tr id="prcs" runat="server">
                    <td style="height: 35px; width: 20%; text-align: left;">
                        <label class="txt_gral">Proceso:</label>
                    </td>
                    <td style="height: 35px; width: 30%; text-align: left;">
                        <asp:DropDownList ID="ddlProceso" runat="server" Width="150px" CssClass="txt_gral"></asp:DropDownList>

                    </td>
                    <td style="height: 35px; width: 20%; text-align: left;">
                        <label class="txt_gral">Subproceso:</label></td>
                    <td>
                        <asp:DropDownList ID="ddlSubproceso" runat="server" Width="150px" CssClass="txt_gral"></asp:DropDownList>

                    </td>

                </tr>
                <tr>
                    <td style="height: 35px; text-align: left;" colspan="4">
                        <label class="txt_gral">Supervisor (es):</label></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <table style="height: 100%; width: 100%; padding: 0px; border-spacing: 0px">
                            <tr>
                                <td style="width: 45%">
                                    <label class="txt_gral">Disponible(s):</label>
                                </td>
                                <td style="width: 10%"></td>
                                <td style="width: 45%">
                                    <label class="txt_gral">Seleccionado(s):</label>
                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: left">
                                    <asp:ListBox ID="lstSupervisorDisponible" runat="server" CssClass="txt_gral" Height="150px" Width="255px"></asp:ListBox>
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="btnDerecha" runat="server" ImageUrl="~/Images/FlechaRojaDer.gif" OnClientClick="AgregarSupervisor(); return false;" />
                                    <br />
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/FlechaRojaIzq.gif" OnClientClick="QuitarSupervisor(); return false;" />
                                </td>
                                <td style="text-align: left">
                                    <asp:ListBox ID="lstSupervisorSeleccionado" runat="server" CssClass="txt_gral" onclick="GetSupervisores();" Height="150px" Width="255px"></asp:ListBox>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <div class="ui-dialog-buttonset" style="text-align: center">
                <button type="button" onclick="Aceptar();" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false"><span class="ui-button-text">Aceptar</span></button>
                <button type="button" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false" onclick="Cerrar();"><span class="ui-button-text">Cancelar</span></button>
            </div>

            <asp:Button ID="btnGuardar" runat="server" Style="display: none" />

            <asp:HiddenField ID="N_Folio" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnUsuario" runat="server" ClientIDMode="Static" />
        </form>
    </div>
</body>
</html>
