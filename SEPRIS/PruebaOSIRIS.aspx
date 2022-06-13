<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PruebaOSIRIS.aspx.vb" Inherits="SEPRIS.PruebaOSIRIS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button1" runat="server" Text="Button" />
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true">
        </asp:GridView>
    </form>
</body>
</html>
