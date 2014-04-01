/// <reference path="LayoutNodeViewModel.ts" />

module NXKit.Web.XForms.Layout {

    export class FormViewModel
        extends LayoutNodeViewModel {

        private _rootStep: FormUtil.Step;
        private _activeStep: KnockoutObservable<FormUtil.Step>;

        public StepChanged: IStepChangedEvent = new TypedEvent();

        constructor(context: KnockoutBindingContext, node: Node) {
            super(context, node);
            var self = this;

            self._activeStep = ko.observable<FormUtil.Step>();
            self._rootStep = new FormUtil.Step(node, null, _ => _ === self._activeStep(), _ => self._activeStep(_));
            self._activeStep(self.GetNextStep(self._rootStep));
        }

        public get RootStep(): FormUtil.Step {
            return this._rootStep;
        }

        public get ActiveStep(): KnockoutObservable<FormUtil.Step> {
            return this._activeStep;
        }

        GetPreviousStep(step: FormUtil.Step): FormUtil.Step {
            var self = this;
            if (step.Parent != null) {
                for (var i = step.Parent.Steps.indexOf(step) - 1; i >= 0; i--) {
                    if (!step.Parent.Steps[i].Disabled()) {
                        return step.Parent.Steps[i];
                    }
                }
            }

            return null;
        }

        public get HasPreviousStep(): KnockoutObservable<boolean> {
            var self = this;
            return ko.computed(() => self.GetPreviousStep(self.ActiveStep()) != null);
        }

        public GoPreviousStep(): void {
            var self = this;
            var p = self.GetPreviousStep(self.ActiveStep());
            if (p != null) {
                self.ActiveStep(p);
                self.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                    type: 'xforms-layout-step-previous',
                });
            }
        }

        GetNextStep(step: FormUtil.Step): FormUtil.Step {
            var self = this;

            // if step has children
            if (step.Steps.length > 0) {
                var s = step.Steps[0];
                return !s.Disabled() ? s : self.GetNextStep(s);
            }

            if (step.Parent != null) {
                for (var i = step.Parent.Steps.indexOf(step) + 1; i < step.Parent.Steps.length; i++) {
                    if (!step.Parent.Steps[i].Disabled()) {
                        return step.Parent.Steps[i];
                    }
                }
            }

            return null;
        }

        public get HasNextStep(): KnockoutObservable<boolean> {
            var self = this;
            return ko.computed(() => self.GetNextStep(self.ActiveStep()) != null);
        }

        public GoNextStep(): void {
            var self = this;
            var p = self.GetNextStep(self.ActiveStep());
            if (p != null) {
                self.ActiveStep(p);
                self.Node.Invoke('NXKit.DOMEvents.INXEventTarget', 'DispatchEvent', {
                    type: 'xforms-layout-step-next',
                });
            }
        }

    }

}