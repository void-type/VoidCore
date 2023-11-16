using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;
using VoidCore.Model.Text;
using Xunit;

namespace VoidCore.Test.Model;

public class RuleValidatorTests
{
    [Theory]
    [InlineData(false, false, false, true, false)]
    [InlineData(false, false, true, false, false)]
    [InlineData(false, false, true, true, false)]
    [InlineData(false, true, false, false, true)]
    [InlineData(false, true, false, true, false)]
    [InlineData(false, true, true, false, false)]
    [InlineData(false, true, true, true, false)]
    [InlineData(true, false, false, false, true)]
    [InlineData(true, false, false, true, false)]
    [InlineData(true, false, true, false, false)]
    [InlineData(true, false, true, true, false)]
    [InlineData(true, true, false, false, true)]
    [InlineData(true, true, false, true, false)]
    [InlineData(true, true, true, true, false)]
    public void Validation_satisfies_truth_table(bool isInvalid1, bool isInValid2, bool isSuppressed1, bool isSuppressed2, bool failureExpected)
    {
        var result = new TruthTableValidator().Validate(new TruthTableParams(isInvalid1, isInValid2, isSuppressed1, isSuppressed2));

        Assert.NotEqual(failureExpected, result.IsSuccess);
        Assert.Equal(failureExpected, result.IsFailed);
        Assert.Equal(failureExpected, result.Failures.Any());
    }

    [Fact]
    public void Failures_can_be_constructed_from_the_entity_properties()
    {
        var result = new FailureBuilderValidator().Validate(new Entity("invalid"));

        Assert.Single(result.Failures);

        var failure = result.Failures.Single();

        Assert.Equal("invalid", failure.Message);
        Assert.Equal("StringProperty", failure.UiHandle);
    }

    [Fact]
    public void Derived_classes_can_be_validated()
    {
        var result = new FailureBuilderValidator().Validate(new DerivedEntity("invalid"));

        Assert.True(result.IsFailed);
        Assert.True(result.Failures.Any());

        result = new FailureBuilderValidator().Validate(new DerivedEntity("valid"));

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());
    }

    [Fact]
    public void Validation_success_when_no_invalid_conditions()
    {
        var result = new NoInvalidConditionsValidator().Validate(new Entity("valid"));

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());
    }

    [Fact]
    public void Validation_object_extension()
    {
        var result = new Entity("valid").Validate(new NoInvalidConditionsValidator());

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());
    }

    [Fact]
    public void Validation_object_builder_extension()
    {
        var result = new Entity("validate me")
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhen(p => p.StringProperty.IsNullOrWhiteSpace());

                validator.CreateRule(string.Empty, nameof(Entity.StringProperty))
                    .InvalidWhen(p => !p.StringProperty.IsNullOrWhiteSpace())
                    .ExceptWhen(p => p.StringProperty.EndsWith("me"));
            });

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());
    }

    [Fact]
    public void Validation_RuleBuilder_IsNullOrWhiteSpace_extension()
    {
        var result = new Entity("validate me")
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrWhiteSpace(p => p.StringProperty);
            });

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());

        result = new Entity(" ")
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrWhiteSpace(p => p.StringProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());

        result = new Entity(string.Empty)
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrWhiteSpace(p => p.StringProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());

        result = new Entity(null)
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrWhiteSpace(p => p.StringProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());
    }

    [Fact]
    public void Validation_RuleBuilder_string_IsNullOrEmpty_extension()
    {
        var result = new Entity("validate me")
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrEmpty(p => p.StringProperty);
            });

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());

        result = new Entity(" ")
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrEmpty(p => p.StringProperty);
            });

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());

        result = new Entity(string.Empty)
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrEmpty(p => p.StringProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());

        result = new Entity(null)
            .Validate(validator =>
            {
                validator.CreateRule("StringProperty can't be empty", nameof(Entity.StringProperty))
                    .InvalidWhenNullOrEmpty(p => p.StringProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());
    }

    [Fact]
    public void Validation_RuleBuilder_collection_IsNullOrEmpty_extension()
    {
        var result = new Entity(string.Empty)
        {
            CollectionProperty = ["item1"]
        }
            .Validate(validator =>
            {
                validator.CreateRule("CollectionProperty can't be empty", nameof(Entity.CollectionProperty))
                    .InvalidWhenNullOrEmpty(p => p.CollectionProperty);
            });

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());

        result = new Entity(string.Empty)
        {
            CollectionProperty = []
        }
            .Validate(validator =>
            {
                validator.CreateRule("CollectionProperty can't be empty", nameof(Entity.CollectionProperty))
                    .InvalidWhenNullOrEmpty(p => p.CollectionProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());

        result = new Entity(string.Empty)
        {
            CollectionProperty = null
        }
            .Validate(validator =>
            {
                validator.CreateRule("CollectionProperty can't be empty", nameof(Entity.CollectionProperty))
                    .InvalidWhenNullOrEmpty(p => p.CollectionProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());
    }

    [Fact]
    public void Validation_RuleBuilder_object_IsNull_extension()
    {
        var result = new Entity(string.Empty)
        {
            CollectionProperty = ["item1"]
        }
            .Validate(validator =>
            {
                validator.CreateRule("CollectionProperty can't be empty", nameof(Entity.CollectionProperty))
                    .InvalidWhenNull(p => p.CollectionProperty);
            });

        Assert.True(result.IsSuccess);
        Assert.False(result.Failures.Any());

        result = new Entity(string.Empty)
        {
            CollectionProperty = null
        }
            .Validate(validator =>
            {
                validator.CreateRule("CollectionProperty can't be empty", nameof(Entity.CollectionProperty))
                    .InvalidWhenNull(p => p.CollectionProperty);
            });

        Assert.False(result.IsSuccess);
        Assert.True(result.Failures.Any());
    }

    private class Entity
    {
        public Entity(string stringProperty)
        {
            StringProperty = stringProperty;
        }

        public string StringProperty { get; }
        public List<string> CollectionProperty { get; set; }
    }

    private class DerivedEntity : Entity
    {
        public DerivedEntity(string stringProperty) : base(stringProperty) { }
    }

    private class TruthTableParams
    {
        public TruthTableParams(bool isInvalid1, bool isInvalid2, bool isSuppressed1, bool isSuppressed2)
        {
            IsInvalid1 = isInvalid1;
            IsInvalid2 = isInvalid2;
            IsSuppressed1 = isSuppressed1;
            IsSuppressed2 = isSuppressed2;
        }

        public bool IsInvalid1 { get; }
        public bool IsInvalid2 { get; }
        public bool IsSuppressed1 { get; }
        public bool IsSuppressed2 { get; }
    }

    private class TruthTableValidator : RuleValidatorAbstract<TruthTableParams>
    {
        public TruthTableValidator()
        {
            CreateRule(new Failure("validation invalid", "someField"))
                .InvalidWhen(p => p.IsInvalid1)
                .InvalidWhen(p => p.IsInvalid2)
                .ExceptWhen(p => p.IsSuppressed1)
                .ExceptWhen(p => p.IsSuppressed2);
        }
    }

    private class FailureBuilderValidator : RuleValidatorAbstract<Entity>
    {
        public FailureBuilderValidator()
        {
            CreateRule(v => new Failure($"{v.StringProperty}", $"{nameof(v.StringProperty)}"))
                .InvalidWhen(v => v.StringProperty == "invalid");
        }
    }

    private class NoInvalidConditionsValidator : RuleValidatorAbstract<Entity>
    {
        public NoInvalidConditionsValidator()
        {
            CreateRule(nameof(Entity.StringProperty), nameof(Entity.StringProperty));
        }
    }
}
