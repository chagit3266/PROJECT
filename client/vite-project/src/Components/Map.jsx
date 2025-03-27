import React, { useEffect } from "react";
import { MapContainer, TileLayer, Marker, Popup, Polyline } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import { useSelector } from "react-redux";


const Map = () => {
  const way = useSelector((state) => state.way)
  const wayArr = way.arr;//store- לוקח את המערך נקודות מה

  const customRedMarker = new L.DivIcon({
    className: 'custom-red-marker',
    html: `
      <div style="width: 30px; height: 30px; background-color: red; border-radius: 50%; position: relative;">
        <div style="width: 15px; height: 15px; background-color: darkred; border-radius: 50%; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%);"></div>
        <!-- Tip added here -->
        <div style="width: 0; height: 0; border-left: 7px solid transparent; border-right: 7px solid transparent; border-top: 10px solid red; position: absolute; bottom: -9px; left: 50%; transform: translateX(-50%);"></div>
      </div>
    `,
    iconSize: [30, 40],  // Adjusted size for proper alignment
    iconAnchor: [15, 40], // Anchor adjusted for proper center alignment with the tip
  });
  const customWazeMarker = new L.DivIcon({
    className: 'custom-waze-marker',
    html: `
      <div style="width: 50px; height: 50px; background-color: rgba(0, 187, 212, 0.49); border-radius: 50%; position: relative; 
                  box-shadow: 0 0 20px rgba(0, 188, 212, 0.8);">
        <!-- Smaller inner circle -->
        <div style="width: 13px; height: 13px; background-color: #00bcd4; border-radius: 50%; 
                    border: 3px solid white; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%);"></div>
      </div>
    `,
    iconSize: [50, 50],  // Size for the halo
    iconAnchor: [25, 25], // Anchor the icon in the center
  });
  if (!way.currentLocation || !way.currentLocation.lat || !way.currentLocation.lon) {
    return <div>Loading...</div>; // אם המיקום לא זמין, תצוגה בזמן טעינה
  }
  return (
    <div className="map">
      <MapContainer center={[way.currentLocation.lat , way.currentLocation.lon ]} zoom={25} className="h-[80vh] w-full rounded-lg" style={{ zIndex: 1 }}>
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {/* שרטוט מסלול לפי רשימת נקודות*/}
        <Polyline positions={wayArr} color="blue" weight={5} />
        {
          wayArr.length > 0 && (
            <Marker position={[wayArr[wayArr.length - 1].lat, wayArr[wayArr.length - 1].lon]} icon={customRedMarker}>
              <Popup>!הגעת ליעד</Popup>
            </Marker>
          )}
        {
          way.currentLocation && (
            <Marker position={[way.currentLocation.lat, way.currentLocation.lon]} icon={customWazeMarker}>
              <Popup>!את/ה כאן</Popup>
            </Marker>)}
      </MapContainer>
    </div>
  );
};

export default Map;
