import "../styles/BucketList.css";
import BucketCard from "./BucketCard";
import {type Dispatch, type SetStateAction, useEffect, useState} from "react";
import {getErrorMessage} from "../utils/utils.ts";
import CreateBucketModal from "./CreateBucketModal.tsx";
import type {Bucket} from "../types/Bucket.tsx";
import type {Transaction} from "../types/Transaction.tsx";

interface BucketListProps {
    setBuckets: Dispatch<SetStateAction<Bucket[]>>
    buckets: Bucket[]
    transactions: Transaction[]
}


const BucketList = ({setBuckets, buckets, transactions}: BucketListProps) => {
    // Dashboard overview with all the userbuckets.
    const [isPending, setPending] = useState(true);
    const [showModal, setShowModal] = useState(false)
    const [errorMessage, setErrorMessage] = useState<Error | undefined>();


    const BucketsisArray = (buckets: Bucket[]) => {
        return Array.isArray(buckets)

    }
    
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
                const response = await fetch("https://localhost:7118/api/v1/buckets/user", {
                    credentials: "include"
                });

                if (!response.ok) {
                    let message = "Something went wrong."

                    // log for debugging purposes.
                    console.error("GET /buckets failed", {
                        status: response.status,
                        statusText: response.statusText
                    });

                    if (response.status === 401) {
                        message = "Unauthorized access."
                    }
                    setErrorMessage(new Error(message))
                    setPending(false)

                    // early return stop flow.
                    return;

                }
                const data = await response.json()
                if (BucketsisArray(data.value)) {
                    setBuckets(data.value);
                    setPending(false)
                }

            } catch (err) {
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

            {isPending && <div className="spinner-border" role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
            }

            {errorMessage && <div><p style={ErrorMessageStyle}>{errorMessage.message}</p></div>}

            {!errorMessage && BucketsisArray(buckets) &&
                <div className="bucket-list">
                    {buckets?.map((b: Bucket) => {
                        return (
                            <div key={b.bucket.id}>
                                <BucketCard
                                    key={b.bucket.id}
                                    id={b.bucket.id}
                                    name={b.bucket.name}
                                    icon={b.bucket.icon}
                                    amount={b.bucketTotal}
                                />
                            </div>
                        );

                    })}
                </div>
            }
            <input className="btn btn-primary" type="button" value="Create Bucket" onClick={() => setShowModal(true)}/>
            {showModal ?
                <CreateBucketModal  setShowModal={setShowModal} showModal={showModal} setBuckets={setBuckets} setErrorMessage={setErrorMessage}/> : null
            }

        </>
    );
};

export default BucketList;
// altijd een extra return wanneer je map() gebruikt;
