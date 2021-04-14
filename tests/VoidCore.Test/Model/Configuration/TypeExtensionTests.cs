using VoidCore.Model.Configuration;
using Xunit;

namespace VoidCore.Test.Model.Configuration
{
    public class TypeExtensionTests
    {
        [Fact]
        public void Inherits_finds_base_classes_interfaces_and_open_generics()
        {
            // Unrelated
            Assert.False(typeof(int).Inherits(typeof(IMy)));
            // Wrong generic arg
            Assert.False(typeof(My).Inherits(typeof(My<int>)));
            // Can't find derivatives
            Assert.False(typeof(My<string>).Inherits(typeof(My)));
            // Wrong generic arg
            Assert.False(typeof(My<string>).Inherits(typeof(My<int>)));

            Assert.True(typeof(My).Inherits(typeof(My)));
            Assert.True(typeof(My).Inherits(typeof(My<string>)));
            Assert.True(typeof(My).Inherits(typeof(My<>)));
            Assert.True(typeof(My).Inherits(typeof(MyAbstract<string>)));
            Assert.True(typeof(My).Inherits(typeof(MyAbstract<>)));
            Assert.True(typeof(My).Inherits(typeof(IMy<string>)));
            Assert.True(typeof(My).Inherits(typeof(IMy<>)));
            Assert.True(typeof(My).Inherits(typeof(IMy)));

            Assert.True(typeof(My<string>).Inherits(typeof(My<string>)));
            Assert.True(typeof(My<string>).Inherits(typeof(My<>)));
            Assert.True(typeof(My<string>).Inherits(typeof(MyAbstract<string>)));
            Assert.True(typeof(My<string>).Inherits(typeof(MyAbstract<>)));
            Assert.True(typeof(My<string>).Inherits(typeof(IMy<string>)));
            Assert.True(typeof(My<string>).Inherits(typeof(IMy<>)));
            Assert.True(typeof(My<string>).Inherits(typeof(IMy)));
        }
    }

    public interface IMy { }

    public interface IMy<T> : IMy { }

    public abstract class MyAbstract<T> : IMy<T> { }

    public class My<T> : MyAbstract<T> { }

    public class My : My<string> { }
}
