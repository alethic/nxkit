/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class Select1ViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        public static GetSelectedItemNodeId(node: Node): KnockoutComputed<string> {
            return ko.computed<string>({
                read: () => {
                    if (node != null &&
                        node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId') != null)
                        return node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId')();
                    else
                        return null;
                },
                write: _ => {
                    if (node != null &&
                        node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId') != null)
                        node.ValueAsString('NXKit.XForms.Select1', 'SelectedItemNodeId')(_);
                },
            });
        }

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Items(): SelectUtil.Item[] {
            return SelectUtil.GetItems(this, this.Node, 1);
        }

        public get SelectedItemNodeId(): KnockoutComputed<string> {
            return Select1ViewModel.GetSelectedItemNodeId(this.Node);
        }

    }

}