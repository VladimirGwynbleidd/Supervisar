<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleVisita.aspx.vb" Inherits="SEPRIS.DetalleVisita" MasterPageFile="~/Master/SiteInterno.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/Controles/CopiarFolios.ascx" TagPrefix="ccf" TagName="CopiarFolios" %>
<%@ Register Src="~/Controles/SubVisitas.ascx" TagPrefix="sb" TagName="SubVisitas" %>
<%@ Register Src="~/Controles/cuDetalleVisita.ascx" TagPrefix="cuv" TagName="cuDetalleVisita" %>
<%@ Register Src="~/Controles/ucDocumentos.ascx" TagPrefix="ucd" TagName="ucDocumentos" %>
<%@ Register Src="~/Visita/UserControls/BitacoraVisita.ascx" TagPrefix="BitacoraVisita" TagName="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/TabsV1.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" />
    <meta http-equiv="Pragma" content="no-cache"/>
    <meta http-equiv="expires" content="0"/>
    <meta http-equiv="Cache-Control" content="no-cache"/>

    <script language="javascript" type="text/javascript">
        //Detectar navegador
        var nav = navigator.userAgent.toLowerCase();

        //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
        if (nav.indexOf("msie") != -1) {
            document.write("<link href='../Styles/estilosIE.css' rel='stylesheet' />");
        } else {
            document.write("<link href='../Styles/estilosChrome.css' rel='stylesheet' />");
        }

        var validNavigation = true;

        function wireUpEvents() {
            window.onbeforeunload = LiberarVisita;

            $("input").bind("click", function () {
                validNavigation = false;
            });

            $("td .txt_submenu").bind("click", function () {
                if ($("td").hasClass("txt_submenu"))
                    validNavigation = true;
            });

            $("td .txt_menu").bind("click", function () {
                if ($("td").hasClass("txt_menu"))
                    validNavigation = true;
            });

        }

        //// Wire up the events as soon as the DOM tree is ready
        $(document).ready(function () {
            wireUpEvents();
        });


        //EVENTO PARA ANTES DE CERRAR LA VENTANA EN EL NAVEGADOR
        function LiberarVisita() {

            if (validNavigation) {
                var idVisita = '<%= Me.AuxIdVisitaGenerado%>';
                var idUsuOcupa = '<%= Me.AuxUsuarioEstaOcupando%>';
                var idUsuLogin = '<%= Me.AuxIdentificadorUsuario%>';

                if (idUsuLogin != idUsuOcupa)
                    return;

                if (idVisita != '') {
                    $.ajax({
                        type: "POST",
                        url: "DetalleVisita.aspx/LiberaVisitaShared",
                        data: "{'idVisita':'" + idVisita + "', 'idUsuOcupa':'" + idUsuOcupa + "', 'idUsuLogin':'" + idUsuLogin + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d != 'OK')
                                alert('No se pudo liberar la visita, [' + response.d + ']');
                        },
                        error: function (result) {
                            //alert('No se pudo liberar la visita o subvisitas, [' + result.status + ' ' + result.statusText + ']');
                        }
                    });
                }
            }

            return;
        }

        function unformatNumberDecimal(num) {
            var numero = num.replace(/([^0-9\.\-])/g, '');

            if (numero.length > 1) {
                if (numero.substring(0, 1) == 0) {
                    numero = numero.substring(1);
                }
            }

            return numero
        }

        function validateCustomFloatMoneda(ctrl, evt, ints, decimals) {
            var maxlength = new Number(ints) + new Number(decimals) + 1;
            ctrl.value = unformatNumberDecimal(ctrl.value)

            var regreso = validateCustomFloat(ctrl, evt, ints, decimals)

            if (ctrl.value.length > maxlength) {
                ctrl.value = ctrl.value.substring(0, maxlength);
            }
            ctrl.value = formatNumber(unformatNumberDecimal(ctrl.value), '$')

            return regreso
        }

        function formatNumber(num, prefix) {
            prefix = prefix || '';
            num += '';
            var splitStr = num.split('.');
            var splitLeft = splitStr[0];
            var splitRight = splitStr.length > 1 ? '.' + splitStr[1] : '';
            var regx = /(\d+)(\d{3})/;
            while (regx.test(splitLeft)) {
                splitLeft = splitLeft.replace(regx, '$1' + ',' + '$2');
            }
            return prefix + splitLeft + splitRight;
        }

        function unformatNumber(num) {
            var maxlength = 15
            var numero = num.replace(/([^0-9\.\-])/g, '');
            if (numero.length > maxlength) {
                numero = numero.substring(0, maxlength);
            }
            //return num.replace(/([^0-9\.\-])/g,'')*1;
            return numero * 1
        }

        function validateCustomFloat(ctrl, evt, ints, decimals) {


            // Validamos que lo que se captura sea permitido
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57)) {
                return false;
            }

            // Sólo un punto
            if (charCode == 46) {
                if (ctrl.value.indexOf(".") !== -1) {
                    return false;
                }

                return true;
            }

            // Obtenemos localizacion del punto
            var indicePunto = ctrl.value.indexOf(".");

            // Determinamos si hay decimales o no
            if (indicePunto == -1) {
                // Si no hay decimales, directamente comprobamos que el número que hay ya supero el número de enteros permitidos
                if (ctrl.value.length >= ints) {
                    return false;
                }
            }
            else {

                // determinamos longitud total cadena capturada
                var total = ctrl.value.length;

                // Obtenemos los decimales ya capturados
                var decimales = ctrl.value.substr(indicePunto + 1, total - (indicePunto + 1));

                // validamos que el numero de decimales sea menor al proporcionado
                if (decimales.length < decimals)
                    return true;
                else
                    return false;


            }

            return true;
        }

        function validaLimite(obj, maxchar) {
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

        function validaLimiteProrroga(obj, maxchar) {
            if (this.id) obj = this;
            var remaningChar = maxchar - obj.value.length;
            //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
            $("#<%=lblProrrogaCaracteres.ClientID%>").text("" + remaningChar);

            if (remaningChar <= 0) {
                //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                $("#<%=lblProrrogaCaracteres.ClientID%>").text("" + 0);
                obj.value = obj.value.substring(maxchar, 0);
                return false;
            }
            else { return true; }
        }

        function validaLimiteCancelacion(obj, maxchar) {
            if (this.id) obj = this;
            var remaningChar = maxchar - obj.value.length;
            //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
            $("#<%=lblCancelacionCaracteres.ClientID%>").text("" + remaningChar);

            if (remaningChar <= 0) {
                //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                $("#<%=lblCancelacionCaracteres.ClientID%>").text("" + 0);
                obj.value = obj.value.substring(maxchar, 0);
                return false;
            }
            else { return true; }
        }

        function ValidateText(obj) {
            if (obj.value.length > 0) {
                obj.value = obj.value.replace(/[^\d]+/g, '');
            }
        }

        function ShowProcesa() {

            if (Page_ClientValidate()) {
                $find('mpeProcesa').show();
                return true;
            }
            else {
                return false;
            }
        }


        function PostBackBoton() {
            return true;
        }


        function LevantaVentanaOficio2(EsReemplazo) {

            var url = '';

            url = '../../Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';

            //winprops = "dialogHeight: " + parseInt(document.body.clientHeight * 1.20).toString() + "px; width: " + parseInt(document.body.clientWidth * 1).toString() + "px; edge: Raised; center: Yes; help: No;resizable: No; status: No; Location: No; Titlebar: No;"
            winprops = "width= " + parseInt(document.body.clientWidth * 1).toString() + ",height=" + parseInt(document.body.clientHeight * 1.20).toString() + ",resizable=NO, toolbar=NO, menubar=NO"
            window.open(url, "", winprops);

            return true;
        }

        function LevantaVentanaOficio(EsReemplazo) {

            var url = '';
            url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';
            //winprops = "dialogHeight: 450px; dialogWidth: 800px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;"
            winprops = "width= " + parseInt(document.body.clientWidth * 1).toString() + ",height=" + parseInt(document.body.clientHeight * 1.20).toString() + ",resizable=NO, toolbar=NO, menubar=NO"
            window.open(url, "SICOD", winprops);

            //window.showModalDialog(url, "", winprops);

            return true;
        }


        mainJquery();
        //para que funcione enpostBacks de updatepanels.
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                mainJquery();
            }
        }

        //coloca el contenido a mostar. 
        function setActiveTab(tabId) {

            //NHM - si no selecciona un tab, el tabId es undefinded, entonces le asigna por default el tabId: li1 (Registro de visita)
            if (typeof tabId === "undefined") {
                tabId = "li1";
            }
            // NHM - fin

            var str = "";
            var idPanel = "";

            str = $("#" + tabId).find("a").attr("href");

            if (str != null) {
                idPanel = str.substring(str.lastIndexOf("#"));

                $("#hfCurrentTab").val(tabId);

                $("ul.tabs li").removeClass("active");
                $("#" + tabId).addClass("active");
                $(".tab_content").hide();
                $(idPanel).fadeIn();
            }
            return false;
        }

        function mainJquery() {
            $(document).ready(function () {
                $("#imgRegresarDocs").click(function () {
                    setActiveTab("li1");
                });

                $("#imgRevisarDocs").click(function () {
                    setActiveTab("li3");
                });

                //Evento para el clic en los tabs. 
                $("ul.tabs li").click(function () {
                    setActiveTab($(this).attr("id"));

                    if ($(this).attr("id") == "li_tab5")
                        $("#btnSeguimiento1").trigger("click");
                });

                //Maneja que no sepierdan los postbacks de la pagina, se guarda el tab actual en el campo oculto con id: MainContent_hfCurrentTab, 
                // y muestra esa tab cuando se refresco la pantalla entera. 
                if ($("#hfCurrentTab").val() != "") {
                    setActiveTab($("#hfCurrentTab").val());
                } else {
                    //NHM li1
                    setActiveTab("li1");
                }
            });
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div style="float: left; width: 92%; text-align: center; position: absolute">
        <asp:Label ID="lblFolioVisita" runat="server" Text="" Style="font-weight: bold; font-size: 13px; color: black;"></asp:Label>
        <br />
        <asp:Label ID="lblPasoVisita" runat="server" Text="" Style="font-weight: bold; font-size: 9px; color: black;"></asp:Label>
    </div>

    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">

        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1">Detalle de Visita</a></li>
            <li id="li2" style="display: none;"><a href="#tab2">Expediente</a></li>
            <li id="li3"><a href="#tab3">Expediente Documentos</a></li>
             <li id="li4"><a href="#tab4">Bitácora de acciones</a></li>
        </ul>

        <div style="float: left; width: 100%; text-align: right; margin: -50px 150px 0px 0px;">
            <asp:ImageButton ID="imgSubvisitas" ToolTip="Separar Subvisitas" runat="server" ImageUrl="../Imagenes/SepararSubvisitas.png" OnClientClick="return MuestraSubvisitas('imgSubvisitas');" Width="38px" />
            <asp:ImageButton ID="imgCancelarVisita" ToolTip="Cancelar visita" runat="server" ImageUrl="../Imagenes/cancelarVisita.png" />
            <asp:ImageButton ID="imgSolicitarProrroga" ToolTip="Solicitar una prórroga" runat="server" ImageUrl="../Imagenes/solicitarProrroga.png" />
            <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick="return MuestraProcesoInspeccion();" />
        </div>
    </div>
    <div style="width: 100%; float: left;">
        <!--NHM inicia -->
        <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <div class="divSnMargen">
                <sb:SubVisitas runat="server" ID="sbSubVisitas" />
                <ccf:CopiarFolios runat="server" ID="ccfCopiarFolios" />
            </div>

            <ajx:TabContainer ID="idContPestanias" runat="server" Width="90%" Font-Bold="false" ActiveTabIndex="0"
                Style="text-align: left">
                <ajx:TabPanel HeaderText="Principal" runat="server" CssClass="tabPestanias" TabIndex="0" ID="tpPestaniaP">
                    <ContentTemplate>
                        <cuv:cuDetalleVisita runat="server" ID="cuDetallePrincipal" />

                    
</ContentTemplate>
                
</ajx:TabPanel>
            </ajx:TabContainer>
            <br />
            <br />
            <br />
            <hr />
            <br />
            <%--asignar abogados--%>
            <table id="t_ddlAbogadoAsignado" runat="server" visible="false" width="60%">
                <tr>
                    <td style="height: 35px; text-align: left;" colspan="4">
                        <%--<asp:DropDownList ID="ddlAbogadoAsignado" runat="server" Width="350px" style="font-size:11px;"></asp:DropDownList>--%>
                        <asp:UpdatePanel runat="server" ID="udpAsignacionUsuarios" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tbAsignacionInspectores" runat="server" style="width: 100%; margin-top: 20px; border-collapse: collapse;">
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <asp:Label ID="lblTituloAbogado" runat="server" Text="Asignar abogados:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: bottom; text-align: center; width: 45%">Usuarios Disponibles:</td>
                                        <td style="vertical-align: bottom; text-align: center; width: 10%"></td>
                                        <td style="vertical-align: bottom; text-align: center; width: 45%">Usuarios Asignados:</td>
                                    </tr>
                                    <tr id="trRenSupTit" runat="server">
                                        <td colspan="3" align="center">Supervisor(es):</td>
                                    </tr>
                                    <tr id="trRenSupUsu" runat="server">
                                        <td rowspan="2" style="vertical-align: middle;">
                                            <asp:ListBox ID="lsbUsuariosDisponibles" runat="server" Width="100%" Height="110px"></asp:ListBox>
                                        </td>
                                        <td style="vertical-align: bottom; text-align: center;">
                                            <asp:ImageButton ID="imgAsignarSupervisor" runat="server" ImageUrl="../Imagenes/FlechaRojaDer.gif" />
                                        </td>
                                        <td rowspan="2" style="vertical-align: bottom;">
                                            <asp:ListBox ID="lsbSupervisor" runat="server" Width="100%" Height="110px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr id="trRenSupFle" runat="server">
                                        <td style="vertical-align: top; text-align: center;">
                                            <asp:ImageButton ID="imgDesasignarSupervisor" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblAsignaAbogado" runat="server" Text="Asesor(es):"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td rowspan="2" style="vertical-align: middle;">
                                            <asp:ListBox ID="lsbAbogadoDisponibles" runat="server" Width="100%" Height="110px"></asp:ListBox>
                                        </td>
                                        <td style="vertical-align: bottom; text-align: center">
                                            <asp:ImageButton ID="imgAsignarAbogado" runat="server" ImageUrl="../Imagenes/FlechaRojaDer.gif" />
                                        </td>
                                        <td rowspan="2" style="vertical-align: top;">
                                            <asp:ListBox ID="lsbAbogado" runat="server" Width="100%" Height="110px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: center;">
                                            <asp:ImageButton ID="imgDesasignarAbogado" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <!-- Tabla días transcurridos de la visita in situ-->

            <table id="tbDiasTranscurridosVisitaInSitu" runat="server" visible="false" width="40%">
                <tr>
                    <td align="left">Días transcurridos de visita in situ:
                    </td>
                    <td>
                        <asp:Label ID="lblDiasTranscurridosVisitaInSitu" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!-- Tabla días transcurridos de diagnostico hallazgos-->
            <table id="tbDiasTranscurridosDiagnosticoHallazgos" runat="server" visible="false" width="40%">
                <tr>
                    <td align="left">Días transcurridos en diagnóstico hallazgos:
                    </td>
                    <td>
                        <asp:Label ID="lblDiasTranscurridosDiagnosticoHallazgos" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <table id="tbHallazgozInt" runat="server" visible="false" width="40%">
                <tr>
                    <td align="left">La presentación de hallazgos interna será:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditAllazgosInt" runat="server" onkeydown="return false;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnEditAllazgosInt" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" OnClientClick="return MuestraSubvisitas('btnEditAllazgosInt');" Width="20px" />
                    </td>
                </tr>
            </table>

            <table id="tbHallazgozExt" runat="server" visible="false" width="40%">
                <tr>
                    <td align="left">La presentación de hallazgos externa será:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditAllazgosExt" runat="server" onkeydown="return false;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnEditAllazgosExt" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" OnClientClick="return MuestraSubvisitas('btnEditAllazgosExt');" Width="20px" />
                    </td>
                </tr>
            </table>

            <!--Se inserta el boton de Vulnerabilidad -->
            <table id="tbVulnerabilidad" runat="server" visible="false" width="40%">
                <tr>
                    <td align="left">Fecha agendada de la reunión será:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaVulnerabilidad" runat="server" onkeydown="return false;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgFecVulnerabilidad" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" OnClientClick="return SolicitarFechaVulnerabilidad();" Width="20px" />
                    </td>
                </tr>
            </table>


            <!-- Tabla días transcurridos posteriores de haber recibido el pago -->
            <table id="tbDiasTranscurridosPosterioresPago" runat="server" visible="false" width="40%">
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 35px; width: 50%; text-align: left;">Estatus:
                    </td>
                    <td style="height: 35px; width: 50%; text-align: left;">Oficio Sanción - Pagado
                    </td>
                </tr>
                <tr>
                    <td style="height: 35px; width: 50%; text-align: left;">Los días transcurridos desde el pago de la sanción son:
                    </td>
                    <td style="height: 35px; width: 50%; text-align: left;">
                        <asp:Label ID="lblDiasTranscurridosPosterioresPago" runat="server" Text=""></asp:Label>
                        <%--<asp:TextBox ID="txtDiasTranscurridosPosterioresPago" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>

            <!-- Tabla de opciones que tiene la afore para solicitar -->
            <table id="tbAforeSolicita" runat="server" visible="false" width="40%">
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" align="left">Afore Solicitó:
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 33%;" align="left">
                        <asp:RadioButton ID="rdbRecurosRevocacion" runat="server" Text="Recurso de Revocación" GroupName="grpRdbOpcionesSolicitarAfore" />
                    </td>
                    <td style="width: 34%;" align="center">
                        <asp:RadioButton ID="rdbJuicioNulidad" runat="server" Text="Juicio de Nulidad" GroupName="grpRdbOpcionesSolicitarAfore" />
                    </td>
                    <td style="width: 33%;" align="right">
                        <asp:RadioButton ID="rdbNinguno" runat="server" Text="Ninguno" GroupName="grpRdbOpcionesSolicitarAfore" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblMensajeSolicitaAforeObligatorio" runat="server" Text="*Favor de seleccionar una opción." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
            </table>

            <!-- Tabla para comentarios y botones -->
            <div id="divComentarios" runat="server" visible="false" style="width: 100%;">
                <table width="100%" style="width: 80%; margin-top: 10px;">
                    <tr>
                        <td colspan="5" style="width: 100%;" align="center">
                            <br />
                            <div style="width: 100%; text-align: left;">
                                <asp:Label runat="server" ID="lblComentarios" Text="Comentarios:"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="width: 100%;" align="center">
                            <asp:TextBox ID="txbComentarios" runat="server" onkeyup="validaLimite(this,600)" TextMode="MultiLine" Width="99%" Height="70px" MaxLength="600"
                                onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblMensajeComentarioObligatorio" runat="server" Text="*Favor de ingresar los comentarios." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center" style="width: 100%;">
                            <div id="Div2" runat="server" style="width: 100%; text-align: right;">
                                <asp:Label runat="server" ID="lblCarComentarios" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                <asp:Label runat="server" ID="lblComentariosCaracteres" CssClass="txt_gral" Text="600"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;</td>
                    </tr>
                </table>
            </div>

            <!-- Tabla para botones -->
            <table id="tbBotones">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: center">
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgDetener');" ID="imgDetener" ToolTip="Finalizar visita in situ" runat="server" ImageUrl="../Imagenes/detener.png" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgNotificar3');" ID="imgNotificar3" ToolTip="Enviar correo de notificación" runat="server" ImageUrl="../Imagenes/notificar_3.png" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgGuardarCambios');" ID="imgGuardarCambios" runat="server" ImageUrl="../Imagenes/registrarVisita.png" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgNotificar');" ID="imgNotificar" runat="server" ImageUrl="../Imagenes/aceptarPaloma.png" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgAnterior');" ID="imgAnterior" ToolTip="Rechazar Documentos" runat="server" Width="38px" ImageUrl="../Imagenes/RechazarDocto.png" />
                        <asp:ImageButton ID="imgInicio" ToolTip="Ir a página principal" runat="server" ImageUrl="../Imagenes/inicio.png" OnClientClick="MuestraImgCarga(this);"/>
                        <asp:ImageButton ID="imgRevisarDocs" runat="server" ImageUrl="../Imagenes/IrADetallevisita.png" ClientIDMode="Static" OnClientClick="javascript:return false;" Width="38px" ToolTip="Ver expediente de la visita" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgSiguiente');" ID="imgSiguiente" ClientIDMode="Static" ToolTip="Aceptar comentarios y avanzar a paso siguiente" runat="server" ImageUrl="../Imagenes/siguiente.png" Width="38px" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgNotificar2');" ID="imgNotificar2" runat="server" ImageUrl="../Imagenes/reload.png" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgIniciarVisita');" ID="imgIniciarVisita" ToolTip="Iniciar visita in situ" runat="server" ImageUrl="../Imagenes/IniciarVisita.png" Width="38px" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgRechazarProrroga');" ID="imgRechazarProrroga" ToolTip="Rechazar prórroga" runat="server" ImageUrl="../Imagenes/RechazarProrroga.png" Width="38px" />
                        <asp:ImageButton OnClientClick="MuestraImgCarga(this); return MuestraSubvisitas('imgAprobarProrroga');" ID="imgAprobarProrroga" ToolTip="Aprobar prórroga" runat="server" ImageUrl="../Imagenes/aprobarProrroga.png" Width="38px" />
                    </td>
                </tr>
            </table>
        </div>
        <!-- NHM fin ya no se usa -->
        <div id="tab2" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto; display: none;">
            <br />
            <br />
            <center>
                <asp:Label runat="server" ID="Label2" Text="Adjuntar Documentos" CssClass="TitulosWebProyectos"></asp:Label>
            </center>
        </div>

        <div id="tab3" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <br />
            <br />

            <ajx:TabContainer ID="tcContPestaDocs" runat="server" Width="90%" Font-Bold="false" ActiveTabIndex="0"
                Style="text-align: left">
                <ajx:TabPanel HeaderText="Principal" runat="server" CssClass="tabPestanias" TabIndex="0" ID="tpPestaniasDocsP">
                    <ContentTemplate>
                        <ucd:ucDocumentos runat="server" ID="ucDocumentos" />

                    
