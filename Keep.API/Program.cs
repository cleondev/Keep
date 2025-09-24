using Keep.Application.Collaborators;
using Keep.Application.Labels;
using Keep.Application.Notes;
using Keep.Application.Reminders;
using Keep.Application.Users;
using Keep.Infrastructure.Data;
using Keep.Infrastructure.Services.Collaborators;
using Keep.Infrastructure.Services.Labels;
using Keep.Infrastructure.Services.Notes;
using Keep.Infrastructure.Services.Reminders;
using Keep.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=keep.db";

builder.Services.AddDbContext<KeepDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<INoteCollaboratorService, NoteCollaboratorService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<KeepDbContext>();
    await context.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
