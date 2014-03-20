/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Utils.ts" />
/// <reference path="Visual.ts" />
/// <reference path="LayoutManager.ts" />

module NXKit.Web {

    export class VisualViewModel {

        public static GetUniqueId(visual: Visual): string {
            return visual != null ? visual.Properties['UniqueId'].ValueAsString() : null;
        }

        private _context: KnockoutBindingContext;
        private _visual: Visual;

        constructor(context: KnockoutBindingContext, visual: Visual) {
            var self = this;

            if (context == null)
                throw new Error('context: null');

            if (!(visual instanceof Visual))
                throw new Error('visual: null');

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
            return VisualViewModel.GetUniqueId(this.Visual);
        }

    }

}