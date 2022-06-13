<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="RegistroOPI.aspx.vb" Inherits="SEPRIS.RegistroOPI" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/OPI/UserControls/SupervisorOPI.ascx" TagPrefix="uc1" TagName="SupervisorOPI" %>
<%@ Register Src="~/OPI/UserControls/InspectoresOPI.ascx" TagPrefix="uc1" TagName="InspectoresOPI" %>


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
            color:red;
            font-size:20px;
        }

        .Oculto {
            display:none;
        }

        .FondoAplicacion
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
     
        .auto-style1 {
            width: 25%;
            height: 35px;
        }
        .auto-style2 {
            width: 75%;
            height: 35px;
        }
     
        .auto-style3 {
            height: 14px;
        }
     
        .auto-style4 {
            height: 17px;
        }
     
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div style="float: left; width: 92%; text-align: center; position: absolute">
        <asp:Label ID="lblPasoOPI" runat="server" Style="font-weight: bold; font-size: 13px; color: black;" Text="Paso 1 :  Registro de Oficio"></asp:Label>
    </div>
    <%--<div style="float: left; width: 100%; text-align: center; position: absolute">
      
        <asp:Label ID="lblPasoOPI" runat="server" Text="" Style="font-weight: bold; font-size: 9px; color: black;">Paso 1 :  Detección de Posible incumplimiento</asp:Label>
    </div>--%>

    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1" id="lnkTituloPag" runat="server">Registro de Oficio </a></li>
            <%--<li id="li6"><a href="#tab6">Expediente documentos</a></li>--%>
            <!--
            <li id="li2"><a href="#tab2">Expediente</a></li>
            <li id="li3"><a href="#tab3">Historial</a></li> 
            -->
        </ul>
    </div>

    
    <div id="tab1" class="tab_content" style="horizontal-align:left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
        <div style="float: left; width: 100%; text-align: right; margin: -50px 150px 0px 0px;">
            <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
        </div>

        <asp:UpdatePanel runat="server" ID="upTabla">
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="imgRegistrarVisita" />--%>
                <%--<asp:AsyncPostBackTrigger ControlID="GridPrincipal" />--%>
            </Triggers>

            <ContentTemplate>
                <!-- Tabla para capturar los datos -->
                <table id="tbDatosCaptura" runat="server" style="width: 95%; margin-top: 20px;">                    
                    <tr>
                        <td style="height: 35px; width: 30%; text-align: left;">
                            <div id="ast" runat="server" class="AsteriscoShow">*</div> Tipo Entidad :
                        </td>
                        <td style="height: 35px; width: 70%; text-align: left;" colspan="2">
                            <asp:DropDownList ID="ddlTipoEntidad" runat="server" Width="50%" AutoPostBack="True" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr runat="server" clientidmode="Static">
                        <td style="height: 35px; width: 30%; text-align: left;">
                            <div id="ast1" runat="server" class="AsteriscoShow">*</div>
                            Entidad :
                        </td>
                        <td style="height: 35px; width: 50%; text-align: left;"  >
                            <asp:DropDownList ID="dplEntidad" runat="server" Width="70%" Enabled="false" AutoPostBack="True" OnSelectedIndexChanged="dplEntidad_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="AddSubEntidades" runat="server" TextAlign="Left" text="Agregar Sub Entidades " Visible="false" AutoPostBack="true" />
                        </td>
                    </tr>
               
                    <tr id="trSubentidad1" runat="server" clientidmode="Static" visible="false" >
                        <td style="height: 35px; width: 25%; text-align: left;">
                            <div id="ast2" runat="server" class="AsteriscoShow">*</div>
                            <asp:Label runat="server" ID="Label2" 
                                CssClass="txt_gral" Text="Sub Entidad :"></asp:Label>
                        </td>
                        <td style="height: 35px; width: 40%; text-align: left;">
                            <asp:DropDownList ID="ddlSubentidad" runat="server" Width="70%"></asp:DropDownList>
                        </td>
                        <td style="text-align: left;"></td>
                    </tr>

                    <tr id="trSubentidad" runat="server" clientidmode="Static" visible="false" >
                        <td style="height: 35px; width: 25%; text-align: left;">
                            <div id="Div2" runat="server" class="AsteriscoShow">*</div>
                            <asp:Label runat="server" ID="Label1" 
                                CssClass="txt_gral" Text="Sub Entidad :"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:CheckBoxList ID="chkSubEntidad" ClientIDMode="Static" Visible="true"  runat="server" AutoPostBack ="false" ></asp:CheckBoxList>
                        </td>
                        <td style="text-align: left;"></td>
                    </tr>

                    <tr  runat="server">
                        <td style="height: 35px; width: 30%; text-align: left;">
                            <div id="ast3" runat="server" class="AsteriscoShow">*</div>
                            Fecha de posible irregularidad :</td>
                        <td style="height: 35px; width: 70%; text-align: left;"  colspan="2">
                            <asp:TextBox ID="txtFechaPI" runat="server" Width="150px"></asp:TextBox>
                            <ajx:CalendarExtender ID="calFechaPI" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFechaPI"
                                TargetControlID="txtFechaPI" CssClass="teamCalendar" />
                            <asp:Image ID="imgFechaPI" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                                ImageAlign="Bottom" Height="16px" />
                        </td>
                    </tr>

                    <tr>
                        <td style="height: 35px; width: 30%; text-align: left;">
                            <div id="ast4" runat="server" class="AsteriscoShow">*</div>
                            Proceso :</td>
                        <td style="height: 35px; width: 70%; text-align: left;" colspan="2">
                            <asp:DropDownList ID="dplProcesoPO" runat="server" AutoPostBack="True" Width="50%" OnSelectedIndexChanged="dplProcesoPO_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 35px; width: 30%; text-align: left;">
                            <div id="ast5" runat="server" class="AsteriscoShow">*</div>
                            Subproceso :</td>
                        <td style="height: 35px; width: 70%; text-align: left;" colspan="2">
                            <asp:DropDownList ID="ddlSubproceso" runat="server" AutoPostBack="True" Width="50%" OnSelectedIndexChanged="ddlSubproceso_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr runat="server" clientidmode="Static">
                        <td style="text-align: left;" class="auto-style1">
                            <div id="ast13" runat="server" class="AsteriscoShow">*</div>
                            Descripción :</td>
                        <td style="text-align: left;" colspan="2" class="auto-style2" rowspan="2">
                            <asp:TextBox ID="txtDescOPI" runat="server" onkeypress="ReemplazaCEspeciales(this.id)" 
                                onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)" 
                                onkeyup="validaLimiteLongitud(this,300,'lblConttxtObjetoOPI')" 
                                TextMode="MultiLine" Width="100%" MaxLength="300" Height="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                        </td>
                    </tr>
                    <tr id="OtroObjVisitaDsc" runat="server" clientidmode="Static">
                        <td style=" width: 25%; text-align: left;">&nbsp;</td>
                        <td style="width: 75%; text-align: right;"  colspan="3">
                            <div id="divConttxtObjetoOPI" runat="server" style="width: 100%; text-align: right; float:left;">
                                <asp:Label runat="server" ID="lbltxtObjetoOPI" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                <asp:Label runat="server" ID="lblConttxtObjetoOPI" CssClass="txt_gral" Text="500" ClientIDMode="Static"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr id="trSupervisores" runat="server" clientidmode="Static">
                        <td style="width: 100%;" colspan="4">
                            <%--<div id="ast12" runat="server" class="AsteriscoShow">*</div>--%>
                            <uc1:SupervisorOPI runat="server" id="SupervisorOPI"  />
                        </td>
                    </tr>
                    <tr id="trInspectores" runat="server" clientidmode="Static">
                        <td style="width: 100%;" colspan="4">
                            <uc1:InspectoresOPI runat="server" id="InspectoresOPI" />
                            <%--<div id="ast14" runat="server" class="AsteriscoHide">*</div>--%>
                        </td>
                    </tr>
                </table>
                <table id="tblDescripcionVisita" runat="server" style="width: 95%; margin-top: 20px;">
                    <tr>
                        <td><div style=" text-align: left;">
                                <asp:Label runat="server" ID="lblComentariosCaracteres" Text="Comentarios :"></asp:Label>
                            </div>
                        </td>
                        <td rowspan="2" colspan="3" style="width: 100%;">
                            <asp:TextBox ID="txtComentariosOPI" runat="server" onkeyup="validaLimiteDescripcion(this,250)" 
                                onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" 
                                onfocus="ReemplazaCEspeciales(this.id)" TextMode="MultiLine" Width="100%" Height="70px" MaxLength="250"></asp:TextBox>
                            <div id="ast11" runat="server" class="AsteriscoHide" style="width: 15%;">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td >                    
                        </td>
                    </tr>
            
                    <tr>
                        <td colspan="4" align="center" style="width: 80%;">
                            <div id="Div3" runat="server" style="width: 100%; text-align: right;">
                                <asp:Label runat="server" ID="lblCarDescripcion" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                <asp:Label runat="server" ID="lblDescripcionContador" CssClass="txt_gral" Text="250"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" style="width: 80%;">
                            
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnSalirM2B2A" />--%>
            <%--<asp:PostBackTrigger ControlID="btnContinuarM2B2A" />--%>
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <%--<asp:PostBackTrigger ControlID="btnGuardar" />--%>
        </Triggers>
    </asp:UpdatePanel>

        <table id="Table1" runat="server" style="width: 100%;">
            <tr>
                <td colspan="5" style="width: 100%; text-align: center;">
                    <asp:ImageButton ID="imgRegistrar" runat="server" ImageUrl="../Imagenes/registrarVisita.png" ToolTip="Guardar datos para registro" />
                    <asp:ImageButton ID="imgInicio" ToolTip="Ir a página principal" runat="server" ImageUrl="../Imagenes/inicio.png"/>
                </td>
            </tr>
        </table>
    </div>
    
    <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Label runat="server" ID="lblTitle" ClientIDMode="Static" Visible="false"></asp:Label>
    <div style="height: 50px; float: left; width: 100%">
        &nbsp;
    </div>

    <!--div y script para el modal que muestra mensajes de error o informativos -->
    <div id="divMensajeUnBotonNoAccion" style="display:none" title='<%="" + lblTitle.Text + "" %>'>
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


    <div id="divMensajeDosBotonesUnaAccion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
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
    <div id="divMensajeUnBotonUnaAccion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
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
        function MensajeConfirmacion() {
            MensajeDosBotonesUnaAccion();
        }

        function MensajeFinalizar() {
            MensajeUnBotonUnaAccion();
        }
    </script>
    
    

   
</asp:Content>
