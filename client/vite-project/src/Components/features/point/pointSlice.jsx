import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    x: 0,
    y: 0,
}

export const pointSlice = createSlice({
  name: 'point',
  initialState,
  reducers: {
    initialization: (state,action) => {
      state.x = action.payload.x,
      state.y = action.payload.y
    },
    decrement: (state) => {
      state.value -= 1
    },
    incrementByAmount: (state, action) => {
      state.value += action.payload
    },
  },
})


export const { initialization, decrement, incrementByAmount } = pointSlice.actions

export default pointSlice.reducer