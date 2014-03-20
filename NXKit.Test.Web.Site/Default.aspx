<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NXKit.Test.Web.Site.Default" %>

<%@ Register Assembly="NXKit.Web.UI" Namespace="NXKit.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="Content/normalize.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/semantic/packaged/css/semantic.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/styles.less" />

    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.0/jquery.js" type="text/javascript"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/less.js/1.7.0/less.js" type="text/javascript"></script>
    <script src="Content/semantic/packaged/javascript/semantic.js" type="text/javascript"></script>
    <script src="Content/knockout/knockout.js" type="text/javascript"></script>
    <script src="Content/knockout/knockout-projections.js" type="text/javascript"></script>
    <script src="Content/knockout/knockout-semantic.js" type="text/javascript"></script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <div class="main container">

            <script type="text/javascript">

                function TypeToTemplate(prefix, type) {
                    return prefix + "__" + (type != null ? type.replace(/[{}:/]/g, '_') : "unknown");
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
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.Layout.FormVisual" type="text/html">
                <div class="xforms-layout-form">
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.Layout.PageVisual" type="text/html">
                <div class="xforms-layout-page">
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.Layout.SectionVisual" type="text/html">
                <div class="xforms-layout-section">
                    <!-- ko foreach: Visuals -->
                    <!-- ko template: {
                        data: $data, 
                        name: $data.Template 
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script id="NXKit.XForms.XFormsLabelVisual" type="text/html">
                <!-- ko with: new NXKit.Web.XForms.LabelViewModel($context, $data) -->
                <span class="xforms-label">
                    <!-- ko if: Text -->
                    <span data-bind="text: Text" />
                    <!-- /ko -->

                    <!-- ko ifnot: Text -->
                    <!-- ko foreach: Visual.Visuals -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->
                    <!-- /ko -->
                </span>
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsHelpVisual" type="text/html">
                <!-- ko with: new NXKit.Web.VisualViewModel($context, $data) -->
                <div class="ui modal" data-bind="attr: {
    id: $data.UniqueId
}">
                    <i class="close icon"></i>
                    <div class="header">
                        title
                    </div>
                    <div class="content">
                        stuff
                    </div>
                    <div class="actions">
                        <div class="ui button">Close</div>
                    </div>
                </div>
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsGroupVisual" type="text/html">
                <!-- ko with: new NXKit.Web.XForms.GroupViewModel($context, $data) -->
                <!-- ko if: $data.Relevant -->

                <!-- ko with: new NXKit.Web.XForms.GroupLayoutManager($context, $data.Visual) -->
                <!-- ko template: {
                    data: $parent,
                    name: 'NXKit.XForms.GroupViewModel__Level_' + $data.Level + '__Layout_' + $data.Layout,
                } -->
                <!-- /ko -->
                <!-- /ko -->

                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_1__Layout_1" type="text/html">
                <div class="xforms-group ui form segment level1 single">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <h2 class="ui header" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui right green corner label">
                        <i class="help icon"></i>
                        <!-- ko template: {
                            data: $data,
                            name: 'NXKit.XForms.XFormsHelpVisual',
                        } -->
                        <!-- /ko -->
                    </div>
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_1__Layout_2" type="text/html">
                <div class="xforms-group ui form segment level1 double">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_2__Layout_1" type="text/html">

                <!-- ko with: Label -->
                <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                <div class="ui red ribbon label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                <!-- /ko -->
                <!-- /ko -->

                <div class="two fields">

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_2__Layout_2" type="text/html">
                <div class="xforms-group ui form segment level2 double">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_2__Layout_4" type="text/html">
                <div class="xforms-group ui form segment level2 fluid">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_3__Layout_1" type="text/html">
                <div class="xforms-group ui form segment level3 single">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_4__Layout_1" type="text/html">
                <div class="xforms-group ui form segment level4 single">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.GroupViewModel__Level_4__Layout_4" type="text/html">
                <div class="xforms-group ui form segment level4 fluid">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="
    template: {
        data: $data,
        name: $data.Template
    }" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko template: {
                        data: $data,
                        name: $data.Template,
                    } -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script id="NXKit.XForms.XFormsRepeatVisual" type="text/html">
                <!-- ko foreach: Visuals -->
                <!-- ko template: { 
                    data: $data, 
                    name: $data.Template 
                } -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsRepeatItemVisual" type="text/html">
                <!-- ko foreach: Visuals -->
                <!-- ko template: { 
                    data: $data, 
                    name: $data.Template 
                } -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsInputVisual" type="text/html">
                <!-- ko with: new NXKit.Web.XForms.VisualViewModel($context, $data) -->

                <div class="field">

                    <!-- ko if: Label -->
                    <label data-bind="attr: { 'for': UniqueId }">
                        <!-- ko template: { 
                            data: Label, 
                            name: Label.Template 
                        } -->
                        <!-- /ko -->
                    </label>
                    <!-- /ko -->

                    <!-- ko template: { 
                        data: Visual,
                        name: TypeToTemplate('NXKit.XForms.XFormsInputVisual', Visual.Properties.Type != null ? Visual.Properties.Type.Value() : null),
                    } -->
                    <!-- /ko -->

                </div>

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
                <!-- ko template: { 
                    name: 'NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_integer' 
                } -->
                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsInputVisual___http___www.w3.org_2001_XMLSchema_integer" type="text/html">
                <input type="number" data-bind="value: Properties.Value.ValueAsNumber" />
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual" type="text/html">
                <!-- ko with: new NXKit.Web.XForms.VisualViewModel($context, $data) -->

                <div class="field">

                    <!-- ko if: Label -->
                    <label data-bind="attr: { 'for': UniqueId }">
                        <!-- ko template: { 
                            data: Label, 
                            name: Label.Template 
                        } -->
                        <!-- /ko -->
                    </label>
                    <!-- /ko -->

                    <!-- ko template: { 
                        data: Visual,
                        name: TypeToTemplate('NXKit.XForms.XFormsRangeVisual', Visual.Properties.Type.Value()) 
                    } -->
                    <!-- /ko -->

                </div>

                <!-- /ko -->
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual__unknown" type="text/html">
                <span class="error">Could not locate template.</span>
            </script>

            <script id="NXKit.XForms.XFormsRangeVisual___http___www.w3.org_2001_XMLSchema_int" type="text/html">
                <!-- ko template: { 
                    name: 'NXKit.XForms.XFormsRangeVisual___http___www.w3.org_2001_XMLSchema_integer' 
                } -->
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
                <!-- ko with: new NXKit.Web.XForms.Select1ViewModel($context, $data) -->

                <div class="field">

                    <!-- ko if: Label -->
                    <label data-bind="attr: { 'for': UniqueId }">
                        <!-- ko template: { 
                            data: Label, 
                            name: Label.Template 
                        } -->
                        <!-- /ko -->
                    </label>
                    <!-- /ko -->

                    <!-- ko template: { 
                        data: $data,
                        name: TypeToTemplate('NXKit.XForms.XFormsSelect1Visual', $data.Visual.Properties.Type != null ? $data.Visual.Properties.Type.Value() : null),
                    } -->
                    <!-- /ko -->

                </div>

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
                <div class="ui fluid selection dropdown" data-bind="dropdown: $data.Visual.Properties['SelectedItemVisualId'].Value">
                    <input type="hidden" />
                    <div class="text">Select</div>
                    <i class="dropdown icon"></i>
                    <div class="ui menu" data-bind="foreach: GetSelect1Items($data.Visual)">
                        <div class="item" data-bind="attr: { 'data-value': $data.Properties['UniqueId'].Value }">
                            <!-- ko template: {
                                data: NXKit.Web.XForms.VisualViewModel.GetLabel($data),
                                name: NXKit.Web.XForms.VisualViewModel.GetLabel($data).Template
                            } -->
                            <!-- /ko -->
                        </div>
                    </div>
                </div>
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
