<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDocumentos.ascx.vb" Inherits="SEPRIS.ucDocumentos" %>
<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>

<script type="text/javascript">
    //Detectar navegador
    var nav = navigator.userAgent.toLowerCase();

    //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
    if (nav.indexOf("msie") != -1) {
        document.write("<link href='../Styles/estilosIE.css' rel='stylesheet' />");
    } else {
        document.write("<link href='../Styles/estilosChrome.css' rel='stylesheet' />");
    }

    function GuardaScrollDocs(psIdDiv, psIdCajaOculta) {
        var elemento = $('#' + psIdDiv);

        if (elemento != null) {
            var pos = $('#' + psIdDiv).scrollTop();
            $('#' + psIdCajaOculta).val(pos);
        }
    }

    function PreguntaNumRequerimiento() {
        OcultaImagenCarga();
        MensajeDosBotonesUnaAccionMCS("divPreguntaNumRequerimiento", "btnAceptarNumReq", 500, 250);
    }

    function SetScroll(psIdDiv, psIdCajaOculta) {
        //Restablecer la posicion del scroll del div de documentos
        var divdocs = $('#' + psIdDiv);

        if (divdocs != null) {
            var hf = $('#' + psIdCajaOculta);
            if (hf != null) {
                divdocs.scrollTop(hf.val());
            }
        }
    }

    //$(document).ready(function () {
    //    SetScroll();
    //});

    function HabilitaLink(idBtnImg, idBtnLink, idBtnFu, idBtnAdj, idBtnBandeja, idBtnSicod) {
        //alert(idBtnLink + ", " + idBtnFu + ", " + idBtnAdj);
        //var btnLink = document.getElementById(idBtnLink);
        $("#" + idBtnImg).hide();
        $("#" + idBtnLink).hide();
        $("#" + idBtnFu).removeClass("OcultarControl");
        $("#" + idBtnAdj).removeClass("OcultarControl");

        if (idBtnSicod != "")
            $("#" + idBtnSicod).removeClass("OcultarControl");

        $("#" + idBtnBandeja).removeClass("OcultarControl");

        return false;
    }


    function LevantaVentanaOficioNvo(idRen, idDocumento, idCatDocumento, idVersion, idNumVersiones, idVisita, idArea, tipo, paso) {
        //MuestraImgCarga(null);
        var url = '';
        //Guarda el id del documento en un campo oculto
        if (idVisita != "-1") {
            $("#hfIdRenglon_" + idVisita).val(idRen);
            $("#hfIdDocSicod_" + idVisita).val(idDocumento);
            $("#hfIdVerDocSicod_" + idVisita).val(idVersion);
            $("#hfNumVerDocSicod_" + idVisita).val(idNumVersiones);
            $("#hfVisita_" + idVisita).val(idVisita);
        } else {
            $("#hfIdRenglon").val(idRen);
            $("#hfIdDocSicod").val(idDocumento);
            $("#hfIdVerDocSicod").val(idVersion);
            $("#hfNumVerDocSicod").val(idNumVersiones);
            $("#hfVisita").val(idVisita);
        }
        //Levanta la modal
        url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';

        //Detectar navegador
        var nav = navigator.userAgent.toLowerCase();

        //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
        if (nav.indexOf("msie") != -1) {
            winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
            window.showModalDialog(url, "SICOD", winprops);
            OcultaImagenCarga();
            return true;
        } else {
            var liTop = (parseInt($(window).height()) / 2) - (450 / 2);
            var liLeft = (parseInt($(window).width()) / 2) - (850 / 2);

            winprops = "width=850,height=450,toolbar=no, menubar=no, scrollbars=no, resizable=Yes, location=no, status=no, left=" + liLeft + ", top=" + liTop;
            //alert("winprops: " + winprops);
            //var modal = window.open(url + "?r=" + idRen + "&ds=" + idDocumento + "&vds=" + idVersion, "SICOD", winprops);
            var modal = window.open(url + "?r=" + idRen + "&ds=" + idDocumento + "&idCat=" + idCatDocumento + "&idArea=" + idArea + "&tipo=" + tipo + "&vds=" + idVersion + "&idFolio=" + idVisita + "&paso=" + paso + "&origen=VISITA", "SICOD", winprops);

            modal.focus();
            OcultaImagenCarga();
            return false;
        }
    }

    function llamaPostback() {
        __doPostBack('', '');
    }

    function RegistroSICOD(IdCatDocumento, IdDocumento, idFolio, clasificacion, idTipoOficio, idPaso) {

        var divs = $("#divRegistroSICOD").toArray().length;
        if (divs == 2) {
            $("#divRegistroSICOD").remove();
        }

        var suffix = "MainContent_tcContPestaDocs_tpPestaniasDocsP333_ucDocumentos_333_ddlPuestoDestinatario";
        var suffix = suffix.replace(/[^0-9]/g, '');
        var suffix2 = suffix.length / 2;
        var suffix2 = suffix.substring(0, suffix2)
        var variable = suffix2 - 1;

        var ddlPuestoDestinatario = "<%=ddlPuestoDestinatario.ClientID%>";
        var ddlPuestoDestinatario = ddlPuestoDestinatario.replace(suffix2, '');
        var ddlPuestoDestinatario = ddlPuestoDestinatario.replace(suffix2, variable);

        var ddlAreaOficio = "<%=ddlAreaOficio.ClientID%>";
        var ddlAreaOficio = ddlAreaOficio.replace(suffix2, '');
        var ddlAreaOficio = ddlAreaOficio.replace(suffix2, variable);

        var ddlAreaFirma = "<%=ddlAreaFirma.ClientID%>";
        var ddlAreaFirma = ddlAreaFirma.replace(suffix2, '');
        var ddlAreaFirma = ddlAreaFirma.replace(suffix2, variable);

        var ddlAreaRubrica = "<%=ddlAreaRubrica.ClientID%>";
        var ddlAreaRubrica = ddlAreaRubrica.replace(suffix2, '');
        var ddlAreaRubrica = ddlAreaRubrica.replace(suffix2, variable);

        var chkFiltroSIE = "<%=chkFiltroSIE.ClientID%>";
        var chkFiltroSIE = chkFiltroSIE.replace(suffix2, '');
        var chkFiltroSIE = chkFiltroSIE.replace(suffix2, variable);

        $("#" + ddlPuestoDestinatario).removeAttr('disabled').removeClass('aspNetDisabled');
        $("#" + ddlAreaOficio).removeAttr('disabled').removeClass('aspNetDisabled');
        $("#" + ddlAreaFirma).removeAttr('disabled').removeClass('aspNetDisabled');
        $("#" + ddlAreaRubrica).removeAttr('disabled').removeClass('aspNetDisabled');
        $("#" + chkFiltroSIE).removeAttr('disabled').removeClass('aspNetDisabled');

        $("#<%=ddlPuestoDestinatario.ClientID%>").removeAttr('disabled').removeClass('aspNetDisabled');
        $("#<%=ddlAreaOficio.ClientID%>").removeAttr('disabled').removeClass('aspNetDisabled');
        $("#<%=ddlAreaFirma.ClientID%>").removeAttr('disabled').removeClass('aspNetDisabled');
        $("#<%=ddlAreaRubrica.ClientID%>").removeAttr('disabled').removeClass('aspNetDisabled');
        $("#<%=chkFiltroSIE.ClientID%>").removeAttr('disabled').removeClass('aspNetDisabled');

        $("#<%=hdCatDocumento.ClientID%>").val(IdCatDocumento);
        $("#<%=hdIdDocumento.ClientID%>").val(IdDocumento);
        $("#<%=hdTIDFolio.ClientID%>").val(idFolio);
        $("#<%=hdClasif.ClientID%>").val(clasificacion);
        $("#<%=hdIdTipoOficio.ClientID%>").val(idTipoOficio);
        $("#<%=hdIdPaso.ClientID%>").val(idPaso);

        $("#txtAsuntoOficio").val("");
        $("#txtComentariosOficio").val("");
        $("#txtDestinatarioOficio").val("");
        $("#ddlPuestoDestinatario").val("-1");
        $("#ddlAreaFirma").val("-1");
        $('#lstUsuariosFirma').children().remove();
        $('#lstFirmas').children().remove();
        $("#ddlAreaRubrica").val("-1");
        $('#lstUsuariosRubrica').children().remove();
        $('#lstRubricas').children().remove();

        $("#divRegistroSICOD").dialog({
            resizable: false,
            autoOpen: true,
            height: 500,
            width: 700,
            modal: true,
            title: "Registro SICOD",
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
                    var err = '<ul>';
                    var conterr = 0;
                    $('#divTextoConfirmacion').html(err);
                    if ($("#txtAsuntoOficio").val() == "") {
                        conterr++;
                        err += "<li>El asunto es obligatorio.</li>";
                    }
                    if ($("#txtComentariosOficio").val() == "") {
                        conterr++;
                        err += "<li>Los comentarios son obligatorios.</li>";
                    }
                    if ($("#txtDestinatarioOficio").val() == "") {
                        conterr++;
                        err += "<li>El destinatario es obligatorio.</li>";
                    }
                    if ($("#ddlPuestoDestinatario").val() == "-1") {
                        conterr++;
                        err += "<li>El puesto es obligatorio.</li>";
                    }
                    if ($("#lstFirmas option").length == 0) {
                        conterr++;
                        err += "<li>Debe haber al menos una firma.</li>";
                    }
                    err += "</ul>";
                    if (conterr == 0) {
                        ConfirmacionAltaSICOD(1, '');
                    }
                    else {
                        ConfirmacionAltaSICOD(0, err);
                    }

                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });

    }

    function ConfirmacionAltaSICOD(Respuesta, err) {

        if (Respuesta == 0) {
            $('#divTextoAviso').html(err);
            $("#divAviso").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 500,
                modal: true,
                title: "Validación",
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
                    },
                }
            });
        }
        else {
            $('#divTextoConfirmacion').html("");
            $('#divTextoConfirmacion').text("Se realizará el registro del documento en SICOD, ¿Deseas continuar?");
            $("#divConfirmacion").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 500,
                modal: true,
                title: "Registro SICOD",
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
                            //
                            //ConsultarOficios();

                            $(this).dialog("close");
                            $("#divRegistroSICOD").dialog("close");


                        }
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            })
        }
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

        if ($("#<%=hdIdDocumento.ClientID%>").val() > 0)
            param_data = '{Folio:' + $("#<%=hdTIDFolio.ClientID%>").val() + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdDocumento.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", TipoDocumentoP: "' + $("#<%=hdIdTipoOficio.ClientID%>").val() + '", Clasificacion: "' + $("#<%=hdClasif.ClientID%>").val() + '", Paso: ' + $("#<%=hdIdPaso.ClientID%>").val() + ', Proceso:6 }';
        else
            param_data = '{Folio:' + $("#<%=hdTIDFolio.ClientID%>").val() + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdDocumento.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", TipoDocumentoP: "' + $("#<%=hdIdTipoOficio.ClientID%>").val() + '", Clasificacion: "' + $("#<%=hdClasif.ClientID%>").val() + '", Paso: ' + $("#<%=hdIdPaso.ClientID%>").val() + ', Proceso:5 }';

        $.ajax({
            type: "POST",
            url: "../PC/ServiciosSICOD.aspx/RegistrarOficioSD",
            contentType: "application/json;charset=utf-8",
            data: param_data,
            dataType: "json",
            success: function (data) {
                var folio = data.d;
                console.log("url_sicod success");
                $("#<%=hfFolioSICOD.ClientID%>").val(data.d);
                $("#<%=btnActualizarGrid.ClientID%>").click();
                //alert("Registrado con el folio: " + folio);
                MensajeRespuesta(folio);
                url_sicod = 'VerOficios.aspx?folio=' + folio.replace("/", "-");
            },

            error: function (result) {
                alert("Error al realizar registro")
                return result
            }
        });

    }

    function MensajeRespuesta(folio) {
        $('#divTextoAvisoR').text("Registrado con el folio: " + folio);
        $("#divAvisoR").dialog({
            resizable: false,
            autoOpen: true,
            height: 300,
            width: 500,
            modal: true,
            title: "Registro SICOD",
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
                    ConsultarOficios(folio);
                    $(this).dialog("close");
                },
            }
        });
    }

    function ConsultarOficios(pFolioSICOD) {
        
        MuestraImgCarga(null);
        var url = '';
        
        //Levanta la modal
        //url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';
        url = '../PC/VerOficios.aspx?folio=' + pFolioSICOD
        //Detectar navegador
        var nav = navigator.userAgent.toLowerCase();

        //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
        if (nav.indexOf("msie") != -1) {
            winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
            window.open(url, "SICOD", winprops);
            OcultaImagenCarga();
            return true;
        } else {
            var liTop = (parseInt($(window).height()) / 2) - (450 / 2);
            var liLeft = (parseInt($(window).width()) / 2) - (850 / 2);

            winprops = "width=850,height=450,toolbar=no, menubar=no, scrollbars=no, resizable=Yes, location=no, status=no, left=" + liLeft + ", top=" + liTop;
            //alert("winprops: " + winprops);
            //var modal = window.open(url + "?r=" + idRen + "&ds=" + idDocumento + "&vds=" + idVersion, "SICOD", winprops);
            var modal = window.open(url, "SICOD", winprops);

            modal.focus();
            OcultaImagenCarga();
            return false;
        }

    }

    function AgregarFirma() {
        if ($("#<%=lstUsuariosFirma.ClientID%> option:selected").length > 0) {
            $("#<%=lstFirmas.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstUsuariosFirma.ClientID%> option:selected").val()).text($("#<%=lstUsuariosFirma.ClientID%> option[value='" + $("#<%=lstUsuariosFirma.ClientID%>").val() + "']").text()));
            $("#<%=lstFirmas.ClientID%>").val($("#<%=lstUsuariosFirma.ClientID%>").val());
            $("#<%=lstUsuariosFirma.ClientID%> option:selected").remove();
        }
        else {
            alert("Selecciona una firma")
        }

    }

    function QuitarFirma() {
        
        if ($("#<%=lstFirmas.ClientID%> option:selected").length > 0) {
            $("#<%=lstUsuariosFirma.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstFirmas.ClientID%> option:selected").val()).text($("#<%=lstFirmas.ClientID%> option[value='" + $("#<%=lstFirmas.ClientID%>").val() + "']").text()));
            $("#<%=lstFirmas.ClientID%> option:selected").remove();

        }
        else {
            alert("Selecciona una firma")
        }

    }

    function AgregarRubrica() {
        if ($("#<%=lstUsuariosRubrica.ClientID%> option:selected").length > 0) {
            $("#<%=lstRubricas.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstUsuariosRubrica.ClientID%>").val()).text($("#<%=lstUsuariosRubrica.ClientID%> option[value='" + $("#<%=lstUsuariosRubrica.ClientID%>").val() + "']").text()));
            $("#<%=lstRubricas.ClientID%>").val($("#<%=lstUsuariosRubrica.ClientID%>").val());
            $("#<%=lstUsuariosRubrica.ClientID%> option:selected").remove();
        }
        else {
            alert("Selecciona una rubrica")
        }

        return false;
    }

    function QuitarRubrica() {
        if ($("#<%=lstRubricas.ClientID%> option:selected").length > 0) {
            $("#<%=lstUsuariosRubrica.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstRubricas.ClientID%>").val()).text($("#<%=lstRubricas.ClientID%> option[value='" + $("#<%=lstRubricas.ClientID%>").val() + "']").text()));
            $("#<%=lstRubricas.ClientID%> option:selected").remove();

        }
        else {
            alert("Selecciona una rubrica")
        }

        return false;
    }

    function ObtenerFirmas(Origen) {
        debugger
        var EsSIE = 0;
        if ($("#<%=chkFiltroSIE.ClientID%>").prop('checked')) {
            EsSIE = 1;
        }

        if (Origen == 1) {
            let valueddlAreaFirmaSelected = $(".ddlAreaFirma_" + $("#<%=hdTIDFolio.ClientID%>").val()).val()
            param_data = '{Area: ' + $("#<%=ddlAreaFirma.ClientID%>").val() + ', EsSIE: ' + EsSIE + ' }'
        }
        else {
            param_data = '{Area: ' + $("#<%=ddlAreaRubrica.ClientID%>").val() + ', EsSIE: ' + EsSIE + ' }'
        }



        $.ajax({
            type: "POST",
            url: "../PC/ServiciosSICOD.aspx/ObtenerUsuariosParaFirma",
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


    function OnDragEnter(e) {
        e.stopPropagation();
        e.preventDefault();
    }

    function OnDragOver(e) {
        e.stopPropagation();
        e.preventDefault();
    }

    function OnDrop(e) {
        e.stopPropagation();
        e.preventDefault();
        selectedName = e.dataTransfer.files[0].name;
        selectedFile = e.dataTransfer.files[0];
        $("#documentoSubir").text(e.dataTransfer.files[0].name + " listo para subir");
    }

    $(document).ready(function () {
        var box;
        box = document.getElementById("DropZone");
        //box.addEventListener("dragenter", OnDragEnter, false);
        box.addEventListener("dragover", OnDragOver, false);
        box.addEventListener("drop", OnDrop, false);

        $('input[type="file"]').change(function (e) {
            selectedName = e.target.files[0].name;
            selectedFile = e.target.files[0];
            $("#documentoSubir").text(e.target.files[0].name + " listo para subir");
        });

    });


</script>

<style type="text/css">
    .fuGrid {
        /*float:left;*/
        margin-top: 10px;
    }

    .imgGrid {
        /*float:left;*/
        width: 25px;
        margin-top: 7px;
    }

    .aspNetDisabled {
        color: #000;
    }

    .OcultarControl {
        display: none;
    }

    .gridScrollW {
    }

    .cssErrorDocs {
        color: red;
        font-size: 14px;
    }
</style>
<asp:UpdatePanel ID="upGridDocs" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Panel ID="pnlEncabezado" runat="server" Width="100%">
            <div style="text-align: left; width: 100%; padding-bottom: 5px; padding-left: 2%">
                <asp:Button ID="btnExportaExcelDocs" runat="server" Text="Exportar a Excel" />
            </div>
            <div style="text-align: left; width: 100%; padding-bottom: 5px; padding-left: 2%">
                <uc1:ucFiltro ID="ucFiltroDocs" runat="server" Width="96%" />
            </div>
            <br />
            <br />
        </asp:Panel>

        <%-- margin-left:2%;--%>
        <asp:Panel ID="pnlGrid" runat="server" Width="100%" ClientIDMode="Static">
            <div style="overflow: hidden; background: #ffffff; z-index: 0; width: 95%; height: 25px; margin-left: 2%; text-align: left; position: static;">
                <asp:GridView ID="gvEncabecados" runat="server" RowStyle-HorizontalAlign="Left"
                    CssClass="anchoGriDocsEncabezado" RowStyle-CssClass="GridViewEncabezado">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px" Visible="false" ItemStyle-CssClass="GridViewEncabezado">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="PASO" DataField="I_ID_PASO_INI" ItemStyle-Width="33px" ItemStyle-CssClass="GridViewEncabezado" />
                        <asp:BoundField HeaderText="DOCUMENTO" DataField="T_NOM_DOCUMENTO_CAT" ItemStyle-Width="35%" ItemStyle-CssClass="GridViewEncabezado" />
                        <asp:BoundField HeaderText="DOCUMENTOS ADJUNTOS" ItemStyle-Width="35%" DataField="T_NOM_DOCUMENTO_ADJ" ItemStyle-CssClass="GridViewEncabezado" />
                        <asp:BoundField HeaderText="" ItemStyle-Width="35px" ItemStyle-CssClass="GridViewEncabezado" />
                        <asp:BoundField HeaderText="SICOD" ItemStyle-CssClass="GridViewEncabezado" />

                    </Columns>
                </asp:GridView>
            </div>
            <div id="pnlGridInt" runat="server" clientidmode="Static" style="overflow-x: hidden; overflow: auto; background: #ffffff; z-index: 0; width: 95%; height: 400px; margin-left: 2%; text-align: left;">
                <asp:GridView ID="gvConsultaDocs" runat="server" RowStyle-Height="25px" RowStyle-HorizontalAlign="Left"
                    CssClass="anchoGriDocs" ShowHeader="false" DataKeyNames="T_NOM_DOCUMENTO_CAT">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="PASO" DataField="I_ID_PASO_INI" ItemStyle-Width="33px" />
                        <asp:BoundField HeaderText="DOCUMENTO" DataField="T_NOM_DOCUMENTO_CAT" ItemStyle-Width="35%" />
                        <asp:TemplateField HeaderText="DOCUMENTOS ADJUNTOS" ItemStyle-Width="35%">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="35px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAgregarDoc" runat="server" ImageUrl="~/Imagenes/masDocumentos.png" Width="25px" ToolTip="Agregar más documentos" />
                                <asp:ImageButton ID="btnMasDocG" runat="server" Visible="false" ImageUrl="~/Imagenes/masDocumentos.png" Width="25px" ToolTip="Agregar más documentos" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnRegistroSICOD" runat="server" Text="Registrar" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <br />
            <br />
            <div id="divBoton" runat="server" style="width: 95%; margin-left: 2%; text-align: center;">

                <asp:ImageButton OnClientClick="setActiveTab('li1'); return false;" ID="imgRegresarDocs" Width="38px"
                    ToolTip="Regresar a Detalle" runat="server" ImageUrl="../Imagenes/VerDetalle.png" />

                <asp:ImageButton ID="btnCargaMasiva" runat="server" ImageUrl="/Imagenes/adjuntarDocs.png" ToolTip="Carga masiva de documentos" OnClientClick="MuestraImgCarga(this);" />
            </div>

        </asp:Panel>
        <%--</div>--%>

        <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
            <asp:Image ID="Image1" runat="server"
                AlternateText="No existen registros para la consulta" ImageAlign="Middle"
                ImageUrl="../Imagenes/no EXISTEN.gif" />
        </div>

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
                                                <table class="style163">
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label1" runat="server" CssClass="txt_gral" Text="Área del Oficio"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAreaOficio" runat="server" Width="410px" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label17" runat="server" CssClass="txt_gral" Text="Asunto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAsuntoOficio" runat="server" CssClass="txt_gral" Width="410px"
                                                                MaxLength="255" ClientIDMode="Static"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblComentariosOficio" Text="Comentarios" CssClass="txt_gral"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtComentariosOficio" runat="server" CssClass="txt_gral" Width="410px"
                                                                MaxLength="255" ClientIDMode="Static"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label18" runat="server" CssClass="txt_gral" Text="Destinatario"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDestinatarioOficio" runat="server" CssClass="txt_gral" Width="410px"
                                                                MaxLength="50" ClientIDMode="Static"></asp:TextBox>
                                                            <input id="destinatarioKey" name="destinatarioKey" runat="server" type="hidden" value="0" />
                                                            <input id="destinatarioText" name="destinatarioText" runat="server" type="hidden"
                                                                value="" />
                                                            <asp:HiddenField runat="server" ID="hdnIdEntidad" Value="0" ClientIDMode="Static" />
                                                            <asp:HiddenField runat="server" ID="hdnIdTipoEntidad" Value="0" ClientIDMode="Static" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label19" runat="server" CssClass="txt_gral" Text="Puesto" ClientIDMode="Static"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPuestoDestinatario" runat="server" CssClass="txt_gral" Width="410px" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 45%">
                                                            <asp:Label ID="lblFiltroSIE" runat="server" CssClass="txt_gral" Text="Con firma SIE:" ClientIDMode="Static"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chkFiltroSIE" runat="server" Checked="true" CssClass="txt_gral" ClientIDMode="Static" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style165">
                                                            <asp:Label ID="Label20" runat="server" CssClass="txt_gral" Text="Firmas" ClientIDMode="Static"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:DropDownList ID="ddlAreaFirma" runat="server" CssClass="txt_gral" Width="410px"
                                                                            onchange="ObtenerFirmas(1);" ClientIDMode="Static">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 45%">
                                                                        <asp:Label ID="lblUsuariosRub" runat="server" CssClass="txt_gral" Text="Disponibles:" ClientIDMode="Static"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10%">&nbsp;
                                                                    </td>
                                                                    <td style="width: 45%">&nbsp;
                                                            <asp:Label ID="lblRubrica" runat="server" CssClass="txt_gral" Text="Seleccionados:" ClientIDMode="Static"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" rowspan="2" style="text-align: right" valign="top">
                                                                        <asp:ListBox ID="lstUsuariosFirma" runat="server" CssClass="txt_gral" Height="68px"
                                                                            TabIndex="26" Width="100%" ClientIDMode="Static"></asp:ListBox>
                                                                    </td>
                                                                    <td style="text-align: center">

                                                                        <img id="imgAgregarFirma" src="../../Imagenes/FlechaRojaDer.gif" style="height: 30px" onclick="AgregarFirma();" />
                                                                        <br />
                                                                        <img id="imgQuitarFirma" src="../../Imagenes/FlechaRojaIzq.gif" style="height: 30px" onclick="QuitarFirma();" />
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        <asp:ListBox ID="lstFirmas" runat="server" CssClass="txt_gral" Height="68px" Width="100%" ClientIDMode="Static"></asp:ListBox>
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
                                                                            onchange="ObtenerFirmas(2);" ClientIDMode="Static">
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
                                                                        <asp:ListBox ID="lstUsuariosRubrica" runat="server" CssClass="txt_gral" Height="68px" TabIndex="26" Width="100%" ClientIDMode="Static"></asp:ListBox>
                                                                    </td>
                                                                    <td style="text-align: center">
                                                                        <img id="imgAgregarRubrica" src="../../Imagenes/FlechaRojaDer.gif" style="height: 30px" onclick="AgregarRubrica();" />
                                                                        <br />
                                                                        <img id="imgQuitarRubrica" src="../../Imagenes/FlechaRojaIzq.gif" style="height: 30px" onclick="QuitarRubrica();" />
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        <asp:ListBox ID="lstRubricas" runat="server" CssClass="txt_gral" Height="68px" Width="100%" ClientIDMode="Static"></asp:ListBox>
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
                                                </table>
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
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image5" runat="server" Width="32px" Height="32px"
                            ImageUrl="~/Imagenes/Errores/Error1.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <div id="divTextoConfirmacion"></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divAviso" style="display: none">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image4" runat="server" Width="32px" Height="32px"
                            ImageUrl="~/Imagenes/Errores/Error1.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <div id="divTextoAviso"></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divAvisoR" style="display: none">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image6" runat="server" Width="32px" Height="32px"
                            ImageUrl="~/Imagenes/Errores/Error1.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <div id="divTextoAvisoR"></div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <asp:HiddenField ID="hfValorScrollDocs" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfIdDocSicod" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfIdVerDocSicod" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfIdRenglon" runat="server" Value="-1" ClientIDMode="Static" />
        <asp:HiddenField ID="hfNumVerDocSicod" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfVisita" runat="server" Value="0" ClientIDMode="Static" />
        <asp:Button ID="btnActualizarGrid" runat="server" OnClick="btnActualizarGrid_Click" Style="display: none" />

        <div id="divAvisoDocs" style="display: none" title="Notificación" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgAviso" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <%= MensajeDocs%>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>


        <div id="divConfirmacionDocs" style="display: none" title="Confirmación" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgConfirmaRegistroVisitaCC" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <%= MensajeDocs%>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divObservacionesRechazo" style="display: none" title="Confirmación" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image2" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <table>
                                <tr>
                                    <td>Favor de ingresar el motivo por el cual se ha rechazado el documento.</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtComentRechazo" runat="server" Width="403px" Height="80px" TextMode="MultiLine"
                                            onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)"
                                            onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblFechaGeneralDocs" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divPreguntaNumRequerimiento" style="display: none" title="Seleccione el número de requerimiento">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgPregunta" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            Por favor seleccione el número del requerimiento al que pertenece la respuesta:
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="text-align: center;">
                        <div class="MensajeModal-UI">
                            Número de requerimiento:
                    <br />
                            <asp:DropDownList ID="ddlNumRequerimientos" runat="server" Width="200px" Style="font-size: 11px;" AppendDataBoundItems="true">
                                <asp:ListItem Value="-1">- Selecciona un Número de Requerimiento -</asp:ListItem>

                            </asp:DropDownList>
                        </div>
                    </td>
                    <td style="text-align: center;">
                        <div class="MensajeModal-UI">
                            Consecutivo:
                    <br />
                            <asp:DropDownList ID="ddlCvoRequerimiento" runat="server" Width="100px" Style="font-size: 11px;">
                                <asp:ListItem Value="-1">- Consecutivo -</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divObservacionesAprobacion" style="display: none" title="Confirmación" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image3" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <table>
                                <tr>
                                    <td>Favor de ingresar el motivo de aprobación del documento.</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtComentAprueba" runat="server" Width="403px" Height="80px" TextMode="MultiLine"
                                            onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)"
                                            onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblFechaGeneralDocsA" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />

        <asp:HiddenField ID="hdIdDocumento" runat="server" Value="-1" />
        <asp:HiddenField ID="hdPrefijo" runat="server" Value="" />
        <asp:HiddenField ID="hdCatDocumento" runat="server" Value="-1" />
        <asp:HiddenField ID="hdTIDFolio" runat="server" Value="" />
        <asp:HiddenField ID="hfFolioSICOD" runat="server" Value="" />
        <asp:HiddenField ID="hdClasif" runat="server" Value="" />
        <asp:HiddenField ID="hdIdTipoOficio" runat="server" Value="" />
        <asp:HiddenField ID="hdIdPaso" runat="server" Value="" />

        <asp:Button runat="server" ID="btnConfirmarDocsSI" Style="display: none" />
        <asp:Button runat="server" ID="btnConfirmarDocsNO" Style="display: none" />
        <asp:Button runat="server" ID="btnComentRechazo" Style="display: none" OnClick="btnComentRechazo_Click" />
        <asp:Button runat="server" ID="btnComentAprueba" Style="display: none" OnClick="btnComentAprueba_Click" />
        <asp:Button runat="server" ID="btnAceptarNumReq" Style="display: none" OnClientClick="MuestraImgCarga();" ClientIDMode="Static" />

        <%--<asp:Button runat="server" ID="btnAceptarNumReq" Style="display: none" OnClientClick="btnAceptarNumReq_Click" ClientIDMode="Static" />--%>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportaExcelDocs" />
        <asp:PostBackTrigger ControlID="btnCargaMasiva" />
        <asp:PostBackTrigger ControlID="btnComentRechazo" />
        <asp:PostBackTrigger ControlID="btnComentAprueba" />
    </Triggers>

</asp:UpdatePanel>


<script type="text/javascript">


</script>
