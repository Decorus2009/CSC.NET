using System;
using System.Collections.Generic;
using NUnit.Framework;
using Option;

namespace Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void IsNone_ForOptionNoneForAValueType_Expected() =>
            Assert.True(Option<int>.None().IsNone);

        [Test]
        public void IsNone_ForOptionNoneForAReferenceType_Expected() =>
            Assert.True(Option<string>.None().IsNone);

        [Test]
        public void IsSome_ForOptionSomeForAValueType_Expected() =>
            Assert.True(Option<int>.Some(1).IsSome);

        [Test]
        public void IsSome_ForOptionSomeForAReferenceType_Expected() =>
            Assert.True(Option<string>.Some("Hello").IsSome);

        [Test]
        public void IsSome_ForOptionNoneForAValueType_ExpectedFalse() =>
            Assert.False(Option<string>.None().IsSome);

        [Test]
        public void IsSome_ForOptionNoneForAReferenceType_ExpectedFalse() =>
            Assert.False(Option<string>.None().IsSome);

        [Test]
        public void IsNone_ForOptionSomeForAValueType_ExpectedFalse() =>
            Assert.False(Option<string>.None().IsSome);

        [Test]
        public void IsNone_ForOptionSomeForAReferenceType_ExpectedFalse() =>
            Assert.False(Option<string>.None().IsSome);


        [Test]
        public void Value_ForOptionSomeForAValueType_IsSome()
        {
            var some = Option<int>.Some(1);
            Assert.True(some.IsSome);
            Assert.AreEqual(1, some.Value);
        }

        [Test]
        public void Value_ForOptionSomeForAReferenceType_IsSome()
        {
            var some = Option<string>.Some("Hello");
            Assert.True(some.IsSome);
            Assert.AreEqual("Hello", some.Value);
        }

        [Test]
        public void Value_ForOptionNoneForAValueType_Throws() =>
            Assert.That(() => Option<int>.None().Value, Throws.InstanceOf<OptionException>());

        [Test]
        public void Value_ForOptionNoneForAReferenceType_Throws() =>
            Assert.That(() => Option<string>.None().Value, Throws.InstanceOf<OptionException>());


        [Test]
        public void Some_ForOptionSomeForAReferenceTypeIfValueIsNull_Throws() =>
            Assert.Throws<NullReferenceException>(() => Option<string>.Some(null));


        [Test]
        public void Map_IntToInt()
        {
            const int value = 2;
            var option = Option<int>.Some(value);
            var f = new Func<int, int>(x => x * 2);
            Assert.AreEqual(f(value), option.Map(f).Value);
        }

        [Test]
        public void Map_IntToString()
        {
            const int value = 2;
            var option = Option<int>.Some(value);
            var f = new Func<int, string>(x => x.ToString());
            Assert.AreEqual(f(value), option.Map(f).Value);
        }

        [Test]
        public void Map_StringToInt()
        {
            const string value = "Hello";
            var option = Option<string>.Some(value);
            var f = new Func<string, int>(x => x.Length);
            Assert.AreEqual(f(value), option.Map(f).Value);
        }

        [Test]
        public void Map_StringToString()
        {
            const string value = "Hello";
            var option = Option<string>.Some(value);
            var f = new Func<string, string>(x => x + x);
            Assert.AreEqual(f(value), option.Map(f).Value);
        }

        [Test]
        public void Map_ForOptionNoneForAValueType_ReturnsOptionNone()
        {
            var option = Option<int>.None();
            var f = new Func<int, int>(x => x * 2);
            Assert.True(option.Map(f).IsNone);
        }

        [Test]
        public void Map_ForOptionNoneForAReferenceType_ReturnsOptionNone()
        {
            var option = Option<string>.None();
            var f = new Func<string, string>(x => x + x);
            Assert.True(option.Map(f).IsNone);
        }
        
        [Test]
        public void Flatten_OptionSomeForAValueType()
        {
            var option1 = Option<int>.Some(2);
            var option2 = Option<Option<int>>.Some(option1);
            var option3 = Option<int>.Flatten(option2);
            Assert.AreEqual(option1, option3);
        }

        [Test]
        public void Flatten_OptionSomeForAReferenceType()
        {
            var option1 = Option<string>.Some("Hello");
            var option2 = Option<Option<string>>.Some(option1);
            var option3 = Option<string>.Flatten(option2);
            Assert.AreEqual(option1, option3);
        }

        [Test]
        public void Value_ForOptionSomeForOptionNoneForAValueType_ThrowsAfterFlatten()
        {
            var none = Option<int>.None();
            var fatOption = Option<Option<int>>.Some(none);
            var flatten = Option<int>.Flatten(fatOption);
            Assert.That(() => flatten.Value, Throws.InstanceOf<OptionException>());
        }

        [Test]
        public void Value_ForOptionSomeForOptionNoneForAReferenceType_ThrowsAfterFlatten()
        {
            var none = Option<string>.None();
            var fatOption = Option<Option<string>>.Some(none);
            var flatten = Option<string>.Flatten(fatOption);
            Assert.That(() => flatten.Value, Throws.InstanceOf<OptionException>());
        }
        
        
        [Test]
        public void Equals_ForOptionSomeEqualsItselfForAValueType_True()
        {
            var option1 = Option<int>.Some(2);
            var option2 = option1;
            Assert.True(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeEqualsItselfForAReferenceType_True()
        {
            var option1 = Option<string>.Some("Hello");
            var option2 = option1;
            Assert.True(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionNoneAndOptionNoneForAValueType_True()
        {
            var option1 = Option<int>.None();
            var option2 = Option<int>.None();
            Assert.True(option1.Equals(option2));
        }  
        
        [Test]
        public void Equals_ForOptionNoneAndOptionNoneForAReferenceType_True()
        {
            var option1 = Option<string>.None();
            var option2 = Option<string>.None();
            Assert.True(option1.Equals(option2));
        }   
        
        [Test]
        public void Equals_ForOptionSomeAndOptionNoneForAValueType_False()
        {
            var option1 = Option<int>.Some(2);
            var option2 = Option<int>.None();
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeAndOptionNoneForAReferenceType_False()
        {
            var option1 = Option<string>.Some("Hello");
            var option2 = Option<string>.None();
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeAndOptionSomeForAValueTypeForSameValue_True()
        {
            var option1 = Option<int>.Some(2);
            var option2 = Option<int>.Some(2);
            Assert.True(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeAndOptionSomeForAReferenceTypeForSameValue_True()
        {
            var option1 = Option<string>.Some("Hello");
            var option2 = Option<string>.Some("Hello");
            Assert.True(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeAndOptionSomeForDifferentTypes_False()
        {
            var option1 = Option<int>.Some(2);
            var option2 = Option<string>.Some("Hello");
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionNoneAndOptionNoneForDifferentTypes_False()
        {
            var option1 = Option<int>.None();
            var option2 = Option<string>.None();
            Assert.False(option1.Equals(option2));
        }

        [Test]
        public void Equals_ForOptionSomeAndOptionNoneForDifferentTypes_False()
        {
            var option1 = Option<int>.Some(2);
            var option2 = Option<string>.None();
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeAndNullForAValueType_False()
        {
            var option1 = Option<int>.Some(2);
            Option<int> option2 = null;
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionSomeAndNullForAReferenceType_False()
        {
            var option1 = Option<int>.Some(2);
            Option<int> option2 = null;
            Assert.False(option1.Equals(option2));
        }
               
        [Test]
        public void Equals_ForOptionSomeAndNullForDifferentTypes_False()
        {
            var option1 = Option<string>.Some("Hello");
            Option<int> option2 = null;
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionNoneAndNullForAValueType_False()
        {
            var option1 = Option<string>.None();
            Option<string> option2 = null;
            Assert.False(option1.Equals(option2));
        }
        
        [Test]
        public void Equals_ForOptionNoneAndNullForAReferenceType_False()
        {
            var option1 = Option<string>.None();
            Option<string> option2 = null;
            Assert.False(option1.Equals(option2));
        }
 
        [Test]
        public void Equals_ForOptionNoneAndNullForDifferentTypes_False()
        {
            var option1 = Option<string>.None();
            Option<int> option2 = null;
            Assert.False(option1.Equals(option2));
        }
        
        
        [Test]
        public void GetHashCode_HashSetContainsOptionSomeForAValueType_True()
        {
            var option1 = Option<int>.Some(2);
            var set = new HashSet<Option<int>> {option1};
            Assert.True(set.Contains(Option<int>.Some(2)));
        }
        
        [Test]
        public void GetHashCode_HashSetContainsOptionSomeForAReferenceType_True()
        {
            var option1 = Option<string>.Some("Hello");
            var set = new HashSet<Option<string>> {option1};
            Assert.True(set.Contains(Option<string>.Some("Hello")));
        }
        
        [Test]
        public void GetHashCode_HashSetContainsOptionNoneForAValueType_True()
        {
            var option1 = Option<int>.None();
            var set = new HashSet<Option<int>> {option1};
            Assert.True(set.Contains(Option<int>.None()));
        }
        
        [Test]
        public void GetHashCode_HashSetContainsOptionNoneForAReferenceType_True()
        {
            var option1 = Option<string>.None();
            var set = new HashSet<Option<string>> {option1};
            Assert.True(set.Contains(Option<string>.None()));
        }
        
        
        // тесты Михаила
        [Test]
        public void NoneReferenceEqualsNoneTest()
        {
            // теперь не падает
            Assert.IsTrue(Option<int>.None() == Option<int>.None());
        }
        
        [Test]
        public void NoneEqualsNoneTest()
        {
            // проходит успешно
            Assert.AreEqual(Option<int>.None(), Option<int>.None());
        }
        
        [Test]
        public void FlattenNoneTest()
        {
            // теперь не падает с исключением
            Assert.IsTrue(Option<int>.Flatten(Option<Option<int>>.None()).IsNone);
        }
        
        [Test]
        public void NoneGetHashCodeEqualsTest()
        {
            Assert.AreEqual(Option<int>.None().GetHashCode(), Option<string>.None().GetHashCode());
        }
    }

}