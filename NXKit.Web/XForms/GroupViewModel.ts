/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />


module NXKit.Web.XForms {

    export module GroupViewModel_ {

        /**
          * Represents a sub-item of a top-level group.
          */
        export class Item {

            private _visual: Visual;
            private _level: number;

            constructor(visual: Visual, level: number) {
                this._visual = visual;
                this._level = level;
            }

            public get Visual(): Visual {
                return this._visual;
            }

            public get Level(): number {
                return this._level;
            }

            public get Label() {
                return this.GetLabel();
            }

            GetLabel(): Visual {
                throw new Error('GetLabel not implemented');
            }

            public get Layout() {
                return this.GetLayout();
            }

            GetLayout(): any {
                throw new Error('GetLayout not implemented');
            }

        }

        /**
          * Describes an item that will render a raw visual.
          */
        export class VisualItem
            extends Item {

            private _itemVisual: Visual;

            constructor(visual: Visual, itemVisual: Visual, level: number) {
                super(visual, level);
                this._itemVisual = itemVisual;
            }

            public get ItemVisual(): Visual {
                return this._itemVisual;
            }

            GetLabel(): Visual {
                return VisualViewModel.GetLabel(this._itemVisual);
            }

            GetLayout(): any {
                return {
                    visual: this.Visual,
                    data: this,
                    layout: 'visual',
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

            constructor(visual: Visual, level: number) {
                super(visual, level);
            }

            public get Item(): Item {
                return this._item;
            }

            public set Item(item: Item) {
                this._item = item;
            }

            GetLabel(): Visual {
                return this._item.Label;
            }

            GetLayout(): any {
                return {
                    visual: this.Visual,
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

            constructor(visual: Visual, level: number) {
                super(visual, level);
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

            GetLabel(): Visual {
                return null;
            }

            GetLayout(): any {
                return {
                    visual: this.Visual,
                    data: this,
                    layout: 'double',
                    level: this.Level,
                }
            }

        }

        export class GroupItem
            extends Item {

            private _groupVisual: Visual;
            private _items: Item[];

            constructor(visual: Visual, groupVisual: Visual, level: number) {
                super(visual, level);
                this._groupVisual = groupVisual;
                this._items = new Array<Item>();
            }

            get Items(): Item[] {
                return this._items;
            }

            set Items(items: Item[]) {
                this._items = items;
            }

            GetLabel(): Visual {
                return VisualViewModel.GetLabel(this._groupVisual);
            }

            GetLayout(): any {
                return {
                    visual: this.Visual,
                    data: this,
                    layout: 'group',
                    level: this.Level,
                };
            }

        }

    }

    export class GroupViewModel
        extends VisualViewModel {

        private _count: number;

        constructor(context: KnockoutBindingContext, visual: Visual, count: number) {
            super(context, visual);

            this._count = count;
        }

        /**
         * Gets the set of contents expressed as template binding objects.
         */
        public get BindingContents(): GroupViewModel_.Item[] {
            return this.GetBindingContents();
        }

        private GetBindingContents(): GroupViewModel_.Item[] {
            return this.GetItems(this.Visual, 1);
        }

        /**
         * Gets the set of contents expressed as template binding objects.
         */
        private GetGroupItem(visual: Visual, level: number): GroupViewModel_.GroupItem {
            var item = new GroupViewModel_.GroupItem(this.Visual, visual, level);
            item.Items = this.GetItems(visual, level + 1);
            return item;
        }

        private GetItems(visual: Visual, level: number): GroupViewModel_.Item[] {
            var list = new Array<GroupViewModel_.Item>();
            var cnts = VisualViewModel.GetContents(visual);
            for (var i = 0; i < cnts.length; i++) {
                var v = cnts[i];

                // nested group obtains single child
                if (v.Type == 'NXKit.XForms.XFormsGroupVisual') {
                    list.push(this.GetGroupItem(v, level));
                    continue;
                }

                // check if last inserted item was a single item, if so, replace with a double item
                var item = list.pop();
                if (item instanceof GroupViewModel_.SingleItem) {
                    var item1 = <GroupViewModel_.SingleItem>item;
                    var item2 = new GroupViewModel_.DoubleItem(this.Visual, level);
                    item2.Item1 = item1.Item;
                    item2.Item2 = new GroupViewModel_.VisualItem(this.Visual, v, level);
                    list.push(item2);
                }
                else {
                    // put previous item back into list
                    if (item != null)
                        list.push(item);

                    // insert new single item
                    var item1 = new GroupViewModel_.SingleItem(this.Visual, level);
                    item1.Item = new GroupViewModel_.VisualItem(this.Visual, v, level);
                    list.push(item1);
                }
            }

            return list;
        }

    }

}