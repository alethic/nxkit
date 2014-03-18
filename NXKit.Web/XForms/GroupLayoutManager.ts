/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="VisualViewModel.ts" />

module NXKit.Web.XForms {

    export enum GroupLayout {

        Fluid  = 1,
        Single = 2,
        Double = 3,
        Expand = 4,

    }

    export interface IGroupLayoutFunc {
        (): GroupLayout;
    }

    export class GroupLayoutItem {

        constructor() {

        }

    }

    export class GroupLayoutItemGroup extends GroupLayoutItem {

        _getLayout: IGroupLayoutFunc;

        constructor(getLayout: IGroupLayoutFunc) {
            super();
            this._getLayout = getLayout;
        }

        get Layout(): GroupLayout {
            return this._getLayout();
        }

    }

    export class GroupLayoutSingleItemGroupFromGroup extends GroupLayoutItem {

        _visual: Visual;

        constructor(visual: Visual) {
            super();
            this._visual = visual;
        }

        get Visual(): Visual {
            return this._visual;
        }

    }

    export class GroupLayoutManager extends NXKit.Web.XForms.VisualViewModel {

        constructor(context: KnockoutBindingContext, visual: Visual) {
            super(context, visual);
        }

        get Level(): number {
            var ctx = this.Context;
            while ((ctx = ctx.$parentContext) != null)
                if (ctx.$data instanceof GroupLayoutManager)
                    return (<GroupLayoutManager>ctx.$data).Level + 1;

            return 1;
        }

        get Layout(): GroupLayout {
            var l = this.Level;
            var a = this.Appearance();

            if (l == 1 && a == "full")
                return GroupLayout.Fluid;
            if (l == 1)
                return GroupLayout.Fluid;

            if (l == 2 && a == "full")
                return GroupLayout.Fluid;
            if (l == 2)
                return GroupLayout.Fluid;

            return GroupLayout.Single;
        }

    }

}
