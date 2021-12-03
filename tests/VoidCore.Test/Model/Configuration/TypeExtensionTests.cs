using VoidCore.Model.Configuration;
using Xunit;

namespace VoidCore.Test.Model.Configuration;

public class TypeExtensionTests
{
    [Fact]
    public void Inherits_finds_base_classes_interfaces_and_open_generics()
    {
        // Unrelated
        Assert.False(typeof(int).Implements(typeof(IMy)));

        // Wrong generic arg
        Assert.False(typeof(My).Implements(typeof(My<int>)));
        Assert.False(typeof(My2<string>).Implements(typeof(IMy<int>)));
        Assert.False(typeof(My2<string>).Implements(typeof(IMy2<int, int>)));

        // Can't find derivatives
        Assert.False(typeof(My<string>).Implements(typeof(My)));

        // Wrong generic arg
        Assert.False(typeof(My<string>).Implements(typeof(My<int>)));

        Assert.True(typeof(My).Implements(typeof(My)));
        Assert.True(typeof(My).Implements(typeof(My<string>)));
        Assert.True(typeof(My).Implements(typeof(My<>)));
        Assert.True(typeof(My).Implements(typeof(MyAbstract<string>)));
        Assert.True(typeof(My).Implements(typeof(MyAbstract<>)));
        Assert.True(typeof(My).Implements(typeof(IMy<string>)));
        Assert.True(typeof(My).Implements(typeof(IMy<>)));
        Assert.True(typeof(My).Implements(typeof(IMy)));
        Assert.True(typeof(My<string>).Implements(typeof(My<string>)));
        Assert.True(typeof(My<string>).Implements(typeof(My<>)));
        Assert.True(typeof(My<string>).Implements(typeof(MyAbstract<string>)));
        Assert.True(typeof(My<string>).Implements(typeof(MyAbstract<>)));
        Assert.True(typeof(My<string>).Implements(typeof(IMy<string>)));
        Assert.True(typeof(My<string>).Implements(typeof(IMy<>)));
        Assert.True(typeof(My<string>).Implements(typeof(IMy)));
        Assert.True(typeof(My2<string>).Implements(typeof(IMy2<,>)));
        Assert.True(typeof(My2<string>).Implements(typeof(IMy2<string, int>)));
        Assert.True(typeof(My2<string>).Implements(typeof(IMy<string>)));
        Assert.True(typeof(My2<string>).Implements(typeof(IMy<>)));
        Assert.True(typeof(My2<string>).Implements(typeof(MyAbstract<string>)));
        Assert.True(typeof(My2<string>).Implements(typeof(MyAbstract<>)));
    }
}

public interface IMy { }
public interface IMy<T> : IMy { }
public abstract class MyAbstract<T> : IMy<T> { }
public class My<T> : MyAbstract<T> { }
public class My : My<string> { }
public interface IMy2<T1, T2> : IMy<T1> { }
public class My2<T1> : MyAbstract<T1>, IMy2<T1, int> { }
