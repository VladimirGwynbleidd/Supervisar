<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AltaIrregularidadesOPI.ascx.vb" Inherits="SEPRIS.AltaIrregularidadesOPI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
     <link href="../Styles/Site.css" rel="stylesheet" />

<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>
        <div id="divGuardarCambios" style="display: none" title='Eliminar Irregularidad'>
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="Image1" runat="server" Width="32px" Height="32px"
                            ImageUrl="~/Imagenes/Errores/Error1.png" />
                    </td>
                    <td style="text-align: left">
                        <div class="MensajeModal-UI">
                            ¿Esta seguro que desea eliminar la irregularidad?                      
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="divAltaIrregularidades" runat="server">
            <br />
            <br />
            <div class="txt_gral" style="font-weight:normal">
                <b>Identificación de irregularidades:</b><br /><br />
            </div>  
            <div>
                <table style="width: 100%;">
                    <tr>   
                        <td>
                            <asp:Label ID="LblNumIrregularidad" runat="server" Text="Fecha de irregularidad" CssClass="txt_gral"></asp:Label>
                            <span style="color:red">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFecIrregularidad" runat="server" Width="150px" CssClass="txt_gral"></asp:TextBox>

                            <ajx:CalendarExtender ID="calExFechaIrregularidad" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCal"
                                TargetControlID="txtFecIrregularidad" CssClass="teamCalendar" />
                            <asp:Image ImageAlign="Bottom" ID="imgCal" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                     
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblProceso" runat="server" Text="Proceso" CssClass="txt_gral"></asp:Label><span style="color:red">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProceso" runat="server" CssClass="txt_gral" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlProcesoAlta_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblSubProceso" runat="server" Text="Subproceso" CssClass="txt_gral"></asp:Label><span style="color:red">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubProceso" runat="server" CssClass="txt_gral" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlSubProcesoAlta_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblConducta" runat="server" Text="Conducta sancionable" CssClass="txt_gral"></asp:Label>
                            <span style="color:red">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlConducta" runat="server" CssClass="txt_gral" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlConducta_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Irregularidad" CssClass="txt_gral"></asp:Label>
                            <span style="color:red">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlIrregularidad" runat="server" CssClass="txt_gral" Width="400px"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblComentarios" runat="server" Text="Comentarios adicionales<br>de la irregularidad" CssClass="txt_gral"></asp:Label>
                            <span style="color:red">&nbsp;*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtComentarios" runat="server" CssClass="txt_gral" Height="45px" Width="394px"
                                TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Label id="LblError"  runat="server" Text="Debe capturar los datos obligatorios<br/><br/>" CssClass ="txt_gral" ForeColor="red" ></asp:Label>
                <asp:Label id="LblIrregularidades"  runat="server" Text="Se agregaron irregularidades<br/><br/>" CssClass ="txt_gral" ForeColor="Blue" ></asp:Label>
            </div>
            <asp:Panel ID="pnlBotones" runat="server">
                <div id="DivAltaIrreg" runat="server">
                    <asp:Button ID="btnAgregarIrregularidad" runat="server" Text="Agregar Irregularidad" Width="138px" />
                </div>
                <div id="DivModificaIrreg" runat="server">
                    <asp:Button ID="btnModificarIrregularidad" runat="server" Text="Modificar Irregularidad" Width="138px" />
                    <asp:Button ID="btnCancelIrreg" runat="server" Text="Cancelar" Width="138px" />
                </div>
            </asp:Panel>
        </div>
        <br />
        <br />
        <div id="DivBandejaIrregularidad" runat="server">
            <div class="txt_gral" style="font-weight:normal">
                <b>Irregularidades identificadas:</b><br />
            </div>  
            <div id="divReqInfo" runat="server">
                <br />

                <cc1:CustomGridView ID="gvReqInformac" runat="server" DataKeyNames="N_ID_FOLIO" HabilitaSeleccion="SimpleCliente"
                    Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValueRequerimiento"
                    HeigthScroll="0" UnicoEnPantalla="True" WidthScroll="0" SelectedMultiRows="false"
                    AllowPaging="true" ShowHeaderWhenEmpty="true">

                    <Columns>
                        <asp:BoundField HeaderText="Consecutivo" DataField="Row#" Visible="true" SortExpression="Row#">
                            <ItemStyle Width="5%" />
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="" DataField="I_ID_IRREGULARIDAD" SortExpression="I_ID_IRREGULARIDAD" Visible="false">
                            <ItemStyle Width="140px" />
                            <HeaderStyle Width="140px" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Fecha de irregularidad" DataField="F_FECH_IRREGULARIDAD" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_IRREGULARIDAD">
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Proceso" DataField="DESC_PROCESO" SortExpression="DESC_PROCESO">
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="SubProceso" DataField="DESC_SUBPROCESO" SortExpression="DESC_SUBPROCESO">
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Conducta" DataField="DESC_CONDUCTA" SortExpression="DESC_CONDUCTA">
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Irregularidad" DataField="DESC_IRREGULARIDAD" SortExpression="DESC_IRREGULARIDAD">
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Comentarios" DataField="T_DSC_COMENTARIO" SortExpression="T_DSC_COMENTARIO">
                            <ItemStyle Width="160px" />
                            <HeaderStyle Width="160px" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnModificarIrreg" runat="server" ImageUrl="~/Imagenes/icono_lapiz.gif" Width="20px" CausesValidation="false" OnClientClick ='<%# "ModificaIrreg(" + DataBinder.Eval(Container, "RowIndex", "{0}") + "," + Eval("I_ID_IRREGULARIDAD").ToString() + ");"%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarIrreg" runat="server" ImageUrl="~/Imagenes/icono_corregir.jpg" Width="20px" CausesValidation ="false" OnClientClick ='<%# "QuestionEliminar(" + DataBinder.Eval(Container, "RowIndex", "{0}") + "," + Eval("I_ID_IRREGULARIDAD").ToString() + ");"%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="text-align:center;">
                            <img id="Noexisten" runat="server" src="~/Imagenes/No%20Existen.gif" alt="No existen Registos para la Consulta" />
                        </div>
                    </EmptyDataTemplate> 
                </cc1:CustomGridView>
            </div>
        </div>
        <asp:HiddenField ID="HiddenFieldIDIrregularidad" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HiddenFieldSubProceso" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HiddenFieldConducta" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HiddenFieldIrregularidad" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HiddenFieldModificar" runat="server" ClientIDMode="Static" />
    </ContentTemplate>

    <Triggers>
    </Triggers>
