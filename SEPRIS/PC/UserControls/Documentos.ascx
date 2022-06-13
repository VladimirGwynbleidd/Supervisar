<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Documentos.ascx.vb" Inherits="SEPRIS.Documentos" %>



<asp:GridView ID="gvConsultaDocs" runat="server" RowStyle-Height="25px" RowStyle-HorizontalAlign="Left" AutoGenerateColumns="false"
    CssClass="anchoGriDocs" DataKeyNames="I_ID_DOCUMENTO">
    <Columns>
        <asp:BoundField HeaderText="PASO" DataField="I_PASO_INICIAL" ItemStyle-Width="33px" />
        <asp:BoundField HeaderText="DOCUMENTO" DataField="T_NOM_DOCUMENTO" ItemStyle-Width="40%" />
        <asp:TemplateField HeaderText="DOCUMENTOS ADJUNTOS">
            <ItemTemplate>
                <asp:FileUpload ID="filead" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="btnAdjuntarDocumento" runat="server" ImageUrl="~/Imagenes/adjuntarDocs2.png" Width="25px"  CausesValidation="false" CommandName="Adjuntar" OnClientClick='<%# "Adjuntar(" + DataBinder.Eval(Container, "RowIndex", "{0}") + "); return false;" %>' ToolTip="Adjuntar Documento" />
                <asp:ImageButton ID="btnMas_" runat="server" ImageUrl="~/Imagenes/masDocumentos.png" Width="25px" CausesValidation="false" OnClientClick="alert('Agregar nuevo fileupload');" />
                <asp:ImageButton ID="btnBuscarSICOD" runat="server" ImageUrl="~/Imagenes/detalleReporte.png" Width="25px" CausesValidation="false" OnClientClick="LevantaVentanaOficio(); return false;" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SICOD">
            <ItemTemplate>
                <asp:Button ID="btnRegitrar" runat="server" Text="Registar" OnClientClick='<%# "RegistroSICOD(" + Eval("I_ID_DOCUMENTO").ToString() + "); return false;" %>' Visible="false" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Núm. Oficio SICOD">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="B_BUSCAR_SICOD" Visible="false" />
        <asp:BoundField DataField="B_REG_SICOD" Visible="false" />
                <asp:BoundField DataField="I_ID_DOCUMENTO" Visible="false"/>

    </Columns>
</asp:GridView>



