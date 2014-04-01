module NXKit.Web.XForms.GroupUtil {

    export module ViewModel_ {

        export class Item {

            private _viewModel: NodeViewModel;
            private _level: number;

            constructor(viewModel: NodeViewModel, level: number) {
                this._viewModel = viewModel;
                this._level = level;
            }

            public get ViewModel(): NodeViewModel {
                return this._viewModel;
            }

            public get Level(): number {
                return this._level;
            }

            public get Relevant(): KnockoutObservable<boolean> {
                return this.GetRelevant();
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return ko.computed<boolean>(() => true);
            }

            public get ReadOnly(): KnockoutObservable<boolean> {
                return this.GetReadOnly();
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return ko.computed<boolean>(() => false);
            }

            public get Required(): KnockoutObservable<boolean> {
                return this.GetRequired();
            }

            GetRequired(): KnockoutObservable<boolean> {
                return ko.computed<boolean>(() => false);
            }

            public get Valid(): KnockoutObservable<boolean> {
                return this.GetValid();
            }

            GetValid(): KnockoutObservable<boolean> {
                return ko.computed<boolean>(() => true);
            }

            public get Label() {
                return this.GetLabel();
            }

            GetLabel(): Node {
                throw new Error('GetLabel not implemented');
            }

            public get Help() {
                return this.GetHelp();
            }

            GetHelp(): Node {
                throw new Error('GetHelp not implemented');
            }

            public get Layout() {
                return this.GetLayout();
            }

            GetLayout(): any {
                throw new Error('GetLayout not implemented');
            }

        }

        /**
          * Describes an item that will render a raw node.
          */
        export class NodeItem
            extends Item {

            private _itemNode: Node;

            constructor(viewModel: NodeViewModel, itemNode: Node, level: number) {
                super(viewModel, level);
                this._itemNode = itemNode;
            }

            public get ItemNode(): Node {
                return this._itemNode;
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return ko.computed(() => Utils.IsModelItemBindable(this._itemNode) ? Utils.GetRelevant(this._itemNode)() : true);
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return ko.computed(() => Utils.IsModelItemBindable(this._itemNode) ? Utils.GetReadOnly(this._itemNode)() : false);
            }

            GetRequired(): KnockoutObservable<boolean> {
                return ko.computed(() => Utils.IsModelItemBindable(this._itemNode) ? Utils.GetRequired(this._itemNode)() : false);
            }

            GetValid(): KnockoutObservable<boolean> {
                return ko.computed(() => Utils.IsModelItemBindable(this._itemNode) ? Utils.GetValid(this._itemNode)() : true);
            }

            GetLabel(): Node {
                if (this._itemNode.Type == 'NXKit.XForms.Input' &&
                    Utils.GetType(this._itemNode)() == '{http://www.w3.org/2001/XMLSchema}boolean')
                    // boolean inputs provide their own label
                    return null;
                else
                    return Utils.GetLabel(this._itemNode);
            }

            GetHelp(): Node {
                return Utils.GetHelp(this._itemNode);
            }

            GetLayout(): any {
                return {
                    node: this.ViewModel.Node,
                    data: this,
                    layout: 'node',
                    level: this.Level,
                };
            }

        }

        /**
          * Describes a sub-item of a top-level group which will render a row of items.
          */
        export class Row
            extends Item {

            private _items: Item[];
            private _done: boolean;

            constructor(viewModel: NodeViewModel, level: number) {
                super(viewModel, level);
                this._items = new Array<Item>();
            }

            public get Items(): Item[] {
                return this._items;
            }

            public get Done(): boolean {
                return this._done;
            }

            public set Done(value: boolean) {
                this._done = value;
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return ko.computed(() => this._items.every(_ => _.Relevant()));
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return ko.computed(() => this._items.every(_ => _.ReadOnly()));
            }

            GetRequired(): KnockoutObservable<boolean> {
                return ko.computed(() => this._items.every(_ => _.Required()));
            }

            GetValid(): KnockoutObservable<boolean> {
                return ko.computed(() => this._items.every(_ => _.Valid()));
            }

            GetLabel(): Node {
                return null;
            }

            GetHelp(): Node {
                return null;
            }

            GetLayout(): any {
                return {
                    node: this.ViewModel.Node,
                    data: this,
                    layout: this.GetLayoutName(),
                    level: this.Level,
                }
            }

            GetLayoutName(): string {
                switch (this._items.length) {
                    case 1:
                        return "single";
                    case 2:
                        return "double";
                    case 3:
                        return "triple";
                    default:
                        throw new Error("Unhandled row size");
                }
            }

        }

        export class GroupItem
            extends Item {

            private _groupNode: Node;
            private _items: Item[];

            constructor(viewModel: NodeViewModel, groupNode: Node, level: number) {
                super(viewModel, level);
                this._groupNode = groupNode;
                this._items = new Array<Item>();
            }

            get Items(): Item[] {
                return this._items;
            }

            set Items(items: Item[]) {
                this._items = items;
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return Utils.GetRelevant(this._groupNode);
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return Utils.GetReadOnly(this._groupNode);
            }

            GetRequired(): KnockoutObservable<boolean> {
                return Utils.GetRequired(this._groupNode);
            }

            GetValid(): KnockoutObservable<boolean> {
                return Utils.GetValid(this._groupNode);
            }

            GetLabel(): Node {
                return Utils.GetLabel(this._groupNode);
            }

            GetHelp(): Node {
                return Utils.GetHelp(this._groupNode);
            }

            GetLayout(): any {
                return {
                    node: this.ViewModel.Node,
                    data: this,
                    layout: 'group',
                    level: this.Level,
                };
            }

        }

    }

    export function GetGroupItem(viewModel: NodeViewModel, node: Node, level: number): ViewModel_.GroupItem {
        var item = new ViewModel_.GroupItem(viewModel, node, level);
        item.Items = GetItems(viewModel, node, level + 1);
        return item;
    }

    export function GetItems(viewModel: NodeViewModel, node: Node, level: number): ViewModel_.Item[] {
        var list = new Array<ViewModel_.Item>();
        var cnts = Utils.GetContents(node);
        for (var i = 0; i < cnts.length; i++) {
            var v = cnts[i];

            // nested group obtains single child
            if (v.Type == 'NXKit.XForms.Group') {
                var groupItem = this.GetGroupItem(v, level);
                list.push(groupItem);
                continue;
            } else if (v.Type == 'NXKit.XForms.TextArea') {
                var textAreaItem = new ViewModel_.Row(viewModel, level);
                textAreaItem.Done = true;
                list.push(textAreaItem);
                continue;
            }

            // check if last inserted item was a single item, if so, replace with a double item
            var item = list.pop();
            if (item instanceof ViewModel_.Row && !(<ViewModel_.Row>item).Done) {
                var item_ = <ViewModel_.Row>item;
                item_.Items.push(new ViewModel_.NodeItem(viewModel, v, level));
                list.push(item_);

                // end row
                if (item_.Items.length >= 2)
                    item_.Done = true;
            } else {
                // put previous item back into list
                if (item != null)
                    list.push(item);

                // insert new row
                var item_ = new ViewModel_.Row(viewModel, level);
                item_.Items.push(new ViewModel_.NodeItem(viewModel, v, level));
                list.push(item_);
            }
        }

        return list;
    }

}