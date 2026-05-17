---
name: AJAX & JS Skill
description: Use this skill when adding AJAX search endpoints, AJAX-powered Index search, autocomplete dropdown partials, or any JavaScript/jQuery interactions in BreweryWarehouse.Web.
---

## Scope — Files To Create Or Modify
- BreweryWarehouse.Web/Controllers/{Entity}Controller.cs — Search() JSON action
- BreweryWarehouse.Web/Views/{Entity}/Index.cshtml — search input + JS
- BreweryWarehouse.Web/Views/Shared/_BeerStyleAutocomplete.cshtml — reusable autocomplete partial
- BreweryWarehouse.Web/wwwroot/js/site.js — shared JS utilities only
- BreweryWarehouse.Web/wwwroot/css/site.css — autocomplete dropdown styles only

## AJAX Search Conventions
- Endpoint name: Search(string q) on every controller, returns JsonResult
- Filters entities case-insensitively on the most human-readable string field
- Always filters out soft-deleted records (same as GetAll)
- Returns a flat anonymous object array — no navigation properties, no collections
- Max 20 results returned
- Index views call this endpoint on keyup with debounce (300ms minimum)
- On response: clear tbody, rebuild rows from JSON using jQuery, apply row-expiring
  class if BestBefore is within 30 days (calculate client-side from returned date)
- Empty query string returns all records (same as initial Index load)

## Autocomplete Dropdown Conventions
- One reusable partial: Views/Shared/_BeerStyleAutocomplete.cshtml
- The partial renders: a visible text input for typing, a hidden input for the selected ID,
  and a positioned <ul> dropdown list
- Search endpoint: BeerStyleController Search() — already built above
- Minimum 2 characters typed before firing AJAX
- Selecting a result sets the hidden input value (Id) and the text input value (Name),
  then hides the dropdown
- Clicking outside the dropdown closes it
- On Edit forms: the partial accepts two optional parameters — selectedId and selectedName —
  used to pre-populate both inputs on page load
- Validation targets the hidden input, not the text input
- All styles for the dropdown list go in site.css under a .bw-autocomplete block

## JavaScript Rules
- All JS in views goes inside @section Scripts — never inline in the body
- Use jQuery for all DOM manipulation and AJAX calls (already loaded in _Layout)
- Debounce all keyup handlers — do not fire on every keystroke
- @section Scripts is not available in partial views — JS for