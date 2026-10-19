export const title = 'Immersive';
export const version = 'main · v2.1 (preview)';
export const repository = 'https://github.com/allors/immersive';
export const branch = 'main';
export const site = process.env.DOCS_SITE || 'https://allors.github.io';
export const base = `/${(process.env.DOCS_BASE || '/immersive/').replace(/^\/+|\/+$/g, '')}/`.replace('//', '/');
