import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import './i18n';
import App from './App.tsx'
import { BrowserRouter } from 'react-router-dom';
import 'rc-slider/assets/index.css';
import 'rc-tooltip/assets/bootstrap.css';
import 'leaflet/dist/leaflet.css';
import 'react-datepicker/dist/react-datepicker.css';


createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
)
