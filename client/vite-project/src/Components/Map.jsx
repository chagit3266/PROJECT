import React from "react";
import { MapContainer,TileLayer } from "react-leaflet";
import 'leaflet/dist/leaflet.css'

export default function Map(){
    return(
    <div className="map">
      <MapContainer center={[48.8566,2.3522]} zoom={13}>
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
      </MapContainer>
    </div>
    )
}