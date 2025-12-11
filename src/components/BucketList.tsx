import "../styles/BucketList.css";
import Bucket from "./Bucket";
import type { Bucket as BucketType } from "../types/Bucket";
import { v4 as uuidv4 } from "uuid";
import { useEffect, useState } from "react";

const BucketList = () => {
  const [buckets, SetBuckets] = useState([]);

  useEffect(() => {
    const fetchBuckets = async () => {
      try {
        const response = await fetch("http://localhost:5286/api/v1/buckets");
        const buckets = await response.json();
        SetBuckets(buckets);
      } catch {
        console.log("Failed to fetch data from api");
      }
    };
    fetchBuckets();
  }, []);
  return (
    <>
      <h1>Transaction Overview</h1>
      <div className="bucket-list">
        {buckets.map((b: BucketType) => {
          return (
            <Bucket
              key={uuidv4()}
              name={b.name}
              amount={b.total}
              icon={b.icon}
            />
          );
        })}
      </div>
    </>
  );
};

export default BucketList;
// altijd een extra return wanneer je map() gebruikt;
