module NXKit.Web {

    export class TemplateManager {

        static _baseUrl: string = '/Content/';

        static Register(name: string) {
            var self = this;

            ViewDeferred.Push(promise => {
                $(document).ready(() => {
                    var div = $(document.createElement('div'))
                        .css('display', 'none')
                        .load(self._baseUrl + name, () => {
                            $('body').append(div);
                            promise.resolve();
                        });
                });
            });
        }

    }

}