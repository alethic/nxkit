/// <reference path="Scripts/typings/jquery/jquery.d.ts" />

module NXKit.Web {

    export class View {

        _element: HTMLElement;
        _visual: Object;

        constructor(element: HTMLElement) {
            this._element = element;
        }

        public get element(): HTMLElement {
            return this._element;
        }

        public set element(value: HTMLElement) {
            this._element = value;
        }

        public  get visual(): Object {
            return this._visual;
        }

        public   set visual(value: Object) {
            console.log('logged');
            this._visual = value;
        }

    }

}