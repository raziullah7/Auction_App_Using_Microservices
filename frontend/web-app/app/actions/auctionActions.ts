"use server";

import { Auction, Bid, PagedResult } from "@/types";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { FieldValues } from "react-hook-form";
import { revalidatePath } from "next/cache";

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

// GET request by id
export async function getDetailedViewData(id: string): Promise<Auction> {
  return await fetchWrapper.get(`auctions/${id}`);
}

// PUT request by id
export async function updateAuction(id: string, data: FieldValues) {
  const res = await fetchWrapper.put(`auctions/${id}`, data);
  revalidatePath(id);
  return res;
}

// DEL request by id
export async function deleteAuction(id: string) {
  const res = await fetchWrapper.del(`auctions/${id}`);
  revalidatePath(id);
  return res;
}

// GET bids for a particular auction
export async function getBidsForAuction(id: string): Promise<Bid[]> {
  return await fetchWrapper.get(`bids/${id}`);
}

// POST request to place a bid
export async function placeBidForAuction(auctionId: string, amount: number) {
  return await fetchWrapper.post(
    `bids?auctionId=${auctionId}&amount=${amount}`,
    {}
  );
}
