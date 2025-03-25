import React from "react";
import { MapContainer,TileLayer } from "react-leaflet";
import 'leaflet/dist/leaflet.css'

export default function Map(){
    return(

      <MapContainer center={[48.8566,2.3522]} zoom={13}>
      <TileLayer
      attribution='&copy; <a href="https://www.maptiler.com/copyright/">MapTiler</a>'
       //url="https://api.maptiler.com/maps/basic/{z}/{x}/{y}.png?key=YOUR_MAPTILER_KEY"
       url="https://api.maptiler.com/maps/basic-v2//{z}/{x}/{y}.png?key=JIAWwfSxJFtPLibS78a3#-0.2/0.00000/-38.44139"
     />   
      </MapContainer>

    )
}