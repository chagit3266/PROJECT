import { configureStore } from '@reduxjs/toolkit'
import userSlice from '../features/user/userSlice'
import waySlice from '../features/way/waySlice'


export const store = configureStore({
  reducer: {
    user:userSlice,
    way:waySlice
  },
})