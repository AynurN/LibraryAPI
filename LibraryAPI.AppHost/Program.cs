var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LibraryAPI>("libraryapi");

builder.Build().Run();
