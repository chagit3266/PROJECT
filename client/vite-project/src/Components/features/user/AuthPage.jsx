import React, { useState } from "react";
import { Box, Button, TextField, Typography, Container, Grid } from "@mui/material";
import { GoogleLogin } from "@react-oauth/google"; //Google התחברות עם 
import { useDispatch } from "react-redux";
import { signInServer, signUpServer } from './userSlice'
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";

export default function AuthPage() {
  const [isLogin, setIsLogin] = useState(true); // האם זה בהתחברות או בהרשמה
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [userName, setUserName] = useState("");
  const [error, setError] = useState("");

  const { status, currentUser } = useSelector(state => state.user);

  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    debugger
    if (isLogin) {
      try {
        const response = await dispatch(signInServer({ email, password })).unwrap();
        if (response === 'OK')
          navigate('/route-map')
        else if (response === 'User not found')
          setIsLogin(false);
        else if (response === 'Invalid password')
          setError("סיסמה שגויה נסה שנית");
      } catch (error) {
        setIsLogin(!isLogin)
      }
    }
    else {
      try {
        const response1 = await dispatch(signUpServer({ email, password, userName })).unwrap();
        if (response1 === 'OK') {
          try {
            const response2 = await dispatch(signInServer({ email, password })).unwrap();
            if (response2 === 'OK')
              navigate('/route-map')
            else if (response2 === 'User not found')
            {
              setIsLogin(false);
              setError('משתמש לא רשום')
            }
            else if (response2 === 'Invalid password')
              setError("סיסמה שגויה נסה שנית");
          } catch (error) {
            setIsLogin(!isLogin)
          }
        }
        else if (response1 === 'User already exists') {
          setIsLogin(!isLogin)
          setError("משתמש רשום")
        }

      } catch (error) {
        setIsLogin(!isLogin)
      }
    }
    setError(""); // לא היה שגיאה
    console.log("מייל:", email, "סיסמה:", password);
  };

  return (
    <div className="form-input">
      <Box
        display="flex"
        flexDirection="column"
        padding="20px"
        justifyContent="center"  // ממרכז את התוכן אנכית
        alignItems="center"
        paddingTop="50px"
        maxWidth="640px"
      >
        <div className="title">
          {isLogin ? "התחברות" : "הרשמה"}
        </div>

        <form onSubmit={handleSubmit} style={{ width: "100%" }}>

          {/* אם אנחנו בהרשמה, נוסיף גם את שם משתמש */}
          {!isLogin && (
            <TextField
              label="שם משתמש"
              type="text"
              fullWidth
              required
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              style={{ marginBottom: "10px", padding: "10px 0 " }}
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
                    opacity: userName ? 0 : 1, // מעלים אותו כשהמשתמש מקליד
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
              }}
            />
          )}

          {/* מייל */}
          <TextField
            label="מייל"
            type="email"
            fullWidth
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={{ marginBottom: "10px", padding: "10px 0 " }}
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
                  opacity: email ? 0 : 1, // מעלים אותו כשהמשתמש מקליד
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
            }}
          />

          {/* סיסמה */}
          <TextField
            label="סיסמה"
            type="password"
            fullWidth
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            style={{ marginBottom: "10px", padding: "10px 0 " }}
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
                  opacity: password ? 0 : 1, // מעלים אותו כשהמשתמש מקליד
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

            }}
          />



          {/* שגיאה */}
          {error && <Typography color="error">{error}</Typography>}

          {/* כפתור לשליחה */}
          <Button type="submit" variant="contained" color="primary" fullWidth>
            {isLogin ? "התחבר" : "הרשם"}
          </Button>
        </form>

        {/* כפתור להחלפה בין התחברות להרשמה */}
        <Button
          sx={{
            marginTop: "20px",
            '&:focus': {
              outline: 'none', // מניעת המסגרת כאשר הכפתור מקבל פוקוס
            },
          }}
          onClick={() => {
            setIsLogin(!isLogin);
            setEmail("");
            setPassword("");
            setError("");
          }}
        >
          {isLogin ? "אין לך חשבון? הירשם עכשיו" : "יש לך חשבון? התחבר"}
        </Button>

        {/* כפתור התחברות עם Google */}
        {/* <Grid container spacing={2} sx={{ marginTop: "20px" }}>
          <Grid>
            <GoogleLogin
              onSuccess={(response) => {
                console.log("Google login success:", response);
                const googleToken = response.credential;
                //לשרת ולהתחיל התחברות token -כאן ניתן לשלוח את ה

              }}
              onError={(error) => {
                console.error("Google login error:", error);
              }}
            />
          </Grid>
        </Grid> */}
      </Box>
    </div>
  );
}