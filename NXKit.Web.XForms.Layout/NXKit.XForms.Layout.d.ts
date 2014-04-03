declare module NXKit.Web.XForms {
    class LayoutNodeViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class FormViewModel extends LayoutNodeViewModel {
        private _rootStep;
        private _activeStep;
        public StepChanged: IStepChangedEvent;
        constructor(context: KnockoutBindingContext, node: Node);
        public RootStep : FormUtil.Step;
        public ActiveStep : KnockoutObservable<FormUtil.Step>;
        public GetPreviousStep(step: FormUtil.Step): FormUtil.Step;
        public HasPreviousStep : KnockoutObservable<boolean>;
        public GoPreviousStep(): void;
        public GetNextStep(step: FormUtil.Step): FormUtil.Step;
        public HasNextStep : KnockoutObservable<boolean>;
        public GoNextStep(): void;
    }
}
declare module NXKit.Web {
    interface IStepChangedEvent extends IEvent {
        add(listener: () => void): void;
        remove(listener: () => void): void;
        trigger(...a: any[]): void;
    }
}
declare module NXKit.Web.XForms.Layout {
    class ParagraphViewModel extends NodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class TableViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class SectionViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout.Utils {
    function GetPages(node: Node): Node[];
}
declare module NXKit.Web.XForms.Layout.FormUtil {
    /**
    * Node types which represent a grouping element.
    */
    var StepNodeTypes: string[];
    /**
    * Returns true if the given node is a step node.
    */
    function IsStepNode(node: Node): boolean;
    /**
    * Returns true if the given node set contains a step node.
    */
    function HasStepNodes(nodes: Node[]): boolean;
    /**
    * Filters out the given node set for step nodes.
    */
    function GetStepNodes(nodes: Node[]): Node[];
    /**
    * Represents a sub-item of a top-level group.
    */
    class Step {
        private _node;
        private _parent;
        private _isActive;
        private _setActive;
        private _steps;
        private _active;
        private _disabled;
        constructor(node: Node, parent: Step, isActive: (step: Step) => boolean, setActive: (step: Step) => void);
        public Node : Node;
        public Parent : Step;
        public Steps : Step[];
        public Label : Node;
        public Active : KnockoutObservable<boolean>;
        public Disabled : KnockoutObservable<boolean>;
        public SetActive(): void;
    }
}
