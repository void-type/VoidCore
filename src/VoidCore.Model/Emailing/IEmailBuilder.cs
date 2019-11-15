using System;

namespace VoidCore.Model.Emailing
{
    public interface IEmailBuilder
    {
        Email Build(Action<EmailOptionsBuilder> configure);
    }
}
