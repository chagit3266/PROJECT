import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit'
import axios from 'axios'
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
  arr: [],
  currentLocation: {},
  status: null
}
// נקראית בתחילה וכן אם המשתמש סטה מן המסלול
// שליחת נקודת התחלה וסיום
export const initialization = createAsyncThunk("way/initialization", async ({ startLat, startLon, endLat, endLon }, thunkApi) => {
  debugger
  try {
    console.log('URL:', `https://localhost:7026/api/Way/initialization?startLat=${startLat}&startLon=${startLon}&endLat=${endLat}&endLon=${endLon}`);
    const { data, status } = await axios.get("https://localhost:7026/api/Way/initialization", {
      params: { startLat, startLon, endLat, endLon }
    });
    // הדפס את הסטטוס קוד של התשובה
    console.log('Response status:', status);
    console.log('Response data:', data);

    // אם הסטטוס קוד לא בטווח של הצלחה (200-299), נזרוק שגיאה
    if (status < 200 || status >= 300) {
      throw new Error(`Unexpected status code: ${status}`);
    }

    return data;
  } catch (error) {
    console.error('Error fetching initialization:', error);

    // במידה ויש שגיאה, נעשה reject עם פרטי השגיאה
    return thunkApi.rejectWithValue(error.response ? error.response.data : error.message);
  }
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
    updateCurrentLocation: (state, action) => {
      state.currentLocation = action.payload
    }
  },
  //מורכבים reducers
  extraReducers: builder => {
    builder.addCase(initialization.fulfilled, (state, action) => {
      state.arr = action.payload;//חוזר מערך נקודות -שהן תהוונה את המסלול
      state.currentLocation=state.arr[0];
    }).addCase(endWalk.fulfilled, (state, action) => {
      state.status = "endWalk";
    })
  }
})


export const { updateCurrentLocation } = waySlice.actions

export default waySlice.reducer