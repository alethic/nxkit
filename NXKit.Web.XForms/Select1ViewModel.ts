/// <reference path="XFormsVisualViewModel.ts" />

module NXKit.Web.XForms {

    export class Select1ViewModel
        extends NXKit.Web.XForms.XFormsVisualViewModel {

        public static GetSelectedItemVisualId(visual: Visual): KnockoutComputed<string> {
            return ko.computed<string>({
                read: () => {
                    if (visual != null &&
                        visual.Properties['SelectedItemVisualId'] != null)
                        return visual.Properties['SelectedItemVisualId'].ValueAsString();
                    else
                        return null;
                },
                write: _ => {
                    if (visual != null &&
                        visual.Properties['SelectedItemVisualId'] != null)
                        visual.Properties['SelectedItemVisualId'].Value(_);
                },
            });
        }

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        public get SelectedItemVisualId(): KnockoutComputed<string> {
            return Select1ViewModel.GetSelectedItemVisualId(this.Visual);
        }

    }

}