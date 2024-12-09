import containerQueries from '@tailwindcss/container-queries'
import forms from '@tailwindcss/forms'
import typography from '@tailwindcss/typography'
import type { Config } from 'tailwindcss'
import animate from 'tailwindcss-animate'
import { fontFamily } from 'tailwindcss/defaultTheme'

const config: Config = {
   darkMode: ['class'],
   content: ['./index.html', './src/**/*.{ts,tsx,js,jsx}'],
   theme: {
      extend: {
         borderRadius: {
            lg: 'var(--radius)',
            md: 'calc(var(--radius) - 2px)',
            sm: 'calc(var(--radius) - 4px)'
         },
         colors: {
            background: 'hsl(var(--background))',
            foreground: 'hsl(var(--foreground))',
            card: {
               DEFAULT: 'hsl(var(--card))',
               foreground: 'hsl(var(--card-foreground))'
            },
            popover: {
               DEFAULT: 'hsl(var(--popover))',
               foreground: 'hsl(var(--popover-foreground))'
            },
            primary: {
               DEFAULT: 'hsl(var(--primary))',
               foreground: 'hsl(var(--primary-foreground))'
            },
            secondary: {
               DEFAULT: 'hsl(var(--secondary))',
               foreground: 'hsl(var(--secondary-foreground))'
            },
            muted: {
               DEFAULT: 'hsl(var(--muted))',
               foreground: 'hsl(var(--muted-foreground))'
            },
            accent: {
               DEFAULT: 'hsl(var(--accent))',
               foreground: 'hsl(var(--accent-foreground))'
            },
            destructive: {
               DEFAULT: 'hsl(var(--destructive))',
               foreground: 'hsl(var(--destructive-foreground))'
            },
            border: 'hsl(var(--border))',
            input: 'hsl(var(--input))',
            ring: 'hsl(var(--ring))',
            chart: {
               1: 'hsl(var(--chart-1))',
               2: 'hsl(var(--chart-2))',
               3: 'hsl(var(--chart-3))',
               4: 'hsl(var(--chart-4))',
               5: 'hsl(var(--chart-5))'
            },
            sidebar: {
               DEFAULT: 'hsl(var(--sidebar-background))',
               foreground: 'hsl(var(--sidebar-foreground))',
               primary: 'hsl(var(--sidebar-primary))',
               'primary-foreground': 'hsl(var(--sidebar-primary-foreground))',
               accent: 'hsl(var(--sidebar-accent))',
               'accent-foreground': 'hsl(var(--sidebar-accent-foreground))',
               border: 'hsl(var(--sidebar-border))',
               ring: 'hsl(var(--sidebar-ring))'
            },

            custom: {
               'bg-primary': 'hsl(var(--bg-primary))',
               'bg-secondary': 'hsl(var(--bg-secondary))',
               'active-accent': 'hsl(var(--active-accent))',
               'hover-accent': 'hsl(var(--hover-accent))',
               'dark-grey': 'hsl(var(--dark-grey))',
               'slate-1': 'hsl(var(--slate-1))'
            }
         },
         boxShadow: {
            button: '0 0 7px 1px var(--tw-shadow-color)'
         },
         fontFamily: {
            sans: [...fontFamily.sans],
            body: ['Nunito', 'sans-serif']
         }
      }
   },
   plugins: [animate, forms, typography, containerQueries]
}

export default config
