<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="SeguimientoDetalle.aspx.vb" Inherits="SEPRIS.SeguimientoDetalle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <META http-equiv="Page-Enter" content="blendTrans(Duration=0.2)">
<META http-equiv="Page-Exit" content="blendTrans(Duration=0.2)">

    <link href="../../Styles/TabsV1.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
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
                $("#btnRegresar").click(function () {
                    setActiveTab("li1");
                });

                $("#imgRevisarDocs").click(function () {
                    setActiveTab("li2");
                });

                //Evento para el clic en los tabs. 
                $("ul.tabs li").click(function () {
                    setActiveTab($(this).attr("id"));

                    if ($(this).attr("id") == "li_tab5")
                        $("#btnSeguimiento1").trigger("click");

                    //                    if ($(this).attr("id") == "li_tab3")
                    //                        $("#btncumplimiento").trigger("click");

                    //                    if ($(this).attr("id") == "li_tab4")
                    //                        $("#btnResolucion").trigger("click");

                    //                    if ($(this).attr("id") == "li_tab1")
                    //                        $("#btnIrregularidad23").trigger("click");
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

        function AquiMuestroOpcionesImgProcesos() {
            DivOpcionesImagenesProcesos();
        };

        function MuestraProcesoInspeccion() {
            var x = (screen.width - 30);
            var y = (screen.height - 100);

            Aviso("divProcesoInspeccion", x, y);
            return false;
        }

        function MuestraProcesoInspeccionAmbas(control) {
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

    </script>

    <style type="text/css">

        .timeEstimado {
            width:18px;
            height:10px;
            color:#0B0B61;
            background-color:#0B0B61;
        }

        .timeReal {
            width:18px;
            height:10px;
            color:#58FAD0;
            background-color:#58FAD0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1">Detalle Seguimiento</a></li>
            <!--<li id="li2"><a href="#tab2">Seguimiento Reporte</a></li>-->
        </ul>

        <div style="float: left; width: 100%; text-align: right; margin: -50px 150px 0px 0px;">
            <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick = "return MuestraProcesoInspeccion();" />
            <asp:ImageButton ID="imgProcesoVisitaAmbos" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
        </div>
    </div>
    <div style="width: 100%; float: left;">
        <!--NHM inicia -->
        <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <br />            
            <!-- Tabla para encabezado reporte -->
            <table id="tbEncabezado" style="background-color:#28916F; width:90%;color:white;font-weight:bold;font-size:small;">
               <tr style="height:40px;">
                    <td style="width: 28%; text-align:center">
                        <asp:Label ID="Label3" runat="server" Text="Folio de Visita:" Font-Size="Small" Font-Bold="true" ForeColor="White"></asp:Label>
                        <asp:Label ID="lblFolioVisita" runat="server" Text="" Font-Size="Small" ForeColor="White"></asp:Label>
                    </td>
                    <td style="width: 5%; text-align:center">/</td>                 
                    <td style="width: 28%; text-align:center">
                        <asp:Label ID="Label1" runat="server" Text="Fecha de Registro:" Font-Size="Small" Font-Bold="true" ForeColor="White"></asp:Label>
                        <asp:Label ID="lblFechaRegistro" runat="server" Text="" Font-Size="Small" ForeColor="White"></asp:Label>
                    </td>
                    <td style="width: 5%; text-align:center">/</td> 
                    <td style="width: 34%; text-align:center">
                        <asp:Label ID="Label2" runat="server" Text="Paso Actual:" Font-Size="Small" Font-Bold="true" ForeColor="White"></asp:Label>
                        <asp:Label ID="lblPasoActual" runat="server" Text="" Font-Size="Small" ForeColor="White"></asp:Label>
                    </td>
                </tr>        
            </table>

            <!-- Tabla para encabezado reporte -->
            <table id="tbPasos" style="width:90%;font-size:small; border-collapse: collapse;" runat="server">
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr style="background-color:#0B0B61;color:white;font-weight:bold; height:30px; border-bottom">
                    <td style="width:10%">Pasos</td>
                    <td style="width:20%">Días efectivos vs Objetivo</td>
                    <td style="width:15%">Fecha</td>
                    <td style="width:12%">Usuario</td>
                    <td style="width:23%">Comentarios</td>
                    <td style="width:20%">Archivos</td>
                </tr>                 
            </table>

            <br/><br/>
            <div style="width:90%; padding-bottom: 5px;">
                <table style="width:50%; text-align:center; border:1px solid gray;">
                    <tr>
                        <td style="width:20%; vertical-align:top; text-align:right"><div class="timeEstimado"></div></td>
                        <td style="width:30%; vertical-align:top;"><label class="txt_gral"> = Tiempo Estimado</label></td>
                        <td style="width:20%; vertical-align:top; text-align:right"><div class="timeReal"></div></td>
                        <td style="width:30%; vertical-align:top;"><label class="txt_gral"> = Tiempo Real</label></td>
                    </tr>
                </table>
            </div>
             <!-- Tabla para botones -->
            <table id="tbBotones">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td style="width: 100%; text-align:center">    
                        <asp:ImageButton ID="imgAnterior" ToolTip="Regresar a Reporte de visita" runat="server" ImageUrl="../Imagenes/Anterior.png" OnClick="imgAnterior_Click"/>
                        <asp:ImageButton ToolTip="Ir a página principal" ID="imgInicio" runat="server" ImageUrl="../Imagenes/inicio.png" />
                        <asp:ImageButton ToolTip="Enviar a imprimir el reporte" ID="imgVistaPreliminar" runat="server" ImageUrl="../Imagenes/vistaPreliminar.png" />                    
                    </td>                   
                </tr>
            </table>
        </div>
        <!-- NHM fin -->
        <!--
        <div id="tab2" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            Seguimiento Reporte
        </div>
        -->
    </div>

    <%--imagen del proceso de sancion--%>
            <div id="divProcesoInspeccion" style="display: none;" title="Proceso Inspección – Sanción">
                <table width="100%">
                    <tr>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                             <asp:Image ID="imgProcesoInspSancion" Width="98%" runat ="server" />
                            </div>                    
                        </td>
                    </tr>
                </table>
            </div>

          <div id="divProcesoInspeccionAmbas" style="display: none;" title="Proceso Inspección – Sanción">
                <table width="100%">
                    <tr>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                             <asp:Image ID="imgProcesoInspSancionVF" Width="98%" runat ="server" />
                             <asp:Image ID="imgProcesoInspSancionOtras" Width="98%" runat ="server" />
                            </div>                    
                        </td>
                    </tr>
                </table>
            </div>

          <div id="dvModalImagenesProcesos" style="display:none" title="Procesos de Inspección - Sanción">
                <table width="100%">
                    <tr>
                        <td style="width: 25px;">
                            &nbsp;
                        </td>
                        <td  style="text-align: left">
                            <div class="MensajeModal-UI">
                                <asp:ImageButton ID="imgProcesoVF" runat="server" AlternateText="Proceso de VF" ToolTip="Proceso de VF" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick = "return MuestraProcesoInspeccionAmbas(this);"/>
                                Imagen del Proceso de VF
                            </div>
                        </td>                        
                        <td style="width: 25px;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25px;">
                            &nbsp;
                        </td>
                        <td  style="text-align: left">
                            <div class="MensajeModal-UI">
                                <asp:ImageButton ID="imgProcesoOtras" runat="server" AlternateText="Proceso de VO, PLD y CGIV" ToolTip="Proceso de VO, PLD y CGIV" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick = "return MuestraProcesoInspeccionAmbas(this);"/>
                                Imagen del Proceso de VO, PLD y CGIV
                            </div>
                        </td>
                        <td style="width: 25px;">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
</asp:Content>
