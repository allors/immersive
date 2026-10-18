---
title: "Documentation website"
description: "The adoption of the Allors3 documentation standard for Immersive, what was reused, how it was validated, and the unpublished state of the website."
audience: "Maintainers"
kind: "Decision record"
status: "Planned"
---

> **Status: Planned.** The website, its Markdown exports and the checks that the documentation
> workflow runs are built and pass for both deployment paths. Publication to GitHub Pages has not
> been run and the repository's Pages settings are unchanged.

This record describes the documentation website of Immersive: what was decided, what was reused
from Allors3 and what remains to do. The [writing guide](writing-documentation.md) owns the
editorial rules and [Terminology](../terminology.md) owns the vocabulary. The pages live in
`docs/src/content/docs/`; the [website instructions](../../../../README.md) cover local
development, validation and publication.

## Decision

Immersive adopts the Allors documentation standard as first implemented for Allors3 in
[allors3 pull request #284](https://github.com/allors/allors3/pull/284), merged as commit
`33452c88c0e99b2db1243c4de0a061da23f0ae0c` on `main`, inspected on 2026-10-18. The Allors
documentation home is intended to present the Allors platforms and a collection of supporting
open-source tools, each product publishing its own documentation beneath its own path. Immersive
is one of the supporting tools; its documentation is smaller than a platform's and proportional
to its scope, with the same writing and accuracy standard.

| Area | Decision |
| --- | --- |
| Readers | Human documentation serves users of the weaver and the libraries, and maintainers. Agent instructions in `AGENTS.md` point to the same content. |
| Site | Astro with Starlight. `docs/` is the website project, with the content in `docs/src/content/docs/`. User and maintainer content have separate navigation groups. |
| Authoring | Markdown. MDX and components are available when a page needs them; none is used yet. |
| Hosting | GitHub Pages, built with GitHub Actions, at the path `/immersive/` beneath the shared origin `https://allors.github.io`, with `/` also validated. |
| Version context | Pages and exports identify the documented version as `main · v2.1 (preview)`, following `version.json` and the published preview packages. |
| Visuals | Mermaid where suitable; the dependency diagram in [How weaving works](../weaving.md) is the first. No interactive component yet. |
| Language and structure | Oxford English, precise practitioner terminology and Diátaxis, as defined in the writing guide. |
| Checks | The tooling tests, the Astro check, the production build with exports, the link check, a test that the control reference matches the libraries, and browser tests with accessibility checks, for both deployment paths. |

## What was reused

The following files were copied from the Allors3 implementation at the commit above, with only
the project's name, repository, branch, version, base path and page names changed:

| File | Change for Immersive |
| --- | --- |
| `astro.config.mjs` | Title, description, sidebar entries; the title is read from `site.mjs`. |
| `site.mjs` | Adds `title`; sets the repository, version and the default base path `/immersive/`. |
| `integrations/exports.mjs` | The agent index heading and description are built from `site.mjs`. |
| `integrations/source-links.mjs`, `integrations/scrollable-content.mjs` | Unchanged. |
| `scripts/check-links.mjs` | The default base path. Target paths in its messages use forward slashes on every platform, so that its tests pass on Windows; port this to Allors3. |
| `src/content.config.ts`, `src/components/PageTitle.astro`, `src/styles/documentation.css`, `public/favicon.ico`, `tsconfig.json`, `playwright.config.ts`, `.gitignore` | Unchanged. |
| `package.json`, `package-lock.json` | The package name; the pinned dependency versions are identical. |
| `tests/exports.test.mjs`, `tests/links.test.mjs`, `tests/source-links.test.mjs`, `tests/fixtures/anchors.md` | Base paths, repository paths and version strings. |
| `tests/browser/documentation.spec.ts` | The page names, the diagram and the table it exercises; the workspace example tests were dropped. |
| `.github/workflows/documentation.yml` | The base path matrix. |

Two additions are specific to Immersive: `scripts/controls-table.mjs` generates the rows of the
tables of [Substituted controls and testers](../controls.md) from the library sources and from
its own list of tester members, and `tests/controls.test.mjs` fails when the rows differ from
that output or a tester is missing from the page.

This second adoption shows what a shared implementation would contain: the three integrations,
the link checker, the title component, the stylesheet, the content schema, the tooling tests and
the workflow, parametrized by the values now in `site.mjs`. Extracting them is a decision for the
Allors documentation home; until then, keep the copies identical where the table says so, and
port fixes between the repositories.

## Validation

On 2026-10-19 the following ran locally from `docs/` for the base paths `/immersive/` and `/`:
`npm test`, `npm run check`, `npm run build`, `npm run check:links` and `npm run test:browser`,
as the [website instructions](../../../../README.md) describe. The examples in
[Getting started](../getting-started.md) were run from a clean directory against the published
`2.1.10-preview` packages, and the observed failure of the MSBuild package and its validated
change are recorded in [Run the weaver](../run-the-weaver.md#the-msbuild-package) and
[Weaver internals](weaver.md#msbuild-package).

Automated checks do not establish that an explanation is accurate or clear. The writing guide
requires editorial review of changed pages and of representative exports.

## Remaining work

- Publish the site: configure the repository's Pages source as GitHub Actions and its
  `github-pages` environment, then run the `Documentation` workflow on `main` with `publish`
  selected. The shared origin's own site and the custom domain are decisions of the documentation
  home, not of this repository.
- Decide with the other Allors repositories whether to extract the reused tooling.
- Revisit this record when the MSBuild package is changed, when `Allors.Immersive.Avalonia` is
  published, and when the website is published; mark it Historical once publication has run and
  the index links to the published site.
