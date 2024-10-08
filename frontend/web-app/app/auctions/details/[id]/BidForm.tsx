"use client";

import { placeBidForAuction } from "@/app/actions/auctionActions";
import { numberWithCommas } from "@/app/lib/numberWithComma";
import { useBidStore } from "@/hooks/useBidStore";
import React from "react";
import { FieldValues, useForm } from "react-hook-form";
import toast from "react-hot-toast";

type Props = {
    auctionId: string;
    highBid: number;
};

export default function BidForm({ auctionId, highBid }: Props) {
    // using react hook forms
    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm();

    // getting AddBid functionality from store
    const addBid = useBidStore((state) => state.addBid);

    // onSubmit function
    function onSubmit(data: FieldValues) {
        // "+" symbol guarantees that its a number
        // (converts it to number if necessary)
        if (data.amount) {
            return toast.error(
                "Bid must be at least $" + numberWithCommas(highBid + 1)
            );
        }
        placeBidForAuction(auctionId, +data.amount)
            .then((bid) => {
                // error handling
                if (bid.error) throw bid.error;
                // add the bid to list of bids and reset form values
                addBid(bid);
                reset();
            })
            .catch((err) => toast.error(err));
    }

    return (
        <form
            onSubmit={handleSubmit(onSubmit)}
            className="flex items-center border-2 rounded-lg py-2"
        >
            <input
                type="number"
                {...register("amount")}
                className="input-custom text-sm text-gray-600"
                placeholder={`
                    Enter your bid (minimum bid is $${numberWithCommas(
                        highBid + 1
                    )})
                `}
            />
        </form>
    );
}
