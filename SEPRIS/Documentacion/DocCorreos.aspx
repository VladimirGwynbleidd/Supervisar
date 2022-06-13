<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCorreos.aspx.vb" Inherits="SEPRIS.DocCorreos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                       Documentación Administración de correos</label>
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
                            Catálogo de correos</p>
                        <p class="SubTitulosWebProyectos">
                            Consultar mensaje de correo</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón de correos.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m5d988037.png" alt="" />
                            <br />
                            Figura 1
                            
                        </p>
                        <p class="txt_gral justificado">
                            3. Doble clic sobre un registro para ver el detalle del mensaje.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m663c87d2.jpg" alt="" />
                            <br />Figura 2
                            
                        </p>
                        <p class="txt_gral justificado">
                            Esto nos envía a la pantalla de mensajes de correo</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m2a1bee38.jpg" alt="" />
                            <br />Figura 3
                           
                        </p>
                        <p class="txt_gral justificado">
                            4. Fin.</p>
                            <br />
                        <p class="SubTitulosWebProyectos">
                            Agregar mensaje de correo</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón de correos.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m5d988037.png" alt="" />
                            <br /> Figura 4
                           
                        </p>
                        <p class="txt_gral justificado">
                            3. Dar clic en el botón Agregar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m663c87d2.jpg" alt="" />
                           <br /> Figura 5
                            
                        </p>
                        <p class="txt_gral justificado">
                            4. De la pantalla de registro que se muestra capturar todos los campos marcados
                            como requeridos.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m76d29a7c.jpg" alt="" />
                           <br /> Figura 6
                            
                        </p>
                        <p class="txt_gral justificado">
                            5. Dar clic en el botón aceptar.</p>
                        <p class="txt_gral justificado">
                            6. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_5abdc12c.jpg" alt="" />
                          <br />  Figura 7
                        </p>
                        <p class="txt_gral justificado">
                            7. Fin.</p>
                            <br />
                        <p class="SubTitulosWebProyectos">
                            Modificar mensaje de correo</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón de correos.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m5d988037.png" alt="" />
                            <br />Figura 8
                            
                        </p>
                        <p class="txt_gral justificado">
                            3. Seleccionar el mensaje de correo que se vaya a modificar y dar clic en el botón
                            Modificar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_mf99e298.jpg" alt="" />
                             <br />Figura 9
                           
                        </p>
                        <p class="txt_gral justificado">
                            Esto nos envía a la pantalla de modificación.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_6d093f18.jpg" alt="" />
                            <br />Figura 10
                            
                        </p>
                        <p class="txt_gral justificado">
                            4. Editar los campos necesarios sin dejar vacíos los campos señalados como obligatorios
                            y dar clic en el botón Aceptar.</p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_7805cd5e.jpg" alt="" />
                           <br />Figura 11
                            
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
                        <p class="SubTitulosWebProyectos">
                            Eliminar mensaje de correo</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón de correos.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m5d988037.png" alt="" />
                            <br />Figura 12
                            
                        </p>
                        <p class="txt_gral justificado">
                            3. Acceder al catálogo de correos.</p>
                        <p class="txt_gral justificado">
                            4. Seleccionar el mensaje de correo que se vaya a eliminar y dar clic en el botón
                            eliminar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m6e3dd352.jpg" alt="" />
                            <br /> Figura 13
                           
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m6c345f23.jpg" alt="" />
                           <br /> Figura 14
                            
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            Correos por perfil</p>
                        <p class="SubTitulosWebProyectos">
                            Consultar correos por perfil</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m7aa6d8e0.jpg" alt="" />
                           <br /> Figura 15
                            
                        </p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Correo-Perfil.</p>
                        <p class="txt_gral justificado">
                            3. Seleccionar un perfil del DropDownList.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_60c8e22c.jpg" alt="" />
                           <br /> Figura 16
                            
                        </p>
                        <p class="txt_gral justificado">
                            4. Fin.</p>
                            <br />

                        <p class="SubTitulosWebProyectos">
                            Asignar correo a perfil</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Correo-Perfil.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_1eaf25c7.png" alt="" />
                            <br />Figura 17
                            
                        </p>
                        <p class="txt_gral justificado">
                            3. Dar clic en el botón Correo-Perfil.</p>
                        <p class="txt_gral justificado">
                            4. Seleccionar un perfil del DropDownList</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m4c31d3c1.jpg" alt="" />
                           <br /> Figura 18
                            
                        </p>
                        <p class="txt_gral justificado">
                            5. Del grid de la derecha (correos disponibles) seleccionar un registro y dar clic
                            en el botón Aceptar.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_1d81ebbe.png" alt="" />
                             <br />Figura 19
                           
                        </p>
                        <p class="txt_gral justificado">
                            6. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m21299eff.jpg" alt="" />
                          <br />  Figura 20
                            
                        </p>
                        <p class="txt_gral justificado">
                            7. Fin.</p>

                        <br />

                        <p class="SubTitulosWebProyectos">
                            Desasignar correo a perfil</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Correo-Perfil.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_1eaf25c7.png" alt="" />
                           <br /> Figura 21
                            
                        </p>
                        <p class="txt_gral justificado">
                            3. Seleccionar un perfil del DropDownList</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m4c31d3c1.jpg" alt="" />
                           <br /> Figura 22
                            
                        </p>
                        <p class="txt_gral justificado">
                            4. Del grid de la izquierda (correos asignados al perfil) seleccionar un registro
                            y dar clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_53987871.jpg" alt="" />
                            <br /> Figura 23
                           
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m21299eff.jpg" alt="" />
                           <br /> Figura 24
                            
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
                            <br />
                        <p class="SubTitulosWebProyectos">
                            Escenario alterno para asignar y desasignar correos a perfiles.</p>
                        <p class="txt_gral justificado">
                            Este proceso asigna y desasigna correos a un perfil simultáneamente.
                        </p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Correo-Perfil.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_1eaf25c7.png" alt="" />
                            <br /> Figura 25
                           
                        </p>
                        <p class="txt_gral justificado">
                            3. Seleccionar un perfil del DropDownList</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m4c31d3c1.jpg" alt="" />
                            <br /> Figura 26
                           
                        </p>
                        <p class="txt_gral justificado">
                            4. Selecciona del grid derecho (correos disponibles) y del grid izquierdo (correos
                            asignados al perfil).
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m6b026cd.jpg" alt="" />
                           <br />  Figura 27
                           
                        </p>
                        <p class="txt_gral justificado">
                            5. Da clic en el botón aceptar.</p>
                        <p class="txt_gral justificado">
                            6. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m21299eff.jpg" alt="" />
                           <br /> Figura 28
                            
                        </p>
                        <p class="txt_gral justificado">
                            7. Fin.</p>
                            <br />

                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m29a2f91c.jpg" alt="" />
                           <br /> Figura 29
                            
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Usuario-Excepciones</p>
                        <p class="SubTitulosWebProyectos">
                            Excluir a usuario de mensaje de correo</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.
                        </p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Usuario-Excepciones.</p>
                        <p class="txt_gral justificado">
                            3. Seleccionar un usuario del DropDownList.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_7a0a50cf.png" alt="" />
                           <br /> Figura 30
                            
                        </p>
                        <p class="txt_gral justificado">
                            4. Seleccionar un registro del grid derecho (correos disponibles que se envían)
                            y dar clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m78e4f9fa.jpg" alt="" />
                            <br />Figura 31
                            
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m21299eff.jpg" alt="" />
                          <br />  Figura 32
                            
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>

                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_5c882d40.jpg" alt="" />
                           <br /> Figura 33
                            <br />
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Incluir a usuario en menaje de correo.</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.
                        </p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Usuario- Excepciones.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_7a0a50cf.png" alt="" />
                            <br />Figura 34
                            
                        </p>
                        <p class="txt_gral justificado">
                            3. Seleccionar un usuario del DropDownList.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_47ebf54d.png" alt="" />
                            <br /> Figura 35
                           
                        </p>
                        <p class="txt_gral justificado">
                            4. Seleccionar un registro del grid izquierdo (los siguientes correos nunca se enviarán
                            al usuario) y dar clic en el botón Aceptar.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m1fc3dd01.jpg" alt="" />
                             <br />Figura 36
                           
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m21299eff.jpg" alt="" />
                            <br />Figura 37
                            
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
                            

                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m6fa4cc90.jpg" alt="" />
                            <br />Figura 38
                            
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Escenario alterno para incluir y excluir menajes de correo aun usuario.</p>
                        <p class="txt_gral justificado">
                            Este proceso incluye y excluye varios mensajes de correo a un usuario en una sola
                            acción.</p>
                        <p class="txt_gral justificado">
                            1. Ingresar a la pantalla opciones de perfil.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_7a0a50cf.png" alt="" />
                            Figura 39
                            <br />
                        </p>
                        <p class="txt_gral justificado">
                            2. Dar clic en el botón Usuario-Excepciones.</p>
                        <p class="txt_gral justificado">
                            3. Seleccionar un usuario del DropDownList</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_4670fff1.jpg" alt="" />
                           <br /> Figura 40
                            
                        </p>
                        <p class="txt_gral justificado">
                            4. Selecciona del grid derecho (correos disponibles que se envían) y del grid izquierdo
                            (los siguientes correos nunca se envían al usuario) y da clic en el botón Aceptar.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_5b43066b.jpg" alt="" />
                           <br /> Figura 41
                            
                        </p>
                        <p class="txt_gral justificado">
                            5. Confirmar acción.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_m21299eff.jpg" alt="" />
                           <br /> Figura 42
                            
                        </p>
                        <p class="txt_gral justificado">
                            6. Fin.</p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_23caab85b2bda157_html_18665c24.jpg" alt="" />
                           <br /> Figura 43
                            
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
                    <div style="text-align: left;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Administración de correos
                        </p>
                        <p class="txt_gral justificado">
                            La administración de correos abarca varias funcionalidades como lo son:</p>
                        <ul>
                            <li>Catálogo de correos</li>
                            <li>Correos por perfil</li>
                            <li>Exclusión de mensajes de correo por usuario.</li>
                        </ul>
                        <p class="SubTitulosWebProyectos">
                            Implementar patrón funcional</p>
                        Requerimientos:
                        <ul>
                            <li>Implementar patrón funcional de GridView</li>
                            <li>Implementa patrón funcional de Mensaje modal.</li>
                            <li>Implementar patrón funcional de filtros dinámicos.</li>
                            <li>Script de base de datos</li>
                        </ul>
                        <p class="txt_gral justificado">
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Para implementar este patrón funcional en cualquier proyecto es necesario tener
                            las siguientes tablas en la base de datos del sistema:</p>
                        <ul class="circle">
                            <li>BDS_C_GR_CORREO</li>
                            <li>BDS_R_GR_PERFIL_CORREO</li>
                            <li>BDS_R_GR_USUARIO_PERFIL_CORREO</li>
                        </ul>
                        <p class="txt_gral justificado">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Estas tablas se pueden obtener del script de creación de base de datos de la ODT-02.</p>
                            <ul>
                                <li>Código fuente</li>
                            </ul>
                            <p class="txt_gral justificado">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                El código fuente de estas clases se encuentra la carpeta Correos que cuenta con
                                cuatro páginas con sus respectivas clases de código que se en raíz del proyecto
                                WEBSITE.
                            </p>
                             <ul class="circle">
                                <li>CatalogoMensajes.aspx</li>
                                <li>CorreosPerfil.aspx</li>
                                <li>ExclusionCorreoUsuario.aspx</li>
                                <li>MenuOpciones.aspx</li>
                            </ul>
                            <p align="center">
                                <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_04f25655ed9rw233a_html_4cb47ffe.png"
                                    alt="" />
                                <br />
                                Figura 1
                            </p>
                            <p class="txt_gral justificado">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                Otras clases necesarias para la implementación del patrón funcional son las que
                                se encuentran en el proyecto Entidades en la clase Correo.vb y en el proyecto Negocio
                                en la clase Correo.vb.
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
