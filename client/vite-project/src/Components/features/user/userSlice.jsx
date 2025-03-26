import { createSlice ,createAsyncThunk} from '@reduxjs/toolkit'

const initialState = {
  currentUser:null,
  status:null
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
export const signInServer=createAsyncThunk("user/signIn",async(user,thunkApi)=>{
  //כאן נבצע קריאה לשרת
  let {data}=await axios.post("https://localhost:7026/api/user/signIn",user);//יכיל- כלומר מה יחזור מהשרת userצריך להחליט מה ה
  return data;
})
export const signUpServer=createAsyncThunk("user/signUp",async(user,thunkApi)=>{
  //כאן נבצע קריאה לשרת
  let {data}=await axios.post("https://localhost:7026/api/user/signUp",user);
  return data;
})
export const userSlice = createSlice({
  name: 'user',
  initialState,
  //פשוטים reducers
  reducers: {
    signOut:(state,action)=>{
      state.currentUser = null;
      state.status = null;
      localStorage.removeItem("dataToken");
    }
  },
  //מורכבים reducers
  extraReducers:builder=>{
    builder.addCase(signInServer.fulfilled,(state,action)=>{//הצליח
      localStorage.setItem("dataToken",action.payload);
      state.status="fulfilled";
    }).addCase(signInServer.rejected,(state,action)=>{//נכשל
      //אם הוא לא הצליח להתחבר אני אשלח אותו להרשמה מחדש
      state.status="failed";
    }).addCase(signInServer.pending,(state,action)=>{//תוך כדי
      //לפי הסטטוס ניצר סימן טוען עמוד 
      state.status="pending";
    })
  }
})


export const {} = userSlice.actions

export default userSlice.reducer