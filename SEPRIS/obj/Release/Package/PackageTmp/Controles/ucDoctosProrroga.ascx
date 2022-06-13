<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDoctosProrroga.ascx.vb" Inherits="SEPRIS.ucDoctosProrroga" %>
<%@ Register src="~/Controles/ucFiltro.ascx" tagname="ucFiltro" tagprefix="uc1" %>

<script type="text/javascript">
    //Detectar navegador
    var nav = navigator.userAgent.toLowerCase();

    //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
    if (nav.indexOf("msie") != -1) {
        document.write("<link href='../Styles/estilosIE.css' rel='stylesheet' />");
    } else {
        document.write("<link href='../Styles/estilosChrome.css' rel='stylesheet' />");
    }

    function GuardaScrollDocs(psIdDiv, psIdCajaOculta) {
        var elemento = $('#' + psIdDiv);

        if (elemento != null) {
            var pos = $('#' + psIdDiv).scrollTop();
            $('#' + psIdCajaOculta).val(pos);
        }
    }

    function SetScroll(psIdDiv, psIdCajaOculta) {
        //Restablecer la posicion del scroll del div de documentos
        var divdocs = $('#' + psIdDiv);

        if (divdocs != null) {
            var hf = $('#' + psIdCajaOculta);
            if (hf != null) {
                divdocs.scrollTop(hf.val());
            }
        }
    }

    //$(document).ready(function () {
    //    SetScroll();
    //});

    function HabilitaLink(idBtnImg, idBtnLink, idBtnFu, idBtnAdj, idBtnSicod) {
        //alert(idBtnLink + ", " + idBtnFu + ", " + idBtnAdj);
        //var btnLink = document.getElementById(idBtnLink);
        $("#" + idBtnImg).hide();
        $("#" + idBtnLink).hide();
        $("#" + idBtnFu).removeClass("OcultarControl");
        $("#" + idBtnAdj).removeClass("OcultarControl");

        if (idBtnSicod != "")
            $("#" + idBtnSicod).removeClass("OcultarControl");

        return false;
    }

    function LevantaVentanaOficio(idRen, idDocumento, idVersion, idNumVersiones, idVisita) {
        MuestraImgCarga(null);

        var url = '';
        //Guarda el id del documento en un campo oculto
        if (idVisita != "-1") {
            $("#hfIdRenglon_" + idVisita).val(idRen);
            $("#hfIdDocSicod_" + idVisita).val(idDocumento);
            $("#hfIdVerDocSicod_" + idVisita).val(idVersion);
            $("#hfNumVerDocSicod_" + idVisita).val(idNumVersiones);
            $("#hfVisita_" + idVisita).val(idVisita);
        } else {
            $("#hfIdRenglon").val(idRen);
            $("#hfIdDocSicod" ).val(idDocumento);
            $("#hfIdVerDocSicod" ).val(idVersion);
            $("#hfNumVerDocSicod" ).val(idNumVersiones);
            $("#hfVisita" ).val(idVisita);
        }

        //Levanta la modal
        url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';

        //Detectar navegador
        var nav = navigator.userAgent.toLowerCase();

        //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
        if (nav.indexOf("msie") != -1) {
            winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
            window.showModalDialog(url, "SICOD", winprops);
            OcultaImagenCarga();
            return true;
        } else {
            var liTop = (parseInt($(window).height()) / 2) - (450 / 2);
            var liLeft = (parseInt($(window).width()) / 2) - (850 / 2);

            winprops = "width=850,height=450,toolbar=no, menubar=no, scrollbars=no, resizable=Yes, location=no, status=no, left=" + liLeft + ", top=" + liTop;
            //alert("winprops: " + winprops);
            var modal = window.open(url + "?r=" + idRen + "&ds=" + idDocumento + "&vds=" + idVersion, "SICOD", winprops);
            modal.focus();
            OcultaImagenCarga();
            return false;
        }
    }

    function llamaPostback() {
        //__doPostBack('', '');
        location.reload(true);
    }


</script>

<style type="text/css">
    .fuGrid {
        /*float:left;*/
        margin-top:10px;
        
    }
    
    .imgGrid {
        /*float:left;*/
        width:25px;
        margin-top:7px;
    }

    .aspNetDisabled
        {
        color: #000;
        }

    .OcultarControl {
        display:none;
    }

    .gridScrollW {
        
    }

    .cssErrorDocs {
        color:red;
        font-size :14px;
    }

    </style>
