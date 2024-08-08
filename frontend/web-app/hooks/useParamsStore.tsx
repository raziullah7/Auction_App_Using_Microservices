import { create } from "zustand";
import { createWithEqualityFn } from "zustand/traditional";

type State = {
  pageNumber: number;
  pageSize: number;
  pageCount: number;
  searchTerm: string;
  searchValue: string;
  orderBy: string;
};

type Action = {
  setParams: (params: Partial<State>) => void;
  reset: () => void;
  setSearchValue: (value: string) => void;
};

const initialState: State = {
  pageNumber: 1,
  pageSize: 12,
  pageCount: 1,
  searchTerm: "",
  searchValue: "",
  orderBy: "make",
};

// creating state store
export const useParamsStore = createWithEqualityFn<State & Action>()((set) => ({
  ...initialState,

  setParams: (newParams: Partial<State>) => {
    // accessing the current state store to check which attribute is being changed
    set((state) => {
      // if its pageNumber, then keeps previous values of all other attributes
      // and update the pageNumber in the state using newParams.pageNumber
      if (newParams.pageNumber) {
        return { ...state, pageNumber: newParams.pageNumber };
      } else {
        return { ...state, ...newParams, pageNumber: 1 };
      }
    });
  },

  reset: () => set(initialState),

  setSearchValue: (value: string) => {
    set({ searchValue: value });
  },
}));
