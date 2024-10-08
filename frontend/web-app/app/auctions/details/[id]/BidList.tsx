"use client";

import { getBidsForAuction } from "@/app/actions/auctionActions";
import Heading from "@/app/components/Heading";
import { useBidStore } from "@/hooks/useBidStore";
import { Auction, Bid } from "@/types";
import { User } from "next-auth";
import React, { useEffect, useState } from "react";
import toast from "react-hot-toast";
import BidItem from "./BidItem";
import { numberWithCommas } from "@/app/lib/numberWithComma";
import EmptyFilter from "@/app/components/EmptyFilter";
import BidForm from "./BidForm";

type Props = {
    user: User | null;
    auction: Auction;
};

export default function BidList({ user, auction }: Props) {
    // for loading purposes
    const [loading, setLoading] = useState(true);

    // functionality for getting bids associated with the passed auction
    const bids = useBidStore((state) => state.bids);
    const setBids = useBidStore((state) => state.setBids);

    //functionality for disabling the bidding input when timer runs out
    const open = useBidStore((state) => state.open);
    const setOpen = useBidStore((state) => state.setOpen);

    // comparing current date with auction's end date
    const openForBids = new Date(auction.auctionEnd) > new Date();

    // getting the highest bid value
    const highBid = bids.reduce(
        (prev, current) =>
            prev > current.amount
                ? prev
                : current.bidStatus.includes("Accepted")
                ? current.amount
                : prev,
        0
    );

    // getting the bids and storing them into the useBidStore for efficiency
    useEffect(() => {
        getBidsForAuction(auction.id)
            .then((res: any) => {
                if (res.error) throw res.error;
                setBids(res as Bid[]);
            })
            .catch((err) => toast.error(err.message))
            .finally(() => setLoading(false));
    }, [auction.id, setLoading, setBids]);

    // use effect for disabling the bidding input when the timer runs out
    useEffect(() => {
        setOpen(openForBids);
    }, [openForBids, setOpen]);

    // loading placeholder
    if (loading) {
        return <span>Loading Bids . . .</span>;
    }

    // the component to be returned
    return (
        <div className="rounded-lg shadow-md">
            <div className="py-2 px-4 bg-white">
                <div className="sticky top-0 bg-white p-2">
                    <Heading
                        title={`
                        Current high bid is ${numberWithCommas(highBid)}
                        `}
                    />
                </div>
            </div>

            <div className="overflow-auto h-[400px] flex flex-col-reverse px-2">
                {bids.length === 0 ? (
                    <EmptyFilter
                        title="No bids for this item"
                        subtitle="Please feel free to make a bid"
                    />
                ) : (
                    <>
                        {bids.map((bid) => (
                            <BidItem key={bid.id} bid={bid} />
                        ))}
                    </>
                )}
            </div>

            <div className="px-2 pb-2 text-gray-500">
                {!open ? (
                    // if the timer has run out
                    <div className="flex items-center justify-center p-2 text-lg font-semibold">
                        This auction has finished
                    </div>
                ) : !user ? (
                    // if user is not signed in
                    <div className="flex items-center justify-center p-2 text-lg font-semibold">
                        Please login to make a bid
                    </div>
                ) : // if user is signed in and is trying to bid on him own auction
                user && user.username === auction.seller ? (
                    <div className="flex items-center justify-center p-2 text-lg font-semibold">
                        You cannot bid on your own auction
                    </div>
                ) : (
                    // if user is trying to bid on someone else's auction
                    <BidForm auctionId={auction.id} highBid={highBid} />
                )}
            </div>
        </div>
    );
}
