/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Visual.ts" />
/// <reference path="LayoutManager.ts" />

module NXKit.Web {

    export class VisualLayoutManager
        extends LayoutManager {

        private _visual: Visual;

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context);

            if (visual == null)
                throw new Error('visual: null');

            this._visual = visual;
        }

        public get Visual(): Visual {
            return this._visual;
        }

    }

}
