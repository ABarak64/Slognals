using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slognals
{
    /* --- UNSUBSCRIBER - This class keeps track of all the info relating to a particular signal/slot
     *                    connection so that they can later be disconnected or disposed of.
     */

    internal class Unsubscriber
    {
        public readonly Delegate _slot;
        public readonly IDisposable _streamSubscription;

        /* --- UNSUBSCRIBER - This constructor simply fills its fields with the passed values.
         *            TAKES - Delegate representing the method that has been subscribed to our signal.
         *                  - IDisposable allowing us to dispose of the unmanaged resources of this connection.
         */

        public Unsubscriber(Delegate slot, IDisposable subscription)
        {
            this._slot = slot;
            this._streamSubscription = subscription;
        }
    }
}
