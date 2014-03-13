<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NXKit.Test.Web.Site.Default" %>

<%@ Register Assembly="NXKit.Web.UI" Namespace="NXKit.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Default.css" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="jquery" />
            </Scripts>
        </asp:ScriptManager>
        <xforms:View ID="View" runat="server"
            CssClass="FormView"
            OnLoad="View_Load"
            OnResourceAction="View_ResourceAction" />
        <asp:Button ID="PrevButton" runat="server"
            Text="Previous"
            CausesValidation="false"
            OnClick="PrevButton_Click" />
        <asp:Button ID="NextButton" runat="server"
            Text="Next"
            CausesValidation="true"
            OnClick="NextButton_Click" />
    </form>
</body>

</html>
