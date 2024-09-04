import React from "react";

export default function Update({ params }: { params: { id: string } }) {
  return <div>Details for {params.id}</div>;
}
