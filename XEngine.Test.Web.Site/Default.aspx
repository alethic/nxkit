﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XEngine.Test.Web.Site.Default" %>
<%@ Register Assembly="XEngine.Forms.Web.UI" Namespace="XEngine.Forms.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <div>
            <xforms:FormView runat="server" />
        </div>
    </form>
</body>
</html>
