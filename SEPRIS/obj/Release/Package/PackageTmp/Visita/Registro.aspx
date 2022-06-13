<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Registro.aspx.vb" Inherits="SEPRIS.Vista" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/Controles/CopiarFolios.ascx" TagPrefix="ccf" TagName="CopiarFolios" %>
<%@ Register Src="UserControls/ObjetoVisita.ascx" TagPrefix="uc" TagName="SelObjeto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/TabsV1.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <!--[if IE]>
       <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
    <![endif]-->


    <script language="javascript" type="text/javascript">

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

        function validaLimiteLongitud(obj, maxchar, labelContador) {
            if (this.id) obj = this;
            var remaningChar = maxchar - obj.value.length;
            //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
            $("#" + labelContador).text("" + remaningChar);

            if (remaningChar <= 0) {
                //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                $("#" + labelContador).text("" + 0);
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
            $find('mpeProcesa').show();
            return true;
        }

        function HideProcesa() {
            $find('mpeProcesa').hide();
            return true;
        }

        function PostBackBoton() {
            return true;
        }

        //$(document).ready(function () {
        //    MuestraImgCargaRegistro("imgDetener");
        //});

        function mainJquery() {
            $(document).ready(function () {

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
            idPanel = str.substring(str.lastIndexOf("#"));

            $("#hfCurrentTab").val(tabId);

            $("ul.tabs li").removeClass("active");
            $("#" + tabId).addClass("active");
            $(".tab_content").hide();
            $(idPanel).fadeIn();
            return false;
        }




    </script>

    <style type="text/css">
        .AsteriscoHide {
            display: none;
        }

        .AsteriscoShow {
            display: inline;
            color: red;
            font-family: Verdana;
            font-size: 1.2em;
            font-weight: bold;
        }

        .lblError {
            color: red;
            font-size: 20px;
        }

        .Oculto {
            display: none;
        }

        .FondoAplicacion {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>
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
            <li id="li1"><a href="#tab1" id="lnkTituloPag" runat="server">Registro de visita</a></li>
            <!--
            <li id="li2"><a href="#tab2">Expediente</a></li>
            <li id="li3"><a href="#tab3">Historial</a></li> 
            -->
        </ul>
    </div>


    <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
        <div style="float: left; width: 100%; text-align: right; margin: -50px 150px 0px 0px;">
            <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick="return MuestraProcesoInspeccion();" />
            <asp:ImageButton ID="imgProcesoVisitaAmbos" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
        </div>

        <asp:UpdatePanel runat="server" ID="upTabla">
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="imgRegistrarVisita" />--%>
                <%--<asp:AsyncPostBackTrigger ControlID="GridPrincipal" />--%>
            </Triggers>

            <ContentTemplate>
                <!-- Tabla para capturar los datos -->
                <table id="tbDatosCaptura" runat="server" style="width: 80%; margin-top: 20px;">
                    <tr>
                        <td style="height: 35px; width: 100%; text-align: right;" colspan="3">
                            <ccf:CopiarFolios runat="server" ID="ccfCopiarFolios" />
                            <div style="margin-left: 30px; float: right;">
                                <asp:CheckBox ID="ChkVConjunta" Text="Visita Conjunta" AutoPostBack="true" runat="server" />
                                <div style="text-align: left">
                                    <asp:CheckBoxList ID="chkListVC" Visible="false" AutoPostBack="true" runat="server"></asp:CheckBoxList>
                                </div>
                                <br />
                                <br />
                                <asp:Label ID="ejemplolabel" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Folio de visita:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:TextBox ID="txbFolioVisita" runat="server" Width="81%" ReadOnly="true" onkeydown="return false;"></asp:TextBox>
                            <div id="ast1" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Fecha de registro:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:TextBox ID="txbFechaRegistro" runat="server" Width="150px"></asp:TextBox>

                            <ajx:CalendarExtender ID="calendarioFechaRegistro" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario1"
                                TargetControlID="txbFechaRegistro" CssClass="teamCalendar" />
                            <asp:Image ID="imgCalendario1" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                                ImageAlign="Bottom" Height="16px" />
                            <div id="ast2" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Fecha de inicio de visita:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:TextBox ID="txbFechaInicio" runat="server" Width="150px"></asp:TextBox>
                            <ajx:CalendarExtender ID="calendarioFechaInicio" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario2"
                                TargetControlID="txbFechaInicio" CssClass="teamCalendar" />
                            <asp:Image ID="imgCalendario2" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                                ImageAlign="Bottom" Height="16px" />

                            <div id="ast3" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Tipo Entidad:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:DropDownList ID="ddlTipoEntidad" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>

                            <div id="ast9" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Entidad:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:DropDownList ID="dplEntidad" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>
                            <div id="ast4" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr id="trSubvisitas" runat="server" clientidmode="Static">
                        <td style="height: 35px; width: 25%; text-align: left;">Generar Subvisitas:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:CheckBox ID="chkSubVisitas" runat="server" onclick="return MuestraSubvisitas(this);" />
                        </td>
                    </tr>
                    <tr id="trSubentidad" runat="server" clientidmode="Static" class="Oculto">
                        <td style="height: 35px; width: 25%; text-align: left;">Sub entidad:</td>
                        <td style="height: 35px; width: 40%; text-align: left;">
                            <%--<asp:DropDownList ID="ddlSubentidad" runat="server" Width="81%"></asp:DropDownList>--%>
                            <%--<asp:ListBox ID="lstSubentidad" Visible ="false"  runat="server" Width="81%" Height="100px" SelectionMode="Multiple" ></asp:ListBox>--%>
                            <asp:CheckBoxList ID="chkSubEntidad" ClientIDMode="Static" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="chkSubEntidad_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </td>
                        <td style="text-align: left;">
                            <div id="ast10" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Tipo de Visita:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:DropDownList ID="dplTipoVisita" runat="server" Width="150px"></asp:DropDownList>

                            <div id="ast5" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <%--<td style="height: 35px; width: 25%; text-align: left;">Objeto de la Visita:</td>--%>
                        <td style="height: 35px; width: 40%; text-align: left;">
                            <%--<asp:DropDownList ID="ddlObjetoVisita" runat="server" Width="150px"></asp:DropDownList>--%>
                            <%--                        <asp:CheckBoxList ID="chkObjetoVisita" runat="server" onclick="javascript:HabilitaTxtDesdeLista(this);"></asp:CheckBoxList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <div id="ast12" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr id="trAcuerdoVul" runat="server" clientidmode="Static">
                        <td style="height: 35px; width: 25%; text-align: left;">Revisión de acuerdo de vulnerabilidades:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:TextBox ID="txtAcuerdoVul" runat="server" Width="150px"></asp:TextBox>
                            <ajx:CalendarExtender ID="calEAcuerdoVul" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgAcuerdoVul"
                                TargetControlID="txtAcuerdoVul" CssClass="teamCalendar" />
                            <asp:Image ID="imgAcuerdoVul" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                                ImageAlign="Bottom" Height="16px" />

                            <div id="divAcuerdoVul" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 25%; text-align: left;">Orden de visita:</td>
                        <td style="height: 35px; width: 75%; text-align: left;" colspan="2">
                            <asp:TextBox ID="txtOrdenVisita" runat="server" Width="81%" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)" TextMode="MultiLine"></asp:TextBox>
                            <div id="divOrdenVisita" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc:SelObjeto runat="server" ID="SelObjetos" />
        <!-- Tabla para seleccionar supervisores e inspectores -->
        <asp:UpdatePanel runat="server" ID="udpAsignacionUsuarios" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="tbAsignacionInspectores" runat="server" style="width: 80%; margin-top: 20px; border-collapse: collapse;">
                    <tr>
                        <td colspan="4" style="text-align: left;">Selecciona a los responsables de la inspección:</td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 40%;">Usuarios Disponibles</td>
                        <td style="width: 10%">&nbsp;</td>
                        <td style="text-align: left; width: 40%">Supervisor
                                        &nbsp;&nbsp;
                                        <div id="ast6" runat="server" class="AsteriscoHide">*</div>
                        </td>
                        <td style="width: 10%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td rowspan="5" style="vertical-align: middle;">
                            <asp:ListBox ID="lsbUsuariosDisponibles" runat="server" Width="100%" Height="230px"></asp:ListBox>
                        </td>
                        <td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgAsignarSupervisor" runat="server" ImageUrl="../Imagenes/FlechaRojaDer.gif" />
                        </td>
                        <td rowspan="2" style="vertical-align: bottom;">
                            <asp:ListBox ID="lsbSupervisor" runat="server" Width="100%" Height="110px"></asp:ListBox>
                        </td>
                        <td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgSubirSupervisor" runat="server" ImageUrl="../Imagenes/collapse.png" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarSupervisor" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                        </td>
                        <td style="vertical-align: top; text-align: center">
                            <asp:ImageButton ID="imgBajarSupervisor" runat="server" ImageUrl="../Imagenes/expand.png" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="text-align: left; height: 10px;">Inspector (es)
                                        &nbsp;&nbsp;
                                        <div id="ast7" runat="server" class="AsteriscoHide">*</div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: bottom; text-align: center">
                            <asp:ImageButton ID="imgAsignarInspector" runat="server" ImageUrl="../Imagenes/FlechaRojaDer.gif" />
                        </td>
                        <td rowspan="2" style="vertical-align: top;">
                            <asp:ListBox ID="lsbInspectores" runat="server" Width="100%" Height="110px"></asp:ListBox>
                        </td>
                        <td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgSubirInspector" runat="server" ImageUrl="../Imagenes/collapse.png" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarInspector" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                        </td>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgBajarInspector" runat="server" ImageUrl="../Imagenes/expand.png" />
                        </td>
                    </tr>
                </table>

                <!-- Tabla para abrir ventana de adjuntar oficios -->

                <table id="tbAgregarOficios" runat="server" style="border-top: 1px solid #999; width: 80%; margin-top: 20px; display: none;">
                    <tr>
                        <td style="width: 45%;">Agregar oficios de inicio de Visita:</td>
                        <td style="width: 55%; text-align: left;">&nbsp;</td>
                    </tr>
                </table>

                <%--Tabla Descripción de la Visita--%>

                <table id="tblDescripcionVisita" runat="server" style="width: 100%; margin-top: 20px;">
                    <tr>
                        <td colspan="5" style="width: 100%;" align="center">
                            <div style="width: 80%; text-align: left;">
                                <asp:Label runat="server" ID="Label5" Text="Descripción visita:"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="width: 100%;" align="center">
                            <asp:TextBox ID="txtDescripcionVisita" runat="server" onkeyup="validaLimiteDescripcion(this,500)" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)" TextMode="MultiLine" Width="80%" Height="70px" MaxLength="600"></asp:TextBox>
                            <div id="ast11" runat="server" class="AsteriscoHide" style="width: 15%;">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center" style="width: 100%;">
                            <div id="Div3" runat="server" style="width: 80%; text-align: right;">
                                <asp:Label runat="server" ID="lblCarDescripcion" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                <asp:Label runat="server" ID="lblDescripcionContador" CssClass="txt_gral" Text="500"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>

                <!-- Tabla para comentarios y botones -->
                <table id="tbComentarios" runat="server" style="width: 100%; margin-top: 20px;">
                    <tr>
                        <td colspan="5" style="width: 100%;" align="center">
                            <div style="width: 80%; text-align: left;">
                                <asp:Label runat="server" ID="lblComentarios" Text="Comentarios:"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="width: 100%;" align="center">
                            <asp:TextBox ID="txbComentarios" runat="server" onfocus="ReemplazaCEspeciales(this.id)" onkeyup="validaLimite(this,600)" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" TextMode="MultiLine" Width="80%" Height="70px" MaxLength="600"></asp:TextBox>
                            <div id="ast8" runat="server" class="AsteriscoHide" style="width: 15%;">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center" style="width: 100%;">
                            <div id="Div2" runat="server" style="width: 80%; text-align: right;">
                                <asp:Label runat="server" ID="lblCarComentarios" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                <asp:Label runat="server" ID="lblComentariosCaracteres" CssClass="txt_gral" Text="600"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>

        <table id="Table1" runat="server" style="width: 100%;">
            <tr>
                <td colspan="5" style="width: 100%; text-align: center;">
                    <asp:ImageButton ID="imgRegistrarVisita" runat="server" ImageUrl="../Imagenes/registrarVisita.png" OnClientClick="MuestraImgCargaRegistro(this);" ToolTip="Guardar datos para registro de visita" />
                    <asp:ImageButton ID="imgSiguiente" ToolTip="Aceptar comentarios y avanzar a paso siguiente" runat="server" ImageUrl="../Imagenes/siguiente.png" Visible="false" />
                    <asp:ImageButton ID="imgInicio" ToolTip="Ir a página principal" runat="server" ImageUrl="../Imagenes/inicio.png" OnClientClick="MuestraImgCargaRegistro(this);" />
                </td>
            </tr>
        </table>

    </div>

    <div style="height: 50px; float: left; width: 100%">
        &nbsp;
    </div>

    <asp:HiddenField ID="hfCurrentTab" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfMostrarMsg" runat="server" Value="0" ClientIDMode="Static" />

    <!--div y script para el modal que muestra mensajes de error o informativos -->

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
    </script>


    <div id="divMensajeDosBotonesUnaAccion" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
    <script type="text/javascript">
        function MensajeConfirmacion() {
            MensajeDosBotonesUnaAccion();
        }
    </script>

    <div id="dvModalImagenesProcesos" style="display: none" title="Procesos de Inspección - Sanción">
        <table width="100%">
            <tr>
                <td style="width: 25px;">&nbsp;
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:ImageButton ID="imgProcesoVF" runat="server" AlternateText="Proceso de VF" ToolTip="Proceso de VF" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick="return MuestraProcesoInspeccionAmbas(this);" />
                        Imagen del Proceso de VF
                    </div>
                </td>
                <td style="width: 25px;">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 25px;">&nbsp;
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:ImageButton ID="imgProcesoOtras" runat="server" AlternateText="Proceso de VO, PLD y CGIV" ToolTip="Proceso de VO, PLD y CGIV" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick="return MuestraProcesoInspeccionAmbas(this);" />
                        Imagen del Proceso de VO, PLD y CGIV
                    </div>
                </td>
                <td style="width: 25px;">&nbsp;
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeConfirmaRegistroVisita" style="display: none" title="Confirmación de Registro de visita">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgConfirmaRegistroVisita" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
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

    <div id="divMensajeConfirmaDetalleVisita" style="display: none" title="Confirme si existe Sanción">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgConfirmacion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje2%>
                        <br />
                        <ul>
                            <li>Si: Se te solicitará ingresar la fecha en que se realizó la reunión.
                            </li>
                            <li>No: Se te solicitará confirmes si habrá revisión de vulnerabilidades.
                            </li>
                        </ul>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeConfirmaHabraRevision" style="display: none" title="Confirme si habrá revisión de vulnerabilidades">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgHabraRevision" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje2%>
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
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

    <div id="divProcesoInspeccionAmbas" style="display: none;" title="Proceso Inspección – Sanción">
        <table width="100%">
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:Image ID="imgProcesoInspSancionVF" Width="98%" runat="server" />
                        <asp:Image ID="imgProcesoInspSancionOtras" Width="98%" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <asp:HiddenField ID="hfOpAct" runat="server" ClientIDMode="Static" Value="-1" />
    <asp:HiddenField ID="hfOpAnt" runat="server" ClientIDMode="Static" Value="-1" />
    <asp:HiddenField ID="hfVisitaPadre" runat="server" ClientIDMode="Static" Value="-1" />

    <script type="text/javascript">
        function ConfirmacionDocumentos(psIdDiv, psIdBtnSi, psIdBtnNo, piIdOp) {
            OcultaImagenCargaRegistro();
            ConfirmacionSiNo(psIdDiv, psIdBtnSi, psIdBtnNo, piIdOp, 500, 300);
        }

        function AvisoDocumentos(psIdDiv) {
            OcultaImagenCargaRegistro();
            Aviso(psIdDiv, 500, 300);
        }

        function AquiMuestroOpcionesImgProcesos() {
            OcultaImagenCargaRegistro();
            DivOpcionesImagenesProcesos();
        };

        function ConfirmaRegistroVisita() {
            OcultaImagenCargaRegistro();
            ConfirmaRegistroVisitaLoad();
        };


        //Valida que solo se seleccione una opcion en los check
        //AGC

        //function ConfirmaRegistroVisitaLoad() {

        //    $("#divMensajeConfirmaRegistroVisita").dialog({
        //        resizable: false,
        //        autoOpen: true,
        //        height: 250,
        //        width: 500,
        //        modal: true,
        //        //closeText: 'Cerrar',
        //        open:
        //            function (event, ui) {
        //                $(this).parent().css('z-index', 3999);
        //                $(this).parent().appendTo(jQuery("form:last"));
        //                $('.ui-widget-overlay').css('position', 'fixed');
        //                $('.ui-widget-overlay').css('z-index', 3998);
        //                $('.ui-widget-overlay').appendTo($("form:last"));
        //            },
        //        buttons: {
        //            "Aceptar": function () {
        //                $('#BtnConfirmaRegistro').trigger("click");
        //                $(this).dialog("close");
        //            }
        //        }
        //    });

        function MensajeConfirmacion() {
            MensajeDosBotonesUnaAccion();
        }



        function SeleccionSimple(poptOption) {
            var opAnt = $("#hfOpAnt").val();
            var opAct = $("#hfOpAct").val();

            if (poptOption != null) {
                rates = poptOption.getElementsByTagName('input');

                if (opAct != -1) {
                    opAnt = opAct;

                    for (var i = 0; i < rates.length; i++)
                        if (rates[i].checked && opAct != i) {
                            opAct = i;
                            break;
                        }

                    if (opAct != opAnt)
                        rates[opAnt].checked = false;
                } else {
                    for (var i = 0; i < rates.length; i++)
                        if (rates[i].checked) {
                            opAct = i;
                            $("#hfOpAnt").val(opAnt);
                            $("#hfOpAct").val(opAct);
                            return true;
                        }
                }
            }

            $("#hfOpAnt").val(opAnt);
            $("#hfOpAct").val(opAct);
            return true;
        }

        //Muestra las subvisitas
        function MuestraSubvisitas(chkSub) {
            debugger
            OcultaImagenCargaRegistro();
            if (chkSub.checked)
                $("#trSubentidad").removeClass("Oculto");
            else
                $("#trSubentidad").addClass("Oculto");

            return true;
        }
        function MuestraSubVistasCB() {
            $("#trSubentidad").removeClass("Oculto");
        }
        function OcultaSubVistasCB() {
            $("#trSubentidad").addClass("Oculto");
        }

        function MuestraProcesoInspeccion() {
            OcultaImagenCargaRegistro();
            var x = (screen.width - 30);
            var y = (screen.height - 100);

            Aviso("divProcesoInspeccion", x, y);
            return false;
        }


        function HabilitaTxtDesdeLista(poptOption) {
            OcultaImagenCargaRegistro();
            if (poptOption != null) {
                rates = poptOption.getElementsByTagName('input');
                var valorSeleccionado;
                for (var i = 0; i < rates.length; i++) {
                    if (rates[i].value == "1") { //Chk otro
                        if (rates[i].checked)
                            valorSeleccionado = 1;
                        else
                            valorSeleccionado = 0;
                    }
                }

                if (valorSeleccionado == 1) {
                    $('#OtroObjVisita').removeClass('Oculto');
                } else {
                    $('#OtroObjVisita').addClass('Oculto');
                }
            }
        }

        function MuestraProcesoInspeccionAmbas(control) {
            OcultaImagenCargaRegistro();
            var x = (screen.width - 30);
            var y = (screen.height - 100);

            if (control.id == 'MainContent_imgProcesoVF') {
                document.getElementById("MainContent_imgProcesoInspSancionVF").style.display = "block";
                document.getElementById("MainContent_imgProcesoInspSancionOtras").style.display = "none";
            } else {
                document.getElementById("MainContent_imgProcesoInspSancionVF").style.display = "none";
                document.getElementById("MainContent_imgProcesoInspSancionOtras").style.display = "block";
            }
            Aviso("divProcesoInspeccionAmbas", x, y);
            return false;
        }

        function ConfirmacionHabraRevision() {
            OcultaImagenCargaRegistro();
            $("#divMensajeConfirmaHabraRevision").attr("title", "Habrá Revisión de Vulnerabilidades");
            MensajeTresBotonesTresAccionRRA("divMensajeConfirmaHabraRevision", "btnConfirmacionSiHabraRevision", "btnConfirmacionNoHabraRevision", "btnCancelaFecVulnerabilidad", 500, 300);
        }

        //RRA SI/NO VULNERABILIDAD
        function ConfirmacionFechaVulnerabilidad() {
            OcultaImagenCargaRegistro();
            $("#divMensajeConfirmaDetalleVisita").attr("title", "Revisión de Vulnerabilidades");
            ConfirmacionSiNo("divMensajeConfirmaDetalleVisita", "btnConfirmacionVulnerabilidadSi", "btnConfirmacionVulnerabilidadNo", 1, 500, 300);
        }

        //RRA FECHA VULNERABILIDAD
        function SolicitarFechaVulnerabilidadRegresa() {
            OcultaImagenCargaRegistro();
            //Inicializa controles y oculta imagenes y mensaje
            $("#MainContent_txtFechaGeneral").val("");
            $("#MainContent_lblFechaGeneral").hide();
            $("#divTituloFechaGeneral").text("Favor de ingresar fecha de revisión de vulnerabilidades");
            MensajeDosBotonesDosAccionRRA("divFechaGeneral", "btnGuardaFecVulnerabilidad", "btnNoFecVulnerabilidad", 550, 380);
            return false;
        }

        ///Funcion generica para solicitar una fecha, version FINAL
        function SolicitarFechaGeneral(psTextoMostrar, psNombreBoton) {
            OcultaImagenCargaRegistro();
            $("#divTituloFechaGeneral").text(psTextoMostrar);
            MensajeDosBotonesUnaAccionAgc("divFechaGeneral", psNombreBoton, 550, 370);
        }

    </script>
    <!-- RRA VULNERABILIDAD BOTONES SINO-->
    <asp:Button runat="server" ID="btnConfirmacionVulnerabilidadSi" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmacionVulnerabilidadSi_Click" />
    <asp:Button runat="server" ID="btnConfirmacionVulnerabilidadNo" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmacionVulnerabilidadNo_Click" />

    <!-- RRA FECHA VULNERABILIDAD -->
    <asp:Button runat="server" ID="btnNoFecVulnerabilidad" Style="display: none" ClientIDMode="Static" OnClick="btnGuardaFecVulnerabilidad_Click" />
    <asp:Button runat="server" ID="btnGuardaFecVulnerabilidad" Style="display: none" ClientIDMode="Static" OnClick="btnGuardaFecVulnerabilidad_Click" />

    <!-- NO HABRÁ REVISIÓN DE VULNERABILIDAD -->
    <asp:Button runat="server" ID="btnConfirmacionSiHabraRevision" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmacionSiHabraRevision_Click" />
    <asp:Button runat="server" ID="btnConfirmacionNoHabraRevision" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmacionNoHabraRevision_Click" />
    <asp:Button runat="server" ID="btnCancelaFecVulnerabilidad" Style="display: none" ClientIDMode="Static" OnClick="btnCancelaFecVulnerabilidad_Click" />

    <asp:HiddenField ID="hfIdVisitaGen" runat="server" />
    <asp:HiddenField ID="hdVisitasGen" runat="server" />

    <asp:Button ID="BtnConfirmaRegistro" runat="server" Text="Button" Style="display: none" ClientIDMode="Static" />
    <asp:Button ID="btnCargaArchivo" runat="server" CssClass="botones" Height="23px"
        onmouseout="style.backgroundColor='#696969'"
        onmouseover="style.backgroundColor='#A9A9A9'" Text="Cargar Archivos"
        Width="150px" Style="display: none" ClientIDMode="Static" />
</asp:Content>
