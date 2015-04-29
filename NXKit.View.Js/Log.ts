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

        static InvokeLogMethod(func: (m: string, ...args: any[]) => void, message: string, args: any[]) {
            // add message as first arg
            args.unshift(message);

            // invoke function
            if (typeof func === 'function') {
                func.apply(console, args);
            } else if (typeof console.log.apply === 'function') {
                console.log.apply(console, args);
            }
        }

        public static Debug(message: string, ...args: any[]) {
            this.InvokeLogMethod(console.debug, message, args);
        }

        public static Information(message: string, ...args: any[]) {
            this.InvokeLogMethod(console.info, message, args);
        }

        public static Warning(message: string, ...args: any[]) {
            this.InvokeLogMethod(console.warn, message, args);
        }

        public static Error(message: string, ...args: any[]) {
            this.InvokeLogMethod(console.error, message, args);
        }

        public static Object(object: any, ...args: any[]) {
            if (typeof console.dir === 'function') {
                console.dir(object, args);
            }
        }

    }

}