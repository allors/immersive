# AGENTS.md

Instructions for agents working in this repository. The technical facts live in the human
documentation; this file says where to find them and how to work here.

## Change policy

Visibility: public. Change policy: conservative.

- Make only changes required by the explicit request. Preserve existing architecture,
  organization and conventions unless directly instructed otherwise.
- Follow existing test placement, fixtures and patterns; do not introduce a separate
  testing approach. Preserve established test expectations unless the requested change
  explicitly changes that behaviour.
- Edit documentation, including README files, changelogs, diagrams and guides, only when
  explicitly requested. Report related improvements or documentation implications
  without implementing them.
- An exception applies only to the change directly authorized; it does not waive the
  remaining rules. Change this policy only when explicitly instructed.

## Working agreement

- Stay within the approved objective and preserve unrelated work. Keep explanations and
  authorized documentation concise.
- Before asking for a decision, explain it fully in chat: the mechanism and relevant code,
  consequences, options, actual costs and available measurements, and a recommendation
  including the argument against it. Ask the question in plain text at the end.
- For long work explicitly divided into subtasks, describe the current subtask's changes,
  files or systems and validation before starting. Approval of a multi-subtask plan
  authorizes only its first subtask.
- At each boundary, summarize changes and verification, describe the next subtask and its
  validation, then wait for explicit approval. Proceed across boundaries only when the
  user explicitly authorizes it for the session. Re-check materially expanded scope.
- Do not invent subtask boundaries for simple tasks or routine implementation steps.
- At agreed boundaries, assess whether a fresh session would help. Before recommending
  one, record the branch, commit SHAs, validation, uncommitted work, pending decisions and
  exact next step. Use authorized project notes, otherwise provide the record in chat.
  Include a 2–4-line restart prompt; do not require a restart after every small task.

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
  engine only together with a test in `test/Allors.Immersive.Tests`. Report affected
  documentation; update it only when explicitly requested.
- The Windows Forms and Avalonia libraries follow one pattern per control: a substitute class in
  `Substitutes/`, its events in `Substitutes/events/` when it reports events, a tester in
  `Testers/` and a test class in the library's test project. Keep that pattern when adding a
  control. When documentation updates are explicitly requested, regenerate the tables of
  [Substituted controls and testers](docs/src/content/docs/controls.md) with
  `node scripts/controls-table.mjs` from `docs/`, after adding any new tester member to the
  `members` list in that script; `npm test` does not check the listed members. The
  `Documentation` workflow runs `npm test` for changes under the two library directories, so
  stale tables fail that check. If documentation updates are not authorized, report stale
  tables and affected member listings without editing them.
- The published MSBuild package fails a fresh `dotnet build`; the documented and validated change
  is in [Weaver internals](docs/src/content/docs/internals/weaver.md#msbuild-package). Do not
  describe the package as working until that change has shipped.
- Error messages should be actionable.

## Git

- Pull requests are the default. Creating or merging a pull request requires explicit
  approval. Keep one branch and one PR for an agreed body of work, with focused commits;
  open the PR when that work is complete.
- Use purpose prefixes such as `feature/`, `fix/`, `docs/` or `chore/`, not agent/tool names.
  Use conventional commits (`type(scope): description`) with no AI attribution.
- Keep the PR title and body accurate for the final diff. Update them when the branch's
  scope changes, including after review commits; describe the resulting behaviour and
  validation, not the development history.
- Default to squash merge. Use the PR title as the commit subject and its full description
  as the body, with wording that remains true after the merge.

- Every push to `main` publishes preview packages to nuget.org through the `CI` workflow, except
  changes limited to `docs/`, the documentation workflow and this file. See
  [Build and release](docs/src/content/docs/internals/build-and-release.md).

## Workflow

- Verify that the build succeeds and the tests pass before considering a task complete:
  `./build.cmd Test` on Windows, and `dotnet test` of `test/Allors.Immersive.Avalonia.Tests`,
  which the Nuke `Test` target does not run.
- For bug fixes, first add a failing regression in a suitable existing test class. Amend
  existing tests only as required by the requested change; preserve established expectations
  unless that change explicitly changes the behaviour.
- Keep implementation facts beside the code in tests and comments, and in documentation when
  its update is explicitly requested; do not maintain a second account of them here.

## Documentation

- `docs/src/content/docs/` is human documentation for users and maintainers. Before changing
  documentation, comments or public-facing explanations, read and apply the
  [writing guide](docs/src/content/docs/internals/writing-documentation.md) and the
  [terminology reference](docs/src/content/docs/terminology.md). The guide owns the audience,
  Diátaxis, status, Oxford English and visual explanation rules. Preserve exact identifiers and
  external names. Its maintenance rules do not expand the authorized documentation scope.
- Do not rename an established concept or substitute a competing term without prior agreement;
  explain the proposed meaning, reason and affected scope first. Apply an approved change
  consistently within the agreed scope across code, prose, diagrams, examples and pull
  request text.
- For explicitly requested documentation changes, maintain affected pages, the index, statuses
  and links within that scope. Otherwise, report pages made inaccurate by code changes.
  Use documentation from the checkout's branch or version when a published page differs.
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
