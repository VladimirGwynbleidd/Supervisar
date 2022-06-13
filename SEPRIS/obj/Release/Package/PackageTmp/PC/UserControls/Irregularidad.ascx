<%@ Control Language="vb" ClassName="Irregularidad" AutoEventWireup="false" CodeBehind="Irregularidad.ascx.vb" Inherits="SEPRIS.Irregularidad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<link href="../Styles/Site.css" rel="stylesheet" />

<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <div id="divAltaIrregularidades">
            <table style="width: 100%;">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFecIrregularidad" ErrorMessage="Seleccione fecha de irregularidad" ForeColor="Red" ValidationGroup="ucIrregularidad">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LblNumIrregularidad" runat="server" Text="Fecha de irregularidad*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtFecIrregularidad" runat="server" Width="150px" CssClass="txt_gral"></asp:TextBox>

                        <ajx:CalendarExtender ID="calExFechaIrregularidad" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCal"
                            TargetControlID="txtFecIrregularidad" CssClass="teamCalendar" />
                        <asp:Image ImageAlign="Bottom" ID="imgCal" runat="server" ImageUrl="~/imagenes/Calendar.gif" />

                    </td>
                </tr>
                <tr>
                    <td><%--<a href="../../Master/">../../Master/</a>--%>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlProceso" ErrorMessage="Seleccione un proceso" ForeColor="Red" ValidationGroup="ucIrregularidad">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LblProceso" runat="server" Text="Proceso*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="ddlProceso" runat="server" CssClass="txt_gral" Width="200px" onchange="ObtenerListaSubprocesos()"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSubproceso" ErrorMessage="Seleccione un subproceso" ForeColor="Red" ValidationGroup="ucIrregularidad">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LblSubProceso" runat="server" Text="Subproceso*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="ddlSubproceso" runat="server" CssClass="txt_gral" Width="200px" onchange="ObtenerListaConductaSaludable()"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlConducta" ErrorMessage="Seleccione una conducta sancionable" ForeColor="Red" ValidationGroup="ucIrregularidad">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LblConducta" runat="server" Text="Conducta sancionable*" CssClass="txt_gral"></asp:Label>


                    </td>
                    <td>
                        <asp:DropDownList ID="ddlConducta" runat="server" CssClass="txt_gral" Width="400px" onchange=" ObtenerListaIrregularidades()"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlIrregularidad" ErrorMessage="Seleccione una irregularidad" ForeColor="Red" ValidationGroup="ucIrregularidad">*</asp:RequiredFieldValidator>
                        <asp:Label ID="Label1" runat="server" Text="Irregularidad*" CssClass="txt_gral"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIrregularidad" runat="server" CssClass="txt_gral" Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TxtComentarios" ErrorMessage="Ingrese los comentarios" ForeColor="Red" ValidationGroup="ucIrregularidad">*</asp:RequiredFieldValidator>
                        <asp:Label ID="LblComentarios" runat="server" Text="Comentarios*" CssClass="txt_gral"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtComentarios" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                            TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:Panel ID="pnlBotones" runat="server">
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Irregularidad" Style="height: 26px; width: 120px" OnClientClick="GuardarInfoIrregularidad(); return false;" />
        </asp:Panel>
    </ContentTemplate>

    <Triggers>
    </Triggers>
</asp:UpdatePanel>

