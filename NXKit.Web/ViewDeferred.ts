module NXKit.Web {

    export class ViewDeferredTuple {

        callback: (promise: JQueryDeferred<void>) => void
        deferred: JQueryDeferred<void>;

        constructor(cb: (deferred: JQueryDeferred<void>) => void) {
            this.callback = cb;
        }

        public get Promise(): JQueryPromise<void> {
            var self = this;

            // promise already generated
            if (self.deferred != null)
                return self.deferred.promise();

            // generate promise and begin execution
            self.deferred = $.Deferred<void>();
            self.callback(self.deferred);

            // return new promise
            return self.deferred.progress();
        }

    }

    export class ViewDeferred {

        static _queue: ViewDeferredTuple[] = new Array<ViewDeferredTuple>();

        static Push(cb: (promise: JQueryDeferred<void>) => void) {
            var self = this;

            self._queue.push(new ViewDeferredTuple(cb));
        }

        static Wait(cb: () => void) {
            var self = this;

            var wait = new Array<JQueryPromise<void>>();
            for (var i = 0; i < self._queue.length; i++)
                wait[i] = self._queue[i].Promise;

            $.when.apply($, wait).done(cb);
        }

    }

}