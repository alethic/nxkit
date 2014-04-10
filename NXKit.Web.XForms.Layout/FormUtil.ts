module NXKit.Web.XForms.Layout.FormUtil {

    /**
      * Node types which represent a grouping element.
      */
    export var StepNodes: string[] = [
        '{http://schemas.nxkit.org/2014/xforms-layout}section',
    ];

    /**
      * Returns true if the given node is a step node.
      */
    export function IsStepNode(node: Node): boolean {
        return StepNodes.some(_ => node.Name == _);
    }

    /**
      * Returns true if the given node set contains a step node.
      */
    export function HasStepNodes(nodes: Node[]): boolean {
        return nodes.some(_ => IsStepNode(_));
    }

    /**
      * Filters out the given node set for step nodes.
      */
    export function GetStepNodes(nodes: Node[]): Node[] {
        return nodes.filter(_ => IsStepNode(_));
    }

    /**
      * Represents a sub-item of a top-level group.
      */
    export class Step {

        private _node: Node;
        private _parent: Step;
        private _isActive: (step: Step) => boolean;
        private _setActive: (step: Step) => void;
        private _steps: Step[];
        private _active: KnockoutObservable<boolean>;
        private _disabled: KnockoutObservable<boolean>;

        constructor(node: Node, parent: Step, isActive: (step: Step) => boolean, setActive: (step: Step) => void) {
            try {
                var self = this;
                self._node = node;
                self._parent = parent;
                self._isActive = isActive;
                self._setActive = setActive;
                self._active = ko.computed(() => self._isActive(self));
                self._disabled = ko.computed(() => !ViewModelUtil.GetRelevant(self._node)());
                self._steps = GetSteps(node, self, isActive, setActive);
            } catch (ex) {
                ex.message = "FormUtil:Step.ctor()" + '\nMessage: ' + ex.message;
                throw ex;
            }
        }

        public get Node(): Node {
            return this._node;
        }

        public get Parent(): Step {
            return this._parent;
        }

        public get Steps(): Step[] {
            return this._steps;
        }

        public get Label(): Node {
            return ViewModelUtil.GetLabelNode(this._node);
        }

        public get Active(): KnockoutObservable<boolean> {
            return this._active;
        }

        public get Disabled(): KnockoutObservable<boolean> {
            return this._disabled;
        }

        public SetActive() {
            this._setActive(this);
        }

    }

    /**
      * Converts each node into a step item.
      */
    function GetSteps(node: Node, parent: Step, isActive: (step: Step) => boolean, setActive: (step: Step) => void): Step[] {
        return node.Nodes()
            .filter(_ => IsStepNode(_))
            .map(_ => new Step(_, parent, isActive, setActive));
    }

}
