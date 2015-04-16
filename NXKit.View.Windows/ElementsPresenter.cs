using System.Windows;
using System.Windows.Controls;

namespace NXKit.View.Windows
{

    public class ElementsPresenter :
        ItemsControl
    {

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ElementContainer;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ElementContainer();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var model = (ElementContainer)element;
            if (model.ContentTemplate == null && ItemTemplate != null)
                model.ContentTemplate = ItemTemplate;
            if (model.ContentTemplateSelector == null && ItemTemplateSelector != null)
                model.ContentTemplateSelector = ItemTemplateSelector;
            if (model.Style == null && ItemContainerStyle != null)
                model.Style = ItemContainerStyle;

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            var model = (ElementContainer)element;
            model.ContentTemplate = null;
            model.ContentTemplateSelector = null;
            model.Style = null;
            base.ClearContainerForItemOverride(element, item);
        }

    }

}
