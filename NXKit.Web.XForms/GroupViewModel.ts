/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class GroupViewModel
        extends XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Items(): GroupUtil.Item[] {
            return this.GetItems();
        }

        GetItems(): GroupUtil.Item[] {
            try {
                return GroupUtil.GetItems(this, this.Node, 1);
            } catch (ex) {
                ex.message = 'GroupViewModel.GetItems()' + '"\nMessage: ' + ex.message;
                throw ex;
            }
        }

    }

}