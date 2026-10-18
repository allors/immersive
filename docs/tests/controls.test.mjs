import assert from 'node:assert/strict';
import { readdirSync, readFileSync } from 'node:fs';
import path from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';
import { controlRows } from '../scripts/controls-table.mjs';

const root = fileURLToPath(new URL('../../', import.meta.url));
const page = readFileSync(fileURLToPath(new URL('../src/content/docs/controls.md', import.meta.url)), 'utf8');

function section(heading) {
  const start = page.indexOf(`\n## ${heading}\n`);
  assert.notEqual(start, -1, `controls.md has a "${heading}" section`);
  const rest = page.slice(start + 1);
  const end = rest.indexOf('\n## ', 1);
  return end === -1 ? rest : rest.slice(0, end);
}

function rows(text) {
  return text.split('\n').filter((line) => line.startsWith('| `'));
}

function files(directory, suffix) {
  return readdirSync(path.join(root, directory), { withFileTypes: true })
    .filter((entry) => entry.isFile() && entry.name.endsWith(suffix))
    .map((entry) => entry.name.slice(0, -suffix.length))
    .sort();
}

for (const [heading, library, kind] of [
  ['Windows Forms', 'Winforms', 'TestingWinformsEventKind'],
  ['Avalonia', 'Avalonia', 'TestingAvaloniaEventKind'],
]) {
  test(`the ${heading} tables match the generated rows and the library sources`, () => {
    const text = section(heading);
    const generated = controlRows(library, kind);
    const substituteRows = rows(text).filter((line) => !/^\| `[A-Za-z]+Tester` \|/.test(line));
    assert.deepEqual(substituteRows, generated, `Run "node scripts/controls-table.mjs" from docs/ and replace the ${heading} table.`);

    const substitutes = files(`src/Allors.Immersive.${library}/Substitutes`, '.cs');
    assert.deepEqual(generated.map((line) => line.split(' | ')[1].replace(/`/g, '')), substitutes);

    const testers = files(`src/Allors.Immersive.${library}/Testers`, 'Tester.cs');
    const listed = [...text.matchAll(/`([A-Za-z]+)Tester`/g)].map((match) => match[1]);
    assert.deepEqual([...new Set(listed)].sort(), testers, `Every tester of the ${library} library is listed in its section.`);
  });
}
