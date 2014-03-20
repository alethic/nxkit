/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />

module NXKit.Web.XForms {

    export class GroupGridViewModel
        extends GroupViewModel {

        private _count: number;

        constructor(context: KnockoutBindingContext, visual: Visual, count: number) {
            super(context, visual);

            this._count = count;
        }

        /**
         * Gets the visuals laid out in column order.
         */
        get Columns(): Visual[][] {
            var l = new Array<Array<Visual>>();

            for (var i = 0; i < this.Contents.length; i += this._count) {
                var c = new Array<Visual>();
                for (var j = 0; j < this._count; j++) {
                    var a = this.Contents[i + j];
                    if (a != null)
                        c.push(a);
                }
                l.push(c);
            }

            return l;
        }

    }

}