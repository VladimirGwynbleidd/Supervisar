Imports System.Globalization
Imports System.Threading

Public Class Fechas

    Public Shared Function toDate(ByVal Fecha As String) As DateTime
        Dim cul As CultureInfo = Thread.CurrentThread.CurrentCulture
        Dim Sal As DateTime

        If cul.Name.IndexOf("MX") >= 0 Then
            Sal = CDate(Fecha)
        Else
            Dim format As String = "dd/MM/yyyy"
            Dim provider As CultureInfo = New CultureInfo("es-MX")
            Try
                Sal = DateTime.ParseExact(Fecha, format, provider)
            Catch ex As Exception
                Sal = #1/1/1900#
            End Try
        End If
        Return Sal
   End Function

   Public Shared Function Vacia(ByVal Fecha As Date) As Boolean
      If Fecha = Date.MinValue Or Fecha = #1/1/1900# Then
         Return True
      End If

      Return False
   End Function

   Public Shared Function Valor(ByVal Fecha As Date) As String
      If (Vacia(Fecha)) Then
         Return ""
      End If

      Return Fecha.ToString("dd/MM/yyyy")
   End Function
End Class