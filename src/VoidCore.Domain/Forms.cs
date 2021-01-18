using System.Collections.Generic;

namespace Forms
{
    public record Form(
        string Title,
        string ShortDescription,
        string Description,
        string Identifier,
        IEnumerable<IFormField> Fields
    );

    public interface IFormField
    {
        string Label { get; }
        string Identifier { get; }
        bool Required { get; }
        string DefaultValue { get; }
        string FieldType { get; }
    }

    public record FormStringField(
        string Label,
        string Identifier,
        bool Required = false,
        string DefaultValue = ""
    ) : IFormField
    {
        public string FieldType { get; } = "string";
    };

    public record FormNumberField(
        string Label,
        string Identifier,
        bool Required = false,
        string DefaultValue = "0"
    ) : IFormField
    {
        public string FieldType { get; } = "number";
    };

    public record FormSelectField(
        string Label,
        string Identifier,
        IEnumerable<FormSelectChoice> Choices,
        bool Required = false,
        string DefaultValue = "",
        FormSelectStyle Style = FormSelectStyle.Select
    ) : IFormField
    {
        public string FieldType { get; } = "select";
    };

    public record FormSelectChoice(
        string Label,
        string Value
    );

    public enum FormSelectStyle
    {
        Select,
        SelectMany,
        Radio
    }

    public record FormBooleanField(
        string Label,
        string Identifier,
        string DefaultValue = "false"
    ) : IFormField
    {
        public string FieldType { get; } = "boolean";
        public bool Required { get; } = true;
    };

    public record FormSubmission(string FormIdentifier, Dictionary<string, string> Values);

    // TODO: Validations should be added. If required==true check for value.EnsureNotNullOrEmpty and cast to type (with checks). Then pass to custom validators (another optional param of the field).

    public class Testing
    {
        public void Run()
        {
            var myForm = new Form(
                Title: "My Form",
                ShortDescription: "Form of mine",
                Description: "Fill this out when needed.",
                Identifier: "my_form",
                Fields: new IFormField[] {
                    new FormStringField(
                        Label: "First name",
                        Identifier: "first_name",
                        Required: true
                    ),
                    new FormStringField(
                        Label: "Last name",
                        Identifier: "last_name",
                        Required: true
                    ),
                    new FormSelectField(
                       Label:  "Job type",
                        Identifier: "job_type",
                        Choices: new [] {
                            new FormSelectChoice("Tech", "tech"),
                            new FormSelectChoice("Supervisor", "super"),
                            new FormSelectChoice("Manager", "manager")
                        },
                        Required: true,
                        Style: FormSelectStyle.Radio
                    ),
                    new FormNumberField(
                        Label: "Salary",
                        Identifier: "salary",
                        Required: true
                    )
                }
            );
        }
    }
}
