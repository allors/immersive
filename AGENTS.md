# AGENTS.md

Instructions for agents working in this repository. The technical facts live in the human
documentation; this file says where to find them and how to work here.

## Scope

- Immersive is an IL weaver: after compilation it rewrites a target assembly so that its use of
  a referenced assembly goes to substitutes declared in an immerse assembly. The repository also
  holds two immerse assemblies for testing desktop applications, for Windows Forms and for
  Avalonia. [README.md](README.md) introduces the project and the
  [documentation index](docs/src/content/docs/index.md) lists the pages.
- Immersive is an open-source supporting tool of Allors. Public documentation, code comments,
  commit messages and GitHub text describe Immersive on its own. Do not name or link private,
  unpublished Allors projects.

## Development notes

- The weaver matches classes and methods by full name and uses the first constructor of a
  substitute class; [How weaving works](docs/src/content/docs/weaving.md) and
  [Weaver internals](docs/src/content/docs/internals/weaver.md) state the exact rules. Change the
  engine only together with a test in `test/Allors.Immersive.Tests` and the pages that describe
  the changed behaviour.
- The Windows Forms and Avalonia libraries follow one pattern per control: a substitute class in
  `Substitutes/`, its events in `Substitutes/events/` when it reports events, a tester in
  `Testers/` and a test class in the library's test project. Keep that pattern when adding a
  control. Regenerate the tables of
  [Substituted controls and testers](docs/src/content/docs/controls.md) with
  `node scripts/controls-table.mjs` from `docs/`, after adding any new tester member to the
  `members` list in that script; `npm test` does not check the listed members.
- The published MSBuild package fails a fresh `dotnet build`; the documented and validated change
  is in [Weaver internals](docs/src/content/docs/internals/weaver.md#msbuild-package). Do not
  describe the package as working until that change has shipped.
- Follow existing naming and structure; avoid new conventions without reason. Error messages
  should be actionable.

## Git

- Pull requests are the default. Creating a pull request requires approval.
- Use one branch and one pull request for an agreed body of work. Its parts land as focused
  commits on that branch. Create the pull request only when that work is complete; if one already
  exists, update it.
- Keep the title and description of a pull request correct for everything on its branch, and
  update them when the branch changes. For a squash merge, the title is the commit subject and the
  description is the commit body, describing the final changes and their validation.
- Use the conventional commit format `type(scope): description`. Keep commits focused and
  well-described. No AI attribution in commits or pull requests.
- Use purpose prefixes for branch names, such as `feature/`, `fix/`, `docs/` or `chore/`, not the
  name of an agent or tool.
- Every push to `main` publishes preview packages to nuget.org through the `CI` workflow, except
  changes limited to `docs/`, the documentation workflow and this file. See
  [Build and release](docs/src/content/docs/internals/build-and-release.md).

## Workflow

- Verify that the build succeeds and the tests pass before considering a task complete:
  `./build.cmd Test` on Windows, and `dotnet test` of `test/Allors.Immersive.Avalonia.Tests`,
  which the Nuke `Test` target does not run.
- When fixing a bug, first write a failing test or amend an existing one. Add a new test to a
  suitable existing class. Do not change an existing test unless instructed to.
- Keep implementation facts beside the code: in tests, in code comments and in the documentation
  pages; do not maintain a second account of them here.

## Documentation

- `docs/src/content/docs/` is human documentation for users and maintainers. Before changing
  documentation, comments or public-facing explanations, read and apply the
  [writing guide](docs/src/content/docs/internals/writing-documentation.md) and the
  [terminology reference](docs/src/content/docs/terminology.md). The guide owns the audience,
  Diátaxis, status, Oxford English and visual explanation rules. Preserve exact identifiers and
  external names.
- Do not rename an established concept or substitute a competing term without prior agreement;
  explain the proposed meaning, reason and affected scope first. Apply an approved change
  consistently across code, prose, diagrams, examples and pull request text.
- Update pages made untrue by a change in the same pull request. Maintain the index, page
  statuses and links according to the writing guide. Use documentation from the checkout's
  branch or version when a published page describes something different.
- Date a verification that depends on an external state, such as a published package version,
  and say what was checked. Run a tutorial from a clean directory before changing its steps or
  its shown output.
- Before completing work, review the changed material for factual accuracy, agreed terminology,
  Oxford spelling and clear practitioner language, including labels, example output and pull
  request text. Editorial review is required; no prose linter is part of the tooling.
- The website in `docs/` has its own checks. Run them from `docs/` before completing documentation
  work, as [docs/README.md](docs/README.md) describes: `npm test`, `npm run check`,
  `npm run build`, `npm run check:links` and `npm run test:browser`.

## Build commands

The build uses Nuke: `./build.cmd Compile`, `./build.cmd Test`, `./build.cmd Pack`. The
documentation website in `docs/` builds with npm; [docs/README.md](docs/README.md) lists its
commands.
