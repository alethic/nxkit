module NXKit.View.XForms.SelectUtil {

    export class Selectable {

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
            return this.GetId();
        }

        GetId(): string {
            return this._node.Property('NXKit.XForms.ISelectable', 'Id').ValueAsString();
        }

        public get Label(): Node {
            return this.GetLabelNode();
        }

        GetLabelNode(): Node {
            return ViewModelUtil.GetLabelNode(this._node);
        }

        public get Value(): Node {
            return this.GetValueNode();
        }

        GetValueNode(): Node {
            return ViewModelUtil.GetValueNode(this._node);
        }

    }

    /**
      * Gets the select item-set. This consists of the item nodes of the given select node.
      */
    export function GetSelectables(viewModel: NodeViewModel, node: Node, level: number): Selectable[] {
        try {
            return node.Nodes()
                .filter(_ => _.Interfaces['NXKit.XForms.ISelectable'] != null)
                .map(_ => new Selectable(viewModel, _));
        } catch (ex) {
            ex.message = 'SelectUtil.GetSelectables()' + '"\nMessage: ' + ex.message;
            throw ex;
        }
    }

}