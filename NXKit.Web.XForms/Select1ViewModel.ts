/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export class Select1ViewModel
        extends NXKit.Web.XForms.XFormsNodeViewModel {

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
        }

        public get Selectables(): SelectUtil.Selectable[] {
            return SelectUtil.GetSelectables(this, this.Node, 1);
        }

        public get SelectedId(): KnockoutComputed<string> {
            var self = this;

            return ko.computed<string>({
                read: () => {
                    if (self.Node != null &&
                        self.Node.Property('NXKit.XForms.Select1', 'SelectedId') != null) {
                        var id = self.Node.Property('NXKit.XForms.Select1', 'SelectedId').ValueAsString();
                        var ls = self.Selectables;
                        for (var i = 0; i < ls.length; i++)
                            if (ls[i].Id == id)
                                return ls[i].Id;
                    }
                    else
                        return null;
                },
                write: id => {
                    var ls = self.Selectables;
                    for (var i = 0; i < ls.length; i++)
                        if (ls[i].Id == id)
                            self.Node.Property('NXKit.XForms.Select1', 'SelectedId').ValueAsString(id);
                },
            });
        }

        public get Selected(): KnockoutComputed<SelectUtil.Selectable> {
            var self = this;

            return ko.computed<SelectUtil.Selectable>({
                read: () => {
                    var id = self.SelectedId();
                    var ls = self.Selectables;
                    for (var i = 0; i < ls.length; i++)
                        if (ls[i].Id == id)
                            return ls[i];

                    return null;
                },
                write: _ => {
                    self.SelectedId(_.Id);
                },
            });
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