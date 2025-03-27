import { useState } from 'react'
import './App.css'
import Map from './Components/Map'
import Login from './Components/features/user/Login' 
import SearchRoute from './Components/SearchRoute'
import Autocomplete from './Components/Autocomplete'

function App() {
 
  return (
    <>
     <div className="app-container">
       <Map /> {/* רכיב המפה */}
       <SearchRoute/>{/*רכיב חיפוש מסלול*/}
     </div> 
     <Login /> {/* רכיב ה-Login */}
    
     </>
  )
}

export default App
