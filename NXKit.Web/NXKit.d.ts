declare module NXKit.Web {
    interface IVisualPropertyValueChangedEvent extends IEvent {
        add(listener: (visual: Visual, property: Property) => void): void;
        remove(listener: (visual: Visual, property: Property) => void): void;
        trigger(visual: Visual, property: Property): void;
    }
    interface IPropertyMap {
        [name: string]: Property;
    }
    class PropertyMap implements IPropertyMap {
        [name: string]: Property;
    }
    class Visual {
        public _type: string;
        public _baseTypes: string[];
        public _properties: IPropertyMap;
        public _visuals: KnockoutObservableArray<Visual>;
        /**
        * Raised when the Visual has changes to be pushed to the server.
        */
        public ValueChanged: IVisualPropertyValueChangedEvent;
        /**
        * Initializes a new instance from the given initial data.
        */
        constructor(source: any);
        public IsVisual : boolean;
        /**
        * Gets the type of this visual.
        */
        public Type : string;
        /**
        * Gets the inheritence hierarchy of this visual.
        */
        public BaseTypes : string[];
        /**
        * Gets the interactive properties of this visual.
        */
        public Properties : IPropertyMap;
        /**
        * Gets the content of this visual.
        */
        public Visuals : KnockoutObservableArray<Visual>;
        /**
        * Integrates the data given by the visual parameter into this Visual.
        */
        public Update(source: any): void;
        /**
        * Updates the type of this Visual with the new value.
        */
        public UpdateType(type: string): void;
        /**
        * Updates the base types of this Visual with the new set of values.
        */
        public UpdateBaseTypes(baseTypes: string[]): void;
        /**
        * Integrates the set of properties given with this Visual.
        */
        public UpdateProperties(source: any): void;
        /**
        * Updates the property given by the specified name with the specified value.
        */
        public UpdateProperty(name: string, source: any): void;
        /**
        * Integrates the set of content Visuals with the given object values.
        */
        public UpdateVisuals(sources: any[]): void;
        public ToData(): any;
        /**
        * Transforms the given Property array into a list of data to push.
        */
        public PropertiesToData(): any;
        /**
        * Transforms the given Property array into a list of data to push.
        */
        public VisualsToData(): any[];
        /**
        * Initiates a push of new values to the server.
        */
        public OnValueChanged(visual: Visual, property: Property): void;
    }
}
declare module NXKit.Web {
    class LayoutManager {
        private _context;
        private _parent;
        constructor(context: KnockoutBindingContext);
        /**
        * Gets the context inside which this layout manager was created.
        */
        public Context : KnockoutBindingContext;
        /**
        * Gets the parent layout manager.
        */
        public Parent : LayoutManager;
        /**
        * Parses the given template binding information for a data structure to pass to the template lookup procedures.
        */
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any;
        /**
        * Gets the templates provided by this layout manager for the given data.
        */
        public GetLocalTemplates(): HTMLElement[];
        /**
        * Gets the set of available templates for the given data.
        */
        public GetTemplates(): HTMLElement[];
        /**
        * Gets the fallback template for the given data.
        */
        public GetUnknownTemplate(data: any): HTMLElement;
        /**
        * Extracts a JSON representation of a template node's data-nxkit bindings.
        */
        public GetTemplateNodeData(node: HTMLElement): any;
        /**
        * Tests whether a template node matches the given data.
        */
        public TemplatePredicate(node: HTMLElement, data: any): boolean;
        /**
        * Tests each given node against the predicate function.
        */
        private TemplateFilter(nodes, data);
        /**
        * Gets the appropriate template for the given data.
        */
        public GetTemplate(data: any): HTMLElement;
        /**
        * Gets the template that applies for the given data.
        */
        public GetTemplateName(data: any): string;
    }
}
declare module NXKit.Web {
    class DefaultLayoutManager extends LayoutManager {
        constructor(context: KnockoutBindingContext);
        /**
        * Parses the given template binding information for a data structure to pass to the template lookup procedures.
        */
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any;
        /**
        * Gets all available templates currently in the document.
        */
        public GetLocalTemplates(): HTMLElement[];
    }
}
declare module NXKit.Web {
    interface IPropertyValueChangedEvent extends IEvent {
        add(listener: (property: Property) => void): void;
        remove(listener: (property: Property) => void): void;
        trigger(property: Property): void;
    }
    class Property {
        private _value;
        private _version;
        private _valueAsString;
        private _valueAsBoolean;
        private _valueAsNumber;
        private _valueAsDate;
        /**
        * Raised when the Property's value has changed.
        */
        public ValueChanged: IPropertyValueChangedEvent;
        constructor(source: any);
        public Value : KnockoutObservable<any>;
        public ValueAsString : KnockoutComputed<string>;
        public ValueAsBoolean : KnockoutComputed<boolean>;
        public ValueAsNumber : KnockoutComputed<number>;
        public ValueAsDate : KnockoutComputed<Date>;
        public Version : KnockoutObservable<number>;
        public Update(source: any): void;
        public ToData(): any;
    }
}
declare module NXKit.Web {
    interface IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
    }
    class TypedEvent implements IEvent {
        public _listeners: any[];
        public add(listener: () => void): void;
        public remove(listener?: () => void): void;
        public trigger(...a: any[]): void;
    }
}
declare module NXKit.Web.Utils {
    /**
    * Generates a unique identifier.
    */
    function GenerateGuid(): string;
    /**
    * Returns the entire context item chain from the specified context upwards.
    */
    function GetContextItems(context: KnockoutBindingContext): any[];
    /**
    * Gets the layout manager in scope of the given binding context.
    */
    function GetLayoutManager(context: KnockoutBindingContext): LayoutManager;
    /**
    * Gets the recommended view model for the given binding information.
    */
    function GetTemplateViewModel(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): any;
    /**
    * Extracts template index data from the given binding information.
    */
    function GetTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext): any;
    /**
    * Determines the named template from the given extracted data and context.
    */
    function GetTemplateName(bindingContext: KnockoutBindingContext, data: any): string;
}
declare module NXKit.Web {
    interface ICallbackRequestEvent extends IEvent {
        add(listener: (data: any) => void): void;
        remove(listener: (data: any) => void): void;
        trigger(data: any): void;
    }
    class View {
        public _body: HTMLElement;
        public _data: any;
        public _root: Visual;
        public _bind: boolean;
        /**
        * Raised when the Visual has changes to be pushed to the server.
        */
        public CallbackRequest: ICallbackRequestEvent;
        constructor(body: HTMLElement);
        public Body : HTMLElement;
        public Data : any;
        /**
        * Initiates a refresh of the view model.
        */
        public Update(): void;
        /**
        * Invoked when the view model initiates a request to push updates.
        */
        public OnCallbackRequest(): void;
        /**
        * Applies the bindings to the view if possible.
        */
        public ApplyBindings(): void;
    }
}
declare module NXKit.Web {
    class VisualLayoutManager extends LayoutManager {
        private _visual;
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Visual : Visual;
    }
}
declare module NXKit.Web {
    class VisualViewModel {
        static GetUniqueId(visual: Visual): string;
        private _context;
        private _visual;
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Context : KnockoutBindingContext;
        public Visual : Visual;
        public UniqueId : string;
    }
}
declare module NXKit.Web.XForms {
    class VisualViewModel extends VisualViewModel {
        static ControlVisualTypes: string[];
        static MetadataVisualTypes: string[];
        static GetValueAsString(visual: Visual): KnockoutComputed<string>;
        static GetRelevant(visual: Visual): KnockoutComputed<boolean>;
        static GetAppearance(visual: Visual): KnockoutComputed<string>;
        static IsMetadataVisual(visual: Visual): boolean;
        static GetLabel(visual: Visual): Visual;
        static GetHelp(visual: Visual): Visual;
        static GetHint(visual: Visual): Visual;
        static GetAlert(visual: Visual): Visual;
        static IsControlVisual(visual: Visual): boolean;
        static HasControlVisual(visual: Visual): boolean;
        static GetControlVisuals(visual: Visual): Visual[];
        static GetContents(visual: Visual): Visual[];
        constructor(context: KnockoutBindingContext, visual: Visual);
        public ValueAsString : KnockoutComputed<string>;
        public Relevant : KnockoutComputed<boolean>;
        public Appearance : KnockoutComputed<string>;
        public Label : Visual;
        public Help : Visual;
        public Contents : Visual[];
    }
}
declare module NXKit.Web.XForms {
    class GroupGridViewModel extends GroupViewModel {
        private _count;
        constructor(context: KnockoutBindingContext, visual: Visual, count: number);
        /**
        * Gets the visuals laid out in column order.
        */
        public Columns : Visual[][];
    }
}
declare module NXKit.Web.XForms {
    class GroupLayoutManager extends VisualLayoutManager {
        private _viewModel;
        private _groupParent;
        constructor(context: KnockoutBindingContext, viewModel: GroupViewModel);
        public ParseTemplateBinding(valueAccessor: KnockoutObservable<any>, viewModel: any, bindingContext: KnockoutBindingContext, data: any): any;
        public ViewModel : GroupViewModel;
        public GroupParent : GroupLayoutManager;
        public Level : number;
    }
}
declare module NXKit.Web.XForms {
    class GroupLevel1LayoutManager extends VisualLayoutManager {
        constructor(context: KnockoutBindingContext, viewModel: GroupViewModel);
        public Layout : string;
    }
}
declare module NXKit.Web.XForms {
    class GroupViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms {
    class LabelViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Text : KnockoutComputed<string>;
    }
}
declare module NXKit.Web.XForms.Layout {
    class FormLayoutManager extends VisualLayoutManager {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms.Layout {
    class FormViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms.Layout {
    class PageViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms.Layout {
    class ParagraphViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms.Layout {
    class SectionViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
declare module NXKit.Web.XForms {
    class Select1ViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
