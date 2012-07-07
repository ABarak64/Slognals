using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Slognals
{
    /// <summary>
    /// One of the numerous child classes of the abstract Signal. It maintains the specialized functionality needed for 
    /// a signal that emits only a single value.
    /// </summary>
    /// <typeparam name="T">Whatever type the signal is transmitting.</typeparam>
    public class Signal<T> : Signal
    {
        /// <summary>
        /// This constructor simply calls the base class constructor and injects the particular Subject<T> needed for 
        /// this child class of Signal.
        /// </summary>
        public Signal() : base(new Subject<Tuple<T>>()) { }

        /// <summary>
        /// This property does the casting necessary to access the _stream field as the appropriate type for 
        /// this particular Signal child class. Necessary to allowing the IDisposable of the _stream to be kept in 
        /// the base class so that all of the disposable functionality can be implemented there.
        /// </summary>
        protected Subject<Tuple<T>> Stream
        {
            get
            {
                return (Subject<Tuple<T>>)_stream;
            }
        }

        /// <summary>
        /// This method does the actual stream subscribing of the slot, as well as forces the user to match only 
        /// appropriate signals and slots (those with the same set of values at compile time.
        /// </summary>
        /// <param name="signal">The particular single-value signal we want to connect.</param>
        /// <param name="slot">The void-returning method we wish to subscribe to the signal.</param>
        /// <param name="condition">Acts as a Where() filter for subscriptions.</param>
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

        /// <summary>
        /// This method exists at this child class level only to enforce the matching of appropriate signal and slot 
        /// types to each other.
        /// </summary>
        /// <param name="signal">The particular single-value signal we want to disconnect.</param>
        /// <param name="slot">The void-returning method we wish to unsubscribe from the signal.</param>
        public static void disconnect(Signal<T> signal, Action<T> slot)
        {
            signal.disconnect(slot);
        }

        /// <summary>
        /// This method takes advantage of the Subject<T>'s methods to transmit a value for a particular signal.
        /// </summary>
        /// <param name="val">The value the user wishes to emit from their signal to all connected slots.</param>
        public void emit(T val)
        {
            Stream.OnNext(new Tuple<T>(val));   // send off a new value to pass into the slots currently subscribed.
        }
    }

    /// <summary>
    /// One of the numerous child classes of the abstract Signal. It maintains the specialized functionality needed
    /// for a signal that emits two values.
    /// </summary>
    /// <typeparam name="T">First type to be emitted in the signal.</typeparam>
    /// <typeparam name="U">Second type to be emitted in the signal.</typeparam>
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

    /// <summary>
    /// One of the numerous child classes of the abstract Signal. It maintains the specialized functionality needed 
    /// for a signal that emits three values.
    /// </summary>
    /// <typeparam name="T">First type to be emitted in the signal.</typeparam>
    /// <typeparam name="U">Second type to be emitted in the signal.</typeparam>
    /// <typeparam name="V">Third type to be emitted in the signal.</typeparam>
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

    /// <summary>
    /// One of the numerous child classes of the abstract Signal. It maintains the specialized functionality needed for a signal that emits four values.
    /// </summary>
    /// <typeparam name="T">First type to be emitted in the signal.</typeparam>
    /// <typeparam name="U">Second type to be emitted in the signal.</typeparam>
    /// <typeparam name="V">Third type to be emitted in the signal.</typeparam>
    /// <typeparam name="W">Fourth type to be emitted in the signal.</typeparam>
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
