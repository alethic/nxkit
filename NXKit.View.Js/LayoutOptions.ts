/// <reference path="Util.ts" />

module NXKit.View {

    export class LayoutOptions {

        /**
          * Gets the full set of currently applied layout option args for the given context.
          */
        public static GetArgs(bindingContext: KnockoutBindingContext): any {
            var a: any = {};
            var c = Util.GetContextItems(bindingContext);
            for (var i = 0; i < c.length; i++)
                if (c[i] instanceof LayoutOptions)
                    a = ko.utils.extend(a, c[i]);

            return a;
        }

        private _args: any;

        constructor(args: any) {
            this._args = args;
        }

        public get Args(): any {
            return this._args;
        }

    }

}