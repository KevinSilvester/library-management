import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'

import App from './App'
import './index.scss'

declare global {
   interface Window {
      _cookie: string | null
   }
}

window._cookie = null

createRoot(document.getElementById('root')!).render(
   <StrictMode>
      <App />
   </StrictMode>
)
