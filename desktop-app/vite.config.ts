import { TanStackRouterVite } from '@tanstack/router-plugin/vite'
import react from '@vitejs/plugin-react-swc'
import path from 'node:path'
import { defineConfig } from 'vite'

const host = process.env.TAURI_DEV_HOST

export default defineConfig({
   plugins: [react()],
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
   },
   resolve: {
      alias: {
         '@': path.resolve(__dirname, './src')
      }
   }
})
