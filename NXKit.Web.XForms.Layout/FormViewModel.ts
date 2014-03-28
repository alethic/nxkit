/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.Web.XForms.Layout {

    export module FormViewModel_ {

        /**
          * Represents a sub-item of a top-level group.
          */
        export class Step {

            private _viewModel: FormViewModel;
            private _node: Node;
            private _index: number;
            private _label: Node;
            private _active: KnockoutObservable<boolean>;
            private _disabled: KnockoutObservable<boolean>;

            constructor(viewModel: FormViewModel, node: Node, index: number) {
                this._viewModel = viewModel;
                this._node = node;
                this._index = index;
                this._label = XForms.Utils.GetLabel(node);
                this._active = ko.computed(() => this._viewModel.ActivePage() == this._node);
                this._disabled = ko.computed(() => !XForms.Utils.GetRelevant(this._node)());
            }

            public get Node(): Node {
                return this._node;
            }

            public get Index(): number {
                return this._index;
            }

            public get Label(): Node {
                return this._label;
            }

            public get Active(): KnockoutObservable<boolean> {
                return this._active;
            }

            public get Disabled(): KnockoutObservable<boolean> {
                return this._disabled;
            }

            public SetActive() {
                this._viewModel.ActivePage(this._node);
            }

        }

    }

    export class FormViewModel
        extends LayoutNodeViewModel {

        private _pages: Node[];
        private _steps: FormViewModel_.Step[];
        private _activePage: KnockoutObservable<Node>;

        public PageChanged: IPageChangedEvent = new TypedEvent();

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
            var self = this;

            self._pages = Utils.GetPages(node);
            self._activePage = ko.observable<Node>(self._pages.length >= 1 ? self._pages[0] : null);
            self._steps = self._pages.map((i, j) => new FormViewModel_.Step(self, i, j));
        }

        public get Pages(): Node[] {
            return this._pages;
        }

        public get ActivePage(): KnockoutObservable<Node> {
            return this._activePage;
        }

        GetPreviousPage(page: Node): KnockoutObservable<Node> {
            var self = this;
            return ko.computed(() => {
                for (var i = self._pages.indexOf(page) - 1; i >= 0; i--) {
                    if (XForms.Utils.GetRelevant(self._pages[i])())
                        return self._pages[i];
                }

                return null;
            });
        }

        public get HasPreviousPage(): KnockoutObservable<boolean> {
            var self = this;
            return ko.computed(() => self.GetPreviousPage(self.ActivePage())() != null);
        }

        public GoPreviousPage(): void {
            var self = this;
            var p = self.GetPreviousPage(self.ActivePage())();
            if (p != null)
                self.ActivePage(p);
        }

        GetNextPage(page: Node): KnockoutObservable<Node> {
            var self = this;
            return ko.computed(() => {
                for (var i = self._pages.indexOf(page) + 1; i < self._pages.length; i++) {
                    if (XForms.Utils.GetRelevant(self._pages[i])())
                        return self._pages[i];
                }

                return null;
            });
        }

        public get HasNextPage(): KnockoutObservable<boolean> {
            var self = this;
            return ko.computed(() => self.GetNextPage(self.ActivePage())() != null);
        }

        public GoNextPage(): void {
            var self = this;
            var p = self.GetNextPage(self.ActivePage())();
            if (p != null)
                self.ActivePage(p);
        }

        public get Steps(): FormViewModel_.Step[] {
            return this._steps;
        }

    }

}