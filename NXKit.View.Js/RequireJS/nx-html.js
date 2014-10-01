NXKit.define(['require', 'jquery'], function (require, $) {
    return {
        load: function (name, parentRequire, onload, config) {
            var div1 = $('body>*[data-nx-html-host]');
            if (div1.length == 0)
                div1 = $(document.createElement('div'))
                    .attr('data-nx-html-host', '')
                    .css('display', 'none')
                    .prependTo('body');
            var div2 = div1.children("[data-nx-require='" + name + "']").get(0);
            if (div2 == null) {
                var url = require.toUrl(name);
                div2 = $(document.createElement('div'))
                    .attr('data-nx-require', name)
                    .appendTo(div1)
                    .load(url, function (response, status) {
                        if (status === 'success' || status === 'notmodified') {
                            onload(div2);
                        } else {
                            onload.error(status);
                        }
                    })
                    .get(0);
            } else {
                onload(div2);
            }
        }
    }
});