</ContentTemplate>
                
</ajx:TabPanel>
            </ajx:TabContainer>
            <br />
            <br />
        </div>

        <div id="tab4" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <BitacoraVisita:uc6 ID="BitacoraVisita1" runat="server" />
        </div>
        <br />
        <br />
    </div>

    <asp:HiddenField ID="hfCurrentTab" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="hfFolioVisita" runat="server" />
    <asp:HiddenField ID="hfEstaEnProrroga" runat="server" />

    <div id="divMensajeDosBotonesUnaAccion_CancelarVisita" style="display: none;" title="Solicitud de cancelación de visita">
        <table width="100%">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txbMotivoCancelacion" runat="server" onkeyup="validaLimiteCancelacion(this,250)" TextMode="MultiLine" Width="90%" Height="70px" MaxLength="600" Font-Size="Small"
                        onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                    <br />
                    <div id="ast1" runat="server" class="AsteriscoHide" style="font-size: small; font-style: normal;">* Favor de ingresar el motivo de cancelación</div>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%;">
                    <div id="Div1" runat="server" style="width: 90%; text-align: right;">
                        <asp:Label runat="server" ID="Label20" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                        <asp:Label runat="server" ID="lblCancelacionCaracteres" CssClass="txt_gral" Text="250"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center" style="width: 100%;"></td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionCancelar() {
            MensajeDosBotonesUnaAccion_CancelarVisita();
        }
    </script>

    <div id="divMensajeDosBotonesUnaAccion_SolicitarProrroga" style="display: none;" title="Solicitud de prórroga">
        <table width="100%">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txbMotivoProrroga" runat="server" onkeyup="validaLimiteProrroga(this,250)" TextMode="MultiLine" Width="90%" Height="70px" MaxLength="600" Font-Size="Small"
                        onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                    <br />
                    <div id="ast2" runat="server" class="AsteriscoHide" style="font-size: small; font-style: normal;">* Favor de ingresar el motivo de la prórroga</div>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%;">
                    <div id="Div3" runat="server" style="width: 90%; text-align: right;">
                        <asp:Label runat="server" ID="Label21" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                        <asp:Label runat="server" ID="lblProrrogaCaracteres" CssClass="txt_gral" Text="250"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center" style="width: 100%;"></td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionProrroga() {
            MensajeDosBotonesUnaAccion_SolicitarProrroga();
        }
    </script>

    <div id="divMensajeUnBotonNoAccion" style="display: none" title="Información">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                        <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function AquiMuestroMensaje() {
            MensajeUnBotonNoAccion();
        };

        function AvisoCatalogoDoc() {
            Confirmacion("divMensajeUnBotonNoAccion", "imgRevisarDocs", 500, 300);
        };
    </script>


    <div id="divMensajeDosBotonesUnaAccion_Redirect" style="display: none;" title="">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionRedirect() {
            MensajeDosBotonesUnaAccion_Redirect();
        }
    </script>

    <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnCancelarM2B1A" Style="display: none" ClientIDMode="Static" />

    <div id="divMensajeDosBotonesUnaAccion_ConfirmarFechaInicioVisita" style="display: none;" title="Confirmación">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image2" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        La fecha de inicio de la visita de inspección es:
                    </div>
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txtFecIniVista" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="calExFecIni" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendarFecIni"
                            TargetControlID="txtFecIniVista" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendarFecIni" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblErrorFecIni" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function MensajeConfirmacionFechaInicioVisita() {
            MensajeDosBotonesUnaAccion_ConfirmarFechaInicioVisita();
        }
    </script>


    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaFinVisita" style="display: none;" title="Visita in situ">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image3" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Si deseas terminar el conteo de los días de la visita in situ y comenzar el diagnóstico de hallazgos, por favor ingresa la fecha fin de la visita in situ en el siguiente espacio: 
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechFinVisita" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario2"
                            TargetControlID="txbFechFinVisita" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario2" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaFinVisitaObligatorio" runat="server" Text="*Favor de ingresar una fecha fin de la visita valida." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaFinVisita() {
            MensajeDosBotonesUnaAccion_SolicitarFechaFinVisita();
        }
    </script>



    <div id="divMensajeDosBotonesUnaAccion_SolicitarRangoSancion" style="display: none;" title="Solicitud de rango de sanción">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image5" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar el rango de sanción:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <table>
                            <tr>
                                <td style="text-align: right">Monto mínimo:</td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="txbRangoInicio" runat="server" MaxLength="14"
                                        onkeyup="return validateCustomFloatMoneda(this, event, 14, 2);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Monto máximo:</td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="txbRangoFin" runat="server" MaxLength="14"
                                        onkeyup="return validateCustomFloatMoneda(this, event, 14, 2);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label ID="Label22" runat="server" Text="Comentarios"></asp:Label>
                        <asp:TextBox ID="txtRango" runat="server" onkeyup="validaLimite(this,600)" TextMode="MultiLine" Width="85%" Height="70px" MaxLength="600"
                            onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblMensajeRangoSancionObligatorio" runat="server" Text="*Favor de ingresar el rango de sanción." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>


    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaEnviaDictamen" style="display: none;" title="Solicitud de fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image6" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha en que se envían los documentos a VJ:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechaEnvioDictamen" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario4"
                            TargetControlID="txbFechaEnvioDictamen" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario4" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaEnvioDictamenObligatorio" runat="server" Text="*Favor de ingresar la fecha en que se envían los documentos a VJ." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaEnviaDictamen() {
            MensajeDosBotonesUnaAccion_SolicitarFechaEnviaDictamen();
        }
    </script>



    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaPosibleEmplazamiento" style="display: none;" title="Solicitud de fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image7" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha posible de emplazamiento:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechaPosibleEmplazamiento" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario5"
                            TargetControlID="txbFechaPosibleEmplazamiento" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario5" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaPosibleEmplazamientoObligatorio" runat="server" Text="*Favor de ingresar la fecha posible de emplazamiento." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaPosibleEmplazamiento() {
            MensajeDosBotonesUnaAccion_SolicitarFechaPosibleEmplazamiento();
        }
    </script>


    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeRecibeOfEmpl" style="display: none;" title="Solicitud de fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image8" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha  del acuse  en que la afore recibe el oficio de emplazamiento:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechaAcuseAforeRecibeOfEmpl" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario6"
                            TargetControlID="txbFechaAcuseAforeRecibeOfEmpl" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario6" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio" runat="server" Text="*Favor de ingresar la fecha  del acuse  en que la afore recibe el oficio de emplazamiento." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaAcuseAforeRecibeOfEmpl() {
            MensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeRecibeOfEmpl();
        }
    </script>



    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeContestoOfEmpl" style="display: none;" title="Solicitud de fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image9" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha del acuse en que la AFORE contestó al oficio de emplazamiento:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechaAcuseAforeContestoOfEmpl" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario7"
                            TargetControlID="txbFechaAcuseAforeContestoOfEmpl" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario7" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaAcuseAforeContestoOfEmplObligatorio" runat="server" Text="*Favor de ingresar la fecha  del acuse en que la AFORE contestó al oficio de emplazamiento." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaAcuseAforeContestoOfEmpl() {
            MensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeContestoOfEmpl();
        }
    </script>



    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancion" style="display: none;" title="Fecha de imposición de la sanción">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image10" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar los datos de imposición de la sanción:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <table style="width: 100%;">
                            <tr>
                                <td>Fecha: </td>
                                <td>
                                    <asp:TextBox ID="txbFechaImposicionSancion" runat="server"></asp:TextBox>
                                    <ajx:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario8"
                                        TargetControlID="txbFechaImposicionSancion" CssClass="teamCalendar" />
                                    <asp:Image ID="imgCalendario8" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                                        ImageAlign="Bottom" Height="16px" />
                                </td>
                            </tr>
                            <tr>
                                <td>Monto:</td>
                                <td>
                                    <asp:TextBox ID="txtMontoImpSan" runat="server" MaxLength="14" onkeyup="return validateCustomFloatMoneda(this, event, 14, 2);"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="2">Comentarios:</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtComentImpSan" TextMode="MultiLine" Height="50px" Width="95%" runat="server"
                                        onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox></td>
                            </tr>
                        </table>




                        <br />
                        <asp:Label ID="lblMensajeFechaImposicionSancionObligatorio" runat="server" Text="*Favor de ingresar la fecha de imposición de la sanción." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaImposicionSancion() {
            MensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancion();
        }
    </script>


    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancionEstimada" style="display: none;" title="Solicitud de fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image11" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha de imposición de la sanción:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechaImposicionSancionEstimada" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender8" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario9"
                            TargetControlID="txbFechaImposicionSancionEstimada" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario9" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaImposicionSancionEstimadaObligatorio" runat="server" Text="*Favor de ingresar la fecha de imposición de la sanción." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaImposicionSancionEstimada() {
            MensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancionEstimada();
        }
    </script>


    <div id="divMensajeDosBotonesUnaAccion_SolicitarFechaAcusePagoAfore" style="display: none;" title="Solicitud de fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image12" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha de acuse del pago por parte de la afore:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txbFechaAcusePagoAfore" runat="server"></asp:TextBox>

                        <ajx:CalendarExtender ID="CalendarExtender9" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario10"
                            TargetControlID="txbFechaAcusePagoAfore" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario10" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblMensajeFechaAcusePagoAforeObligatorio" runat="server" Text="*Favor de ingresar la fecha de acuse del pago por parte de la afore." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function MensajeConfirmacionSolicitarFechaAcusePagoAfore() {
            MensajeDosBotonesUnaAccion_SolicitarFechaAcusePagoAfore();
        }
    </script>

    <%--Fecha de campo--%>
    <div id="divFechaCampo" style="display: none;" title="Visita in situ">
        <table width="100%">

            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image17" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        Favor de ingresar la fecha de inicio de la visita in situ:
                    </div>
                    <br />
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txtFechaCampo" runat="server"></asp:TextBox>
                        <ajx:CalendarExtender ID="ceFechaCampo" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFechaCampo"
                            TargetControlID="txtFechaCampo" CssClass="teamCalendar" />
                        <asp:Image ID="imgFechaCampo" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        <br />
                        <asp:Label ID="lblFechaCampo" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>


    <div id="divFechaReunion" style="display: none;" title="Fecha de Reunión con Presidencia">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image15" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: center; vertical-align: bottom;">
                    <div class="MensajeModal-UI">
                       Favor de ingresar la fecha para la reunión con Presidencia:
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center" colspan="2">
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txtFechaReunion" runat="server"></asp:TextBox>
                        <ajx:CalendarExtender ID="ceFechaReunion" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFechaReunion"
                        TargetControlID="txtFechaReunion" CssClass="teamCalendar" />
                        <asp:Image ID="imgFechaReunion" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                        ImageAlign="Bottom" Height="16px" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center" colspan="2">
                     <br />
                       <asp:Label ID="lblFechaReunion" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <%--imagen del proceso de sancion--%>
    <div id="divProcesoInspeccion" style="display: none;" title="Proceso Inspección – Sanción">
        <table width="100%">
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:Image ID="imgProcesoInspSancion" Width="98%" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <%--este div es generico para solicitar una fecha ya que la pantalla tiene muchas modales ya--%>
    <div id="divFechaGeneral" style="display: none;" title="Solicitud de Fecha">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgErrorGeneral" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: center; vertical-align: bottom;">
                    <div class="MensajeModal-UI" id="divTituloFechaGeneral" style="text-align: center;">
                        Favor de ingresar una fecha:
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center" colspan="2">
                    <div class="MensajeModal-UI">
                        <asp:TextBox ID="txtFechaGeneral" runat="server"></asp:TextBox>
                        <ajx:CalendarExtender ID="ceFechaGeneral" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFechaGeneral"
                            TargetControlID="txtFechaGeneral" CssClass="teamCalendar" />
                        <asp:Image ID="imgFechaGeneral" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center" colspan="2">
                    <br />
                    <asp:Label ID="lblFechaGeneral" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeConfirmaDetalleVisita" style="display: none" title="Confirme si existe Sanción">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgConfirmacion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeConfirmaSancion" style="display: none" title="Confirme si existe Sanción">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image1" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Existe sanción para la visita?
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divSubVisitas" style="display: none" title="Desmarque las subvisitas que desea separar" runat="server" clientidmode="Static">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image14" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:CheckBoxList ID="chkSubVisitasMod" runat="server"></asp:CheckBoxList>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function MuestraProcesoInspeccion() {
            var x = (screen.width - 30);
            var y = (screen.height - 100);

            Aviso("divProcesoInspeccion", x, y);
            return false;
        }

        function SolicitarFechaCampo() {
            MensajeDosBotonesUnaAccionAgc("divFechaCampo", "btnFechaCampo", 550, 420);
        }

        function SolicitarFechaReunionPresidencia() {
            MensajeDosBotonesUnaAccionAgc("divFechaReunion", "btnFechaReunion", 550, 420);
        }

        function SolicitarRangoSancion() {
            MensajeDosBotonesUnaAccionAgc("divMensajeDosBotonesUnaAccion_SolicitarRangoSancion", "btnRangoSancion", 550, 420);
        }

        function SolicitarRangoSancionPaso17() {
            MensajeDosBotonesUnaAccionAgc("divMensajeDosBotonesUnaAccion_SolicitarRangoSancion", "btnRangoSancionPaso17", 550, 420);
        }

        function SolicitarFechaReunionEntidad() {
            $("#divTituloFechaGeneral").text("Favor de ingresar la fecha para la reunión con la Afore y Presidencia:");
            MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnFechaAforePresi", 550, 370);
        }

        function ConfirmacionDetalleVisita() {
            $("#divMensajeConfirmaDetalleVisita").attr("title", "Confirme si existe Sanción");
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "btnConfirmarDv", "btnConfirmarNoDv", 1, 500, 300);
        }


        function PreguntarSancionPasoSiete() {
            $("#divMensajeConfirmaSancion").attr("title", "Confirme si existe Sanción");
            ConfirmacionSiNo("divMensajeConfirmaSancion", "btnConfSanPasoSieteSi", "btnConfSanPasoSieteNo", 1, 500, 300);
        }

        //RRA SI/NO VULNERABILIDAD
        function ConfirmacionFechaVulnerabilidad() {
            $("#divMensajeConfirmaDetalleVisita").attr("title", "Revisión de Vulnerabilidades");
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "btnConfirmacionVulnerabilidadSi", "btnConfirmacionVulnerabilidadNo", 1, 500, 300);
        }

        //RRA FECHA VULNERABILIDAD
        function SolicitarFechaVulnerabilidadRegresa() {
            //Inicializa controles y oculta imagenes y mensaje
            $("#MainContent_txtFechaGeneral").val("");
            $("#MainContent_lblFechaGeneral").hide();
            $("#divTituloFechaGeneral").text("Favor de ingresar fecha de revisión de vulnerabilidades");
            MensajeDosBotonesDosAccionRRA("divFechaGeneral", "btnGuardaFecVulnerabilidad", "btnCancelaFecVulnerabilidad", 550, 370);
            return false;
        }

        function SolicitarFechaVulnerabilidad() {
            //Inicializa controles y oculta imagenes y mensaje
            $("#MainContent_txtFechaGeneral").val("");
            $("#MainContent_lblFechaGeneral").hide();
            $("#MainContent_imgErrorGeneral").hide();
            $("#divTituloFechaGeneral").text("Favor de ingresar fecha de revisión de vulnerabilidades");
            MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnGuardaFecVulnerabilidad", 550, 370);
            return false;
        }
        // RRA TERMINA FECHA VULNERABILIDAD

        function MuestraSubvisitas(psBtnPresionado) {
            $("#hfBotonPresionado").val(psBtnPresionado);
            OcultaImagenCarga();
            Confirmacion("divSubVisitas", "btnSubvisitasModal", 550, 420);
            return false;
        }

        function ConfirmacionDocumentos(psIdDiv, psIdBtnSi, psIdBtnNo, piIdOp) {
            ConfirmacionSiNo(psIdDiv, psIdBtnSi, psIdBtnNo, piIdOp, 500, 300);
        }

        function AvisoDocumentos(psIdDiv) {
            Aviso(psIdDiv, 500, 300);
        }

        function SolicitarComentariosRechazoDocs(psNombreDiv, psNomBoton) {
            OcultaImagenCarga();
            MensajeDosBotonesUnaAccionAgc(psNombreDiv, psNomBoton, 550, 370);
        }

        function SolicitarFechaReunPreciJuridico() {
            $("#divTituloFechaGeneral").text("¿En qué fecha se llevará a cabo la reunión con la Vicepresidencia Jurídica?");
            MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnFechaReunionVj", 550, 370);
        }

        function SolicitarFechaReunPreciJuridicoP16() {
            $("#divTituloFechaGeneral").text("¿En qué fecha se llevará a cabo la reunión con la Vicepresidencia Jurídica?");
            MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnFechaReunionVjP16", 550, 370);
        }

        ///PASO 9 INICIA
        function ConfirmarFechaReunPreciJuridico() {
            $("#divTituloFechaGeneral").text("La fecha de la reunión con el área de Supervisión  fue:");
            //MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnConfFechaReunionVjp9", 550, 370);
            ConfirmacionSiNo("divFechaGeneral", "btnConfFechaReunionVjp9", "btnCancelaFechaReunionVjp9", 1, 550, 370);
            return false;
        }

        function ConfirmarFechaReunPreciJuridicoSiNo() {
            ConfirmacionvJ("divMensajeUnBotonNoAccion", "btnConfFechaReunionP9ok", "btnConfFechaReunionP9No", 1, "btnConfFechaReunionP9NoDos", 550, 370);
        }
        ///PASO 9 FIN

        ///PASO 16 INICIA
        function ConfirmarFechaReunPreciJuridicoP16() {
            $("#divTituloFechaGeneral").text("La fecha de la reunión con el área de Supervisión  fue:");
            //MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnConfFechaReunionVjp16", 550, 370);
            ConfirmacionSiNo("divFechaGeneral", "btnConfFechaReunionVjp16", "btnCancelaFechaReunionVjp16", 1, 550, 370);
        }

        function ConfirmarFechaReunPreciJuridicoP16SiNo() {
            ConfirmacionvJ("divMensajeUnBotonNoAccion", "btnConfFechaReunionVjpok", "btnConfFechaReunionVjp16No", 1, "btnConfFechaReunionVjp16NoDos", 550, 370);
        }
        ///PASO 16 FIN


        ///PASO 25 INICIA
        function ConfirmarFechaReunPaso25() {
            $("#divTituloFechaGeneral").text("La fecha de la reunión con el área de Supervisión  fue:");
            //MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnFechaPaso25Conf", 550, 370);
            ConfirmacionSiNo("divFechaGeneral", "btnFechaPaso25Conf", "btnCancelaFechaPaso25", 1, 550, 370);
        }

        function ConfirmarFechaReunPaso25SiNo() {
            ConfirmacionvJ("divMensajeUnBotonNoAccion", "btnFechaPaso25Ok", "btnFechaPaso25No", 1, "btnFechaPaso25NoDos", 550, 370);
        }
        ///PASO 25 FIN

        ///PASO 32 INICIA
        function ConfirmarFechaReunPaso32() {
            $("#divTituloFechaGeneral").text("La fecha de la reunión con el área de Supervisión  fue:");
            //MensajeDosBotonesUnaAccionAgc("divFechaGeneral", "btnFechaPaso32Conf", 550, 370);
            ConfirmacionSiNo("divFechaGeneral", "btnFechaPaso32Conf", "btnCancelaFechaPaso32", 1, 550, 370);
        }

        function ConfirmarFechaReunPaso32SiNo() {
            ConfirmacionvJ("divMensajeUnBotonNoAccion", "btnFechaPaso32Ok", "btnFechaPaso32No", 1, "btnFechaPaso32NoDos", 550, 370);
        }
        ///PASO 32 FIN

        function MuestraAlertaPaso18() {
            $("div#divMensajeConfirmaDetalleVisita").attr("title", "Alerta de recordatorio para adjuntar documentos");
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "imgRevisarDocs", "btnAlertaPaso18", 1, 550, 370);
        }

        function ModificarAbogadosVisita(psNomDiv, psNomBtnAceptar) {
            MensajeDosBotonesUnaAccionAgc(psNomDiv, psNomBtnAceptar, 650, 420);
        }

        function ModificarResponsablesVisita(psNomDiv, psNomBtnAceptar) {
            MensajeDosBotonesUnaAccionAgc(psNomDiv, psNomBtnAceptar, 600, 500);
        }

        function PreguntaReunionVjPresidencia() {
            $("div#divMensajeConfirmaDetalleVisita").attr("title", "Confirmación de reunión");
            //Habrá Presentación con VJ y Presidencia?
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "btnPasoSieteConfirmSi", "btnPasoSieteConfirmNoSnPostback", 1, 550, 370);
        }

        function ConfirmacionDeSancionPaso8() {
            $("#divMensajeConfirmaDetalleVisita").title = "Confirme si existe Sanción";
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "btnSancionPaso8Si", "btnSancionPaso8No", 1, 500, 300);
        }

        ///Funcion generica para solicitar una fecha, version FINAL
        function SolicitarFechaGeneral(psTextoMostrar, psNombreBoton) {
            $("#divTituloFechaGeneral").text(psTextoMostrar);
            MensajeDosBotonesUnaAccionAgc("divFechaGeneral", psNombreBoton, 550, 370);
        }

        //Pregunta en paso 10 si se requiere ingresar el rango se sancion
        function PreguntaPorSancionPasoDiez() {
            $("div#divMensajeConfirmaDetalleVisita").attr("title", "Confirmación de Sanción");
            //Habrá Presentación con VJ y Presidencia?
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "btnPasoDiezConfSancionSi", "btnPasoDiezConfSancionNo", 1, 550, 370);
        }
    </script>

    <asp:Button runat="server" ID="btnPasoDiezConfSancionNo" Style="display: none" ClientIDMode="Static" OnClick="btnPasoDiezConfSancionNo_Click" />
    <input id="btnPasoDiezConfSancionSi" type="hidden" onclick="return SolicitarRangoSancion();" />

    <asp:Button runat="server" ID="btnConfSanPasoSieteSi" Style="display: none" ClientIDMode="Static" OnClick="btnConfSanPasoSieteSi_Click" />
    <asp:Button runat="server" ID="btnConfSanPasoSieteNo" Style="display: none" ClientIDMode="Static" OnClick="btnConfSanPasoSieteNo_Click" />

    <asp:Button runat="server" ID="btnSancionPaso8Si" Style="display: none" ClientIDMode="Static" OnClick="btnSancionPaso8Si_Click" />
    <asp:Button runat="server" ID="btnSancionPaso8No" Style="display: none" ClientIDMode="Static" OnClick="btnSancionPaso8No_Click" />

    <asp:Button runat="server" ID="btnPasoSieteConfirmSi" Style="display: none" ClientIDMode="Static" OnClick="btnPasoSieteConfirmSi_Click" />
    <asp:Button runat="server" ID="btnPasoSieteConfirmNo" Style="display: none" ClientIDMode="Static" OnClick="btnPasoSieteConfirmNo_Click" />
    <input id="btnPasoSieteConfirmNoSnPostback" type="hidden" onclick="return PreguntarSancionPasoSiete();" />

    <asp:Button runat="server" ID="btnFechaPaso25Ok" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso25Ok_Click" />
    <input id="btnFechaPaso25No" type="hidden" onclick="ConfirmarFechaReunPaso25();" />
    <asp:Button runat="server" ID="btnFechaPaso25NoDos" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso25NoDos_Click" />

    <asp:Button runat="server" ID="btnFechaPaso32Ok" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso32Ok_Click" />
    <input id="btnFechaPaso32No" type="hidden" onclick="ConfirmarFechaReunPaso32();" />
    <asp:Button runat="server" ID="btnFechaPaso32NoDos" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso32NoDos_Click" />

    <asp:Button runat="server" ID="btnFechaPaso25" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso25_Click" />
    <asp:Button runat="server" ID="btnFechaPaso32" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso32_Click" />

    <asp:Button runat="server" ID="btnFechaPaso25Conf" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso25Conf_Click" />
    <asp:Button runat="server" ID="btnFechaPaso32Conf" Style="display: none" ClientIDMode="Static" OnClick="btnFechaPaso32Conf_Click" />

    <asp:Button runat="server" ID="btnConfFechaReunionP9ok" Style="display: none" ClientIDMode="Static" OnClick="btnConfFechaReunionP9ok_Click" />
    <input id="btnConfFechaReunionP9No" type="hidden" onclick="return ConfirmarFechaReunPreciJuridico();" />
    <asp:Button runat="server" ID="btnConfFechaReunionP9NoDos" Style="display: none" ClientIDMode="Static" OnClick="btnConfFechaReunionP9NoDos_Click" />

    <asp:Button runat="server" ID="btnAlertaPaso18" Style="display: none" ClientIDMode="Static" OnClick="btnAlertaPaso18_Click" />
    <asp:Button runat="server" ID="btnConfFechaReunionVjp9" Style="display: none" ClientIDMode="Static" OnClick="btnConfFechaReunionVjp9_Click" />
    <asp:Button runat="server" ID="btnCancelaFechaReunionVjp9" Style="display: none" ClientIDMode="Static" OnClick="btnCancelaFechaReunionVjp9_Click" />
    <asp:Button runat="server" ID="btnCancelaFechaReunionVjp16" Style="display: none" ClientIDMode="Static" OnClick="btnCancelaFechaReunionVjp16_Click" />
    <asp:Button runat="server" ID="btnCancelaFechaPaso25" Style="display: none" ClientIDMode="Static" OnClick="btnCancelaFechaPaso25_Click" />
    <asp:Button runat="server" ID="btnCancelaFechaPaso32" Style="display: none" ClientIDMode="Static" OnClick="btnCancelaFechaPaso32_Click" />

    <asp:Button runat="server" ID="btnConfFechaReunionVjp16" Style="display: none" ClientIDMode="Static" OnClick="btnConfFechaReunionVjp16_Click" />
    <input id="btnConfFechaReunionVjp16No" type="hidden" onclick="ConfirmarFechaReunPreciJuridicoP16();" />
    <asp:Button runat="server" ID="btnConfFechaReunionVjpok" Style="display: none" ClientIDMode="Static" OnClick="btnConfFechaReunionVjpok_Click" />
    <asp:Button runat="server" ID="btnConfFechaReunionVjp16NoDos" Style="display: none" ClientIDMode="Static" OnClick="btnConfFechaReunionVjp16NoDos_Click" />

    <asp:Button runat="server" ID="btnFechaInSituActaC" Style="display: none" ClientIDMode="Static" OnClick="btnFechaInSituActaC_Click" />
    <asp:Button runat="server" ID="btnFechaLevantaActa" Style="display: none" ClientIDMode="Static" OnClick="btnFechaLevantaActa_Click" />

    <asp:HiddenField ID="hfBotonPresionado" runat="server" Value="" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnSubvisitasModal" Style="display: none" ClientIDMode="Static" OnClientClick="MuestraImgCarga(this);" OnClick="btnSubvisitasModal_Click" />
    <asp:Button runat="server" ID="btnFechaCampo" Style="display: none" ClientIDMode="Static" OnClick="btnFechaCampo_Click" />
    <asp:Button runat="server" ID="btnFechaReunion" Style="display: none" ClientIDMode="Static" OnClick="btnFechaReunion_Click" />
    <asp:Button runat="server" ID="btnRangoSancion" Style="display: none" ClientIDMode="Static" OnClick="btnRangoSancion_Click" />
    <asp:Button runat="server" ID="btnRangoSancionPaso17" Style="display: none" ClientIDMode="Static" OnClick="btnRangoSancionPaso17_Click" />
    <asp:Button runat="server" ID="btnFechaAforePresi" Style="display: none" ClientIDMode="Static" OnClick="btnFechaAforePresi_Click" />
    <asp:Button runat="server" ID="btnConfirmarDv" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmarDv_Click" />
    <asp:Button runat="server" ID="btnConfirmarNoDv" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmarNoDv_Click" />
    <asp:Button runat="server" ID="btnFechaReunionVj" Style="display: none" ClientIDMode="Static" OnClick="btnFechaReunionVj_Click" />
    <asp:Button runat="server" ID="btnFechaReunionVjP16" Style="display: none" ClientIDMode="Static" OnClick="btnFechaReunionVjP16_Click" />
    
    <!-- RRA VULNERABILIDAD BOTONES SINO-->
    <asp:Button runat="server" ID="btnConfirmacionVulnerabilidadSi" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmacionVulnerabilidadSi_Click"/>
    <asp:Button runat="server" ID="btnConfirmacionVulnerabilidadNo" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmacionVulnerabilidadNo_Click"/>
    <!-- RRA FECHA VULNERABILIDAD -->
    <asp:Button runat="server" ID="btnGuardaFecVulnerabilidad" Style="display: none" ClientIDMode="Static" OnClick="btnGuardaFecVulnerabilidad_Click"/>
    <asp:Button runat="server" ID="btnCancelaFecVulnerabilidad" Style="display: none" ClientIDMode="Static" OnClick="btnCancelaFecVulnerabilidad_Click"/>
</asp:Content>
