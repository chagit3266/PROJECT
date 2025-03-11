import { createSlice } from '@reduxjs/toolkit'

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
})


export const { initialization,} = pointsSlice.actions

export default pointsSlice.reducer