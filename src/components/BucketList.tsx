import "../styles/BucketList.css";
import Bucket from "./Bucket";
import type { Bucket as BucketType } from "../types/Bucket";
import { useEffect, useState } from "react";
import type {Transaction} from "../types/Transaction.tsx";
import {getErrorMessage} from "../utils/utils.ts";

interface BucketListProps{
  transactions: Transaction[]
}


const BucketList = ({transactions}:BucketListProps) => {
  const [buckets, SetBuckets] = useState<BucketType[]>([]);
  const [isPending, setPending] = useState(true);

  
  const [error, setError] = useState<Error | undefined>();


  useEffect(() => {
    const fetchBuckets = async () => {
      try {
        const response = await fetch("http://localhost:5286/api/v1/buckets");
        
        if(!response.ok){
          return;
        }
          const data = await response.json()
          SetBuckets(data);
          setPending(false)
        
      } catch (err) {
        setPending(false)
        const message = getErrorMessage(err)
        // 1. Log the actual error to the console.
        // 2. Show an generic message in the UI for the user.
        console.error(message)
        setError(new Error("Failed to retrieve buckets data."))
      }
    }
    fetchBuckets();
  }, [transactions]);
  return (
    <>
      <h1>Transaction Overview</h1>
      
      {isPending &&  <div className="spinner-border" role="status">
        <span className="visually-hidden">Loading...</span>
      </div>
      }

      {error && <div>{error.message}</div>}

      {!error &&
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
      }
    </>
  );
  };

export default BucketList;
// altijd een extra return wanneer je map() gebruikt;
