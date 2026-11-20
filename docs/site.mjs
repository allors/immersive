export const title = 'Immersive';
export const description = 'Documentation for users of the Immersive IL weaver and for its maintainers.';
export const version = 'main · v2.1 (preview)';
export const repository = 'https://github.com/allors/immersive';
export const branch = 'main';
// The root of the repository, which holds this website in docs/. A link from a page to a file that is not
// a page becomes a link to that file on GitHub, by its path from this root.
export const repositoryRoot = new URL('../', import.meta.url);
// The audiences that a page can name, in the order in which the agent index lists them. The content
// schema accepts these names and no others.
export const audiences = ['Users and maintainers', 'Users', 'Maintainers'];
export const site = process.env.DOCS_SITE || 'https://docs.allors.com';
export const base = `/${(process.env.DOCS_BASE || '/immersive/').replace(/^\/+|\/+$/g, '')}/`.replace('//', '/');
// The documentation home is published at the root of the same origin by allors/allors.github.io. The
// sidebar links to it, and the link checker leaves that link to the home's own build.
export const home = new URL('/', site).href;
