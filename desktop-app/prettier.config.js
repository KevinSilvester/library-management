/** @type {import("prettier").Config} */
export default {
   arrowParens: 'always',
   jsxSingleQuote: true,
   quoteProps: 'as-needed',
   tabWidth: 3,
   useTabs: false,
   singleQuote: true,
   trailingComma: 'none',
   printWidth: 120,
   semi: false,
   importOrderSeparation: true,
   importOrder: ['^[./]'],
   plugins: ['@trivago/prettier-plugin-sort-imports']
}
