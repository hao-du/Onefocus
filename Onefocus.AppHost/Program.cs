var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Onefocus_Web>("onefocus-web");
builder.AddProject<Projects.Onefocus_Gateway>("onefocus-gateway");
builder.AddProject<Projects.Onefocus_Home_Api>("onefocus-home-api");
builder.AddProject<Projects.Onefocus_Identity_Api>("onefocus-identity-api");
builder.AddProject<Projects.Onefocus_Wallet_Api>("onefocus-wallet-api");
builder.AddProject<Projects.Onefocus_Search_Api>("onefocus-search-api");
builder.AddProject<Projects.Onefocus_Membership_Api>("onefocus-membership-api");

builder.Build().Run();