<div id="divRegistroSICOD" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    <div class="txt_gral">

                        <asp:Panel ID="PanelDatosSicod" runat="server" CssClass="PanelErrores1" Width="550px">
                            <table cellpadding="0" cellspacing="0" style="width: 100%; border-style: solid; border-width: 2px; border-color: White">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <script type="text/javascript">

                                                    function setACEValueKey(source, eventArgs) {
                                                        document.getElementById('<%= destinatarioKey.ClientID %>').value = eventArgs.get_value();
                                                        document.getElementById('<%= destinatarioText.ClientID %>').value = document.getElementById("<%= txtDestinatarioOficio.ClientID %>").value;
                                                    }

                                                </script>
                                                <asp:HiddenField runat="server" ID="hdnTipoOficio" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdnEditaOficio" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdnIdOficio" Value="0" />
                                                <table class="style163">
                                                    <tr id="trFecha" runat="server" visible="false">
                                                        <td class="style165">
                                                            <asp:Label ID="Label22" runat="server" CssClass="txt_gral" Text="Fecha del Oficio"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFechaOficio" runat="server" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label17" runat="server" CssClass="txt_gral" Text="Asunto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAsuntoOficio" runat="server" CssClass="txt_gral" Width="410px"
                                                                MaxLength="255"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblComentariosOficio" Text="Comentarios" CssClass="txt_gral"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtComentariosOficio" runat="server" CssClass="txt_gral" Width="410px"
                                                                MaxLength="255"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label18" runat="server" CssClass="txt_gral" Text="Destinatario"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDestinatarioOficio" runat="server" CssClass="txt_gral" Width="410px"
                                                                MaxLength="50"></asp:TextBox>
                                                            <input id="destinatarioKey" name="destinatarioKey" runat="server" type="hidden" value="0" />
                                                            <input id="destinatarioText" name="destinatarioText" runat="server" type="hidden"
                                                                value="" />
                                                            <asp:HiddenField runat="server" ID="hdnIdEntidad" Value="0" />
                                                            <asp:HiddenField runat="server" ID="hdnIdTipoEntidad" Value="0" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label19" runat="server" CssClass="txt_gral" Text="Puesto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPuestoDestinatario" runat="server" CssClass="txt_gral" Width="410px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label20" runat="server" CssClass="txt_gral" Text="Firmas"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:DropDownList ID="ddlAreaFirma" runat="server" CssClass="txt_gral" Width="410px"
                                                                            onchange="ObtenerFirmas(1);">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 45%">
                                                                        <asp:Label ID="lblFiltroSIE" runat="server" CssClass="txt_gral" Text="Con firma SIE:"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:CheckBox ID="chkFiltroSIE" runat="server" Checked="true" CssClass="txt_gral" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 45%">
                                                                        <asp:Label ID="lblUsuariosRub" runat="server" CssClass="txt_gral" Text="Disponibles:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10%">&nbsp;
                                                                    </td>
                                                                    <td style="width: 45%">&nbsp;
                                                            <asp:Label ID="lblRubrica" runat="server" CssClass="txt_gral" Text="Seleccionados:"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" rowspan="2" style="text-align: right" valign="top">
                                                                        <asp:ListBox ID="lstUsuariosFirma" runat="server" CssClass="txt_gral" Height="68px"
                                                                            TabIndex="26" Width="100%"></asp:ListBox>
                                                                    </td>
                                                                    <td style="text-align: center">

                                                                        <img id="imgAgregarFirma" src="../../Imagenes/FlechaRojaDer.gif" style="height: 30px" onclick="AgregarFirma();" />
                                                                        <br />
                                                                        <img id="imgQuitarFirma" src="../../Imagenes/FlechaRojaIzq.gif" style="height: 30px" onclick="QuitarFirma();" />
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        <asp:ListBox ID="lstFirmas" runat="server" CssClass="txt_gral" Height="68px" Width="100%"></asp:ListBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center">
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label21" runat="server" CssClass="txt_gral" Text="Rubricas"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:DropDownList ID="ddlAreaRubrica" runat="server" CssClass="txt_gral" Width="410px"
                                                                            onchange="ObtenerFirmas(2);">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 45%">
                                                                        <asp:Label ID="Label8" runat="server" CssClass="txt_gral" Text="Disponibles:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10%">&nbsp;
                                                                    </td>
                                                                    <td style="width: 45%">&nbsp;
                                                            <asp:Label ID="Label10" runat="server" CssClass="txt_gral" Text="Seleccionados:"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" rowspan="2" style="text-align: right" valign="top">
                                                                        <asp:ListBox ID="lstUsuariosRubrica" runat="server" CssClass="txt_gral" Height="68px" TabIndex="26" Width="100%"></asp:ListBox>
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <img id="imgAgregarRubrica" src="../../Imagenes/FlechaRojaDer.gif" style="height: 30px" onclick="AgregarRubrica();" />
                                                                        <br />
                                                                        <img id="imgQuitarRubrica" src="../../Imagenes/FlechaRojaIzq.gif" style="height: 30px" onclick="QuitarRubrica();" />
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        <asp:ListBox ID="lstRubricas" runat="server" CssClass="txt_gral" Height="68px" Width="100%"></asp:ListBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center">&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trTema" runat="server" visible="false">
                                                        <td class="style165">
                                                            <asp:Label ID="Label23" runat="server" CssClass="txt_gral" Text="Tipo de Oficio"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTema" runat="server" CssClass="txt_gral" Width="410px" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trPlantillas" runat="server" visible="false">
                                                        <td class="style165">
                                                            <asp:Label ID="Label15" runat="server" CssClass="txt_gral" Text="Plantilla"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPlantilla" runat="server" CssClass="txt_gral" Width="410px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>


