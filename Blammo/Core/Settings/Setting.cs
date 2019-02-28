namespace Agent.Core.Settings
{
    public class Setting
    {
        public string Id { get; set; }

        public string Owner { get; set; }

        public string AppSetting { get; set; }

        public string DefaultValue { get; set; }

        public Setting(string id, string owner, string appSetting, string value)
        {
            Id = id;
            Owner = owner;
            AppSetting = appSetting;
            DefaultValue = value;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}", Id, Owner, AppSetting, DefaultValue);
        }
    }
}
