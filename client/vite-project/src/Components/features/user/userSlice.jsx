import { createSlice ,createAsyncThunk} from '@reduxjs/toolkit'

const initialState = {
   userName:"",
}
//נשמור רק שם משתמש משום שרק זה מה שנציג לו על המסך
//(תיתכן אפשרות לשמור חשבון ואז יופיע הפרופיל)
//1)להתחבר משתמש רשום

//2)להתחבר משתמש חדש

//3)להתנתק מהחשבון
//כלומר לא יופיע השם משתמש וכן לא ישמור עליו נתונים-השאלה אם לאפשר

export const loginServer=createAsyncThunk("user-login",async(user,thunkApi)=>{
  //כאן נבצע קריאה לשרת
  let {data}=await axios.post("https://localhost:7026/user/login",user);
  return data
})

export const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    login:(state,action)=>{
         
    },
    logout:(state,action)=>{
         
    }
  },
})


export const {} = userSlice.actions

export default userSlice.reducer