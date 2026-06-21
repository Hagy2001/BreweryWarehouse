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
- @section Scripts is not available in partial views — JS for partials must be added in the parent view's @section Scripts block

## Global Search

A cross-entity search is available at `GET /search/global?q={query}` (GlobalSearchController).

**Deviations from per-entity Search() conventions:**
- Minimum 2 characters required — empty/short queries return an empty array, NOT all records.
  This is intentional: returning all records from 6 tables simultaneously is never useful.
- Results are capped at 5 per entity type (~25–30 total max) so the dropdown stays manageable.
- Requires authentication (`[Authorize]` at class level); per-entity Search actions use `[AllowAnonymous]`.

**Response shape** — flat array, each item:
```json
{ "category": "Beer Style", "label": "Imperial Stout", "subtitle": "Stout, 8.5% ABV", "url": "/beer-styles/12/detail" }
{ "category": "Page", "label": "Locations", "subtitle": null, "url": "/locations" }
```
`category` is always present; `subtitle` may be null. `url` is always an absolute path.

**Ordering:** Pages appear first, followed by each entity type (Beer Style → Can → Keg → Location → Stock Entry → Employee). The dropdown groups by `category` with a labelled section header per group.

**Entity fields searched:**
- BeerStyle → Name
- Can → SLCode, Barcode
- Keg → SLCode, SerialNumber
- WarehouseLocation → LocationCode
- StockEntry → Container.SLCode, Location.LocationCode
- Employee → FirstName, LastName

**Page entries:** Static list matching the sidebar nav. "User Roles" page is only included when the current user is in the Admin role (mirrors the conditional nav link).

**URL generation:** URLs are built with `Url.Action` — not hardcoded strings — because per-entity detail routes are non-uniform (`/beer-styles/{id}/detail`, `/cans/{id}/info`, `/kegs/{id}/info`, `/locations/{id}/view`, `/StockEntry/Details/{id}`, `/Employee/Details/{id}`).

**JS integration (site.js):** `#bw-global-search-input` triggers debounced (300ms) `$.getJSON('/search/global', { q })`. Results render into `#bw-global-search-results` (`<ul>`) grouped by category with `.bw-global-search__group-label` section headers. Clicking a result navigates to its `url`. Click-outside and Escape close the dropdown.

**CSS namespace:** `.bw-global-search` — separate from `.bw-autocomplete` to avoid visual/behavioral collisions with the BeerStyle autocomplete partial.
