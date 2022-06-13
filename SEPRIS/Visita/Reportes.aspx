<%@ Page Title="" MaintainScrollPositionOnPostback="true"  Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Reportes.aspx.vb" Inherits="SEPRIS.Reportes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/Controles/CopiarFolios.ascx" TagPrefix="ccf" TagName="CopiarFolios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%--<script src="../Scripts/Chart.bundle.js"></script>
   <script src="../Scripts/utils.js"></script>--%>
    <link href="../Styles/TabsV1.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" />
   <link href="../Styles/estilosPayGrafo.css" rel="stylesheet" />
   
   <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>
<!--[if IE]>
<script type="text/javascript" src="http://explorercanvas.googlecode.com/svn/trunk/excanvas.js"></script>
   <![endif]-->

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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

       function AquiMuestroMensaje() {
          MensajeUnBotonNoAccion();
       }
    		
       $(pieChart);

       function pieChart() {

          // Config settings
          var chartSizePercent = 55;                        // The chart radius relative to the canvas width/height (in percent)
          var sliceBorderWidth = 1;                         // Width (in pixels) of the border around each slice
          var sliceBorderStyle = "#fff";                    // Colour of the border around each slice
          var sliceGradientColour = "#ddd";                 // Colour to use for one end of the chart gradient
          var maxPullOutDistance = 25;                      // How far, in pixels, to pull slices out when clicked
          var pullOutFrameStep = 4;                         // How many pixels to move a slice with each animation frame
          var pullOutFrameInterval = 40;                    // How long (in ms) between each animation frame
          var pullOutLabelPadding = 65;                     // Padding between pulled-out slice and its label  
          var pullOutLabelFont = "bold 16px 'Trebuchet MS', Verdana, sans-serif";  // Pull-out slice label font
          var pullOutValueFont = "bold 12px 'Trebuchet MS', Verdana, sans-serif";  // Pull-out slice value font
          var pullOutValuePrefix = "";                     // Pull-out slice value prefix
          var pullOutShadowColour = "rgba( 0, 0, 0, .5 )";  // Colour to use for the pull-out slice shadow
          var pullOutShadowOffsetX = 5;                     // X-offset (in pixels) of the pull-out slice shadow
          var pullOutShadowOffsetY = 5;                     // Y-offset (in pixels) of the pull-out slice shadow
          var pullOutShadowBlur = 5;                        // How much to blur the pull-out slice shadow
          var pullOutBorderWidth = 2;                       // Width (in pixels) of the pull-out slice border
          var pullOutBorderStyle = "#333";                  // Colour of the pull-out slice border
          var chartStartAngle = -.5 * Math.PI;              // Start the chart at 12 o'clock instead of 3 o'clock

          // Declare some variables for the chart
          var canvas;                       // The canvas element in the page
          var currentPullOutSlice = -1;     // The slice currently pulled out (-1 = no slice)
          var currentPullOutDistance = 0;   // How many pixels the pulled-out slice is currently pulled out in the animation
          var animationId = 0;              // Tracks the interval ID for the animation created by setInterval()
          var chartData = [];               // Chart data (labels, values, and angles)
          var chartColours = [];            // Chart colours (pulled from the HTML table)
          var totalValue = 0;               // Total of all the values in the chart
          var canvasWidth;                  // Width of the canvas, in pixels
          var canvasHeight;                 // Height of the canvas, in pixels
          var centreX;                      // X-coordinate of centre of the canvas/chart
          var centreY;                      // Y-coordinate of centre of the canvas/chart
          var chartRadius;                  // Radius of the pie chart, in pixels

          // Set things up and draw the chart
          init();

          function init() {

             // Get the canvas element in the page
             canvas = document.getElementById('chartG1');

             // Exit if the browser isn't canvas-capable
             if (typeof canvas.getContext === 'undefined') return;

             // Initialise some properties of the canvas and chart
             canvasWidth = canvas.width;
             canvasHeight = canvas.height;
             centreX = canvasWidth / 2;
             centreY = canvasHeight / 2;
             chartRadius = Math.min(canvasWidth, canvasHeight) / 2 * (chartSizePercent / 100);

             // Grab the data from the table,
             // and assign click handlers to the table data cells

             var currentRow = -1;
             var currentCell = 0;

             $('#chartDataG1 td').each(function () {
                currentCell++;
                if (currentCell % 2 != 0) {
                   currentRow++;
                   chartData[currentRow] = [];
                   chartData[currentRow]['label'] = $(this).text();
                } else {
                   var value = parseFloat($(this).text());
                   totalValue += value;
                   value = value.toFixed(0);
                   chartData[currentRow]['value'] = value;
                }

                // Store the slice index in this cell, and attach a click handler to it
                $(this).data('slice', currentRow);
                $(this).click(handleTableClick);

                // Extract and store the cell colour
                if (rgb = $(this).css('color').match(/rgb\((\d+), (\d+), (\d+)/)) {
                   chartColours[currentRow] = [rgb[1], rgb[2], rgb[3]];
                } else if (hex = $(this).css('color').match(/#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/)) {
                   chartColours[currentRow] = [parseInt(hex[1], 16), parseInt(hex[2], 16), parseInt(hex[3], 16)];
                } else {
                   alert("Error: Colour could not be determined! Please specify table colours using the format '#xxxxxx'");
                   return;
                }

             });

             // Now compute and store the start and end angles of each slice in the chart data

             var currentPos = 0; // The current position of the slice in the pie (from 0 to 1)

             for (var slice in chartData) {
                chartData[slice]['startAngle'] = 2 * Math.PI * currentPos;
                chartData[slice]['endAngle'] = 2 * Math.PI * (currentPos + (chartData[slice]['value'] / totalValue));
                currentPos += chartData[slice]['value'] / totalValue;
             }

             // All ready! Now draw the pie chart, and add the click handler to it
             drawChart();
             $('#chartG1').click(handleChartClick);
          }


          /**
           * Process mouse clicks in the chart area.
           *
           * If a slice was clicked, toggle it in or out.
           * If the user clicked outside the pie, push any slices back in.
           *
           * @param Event The click event
           */

          function handleChartClick(clickEvent) {

             // Get the mouse cursor position at the time of the click, relative to the canvas
             var mouseX = clickEvent.pageX - this.offsetLeft;
             var mouseY = clickEvent.pageY - this.offsetTop;

             // Was the click inside the pie chart?
             var xFromCentre = mouseX - centreX;
             var yFromCentre = mouseY - centreY;
             var distanceFromCentre = Math.sqrt(Math.pow(Math.abs(xFromCentre), 2) + Math.pow(Math.abs(yFromCentre), 2));

             if (distanceFromCentre <= chartRadius) {

                // Yes, the click was inside the chart.
                // Find the slice that was clicked by comparing angles relative to the chart centre.

                var clickAngle = Math.atan2(yFromCentre, xFromCentre) - chartStartAngle;
                if (clickAngle < 0) clickAngle = 2 * Math.PI + clickAngle;

                for (var slice in chartData) {
                   if (clickAngle >= chartData[slice]['startAngle'] && clickAngle <= chartData[slice]['endAngle']) {

                      // Slice found. Pull it out or push it in, as required.
                      toggleSlice(slice);
                      return;
                   }
                }
             }

             // User must have clicked outside the pie. Push any pulled-out slice back in.
             pushIn();
          }


          /**
           * Process mouse clicks in the table area.
           *
           * Retrieve the slice number from the jQuery data stored in the
           * clicked table cell, then toggle the slice
           *
           * @param Event The click event
           */

          function handleTableClick(clickEvent) {
             var slice = $(this).data('slice');
             toggleSlice(slice);
          }


          /**
           * Push a slice in or out.
           *
           * If it's already pulled out, push it in. Otherwise, pull it out.
           *
           * @param Number The slice index (between 0 and the number of slices - 1)
           */

          function toggleSlice(slice) {
             if (slice == currentPullOutSlice) {
                pushIn();
             } else {
                startPullOut(slice);
             }
          }


          /**
           * Start pulling a slice out from the pie.
           *
           * @param Number The slice index (between 0 and the number of slices - 1)
           */

          function startPullOut(slice) {

             // Exit if we're already pulling out this slice
             if (currentPullOutSlice == slice) return;

             // Record the slice that we're pulling out, clear any previous animation, then start the animation
             currentPullOutSlice = slice;
             currentPullOutDistance = 0;
             clearInterval(animationId);
             animationId = setInterval(function () { animatePullOut(slice); }, pullOutFrameInterval);

             // Highlight the corresponding row in the key table
             $('#chartDataG1 td').removeClass('highlight');
             var labelCell = $('#chartDataG1 td:eq(' + (slice * 2) + ')');
             var valueCell = $('#chartDataG1 td:eq(' + (slice * 2 + 1) + ')');
             labelCell.addClass('highlight');
             valueCell.addClass('highlight');
          }


          /**
           * Draw a frame of the pull-out animation.
           *
           * @param Number The index of the slice being pulled out
           */

          function animatePullOut(slice) {

             // Pull the slice out some more
             currentPullOutDistance += pullOutFrameStep;

             // If we've pulled it right out, stop animating
             if (currentPullOutDistance >= maxPullOutDistance) {
                clearInterval(animationId);
                return;
             }

             // Draw the frame
             drawChart();
          }


          /**
           * Push any pulled-out slice back in.
           *
           * Resets the animation variables and redraws the chart.
           * Also un-highlights all rows in the table.
           */

          function pushIn() {
             currentPullOutSlice = -1;
             currentPullOutDistance = 0;
             clearInterval(animationId);
             drawChart();
             $('#chartDataG1 td').removeClass('highlight');
          }


          /**
           * Draw the chart.
           *
           * Loop through each slice of the pie, and draw it.
           */

          function drawChart() {

             // Get a drawing context
             var context = canvas.getContext('2d');

             // Clear the canvas, ready for the new frame
             context.clearRect(0, 0, canvasWidth, canvasHeight);

             // Draw each slice of the chart, skipping the pull-out slice (if any)
             for (var slice in chartData) {
                if (slice != currentPullOutSlice) drawSlice(context, slice);
             }

             // If there's a pull-out slice in effect, draw it.
             // (We draw the pull-out slice last so its drop shadow doesn't get painted over.)
             if (currentPullOutSlice != -1) drawSlice(context, currentPullOutSlice);
          }


          /**
           * Draw an individual slice in the chart.
           *
           * @param Context A canvas context to draw on  
           * @param Number The index of the slice to draw
           */

          function drawSlice(context, slice) {

             // Compute the adjusted start and end angles for the slice
             var startAngle = chartData[slice]['startAngle'] + chartStartAngle;
             var endAngle = chartData[slice]['endAngle'] + chartStartAngle;

             if (slice == currentPullOutSlice) {

                // We're pulling (or have pulled) this slice out.
                // Offset it from the pie centre, draw the text label,
                // and add a drop shadow.

                var midAngle = (startAngle + endAngle) / 2;
                var actualPullOutDistance = currentPullOutDistance * easeOut(currentPullOutDistance / maxPullOutDistance, .8);
                startX = centreX + Math.cos(midAngle) * actualPullOutDistance;
                startY = centreY + Math.sin(midAngle) * actualPullOutDistance;
                context.fillStyle = 'rgb(' + chartColours[slice].join(',') + ')';
                context.textAlign = "center";
                context.font = pullOutLabelFont;
                context.fillText(chartData[slice]['label'], centreX + Math.cos(midAngle) * (chartRadius + maxPullOutDistance + pullOutLabelPadding), centreY + Math.sin(midAngle) * (chartRadius + maxPullOutDistance + pullOutLabelPadding));
                context.font = pullOutValueFont;
                context.fillText(pullOutValuePrefix + chartData[slice]['value'] + " (" + (parseInt(chartData[slice]['value'] / totalValue * 100 + .5)) + "%)", centreX + Math.cos(midAngle) * (chartRadius + maxPullOutDistance + pullOutLabelPadding), centreY + Math.sin(midAngle) * (chartRadius + maxPullOutDistance + pullOutLabelPadding) + 20);
                context.shadowOffsetX = pullOutShadowOffsetX;
                context.shadowOffsetY = pullOutShadowOffsetY;
                context.shadowBlur = pullOutShadowBlur;

             } else {

                // This slice isn't pulled out, so draw it from the pie centre
                startX = centreX;
                startY = centreY;
             }

             // Set up the gradient fill for the slice
             var sliceGradient = context.createLinearGradient(0, 0, canvasWidth * .75, canvasHeight * .75);
             sliceGradient.addColorStop(0, sliceGradientColour);
             sliceGradient.addColorStop(1, 'rgb(' + chartColours[slice].join(',') + ')');

             // Draw the slice
             context.beginPath();
             context.moveTo(startX, startY);
             context.arc(startX, startY, chartRadius, startAngle, endAngle, false);
             context.lineTo(startX, startY);
             context.closePath();
             context.fillStyle = sliceGradient;
             context.shadowColor = (slice == currentPullOutSlice) ? pullOutShadowColour : "rgba( 0, 0, 0, 0 )";
             context.fill();
             context.shadowColor = "rgba( 0, 0, 0, 0 )";

             // Style the slice border appropriately
             if (slice == currentPullOutSlice) {
                context.lineWidth = pullOutBorderWidth;
                context.strokeStyle = pullOutBorderStyle;
             } else {
                context.lineWidth = sliceBorderWidth;
                context.strokeStyle = sliceBorderStyle;
             }

             // Draw the slice border
             context.stroke();
          }


          /**
           * Easing function.
           *
           * A bit hacky but it seems to work! (Note to self: Re-read my school maths books sometime)
           *
           * @param Number The ratio of the current distance travelled to the maximum distance
           * @param Number The power (higher numbers = more gradual easing)
           * @return Number The new ratio
           */

          function easeOut(ratio, power) {
             return (Math.pow(1 - ratio, power) + 1);
          }

       };
       
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

    
    <div id="tab1" class="tab_content" style="width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
        
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
                        <asp:CheckBox ID="chkGrafica1" Text="Total de Visitas por Área" runat="server" AutoPostBack="true" Checked="true" />
                     </td>
                     <td>
                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkGrafica2" Text="Total de Visitas por Área y Estatus" runat="server" AutoPostBack="true"/>
                     </td>
                     <td>
                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkGrafica3" Text="Total de Visitas por Área y Entidad" runat="server" AutoPostBack="true"/>
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


       <%-- GRÁFICA DE TOTAL DE VISITAS POR ÁREA --%>
       <div id="containerG1" runat="server" visible="false">
        <canvas id="chartG1" width="600" height="500"></canvas>
        <table id="chartDataG1" style="display:block;">
          <tr>
            <th>Área</th><th>Total de Visitas</th>
           </tr>
          <tr style="color: #0DA068">
            <td>VF</td><td><asp:Label ID="lblTotalVFG1" Text="" runat="server"></asp:Label></td>
          </tr>
          <tr style="color: #194E9C">
            <td>VO</td><td><asp:Label ID="lblTotalVOG1" Text="" runat="server"></asp:Label></td>
          </tr>
          <tr style="color: #ED9C13">
            <td>CGIV</td><td><asp:Label ID="lblTotalCGIVG1" Text="" runat="server"></asp:Label></td>
          </tr>
           <tr style="color: #F88E5D">
            <td>PLD</td><td><asp:Label ID="lblTotalPLDG1" Text="" runat="server"></asp:Label></td>
          </tr>
        </table>
      </div>

       <%-- MCS Grafo --%>

        <table id="Table1" runat="server" style="width: 100%;">
            <tr>
                <td colspan="5" style="width: 100%; text-align: center;">
                    <asp:ImageButton ID="imgInicio" ToolTip="Ir a página principal" runat="server" ImageUrl="../Imagenes/inicio.png" OnClientClick="MuestraImgCargaRegistro(this);"/>
                    <asp:ImageButton ID="btnBuscar" ToolTip="Ver detalle del reporte de la visita" runat="server" ImageUrl="~/Imagenes/detalleReporte.png" OnClientClick="btnBuscar_Click"/>
                </td>
            </tr>
        </table>

    </div>

</asp:Content>