<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocMensajesNotificacion.aspx.vb" Inherits="SEPRIS.DocMensajesNotificacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Mensajes de Notificación</label>
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
                    <div style="text-align: justify;" class="txt_gral">
                        <p class="txt_gral Justificado">
                            Generar un control que permita configurar mensajes que aparezcan al inicio de la
                            sesión a un usuario en específico en modo de notificación.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Pantalla Principal
                        </p>
                        <p class="txt_gral Justificado">
                            Para configurar los mensajes de notificación es necesario ir al menú de “Mantenimiento”
                            y seleccionar el submenú de “Mensajes de Notificación”
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_m540da677.png"
                                    width="567" />
                                <br />
                                Figura 1
                            </p>
                            <br />
                            La pantalla que nos muestra es como la siguiente.
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_m36d04b7.png"
                                    width="567" />
                                <br />
                                Figura 2
                            </p>
                            <br />
                            <p>
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Mensajes de Notificación
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Bandeja de Mensajes de Notificación
                            </p>
                            <p class="txt_gral Justificado">
                                La bandeja de mensajes de Notificación es como se muestra en la figura siguiente.
                                <p align="CENTER">
                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_m76e5f7fc.png"
                                        width="567" />
                                    <br />
                                    Figura 3
                                </p>
                                <br />
                                <p>
                                </p>
                                <p class="SubTitulosWebProyectos">
                                    Alta de Mensajes de Notificación
                                </p>
                                <p class="txt_gral Justificado">
                                    Para dar de alta un nuevo Mensaje de Notificación es necesario seguir los siguientes
                                    pasos:
                                </p>
                                <p class="txt_gral Justificado">
                                    1. Dar Clic en el botón “Agregar”, esto nos enviará a la pantalla de “Alta de Mensaje”,
                                    como se muestra en la figura siguiente.
                                </p>
                                <p class="txt_gral Justificado">
                                    2. Llenar los campos obligatorios marcados con (*).
                                </p>
                                <p class="txt_gral Justificado">
                                    3. Introducir el mensaje con ayuda de los botones.
                                </p>
                                <p class="txt_gral Justificado">
                                    4. Adjuntar un archivo adjunto al mensaje si es necesario.
                                </p>
                                <p class="txt_gral Justificado">
                                    5. De la bandeja de Usuarios seleccionar los deseados.
                                </p>
                                <p class="txt_gral Justificado">
                                    6. Dar Clic en el botón de “Aceptar”, esto nos desplegará un Mensaje donde introduciremos
                                    las fechas en las cuales el mensaje le aparecerá al usuario o los usuarios seleccionados,
                                    como se muestra en la figura siguiente.
                                </p>
                                <p align="CENTER">
                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_15e669c8.png"
                                        width="567" />
                                    <br />
                                    Figura 4
                                </p>
                                <br />
                                <p class="txt_gral Justificado">
                                    7. Una vez introducidas las fechas, dar clic en el botón “Aceptar”, esto guardara
                                    la información y direccionará a la “Bandeja de Mensajes de Notificación”</p>
                                <p class="txt_gral Justificado">
                                    8. Fin.
                                </p>
                                <p class="SubTitulosWebProyectos">
                                    Modificación de Mensajes de Notificación
                                </p>
                                <p class="txt_gral Justificado">
                                    Para modificar un Mensaje de notificación, es necesario:
                                </p>
                                <p class="txt_gral Justificado">
                                    1. Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro
                                    nos mostrará un mensaje de Error como se muestra en la figura siguiente.
                                </p>
                                <p class="txt_gral Justificado">
                                    2. Dar clic en el botón de “Modificar”, lo cual nos enviará a la pantalla de “Modificar
                                    Mensaje”.
                                </p>
                                <p class="txt_gral Justificado">
                                    3. Modificamos los campos deseados.
                                </p>
                                <p class="txt_gral Justificado">
                                    4. En la parte inferior nos mostrara dos GridView, con los usuarios asignados a
                                    ese mensaje y otro con los que aun no han sido asignados.
                                </p>
                                <p class="txt_gral Justificado">
                                    5. Si asignamos nuevos usuarios, esto nos desplegara un Mensaje con el intervalo
                                    de fechas para el nuevo usuario ingresado.
                                </p>
                                <p class="txt_gral Justificado">
                                    6. Finalmente dar clic en el botón de “Aceptar” esto nos desplegará un Mensaje de
                                    confirmación.
                                </p>
                                <p class="txt_gral Justificado">
                                    7. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará
                                    la información y direccionará a la Bandeja de Mensajes de confirmación.
                                </p>
                                <p class="txt_gral Justificado">
                                    8. Si algún campo se encuentra vacio, la aplicación validará los campos que faltan
                                    de capturar.
                                </p>
                                <p class="txt_gral Justificado">
                                    9. Fin.
                                </p>
                                <p align="CENTER">
                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_7728864f.png"
                                        width="567" />
                                    <br />
                                    Figura 5
                                </p>
                                <br />
                                <p align="CENTER">
                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_m65c1e645.png"
                                        width="567" />
                                    <br />
                                    Figura 6
                                </p>
                                <br />
                                <p class="SubTitulosWebProyectos">
                                    Eliminación de Mensajes de Notificación
                                </p>
                                <p class="txt_gral Justificado">
                                    Para eliminar algún mensaje de notificación debemos seguir los siguientes pasos:
                                    <p class="txt_gral Justificado">
                                        1. Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro
                                        nos mostrará un mensaje de Error.
                                        <p class="txt_gral Justificado">
                                            2. Dar clic en el botón de “Eliminar”, esto nos mostrará un mensaje de confirmación
                                            como se muestra en la figura siguiente.
                                            <p class="txt_gral Justificado">
                                                3. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto eliminará
                                                el registro seleccionado y mostrará la Bandeja de Mensajes de Notificación.
                                                <p class="txt_gral Justificado">
                                                    4. Fin.
                                                </p>
                                                <p class="SubTitulosWebProyectos">
                                                    Consulta de Mensajes de Notificación
                                                </p>
                                                <p class="txt_gral Justificado">
                                                    Para consultar un mensaje en específico debemos seguir los pasos:
                                                </p>
                                                <p class="txt_gral Justificado">
                                                    1. Dar doble clic sobre el registro deseado, esto nos direccionará a la pantalla
                                                    de “Consulta de Mensaje de Notificación”.</p>
                                                <p class="txt_gral Justificado">
                                                    2. Una vez en la pantalla de consulta damos clic en el botón de “Regresar” y nos
                                                    direccionará a la Bandeja de Mensajes de Notificación”
                                                </p>
                                                <p class="txt_gral Justificado">
                                                    3. Fin.
                                                </p>
                                                <p class="SubTitulosWebProyectos">
                                                    Configuración de Estilos
                                                </p>
                                                <p class="txt_gral Justificado">
                                                    Para situarnos en la pantalla de configuración de estilos es necesario dar clic
                                                    en el botón de “Configuración de Estilos” en la pantalla principal. La pantalla
                                                    de configuración de estilos es como la que se muestra en la figura siguiente.
                                                </p>
                                                <p align="CENTER">
                                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_m4bede4f3.png"
                                                        width="567" />
                                                    <br />
                                                    Figura 7
                                                </p>
                                                <br />
                                                <p class="txt_gral Justificado">
                                                    Para modificar cada estilo se deberá hacer directamente en el GridView, cada control
                                                    muestra las características del estilo, una vez modificado deberá dar clic en el
                                                    botón de “Aceptar”, esto mostrará un mensaje de confirmación.
                                                </p>
                                                <p align="CENTER">
                                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_mbddb596.png"
                                                        width="567" />
                                                    <br />
                                                    Figura 8
                                                </p>
                                                <br />
                                                <p class="txt_gral Justificado">
                                                    El mensaje de Notificación se mostrará una vez que el usuario ingrese al sistema
                                                    y ya no se volverá a mostrar durante la sesión.</p>
                                                <p align="CENTER">
                                                    <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_6275622ece4c8ffa_html_m6c0325ff.png"
                                                        width="567" />
                                                    <br />
                                                    Figura 9
                                                </p>
                                                <br />
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                                <p>
                                                </p>
                                            </p>
                                        </p>
                                    </p>
                                </p>
                            </p>
                        </p>
                    </div>
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" Width="110px" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlTecnico" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualTecnico" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalTecnico" runat="server">
                    <div style="text-align: justify;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Requerimientos:</p>
                        <ul>
                            <li>Implementar patrón funcional de GridView.</li>
                            <li>Implementa patrón funcional de Mensaje modal.</li>
                            <li>Implementar patrón funcional de Catálogo de usuarios.</li>
                            <li>Filtros dinámicos.</li>
                            <li>Implementar Bitácora.</li>
                            <li>Script de base de datos.- el patrón funcional requiere de las siguientes tablas:</li>
                            <br />
                            <ul class="circle">
                                <li>BDS_C_GR_COLOR</li>
                                <li>BDS_C_GR_ALINEACION</li>
                                <li>BDS_C_GR_TAMANO_FUENTE</li>
                                <li>BDS_C_GR_TIPO_FUENTE</li>
                                <li>BDS_C_GR_ESTILO_CONTENIDO</li>
                                <li>BDS_C_GR_NOTIFICACION_PANTALLA</li>
                                <li>BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA</li>
                            </ul>
                            <br />
                            Estas tablas se pueden obtener del script de creación de base de datos de la ODT-02.
                            <br />
                            <li>Código fuente.- Este patrón funcional está compuesto por las páginas que se encuentran
                                en la carpeta Notificaciones que a su vez pertenece a la carpeta Mantenimiento del
                                proyecto WEBSITE.</li>
                                <br />
                            <ul class="circle">
                                <li>Notificaciones.aspx</li>
                                <li>NotificacionesBandeja.aspx</li>
                                <li>NotificacionesConfiguracionEstilos.aspx</li>
                                <li>NotificacionesDatos.aspx</li>
                            </ul>
                            <br />
                            Estas páginas heredan de la clase Notificaciones.vb que se encuentra en el proyecto
                            Entities.
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_04f2sdaa43a34rfw4233a_html_447ffe.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            <br />
                            <br />
                        </ul>
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
