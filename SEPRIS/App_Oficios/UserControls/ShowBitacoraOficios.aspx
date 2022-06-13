<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowBitacoraOficios.aspx.vb"
    Inherits="SICOD.ShowBitacoraOficios" %>

<%@ Register Src="ucShowBitacoraOficios.ascx" TagPrefix="ucSB" TagName="ShowBitacora" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Historial</title>
    <%--<link href="../Styles.css" type="text/css" rel="Stylesheet" />--%>
    <link href="../../Styles.css" type="text/css"  rel="stylesheet" />
    <script type="text/javascript" language="javascript">

        function Cierrame() {

            window.close();

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scrManager" runat="server">
        </asp:ScriptManager>
        <ucSB:ShowBitacora ID="ctrShowBitacora" runat="server" />
    </div>
    </form>
</body>
</html>
