<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocFiltrosDinamicos.aspx.vb" Inherits="SEPRIS.DocFiltrosDinamicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Filtros Dinámicos</label>
                </div>
                <br />
                <br />
                <asp:Button ID="btnManualUsuario" runat="server" Text="Manual Usuario" />
                <br />
                <br />
                <asp:Button ID="btnManualTecnico" runat="server" Text="Manual Técnico" />
            </asp:Panel>
            <asp:Panel ID="pnlUsuario" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualUsuario" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalUsuario" runat="server">
                    <div style="text-align:justify;" class="txt_gral">
                        <p class="txt_gral Justificado">
                            Generar un control que permita agregar y quitar filtros de acuerdo a la necesidad
                            del cliente dinámicamente, además que permita guardar el estado del filtro aunque
                            se cambie de página o se realice un postback.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Agregar Filtro
                        </p>
                        <p class="txt_gral Justificado">
                            El control de filtros deberá implementarse en aquellas pantallas en las que haya 
                            una Bandeja de registros con los campos suficientes para poder filtrar. Para 
                            agregar un filtro en nuestro control será necesario seguir los siguientes pasos.
                        </p>
                        <p class="txt_gral Justificado">
                            1. Seleccionar el filtro que se desea agregar del Combo que aparece en la 
                            siguiente figura.
                        </p>
                        <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_64b4e8b8936d2efa_html_75b40a3c.png" width="567" />
                                <br />
                                Figura 1
                            </p>
                            <br />
                        <p class="txt_gral Justificado">
                            2. Llenar el campo con el valor deseado de acuerdo al filtro que se agrego.
                        </p>
                        <p class="txt_gral Justificado">
                            3. Dar clic en “Filtrar” para obtener los resultados esperados.
                        </p>
                        <p class="txt_gral Justificado">
                            4. Fin.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Eliminar Filtro
                        </p>
                        <p class="txt_gral Justificado">
                            Para eliminar un filtro de nuestro control es necesario dar clic en el botón de 
                            lado derecho del filtro deseado y automáticamente desaparecerá dicho control.
                        </p>
                         <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_64b4e8b8936d2efa_html_m483abe2.png" width="567" />
                                <br />
                                Figura 2
                            </p>
                            <br />
                    </div>
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlTecnico" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualTecnico" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalTecnico" runat="server">
                    <div style="text-align:justify;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Instalación
                        </p>
                        <p class="txt_gral Justificado">
                            La instalación del los Filtros Dinámicos consiste en que los controles personalizados se encuentren dentro del proyecto que los consumirá, como se muestra en la figura (Imagen 1).
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_16f96b25763ca0f3_html_m63912bed.png" name="Picture 1" width="244" height="325" border="0" alt="Instalación">
                            <br />
                            Imagen 1
                        </p>
                        <p class="txt_gral Justificado">
                            Es importante señalar que el nombre de las carpetas “Controles”, “Filtro” e “imagenes” no deben cambiar
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Implementación
                        </p>
                        <p class="txt_gral Justificado">
                            Para utilizar los Filtros Dinámicos es necesario que se encuentren incluidos en el proyecto.
                        </p>
                        <p class="txt_gral Justificado">
                            Antes de poder utilizarlo en una pantalla es necesario referenciarlo, para esto se escribe la línea que se muestra en la figura (Imagen 2) debajo del registro de la pantalla.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_16f96b25763ca0f3_html_m7579120b.png" name="Picture 2" width="589" height="26" border="0" alt="Registro">
                            <br />
                            Imagen 2
                        </p>
                        <p class="txt_gral Justificado">
                            Posterior a esto se escribe la línea de la figura (Imagen 3)
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_16f96b25763ca0f3_html_44d0f393.png" name="Picture 3" width="432" height="26" border="0" alt="Control">
                            <br />
                            Imagen 3
                        </p>
                        <p class="txt_gral Justificado">
                            Con esto se ha creado el control, lo siguiente es configurarlo, para esto se debe realizar la carga inicial del filtro en el load de la pagina la primera vez que se acede a ella.
                        </p>
                        <p class="txt_gral Justificado">
                            La primera instrucción que se utiliza es:
                        </p>
                        <p class="txt_gral Justificado">
                            ucFiltro1.resetSession()
                        </p>
                        <p class="txt_gral Justificado">
                            Con esta se borra la sesión del filtro, hay que tomar en cuenta que se está asumiendo que el nombre del Filtro Dinámico es ucFiltro1.
                        </p>
                        <p class="txt_gral Justificado">
                            Ahora que se borro podemos cargar nuevos filtros, para esto se utiliza el método “AddFiltro” de nuestro filtro, para nuestro ejemplo se utiliza una instrucción como la de la siguiente figura (Imagen 4)
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_16f96b25763ca0f3_html_m680ff247.png" name="Picture 4" width="800" height="20" border="0" alt="Control">
                            <br />
                            Imagen 4
                        </p>
                        <p class="txt_gral Justificado">
                            Los parámetros que recibe son los siguientes:
                            <ul>
                                <li>Nombre: Nombre que tendrá el control</li>
                                <li>TipoControl: Se tienen 5 tipos
                                    <ul>
                                        <li>TextBox</li>
                                        <li>DropDownList</li>
                                        <li>Calendar</li>
                                        <li>RadioButton</li>
                                        <li>CheckBox</li>
                                    </ul>
                                </li>
                                <li>FuenteDatos: Objeto de la fuente de datos del filtro</li>
                                <li>DataTextField: Nombre del elemento de la fuente de datos que contiene la descripción de dicho elemento</li>
                                <li>DataValueField: Nombre del elemento de la fuente de datos que contiene el valor de dicho elemento</li>
                                <li>DataValueType: Contiene el tipo de valor del cual será el filtro, se utiliza para formar la sentencia WHERE, sus opciones son
                                    <ul>
                                        <li>IntegerType: Tipo entero tomara el valor tal cual este en el control</li>
                                        <li>StringType: Tipo cadena, le agregara apostrofes al inicio y final del valor del control</li>
                                        <li>BoolType: Solo se utiliza junto con el DropDownList cuando se requiere un filtro de vigencia, al estar en la selección default “-1” “Todos”, no regresa cadena para el WHERE</li>
                                    </ul>
                                </li>
                                <li>isCalendarSingle: Si es de tipo calendario indica si es un solo calendario</li>
                                <li>isTextBoxLike: Si es de de tipo TextBox el filtrado será con un LIKE</li>
                                <li>isTextBoxRange: Si es de tipo TextBox indica si será uno o dos controles para rango.</li>
                                <li>isDefault: Indica si el control estará dibujado desde que se ingresa a la pantalla.</li>
                                <li>isFixed: Indica si el control estará dibujado desde que se ingresa a la pantalla y no podrá eliminarse.</li>
                                <li>initValue: Valor inicial del filtro.</li>
                            </ul>
                            <p>
                            </p>
                            <p class="txt_gral Justificado">
                                Cabe señalar que solo los dos primeros parámetros son obligatorios, el resto son 
                                opcionales.
                            </p>
                            <p class="txt_gral Justificado">
                                Después de colocar los filtros deseados se debe ejecutar la instrucción
                            </p>
                            <p class="txt_gral Justificado">
                                ucFiltro1.LoadDDL(&quot;NombrePantalla&quot;)
                            </p>
                            <p class="txt_gral Justificado">
                                Donde NombrePantalla corresponde a la pantalla donde se está implementando, esto 
                                con el objetivo de que sea único.
                            </p>
                            <p class="txt_gral Justificado">
                                Finalmente para realizar el filtrado se debe controlar el evento de filtrado, 
                                para esto se crea el manejador del evento clic del botón Filtrar, como se 
                                muestra en la figura (Imagen 5)
                            </p>
                            <p align="center">
                                <img alt="Filtrar" border="0" height="83" name="Picture 5" 
                                    src="Imagenes_Manual/o_16f96b25763ca0f3_html_m7579999z.png" width="593">
                                <br />
                                Imagen 5 </img></p>
                            <p class="txt_gral Justificado">
                                Como se puede observar en la Imagen 5 se utiliza el método “getFilterSelection” 
                                para obtener los valores del los filtros.
                            </p>
                        </p>
                    </div>

                    <asp:Button ID="btnRegresarTecnico" runat="server" Text="Regresar" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnManualUsuario" />
            <asp:PostBackTrigger ControlID="btnManualTecnico" />
            <asp:PostBackTrigger ControlID="btnRegresarUusario" />
            <asp:PostBackTrigger ControlID="btnRegresarTecnico" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
