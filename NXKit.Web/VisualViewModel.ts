/// <reference path="Visual.ts" />

module NXKit.Web {

    /**
      * Base view model class for wrapping a Visual.
      */
    export class VisualViewModel {

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

        /**
          * Gets the binding context available at the time the view model was created.
          */
        public get Context(): KnockoutBindingContext {
            return this._context;
        }

        /**
          * Gets the visual that is wrapped by this view model.
          */
        public get Visual(): Visual {
            return this._visual;
        }

        /**
          * Gets the unique document ID of the wrapped visual.
          */
        public get UniqueId(): string {
            return Utils.GetUniqueId(this.Visual);
        }

    }

}