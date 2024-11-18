var builder = DistributedApplication.CreateBuilder(args);

// var sql1 = builder.AddAzureSqlServer("sql1")
//     .RunAsContainer();

var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var sqldb = sql.AddDatabase("sqldb");

var cache = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();
    //.WithRedisCommander();

var apiService = builder.AddProject<Projects.Aspire9_MelbDotNet_ApiService>("apiservice")
    .WithReference(sqldb);

builder.AddProject<Projects.Aspire9_MelbDotNet_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WaitFor(cache)
    .WaitFor(apiService);

builder.Build().Run();
