/// <reference path="TypedEvent.ts" />

module NXKit.Web {

    export class Message {

        private _severity: string;
        private _text: string;

        constructor(severity: string, text: string) {
            var self = this;

            this._severity = severity;
            this._text = text;
        }

        public get Severity(): string {
            return this._severity;
        }

        public get Text(): string {
            return this._text;
        }

    }

}