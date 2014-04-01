module NXKit.Web.XForms.SelectUtil {

    export class Item {

        private _viewModel: NodeViewModel;
        private _node: Node;

        constructor(viewModel: NodeViewModel, node: Node) {
            this._viewModel = viewModel;
            this._node = node;
        }

        public get ViewModel(): NodeViewModel {
            return this._viewModel;
        }

        public get Node(): Node {
            return this._node;
        }

        public get Id(): string {
            return this._node.ValueAsString('NXKit.NXElement', 'UniqueId')();
        }

        public get Label(): Node {
            return ViewModelUtil.GetLabelNode(this._node);
        }

        public get Value(): Node {
            return ViewModelUtil.GetValueNode(this._node);
        }

    }

    /**
      * Gets the select item-set. This consists of the item nodes of the given select node.
      */
    export function GetItems(viewModel: NodeViewModel, node: Node, level: number): Item[] {
        try {
            return node.Nodes()
                .filter(_ => _.Type == 'NXKit.XForms.Item')
                .map(_ => new Item(viewModel, _));
        } catch (ex) {
            ex.message = 'SelectUtil.GetItems()' + '"\nMessage: ' + ex.message;
            throw ex;
        }
    }

}