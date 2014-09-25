module NXKit.View {

    export class Log {

        public static Verbose: boolean = false;

        public static Group<T>(title?: string, func?: () => T): any {

            // start group
            if (typeof console.group === 'function')
                if (Log.Verbose)
                    console.group(title);

            var result = func != null ? func() : null;
            if (Log.Verbose)
                console.dir(result);

            // end group
            if (typeof console.groupEnd === 'function')
                if (Log.Verbose)
                    console.groupEnd();

            return result;
        }

        public static Debug(message: string, ...args: any[]) {
            if (Log.Verbose)
                console.debug(message, args);
        }

        public static Object(object: any, ...args: any[]) {
            if (Log.Verbose)
                console.dir(object, args);
        }

    }

}