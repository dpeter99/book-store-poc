import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import * as path from "node:path";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react({
      babel: {
        //  plugins: [['babel-plugin-react-compiler']],
      },
    }),
  ],
	resolve: {
		alias: {
			'@': path.resolve(__dirname, './src'),
		},
	},
	server: {
		host: true,
		port: parseInt(process.env.PORT ?? "5173"),
		proxy: {
			'/api': {
				target: (process.env.services__apiservice__https__0 || process.env.services__apiservice__http__0).replace('localhost', 'test.localhost'),
				changeOrigin: true,
				rewrite: path => path.replace(/^\/api/, ''),
				secure: false
			}
		}
	}
})
