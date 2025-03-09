import {defineConfig} from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig(({ mode }) => {
    return {
        plugins: [
            react(),
            {
                name: "html-update-script-tag",
                enforce: "post",
                transformIndexHtml(html: string) {
                    const regex = /<script type="module" crossorigin src="\//g;
                    const replacement = '<script type="module" crossorigin src="';
                    const output = html.replace(regex, replacement);
                    return output;
                },
            },
            {
                name: "html-update-link-tag",
                enforce: "post",
                transformIndexHtml(html: string) {
                    const regex = /<link rel="stylesheet" crossorigin href="\//g;
                    const replacement = '<link rel="stylesheet" crossorigin href="';
                    const output = html.replace(regex, replacement);
                    return output;
                },
            },
        ],
        build: {
            sourcemap: mode === 'development' ? true : false,
        },
    }
})
