"use client";

import { useAuctionStore } from "@/hooks/useAuctionStore";
import { useBidStore } from "@/hooks/useBidStore";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { useParams } from "next/navigation";
import { ReactNode, useEffect, useRef } from "react";

type Props = {
    children: ReactNode;
};

export default function SignalRProvider({ children }: Props) {
    // connection using useRef
    const connection = useRef<HubConnection | null>(null);

    // connecting with stores to make global changes
    const setCurrentPrice = useAuctionStore((state) => state.setCurrentPrice);
    const addBid = useBidStore((state) => state.addBid);
    const params = useParams<{ id: string }>();

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
    }, []);

    // return children (which contains the whole react app)
    return children;
}
