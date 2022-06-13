<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoUsuarios.aspx.vb" Inherits="SEPRIS.DocCatalogoUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Catálogo de Usuarios Externos</label>
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
                            Generar un Catálogo de usuarios externos a la CONSAR para el uso de las aplicaciones.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Cambiar el parámetro de Tipo de Login
                        </p>
                        <p class="txt_gral Justificado">
                            Para cambiar el parámetro de “Tipo Login” es necesario seguir los pasos siguientes:
                        </p>
                        <p class="txt_gral Justificado">
                            1. Dar clic al Menú de “Seguridad”.
                        </p>
                        <p class="txt_gral Justificado">
                            2. Seleccionar el submenú de “Catálogo de Parámetros”.
                        </p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_m6f81e84f.png"
                                width="567" />
                            <br />
                            Figura 1
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            3. En el GridView seleccionar el registro cuyo Parámetro es llamado “TipoLogin”.</p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_17485da2.png"
                                width="567" />
                            <br />
                            Figura 2
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            4. Una vez que hayamos seleccionado el Registro, damos clic al Botón de “Modificar”,
                            esto nos llevará a la pantalla de “Modificación de Parámetros”
                        </p>
                        <p class="txt_gral Justificado">
                            5. En el campo de “Valor” introducimos el valor de “Externo”</p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_3f2c11b0.png"
                                width="567" />
                            <br />
                            Figura 3
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            6. Y damos clic en “Aceptar”, esto nos guardará la información y nos direccionará
                            nuevamente a la pantalla de “Catálogo de Parámetros”
                        </p>
                        <p class="txt_gral Justificado">
                            7. Fin.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Bandeja de Catálogo de Usuarios Externos
                        </p>
                        <p class="txt_gral Justificado">
                            1. Ir al Menú de Mantenimiento
                        </p>
                        <p class="txt_gral Justificado">
                            2. Una vez en el menú de Mantenimiento hacer clic en el submenú de Catálogo de Usuarios.</p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_m65f0960c.png"
                                width="567" />
                            <br />
                            Figura 4
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            3. La bandeja de Catálogo de Usuarios es como se muestra a continuación.
                        </p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_74bb8c5e.png"
                                width="567" />
                            <br />
                            Figura 5
                        </p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Alta de Usuario Externo
                        </p>
                        <p class="txt_gral Justificado">
                            Para dar de alta un nuevo usuario es necesario seguir los siguientes pasos:
                        </p>
                        <p class="txt_gral Justificado">
                            1. Dar Clic en el botón “Agregar”, esto nos enviará a la pantalla de “Alta de Usuarios”,
                            como se muestra en la figura siguiente.</p>
                        <p class="txt_gral Justificado">
                            2. Llenar los campos obligatorios marcados con (*).
                        </p>
                        <p class="txt_gral Justificado">
                            3. Dar Clic en el botón de “Aceptar”, esto nos desplegará un Mensaje de confirmación.
                        </p>
                        <p class="txt_gral Justificado">
                            4. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará
                            la información y direccionará a la Bandeja de Catálogo de Usuarios.</p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_23f4575d.png"
                                width="567" />
                            <br />
                            Figura 6
                        </p>
                        <br />
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_35db0a48.png"
                                width="567" />
                            <br />
                            Figura 7
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            5. Si algún campo se encuentra vacio, la aplicación validará y enviará el siguiente
                            mensaje, figura siguiente, con los campos que faltan de capturar.
                        </p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_b09f2f6.png"
                                width="567" />
                            <br />
                            Figura 8
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            6. Fin.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Modificación de Usuario Externo
                        </p>
                        <p class="txt_gral Justificado">
                            Para modificar un Usuario externo, es necesario: 8. Seleccionar un registro de la
                            Bandeja, en caso que no seleccionemos un registro nos mostrará un mensaje de Error
                            como se muestra en la figura siguiente.
                        </p>
                        <p class="txt_gral Justificado">
                            1. Dar clic en el botón de “Modificar”, lo cual nos enviará a la pantalla de “Modificar
                            Usuario”.
                        </p>
                        <p class="txt_gral Justificado">
                            2. Modificamos los campos deseados.
                        </p>
                        <p class="txt_gral Justificado">
                            3. Dar clic en el botón de “Aceptar” ”, esto nos desplegará un Mensaje de confirmación.
                        </p>
                        <p class="txt_gral Justificado">
                            4. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará
                            la información y direccionará a la Bandeja de Catálogo de Usuarios.
                        </p>
                        <p class="txt_gral Justificado">
                            5. Si algún campo se encuentra vacio, la aplicación validará los campos que faltan
                            de capturar.
                        </p>
                        <p class="txt_gral Justificado">
                            6. Fin.</p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_m358199a9.png"
                                width="567" />
                            <br />
                            Figura 9
                        </p>
                        <br />
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_68295875.png"
                                width="567" />
                            <br />
                            Figura 10
                        </p>
                        <br />
                        <p class="txt_gral Justificado">
                            El botón cancelar nos mostrará una advertencia antes de direccionarnos a la Bandeja
                            de Catálogo de Usuarios.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Eliminación de Usuario Externo
                        </p>
                        <p class="txt_gral Justificado">
                            Para eliminar algún usuario debemos seguir los siguientes pasos:
                        </p>
                        <p class="txt_gral Justificado">
                            1. Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro
                            nos mostrará un mensaje de Error como se muestra en la figura 9.
                        </p>
                        <p class="txt_gral Justificado">
                            2. Dar clic en el botón de “Eliminar”, esto nos mostrará un mensaje de confirmación
                            como se muestra en la figura siguiente.
                        </p>
                        <p class="txt_gral Justificado">
                            3. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto eliminará
                            el registro seleccionado y mostrará la Bandeja de Catálogo de Usuarios.
                        </p>
                        <p class="txt_gral Justificado">
                            4. Fin.</p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_m4bede4f3.png"
                                width="567" />
                            <br />
                            Figura 11
                        </p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Consulta de Usuario Externo
                        </p>
                        <p class="txt_gral Justificado">
                            Para consultar un usuario en específico debemos seguir los pasos:
                        </p>
                        <p class="txt_gral Justificado">
                            1. Dar doble clic sobre el registro deseado, esto nos direccionará a la pantalla
                            de “Consulta de Usuarios” como se muestra en la siguiente figura.</p>
                        <p class="txt_gral Justificado">
                            2. Una vez en la pantalla de consulta damos clic en el botón de “Regresar” y nos
                            direccionará a la Bandeja de Catálogo de Usuarios”
                        </p>
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_26dfc53240755105_html_m247c48da.png"
                                width="567" />
                            <br />
                            Figura 12
                        </p>
                        <br />
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
                            <li>Filtros dinámicos.</li>
                            <li>Implementar Bitácora</li>
                            <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_C_GR_USUARIO
                                y BDS_C_GR_PARAMETRO. Esta tabla se puede obtener del script de creación de base
                                de datos de la ODT-02.</li>
                            <li>Parámetro en la tabla BDS_C_GR_PARAMETRO que indique si el sistema es interno o
                                externo mediante un registro TipoLogin, el cual podrá ser Interno o externo. Para
                                esto deberá existir un registro con los siguientes valores.</li>
                                <br />
                            <ul class="circle">
                                <li>T_DESC_PARAMETRO= TipoLogin</li>
                                <li>T_DESC_VALOR= [Interno] ó [Externo]</li>
                            </ul>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_04fa565543a34rfw43wssa3a_html_47ffe.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            <li>Código fuente.- Este patrón funcional está compuesto por una página CatalogoUsuarios.aspx
                                que se encuentra en la carpeta Seguridad del proyecto WEBSITE, que hereda de la
                                clase Usuario.vb y Parametros.vb que se encuentran en el proyecto Entities.</li>
                        </ul>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_04f2565543a34rfw4233a_html_447ffe.png"
                                alt="" />
                            <br />
                            Figura 2
                        </p>
                        <br />
                        <br />
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
