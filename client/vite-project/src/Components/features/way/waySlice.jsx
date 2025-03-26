import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

//יהיה  מערך שיכיל את הנקודות שבהן יעבור המשתמש
//יתכנו שינויים במהלך הדרך כאשר המשתמש יסתה מהנתיב שהוכן לו
//בכל שלב המערך יכיל את הנקודות מהנקודה הנוכית ועד נקודת היעד
//:הפעולות שיהיו
//אתחול (1 
//שליחה לשרת לקבלת מערך נקודות, יקרא גם כאשר המשתמש סתה ומחשבים לו מסלול מחדש
//2) קידום סמן
//לכאורה רק אם סתה מהמסלול C# אחרי 3 שניות מעבר הסמן וכן שליחה ל
//3) הגעת ליעד
//"כאשר הגענו לנקודה האחרונה -שינוי סמן והשמעת "הגעת ליעד

const initialState = {
    arr:[],
    status:null
}
// נקראית בתחילה וכן אם המשתמש סטה מן המסלול
// שליחת נקודת התחלה וסיום
export const initialization =createAsyncThunk("way/initialization",async({ start, end },thunkApi)=>{
  let {data}=await axios.post("https://localhost:7026/api/way/initialization",{ start, end });
  return data;
})

// סיום הניווט 
// כאשר המשתמש סוגר את האתר
// או כאשר הוא מגיע ליעד
export const endWalk = createAsyncThunk("way/end", async (_, thunkApi) => {
  await axios.post("https://localhost:7026/api/way/end");
  return null;
});

export const waySlice = createSlice({
  name: 'points',
  initialState,
  reducers: {
    
  },
  //מורכבים reducers
  extraReducers:builder=>{
    builder.addCase(initialization.fulfilled,(state,action)=>{
        state.arr=action.payload;//חוזר מערך נקודות -שהן תהוונה את המסלול
    }).addCase(endWalk.fulfilled,(state,action)=>{
        state.status="endWalk";
    })
  }
})


export const {} = waySlice.actions

export default waySlice.reducer