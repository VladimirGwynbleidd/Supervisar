<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCarrusel.aspx.vb" Inherits="SEPRIS.DocCarrusel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación Carrusel de Asignaciones</label>
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
                    <div style="text-align: left;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            <p class="txt_gral justificado">
                                <p class="SubTitulosWebProyectos">
                                    Subir/Bajar Orden
                                </p>
                                <p class="txt_gral justificado">
                                    1. Accede a la bandeja de Carrusel de Asignaciones (Seguridad  Carrusel de asignaciones).
                                </p>
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_m32ea89d7.jpg"
                                        alt="" />
                                    <br />
                                    Figura 1
                                </p>
                                <p class="txt_gral justificado">
                                    2. Utilizar las flechas que están en la columna Subir y Bajar.</p>
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_14218611.jpg"
                                        alt="" />
                                    <br />
                                    Figura 2
                                </p>
                                <p class="txt_gral justificado">
                                    Ejemplo: subir el orden del usuario Jorge haciendo clic en la flecha .
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_69517e05.jpg"
                                        alt="" />
                                </p>
                                <br />
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_1dc7a5be.jpg"
                                        alt="" />
                                    <br />
                                    Figura 3
                                </p>
                                Resultado:
                                <br />
                                <br />
                                <p align="center">
                                    <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_10d6a628.jpg"
                                        alt="" />
                                    <br />
                                    Figura 4
                                </p>
                                <p class="SubTitulosWebProyectos">
                                    Habilita/Deshabilita Recibe</p>
                                <p class="txt_gral justificado">
                                    Un usuario está habilitado para recibir asignaciones si se encuentra marcada la
                                    casilla de la columna recibe.
                                    <br />
                                    <br />
                                    Para habilitar las asignaciones a un usuario es necesario marcar la casilla de la
                                    columna recibe, con esto automáticamente asigna un orden, que será el último de
                                    la lista.
                                    <br />
                                    <p class="txt_gral justificado">
                                        Deshabilita usuario
                                    </p>
                                    <br />
                                    <p class="txt_gral justificado">
                                        Desmarcar la casilla del usuario Alan.
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_10d6a628.jpg"
                                            alt="" />
                                        <br />
                                        Figura 5
                                    </p>
                                    <p class="txt_gral justificado">
                                        Resultado:
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_m725a696f.jpg"
                                            alt="" />
                                        <br />
                                        Figura 6
                                    </p>
                                    <p class="txt_gral justificado">
                                        Habilita usuario
                                        <br />
                                        Marca la casilla de recibe del usuario Alan.
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_m725a696f.jpg"
                                            alt="" />
                                        <br />
                                        Figura 7
                                    </p>
                                    Resultado:
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_702b68cd.jpg"
                                            alt="" />
                                        <br />
                                        Figura 8
                                    </p>
                                    <p class="SubTitulosWebProyectos">
                                        Periodo de no asignación
                                        <br />
                                        Consultar periodos
                                    </p>
                                    <p class="txt_gral justificado">
                                        1. Accede a la bandeja de Carrusel de Asignaciones (Seguridad  Carrusel de asignaciones).
                                        <br />
                                        2. Seleccionar un usuario del grid que este habilitado (cacilla recibe marcada).
                                    </p>
                                    <p align="center">
                                        <br />
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_m48a0d445.jpg"
                                            alt="" />
                                        <br />
                                        Figura 9
                                    </p>
                                    <p>
                                    </p>
                                    <p class="txt_gral justificado">
                                        3. Dar clic en el botón Modificar Periodos de No Asignación.
                                        <br />
                                        4. Fin.
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_7bf843bc.jpg"
                                            alt="" />
                                        <br />
                                        Figura 10
                                    </p>
                                    <p class="SubTitulosWebProyectos">
                                        Agregar periodo</p>
                                    <p class="txt_gral justificado">
                                        1. Accede a la bandeja de Carrusel de Asignaciones (Seguridad  Carrusel de asignaciones).
                                        <br />
                                        2. Seleccionar un usuario del grid que este habilitado (cacilla recibe marcada).
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_m48a0d445.jpg"
                                            alt="" />
                                        <br />
                                        Figura 11
                                    </p>
                                    <p class="txt_gral justificado">
                                        3. Dar clic en el botón Modificar Periodos de No Asignación.
                                        <br />
                                        4. Capturar las fechas del periodo
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_5fefbdef.jpg"
                                            alt="" />
                                        <br />
                                        Figura 12
                                    </p>
                                    <p class="txt_gral justificado">
                                        5. Clic en el botón Agregar
                                        <br />
                                        6. Fin.
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_3df3b8bd.jpg"
                                            alt="" />
                                        <br />
                                        Figura 13
                                    </p>
                                    <p class="SubTitulosWebProyectos">
                                        Eliminar Periodo</p>
                                    <p class="txt_gral justificado">
                                        1. Accede a la bandeja de Carrusel de Asignaciones (Seguridad  Carrusel de asignaciones).
                                        <br />
                                        2. Seleccionar un usuario del grid que este habilitado (cacilla recibe marcada).
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_m48a0d445.jpg"
                                            alt="" />
                                        <br />
                                        Figura 14
                                    </p>
                                    <p class="txt_gral justificado">
                                        3. Dar clic en el botón Modificar Periodos de No Asignación.
                                        <br />
                                        4. Da clic en el texto Eliminar del grid sobre el registro del periodo que se quiera
                                        eliminar.
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_477e319e.png"
                                            alt="" />
                                        <br />
                                        Figura 15
                                    </p>
                                    <p class="txt_gral justificado">
                                        5. Fin.
                                    </p>
                                    <p align="center">
                                        <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_1bbd49cff09d85bc_html_47b41473.jpg"
                                            alt="" />
                                        <br />
                                        Figura 16
                                        <p>
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
                    <div style="text-align: left;" class="txt_gral">
                        <p class="txt_gral justificado">
                            El carrusel de asignaciones es un componente utilizado para la asignación de tareas
                            a usuarios de acuerdo a un orden asignado que está compuesto por dos páginas, una
                            la del propio carrusel y la otra de periodos de no asignación.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Implementar patrón funcional
                            <br />
                            <br />
                            Requerimientos.</p>
                        <p class="txt_gral justificado">
                            Para la implementación del patrón funcional es necesario</p>
                        <p class="txt_gral justificado">
                            <ul>
                                <li>Implementar patrón funcional de GridView</li>
                                <li>Implementa patrón funcional de Mensaje modal.</li>
                                <li>Código fuente.- en la carpeta Carrusel que está en raíz del proyecto WEBSITE se
                                    encuentra dos páginas que heredan de la clase Carrusel.vb que está dentro del proyecto
                                    Negocio.</li>
                            </ul>
                            <p>
                            </p>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_04f25655ed9w233a_html_4cb47ffe0112.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            <li>Script de base de datos</li>
                            <ul>
                            </ul>
                            Para implementar este patrón funcional en cualquier proyecto es necesario tener
                            las siguientes tablas en la base de datos del sistema:
                            <ul class="circle">
                                <li>BDS_R_GR_USUARIO_CARRUSEL</li>
                                <li>BDS_R_GR_USUARIO_CARRUSEL_PERIODO_NO_ASIGNADO</li>
                            </ul>
                            <p class="txt_gral justificado">
                                Estas tablas se pueden obtener del script de creación de base de datos de la ODT-02.
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
