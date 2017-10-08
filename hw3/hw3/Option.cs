using System;

namespace hw3
{
    public class Option<T>
    {
        private readonly T _value;

        private Option()
        {
            _value = default(T);
            IsSome = false;
        }

        private Option(T value)
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }
            _value = value;
            IsSome = true;
        }

        public bool IsSome { get; }

        public bool IsNone => !IsSome;

        public T Value
        {
            get
            {
                if (IsNone)
                {
                    throw new ArgumentNullException();
                }
                return _value;
            }
        }


        public static Option<T> Some(T value) => new Option<T>(value);

        public static Option<T> None() => new Option<T>();

        public Option<TR> Map<TR>(Func<T, TR> f) => IsNone ? Option<TR>.None() : Option<TR>.Some(f(Value));

        public static Option<T> Flatten(Option<Option<T>> mapper) => mapper.Value;

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null || obj.GetType() != typeof(Option<T>))
            {
                return false;
            }
            var other = (Option<T>) obj;
            if (IsNone && other.IsNone)
            {
                return true;
            }
            if (IsNone && other.IsSome || IsSome & other.IsNone)
            {
                return false;
            }
            return Value.Equals(other.Value);
        }

        public override int GetHashCode() => IsNone ? 0 : Value.GetHashCode();

        public override string ToString() => IsNone ? "None" : Value.ToString();
    }
}