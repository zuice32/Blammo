using System;
using System.IO;
using Newtonsoft.Json;

namespace Agent.Core.Application
{
    //TODO: Move to Core.Application?
    public class AgentIdentity : IAgentIdentity
    {
        public static string LoadedAgentDataDirectory { get; private set; }

        public static IAgentIdentity Load(string pathToAgentDataDirectory)
        {
            AgentIdentity agentIdentity;

            string pathToIdentityFile = Path.Combine(pathToAgentDataDirectory, "agent.id");

            Directory.CreateDirectory(pathToAgentDataDirectory);

            if (!File.Exists(pathToIdentityFile))
            {
                agentIdentity = new AgentIdentity
                {
                    PathToAgentDataDirectory = pathToAgentDataDirectory,
                    ID = Guid.NewGuid().ToString("N")
                };

                string serializedIdentity = JsonConvert.SerializeObject(agentIdentity);

                using (TextWriter textWriter = new StreamWriter(pathToIdentityFile))
                {
                    textWriter.Write(serializedIdentity);
                }
            }
            else
            {
                string serializedIdentity;
                using (TextReader textReader = new StreamReader(pathToIdentityFile))
                {
                    serializedIdentity = textReader.ReadToEnd();
                }

                agentIdentity = JsonConvert.DeserializeObject<AgentIdentity>(serializedIdentity);
            }

            LoadedAgentDataDirectory = pathToAgentDataDirectory;

            return agentIdentity;
        }

        public string ID { get; set; }
        public string PathToAgentDataDirectory { get; set; }
    }
}