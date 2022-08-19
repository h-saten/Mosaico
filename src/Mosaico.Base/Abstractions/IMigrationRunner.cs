namespace Mosaico.Base.Abstractions
{
    public interface IMigrationRunner
    {
        void RunMigrations(string contextName = null);
    }
}