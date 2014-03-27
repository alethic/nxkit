/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class InputViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get ShowLabel(): boolean {
            return !NXKit.Web.LayoutOptions.GetArgs(this.Context).SuppressLabel;
        }

        GetHintText(): string {
            var n = this.Hint;
            if (n == null)
                return null;

            var s = '';
            var l = n.Nodes();
            for (var i = 0; i < l.length; i++)
                if (l[i].Type === 'NXKit.NXText')
                    s += l[i].Properties['Text'].ValueAsString();
            return s;
        }

        public get PlaceHolderText(): string {
            var n = this.Hint;
            if (n == null)
                return null;

            if (n.Properties['Appearance'].ValueAsString() != 'minimal')
                return null;

            return this.GetHintText();
        }

        public get ShowAdvice(): KnockoutObservable<boolean> {
            return ko.computed(() =>
                this.Hint != null && this.Hint.Properties['Appearance'].ValueAsString() != 'minimal');
        }

        public get ShowError(): KnockoutObservable<boolean> {
            return ko.computed(() =>
                this.Alert != null && !this.Valid());
        }

    }

}