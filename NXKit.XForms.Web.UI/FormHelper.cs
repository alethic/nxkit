using System;
using System.Text;
using System.Web.UI;
using System.Linq;

using NXKit.XForms;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Provides common methods for form parts.
    /// </summary>
    public static class FormHelper
    {

        /// <summary>
        /// Converts the control's label to a string.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static string ControlLabelToString(XFormsVisual visual)
        {
            var labelVisual = visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (labelVisual != null)
                return LabelToString(labelVisual);
            else
                return null;
        }

        public static string LabelToString(XFormsLabelVisual visual)
        {
            var builder = new StringBuilder();
            LabelToText(builder, visual);
            return builder.ToString().Trim();
        }

        public static string AlertToString(XFormsAlertVisual visual)
        {
            var builder = new StringBuilder();
            AlertToText(builder, visual);
            return builder.ToString().Trim();
        }

        public static string OutputToString(XFormsOutputVisual visual)
        {
            var builder = new StringBuilder();
            OutputToText(builder, visual);
            return builder.ToString().Trim();
        }

        public static string TextToString(TextVisual visual)
        {
            var builder = new StringBuilder();
            TextToText(builder, visual);
            return builder.ToString().Trim();
        }

        public static void InlineVisualChildrenToText(StringBuilder builder, StructuralVisual visual)
        {
            foreach (var child in visual.Children)
                InlineVisualToText(builder, child);
        }

        public static void InlineVisualToText(StringBuilder builder, Visual visual)
        {
            if (visual is XFormsLabelVisual)
                LabelToText(builder, (XFormsLabelVisual)visual);
            else if (visual is XFormsOutputVisual)
                OutputToText(builder, (XFormsOutputVisual)visual);
            else if (visual is TextVisual)
                TextToText(builder, (TextVisual)visual);
            else
                throw new InvalidOperationException();
        }

        public static void LabelToText(StringBuilder builder, XFormsLabelVisual visual)
        {
            if (visual.Binding != null)
                builder.Append(visual.Binding.Value ?? "");
            else
                InlineVisualChildrenToText(builder, visual);
        }

        public static void AlertToText(StringBuilder builder, XFormsAlertVisual visual)
        {
            if (visual.Binding != null)
                builder.Append(visual.Binding.Value ?? "");
            else
                InlineVisualChildrenToText(builder, visual);
        }

        public static void OutputToText(StringBuilder builder, XFormsOutputVisual visual)
        {
            builder.Append((visual.Binding != null ? visual.Binding.Value : null) ?? "");
        }

        public static void TextToText(StringBuilder builder, TextVisual visual)
        {
            builder.Append(visual.Text);
        }

        /// <summary>
        /// Renders all children of <paramref name="visual"/> by type.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="visual"></param>
        public static void RenderInlineVisualChildren(HtmlTextWriter writer, StructuralVisual visual)
        {
            foreach (var child in visual.Children)
                RenderInlineVisual(writer, child);
        }

        /// <summary>
        /// Renders the given inline visual.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="visual"></param>
        public static void RenderInlineVisual(HtmlTextWriter writer, Visual visual)
        {
            if (visual is XFormsLabelVisual)
                RenderLabel(writer, (XFormsLabelVisual)visual);
            else if (visual is XFormsOutputVisual)
                RenderOutput(writer, (XFormsOutputVisual)visual);
            else if (visual is TextVisual)
                RenderText(writer, (TextVisual)visual);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Renders a <see cref="XFormsLabelVisual"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="visual"></param>
        public static void RenderLabel(HtmlTextWriter writer, XFormsLabelVisual visual)
        {
            if (visual.Binding != null)
                writer.WriteEncodedText(visual.Binding.Value ?? "");
            else
                RenderInlineVisualChildren(writer, visual);
        }

        /// <summary>
        /// Renders a <see cref="XFormsOutputVisual"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="visual"></param>
        public static void RenderOutput(HtmlTextWriter writer, XFormsOutputVisual visual)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.WriteEncodedText((visual.Binding != null ? visual.Binding.Value : null) ?? "");
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders a simple <see cref="TextVisual"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="visual"></param>
        public static void RenderText(HtmlTextWriter writer, TextVisual visual)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.WriteEncodedText(visual.Text);
            writer.RenderEndTag();
        }

    }

}
