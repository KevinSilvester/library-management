import path from 'node:path';
import { defineConfig } from 'vite';
import { sveltekit } from '@sveltejs/kit/vite';

const host = process.env.TAURI_DEV_HOST;

export default defineConfig(async () => ({
   plugins: [sveltekit()],
   resolve: {
      alias: {
         $lib: path.resolve('./src/lib')
      }
   },
   clearScreen: false,
   server: {
      port: 1420,
      strictPort: true,
      host: host || false,
      hmr: host
         ? {
              protocol: 'ws',
              host,
              port: 1421
           }
         : undefined,
      watch: {
         ignored: ['**/src-tauri/**']
      }
   }
}));
