import { useState } from "react";
import { TextField, MenuItem, Paper, List, ListItem, InputAdornment } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search"; // אייקון של זכוכית מגדלת
import ClearIcon from "@mui/icons-material/Clear"; // אייקון של X

const Autocomplete = ({ onSelect, textInput, type }) => {
    const [query, setQuery] = useState("");//הקלט מהמשתמש
    const [suggestions, setSuggestions] = useState([]);//מערך הצעות הכתובות למשתמש

    //עם הכתובת שהמשתמש מקליד Nominatim API-שליחת בקשה ל
    const fetchSuggestions = async (input) => {
        if (input.length < 2) {
            setSuggestions([]); // לא מחפש לפני 2 תווים
            return;
        }

        //geoapify- בקשה ל
        //מביא 5 כתובות שמכילות את האותיות שכבר הוקשו
        const url = `https://api.geoapify.com/v1/geocode/search?text=${encodeURIComponent(input + '*')}&lang=he&limit=5&result_type=street&apiKey=5cecf88537aa4ad9a537dff0741fa1c2`;
        try {
            const response = await fetch(url);
            const data = await response.json();
            console.log(data);

            if (data && Array.isArray(data.features)) {
                const filteredResults = data.features.map((item) => {
                    const formattedAddress = item.properties.formatted;
                    const addressWithoutPostalCode = formattedAddress//.replace(/\d{5}(?=\s|$)/, "") // מסיר את המיקוד
                    return {
                        display_name: addressWithoutPostalCode || "כתובת לא זמינה",
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
        onSelect(address, type); // שולח את הכתובת לקומפוננטה ההורה
    };
    const handleClear = () => {
        setQuery("");
        setSuggestions([]);
    };

    return (
        <div style={{ position: "relative" }}>
            <TextField
                value={query}
                onChange={handleChange}
                label={textInput}
                variant="outlined"
                fullWidth
                style={{ marginBottom: "10px" }}
                InputLabelProps={{
                    shrink: false, // יסתיר את ה-Label כשיש טקסט
                    sx: {
                        "&.MuiInputLabel-root": {
                            position: "absolute",
                            right: "8px", // מיישר את ה-Label להתחלה של הטקסט
                            top: "50%", // מרכז את ה-Label באמצע ה-Input
                            transform: "translateY(-50%)", // מכווץ את ה-Label למרכז
                            textAlign: "right",
                            color: "#a0a0a0",
                            pointerEvents: "none", // מונע מה-Label להיות לחיץ
                            transition: "opacity 0.2s ease-in-out",
                            opacity: query ? 0 : 1, // מעלים אותו כשהמשתמש מקליד
                        },
                    },
                }}
                sx={{
                    "& .MuiOutlinedInput-root": {
                        backgroundColor: "#f0f0f0", // רקע אפור בהיר
                        borderRadius: "7px", // פינות מעוגלות קלות
                        padding: "0 8px",
                        "& .MuiOutlinedInput-notchedOutline": {
                            border: "none", // מסיר את המסגרת
                        },
                        "&.Mui-focused .MuiInputLabel-root": {
                            display: "none", // הסתרת ה-Label בפוקוס
                        },
                        "&.Mui-focused:after": {
                            content: '""',
                            position: "absolute",
                            left: 0,
                            right: 0,
                            bottom: 0,
                            height: "1.5px", //עובי פס תכלת
                            backgroundColor: "#3a87cb", // צבע התכלת
                        },
                        fontFamily: "'Roboto', sans-serif", // שינוי גופן של תיבת הטקסט
                    },
                }}
                InputProps={{
                    sx: {
                        textAlign: "right", // יישור טקסט לימין
                        "& input": {
                            textAlign: "right",
                        },

                    },
                    startAdornment: (
                        <InputAdornment position="end">
                            {query ? (
                                <ClearIcon
                                    style={{ color: "rgb(74 74 74 / 54%)", cursor: "pointer" }}
                                    onClick={handleClear} // כאשר לוחצים על ה-X, מנקים את הטקסט
                                />
                            ) : (
                                <SearchIcon style={{ color: "rgb(74 74 74 / 54%)" }} />
                            )}
                        </InputAdornment>
                    ),
                }}
            />
            {suggestions.length > 0 && (
                <Paper style={{ position: "absolute", width: "100%", zIndex: 1000 }}>
                    <List>
                        {textInput === "בחרו נקודת התחלה" && (
                            <ListItem
                                button
                                onClick={() => handleSelect("המיקום שלך")}
                                sx={{
                                    textAlign: "right", // יישור טקסט מימין לשמאל
                                    direction: "rtl",
                                }}>
                                המיקום שלך
                            </ListItem>
                        )}
                        {suggestions.map((item, index) => (
                            <ListItem
                                key={index}
                                button
                                onClick={() => handleSelect(item.display_name)}
                                sx={{
                                    textAlign: "right", // יישור טקסט מימין לשמאל
                                    direction: "rtl",
                                }}
                            >
                                <div style={{ fontSize: "1.1rem" }}>
                                    {/* הצגת הרחוב ומספר הבית אם יש */}
                                        <div style={{ fontWeight: "normal" }}>
                                            {item.road&&item.road} {item.house_number && item.house_number}{!item.road&&item.city}
                                        </div>
                                </div>
                                <div style={{ fontSize: "0.7rem", fontWeight: "lighter", marginTop: "5px" }}>
                                        <div>
                                             {(item.road&&item.city)&&item.city} {item.country&&item.country}
                                        </div>
                                </div>
                            </ListItem>
                        ))}
                    </List>
                </Paper>
            )}
        </div>
    );
};

export default Autocomplete;

