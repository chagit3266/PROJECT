import React, { useEffect, useState} from "react";
import { useSelector, useDispatch } from "react-redux";
import { initialization } from "./features/way/waySlice";
export default function SearchRoute() {

    const dispath = useDispatch();
    const wayArr = useSelector((state) => state.way.arr)

    const [from, setFrom] = useState("");
    const [to, setTo] = useState("");
    const [fromSuggestions, setFromSuggestions] = useState([]);
    const [toSuggestions, setToSuggestions] = useState([]);
    const [isLocationSelected, setIsLocationSelected] = useState(false);

    //פונקציה לזיהוי מקום
    const detectLocation = async () => {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject("הדפדפן לא תומך בזיהוי מיקום");
            }
            navigator.geolocation.getCurrentPosition(
                (position) => resolve({ lat: position.coords.latitude, lon: position.coords.longitude }),
                (error) => reject(error.message),
                { enableHighAccuracy: true }
            );
        });
    };

    //פונקציה להמרה מכתובת מילולית לקורדינאטות
    const convertAddressToCoordinates = async (address) => {
        const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(address)}&limit=1`;
        let { data } = await axios.get(url);
        if (data.length === 0) throw new Error("כתובת לא נמצאה");
        return { lat: data[0].lat, lon: data[0].lon };
    };
    //בדיקה האם הנקודה הנוכחית היא בתוך המסלול
    //ולמחוק את הנקודות שכבר עברתי
    async function isPointInRoute(local) {
        //arrבדיקה האם הנוכחי הוא בין הראשון לשני ב
        return true;
    }

    function CalculateDistance(startLat, startLon, endLat, endLon) {
        let d = 0;
        //d = 2R * arcsin( sqrt( sin²(Δφ/2) + cos(φ1) * cos(φ2) * sin²(Δλ/2) ) ) 
        const R = 6371e3;//רדיוס במטרים
        let φ1 = startLat * Math.PI / 180;//...tan,cos,sin 'המרה ממעלות לרדיאנים ע"מ שנוכל להשתמש בפונק
        let φ2 = endLat * Math.PI / 180;
        let Δφ = (endLat - startLat) * Math.PI / 180;
        let Δλ = (endLon - startLon) * Math.PI / 180;
        d = 2 * R *
            Math.asin(
                Math.sqrt(
                    Math.pow(Math.sin(Δφ / 2), 2) +
                    (Math.cos(φ1) *
                        Math.cos(φ2) *
                        Math.pow(Math.sin(Δλ / 2), 2))
                )
            );
        return d;
    }



    //כל 3 שניות שליחה לפונקצית זיהוי מקום
    //צריך לבדוק אם סטה מהמסלול
    useEffect(() => {
        const checkPoint = async () => {
            let local = detectLocation();
            if (!await isPointInRoute(local)) {//כלומר אם לא חלק מהמסלול
                dispath(initialization(local, wayArr[length - 1]))
                // C#שלב שני להוסיף פה גם שליחה ל
            }
        }
        setTimeout(() => {
            checkPoint();
        }, 3000);
    }, [])



    return (
        
        <div className="search-route">
            {/* 🔹 שדה נקודת מוצא */}
            <div className="input-container">
                <input
                    type="text"
                    placeholder="נקודת מוצא"
                    value={from}
                    onChange={(e) => {
                        setFrom(e.target.value);
                        fetchAddressSuggestions(e.target.value, setFromSuggestions);
                    }}
                />
                {/* 🔹 זיהוי מקום */}
                {!isLocationSelected && (
                    <button onClick={() => detectLocation(setFrom)}>📍 זיהוי מיקום</button>
                )}
                {/* 🔹 הצעות אוטומטיות */}
                {fromSuggestions.length > 0 && (
                    <ul className="suggestions">
                        {fromSuggestions.map((suggestion, index) => (
                            <li key={index} onClick={() => setFrom(suggestion)}>
                                {suggestion}
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            {/* 🔹 שדה יעד */}
            <div className="input-container">
                <input
                    type="text"
                    placeholder="יעד"
                    value={to}
                    onChange={(e) => {
                        setTo(e.target.value);
                        fetchAddressSuggestions(e.target.value, setToSuggestions);
                    }}
                />
                {/* 🔹 הצעות אוטומטיות */}
                {toSuggestions.length > 0 && (
                    <ul className="suggestions">
                        {toSuggestions.map((suggestion, index) => (
                            <li key={index} onClick={() => setTo(suggestion)}>
                                {suggestion}
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            <button onClick={() => dispath(initialization({ from, to }))}>
                חפש מסלול
            </button>
        </div>
    )
}
// const [selectedAddress, setSelectedAddress] = useState("");

//   const handleAddressSelect = (address) => {
//     debugger
//     setSelectedAddress(address);
//     console.log("כתובת שנבחרה:", address);
//   };

//   return (
//     <div style={{ padding: "20px", fontFamily: "Arial, sans-serif" }}>
//       <h2>בחירת כתובת</h2>
//       <Autocomplete onSelect={handleAddressSelect} />
//       {selectedAddress && (
//         <p>
//           <strong>כתובת שנבחרה:</strong> {selectedAddress}
//         </p>
//       )}
//     </div>
//   );