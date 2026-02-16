import {useEffect, useState} from "react";
import type {TransactionsSummary, BucketTransactions} from "../components/OverviewRow"
import OverviewRow from "./OverviewRow"
import TotalCard from "./TotalCard"
import {TransactionType} from "../types/TransactionType"

const OverviewTable = () => {
    // fetch doen naar:
    // http://localhost:5286/api/v1/buckets/summary?month=3&year=2027
    
    // useState()
    const [transactions, setTransactionsData] = useState<TransactionsSummary | undefined>(undefined);
    
    const [data, setData] = useState({
        month:2,
        year: 2025
    })
    // useEffect()
    useEffect(() => {
        console.log("useffect is running!")
        const fetchSummary =  async () =>
        {
            try{
                const response = await fetch(`http://localhost:5286/api/v1/buckets/summary?month=${data.month}&year=${data.year}`)
                if (!response.ok) {
                    throw new Error(`HTTP error: ${response.status}`);
                }
                const result = await response.json();
                setTransactionsData(result)
            }
            catch(error) {
                console.log(error)
            }
        }
        
        fetchSummary()
    },
    [data])

    
    
    return (
        <div>
            <div className="bucket-list">
                {/*TODO verander dit.*/}
                <TotalCard  icon="💰" name="Income" type={TransactionType.Income} data={transactions}/>
                <TotalCard icon="💸"  name="Expenses" type={TransactionType.Expense}  data={transactions}/>
            </div>
            <div>
                <input name="month" type="text" placeholder="Month" onChange={(e) => {
                    setData({...data, month: e.target.value})
                }}/>
                <input name="year" type="text" placeholder="Year" onChange={(e) => {
                    setData({...data, year: e.target.value})
                }}/>
            </div>
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