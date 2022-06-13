<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdjuntarDocumentos.aspx.vb"
    Inherits="SICOD.WebForm1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
    <script language="javascript" src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">

        function Extender_Onchange(sender, args) {

            $('#<%= FileUpAcuse.ClientID %>').attr('disabled', '')
        }

        function getFlickerSolved() {
            jQuery('<%= PanelErrores.ClientID %>').css("display", "none");
        }

        //NHM INI
        function verImgCargando(sender, args) {
            //alert("inica carga");
            var img = document.getElementById('<%= imgCargandoArchivo.ClientID%>');
            img.style.display = "block";
        }

        //NHM FIN

    

    </script>
    <br />
    <center>
        <asp:Label runat="server" ID="lblTitulo" Text="Adjuntar Documentos" CssClass="TitulosWebProyectos"></asp:Label>
    </center>
    <br />
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel runat="server" ID="pnlAdjuntarDocumentos">
            <table align="center" width="90%">
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocWord" runat="server" CssClass="txt_gral" Text="Archivo de trabajo Word (.doc, .docx)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="fileUpWord" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteWord" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" Height="16px" />
                        <asp:LinkButton ID="LinkDocWord" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Documento Word</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocPDF" runat="server" CssClass="txt_gral" Text="Archivo PDF del documento (.pdf)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="fileUpPDF" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeletePDF" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocPDF" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Documento PDF</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocCedulaPDF" runat="server" CssClass="txt_gral" Text="Archivo PDF de la cédula (.pdf)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="fileUpCNE" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteCNE" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocCNE" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Cedula de Notificación Electrónica</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style4">
                        <asp:Label ID="lblAnexo" runat="server" CssClass="txt_gral" Text="Adjuntar anexo (cualquier tipo)"></asp:Label>
                    </td>
                    <td class="style5">
                        <asp:FileUpload ID="FileUpAnexo1" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteAnexo1" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkAnexo1" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Anexo</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocFirmaDigital" runat="server" CssClass="txt_gral" Text="Oficio con firma digital (.sbm, .sbmx)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="FileUpFirmaDigital" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteFirmaDigital" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocFirmaDigital" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Firma Digital</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocCedulaDigital" runat="server" CssClass="txt_gral" Text="Cédula de Notificación Digital (.sbm, .sbmx)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="FileUpCedulaDigital" runat="server" Height="22px" Width="350px" />
                        <asp:ImageButton ID="btnDeleteCedulaDigital" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocCedulaDigital" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Cédula Digital</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style4">
                        <asp:Label ID="Label1" runat="server" CssClass="txt_gral" Text="Archivo Anexo Digital (.sbm, .sbmx)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="FileUpAnexo2" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteAnexo2" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkAnexo2" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Anexo Digital</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocAcuse" runat="server" CssClass="txt_gral" Text="Acuse del documento (.pdf)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="FileUpAcuse" disabled="disabled" runat="server" Width="210px" />
                        <asp:TextBox ID="txtFechaAcuse" Text="Fecha del Acuse" runat="server" CssClass="txt_gral"
                            Width="110" TabIndex="15"></asp:TextBox>
                        <asp:Image ImageAlign="Bottom" ID="imgCalFechaAcuse" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                        <asp:CalendarExtender runat="server" ID="calFechaRecepcion" TargetControlID="txtFechaAcuse"
                            Format="dd/MM/yyyy" OnClientDateSelectionChanged="Extender_Onchange" PopupButtonID="imgCalFechaAcuse">
                        </asp:CalendarExtender>
                        <asp:ImageButton ID="btnDeleteAcuse" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocAcuse" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Documento Acuse Respuesta</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocRespuesta" runat="server" CssClass="txt_gral" Text="Respuesta electrónica (.pdf)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="FileUpRespuesta" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteRespuesta" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocRespuesta" runat="server" Visible="False" CssClass="txt_gral"
                            ForeColor="Blue">Documento Respuesta Oficio</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblDocExpediente" runat="server" CssClass="txt_gral" Text="Expediente (.pdf, .zip)"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:FileUpload ID="FileUpExpediente" runat="server" Width="350px" />
                        <asp:ImageButton ID="btnDeleteExpediente" runat="server" ImageUrl="~/imagenes/Delete.png"
                            Visible="False" />
                        <asp:LinkButton ID="LinkDocExpediente" runat="server" CssClass="txt_gral" ForeColor="Blue"
                            Visible="False">Documento Expediente</asp:LinkButton>
                    </td>
                </tr>               
                <tr>
                    <td colspan="3">
                         <center>
                                <asp:Image ID="imgCargandoArchivo" runat="server" ImageUrl="./iconoCargando.gif" Width="143px"  Height="70px" Style="display: none" />
                                    </center>
                    </td>
                </tr>
                 <tr>
                   <td colspan="3">
                       &nbsp;
                   </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:Button ID="btnAdjuntar" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                            onmouseover="style.backgroundColor='#A9A9A9'" Text="Adjuntar" Width="93px" OnClientClick="verImgCargando()" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancelar" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                            onmouseover="style.backgroundColor='#A9A9A9'" Text="Regresar" Width="93px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:LinkButton ID="LB1" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
        <asp:Label ID="lblPostBack" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Panel ID="PanelErrores" runat="server" CssClass="PanelErrores" Width="550px"
            Style="display: none">
            <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:Panel ID="PanelErroresHandle" runat="server" CssClass="PanelErroresHandle" Width="100%"
                            Height="20px">
                            <asp:Label runat="server" ID="lblErroresTitulo" Text="La información presenta errores"
                                Style="vertical-align: middle; margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblErroresPopup" CssClass="txt_gral txt_gral_o_gris"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 35px">
                        <center>
                            <asp:Button ID="BtnModalOk" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                CssClass="botones" OnClientClick="getFlickerSolved()" />
                            <asp:Button ID="BtnContinua" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Continuar"
                                CssClass="botones" Visible="false" />
                        </center>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
            TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle"
            OnCancelScript="getFlickerSolved()">
        </asp:ModalPopupExtender>
        <asp:Label runat="server" ID="lblErrores" CssClass="txt_gral_rojo" Visible="false"
            Style="display: none"></asp:Label>
    </div>
    </form>
</body>
</html>
