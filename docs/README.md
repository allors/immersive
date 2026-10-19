# Documentation website

> **Status: Current.** How-to guide for maintainers.

The Immersive documentation uses Astro and Starlight. Human pages live in
[`src/content/docs/`](src/content/docs/index.md); start with the
[documentation index](src/content/docs/index.md). This website documents `main`, which publishes
**v2.1 preview** packages. Use documentation from the relevant branch or version when working on
another one.

## Run locally

Use Node.js 24 and npm. The website has its own package and lockfile. From the repository root:

```sh
cd docs
npm ci
npm run dev
```

Open the local URL printed by Astro, under `/immersive/`. Development mode provides a fast
preview; use a production build to check search and generated exports:

```sh
npm run build
npm run preview
```

The build clears Astro's content cache so changes to the local Markdown transformations are
applied to every page.

## Edit a page

Read the [writing guide](src/content/docs/internals/writing-documentation.md) and
[terminology reference](src/content/docs/terminology.md) before editing. Keep Markdown as the
default; use MDX when a page needs a component, with the component implementation separate
from the prose.

Every page has frontmatter with `title`, `description`, `audience`, `kind` and `status`.
Keep its opening status line and the documentation index consistent with that metadata.
Starlight renders the title, so page content starts with the status line, followed by
second-level headings. Update navigation in `astro.config.mjs` when adding a page.

Use relative Markdown links between source pages so they remain useful in the checkout.
The build converts them to website URLs. Repository links outside the documentation are
converted to GitHub links, and the build checks their heading anchors against the checkout.
Check headings and anchors after changing a title or moving a page.

Use Mermaid code fences for diagrams where the notation fits. Diagrams render in the browser;
a successful site build alone does not validate their rendering. Keep the explanation of
arrows and notation in the page, and provide accessible labels as described in the writing
guide.

The rows of the tables in [Substituted controls and testers](src/content/docs/controls.md) are
generated: after adding or renaming a substitute or tester, or adding a tester member, update the
`members` list in `scripts/controls-table.mjs` when needed, run `node scripts/controls-table.mjs`
from `docs/` and replace the rows with its output. `npm test` fails while the rows differ from
that output or a tester is missing from the page; it does not check the listed tester members.
The event column includes events inherited from other substitute classes. Framework base classes
do not contribute testing events.

## Check a change

Run these commands from `docs/`:

```sh
npm test
npm run check
npm run build
npm run check:links
npx playwright install chromium
npm run test:browser
```

The browser checks use the production preview and cover navigation, search, the diagram and
accessibility at desktop and narrow widths. On Linux, use
`npx playwright install --with-deps chromium` to install system dependencies as well.
Review prose and representative rendered pages too; automated checks do not establish that
an explanation is accurate or clear.

The default deployment origin is `https://allors.github.io`, with `/immersive/` as the base path.
`DOCS_SITE` changes the origin and `DOCS_BASE` changes the base path. Use the same settings for
build, link checking and preview. For example, check deployment at the origin root:

```sh
DOCS_BASE=/ npm run build
DOCS_BASE=/ npm run check:links
DOCS_BASE=/ npm run test:browser
```

Rebuild with the default settings before previewing or publishing under `/immersive/` again.
The documentation workflow validates both base paths.

In Git Bash on Windows, MSYS rewrites the value `/` into a Windows path before Node sees it,
so the root-path build silently uses a wrong base. Set `MSYS_NO_PATHCONV=1` for those
commands, or run them from PowerShell with `$env:DOCS_BASE = '/'`.

## Review exports

The build generates Markdown exports and `llms.txt` from rendered page content and the same
metadata used by the website. Titles, audience, kind, status and version context belong to
that source; do not maintain a separate explanation for agents. `llms.txt` is an agent
discovery proposal. Agents working in a checkout should use its documentation.

Each page is exported at `<slug>.md` under the deployment base path; the documentation index
is `index.md`. These files and `llms.txt` are generated during production builds.

Mermaid source is retained in Markdown exports. A custom interactive component must mark its
wrapper with `data-documentation-interactive` and render a useful static explanation that
preserves its meaning. `<details>` explanations are retained in exports. Give SVG drawings a
title and description, or a figure caption. Use `data-documentation-export-ignore` only for
interface controls that add no meaning to the explanation. Inspect the relevant exported
Markdown after changing a diagram or component.

## Publish

The [documentation workflow](../.github/workflows/documentation.yml) checks relevant pull
requests and changes to `main`, including documentation files and the source directories
`src/Allors.Immersive.Winforms/` and `src/Allors.Immersive.Avalonia/`. Changes to either library
run the control-table consistency checks along with the other website checks. Those events
validate the site without publishing it.

After publication has been approved, configure the repository's Pages source as **GitHub
Actions** and its `github-pages` environment for deployment from `main`. Run the
**Documentation** workflow manually on `main` with **publish** selected. Both base-path
validation jobs must pass before the workflow deploys the `/immersive/` build. The default
manual run leaves **publish** unselected and only validates.

The workflow follows GitHub's
[custom Pages workflow](https://docs.github.com/en/pages/getting-started-with-github-pages/using-custom-workflows-with-github-pages)
model. Changing the workflow does not configure repository hosting or publish the site.
