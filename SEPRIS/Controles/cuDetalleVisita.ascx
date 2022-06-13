<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="cuDetalleVisita.ascx.vb" Inherits="SEPRIS.cuDetalleVisita" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="~/Controles/ucRespVisita.ascx" TagPrefix="ucr" TagName="ucrRespVisita" %>
<%@ Register Src="~/Visita/UserControls/AltaIrregularidadesVisita.ascx" TagName="AltaIrregularidad" TagPrefix="uc8" %>

    <style type="text/css">
        .ucaAlignCenter {
            text-align:center;
        }

        .ucaAlignRight {
            text-align:right;
            width:50%;
        }

        .ucaAlignLeft {
            text-align:left;
            width:50%;
        }

        .ucaTabla {
            width:100%;
        }
    </style>

            <div class="divBtnEditar">
                <asp:ImageButton ID="imgEditarVisita" OnClientClick="MuestraImgCarga(this);" ToolTip="Editar información de la Visita" runat="server" ImageUrl="~/Imagenes/icono_lapiz_Sub.GIF" Width="30px"/>
            </div>
            <fieldset class="divMargen" style="text-align:left;">
                <legend style="font-weight:bold">Datos Generales:</legend>
                <table id="tbDatosVisita" runat="server" class="tablaDetalle">
                    <tr runat="server" id="trFolioVisita" visible="false">
                        <td class="coumnaUno">Subfolio:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtFolioVisita" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                            <asp:HiddenField ID="hfIdVisita" runat="server" Value="0" />
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">&nbsp;</td>
                        <td class="coumnaTres">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Entidad:</td>
                        <td class="coumnaTres">
                            <%--<asp:Label ID="lblEntidad" runat="server" Text=""></asp:Label>--%>
                            <asp:TextBox ReadOnly="true" ID="txtEntidad" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td id="tdsubentidadT" runat="server" class="coumnaUno">Subentidad:</td>
                        <td id="tdsubentidadV" runat="server" class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtSubEntidad" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Tipo de Visita:</td>
                        <td class="coumnaTres">
                            <%--<asp:Label ID="lblTipoVisita" runat="server" Text=""></asp:Label>--%>
                            <asp:TextBox ReadOnly="true" ID="txtTipoVisita" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Objeto(s) de visita:</td>
                        <td class="coumnaTres">
                            <asp:ListBox  ID="lbObjetoVisita" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                        </td>
                    </tr>
                    <tr id="trObjetoVisitaOtro" runat="server" visible="false">
                        <td class="coumnaUno">&nbsp;</td>
                        <td class="coumnaTres">&nbsp;</td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Especificación de otro objeto de visita:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtDscObjetoV" TextMode="MultiLine"  runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Fecha de registro:</td>
                        <td class="coumnaTres">
                            <%--<asp:Label ID="lblFechaRegistro" runat="server" Text=""></asp:Label>--%>
                            <asp:TextBox ReadOnly="true" ID="txtFechaRegistro" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Fecha de inicio de visita:</td>
                        <td class="coumnaTres">
                            <%--<asp:Label ID="lblFechaInicioVisita" runat="server" Text=""></asp:Label>--%>
                            <asp:TextBox ReadOnly="true" ID="txtFechaInicioVisita" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trAcuerdoVul" runat="server">
                        <td class="coumnaUno">Revisión de acuerdo de vulnerabilidades:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtAcuerdoVul" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">&nbsp;</td>
                        <td class="coumnaTres">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Descripción de la visita:</td>
                        <td class="renglonDetalle" colspan="4">
                            <%--<asp:Label ID="lblDescrip" runat="server" Text=""></asp:Label>--%>
                            <asp:TextBox ReadOnly="true" ID="txtDescrip" TextMode="MultiLine" runat="server"  Width="98%" Height="50px" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Orden de visita:</td>
                        <td class="renglonDetalle" colspan="4">
                            <%--<asp:Label ID="lblDescrip" runat="server" Text=""></asp:Label>--%>
                            <asp:TextBox ReadOnly="true" ID="txtOrdenVisita" TextMode="MultiLine" runat="server"  Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" class="renglonDetalle">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5" class="renglonDetalle"><b>Personal Responsable de la visita:</b><br /><br /></td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Supervisor(es):</td>
                        <td class="coumnaTres">
                            <%--<asp:Label ID="lblResponsableInspeccion" runat="server" Text=""></asp:Label>--%>
                            <%--<asp:TextBox ReadOnly="true" ID="txtResponsableInspeccion" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstResponsableInspeccion" runat="server" Width="100%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgResponsableInspeccion" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraResponsables_Click" CommandArgument="2" Visible="false" style="vertical-align:top"/>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Inspector(es):</td>
                        <td class="coumnaTres">
                            <%--<div id="divInspectoresAsignados" runat="server"></div>--%>
                            <%--<asp:TextBox ID="txtInspectoresAsignados" runat="server"></asp:TextBox>--%>
                            <asp:ListBox ID="lstInspectoresAsignados" runat="server" Width="100%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgInspectoresAsignados" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraResponsables_Click" CommandArgument="3" Visible="false" style="vertical-align:top"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" class="renglonDetalle">&nbsp;</td>
                    </tr>
                    <tr runat="server" id="renTituloJuridico">
                        <td colspan="5" class="renglonDetalle"><b>Jurídico:</b><br /><br /></td>
                    </tr>
                    <tr runat="server" id="renAsesor">
                        <td class="coumnaUno">Supervisor Asesor:</td>
                        <td class="renglonDetalle">
                            <%--<asp:TextBox ReadOnly="true" ID="txtSupJuridico" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstSupJuridicoA" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgSupJuridicoA" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraAbogado_Click" CommandArgument="25" Visible="false" style="vertical-align:top"/>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Asesor:</td>
                        <td class="coumnaTres">
                            <%--<asp:TextBox ReadOnly="true" ID="txtAbogadoAsesor" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstAbogadoAsesor" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgMuestraAsesor" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraAbogado_Click" CommandArgument="5" Visible="false" style="vertical-align:top"/>
                        </td>
                    </tr>

                    <tr runat="server" id="renSanciones">
                        <td class="coumnaUno">Supervisor Sanciones:</td>
                        <td class="renglonDetalle">
                            <%--<asp:TextBox ReadOnly="true" ID="txtSupJuridico" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstSupJuridicoS" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgSupJuridicoS" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraAbogado_Click" CommandArgument="26" Visible="false" style="vertical-align:top"/>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Sanciones:</td>
                        <td class="coumnaTres">
                            <%--<asp:Label ID="lblAbogadoAsignado" runat="server" Text=""></asp:Label>--%>
                            <%--<asp:TextBox ReadOnly="true" ID="txtAbogadoAsignado" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstAbogadoSancion" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgMuestraSancion" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraAbogado_Click" CommandArgument="6" Visible="false" style="vertical-align:top"/>
                        </td>
                    </tr>
                    <tr runat="server" id="renContencioso">
                        <td class="coumnaUno">Supervisor Contencioso:</td>
                        <td class="renglonDetalle">
                            <%--<asp:TextBox ReadOnly="true" ID="txtSupJuridico" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstSupJuridicoC" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgSupJuridicoC" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraAbogado_Click" CommandArgument="27" Visible="false" style="vertical-align:top"/>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Contencioso:</td>
                        <td class="coumnaTres">
                            <%--<asp:TextBox ReadOnly="true" ID="TextBox2" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>--%>
                            <asp:ListBox ID="lstContencioso" runat="server" Width="98%" onkeydown="return false;" CssClass="txt_gral"></asp:ListBox>
                            <asp:ImageButton ID="imgMuestraContencioso" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="imgMuestraAbogado_Click" CommandArgument="7" Visible="false" style="vertical-align:top"/>
                        </td>
                    </tr>
                </table>
            </fieldset>

            <%--No pude hacer que se vajara el fielset 25px asi que meti un div con 25 px--%>
            <div id="divFechasAllazgos" class="marginTopCincuenta" runat="server"></div>
            <fieldset class="divMargen" id="fsFechasAllazgos" runat="server" style="text-align:left;">
                <legend style="font-weight:bold">Fechas de presentación de hallazgos</legend>
                <table class="tablaDetalle">
                    <tr>
                        <td class="coumnaUno">Presentación interna:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtFechaPresentacionInt" runat="server" onkeydown="return false;"></asp:TextBox>
                            <asp:ImageButton ID="btnEditFechaPI" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" Visible="false" OnClick="btnEditFechaPI_Click"/>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno" id="tdFechaExtT" runat="server" >Presentación externa:</td>
                        <td class="coumnaTres" id="tdFechaExtC" runat="server" >
                            <asp:TextBox ReadOnly="true" ID="txtFechaPresentacionExt" runat="server" onkeydown="return false;"></asp:TextBox>
                            <asp:ImageButton ID="btnEditFechaPE" runat="server" ImageUrl="~/Imagenes/icono_lapiz.GIF" Width="20px" OnClick="btnEditFechaPE_Click" Visible="false"/>
                        </td>
                    </tr>
                </table>
            </fieldset>

            <!-- Fieldset para Visita en caso de que este Cancelada -->
            <div id="divVisitaCancel" class="marginTopCincuenta" runat="server"></div>
            <fieldset class="divMargen" id="FVCancelada" visible="false" runat="server" style="text-align: left;">
                <legend style="font-weight: bold">Visita Cancelada</legend>
                <table class="tablaDetalle">
                    <tr>
                        <td class="coumnaUno">Motivo:</td>
                        <td class="renglonDetalle" colspan="4">
                            <asp:TextBox ReadOnly="true" TextMode="MultiLine" Width="98%"  ID="txtMotivoCancela" runat="server" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Usuario:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtUsuarioCancela"  Width="98%"  runat="server" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno">Fecha:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtFechaCancela"  Width="98%"  runat="server" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <!-- FIN Fieldset para Visita en caso de que este Cancelada -->
            <div id="divDatosSancion" class="marginTopCincuenta" runat="server"></div>
            <fieldset class="divMargen" id="fsDatosSancion" runat="server" style="text-align: left;">
                <legend style="font-weight: bold">Datos de la sanción</legend>
                <table class="tablaDetalle">
                    <tr>
                        <td class="coumnaUno">Fecha de imposición:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtFechaImposicion" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                        <td class="coumnaUno" rowspan="2">Comentarios de la sanción:</td>
                        <td class="coumnaTres" rowspan="2">
                            <asp:TextBox ReadOnly="true" ID="txtComenSancion" TextMode="MultiLine" runat="server" Width="98%" onkeydown="return false;" Height="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Monto de sanción:</td>
                        <td class="coumnaTres">
                            <asp:TextBox ReadOnly="true" ID="txtMntoSancion" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                        <td class="coumnaDos">&nbsp;</td>
                    </tr>
                </table>
            </fieldset>

            <div id="divUltimosDocs" class="marginTopCincuenta" runat="server"></div>
            <fieldset class="divMargen" id="fsUltimosDocs" runat="server" style="text-align:left;">
                <legend style="font-weight:bold">Últimos comentarios y documentos adjuntos:</legend>
                <table class="tablaDetalle">
                    <tr>
                        <td>Comentarios:</td>
                        <td colspan="4">
                            <asp:TextBox ReadOnly="true" ID="txtUltimosComentarios" runat="server"  TextMode ="MultiLine" Height="30px" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Usuario que los ingresó:</td>
                        <td colspan="4">
                            <asp:TextBox ReadOnly="true" ID="txtUsuarioComentarios" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox> 
                        </td>
                    </tr>
                    <tr>
                        <td>Documentos:</td>
                        <td colspan="4">
                            <asp:BulletedList ID="blUltimosDocs" runat="server" CssClass="txt_gral">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td class="coumnaUno">Usuario que adjuntó:</td>
                        <td colspan="4">
                            <asp:TextBox ReadOnly="true" ID="txtUsuarioAdjunto" runat="server" Width="98%" onkeydown="return false;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>

            <div class="marginTopCincuenta" runat="server"></div>
            <fieldset class="divMargen" id="fsIrregularidades" runat="server" style="text-align:left;" visible="false">
                <legend style="font-weight:bold">Irregularidades identificadas:</legend>
                <uc8:AltaIrregularidad ID="AltaIrregularidad" runat="server" />
            </fieldset>
            
            <div id="divAvisoDty" style="display: none" title="Notificación" runat="server">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgAviso" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= MensajeDty%>
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divAbogados" style="display: none" title="Modificación Responsables Jurídico" runat="server">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgSelecAbogado" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <table class="ucaTabla">
                                    <thead>
                                        <tr>
                                            <td class="ucaAlignCenter">
                                                <asp:Label ID="lblTituloAbogado" Font-Size="15px" runat="server" Text="Seleccionar nuevos responsables"></asp:Label>
                                            </td>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <tr>
                                            <td class="ucaAlignCenter">
                                                <%--<asp:DropDownList ID="ddlListAbogados" runat="server" Width="70%"></asp:DropDownList>--%>
                                                <asp:UpdatePanel runat="server" ID="udpAsignacionUsuarios" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="tbAbogados" style="width: 100%; margin-top: 20px; border-collapse: collapse;">
                                                        <tr>
                                                            <td colspan="3">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: bottom; text-align: center; width: 45%">Usuarios Disponibles:</td>
                                                            <td style="vertical-align: bottom; text-align: center; width: 10%"></td>
                                                            <td style="vertical-align: bottom; text-align: center; width: 45%">Usuarios Asignados:</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="center"><asp:Label ID="lblTituloInterno" Font-Size="15px" runat="server" Text="Supervisor(es):"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td rowspan="2" style="vertical-align: middle;">
                                                                <asp:ListBox ID="lstUsuariosDisponibles" runat="server" Width="100%" Height="110px"></asp:ListBox>
                                                            </td>
                                                            <td style="vertical-align: bottom; text-align: center;">
                                                                <asp:ImageButton ID="imgAsignar" runat="server" ImageUrl="../Imagenes/FlechaRojaDer.gif" />
                                                            </td>
                                                            <td rowspan="2" style="vertical-align: bottom;">
                                                                <asp:ListBox ID="lstUsuariosAsignados" runat="server" Width="100%" Height="110px"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; text-align: center;">
                                                                <asp:ImageButton ID="imgDesasignar" runat="server" ImageUrl="../Imagenes/FlechaRojaIzq.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="text-align: center;">
                                                                <asp:Label ID="lblErrorAbogado" runat="server" Text="" Font-Size="Small" ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            
            <div id="divRespInsp" style="display: none" title="Modificar responsables de la visita de inspección" runat="server">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgRespVista" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <ucr:ucrRespVisita runat="server" id="ucRespVisita" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <asp:Button runat="server" ID="btnEditarFecPI" Style="display: none" ClientIDMode="Static" OnClick="btnEditarFecPI_Click"/>
            <asp:Button runat="server" ID="btnEditarFecPE" Style="display: none" ClientIDMode="Static" OnClick="btnEditarFecPE_Click"/>
            <asp:Button runat="server" ID="btnGuardarAbogado" Style="display: none" OnClick="btnGuardarAbogado_Click"/>
            <asp:Button runat="server" ID="btnGuardarRespVisita" Style="display: none" OnClick="btnGuardarRespVisita_Click"/>