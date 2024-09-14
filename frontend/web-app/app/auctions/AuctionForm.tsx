"use client";

import { Button, TextInput } from "flowbite-react";
import React, { useEffect } from "react";
import { FieldValues, useForm } from "react-hook-form";
import Input from "../components/Input";
import DateInput from "../components/DateInput";
import { createAuction, updateAuction } from "../actions/auctionActions";
import { usePathname, useRouter } from "next/navigation";
import toast from "react-hot-toast";
import { Auction } from "@/types";

type Props = {
  auction?: Auction;
};

export default function AuctionForm({ auction }: Props) {
  // useRouter hook
  const router = useRouter();
  const pathname = usePathname();

  // useForm hook
  const {
    control,
    handleSubmit,
    setFocus,
    reset,
    formState: { isSubmitting, isValid },
  } = useForm({
    mode: "onTouched",
  });

  // iseEffect hook
  useEffect(() => {
    if (auction) {
      const { make, model, color, mileage, year } = auction;
      reset({ make, model, color, mileage, year });
    }
    setFocus("make");
  }, [setFocus]);

  // function that attempts to make the POST request
  async function onSubmit(data: FieldValues) {
    try {
      let id = "";
      let res;
      // if a new auction is being created, assign the response's id to id variable
      if (pathname === "/auctions/create") {
        res = await createAuction(data);
        id = res.id;
      }
      // if an auction is being updated, assign passed auction's id to id variable
      else {
        if (auction) {
          res = await updateAuction(auction.id, data);
          id = auction.id;
        }
      }
      if (res.error) {
        throw res.error;
      }
      router.push(`/auctions/details/${id}`);
    } catch (error: any) {
      toast.error(error.status + " " + error.message);
    }
  }

  return (
    <form className="flex flex-col mt-3" onSubmit={handleSubmit(onSubmit)}>
      <Input
        label="Make"
        name="make"
        control={control}
        rules={{ required: "Make is required" }}
      />
      <Input
        label="Model"
        name="model"
        control={control}
        rules={{ required: "Model is required" }}
      />
      <Input
        label="Color"
        name="color"
        control={control}
        rules={{ required: "Color is required" }}
      />
      <div className="grid grid-cols-2 gap-3">
        <Input
          label="Year"
          name="year"
          type="number"
          control={control}
          rules={{ required: "Year is required" }}
        />
        <Input
          label="Mileage"
          name="mileage"
          type="number"
          control={control}
          rules={{ required: "Mileage is required" }}
        />
      </div>

      {/* display these only if a new auction is being created */}
      {pathname === "/auctions/create" && (
        <>
          <Input
            label="Image URL"
            name="imageUrl"
            control={control}
            rules={{ required: "Image URL is required" }}
          />
          <div className="grid grid-cols-2 gap-3">
            <Input
              label="Reserve Price (enter 0 if no reserve price)"
              name="reservePrice"
              type="number"
              control={control}
              rules={{ required: "Reserve Price is required" }}
            />
            <DateInput
              label="Auction end date/time"
              name="auctionEnd"
              dateFormat="dd MMMM yyyy h:mm a"
              showTimeSelect
              control={control}
              rules={{ required: "Auction end date is required" }}
            />
          </div>
        </>
      )}

      <div className="flex justify-between">
        <Button outline color="gray">
          Cancel
        </Button>
        <Button
          outline
          isProcessing={isSubmitting}
          disabled={!isValid}
          type="submit"
          color="success"
        >
          Submit
        </Button>
      </div>
    </form>
  );
}
