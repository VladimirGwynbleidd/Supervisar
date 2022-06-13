<%@ Page Language="vb" AutoEventWireup="false" enableEventValidation="true" CodeBehind="NuevaIrregularidad.aspx.vb" Inherits="SEPRIS.NuevaIrregularidad" %>

<%@ Register src="UserControls/Irregularidad.ascx" tagname="Irregularidad" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/> 
    <title>Nueva irregularidad</title>
    <link href="/Styles/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap-3.3.4/js/bootstrap.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>
    <script src="/Scripts/MensajeModal.js" type="text/javascript"></script>
</head>
<body>
    <div>
        <form runat="server">
        <asp:ScriptManager runat="server" ID="SM" EnableScriptGlobalization="true" EnablePageMethods="true">
        </asp:ScriptManager>

        <uc1:Irregularidad ID="Irregularidad1" runat="server" />
            </form>
    </div>
</body>
</html>

<script>
        //window.onunload = refreshParent;

        //function refreshParent() {
        //    window.opener.upnlConsulta.reload();
        //}
    </script>



