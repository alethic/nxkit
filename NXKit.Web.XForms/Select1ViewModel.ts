/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class Select1ViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        public static GetSelectedId(node: Node): KnockoutComputed<string> {
            return ko.computed<string>({
                read: () => {
                    if (node != null &&
                        node.Property('NXKit.XForms.Select1', 'SelectedId') != null)
                        return node.Property('NXKit.XForms.Select1', 'SelectedId').ValueAsString();
                    else
                        return null;
                },
                write: _ => {
                    if (node != null &&
                        node.Property('NXKit.XForms.Select1', 'SelectedId') != null)
                        node.Property('NXKit.XForms.Select1', 'SelectedId').ValueAsString(_);
                },
            });
        }

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Selectables(): SelectUtil.Selectable[] {
            return SelectUtil.GetSelectables(this, this.Node, 1);
        }

        public get SelectedValue(): KnockoutComputed<string> {
            throw new Error('NotImplemented');
        }

        public get SelectedId(): KnockoutComputed<string> {
            return Select1ViewModel.GetSelectedId(this.Node);
        }

    }

}