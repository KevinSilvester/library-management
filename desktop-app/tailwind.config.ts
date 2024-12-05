import type { Config } from 'tailwindcss';
import { fontFamily } from 'tailwindcss/defaultTheme';

const config: Config = {
   darkMode: ['class'],
   content: ['./src/**/*.{html,js,svelte,ts}'],
   safelist: ['dark'],
   theme: {
      container: {
         center: true,
         padding: '2rem',
         screens: {
            '2xl': '1400px'
         }
      },
      extend: {
         colors: {
            border: 'hsl(var(--border) / <alpha-value>)',
            input: 'hsl(var(--input) / <alpha-value>)',
            ring: 'hsl(var(--ring) / <alpha-value>)',
            background: 'hsl(var(--background) / <alpha-value>)',
            foreground: 'hsl(var(--foreground) / <alpha-value>)',
            primary: {
               DEFAULT: 'hsl(var(--primary) / <alpha-value>)',
               foreground: 'hsl(var(--primary-foreground) / <alpha-value>)'
            },
            secondary: {
               DEFAULT: 'hsl(var(--secondary) / <alpha-value>)',
               foreground: 'hsl(var(--secondary-foreground) / <alpha-value>)'
            },
            destructive: {
               DEFAULT: 'hsl(var(--destructive) / <alpha-value>)',
               foreground: 'hsl(var(--destructive-foreground) / <alpha-value>)'
            },
            muted: {
               DEFAULT: 'hsl(var(--muted) / <alpha-value>)',
               foreground: 'hsl(var(--muted-foreground) / <alpha-value>)'
            },
            accent: {
               DEFAULT: 'hsl(var(--accent) / <alpha-value>)',
               foreground: 'hsl(var(--accent-foreground) / <alpha-value>)'
            },
            popover: {
               DEFAULT: 'hsl(var(--popover) / <alpha-value>)',
               foreground: 'hsl(var(--popover-foreground) / <alpha-value>)'
            },
            card: {
               DEFAULT: 'hsl(var(--card) / <alpha-value>)',
               foreground: 'hsl(var(--card-foreground) / <alpha-value>)'
            },
            custom: {
               slate: {
                  50: 'hsl(var(--slate-50))',
                  100: 'hsl(var(--slate-100))',
                  200: 'hsl(var(--slate-200))',
                  300: 'hsl(var(--slate-300))',
                  400: 'hsl(var(--slate-400))'
               },
               blue: {
                  100: 'hsl(var(--blue-100))',
                  200: 'hsl(var(--blue-200))'
               },
               navy: {
                  100: 'hsl(var(--navy-100))',
                  200: 'hsl(var(--navy-200))',
                  300: 'hsl(var(--navy-300))',
                  400: 'hsl(var(--navy-400))',
                  500: 'hsl(var(--navy-500))',
                  600: 'hsl(var(--navy-600))',
                  700: 'hsl(var(--navy-700))'
               },
               white: {
                  100: 'hsl(var(--white-100))',
                  200: 'hsl(var(--white-200))'
               },
               grey: {
                  100: 'hsl(var(--grey-100))',
                  200: 'hsl(var(--grey-200))'
               }
            }
         },
         borderRadius: {
            lg: 'var(--radius)',
            md: 'calc(var(--radius) - 2px)',
            sm: 'calc(var(--radius) - 4px)'
         },
         fontFamily: {
            sans: [...fontFamily.sans],
            body: ['Nunito', 'sans-serif']
         }
      }
   }
};

export default config;
