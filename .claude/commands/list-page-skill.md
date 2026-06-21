---
name: List Page Skill
description: Use this skill when creating a new Index/list view and its supporting controller action and repository method in BreweryWarehouse.Web.
---

## Scope — Files To Create Or Modify
- BreweryWarehouse.Web/Controllers/{Entity}Controller.cs — add Index() action if missing
- BreweryWarehouse.Web/Repositories/{Entity}Repository.cs — verify GetAll() exists
- BreweryWarehouse.Web/Views/{Entity}/Index.cshtml — create the list view

## List Page Conventions
- Use the brewery industrial design system defined in .github/agents/ux-agent.agent.md
- Table must be full-width with border-collapse: collapse
- thead: background --bw-surface-2, uppercase condensed 12px, text --bw-text-muted
- tbody rows: alternating --bw-surface / --bw-surface-2, hover brightness(1.08)
- Last column: Details link styled as small amber pill button using asp-controller, asp-action, asp-route-id tag helpers
- Rows where BestBefore is within 30 days get class row-expiring and text --bw-danger
- No logic in views beyond simple @if and @foreach
- Use @Html.DisplayFor and @Html.DisplayNameFor where possible
- DateTime format: item.BestBefore.ToString("dd MMM yyyy")
- No inline style attributes — all styles go in wwwroot/css/site.css

## Post-Task Rules
- Always update CLAUDE.md after adding a new page
- Always update docs/sitemap.md with the new route
