Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient
Imports System.Data
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<ToolboxItem(False)> _
Public Class WebService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()> _
      <System.Web.Script.Services.ScriptMethod()> _
    Public Function MensajeInfo(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim lista As String() = {"Hola mundo", "Hola a todos", "Hola cara de bola"}
        Return lista
    End Function

    <WebMethod()> _
      <System.Web.Script.Services.ScriptMethod()> _
    Public Function ObtenerNiveles(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim lista As List(Of String) = New Entities.NivelServicio().ObtenerLista(prefixText, count.ToString)
        Return lista
    End Function

End Class