<script type="text/javascript">
    $("#<%=ddlConducta.ClientID()%>").hover(
          function () {
              $(this).attr("title", "");
              $(this).attr("title", $(this).find('option:selected').text());
          }
        );

    $("#<%=ddlIrregularidad.ClientID()%>").hover(
          function () {
              $(this).attr("title", "");
              $(this).attr("title", $(this).find('option:selected').text());
          }
        );

    $(function () {


        MensajeUnBotonNoAccionLoad();
        MensajeUnBotonUnaAccionLoad();
        MensajeDosBotonesUnaAccionLoad();
    });

    function limpia() {
        $("#<%=txtFecIrregularidad.ClientID%>").val('')
        $("#<%=ddlSubproceso.ClientID%>").empty();
        $("#<%=ddlConducta.ClientID%>").empty();
        $("#<%=ddlIrregularidad.ClientID%>").empty();
        $("#<%=TxtComentarios.ClientID%>").val("");
    }

    function CargaProcesosIrregularidad() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerProcesos",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
 
                $("#<%=ddlProceso.ClientID%>").html("").append('<option value="0">--Selecciona un Proceso--</option>');
                $.each(data.d, function () {
                    $("#<%=ddlProceso.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                });
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar los procesos');
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

    function ObtenerListaSubprocesos() {
         
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerSubprocesos",
            data: '{Proceso: ' + $("#<%=ddlProceso.ClientID%>").val() + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $("#<%=ddlSubproceso.ClientID%>").html("").append('<option value="0">--Selecciona un Subproceso--</option>');
                $.each(data.d, function () {
                    $("#<%=ddlSubproceso.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
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
    }

    function ObtenerListaSubprocesosModificar(Valor) {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerSubprocesos",
            data: '{Proceso: ' + Valor + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#<%=ddlSubproceso.ClientID%>").html("").append('<option value="0">--Selecciona un Subproceso--</option>');
                $.each(data.d, function () {
                    $("#<%=ddlSubproceso.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
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
    }
    
    
    function ObtenerListaConductaSaludable() {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerConducta",
            data: '{Proceso: ' + $("#<%=ddlProceso.ClientID%>").val() + ', SubProceso: ' + $("#<%=ddlSubproceso.ClientID()%>").val() + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $("#<%=ddlConducta.ClientID%>").html("").append('<option value="0">- Seleccionar -</option>');
                //     $("#<%'=lstSupervisorDisponible.ClientID%>").html("")
                $.each(data.d, function () {
                    $("#<%=ddlConducta.ClientID()%>").append($("<option title='" + this['Text'] + "'></option>").attr("value", this['Value']).text(this['Text']));
                });
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar las conductas sancionables');
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

    function ObtenerListaConductaSaludableModificar(Valor) {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerConducta",
            data: '{Proceso: ' + Valor + ', SubProceso: ' + $("#<%=ddlSubproceso.ClientID()%>").val() + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#<%=ddlConducta.ClientID%>").html("").append('<option value="0">- Seleccionar -</option>');
                //     $("#<%'=lstSupervisorDisponible.ClientID%>").html("")
                $.each(data.d, function () {
                    $("#<%=ddlConducta.ClientID()%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                });
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar las conductas sancionables');
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

    function ObtenerListaIrregularidades() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerIrregularidadesSISAN",
            data: '{Proceso: ' + $("#<%=ddlProceso.ClientID%>").val() + ', SubProceso: ' + $("#<%=ddlSubproceso.ClientID()%>").val() + ', Conducta: ' + $("#<%=ddlConducta.ClientID()%>").val() + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $("#<%=ddlIrregularidad.ClientID%>").empty();
                $("#<%=ddlIrregularidad.ClientID%>").html("").append('<option value="0">- Seleccionar -</option>');
                $.each(data.d, function () {
                    $("#<%=ddlIrregularidad.ClientID()%>").append($("<option title='" + this['Text'] + "'></option>").attr("value", this['Value']).text(this['Text']));
                });
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al cargar las conductas sancionables');
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

    function GuardarIrregularidad() {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardarIrregularidad",
            data: '{Folio: ' + '<%=Folio%>' +
                ', Fecha: "' + $("#<%=txtFecIrregularidad.ClientID%>").val() +
                '", Proceso: "' + $("#<%=ddlProceso.ClientID%>").val() +
                '", SubProceso: "' + $("#<%=ddlSubproceso.ClientID%>").val() +
                '", Conducta: "' + $("#<%=ddlConducta.ClientID%>").val() +
                '", Irregularidad: "' + $("#<%=ddlIrregularidad.ClientID%>").val() + '", Comentarios: "' + $("#<%=TxtComentarios.ClientID%>").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#<%=ddlSubproceso.ClientID%>").empty();
                $("#<%=ddlConducta.ClientID%>").empty();
                $("#<%=ddlIrregularidad.ClientID%>").empty();
                $("#<%=TxtComentarios.ClientID%>").val("");
                $(location).attr('href', './DetallePC.aspx');


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
    }


    function ActualizarIrregularidad(IdIrregularidad) {
    
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ActualizarIrregularidad",
            async: false,
            data: '{Folio: ' + '<%=Folio%>' +
                ', IdIrregularidad: ' + IdIrregularidad +
                ', Fecha: "' + $("#<%=txtFecIrregularidad.ClientID%>").val() +
                '", Proceso: "' + $("#<%=ddlProceso.ClientID%>").val() +
                '", SubProceso: "' + $("#<%=ddlSubproceso.ClientID%>").val() +
                '", Conducta: "' + $("#<%=ddlConducta.ClientID%>").val() +
                '", Irregularidad: "' + $("#<%=ddlIrregularidad.ClientID%>").val() + '", Comentarios: "' + $("#<%=TxtComentarios.ClientID%>").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                BindIrregularidades();
                $("#<%=ddlSubproceso.ClientID%>").empty();
                $("#<%=ddlConducta.ClientID%>").empty();
                $("#<%=ddlIrregularidad.ClientID%>").empty();
                $("#<%=TxtComentarios.ClientID%>").val("");
                $("#divAgregarIrregularidad").dialog("close");
                $(location).attr('href', './DetallePC.aspx');
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
    }

    function BindIrregularidades() {

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerIrregularidades",
            contentType: "application/json;charset=utf-8",
            data: '{Folio: ' + '<%=Folio%>' + '}',
            dataType: "json",
            async: false,
            success: function (data) {
                $('#tblIrregularidades').empty();
                $('#tblIrregularidades').append('<thead class="GridViewEncabezadoTbl"><tr><th colspan="11">Irregularidades identificadas:</th></tr></thead><tr class="GridViewEncabezado"><th>No.</th><th>Fecha</th><th>Proceso</th><th>Subproceso</th><th>Conducta sancionable</th><th>Irregularidad</th><th>Participante</th><th>Gravedad</th><th>Comentarios</th><th></th><th></th></tr>');
                $.map(data.d, function (GridView) {
                    
                    $('#tblIrregularidades').append('<tr id="' + GridView.IdIrregularidad + '" class="GridViewContenidoAlternate" ><td>' + GridView.Numero + '</td><td>' + GridView.Fecha + '</td><td>' + GridView.Proceso + '</td><td>' + GridView.Subproceso + '</td><td>' + GridView.Conducta + '</td><td>' + GridView.Irregularidad + '</td><td>' + GridView.Participante + '</td><td>' + GridView.Gravedad + '</td><td>' + GridView.Comentarios + '</td><td><input type="image" src="../../Imagenes/icono_lapiz.gif" title="Editar irregularidad" style="width:20px;" onclick="MostrarModificaIrregularidad(' + GridView.IdIrregularidad + '); return false;" /><td><input type="image" src="../../Imagenes/icono_corregir.jpg" title="Eliminar irregularidad" style="width:20px;" onclick="QuestionEliminar(' + GridView.IdIrregularidad + '); return false;" />' + '</td></tr>');
                });
            },
            error: function (result) {
            }
        });
    }


    function GuardarInfoIrregularidad() {



        if (Page_ClientValidate("ucIrregularidad")) {

            $('#divTextoRespuesta').text('¿Estas seguro que deseas agregar la irregularidad?');

            $("#divMensajeRespuesta").dialog({
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
                        GuardarIrregularidad();
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });


        }
        else {

            $('#divTextoRespuesta').text('Es necesario completar la  información necesaria para la irregularidad. Por favor completa los siguientes datos:');

            $("#divMensajeRespuesta").dialog({
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
                        $(this).dialog("close");
                    }
                }
            });
        }



    }

    function CerrarAlta() {
        window.close();
    }


    
</script>

