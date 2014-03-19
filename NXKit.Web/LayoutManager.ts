/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Visual.ts" />

module NXKit.Web {

    export class LayoutManager {

        private _context: KnockoutBindingContext;

        constructor(context: KnockoutBindingContext) {
            var self = this;
            self._context = context;
        }

        public get Context(): KnockoutBindingContext {
            return this._context;
        }

        public GetTemplate(descriptor: any): string {
            console.error('LayoutManager.GetTemplate not implemented');
            return null;
        }

        public GetVisualTemplate(visual: Visual): string {
            return this.GetTemplate({ Type: visual.Type });
        }

    }

}
