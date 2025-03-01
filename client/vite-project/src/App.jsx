import { useState } from 'react'
import './App.css'
import Map from './Components/Map'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
     <Map></Map> 
    </>
  )
}

export default App
