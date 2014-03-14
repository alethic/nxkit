<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NXKit.Test.Web.Site.Default" %>

<%@ Register Assembly="NXKit.Web.UI" Namespace="NXKit.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="Content/normalize.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/styles.less" />

    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.0/jquery.js" type="text/javascript"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/less.js/1.7.0/less.js" type="text/javascript"></script>
    <script src="Content/semantic/packaged/javascript/semantic.js" type="text/javascript"></script>
    <script src="Content/knockout/knockout.js" type="text/javascript"></script>
    <script src="Content/kendoui/js/kendo.all.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var init = function () {
            kendo.init($('body'));

            $('.xforms-layout-section')
                .addClass('xforms-group');

            $('.xforms-group')
                .addClass('ui segment green');
            $('.xforms-layout-section')
                .removeClass('green')
                .addClass('blue');
            $('.xforms-group > .xforms-label')
                .addClass('ui top attached label');

            $('.xforms-input, .xforms-range, .xforms-select1')
                .addClass('ui input');

            $('.xforms-input input[type=checkbox]')
                .parent('.xforms-input')
                .addClass('ui checkbox');

            $('.xforms-group')
                .children('.xforms-input, .xforms-range, .xforms-select1')
                .parent()
                .addClass('ui form');

            $('.xforms-group')
                .children('.xforms-input, .xforms-range, .xforms-select1')
                .removeClass('ui input')
                .addClass('field');

            $('.xforms-group > .xforms-group.ui.form')
                .filter(function (i) {
                    return $('.xforms-input, .xforms-range, .xforms-select1', this).length === 2;
                })
                .removeClass('form')
                .addClass('ui two fields');

            $('.xforms-group > .xforms-group.ui.form')
                .filter(function (i) {
                    return $('.xforms-input, .xforms-range, .xforms-select1', this).length === 3;
                })
                .removeClass('form')
                .addClass('ui three fields');
        };

        $().ready(init);
        $().ready(function () { Sys.WebForms.PageRequestManager.getInstance().add_endRequest(init) });
        
        $().ready(function () {
            var page = Sys.WebForms.PageRequestManager.getInstance();

            page.add_beginRequest(function (sender, args) {

            });

            page.add_endRequest(function (sender, args) {

            });

            page.add_pageLoaded(function (sender, args) {

            });

            page.beginAsyncPostBack(null, 'TARGET_STUFF', 'ARGUMENT', false);
        });

    </script>
</head>

<body>
<form id="form1" runat="server">
<asp:ScriptManager runat="server" />

<div class="main container">

    <script id="NXKit.Visual" type="text/html">
        <p>no template for <span data-bind="text: type"></span>.</p>
    </script>

    <script id="NXKit.XForms.Layout.FormVisual" type="text/html">
        <div class="xforms-layout-form">
            <h1 data-bind="text: type"></h1>
            <div data-bind="foreach: visuals">
                <div data-bind="template : { name: template }" />
            </div>
        </div>
    </script>

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

</div>
</form>
</body>

</html>
