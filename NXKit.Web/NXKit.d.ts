/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
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
declare module NXKit.Web {
    interface IPropertyValueChangedEvent extends IEvent {
        add(listener: (property: Property) => void): void;
        remove(listener: (property: Property) => void): void;
        trigger(property: Property): void;
    }
    class Property {
        private _value;
        private _version;
        private _valueAsBoolean;
        private _valueAsNumber;
        private _valueAsDate;
        /**
        * Raised when the Property's value has changed.
        */
        public ValueChanged: IPropertyValueChangedEvent;
        constructor(source: any);
        public Value : KnockoutObservable<any>;
        public ValueAsBoolean : KnockoutComputed<boolean>;
        public ValueAsNumber : KnockoutComputed<number>;
        public ValueAsDate : KnockoutComputed<Date>;
        public Version : KnockoutObservable<number>;
        public Update(source: any): void;
        public ToData(): any;
    }
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
    interface IVisualPropertyValueChangedEvent extends IEvent {
        add(listener: (visual: Visual, property: Property) => void): void;
        remove(listener: (visual: Visual, property: Property) => void): void;
        trigger(visual: Visual, property: Property): void;
    }
    class Visual {
        public _type: string;
        public _baseTypes: string[];
        public _properties: Property[];
        public _visuals: KnockoutObservableArray<Visual>;
        /**
        * Raised when the Visual has changes to be pushed to the server.
        */
        public ValueChanged: IVisualPropertyValueChangedEvent;
        /**
        * Initializes a new instance from the given initial data.
        */
        constructor(source: any);
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
        public Properties : any;
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
        /**
        * Gets the template that should be used to render this Visual.
        */
        public Template : any;
    }
}
declare module NXKit.Web.XForms {
    class VisualViewModel extends VisualViewModel {
        static ControlVisualTypes: string[];
        static MetadataVisualTypes: string[];
        static GetAppearance(visual: any): string;
        static IsMetadataVisual(visual: any): boolean;
        static GetLabel(visual: any): Visual;
        static GetHelp(visual: any): Visual;
        static GetHint(visual: any): Visual;
        static GetAlert(visual: any): Visual;
        static IsControlVisual(visual: any): boolean;
        static HasControlVisual(visual: any): boolean;
        static GetControlVisuals(visual: any): KnockoutObservableArray<Visual>;
        static GetContents(visual: any): KnockoutObservableArray<Visual>;
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Label : Visual;
        public Help : Visual;
        public Contents : KnockoutObservableArray<Visual>;
        public Appearance : string;
    }
}
declare module NXKit.Web {
    class VisualViewModel {
        static GetUniqueId(visual: any): string;
        private _context;
        private _visual;
        constructor(context: KnockoutBindingContext, visual: Visual);
        public Context : KnockoutBindingContext;
        public Visual : Visual;
        public UniqueId : string;
    }
}
declare module NXKit.Web.XForms {
    class GroupViewModel extends VisualViewModel {
        constructor(context: KnockoutBindingContext, visual: Visual);
    }
}
