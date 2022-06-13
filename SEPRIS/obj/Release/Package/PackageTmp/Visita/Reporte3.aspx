<%@ Page Title="" MaintainScrollPositionOnPostback="true"  Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Reporte3.aspx.vb" Inherits="SEPRIS.Reporte3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/Controles/CopiarFolios.ascx" TagPrefix="ccf" TagName="CopiarFolios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <link href="../Styles/TabsV1.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" />
   
   
   <script type="text/javascript" src="../Scripts/funcion2.js"></script>
   <script type="text/javascript" src="../Scripts/highcharts.js"></script>
   <script type="text/javascript" src="../Scripts/highcharts-3d.js"></script>
   <script type="text/javascript" src="../Scripts/data.js"></script>
   
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />

    <script language="javascript" type="text/javascript">

       var nav = navigator.userAgent.toLowerCase();

       //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
       if (nav.indexOf("chrome") != -1) {
          document.write('<style>#tdMainContent { width: 100% !important; }</style>');
       }
       else {
          document.write('<style>#tdMainContent { width: 91% !important; }</style>');
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

    </script>

   <style type="text/css" >

      .wideBox {
        margin-top: 25px;
        padding: 0px;
      }

      #container{
	      /*width: 120%; 
	      height: 450px; 
	      margin: auto;*/	
      }

   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

       function AquiMuestroMensaje() {
          MensajeUnBotonNoAccion();
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

    <div style="float: left; width: 92%; text-align: center; position: absolute">
        <asp:Label ID="lblFolioVisita" runat="server" Text="" Style="font-weight: bold; font-size: 13px; color: black;"></asp:Label>
        <br />
        <asp:Label ID="lblPasoVisita" runat="server" Text="" Style="font-weight: bold; font-size: 9px; color: black;"></asp:Label>
    </div>

    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1" id="lnkTituloPag" runat="server">Reportes de Visitas</a></li>
        </ul>
    </div>

    
    <div id="tab1" class="tab_content" style="width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.9em; padding: 0px; height: auto;">
        
        <asp:UpdatePanel runat="server" ID="upTabla">
            <ContentTemplate>
                
               <table>
                  <tr>
                     <td>
                        &nbsp;
                     </td>
                  </tr>
                  <tr>
                     <td>
                        <asp:CheckBox ID="chkGrafica1" Text="Total de Visitas por Área" runat="server" AutoPostBack="true"/>
                     </td>
                     <td>
                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkGrafica2" Text="Total de Visitas por Área y Estatus" runat="server" AutoPostBack="true" />
                     </td>
                     <td>
                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkGrafica3" Text="Total de Visitas por Área y Entidad" runat="server" AutoPostBack="true" Checked="true" />
                     </td>
                     <td>
                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkGrafica4" Text="Total de Visitas por Tipo de Visita y Área" runat="server" AutoPostBack="true"/>
                     </td>
                  </tr>
                  <tr>
                     <td>
                        &nbsp;
                     </td>
                  </tr>
               </table>

               <table>
                  <tr>
                     <td colspan="3">
                        <hr />
                     </td>
                  </tr>
                  <tr>
                     <td style="text-align:left; width:40%;">
                       Selecciona el rango de fechas:
                    
                        &nbsp;&nbsp;

                        <asp:TextBox ID="txtFechaRangoIni" runat="server" Width="100px" ReadOnly="false" ></asp:TextBox>

                        <ajx:CalendarExtender ID="calendarioFechaIni" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario1"
                            TargetControlID="txtFechaRangoIni" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario1" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />

                        &nbsp;&nbsp;

                        <asp:TextBox ID="txtFechaRangoFin" runat="server" Width="100px" ReadOnly="false" ></asp:TextBox>

                        <ajx:CalendarExtender ID="calendarioFechaFin" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalendario2"
                            TargetControlID="txtFechaRangoFin" CssClass="teamCalendar" />
                        <asp:Image ID="imgCalendario2" runat="server" ImageUrl="../Imagenes/Calendar.GIF" Width="16px"
                            ImageAlign="Bottom" Height="16px" />
                     </td>
                  </tr>
               </table>

            </ContentTemplate>
        </asp:UpdatePanel>

       <%-- GRÁFICA DE TOTAL DE VISITAS POR ÁREA Y ESTATUS --%>
       <div id="wideBox" runat="server">
        <div id="container" width="600" height="500"></div>
      </div>
       <span id="listaAreasEntidad" runat="server" visible="true" ></span>
      <%-- MCS Grafo --%>

        <table id="Table1" runat="server" style="width: 100%;">
           <tr>
              <td>&nbsp;</td>
           </tr>
           <tr>
              <td>&nbsp;</td>
           </tr>
            <tr>
                <td colspan="5" style="width: 100%; text-align: center;">
                    <asp:ImageButton ID="imgInicio" ToolTip="Ir a página principal" runat="server" ImageUrl="../Imagenes/inicio.png" OnClientClick="MuestraImgCargaRegistro(this);"/>
                    <asp:ImageButton ID="btnBuscar" ToolTip="Ver detalle del reporte de la visita" runat="server" ImageUrl="~/Imagenes/detalleReporte.png" OnClientClick="btnBuscar_Click"/>
                </td>
            </tr>
        </table>

    </div>

</asp:Content>