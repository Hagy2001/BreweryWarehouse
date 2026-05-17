---
name: Datepicker Skill
description: Use this skill when creating or applying the custom _DatePicker partial view in BreweryWarehouse.Web. Never use the native browser input[type=date].
---

## Scope — Files To Create Or Modify
- BreweryWarehouse.Web/Views/Shared/_DatePicker.cshtml — the reusable partial
- wwwroot/css/site.css — datepicker styles, no inline style attributes
- wwwroot/js/site.js — datepicker JS logic

## Rules
- Must NOT use the native browser input[type=date] control
- Must support hr format (dd.MM.yyyy) and en-US format (MM/dd/yyyy)
  by reading the browser's Accept-Language via a data attribute set from the controller/view
- The partial accepts a model of type DatePickerModel with properties:
  FieldName (string), Value (DateTime?), Label (string), IsRequired (bool)
- Renders a visible text input for the user and a hidden input for form posting
- The hidden input name must match the ViewModel property name exactly so model binding works
- Validation span uses asp-validation-for on the hidden input field name
- All calendar popup styles go in site.css under a .bw-datepicker namespace
- JS initialisation goes in site.js — no inline scripts in the partial itself
- The calendar popup must match the brewery industrial design system colours

## Post-Task Rules
- Always note in copilot-instructions.md that _DatePicker partial exists and where it applies