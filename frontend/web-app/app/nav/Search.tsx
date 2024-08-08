"use client";

import { useParamsStore } from "@/hooks/useParamsStore";
import React, { useState } from "react";
import { FaSearch } from "react-icons/fa";

export default function Search() {
  const setParams = useParamsStore((state) => state.setParams);
  const searchValue = useParamsStore((state) => state.searchValue);
  const setSearchValue = useParamsStore((state) => state.setSearchValue);

  function onTheChange(event: any) {
    setSearchValue(event.target.value);
  }

  function search() {
    setParams({ searchTerm: searchValue });
  }

  return (
    <div className="flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm">
      <input
        onKeyDown={(e: any) => {
          if (e.key === "Enter") search();
        }}
        value={searchValue}
        onChange={onTheChange}
        type="text"
        placeholder="Search for cars by make, model or color"
        className="
        flex-grow pl-5 bg-transparent border-transparent focus:outline-none focus:border-transparent focus:ring-0 text-sm text-gray-600"
      />
      <button onClick={search}>
        <FaSearch
          size={34}
          className="bg-red-400 text-white rounded-full cursor-pointer mx-2 p-2"
        />
      </button>
    </div>
  );
}
