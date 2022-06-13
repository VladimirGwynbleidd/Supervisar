<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Supervisor.ascx.vb" Inherits="SEPRIS.Supervisor1" %>

<style type="text/css">
    .auto-style1 {
        height: 26px;
    }
</style>

<script type="text/javascript">
    var SubEntidad;
    var Supervisor;

    $(document).ready(function () {
   
        $("#<%=ddlProcesoP2.ClientID%>").change(function () {
            $("#<%=lstSupervisorDisponible.ClientID%>").empty();
            $("#<%=lstSupervisorSeleccionado.ClientID%>").empty();


            $.ajax({
                type: "POST",
                url: "Paso1.aspx/ObtenerSubprocesos",
                data: '{Proceso: ' + $("#<%=ddlProcesoP2.ClientID%>").val() + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#<%=ddlSubprocesoP2.ClientID%>").html("").append('<option value="0">--Seleccionar--</option>');
                    $("#<%=lstSupervisorDisponible.ClientID%>").html("")
                    $.each(data.d, function () {
                        $("#<%=ddlSubprocesoP2.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
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
        
        $("#<%=lstSupervisorDisponible.ClientID%>").empty();
        $("#<%=lstSupervisorSeleccionado.ClientID%>").empty();
        $.ajax({
            type: "POST",
            url: "Paso1.aspx/ObtenerSupervisores",
            data: '{Subproceso: ' + $("#<%=ddlSubprocesoP2.ClientID%>").val() + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#<%=lstSupervisorDisponible.ClientID%>").html("")
                $.each(data.d, function () {
                    $("#<%=lstSupervisorDisponible.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));

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
        SubProcesoValor = $("#<%=ddlSubprocesoP2.ClientID%>").val();
    }

    function ObtenerSubEntidades() {
      
        $("#Tabla_SubEntidades").empty();

        //if ($("#<%=chkSubentidades.ClientID%>").prop('checked')) {
            $.ajax({
                type: "POST",
                url: "Paso1.aspx/ObtenerSubEntidades",
                data: '{Entidad: ' + $("#<%=ddlEntidad.ClientID%>").val() + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    /*
                    $.each(data.d, function () {
                        var nuevafila = "<tr><td><input type='checkbox' name='chkSubEntidades' value='" + this['Value'] + "'onclick='GetSelected();'>" + this['Text'] + "</td></tr>"
                        $("#Tabla_SubEntidades").append(nuevafila)
                    });
                    */

                    var ddlSubEntidad = $("[id*=ddlSubEntidad]");
                    ddlSubEntidad.empty().append(
                        '<option selected="selected" value="0">--Selecciona--</option>'
                    );
                    $.each(data.d, function () {
                        ddlSubEntidad.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
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
        //}
    }

    function ReAsignarSupervisor() {
        GetSupervisores();
        if (Supervisor.length > 0) {
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ReasignarSupervisor",
                data: '{Folio: ' + '<%=Folio%>' +
                      ', Proceso: ' + $("#<%=ddlProcesoP2.ClientID%>").val() +
                      ', SubProceso: ' + $("#<%=ddlSubprocesoP2.ClientID%>").val() +
                      ',  Supervisor: "' + Supervisor + "\"}",
                <%--                    ', Entidad: ' + $("#<%=ddlEntidad.ClientID%>").val() +
                    ', Supervisor: "' + Supervisor + "\"}",--%>
                async:false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                  
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


    function AsignarPC() {

        GetSupervisores();

        $.ajax({
            type: "POST",
            url: "Paso1.aspx/AsignarPC",
            async: false,
            data: '{Folio: ' + '<%=Folio%>' +
            ', TipoEntidad: "' + $("#<%=ddlTipoEntidad.ClientID%>").val() + "," + $("#<%=ddlTipoEntidad.ClientID%> option:selected").text() +
            '",  Entidad: "' + $("#<%=ddlEntidad.ClientID%>").val() + "," + $("#<%=ddlEntidad.ClientID%> option:selected").text() +
            '", NumeroPcInterno: "' + $("#<%=txtNumInterno.ClientID%>").val() +
            '", FechaVencimiento: "' + $("#<%=txtFechaVencimiento.ClientID%>").val() +
            '", Proceso: ' + $("#<%=ddlProcesoP2.ClientID%>").val() +
            ',  SubProceso: ' + $("#<%=ddlSubprocesoP2.ClientID%>").val() +
            ',  Descripcion: "' + $("#<%=txtDescripcion.ClientID%>").val() +
            '", SubEntidad: "' + SubEntidad +
            '", Area: "' + '<%=AbrArea%>' +
            '", IdArea: ' + '<%=IdArea%>' +
            ',  ddlSubEntidad: "' + $("#<%=ddlSubEntidad.ClientID%>").val() + "," + $("#<%=ddlSubEntidad.ClientID%> option:selected").text() +
            '",  Supervisor: "' + Supervisor + "\"}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {


                $('#divTextoRespuesta').html('Se ha asignado correctamente el PC ' + data.d);
                GuardarBitacora(<%=Folio%>, '<%=Usuario%>', 1, 'Se asignaron el/los folios: '+data.d.replace(/<+\/br>/g,","),$("#<%=txtDescripcion.ClientID%>").val() );

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
                            $(location).attr('href', '../BandejaSICOD.aspx');
                        }
                    }
                });
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al asignar el PC');
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


    function AceptarPC() {

        GetSupervisores();

        //', IsSubEntidad: "' + control.prop('checked') +
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/AprobacionPC",
            data: '{Folio: ' + '<%=Folio%> ' + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                GuardarBitacora(<%=Folio%>,"<%=Usuario%>","1","Se acepta el PC y se asignan supervisores.");
                $(location).attr('href', './DetallePC.aspx');
            },

            failure: function () {
                $('#divTextoAlerta').html('Error al guardar el supervisor');
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

        )

    }

    function AgregarSupervisor() {
        if ($("#<%=lstSupervisorDisponible.ClientID%> option:selected").length > 0) {
            $("#<%=lstSupervisorSeleccionado.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstSupervisorDisponible.ClientID%> option:selected").val()).text($("#<%=lstSupervisorDisponible.ClientID%> option:selected").text()));
            $("#<%=lstSupervisorSeleccionado.ClientID%>").val($("#<%=lstSupervisorDisponible.ClientID%>").val());
            $("#<%=lstSupervisorDisponible.ClientID%> option:selected").remove();
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
        if ($("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").length > 0) {
            $("#<%=lstSupervisorDisponible.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").val()).text($("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").text()));
            $("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").remove();

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


    function GetSelected() {
        let ddlSubEntidad = document.getElementById('ddlSubEntidad');
        SubEntidad = new Array();
        SubEntidad.push(ddlSubEntidad.value)
        /*
        $("[name='chkSubEntidades']").each(function (index, data) {
            if (data.checked) {
                SubEntidad.push(data.value);
            }
        });
        */
    }

    function GetSupervisores() {
        Supervisor = new Array();
        $("#<%=lstSupervisorSeleccionado.ClientID%> option").each(function (i, selected) {
            Supervisor.push($(selected).val())

        });
    }

    function validaLimiteDescripcion(obj, maxchar) {
            if (this.id) obj = this;
            var remaningChar = maxchar - obj.value.length;
            //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
            $("#<%=lblDescripcionContador.ClientID%>").text("" + remaningChar);

            if (remaningChar <= 0) {
                //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                $("#<%=lblDescripcionContador.ClientID%>").text("" + 0);
                obj.value = obj.value.substring(maxchar, 0);
                return false;
            }
            else { return true; }
        }


</script>

<asp:Panel ID="pnlEntidad" runat="server">
    <table style="width: 100%">
        <tr>
            <td style="height: 35px; width: 20%; text-align: left;">
                <label class="txt_gral">Tipo Entidad:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
            </td>
            <td style="width: 30%; text-align: left;">
                <asp:DropDownList ID="ddlTipoEntidad" runat="server" Width="150px" CssClass="txt_gral" AutoPostBack="True"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipoEntidad" ErrorMessage="Seleccione un tipo entidad" ForeColor="Red" ValidationGroup="ucSupervisor" InitialValue="-1">&nbsp;</asp:RequiredFieldValidator>
            </td>
            <td style="height: 35px; width: 20%; text-align: left;"></td>
            <td style="width: 30%; text-align: left;"></td>
        </tr>
        <tr>
            <td style="height: 35px; width: 20%; text-align: left;">
                <label class="txt_gral">Entidad:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
            </td>
            <td style="width: 30%; text-align: left;">
                <asp:DropDownList ID="ddlEntidad" runat="server" Width="150px" CssClass="txt_gral" Enabled="false"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEntidad" ErrorMessage="Seleccione una entidad" ForeColor="Red" ValidationGroup="ucSupervisor" InitialValue="-1">&nbsp;</asp:RequiredFieldValidator>
            </td>
            <td style="height: 35px; width: 20%; text-align: left;"></td>
            <td style="width: 30%; text-align: left;"></td>
        </tr>
        <!--
        <tr id="trSubentidadesCheck" runat="server" visible="false">
            <td style="text-align: left;" class="auto-style1">
                <label class="txt_gral">Agregar subentidades</label>
            </td>
            <td colspan="3" class="auto-style1">
                <asp:CheckBox ID="chkSubentidades" runat="server" CssClass="txt_gral" Text="" TextAlign="Right"></asp:CheckBox>
            </td>
        </tr>
        -->
        <tr id="trSubentidadesList" runat="server" visible="false">
            <td style="height: 35px; text-align: left;">
                <label class="txt_gral">Sub entidad:</label>
            </td>
            <td>

                <div class="txt_gral">
                    <table>
                        <asp:Literal ID="ltl_SubEntidad" runat="server"></asp:Literal>
                    </table>

                </div>
                <%--<label class="txt_gral" id ="lbl_SubEntidad" runat ="server"></label>--%>
                <!--
                <div class="txt_gral">
                    <table id="Tabla_SubEntidades" style="width: 100%"></table>
                </div>
                -->
                <asp:DropDownList ID="ddlSubEntidad" runat="server" Width="150px" CssClass="txt_gral"></asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td style="height: 35px; text-align: left;">
                <label class="txt_gral">Núm.  interno de PC:</label>
            </td>
            <td>
                <asp:TextBox ID="txtNumInterno" runat="server" Width="150px" CssClass="txt_gral" MaxLength="50"></asp:TextBox>
            </td>
            <td>
                <label class="txt_gral">Fecha de Vencimiento:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
            </td>
            <td>
                <asp:TextBox ID="txtFechaVencimiento" runat="server" Width="150px" CssClass="txt_gral" Enabled="False"></asp:TextBox>
            </td>

        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnlAgregarSupervisor" runat="server">
    <table style="width: 100%">
        <tr>
            <td style="height: 35px; width: 20%; text-align: left;">
                <label class="txt_gral">Proceso:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlProcesoP2" ErrorMessage="Seleciona un proceso" ForeColor="Red" ValidationGroup="ucSupervisor" InitialValue="-1"> &nbsp;</asp:RequiredFieldValidator>
            </td>
            <td style="height: 35px; width: 30%; text-align: left;">
                <asp:DropDownList ID="ddlProcesoP2" runat="server" Width="150px" CssClass="txt_gral"></asp:DropDownList>



            </td>
            <td style="height: 35px; width: 20%; text-align: left;">
                <label class="txt_gral">Subproceso:</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlSubprocesoP2" ErrorMessage="Seleccione un subproceso" ForeColor="Red" ValidationGroup="ucSupervisor">&nbsp;</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:DropDownList ID="ddlSubprocesoP2" runat="server" Width="150px" CssClass="txt_gral ddlSubproceso"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="height: 35px; text-align: left;">
                <label class="txt_gral">Supervisor (es):</label></td>
            <td></td>
            <td></td>
            <td></td>
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
                            <label class="txt_gral">Seleccionado(s):</label><samp style="color: red; font-size: 1.3em"><b />&nbsp;*<b /></samp>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="lstSupervisorSeleccionado" ErrorMessage="Seleccione un supervisor" ForeColor="Red" ValidationGroup="ucSupervisor">&nbsp;</asp:RequiredFieldValidator>
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
</asp:Panel>

<asp:Panel ID="pnlDescripcion" runat="server">
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="height: 15px;"></td>
        </tr>
        <tr>
            <td style="height: 35px; width: 20%; text-align: left;">
                <label class="txt_gral">Descripción:</label><samp style="color: red; font-size: 1.3em" ><b />&nbsp;*<b /></samp>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="Ingrese una descripción" ForeColor="Red" ValidationGroup="ucSupervisor" Visible="true">&nbsp;</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="txtDescripcion" runat="server" onkeyup="validaLimiteDescripcion(this,500)" TextMode="MultiLine" CssClass="txt_gral" Height="80px" Width="800px" MaxLength="500" ></asp:TextBox>
           </td>
            <td></td>
            <td></td>
            <td></td>
       </tr>
        <tr>
            <td class="auto-style1">
            </td>
            <td colspan="2" class="auto-style1">
                <asp:Label runat="server" ID="lblCarDescripcion" CssClass="txt_gral" Text="Caracteres restantes: " Font-Size="Small"></asp:Label>
                <asp:Label runat="server" ID="lblDescripcionContador" CssClass="txt_gral" Text="500" Font-Size="Small"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
