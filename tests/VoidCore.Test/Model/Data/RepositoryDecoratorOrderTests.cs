using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using Xunit;

namespace VoidCore.Test.Model.Data;

public class RepositoryDecoratorOrderTests
{
    private static readonly DateTimeOffset s_date = new(2001, 2, 12, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Soft_deleting_through_the_outer_soft_delete_decorator_also_records_the_modification()
    {
        var entity = new TrackedEntity();
        var repository = CreateInner()
            .AddAuditability(new FakeTimeProvider(s_date), GetUserAccessor())
            .AddSoftDeletability(new FakeTimeProvider(s_date), GetUserAccessor());

        await repository.Remove(entity, default);

        Assert.True(entity.IsDeleted);
        Assert.Equal("userName", entity.DeletedBy);
        Assert.Equal("userName", entity.ModifiedBy);
        Assert.Equal(s_date, entity.ModifiedOn);
    }

    [Fact]
    public async Task Soft_deleting_through_the_inner_soft_delete_decorator_skips_the_audit_decorator()
    {
        var entity = new TrackedEntity();
        var repository = CreateInner()
            .AddSoftDeletability(new FakeTimeProvider(s_date), GetUserAccessor())
            .AddAuditability(new FakeTimeProvider(s_date), GetUserAccessor());

        await repository.Remove(entity, default);

        Assert.True(entity.IsDeleted);
        Assert.Equal(string.Empty, entity.ModifiedBy);
    }

    private static IWritableRepository<TrackedEntity> CreateInner()
    {
        var repository = Substitute.For<IWritableRepository<TrackedEntity>>();
        repository.Update(Arg.Any<TrackedEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        return repository;
    }

    private static ICurrentUserAccessor GetUserAccessor()
    {
        var accessor = Substitute.For<ICurrentUserAccessor>();
        accessor.GetUser().Returns(new DomainUser("userName", Array.Empty<string>()));
        return accessor;
    }

    public class TrackedEntity : IAuditable, ISoftDeletable
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
