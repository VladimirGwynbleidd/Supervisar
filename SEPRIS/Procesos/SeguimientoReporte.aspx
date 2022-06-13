<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="SeguimientoReporte.aspx.vb" Inherits="SEPRIS.SeguimientoReporte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        function MuestraProcesoInspeccion() {
            var x = (screen.width - 30);
            var y = (screen.height - 100);

            Aviso("divProcesoInspeccion", x, y);
            return false;
        }
        
        function AquiMuestroOpcionesImgProcesos() {
            DivOpcionesImagenesProcesos();
        };

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1">Reporte de Seguimiento</a></li>
            <!--<li id="li2"><a href="#tab2">Detalle Seguimiento</a></li>-->
        </ul> 
           
        <div style="float: left; width: 100%; text-align: right; margin: -50px 150px 0px 0px;">
            <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick ="return MuestraProcesoInspeccion();" />
            <asp:ImageButton ID="imgProcesoVisitaAmbos" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
        </div>      
    </div>
    <div style="width: 100%; float: left;">
        <!--NHM inicia -->
        <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <!-- Tabla para encabezado reporte -->
            <table id="tbEncabezado" style="width:90%; margin-top: 20px; background-color:#28916F; color:white;font-weight:bold;font-size:small;">                
                <tr style="height:40px;">
                    <td style="width: 10%; text-align:right">Folio: </td>
                    <td style="width: 20%; text-align:left">
                        <asp:Label ID="lblFolioVisita" runat="server" Text="" Font-Size="Small" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                    <td style="width: 20%; text-align:right">Fecha de Registro: </td>
                    <td style="width: 20%; text-align:left">
                        <asp:Label ID="lblFechaRegistro" runat="server" Text="" Font-Size="Small" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                    <td style="width: 10%; text-align:right">Paso actual: </td>
                    <td style="width: 20%; text-align:left">
                        <asp:Label ID="lblEstatus" runat="server" Text="" Font-Size="Small" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                </tr>               
            </table>
           <%-- Tabla para mostrar el total de días hábiles de la visita de inspección desde su creación --%>
           <table style="width:90%; margin-top: 20px; border: 3px solid #999;">
              <tr>
                 <td>
                    &nbsp;
                 </td>
              </tr>
              <tr>
                 <td colspan="2" style="text-align:left; font-weight:bold;font-size:small;">
                    Días hábiles de la visita de inspección
                 </td>
              </tr>
              <tr>
                 <td colspan="2">
                    <hr />
                 </td>
              </tr>
              <tr>
                 <td style="text-align:left; width:12%;">
                    Fecha de inicio de visita
                 </td>
                 <td style="text-align:left; font-weight:bold; width:88%;">
                    <asp:Label ID="lblFechIniVis" style="font-weight: bold !important;" runat="server" Text="Fecha inicio de visita"></asp:Label>
                 </td>
              </tr>
              <tr>
                 <td style="text-align:left; width:12%;">
                    Tiempo Estimado
                 </td>
                 <td style="width:88%; text-align:left">
                    <div id="divTotalDias" runat="server"></div>
                        <div style="float: left;" >
                             &nbsp;<asp:Label ID="lblTETotal" runat="server" Text="Tiempo Estimado"></asp:Label>
                        </div> 
                 </td>
              </tr>
              <tr>
                 <td style="text-align:left; width:12%;">
                    Tiempo Real
                 </td>
                 <td style="width:88%; text-align:left">
                    <div id="divTotalDiasR" runat="server"></div>
                        <div style="float: left;" >
                             &nbsp;<asp:Label ID="lblTRTotal" runat="server" Text="Tiempo Real"></asp:Label>
                        </div> 
                 </td>
              </tr>
              <tr>
                 <td>
                    &nbsp;
                 </td>
              </tr>
           </table>

            <!-- Tabla para Planeación de Visita  -->
            <table style="width:90%; margin-top: 20px;">
               <tr>
                    <td colspan="2" style="text-align:left; font-weight:bold;font-size:small;">
                        Resumen de tiempos del proceso de Inspección-Sanción
                    </td>                   
                </tr>
               <tr><td colspan="2"><hr /></td></tr>
                <tr>
                    <td colspan="2" style="text-align:left; font-weight:bold;font-size:small;">
                        Inicio de visita de inspección
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE1" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE1" runat="server" Text="Label"></asp:Label>
                        </div>                       
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                         <div id="divTRE1" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE1" runat="server" Text="Label"></asp:Label>
                        </div>                       
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 1 al 4--%>
                        <asp:Label ID="lblRangoPaso1" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>
            <!-- Tabla para Ejecución de Visita  -->
            <table style="width:90%; margin-top: 20px;">
                <tr>
                    <td colspan="2"  style="text-align:left; font-weight:bold;font-size:small;">
                        Desarrollo de la visita de inspección
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE2" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE2" runat="server" Text="Label"></asp:Label>
                        </div>        
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTRE2" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE2" runat="server" Text="Label"></asp:Label>
                        </div>        
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 5 al 8--%>
                        <asp:Label ID="lblRangoPaso2" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>
            <!-- Tabla para Presentación de Hallazgos -->
            <table style="width:90%; margin-top: 20px;">
                <tr>
                    <td colspan="2"  style="text-align:left; font-weight:bold;font-size:small;">
                        Resultados de la visita de inspección
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE3" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE3" runat="server" Text="Label"></asp:Label>
                        </div>       
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTRE3" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE3" runat="server" Text="Label"></asp:Label>
                        </div>       
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 9 al 15--%>
                        <asp:Label ID="lblRangoPaso3" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>
             <!-- Tabla para Proceso Inspección - Sanción -->
            <table style="width:90%; margin-top: 20px; border-top: 1px solid #999;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  style="text-align:left; font-weight:bold;font-size:small;">
                       Cierre de visita de inspección
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE4" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE4" runat="server" Text="Label"></asp:Label>
                        </div>                                
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTRE4" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE4" runat="server" Text="Label"></asp:Label>
                        </div>       
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 16 al 19--%>
                        <asp:Label ID="lblRangoPaso4" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>

            <table style="width:90%; margin-top: 20px; border-top: 1px solid #999;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  style="text-align:left; font-weight:bold;font-size:small;">
                       Sanción de visita de inspección
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE5" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE5" runat="server" Text="Label"></asp:Label>
                        </div>                                
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTRE5" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE5" runat="server" Text="Label"></asp:Label>
                        </div>       
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 20 al 29--%>
                        <asp:Label ID="lblRangoPaso5" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>

            <table style="width:90%; margin-top: 20px; border-top: 1px solid #999;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  style="text-align:left; font-weight:bold;font-size:small;">
                       Contencioso
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE6" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE6" runat="server" Text="Label"></asp:Label>
                        </div>                                
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTRE6" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE6" runat="server" Text="Label"></asp:Label>
                        </div>       
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 30 al 37--%>
                        <asp:Label ID="lblRangoPaso6" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>

            <table style="width:90%; margin-top: 20px; border-top: 1px solid #999;">
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  style="text-align:left; font-weight:bold;font-size:small;">
                       Proceso de inspección - Sanción
                    </td>                   
                </tr>
                <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Estimado 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTEE7" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTEE7" runat="server" Text="Label"></asp:Label>
                        </div>                                
                    </td>                     
                </tr>
                 <tr>
                    <td style="width:12%; text-align:left">
                        Tiempo Real 
                    </td>
                    <td style="width:88%; text-align:left">
                        <div id="divTRE7" runat="server"></div>
                        <div style="float: left; margin-left:10px;" >
                             <asp:Label ID="lblNumDiasTRE7" runat="server" Text="Label"></asp:Label>
                        </div>       
                    </td>                     
                </tr>
                <tr>
                    <td style="width:12%; text-align:left" colspan="2">
                        <%--Pasos del 1 al 29--%>
                        <asp:Label ID="lblRangoPaso7" runat="server" Text=""></asp:Label>
                    </td>                  
                </tr>
            </table>

             <!-- Tabla para botones -->
            <table id="tbBotones">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td style="width: 100%; text-align:center">    
                        <asp:ImageButton ID="imgVistaPreliminar" ToolTip="Enviar a imprimir el reporte" runat="server" ImageUrl="../Imagenes/vistaPreliminar.png" />                    
                        <asp:ImageButton ID="imgInicio" ToolTip="Ir a página principal" runat="server" ImageUrl="../Imagenes/inicio.png" />                      
                        <asp:ImageButton ID="btnSeguimientoDetalle" ToolTip="Ver detalle del reporte de la visita" runat="server" ImageUrl="~/Imagenes/detalleReporte.png" />
                    </td>                   
                </tr>
                <%--<tr>
                    <td style="width: 100%; text-align:center">
                        
                    </td>
                </tr>--%>
            </table>
        </div>
        <!-- NHM fin -->
        <!--
        <div id="tab2" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            Detalle Seguimiento
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
