namespace NXKit.XForms.Layout
{

    [Visual("table")]
    public class TableVisual : 
        LayoutVisual,
        ITableColumnGroupContainer
    {

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
