import {defineConfig} from 'vite';
import {JSDOM} from 'jsdom';
import react from '@vitejs/plugin-react';

export default defineConfig(({mode}) => {
    return {
        plugins: [
            react(),
            {
                name: "html-remove-first-slash",
                enforce: "post",
                transformIndexHtml(html: string) {
                    const jsdom = new JSDOM(html);
                    console.log('\n---- Onefocus -------------------------------------------------------------------------');
                    console.log('\x1b[32m', '\nReplacing first "/" in <script> and <link> tags:');
                    console.log('\x1b[33m');
                    for (const script of jsdom.window.document.querySelectorAll('script')) {
                        if (script.type == 'module' && script.src.startsWith('/')) {
                            console.log('Replacing script with url: ' + script.src + '...');
                            script.src = script.src.slice(1);
                            console.log('Replaced to: ' + script.src);
                        }
                    }
                    console.log('\x1b[34m');
                    for (const link of jsdom.window.document.querySelectorAll('link')) {
                        if (link.rel == 'stylesheet' && link.href.startsWith('/')) {
                            console.log('Replacing link with url: ' + link.href + '...');
                            link.href = link.href.slice(1);
                            console.log('Replaced to: ' + link.href);
                        }
                    }
                    console.log('\x1b[0m');
                    console.log('-----------------------------------------------------------------------------------------');
                    return jsdom.serialize();
                },
            },
        ],
        build: {
            sourcemap: mode === 'development' ? true : false,
        },
        css: {
            preprocessorOptions: {
                scss: {
                    api: 'modern',
                    quietDeps: true,
                },
            },
        },
    };
});
