/// <reference path="DeferredExecutor.ts"/>

module NXKit.View {

    /**
    * Manages a set of templates that are injected into the document upon request.
    * @class NXKit.Web.TemplateManager
    */
    export class TemplateManager {

        /**
         * Global TemplateManager instance.
         */
        public static Default: TemplateManager;

        private _executor: DeferredExecutor = new DeferredExecutor();
        private _baseUrl: string;

        /**
         * Initializes a new instance.
         */
        constructor(baseUrl: string) {
            var self = this;

            self._baseUrl = baseUrl;
        }

        /**
         * Registers a given template name to be retrieved and injected into the page when required.
         * @method Register
         */
        public Register(name: string) {
            var self = this;

            self._executor.Register(promise => {
                $(document).ready(() => {
                    var div1 = $('body>*[nx-template-container]');
                    if (div1.length == 0)
                        div1 = $(document.createElement('div'))
                            .attr('nx-template-container', '')
                            .css('display', 'none')
                            .prependTo('body');
                    var div2 = $(document.createElement('div'))
                        .attr('nx-template-url', self._baseUrl + name)
                        .load(self._baseUrl + name, () => {
                            $(div1).append(div2);
                            promise.resolve();
                        });
                });
            });
        }

        /**
         * Ensures the templates are registered before invoking the callback.
         * @method Register
         */
        public Wait(cb: () => void) {
            var self = this;

            self._executor.Wait(cb);
        }

    }

    TemplateManager.Default = new TemplateManager('/Content/');
    TemplateManager.Default.Register('nxkit.html');

}