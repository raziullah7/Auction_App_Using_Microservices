// "use client"
import React from "react";
import Search from "./Search";
import Logo from "./Logo";

export default function Navbar() {
  // console.log("Navbar.tsx")
  return (
    <header className="sticky top-0 z-50 flex justify-between text-gray-800 bg-white p-5 items-center shadow-md">
      <Logo />
      <Search />
      <div>Login</div>
    </header>
  );
}
