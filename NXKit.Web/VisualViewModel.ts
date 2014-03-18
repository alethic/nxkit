/// <reference path="Scripts/typings/knockout/knockout.d.ts" />

module NXKit.Web {

    export class VisualViewModel {

        public static GetUniqueId(visual): string {
            return visual != null ? visual.Properties.UniqueId : null;
        }

        private _context: KnockoutBindingContext;
        private _visual: Visual;

        constructor(context: KnockoutBindingContext, visual: Visual) {
            var self = this;
            self._context = context;
            self._visual = visual;
        }

        public get Context(): KnockoutBindingContext {
            return this._context;
        }

        public get Visual(): Visual {
            return this._visual;
        }

        public get UniqueId(): string {
            return VisualViewModel.GetUniqueId(this);
        }

    }

}