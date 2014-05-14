<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NXKit.Test.Web.Site.Default" %>

<%@ Register Assembly="NXKit.Web.UI" Namespace="NXKit.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="Content/normalize.css" />
    <%--<link rel="stylesheet/less" type="text/css" href="Content/semantic/packaged/css/semantic.css" />--%>
    <link rel="stylesheet/less" type="text/css" href="Content/styles.less" />

    <style type="text/css">
        html, body {
            font-size: 15px;
        }
    </style>

    <script type="text/html"
        data-nxkit-name="{http://www.w3.org/2002/xforms}group"
        data-nxkit-data-type="*">
        <!-- ko with: new NXKit.Web.XForms.GroupViewModel($context, $parent) -->
        <div class="xforms-group"
            data-bind="nxkit_visible: Relevant">
            <!-- ko with: Label -->
            <div class="label">
                <!-- ko nxkit_template: $data -->
                <!-- /ko -->
            </div>
            <!-- /ko -->
            <!-- ko with: Hint -->
            <div class="hint">
                <!-- ko nxkit_template: $data -->
                <!-- /ko -->
            </div>
            <!-- /ko -->
            <!-- ko with: Help -->
            <div class="help">
                <!-- ko nxkit_template: $data -->
                <!-- /ko -->
            </div>
            <!-- /ko -->
            <div class="contents">
                <!-- ko foreach: Contents -->
                <!-- ko nxkit_template -->
                <!-- /ko -->
                <!-- /ko -->
            </div>
        </div>
        <!-- /ko -->
    </script>

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

        <div class="ui fixed transparent main menu">
            <div class="ui item">
                <div class="ui fluid action input">
                    <asp:TextBox ID="UriTextBox" runat="server"></asp:TextBox>
                    <div id="LoadButton" class="ui right icon button" onclick="<%= Page.ClientScript.GetPostBackEventReference(Page, "Load") %>">
                        <i class="download icon"></i>
                        Load
                    </div>
                </div>
            </div>
        </div>

        <div class="main container" style="padding-top: 64px;">
            <xforms:View ID="View" runat="server"
                CssClass="FormView"
                OnLoad="View_Load" />
        </div>
    </form>
</body>

</html>
