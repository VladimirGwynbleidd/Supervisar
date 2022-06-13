Public Class Bitacora

    Public Property Identificador As Integer
    Public Property Fecha As Date
    Public Property Usuario As String
    Public Property Procedimiento As String
    Public Property Linea As String
    Public Property Funcion As String
    Public Property Tabla As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal identificador As Integer)

        Me.Identificador = identificador
        CargarDatos()

    End Sub

    Public Sub CargarDatos()
        Dim conexion As New Conexion.SQLServer

        Try

            Dim reader As SqlClient.SqlDataReader

            Dim campos As New System.Text.StringBuilder
            campos.Append("F_FECH_BITACORA,")
            campos.Append("T_ID_USUARIO,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/Procedimiento/node())[1]', 'VARCHAR(255)') PROCEDIMIENTO,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/linea/node())[1]', 'INT') LINEA,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/función/node())[1]', 'VARCHAR(255)') FUNCION,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/@Resultado)[1]', 'BIT') RESULTADO,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/Acciones/node()/node())[1]', 'VARCHAR(255)') TABLA")




            reader = conexion.ConsultarRegistrosDR(campos.ToString(), "BDS_D_GR_BITACORA", New List(Of String)(New String() {"N_ID_BITACORA"}), New List(Of Object)(New Object() {Me.Identificador}))

            If reader.Read Then

                If IsDBNull(reader("F_FECH_BITACORA")) Then
                    Fecha = String.Empty
                Else
                    Fecha = reader("F_FECH_BITACORA")
                End If

                If IsDBNull(reader("T_ID_USUARIO")) Then
                    Usuario = String.Empty
                Else
                    Usuario = reader("T_ID_USUARIO")
                End If


                If IsDBNull(reader("PROCEDIMIENTO")) Then
                    Procedimiento = String.Empty
                Else
                    Procedimiento = reader("PROCEDIMIENTO")
                End If


                If IsDBNull(reader("LINEA")) Then
                    Linea = String.Empty
                Else
                    Linea = reader("LINEA")
                End If


                If IsDBNull(reader("FUNCION")) Then
                    Funcion = String.Empty
                Else
                    Funcion = reader("FUNCION")
                End If


                If IsDBNull(reader("TABLA")) Then
                    Tabla = String.Empty
                Else
                    Tabla = reader("TABLA")
                End If

            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Sub

    Public Function ObtenerRango(ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim campos As New System.Text.StringBuilder
            campos.Append("N_ID_BITACORA,")
            campos.Append("F_FECH_BITACORA,")
            campos.Append("T_ID_USUARIO,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/Procedimiento/node())[1]', 'VARCHAR(255)') PROCEDIMIENTO,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/linea/node())[1]', 'INT') LINEA,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/función/node())[1]', 'VARCHAR(255)') FUNCION,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/@Resultado)[1]', 'BIT') RESULTADO,")
            campos.Append("X_XML_BITACORA.value('(/Entrada/Acciones/node()/node())[1]', 'VARCHAR(255)') TABLA")

            Dim where As String = "WHERE F_FECH_BITACORA >= '" + fechaInicio.ToString("yyyyMMdd") + "' AND F_FECH_BITACORA < '" + fechaFin.AddDays(1).ToString("yyyyMMdd") + "'"


            Return conexion.ConsultarDT("SELECT " + campos.ToString() + " FROM BDS_D_GR_BITACORA " + where + " ORDER BY F_FECH_BITACORA DESC").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            listCampos.Add("N_ID_BITACORA")
            listCampos.Add("F_FECH_BITACORA")
            listCampos.Add("T_ID_USUARIO")
            listCampos.Add("X_XML_BITACORA.value('(/Entrada/Procedimiento/node())[1]', 'VARCHAR(255)') PROCEDIMIENTO")
            listCampos.Add("X_XML_BITACORA.value('(/Entrada/@Resultado)[1]', 'BIT') RESULTADO")

            Return conexion.ConsultarRegistrosDT(listCampos, "BDS_D_GR_BITACORA").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    Public Function ObtenerProcedimientos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            listCampos.Add("DISTINCT X_XML_BITACORA.value('(/Entrada/Procedimiento/node())[1]', 'VARCHAR(255)') PROCEDIMIENTO")
            Return conexion.ConsultarRegistrosDT(listCampos, "BDS_D_GR_BITACORA").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

End Class
