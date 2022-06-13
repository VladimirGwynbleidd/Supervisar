<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Checklist.ascx.vb" Inherits="SEPRIS.Checklist" %>
<%@ Register Src="~/PC/UserControls/RequerimientoInformacion.ascx" TagPrefix="uc1" TagName="RequerimientoInformacion" %>

<script type="text/javascript">
    var registrosP = 0;
    var SubEntidad = "";
    var Chk;

    function GetSelected() {
        SubEntidad = new Array();
        $("[name='chkSubEntidades']").each(function (index, data) {
            if (data.checked) {
                SubEntidad.push(data.value);
            }
        });
    }


    function GuardarActividades() {
        $.ajax({
            type: "POST",
            async: false,
            url: "DetallePC.aspx/ValidaExpediente",
            data: '{Folio: ' + '<%=Folio%>' + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data2) {
                if (data2.d == false) {
                    var msj = "Es necesario completar las Actividades o adjuntar documentos"
                    $('#divTextoRespuesta').html(msj);
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
                            "Cerrar": function () {
                                $(this).dialog("close");
                            },
                        }
                    });
                }
                else {

                    CambiaEstatusFolio(3, 108);
                    GuardarBitacora(<%=Folio%>,"<%=Usuario%>","3","Se guardaron actividades.");
                }
            }
        });
    }

    function GuardaComentario() {
        debugger
        $.ajax({
            type: "POST",
            async: false,
            url: "DetallePC.aspx/ValidaExpediente",
            data: '{Folio: ' + '<%=Folio%>' + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data2) {
                if (data2.d == false) {
                    var msj = "Existen documentos pendientes de adjuntar"
                    $('#divTextoRespuesta').html(msj);
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
                            "Cerrar": function () {
                                $(this).dialog("close");
                                setActiveTab('li6');
                            },
                        }
                    });
                }
                else {

                    $.ajax({
                        type: "POST",
                        url: "DetallePC.aspx/GuardarComentarioRes",
                        data: '{Folio: ' + '<%=Folio%>' +
           ', Comentario: "' + $("#<%= txt_ComentariosAdicionales.ClientID%>").val() + '\"}',
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {
                                        GuardarBitacora(<%=Folio%>,"<%=Usuario%>","3","Se notificó Oficio de la resolución seleccionada.", $("#<%= txt_ComentariosAdicionales.ClientID%>").val());
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

                        }
        });
                 }


                 function GuardaComentarioDictamen() { // MMOB Nuevo - Para validar el documento de Dictamen que realemte esté arriba.
                     debugger
                     $.ajax({
                         type: "POST",
                         async: false,
                         url: "DetallePC.aspx/ValidaExpediente",
                         data: '{Folio: ' + '<%=Folio%>' + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data2) {
                if (data2.d == false) {
                    var msj = "Existen documentos pendientes de adjuntar"
                    $('#divTextoRespuesta').html(msj);
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
                            "Cerrar": function () {
                                $(this).dialog("close");
                                setActiveTab('li6');
                            },
                        }
                    });
                }
                else {

                    $.ajax({
                        type: "POST",
                        url: "DetallePC.aspx/GuardarComentarioResASisan",
                        data: '{Folio: ' + '<%=Folio%>' +
            ', Comentario: "' + $("#<%= txt_ComentariosAdicionales.ClientID%>").val() + '\"}',
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (data) {
                                        GuardarBitacora(<%=Folio%>,"<%=Usuario%>","4","Se guarda comentario del dictamen.",$("#<%= txt_ComentariosAdicionales.ClientID%>").val());
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

                        }
        });
                }

                function GuardaDocumentosFirmadosPlazo() {

                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "DetallePC.aspx/ValidaExpediente",
                        data: '{Folio: ' + '<%=Folio%>' + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data2) {
                            if (data2.d == false) {
                                var msj = "Existen documentos pendientes de adjuntar"
                                $('#divTextoRespuesta').html(msj);
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
                                        "Cerrar": function () {
                                            $(this).dialog("close");
                                            setActiveTab('li6');
                                        },
                                    }
                                });
                            }
                            else {
                                CambiaEstatusFolio(3, 104);
                            }

                        }
                    });
                }


                function CargarChecklist() {
                    setActiveTab('li4');
                    $("#tabla_resultados").empty();
                    $("#<%= txtMotivoNo.ClientID%>").prop('disabled', true);
                    $.ajax({
                        type: "POST",
                        url: "DetallePC.aspx/ObtenerPreguntas",
                        data: '{ }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $.each(data.d, function () {
                                registrosP++;
                                $("input[name='CumpleCKL'][value='1']").prop('disabled', true);
                                $("#<%= txtMotivoNo.ClientID%>").removeAttr('disabled');
                                var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' name='chkQ" + this['Value'] + "' value='1' onclick='ValidarChecklist();'> Sí</td><td><input type='radio' checked name='chkQ" + this['Value'] + "' value='0' onclick='ValidarChecklist();'> No</td></tr>"
                                $("#tabla_resultados").append(nuevafila)
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
                }

                function CargarChecklistFolio() {
                    setActiveTab('li4');
                    $("#tabla_resultados").empty();
                    $("#<%= txtMotivoNo.ClientID%>").prop('disabled', true);
                    $.ajax({
                        type: "POST",
                        url: "DetallePC.aspx/ObtenerPreguntasFolio",
                        data: '{Folio: ' + '<%=Folio%>' + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $.each(data.d, function () {
                                if (this['Value'] == "999") {
                                    $("#<%= txtMotivoNo.ClientID%>").val(this['Text']);
                                    $("#btnAceptar_tab4").removeAttr('onclick');
                                    $("#btnAceptar_tab4").attr('onclick', 'ValidaIrregularidadesComp(); ')
                                    if (this['Check'] == "True") {
                                        $("input[name='CumpleCKL'][value='1']").prop('checked', true);
                                    }
                                    else {
                                        $("input[name='CumpleCKL'][value='0']").prop('checked', true);
                                    }
                                    $("input[name='CumpleCKL'][value='1']").prop('disabled', true);
                                    $("input[name='CumpleCKL'][value='0']").prop('disabled', true);
                                }
                                else {
                                    if (this['Check'] == "True") {
                                        var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' checked disabled name='chkQ" + this['Value'] + "' value='1'> Sí</td><td><input type='radio' name='chkQ" + this['Value'] + "' value='0' disabled> No</td></tr>"
                                    }
                                    else {
                                        var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' disabled name='chkQ" + this['Value'] + "' value='1'> Sí</td><td><input type='radio' checked name='chkQ" + this['Value'] + "' value='0' disabled> No</td></tr>"
                                    }
                                }
                                $("#tabla_resultados").append(nuevafila)
                                $("#lblComentariosCaracteres").hide();
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
                }

    function CargarChecklistFolioRes(res, pnl) {
        setActiveTab('li4');
        $("#tabla_resultados").empty();
        if ('<%=Area%>' == '36' && pnl == "1") {
            $("#pnlSepara").css('display', 'block');

            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ObtenerSubEntidadesComplete",
                contentType: "application/json;charset=utf-8",
                data: '{Folio: ' + '<%=Folio%>' + ' }',
                dataType: "json",
                async: false,
                success: function (data) {
                    btns = data.d.length;
                    if (data.d.length > 0) {
                        $('#lstSubfolios').empty();
                        $.each(data.d, function () {
                            var nuevafila = "<li>" + this['Text'].split('/')[5] + "</li>"
                            $("#lstSubfolios").append(nuevafila)
                        });
                    }
                },
                error: function (result) {
                    return result
                }
            });
        }

        $("#<%= txtMotivoNo.ClientID%>").prop('disabled', true);
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerPreguntasFolio",
            data: '{Folio: ' + '<%=Folio%>' + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                $.each(data.d, function () {
                    if (this['Value'] == "999") {
                        $("#<%= txtMotivoNo.ClientID%>").val(this['Text']);
                        $("#btnAceptar_tab4").removeAttr('onclick');
                        $("#btnAceptar_tab4").attr('onclick', res)
                        if (this['Check'] == "True") {
                            $("input[name='CumpleCKL'][value='1']").prop('checked', true);
                        }
                        else {
                            $("input[name='CumpleCKL'][value='0']").prop('checked', true);
                        }
                        $("input[name='CumpleCKL'][value='1']").prop('disabled', true);
                        $("input[name='CumpleCKL'][value='0']").prop('disabled', true);
                    }
                    else {
                        if (this['Check'] == "True") {
                            var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' checked disabled name='chkQ" + this['Value'] + "' value='1'> Sí</td><td><input type='radio' name='chkQ" + this['Value'] + "' value='0' disabled> No</td></tr>"
                        }
                        else {
                            var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' disabled name='chkQ" + this['Value'] + "' value='1'> Sí</td><td><input type='radio' checked name='chkQ" + this['Value'] + "' value='0' disabled> No</td></tr>"
                        }
                    }
                    $("#tabla_resultados").append(nuevafila)
                    $("#lblComentariosCaracteres").hide();
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

        if ($("#<%=EstatusPC%>") == "108") {
            setActiveTab('li4');
        }
    }

    function CargarChecklistFolioResOk(res, cres, come, onclk) {
        debugger
        $(".trSubFolios").css('display', 'none')

        setActiveTab('li4');
        $("#tabla_resultados").empty();
        $("#<%= txtMotivoNo.ClientID%>").prop('disabled', true);
                    $.ajax({
                        type: "POST",
                        url: "DetallePC.aspx/ObtenerPreguntasFolio",
                        data: '{Folio: ' + '<%=Folio%>' + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $.each(data.d, function () {
                                if (this['Value'] == "999") {
                                    $("#<%= txtMotivoNo.ClientID%>").val(this['Text']);
                                    $("#btnAceptar_tab4").removeAttr('onclick');
                                    $("#btnAceptar_tab4").attr('onclick', onclk)
                                    if (this['Check'] == "True") {
                                        $("input[name='CumpleCKL'][value='1']").prop('checked', true);
                                    }
                                    else {
                                        $("input[name='CumpleCKL'][value='0']").prop('checked', true);
                                    }
                                    $("input[name='CumpleCKL'][value='1']").prop('disabled', true);
                                    $("input[name='CumpleCKL'][value='0']").prop('disabled', true);
                                }
                                else {
                                    if (this['Check'] == "True") {
                                        var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' checked disabled name='chkQ" + this['Value'] + "' value='1'> Sí</td><td><input type='radio' name='chkQ" + this['Value'] + "' value='0' disabled> No</td></tr>"
                                    }
                                    else {
                                        var nuevafila = "<tr><td>" + this['Value'] + "</td><td>" + this['Text'] + "</td><td><input type='radio' disabled name='chkQ" + this['Value'] + "' value='1'> Sí</td><td><input type='radio' checked name='chkQ" + this['Value'] + "' value='0' disabled> No</td></tr>"
                                    }
                                }
                                $("#tabla_resultados").append(nuevafila)
                                $("#lblComentariosCaracteres").hide();
                                debugger
                                $("#<%=ddl_Resolucion.ClientID%>").val(res);
                                $("#<%=txt_ComentariosResolucion.ClientID%>").val(cres);

                                $("#<%=txt_ComentariosAdicionales.ClientID%>").val(come);
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
                }

                function ValidarChecklist() {
                    var chkOk = 0;
                    for (var index = 1; index <= registrosP; index++) {
                        if ($('input:radio[name=chkQ' + index + ']:checked').val() == 0) {
                            $("input[name='CumpleCKL'][value='0']").prop('checked', true);
                            $("input[name='CumpleCKL'][value='0']").removeAttr('disabled');
                            $("input[name='CumpleCKL'][value='1']").prop('disabled', true);
                            $("#<%= txtMotivoNo.ClientID%>").removeAttr('disabled');
                        }
                        else {
                            chkOk++;
                        }
                        if (chkOk == registrosP) {
                            $("input[name='CumpleCKL'][value='1']").prop('checked', true);
                            $("input[name='CumpleCKL'][value='1']").removeAttr('disabled');
                            $("input[name='CumpleCKL'][value='0']").prop('disabled', true);
                            $("#<%= txtMotivoNo.ClientID%>").val('');
                            $("#<%= txtMotivoNo.ClientID%>").prop('disabled', true);
                        }
                    }

                }

                function GetCheckList() {
                    
                    Chk = new Array();
                    for (var index = 1; index <= registrosP; index++) {
                        if ($('input:radio[name=chkQ' + index + ']:checked').length > 0) {
                            Chk.push(index + "|" + $('input:radio[name=chkQ' + index + ']:checked').val())
                        }
                    }
                }

                function GuardarCheckList() { // MMOB - SE AGREGÓ LA VALIDACIÓN PARA QUE AGREGUE EL "MOTIVO DEL NO" CUANDO SEA NECESARIO
                    debugger
                    var isNo
                    var istexto = $("#<%=txtMotivoNo.ClientID%>").val()
        isNo = $("input:radio[name='CumpleCKL']:checked").val()
        if (isNo != 0) {
            GetCheckList();
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/GuardarCheckList",
                data: '{Folio: ' + '<%=Folio%>' +
    ', Checklist: "' + Chk +
    '", Cumple: ' + $("input:radio[name='CumpleCKL']:checked").val() +
    ', Motivo: "' + $("#<%=txtMotivoNo.ClientID%>").val() + '\"}',
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

        else {
            if (istexto != '') {
                GetCheckList();
                $.ajax({
                    type: "POST",
                    url: "DetallePC.aspx/GuardarCheckList",
                    data: '{Folio: ' + '<%=Folio%>' +
        ', Checklist: "' + Chk +
        '", Cumple: ' + $("input:radio[name='CumpleCKL']:checked").val() +
        ', Motivo: "' + $("#<%=txtMotivoNo.ClientID%>").val() + '\"}',
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
            else {

                $('#divTextoRespuesta').html('Es necesario completar la información necesaria para el  PC. Por favor completa los siguientes datos:');
                $("#divTextoRespuesta").append('<ul><li>Favor de ingresar el Motivo del NO.</li></ul>');

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
    }


    function GuardarResolucionP3() {
        $("#<%=ddl_Resolucion.ClientID%>").prop('disabled', true)
        $("#btnAceptar_tab4").prop('disabled', true)

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardarResolucion",
            data: '{Folio: ' + '<%=Folio%>' +
                            ', Resolucion: ' + $("#<%=ddl_Resolucion.ClientID()%>").val() +
            ', Descripcion: "' + $("#<%=txt_ComentariosResolucion.ClientID%>").val() + '\"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $("#divSubEntidadesSeleccion").dialog("close");
                            $("#<%=ddl_Resolucion.ClientID%>").prop('disabled', true)
                            $("#<%=txt_ComentariosResolucion.ClientID%>").prop('disabled', true);
                            $("#btnNotifica").prop('display', 'block')
                            GuardarBitacora(<%=Folio%>,"<%=Usuario%>","2","Se guarda la resolución: "+ $("#<%=ddl_Resolucion.ClientID()%> option:selected").text());
                            $(location).attr('href', './DetallePC.aspx');
                        },
                        failure: function () {
                            $('#divTextoAlerta').html('Error al guardar la resolución');
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

                function GuardarResolucionP4() {
                    $("#<%=ddl_Resolucion.ClientID%>").prop('disabled', true)
        $("#btnAceptar_tab4").prop('disabled', true)

        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/GuardarResolucionP",
            data: '{Folio: ' + '<%=Folio%>' +
                            ', Resolucion: ' + $("#<%=ddl_Resolucion.ClientID()%>").val() +
            ', Descripcion: "' + $("#<%=txt_ComentariosResolucion.ClientID%>").val() + '\"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#divSubEntidadesSeleccion").dialog("close");
                $("#<%=ddl_Resolucion.ClientID%>").prop('disabled', true)
                $("#<%=txt_ComentariosResolucion.ClientID%>").prop('disabled', true);
                $("#btnNotifica").prop('display', 'block')
                $(location).attr('href', './DetallePC.aspx');
            },
            failure: function () {
                $('#divTextoAlerta').html('Error al guardar la resolución');
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


    function ObtenerValidacionSISAN() {
        
        $.ajax({
            type: "POST",
            async: false,
            url: "DetallePC.aspx/ValidacionSISAN",
            data: '{"Folio":"' + '<%=Folio%>' + '","Usuario":"' + '<%=Usuario%>' + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $("#divMensajeRespuesta").dialog("close");
                            GuardarBitacora(<%=Folio%>,"<%=Usuario%>","4","Se envía a SISAN");
                            MensajeResSicod(data.d);

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



                function ObtenerSubEntidadesResolucion() {

                    $("#Tabla_SubEntidadesMenj").empty();


                    $.ajax({
                        type: "POST",
                        url: "DetallePC.aspx/ObtenerSubEntidadesMensaje",
                        data: '{Folio: ' + '<%=Folio%>' + ' }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $.each(data.d, function () {
                                var nuevafila = "<tr><td><input type='checkbox' name='chkSubEntidades' value='" + this['Value'] + "'onclick='GetSelected();'>" + this['Text'] + "</td></tr>"
                                $("#Tabla_SubEntidadesMenj").append(nuevafila)
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
                }


                function chkS() {
                    $("#<%= txtMotivoNo.ClientID%>").val('');
                    $("#<%= txtMotivoNo.ClientID%>").prop('disabled', true);
                    $("#tel").removeAttr("required");
                }

                function chkN() {
                    $("#<%= txtMotivoNo.ClientID%>").removeAttr('disabled');


                }

                function validaLimiteLongitud(obj, maxchar) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                    $("#<%=lblComentariosCaracteres.ClientID%>").text("" + remaningChar);

                    if (remaningChar <= 0) {
                        //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                        $("#<%=lblComentariosCaracteres.ClientID%>").text("" + 0);
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else { return true; }
                }

                function validaLimiteLongitud(obj, maxchar) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                    $("#<%=lbl_Txt2.ClientID%>").text("" + remaningChar);

                    if (remaningChar <= 0) {
                        //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                        $("#<%=lbl_Txt2.ClientID%>").text("" + 0);
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else { return true; }
                }

                function validaLimiteLongitudComentarios(obj, maxchar) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                    $("#<%=lbl_Comentarios_2.ClientID%>").text("" + remaningChar);

                    if (remaningChar <= 0) {
                        //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                        $("#<%=lbl_Comentarios_2.ClientID%>").text("" + 0);
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else { return true; }

                }

</script>

<style>
    .list-inline > li {
        display: inline-block;
        padding-right: 5px;
        padding-left: 5px;
    }
</style>
<br />
<table>
    <tr id="trSubFolios" class="trSubFolios" runat="server" visible="false">
        <td style="border-bottom: 1px solid black;">
            <div class="containerlst">
                <ul id="lstSubfolios" class="list-inline">
                </ul>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="txt_gral" style="font-weight: bold; text-align: center;">
                Contesta el siguiente checklist para determinar si el Programa de corrección cumple con los requisitos:
            </div>
            <br />
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <div class="txt_gral">
                <table id="tabla_resultados"></table>
            </div>
            <br />
            <br />
            <br />
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <div class="txt_gral">
                ¿El Programa de Corrección cumple con todos los requisitos para ser considerado como Programa de Corrección?
            <input type="radio" name="CumpleCKL" value="1" onclick="chkS();">Sí
            <input type="radio" name="CumpleCKL" value="0" onclick="chkN();" checked>No
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <br />
            <br />
            <div class="txt_gral">
                <label class="">Motivo de No cumplimiento </label>
                <br />
                <asp:TextBox ID="txtMotivoNo" runat="server" MaxLength="500" onkeyup="validaLimiteLongitud(this,500)" TextMode="MultiLine" Height="50px" Width="584px"></asp:TextBox>
                <asp:Label runat="server" ID="lblComentariosCaracteres" CssClass="txt_gral" Text="500" ClientIDMode="Static"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMotivoNo" ErrorMessage="*" ForeColor="Red" ValidationGroup="ucResolucionNO">&nbsp;</asp:RequiredFieldValidator>
            </div>
            <br />
            <br />
            <asp:Panel ID="pnlResolucion" runat="server">
                <div class="txt_gral">
                    <samp style="color: red; font-size: 1.3em"><b />*<b /></samp>Resolución para el programa de corrección&nbsp;&nbsp;
                    <asp:DropDownList ID="ddl_Resolucion" runat="server">
                        <asp:ListItem Value="0">- Seleccione -</asp:ListItem>
                        <asp:ListItem Value="1">No procede</asp:ListItem>
                        <asp:ListItem Value="2">Procede</asp:ListItem>
                        <asp:ListItem Value="3">Procede con plazo</asp:ListItem>
                        <asp:ListItem Value="4">No presentado</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddl_Resolucion" ErrorMessage="Seleccione una resolución." ForeColor="Red" ValidationGroup="ucResolucion" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                    <br />
                    <br />
                    Comentarios de la resolución:
                    <br />

                    <asp:TextBox ID="txt_ComentariosResolucion" runat="server" TextMode="MultiLine" Height="50px" Width="584px" onkeyup="validaLimiteLongitud(this,600)" MaxLength="600"></asp:TextBox>
                    <div style="width: 584px; text-align: right">
                        <asp:Label runat="server" ID="lblCarAsunto" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                        <asp:Label runat="server" ID="lbl_Txt2" CssClass="txt_gral" Text="600" ClientIDMode="Static"></asp:Label>
                    </div>

                </div>
            </asp:Panel>


            <asp:Panel ID="pnl_Comentarios" runat="server" Visible="false">
                <div class="txt_gral">
                    Comentarios  adicionales:
                    <br />

                    <asp:TextBox ID="txt_ComentariosAdicionales" runat="server" TextMode="MultiLine" Height="50px" Width="584px" onkeyup="validaLimiteLongitudComentarios(this,300)" MaxLength="300"></asp:TextBox>
                    <div style="width: 584px; text-align: right">
                        <asp:Label runat="server" ID="lbl_Comentarios_1" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                        <asp:Label runat="server" ID="lbl_Comentarios_2" CssClass="txt_gral" Text="600" ClientIDMode="Static"></asp:Label>
                    </div>

                </div>
            </asp:Panel>

        </td>

    </tr>
    <tr>
        <td>
            <uc1:RequerimientoInformacion runat="server" ID="RequerimientoInformacion" Visible="false" />
        </td>

    </tr>

</table>

<br />

<br />
<br />

<div id="divSubEntidadesSeleccion" style="display: none">
    <table width="100%">
        <tr>
            <td style="width: 50px; text-align: center; vertical-align: top">
                <asp:Image ID="Image3" runat="server" Width="32px" Height="32px"
                    ImageUrl="~/Imagenes/Errores/Error1.png" />
            </td>
            <td style="text-align: left">
                <div class="MensajeModal-UI">
                    <div id="divSubEtidadesText">
                    </div>

                    <table id="Tabla_SubEntidadesMenj" style="width: 100%"></table>
                </div>
            </td>
        </tr>
    </table>
</div>
