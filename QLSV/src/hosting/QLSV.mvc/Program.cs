using QLSV.data.Services;
using QLSV.domain.entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbService<SinhVien>, DbService<SinhVien>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use( async (context, next) => {
    await next();
    if(context.Response.StatusCode == 404 && context.Request.Path.Value.Contains("/images/")) {
        context.Request.Path = "/images/default.jpg";
        await next();
    }
}); 

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
