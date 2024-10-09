namespace BlazorApp.Persistence;

public static class Db
{
    private static IServiceProvider _serviceProvider;

    public static void Configure(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static CrmDbContext getContext()
    {
        return _serviceProvider.GetService<CrmDbContext>();
    }
}