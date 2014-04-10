module NXKit.Web.ViewModelUtil {

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
      * Returns true if the given node is a control node.
      */
    export function IsGroupNode(node: Node): boolean {
        return GroupNodes.some(_ => node.Name == _);
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
        return ControlNodes.some(_ => node.Name == _);
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
        return MetadataNodes.some(_ => node.Name == _);
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
        return TransparentNodes.some(_ => node.Name == _);
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
