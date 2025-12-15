using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogWeb.Data;
using BlogWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Добавьте контекст базы данных для PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавьте Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Автоматическое создание базы данных и ролей при запуске
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    try
    {
        // Получаем контекст базы данных
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        
        // 1. Создаем базу данных, если ее не существует
        Console.WriteLine("Проверка существования базы данных...");
        var databaseCreated = await dbContext.Database.EnsureCreatedAsync();
        
        if (databaseCreated)
        {
            Console.WriteLine("База данных успешно создана!");
        }
        else
        {
            Console.WriteLine("База данных уже существует.");
        }
        
        // 2. Применяем миграции (если они есть)
        // Console.WriteLine("Применение миграций...");
        // await dbContext.Database.MigrateAsync();
        // Console.WriteLine("Миграции успешно применены.");
        
        // 3. Создаем роли
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Администратор", "Пользователь" }; // Оставим только базовые роли
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"Роль '{role}' создана.");
            }
            else
            {
                Console.WriteLine($"Роль '{role}' уже существует.");
            }
        }
        
        // 4. Создаем администратора по умолчанию (с заполнением всех обязательных полей)
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = services.GetRequiredService<IConfiguration>();
        
        var adminEmail = configuration["Admin:Email"] ?? "admin@example.com";
        var adminPassword = configuration["Admin:Password"] ?? "Admin123!";
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DisplayName = "Системный администратор", // Обязательное поле
                PhoneNumber = "+79000000000", // Если нужно
                PhoneNumberConfirmed = true, // Если нужно
                IsActive = true // Если есть такое свойство
                // Добавьте другие поля из вашей модели ApplicationUser
            };
            
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Администратор");
                Console.WriteLine($"Администратор {adminEmail} создан.");
            }
            else
            {
                Console.WriteLine($"Ошибка создания администратора: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine("Администратор уже существует.");
        }
        
        Console.WriteLine("Инициализация базы данных завершена успешно!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
        }
    }
}

app.Run();