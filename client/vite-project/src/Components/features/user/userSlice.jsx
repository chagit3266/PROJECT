import { createSlice ,createAsyncThunk} from '@reduxjs/toolkit'

const initialState = {
  currentUser:null,
  status:null
}
//נשמור רק שם משתמש משום שרק זה מה שנציג לו על המסך
//(תיתכן אפשרות לשמור חשבון ואז יופיע הפרופיל)
//1)להתחבר משתמש רשום

//2)להתחבר משתמש חדש

//3)להתנתק מהחשבון
//כלומר לא יופיע השם משתמש וכן לא ישמור עליו נתונים-השאלה אם לאפשר

export const loginServer=createAsyncThunk("user-login",async(user,thunkApi)=>{
  //כאן נבצע קריאה לשרת
  let {data}=await axios.post("https://localhost:7026/api/user/signIn",user);//יכיל- כלומר מה יחזור מהשרת userצריך להחליט מה ה
  return data;
})

export const userSlice = createSlice({
  name: 'user',
  initialState,
  //פשוטים reducers
  reducers: {
    login:(state,action)=>{
         
    },
    logout:(state,action)=>{
         
    }
  },
  //מורכבים reducers
  extraReducers:builder=>{
    builder.addCase(loginServer.fulfilled,(state,action)=>{//הצליח
      localStorage("dataToken",action.payload);
      state.status=fulfilled;
    }).addCase(loginServer.rejected,(state,action)=>{//נכשל
      //אם הוא לא הצליח להתחבר אני אשלח אותו להרשמה מחדש
      state.status="failed";
    }).addCase(loginServer.pending,(state,action)=>{//תוך כדי
      //לפי הסטטוס ניצר סימן טוען עמוד 
      state.status="pending";
    })
  }
})


export const {} = userSlice.actions

export default userSlice.reducer