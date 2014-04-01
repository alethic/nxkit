module NXKit.Web.XForms.Layout.Utils {

    export function GetPages(node: Node): Node[] {
        var l = node.Nodes();
        var r = new Array<Node>();
        for (var i = 0; i < l.length; i++) {
            var v = l[i];
            if (v.Type !== 'NXKit.XForms.Layout.Page') {
                var s = GetPages(v);
                for (var j = 0; j < s.length; j++)
                    r.push(s[j]);
            } else {
                r.push(v);
            }
        }

        return r;
    }

}