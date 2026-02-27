import "../styles/BucketList.css";
import Bucket from "./Bucket";
import type { Bucket as BucketType } from "../types/Bucket";
import { useEffect, useState } from "react";
import type {Transaction} from "../types/Transaction.tsx";

interface BucketListProps{
  transactions: Transaction[]
}


const BucketList = ({transactions}:BucketListProps) => {
  const [buckets, SetBuckets] = useState<BucketType[]>([]);
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
            console.log(error.name === TypeError.name)
            console.log(error.message)
            setError(error.message);
          } else {
            setError("Something went wrong with loading the data.");
          }
      }
    };
    fetchBuckets();
  }, [transactions]);
  return (
    <>
      <h1>Transaction Overview</h1>
      <div className="bucket-list">
        {buckets !== null &&
          buckets.map((b: BucketType) => {
            return (
              <Bucket
                key={b.id}
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
