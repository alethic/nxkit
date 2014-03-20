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
    <script src="Content/nxkit/nxkit-knockout.js" type="text/javascript"></script>
    <script src="Content/nxkit/nxkit-knockout-semantic.js" type="text/javascript"></script>
    <script src="Content/nxkit/nxkit-knockout-transitions.js" type="text/javascript"></script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <div class="main container">

            <script type="text/javascript">

                function GetGroupColumnLength() {
                    return ko.computed(function () {

                    });
                }

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.TextVisual">
                <span data-bind="text: Properties.Text.Value" />
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.FormVisual">
                <!-- ko with: new NXKit.Web.XForms.Layout.FormLayoutManager($context, $data) -->
                <!-- ko with: new NXKit.Web.XForms.Layout.FormViewModel($context, $parent) -->
                <div class="xforms-layout-form">
                    <!-- ko foreach: Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.PageVisual">
                <div class="xforms-layout-page">
                    <!-- ko foreach: Visuals -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.SectionVisual">
                <div class="xforms-layout-section">
                    <!-- ko foreach: Visuals -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.ParagraphVisual">
                <!-- ko with: new NXKit.Web.VisualViewModel($context, $data) -->
                <div class="xforms-layout-paragraph">
                    <!-- ko foreach: Visual.Visuals -->
                    <!-- ko nxkit_template: $data -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsLabelVisual">
                <!-- ko with: new NXKit.Web.XForms.LabelViewModel($context, $data) -->
                <span class="xforms-label">
                    <!-- ko if: Text -->
                    <span data-bind="text: Text" />
                    <!-- /ko -->

                    <!-- ko ifnot: Text -->
                    <!-- ko foreach: Visual.Visuals -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                    <!-- /ko -->
                </span>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsHelpVisual">
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

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual">
                <!-- ko with: new NXKit.Web.XForms.GroupViewModel($context, $data) -->
                <!-- ko with: new NXKit.Web.XForms.GroupLayoutManager($context, $data) -->

                <!-- ko template: {
                    data: $parent,
                    name: 'NXKit.XForms.GroupViewModel__Level_' + $data.Level + '__Layout_' + $data.Layout,
                } -->
                <!-- /ko -->

                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" id="NXKit.XForms.GroupViewModel__Level_1__Layout_1">
                <div class="xforms-group ui form segment level1 single" data-bind="
    nxkit_visible: Relevant,
    css: {
        relevant: Relevant,
    }">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <h2 class="ui header" data-bind="nxkit_template: {
    visual: $data
}" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui right green corner label">
                        <i class="help icon"></i>
                        <!-- ko nxkit_template -->
                        <!-- /ko -->
                    </div>
                    <!-- /ko -->

                    <!-- ko foreach: Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
            </script>

            <script type="text/html" id="NXKit.XForms.GroupViewModel__Level_2__Layout_1">

                <!-- ko with: Label -->
                <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                <div class="ui red ribbon label" data-bind="
    nxkit_template: $data,
    nxkit_visible: $parent.Relevant,
    css: {
        relevant: $parent.Relevant
    }" />
                <!-- /ko -->
                <!-- /ko -->

                <div class="two fields" data-bind="
    nxkit_visible: Relevant,
    css: {
        relevant: Relevant
    }">

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script type="text/html" id="NXKit.XForms.GroupViewModel__Level_3__Layout_1">
                <div class="xforms-group ui form segment level3 single" data-bind="
    nxkit_visible: Relevant,
    css: {
        relevant: Relevant
    }">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="nxkit_template: $data" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script type="text/html" id="NXKit.XForms.GroupViewModel__Level_4__Layout_1">
                <div class="xforms-group ui form segment level4 single" data-bind="
    nxkit_visible: Relevant,
    css: {
        relevant: Relevant
    }">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached label" data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui float right label" data-bind="nxkit_template: $data" />
                    <!-- /ko -->

                    <!-- ko foreach: $data.Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsRepeatVisual">
                <!-- ko foreach: Visuals -->
                <!-- ko nxkit_template: $data -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsRepeatItemVisual">
                <!-- ko foreach: Visuals -->
                <!-- ko nxkit_template -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual">
                <!-- ko with: new NXKit.Web.XForms.VisualViewModel($context, $data) -->

                <div class="field">

                    <!-- ko if: Label -->
                    <label data-bind="attr: { 'for': UniqueId }">
                        <!-- ko nxkit_template: Label -->
                        <!-- /ko -->
                    </label>
                    <!-- /ko -->

                    <!-- ko nxkit_template: { 
                        visual: Visual,
                        type: Visual.Properties.Type != null && Visual.Properties.Type.ValueAsString() != null ? Visual.Properties.Type.ValueAsString() : 'unknown',
                    } -->
                    <!-- /ko -->
                </div>

                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}string">
                <input type="text" data-bind="value: Properties.Value.Value" />
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}boolean">
                <div class="ui checkbox" data-bind="nxkit_checkbox: Properties.Value.ValueAsBoolean">
                    <input type="checkbox" />
                    <label>stuff</label>
                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}date">
                <input type="date" data-bind="value: Properties.Value.Value" />
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}int">
                <!-- ko nxkit_template: {
                    visual: $data,
                    type: '{http://www.w3.org/2001/XMLSchema}integer',
                } -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}integer">
                <input type="number" data-bind="value: Properties.Value.ValueAsNumber" />
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsRangeVisual">
                <!-- ko with: new NXKit.Web.XForms.VisualViewModel($context, $data) -->

                <div class="field">

                    <!-- ko if: Label -->
                    <label data-bind="attr: { 'for': UniqueId }">
                        <!-- ko nxkit_template: Label -->
                        <!-- /ko -->
                    </label>
                    <!-- /ko -->
                    
                    <!-- ko nxkit_template: { 
                        visual: Visual,
                        type: Visual.Properties.Type != null ? Visual.Properties.Type.Value : 'foo',
                    } -->
                    <!-- /ko -->

                </div>

                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsRangeVisual__unknown">
                <span class="error">Could not locate template.</span>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsRangeVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}int">
                <!-- ko nxkit_template: {
                    visual: $data,
                    type: '{http://www.w3.org/2001/XMLSchema}integer',
                } -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsRangeVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}integer">
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

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsSelect1Visual">
                <!-- ko with: new NXKit.Web.XForms.Select1ViewModel($context, $data) -->

                <div class="field">

                    <!-- ko if: Label -->
                    <label data-bind="attr: { 'for': UniqueId }">
                        <!-- ko nxkit_template: Label -->
                        <!-- /ko -->
                    </label>
                    <!-- /ko -->

                    <!-- ko nxkit_template: { 
                        visual: Visual,
                        data: $data,
                        type: Visual.Properties.Type != null && Visual.Properties.Type.ValueAsString() != null ? Visual.Properties.Type.ValueAsString : 'unknown',
                    } -->
                    <!-- /ko -->
                </div>

                <!-- /ko -->
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

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsSelect1Visual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}string">
                <div class="ui fluid selection dropdown" data-bind="nxkit_dropdown: $data.Visual.Properties['SelectedItemVisualId'].Value">
                    <input type="hidden" />
                    <div class="text">Select</div>
                    <i class="dropdown icon"></i>
                    <div class="ui menu" data-bind="foreach: GetSelect1Items($data.Visual)">
                        <div class="item" data-bind="attr: { 'data-value': $data.Properties['UniqueId'].Value }">
                            <!-- ko nxkit_template: NXKit.Web.XForms.VisualViewModel.GetLabel($data) -->
                            <!-- /ko -->
                        </div>
                    </div>
                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsSelect1Visual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}boolean">
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
