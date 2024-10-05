import { Bid } from "@/types";
import { create } from "zustand";

type State = {
  bids: Bid[];
};

type Actions = {
  setBids: (bids: Bid[]) => void;
  addBid: (bid: Bid) => void;
};

export const useBidStore = create<State & Actions>((set) => ({
  // used the property itself instead of ...initialState
  bids: [],

  // set the bids array equal to the passed array (also named bids)
  setBids: (bids: Bid[]) => {
    set(() => ({ bids: bids }));
  },

  addBid: (bid: Bid) => {
    set((state) => ({
      // first check for duplicate, then add the bid
      bids: state.bids.find((x) => x.id === bid.id)
        ? [...state.bids]
        : [bid, ...state.bids],
    }));
  },
}));
