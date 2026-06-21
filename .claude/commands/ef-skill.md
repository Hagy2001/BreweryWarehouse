---
name: EF Skill
description: Use this skill when adding or modifying EF entities, creating migrations, updating DbContext, or working with EF repositories in BreweryWarehouse.
---

## Scope — Allowed Files
- BreweryWarehouse.Model/**/*.cs (entity classes only)
- BreweryWarehouse.Web/Data/BreweryWarehouseDbContext.cs
- BreweryWarehouse.Web/Repositories/*Repository.cs (EF repositories only, not Mock)
- BreweryWarehouse.Web/Migrations/**

## EF Conventions Used In This Project
- All entity classes have [Key] on the Id property
- All 1-N navigation collections are virtual ICollection<T>
- All foreign keys follow the pattern: public int EntityNameId + [ForeignKey("EntityName")] + public virtual EntityName EntityName
- TPH inheritance: Can and Keg both inherit Container, stored in a single Container table with a Discriminator column
- DeleteBehavior.Restrict is set on Can.BeerStyleId and Keg.BeerStyleId in OnModelCreating to avoid cascade conflicts
- All EF repositories use AddScoped lifetime in Program.cs
- Repositories receive BreweryWarehouseDbContext via constructor DI
- GetAll() uses Include() for direct navigation properties only
- GetById(int id) uses .FirstOrDefault(x => x.Id == id) with the same includes

## Migration Workflow
After any model change, run these commands from BreweryWarehouse.Web/:
1. dotnet ef migrations add DescriptiveMigrationName
2. dotnet ef database update

## Post-Task Rules
- Always update CLAUDE.md to reflect any structural changes
- Always update docs/semantic-model.md if any entity, property, or relationship changes
- Never manually edit generated migration files in Migrations/
- Never modify Mock repositories
