/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class Select1ViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        public static GetSelectedId(node: Node): KnockoutComputed<string> {
            return ko.computed<string>({
                read: () => {
                    if (node != null &&
                        node.ValueAsString('NXKit.XForms.Select1', 'SelectedId') != null)
                        return node.ValueAsString('NXKit.XForms.Select1', 'SelectedId')();
                    else
                        return null;
                },
                write: _ => {
                    if (node != null &&
                        node.ValueAsString('NXKit.XForms.Select1', 'SelectedId') != null)
                        node.ValueAsString('NXKit.XForms.Select1', 'SelectedId')(_);
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