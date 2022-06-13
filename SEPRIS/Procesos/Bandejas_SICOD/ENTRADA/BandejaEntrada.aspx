<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BandejaEntrada.aspx.vb"
    EnableEventValidation="false" Inherits="SEPRIS.BandejaEntrada" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="SISVIG" TagName="FiltroBusqueda" Src="FiltroBusqueda.ascx" %>

<%@ Register TagPrefix="AnexosRow" TagName="AnexosRegistro" Src="UserControls/ucAnexoModalRow.ascx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SISVIG</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="Styles.css" type="text/css" rel="Stylesheet" />
       <script src="Scripts/script.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
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
        } ();


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
                            Bandeja de Entrada SICOD
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="BtnPersonalizar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                            runat="server" Height="22px" ToolTip="Personalizar Columnas" Text="Personalizar Columas"
                            CssClass="botones" Width="130" Visible="False"></asp:Button>
                              <asp:TextBox ID="NO_OFICIO" runat="server" VISIBLE="false"></asp:TextBox>
                              <asp:TextBox ID="ID_FOLIO" runat="server" VISIBLE="false"></asp:TextBox>
                       <asp:TextBox ID="FECHA_DOCUMENTO" runat="server" VISIBLE="false"></asp:TextBox>
                        <asp:TextBox ID="FECHA_RECEPCION" runat="server" VISIBLE="false"></asp:TextBox>
                       <br />
                    </td>
                </tr>
                <tr>
                    <td class="FiltroBandeja">
                        <table width="100%">
                            <tr>
                                <td>
                                    <SISVIG:FiltroBusqueda ID="Filtros" runat="server"></SISVIG:FiltroBusqueda>
                                  <%--  <uc1:ucFiltro ID="ucFiltro1" runat="server" />--%>
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
                                    <div id="GRID" runat="server" style="overflow: auto; width: 800px; height: 450px;
                                        position: absolute">
                                        <asp:DataGrid ID="GridPrincipal" runat="server" Font-Size="7.5pt" AutoGenerateColumns="False"
                                            BackColor="#D6EBBD" CellPadding="1" Font-Name="Arial" HorizontalAlign="Center"
                                            Font-Names="Arial" ForeColor="#555555" DataKeyField="ID_FOLIO" BorderColor="White"
                                            >
                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" ForeColor="#FFFFFF"
                                                CssClass="HeaderBandeja"></HeaderStyle>
                                            <Columns>
                                                <%--Item.Cells(0)--%>
                                                <asp:TemplateColumn HeaderText="T" ItemStyle-CssClass="ColumnFixBandeja" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="50px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSeleccionar" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(1)--%>
                                                <asp:ButtonColumn Text="O" CommandName="Delete" Visible="False">
							    <ItemStyle HorizontalAlign="Center" Width="20px" ></ItemStyle>
							</asp:ButtonColumn>
                                                <asp:TemplateColumn HeaderText="Folio" ItemStyle-Width="50px" SortExpression="ID_FOLIO" ItemStyle-CssClass="ColumnFixBandeja">
                                                    <ItemStyle HorizontalAlign="left" Width="180px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkHistorial" runat="server" Text='<%# Bind("ID_FOLIO") %>' ForeColor="White"
                                                             ></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(2)--%>
                                                <asp:BoundColumn DataField="FECH_RECEPCION" HeaderText="Fecha Rec. Doc." ReadOnly="true"
                                                    ItemStyle-Width="75px" SortExpression="FECH_RECEPCION">
                                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(3)--%>
                                                <asp:BoundColumn DataField="DSC_REFERENCIA" HeaderText="Referencia" ReadOnly="true"
                                                    ItemStyle-Width="150px" SortExpression="DSC_REFERENCIA">
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(4)--%>
                                                <asp:BoundColumn DataField="DSC_NUM_OFICIO" HeaderText="Oficio" ReadOnly="true" ItemStyle-Width="100px"
                                                    SortExpression="DSC_NUM_OFICIO">
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(5)--%>
                                                <asp:BoundColumn DataField="DSC_T_DOC" HeaderText="Tipo de Documento" ReadOnly="true"
                                                    ItemStyle-Width="75px" SortExpression="DSC_T_DOC">
                                                    <ItemStyle Width="75px" HorizontalAlign="Left" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(6)--%>
                                                <asp:BoundColumn DataField="DSC_REMITENTE" HeaderText="Remitente" ReadOnly="true"
                                                     SortExpression="DSC_REMITENTE" Visible="true">
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(7)--%>
                                                <asp:BoundColumn HeaderText="Anexo" DataField="ANEXO" SortExpression="ANEXO" Visible="false"></asp:BoundColumn>
                                                <%--Item.Cells(8)--%>
                                                <asp:TemplateColumn HeaderText="Nombre Archivo" ItemStyle-Width="300px" SortExpression="NOM_ARCH" Visible="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkArchivo" runat="server" Text='<%# Bind("DSC_ARCHIVO") %>' OnClick="nomArch_Click1"
                                                            ForeColor="Blue"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(9)--%>
                                                <asp:TemplateColumn HeaderText="Recibido" ItemStyle-HorizontalAlign="Center"  Visible="false"
                                                    SortExpression="ESTATUS_RECIBIDO">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRecibido" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(10)--%>
                                                <asp:BoundColumn DataField="DESTINATARIO" HeaderText="Destinatario" ReadOnly="true"
                                                    Visible="false" SortExpression="DESTINATARIO">
                                                    <ItemStyle Width="20px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(11)--%>
                                                <asp:BoundColumn DataField="ESTATUS_RECIBIDO" HeaderText="Recibido" ReadOnly="true"
                                                    Visible="false"  SortExpression="ESTATUS_RECIBIDO">
                                                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(12)--%>
                                                <asp:TemplateColumn HeaderText="Es Copia" ItemStyle-Width="50px" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"  />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgD" runat="server" ImageUrl='<%# ImagenEstatusCopia(DataBinder.Eval(Container.DataItem,"DESTINATARIO"), DataBinder.Eval(Container.DataItem,"BLOQ_TURNADO"))%>' />
                                                        <asp:Label ID="lblEsCopia" runat="server" Text='<%# Bind("DESTINATARIO") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(13)--%>
                                                <asp:BoundColumn DataField="DSC_ARCHIVO" HeaderText="Nombre Archivo" ReadOnly="true"
                                                    Visible="true" SortExpression="NOMBRE_ARCHIVO">
                                                    <ItemStyle ></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(14)--%>
                                                <asp:BoundColumn DataField="ID_ARCHIVO" HeaderText="Clave Archivo" ReadOnly="true"
                                                    Visible="false" ItemStyle-Width="20px" SortExpression="ID_ARCHIVO">
                                                    <ItemStyle Width="20px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(15)--%>
                                                <asp:BoundColumn DataField="ESTATUS_ATENDIDA" HeaderText="ATENDIDA" ReadOnly="true"
                                                    Visible="false" SortExpression="ESTATUS_ATENDIDA">
                                                    <ItemStyle></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(16)--%>
                                                <asp:TemplateColumn HeaderText="Atendido" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px"
                                                    SortExpression="ESTATUS_ATENDIDA" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAtendido" runat="server" Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(17)--%>
                                                <asp:BoundColumn DataField="DSC_ASUNTO" HeaderText="Asunto" ReadOnly="true" ItemStyle-Width="200px"
                                                    SortExpression="DSC_ASUNTO">
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                </asp:BoundColumn>
                                                <%--Item.Cells(18)--%>
                                                <asp:BoundColumn DataField="NUM_ATENCION" ReadOnly="True" HeaderText="Num. Oficio"
                                                    SortExpression="NUM_ATENCION" visible="false">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(19)--%>
                                                <asp:BoundColumn DataField="FECH_REGISTRO" ReadOnly="True" HeaderText="F.Registro"
                                                    DataFormatString="{0:dd/MM/yyyy}" SortExpression="FECH_REGISTRO">
                                                    <ItemStyle HorizontalAlign="Center" Width="175px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(20)--%>
                                                <asp:BoundColumn DataField="USUARIO" ReadOnly="True" HeaderText="Usuario" Visible="false"
                                                    SortExpression="USUARIO"></asp:BoundColumn>
                                                <%--Item.Cells(21)--%>
                                                <asp:BoundColumn DataField="FECHA_LIMITE" ReadOnly="True" HeaderText="F.Limite" SortExpression="FECHA_LIMITE"
                                                    DataFormatString="{0:dd/MM/yyyy}" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(22)--%>
                                                <asp:TemplateColumn HeaderText="Archivo SIE" ItemStyle-Width="300px" SortExpression="ARCHIVO_SIE"
                                                    ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide">
                                                    <FooterStyle CssClass="hide" />
                                                    <HeaderStyle CssClass="hide" />
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkArchivoSIE" runat="server" Text='<%# Bind("ARCHIVO_SIE") %>'
                                                            ForeColor="Blue"></asp:LinkButton>
                                                        &nbsp;<asp:Label ID="lblArchivoSIE" runat="server" Text="Consulte Documentos Adjuntos de SIE"
                                                            Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <%--Item.Cells(23)--%>
                                                <asp:BoundColumn DataField="NOMBRE" ReadOnly="True" ItemStyle-Width="300px" HeaderText="Responsable"
                                                    SortExpression="NOMBRE">
                                                    <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(24)--%>
                                                <asp:BoundColumn DataField="INSTRUCCIONES" ReadOnly="True" ItemStyle-Width="300px"
                                                    HeaderText="Instrucciones" SortExpression="INSTRUCCIONES">
                                                    <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%--Item.Cells(25)--%>
                                                <asp:TemplateColumn HeaderText="SIE" ItemStyle-Width="150px" SortExpression="CORREO_SIE" Visible="false">
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCorreosSIE" runat="server" Text='<%# Bind("CORREO_SIE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                               <%-- <asp:TemplateColumn HeaderText="Asociar Archivos" HeaderStyle-Width="400px">
                                                    <HeaderStyle Width="400px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="400px" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgAsociado" runat="server" Width="22px" Height="22px" ImageUrl='<%# FolioAsociados(DataBinder.Eval(Container.DataItem, "ID_EXPEDIENTE"), DataBinder.Eval(Container.DataItem, "DUPLICADO_FLAG"))  %>' />
                                                        &nbsp;<asp:LinkButton ID="lnkAsociar" runat="server" Text="Asociar Documento" ForeColor="Blue"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>--%>
                                                <asp:TemplateColumn HeaderText="DxV" ItemStyle-Width="200px" SortExpression="DxV" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalDias" runat="server" Text='<%# Bind("DxV") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Estatus" SortExpression="ATENDIDA" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgEstatus" runat="server" Width="22px" Height="22px" ImageUrl='<%# ImagenFechaLimite(DataBinder.Eval(Container.DataItem, "ATENDIDA"))  %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <%-- 28  --%>
                                                <asp:TemplateColumn Visible="false">
                                                   <%-- <ItemTemplate>
                                                        <asp:Button ID="BtnAtender" runat="server" CssClass="botones" Text="Atender" />
                                                    </ItemTemplate>--%>
                                                    <ItemStyle HorizontalAlign="Center" Width="175px" />
                                                </asp:TemplateColumn>
                                                <%-- e.Item.Cells(30)--%>
                                                <asp:BoundColumn DataField="ARCHIVO_SBM" ReadOnly="True" HeaderText="ARCHIVO_SBM"
                                                    Visible="true" SortExpression="ARCHIVO_SBM">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%-- e.Item.Cells(31)--%>
                                                <asp:BoundColumn DataField="ID_EXPEDIENTE" ReadOnly="True" HeaderText="ID_EXPEDIENTE"
                                                    Visible="false" SortExpression="ID_EXPEDIENTE">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%-- e.Item.Cells(32) --%>
                                                <asp:BoundColumn DataField="BLOQ_TURNADO" ReadOnly="True" HeaderText="" Visible="false"
                                                    SortExpression="BLOQ_TURNADO">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%-- e.Item.Cells(33) --%>
                                                <asp:BoundColumn DataField="ORIGINAL_FLAG" ReadOnly="True" HeaderText="" Visible="false"
                                                    SortExpression="ORIGINAL_FLAG">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%-- e.Item.Cells(34)--%>
                                                <asp:BoundColumn DataField="TURNADO_FLAG" ReadOnly="True" HeaderText="TURNADO_FLAG"
                                                    Visible="false" SortExpression="TURNADO_FLAG">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <%-- e.Item.Cells(35)--%>
                                                <asp:BoundColumn DataField="ESTATUS_TRAMITE" ReadOnly="True" HeaderText="ESTATUS_TRAMITE"
                                                    Visible="false" SortExpression="ESTATUS_TRAMITE">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="FECH_DOC" ReadOnly="True" HeaderText="FECH_DOC"
                                                    Visible="true" SortExpression="FECH_DOC">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ID_FOLIO" HeaderText="ID_FOLIO" ReadOnly="true"
                                                    ItemStyle-Width="75px" SortExpression="ID_FOLIO" Visible="false">
                                                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                                                </asp:BoundColumn>
                                                <%-- e.Item.Cells(36)--%>
                                              <%--  <asp:ButtonColumn ButtonType="LinkButton" CausesValidation="false" Text="DoubleClick"
                                                    CommandName="DoubleClick" Visible="false" />--%>
                                                <%-- e.Item.Cells(37)--%>
                                                <%--<asp:TemplateColumn HeaderText="N&uacute;m. Expediente" ItemStyle-Width="180px" SortExpression="ID_EXPEDIENTE_EXT">
                                                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkExpediente" runat="server" Text='<%# Bind("ID_EXPEDIENTE_EXT") %>'
                                                            ForeColor="Blue"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>--%>
                                            </Columns>
                                            <ItemStyle Width="800px" />
                                        </asp:DataGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <!--Tabla Control Botones--->
            <table id="tblControles" runat="server" width="800px" align="center" visible="false">
                <tr>
                    <td align="left" class="style1">
                        <div id="Boton_guardar" runat="server">
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
                            runat="server" Width="123px" CssClass="botones" Text="Turnar" Height="20px" Visible="false">
                        </asp:Button>
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
                            runat="server" Width="123px" CssClass="botones" Text="Cancelar" Height="20px">
                        </asp:Button>
                    </td>
                </tr>
            </table>
            <!--Tabla Controles Modales--->
            <table width="800px" align="center">
                <tr>
                    <td>
                        <asp:Panel ID="PanelPersonaliza" runat="server" Visible="false" >
                            <table> 
                                <tr>
                                    <td>
                                                                            </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--by --%>
                                        <%--Boton para cerrar la modal Personalizado--%>
                                        <button id="btnCerrarModal" runat="server" style="display: none">
                                            Cerrar Modal Personalizar</button>
                                        <%--Boton para actualizar el Grid al cerrar la modal --%>
                                        <asp:Button ID="btnAcualizaGridPersonalizado" runat="server" Text="Actualiza Grid"
                                            Style="display: none" />
                                    </td>
                                </tr>
                            </table>
                            <asp:ModalPopupExtender ID="ModalPersinaliza" runat="server" PopupControlID="PanelPersonaliza"
                                TargetControlID="BtnPersonalizar" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle"
                                DropShadow="true" CancelControlID="btnCerrarModal">
                            </asp:ModalPopupExtender>
                        </asp:Panel>
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
                                                        <asp:Label runat="server" ID="lblErroresTitulo" Text="Información" Style="vertical-align: middle;
                                                            margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--&nbsp;&nbsp;--%>
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
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Panel Width="100%" Height="300px" ID="pnlAnexos" runat="server" ScrollBars="Vertical">
                                <asp:GridView ID="grvAnexos" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                    ShowHeader="false" Width="95%">
                                    <Columns>
                                        <asp:BoundField DataField="ID_FOLIO" ItemStyle-CssClass="hide" />
                                        <asp:TemplateField ItemStyle-Width="665px">
                                            <ItemTemplate>
                                                <AnexosRow:AnexosRegistro ID="AnexoRenglon" runat="server" ID_FOLIO='<%# Bind("ID_FOLIO") %>'
                                                    CHECK_VALUES='<%# Bind("Anexos") %>' OTROS_DSC='<%# Bind("DSC_OTRO") %>' ImageURL='../../images/question.png' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
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
                        <td>
                            &nbsp;
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
