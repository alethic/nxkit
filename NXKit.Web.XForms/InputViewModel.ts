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
                if (l[i].Type === 'NXKit.IText')
                    s += l[i].Property('NXKit.IText', 'Text').ValueAsString();
            return s;
        }

        public get PlaceHolderText(): string {
            var n = this.Hint;
            if (n == null)
                return null;

            if (Utils.GetAppearance(n)() != 'minimal')
                return null;

            return this.GetHintText();
        }

        public get ShowAdvice(): KnockoutObservable<boolean> {
            return ko.computed(() =>
                this.Hint != null && Utils.GetAppearance(this.Hint)() !== 'minimal');
        }

        public get ShowError(): KnockoutObservable<boolean> {
            return ko.computed(() =>
                this.Alert != null && !this.Valid());
        }

    }

}