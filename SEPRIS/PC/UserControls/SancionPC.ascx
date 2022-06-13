<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SancionPC.ascx.vb" Inherits="SEPRIS.SancionPC" %>

<style type="text/css">
    .auto-style1 {
        width: 300px;
    }

    .auto-style4 {
        width: 320px;
    }

    .auto-style5 {
        width: 209px;
    }

    .txt_gral {
    }
</style>

<asp:Panel ID="pnlSISAN" runat="server" CssClass="txt_gral" Enabled="false" Width="85%">

           <table  >
               <tr>
                    <td style="background:purple; height:3px; " width:"85%" colspan="4"></td>
               </tr>
               <tr >
                <td style="height: 35px; width: 20%; text-align: center;" colspan="4" > Seguimiento en Sanciones  </td></tr>
                <tr ><td class="auto-style5"></td></tr>
                <tr>
                    <td style="height: 35px; width: 20%; text-align: left;" colspan="2">Folio en SISAN: <asp:Label ID="lblFolioSISAN" runat="server"></asp:Label></td>
                    
                    <td style="height: 35px; width: 20%; text-align: left;" colspan="2">Estatus en SISAN: <asp:Label ID="lblEstatusSISAN" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td >Abogado(s) Supervisor(es):</td>
                    <td class="auto-style1"><asp:TextBox ID="txtAbogadoSuperiorSISAN" runat="server" Width="85%" style="margin-left: 0px"></asp:TextBox></td>
                    
                </tr>
               <tr>
                   <td >Abogado(s) sanciones:</td>
                    <td class="auto-style1"><asp:TextBox ID="txtAbogadosSancionesSISAN" runat="server" Width="85%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="auto-style5">Último comentario:</td>
                    <td colspan="3">
                        <asp:TextBox id="txtComentariosSISAN" runat="server" TextMode="MultiLine" Rows="3" Width="71%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Usuario que los agrego:</td>
                    <td colspan="3">
                        <asp:TextBox id="txtUsuarioSISAN" runat="server" Width="71%"></asp:TextBox>

                    </td>
                </tr>
                <tr id="trMonto" runat="server" visible="false">
                    <td class="auto-style5">Monto de la sanción:</td>
                    <td class="auto-style4"><asp:TextBox ID="txtMontoSISAN" runat="server" Width="126px"></asp:TextBox></td>
                    <td>Fecha de imposición:</td>
                    <td class="auto-style1"><asp:TextBox ID="txtFechaSISAN" runat="server"></asp:TextBox></td>
                </tr>
                <tr id="trFechaPago" runat="server" visible="false">
                    <td class="auto-style5">Fecha de pago:</td>
                    <td class="auto-style4"><asp:TextBox ID="txtPagoSISAN" runat="server"></asp:TextBox></td>
                    <td></td>
                    <td class="auto-style1"></td>
                </tr>
                <tr style="height:30px; ">
                    <td colspan="4"></td>
               </tr>
               <tr><td colspan="4"><hr /></td></tr>
                               <tr style="height:30px; ">
                    <td colspan="4"></td>
               </tr>
                <tr id="trDocumentos" runat="server" visible="false">
                    <td class="auto-style5">Documentos:</td>
                    <td colspan="2" >
                       <asp:GridView ID="gvDocumentosSISAN" runat="server" AutoGenerateColumns="false" Width="100%">
                            <Columns>                                   
                                <asp:BoundField HeaderText="DOCUMENTO" DataField="I_ID_TIPO_DOCUMENTO" />     
                                
                                <asp:TemplateField HeaderText="DOCUMENTOS NOTIFICADOS">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Link" runat="server" Text='<%#Bind("T_NOMBRE_DOCUMENTO")%>'  CommandName="Detalle" ></asp:LinkButton>                            
                                    </ItemTemplate>
                                </asp:TemplateField>     
                                
                                <asp:BoundField DataField="I_ID_DOCUMENTO" visible="false"/>

                                  <asp:BoundField HeaderText="IRREGULARIDAD" DataField="I_ID_IRREGULARIDAD" visible="false" />   

                                 <asp:BoundField HeaderText="FECHA ACUSE" DataField="F_FECH_ACUSE" visible="true" />   
                                
                                  <%--  <asp:TemplateField HeaderText="Fecha Acuse">
                                     <ItemTemplate>
                                     <table width="100%">
                                       <tr>
                                    
                                    <td style="border-right:0; border-bottom: 0">
                                        <asp:Label ID="F_FECH_ACUSE" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                            </Columns>                       
                          </asp:GridView>
                    </td>
                    <td></td>
                </tr>
            </table>
            <br />
  
    <div >
        <asp:Button ID="Button1" runat="server" Style="display: none"  />
    </div>
            
    </asp:Panel>