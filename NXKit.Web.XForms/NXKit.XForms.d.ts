declare module NXKit.Web.XForms {
    class XFormsVisualViewModel extends VisualViewModel {
        static ControlVisualTypes: string[];
        static MetadataVisualTypes: string[];
        static TransparentVisualTypes: string[];
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Value : KnockoutComputed<any>;
        public ValueAsString : KnockoutComputed<string>;
        public Relevant : KnockoutComputed<boolean>;
        public Type : KnockoutComputed<string>;
        public Appearance : KnockoutComputed<string>;
        public Label : Visual;
        public Help : Visual;
        public Hint : Visual;
        public Contents : Visual[];
    }
}
declare module NXKit.Web.XForms {
    class GroupLayoutManager extends LayoutManager {
        constructor(context: KnockoutBindingContext);
        /**
        * Applies the 'level' and 'layout' bindings to the template search.
        */
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any;
    }
}
declare module NXKit.Web.XForms {
    module GroupViewModel_ {
        /**
        * Represents a sub-item of a top-level group.
        */
        class Item {
            private _visual;
            private _level;
            constructor(visual: Visual, level: number);
            public Visual : Visual;
            public Level : number;
            public Relevant : KnockoutObservable<boolean>;
            public GetRelevant(): KnockoutObservable<boolean>;
            public Label : Visual;
            public GetLabel(): Visual;
            public Help : Visual;
            public GetHelp(): Visual;
            public Layout : any;
            public GetLayout(): any;
        }
        /**
        * Describes an item that will render a raw visual.
        */
        class VisualItem extends Item {
            private _itemVisual;
            constructor(visual: Visual, itemVisual: Visual, level: number);
            public ItemVisual : Visual;
            public GetRelevant(): KnockoutObservable<boolean>;
            public GetLabel(): Visual;
            public GetHelp(): Visual;
            public GetLayout(): any;
        }
        class InputItem extends VisualItem {
            constructor(visual: Visual, inputVisual: Visual, level: number);
            public InputVisual : Visual;
            public GetLayout(): any;
        }
        /**
        * Describes a sub-item of a top-level group that will render a single underlying item.
        */
        class SingleItem extends Item {
            private _item;
            private _force;
            constructor(visual: Visual, level: number);
            public Item : Item;
            public Force : boolean;
            public GetRelevant(): KnockoutObservable<boolean>;
            public GetLabel(): Visual;
            public GetHelp(): Visual;
            public GetLayout(): any;
        }
        /**
        * Describes a sub-item of a top-level group which will render two items.
        */
        class DoubleItem extends Item {
            private _item1;
            private _item2;
            constructor(visual: Visual, level: number);
            public Item1 : Item;
            public Item2 : Item;
            public GetRelevant(): KnockoutObservable<boolean>;
            public GetLabel(): Visual;
            public GetHelp(): Visual;
            public GetLayout(): any;
        }
        class GroupItem extends Item {
            private _groupVisual;
            private _items;
            constructor(visual: Visual, groupVisual: Visual, level: number);
            public Items : Item[];
            public GetRelevant(): KnockoutObservable<boolean>;
            public GetLabel(): Visual;
            public GetHelp(): Visual;
            public GetLayout(): any;
        }
    }
    class GroupViewModel extends XFormsVisualViewModel {
        private _count;
        constructor(context: KnockoutBindingContext, visual: Visual, count: number);
        /**
        * Gets the set of contents expressed as template binding objects.
        */
        public BindingContents : GroupViewModel_.Item[];
        private GetBindingContents();
        /**
        * Gets the set of contents expressed as template binding objects.
        */
        private GetGroupItem(visual, level);
        private GetItems(visual, level);
    }
}
declare module NXKit.Web.XForms {
    class HelpViewModel extends XFormsVisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Text : KnockoutComputed<string>;
    }
}
declare module NXKit.Web.XForms {
    class HintViewModel extends XFormsVisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Text : KnockoutComputed<string>;
    }
}
declare module NXKit.Web.XForms {
    class InputViewModel extends XFormsVisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms {
    class LabelViewModel extends XFormsVisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Text : KnockoutComputed<string>;
    }
}
declare module NXKit.Web.XForms {
    class RangeViewModel extends XFormsVisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms {
    class Select1ViewModel extends XFormsVisualViewModel {
        static GetSelectedItemVisualId(visual: Visual): KnockoutComputed<string>;
        constructor(context: KnockoutBindingContext, visual: Visual);
        public SelectedItemVisualId : KnockoutComputed<string>;
    }
}
declare module NXKit.Web.XForms {
    class TextAreaViewModel extends XFormsVisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms.Utils {
    function GetValue(visual: Visual): KnockoutComputed<any>;
    function GetValueAsString(visual: Visual): KnockoutComputed<string>;
    function GetRelevant(visual: Visual): KnockoutComputed<boolean>;
    function GetType(visual: Visual): KnockoutComputed<string>;
    function GetAppearance(visual: Visual): KnockoutComputed<string>;
    function IsMetadataVisual(visual: Visual): boolean;
    function GetLabel(visual: Visual): Visual;
    function GetHelp(visual: Visual): Visual;
    function GetHint(visual: Visual): Visual;
    function GetAlert(visual: Visual): Visual;
    function IsControlVisual(visual: Visual): boolean;
    function HasControlVisual(visual: Visual): boolean;
    function GetControlVisuals(visual: Visual): Visual[];
    function IsTransparentVisual(visual: Visual): boolean;
    function GetContents(visual: Visual): Visual[];
}
