using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;

namespace Slognals
{
    /* --- SIGNAL - Main purpose is to keep all common implementation for the various child Signal classes
     *              in here. Contains the important fields for all child Signals and deals with disposal.
     */

    public abstract class Signal : IDisposable
    {
        private readonly List<Unsubscriber> _unsubscriberList = new List<Unsubscriber>();
        protected readonly IDisposable _stream;

        /* --- SIGNAL - This constructor takes an IDisposable that the child class injects so that this
         *              base class can maintain all of the necessary fields for disposing.
         *      TAKES - IDisposable for the injected stream that will later be disposed of in the Dispose()
         *              method of this class.
         */

        public Signal(IDisposable myStream)
        {
            _stream = myStream;
        }

        /* --- DISCONNECT - This method does the bulk of the disconnection work for the child classes, which
         *                  entails disposing of a particular subscription that is being kept track of in the
         *                  _unsubscriberList field.
         *          TAKES - Delegate that represents the method we have previously subscribed to the signal that
         *                  we want to unsubscribe.
         *        RETURNS - Void.
         */

        protected void disconnect(Delegate slot)
        {
                                       // find the unsubscriber entry that matches the passed slot for this signal.
            var unsubMe = from u in this._unsubscriberList
                          where u._slot == slot
                          select u;

            if (unsubMe.Count() > 0)    // if there is an entry that matches.
            {
                // only grab the first one, since we don't want to unsubscribe multiple if they exist.
                var unsubThis = unsubMe.First();
                unsubThis._streamSubscription.Dispose();
                this._unsubscriberList.Remove(unsubThis);
            }
        }

        /* --- CONNECT - This method picks up where the child Signal's connect() left off by keeping track
         *               of the new subscription that was just made.
         *       TAKES - Delegate that represents the method being subscribed to this signal.
         *               IDisposable that represents the disposable that we need in order to unsubscribe it later.
         *     RETURNS - Void.
         */               

        protected void connect(Delegate slot, IDisposable unsub)
        {
                       // add an unsubscriber to the list with relevant info.
            _unsubscriberList.Add(new Unsubscriber(slot, unsub));
        }

        /* --- DISPOSE - This method takes care of disposing all the disposables found within the signal.
         *       TAKES - Void.
         *     RETURNS - Void.
         */

        public void Dispose()
        {                                    // for each Unsubscriber currently in the list.
            foreach (var unsubber in this._unsubscriberList)
            {
                                             // dispose() of each of them.
                unsubber._streamSubscription.Dispose();
            }
            this._unsubscriberList.Clear();  // empty out the list.
            this._stream.Dispose();          // dispose of the stream.
        }
    }
}
