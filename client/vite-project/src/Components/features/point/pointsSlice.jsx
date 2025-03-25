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
    arr:[]
}
export const /*שליחת נקודת התחלה וסיום*/ a =createAsyncThunk("",async(point1,point2,thunkApi)=>{
  let {data}=await axios.post("https://localhost:7026/way",point1,point2);
  return data;
})

export const pointsSlice = createSlice({
  name: 'points',
  initialState,
  reducers: {
    //אתחול
    initialization: (state,action) => {
      //קריאת שרת לקבל את מערך המסלול
      //יהיה נקודת התחלה ונקודת סיום actionב
      
    },
    
  },
  //מורכבים reducers
  extraReducers:builder=>{
    builder.addCase(a.fulfilled,(state,action)=>{
        
    })
  }
})


export const { initialization} = pointsSlice.actions

export default pointsSlice.reducer