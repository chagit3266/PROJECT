import React from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { signOut } from './userSlice'; // התנתקות

export default function SignOutButton(){
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = async() => {

    await dispatch(signOut());
    // ניווט לעמוד התחברות-הרשמה אחרי ההתנתקות
    navigate("/auth");
  };

  return (
    <button className='out'
      onClick={handleLogout}
    >
      התנתק
    </button>
  );
};
