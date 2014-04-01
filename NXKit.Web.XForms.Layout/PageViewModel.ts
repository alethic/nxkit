/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.Web.XForms.Layout {

    export class PageViewModel
        extends LayoutNodeViewModel {

        private _form: FormViewModel;
        private _active: KnockoutObservable<boolean>;

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
            var self = this;

            // subscribe to form page change event, to deactivate ourselves
            var parents = context.$parents;
            for (var i = 0; i < parents.length; i++) {
                if (parents[i] instanceof FormViewModel) {
                    self._form = parents[i];
                }
            }

            // set ourselves as the active page if nobody else has claimed it
            if (self._form.ActivePage() == null)
                self._form.ActivePage(self.Node);
        }

        public get Active(): KnockoutObservable<boolean> {
            var self = this;
            return ko.computed(() => self._form.ActivePage() === self.Node);
        }

        public get Layout(): any {
            if (!this.Contents.some(_ => _.Type === 'NXKit.XForms.Group'))
                return 'body-with-group';
            else
                return 'body';
        }

    }

}