</asp:UpdatePanel>


<script type="text/javascript">
    function QuestionEliminar(RowIndex, nIDIrreg) {
        $("#divGuardarCambios").dialog({
            resizable: false,
            autoOpen: true,
            height: 300,
            width: 500,
            modal: true,
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
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                    EliminaIrreg(nIDIrreg);
                    $(location).attr('href', './DetalleOPI.aspx');
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
    }


    function EliminaIrreg(nIDIrreg) {
        $.ajax({
            type: "POST",
            url: "DetalleOPI.aspx/wsEliminandoIrreg",
            data: '{IDIrreg: ' + nIDIrreg + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                return true;
            },
            failure: function () {
                alert("Error al cargar información de las irregularidades");
            }
        });
    }

    function ModificaIrreg(RowIndex, nIDIrreg) {
        $.ajax({
            type: "POST",
            url: "DetalleOPI.aspx/wsCargandoIrreg",
            data: '{IDIrreg: ' + nIDIrreg + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: false,
            success: function (data) {
                $("#<%=txtFecIrregularidad.ClientID%>").val(data.split("|")[0].substring(6, 16));
                document.getElementById('<%=txtFecIrregularidad.ClientID%>').value = data.split("|")[0].substring(6, 16);
                $("#<%=ddlProceso.ClientID%>").val(data.split("|")[1])
                $("#<%=HiddenFieldSubProceso.ClientID%>").val(data.split("|")[2])
                $("#<%=ddlSubProceso.ClientID%>").val(data.split("|")[2])
                $("#<%=HiddenFieldConducta.ClientID%>").val(data.split("|")[3])
                $("#<%=ddlConducta.ClientID%>").val(data.split("|")[3])
                $("#<%=HiddenFieldIrregularidad.ClientID%>").val(data.split("|")[4]);
                $("#<%=ddlIrregularidad.ClientID%>").val(data.split("|")[4])
                $("#<%=TxtComentarios.ClientID%>").val(data.split("|")[5]);
                $("#<%=HiddenFieldIDIrregularidad.ClientID%>").val(data.split("|")[6]);
                $("#<%=HiddenFieldModificar.ClientID%>").val("S");
                return true;
            },
            failure: function () {
                alert("Error al cargar información de las irregularidades");
            }
        });
    }





        function CerrarAlta() {
            window.close();
        }


    </script>

