import App from './App.tsx'
import './index.scss'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'

createRoot(document.getElementById('root')!).render(
   <StrictMode>
      <App />
   </StrictMode>
)
