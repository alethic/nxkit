namespace NXKit.XForms.Layout
{

    [Visual("table")]
    public class TableVisual : 
        XFormsGroupVisual,
        ITableColumnGroupContainer
    {

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
