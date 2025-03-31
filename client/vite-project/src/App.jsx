import './App.css'
import RouteMap from './Components/RouteMap'
import HomePage from './Components/HomePage';
import AuthPage from './Components/features/user/AuthPage';
import { Route, Routes, useNavigate } from 'react-router-dom';
import { useEffect } from 'react';


function App() {
  const navigate=useNavigate();
  useEffect(() => {
    // מחכה 2 שניות על דף הבית לפני הניתוב
    setTimeout(() => {
      // בודק אם יש טוקן
      const token = localStorage.getItem('dataToken'); // או כל מקום שבו אתה שומר את הטוקן
      if (token) {
        navigate('/route-map'); // אם יש טוקן, עובר ל-RouteMap
      } else {
        navigate('/auth'); // אם אין טוקן, עובר ל-AuthPage
      }
    }, 2000);
  }, [navigate]);
  return (
    <>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/auth" element={<AuthPage />} />
        <Route path="/route-map" element={<RouteMap />} />
      </Routes>
    </>
  )
}

export default App
