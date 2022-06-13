<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Bandeja.aspx.vb" Inherits="SEPRIS.Bandeja1" %>

<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>

<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--[if lt IE 8]>
        <style type="text/css" >
            .estilosOtros {
                position: absolute;
                top: 8px;
                left: 250px;

                position: absolute !important;
                top: 8px !important;
                left: 100px !important;
            }
        </style>
    <![endif]-->

    <style type="text/css">
        .estilosOtros {
            position: absolute;
            top: 8px;
            left: 250px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />

    <asp:ImageButton ID="imgModalPendientes" CssClass="estilosOtros" ToolTip="Ver alertas y pendientes" Visible="false" runat="server" ImageUrl="../Imagenes/campanaPendientes.png" />
    <%--<asp:ImageButton ID="imgModalPendientes" ToolTip="Ver alertas y pendientes" runat="server" ImageUrl="../Imagenes/campanaPendientes.png" OnClientClick = "return MuestraModalPendientes();" />--%>

    <cc1:ModalPopupExtender ID="mpeProcesa" runat="server" PopupControlID="UpdatePanel1Pendientes" OnCancelScript="imgCierraModalPendientes" CancelControlID="imgCierraModalPendientes"
        BackgroundCssClass="modalBackground" TargetControlID="imgModalPendientes">
    </cc1:ModalPopupExtender>
    <asp:UpdatePanel ID="UpdatePanel1Pendientes" runat="server" UpdateMode="Always" Visible="false">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="" Height="50%" Width="75%" ID="pnlProcesando">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="margin-top: 50px;">
                    <tr>
                        <td style="background-color: #34b68e !important; text-align: center !important;">
                            <label id="Label2lblTituloPendientes" class="TitulosWebProyectos" runat="server" style="color: #FFFFFF !important;">Alertas y Pendientes</label>
                        </td>
                        <td style="text-align: right; background-color: #34b68e !important;">
                            <%--<asp:Button ID="btnClose" runat="server" Text="X" ToolTip="Cerrar ventana de pendientes" Style="width:20px; font-size:14px; font-weight: bold; margin-right:5px; " />--%>
                            <asp:ImageButton ID="imgCierraModalPendientes" runat="server" ToolTip="Cerrar ventana de pendientes" Text="X" Style="width: 30px; font-size: 14px; font-weight: bold; margin-right: 5px;" OnClick="imgCierraModalPendientes_Click" ImageUrl="../Imagenes/cerrar.png" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #FFFFFF !important;" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="background-color: #FFFFFF !important; text-align: left !important; margin-left: 50px;" colspan="2">
                            <asp:Button ID="btnExportaExcelPendientes" runat="server" Style="margin-left: 5px; font: 11px Arial !important;" Text="Exportar a Excel" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #FFFFFF !important;" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="text-align: right; background-color: #34b68e !important;" colspan="2">
                            <hr />
                        </td>
                    </tr>
                </table>

                <div id="Div1" style="height: 300px; background-color: #FFFFFF !important;">
                    <div id="dvModalPendientes" style="display: block; height: 300px; overflow-x: hidden; overflow-y: scroll; background-color: #FFFFFF !important;" title="Alertas y Pendientes">
                        <table class="FiltroDinamico" style="width: 100%; height: 25px;" cellpadding="0" border="0">
                            <tr>
                                <td colspan="3" style="background-color: #34b68e !important;">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: left; font-size: 14px !important; font-weight: bold;">
                                    <asp:Label ID="lblCriteriosDDL" Text="Filtrar por: " runat="server" Style="padding-left: 10px; color: #FFFFFF;"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trFolio" style="display: block;">
                                <td colspan="3" style="width: 30% !important;">
                                    <asp:Label ID="lblFolio" Text="Folio de Visita:" runat="server" Style="padding-left: 10px; color: #FFFFFF;"></asp:Label>
                                </td>
                                <td colspan="2" style="text-align: left; width: 70% !important;">
                                    <asp:DropDownList ID="ddlFolioVisita" runat="server" CssClass="txt_gral" Width="200px" Style="margin-top: 10px; font-size: 11px !important; font-family: Arial !important;"
                                        ValidationGroup="Forma" AutoPostBack="false">
                                    </asp:DropDownList>
                                    <%--<img src="../Imagenes/quitaCriterio.jpg" alt="Eliminar criterio" onclick="ocultarControlFiltroPendientes(1);" title="Eliminar criterio" id="imgOcultarCriterioFolio" />--%>
                                </td>
                            </tr>
                            <tr id="trPaso" style="display: block;">
                                <td colspan="3" style="width: 9.4% !important;">
                                    <asp:Label ID="lblPasoA" Text="Del paso: " runat="server" Style="padding-left: 10px; color: #FFFFFF;"></asp:Label>
                                </td>
                                <td colspan="4" style="text-align: left; width: 100% !important;">
                                    <asp:DropDownList ID="ddlPasoActual" runat="server" CssClass="txt_gral" Width="200px" Style="font-size: 11px !important; font-family: Arial !important;"
                                        ValidationGroup="Forma" AutoPostBack="false">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3" style="width: 9.4% !important;">
                                    <asp:Label ID="lblPasoB" Text="al paso: " runat="server" Style="padding-left: 10px; color: #FFFFFF;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPasoActualFin" runat="server" CssClass="txt_gral" Width="200px" Style="font-size: 11px !important; font-family: Arial !important;"
                                        ValidationGroup="Forma" AutoPostBack="false">
                                    </asp:DropDownList>

                                    <%--<img src="../Imagenes/quitaCriterio.jpg" alt="Eliminar criterio" onclick="ocultarControlFiltroPendientes(2);" title="Eliminar criterio" id="imgOcultarCriterioPaso" />--%>
                                </td>
                                <td colspan="1" style="text-align: right; width: 25% !important; margin-top: 10px; margin-bottom: 10px;">
                                    <asp:Button ID="btnFiltrarPendientes" runat="server" Style="font: 11px Arial !important;" Text="Filtrar" />
                                </td>
                            </tr>
                            <tr style="background-color: #FFFFFF !important;">
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                        </table>

                        <cc1:CustomGridView ID="gvConsultaPendientes" runat="server" DataKeyNames="T_ID_FOLIO, I_ID_PASO_ACTUAL, I_ID_VISITA" Width="100%"
                            AllowSorting="true" AutoGenerateColumns="false" HabilitaScroll="true" HeigthScroll="400" UnicoEnPantalla="false" HeaderStyle-Font-Size="14"
                            HabilitaSeleccion="SimpleCliente" HiddenFieldSeleccionSimple="hfSelectedValue">
                            <Columns>
                                <asp:BoundField HeaderText="" HeaderStyle-Width="5px" ItemStyle-Width="5px" />
                                <asp:BoundField HeaderText="Folio de Visita" DataField="T_ID_FOLIO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                                <asp:BoundField HeaderText="# Paso actual" DataField="I_ID_PASO_ACTUAL" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="Descripción de Alerta" DataField="MSG_ALERTASCC" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="Sub folios para los que aplica" DataField="subVisitas" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="" HeaderStyle-Width="5px" ItemStyle-Width="5px" />
                            </Columns>
                        </cc1:CustomGridView>

                        <div id="pnlNoExistePendientes" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                            <asp:Image ID="Image2" runat="server"
                                AlternateText="No existen registros para la consulta" ImageAlign="Middle"
                                ImageUrl="../Imagenes/no EXISTEN.gif" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upnlConsulta" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">

                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label id="lbTituloBandeja" class="TitulosWebProyectos" runat="server">Bandeja de Visitas</label>
                </div>

                <div style="float: left; text-align: right; margin: -50px 150px 0px 0px;">
                    <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" OnClientClick="return MuestraProcesoInspeccion();" />
                    <asp:ImageButton ID="imgProcesoVisitaAmbos" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
                </div>

                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                    <asp:Button ID="btnPersonalizaColumnas" runat="server" Text="Personalizar Columnas" Width="150px" />

                </div>

                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />

                <div id="pnlGrid" runat="server">

                    <div style="text-align: left; width: 800px; padding: 5px; display: none;">
                        <asp:CheckBox ID="chkSeleccionaTodos" runat="server" AutoPostBack="true" />
                        <asp:Label ID="lblSeleccionaTodos" runat="server" Text="Seleccionar Todo" CssClass="txt_gral"></asp:Label>
                    </div>
                    <div style="height: 15px;">
                        &nbsp;
                    </div>

                    <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="IdVisitaGenerado, IdEstatusSemaforo, TieneSubvisitas, IdPasoActual, EstatusVisitaInt, DiasAcumCSPro, FechaRegistroFormateada" Width="100%"
                        AllowSorting="true" AutoGenerateColumns="false" HabilitaScroll="true" HeigthScroll="400" UnicoEnPantalla="false" HeaderStyle-Font-Size="14"
                        ColumnasCongeladas="2" HabilitaSeleccion="SimpleCliente" HiddenFieldSeleccionSimple="hfSelectedValue">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="25px" ItemStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkElemento" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Folio" DataField="FolioVisita" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                            <asp:BoundField HeaderText="Fecha inicio de visita" DataField="StrFechaInicioVisita" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Entidad" DataField="DscEntidad" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Sub Entidad" DataField="DSCSUBENTIDAD" ItemStyle-CssClass="Wrap" Visible ="false" />
                            <asp:BoundField HeaderText="Vicepresidencia" DataField="DscArea" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Tipo de visita" DataField="DscTipoVisita" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="# Días hábiles legales de la visita" DataField="DiasPlazosLegalesVisitaDsc" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="DS" DataField="DiasAcumCSPro" Visible="false" />
                            <asp:TemplateField HeaderText="Superávit / Déficit">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeficitSuperavit" Width="25px" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="# Paso actual" DataField="IdPasoActual" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="# Días transcurridos en el paso actual" DataField="DiasTranscurridos" ItemStyle-CssClass="Wrap" />
                            <asp:TemplateField HeaderText="Estatus Paso Actual"><%--15--%>
                                <ItemTemplate>
                                    <asp:Image ID="imgSemaforo" Width="25px" runat="server" ImageUrl='~/Imagenes/semaforo1.png' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Responsable de inspección" DataField="NombreInspectorResponsable" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Abogado asesor" DataField="NombreAbogadoAsesor" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Abogado sanciones" DataField="NombreAbogadoSancion" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Abogado contencioso" DataField="NombreAbogadoContencioso" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Fecha de Registro" DataField="FechaRegistroFormateada" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Orden de visita" DataField="OrdenVisita" ItemStyle-CssClass="Wrap" />

                            <asp:BoundField HeaderText="Estatus Visita" DataField="EstatusVisitaDsc" ItemStyle-CssClass="Wrap" />

                            <asp:TemplateField HeaderText="Subfolios">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgVerSubvisita" runat="server" ImageUrl="~/Imagenes/Subvisitas.png" Visible="false"
                                        Width="25px" OnClick="imgVerSubvisita_Click" CommandArgument='<%# Bind("IdVisitaGenerado")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF"
                                        Width="20px" OnClick="btnEditar_Click" CommandArgument='<%# Bind("IdVisitaGenerado")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCancelar" runat="server" OnClick="btnCancelar_Click"
                                        ImageUrl="~/Imagenes/icono_corregir.jpg" Width="20px" CausesValidation="false"
                                        CommandArgument='<%# Bind("IdVisitaGenerado")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField HeaderText="Folio Sisan" Visible="true" DataField="Folio_SISAN" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Fecha de Envío a Sanciones" Visible="true" DataField="F_FECH_ENVIA_SANSIONES" ItemStyle-CssClass="Wrap" />
                           <%-- <asp:TemplateField HeaderText="Fecha de Envío a Sanciones" >

                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>

                                            <td style="border-right: 0; border-bottom: 0">
                                                <asp:Label ID="F_FECH_ENVIA_SANSIONES" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </cc1:CustomGridView>
                </div>

                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server"
                        AlternateText="No existen registros para la consulta" ImageAlign="Middle"
                        ImageUrl="../Imagenes/no EXISTEN.gif" />
                </div>

                <asp:HiddenField ID="hfGridView1SV_gvConsulta" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH_gvConsulta" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfSelectedValue_gvConsulta" runat="server" ClientIDMode="Static" Value="-1" />

                <br />
                <br />
                <div style="width: 100%; padding-bottom: 5px;">
                    <table style="width: 100%;">
                        <tr>
                            <%--<td style="width:30%; vertical-align:top;">
                                <fieldset style="display:none">
                                    <legend class="txt_gral">&nbsp;Estatus&nbsp;</legend>
                                    <asp:Image ID="imgOK" runat="server" ImageUrl='~/Imagenes/concluido.png' Width="18px"/>
                                    <label class="txt_gral"> = Vigente</label>
                                    <asp:Image ID="imgERROR" runat="server" ImageUrl='~/Imagenes/vencido.png' Width="18px" />
                                    <label class="txt_gral"> = Cancelada</label>
                                    <asp:Image ID="Image7" runat="server" ImageUrl='~/Imagenes/prorroga.png' Width="18px" />
                                    <label class="txt_gral"> = Con prórroga</label>
                                </fieldset>
                            </td>--%><td style="width: 80%; vertical-align: top;">
                                <fieldset style="height: 45px;">
                                    <legend class="txt_gral">&nbsp;Estatus paso actual&nbsp;</legend>
                                    <asp:Image ID="Image3" runat="server" ImageUrl='~/Imagenes/semaforo1.png' Width="25px" />
                                    <label class="txt_gral">= Paso actual en tiempo (más de un día para vencer)</label>
                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/semaforo2.png" Width="25px" />
                                    <label class="txt_gral">= Paso actual por vencer (queda solo un día)</label>
                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Imagenes/semaforo3.png" Width="25px" />
                                    <label class="txt_gral">= Paso actual vencido</label>
                                </fieldset>
                            </td>
                            <td style="width: 20%; vertical-align: top;">
                                <fieldset style="height: 45px;">
                                    <legend class="txt_gral">&nbsp;Subfolios&nbsp;</legend>
                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Imagenes/Subvisitas.png" Width="18px" />
                                    <label class="txt_gral">= Ver subfolios de visita</label>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="width: 100%; height: 5px;"></div>
                <br />

                <%--<asp:Button ID="btnVerReporte" runat="server" Text="Ver reporte" />--%>
                <asp:ImageButton ID="btnVerReporte" ToolTip="Ver Reporte de visita" runat="server" ImageUrl="../Imagenes/VerReporte.png" />
                &nbsp;&nbsp;&nbsp;
                <%--<asp:Button ID="btnRegresarBandeja" ToolTip="Regresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" Visible="false" />--%>
                <asp:ImageButton ID="btnRegresarBandeja" ToolTip="Regresar" runat="server" ImageUrl="~/Imagenes/Inicio.png" OnClick="btnRegresar_Click" Visible="false" />
                <br />
            </asp:Panel>

            <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblDetalle" runat="server" class="TitulosWebProyectos">Inserta Registro</asp:Label>
                </div>
                <br />
                <asp:Label ID="Label1" runat="server" CssClass="txt_gral">En esta pantalla se insertara, modificara o vera el detalle de un registro</asp:Label>
                <br />
                <br />
                <asp:Button ID="btnRegresa" runat="server" Text="Regresa" />
            </asp:Panel>

            <div id="divMensajeUnBotonNoAccion" style="display: none" title="Información">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje %>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

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

            <div id="divConfirmacionM2B2A" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgM2B2A" runat="server" Width="32px" Height="32px" />
                        </td>
                        <td style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divMensajeUnBotonUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" />
                        </td>
                        <td style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>


            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" />
                        </td>
                        <td style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divCancelarVisita" style="display: none;" title="Motivo de cancelación">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="Image16" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                        </td>
                        <td style="text-align: center; vertical-align: bottom;">
                            <div class="MensajeModal-UI" id="divTituloFechaGeneral" style="text-align: center;">
                                Favor de ingresar el motivo de la cancelación:
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <br />
                            <asp:TextBox ID="txtMotivoCancelacion" runat="server" ClientIDMode="Static"
                                TextMode="MultiLine" Width="95%" Height="70px" MaxLength="600" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Label ID="lblFechaGeneral" runat="server" Text="*Texto dinamico." Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
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

            <asp:Button runat="server" ID="btnSalirM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnContinuarM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" />
            <asp:Button runat="server" ID="btnConsultaPendientes" Style="display: none" />
            <asp:Button runat="server" ID="btnCancelarVisita" Style="display: none" ClientIDMode="Static" OnClick="btnCancelarVisita_Click" />
            <asp:Button runat="server" ID="btnSiRegistra" Style="display: none" ClientIDMode="Static" OnClick="btnSiRegistra_Click" />
            <asp:Button runat="server" ID="btnNoRegistra" Style="display: none" ClientIDMode="Static" OnClick="btnNoRegistra_Click" />
            <script type="text/javascript">

                $(function () {
                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();
                    MensajeDosBotonesUnaAccionLoad();
                    ConfirmacionSiNoLoad("divMensajeDosBotonesUnaAccion", "btnSiRegistra", "btnNoRegistra", 0, 550, 270);
                    MensajeDosBotonesUnaAccionAgcLoad("divCancelarVisita", "btnCancelarVisita", 550, 370);
                });

                function AquiMuestroMensaje() {
                    MensajeUnBotonNoAccion();
                };

                function MuestraModalPendientes() {
                    MensajeModalPendientes();
                };

                function AquiMuestroOpcionesImgProcesos() {
                    DivOpcionesImagenesProcesos();
                };

                function ConfirmacionEliminar() {
                    MensajeDosBotonesUnaAccion();
                };


                function MensajeFinalizar() {
                    MensajeUnBotonUnaAccion();
                }

                function MensajeConfirmacion() {
                    MensajeDosBotonesUnaAccion();
                }

                function cierraModalPendientes() {
                    var modal = $find('‘ctl00_ContentPlaceHolder1_UpdatePanel1Pendientes');
                    modal.hide();
                }

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

                //Cancela la visita agc
                function CancelarVisita() {
                    //$("#divTituloFechaGeneral").text("Favor de ingresar la fecha fin de la visita:");
                    $("#txtMotivoCancelacion").val("");
                    MensajeDosBotonesUnaAccionAgc("divCancelarVisita", "btnCancelarVisita", 550, 370);
                    return false;
                }

                function MostrarMensajeRegistro() {
                    ConfirmacionSiNo("divMensajeDosBotonesUnaAccion", "btnSiRegistra", "btnNoRegistra", 0, 550, 270);
                }

                //function validaFiltrosSeleccionados() {

                //    var visita = 0;
                //    var visitatxt = "";
                //    var paso = 0;

                //    if ((document.getElementById('trFolio').style.display == "none") && (document.getElementById("trPaso").style.display == "none")) {
                //        alert("Por favor selecciona un criterio de consulta.");
                //        return false;
                //    } else {
                //        if ((document.getElementById('trFolio').style.display == "block")) {
                //            visita = $('#<=ddlFolioVisita.ClientID%>').val();

                //            if (visita < 1) {
                //                alert("Por favor selecciona una visita a consultar.");
                //                return false;
                //            }
                //        }

                //        if ((document.getElementById('trPaso').style.display == "block")) {
                //            paso = $('#<=ddlPasoActual.ClientID%>').val();

                //            if (paso < 1) {
                //                alert("Por favor selecciona una visita a consultar.");
                //                return false;
                //            }
                //        }

                //    return obtieneDatosFiltroPendientes(paso, visita);
                //    obtieneDatosFiltroPendientes();
                //    return false;
                //    }
                //}

                //function muestraCtrlFiltro(control) {
                //      if (control == 1) {
                //          document.getElementById('trFolio').style.display = "block";
                //      } else if (control == 2) {
                //          document.getElementById("trPaso").style.display = "block";
                //      }
                //  }

                //  function ocultarControlFiltroPendientes(control) {
                //      if (control == 1) {
                //          document.getElementById("trFolio").style.display = "none";
                //      } else if (control == 2) {
                //          document.getElementById("trPaso").style.display = "none";
                //      }
                //  }

                //function obtienedatosfiltropendientes(paso, visita) {
                function obtienedatosfiltropendientes() {
                    //var actiondata = "{'idpaso': '" + paso + "', 'idvisita': '" + visita + "'}"; 
                    //data: actiondata,

                    $.ajax({
                        type: "POST",
                        url: "Bandeja.aspx/RellenarGridPendientes",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (msg) { alert(msg.d); },
                        error: function (result) {
                            alert("ERROR " + result.status + ' ' + result.statustext);
                            return false;
                        }
                    });
                }

                //function ejecutaClick(){
                //    var objO = $("#<=imgModalPendientes.ClientID%>");
                //    objO.click();
                //}

            </script>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnConsulta" />
            <asp:PostBackTrigger ControlID="btnConsultaPendientes" />
            <asp:PostBackTrigger ControlID="btnPersonalizaColumnas" />
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnExportaExcelPendientes" />
            <asp:PostBackTrigger ControlID="btnRegresa" />
            <asp:PostBackTrigger ControlID="btnCancelarVisita" />
            <asp:PostBackTrigger ControlID="btnSiRegistra" />
            <asp:PostBackTrigger ControlID="btnNoRegistra" />

        </Triggers>
    </asp:UpdatePanel>

    <wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server" />

</asp:Content>
