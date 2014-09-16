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
        public LabelAppearance : KnockoutObservable<string>;
        public Help : Node;
        public Hint : Node;
        public Alert : Node;
        public CountEnabled : number;
    }
}
declare module NXKit.Web.XForms {
    class DefaultLayoutManager extends LayoutManager {
        constructor(context: KnockoutBindingContext);
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
    }
}
declare module NXKit.Web.XForms {
    class GroupLayoutManager extends LayoutManager {
        constructor(context: KnockoutBindingContext);
        public GetTemplateOptions(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, options: any): any;
    }
}
declare module NXKit.Web.XForms {
    class OutputViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Text : KnockoutObservable<string>;
    }
}
declare module NXKit.Web.XForms {
    class RepeatViewModel extends XFormsNodeViewModel {
        private _items;
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.Knockout {
    class InputBooleanBindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.Web.XForms.Knockout {
    class Select1BindingHandler implements KnockoutBindingHandler {
        static _init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        static _update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public init(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public update(element: HTMLElement, valueAccessor: () => any, allBindings: any, viewModel: any, bindingContext: KnockoutBindingContext): void;
    }
}
declare module NXKit.Web.XForms {
    class SubmitViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Activate(): void;
    }
}
declare module NXKit.Web.XForms {
    class TriggerViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Activate(): void;
    }
}
declare module NXKit.Web.XForms.SelectUtil {
    class Selectable {
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
    function GetSelectables(viewModel: NodeViewModel, node: Node, level: number): Selectable[];
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
        public SetFocus(): void;
    }
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
        public SetFocus(): void;
    }
    function GetGroupItem(viewModel: NodeViewModel, node: Node, level: number): GroupItem;
    function GetItems(viewModel: NodeViewModel, node: Node, level: number): Item[];
}
declare module NXKit.Web.XForms {
    class GroupViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public LabelAppearance : KnockoutObservable<string>;
        public Count : number;
        public CountEnabled : number;
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
        constructor(context: KnockoutBindingContext, node: Node);
        public Selectables : SelectUtil.Selectable[];
        public SelectedId : KnockoutComputed<string>;
        public Selected : KnockoutComputed<SelectUtil.Selectable>;
        public FocusIn(): void;
        public FocusOut(): void;
    }
}
declare module NXKit.Web.XForms {
    class TextAreaViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms {
    class Constants {
        static UINode: string;
        static DataNode: string;
    }
}
declare module NXKit.Web.XForms.ViewModelUtil {
    function GetValue(node: Node): KnockoutObservable<string>;
    function IsDataNode(node: Node): boolean;
    function GetDataValue(node: Node): KnockoutObservable<any>;
    function GetDataValueAsString(node: Node): KnockoutObservable<string>;
    function GetDataValueAsBoolean(node: Node): KnockoutObservable<boolean>;
    function GetDataValueAsNumber(node: Node): KnockoutObservable<number>;
    function GetDataValueAsDate(node: Node): KnockoutObservable<Date>;
    function GetDataType(node: Node): KnockoutObservable<string>;
    function IsUINode(node: Node): boolean;
    function GetRelevant(node: Node): KnockoutObservable<boolean>;
    function GetReadOnly(node: Node): KnockoutObservable<boolean>;
    function GetRequired(node: Node): KnockoutObservable<boolean>;
    function GetValid(node: Node): KnockoutObservable<boolean>;
    function GetAppearance(node: Node): KnockoutObservable<string>;
    function GetDataItem(node: Node): KnockoutObservable<any>;
    function GetLabelNode(node: Node): Node;
    function GetHelpNode(node: Node): Node;
    function GetHintNode(node: Node): Node;
    function GetAlertNode(node: Node): Node;
    function GetValueNode(node: Node): Node;
}
