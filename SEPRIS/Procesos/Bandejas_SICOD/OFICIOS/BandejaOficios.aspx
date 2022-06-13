<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BandejaOficios.aspx.vb" Inherits="SEPRIS.BandejaOficios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="SEPRIS" TagName="FiltroBusqueda" Src="FiltroBusquedaOfi.ascx" %>
<%@ Register Src="PersonalizarColumasOfi.ascx" TagName="PersonalizarColumas" TagPrefix="cc1" %>
<%@ Register TagPrefix="AnexosRow" TagName="AnexosRegistro" Src="UserControls/ucAnexoModalRowOfi.ascx" %>
<script src="/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script src="/Scripts/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>
<%--<script src="/Scripts/MensajeModal.js" type="text/javascript"></script>--%>
<script src="../../../Scripts/ToolTip.js" type="text/javascript"></script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SEPRIS</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="Styles.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" src="Scripts/script.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        function CierraModal(psRen, psIdDoc, psIdDocVer) {
            $('#divTextoRespuesta').text('¿Estás seguro de ligar el folio SICOD ' + psIdDoc + '?');

            $("#divMensajeRespuesta").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 500,
                modal: true,
                title: 'Ligar Folio SICOD',
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
                        opener.llamaPostback()
                        window.close();
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function CierraModalOpiPC(psRen, psIdDoc, psIdDocVer, cveOficio) {
            $('#divTextoRespuesta').text('¿Estás seguro de ligar el folio SICOD ' + cveOficio + '?');

            $("#divMensajeRespuesta").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 500,
                modal: true,
                title: 'Ligar Folio SICOD',
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
                        $('#btnLigar').trigger("click");
                        window.opener.llamaPostback();
                        window.close();
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function Ejemplo(e) {



        }

        function hidePopup() {
            document.getElementById('PanelErrores').style.display = 'none';
        }


        function AbreArchivoSIE(RutaArchivo) {
            alert(RutaArchivo)
            var activeXO = new ActiveXObject("Scripting.FileSystemObject");
            var f = activeXO.OpenTextFile(RutaArchivo, 1);
            alert(f.ReadAll());

        }

        function cambio2(obj) {
            obj.style.background = 'transparent';
            obj.style.color = '#555555';
            tooltip.hide();
        }


        var tooltip = function () {
            var id = 'tt';
            var top = 3;
            var left = 3;
            var maxw = 300;
            var speed = 5;
            var timer = 10;
            var endalpha = 95;
            var alpha = 0;
            var tt, t, c, b, h;
            var ie = document.all ? true : false;
            return {
                show: function (v, w) {
                    if (tt == null) {
                        tt = document.createElement('div');
                        tt.setAttribute('id', id);
                        t = document.createElement('div');
                        t.setAttribute('id', id + 'top');
                        c = document.createElement('div');
                        c.setAttribute('id', id + 'cont');
                        b = document.createElement('div');
                        b.setAttribute('id', id + 'bot');
                        tt.appendChild(t);
                        tt.appendChild(c);
                        tt.appendChild(b);
                        document.body.appendChild(tt);
                        tt.style.opacity = 0;
                        tt.style.filter = 'alpha(opacity=0)';
                        document.onmousemove = this.pos;
                    }
                    tt.style.display = 'block';
                    c.innerHTML = v;
                    tt.style.width = w ? w + 'px' : 'auto';
                    if (!w && ie) {
                        t.style.display = 'none';
                        b.style.display = 'none';
                        tt.style.width = tt.offsetWidth;
                        t.style.display = 'block';
                        b.style.display = 'block';
                    }
                    if (tt.offsetWidth > maxw) { tt.style.width = maxw + 'px' }
                    h = parseInt(tt.offsetHeight) + top;
                    clearInterval(tt.timer);
                    tt.timer = setInterval(function () { tooltip.fade(1) }, timer);
                },
                pos: function (e) {
                    var u = ie ? event.clientY + document.documentElement.scrollTop : e.pageY;
                    var l = ie ? event.clientX + document.documentElement.scrollLeft : e.pageX;
                    tt.style.top = (u - h) + 'px';
                    tt.style.left = (l + left) + 'px';
                },
                fade: function (d) {
                    var a = alpha;
                    if ((a != endalpha && d == 1) || (a != 0 && d == -1)) {
                        var i = speed;
                        if (endalpha - a < speed && d == 1) {
                            i = endalpha - a;
                        } else if (alpha < speed && d == -1) {
                            i = a;
                        }
                        alpha = a + (i * d);
                        tt.style.opacity = alpha * .01;
                        tt.style.filter = 'alpha(opacity=' + alpha + ')';
                    } else {
                        clearInterval(tt.timer);
                        if (d == -1) { tt.style.display = 'none' }
                    }
                },
                hide: function () {
                    clearInterval(tt.timer);
                    tt.timer = setInterval(function () { tooltip.fade(-1) }, timer);
                }
            };
        }();


        function click_Archivo(obj) {
            var linkboton = document.getElementById('linkbotonArchivo');
            linkboton = obj;
            linkboton.click();
        }

        function CallClick(objclick) {
            try {
                window.location.href = document.getElementById(objclick).href;
                return false;
            }
            catch (e) { }

        }

        function CallClickINICIO() {
            try {
                //document.getElementById('LinkButton1').click(); //ONLY WORK IN IE
                //WORK IN ALL BROWSER
                document.getElementById('Imagen_procesando').style.display = '';
                document.getElementById('Imagen_procesando').style.visibility = 'visible';
                document.getElementById('Imagen_procesando').style.height = '150px';
                document.getElementById('GRID').style.visibility = 'hidden';
                try { window.location.href = document.getElementById('BotonInicio').href; } catch (e) { }

                return false;
            }
            catch (e) { }
        }

        function BotonFiltrar() {
            document.getElementById('Imagen_procesando').style.display = '';
            document.getElementById('Imagen_procesando').style.visibility = 'visible';
            document.getElementById('Imagen_procesando').style.height = '150px';
            document.getElementById('GRID').style.visibility = 'hidden';
            try {
                document.getElementById('Boton_guardar').style.visibility = 'hidden';
            }
            catch (e) { }
        }

        function CierraModalPersonzalizar(AcutilzarGrid) {
            //Cierra Modal                               
            if (AcutilzarGrid == 1) {
                document.getElementById('btnAcualizaGridPersonalizado').click();
            }
            document.getElementById('btnCerrarModal').click();

        }

        function ShowProcesa() {

            $find('mpeProcesa').show();
            return true;

        }


        function Cierrame() {

            window.close();
            return true;
        }


        function ShowBitacora(link) {


            url = 'UserControls/ShowBitacora.aspx?doc=' + link.innerText + '&tipo=0';

            winprops = "dialogHeight: 400px; dialogWidth: " + parseInt(document.body.clientWidth * 1.10).toString() + "px; edge: Raised; center: Yes; help: No;resizable: No; status: No; Location: No; Titlebar: No;"

            window.showModalDialog(url, "Historial", winprops);

            return false;

        }


    </script>

    <link href="/Styles/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />




    <%-- PRUENAS DE TOOLTIP--%>
</head>
<body>



    <form id="frmBandejaEntrada" runat="server">
        <asp:ToolkitScriptManager ID="Toolkitscriptmanager2" runat="server" EnableScriptGlobalization="true">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="Controles" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="BtnPersonalizar" />
                <asp:AsyncPostBackTrigger ControlID="GridPrincipal" />
            </Triggers>
            <ContentTemplate>

                <table width="800px" align="center">
                    <tr>
                        <td>
                            <p class="TitulosWebProyectos" style="text-align: center" align="center">
                                Bandeja de Oficios SICOD
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="BtnPersonalizar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Height="22px" ToolTip="Personalizar Columnas" Text="Personalizar Columas"
                                CssClass="botones" Width="130" Visible="false"></asp:Button>

                            <asp:TextBox ID="ID_OFICIO0" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="CLAVE_OFICIO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TIPO_OFICIO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="FECHA_VENCIMIENTO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="PLAZO_DIAS" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="FECHA_NOTIFICACION" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="ENTIDAD_CORTO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="ID_TIPO_DOCUMENTO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="F_FECHA_OFICIO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="ID_CONSAR" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="ID_CLASIFICACION" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="T_CLASIFICACION" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="T_HYP_ARCHIVOSCAN" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="ID_ENTIDAD" runat="server" Visible="false"></asp:TextBox>

                            <asp:TextBox ID="ID_AREA_OFICIO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="ID_ANIO" runat="server" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="I_OFICIO_CONSECUTIVO" runat="server" Visible="false"></asp:TextBox>

                            <asp:TextBox ID="NOTIFICACION" runat="server" Visible="false"></asp:TextBox>



                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="FiltroBandeja">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <SEPRIS:FiltroBusqueda ID="Filtros" runat="server"></SEPRIS:FiltroBusqueda>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <!--Tabla Control DataGrid--->
                            <table width="800px" align="center">
                                <tr>
                                    <td>
                                        <div id="Imagen_procesando" runat="server" style="display: none; text-align: center">
                                            <img src="../../../Images/carga.gif" alt="Espere un momento" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <div id="GRID" runat="server" style="overflow: auto; width: 800px; height: 300px; position: absolute" >
                                            <asp:DataGrid ID="GridPrincipal" runat="server" Font-Size="7.5pt" AutoGenerateColumns="false"
                                                BackColor="#D6EBBD" CellPadding="1" Font-Name="Arial" HorizontalAlign="Center"
                                                Font-Names="Arial" ForeColor="#555555" BorderColor="White"
                                                DataKeyNames="ID_UNIDAD_ADM, ID_ANIO, I_OFICIO_CONSECUTIVO, ID_TIPO_DOCUMENTO,ID_FOLIO"
                                                SelectedIndexChanged="GridPrincipal_SelectedIndexChanged">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" ForeColor="#FFFFFF"
                                                    CssClass="HeaderBandeja"></HeaderStyle>
                                                <Columns>


                                                    <asp:ButtonColumn Text="O" CommandName="Delete" ItemStyle-HorizontalAlign="Center" Visible="true"></asp:ButtonColumn>

                                                

                                                    <asp:BoundColumn HeaderText="Area" DataField="DSC_UNIDAD_ADM" SortExpression="DSC_UNIDAD_ADM"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>


                                                    <asp:BoundColumn HeaderText="Oficio" DataField="T_OFICIO_NUMERO" SortExpression="T_OFICIO_NUMERO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Id_Oficio" DataField="ID_OFICIO" SortExpression="ID_OFICIO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Asunto" DataField="T_ASUNTO" SortExpression="T_ASUNTO"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Plazo" DataField="I_PLAZO_DIAS" SortExpression="I_PLAZO_DIAS"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Elaboró" DataField="ELABORO" SortExpression="ELABORO"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Registró" DataField="REGISTRO" SortExpression="REGISTRO"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"
                                                        HeaderStyle-ForeColor="White" HeaderText="Destinatario" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Image ID="logoImg" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn HeaderText="T_INICIALES" DataField="T_INICIALES" SortExpression="T_INICIALES"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Entidad" DataField="T_ENTIDAD_CORTO" SortExpression="T_ENTIDAD_CORTO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Año" DataField="ID_ANIO" HeaderStyle-CssClass="BO_Column"
                                                        SortExpression="ID_ANIO" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Tipo de Documento" DataField="T_TIPO_DOCUMENTO" SortExpression="T_TIPO_DOCUMENTO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" SortExpression="DESTINATARIO"
                                                        HeaderStyle-CssClass="BO_Column" HeaderStyle-ForeColor="White" HeaderText="Destinatario">
                                                        <ItemTemplate>
                                                            <asp:Label ID="destinatario" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn HeaderText="Fecha Notificacion" DataField="F_FECHA_NOTIFICACION" SortExpression="F_FECHA_NOTIFICACION"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Fecha de Recepción" DataField="F_FECHA_RECEPCION" SortExpression="F_FECHA_RECEPCION"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Fecha de Oficio" DataField="F_FECHA_OFICIO" SortExpression="F_FECHA_OFICIO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Fecha de Acuse" DataField="F_FECHA_ACUSE" SortExpression="F_FECHA_ACUSE"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="Cedula" HeaderText="Generar Cédula PDF" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="btn_notificar" HeaderStyle-ForeColor="White"></asp:ButtonColumn>
                                                    <asp:ButtonColumn
                                                        CausesValidation="false" CommandName="Notificar" HeaderText="Notificar" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="btn_notificar" HeaderStyle-ForeColor="White"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_HYP_ARCHIVOWORD" SortExpression="T_HYP_ARCHIVOWORD" HeaderText="Word"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="T_HYP_ARCHIVOSCAN" SortExpression="T_HYP_ARCHIVOSCAN" HeaderText="PDF"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_HYP_CEDULAPDF" SortExpression="T_HYP_CEDULAPDF" HeaderText="Cédula"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_CEDULADIGITAL" SortExpression="T_CEDULADIGITAL" HeaderText="Cédula SBM"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_HYP_FIRMADIGITAL" SortExpression="T_HYP_FIRMADIGITAL" HeaderText="Firma SBM"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_HYP_RESPUESTAOFICIO" SortExpression="T_HYP_RESPUESTAOFICIO" HeaderText="Respuesta_old"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"
                                                        Visible="false"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_HYP_ACUSERESPUESTA" SortExpression="T_HYP_ACUSERESPUESTA" HeaderText="Acuse"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_ANEXO_UNO" SortExpression="T_ANEXO_UNO" HeaderText="Anexos" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_ANEXO_DOS" SortExpression="T_ANEXO_DOS" HeaderText="Anexos SBM"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CausesValidation="false"
                                                        CommandName="T_HYP_EXPEDIENTE" SortExpression="T_HYP_EXPEDIENTE" HeaderText="Expediente"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonColumn>
                                                    <asp:ButtonColumn CommandName="DICTAMINADO_FLAG"
                                                        SortExpression="DICTAMINADO_FLAG" HeaderText="Dictaminado" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-CssClass="BO_Column"></asp:ButtonColumn>
                                                    <asp:BoundColumn ItemStyle-HorizontalAlign="Center" DataField="F_FECHA_VENCIMIENTO" HeaderText="FECHA_VENCIMIENTO"
                                                        SortExpression="F_FECHA_VENCIMIENTO" HeaderStyle-CssClass="BO_Column" Visible="true"></asp:BoundColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderText="Notificación" SortExpression="NOTIF_ELECTRONICA_FLAG"
                                                        HeaderStyle-CssClass="BO_Column"></asp:TemplateColumn>
                                                    <asp:BoundColumn HeaderText="Clasificación" DataField="T_CLASIFICACION" SortExpression="T_CLASIFICACION"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:ButtonColumn CausesValidation="false" Text="DoubleClick" CommandName="DoubleClick"
                                                        Visible="false" />
                                                    <asp:BoundColumn HeaderText="Estatus" DataField="T_ESTATUS" SortExpression="T_ESTATUS"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-Font-Bold="true"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="ID_TIPO_DOCUMENTO" DataField="ID_TIPO_DOCUMENTO" SortExpression="ID_TIPO_DOCUMENTO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="ID_ENTIDAD_TIPO" DataField="ID_ENTIDAD_TIPO" SortExpression="ID_ENTIDAD_TIPO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="ID_CLASIFICACION" DataField="ID_CLASIFICACION" SortExpression="ID_CLASIFICACION"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>

                                                    <asp:BoundColumn HeaderText="ID_ENTIDAD" DataField="ID_ENTIDAD" SortExpression="ID_ENTIDAD"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>

                                                    <asp:BoundColumn HeaderText="ID_AREA_OFICIO" DataField="ID_AREA_OFICIO" SortExpression="ID_AREA_OFICIO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="ID_ANIO" DataField="ID_ANIO" SortExpression="ID_ANIO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="I_OFICIO_CONSECUTIVO" DataField="I_OFICIO_CONSECUTIVO" SortExpression="I_OFICIO_CONSECUTIVO"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>

                                                    <asp:BoundColumn HeaderText="NOTIF_ELECTRONICA_FLAG" DataField="NOTIF_ELECTRONICA_FLAG" SortExpression="NOTIF_ELECTRONICA_FLAG"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" Visible="false"></asp:BoundColumn>

                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <!--Tabla Control Botones--->
                <table id="tblControles" runat="server" width="800px" align="center" visible="false">
                    <tr>
                        <td align="left" class="style1">
                            <div id="Boton_guardar" runat="server" visible="false">
                            </div>
                            <%-- <fieldset class="txt_gral">
                            <legend>Tipo de Docto.</legend>&nbsp;<asp:Image ID="imgOriginal" ImageUrl="~/images/original.gif"
                                runat="server" /><asp:Label ID="lblOriginal" Text=" = Original" CssClass="txt_gral"
                                    runat="server"></asp:Label>
                            &nbsp;<asp:Image ID="imgCopia" ImageUrl="../../images/copia.gif" runat="server" /><asp:Label
                                ID="lblCopia" Text=" = Copia" CssClass="txt_gral" runat="server"></asp:Label>
                            &nbsp;<asp:Image ID="Image1" ImageUrl="~/images/TemplateTab2.gif" runat="server" /><asp:Label
                                ID="Label1" Text=" = Turnado Original" CssClass="txt_gral" runat="server"></asp:Label>
                            &nbsp;<asp:Image ID="Image8" ImageUrl="~/images/TemplateTab1.gif" runat="server" /><asp:Label
                                ID="Label3" Text=" = Turnado Copia" CssClass="txt_gral" runat="server"></asp:Label>
                        </fieldset>--%>
                        </td>
                        <td>
                            <%-- &nbsp;<asp:Image ID="Image6" runat="server" Height="16px" ImageUrl="~/images/Duplicado.png"
                            Width="16px" />
                        <asp:Label ID="lblDuplicado" runat="server" CssClass="txt_gral">= Duplicado</asp:Label>
                        &nbsp;<asp:Image ID="Image7" runat="server" Height="16px" ImageUrl="~/images/Asociado.png"
                            Width="16px" />
                        <asp:Label ID="lblAsociado" runat="server" CssClass="txt_gral">= Asociado</asp:Label>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <%--<fieldset class="txt_gral">
                            <legend>Estatus</legend>&nbsp;<asp:Image ID="Image4" runat="server" Height="16px"
                                ImageAlign="AbsBottom" ImageUrl="~/images/statusNormal.png" Width="16px" />
                            <asp:Label ID="lblNormal" runat="server" CssClass="txt_gral">= Normal</asp:Label>
                            &nbsp;<asp:Image ID="Image9" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/images/tramite.png" Width="16px" />
                            <asp:Label ID="Label2" runat="server" CssClass="txt_gral">= En trámite</asp:Label>
                            &nbsp;<asp:Image ID="lblEsperaVobo" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/images/question.png" Width="16px" />
                            <asp:Label ID="Label4" runat="server" CssClass="txt_gral">= Espera Vo. Bo.</asp:Label>
                            &nbsp;<asp:Image ID="lblComplemento" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/images/question.png" Width="16px" />
                            <asp:Label ID="Label5" runat="server" CssClass="txt_gral">= Complemento</asp:Label>
                            &nbsp;<asp:Image ID="Image3" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/images/PREVENTIVO.png" Width="16px" />
                            <asp:Label ID="lblPorVencer" runat="server" CssClass="txt_gral">= Por vencer</asp:Label>
                            &nbsp;<asp:Image ID="Image5" runat="server" Height="16px" ImageUrl="~/images/VENCIDO.png"
                                Width="16px" />
                            <asp:Label ID="lblVencido" runat="server" CssClass="txt_gral">= Vencido</asp:Label>
                            &nbsp;<asp:Image ID="Image2" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/images/ATENDIDO.png" Width="16px" />
                            <asp:Label ID="lblAtendido" runat="server" CssClass="txt_gral">= Atendido</asp:Label>
                            &nbsp;
                        </fieldset>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <br />
                            <br />
                            <asp:Button ID="btnReasignar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Width="123px" CssClass="botones" Text="Reasignar" Height="20px"
                                Visible="False"></asp:Button>
                            &nbsp;<asp:Button ID="btnTurnar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Width="123px" CssClass="botones" Text="Turnar" Height="20px" Visible="false"></asp:Button>
                            &nbsp;<asp:Button ID="btnQuitarTurnado" runat="server" CssClass="botones" Height="20px"
                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Consultar Turnado" Visible="false" Width="123px" />
                            <asp:Button ID="btnAdjuntarSIE" runat="server" CssClass="botones" Height="0px" onmouseout="style.backgroundColor='#696969'"
                                onmouseover="style.backgroundColor='#A9A9A9'" Text="Adjuntar SIE" Width="0px"
                                Visible="False" />
                            &nbsp;<asp:Button ID="btnGuardar" runat="server" CssClass="botones" Height="20px"
                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Guardar" Visible="False" Width="130" OnClientClick="return ShowProcesa()" />
                            <asp:HiddenField runat="server" ID="hdnChecks" />
                        </td>
                    </tr>
                    <tr id="trBtnCancelar" runat="server" visible="false">
                        <td align="center" colspan="2">
                            <asp:Button ID="btnCancelar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Width="123px" CssClass="botones" Text="Cancelar" Height="20px"></asp:Button>
                        </td>
                    </tr>
                </table>
                <!--Tabla Controles Modales--->
                <table width="800px" align="center">
                    <tr>
                        <td>
                            <%-- <asp:Panel ID="PanelPersonaliza" runat="server">
                           <%-- <table>
                                <tr>
                                    <td>
                                        <cc1:PersonalizarColumas ID="ModalPersonalizarColumas" runat="server" style="display: none" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--by --%>
                            <%--Boton para cerrar la modal Personalizado--%>
                            <%--  <button id="btnCerrarModal" runat="server" style="display: none">
                                            Cerrar Modal Personalizar</button>
                                        <%--Boton para actualizar el Grid al cerrar la modal --%>
                            <%-- <asp:Button ID="btnAcualizaGridPersonalizado" runat="server" Text="Actualiza Grid"
                                            Style="display: none" />
                                      
                                    </td>
                                </tr>
                            </table>--%>
                            <%-- <asp:ModalPopupExtender ID="ModalPersinaliza" runat="server" PopupControlID="PanelPersonaliza"
                                TargetControlID="BtnPersonalizar" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle"
                                DropShadow="true" CancelControlID="btnCerrarModal">
                            </asp:ModalPopupExtender>--%>
                            <%--   </asp:Panel>--%>
                            <asp:UpdatePanel runat="server" ID="updPanel">
                                <ContentTemplate>
                                    <div>
                                        <asp:LinkButton ID="LB1" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
                                        <asp:Panel ID="PanelErrores" runat="server" CssClass="PanelErrores" Width="550px">
                                            <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                                                cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="PanelErroresHandle" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                                            Height="20px">
                                                            <asp:Label runat="server" ID="lblErroresTitulo" Text="Información" Style="vertical-align: middle; margin-left: 5px;"
                                                                CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>

                                                        <asp:Label runat="server" ID="lblErroresPopup" Visible="true" Text="Debe seleccionar los filtros visibles:"
                                                            CssClass="txt_gral txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblFecRec" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblFecDoc" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblTDoc" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblArea" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblDest" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblRec" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblRefere" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblRegistros" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblFolio" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblOficio" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblRemitente" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblAtendidaStatus" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblAsunto" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblResponsable" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblFechaRegistro" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblFechaLimite" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                        <asp:Label runat="server" ID="lblProvieneSIE" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center; height: 35px">
                                                        <center>
                                                            <asp:Button ID="BtnModalOk" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                                                CssClass="botones" />
                                                            <asp:Button ID="BtnContinua" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                                                CssClass="botones" Visible="false" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                                            TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                                            PopupDragHandleControlID="PanelErroresHandle" CancelControlID="BtnModalOk">
                                        </asp:ModalPopupExtender>
                                        <asp:Label runat="server" ID="lblErrores" CssClass="txt_gral_rojo" Visible="false"
                                            Style="display: none"></asp:Label>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="visibility: hidden; height: 0px">
                                <asp:LinkButton ID="linkbotonArchivo" runat="server" ClientIDMode="Static" Text="Click_me"></asp:LinkButton>
                                <asp:LinkButton ID="BotonInicio" runat="server" ClientIDMode="Static" Text="Click_me"
                                    OnClick="CargaGRID_click"></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:LinkButton ID="lnkCaptura" runat="server" Text="PopUp" Style="display: none"></asp:LinkButton>
                <asp:Panel ID="PanelCaptura" runat="server" CssClass="PanelErrores" Width="690px"
                    Style="display: none">
                    <table style="border-style: solid; border-width: 2px; border-color: White" cellspacing="0"
                        cellpadding="0" width="690px">
                        <tr>
                            <td style="text-align: center">
                                <asp:Panel ID="PanelCapturaHandel" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                    Height="40px">
                                    <asp:Label runat="server" ID="lblCapturaTitulo" Text="Confirmar Anexos Recibidos"
                                        Style="vertical-align: middle; margin-left: 5px; color: White" CssClass="titulo_seccioninterior"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <%-- <asp:Panel Width="100%" Height="300px" ID="pnlAnexos" runat="server" ScrollBars="Vertical">
                                <asp:GridView ID="grvAnexos" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                    ShowHeader="false" Width="95%">
                                    <Columns>
                                        <asp:BoundColumn DataField="ID_FOLIO" ItemStyle-CssClass="hide" />
                                        <asp:TemplateColumn ItemStyle-Width="665px">
                                            <ItemTemplate>
                                                <AnexosRow:AnexosRegistro ID="AnexoRenglon" runat="server" ID_FOLIO='<%# Bind("ID_FOLIO") %>'
                                                    CHECK_VALUES='<%# Bind("Anexos") %>' OTROS_DSC='<%# Bind("DSC_OTRO") %>' ImageURL='../../images/question.png' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                <asp:Button ID="BtnCapturaOK" runat="server" Text="Aceptar" CssClass="botones" Width="130"
                                    Height="22px" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                    OnClientClick="return ShowProcesa()" />
                                &nbsp;
                            <asp:Button ID="BtnCapturaCancelar" runat="server" Text="Cancelar" CssClass="botones"
                                Width="130" Height="22px" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ModalPopupExtender ID="ModalPopUpCaptura" runat="server" PopupControlID="PanelCaptura"
                    TargetControlID="lnkCaptura" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                    PopupDragHandleControlID="PanelCapturaHandel" CancelControlID="BtnCapturaCancelar">
                </asp:ModalPopupExtender>
                <asp:LinkButton ID="lnkProcesa" runat="server" Text="" Style="display: none"></asp:LinkButton>
                <asp:Panel ID="pnlProcesando" runat="server" Width="400px" Height="200px" CssClass="FondoManualOut">
                    <asp:LinkButton ID="lnkCierra" runat="server" Text="" Style="display: none"></asp:LinkButton>
                    <img src="../../../Images/carga.gif" alt="Espere un momento" />
                </asp:Panel>
                <asp:ModalPopupExtender ID="mpeProcesa" runat="server" PopupControlID="pnlProcesando"
                    BackgroundCssClass="FondoAplicacion" TargetControlID="lnkProcesa" CancelControlID="lnkCierra">
                </asp:ModalPopupExtender>
                <asp:Button ID="btnLigar" OnClick="LigarFolioSICOD" runat="server" Style="display: none" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <div id="divMensajeRespuesta" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image2" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <div id="divTextoRespuesta"></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
