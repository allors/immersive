import { readdirSync, readFileSync } from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

/** Generates the control tables of docs/src/content/docs/controls.md from the library sources. */
const root = fileURLToPath(new URL('../../', import.meta.url));

const members = {
  Winforms: {
    BindingNavigator: '`AddNewItem()`',
    Button: '`Click()`',
    ComboBox: '`Select<T>(Predicate<T>)`',
    DataGridView: '`FindRow(predicate)`, `FindRows(predicate)`',
    MenuItem: '`Click()`',
    PropertyGrid: '`FindGridItem(params string[] names)`',
    ToolBar: '`Click(ToolBarButtonTester)`',
    ToolBarButton: '`Click()`',
    ToolStripButton: '`Click()`',
    TreeView: '`SelectNode(params int[])`, `SelectNode(TreeNodeTester)`, `FindNode(params int[])`, `SelectedNode`, `this[string[] ancestors]`',
  },
  Avalonia: {
    Button: '`Click()`',
    MenuItem: '`Click()`',
    RepeatButton: '`Click()`',
    SplitButton: '`Click()`',
    ToggleButton: '`Click()`',
    ToggleSplitButton: '`Click()`',
  },
};

function read(file) {
  return readFileSync(file, 'utf8').replace(/^﻿/, '');
}

function names(directory, suffix) {
  return readdirSync(directory, { withFileTypes: true })
    .filter((entry) => entry.isFile() && entry.name.endsWith(suffix))
    .map((entry) => entry.name.slice(0, -suffix.length))
    .sort();
}

export function controlRows(library, kind) {
  const base = path.join(root, 'src', `Allors.Immersive.${library}`);
  const substitutes = path.join(base, 'Substitutes');
  const testers = names(path.join(base, 'Testers'), 'Tester.cs');
  const classes = new Map(names(substitutes, '.cs').map((name) => {
    const source = read(path.join(substitutes, `${name}.cs`));
    const namespace = source.match(/\bnamespace\s+([A-Za-z0-9_.]+)/)?.[1];
    if (!namespace) throw new Error(`${library}/${name}: cannot find the namespace.`);
    const baseClass = source.match(new RegExp(`class ${name}\\s*:\\s*((?:global::)?[A-Za-z0-9_.]+)`))?.[1];
    return [name, { source, namespace, baseClass }];
  }));
  const qualifiedNames = new Map([...classes].map(([name, { namespace }]) => [`${namespace}.${name}`, name]));
  return [...classes].map(([name, { source, baseClass }]) => {
    let mode;
    let target;
    const explicit = source.match(/SubstituteClass\(typeof\s*\(([^)]*)\)\)/);
    if (explicit) {
      mode = 'Explicit';
      target = explicit[1].replace(/\s/g, '');
    } else if (/\[SubstituteClass\]/.test(source)) {
      mode = 'Base';
      if (!baseClass) throw new Error(`${library}/${name}: cannot find the base class in the class declaration.`);
      target = baseClass.replace(/^global::/, '');
    } else {
      mode = 'Static `Show` overloads';
      const method = source.match(/SubstituteMethod\(typeof\s*\(([^)]*)\)\)/);
      if (!method) throw new Error(`${library}/${name}: neither SubstituteClass nor SubstituteMethod found.`);
      target = method[1].replace(/\s/g, '');
    }
    const events = new Set();
    const visited = new Set();
    for (let current = name; current;) {
      if (visited.has(current)) throw new Error(`${library}/${name}: cyclic substitute inheritance through ${current}.`);
      visited.add(current);
      for (const file of [path.join(substitutes, `${current}.cs`), path.join(substitutes, 'events', `${current}.cs`)]) {
        try {
          for (const match of read(file).matchAll(new RegExp(`${kind}\\.([A-Za-z]+)`, 'g'))) events.add(match[1]);
        } catch (error) {
          if (error.code !== 'ENOENT') throw error;
        }
      }
      const ancestor = classes.get(current);
      if (!ancestor.baseClass) break;
      // Follow local substitutes only; a framework class with the same name is a different type.
      const qualifiedBase = ancestor.baseClass.startsWith('global::')
        ? ancestor.baseClass.slice('global::'.length)
        : ancestor.baseClass.includes('.') ? ancestor.baseClass : `${ancestor.namespace}.${ancestor.baseClass}`;
      current = qualifiedNames.get(qualifiedBase);
    }
    const tester = testers.includes(name) ? `\`${name}Tester\`` : '—';
    return `| \`${target}\` | \`${name}\` | ${mode} | ${tester} | ${[...events].sort().join(', ')} | ${members[library][name] ?? ''} |`;
  });
}

export const header = '| Substituted type | Substitute | Substitution | Tester | Events reported | Tester members |\n| --- | --- | --- | --- | --- | --- |';

if (process.argv[1] && path.resolve(process.argv[1]) === fileURLToPath(import.meta.url)) {
  console.log(`## Windows Forms rows\n\n${header}\n${controlRows('Winforms', 'TestingWinformsEventKind').join('\n')}\n`);
  console.log(`## Avalonia rows\n\n${header}\n${controlRows('Avalonia', 'TestingAvaloniaEventKind').join('\n')}`);
}
