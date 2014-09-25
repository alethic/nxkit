module NXKit.View.ViewModelUtil {

    /**
      * Set of functions to inject layout managers at the top of the hierarchy.
      */
    export var LayoutManagers: Array<(context: KnockoutBindingContext) => LayoutManager> = [
        (c) => new DefaultLayoutManager(c),
    ];

    /**
      * Nodes which represent a grouping element.
      */
    export var GroupNodes: string[] = [];

    /**
      * Nodes which are considered to be control elements.
      */
    export var ControlNodes: string[] = [];

    /**
      * Nodes which are considered to be metadata elements for their parents.
      */
    export var MetadataNodes: string[] = [];

    /**
      * Nodes which are considered to be transparent, and ignored when calculating content membership.
      */
    export var TransparentNodes: string[] = [];

    /**
      * Nodes which are considered to be transparent, and ignored when calculating content membership.
      */
    export var TransparentNodePredicates: Array<(n: Node) => boolean> = [
        (n: Node) => TransparentNodes.some(_ => _ === n.Name),
    ];
    
    /**
      * Returns true of the given node is an empty text node.
      */
    export function IsEmptyTextNode(node: Node): boolean {
        return node.Type == NodeType.Text && (node.Value() || '').trim() === '';
    }
    
    /**
      * Returns true if the current node is one that should be completely ignored.
      */
    export function IsIgnoredNode(node: Node): boolean {
        return node == null || IsEmptyTextNode(node);
    }

    /**
      * Returns true if the given node is a control node.
      */
    export function IsGroupNode(node: Node): boolean {
        return !IsIgnoredNode(node) && GroupNodes.some(_ => node.Name == _);
    }

    /**
      * Returns true if the given node set contains a control node.
      */
    export function HasGroupNode(nodes: Node[]): boolean {
        return nodes.some(_ => IsGroupNode(_));
    }

    /**
      * Filters out the given node set for control nodes.
      */
    export function GetGroupNodes(nodes: Node[]): Node[] {
        return nodes.filter(_ => IsGroupNode(_));
    }

    /**
      * Returns true if the given node is a control node.
      */
    export function IsControlNode(node: Node): boolean {
        return !IsIgnoredNode(node) && ControlNodes.some(_ => node.Name == _);
    }

    /**
      * Returns true if the given node set contains a control node.
      */
    export function HasControlNode(nodes: Node[]): boolean {
        return nodes.some(_ => IsControlNode(_));
    }

    /**
      * Filters out the given node set for control nodes.
      */
    export function GetControlNodes(nodes: Node[]): Node[] {
        return nodes.filter(_ => IsControlNode(_));
    }

    /**
      * Returns true if the given node is a transparent node.
      */
    export function IsMetadataNode(node: Node): boolean {
        return !IsIgnoredNode(node) && MetadataNodes.some(_ => node.Name == _);
    }

    /**
      * Returns true if the given node set contains a metadata node.
      */
    export function HasMetadataNode(nodes: Node[]): boolean {
        return nodes.some(_ => IsMetadataNode(_));
    }

    /**
      * Filters out the given node set for control nodes.
      */
    export function GetMetadataNodes(nodes: Node[]): Node[] {
        return nodes.filter(_ => IsMetadataNode(_));
    }

    /**
      * Returns true if the given node is a transparent node.
      */
    export function IsTransparentNode(node: Node): boolean {
        return IsIgnoredNode(node) || TransparentNodePredicates.some(_ => _(node));
    }

    /**
      * Returns true if the given node set contains a transparent node.
      */
    export function HasTransparentNode(nodes: Node[]): boolean {
        return nodes.some(_ => IsTransparentNode(_));
    }

    /**
      * Filters out the given node set for transparent nodes.
      */
    export function GetTransparentNodes(nodes: Node[]): Node[] {
        return nodes.filter(_ => IsControlNode(_));
    }

    /**
      * Filters out the given node set for content nodes. This descends through transparent nodes.
      */
    export function GetContentNodes(nodes: Node[]): Node[] {
        try {
            var l = nodes.filter(_ => !IsMetadataNode(_));
            var r = new Array<Node>();
            for (var i = 0; i < l.length; i++) {
                var v = l[i];
                if (v == null) {
                    throw new Error('ViewModelUtil.GetContentNodes(): prospective Node is null');
                }
                if (IsTransparentNode(v)) {
                    var s = GetContentNodes(v.Nodes());
                    for (var j = 0; j < s.length; j++)
                        r.push(s[j]);
                } else {
                    r.push(v);
                }
            }

            return r;
        } catch (ex) {
            ex.message = 'ViewModelUtil.GetContentNodes()' + '"\nMessage: ' + ex.message;
            throw ex;
        }
    }

    /**
      * Gets the content nodes of the given node. This descends through transparent nodes.
      */
    export function GetContents(node: Node): Node[] {
        try {
            return GetContentNodes(node.Nodes());
        } catch (ex) {
            ex.message = 'ViewModelUtil.GetContents()' + '"\nMessage: ' + ex.message;
            throw ex;
        }
    }

}
