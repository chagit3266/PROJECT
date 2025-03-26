import React, { useState } from "react";
import './Login.css';

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    // כאן אפשר להוסיף לוגיקה לבדוק את פרטי המשתמש בשרת
    if (email === "" || password === "") {
      setErrorMessage("אנא מלא את כל השדות");
    } else {
      setErrorMessage("");
      console.log("Login attempt with", email, password);
      //של התחברות API-יכול להיות כאן קריאה ל 
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>התחברות</h2>
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <label htmlFor="email">אימייל</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="הכנס אימייל"
            />
          </div>

          <div className="input-group">
            <label htmlFor="password">סיסמה</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="הכנס סיסמה"
            />
          </div>

          {errorMessage && <div className="error-message">{errorMessage}</div>}

          <button type="submit" className="login-button">התחבר</button>
        </form>
      </div>
    </div>
  );
};

export default Login;