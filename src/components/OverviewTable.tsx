import {useEffect, useState} from "react";
import type {TransactionsSummary, BucketTransactions} from "../components/OverviewRow"
import OverviewRow from "./OverviewRow"

const OverviewTable = () => {
    // fetch doen naar:
    // http://localhost:5286/api/v1/buckets/summary?month=3&year=2027
    
    // useState()
    const [transactions, setTransactionsData] = useState<TransactionsSummary>();
    
    // useEffect()
    useEffect( () => {
        console.log("useffect is running!")
        const fetchSummary =  async () =>
        {
            try{
                const response = await fetch("http://localhost:5286/api/v1/buckets/summary?month=2&year=2026")
                if (!response.ok) {
                    throw new Error(`HTTP error: ${response.status}`);
                }
                const data = await response.json();
                setTransactionsData(data)
            }
            catch(error) {
                console.log(error)
            }
        }
        fetchSummary()
    },
    [])
    
        
    return (
        <div>
            <table className="table">
            <thead>
                <tr>
                    <th scope="col">Bucket</th>
                    <th scope="col">Transaction</th>
                    <th scope="col">Date</th>
                    <th scope="col">Type</th>
                    <th scope="col">Amount</th>
                </tr>
            </thead>
                <tbody>
                    <OverviewRow data={transactions}/>
                </tbody>
            </table>
               
        </div>
    )
}

export default OverviewTable;