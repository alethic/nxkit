/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="LayoutManager.ts" />

module NXKit.Web {

    export class DefaultLayoutManager
        extends LayoutManager {

        constructor(context: KnockoutBindingContext) {
            super(context);
        }

        GetTemplates_Test_Value(name: string, data1: JQuery, data2: any) {
            var value1 = data1.data('nxkit-' + name) || null;
            var value2 = data2[name] || null;
            return value1 == value2;
        }

        public GetTemplates(data: any): HTMLElement[] {
            var self = this;
            return <HTMLElement[]>$('script[type="text/html"]').filter(function (): boolean {

                for (var i in data)
                    if (!self.GetTemplates_Test_Value(i, $(this), data))
                        return false;

                for (var n = 0; n < (<HTMLElement>this).attributes.length; n++) {
                    var attr = (<HTMLElement>this).attributes.item(n);
                    if (attr.nodeName.indexOf('data-nxkit-') == 0)
                        if (!self.GetTemplates_Test_Value(attr.nodeName.substring(11), $(this), data))
                            return false;
                }

                return true;

            }).toArray();
        }

    }

}
