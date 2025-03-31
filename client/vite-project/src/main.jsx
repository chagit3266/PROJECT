import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { store } from './Components/app/store'
import { Provider } from 'react-redux'
import { GoogleOAuthProvider } from '@react-oauth/google';
import { BrowserRouter } from 'react-router-dom';

const clientId = '452117313253-48cj5912693v9jrv5gtr4j1rvm0c4l6s.apps.googleusercontent.com';

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
