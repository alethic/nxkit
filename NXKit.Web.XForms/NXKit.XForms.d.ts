declare module NXKit.Web.XForms {
    class XFormsNodeViewModel extends NodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Value : KnockoutObservable<any>;
        public ValueAsString : KnockoutObservable<string>;
        public ValueAsBoolean : KnockoutObservable<boolean>;
        public ValueAsNumber : KnockoutObservable<number>;
        public Relevant : KnockoutObservable<boolean>;
        public ReadOnly : KnockoutObservable<boolean>;
        public Required : KnockoutObservable<boolean>;
        public Valid : KnockoutObservable<boolean>;
        public Type : KnockoutObservable<string>;
        public Appearance : KnockoutObservable<string>;
        public Label : Node;
        public Help : Node;
        public Hint : Node;
        public Alert : Node;
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
    class TriggerViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Activate(): void;
    }
}
declare module NXKit.Web.XForms.SelectUtil {
    class Item {
        private _viewModel;
        private _node;
        constructor(viewModel: NodeViewModel, node: Node);
        public ViewModel : NodeViewModel;
        public Node : Node;
        public Id : string;
        public GetId(): string;
        public Label : Node;
        public GetLabelNode(): Node;
        public Value : Node;
        public GetValueNode(): Node;
    }
    /**
    * Gets the select item-set. This consists of the item nodes of the given select node.
    */
    function GetItems(viewModel: NodeViewModel, node: Node, level: number): Item[];
}
declare module NXKit.Web.XForms.GroupUtil {
    class Item {
        private _viewModel;
        private _level;
        constructor(viewModel: NodeViewModel, level: number);
        public ViewModel : NodeViewModel;
        public Level : number;
        public Relevant : KnockoutObservable<boolean>;
        public GetRelevant(): KnockoutObservable<boolean>;
        public ReadOnly : KnockoutObservable<boolean>;
        public GetReadOnly(): KnockoutObservable<boolean>;
        public Required : KnockoutObservable<boolean>;
        public GetRequired(): KnockoutObservable<boolean>;
        public Valid : KnockoutObservable<boolean>;
        public GetValid(): KnockoutObservable<boolean>;
        public Label : Node;
        public GetLabel(): Node;
        public Help : Node;
        public GetHelp(): Node;
        public Layout : any;
        public GetLayout(): any;
    }
    /**
    * Describes an item that will render a raw node.
    */
    class NodeItem extends Item {
        private _itemNode;
        constructor(viewModel: NodeViewModel, itemNode: Node, level: number);
        public ItemNode : Node;
        public GetRelevant(): KnockoutObservable<boolean>;
        public GetReadOnly(): KnockoutObservable<boolean>;
        public GetRequired(): KnockoutObservable<boolean>;
        public GetValid(): KnockoutObservable<boolean>;
        public GetLabel(): Node;
        public GetHelp(): Node;
        public GetLayout(): any;
    }
    /**
    * Describes a sub-item of a top-level group which will render a row of items.
    */
    class Row extends Item {
        private _items;
        private _done;
        constructor(viewModel: NodeViewModel, level: number);
        public Items : Item[];
        public Done : boolean;
        public GetRelevant(): KnockoutObservable<boolean>;
        public GetReadOnly(): KnockoutObservable<boolean>;
        public GetRequired(): KnockoutObservable<boolean>;
        public GetValid(): KnockoutObservable<boolean>;
        public GetLabel(): Node;
        public GetHelp(): Node;
        public GetLayout(): any;
        public GetLayoutName(): string;
    }
    class GroupItem extends Item {
        private _groupNode;
        private _items;
        constructor(viewModel: NodeViewModel, groupNode: Node, level: number);
        public Items : Item[];
        public GetRelevant(): KnockoutObservable<boolean>;
        public GetReadOnly(): KnockoutObservable<boolean>;
        public GetRequired(): KnockoutObservable<boolean>;
        public GetValid(): KnockoutObservable<boolean>;
        public GetLabel(): Node;
        public GetHelp(): Node;
        public GetLayout(): any;
    }
    function GetGroupItem(viewModel: NodeViewModel, node: Node, level: number): GroupItem;
    /**
    * Gets the group item-set. This consists of the content nodes of the group organized by row.
    */
    function GetItems(viewModel: NodeViewModel, node: Node, level: number): Item[];
}
declare module NXKit.Web.XForms {
    class GroupViewModel extends XFormsNodeViewModel {
        private _count;
        constructor(context: KnockoutBindingContext, node: Node, count: number);
        public Items : GroupUtil.Item[];
        public GetItems(): GroupUtil.Item[];
    }
}
declare module NXKit.Web.XForms {
    class HelpViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Text : KnockoutObservable<string>;
    }
}
declare module NXKit.Web.XForms {
    class AlertViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Text : KnockoutObservable<string>;
    }
}
declare module NXKit.Web.XForms {
    class HintViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Text : KnockoutObservable<string>;
    }
}
declare module NXKit.Web.XForms {
    class InputViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public ShowLabel : boolean;
        public GetHintText(): string;
        public PlaceHolderText : string;
        public ShowAdvice : KnockoutObservable<boolean>;
        public ShowError : KnockoutObservable<boolean>;
        public FocusIn(): void;
        public FocusOut(): void;
    }
}
declare module NXKit.Web.XForms {
    class LabelViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Text : KnockoutObservable<string>;
    }
}
declare module NXKit.Web.XForms {
    class RangeViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Start : KnockoutObservable<number>;
        public End : KnockoutObservable<number>;
        public Step : KnockoutObservable<number>;
    }
}
declare module NXKit.Web.XForms {
    class Select1ViewModel extends XFormsNodeViewModel {
        static GetSelectedItemNodeId(node: Node): KnockoutComputed<string>;
        constructor(context: KnockoutBindingContext, node: Node);
        public Items : SelectUtil.Item[];
        public SelectedItemNodeId : KnockoutComputed<string>;
    }
}
declare module NXKit.Web.XForms {
    class TextAreaViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.ViewModelUtil {
    function GetValue(node: Node): KnockoutObservable<any>;
    function GetValueAsString(node: Node): KnockoutObservable<string>;
    function GetValueAsBoolean(node: Node): KnockoutObservable<boolean>;
    function GetValueAsNumber(node: Node): KnockoutObservable<number>;
    function GetValueAsDate(node: Node): KnockoutObservable<Date>;
    function GetRelevant(node: Node): KnockoutObservable<boolean>;
    function GetReadOnly(node: Node): KnockoutObservable<boolean>;
    function GetRequired(node: Node): KnockoutObservable<boolean>;
    function GetValid(node: Node): KnockoutObservable<boolean>;
    function GetType(node: Node): KnockoutObservable<string>;
    function IsModelItemValue(node: Node): boolean;
    function IsModelItemBinding(node: Node): boolean;
    function GetAppearance(node: Node): KnockoutObservable<string>;
    function GetLabelNode(node: Node): Node;
    function GetHelpNode(node: Node): Node;
    function GetHintNode(node: Node): Node;
    function GetAlertNode(node: Node): Node;
    function GetValueNode(node: Node): Node;
}
