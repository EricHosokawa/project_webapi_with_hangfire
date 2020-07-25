namespace webapi.hangfire.Interfaces
{
    public interface IMongoDbSetting
    {
        string ConnectionString { get; set; }
        
        string DatabaseName { get; set; }
    }
}