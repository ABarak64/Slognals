using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slognals
{
    /// <summary>
    /// Keeps track of all tting to a particular signal/slo connection so that they can later be disconnected or disposed of.
    /// </summary>
    internal class Unsubscriber
    {
        public readonly Delegate _slot;
        public readonly IDisposable _streamSubscription;

        /// <summary>
        /// This constructor simply fills its fields with the passed values.
        /// </summary>
        /// <param name="slot">The method that has been subscribed to our signal.</param>
        /// <param name="subscription">IDisposable allowing us to dispose of the unmanaged resources of this connection.</param>
        public Unsubscriber(Delegate slot, IDisposable subscription)
        {
            this._slot = slot;
            this._streamSubscription = subscription;
        }
    }
}
