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
    <script src="Content/knockout/knockout-projections.js" type="text/javascript"></script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <div class="main container">

            <script type="text/javascript">

                function TypeToTemplate(prefix, type) {
                    return prefix + "__" + (type != null ? type.replace(/[{}:/]/g, '_') : "unknown");
                }

                function GetUniqueId(visual) {
                    return visual != null ? visual.Properties.UniqueId : null;
                }

                function GetAppearance(visual) {
                    return ko.computed(function () {
                        if (visual != null &&
                            visual.Properties.Appearance != null &&
                            visual.Properties.Appearance.Value() != null)
                            return visual.Properties.Appearance.Value();
                        else
                            return "full";
                    });
                }

                function GetGroupColumnLength() {
                    return ko.computed(function () {

                    });
                }

            </script>

            <script id="NXKit.TextVisual" type="text/html">
                <span data-bind="text: Properties.Text.Value" />
            </script>

            <script id="NXKit.XForms.Layout.ParagraphVisual" type="text/html">
                <div class="xforms-layout-paragraph">
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: { name: Template } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.Layout.FormVisual" type="text/html">
                <div class="xforms-layout-form">
                    <h1 data-bind="text: Type"></h1>
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: { name: Template } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.XFormsLabelVisual" type="text/html">
                <span class="xforms-label">
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: { name: Template } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </span>
            </script>

            <script id="NXKit.XForms.XFormsHelpVisual" type="text/html">
                <span class="xforms-help">
                    <i class="help icon" />
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: { name: Template } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </span>
            </script>

            <script id="NXKit.XForms.XFormsGroupVisual" type="text/html">
                <!-- ko with: new NXKit.Web.XForms.GroupViewModel($context, $data) -->
                <div class="xforms-group ui segment" data-bind="
    fadeVisible: $data.Visual.Properties.Relevant.ValueAsBoolean,
    css: {
        form: NXKit.Web.XForms.VisualViewModel.HasControlVisual($data.Visual),
    }">
                    <!-- ko if: $data.Label -->
                    <!-- ko if: GetAppearance($data.Label)() == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data.Label,
        name: $data.Label.Template
    }" />
                    <!-- ko if: $data.Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data.Help,
        name: $data.Help.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->
                    <!-- /ko -->
                    <!-- ko foreach: Contents -->
                    <!-- ko template: { name: Template } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsRepeatVisual" type="text/html">
                <!-- ko foreach: Visuals -->
                <!-- ko template: { name: Template } -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsRepeatItemVisual" type="text/html">
                <div class="xforms-repeat-item ui segment">
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: { name: Template } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.XFormsInputVisual" type="text/html">
                <!-- ko if: NXKit.Web.XForms.VisualViewModel.GetLabel($data) -->
                <label data-bind="attr: { 'for': GetUniqueId($data) }">
                    <!-- ko template: { data: $data, name: NXKit.Web.XForms.VisualViewModel.GetLabel($data).Template } -->
                    <!-- /ko -->
                </label>
                <!-- /ko -->
                <!-- ko template: { name: TypeToTemplate('NXKit.XForms.XFormsInputVisual', Properties.Type != null ? Properties.Type.Value() : null) } -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsInputVisual__unknown" type="text/html">
                <span class="error">Could not locate template.</span>
            </script>

            <script id="NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_string" type="text/html">
                <input type="text" data-bind="value: Properties.Value.Value" />
            </script>

            <script id="NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_boolean" type="text/html">
                <div class="ui checkbox">
                    <label>stuff</label>
                    <input type="checkbox" data-bind="checked: Properties.Value.ValueAsBoolean" />
                </div>
            </script>

            <script type="text/javascript">

                ko.bindingHandlers.fadeVisible = {
                    init: function (element, valueAccessor) {
                        var value = valueAccessor();
                        $(element).toggle(ko.utils.unwrapObservable(value));
                    },
                    update: function (element, valueAccessor) {
                        var value = valueAccessor();
                        ko.utils.unwrapObservable(value) ? $(element).fadeIn() : $(element).fadeOut();
                    }
                };

                //$(document).ready(function () {
                //    $(document).bind('DOMNodeInserted', function (event) {
                //        $(event.target)
                //            .filter('.ui.checkbox')
                //            .checkbox();
                //    });

                //    $('.ui.checkbox')
                //        .checkbox();
                //});
            </script>

            <script id="NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_date" type="text/html">
                <input type="date" data-bind="value: Properties.Value.Value" />
            </script>

            <script id="NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_int" type="text/html">
                <!-- ko template: { name: 'NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_integer' } -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_integer" type="text/html">
                <input type="number" data-bind="value: Properties.Value.ValueAsNumber" />
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual" type="text/html">
                <!-- ko template: { name: TypeToTemplate('NXKit.XForms.XFormsRangeVisual', Properties.Type.Value()) } -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual__unknown" type="text/html">
                <span class="error">Could not locate template.</span>
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual___http___www.w3.org_2001_XMLSchema_int" type="text/html">
                <!-- ko template: { name: 'NXKit.XForms.XFormsRangeVisual___http___www.w3.org_2001_XMLSchema_integer' } -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual___http___www.w3.org_2001_XMLSchema_integer" type="text/html">
                <input
                    type="number"
                    data-bind="
    value: Properties.Value.ValueAsNumber,
    attr: {
        min: Properties.Start.ValueAsNumber,
        max: Properties.End.ValueAsNumber,
        step: Properties.Step.ValueAsNumber
    }" />
            </script>

            <script id="NXKit.XForms.XFormsSelect1Visual" type="text/html">
                <!-- ko template: { name: TypeToTemplate('NXKit.XForms.XFormsSelect1Visual', Properties.Type != null ? Properties.Type.Value() : null) } -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsSelect1Visual__unknown" type="text/html">
                <span class="error">Could not locate template.</span>
            </script>

            <script type="text/javascript">

                function GetSelect1Items(visual) {
                    return ko.utils.arrayFilter(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsItemVisual';
                    });
                }

                function GetItemValueVisual(visual) {
                    return ko.utils.arrayFirst(visual.Visuals(), function (_) {
                        return _.Type == 'NXKit.XForms.XFormsValueVisual';
                    });
                }

            </script>

            <script id="NXKit.XForms.XFormsSelect1Visual___http___www.w3.org_2001_XMLSchema_string" type="text/html">
                <select data-bind="
    value: Properties.SelectedItemVisualId.Value,
    foreach: GetSelect1Items($data)">
                    <option data-bind="
    value: Properties.UniqueId.Value,
    template: {
        data: NXKit.Web.XForms.VisualViewModel.GetLabel($data),
        name: NXKit.Web.XForms.VisualViewModel.GetLabel($data).Template
    }" />
                </select>
            </script>

            <script id="NXKit.XForms.XFormsSelect1Visual___http___www.w3.org_2001_XMLSchema_boolean" type="text/html">
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
