import "../styles/BucketList.css";
import Bucket from "./Bucket";
import type { Bucket as BucketType } from "../types/Bucket";
import { v4 as uuidv4 } from "uuid";
import { useEffect, useState } from "react";

const BucketList = () => {
  const [buckets, SetBuckets] = useState([]);
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [error, setError] = useState<string>();


  useEffect(() => {
    const fetchBuckets = async () => {
      try {
        const response = await fetch("http://localhost:5286/api/v1/buckets");
        const data = await response.json();
        SetBuckets(data);
      } catch (error) {
        if (error instanceof Error) {
          setError(error.message);
        } else {
          setError("Something went wrong");
        }
      }
    };
    fetchBuckets();
  }, []);
  return (
    <>
      <h1>Transaction Overview</h1>
      <div className="bucket-list">
        {buckets &&
          buckets.map((b: BucketType) => {
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
