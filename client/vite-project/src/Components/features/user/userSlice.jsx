import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import axios from 'axios';

const initialState = {
  currentUser: null,
  status: null,
}

//נשמור רק שם משתמש משום שרק זה מה שנציג לו על המסך
//(תיתכן אפשרות לשמור חשבון ואז יופיע הפרופיל)
//1)להתחבר משתמש רשום
//singIn
//2)להתחבר משתמש חדש
//singUp
//3)להתנתק מהחשבון
//כלומר לא יופיע השם משתמש וכן לא ישמור עליו נתונים-השאלה אם לאפשר
//singOut
export const signInServer = createAsyncThunk("user/signIn", async ({ email, password }, thunkApi) => {
  //כאן נבצע קריאה לשרת
  try {
    let { data } = await axios.post("https://localhost:7026/api/user/signIn", { email, password });//יכיל- כלומר מה יחזור מהשרת userצריך להחליט מה ה
    //אם עוד לא רשום להחזיר להרשמה
    return data;
  } catch (error) {
     return thunkApi.rejectWithValue(error.response.data);
  }

})
export const signUpServer = createAsyncThunk("user/signUp", async ({ email, password, userName }, thunkApi) => {
  //כאן נבצע קריאה לשרת
  try {
    let { data } = await axios.post("https://localhost:7026/api/user/signUp", { email, password, userName });
  //אם כבר רשום להחזיר להתחברות
    return data;
  } catch (error) {
    if (!error.response) {
      return thunkApi.rejectWithValue("Server is unreachable");
    }
    if (error.response.status === 404) {
      return thunkApi.rejectWithValue("User not found");
    }
    if (error.response.status === 400) {
      return thunkApi.rejectWithValue("Invalid password");
    }
    return thunkApi.rejectWithValue("Unknown error occurred");
  }
})
export const userSlice = createSlice({
  name: 'user',
  initialState,
  //פשוטים reducers
  reducers: {
    signOut: (state, action) => {
      state.currentUser = null;
      state.status = null;
      localStorage.removeItem("dataToken");
    }
  },
  //מורכבים reducers
  extraReducers: builder => {
    // סטטוסים עבור תהליך התחברות
    builder.addCase(signInServer.fulfilled, (state, action) => {//הצליח
      localStorage.setItem("dataToken", action.payload);
      state.status = "fulfilled";
      try {
        //חילוץ שם המשתמש מהטוקן
        const decodedToken = jwtDecode(action.payload);
        state.currentUser = decodedToken.userName;
      } catch (error) {
        state.currentUser = null; 
      }
    }).addCase(signInServer.rejected, (state, action) => {//נכשל
      //אם הוא לא הצליח להתחבר אני אשלח אותו להרשמה מחדש
      state.status = "failed";
      state.typeError = action.payload.message || 'התחברות נכשלה';  // נשמור שגיאה
    }).addCase(signInServer.pending, (state, action) => {//תוך כדי
      //לפי הסטטוס ניצר סימן טוען עמוד 
      state.status = "pending";
    })
     // סטטוסים עבור תהליך הרשמה
     builder
     .addCase(signUpServer.pending, (state) => {
       state.status = 'pending';  
     })
     .addCase(signUpServer.fulfilled, (state, action) => {
       state.currentUser = action.payload.userName; 
       state.status = 'fulfilled';  
       localStorage.setItem('dataToken', action.payload.token);  // שמירת ה-token ב-localStorage
     })
     .addCase(signUpServer.rejected, (state, action) => {
       state.status = 'failed';  
       state.typeError = action.payload.message || 'ההרשמה נכשלה';  // שמירת השגיאה
     });
  }
})


export const { signOut } = userSlice.actions

export default userSlice.reducer