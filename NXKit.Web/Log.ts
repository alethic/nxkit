module NXKit.Web {

    export class Log {

        public static Group<T>(title?: string, func?: () => T): any {

            // start group
            if (typeof console.group === 'function')
                console.group(title);

            var result = func != null ? func() : null;
            console.dir(result);

            // end group
            if (typeof console.groupEnd === 'function')
                console.groupEnd();

            return result;
        }

    }

}