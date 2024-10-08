"use client";

import { useAuctionStore } from "@/hooks/useAuctionStore";
import { useBidStore } from "@/hooks/useBidStore";
import { Auction, AuctionFinished, Bid } from "@/types";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { User } from "next-auth";
import { useParams } from "next/navigation";
import { ReactNode, useCallback, useEffect, useRef } from "react";
import AuctionCreatedToast from "../components/AuctionCreatedToast";
import toast from "react-hot-toast";
import { getDetailedViewData } from "../actions/auctionActions";
import AuctionFinishedToast from "../components/AuctionFinishedToast";

type Props = {
    children: ReactNode;
    user: User | null;
};

export default function SignalRProvider({ children, user }: Props) {
    // connection using useRef
    const connection = useRef<HubConnection | null>(null);
    // connecting with stores to make global changes
    const setCurrentPrice = useAuctionStore((state) => state.setCurrentPrice);
    const addBid = useBidStore((state) => state.addBid);
    const params = useParams<{ id: string }>();

    // using useCallback hook to stop functions from being recreated
    // its "memo-ized" unless the dependencies change
    // event handler for BidPlaced event
    const handleBidPlaced = useCallback(
        (bid: Bid) => {
            // check if a bid gets accepted
            if (bid.bidStatus.includes("Accepted")) {
                setCurrentPrice(bid.auctionId, bid.amount);
            }

            // check if a bid got added to the list of bids
            if (params.id === bid.auctionId) {
                addBid(bid);
            }
        },
        [setCurrentPrice, addBid, params.id]
    );

    // event handler for AuctionCreated event
    const handleAuctionCreated = useCallback(
        (auction: Auction) => {
            // toast if the signed in user is not the same as the seller
            if (user?.username !== auction.seller) {
                return toast(<AuctionCreatedToast auction={auction} />, {
                    duration: 10000,
                });
            }
        },
        [user?.username]
    );

    const handleAuctionFinished = useCallback(
        (finishedAuction: AuctionFinished) => {
            const auction = getDetailedViewData(finishedAuction.auctionId);
            return toast.promise(
                auction,
                {
                    loading: "Loading",
                    success: (auction) => (
                        <AuctionFinishedToast
                            auction={auction}
                            finishedAuction={finishedAuction}
                        />
                    ),
                    error: (err) => "Auction finished",
                },
                { success: { duration: 10000, icon: null } }
            );
        },
        []
    );

    // useEffect hook to actually reflect changes
    useEffect(() => {
        // checking connection with the HubConnection
        if (!connection.current) {
            connection.current = new HubConnectionBuilder()
                .withUrl("http://localhost:6001/notifications")
                .withAutomaticReconnect()
                .build();

            // start connection
            connection.current
                .start()
                .then(() => "Conneted to notification hub")
                .catch((err) => console.log({ err }));
        }

        // real time bid changes via SignalR (on BidPlaced event)
        // this is effectively a listener for "BidPlaced" events
        // turning on the listener BidPlaced event
        connection.current.on("BidPlaced", handleBidPlaced);

        // turning on the listener for AuctionCreated event
        connection.current.on("AuctionCreated", handleAuctionCreated);

        // turning on the listener for AuctionFinished event
        connection.current.on("AuctionFinished", handleAuctionFinished);

        // cleaning up the useEffect hook by turning off the listener
        return () => {
            connection.current?.off("BidPlaced", handleBidPlaced);
            connection.current?.off("AuctionCreated", handleAuctionCreated);
            connection.current?.off("AuctionFinished", handleAuctionFinished);
        };
    }, [handleBidPlaced, handleAuctionCreated, handleAuctionFinished]);

    // return children (which contains the whole react app)
    return children;
}
