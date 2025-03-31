import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { store } from './Components/app/store'
import { Provider } from 'react-redux'
import { GoogleOAuthProvider } from '@react-oauth/google';
import { BrowserRouter } from 'react-router-dom';

const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <Provider store={store}>
      {/* <GoogleOAuthProvider clientId={clientId}> */}
        <BrowserRouter>
          <App />
        </BrowserRouter>
      {/* </GoogleOAuthProvider> */}
    </Provider>
  </StrictMode>
)
