using System;

namespace Settings.Adjustment
{
    public abstract class AdjustableValue<T> where T : struct, IComparable
    {
        public T Value {
            get {
                return GetValue();
            }
        }
        protected T _value;
        protected T _cachedAdjustedValue;
        protected bool _isDirty = true;

        public readonly ValueAdjuster<T> Adjuster;

        public AdjustableValue(ValueAdjuster<T> adjuster)
        {
            Adjuster = adjuster;
            Adjuster.SetParentAdjustableValue(this);
        }

        /// <summary>
        /// Get the value with adjustments made.
        /// </summary>
        /// <returns></returns>
        public virtual T GetValue()
        {
            _cachedAdjustedValue = Adjuster.AdjustValue(_value);
            //_isDirty = false;
            return _cachedAdjustedValue;
        }

        /// <summary>
        /// Get the value with no adjustments.
        /// </summary>
        /// <returns></returns>
        public virtual T GetRawValue()
        {
            return _value;
        }

        public void SetValue(T newValue)
        {
            _value = Value;
            _isDirty = true;
        }

        public void SetDirty()
        {
            _isDirty = true;
        }
    }

    public class AdjustableFloat : AdjustableValue<float>
    {
        public AdjustableFloat(ValueAdjuster<float> adjuster) : base(adjuster)
        {
        }

        public AdjustableFloat(ValueAdjuster<float> adjuster, float value) : this(adjuster)
        {
            _value = value;
        }

        public void Add(float value)
        {
            _value += value;
        }

        public void Add(int value)
        {
            Add((float)value);
        }

        public static AdjustableFloat operator ++(AdjustableFloat value)
        {
            value._value++;
            return value;
        }

        public static AdjustableFloat operator --(AdjustableFloat value)
        {
            value._value--;
            return value;
        }

        public static bool operator <(AdjustableFloat a, AdjustableFloat b)
        {
            return a.GetValue() < b.GetValue();
        }

        public static bool operator >(AdjustableFloat a, AdjustableFloat b)
        {
            return a.GetValue() > b.GetValue();
        }

    }

    public class AdjustableInt : AdjustableValue<int>
    {
        public AdjustableInt(ValueAdjuster<int> adjuster) : base(adjuster)
        {
        }

        public void Add(int value)
        {
            _value += value;
        }

        public void Add(float value)
        {
            Add((int)value);
        }

        public static AdjustableInt operator ++(AdjustableInt value)
        {
            value._value++;
            return value;
        }

        public static AdjustableInt operator --(AdjustableInt value)
        {
            value._value--;
            return value;
        }

        public static bool operator <(AdjustableInt a, AdjustableInt b)
        {
            return a.GetValue() < b.GetValue();
        }

        public static bool operator >(AdjustableInt a, AdjustableInt b)
        {
            return a.GetValue() > b.GetValue();
        }
    }
}
