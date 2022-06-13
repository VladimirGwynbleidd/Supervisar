Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module ControlExtensions
    <Extension()> _
    Public Function GetAllControlsOfType(Of T As Control)(ByVal parent As Control) As IEnumerable(Of T)
        Dim result = New List(Of T)()
        For Each control As Control In parent.Controls
            If TypeOf control Is T Then
                result.Add(DirectCast(control, T))
            End If
            If control.HasControls() Then
                result.AddRange(control.GetAllControlsOfType(Of T)())
            End If
        Next
        Return result
    End Function


    Public Function ConvertToDataTable(Of T)(ByVal list As IList(Of T)) As DataTable
        Dim table As New DataTable()
        If Not list.Any Then
            Return table
        End If
        Dim fields() = list.First.GetType.GetProperties
        For Each field In fields
            table.Columns.Add(field.Name, field.PropertyType)
        Next
        For Each item In list
            Dim row As DataRow = table.NewRow()
            For Each field In fields
                Dim p = item.GetType.GetProperty(field.Name)
                row(field.Name) = p.GetValue(item, Nothing)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    <Extension()>
    Public Function ToList(Of T)(ByVal data As SqlClient.SqlDataReader) As IEnumerable(Of T)

        Try

            Dim list As New List(Of T)
            Dim obj As T

            Dim lstColumnsNames As New List(Of String)

            lstColumnsNames = GetDataReaderColumnsExt(data)

            While data.Read()
                obj = Activator.CreateInstance(Of T)()

                For Each prop As PropertyInfo In obj.GetType().GetProperties()

                    If prop.Name = "data" Then
                        Continue For
                    End If

                    If Not lstColumnsNames.Contains(prop.Name) Then
                        Continue For
                    End If


                    If Not Object.Equals(data(prop.Name), DBNull.Value) Then
                        prop.SetValue(obj, data(prop.Name), Nothing)
                    End If
                Next
                list.Add(obj)
            End While
            Return list


        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetDataReaderColumnsExt(ByRef data As SqlClient.SqlDataReader) As List(Of String)
        Dim lst As New List(Of String)
        Dim dtSchema As New DataTable

        dtSchema = data.GetSchemaTable()
        For Each row As DataRow In dtSchema.Rows
            lst.Add(row.Field(Of String)("ColumnName"))
        Next
        Return lst
    End Function

    <Extension()>
    Public Function toListHtml(ByVal lstOfString As List(Of String)) As String
        Dim result As String = "<ul>"

        If lstOfString.Count > 0 Then
            For Each lsCad As String In lstOfString
                result &= "<li>" & lsCad & "</li>"
            Next
            Return result & "</ul>"
        Else
            Return ""
        End If
    End Function

    <Extension()>
    Public Function toListPersona(ByVal lstOfSupervisor As List(Of SupervisorAsignado)) As List(Of Persona)
        Dim lstPersonas As New List(Of Persona)
        Dim objPersona As Persona

        If lstOfSupervisor.Count > 0 Then
            For Each objSup As SupervisorAsignado In lstOfSupervisor
                objPersona = New Persona
                objPersona.Correo = objSup.Correo
                objPersona.Nombre = objSup.Nombre

                lstPersonas.Add(objPersona)
            Next
        End If

        Return lstPersonas
    End Function

    <Extension()>
    Public Function toListPersona(ByVal lstOfSupervisor As List(Of InspectorAsignado)) As List(Of Persona)
        Dim lstPersonas As New List(Of Persona)
        Dim objPersona As Persona

        If lstOfSupervisor.Count > 0 Then
            For Each objInspec As InspectorAsignado In lstOfSupervisor
                objPersona = New Persona
                objPersona.Correo = objInspec.Correo
                objPersona.Nombre = objInspec.Nombre

                lstPersonas.Add(objPersona)
            Next
        End If

        Return lstPersonas
    End Function

    <Extension()>
    Public Function ToListQuery(ByVal lstOfTipoSubEntidad As List(Of Entities.TipoSubEntidad)) As String
        Dim lsCadena As String = ""
        If lstOfTipoSubEntidad.Count > 0 Then
            For Each objTipoSubEnt As Entities.TipoSubEntidad In lstOfTipoSubEntidad
                lsCadena &= objTipoSubEnt.IdSubEntidad.ToString() + objTipoSubEnt.IdTipoEntidad.ToString() + ","
            Next

            Return lsCadena.Substring(0, lsCadena.Length - 1)
        End If

        Return ""
    End Function
End Module