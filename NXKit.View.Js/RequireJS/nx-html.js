NXKit.define('nx-html', ['jquery'], function ($) {
    return {
        load: function (name, parentRequire, onload, config) {
            parentRequire(['jquery'], function ($) {
                var div1 = $('body>*[data-nx-html-host]');
                if (div1.length == 0)
                    div1 = $(document.createElement('div'))
                        .attr('data-nx-html-host', '')
                        .css('display', 'none')
                        .prependTo('body');
                if (div1.children("[data-nx-require='" + name + "']").length == 0) {
                    var div2 = $(document.createElement('div'))
                        .attr('data-nx-require', name)
                        .appendTo(div1)
                        .load(_NXKit.View.Web.UI.handlerUrl + '?m=' + name, function () {
                            onload(div2.get(0));
                        });
                }
            });
        },
    }
});