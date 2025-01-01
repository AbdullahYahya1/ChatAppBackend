var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ZChatAppBackend>("zchatappbackend");
builder.Build().Run();
