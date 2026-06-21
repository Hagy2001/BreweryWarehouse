---
name: CRUD Forms Skill
description: Use this skill when creating CreateModel or EditModel ViewModels, or any Create/Edit/Delete controller actions and views in BreweryWarehouse.Web.
---

## Scope — Files To Create Or Modify
- BreweryWarehouse.Web/Models/{Entity}CreateModel.cs — new form ViewModel for create
- BreweryWarehouse.Web/Models/{Entity}EditModel.cs — new form ViewModel for edit
- BreweryWarehouse.Web/Controllers/{Entity}Controller.cs — add CRUD actions
- BreweryWarehouse.Web/Views/{Entity}/Create.cshtml — create view
- BreweryWarehouse.Web/Views/{Entity}/Edit.cshtml — edit view
- BreweryWarehouse.Web/Views/{Entity}/_CreateOrEdit.cshtml — shared form partial

## ViewModel Conventions
- Namespace: BreweryWarehouse.Web.Models
- CreateModel contains all user-editable fields, no Id, no navigation properties
- EditModel inherits nothing — it is a flat copy of CreateModel fields plus int Id
- No EF entity types as properties — use primitive types and enums only
- For FK fields (e.g. BeerStyleId): include both int {Entity}Id and string {Entity}Name
  - {Entity}Id is the hidden input value (posted to server)
  - {Entity}Name is the display-only text shown in the autocomplete control
- DateTime properties for dates that will use the custom _DatePicker partial
- DataAnnotations go on CreateModel; EditModel repeats them (no inheritance shortcut)

## Required DataAnnotations Per Type
- string (required): [Required], [StringLength(max, MinimumLength = min)]
- string (optional): no [Required], [StringLength(max)] only
- double/decimal: [Required], [Range(min, max)]
- int (required): [Required], [Range(min, max)]
- int FK (BeerStyleId etc.): [Required], [Range(1, int.MaxValue, ErrorMessage = "Please select a value.")]
- DateTime: [Required]
- bool: no annotations needed
- enum: [Required]
- email: [Required], [EmailAddress]

## Soft Delete Convention (Phase 2 onwards)
- BeerStyle, Can, Keg get DateTime? DeletedAt on the EF entity (not on the ViewModel)
- WarehouseLocation, StockEntry, Employee are hard-deleted
- Never add DeletedAt to a ViewModel

## Post-Task Rules
- Always update CLAUDE.md after adding new ViewModel classes
- Always update docs/semantic-model.md if any EF entity is modified