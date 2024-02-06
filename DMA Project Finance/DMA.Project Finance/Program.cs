using Blazored.Toast;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.Services.Interfaces;
using DMA.Infrastructure.Data;
using DMA.Infrastructure.Repositories;
using DMA.ProjectManagement.Implementations;
using DMA_ProjectManagement.Core.Services;
using DMA_ProjectManagement.Core.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var httpClientBaseAddress = builder.Configuration["httpClientBaseAddress"];
var httpClientBaseAddressAsana = builder.Configuration["httpClientBaseAddressAsana"];
var accessToken = builder.Configuration["AsanaAccessToken"];
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());
builder.Services.AddScoped<IUserManager, UserManagerService>();
builder.Services.AddScoped<IAsanaService,AsanaService>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddBlazoredToast();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(defaultConnectionString));
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IImportRepository,ImportRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();

builder.Services.AddHttpClient("httpClient", client =>
{
    client.BaseAddress = new Uri(httpClientBaseAddress);
});

builder.Services.AddHttpClient("httpClientAsana", client =>
{
    client.BaseAddress = new Uri(httpClientBaseAddressAsana);
    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
