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

            <script type="text/html" data-nxkit-visual="NXKit.TextVisual">
                <span data-bind="text: Properties.Text.Value" />
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.FormVisual">
                <!-- ko with: new NXKit.Web.XForms.Layout.FormViewModel($context, $data) -->
                <div class="xforms-layout-form">
                    <!-- ko foreach: Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.PageVisual">
                <!-- ko with: new NXKit.Web.XForms.Layout.PageViewModel($context, $data) -->
                <div class="xforms-layout-page">
                    <!-- ko foreach: Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.SectionVisual">
                <!-- ko with: new NXKit.Web.XForms.Layout.SectionViewModel($context, $data) -->
                <div class="xforms-layout-section">
                    <!-- ko foreach: Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.Layout.ParagraphVisual">
                <!-- ko with: new NXKit.Web.XForms.Layout.ParagraphViewModel($context, $data) -->
                <div class="xforms-layout-paragraph">
                    <!-- ko foreach: Contents -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsModelVisual">
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

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsHintVisual">
                <!-- ko with: new NXKit.Web.XForms.HintViewModel($context, $data) -->
                <span class="xforms-hint">
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

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual">
                <!-- ko with: new NXKit.Web.XForms.GroupLayoutManager($context) -->
                <!-- ko with: new NXKit.Web.XForms.GroupViewModel($context, $parent) -->
                <div class="xforms-group ui form segment" data-bind="nxkit_visible: Relevant, css: { attached: Help }">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <h2 class="ui header" data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko with: Help -->
                    <div class="ui right green corner label">
                        <i class="link help icon"></i>
                        <!-- ko nxkit_template -->
                        <!-- /ko -->
                    </div>
                    <!-- /ko -->

                    <!-- ko with: Hint -->
                    <div class="ui small blue message">
                        <div class="content">
                            <!-- ko nxkit_template -->
                            <!-- /ko -->
                        </div>
                    </div>
                    <!-- /ko -->

                    <!-- ko foreach: BindingContents -->
                    <!-- ko nxkit_template: Layout -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>

                <!-- ko with: Help -->
                <div class="ui bottom attached info message">
                    <i class="link help icon"></i>
                    Are you sure you know what you're doing?
                </div>
                <!-- /ko -->

                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="1" data-nxkit-layout="single">

                <div class="field" data-bind="nxkit_visible: Relevant">
                    <!-- ko with: Item -->

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <label data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko nxkit_template: Layout -->
                    <!-- /ko -->

                    <!-- /ko -->
                </div>

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="1" data-nxkit-layout="double">

                <div class="two fields" data-bind="nxkit_visible: Relevant">

                    <!-- ko with: Item1 -->
                    <div class="field" data-bind="nxkit_visible: Relevant">

                        <!-- ko with: Label -->
                        <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                        <label data-bind="nxkit_template: $data" />
                        <!-- /ko -->
                        <!-- /ko -->

                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->

                    </div>
                    <!-- /ko -->

                    <!-- ko with: Item2 -->
                    <div class="field" data-bind="nxkit_visible: Relevant">

                        <!-- ko with: Label -->
                        <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                        <label data-bind="nxkit_template: $data" />
                        <!-- /ko -->
                        <!-- /ko -->

                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->

                    </div>
                    <!-- /ko -->

                </div>

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="1" data-nxkit-layout="group">

                <div class="xforms-group secondary" data-bind="nxkit_visible: Relevant">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui blue ribbon label" data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko foreach: Items -->
                    <!-- ko nxkit_template: Layout -->
                    <!-- /ko -->
                    <!-- /ko -->

                </div>

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="1" data-nxkit-layout="visual">
                <!-- ko nxkit_template: ItemVisual -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="1" data-nxkit-layout="input">

                <!-- ko nxkit_template: ItemVisual -->
                <!-- /ko -->

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="2" data-nxkit-layout="single">

                <!-- ko with: Item -->
                <div class="field">

                    <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <label data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->

                    <!-- ko nxkit_template: Layout -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="2" data-nxkit-layout="double">

                <div class="two fields">

                    <!-- ko with: Item1 -->
                    <div class="field">

                        <!-- ko with: Label -->
                        <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                        <label data-bind="nxkit_template: $data" />
                        <!-- /ko -->
                        <!-- /ko -->

                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->

                    </div>
                    <!-- /ko -->

                    <!-- ko with: Item2 -->
                    <div class="field">

                        <!-- ko with: Label -->
                        <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                        <label data-bind="nxkit_template: $data" />
                        <!-- /ko -->
                        <!-- /ko -->

                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->

                    </div>
                    <!-- /ko -->

                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="2" data-nxkit-layout="group">

                <div class="xforms-group" data-bind="nxkit_visible: Relevant">

                   <%-- <!-- ko with: Label -->
                    <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                    <div class="ui top attached header" data-bind="nxkit_template: $data" />
                    <!-- /ko -->
                    <!-- /ko -->--%>

                    <div class="ui secondary segment" data-bind="css: { attached: Label }">

                        <!-- ko foreach: Items -->
                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->
                        <!-- /ko -->

                    </div>

                </div>

            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="2" data-nxkit-layout="visual">
                <!-- ko nxkit_template: ItemVisual -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="2" data-nxkit-layout="input">
                <!-- ko nxkit_template: ItemVisual -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="3" data-nxkit-layout="double">

                <div class="two fields">

                    <!-- ko with: Item1 -->
                    <div class="field">

                        <!-- ko with: Label -->
                        <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                        <label data-bind="nxkit_template: $data" />
                        <!-- /ko -->
                        <!-- /ko -->

                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->

                    </div>
                    <!-- /ko -->

                    <!-- ko with: Item2 -->
                    <div class="field">

                        <!-- ko with: Label -->
                        <!-- ko if: ($data.Appearance || 'full') == 'full' -->
                        <label data-bind="nxkit_template: $data" />
                        <!-- /ko -->
                        <!-- /ko -->

                        <!-- ko nxkit_template: Layout -->
                        <!-- /ko -->

                    </div>
                    <!-- /ko -->

                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="3" data-nxkit-layout="visual">
                <!-- ko nxkit_template: ItemVisual -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsGroupVisual" data-nxkit-level="3" data-nxkit-layout="input">
                <!-- ko nxkit_template: ItemVisual -->
                <!-- /ko -->
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
                <!-- ko nxkit_template: { 
                    visual: Visual,
                    type: Visual.Properties.Type != null && Visual.Properties.Type.ValueAsString() != null ? Visual.Properties.Type.ValueAsString() : 'unknown',
                } -->
                <!-- /ko -->
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
                <!-- ko nxkit_template: { 
                    visual: Visual,
                    type: Visual.Properties.Type != null ? Visual.Properties.Type.Value : 'foo',
                } -->
                <!-- /ko -->
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
                <!-- ko nxkit_template: { 
                    visual: Visual,
                    data: $data,
                    type: Visual.Properties.Type != null && Visual.Properties.Type.ValueAsString() != null ? Visual.Properties.Type.ValueAsString : 'unknown',
                } -->
                <!-- /ko -->
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
