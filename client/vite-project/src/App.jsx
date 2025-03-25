import { useState } from 'react'
import './App.css'
import Map from './Components/Map'
import Login from './Components/features/user/Login' 

function App() {

  return (
    <>
     <div className="app-container">
      <Map />  {/* רכיב המפה */}
      {/*<Login />  רכיב ה-Login */}
    </div> 
    </>
  )
}

export default App
