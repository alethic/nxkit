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
                if (l[i].Type === NodeType.Text)
                    s += l[i].Value;
            return s;
        }

        public get PlaceHolderText(): string {
            var n = this.Hint;
            if (n == null)
                return null;

            if (ViewModelUtil.GetAppearance(n)() != 'minimal')
                return null;

            return this.GetHintText();
        }

        public get ShowAdvice(): KnockoutObservable<boolean> {
            return ko.computed(() =>
                this.Hint != null && ViewModelUtil.GetAppearance(this.Hint)() !== 'minimal');
        }

        public get ShowError(): KnockoutObservable<boolean> {
            return ko.computed(() =>
                this.Alert != null && !this.Valid());
        }

        public FocusIn() {
            this.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                type: 'DOMFocusIn'
            });
        }

        public FocusOut() {
            this.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                type: 'DOMFocusOut'
            });
        }

    }

}