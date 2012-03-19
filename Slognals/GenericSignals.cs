using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Slognals
{
    /* --- SIGNAL<T> - One of the numerous child classes of the abstract Signal. It maintains the specialized
     *                 functionality needed for a signal that emits only a single value.
     */

    public class Signal<T> : Signal
    {

        /* --- SIGNAL - This constructor simply calls the base class constructor and injects the particular
         *              Subject<T> needed for this child class of Signal.
         *      TAKES - Void.
         */

        public Signal() : base(new Subject<Tuple<T>>()) { }

        /* --- STREAM - This property does the casting necessary to access the _stream field as the 
         *              appropriate type for this particular Signal child class. Necessary to allowing
         *              the IDisposable of the _stream to be kept in the base class so that all of the
         *              disposable functionality can be implemented there.
         *      TAKES - Void.
         *    RETURNS - Subject<Tuple<T>> as our protected _stream.
         */

        protected Subject<Tuple<T>> Stream
        {
            get
            {
                return (Subject<Tuple<T>>)_stream;
            }
        }

        /* --- CONNECT - This method does the actual stream subscribing of the slot, as well as forces the
         *               user to match only appropriate signals and slots (those with the same set of values)
         *               at compile time.
         *       TAKES - Signal<T> representing the particular single-value signal we want to connect.
         *             - Action<T> representing the void-returning method we wish to subscribe to the signal.
         *             - Expression<Func<T, bool>> that acts as a Where() filter for subscriptions.
         *     RETURNS - Void.
         */

        public static void connect(Signal<T> signal, Action<T> slot, Expression<Func<T, bool>> condition = null)
        {
            if (condition != null)                 // if user is requesting a Where restriction on the subscription.
            {
                var converter = new ExpressionConverter();

                var convertedCondition = (Expression<Func<Tuple<T>, bool>>)converter.Modify((Expression)condition,
                    Expression.Parameter(typeof(Tuple<T>)), typeof(Func<Tuple<T>, bool>), condition.Parameters);

                signal.connect(slot, signal.Stream.Where(convertedCondition.Compile()).Subscribe(x => slot(x.Item1)));
            }
            else
            {
                signal.connect(slot, signal.Stream.Subscribe(x => slot(x.Item1)));
            }
        }

        /* --- DISCONNECT - This method exists at this child class level only to enforce the matching of
         *                  appropriate signal and slot types to each other.
         *          TAKES - Signal<T> representing the particular single-value signal we want to disconnect.
         *                - Action<T> representing the void-returning method we wish to unsubscribe from the signal.
         *        RETURNS - Void.
         */

        public static void disconnect(Signal<T> signal, Action<T> slot)
        {
            signal.disconnect(slot);
        }

        /* --- EMIT - This method takes advantage of the Subject<T>'s methods to transmit a value for a particular
         *            signal.
         *    TAKES - T representing the value the user wishes to emit from their signal to all connected slots.
         *  RETURNS - Void.
         */

        public void emit(T val)
        {
            Stream.OnNext(new Tuple<T>(val));   // send off a new value to pass into the slots currently subscribed.
        }
    }

    /* --- SIGNAL<T, U> - One of the numerous child classes of the abstract Signal. It maintains the specialized
     *                    functionality needed for a signal that emits two values.
     */

    public class Signal<T, U> : Signal
    {
        public Signal() : base(new Subject<Tuple<T, U>>()) { }

        protected Subject<Tuple<T, U>> Stream
        {
            get
            {
                return (Subject<Tuple<T, U>>)_stream; 
            }
        }

        public static void connect(Signal<T, U> signal, Action<T, U> slot, Expression<Func<T, U, bool>> condition = null)
        {
            if (condition != null)                 
            {
                var converter = new ExpressionConverter();

                var convertedCondition = (Expression<Func<Tuple<T, U>, bool>>)converter.Modify((Expression)condition,
                    Expression.Parameter(typeof(Tuple<T, U>)), typeof(Func<Tuple<T, U>, bool>), condition.Parameters);

                signal.connect(slot, signal.Stream.Where(convertedCondition.Compile()).Subscribe(x => slot(x.Item1, x.Item2)));
            }
            else
            {
                signal.connect(slot, signal.Stream.Subscribe(x => slot(x.Item1, x.Item2)));
            }
        }

        public static void disconnect(Signal<T, U> signal, Action<T, U> slot)
        {
            signal.disconnect(slot);
        }

        public void emit(T val1, U val2)
        {
            this.Stream.OnNext(new Tuple<T, U>(val1, val2));
        }
    }

    /* --- SIGNAL<T, U, V> - One of the numerous child classes of the abstract Signal. It maintains the specialized
     *                       functionality needed for a signal that emits three values.
     */

    public class Signal<T, U, V> : Signal
    {
        public Signal() : base(new Subject<Tuple<T, U, V>>()) { }

        protected Subject<Tuple<T, U, V>> Stream
        {
            get
            {
                return (Subject<Tuple<T, U, V>>)_stream; // needed in order to support the stream being contained in the abstract class.
            }
        }

        public static void connect(Signal<T, U, V> signal, Action<T, U, V> slot, Expression<Func<T, U, V, bool>> condition = null)
        {
            if (condition != null)                
            {
                var converter = new ExpressionConverter();

                var convertedCondition = (Expression<Func<Tuple<T, U, V>, bool>>)converter.Modify((Expression)condition,
                    Expression.Parameter(typeof(Tuple<T, U, V>)), typeof(Func<Tuple<T, U, V>, bool>), condition.Parameters);

                signal.connect(slot, signal.Stream.Where(convertedCondition.Compile()).Subscribe(x => slot(x.Item1, x.Item2, x.Item3)));
            }
            else
            {
                signal.connect(slot, signal.Stream.Subscribe(x => slot(x.Item1, x.Item2, x.Item3)));
            }
        }

        public static void disconnect(Signal<T, U, V> signal, Action<T, U, V> slot)
        {
            signal.disconnect(slot);
        }

        public void emit(T val1, U val2, V val3)
        {
            this.Stream.OnNext(new Tuple<T, U, V>(val1, val2, val3));
        }
    }

    /* --- SIGNAL<T, U, V, W> - One of the numerous child classes of the abstract Signal. It maintains the specialized
     *                          functionality needed for a signal that emits four values.
     */

    public class Signal<T, U, V, W> : Signal
    {
        public Signal() : base(new Subject<Tuple<T, U, V, W>>()) { }

        protected Subject<Tuple<T, U, V, W>> Stream
        {
            get
            {
                return (Subject<Tuple<T, U, V, W>>)_stream;
            }
        }

        public static void connect(Signal<T, U, V, W> signal, Action<T, U, V, W> slot, Expression<Func<T, U, V, W, bool>> condition = null)
        {
            if (condition != null)               
            {
                var converter = new ExpressionConverter();

                var convertedCondition = (Expression<Func<Tuple<T, U, V, W>, bool>>)converter.Modify((Expression)condition,
                    Expression.Parameter(typeof(Tuple<T, U, V, W>)), typeof(Func<Tuple<T, U, V, W>, bool>), condition.Parameters);

                signal.connect(slot, signal.Stream.Where(convertedCondition.Compile()).Subscribe(x => slot(x.Item1, x.Item2, x.Item3, x.Item4)));
            }
            else
            {
                signal.connect(slot, signal.Stream.Subscribe(x => slot(x.Item1, x.Item2, x.Item3, x.Item4)));
            }
        }

        public static void disconnect(Signal<T, U, V, W> signal, Action<T, U, V, W> slot)
        {
            signal.disconnect(slot);
        }

        public void emit(T val1, U val2, V val3, W val4)
        {
            this.Stream.OnNext(new Tuple<T, U, V, W>(val1, val2, val3, val4));
        }
    }
}
