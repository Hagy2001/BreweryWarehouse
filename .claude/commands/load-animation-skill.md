---
name: Load Animation Skill
description: Use this skill when creating or modifying the brewery load animation overlay in BreweryWarehouse.Web.
---

## Scope — Files To Create Or Modify
- `BreweryWarehouse.Web/wwwroot/css/load-animation.css` — all overlay and animation styles
- `BreweryWarehouse.Web/wwwroot/js/load-animation.js` — sequencing and dismiss logic
- `BreweryWarehouse.Web/Views/Shared/_Layout.cshtml` — overlay HTML, css/script includes

## Design Intent
The load animation must feel industrial and deliberate — not playful, not bouncy.
It is the first thing the user sees and sets the tone for the entire application.
No vessel or glass shape. The screen IS the container. Amber liquid rises to fill
the entire viewport from the bottom upward, revealing the brewery logo as it climbs.

---

## Visual Specification

### Fill
- Full-viewport dark overlay (`--bw-bg`, z-index 9999)
- `.bw-load-fill` div: `background: var(--bw-accent)`, starts at `translateY(100vh)`, rises to `translateY(0)`
- Easing: `cubic-bezier(0.4, 0.0, 0.2, 1.0)` — slow start, steady middle, gentle arrival
- Duration: `2500ms`, delay: `150ms`

### Wave surface
- SVG wave (`viewBox="0 0 1440 40"`) sits `top: -40px` above the fill div, rises with it
- Three `<path>` elements:
  - **Primary** (`fill: var(--bw-accent)`): sinusoidal, full opacity
  - **Secondary** (`fill: var(--bw-accent)`, `opacity: 0.35`): opposite-phase counterpart
  - **Foam** (`fill: rgba(232,200,90,0.72)`): lighter amber, centered at y=10, bottom closure at y=30
- Wave animation: **SMIL `<animate>` only** — morphs between two opposite-phase `d` values over 3s, `repeatCount="indefinite"`
- Secondary wave SMIL uses `begin="-1.5s"` to run at 180° phase offset (half of 3s cycle)
- **Do not use CSS `d` property animation** — it is unreliable in Chrome across versions
- Wave amplitude: control points at y=0 and y=20 (±10 from center y=10 for foam, ±10 from y=20 for primary/secondary)

### Logo reveal
- Logo SVG is inline inside `.bw-load-logo-viewport` SVG element
- A `<mask>` with a rising white rect (`<animate>` on `y` and `height`) reveals the logo bottom-to-top in sync with the fill rise
- **The `<defs>` containing the mask MUST be inside the same `<svg>` as the masked `<g>`** — Chrome resolves `maskContentUnits="userSpaceOnUse"` against the defining SVG's viewport; a separate zero-size SVG element makes the mask invisible in Chrome
- `bw-load-logo--base`: `fill: var(--bw-accent)` — the amber logo always visible
- `bw-load-logo--overlay`: `fill: var(--bw-bg)` — dark logo, revealed as the mask rises, sits on top of the amber base to create a "dark logo over amber liquid" effect

### Bubbles
- 12 `<span>` elements inside `.bw-load-bubbles` (child of `.bw-load-fill`)
- `bottom: 20vh–91vh` range — ensures bubbles enter the visible area progressively as the fill rises (a bubble at `bottom: Xvh` inside the fill div becomes screen-visible when `translateY < Xvh`)
- Sizes: 5px–9px diameter, varied across the 12
- Animation: **single run** (`1 forwards`), `ease-in`, rise `-28vh`
- Delays staggered 0s–3.2s so bubbles appear at different phases of the fill rise
- Keyframe: quick fade-in (0–10%), hold (10–55%), slow fade-out (55–100%), final opacity 0 — `forwards` keeps them gone after their run

### Dismiss
- At 3050ms: `bw-overlay--dismiss` class added → `transform: translateY(-100vh)` transition over 600ms
- `transitionend` removes overlay from DOM
- Hard fallback `setTimeout` at 3700ms removes overlay if `transitionend` never fires
- `sessionStorage` key: `bw-animated` — currently commented out during development; restore to re-enable once-per-session behaviour

---

## Timing Sequence
1. Page load — overlay immediately visible, fill at `translateY(100vh)`
2. `150ms` — fill begins rising, mask reveal begins, wave SMIL running
3. `2650ms` — fill at top, logo fully revealed
4. `400ms` pause
5. `3050ms` — dismiss class applied, overlay slides up over 600ms
6. `transitionend` (or 3700ms fallback) — overlay removed from DOM

**Total before DOM removal: 3650–3700ms**

---

## Technical Rules
- `load-animation.css` is a **separate file** — never add these styles to `site.css`
- `load-animation.js` handles only sequencing: sessionStorage check, reduced-motion check, dismiss class, DOM removal
- No JS animation, no `requestAnimationFrame`, no Web Animations API
- Wave morphing is SMIL `<animate>` on the `d` attribute — not CSS keyframes
- Mask `<defs>` always inside the same SVG as the masked element
- Overlay must be fully removed from DOM after dismiss (no `display:none` leftovers)
- If `prefers-reduced-motion: reduce`: remove overlay instantly, skip all animation
- Bubble `<span>` elements are children of `.bw-load-fill` so they translate up with the liquid automatically

---

## Cross-Browser Gotchas
- **Chrome SVG mask**: Chrome resolves `maskContentUnits="userSpaceOnUse"` against the coordinate space of the SVG that owns the `<defs>`. If the `<defs>` is in a different SVG (even `width=0 height=0`), the mask rect has zero effective size and nothing is revealed. Fix: always colocate `<defs>` and the masked element in the same `<svg>`.
- **CSS `d` property**: Chrome support for `@keyframes` animating the `d` attribute is inconsistent across versions. Always use SMIL `<animate attributeName="d">` for path morphing.
- **SMIL `fill` attribute**: Use `fill="freeze"` on mask reveal animates so the revealed state persists after the animation ends.
- **Wave SVG `overflow: visible`**: Required so the wave path can extend above y=0 of its SVG element without clipping.

---

## Post-Task Rules
- Always update `CLAUDE.md` after any change to the animation
- Always update this skill file if the timing, wave approach, bubble strategy, or logo reveal technique changes
- Do not modify any controller, repository, ViewModel, or EF entity
- Do not add animation styles to `site.css`
