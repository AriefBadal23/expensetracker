import {useEffect, useState} from "react";
import type {TransactionsSummary} from "../components/OverviewRow"
import OverviewRow from "./OverviewRow"
import TotalCard from "./TotalCard"
import {TransactionType} from "../types/TransactionType"

const OverviewTable = () => {
    // fetch doen naar:
    // http://localhost:5286/api/v1/buckets/summary?month=3&year=2027
    
    // useState()
    const [transactions, setTransactionsData] = useState<TransactionsSummary | undefined>(undefined);
    const [data, setData] = useState({
        month:"1",
        year: "2025"
    })
    
    // useEffect()
    useEffect(() => {
        const fetchSummary =  async () =>
        {
            try{
                const url = `http://localhost:5286/api/v1/buckets/summary?month=${data.month}&year=${data.year}`
                
                const response = await fetch(url)
                
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
                {/*Predefined values*/}
                <select name="months" onChange={(e) => {
                    setData({...data, month: e.target.value})
                }}>
                    <option value="1">January</option>
                    <option value="2">February</option>
                    <option value="3">March</option>
                    <option value="4">April</option>
                    <option value="5">May</option>
                    <option value="6">June</option>
                    <option value="7">July</option>
                    <option value="8">August</option>
                    <option value="9">September</option>
                    <option value="10">October</option>
                    <option value="11">November</option>
                    <option value="12">December</option>
                </select>
                
                <select name="year" onChange={(e) => {
                    setData({...data, year: e.target.value})
                }}>
                    <option value="2025">2025</option>
                    <option value="2026">2026</option>
                </select>
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