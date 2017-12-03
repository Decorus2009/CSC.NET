using System;

namespace Option
{
    public class Option<T>
    {
        private static readonly Option<T> _none = new Option<T>(); 
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
                    throw new OptionException("Option.None does not contain value");
                }
                return _value;
            }
        }


        public static Option<T> Some(T value) => new Option<T>(value);

        public static Option<T> None() => _none;

        public Option<TR> Map<TR>(Func<T, TR> f) => IsNone ? Option<TR>.None() : Option<TR>.Some(f(Value));

        public static Option<T> Flatten(Option<Option<T>> mapper) => mapper.IsNone ? None() : mapper.Value;

        public override bool Equals(object obj)
        {
            if (!(obj is Option<T> other))
            {
                return false;
            }
            
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