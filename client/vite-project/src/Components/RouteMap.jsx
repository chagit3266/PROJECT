import React from "react";
import Map from "./Map";
import SearchRoute from "./SearchRoute";
export default function RouteMap() {
    return (
    <div className="app-container">
        <Map /> {/* רכיב המפה */}
        <SearchRoute />{/*רכיב חיפוש מסלול*/}
    </div>
    )
}