import { useState } from "react";

const Autocomplete = ({ onSelect, textInput }) => {
    const [query, setQuery] = useState("");//הקלט מהמשתמש
    const [suggestions, setSuggestions] = useState([]);//מערך הצעות הכתובות למשתמש

    //עם הכתובת שהמשתמש מקליד Nominatim API-שליחת בקשה ל
    const fetchSuggestions = async (input) => {
        if (input.length < 0) {
            setSuggestions([]); // לא מחפש לפני 3 תווים
            return;
        }

        //OpenStreetMap- בקשה ל
        //const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(input)}&addressdetails=1&countrycodes=IL&limit=5`;//
        //const url = `https://api.geoapify.com/v1/geocode/search?text=${encodeURIComponent(input)}&lang=he&limit=5&apiKey=5cecf88537aa4ad9a537dff0741fa1c2`;
        const url = `https://api.geoapify.com/v1/geocode/search?text=${encodeURIComponent(input)}&lang=he&limit=5&result_type=street&apiKey=5cecf88537aa4ad9a537dff0741fa1c2`;
        try {
            const response = await fetch(url);
            const data = await response.json();
            console.log(data);

            if (data && Array.isArray(data.features)) {
                const filteredResults = data.features.map((item) => {
                    return {
                        display_name: item.properties.formatted || "כתובת לא זמינה",
                        road: item.properties.street || "",
                        house_number: item.properties.housenumber || "",
                        city: item.properties.city || "",
                        country: item.properties.country || "",
                    };
                })
                //state-לוקח רק עד 5 אפשרויות - ואז מכניס למערך ה
                setSuggestions(filteredResults); // הצגת התוצאות
            }
        } catch (error) {
            console.error("Error fetching autocomplete data:", error);
            if (error.response) {
                console.error("Response error:", error.response);
            } else if (error.request) {
                console.error("Request error:", error.request);
            } else {
                console.error("General error:", error.message);
            }
        }
    };

    // עדכון טקסט חיפוש
    const handleChange = (e) => {
        const input = e.target.value;
        setQuery(input);
        fetchSuggestions(input);
    };

    // בחירת כתובת מהתוצאות
    const handleSelect = (address) => {
        setQuery(address);
        setSuggestions([]);
        onSelect(address); // שולח את הכתובת לקומפוננטה ההורה
    };

    return (
        <div className="input" style={{ position: "relative", width: "300px" }}>
            <input
                type="text"
                value={query}
                onChange={handleChange}//כתובות שמתחליות באותיות אלו API ושולחת לחיפוש ב input נשלח לפונקציה שמעדכנת 
                placeholder={textInput}
                style={{ width: "100%", padding: "8px", fontSize: "16px" }}
            />
            {suggestions.length > 0 && (
                <ul
                    style={{
                        position: "absolute",
                        top: "100%",
                        left: "0",
                        width: "100%",
                        background: "white",
                        border: "1px solid #ccc",
                        listStyleType: "none",
                        padding: "0",
                        margin: "0",
                        zIndex: "1000",
                    }}
                >
                    {textInput === "בחרו נקודת התחלה" && <li onClick={()=>onSelect("זיהוי מקום")}style={{
                                padding: "10px",
                                cursor: "pointer",
                                borderBottom: "1px solid #eee",
                            }}>המיקום שלך</li>}
                    {suggestions.map((item, index) => (
                        <li
                            key={index}
                            onClick={() => handleSelect(item.display_name)}//כאשר בוחרים כתובת מהקשימה אז נשלח לאב מה נבחר
                            style={{
                                padding: "10px",
                                cursor: "pointer",
                                borderBottom: "1px solid #eee",
                            }}
                        >
                            {item.display_name}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default Autocomplete;

