<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NXKit.Test.Web.Site.Default" %>

<%@ Register Assembly="NXKit.Web.UI" Namespace="NXKit.Web.UI" TagPrefix="xforms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="Content/normalize.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/semantic/packaged/css/semantic.css" />
    <link rel="stylesheet/less" type="text/css" href="Content/styles.less" />
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="less" />
                <asp:ScriptReference Name="semantic" />
                <asp:ScriptReference Name="knockout" />
            </Scripts>
        </asp:ScriptManager>

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
                <!-- ko with: new NXKit.Web.XForms.HelpViewModel($context, $data) -->
                <div class="xforms-help">
                    <!-- ko if: Text -->
                    <span data-bind="text: Text" />
                    <!-- /ko -->

                    <!-- ko ifnot: Text -->
                    <!-- ko foreach: Visual.Visuals -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsHintVisual">
                <!-- ko with: new NXKit.Web.XForms.HintViewModel($context, $data) -->
                <div class="xforms-hint">
                    <!-- ko if: Text -->
                    <span data-bind="text: Text" />
                    <!-- /ko -->

                    <!-- ko ifnot: Text -->
                    <!-- ko foreach: Visual.Visuals -->
                    <!-- ko nxkit_template -->
                    <!-- /ko -->
                    <!-- /ko -->
                    <!-- /ko -->
                </div>
                <!-- /ko -->
            </script>

            <script type="text/javascript">

                function ShowModal(id) {
                    $('#' + id).modal('show');
                }

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
                <!-- ko with: new NXKit.Web.XForms.InputViewModel($context, $data) -->
                <!-- ko nxkit_template: {
                    visual: Visual,
                    type: Type,
                } -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}string">
                <input type="text" data-bind="value: Properties.Value.ValueAsString" />
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}boolean">
                <div class="ui checkbox" data-bind="nxkit_checkbox: Properties.Value.ValueAsBoolean">
                    <input type="checkbox" />
                    <label>stuff</label>
                </div>
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsInputVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}date">
                <input type="date" data-bind="value: Properties.Value.ValueAsString" />
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
                <!-- ko with: new NXKit.Web.XForms.RangeViewModel($context, $data) -->
                <!-- ko nxkit_template: { 
                    visual: Visual,
                    type: Type,
                } -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsTextAreaVisual">
                <!-- ko with: new NXKit.Web.XForms.TextAreaViewModel($context, $data) -->
                <!-- ko nxkit_template: { 
                    visual: Visual,
                    type: Type,
                } -->
                <!-- /ko -->
                <!-- /ko -->
            </script>

            <script type="text/html" data-nxkit-visual="NXKit.XForms.XFormsTextAreaVisual" data-nxkit-type="{http://www.w3.org/2001/XMLSchema}string">
                <textarea data-bind="value: Properties.Value.ValueAsString" />
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
                    type: Type,
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
                <div class="ui fluid selection dropdown" data-bind="nxkit_dropdown: SelectedItemVisualId">
                    <input type="hidden" />
                    <div class="text">Select</div>
                    <i class="dropdown icon"></i>
                    <div class="ui menu" data-bind="foreach: GetSelect1Items($data.Visual)">
                        <div class="item" data-bind="attr: { 'data-value': $data.Properties['UniqueId'].ValueAsString }">
                            <!-- ko nxkit_template: NXKit.Web.XForms.Utils.GetLabel($data) -->
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
