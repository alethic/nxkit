/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.View.XForms {

    export class InputViewModel
        extends NXKit.View.XForms.XFormsNodeViewModel {

        private _valueAsDateString: KnockoutComputed<string>;

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
            var self = this;

            self._valueAsDateString = Util.Computed({
                read: () => {
                    var value = self.ValueAsString();
                    if (value == null)
                        return null;

                    // attempt to convert value into date
                    var date = self.ParseDateString(value);
                    if (date) {
                        return date.format("YYYY-MM-DD");
                    }

                    // fallback to raw value
                    return self.ValueAsString();
                },
                write: (value: string) => {
                    if (value == null) {
                        self.ValueAsString(value);
                        return;
                    }

                    // attempt to convert value into date
                    var date = self.ParseDateString(value);
                    if (date) {
                        self.ValueAsString(date.format("YYYY-MM-DD"));
                        return;
                    }

                    // fall back to raw value
                    self.ValueAsString(value);
                },
            });
        }

        ParseDateString(value: string): any {
            var fmts = ['YYYY-MM-DD', 'YYYY/MM/dd', 'MM-DD-YYYY', 'MM/DD/YYYY'];
            for (var i in fmts) {
                var date = moment(value, fmts[i], true);
                if (date.isValid()) {
                    return date;
                }
            }

            return null;
        }

        public get ValueAsDateString(): KnockoutObservable<string> {
            return this._valueAsDateString;
        }

        public get ShowLabel(): boolean {
            return !NXKit.View.LayoutOptions.GetArgs(this.Context).SuppressLabel;
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
            this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                type: 'DOMFocusIn'
            });
        }

        public FocusOut() {
            this.Node.Invoke('NXKit.DOMEvents.EventTarget', 'Dispatch', {
                type: 'DOMFocusOut'
            });
        }

    }

}