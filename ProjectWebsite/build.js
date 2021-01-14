const csso = require('csso');
const { dirname, extname, join, resolve } = require('path');
const { existsSync, mkdirSync, rmSync, writeFileSync, readFileSync } = require('fs');
const readdir = require('fs-readdir-recursive');
const htmlmin = require('html-minifier-terser');

const src = resolve('./src');
const dist = resolve('./dist');

if (existsSync(dist)) {
  rmSync(dist, { recursive: true, force: true });
}
mkdirSync(dist);

const srcFiles = readdir(src);

srcFiles.forEach(s => {
  const d = join(dist, s);
  const ext = extname(s);
  switch (ext) {
    case '.css':
      copyCssFile(s, d);
      break;
    case '.html':
    case '.svg':
      copyMarkupFile(s, d);
        break;
    default:
      copyFile(s, d);
  }
});

function copyCssFile(source, destination) {
  const css = readFileSync(join(src, source)).toString('utf-8');
  const min = csso.minify(css).css;
  createFile(destination, min);
}

function copyMarkupFile(source, destination) {
  const html = readFileSync(join(src, source)).toString('utf-8');
  const min = htmlmin.minify(html, {
    collapseWhitespace: true,
    preserveLineBreaks: false,
    minifyCSS: true
  });
  createFile(destination, min, { encoding: 'utf-8' });
}

function copyFile(source, destination) {
  const path = dirname(destination);
  if (!existsSync(path)) {
    mkdirSync(path, { recursive: true });
  }
  const data = readFileSync(join(src, source));
  writeFileSync(destination, data);
}

function createFile(destination, content) {
  const path = dirname(destination);
  if (!existsSync(path)) {
    mkdirSync(path, { recursive: true });
  }
  writeFileSync(destination, content, { encoding: 'utf-8' });
}
