using HCM.Domain.Identity;
using HCM.Domain.Persons;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure;

public static class SeedDb
{
    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher<Person> passwordHasher)
    {
        await context.Database.MigrateAsync();
        
        if (await context.Persons.AnyAsync())
            return;
        
        var rawData = new[]
        {
            new { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@company.com", JobTitle = "HR Manager", Salary = 90000m, Department = "HR", Role = ApplicationRoles.HrAdmin },
            new { FirstName = "Bob", LastName = "Thompson", Email = "bob.thompson@company.com", JobTitle = "HR Assistant", Salary = 55000m, Department = "HR", Role = ApplicationRoles.Employee },
            new { FirstName = "Clara", LastName = "Williams", Email = "clara.williams@company.com", JobTitle = "Recruiter", Salary = 58000m, Department = "HR", Role = ApplicationRoles.Employee },
            new { FirstName = "Daniel", LastName = "Smith", Email = "daniel.smith@company.com", JobTitle = "IT Director", Salary = 110000m, Department = "IT", Role = ApplicationRoles.Manager },
            new { FirstName = "Eva", LastName = "Brown", Email = "eva.brown@company.com", JobTitle = "Software Engineer", Salary = 87000m, Department = "IT", Role = ApplicationRoles.Employee },
            new { FirstName = "Frank", LastName = "Miller", Email = "frank.miller@company.com", JobTitle = "System Administrator", Salary = 80000m, Department = "IT", Role = ApplicationRoles.Employee },
            new { FirstName = "Grace", LastName = "Davis", Email = "grace.davis@company.com", JobTitle = "DevOps Engineer", Salary = 88000m, Department = "IT", Role = ApplicationRoles.Employee },
            new { FirstName = "Henry", LastName = "Garcia", Email = "henry.garcia@company.com", JobTitle = "Finance Director", Salary = 105000m, Department = "Finance", Role = ApplicationRoles.HrAdmin },
            new { FirstName = "Irene", LastName = "Martinez", Email = "irene.martinez@company.com", JobTitle = "Accountant", Salary = 70000m, Department = "Finance", Role = ApplicationRoles.Employee },
            new { FirstName = "Jack", LastName = "Rodriguez", Email = "jack.rodriguez@company.com", JobTitle = "Financial Analyst", Salary = 75000m, Department = "Finance", Role = ApplicationRoles.Employee },
            new { FirstName = "Karen", LastName = "Lee", Email = "karen.lee@company.com", JobTitle = "Marketing Manager", Salary = 85000m, Department = "Marketing", Role = ApplicationRoles.Manager },
            new { FirstName = "Leo", LastName = "Walker", Email = "leo.walker@company.com", JobTitle = "SEO Specialist", Salary = 65000m, Department = "Marketing", Role = ApplicationRoles.Employee },
            new { FirstName = "Maria", LastName = "Hall", Email = "maria.hall@company.com", JobTitle = "Content Creator", Salary = 62000m, Department = "Marketing", Role = ApplicationRoles.Employee },
            new { FirstName = "Nathan", LastName = "Allen", Email = "nathan.allen@company.com", JobTitle = "Sales Director", Salary = 95000m, Department = "Sales", Role = ApplicationRoles.Manager },
            new { FirstName = "Olivia", LastName = "Young", Email = "olivia.young@company.com", JobTitle = "Sales Associate", Salary = 58000m, Department = "Sales", Role = ApplicationRoles.Employee },
            new { FirstName = "Paul", LastName = "Hernandez", Email = "paul.hernandez@company.com", JobTitle = "Account Executive", Salary = 60000m, Department = "Sales", Role = ApplicationRoles.Employee },
            new { FirstName = "Quinn", LastName = "King", Email = "quinn.king@company.com", JobTitle = "Support Manager", Salary = 78000m, Department = "Support", Role = ApplicationRoles.Manager },
            new { FirstName = "Rachel", LastName = "Wright", Email = "rachel.wright@company.com", JobTitle = "Customer Support Rep", Salary = 52000m, Department = "Support", Role = ApplicationRoles.Employee },
            new { FirstName = "Steve", LastName = "Lopez", Email = "steve.lopez@company.com", JobTitle = "Tech Support", Salary = 54000m, Department = "Support", Role = ApplicationRoles.Employee },
            new { FirstName = "Tina", LastName = "Scott", Email = "tina.scott@company.com", JobTitle = "Office Assistant", Salary = 48000m, Department = "Operations", Role = ApplicationRoles.Employee },
            new { FirstName = "Lina", LastName = "Sot", Email = "lina.sot@company.com", JobTitle = "Office Assistant", Salary = 45000m, Department = "Operations", Role = ApplicationRoles.Employee }
        };
        
        foreach (var p in rawData)
        {
            var person = Person.Create(
                p.FirstName,
                p.LastName,
                p.Email,
                p.JobTitle,
                p.Salary,
                p.Department,
                p.Role,
                passwordHasher,
                "Password123!"
            );
            
            context.Persons.Add(person);
        }
        
        await context.SaveChangesAsync();
    }
}
