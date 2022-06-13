Option Strict On
Option Explicit On

Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Diagnostics
Imports System.IO
Imports System.Xml

Public Class Bitacora

    Public Property Conexion As Conexion.SQLServer
    Private Property BitacoraXml As String
    Private Property SessionId As String
    Private Property IdUsuario As String

    Public Sub New(ByVal procedimiento As String, ByVal sessionId As String, ByVal idUsuario As String)
        Try

            Me.SessionId = sessionId
            Me.IdUsuario = idUsuario

            Dim strB As StringBuilder = New StringBuilder()
            Dim xmlTW As XmlTextWriter = New XmlTextWriter(New StringWriter(strB))

            xmlTW.Formatting = Formatting.Indented
            xmlTW.WriteStartDocument()
            xmlTW.WriteStartElement("Entrada")
            xmlTW.WriteElementString("Procedimiento", procedimiento)
            xmlTW.WriteElementString("timestamp", Date.Now.ToString("yyyyMMdd HH:mm:ss"))

            Dim sf As StackFrame = New StackTrace(True).GetFrames(1)
            xmlTW.WriteElementString("linea", sf.GetFileLineNumber().ToString())
            xmlTW.WriteElementString("función", sf.GetMethod().ToString())
            xmlTW.WriteStartElement("Acciones")
            xmlTW.WriteEndElement()
            xmlTW.WriteEndDocument()
            xmlTW.Close()

            BitacoraXml = strB.ToString()

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método IniciarBitacora de la clase Bitacora.", ex)
        End Try
    End Sub

    Public Sub Finalizar(ByVal resultado As Boolean)
        Try
            Conexion = New Conexion.SQLServer
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            xmlDocumento.DocumentElement().SetAttribute("Resultado", resultado.ToString())

            Dim strB As StringBuilder = New StringBuilder()
            Dim xmlTW As XmlTextWriter = New XmlTextWriter(New StringWriter(strB))

            xmlDocumento.WriteTo(xmlTW)

            BitacoraXml = strB.ToString()

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)


            listCampos.Add("T_ID_SESION") : listValores.Add(Me.SessionId)
            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdUsuario)
            listCampos.Add("X_XML_BITACORA") : listValores.Add(BitacoraXml)

            Conexion.Insertar("BDS_D_GR_BITACORA", listCampos, listValores)

        Catch ex As Exception

            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método FinalizarBitacora de la clase Bitacora.", ex)

        Finally

            If Not IsNothing(Conexion) Then

                If Conexion.EstadoConexion Then

                    Conexion.CerrarConexion()
                    Conexion = Nothing

                End If

            End If

        End Try

    End Sub

    Public Sub BuscarUnRegistro(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

        Try
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            Dim nodo As XmlNode = xmlDocumento.DocumentElement
            nodo = nodo.SelectSingleNode("Acciones")

            Dim selectElement As XmlElement = xmlDocumento.CreateElement("BuscarUnRegistro")
            Dim sqlElement As XmlElement = xmlDocumento.CreateElement("SQL")

            Dim tablaElement As XmlElement = xmlDocumento.CreateElement("Tabla")
            tablaElement.InnerText = tabla
            selectElement.AppendChild(tablaElement)

            Dim condicionElement As XmlElement = xmlDocumento.CreateElement("Condicion")
            For count As Integer = 0 To camposCondicion.Count - 1
                Dim columna As String = camposCondicion(count).ToString().Trim
                Dim valor As String = valoresCondicion(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                condicionElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(condicionElement)

            selectElement.SetAttribute("Resultado", resultado.ToString())
            If Not resultado Then
                selectElement.SetAttribute("Error", mensajeError)
            End If


            nodo.AppendChild(selectElement)
            BitacoraXml = xmlDocumento.OuterXml

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método BitacoraConsulta de la clase Bitacora.", ex)
        End Try

    End Sub

    Public Sub BuscarUnRegistroConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)
        Try
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            Dim nodo As XmlNode = xmlDocumento.DocumentElement
            nodo = nodo.SelectSingleNode("Acciones")

            Dim selectElement As XmlElement = xmlDocumento.CreateElement("BuscarUnRegistro")
            Dim sqlElement As XmlElement = xmlDocumento.CreateElement("SQL")

            Dim tablaElement As XmlElement = xmlDocumento.CreateElement("Tabla")
            tablaElement.InnerText = tabla
            selectElement.AppendChild(tablaElement)

            Dim condicionElement As XmlElement = xmlDocumento.CreateElement("Condicion")
            For count As Integer = 0 To camposCondicion.Count - 1
                Dim columna As String = camposCondicion(count).ToString().Trim
                Dim valor As String = valoresCondicion(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                condicionElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(condicionElement)

            selectElement.SetAttribute("Resultado", resultado.ToString())
            If Not resultado Then
                selectElement.SetAttribute("Error", mensajeError)
            End If


            nodo.AppendChild(selectElement)
            BitacoraXml = xmlDocumento.OuterXml

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método BitacoraConsulta de la clase Bitacora.", ex)
        End Try

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)
    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub


    Public Sub ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub


    Public Sub ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub


    Public Sub ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal campos As List(Of String), ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDR(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDRConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDS(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDSConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDT(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarRegistrosDTConTransaccion(ByVal campos As String, ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal campoOrder As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarDR(ByVal query As String)

    End Sub

    Public Sub ConsultarDRConTransaccion(ByVal query As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarDS(ByVal query As String)

    End Sub

    Public Sub ConsultarDSConTransaccion(ByVal query As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ConsultarDT(ByVal query As String)

    End Sub

    Public Sub ConsultarDT(ByVal query As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub


#Region "Métodos para insertar"

    Public Sub Insertar(ByVal tabla As String, ByVal valores As String)

    End Sub

    Public Sub InsertarConTransaccion(ByVal tabla As String, ByVal valores As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Insertar(ByVal tabla As String, ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub InsertarConTransaccion(ByVal tabla As String, ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Insertar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)
        Try
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            Dim nodo As XmlNode = xmlDocumento.DocumentElement
            nodo = nodo.SelectSingleNode("Acciones")

            Dim selectElement As XmlElement = xmlDocumento.CreateElement("Insertar")
            Dim sqlElement As XmlElement = xmlDocumento.CreateElement("SQL")

            Dim tablaElement As XmlElement = xmlDocumento.CreateElement("Tabla")
            tablaElement.InnerText = tabla
            selectElement.AppendChild(tablaElement)

            Dim camposElement As XmlElement = xmlDocumento.CreateElement("Campos")
            For count As Integer = 0 To campos.Count - 1
                Dim columna As String = campos(count).ToString().Trim
                Dim valor As String = valores(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                camposElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(camposElement)

            selectElement.SetAttribute("Resultado", resultado.ToString())
            If Not resultado Then
                selectElement.SetAttribute("Error", mensajeError)
            End If


            nodo.AppendChild(selectElement)
            BitacoraXml = xmlDocumento.OuterXml

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método Insertar de la clase Bitacora.", ex)
        End Try
    End Sub


    Public Sub InsertarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Insertar(ByVal tabla As String, ByVal campo As String, ByVal valor As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub InsertarConTransaccion(ByVal tabla As String, ByVal campo As String, ByVal valor As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub InsertarConTransaccion(ByVal dataTable As DataTable, ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Insertar(ByVal dataTable As DataTable, ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

#End Region

#Region "Métodos para actualizar"

    Public Sub ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Actualizar(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)
        Try
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            Dim nodo As XmlNode = xmlDocumento.DocumentElement
            nodo = nodo.SelectSingleNode("Acciones")

            Dim selectElement As XmlElement = xmlDocumento.CreateElement("Actualizar")
            Dim sqlElement As XmlElement = xmlDocumento.CreateElement("SQL")

            Dim tablaElement As XmlElement = xmlDocumento.CreateElement("Tabla")
            tablaElement.InnerText = tabla
            selectElement.AppendChild(tablaElement)

            Dim camposElement As XmlElement = xmlDocumento.CreateElement("Campos")
            For count As Integer = 0 To campos.Count - 1
                Dim columna As String = campos(count).ToString().Trim
                Dim valor As String = valores(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                camposElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(camposElement)

            Dim condicionElement As XmlElement = xmlDocumento.CreateElement("Condicion")
            For count As Integer = 0 To camposCondicion.Count - 1
                Dim columna As String = camposCondicion(count).ToString().Trim
                Dim valor As String = valoresCondicion(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                condicionElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(condicionElement)

            selectElement.SetAttribute("Resultado", resultado.ToString())
            If Not resultado Then
                selectElement.SetAttribute("Error", mensajeError)
            End If

            nodo.AppendChild(selectElement)
            BitacoraXml = xmlDocumento.OuterXml

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método Actualizar de la clase Bitacora.", ex)
        End Try
    End Sub

    Public Sub ActualizarConTransaccion(ByVal tabla As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)
        Try
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            Dim nodo As XmlNode = xmlDocumento.DocumentElement
            nodo = nodo.SelectSingleNode("Acciones")

            Dim selectElement As XmlElement = xmlDocumento.CreateElement("ActualizarConTransaccion")
            Dim sqlElement As XmlElement = xmlDocumento.CreateElement("SQL")

            Dim tablaElement As XmlElement = xmlDocumento.CreateElement("Tabla")
            tablaElement.InnerText = tabla
            selectElement.AppendChild(tablaElement)

            Dim condicionElement As XmlElement = xmlDocumento.CreateElement("Condicion")
            For count As Integer = 0 To camposCondicion.Count - 1
                Dim columna As String = camposCondicion(count).ToString().Trim
                Dim valor As String = valoresCondicion(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                condicionElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(condicionElement)

            selectElement.SetAttribute("Resultado", resultado.ToString())
            If Not resultado Then
                selectElement.SetAttribute("Error", mensajeError)
            End If


            nodo.AppendChild(selectElement)
            BitacoraXml = xmlDocumento.OuterXml

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método BitacoraConsulta de la clase Bitacora.", ex)
        End Try

    End Sub

    Public Sub Actualizar(ByVal tabla As String, ByVal campo As String, ByVal valor As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub ActualizarConTransaccion(ByVal tabla As String, ByVal pCampo As String, ByVal pValor As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub
#End Region

#Region "Métodos para eliminar"

    Public Sub Eliminar(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EliminarConTransaccion(ByVal tabla As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Eliminar(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)
        Try
            Dim xmlDocumento As XmlDocument
            xmlDocumento = New XmlDocument
            xmlDocumento.LoadXml(BitacoraXml)
            Dim nodo As XmlNode = xmlDocumento.DocumentElement
            nodo = nodo.SelectSingleNode("Acciones")

            Dim selectElement As XmlElement = xmlDocumento.CreateElement("Eliminar")
            Dim sqlElement As XmlElement = xmlDocumento.CreateElement("SQL")

            Dim tablaElement As XmlElement = xmlDocumento.CreateElement("Tabla")
            tablaElement.InnerText = tabla
            selectElement.AppendChild(tablaElement)

            Dim camposElement As XmlElement = xmlDocumento.CreateElement("Condicion")
            For count As Integer = 0 To camposCondicion.Count - 1
                Dim columna As String = camposCondicion(count).ToString().Trim
                Dim valor As String = valoresCondicion(count).ToString().Trim
                Dim valorElement As XmlElement = xmlDocumento.CreateElement(columna)
                valorElement.InnerText = valor
                camposElement.AppendChild(valorElement)
            Next
            selectElement.AppendChild(camposElement)

            selectElement.SetAttribute("Resultado", resultado.ToString())
            If Not resultado Then
                selectElement.SetAttribute("Error", mensajeError)
            End If


            nodo.AppendChild(selectElement)
            BitacoraXml = xmlDocumento.OuterXml

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.Message & ex.StackTrace, EventLogEntryType.Error, "Bitacora", "")
            Throw New Exception("Ocurrió un error en el método Actualizar de la clase Bitacora.", ex)
        End Try
    End Sub

    Public Sub EliminarConTransaccion(ByVal tabla As String, ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

#End Region

#Region "Métodos para ejecutar stored procedures"

    Public Sub EjecutarSP(ByVal nombreSp As String)

    End Sub

    Public Sub EjecutarSPConTransaccion(ByVal nombreSp As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDR(ByVal nombreSp As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDRConTransaccion(ByVal nombreSP As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDS(ByVal nombreSp As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDSConTransaccion(ByVal nombreSp As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDT(ByVal nombreSp As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDTConTransaccion(ByVal nombreSp As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSP(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSP(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDR(ByVal nombreSP As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDRConTransaccion(ByVal pNombreSP As String, ByVal pParametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDS(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDSConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDT(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDTConTransaccion(ByVal nombreSp As String, ByVal parametros As List(Of String), ByVal valores As List(Of Object), ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDR(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDRConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDS(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDSConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDT(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub EjecutarSPConsultaDTConTransaccion(ByVal nombreSp As String, ByVal parametros() As SqlParameter, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub
#End Region


    Public Sub EjecutarConTransaccion(ByVal query As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub

    Public Sub Ejecutar(ByVal query As String, ByVal resultado As Boolean, ByVal mensajeError As String)

    End Sub




End Class

