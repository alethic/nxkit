/// <reference path="Node.ts" />

module NXKit.Web {

    /**
      * Base view model class for wrapping a node.
      */
    export class NodeViewModel {

        private _context: KnockoutBindingContext;
        private _node: Node;

        constructor(context: KnockoutBindingContext, node: Node) {
            var self = this;

            if (context == null)
                throw new Error('context: null');

            if (!(node instanceof Node))
                throw new Error('node: null');

            self._context = context;
            self._node = node;
        }

        /**
          * Gets the binding context available at the time the view model was created.
          */
        public get Context(): KnockoutBindingContext {
            return this._context;
        }

        /**
          * Gets the node that is wrapped by this view model.
          */
        public get Node(): Node {
            return this._node;
        }

        /**
          * Gets the unique document ID of the wrapped node.
          */
        public get UniqueId(): string {
            return Util.GetUniqueId(this.Node);
        }

        /**
          * Gets the content nodes of the current node.
          */
        public get Contents(): Node[] {
            return this.GetContents();
        }

        GetContents(): Node[] {
            try {
                return ViewModelUtil.GetContents(this.Node);
            } catch (ex) {
                ex.message = 'NodeViewModel.GetContents()' + '"\nMessage: ' + ex.message;
                throw ex;
            }
        }

    }

}