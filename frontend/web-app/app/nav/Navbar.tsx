// "use client"
import React from 'react'
import { IoCarOutline } from 'react-icons/io5'

export default function Navbar() {
    // console.log("Navbar.tsx")
    return (
        <header className="sticky top-0 z-50 flex justify-between text-gray-800 bg-white p-5 items-center shadow-md">
            <div className="flex items-center gap-2 text-3xl font-semibold text-red-500">
                <IoCarOutline size={34}/>
                <div>Carsties Auctions</div>
            </div>
            <div>Search</div>
            <div>Login</div>
        </header>
    );
}
