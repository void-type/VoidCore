namespace VoidCore.Model.Emailing
{
    public interface IAppVariables
    {
        string AppName { get; }
        string BaseUrl { get; }
        string Environment { get; }
    }
}
