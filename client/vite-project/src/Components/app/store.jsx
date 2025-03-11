import { configureStore } from '@reduxjs/toolkit'
import userSlice from '../features/user/userSlice'
import pointsSlice from '../features/points/pointsSlice'


export const store = configureStore({
  reducer: {
    user:userSlice,
    points:pointsSlice
  },
})