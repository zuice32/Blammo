namespace Agent.Core.Application
{
    public interface IAgentIdentity
    {
        string ID { get; }

        string PathToAgentDataDirectory { get; }
    }
}