<asp:UpdatePanel ID="upGridDocs" runat="server" UpdateMode="Always"  >
    <ContentTemplate>
        <%--<asp:Panel ID="pnlEncabezado" runat="server" Width="100%" >
            <div style="text-align:left; width:100%; padding-bottom: 5px; padding-left:2%">
                    <asp:Button ID="btnExportaExcelDocs" runat="server" Text="Exportar a Excel" />
                </div>
            <div style="text-align:left; width:100%; padding-bottom: 5px; padding-left:2%">
                <uc1:ucFiltro ID="ucFiltroDocs" runat="server" Width="96%" />
            </div>
            <br />
            <br />
        </asp:Panel>--%>

        <%-- margin-left:2%;--%>
        <asp:Panel ID="pnlGrid" runat="server" Width="100%" ClientIDMode="Static">
            <div style="OVERFLOW:hidden ; BACKGROUND: #ffffff; Z-INDEX: 0; width:95%; height:25px; margin-left:2%; text-align:left; position:static;">
                <asp:GridView  ID="gvEncabecados" runat="server" RowStyle-HorizontalAlign="Left"  
                    CssClass="anchoGriDocsEncabezado" RowStyle-CssClass="GridViewEncabezado">
                    <Columns>
                        <%--<asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px" Visible="false" ItemStyle-CssClass="GridViewEncabezado">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <%--<asp:BoundField HeaderText="PASO" DataField="I_ID_PASO_INI" ItemStyle-Width="33px" ItemStyle-CssClass="GridViewEncabezado"/>
                        <asp:BoundField HeaderText="DOCUMENTO" DataField="T_NOM_DOCUMENTO_CAT" ItemStyle-Width="40%" ItemStyle-CssClass="GridViewEncabezado"/>
                        <asp:BoundField HeaderText="DOCUMENTOS ADJUNTOS" DataField="T_NOM_DOCUMENTO_ADJ" ItemStyle-Width="40%" ItemStyle-CssClass="GridViewEncabezado"/>
                        <asp:BoundField HeaderText="" ItemStyle-Width="30px" ItemStyle-CssClass="GridViewEncabezado"/>--%>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="pnlGridInt"  runat="server" ClientIDMode="Static" style="overflow-x:hidden; OVERFLOW:auto; BACKGROUND: #ffffff; Z-INDEX: 0; width:95%; height:100px;  margin-left:2%;  text-align:left;">
                <asp:GridView  ID="gvConsultaDocs" runat="server" RowStyle-Height ="25px" RowStyle-HorizontalAlign="Left"
                    CssClass="anchoGriDocs" ShowHeader="false" DataKeyNames="T_NOM_DOCUMENTO_CAT">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="PASO" DataField="I_ID_PASO_INI" ItemStyle-Width="33px" Visible="false"/>
                        <asp:BoundField HeaderText="DOCUMENTO" DataField="T_NOM_DOCUMENTO_CAT" ItemStyle-Width="40%"/>
                        <asp:TemplateField HeaderText ="DOCUMENTOS ADJUNTOS" ItemStyle-Width="85%">
                            <ItemTemplate>
                               <asp:ImageButton ID="btnAgregarDoc" runat="server" ImageUrl="~/Imagenes/masDocumentos.png" Width="25px" Visible="false"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                            <ItemTemplate>--%>
                                <%--<asp:ImageButton ID="btnAgregarDoc" runat="server" ImageUrl="~/Imagenes/masDocumentos.png" Width="25px" Visible="false"/>--%>
                        <%--    </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>

            <%--<div id="divBoton"  runat="server" style="width:95%;  margin-left:2%;  text-align:center;">
                
                <asp:ImageButton OnClientClick="setActiveTab('li1'); return false;" ID="imgRegresarDocs" Width="38px"
                ToolTip="Regresar a Detalle" runat="server" ImageUrl="../Imagenes/VerDetalle.png" />

                <asp:ImageButton ID="btnCargaMasiva" runat="server" ImageUrl="/Imagenes/adjuntarDocs.png" ToolTip="Carga masiva de documentos" OnClientClick="MuestraImgCarga(this);" />
            </div>--%>

        </asp:Panel>
        <%--</div>--%>
        
        <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
            <asp:Image ID="Image1" runat="server" 
                    AlternateText="No existen registros para la consulta" ImageAlign="Middle" 
                    imageurl="../Imagenes/no EXISTEN.gif" />
        </div>

        <asp:HiddenField ID="hfValorScrollDocs" runat="server" Value="0" ClientIDMode="Static"/>
        <asp:HiddenField ID="hfIdDocSicod" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfIdVerDocSicod" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfIdRenglon" runat="server" Value="-1" ClientIDMode="Static" />
        <asp:HiddenField ID="hfNumVerDocSicod" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hfVisita" runat="server" Value="0" ClientIDMode="Static" />

        <div id="divAvisoDocs" style="display: none" title="Notificación" runat="server">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgAviso" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <%= MensajeDocs%>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>


        <div id="divConfirmacionDocs" style="display: none" title="Confirmación" runat="server">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgConfirmaRegistroVisitaCC" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <%= MensajeDocs%>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="divObservacionesRechazo" style="display: none"  title="Confirmación" runat="server">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image2" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <table>
                                <tr>
                                    <td>Favor de ingresar el motivo por el cual se ha rechazado el documento.</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtComentRechazo" runat="server" Width="403px" Height="80px" TextMode="MultiLine" 
                                            onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" 
                                           onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblFechaGeneralDocs" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>

       <div id="divObservacionesAprobacion" style="display: none"  title="Confirmación" runat="server">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image3" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            <table>
                                <tr>
                                    <td>Favor de ingresar el motivo de aprobación del documento.</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtComentAprueba" runat="server" Width="403px" Height="80px" TextMode="MultiLine" 
                                            onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" 
                                           onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <br />
                                        <asp:Label ID="lblFechaGeneralDocsA" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
        <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />

        <asp:Button runat="server" ID="btnConfirmarDocsSI" Style="display: none"/>
        <asp:Button runat="server" ID="btnConfirmarDocsNO" Style="display: none"/>
        <asp:Button runat="server" ID="btnComentRechazo" Style="display: none" OnClick="btnComentRechazo_Click"/>
        <asp:Button runat="server" ID="btnComentAprueba" Style="display: none" OnClick="btnComentAprueba_Click"/>
        </ContentTemplate>
    <Triggers>
        <%--<asp:PostBackTrigger ControlID="btnExportaExcelDocs" />--%>
        <%--<asp:PostBackTrigger ControlID="btnCargaMasiva" />--%>
        <asp:PostBackTrigger ControlID="btnComentRechazo" />
        <asp:PostBackTrigger ControlID="btnComentAprueba" />
    </Triggers>

</asp:UpdatePanel>