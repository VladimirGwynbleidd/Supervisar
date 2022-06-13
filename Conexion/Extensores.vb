Imports System.Runtime.CompilerServices
Imports System.Data.SqlClient
Imports System.Reflection

Module Extensores
    <Extension()>
    Public Function GetParameters(Of T)(ByVal cmd As SqlClient.SqlCommand, ByVal item As T) As System.Data.Common.DbParameterCollection
        Dim obj As T
        obj = Activator.CreateInstance(Of T)()

        SqlCommandBuilder.DeriveParameters(cmd)
        For Each p As SqlParameter In cmd.Parameters
            If p.ParameterName = String.Format("{0}", "@RETURN_VALUE") Then
                Continue For
            End If

            For Each prop As PropertyInfo In obj.GetType().GetProperties()
                If ("@" + prop.Name) = p.ParameterName Then
                    p.SqlValue = prop.GetValue(item, Nothing)
                    Exit For
                End If
            Next
        Next

        Return cmd.Parameters
    End Function
End Module