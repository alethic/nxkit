/// <reference path="XFormsNodeViewModel.ts" />

module NXKit.Web.XForms {

    export module GroupViewModel_ {

        /**
          * Represents a sub-item of a top-level group.
          */
        export class Item {

            private _viewModel: GroupViewModel;
            private _level: number;

            constructor(viewModel: GroupViewModel, level: number) {
                this._viewModel = viewModel;
                this._level = level;
            }

            public get ViewModel(): GroupViewModel {
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

            constructor(viewModel: GroupViewModel, itemNode: Node, level: number) {
                super(viewModel, level);
                this._itemNode = itemNode;
            }

            public get ItemNode(): Node {
                return this._itemNode;
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return Utils.GetRelevant(this._itemNode);
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return Utils.GetReadOnly(this._itemNode);
            }

            GetRequired(): KnockoutObservable<boolean> {
                return Utils.GetRequired(this._itemNode);
            }

            GetValid(): KnockoutObservable<boolean> {
                return Utils.GetValid(this._itemNode);
            }

            GetLabel(): Node {
                if (this._itemNode.Type == 'NXKit.XForms.InputElement' &&
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

        export class InputItem
            extends NodeItem {

            constructor(viewModel: GroupViewModel, inputNode: Node, level: number) {
                super(viewModel, inputNode, level);
            }

            public get InputNode(): Node {
                return this.ItemNode;
            }

            GetLayout(): any {
                return {
                    node: this.ViewModel.Node,
                    data: this,
                    layout: 'input',
                    type: Utils.GetType(this.InputNode),
                    level: this.Level,
                };
            }

        }

        /**
          * Describes a sub-item of a top-level group that will render a single underlying item.
          */
        export class SingleItem
            extends Item {

            private _item: Item;
            private _force: boolean;

            constructor(viewModel: GroupViewModel, level: number) {
                super(viewModel, level);
            }

            public get Item(): Item {
                return this._item;
            }

            public set Item(item: Item) {
                this._item = item;
            }

            public get Force() {
                return this._force;
            }

            public set Force(force: boolean) {
                this._force = force;
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return this.Item.Relevant;
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return this.Item.ReadOnly;
            }

            GetRequired(): KnockoutObservable<boolean> {
                return this.Item.Required;
            }

            GetValid(): KnockoutObservable<boolean> {
                return this.Item.Valid;
            }

            GetLabel(): Node {
                return this._item.Label;
            }

            GetHelp(): Node {
                return this._item.Help;
            }

            GetLayout(): any {
                return {
                    node: this.ViewModel.Node,
                    data: this,
                    layout: 'single',
                    level: this.Level,
                }
            }

        }

        /**
          * Describes a sub-item of a top-level group which will render two items.
          */
        export class DoubleItem
            extends Item {

            private _item1: Item;
            private _item2: Item;

            constructor(viewModel: GroupViewModel, level: number) {
                super(viewModel, level);
            }

            public get Item1(): Item {
                return this._item1;
            }

            public set Item1(item: Item) {
                this._item1 = item;
            }

            public get Item2(): Item {
                return this._item2;
            }

            public set Item2(item: Item) {
                this._item2 = item;
            }

            GetRelevant(): KnockoutObservable<boolean> {
                return ko.computed(() => this._item1.Relevant() && this._item2.Relevant());
            }

            GetReadOnly(): KnockoutObservable<boolean> {
                return ko.computed(() => this._item1.ReadOnly() && this._item2.ReadOnly());
            }

            GetRequired(): KnockoutObservable<boolean> {
                return ko.computed(() => this._item1.Required() && this._item2.Required());
            }

            GetValid(): KnockoutObservable<boolean> {
                return ko.computed(() => this._item1.Valid() && this._item2.Valid());
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
                    layout: 'double',
                    level: this.Level,
                }
            }

        }

        export class GroupItem
            extends Item {

            private _groupNode: Node;
            private _items: Item[];

            constructor(viewModel: GroupViewModel, groupNode: Node, level: number) {
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

    export class GroupViewModel
        extends XFormsNodeViewModel {

        private _count: number;

        constructor(context: KnockoutBindingContext, node: Node, count: number) {
            super(context, node);

            this._count = count;
        }

        /**
         * Gets the set of contents expressed as template binding objects.
         */
        public get BindingContents(): GroupViewModel_.Item[] {
            return this.GetBindingContents();
        }

        private GetBindingContents(): GroupViewModel_.Item[] {
            return this.GetItems(this.Node, 1);
        }

        /**
         * Gets the set of contents expressed as template binding objects.
         */
        private GetGroupItem(node: Node, level: number): GroupViewModel_.GroupItem {
            var item = new GroupViewModel_.GroupItem(this, node, level);
            item.Items = this.GetItems(node, level + 1);
            return item;
        }

        private GetItems(node: Node, level: number): GroupViewModel_.Item[] {
            var list = new Array<GroupViewModel_.Item>();
            var cnts = Utils.GetContents(node);
            for (var i = 0; i < cnts.length; i++) {
                var v = cnts[i];

                // nested group obtains single child
                if (v.Type == 'NXKit.XForms.GroupElement') {
                    var groupItem = this.GetGroupItem(v, level);
                    list.push(groupItem);
                    continue;
                } else if (v.Type == 'NXKit.XForms.TextAreaElement') {
                    var textAreaItem = new GroupViewModel_.SingleItem(this, level);
                    textAreaItem.Force = true;
                    list.push(textAreaItem);
                    continue;
                }

                // check if last inserted item was a single item, if so, replace with a double item
                var item = list.pop();
                if (item instanceof GroupViewModel_.SingleItem && !(<GroupViewModel_.SingleItem>item).Force) {
                    var item1 = <GroupViewModel_.SingleItem>item;
                    var item2 = new GroupViewModel_.DoubleItem(this, level);
                    item2.Item1 = item1.Item;
                    item2.Item2 = new GroupViewModel_.NodeItem(this, v, level);
                    list.push(item2);
                }
                else {
                    // put previous item back into list
                    if (item != null)
                        list.push(item);

                    // insert new single item
                    var item1 = new GroupViewModel_.SingleItem(this, level);
                    item1.Item = new GroupViewModel_.NodeItem(this, v, level);
                    list.push(item1);
                }
            }

            return list;
        }

    }

}