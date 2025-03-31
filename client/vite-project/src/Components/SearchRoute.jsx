import React, { useEffect, useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { initialization, updateCurrentLocation } from "./features/way/waySlice";
import Autocomplete from "./Autocomplete";
import axios from "axios";
import { Button, Box,Typography } from "@mui/material";
import SignOutButton from "./features/user/SignOutButton";

export default function SearchRoute() {

    const dispatch = useDispatch();
    const wayArr = useSelector((state) => state.way.arr)

    const [from, setFrom] = useState({});
    const [to, setTo] = useState({});
    const[error,SetError]=useState("");

    let intervalId;

    //פונקציה לזיהוי מקום
    const detectLocation = async () => {
        try {
            // מנסה להשיג את המיקום דרך ה-GPS של המחשב
            const location = await new Promise((resolve, reject) => {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(
                        (position) => {
                            resolve({
                                lat: position.coords.latitude,
                                lon: position.coords.longitude,
                            });
                        },
                        (error) => {
                            reject({ error: error.message });
                        },
                        {
                            enableHighAccuracy: true,  // מבקש דיוק גבוה יותר
                            timeout: 50000000000000,  // זמן מקסימלי לחכות
                            maximumAge: 0  // לא להשתמש במיקום שנשמר קודם
                        }
                    );
                } else {
                    reject({ error: "Geolocation is not supported by this browser." });
                }
            });
            dispatch(updateCurrentLocation({
                lat: location.lat,
                lon: location.lon,
            }));
            // מחזיר את הקואורדינטות (latitude, longitude)
            return {
                lat: location.lat,
                lon: location.lon,
            };
        } catch (error) {
            console.error(error);
            return { error: error.message };  // אם קרתה שגיאה
        }
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

    //פונקציה למציאת מרחק
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
    useEffect(() => {
        dispatch(updateCurrentLocation({ lat: 31.7767, lon: 35.2345 }))
        return () => clearInterval(intervalId);
    }, [])

    //כל 3 שניות שליחה לפונקצית זיהוי מקום
    //צריך לבדוק אם סטה מהמסלול
    //צריך להתחיל רק אחרי שנלחץ על צא לדרך
    const checkPoint = async () => {
        let local = await detectLocation();
        if (!await isPointInRoute(local)) {//כלומר אם לא חלק מהמסלול
            dispath(initialization(local, wayArr[wayArr.length - 1]))
            // C#שלב שני להוסיף פה גם שליחה ל
        }
    }
    const startRoute = async () => {
        dispatch(await initialization({ startLat: from.lat, startLon: from.lon, endLat: to.lat, endLon: to.lon }));
        // // כאן מתחילים את הבדיקה כל 3 שניות
        // intervalId = setInterval(async () => {
        //     await checkPoint();
        // }, 3000);
    };

    const handleAddressSelect = async (address, type) => {
        let node;
        if (address === "המיקום שלך") {
            node = await detectLocation();
        }
        else {
            //נשלח את הכתובת להמרה לקורדינאטות
            try {
                node = await convertAddressToCoordinates(address);
            } catch (error) {
                 SetError("כתובת לא נמצאה"+{type})
            }
        }
        console.log("כתובת שנבחרה:", address);
        if (type === "source") {
            setFrom({ lat: node.lat, lon: node.lon });

        } else if (type === "target") {
            setTo({ lat: node.lat, lon: node.lon });
        }
    };

    return (
        <>
            <SignOutButton />
            <div className="search-route form-input">

                <Box
                    display="flex"
                    flexDirection="column"
                    padding="20px"
                    justifyContent="center"  // ממרכז את התוכן אנכית
                    alignItems="center"
                    paddingTop="50px"
                >
                    <div className="title">הוראות הליכה</div>
                    <Box
                        display="flex"
                        flexDirection="column"
                        gap="20px"
                        width="350px"
                        justifyContent="center"
                        alignItems="center"
                        padding="20px"
                    >
                        <Autocomplete onSelect={handleAddressSelect} textInput="בחרו נקודת התחלה" type="source" />
                        <Autocomplete onSelect={handleAddressSelect} textInput="בחרו יעד" type="target" />

                    </Box>
                    <Button variant="contained" color="primary" onClick={startRoute}>
                        צא לדרך
                    </Button>
                    {/* שגיאה */}
                    {error && <Typography color="error">{error}</Typography>}

                </Box>

            </div>
        </>
    );
}
