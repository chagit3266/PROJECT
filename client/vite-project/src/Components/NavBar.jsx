const NavBar = () => {
    return (
      <div className="fixed top-0 w-full bg-black bg-opacity-80 text-white flex justify-between px-4 py-2">
        <button>🔍 חיפוש</button>
        <h1 className="text-lg">🛣️ ניווט חכם</h1>
        <button>⚙️ הגדרות</button>
      </div>
    );
  };
  
  export default NavBar;