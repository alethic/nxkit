<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NXKit.Test.Web.Site.Default" %>

<%@ Register Assembly="NXKit.Web.UI" Namespace="NXKit.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="Content/normalize.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/semantic/packaged/css/semantic.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/styles.less" />

    <style type="text/css">
        html, body {
            font-size: 15px;
        }

        .ui.checkbox label {
            margin-bottom: 16px !important;
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="jquery.ui.combined" />
                <asp:ScriptReference Name="less" />
                <asp:ScriptReference Name="semantic" />
                <asp:ScriptReference Name="knockout" />
            </Scripts>
        </asp:ScriptManager>

        <div class="main container">

            <div class="ui segment">
                <div class="ui fluid action input">
                    <asp:TextBox ID="UriTextBox" runat="server" />
                    <button ID="LoadButton" runat="server" onserverclick="LoadButton_Click" class="ui right labeled button">Load</button>
                </div>
            </div>

            <xforms:View ID="View" runat="server"
                CssClass="FormView"
                OnLoad="View_Load" />

        </div>
    </form>
</body>

</html>
