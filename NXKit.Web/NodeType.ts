module NXKit.Web {

    export class NodeType {

        public static Object: NodeType = new NodeType("object");
        public static Text: NodeType = new NodeType("text");
        public static Element: NodeType = new NodeType("element");

        private _value: string;

        public static Parse(value: string): NodeType {
            switch (value.toLowerCase()) {
                case 'text':
                    return NodeType.Text;
                case 'element':
                    return NodeType.Element;
            }

            return NodeType.Object;
        }

        constructor(value: string) {
            this._value = value;
        }

        public ToString(): string {
            return this._value;
        }

    }

}