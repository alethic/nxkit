declare module NXKit.Web.XForms {
    class LayoutNodeViewModel extends XFormsNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class FormViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class IconViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Name : KnockoutObservable<string>;
    }
}
declare module NXKit.Web.XForms.Layout {
    class ItemViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public LabelAppearance : KnockoutObservable<string>;
        public Count : number;
        public CountEnabled : number;
    }
}
declare module NXKit.Web.XForms.Layout {
    class ListViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Items : Node[];
    }
}
declare module NXKit.Web.XForms.Layout {
    class StrongViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class ParagraphViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class SegmentViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
    }
}
declare module NXKit.Web.XForms.Layout {
    class TableCellViewModel extends LayoutNodeViewModel {
        constructor(context: KnockoutBindingContext, node: Node);
        public Activate(): void;
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
declare module NXKit.Web.XForms.Layout.FormUtil {
}
