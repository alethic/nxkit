module NXKit.View {

    class DeferredExecutorItem {

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

    /**
     * Executes a series of callbacks once for the first waiter.
     * @class NXKit.Web.DeferredExecutor
     */
    export class DeferredExecutor {

        private _queue: DeferredExecutorItem[] = new Array<DeferredExecutorItem>();
        
        /**
         * Registers a callback to be executed. The callback is passed a JQueryDeferred that it can resolve upon
         * completion.
         * @method Register
         */
        public Register(cb: (promise: JQueryDeferred<void>) => void) {
            var self = this;

            self._queue.push(new DeferredExecutorItem(cb));
        }
        
        /**
         * Invokes the given callback when the registered callbacks are completed.
         * @method Wait
         */
        public Wait(cb: () => void) {
            var self = this;

            var wait = new Array<JQueryPromise<void>>();
            for (var i = 0; i < self._queue.length; i++)
                wait[i] = self._queue[i].Promise;

            $.when.apply($, wait).done(cb);
        }

    }

}