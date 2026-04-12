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

  const BucketsisArray = (buckets:BucketType[]) => {
    return Array.isArray(buckets)
    
  }
  const [errorMessage, setErrorMessage] = useState<Error | undefined>();
  const ErrorMessageStyle = {
    color: "#B00020",
    backgroundColor: "#FFEBEE",
    borderLeft: "4px solid #D32F2F",
    padding: "8px 12px",
    borderRadius: "4px",
    fontSize: "16px",
    lineHeight: "1.4",
    fontFamily: "Segoe UI, Tahoma, sans-serif",
    marginTop: "6px"
  };

  useEffect(() => {
    const fetchBuckets = async () => {
      try {
        const response = await fetch("https://localhost:7118/api/v1/buckets", {
          credentials: "include"
        });
        
        if(!response.ok){
          let message = "Something went wrong."
          
          // log for debugging purposes.
          console.error("GET /buckets failed", {
            status: response.status,
            statusText: response.statusText
          });
          
          if(response.status === 401){
            message = "Unauthorized access."
          }
          setErrorMessage(new Error(message))
          setPending(false)
          
          // early return stop flow.
          return;
          
        }
        const data = await response.json()
        if(BucketsisArray(data.value)){
          SetBuckets(data.value);
          setPending(false)
        }
        
      } 
      catch (err) {
        setPending(false)
        const message = getErrorMessage(err)
        // 1. Log the actual error to the console.
        console.error(message)
        // 2. Show an generic message in the UI for the user.
        setErrorMessage(new Error("Failed to retrieve buckets data."))
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

      {errorMessage && <div><p style={ErrorMessageStyle}>{errorMessage.message}</p></div>}

      {!errorMessage && BucketsisArray(buckets) &&
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