<div id="divConfirmacion" style="display: none">
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    <div id="divTextoConfirmacion"></div>
                </div>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">

    function ConsultarOficios(pFolioSICOD) {

        if (pFolioSICOD == 'Empty')
        {
            pFolioSICOD = $("#<%=hfFolioSICOD.ClientID%>").val();
        }


        var url = 'VerOficios.aspx?folio=' + pFolioSICOD
        var winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
       
        new_window = window.open(url, "SICOD", winprops);

        new_window.onbeforeunload = function () {
            var pnlupd = window.parent.$("div[id*='upnlReqInfo']")
            if (pnlupd.length > 0) {
                pnlupd = pnlupd[0].id
                window.parent.__doPostBack(pnlupd, '');
            }
        }
    }


    function LevantaVentanaOficio() {




        //Levanta la modal
        var url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';
        var winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
        window.showModalDialog(url, "SICOD", winprops);


    }

    function ObtenerFirmas(Origen) {
        //alert('{Area: ' + $("#<%=ddlAreaFirma.ClientID%>").val() + ', EsSIE: ' + $("#<%=chkFiltroSIE.ClientID%>").val() + ' }');

        var EsSIE = 0;
        if ($("#<%=chkFiltroSIE.ClientID%>").prop('checked')) {
            EsSIE = 1;
        }

        if (Origen == 1) {
            param_data = '{Area: ' + $("#<%=ddlAreaFirma.ClientID%>").val() + ', EsSIE: ' + EsSIE + ' }'
        }
        else {
            param_data = '{Area: ' + $("#<%=ddlAreaRubrica.ClientID%>").val() + ', EsSIE: ' + EsSIE + ' }'
        }



        $.ajax({
            type: "POST",
            url: "ServiciosSICOD.aspx/ObtenerUsuariosParaFirma",
            contentType: "application/json;charset=utf-8",
            data: param_data,
            dataType: "json",
            success: function (data) {

                if (Origen == 1) {
                    $("#<%=lstUsuariosFirma.ClientID%>").html("")
                    $("#<%=lstFirmas.ClientID%>").html("")
                    $.each(data.d, function () {
                        $("#<%=lstUsuariosFirma.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                    });
                }
                else {
                    $("#<%=lstUsuariosRubrica.ClientID%>").html("")
                    $("#<%=lstRubricas.ClientID%>").html("")
                    $.each(data.d, function () {
                        $("#<%=lstUsuariosRubrica.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                    });
                }


            },
            error: function (result) {
                return result
            }
        });

    }

    function ConfirmacionAltaSICOD(Respuesta) {

        if (Respuesta == 0)
            $('#divTextoConfirmacion').text("Se cancelará  el registro del documento en SICOD, ¿Deseas continua?");
        else
            $('#divTextoConfirmacion').text("Se realizará el registro del documento en SICOD, ¿Deseas continua?");

        $("#divConfirmacion").dialog({
            resizable: false,
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
                    if (Respuesta == 0) {
                        $(this).dialog("close");
                        $("#divRegistroSICOD").dialog("close");
                    }
                    else {
                        CrearFolio();
                        ConsultarOficios();
                        $(this).dialog("close");
                        $("#divRegistroSICOD").dialog("close");
                    }
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });

    }

    function CrearFolio() {

        var Firmas = '';
        var Rubricas = '';
        var EsSIE = 0;
        $("#<%=lstFirmas.ClientID%> option").each(function (i, selected) {
            Firmas += $(selected).val() + ',';
        });
        $("#<%=lstRubricas.ClientID%> option").each(function (i, selected) {
            Rubricas += $(selected).val() + ',';
        });

        if ($("#<%=chkFiltroSIE.ClientID%>").prop('checked')) {
            EsSIE = 1;
        }

        param_data = '{Folio:' + <%=FolioSICOD%> + ', IdDocumento: ' + $("#<%=hfRowIndex.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: ' + '<%=Area%>' + ' }'; //MMOB - ANTES "Folio" AHORA "FolioSICOD"

        $.ajax({
            type: "POST",
            url: "ServiciosSICOD.aspx/RegistarOficio",
            contentType: "application/json;charset=utf-8",
            data: param_data,
            dataType: "json",
            success: function (data) {
                $("#<%=hfFolioSICOD.ClientID%>").val(data.d);
                
            },
            error: function (result) {
                return result
            }
        });

    }

    function RegistroSICOD(IdDocumento) {

        $("#<%=hfRowIndex.ClientID%>").val(IdDocumento);

        $("#divRegistroSICOD").dialog({
            resizable: false,
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
                    ConfirmacionAltaSICOD(1);

                },
                "Cancelar": function () {
                    ConfirmacionAltaSICOD(0);
                }
            }
        });

    }

    function AgregarFirma() {
        if ($("#<%=lstUsuariosFirma.ClientID%> option:selected").length > 0) {
            $("#<%=lstFirmas.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstUsuariosFirma.ClientID%>").val()).text($("#<%=lstUsuariosFirma.ClientID%>").text()));
            $("#<%=lstFirmas.ClientID%>").val($("#<%=lstUsuariosFirma.ClientID%>").val());
            $("#<%=lstUsuariosFirma.ClientID%> option:selected").remove();
        }
        else {
            $('#divTextoAlerta').html('Selecciona una firma');
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

        return false;
    }

    function QuitarFirma() {
        if ($("#<%=lstFirmas.ClientID%> option:selected").length > 0) {
            $("#<%=lstUsuariosFirma.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstFirmas.ClientID%>").val()).text($("#<%=lstFirmas.ClientID%>").text()));
            $("#<%=lstFirmas.ClientID%> option:selected").remove();

        }
        else {
            $('#divTextoAlerta').html('Selecciona una firma');
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

        return false;
    }

    function AgregarRubrica() {
        if ($("#<%=lstUsuariosRubrica.ClientID%> option:selected").length > 0) {
            $("#<%=lstRubricas.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstUsuariosRubrica.ClientID%>").val()).text($("#<%=lstUsuariosRubrica.ClientID%>").text()));
            $("#<%=lstRubricas.ClientID%>").val($("#<%=lstUsuariosRubrica.ClientID%>").val());
            $("#<%=lstUsuariosRubrica.ClientID%> option:selected").remove();
        }
        else {
            $('#divTextoAlerta').html('Selecciona una rubrica');
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

        return false;
    }

    function QuitarRubrica() {
        if ($("#<%=lstRubricas.ClientID%> option:selected").length > 0) {
            $("#<%=lstUsuariosRubrica.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstRubricas.ClientID%>").val()).text($("#<%=lstRubricas.ClientID%>").text()));
            $("#<%=lstRubricas.ClientID%> option:selected").remove();

        }
        else {
            $('#divTextoAlerta').html('Selecciona una rubrica');
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

        return false;
    }

    function Adjuntar(RowIndex) {
        $("#<%=hfRowIndex.ClientID%>").val(RowIndex);
        $("#<%=Button1.ClientID%>").click();
    }

</script>
<asp:Button ID="Button1" runat="server" Text="Button" CausesValidation="false" Style="display: none" />
<asp:Button ID="Button2" runat="server" Text="Button" CausesValidation="false" Style="display: none" />
<asp:Button ID="Button3" runat="server" Text="Button" CausesValidation="false" Style="display: none" />
<asp:HiddenField ID="hfRowIndex" runat="server" Value="0" />
<asp:HiddenField ID="hfFolioSICOD" runat="server" Value="" />
