import { create } from "zustand";

type State = {
  pageNumber: number;
  pageSize: number;
  pageCount: number;
  searchTerm: string;
};

type Action = {
  setParams: (params: Partial<State>) => void;
  reset: () => void;
};

const initialState: State = {
  pageNumber: 1,
  pageSize: 12,
  pageCount: 1,
  searchTerm: "",
};

// creating state store
export const useParamsStore = create<State & Action>()((set) => ({
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
}));
