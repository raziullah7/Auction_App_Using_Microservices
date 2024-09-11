"use server";

import { auth } from "@/auth";
import { Auction, PagedResult } from "@/types";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { FieldValues } from "react-hook-form";

export async function getData(query: string): Promise<PagedResult<Auction>> {
  return await fetchWrapper.get(`search${query}`);
}

// GET request
export async function updateAuctionTest() {
  const data = {
    mileage: Math.floor(Math.random() * 10000) + 1,
  };

  return await fetchWrapper.put(
    "auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c",
    data
  );
}

// POST request
export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post("auctions", data);
}
