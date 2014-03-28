module NXKit.Web.Utils {

    /**
     * Tests two objects for equality.
     */
    export function DeepEquals(a: any, b: any): boolean {

        if (a == null &&
            b === null)
            return true;

        if (typeof a !== typeof b)
            return false;

        if (typeof a === 'boolean' &&
            typeof b === 'boolean')
            return a === b;

        if (typeof a === 'string' &&
            typeof b === 'string')
            return a === b;

        if (typeof a === 'number' &&
            typeof b === 'number')
            return a === b;

        if (typeof a === 'function' &&
            typeof b === 'function')
            return a.toString() === b.toString();

        for (var i in a) {
            if (a.hasOwnProperty(i)) {
                if (!b.hasOwnProperty(i))
                    return false;
                if (!Utils.DeepEquals(a[i], b[i]))
                    return false;
            }
        }

        for (var i in b) {
            if (b.hasOwnProperty(i)) {
                if (!a.hasOwnProperty(i))
                    return false;
                if (!Utils.DeepEquals(b[i], a[i]))
                    return false;
            }
        }

        return true;
    }

    /**
     * Generates a unique identifier.
     */
    export function GenerateGuid(): string {
        // http://www.ietf.org/rfc/rfc4122.txt

        var s: Array<string> = [];
        var d = "0123456789abcdef";
        for (var i = 0; i < 36; i++) {
            s[i] = d.substr(Math.floor(Math.random() * 0x10), 1);
        }
        s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
        s[19] = d.substr((<any>s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
        s[8] = s[13] = s[18] = s[23] = "-";

        return s.join("");
    }

    /**
      * Gets the unique document ID of the given node.
      */
    export function GetUniqueId(node: Node): string {
        return node.Property('NXKit.NXElement', 'UniqueId').ValueAsString();
    }

    /**
     * Returns the entire context item chain from the specified context upwards.
     */
    export function GetContextItems(context: KnockoutBindingContext): any[] {
        return [context.$data].concat(context.$parents);
    }

    /**
     * Gets the layout manager in scope of the given binding context.
     */
    export function GetLayoutManager(context: KnockoutBindingContext): LayoutManager {
        return <LayoutManager>ko.utils.arrayFirst(GetContextItems(context), _ => _ instanceof LayoutManager);
    }

}
